Imports atcUtility
Imports atcWASP
Imports atcMwGisUtility
Imports MapWinUtility
Imports atcData
Imports System.Drawing
Imports System
Imports System.Windows.Forms


''' <summary>
''' After the stream segments have been selected, this form is shown to allow you to refine the Wasp segment organization (subdividing and combining NHD segments)
''' and then assigning input time series to each Wasp Segment. Finally, you can create the Wasp INP file and fire up Wasp 8
''' </summary>
Public Class frmWASPSetup
    Inherits System.Windows.Forms.Form

    Friend pPlugIn As PlugIn
    Friend pSelectedIndexes As atcCollection
    Friend pSegmentLayerIndex As Integer
    Friend pBasinsFolder As String
    Friend pfrmWASPFieldMapping As frmWASPFieldMapping
    Friend pWASPFolder As String
    Friend IsModified As Boolean = False

    Private Const WASPSegmentLayerName As String = "WASP Segments"

    'these are helper classes that enable advanced editing of datagridviews...

    Private WithEvents dgvSegmentation, dgvFlow, dgvLoad, dgvBound, dgvTime As WRDB.Controls.DGVEditor

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub mnuHelpAbout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuHelpAbout.Click
        Logger.Msg("BASINS WASP 8 Model Builder for MapWindow" & vbCrLf & vbCrLf & "Version " & My.Application.Info.Version.ToString(2), MsgBoxStyle.OkOnly, "BASINS WASP 8 Model Builder")
    End Sub

    Private Sub btnHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuHelpManual.Click, btnHelp.Click
        ShowHelp("BASINS Details\Watershed and Instream Model Setup\WASP.html")
    End Sub

    Private Sub frmWASPSetup_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        SaveWindowPos(REGAPPNAME, Me)
        'remove WASP Builder layers and group
        With pPlugIn.MapWin.Layers
            For i As Integer = 0 To .Groups.Count - 1
                If .Groups(i).Text = "WASP Builder" Then
                    pPlugIn.MapWin.Layers.Groups.Remove(.Groups(i).Handle)
                    Exit For
                End If
            Next
        End With
    End Sub

    Friend Sub EnableControls(ByVal aEnabled As Boolean)
        btnNew.Enabled = aEnabled
        btnOpen.Enabled = aEnabled
        btnSave.Enabled = aEnabled
        btnBuild.Enabled = aEnabled
        btnClose.Enabled = aEnabled
        btnHelp.Enabled = aEnabled
    End Sub

    Private Sub btnBuild_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuild.Click
        Try
            Logger.Dbg("Setup WASP input files")

            UpdateStatus("Preparing to process...")

            Dim lName As String = tbxName.Text

            If Not SaveForm() Then Exit Sub

            With pPlugIn.WASPProject
                Dim WaspDir As String = My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\Wasp"
                If Not My.Computer.FileSystem.DirectoryExists(WaspDir) Then My.Computer.FileSystem.CreateDirectory(WaspDir)
                If String.IsNullOrEmpty(.INPFileName) Then .INPFileName = String.Format("{0}\{1}.inp", WaspDir, .Name)

                Using dlg As New SaveFileDialog
                    dlg.AddExtension = True
                    dlg.AutoUpgradeEnabled = True
                    dlg.CheckPathExists = True
                    dlg.DefaultExt = ".inp"
                    dlg.AddExtension = True
                    dlg.FileName = pPlugIn.WASPProject.INPFileName
                    If FileExists(IO.Path.GetDirectoryName(dlg.FileName), True, False) Then
                        dlg.InitialDirectory = IO.Path.GetDirectoryName(dlg.FileName)
                    End If
                    dlg.Filter = "Wasp Import Files (*.inp)|*.inp|All Files (*.*)|*.*"
                    dlg.OverwritePrompt = True
                    If dlg.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                        'save project file and start WASP
                        UpdateStatus("Saving project...")
                        .SaveProject(.Filename)

                        'save shapefile in same folder
                        CreateSegmentShapeFile()

                        UpdateStatus("Writing WASP input file...")
                        If Not .WriteINP(.INPFileName) Then
                            WarningMsg("Unable to create WASP input file")
                        Else
                            If .WriteErrors <> "" Then WarningMsg(.WriteErrors & vbCr & "Check that the specified table contains data within the indicated model date range for the specified Station ID and PCode (or mapped values, if appropriate).")
                            Shell("notepad " & .INPFileName, AppWinStyle.NormalFocus)
                            .Run(.INPFileName)
                        End If
                    End If
                    .INPFileName = dlg.FileName
                End Using
            End With
        Catch ex As Exception
            Throw
        Finally
            UpdateStatus()
        End Try
    End Sub

    Public Sub InitializeUI(ByVal aPlugIn As PlugIn, ByVal aSelectedIndexes As atcCollection, ByVal aSegmentLayerIndex As Integer)
        Logger.Dbg("InitializeUI")

        EnableControls(False)
        pPlugIn = aPlugIn
        pSelectedIndexes = aSelectedIndexes
        pSegmentLayerIndex = aSegmentLayerIndex

        pBasinsFolder = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\AQUA TERRA Consultants\BASINS", "Base Directory", "C:\Basins")

        'LCW 11/28/10: determine WASP installation folder so know where to get database from 

        'Note: folder name must NOT end in backslash
        'Apparently, epa.sqlite file is in parent of this directory (Wasp program dir)

        pWASPFolder = TestNull(My.Computer.Registry.GetValue("HKEY_CURRENT_USER\Software\USEPA\WASP\8.0", "DatabaseDir", "C:\WASP8"), "C:\WASP8").Trim(New Char() {"\"}) & "\.."

        If Not My.Computer.FileSystem.DirectoryExists(pWASPFolder) Then
            'Logger.Message("Database directory for the WASP 8 program could not be found: " & pWASPFolder, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning, Windows.Forms.DialogResult.OK)
            'WASP 8 is not ready to go, so in that case just use the epa.sqlite database being distributed with the plugin
            pWASPFolder = IO.Path.GetDirectoryName(Reflection.Assembly.GetExecutingAssembly.Location)
        End If

        cboModel.Items.Clear()
        For Each mdl As clsWASPModel In pPlugIn.WASPProject.GetWASPModels(pWASPFolder)
            cboModel.Items.Add(mdl.Description)
        Next

        cboModel.SelectedIndex = 0

        cboMet.Items.Add("<none>")

        For lLayerIndex As Integer = 0 To GisUtil.NumLayers() - 1
            Dim lLayerName As String = GisUtil.LayerName(lLayerIndex)
            If GisUtil.LayerType(lLayerIndex) = 3 Then 'PolygonShapefile 

            ElseIf GisUtil.LayerType(lLayerIndex) = 2 Then 'LineShapefile 

            ElseIf GisUtil.LayerType(lLayerIndex) = 1 Then 'PointShapefile
                cboMet.Items.Add(lLayerName)
                If lLayerName.ToUpper.IndexOf("WEATHER STATION SITES 20") > -1 Then
                End If
            ElseIf GisUtil.LayerType(lLayerIndex) = 4 Then 'Grid

            End If
        Next

        If cboMet.Items.Count > 0 And cboMet.SelectedIndex < 0 Then
            cboMet.SelectedIndex = 0
        End If

        For lLayerIndex As Integer = 0 To cboMet.Items.Count - 1
            Dim lLayerName As String = cboMet.Items(lLayerIndex)
            If lLayerName.IndexOf("Weather Station Sites 20") > -1 Then
                cboMet.SelectedIndex = lLayerIndex
            End If
        Next

        tbxName.Text = IO.Path.GetFileNameWithoutExtension(GisUtil.ProjectFileName)

        With cboLabelLayer.Items
            .Clear()
            .AddRange(New String() {"(None)", "SEGID", "WASPID", "WASPNAME", "NAME", "LENGTH", "MEANFLOW", "SLOPE", "TRAVELTIME"})
            cboLabelLayer.SelectedIndex = 0
        End With

        EnableControls(True)
        Logger.Dbg("InitializeUI Complete")

        If Not pPlugIn.StartupAlreadyShown Then
            pPlugIn.StartupAlreadyShown = True
            With New frmStartup
                .ShowDialog(Me)
            End With
        End If
    End Sub

    Friend Sub InitializeStationLists()
        UpdateStatus("Reading Timeseries Data...")

        With pPlugIn.WASPProject
            .BuildListofValidStationNames("FLOW", .FlowStationCandidates)
            .BuildListofValidStationNames("", .AllStationCandidates)

            SetStationCoordinates()

            'set valid values
            SetFlowStationGrid()
            SetLoadStationGrid()
            SetBoundaryGrid()
            SetTimeFunctionGrid()
        End With

        UpdateStatus()
    End Sub

    Private Sub btnGenerate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGenerate.Click
        GenerateSegments()
        btnSave.PerformClick() 'so the shape file is recreated
    End Sub

    Friend Sub GenerateSegments()
        'this takes some time, show window and then do this

        Dim MinTravelTime As Double = atxTravelTimeMin.ValueDouble

        UpdateStatus("Regenerating segments...")

        If Not pPlugIn.WASPProject.GenerateSegments(pSegmentLayerIndex, pSelectedIndexes, CDbl(atxTravelTimeMax.Text), CDbl(atxTravelTimeMin.Text)) Then
            ErrorMsg("An error occurred while generating WASP segments: " & Logger.LastDbgText)
        End If

        pPlugIn.WASPProject.ReadWASPdb(pWASPFolder, cboModel.Text)

        UpdateStatus("Refreshing data grids...")

        TabMain.SelectedIndex = 1
        SetSegmentationGrid()
        SetFlowStationGrid()
        SetLoadStationGrid()
        SetBoundaryGrid()

        UpdateStatus()

        'atc's generate routine sometimes doesn't join all the segments; warn the user
        For Each seg As atcWASPSegment In pPlugIn.WASPProject.Segments
            If pPlugIn.WASPProject.TravelTime(seg.Length, seg.Velocity) < MinTravelTime + 0.001 Then
                WarningMsg("One or more segments could not be combined to meet the minimum travel time requirement. Please check your data and manually combine segments if desired.")
                Exit For
            End If
        Next
    End Sub

    ''' <summary>
    ''' Display status message on form
    ''' </summary>
    ''' <param name="Text">If omitted reverts to standard instructions and cancels wait cursor</param>
    ''' <remarks></remarks>
    Private Sub UpdateStatus(Optional ByVal Text As String = "")
        If Text = "" Then
            Logger.Dbg("Prior operation complete")
            Text = "Update specifications if desired, then click 'Build WASP File' to proceed."
            Me.Cursor = Cursors.Default
            Me.UseWaitCursor = False
            EnableControls(True)
        Else
            Logger.Dbg(Text)
            Me.Cursor = Cursors.WaitCursor
            Me.UseWaitCursor = True
            EnableControls(False)
        End If
        lblStatus.Text = Text
        lblStatus.Refresh()
        Application.DoEvents()
    End Sub

#Region "Load and Save form and grid contents..."

    ''' <summary>
    ''' Save contents of all textboxes, grids, etc., to project structure
    ''' </summary>
    ''' <returns>True if successful</returns>
    Private Function SaveForm() As Boolean

        Dim lName As String = tbxName.Text

        'check that specified dates are valid
        Dim lSDate, lEDate As Date
        Try
            lSDate = New Date(atxSYear.Text, atxSMonth.Text, atxSDay.Text)
            lEDate = New Date(atxEYear.Text, atxEMonth.Text, atxEDay.Text)
        Catch ex As Exception
            Logger.Msg("The specified start/end dates are invalid.", vbOKOnly, "BASINS WASP Problem")
            Return False
        End Try
        If lSDate > lEDate Then 'failed date check
            Logger.Msg("The specified starting date is after the ending date.", vbOKOnly, "BASINS WASP Problem")
            EnableControls(True)
            Return False
        End If

        With pPlugIn.WASPProject
            .Name = lName
            For Each mdl As clsWASPModel In .GetWASPModels(pWASPFolder)
                If mdl.Description = cboModel.Text Then
                    .ModelType = mdl.ModelID
                    Exit For
                End If
            Next
            .MetLayer = cboMet.Text
            'set start and end dates
            .SDate = lSDate
            .EDate = lEDate
            .MinTravelTime = atxTravelTimeMin.Text
            .MaxTravelTime = atxTravelTimeMax.Text
        End With

        'put contents of segment class back into structure
        With dgSegmentation
            For r As Integer = 0 To .RowCount - 1
                Dim ID_Name As String = .Item(0, r).Value
                For Each seg As atcWASPSegment In pPlugIn.WASPProject.Segments
                    If seg.ID_Name = ID_Name Then
                        seg.WaspName = .Item(2, r).Value
                        seg.Length = .Item(3, r).Value
                        seg.Width = .Item(4, r).Value
                        seg.Depth = .Item(5, r).Value
                        seg.Slope = .Item(6, r).Value
                        seg.Roughness = .Item(7, r).Value
                        seg.DownID = .Item(8, r).Value
                        seg.Velocity = .Item(9, r).Value
                        Exit For
                    End If
                Next
            Next
        End With

        With dgFlow
            For r As Integer = 0 To .RowCount - 1
                Dim WaspID As Integer = .Item(0, r).Value
                Dim ID_Name As String = .Item(1, r).Value
                Dim DA As String = .Item(2, r).Value
                Dim Flow As String = .Item(3, r).Value
                Dim TS As String = .Item(4, r).Value
                For Each seg As atcWASPSegment In pPlugIn.WASPProject.Segments
                    If seg.WaspID = WaspID Then
                        seg.CumulativeDrainageArea = DA
                        seg.MeanAnnualFlow = Flow
                        seg.FlowTimeSeries = .Item(4, r).Tag 'contains selection
                        Exit For
                    End If
                Next
            Next
        End With

        With dgBound
            For r As Integer = 0 To .RowCount - 1
                Dim WaspID As Integer = .Item(0, r).Value
                Dim ID_Name As String = .Item(1, r).Value
                For Each seg As atcWASPSegment In pPlugIn.WASPProject.Segments
                    If seg.WaspID = WaspID Then
                        For c As Integer = 2 To .Columns.Count - 1
                            seg.BoundTimeSeries(c - 2) = dgBound.Item(c, r).Tag
                        Next
                        Exit For
                    End If
                Next
            Next
        End With

        With dgLoad
            For r As Integer = 0 To .RowCount - 1
                For Each seg As atcWASPSegment In pPlugIn.WASPProject.Segments
                    Dim WaspID As Integer = .Item(0, r).Value
                    Dim ID_Name As String = .Item(1, r).Value
                    If seg.WaspID = WaspID Then
                        For c As Integer = 2 To seg.BoundTimeSeries.Length - 1
                            seg.LoadTimeSeries(c - 2) = dgLoad.Item(c, r).Tag
                        Next
                        Exit For
                    End If
                Next
            Next
        End With

        ReDim pPlugIn.WASPProject.TimeFunctionSeries(pPlugIn.WASPProject.WASPTimeFunctions.Count - 1)
        With dgTime
            For r As Integer = 0 To .RowCount - 1
                pPlugIn.WASPProject.TimeFunctionSeries(r) = .Item(.ColumnCount - 1, r).Tag
            Next
        End With

        Return True
    End Function

    ''' <summary>
    ''' Load contents of project structure to all textboxes, grids, etc.
    ''' </summary>
    ''' <returns>True if successful</returns>
    Private Function LoadForm() As Boolean

        With pPlugIn.WASPProject
            tbxName.Text = .Name
            For Each mdl As clsWASPModel In .GetWASPModels(pWASPFolder)
                If mdl.ModelID = .ModelType Then
                    cboModel.Text = mdl.Description
                    Exit For
                End If
            Next
            cboMet.Text = .MetLayer
            atxSYear.Text = .SDate.Year
            atxSMonth.Text = .SDate.Month
            atxSDay.Text = .SDate.Day
            atxEYear.Text = .EDate.Year
            atxEMonth.Text = .EDate.Month
            atxEDay.Text = .EDate.Day
            atxTravelTimeMin.Text = .MinTravelTime
            atxTravelTimeMax.Text = .MaxTravelTime
        End With

        RefreshingSelection = True
        SetSegmentationGrid()
        SetTimeFunctionGrid()
        SetFlowStationGrid()
        SetBoundaryGrid()
        SetLoadStationGrid()
        RefreshingSelection = False

        IsModified = False

        Return True
    End Function

    Private Sub SetSegmentationGrid()

        dgvSegmentation = New WRDB.Controls.DGVEditor(dgSegmentation)

        With dgSegmentation
            .Visible = False
            dgvSegmentation.SetHeadings("Segment~WASP ID~Segment|Name~Length|(km)~Width|(m)~Depth|(m)~Slope|(m/m)~Roughness~Downstr|ID~Velocity|(m/s)~Travel Time|(days)", False)
            dgvSegmentation.AllowRowChange = False
            .MultiSelect = True
            .SelectionMode = DataGridViewSelectionMode.CellSelect

            Dim MinTravelTime As Double = Double.MaxValue
            Dim MaxTravelTime As Double = Double.MinValue

            .SuspendLayout()
            .Rows.Clear()
            For s As Integer = 1 To pPlugIn.WASPProject.Segments.Count
                For Each seg As atcWASPSegment In pPlugIn.WASPProject.Segments
                    With seg
                        If .WaspID = s Then
                            Dim TravelTime As Double = Math.Round(pPlugIn.WASPProject.TravelTime(.Length, .Velocity), 3)
                            MaxTravelTime = Math.Max(MaxTravelTime, TravelTime)
                            MinTravelTime = Math.Min(MinTravelTime, TravelTime)
                            dgvSegmentation.AddRowItems(.ID_Name, .WaspID, .WaspName, .Length, .Width, .Depth, .Slope, .Roughness, .DownID, .Velocity, TravelTime)
                            Exit For
                        End If
                    End With
                Next
            Next
            .ResumeLayout(True)

            For c As Integer = 0 To .Columns.Count - 1
                With .Columns(c)
                    Select Case c
                        Case 0, 1, 2, 8 'string columns
                            .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                            .ValueType = GetType(String)
                        Case 6
                            .DefaultCellStyle.Format = "0.00000"
                        Case 3, 7, 10
                            .DefaultCellStyle.Format = "0.000"
                        Case Else
                            .DefaultCellStyle.Format = "0.00"
                    End Select
                    Select Case c
                        Case 0, 1, 8, 10 : dgvSegmentation.ReadOnlyColumn(c) = True
                    End Select
                    Select Case c
                        Case 0, 1
                            .Frozen = True
                    End Select
                End With
            Next
            .AutoResizeColumn(0, DataGridViewAutoSizeColumnMode.AllCells)
            .AutoResizeColumn(1, DataGridViewAutoSizeColumnMode.AllCells)
            .AutoResizeColumn(2, DataGridViewAutoSizeColumnMode.AllCells)
            dgvSegmentation.AutoResizeColumnsEqual(3, .Columns.Count - 1, DataGridViewAutoSizeColumnMode.AllCells)

            'don't let slope be zero or negative; average with values before and after
            For r As Integer = 0 To .RowCount - 1
                Dim slope As Double = .Item(6, r).Value
                If slope <= 0 Then
                    slope = (.Item(6, Math.Max(0, r - 1)).Value + .Item(6, Math.Min(.RowCount - 1, r + 1)).Value) / 2
                    .Item(6, r).Value = slope
                    .Item(6, r).Style.ForeColor = Color.Red
                End If
            Next

            If Math.Abs(atxTravelTimeMax.ValueDouble - MaxTravelTime) > 0.001 AndAlso MaxTravelTime <> Double.MinValue Then atxTravelTimeMax.Text = MaxTravelTime
            If Math.Abs(atxTravelTimeMin.ValueDouble - MinTravelTime) > 0.001 AndAlso MinTravelTime <> Double.MaxValue Then atxTravelTimeMin.Text = MinTravelTime

            .ClearSelection()
            .Visible = True
        End With
    End Sub

    Private Sub SetFlowStationGrid()
        dgFlow.Visible = False
        dgvFlow = New WRDB.Controls.DGVEditor(dgFlow)
        With dgvFlow
            .SetHeadings("WASP|ID~Segment|Name~Cum. Drainage|Area (km^2)~Mean Annual|Flow (cms)~Input Flow Series|(cms)", False)
            For i As Integer = 0 To 1
                .ReadOnlyColumn(i) = True
                With .DataGridView.Columns(i)
                    .ReadOnly = True
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                End With
            Next i
            .ReadOnlyColumn(2) = True
            .ReadOnlyColumn(3) = True
            .SetColumnFormat(WRDB.Controls.DGVEditor.enumColumnTypes.Link, 4)
            .AllowRowChange = False
        End With

        With dgFlow
            .Rows.Clear()
            For Each seg As atcWASPSegment In pPlugIn.WASPProject.Segments
                seg.IsBoundary = pPlugIn.WASPProject.IsBoundary(seg)
            Next

            For s As Integer = 0 To pPlugIn.WASPProject.Segments.Count
                For Each seg As atcWASPSegment In pPlugIn.WASPProject.Segments
                    With seg
                        If pPlugIn.WASPProject.Segments.Count = 1 OrElse (seg.IsBoundary And Not seg.WaspID = 1) Then 'only upstream boundaries?
                            If .WaspID = s Then
                                dgvFlow.AddRowItems(.WaspID, .WaspName, .CumulativeDrainageArea, .MeanAnnualFlow, .FlowTimeSeries.ToString)
                                dgFlow.Item(dgFlow.ColumnCount - 1, dgFlow.RowCount - 1).Tag = .FlowTimeSeries
                                Exit For
                            End If
                        End If
                    End With
                Next
            Next
            If .RowCount > 0 Then .CurrentCell = .Item(.ColumnCount - 1, 0)
            .Visible = True
        End With

        Logger.Dbg("FlowStationGrid refreshed")
    End Sub

    Friend Sub SetLoadStationGrid()
        dgLoad.Visible = False
        dgvLoad = New WRDB.Controls.DGVEditor(dgLoad)
        With dgvLoad
            Dim hdg As String = "WASP|ID~Segment|Name"
            For Each con As clsWASPConstituent In pPlugIn.WASPProject.WASPConstituents
                'constituent names contain concentration units; remove these then add back in
                hdg &= "~" & con.Description.Split("(")(0).Trim & IIf(con.LoadUnits <> "", " (" & con.LoadUnits & ")", "")
            Next
            .SetHeadings(hdg, False)
            .AllowRowChange = False
            For c As Integer = 0 To .DataGridView.ColumnCount - 1
                If c <= 1 Then
                    .ReadOnlyColumn(c) = True
                Else
                    .SetColumnFormat(WRDB.Controls.DGVEditor.enumColumnTypes.Link, c)
                End If
            Next
        End With

        With dgLoad
            .Rows.Clear()
            For s As Integer = 1 To pPlugIn.WASPProject.Segments.Count
                For lRow As Integer = 0 To pPlugIn.WASPProject.Segments.Count - 1
                    Dim seg As atcWASP.atcWASPSegment = pPlugIn.WASPProject.Segments(lRow)
                    If seg.WaspID = s Then
                        .Rows.Add()
                        .Item(0, .Rows.Count - 1).Value = seg.WaspID
                        .Item(1, .Rows.Count - 1).Value = seg.WaspName
                        For lColumn As Integer = 2 To .ColumnCount - 1
                            With .Item(lColumn, .Rows.Count - 1)
                                .Tag = seg.LoadTimeSeries(lColumn - 2)
                                Dim str As String = seg.LoadTimeSeries(lColumn - 2).ToString
                                .Value = str
                            End With
                        Next
                        Exit For
                    End If
                Next
            Next
            If .RowCount > 0 And .ColumnCount > 1 Then .CurrentCell = .Item(2, 0)
            .Visible = True
            For c As Integer = 0 To .ColumnCount - 1
                With .Columns(c)
                    .Frozen = c <= 1
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                End With
            Next
        End With

        Logger.Dbg("LoadStationGrid refreshed")
    End Sub

    Friend Sub SetBoundaryGrid()
        dgBound.Visible = False
        dgvBound = New WRDB.Controls.DGVEditor(dgBound)
        With dgvBound
            Dim hdg As String = "WASP|ID~Segment|Name"
            For Each con As atcWASP.clsWASPConstituent In pPlugIn.WASPProject.WASPConstituents
                'constituent names contain concentration units; remove these then add back in
                hdg &= "~" & con.Description.Split("(")(0).Trim & IIf(con.ConcUnits <> "", " (" & con.ConcUnits & ")", "")
            Next
            .SetHeadings(hdg, False)
            .AllowRowChange = False
            For c As Integer = 0 To .DataGridView.ColumnCount - 1
                If c <= 1 Then
                    .ReadOnlyColumn(c) = True
                Else
                    .SetColumnFormat(WRDB.Controls.DGVEditor.enumColumnTypes.Link, c)
                End If
            Next
        End With

        'only load segments that are boundary segments
        With dgBound
            .Rows.Clear()
            For s As Integer = 1 To pPlugIn.WASPProject.Segments.Count
                For Each lSegment As atcWASPSegment In pPlugIn.WASPProject.Segments
                    If lSegment.WaspID = s AndAlso pPlugIn.WASPProject.IsBoundary(lSegment) Then
                        .Rows.Add()
                        .Item(0, .Rows.Count - 1).Value = lSegment.WaspID
                        .Item(1, .Rows.Count - 1).Value = lSegment.WaspName
                        For lColumn As Integer = 2 To .ColumnCount - 1
                            With .Item(lColumn, .Rows.Count - 1)
                                .Tag = lSegment.BoundTimeSeries(lColumn - 2)
                                .Value = lSegment.BoundTimeSeries(lColumn - 2).ToString
                            End With
                        Next
                        Exit For
                    End If
                Next
            Next
            If .RowCount > 0 And .ColumnCount > 1 Then .CurrentCell = .Item(2, 0)
            For c As Integer = 0 To .ColumnCount - 1
                With .Columns(c)
                    .Frozen = c <= 1
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                End With
            Next
            .Visible = True
        End With

        Logger.Dbg("BoundaryGrid refreshed")
    End Sub

    Friend Sub SetTimeFunctionGrid()
        dgTime.Visible = False
        dgvTime = New WRDB.Controls.DGVEditor(dgTime)
        With dgvTime
            .SetHeadings("Description~Time Series", False)
            .ReadOnlyColumn(0) = True
            .AllowRowChange = False
            For c As Integer = 0 To .DataGridView.ColumnCount - 1
                If c <= 0 Then
                    .ReadOnlyColumn(c) = True
                Else
                    .SetColumnFormat(WRDB.Controls.DGVEditor.enumColumnTypes.Link, c)
                End If
            Next
        End With

        With dgTime
            .RowCount = pPlugIn.WASPProject.WASPTimeFunctions.Count
            For lRow As Integer = 0 To .RowCount - 1
                .Item(0, lRow).Value = pPlugIn.WASPProject.WASPTimeFunctions(lRow).Description
                .Item(1, lRow).Tag = pPlugIn.WASPProject.TimeFunctionSeries(lRow)
                .Item(1, lRow).Value = pPlugIn.WASPProject.TimeFunctionSeries(lRow).ToString
            Next
            .CurrentCell = .Item(1, 0)
            For c As Integer = 0 To .ColumnCount - 1
                With .Columns(c)
                    .Frozen = c <= 0
                    .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                End With
            Next
            .Visible = True
        End With

        Logger.Dbg("TimeFunctionGrid refreshed")
    End Sub

#End Region

    Private Sub lnkFieldMapping_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkFieldMapping.LinkClicked
        If IsNothing(pfrmWASPFieldMapping) Then
            pfrmWASPFieldMapping = New frmWASPFieldMapping
            pfrmWASPFieldMapping.Init(pSegmentLayerIndex, pPlugIn.WASPProject.SegmentFieldMap, Me)
            pfrmWASPFieldMapping.ShowDialog(Me)
        Else
            If pfrmWASPFieldMapping.IsDisposed Then
                pfrmWASPFieldMapping = New frmWASPFieldMapping
                pfrmWASPFieldMapping.Init(pSegmentLayerIndex, pPlugIn.WASPProject.SegmentFieldMap, Me)
                pfrmWASPFieldMapping.ShowDialog(Me)
            Else
                pfrmWASPFieldMapping.WindowState = FormWindowState.Normal
                pfrmWASPFieldMapping.BringToFront()
            End If
        End If
    End Sub

    Private Sub lnkCreateShapefile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lnkCreateShapefile.LinkClicked
        Try
            'only allow this after project has been saved
            If String.IsNullOrEmpty(pPlugIn.WASPProject.Filename) Then
                WarningMsg("You cannot create the buffered shape file until you have saved the project.")
                Exit Sub
            End If
            UpdateStatus("Creating buffered shapefile...")
            Dim lWASPSegmentShapefile As String = ""
            CreateSegmentShapeFile()
            CreateBufferedSegmentShapeFile()
        Catch lEX As Exception
            Logger.Msg(lEX.Message, MsgBoxStyle.Critical, "Problem Creating Shapefile")
        Finally
            UpdateStatus()
        End Try
    End Sub

    ''' <summary>
    ''' Set dates on the general tab to the last common year of the selected timeseries       
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetDates()

        Dim lSJDate As Double = 0.0
        Dim lEJDate As Double = 0.0

        If lEJDate > lSJDate Then
            Dim lEDate(5) As Integer, lSDate(5) As Integer
            J2Date(lEJDate, lEDate)
            J2Date(lSJDate, lSDate)

            'default to last calendar year of data
            lSDate(0) = lEDate(0) - 1
            lSDate(1) = 1
            lSDate(2) = 1
            lEDate(0) = lSDate(0)
            lEDate(1) = 12
            lEDate(2) = 31
            atxSYear.Text = lSDate(0)
            atxSMonth.Text = lSDate(1)
            atxSDay.Text = lSDate(2)
            atxEYear.Text = lEDate(0)
            atxEMonth.Text = lEDate(1)
            atxEDay.Text = lEDate(2)
        End If
    End Sub

    Private Sub cboMet_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboMet.SelectedIndexChanged
        SetStationCoordinates()
    End Sub

    Private Sub SetStationCoordinates()
        With pPlugIn.WASPProject
            If cboMet.SelectedIndex > 0 Then
                Dim lMetLayerIndex As Integer = GisUtil.LayerIndex(cboMet.Items(cboMet.SelectedIndex))
                .GetMetStationCoordinates(lMetLayerIndex, .AllStationCandidates)
            End If
        End With
    End Sub

    Private Sub cboModel_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboModel.SelectedIndexChanged
        Dim lModelName As String = cboModel.Text
        pPlugIn.WASPProject.ReadWASPdb(pWASPFolder, lModelName)

        UpdateStatus("Refreshing data grids...")

        SetLoadStationGrid()
        SetBoundaryGrid()
        SetTimeFunctionGrid()

        UpdateStatus()

    End Sub

    ''' <summary>
    ''' Display unhandled error message (for backwards compatibility with error handling macro in VS 2008)
    ''' </summary>
    ''' <param name="ErrorText">Specific error text (if missing, will be generic)</param>
    ''' <param name="ex">Exception</param>
    Public Sub ErrorMsg(Optional ByVal ErrorText As String = "", Optional ByVal ex As Exception = Nothing)
        If ErrorText = "" Then ErrorText = "An unexpected error has occurred in the BASINS WASP utility."
        If ex IsNot Nothing Then ErrorText &= " The detailed error is: " & vbCr & vbCr & ex.ToString
        Logger.Message(ErrorText, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, DialogResult.OK)
    End Sub

    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click, mnuSaveProj.Click
        If Not SaveForm() Then Exit Sub
        RefreshingSelection = True
        With pPlugIn.WASPProject
            If Not String.IsNullOrEmpty(.Filename) Then
                UpdateStatus("Saving project...")
                .SaveProject(.Filename)
                CreateSegmentShapeFile()
                Text = "BASINS WASP Model Builder - " & IO.Path.GetFileName(.Filename)
            Else
                mnuSaveAs.PerformClick()
            End If
        End With
        UpdateStatus()
        If dgSegmentation.CurrentRow IsNot Nothing Then HighlightSegment(dgSegmentation.CurrentRow.Cells(1).Value)
        IsModified = False
        RefreshingSelection = False
    End Sub

    Private Sub btnSaveAs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuSaveAs.Click
        With New SaveFileDialog
            .AddExtension = True
            .CheckFileExists = False
            .CheckPathExists = True
            .DefaultExt = ".WaspBuilder"
            .Filter = "WASP Builder input files (*.WaspBuilder)|*.WaspBuilder"
            .FilterIndex = 0
            .InitialDirectory = IO.Path.GetDirectoryName(pPlugIn.WASPProject.Filename)
            If String.IsNullOrEmpty(.InitialDirectory) Then .InitialDirectory = IO.Path.GetDirectoryName(pPlugIn.MapWin.Project.FileName) & "\Wasp"
            If Not My.Computer.FileSystem.DirectoryExists(.InitialDirectory) Then My.Computer.FileSystem.CreateDirectory(.InitialDirectory)
            .Title = "Save WASP Builder File"
            .FileName = pPlugIn.WASPProject.Filename
            If .FileName = "" Then .FileName = pPlugIn.WASPProject.Name & ".WaspBuilder"
            If FileExists(IO.Path.GetDirectoryName(.FileName), True, False) Then
                .InitialDirectory = IO.Path.GetDirectoryName(.FileName)
            End If
            If .ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                If Not SaveForm() Then Exit Sub
                pPlugIn.WASPProject.SaveProject(.FileName)
                CreateSegmentShapeFile()
                SaveSetting(REGAPPNAME, "Settings", "LastFilename", .FileName)
                Text = "BASINS WASP Model Builder - " & IO.Path.GetFileName(.FileName)
            End If
            .Dispose()
        End With
        IsModified = False
    End Sub

    Private Sub CheckModified()
        If IsModified AndAlso MessageBox.Show("You have unsaved changes to this project. Do you want to save your WASP Builder project before continuing?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) = Windows.Forms.DialogResult.Yes Then mnuSaveProj.PerformClick()
    End Sub

    Private Sub LoadSegmentShapefile()
        'see if shape file exists; if so, load it
        Dim WASPShapefileName As String = IO.Path.ChangeExtension(pPlugIn.WASPProject.Filename, ".shp")
        If My.Computer.FileSystem.FileExists(WASPShapefileName) Then

            'if there is already a layer by this name on the map, remove it
            If GisUtil.IsLayer(WASPSegmentLayerName) Then
                GisUtil.RemoveLayer(GisUtil.LayerIndex(WASPSegmentLayerName))
            End If

            GisUtil.AddGroup("WASP Builder")
            GisUtil.AddLayerToGroup(WASPShapefileName, WASPSegmentLayerName, "WASP Builder")
            Logger.Progress(100, 100) 'force it to close (for some reason, was stuck on 0 of 100 down in the bowels of MW or BASINS)
            Dim lNewLayerIndex As Integer = GisUtil.LayerIndex(WASPShapefileName)
            GisUtil.LayerVisible(lNewLayerIndex) = True
            GisUtil.SetLayerLineSize(lNewLayerIndex, 2)
            'add rendering
            GisUtil.UniqueValuesRenderer(lNewLayerIndex, GisUtil.FieldIndex(lNewLayerIndex, "WASPID"))
            GisUtil.LayerVisible(lNewLayerIndex) = False
            GisUtil.LayerVisible(lNewLayerIndex) = True
            'pPlugIn.MapWin.Layers(lNewLayerIndex).UpdateLabelInfo()
        End If
    End Sub

    Private Sub btnOpen_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOpen.Click, mnuOpenLast.Click
        CheckModified()
        Dim LastFileName As String = GetSetting(REGAPPNAME, "Settings", "LastFilename")
        If LastFileName <> "" AndAlso My.Computer.FileSystem.FileExists(LastFileName) Then
            EnableControls(False)
            pPlugIn.WASPProject.LoadProject(LastFileName)
            LoadSegmentShapefile()
            LoadForm()
            EnableControls(True)
        Else
            mnuOpenProj.PerformClick()
        End If
        IsModified = False
    End Sub

    Private Sub mnuOpenProj_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuOpenProj.Click
        CheckModified()
        Dim InitialDir As String = ""
        If pPlugIn.WASPProject.Filename = "" Then
            InitialDir = IO.Path.GetDirectoryName(pPlugIn.MapWin.Project.FileName)
        Else
            InitialDir = IO.Path.GetDirectoryName(pPlugIn.WASPProject.Filename)
        End If
        Dim FileName As String = OpenFile(InitialDir)
        If FileName <> "" Then
            If pPlugIn.WASPProject.LoadProject(FileName) Then
                LoadSegmentShapefile()
                EnableControls(False)

                'need to load information from GIS file
                pSegmentLayerIndex = GisUtil.LayerIndex("WASP Segments")
                pSelectedIndexes = New atcCollection
                For lIndex As Integer = 0 To GisUtil.NumFeatures(pSegmentLayerIndex) - 1
                    pSelectedIndexes.Add(lIndex, lIndex)
                Next
                LoadForm()
                EnableControls(True)
            End If
            IsModified = False
        End If
    End Sub

    Private Sub btnNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuNewProj.Click
        CheckModified()
        InitializeUI(pPlugIn, pSelectedIndexes, pSegmentLayerIndex)
        LoadForm()
    End Sub

    Private Sub mnuNewSelect_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNew.Click, mnuNewSelect.Click
        CheckModified()
        Close()
        pPlugIn.WASPInitialize()
        IsModified = True
    End Sub

    Private Sub dg_CellContentClick2(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgFlow.CellContentClick, dgBound.CellContentClick, dgLoad.CellContentClick, dgTime.CellContentClick
        IsModified = True
    End Sub

    Private Sub dgFlow_CellContentClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgFlow.CellContentClick
        Dim dg As DataGridView = sender
        If TypeOf dg.Columns(e.ColumnIndex) Is DataGridViewLinkColumn And e.RowIndex >= 0 Then 'clicked on link
            Dim sel As clsTimeSeriesSelection = dg.Item(e.ColumnIndex, e.RowIndex).Tag
            If sel.SelectionType = clsTimeSeriesSelection.enumSelectionType.None Then 'set equal to average flow in prior column so will be default
                sel = New clsTimeSeriesSelection(clsTimeSeriesSelection.enumSelectionType.Constant, , , dg.Item(e.ColumnIndex - 1, e.RowIndex).Value)
            End If
            With New frmWASPSelectTimeSeries(pPlugIn, String.Format("for {0} at {1}", dg.Columns(e.ColumnIndex).HeaderText.Replace(vbLf, " "), dg.Item(0, e.RowIndex).Value), sel)
                Dim res As DialogResult = .ShowDialog(Me)
                sel = .Selection
                If sel IsNot Nothing Then
                    Select Case res
                        Case Windows.Forms.DialogResult.OK, Windows.Forms.DialogResult.Abort
                            With dg.Item(e.ColumnIndex, e.RowIndex)
                                .Value = sel.ToString
                                .Tag = sel
                            End With
                        Case Windows.Forms.DialogResult.Retry
                            For c As Integer = 0 To dg.ColumnCount - 1
                                If Not dg.Columns(c).ReadOnly Then
                                    For r As Integer = 0 To dg.RowCount - 1
                                        With dg.Item(c, r)
                                            .Value = sel.ToString
                                            .Tag = sel
                                        End With
                                    Next
                                End If
                            Next
                    End Select
                End If
            End With
        End If
    End Sub

    Private Sub dg_CellContentClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgBound.CellContentClick, dgLoad.CellContentClick, dgTime.CellContentClick
        Dim dg As DataGridView = sender
        If TypeOf dg.Columns(e.ColumnIndex) Is DataGridViewLinkColumn And e.RowIndex >= 0 Then 'clicked on link
            Dim sel As clsTimeSeriesSelection = dg.Item(e.ColumnIndex, e.RowIndex).Tag
            Dim AddlTitle As String = ""
            If sender Is dgTime Then
                AddlTitle = String.Format("for {0}", dg.Item(0, e.RowIndex).Value)
            Else
                AddlTitle = String.Format("for {0} at {1}", dg.Columns(e.ColumnIndex).HeaderText.Replace(vbCr, " "), dg.Item(0, e.RowIndex).Value)
            End If
            With New frmWASPSelectTimeSeries(pPlugIn, AddlTitle, dg.Item(e.ColumnIndex, e.RowIndex).Tag)
                Dim res As DialogResult = .ShowDialog(Me)
                sel = .Selection
                If sel IsNot Nothing Then
                    Select Case res
                        Case Windows.Forms.DialogResult.OK, Windows.Forms.DialogResult.Abort
                            With dg.Item(e.ColumnIndex, e.RowIndex)
                                .Value = sel.ToString
                                .Tag = sel
                            End With
                        Case Windows.Forms.DialogResult.Retry
                            For c As Integer = 0 To dg.ColumnCount - 1
                                If Not dg.Columns(c).ReadOnly Then
                                    For r As Integer = 0 To dg.RowCount - 1
                                        With dg.Item(c, r)
                                            .Value = sel.ToString
                                            .Tag = sel
                                        End With
                                    Next
                                End If
                            Next
                    End Select
                End If
            End With
            dg.AutoResizeColumn(e.ColumnIndex)
        End If
    End Sub

    Private Sub frmWASPSetup_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If IsModified Then
            Select Case MessageBox.Show("You have unsaved changes to this project. Do you want to save your WASP Builder project before closing?", "Warning", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1)
                Case Windows.Forms.DialogResult.Yes : mnuSaveProj.PerformClick()
                Case Windows.Forms.DialogResult.No : e.Cancel = False
                Case Windows.Forms.DialogResult.Cancel : e.Cancel = True
            End Select
        End If
    End Sub

    Private Sub frmWASPSetup_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.Control Then
            Select Case e.KeyCode
                Case Keys.N : mnuNewSelect.PerformClick()
                Case Keys.O : mnuOpenProj.PerformClick()
                Case Keys.L : mnuOpenLast.PerformClick()
                Case Keys.S : mnuSaveProj.PerformClick()
                Case Keys.A : mnuSaveAs.PerformClick()
            End Select
        End If
    End Sub

    Private Sub frmWASPSetup_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GetWindowPos(REGAPPNAME, Me)
        Application.EnableVisualStyles()
        IsModified = False
    End Sub

    Private Sub frmWASPSetup_Shown(sender As Object, e As System.EventArgs) Handles Me.Shown
        Dim wrdbinifolder As String = My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\WRDB"
        Dim wrdbinifile As String = wrdbinifolder & "\WRDB.ini"
        If Not (My.Computer.FileSystem.DirectoryExists(wrdbinifolder) AndAlso My.Computer.FileSystem.FileExists(wrdbinifile)) Then
            WarningMsg("The WASP Model Builder can be used to import time series data for WASP input using WRDB 5.0. However, it appears that you do not yet have WRDB 5.0 installed. We suggest that you do so by downloading the software from www.wrdb.com.")
            If Not My.Computer.FileSystem.DirectoryExists(wrdbinifolder) Then My.Computer.FileSystem.CreateDirectory(wrdbinifolder)
            With IO.File.Create(wrdbinifile)
                .Close()
                .Dispose()
            End With
        End If
    End Sub

    Private Sub btnReselect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReselect.Click
        Me.Visible = False
        pPlugIn.WASPInitialize()
        IsModified = True
    End Sub

    Private Sub btnBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowse.Click
        With New FolderBrowserDialog
            .Description = "Select Folder for Wasp Files"
            .RootFolder = Environment.SpecialFolder.MyDocuments
            .SelectedPath = txtOutputDir.Text
            .ShowNewFolderButton = True
            If .ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                txtOutputDir.Text = .SelectedPath
            End If
        End With
    End Sub

    Private Sub cboModel_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboModel.SelectionChangeCommitted
        IsModified = True
    End Sub

    Private Sub dgv_HasBeenModified(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgvFlow.HasBeenModified, dgvBound.HasBeenModified, dgvLoad.HasBeenModified, dgvTime.HasBeenModified
        IsModified = True
    End Sub

    Private Sub tbxName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbxName.KeyPress
        IsModified = True
    End Sub

    Private Sub atx_Change() Handles atxEDay.Change, atxEMonth.Change, atxEYear.Change, atxSDay.Change, atxSMonth.Change, atxSYear.Change, atxTravelTimeMax.Change, atxTravelTimeMin.Change
        IsModified = True
    End Sub

    Private Sub TabMain_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabMain.SelectedIndexChanged
        If TabMain.SelectedIndex > 0 AndAlso pPlugIn.WASPProject.Segments.Count = 0 Then
            MessageBox.Show("You must either open an existing WASP Builder project (Open button) or build a new one (New button) before continuing.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            TabMain.SelectedIndex = 0
        End If
    End Sub

    Private Sub dgSegmentation_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgSegmentation.CellEndEdit
        If e.ColumnIndex = 2 Then
            'may need to refresh due to segment name getting revised
            UpdateStatus("Saving Wasp Builder data file and rebuilding shape file...")
            RefreshingSelection = True 'so creation of grids doesn't intereact with map
            SaveForm()
            SetFlowStationGrid()
            SetBoundaryGrid()
            SetLoadStationGrid()
            UpdateStatus()
            RefreshingSelection = False
            'UpdateStatus()
        End If
    End Sub

    Private Sub dg_RowEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgBound.RowEnter, dgFlow.RowEnter, dgLoad.RowEnter
        HighlightSegment(CInt(CType(sender, DataGridView).Item(0, e.RowIndex).Value))
    End Sub

    Private Sub dg_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgBound.Leave, dgFlow.Leave, dgLoad.Leave
        With CType(sender, DataGridView)
            .ClearSelection()
            ClearHighlight()
        End With
    End Sub

    Private RefreshingSelection As Boolean = False

    ''' <summary>
    ''' Called when MapWindow shapes are selected
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub RefreshSelectionInfo()
        RefreshingSelection = True
        Dim lstIDs As Generic.List(Of Integer) = GetSelectedSegmentIDs()
        With dgSegmentation
            Dim FirstRow As Integer = 0
            .ClearSelection()
            .CurrentCell = Nothing
            Dim NumRowsSelected As Integer = 0
            For r As Integer = 0 To .Rows.Count - 1
                Dim CellSelected As Boolean = False
                For c As Integer = 0 To .Columns.Count - 1
                    If lstIDs.Contains(r + 1) Then .Item(c, r).Selected = True : CellSelected = True
                Next
                If CellSelected Then
                    NumRowsSelected += 1
                    If FirstRow = 0 Then FirstRow = r
                End If
            Next
            lblNumSelected.Text = String.Format("{0} Segments Selected", NumRowsSelected)
            If FirstRow <= .RowCount - 1 Then .FirstDisplayedCell = .Item(0, FirstRow)
            '.Visible = True
        End With

        Dim lstNames As Generic.List(Of String) = GetSelectedSegmentNames()
        For i As Integer = 1 To 3
            Dim FirstRow As Integer = 0
            With CType(Choose(i, dgBound, dgFlow, dgLoad), DataGridView)
                .ClearSelection()
                For r As Integer = 0 To .Rows.Count - 1
                    Dim CellSelected As Boolean = False
                    For c As Integer = 0 To .Columns.Count - 1
                        If lstNames.Contains(.Item(1, r).Value.split(":")(0)) Then .Item(c, r).Selected = True : CellSelected = True
                    Next
                    If CellSelected AndAlso FirstRow = 0 Then FirstRow = r
                Next
                If FirstRow <= .RowCount - 1 Then .FirstDisplayedCell = .Item(0, FirstRow)
            End With
        Next
        RefreshingSelection = False
    End Sub

    Private Sub dgSegmentation_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgSegmentation.SelectionChanged
        If RefreshingSelection Then Exit Sub
        RefreshingSelection = True
        With dgSegmentation
            If .Visible Then
                ClearHighlight()
                For r As Integer = 0 To .Rows.Count - 1
                    Dim CellSelected As Boolean = False
                    For c As Integer = 0 To .Columns.Count - 1
                        If .Item(c, r).Selected Then
                            CellSelected = True
                            Exit For
                        End If
                    Next
                    If CellSelected Then HighlightSegment(r + 1)
                Next
            End If
        End With
        RefreshingSelection = False
    End Sub

    Private Sub dg_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgBound.SelectionChanged, dgFlow.SelectionChanged, dgLoad.SelectionChanged
        If RefreshingSelection Then Exit Sub
        With CType(sender, DataGridView)
            If .Visible Then
                ClearHighlight()
                For r As Integer = 0 To .Rows.Count - 1
                    For c As Integer = 0 To .Columns.Count - 1
                        If .Item(c, r).Selected Then
                            HighlightSegment(CInt(.Item(0, r).Value))
                            Exit For
                        End If
                    Next
                Next
            End If
        End With
    End Sub

    Private Sub btnSegments_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCombineSegments.Click, btnDivideSegments.Click, btnDeleteSegments.Click
        Try
            'combine all highlighted segments, two at a time
            With dgSegmentation

                'highlight all selected rows and save
                Dim lstSelectedSegments As New Generic.List(Of String)

                Dim FirstRow As Integer = -1

                For Each cell As DataGridViewCell In .SelectedCells
                    Dim ID_Name As String = .Item(0, cell.RowIndex).Value
                    If Not lstSelectedSegments.Contains(ID_Name) Then lstSelectedSegments.Add(ID_Name)
                    If FirstRow = -1 Then FirstRow = cell.RowIndex
                Next

                If FirstRow = -1 Then Exit Sub

                If sender Is btnCombineSegments Then

                    If lstSelectedSegments.Count <= 1 Then
                        WarningMsg("Please select two or more connected reaches before trying to combine them.")
                        Exit Sub
                    End If

                    .Visible = False

                    While True

                        Dim Seg1 As atcWASPSegment = Nothing
                        Dim Seg2 As atcWASPSegment = Nothing

                        For r As Integer = 0 To .RowCount - 1
                            Dim ID_Name As String = .Item(0, r).Value
                            Dim ID As String = ID_Name.Split(":")(0)

                            If lstSelectedSegments.Contains(ID_Name) Then
                                'highlight entire row 
                                For c As Integer = 0 To .ColumnCount - 1
                                    .Item(c, r).Selected = True
                                Next
                                If Seg1 Is Nothing Then
                                    Seg1 = pPlugIn.WASPProject.Segments(ID)
                                    FirstRow = r
                                Else
                                    Seg2 = pPlugIn.WASPProject.Segments(ID)
                                    If Seg1.DownID <> Seg2.ID And Seg2.DownID <> Seg1.ID Then
                                        WarningMsg("The selected segments must be logically connected in the system.")
                                        .Visible = True
                                        Exit Sub
                                    Else
                                        pPlugIn.WASPProject.CombineSegments(Seg1, Seg2, Seg2.DownID = Seg1.ID)
                                        LoadForm()
                                    End If
                                    Exit For
                                End If
                            End If
                        Next
                        If Seg2 Is Nothing Then Exit While
                    End While
                ElseIf sender Is btnDeleteSegments Then
                    For i As Integer = pPlugIn.WASPProject.Segments.Count - 1 To 0 Step -1
                        Dim seg As atcWASPSegment = pPlugIn.WASPProject.Segments(i)
                        'make sure no segments would be orphaned
                        If pPlugIn.WASPProject.Segments.Contains(seg.DownID) AndAlso lstSelectedSegments.Contains(pPlugIn.WASPProject.Segments(seg.DownID).ID_Name) Then
                            If WarningMsg("Segment(s) cannot be deleted because one or more other segments would be orphaned. Make sure you delete from upstream to downstream. Do you want to delete anyway?") = Windows.Forms.DialogResult.OK Then
                                Exit For
                            Else
                                .Visible = True
                                Exit Sub
                            End If
                        End If
                    Next
                    For i As Integer = pPlugIn.WASPProject.Segments.Count - 1 To 0 Step -1
                        Dim seg As atcWASPSegment = pPlugIn.WASPProject.Segments(i)
                        If lstSelectedSegments.Contains(seg.ID_Name) Then
                            pPlugIn.WASPProject.Segments.RemoveAt(i)
                        End If
                    Next
                    pPlugIn.WASPProject.Segments.AssignWaspIds()
                    LoadForm()
                    If .RowCount > 0 Then .CurrentCell = .Item(2, Math.Min(.RowCount - 1, Math.Max(0, FirstRow - 1)))
                Else
                    Dim MaxTravelTime As Double = Val(InputBox("What is the maximum travel time you want to use to divide the selected segments?", "Divide Segments", Val(atxTravelTimeMax.Text) / 2 + 0.01))
                    .Visible = False
                    If MaxTravelTime > 0 Then
                        For i As Integer = pPlugIn.WASPProject.Segments.Count - 1 To 0 Step -1
                            FirstRow = i
                            Dim seg As atcWASPSegment = pPlugIn.WASPProject.Segments(i)
                            If lstSelectedSegments.Contains(seg.ID_Name) Then
                                pPlugIn.WASPProject.DivideSegments(MaxTravelTime, seg)
                            End If
                        Next
                        pPlugIn.WASPProject.Segments.AssignWaspIds()
                        LoadForm()
                    End If
                End If
                CreateSegmentShapeFile()
                For c As Integer = 0 To .ColumnCount - 1
                    If .RowCount > 0 Then .Item(c, Math.Min(.RowCount - 1, Math.Max(0, FirstRow))).Selected = True
                Next
                .FirstDisplayedCell = .Item(0, Math.Min(.RowCount - 1, Math.Max(0, FirstRow)))
                .Visible = True
            End With
        Catch ex As Exception
            ErrorMsg(, ex)
        Finally
            dgSegmentation.Visible = True
        End Try
    End Sub

#Region "Create Shapefile routines..."

    ''' <summary>
    ''' Remove all layers referencing a shape file and then delete it and all related files
    ''' </summary>
    ''' <param name="Filename">Name of shape file (must have extension .shp)</param>
    ''' <remarks></remarks>
    Private Sub DeleteShapeFile(ByVal Filename As String)
        If GisUtil.IsLayerByFileName(Filename) Then
            For i As Integer = GisUtil.NumLayers - 1 To 0 Step -1
                If GisUtil.LayerFileName(i).ToUpper = Filename.ToUpper Then
                    GisUtil.RemoveLayer(i)
                End If
            Next
        End If
        For i As Integer = 1 To 4
            Dim fn As String = IO.Path.ChangeExtension(Filename, Choose(i, ".shp", ".dbf", ".shx", ".prj"))
            If My.Computer.FileSystem.FileExists(fn) Then
                My.Computer.FileSystem.DeleteFile(fn)
            End If
        Next
    End Sub

    ''' <summary>
    ''' Create a new shapefile to be saved with the waspbuilder project file containing line segments for each segment
    ''' </summary>
    Public Sub CreateSegmentShapeFile()

        'this cannot be called unless the project has been saved
        If String.IsNullOrEmpty(pPlugIn.WASPProject.Filename) Then
            Logger.Msg("The project must be saved before the shapefile can be created.")
            Exit Sub
        End If

        'store shape file in same folder as project file and give same name (will overwrite without warning)

        Dim WASPShapefileName As String = IO.Path.ChangeExtension(pPlugIn.WASPProject.Filename, ".shp")

        DeleteShapeFile(WASPShapefileName)

        'create the new empty shapefile and add to group
        GisUtil.CreateEmptyShapefile(WASPShapefileName, "", "line")
        GisUtil.AddGroup("WASP Builder")
        GisUtil.AddLayerToGroup(WASPShapefileName, WASPSegmentLayerName, "WASP Builder")
        Dim lNewLayerIndex As Integer = GisUtil.LayerIndex(WASPShapefileName)
        GisUtil.LayerVisible(lNewLayerIndex) = True

        'add an id field to the new shapefile
        Dim lNewIDFieldIndex As Integer = GisUtil.AddField(lNewLayerIndex, "SEGID", 0, 20)
        Dim lNewWASPIDFieldIndex As Integer = GisUtil.AddField(lNewLayerIndex, "WASPID", 1, 10) 'integer
        Dim lNewWASPNameFieldIndex As Integer = GisUtil.AddField(lNewLayerIndex, "WASPNAME", 0, 50)
        Dim lNewNameFieldIndex As Integer = GisUtil.AddField(lNewLayerIndex, "NAME", 0, 50)
        Dim lNewLengthFieldIndex As Integer = GisUtil.AddField(lNewLayerIndex, "LENGTH", 2, 10)
        Dim lNewFlowFieldIndex As Integer = GisUtil.AddField(lNewLayerIndex, "MEANFLOW", 2, 10)
        Dim lNewSlopeFieldIndex As Integer = GisUtil.AddField(lNewLayerIndex, "SLOPE", 2, 10)
        Dim lNewTravelTimeFieldIndex As Integer = GisUtil.AddField(lNewLayerIndex, "TRAVELTIME", 2, 10)

        'add line segments to the shape file (do not use GISUtil.Addline because it is too slow!)
        Dim lMWLayerIndex As Integer = 0
        For lindex As Integer = 0 To pPlugIn.MapWin.Layers.NumLayers - 1
            If pPlugIn.MapWin.Layers(lindex).Name = "WASP Segments" Then
                lMWLayerIndex = lindex
            End If
        Next

        Dim sf As MapWinGIS.Shapefile = pPlugIn.MapWin.Layers(lMWLayerIndex).GetObject
        Dim Success As Boolean = True

        If sf.StartEditingShapes(True) Then
            For s As Integer = 1 To pPlugIn.WASPProject.Segments.Count
                For Each lSegment As atcWASPSegment In pPlugIn.WASPProject.Segments
                    With lSegment
                        If .WaspID = s Then
                            Dim shpLine As New MapWinGIS.Shape
                            If Not shpLine.Create(MapWinGIS.ShpfileType.SHP_POLYLINE) Then Success = False : Exit For
                            For i As Integer = 0 To .PtsX.Length - 1
                                Dim Point As New MapWinGIS.Point
                                Point.x = .PtsX(i)
                                Point.y = .PtsY(i)
                                If Not shpLine.InsertPoint(Point, shpLine.numPoints) Then Success = False : Exit For
                            Next
                            If Success AndAlso Not sf.EditInsertShape(shpLine, sf.NumShapes) Then Success = False : Exit For
                            sf.EditCellValue(lNewIDFieldIndex, sf.NumShapes - 1, .ID)
                            sf.EditCellValue(lNewWASPIDFieldIndex, sf.NumShapes - 1, .WaspID)
                            sf.EditCellValue(lNewWASPNameFieldIndex, sf.NumShapes - 1, .WaspName)
                            sf.EditCellValue(lNewNameFieldIndex, sf.NumShapes - 1, .Name)
                            sf.EditCellValue(lNewLengthFieldIndex, sf.NumShapes - 1, Math.Round(.Length, 3))
                            sf.EditCellValue(lNewFlowFieldIndex, sf.NumShapes - 1, Math.Round(.MeanAnnualFlow, 2))
                            sf.EditCellValue(lNewSlopeFieldIndex, sf.NumShapes - 1, Math.Round(.Slope, 5))
                            sf.EditCellValue(lNewTravelTimeFieldIndex, sf.NumShapes - 1, Math.Round(pPlugIn.WASPProject.TravelTime(.Length, .Velocity), 3))
                            Exit For
                        End If
                    End With
                Next
            Next
            If Success AndAlso Not sf.StopEditingShapes(True, True) Then Success = False
        End If

        'add rendering
        GisUtil.SetLayerLineSize(lNewLayerIndex, 3)
        'this got broken in MW4.8
        GisUtil.UniqueValuesRenderer(lNewLayerIndex, GisUtil.FieldIndex(lNewLayerIndex, "WASPID"))
        GisUtil.LayerVisible(lNewLayerIndex) = False
        GisUtil.LayerVisible(lNewLayerIndex) = True
        LabelSegmentLayer()
        Logger.Progress(100, 100)
    End Sub

    Private Sub LabelSegmentLayer()
        'add labeling
        If String.IsNullOrEmpty(pPlugIn.WASPProject.Filename) Then Exit Sub
        Dim WASPShapefileName As String = IO.Path.ChangeExtension(pPlugIn.WASPProject.Filename, ".shp")
        If Not GisUtil.IsLayerByFileName(WASPShapefileName) Then Exit Sub
        'Dim lNewLayerIndex As Integer = GisUtil.LayerIndex(WASPShapefileName)
        Dim lNewLayerIndex As Integer = 0
        For lindex As Integer = 0 To pPlugIn.MapWin.Layers.NumLayers - 1
            If pPlugIn.MapWin.Layers(lindex).FileName = WASPShapefileName Then
                lNewLayerIndex = lindex
            End If
        Next
        Dim sf As MapWinGIS.Shapefile = pPlugIn.MapWin.Layers(lNewLayerIndex).GetObject
        With pPlugIn.MapWin.Layers(lNewLayerIndex)
            .Font("Arial", 7)
            .ClearLabels()
            .UseLabelCollision = False
        End With
        If cboLabelLayer.Text <> "(None)" Then
            For i As Integer = 0 To sf.NumShapes - 1
                Dim fldidx As Integer = GisUtil.FieldIndex(lNewLayerIndex, cboLabelLayer.Text)
                Dim text As String = GisUtil.FieldValue(lNewLayerIndex, i, fldidx)
                With sf.Shape(i)
                    Dim sumX As Double = 0.0, sumY As Double = 0.0
                    For j As Integer = 0 To sf.Shape(i).numPoints - 1
                        With .Point(j)
                            sumX += .x
                            sumY += .y
                        End With
                    Next
                    pPlugIn.MapWin.Layers(lNewLayerIndex).AddLabel(text, Color.Black, sumX / .numPoints, sumY / .numPoints, MapWinGIS.tkHJustification.hjCenter)
                    pPlugIn.MapWin.Layers(lNewLayerIndex).LabelsShadow = True
                End With
            Next
        End If
    End Sub

    ''' <summary>
    ''' If the segment shapefile is found, highlight the specified segment
    ''' </summary>
    ''' <param name="WaspID">ID of segment</param>
    ''' <remarks></remarks>
    Public Sub HighlightSegment(ByVal WaspID As Integer)
        If GisUtil.IsLayer(WASPSegmentLayerName) Then
            Dim LayerIndex As Integer = GisUtil.LayerIndex(WASPSegmentLayerName)
            GisUtil.SetSelectedFeature(LayerIndex, WaspID - 1)
            UpdateSegmentLabel()
        End If
    End Sub

    ''' <summary>
    ''' If the segment shapefile is found, highlight the specified segment
    ''' </summary>
    ''' <param name="WaspID">ID of segment</param>
    ''' <remarks></remarks>
    Public Sub HighlightSegment(ByVal WaspID As Generic.List(Of Integer))
        If GisUtil.IsLayer(WASPSegmentLayerName) Then
            Dim LayerIndex As Integer = GisUtil.LayerIndex(WASPSegmentLayerName)
            If Not GisUtil.IsField(LayerIndex, "WASPID") Then Exit Sub
            Dim FieldIndex As Integer = GisUtil.FieldIndex(LayerIndex, "WASPID")
            For i As Integer = 0 To GisUtil.NumFeatures(LayerIndex) - 1
                If WaspID.Contains(Val(GisUtil.FieldValue(LayerIndex, i, FieldIndex))) Then
                    GisUtil.SetSelectedFeature(LayerIndex, i)
                End If
            Next
            UpdateSegmentLabel()
        End If
    End Sub

    Public Sub ClearHighlight()
        If GisUtil.IsLayer(WASPSegmentLayerName) Then
            Dim LayerIndex As Integer = GisUtil.LayerIndex(WASPSegmentLayerName)
            GisUtil.ClearSelectedFeatures(LayerIndex)
            UpdateSegmentLabel()
        End If
    End Sub

    Private Sub UpdateSegmentLabel()
        With dgSegmentation
            Dim NumRowsSelected As Integer = 0
            For r As Integer = 0 To .Rows.Count - 1
                Dim CellSelected As Boolean = False
                For c As Integer = 0 To .Columns.Count - 1
                    If .Item(c, r).Selected Then CellSelected = True
                Next
                If CellSelected Then NumRowsSelected += 1
            Next
            lblNumSelected.Text = String.Format("{0} Segments Selected", NumRowsSelected)
        End With
    End Sub

    ''' <summary>
    ''' If the segment shapefile is found, return IDs of selected segments
    ''' </summary>
    ''' <remarks></remarks>
    Public Function GetSelectedSegmentIDs() As Generic.List(Of Integer)
        Dim lst As New Generic.List(Of Integer)
        If GisUtil.IsLayer(WASPSegmentLayerName) Then
            Dim LayerIndex As Integer = GisUtil.LayerIndex(WASPSegmentLayerName)
            Dim FieldIndex As Integer = GisUtil.FieldIndex(LayerIndex, "WASPID")
            If FieldIndex <> -1 Then
                For i As Integer = 0 To GisUtil.NumSelectedFeatures(LayerIndex) - 1
                    lst.Add(Val(GisUtil.FieldValue(LayerIndex, GisUtil.IndexOfNthSelectedFeatureInLayer(i, LayerIndex), FieldIndex)))
                Next
            End If
        End If
        Return lst
    End Function

    ''' <summary>
    ''' If the segment shapefile is found, return IDs Names selected segments
    ''' </summary>
    ''' <remarks></remarks>
    Public Function GetSelectedSegmentNames() As Generic.List(Of String)
        Dim lst As New Generic.List(Of String)
        If GisUtil.IsLayer(WASPSegmentLayerName) Then
            Dim LayerIndex As Integer = GisUtil.LayerIndex(WASPSegmentLayerName)
            Dim FieldIndex As Integer = GisUtil.FieldIndex(LayerIndex, "WASPNAME")
            If FieldIndex <> -1 Then
                For i As Integer = 0 To GisUtil.NumSelectedFeatures(LayerIndex) - 1
                    lst.Add(Val(GisUtil.FieldValue(LayerIndex, GisUtil.IndexOfNthSelectedFeatureInLayer(i, LayerIndex), FieldIndex)))
                Next
            End If
        End If
        Return lst
    End Function

    Public Sub CreateBufferedSegmentShapeFile() '(ByVal aWASPShapefileFilename As String)
        'this cannot be called unless the project has been saved
        If String.IsNullOrEmpty(pPlugIn.WASPProject.Filename) Then
            Logger.Msg("The project must be saved before the shapefile can be created.")
            Exit Sub
        End If

        'store shape file in same folder as project file and give same name (will overwrite without warning)

        Dim WASPShapefileName As String = IO.Path.ChangeExtension(pPlugIn.WASPProject.Filename, ".shp")
        Dim WASPBuffShapefileName As String = WASPShapefileName.Replace(".shp", "-Buffered.shp")

        DeleteShapeFile(WASPBuffShapefileName)
        GisUtil.BufferLayer(WASPShapefileName, WASPBuffShapefileName, 100)

        If GisUtil.IsLayer(WASPSegmentLayerName & "-Buffered") Then
            GisUtil.RemoveLayer(GisUtil.LayerIndex(WASPSegmentLayerName & "-Buffered"))
        End If
        GisUtil.AddGroup("WASP Builder")
        GisUtil.AddLayerToGroup(WASPBuffShapefileName, WASPSegmentLayerName & "-Buffered", "WASP Builder")
        Dim lNewLayerIndex As Integer = GisUtil.LayerIndex(WASPBuffShapefileName)
        GisUtil.LayerVisible(lNewLayerIndex) = True

        'add rendering
        Dim lLayerIndex As Integer = GisUtil.LayerIndex(WASPBuffShapefileName)
        GisUtil.UniqueValuesRenderer(lLayerIndex, GisUtil.FieldIndex(lLayerIndex, "WASPID"))
        GisUtil.LayerVisible(lNewLayerIndex) = False
        GisUtil.LayerVisible(lNewLayerIndex) = True
        GisUtil.SetLayerLineSize(lLayerIndex, 0)
        Logger.Progress(100, 100)
    End Sub

    ''' <summary>
    ''' This routine is not used anymore
    ''' </summary>
    ''' <param name="aSegmentLayerIndex"></param>
    ''' <param name="aSelectedIndexes"></param>
    ''' <remarks></remarks>
    Public Sub CreateFlowlineShapeFile(ByVal aSegmentLayerIndex As Integer, ByRef aSelectedIndexes As atcCollection)
        Dim lNewFileName As String = ""
        Dim lSelectedIndexCollection As New Collection
        For Each lSelectedIndex As Object In aSelectedIndexes
            lSelectedIndexCollection.Add(lSelectedIndex)
        Next
        GisUtil.SaveSelectedFeatures(aSegmentLayerIndex, lSelectedIndexCollection, lNewFileName, "polylinez")
        If GisUtil.IsLayer("Flowlines for WASP Project") Then
            GisUtil.RemoveLayer(GisUtil.LayerIndex("Flowlines for WASP Project"))
        End If
        GisUtil.AddLayer(lNewFileName, "Flowlines for WASP Project")
        Dim lNewLayerIndex As Integer = GisUtil.LayerIndex(lNewFileName)
        GisUtil.LayerVisible(lNewLayerIndex) = True
        GisUtil.SetLayerLineSize(lNewLayerIndex, 2)
    End Sub

#End Region

    Private Sub cboLabelLayer_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cboLabelLayer.SelectedIndexChanged
        LabelSegmentLayer()
    End Sub

End Class
