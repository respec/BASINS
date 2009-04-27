Friend Class frmDataset
    Inherits System.Windows.Forms.Form
    '******************************************************************************
    ' Project: Mercury Model, FrmDataManagment form
    ' Author: Raghu, Tetra Tech Inc.
    ' Date Created: 04 August , 2005
    ' Date Modified:
    ' Purpose:  Provide a user interface for the user to define all input variables
    ' Modification History:
    '
    '
    '******************************************************************************


    Private Sub cmdCancel_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdCancel.Click
        Me.Close()
    End Sub

    Private Sub cmdSave_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSave.Click
        Try
            Dim pOptData As New Generic.List(Of Control)
            pOptData.Add(cboPoint_Sources)
            pOptData.Add(cboPoint_Source_Table)
            pOptData.Add(cboNHD_Lakes)

            If BlankCheck(Me, pOptData) Then
                If FormValidator() Then
                    modFormInteract.WritetoDict(Me)
                    modFormInteract.SaveDict()
                    Me.Close()
                Else
                    Exit Sub
                End If

                'assign color scheme to 
                'Make zoom the extent to dem, and streams visible
                'pActiveView = gMap
                Dim pDEMRLayer As MapWindow.Interfaces.Layer = GetInputLayer("DEM")
                Dim pNHDFLayer As MapWindow.Interfaces.Layer = GetInputLayer("NHD")


                'Change DEM color

                ApplyColoringScheme("DEM", MapWinGIS.PredefinedColorScheme.Desert)

                pDEMRLayer.Visible = True
                pNHDFLayer.Visible = True
                GisUtil.ZoomToLayerExtents("DEM")
                pNHDFLayer = Nothing
                pDEMRLayer = Nothing

                'Render landuse layer
                RenderLanduseSymbology()

                If Not ValidateSpatialReferenceOfInputLayers() Then
                    Me.Close()
                    Exit Sub
                End If

            End If
            pOptData = Nothing
        Catch ex As Exception
            ErrorMsg(, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Respond to browse button clicks
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    ''' <remarks>Comboboxes and buttons have similar names such that they can be used for caption as well; control tags contain text describing type type of file to open</remarks>
    Private Sub btn_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles btnClimate_Station.Click, btnClimate_Data.Click, btnDEM.Click, btnNHD_Drains.Click, btnNHD_Lakes.Click, btnLanduse.Click, btnLanduse_CN_Lookup_Table.Click, btnLanduse_Lookup_Table.Click, btnNHD_Streams.Click, btnNHD_Flow_Relation.Click, btnPoint_Sources.Click, btnPoint_Source_Table.Click, btnSoil_Map.Click, btnSoil_Property.Click
        Dim btn As Button = eventSender
        Dim cbo As ComboBox = Controls(btn.Name.Replace("btn", "cbo"))
        Dim caption As String = btn.Name.Substring(3, btn.Name.Length - 3).Replace("_", " ")
        Dim selectedName As String = SelectLayerOrTableData(enumDataType.Feature, caption)
        Dim datatype As enumDataType
        Select Case btn.Tag
            Case "Feature" : datatype = enumDataType.Feature
            Case "Raster" : datatype = enumDataType.Raster
            Case "Table" : datatype = enumDataType.Table
        End Select
        Dim name As String = SelectLayerOrTableData(datatype, caption)
        If name <> "" Then cbo.Text = name
    End Sub



    Private Function FormValidator() As Boolean
        Try
            If cboSoil_Map.Text = "" Or cboLanduse.Text = "" Or cboDEM.Text = "" Or cboNHD_Streams.Text = "" Or cboNHD_Flow_Relation.Text = "" Or cboLanduse_Lookup_Table.Text = "" Or cboLanduse_CN_Lookup_Table.Text = "" Or cboClimate_Station.Text = "" Or cboSoil_Property.Text = "" Or cboClimate_Data.Text = "" Then
                WarningMsg("One or more required layers or tables is missing!")
                Return False
            End If

            If Not (CheckTableFields(cboLanduse_Lookup_Table.Text, "LUCODE", "LUC", "LUP", "GETCOVER", "NGETCOVER", "IMP_DCON", "IMP_TOT", "TYPE", "N") AndAlso _
                    CheckTableFields(cboLanduse_CN_Lookup_Table.Text, "LU_CNCODE", "CN") AndAlso _
                    CheckTableFields(cboSoil_Property.Text, "AWC", "KFACT", "PERM", "BD", "CLAYPERC", "GROUP", "GROUPVALUE") AndAlso _
                    CheckTableFields(cboClimate_Data.Text, "STA_ID", "IDATE", "TAVG_C", "PRCP_CM")) Then Return False

            If (cboPoint_Sources.Text <> "" And cboPoint_Source_Table.Text = "") Or (cboPoint_Sources.Text = "" And cboPoint_Source_Table.Text <> "") Then
                WarningMsg("Both Point Source Map information and table need to be populated, if either one is filled.")
                Return False
            ElseIf (cboPoint_Sources.Text = "" And cboPoint_Source_Table.Text = "") Then
            Else
                If Not (CheckTableFields(cboPoint_Source_Table.Text, "STA_ID") AndAlso _
                        CheckTableFields(cboPoint_Sources.Text, "STA_ID") AndAlso _
                        CheckTableFields(cboClimate_Station.Text, "STA_ID")) Then Return False
            End If
            Return True
        Catch ex As Exception
            ErrorMsg(, ex)
        End Try
    End Function

    Private Sub frmDataset_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        ReadDataManagementUserInputsFromFile()

        'Reads from input file
        modFormInteract.Filer()
        modFormInteract.LoadForm(Me)

        If ComputeWASPFlag Then
            lblNHD_Drains.Enabled = True
            cboNHD_Drains.Enabled = True
            btnNHD_Drains.Enabled = True
            lblNHD_Lakes.Enabled = False
            cboNHD_Lakes.Enabled = False
            btnNHD_Lakes.Enabled = False
            cboNHD_Lakes.Text = ""
        Else
            lblNHD_Drains.Enabled = False
            cboNHD_Drains.Enabled = False
            btnNHD_Drains.Enabled = False
            cboNHD_Drains.Text = ""
            lblNHD_Lakes.Enabled = True
            cboNHD_Lakes.Enabled = True
            btnNHD_Lakes.Enabled = True
        End If
    End Sub

    ''' <summary>
    ''' Subroutine to load route.rch, route.drain, nhd.rflow, nhd.polygon classes
    ''' </summary>
    ''' <param name="NHDPath"></param>
    ''' <remarks></remarks>
    Private Sub LoadNHDCoverageClasses(ByRef NHDPath As String)

        If (NHDPath = "") Then Exit Sub

        'todo: need to figure out how ArcGIS stores this data

        '        'Streams layer
        '        Dim pFeatureLayer As MapWindow.Interfaces.Layer = OpenFeatureClassFromCoverage(NHDPath, "nhd", "route.rch")
        '        NHD.Items.Add(pFeatureLayer.name)
        '        NHD.SelectedIndex = NHD.Items.Count - 1

        '        'Drains layer
        '        pFeatureLayer = OpenFeatureClassFromCoverage(NHDPath, "nhd", "route.drain")
        '        Drain.Items.Add(pFeatureLayer.name)
        '        Drain.SelectedIndex = Drain.Items.Count - 1

        '        pFeatureLayer = OpenFeatureClassFromCoverage(NHDPath, "nhd", "region.wb")
        '        Lakes.Items.Add(pFeatureLayer.name)
        '        Lakes.SelectedIndex = Lakes.Items.Count - 1

        '        Dim pStandAloneTable As DataTable
        '        pStandAloneTable = OpenInfoTableFromCoverage(NHDPath, "nhd.rflow")
        '        Dim pStandAloneColl As IStandaloneTableCollection
        '        pStandAloneColl = gMap
        '        pStandAloneColl.AddStandaloneTable(pStandAloneTable)
        '        NHDRFlowTable.Items.Add(pStandAloneTable.name)
        '        NHDRFlowTable.SelectedIndex = NHDRFlowTable.Items.Count - 1

        '        GoTo CleanUp
        'ShowError:
        '        MsgBox(Err.Description)
        'CleanUp:
        '        pFeatureLayer = Nothing
        '        pStandAloneTable = Nothing

    End Sub

    'Public Function OpenInfoTableFromCoverage(ByVal spath As String, ByVal TableName As String) As DataTable
    '    Try
    '        Dim csb As New OleDb.OleDbConnectionStringBuilder
    '        csb.DataSource = IO.Path.GetDirectoryName(TableName)
    '        Dim cn As New OleDb.OleDbConnection(csb.ConnectionString)
    '        Dim name As String = IO.Path.GetFileNameWithoutExtension(TableName)
    '        Dim da As New OleDb.OleDbDataAdapter("SELECT * FROM " & name, cn)
    '        Dim dt As New DataTable(name)
    '        da.Fill(dt)
    '        da.Dispose()
    '        cn.Close()
    '        cn.Dispose()
    '        Return dt
    '    Catch ex As Exception
    '        ErrorMsg("Unable to open table: " & TableName, ex)
    '        Return Nothing
    '    End Try
    'End Function

    ''' <summary>
    ''' NOT SURE HOW THIS WORKS WITH ARCGIS!!!!
    ''' </summary>
    ''' <param name="spath"></param>
    ''' <param name="sCovName"></param>
    ''' <param name="ClassName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function OpenFeatureClassFromCoverage(ByRef spath As String, ByRef sCovName As String, ByRef ClassName As String) As MapWindow.Interfaces.Layer
        ' Open a feature class in the specified coverage by giving a class ID
        If GisUtil.AddLayer(spath, sCovName) Then
            Return gMapWin.Layers(GisUtil.LayerIndex(sCovName))
        Else
            Return Nothing
        End If
    End Function

    '** Subroutine to read values from input file and load DataManagement form
    Private Sub ReadDataManagementUserInputsFromFile()
        If Not EnableHgMenu() Then Exit Sub

        DefineApplicationPath()

        'Create a new file for hydrograph
        If Not (My.Computer.FileSystem.FileExists(gMapInputFolder & "\InputData.txt")) Then InitializeDataManagementForm()

        SetDefaultLayerOrTable(cboClimate_Station)
        SetDefaultLayerOrTable(cboClimate_Data)
        SetDefaultLayerOrTable(cboDEM)
        SetDefaultLayerOrTable(cboNHD_Lakes)
        SetDefaultLayerOrTable(cboNHD_Drains)
        SetDefaultLayerOrTable(cboLanduse)
        SetDefaultLayerOrTable(cboLanduse_CN_Lookup_Table)
        SetDefaultLayerOrTable(cboLanduse_Lookup_Table)
        SetDefaultLayerOrTable(cboNHD_Flow_Relation)
        SetDefaultLayerOrTable(cboNHD_Streams)
        SetDefaultLayerOrTable(cboPoint_Sources)
        SetDefaultLayerOrTable(cboPoint_Source_Table)
        SetDefaultLayerOrTable(cboSoil_Map)
        SetDefaultLayerOrTable(cboSoil_Property)

        InitializeDataManagementForm()
    End Sub

    Private Sub InitializeDataManagementForm()

        Dim FeatureLayerNames As Generic.List(Of String) = Nothing
        Dim RasterLayerNames As Generic.List(Of String) = Nothing
        Dim TableNames As Generic.List(Of String) = Nothing
        GetNameLists(FeatureLayerNames, RasterLayerNames, TableNames)

        LoadDefaultLayerNames(cboClimate_Station, "ClimateStation", FeatureLayerNames.ToArray)
        LoadDefaultLayerNames(cboClimate_Data, "climate_data", TableNames.ToArray)
        LoadDefaultLayerNames(cboDEM, "DEM", RasterLayerNames.ToArray)
        LoadDefaultLayerNames(cboNHD_Streams, "NHD", FeatureLayerNames.ToArray)
        LoadDefaultLayerNames(cboPoint_Sources, "PointSources", FeatureLayerNames.ToArray)
        LoadDefaultLayerNames(cboNHD_Lakes, "Lakes", FeatureLayerNames.ToArray)
        LoadDefaultLayerNames(cboNHD_Drains, "Drain", FeatureLayerNames.ToArray)
        LoadDefaultLayerNames(cboLanduse, "Landuse", RasterLayerNames.ToArray)
        LoadDefaultLayerNames(cboLanduse_CN_Lookup_Table, "LUCNcode", TableNames.ToArray)
        LoadDefaultLayerNames(cboLanduse_Lookup_Table, "lulookup", TableNames.ToArray)
        LoadDefaultLayerNames(cboNHD_Flow_Relation, "RFlow", TableNames.ToArray)
        LoadDefaultLayerNames(cboPoint_Source_Table, "pointsource_data", TableNames.ToArray)
        LoadDefaultLayerNames(cboSoil_Map, "SoilMap", RasterLayerNames.ToArray)
        LoadDefaultLayerNames(cboSoil_Property, "SoilProperty", TableNames.ToArray)

    End Sub

    Private Sub RenderLanduseSymbology()

        'assign random colors to each value in grid
        Dim lyrIdx As Integer = GisUtil.LayerIndex(GetInputLayerName("Landuse"))
        GisUtil.UniqueValuesRenderer(lyrIdx)

        'override for specific land uses
        Dim LuDescDict As Generic.Dictionary(Of String, String) = GetLuDictionary()
        If LuDescDict Is Nothing Then Exit Sub

        Dim scheme As MapWinGIS.GridColorScheme = GisUtil.GetColoringScheme(lyrIdx)

        For b As Integer = 0 To scheme.NumBreaks - 1
            Dim brk As MapWinGIS.GridColorBreak = scheme.Break(b)
            Dim v As Single = brk.LowValue
            Dim LUDesc As String = ""
            If LuDescDict.TryGetValue(v, LUDesc) Then
                If LUDesc.ToUpper.Contains("CROP") Then
                    brk.LowColor = System.Convert.ToUInt32(RGB(0, 255, 0))
                ElseIf LUDesc.ToUpper.Contains("WATER") Then
                    brk.LowColor = System.Convert.ToUInt32(RGB(0, 0, 255))
                ElseIf LUDesc.ToUpper.Contains("PASTURE") Then
                    brk.LowColor = System.Convert.ToUInt32(RGB(0, 255, 155))
                End If
                brk.HighColor = brk.LowColor
            End If
        Next
    End Sub

    ''' <summary>
    ''' Creates a dictionary for mapping land use description and lu code
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetLuDictionary() As Generic.Dictionary(Of String, String)
        Try
            Dim dtLookup As DataTable = GetDataTable("LuLookupTable")
            If dtLookup Is Nothing Then Return Nothing

            Dim dict As New Generic.Dictionary(Of String, String)

            For Each row As DataRow In dtLookup.Rows
                Dim luCode As String = row.Item("LUCODE")
                Dim luName As String = row.Item("LUNAME")
                If dict.ContainsKey(luCode) Then
                    ErrorMsg("LUCode ID is not unique in land use lookup table")
                    Return Nothing
                Else
                    dict.Add(luCode, luName)
                End If
            Next
            Return dict
        Catch ex As Exception
            ErrorMsg(, ex)
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Because items may be long, set tooltip whenever selected item changes
    ''' </summary>
    Private Sub cbo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboClimate_Data.SelectedIndexChanged, cboClimate_Station.SelectedIndexChanged, cboDEM.SelectedIndexChanged, cboLanduse.SelectedIndexChanged, cboLanduse_CN_Lookup_Table.SelectedIndexChanged, cboLanduse_Lookup_Table.SelectedIndexChanged, cboNHD_Drains.SelectedIndexChanged, cboNHD_Flow_Relation.SelectedIndexChanged, cboNHD_Lakes.SelectedIndexChanged, cboNHD_Streams.SelectedIndexChanged, cboPoint_Source_Table.SelectedIndexChanged, cboPoint_Sources.SelectedIndexChanged, cboSoil_Map.SelectedIndexChanged, cboSoil_Property.SelectedIndexChanged
        ToolTip1.SetToolTip(sender, CType(sender, ComboBox).Text)
    End Sub
End Class