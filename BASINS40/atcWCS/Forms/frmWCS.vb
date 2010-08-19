Imports MapWinUtility
Imports atcUtility

Public Class frmWCS

    Private Sub UpdProg(ByVal Message As String, ByVal ItemNum As Integer, ByVal MaxItemNum As Integer)
        If Message <> "" And ItemNum <> MaxItemNum Then
            lblProgress.Text = Message
            ProgressBar.Value = ItemNum * 100.0 / MaxItemNum
            lnkCancel.LinkVisited = False
            tblProgress.Visible = True
        Else
            tblProgress.Visible = False
        End If
    End Sub

    Friend Function UpdateProgress(ByVal Message As String, ByVal ItemNum As Integer, ByVal MaxItemNum As Integer) As Boolean
        UpdProg(Message, ItemNum, MaxItemNum)
        Return Not GisUtil.Cancel
    End Function

    Private Sub btnAbout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAbout.Click
        Logger.Msg("BASINS Watershed Characterization System (WCS)" & vbCrLf & vbCrLf & _
           "Version " & System.Reflection.Assembly.GetExecutingAssembly.GetName.Version.ToString, _
           "BASINS - WCS")
    End Sub

    Private Sub btnAllNone_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAll.Click, btnNone.Click, btnNoneDS.Click, btnAllDS.Click
        Dim lst As CheckedListBox
        If sender Is btnAll Or sender Is btnNone Then
            lst = lstReports
        Else
            lst = lstDataSources
        End If
        For i As Integer = 0 To lst.Items.Count - 1
            lst.SetItemChecked(i, sender Is btnAll Or sender Is btnAllDS)
        Next
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        SaveForm()
        Close()
    End Sub

    ''' <summary>
    ''' Copy report output in HTML format to clipboard
    ''' </summary>
    Private Sub btnCopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCopy.Click
        Dim Header As String = String.Format("Version:1.0{0}StartHTML:0000000105{0}EndHTML:{1,10:0}{0}StartFragment:0000000105{0}EndFragment:{1,10:0}{0}", vbCrLf, wbResults.DocumentText.Length + 105)
        Clipboard.SetDataObject(New DataObject(DataFormats.Html, Header & wbResults.DocumentText))
    End Sub

    Private Sub btnGenerate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGenerate.Click

        If lstReports.CheckedItems.Count = 0 Then
            WarningMsg("You must select which reports you want to generate first!")
            tabWCS.SelectedIndex = 1
            Exit Sub
        End If

        tabWCS.SelectedIndex = 2
        wbResults.DocumentText = ""

        EnableControls(False)
        Application.DoEvents()

        'save current settings as defaults
        SaveForm()

        Logger.Status("Refreshing Results report...", False)
        Dim filename As String = Project.ConcatReport(lstReports.CheckedIndices)
        wbResults.Navigate(filename)

        lblGenerate.Text = String.Format("{0} (Click Generate button to refresh results)", IO.Path.GetFileName(filename))
        EnableControls(True)

        If lstReports.CheckedIndices.Contains(4) Then 'land use report was generated
            Select Case Project.LanduseType
                Case clsProject.enumLandUseType.UserGrid, clsProject.enumLandUseType.UserShapefile
                    LoadLanduseGrid()
            End Select
        End If
    End Sub

    Private Sub btnOutputName_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOutputName.Click
        Dim btn As Button = sender
        Dim txt As TextBox = txtOutputName
        With New FolderBrowserDialog
            .SelectedPath = Project.OutputFolder
            .Description = "Select path where WCS reports will be saved:"
            .RootFolder = Environment.SpecialFolder.MyComputer
            If .ShowDialog(Me) = Windows.Forms.DialogResult.OK Then txt.Text = .SelectedPath
            .Dispose()
        End With
    End Sub

    Private Sub btnPreview_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPreview.Click
        wbResults.ShowPrintPreviewDialog()
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        wbResults.ShowPrintDialog()
    End Sub

    Private Sub cboLandUseType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboLandUseType.SelectedIndexChanged
        cboLanduseLayer.Enabled = True
        lblLanduseLayer.Enabled = True
        cboLanduseField.Enabled = False
        cboLanduseLayer.Items.Clear()
        Project.LanduseType = cboLandUseType.SelectedIndex
        Select Case Project.LanduseType
            Case clsProject.enumLandUseType.GIRAS
                cboLanduseLayer.Enabled = False
                lblLanduseLayer.Enabled = False
            Case clsProject.enumLandUseType.UserShapefile
                cboLanduseField.Enabled = True
        End Select
        For j As Integer = 0 To GisUtil.NumLayers - 1
            Dim lt As MapWindow.Interfaces.eLayerType = GisUtil.LayerType(j)
            If lt = MapWindow.Interfaces.eLayerType.PolygonShapefile And Project.LanduseType = clsProject.enumLandUseType.UserShapefile Then
                cboLanduseLayer.Items.Add(GisUtil.LayerName(j))
            ElseIf lt = MapWindow.Interfaces.eLayerType.Grid And Project.LanduseType <> clsProject.enumLandUseType.GIRAS And Project.LanduseType <> clsProject.enumLandUseType.UserShapefile Then
                cboLanduseLayer.Items.Add(GisUtil.LayerName(j))
            End If
        Next
        If cboLanduseLayer.Items.Count > 0 Then cboLanduseLayer.SelectedIndex = 0
        LoadLanduseGrid()
    End Sub

    Private Sub cboLayer_SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboSubbasinLayer.SelectedValueChanged, cboPop1Layer.SelectedValueChanged, cboSewerLayer.SelectedValueChanged, cboSoilLayer.SelectedValueChanged, cboLanduseLayer.SelectedValueChanged, cboReachLayer.SelectedValueChanged, cbo303dLayer.SelectedValueChanged, cboPCSLayer.SelectedValueChanged
        With Project
            If sender Is cboSubbasinLayer Then
                FillFields(sender, cboSubbasinField, .SubbasinField)
            ElseIf sender Is cboReachLayer Then
                FillFields(sender, cboReachField, .ReachField)
            ElseIf sender Is cbo303dLayer Then
                FillFields(sender, cboWaterBodyField, .WaterbodyField)
                FillFields(sender, cboImpairmentField, .ImpairmentField)
            ElseIf sender Is cboPop1Layer Then
                FillFields(sender, cboPopNameField, .PopNameField)
                FillFields(sender, cboPopPopField, .PopPopField)
            ElseIf sender Is cboSewerLayer Then
                FillFields(sender, cboSewerNameField, .SewerNameField)
                FillFields(sender, cboSewerPopField, .SewerPopField)
                FillFields(sender, cboSewerHouseField, .SewerHouseField)
                FillFields(sender, cboSewerPublicField, .SewerPublicField)
                FillFields(sender, cboSewerSepticField, .SewerSepticField)
                FillFields(sender, cboSewerOtherField, .SewerOtherField)
            ElseIf sender Is cboSoilLayer Then
                FillFields(sender, cboSoilField, .SoilField)
            ElseIf sender Is cboLanduseLayer And Project.LanduseType = clsProject.enumLandUseType.UserShapefile Then
                FillFields(sender, cboLanduseField, .LanduseField)
            ElseIf sender Is cboPCSLayer Then
                FillFields(sender, cboNPDESField, .PCSNpdesField)
                FillFields(sender, cboFacNameField, .PCSFacNameField)
                FillFields(sender, cboSICField, .PCSSicField)
                FillFields(sender, cboSICNameField, .PCSSicNameField)
                FillFields(sender, cboCityField, .PCSCityField)
                FillFields(sender, cboMajorField, .PCSMajorField)
                FillFields(sender, cboRecWaterField, .PCSRecWaterField)
                FillFields(sender, cboActiveField, .PCSActiveField)
            End If
        End With
    End Sub

    Private Sub cboLU_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboLandUseType.SelectionChangeCommitted, cboLanduseLayer.SelectionChangeCommitted, cboLanduseField.SelectionChangeCommitted
        lstReports.SetItemChecked(4, True)
    End Sub

    ''' <summary>
    ''' Land use field has changed or User grid selected; show Refresh button
    ''' </summary>
    Private Sub cboLUField_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboLanduseField.SelectionChangeCommitted, cboLandUseType.SelectionChangeCommitted, cboLanduseLayer.SelectionChangeCommitted
        'tblLanduse.Visible = False
        'dgLandUse.ReadOnly = True
        'Select Case CType(cboLandUseType.SelectedIndex, clsProject.enumLandUseType)
        '    Case clsProject.enumLandUseType.UserGrid, clsProject.enumLandUseType.UserShapefile
        '        tblLanduse.Visible = True
        '        dgLandUse.ReadOnly = False
        'End Select
    End Sub

    Private Sub cboPCS_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboPCSLayer.SelectionChangeCommitted, cboNPDESField.SelectionChangeCommitted, cboFacNameField.SelectionChangeCommitted, cboSICField.SelectionChangeCommitted, cboSICNameField.SelectionChangeCommitted, cboCityField.SelectionChangeCommitted, cboMajorField.SelectionChangeCommitted, cboWaterBodyField.SelectionChangeCommitted, cboActiveField.SelectionChangeCommitted
        lstReports.SetItemChecked(5, True)
    End Sub

    Private Sub cboPop_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboPop1Layer.SelectionChangeCommitted, cboPop2Layer.SelectionChangeCommitted, cboPopNameField.SelectionChangeCommitted, cboPopPopField.SelectionChangeCommitted
        lstReports.SetItemChecked(1, True)
    End Sub

    Private Sub cboSewage_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboSewerLayer.SelectionChangeCommitted, cboSewerHouseField.SelectionChangeCommitted, cboSewerNameField.SelectionChangeCommitted, cboSewerOtherField.SelectionChangeCommitted, cboSewerPopField.SelectionChangeCommitted, cboSewerPublicField.SelectionChangeCommitted, cboSewerSepticField.SelectionChangeCommitted
        lstReports.SetItemChecked(2, True)
    End Sub

    Private Sub cboSoils_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboSoilLayer.SelectionChangeCommitted, cboSoilField.SelectionChangeCommitted
        lstReports.SetItemChecked(3, True)
    End Sub

    Private Sub cboWater_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboReachLayer.SelectionChangeCommitted, cboReachField.SelectionChangeCommitted, cboReachField.SelectionChangeCommitted, cbo303dLayer.SelectionChangeCommitted, cboWaterBodyField.SelectionChangeCommitted, cboImpairmentField.SelectionChangeCommitted
        lstReports.SetItemChecked(0, True)
    End Sub

    Private Sub EnableControls(ByVal b As Boolean)
        btnGenerate.Enabled = b
        btnCancel.Enabled = b
        btnAbout.Enabled = b
        tabWCS.Enabled = b
        tabReports.Enabled = b
    End Sub

    Private Sub FillFields(ByVal cboLayer As ComboBox, ByVal cboField As ComboBox, ByVal defaultField As String)
        If Not GisUtil.IsLayer(cboLayer.Text) Then Exit Sub
        Dim lyr As Integer = GisUtil.LayerIndex(cboLayer.Text)
        Dim lt As MapWindow.Interfaces.eLayerType = GisUtil.LayerType(lyr)
        Dim cboText As String = ""
        With cboField.Items
            .Clear()
            If lt <> MapWindow.Interfaces.eLayerType.Grid Then
                For i As Integer = 0 To GisUtil.NumFields(lyr) - 1
                    Dim fld As String = GisUtil.FieldName(i, lyr)
                    .Add(fld)
                    If String.Equals(fld, defaultField, StringComparison.OrdinalIgnoreCase) Then cboText = fld
                Next
                cboField.Text = cboText
            End If
        End With
    End Sub

    Private Sub frmWCS_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        Project.Save()
        WCSForm = Nothing
        gMapWin.StatusBar(2).Text = ""
        SaveWindowPos(REGAPP, Me)
        RemoveHandler GisUtil.Progress, AddressOf UpdProg
    End Sub

    'following has been disabled by LCW 8/19/10; really want user to be able to press F1 at input field to get context-
    'sensitive help. To get full help manual, must select from help button.

    'Private Sub frmWCS_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
    '    If e.KeyValue = Windows.Forms.Keys.F1 Then
    '        ShowHelp("BASINS Details\Analysis\Watershed Characterization System.html")
    '    End If
    'End Sub

    Private Sub frmWCS_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GetWindowPos(REGAPP, Me)
        Project = New clsProject
        Project.Load()
        LoadForm()
        AddHandler GisUtil.Progress, AddressOf UpdProg
    End Sub

    Private Sub lnkCancel_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lnkCancel.LinkClicked
        GisUtil.Cancel = True
    End Sub

    Private Sub LoadForm()
        With Project
            txtOutputName.Text = .OutputFolder
            For i As Integer = 1 To 8
                Dim cbo As ComboBox = Choose(i, cboSubbasinLayer, cboReachLayer, cbo303dLayer, cboPop1Layer, cboPop2Layer, cboSewerLayer, cboSoilLayer, cboPCSLayer)
                For j As Integer = 0 To GisUtil.NumLayers - 1
                    Dim lt As MapWindow.Interfaces.eLayerType = GisUtil.LayerType(j)
                    Dim ltTarget As MapWindow.Interfaces.eLayerType
                    Select Case i
                        Case 2, 3 : ltTarget = MapWindow.Interfaces.eLayerType.LineShapefile
                        Case 8 : ltTarget = MapWindow.Interfaces.eLayerType.PointShapefile
                        Case Else
                            ltTarget = MapWindow.Interfaces.eLayerType.PolygonShapefile
                    End Select
                    If lt = ltTarget Then cbo.Items.Add(GisUtil.LayerName(j))
                Next
            Next
            SetDefault(cboSubbasinLayer, .SubbasinLayer)
            SetDefault(cboReachLayer, .ReachLayer)
            SetDefault(cbo303dLayer, ._303dLayer)
            SetDefault(cboPop1Layer, .Pop1Layer)
            SetDefault(cboPop2Layer, .Pop2Layer)
            SetDefault(cboSewerLayer, .SewerLayer)
            SetDefault(cboSoilLayer, .SoilLayer)
            With cboLandUseType
                .Items.Clear()
                .Items.AddRange(New String() {"USGS GIRAS Shapefiles", "NLCD 1992 Grid", "NLCD 2001 Grid", "User Shapefile", "User Grid"})
                .SelectedIndex = Project.LanduseType
            End With
            chkLanduseIDShown.Checked = .LanduseIDShown
            SetDefault(cboPCSLayer, .PCSLayer)

            'populate soil grid (landuse grid gets populated on listchanged event
            With dgSoil
                .ReadOnly = True
                .AllowUserToAddRows = False
                .AllowUserToOrderColumns = False
                .SelectionMode = DataGridViewSelectionMode.FullRowSelect
                For i As Integer = 0 To 2
                    .Columns.Add(Choose(i + 1, "ID", "Name", "Group"), Choose(i + 1, "Soil ID", "Soil Name", "Group"))
                    With .Columns(i)
                        .ValueType = GetType(String)
                        .HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                        .DefaultCellStyle.Alignment = IIf(i = 1, DataGridViewContentAlignment.MiddleLeft, DataGridViewContentAlignment.MiddleCenter)
                        .DefaultCellStyle.ForeColor = Drawing.Color.Gray
                        .AutoSizeMode = IIf(i = 1, DataGridViewAutoSizeColumnMode.Fill, DataGridViewAutoSizeColumnMode.None)
                        .SortMode = DataGridViewColumnSortMode.NotSortable
                    End With
                Next
                For Each kv As KeyValuePair(Of String, clsSoil) In Project.dictSoil
                    .Rows.Add(kv.Value.ID, kv.Value.Name, kv.Value.Group)
                Next
            End With

            'populate list of datasources
            For Each ds As atcData.atcDataSource In atcData.atcDataManager.DataSources
                Dim dataSource As String = ds.Name.Split(":")(2)
                lstDataSources.Items.Add(dataSource & ": " & ds.Specification)
            Next

            'field names get set on changed event
            lstReports.SelectedIndex = 0
        End With
    End Sub

    Private Sub lstDataSources_SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstDataSources.SelectedValueChanged
        lstReports.SetItemChecked(6, True)
    End Sub

    Private Sub lstReports_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstReports.SelectedIndexChanged
        tabReports.SelectedIndex = lstReports.SelectedIndex
    End Sub

    Private Sub SaveForm()
        With Project
            .SubbasinLayer = cboSubbasinLayer.Text
            .SubbasinField = cboSubbasinField.Text
            .ReachLayer = cboReachLayer.Text
            .ReachField = cboReachField.Text
            ._303dLayer = cbo303dLayer.Text
            .WaterbodyField = cboWaterBodyField.Text
            .ImpairmentField = cboImpairmentField.Text
            .Pop1Layer = cboPop1Layer.Text
            .Pop2Layer = cboPop2Layer.Text
            .PopNameField = cboPopNameField.Text
            .PopPopField = cboPopPopField.Text
            .SewerLayer = cboSewerLayer.Text
            .SewerNameField = cboSewerNameField.Text
            .SewerPopField = cboSewerPopField.Text
            .SewerHouseField = cboSewerHouseField.Text
            .SewerPublicField = cboSewerPublicField.Text
            .SewerSepticField = cboSewerSepticField.Text
            .SewerOtherField = cboSewerOtherField.Text
            .SoilLayer = cboSoilLayer.Text
            .SoilField = cboSoilField.Text
            .LanduseType = cboLandUseType.SelectedIndex
            .LanduseLayer = cboLanduseLayer.Text
            .LanduseField = cboLanduseField.Text
            .LanduseIDShown = chkLanduseIDShown.Checked
            .PCSLayer = cboPCSLayer.Text
            .PCSNpdesField = cboNPDESField.Text
            .PCSFacNameField = cboFacNameField.Text
            .PCSSicField = cboSICField.Text
            .PCSSicNameField = cboSICNameField.Text
            .PCSCityField = cboCityField.Text
            .PCSMajorField = cboMajorField.Text
            .PCSRecWaterField = cboRecWaterField.Text
            .PCSActiveField = cboActiveField.Text
            .PCSActiveOnly = chkActiveOnly.Checked

            .lstDatasets.Clear()
            For Each s As String In lstDataSources.CheckedItems
                .lstDatasets.Add(s)
            Next

            SaveLanduseGrid()

        End With
    End Sub

    Private Sub SetDefault(ByVal cbo As ComboBox, ByVal WildCard As String)
        For Each s As String In cbo.Items
            If s Like WildCard Then
                cbo.Text = s
                Exit Sub
            End If
        Next
    End Sub

    Private Sub tabReports_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tabReports.SelectedIndexChanged
        lstReports.SelectedIndex = tabReports.SelectedIndex
    End Sub

    Private Sub lnkDeleteReports_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lnkDeleteReports.LinkClicked
        If MessageBox.Show(String.Format("Are you sure you want to delete all {0} previously generated report files?", My.Computer.FileSystem.GetFiles(Project.OutputFolder, FileIO.SearchOption.SearchTopLevelOnly, "Report*.htm").Count), "Delete Previous Report Files", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) = Windows.Forms.DialogResult.OK Then
            For Each rpt As String In My.Computer.FileSystem.GetFiles(Project.OutputFolder, FileIO.SearchOption.SearchTopLevelOnly, "Report*.htm")
                My.Computer.FileSystem.DeleteFile(rpt)
            Next
        End If
        'if delete reports, also clear current HTML report
        wbResults.DocumentText = ""
    End Sub

    Private Sub btnHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHelp.Click
        ShowHelp("BASINS Details\Analysis\Watershed Characterization System.html")
    End Sub

    Private Sub lnkLandUseRefresh_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkLandUseRefresh.Click
        SaveForm()
        Project.GetLanduses(Me.cboLandUseType.SelectedIndex)
        LoadLanduseGrid()
    End Sub

    Private Sub lnkLandUseClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkLandUseClear.Click
        SaveForm()
        Project.dictLanduse(cboLandUseType.SelectedIndex).Clear()
        dgLandUse.Rows.Clear()
    End Sub

    Private Sub lnkLandUseSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkLandUseSave.Click
        If MessageBox.Show("This will overwrite the landuse description file (LandUses.txt) used for all projects. Are you sure you want to continue?", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) = Windows.Forms.DialogResult.Cancel Then Exit Sub
        SaveForm()
        SaveLanduseGrid()
        Project.SaveLanduses()
    End Sub

    Private Sub lnkLandUseReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkLandUseReset.Click
        If MessageBox.Show("This will overwrite the landuse description file (LandUses.txt) used for all projects with default values. Are you sure you want to continue?", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) = Windows.Forms.DialogResult.Cancel Then Exit Sub
        IO.File.WriteAllText(Project.AppFolder & "\Landuses.txt", My.Resources.LandUses)
        Project.LoadLanduses()
        LoadLanduseGrid()
    End Sub

    Private Sub SaveLanduseGrid()
        With Project.dictLanduse(cboLandUseType.SelectedIndex)
            dgLandUse.EndEdit()
            .Clear()
            For r As Integer = 0 To dgLandUse.Rows.Count - 1
                Dim lucode As String = dgLandUse.Item(0, r).Value
                Dim luname As String = dgLandUse.Item(1, r).Value
                If Not String.IsNullOrEmpty(lucode) AndAlso Not .ContainsKey(lucode) Then
                    .Add(lucode, New clsLanduse(lucode, luname))
                End If
            Next
        End With
    End Sub

    Private Sub LoadLanduseGrid()
        'populate landuse grid
        With dgLandUse
            .AllowUserToAddRows = True
            .AllowUserToOrderColumns = False
            .Columns.Clear()
            For i As Integer = 0 To 1
                .Columns.Add(Choose(i + 1, "ID", "Name"), Choose(i + 1, "Landuse ID", "Landuse Name"))
                With .Columns(i)
                    .ValueType = GetType(String)
                    .HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                    .DefaultCellStyle.Alignment = IIf(i = 1, DataGridViewContentAlignment.MiddleLeft, DataGridViewContentAlignment.MiddleCenter)
                    .AutoSizeMode = IIf(i = 1, DataGridViewAutoSizeColumnMode.Fill, DataGridViewAutoSizeColumnMode.None)
                    .SortMode = DataGridViewColumnSortMode.NotSortable
                End With
            Next
            'Select Case Project.LanduseType
            '    Case clsProject.enumLandUseType.UserGrid, clsProject.enumLandUseType.UserShapefile
            '        .ReadOnly = False
            '        .DefaultCellStyle.ForeColor = Drawing.Color.Black
            '        .Columns(0).ReadOnly = True
            '        .Columns(0).DefaultCellStyle.ForeColor = Drawing.Color.Gray
            '        .SelectionMode = DataGridViewSelectionMode.CellSelect
            '    Case Else
            '        .DefaultCellStyle.ForeColor = Drawing.Color.Gray
            '        .ReadOnly = True
            '        .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            'End Select
            For Each kv As KeyValuePair(Of String, clsLanduse) In Project.dictLanduse(cboLandUseType.SelectedIndex)
                .Rows.Add(kv.Value.ID, kv.Value.Name)
            Next
            .Columns(0).AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
        End With
    End Sub

End Class