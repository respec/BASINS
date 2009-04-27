Module modUtility

    Public startDAY As Short
    Public startMONTH As Short
    Public startYEAR As Short
    Public endDAY As Short
    Public endMONTH As Short
    Public endYEAR As Short
    Public startDATE As Date
    Public endDATE As Date
    Public startgrowingSEASON As Short
    Public endgrowingSEASON As Short
    Public LakesThreshold As Double
    'Public growingSEASONDictionary As Scripting.Dictionary
    Public InputDataDictionary As Generic.Dictionary(Of String, String)
    'Public ClimateStationDictionary As Scripting.Dictionary
    'Public ClimStatLatitudeDictionary As Scripting.Dictionary
    Public ComputeSedimentFlag As Boolean
    Public ComputeMercuryFlag As Boolean
    Public ComputeWhAEMFlag As Boolean
    Public ComputeWASPFlag As Boolean
    Public ComputeBalanceFlag As Boolean
    Public AverageWhAEMDuration As Integer
    Public AssessmentPointSimulationFlag As Boolean
    Public pMasterRunOffArray() As Double
    Public pMasterSedimentArray() As Double
    Public ElevationTextSampleSize As Double
    Public AssessPointSnappingDist As Double

    'Public g_pEditor As IEditor

    'Private pStepProgressor As IStepProgressor

    Public Declare Function ShowWindow Lib "user32" (ByVal hwnd As Integer, ByVal nCmdShow As Integer) As Integer
    Public Const SW_SHOWNORMAL As Integer = 1

    Public gGBMMSubWaterFileHEADER As String 'File Header giving brief description
    Public gLURunoffFileHEADER As String 'File Header for lu distribution
    Public gLUSedimentFileHEADER As String 'File Header for lu distribution
    Public gLUMercuryFileHEADER As String 'File Header for lu distribution
    Public gLakeOutputFileHEADER As String 'File Header for lake output information
    Public gLakeSedimentFileHEADER As String 'File Header for sediment output information
    Public gLakeMercuryFileHEADER As String 'File Header for mercury output information
    Public gAreaFileHEADER As String 'File Header for area information
    Public gWaterBalanceFileHEADER As String 'File Header for water balance
    Public gSedimentBalanceFileHEADER As String 'File Header for sediment mass balance
    Public gMercuryBalanceFileHEADER As String 'File Header for mercury load balance
    Public gEndBlockHEADER As String 'End of File Header Block
    Public gLandUseAreaHEADER As String 'File Header for different landuse types

    ' Change pixeltype for those not supported by VB
    Public Function GetVBSupportedPixelType(ByRef iPixeltype As Short) As Object
        Try
            If iPixeltype <= 4 Then
                GetVBSupportedPixelType = 3 ' PT_UCHAR
            ElseIf iPixeltype <= 6 Then
                GetVBSupportedPixelType = 6 ' PT_SHORT
            ElseIf iPixeltype <= 8 Then
                GetVBSupportedPixelType = 8 ' PT_LONG
            ElseIf iPixeltype >= 9 Then
                GetVBSupportedPixelType = 9 ' PT_FLOAT
            End If
            Exit Function
        Catch ex As Exception
            ErrorMsg(, ex)
        End Try
    End Function

    Public Sub GetSetupModelParameters()
        LakesThreshold = InputDataDictionary.Item("LakesThreshold")
    End Sub

    'Subroutine to get the ls factor dialog parameters
    'Public Sub GetLSFactorParameters()
    '    If (FrmLSFactor.optionDefineSlopeLength.Checked = True) Then
    '        boolDefineSlopeLengthValue = True
    '        pCSlopeLengthValue = CDbl(FrmLSFactor.txtCSL.Text)
    '    Else
    '        boolDefineSlopeLengthValue = False
    '    End If
    'End Sub

    ''Subroutine to delineate subwatersheds
    'Public Sub DelineateWatershed()
    '    Dim startTime As Date
    '    startTime = Now

    '    '*** Define the map and layers and tables in map
    '    InitializeInputDataDictionary()
    '    modInit.Initialize()

    '    If Not (ValidateSpatialReferenceOfInputLayers) Then
    '        Exit Sub
    '    End If

    '    '* Modules for Hydrology
    '    ModuleFlowDirectionAccumulation.RunNHDAgreeDEMForFlowDirAndAccu()

    '    'Get the end time
    '    ''''''' Call CleanUpMemory  '''Raghu
    '    Dim endTime As Date
    '    endTime = Now
    '    MsgBox("Computation Time: " & DateDiff(Microsoft.VisualBasic.DateInterval.Second, startTime, endTime) & " seconds.")
    'End Sub


    '    Public Sub SetupMercuryModel()
    '        Dim pgStatusBar As Object
    '        On Error GoTo ShowError
    '        Dim pgStatusBar As esriSystem.IStatusBar
    '        pgStatusBar = gApplication.StatusBar

    '        '*** Define the map and layers and tables in map
    '        InitializeInputDataDictionary()
    '        If Not (ValidateInputDatasets) Then
    '            Exit Sub
    '        End If
    '        modInit.Initialize()

    '        modFormInteract.Filer()

    '        If Not (ValidateSpatialReferenceOfInputLayers) Then
    '            Exit Sub
    '        End If

    '        GetSetupModelParameters()
    '        'Open the LSfactor computation dialog box

    '        If ComputeSedimentFlag Then
    '            FrmLSFactor.ShowDialog()
    '        Else
    '            skipLSfactor = True
    '            boolContinueLSFactor = True
    '        End If


    '        If (boolContinueLSFactor = False) Then
    '            Exit Sub
    '        End If

    '        If Not skipLSfactor Then ''Raghu
    '            Call CheckLSFactorRasterDataset()
    '        End If


    '        If Not boolContinueLSFactor Then
    '            Exit Sub
    '        End If

    '        'Open the status option
    '        Dim lres As Object
    '        lres = ShowWindow(FrmStatus.Handle.ToInt32, SW_SHOWNORMAL)
    '        FrmStatus.Show() ''Raghu
    '        FrmStatus.ProgressBar.Minimum = 1
    '        FrmStatus.ProgressBar.Maximum = 17


    '        'Delete files before setup

    '        Dim fso As New Scripting.FileSystemObject

    '        If fso.FolderExists(gMapTempFolder & "\Lakes") Then
    '            DeleteRasterDataset(("Lakes"))
    '        End If

    '        If fso.FolderExists(gMapTempFolder & "\LakesGrid") Then
    '            DeleteRasterDataset(("LakesGrid"))
    '        End If

    '        If fso.FolderExists(gMapTempFolder & "\pointsources") Then
    '            DeleteRasterDataset(("pointsources"))
    '        End If

    '        'Added new files to delete - Alvi = 01/12/2006
    '        If skipLSfactor = False Then

    '            If fso.FolderExists(gMapTempFolder & "\hydradius") Then
    '                DeleteRasterDataset(("hydradius"))
    '            End If

    '            If fso.FolderExists(gMapTempFolder & "\roughness") Then
    '                DeleteRasterDataset(("roughness"))
    '            End If

    '            If fso.FolderExists(gMapTempFolder & "\overlandtt") Then
    '                DeleteRasterDataset(("overlandtt"))
    '            End If

    '            If fso.FolderExists(gMapTempFolder & "\streamtime") Then
    '                DeleteRasterDataset(("streamtime"))
    '            End If

    '            If fso.FolderExists(gMapTempFolder & "\totaltime") Then
    '                DeleteRasterDataset(("totaltime"))
    '            End If

    '            If fso.FolderExists(gMapTempFolder & "\kdcomp") Then
    '                DeleteRasterDataset(("kdcomp"))
    '            End If

    '            If fso.FolderExists(gMapTempFolder & "\cn2") Then
    '                DeleteRasterDataset(("cn2"))
    '            End If

    '            If fso.FolderExists(gMapTempFolder & "\cnimp") Then
    '                DeleteRasterDataset(("cnimp"))
    '            End If

    '            If fso.FolderExists(gMapTempFolder & "\avslope") Then
    '                DeleteRasterDataset(("avslope"))
    '            End If
    '            If fso.FolderExists(gMapTempFolder & "\ovlength") Then
    '                DeleteRasterDataset(("ovlength"))
    '            End If
    '            If fso.FolderExists(gMapTempFolder & "\avroughness") Then
    '                DeleteRasterDataset(("avroughness"))
    '            End If
    '        End If

    '        '* Modules for Hydrology
    '        pgStatusBar.Message(0) = "Computing FlowDirection and FlowAccumulation"
    '        UpdateProgressBarDuringSetUp(1, "Computing FlowDirection and FlowAccumulation")
    '        ModuleFlowDirectionAccumulation.RunNHDAgreeDEMForFlowDirAndAccu()
    '        pgStatusBar.Message(0) = "Computing Curve Number For Pervious Land"
    '        UpdateProgressBarDuringSetUp(2, "Computing Curve Number For Pervious Land")
    '        ModuleCurveNumber.CreateCurveNumberForPervious()
    '        pgStatusBar.Message(0) = "Computing Curve Number For Impervious Land"
    '        UpdateProgressBarDuringSetUp(3, "Computing Curve Number For Impervious Land")
    '        ModuleCurveNumber.CreateCurveNumberForImpervious()

    '        UpdateProgressBarDuringSetUp(4, "Computing Vegetation Cover")

    '        'Alvi - Nov 7, 2005
    '        'ModuleVegetationCover.CreateLUCoverRelatedRasters

    '        UpdateProgressBarDuringSetUp(5, "Computing Soil Properties")

    '        'Alvi - Nov 7, 2005
    '        'ModuleSoilProperties.CreateSoilPropertiesRasters

    '        ModuleInit.InitializeAlgebraOperator() 'Cleanup

    '        ''    If ComputeSedimentFlag = True Then
    '        ''        UpdateProgressBarDuringSetUp 6, "Computing LSFactor"
    '        ''        ModuleLSFactor.StartLSFactorComputations
    '        ''    End If
    '        pgStatusBar.Message(0) = "Computing LSFactor"
    '        UpdateProgressBarDuringSetUp(6, "Computing LSFactor")
    '        ModuleLSFactor.StartLSFactorComputations()

    '        pgStatusBar.Message(0) = "Computing Roughness Coefficient"
    '        UpdateProgressBarDuringSetUp(7, "Computing ")
    '        ModuleRoughnessCoeff.CreateManningsCoefficientRaster()

    '        pgStatusBar.Message(0) = "Computing Hydraulic Radius"
    '        UpdateProgressBarDuringSetUp(8, "Computing Hydraulic Radius")
    '        ModuleHydraulicRadius.CalculateHydraulicRadius()

    '        pgStatusBar.Message(0) = "Processing Lakes"
    '        UpdateProgressBarDuringSetUp(9, "")
    '        ModuleLakeAndStreamMasks.DefineLakeAttributes()

    '        UpdateProgressBarDuringSetUp(10, "Processing Lakes")
    '        ModuleLakeAndStreamMasks.CreateLakesRaster()

    '        pgStatusBar.Message(0) = "Computing Flow Length and Travel Time"
    '        UpdateProgressBarDuringSetUp(11, "Computing Flow Length and Travel Time ")
    '        ModuleLakeAndStreamMasks.ComputeMasksFlowLengthAndTravelTime()

    '        pgStatusBar.Message(0) = "Defining Lake Outlets"
    '        UpdateProgressBarDuringSetUp(12, "Defining Lake Outlets")
    '        ModuleLakeOutlets.DefineLakeOutlets()
    '        ModuleInit.InitializeAlgebraOperator() 'Cleanup

    '        pgStatusBar.Message(0) = "Computing Thiessen's Polygons"
    '        UpdateProgressBarDuringSetUp(13, "Computing Thiessen's Polygons")

    '        'Raghu
    '        ModuleNewUtilities.FindClimateMinMax(InputDataDictionary.Item("ClimateDataTextFile"))

    '        ''' ModuleUtility.ExportClimateTextFileToDBFtable 'Silenced by raghu - DBF conversion not needed - Alvi

    '        ModuleThiessenPolygon.CreateThiessenPolygons()



    '        If (ComputeMercuryFlag = True) And (NeedHgThiessen = True) Then
    '            ModuleNewUtilities.GetHgStation()
    '            ModuleNewThiessenPolygon.CreateNewThiessenPolygons(MercuryStation, "thiessenHg")
    '        End If


    '        'Alvi - Nov 7, 2005
    '        'If (ComputeMercuryFlag = True) And (NeedHgThiessen = False) Then
    '        '    UpdateProgressBarDuringSetUp 14, "Computing Daily Mercury Deposition Rate"
    '        '    ModuleMercury.ComputeDailyMercuryDepositionRates
    '        'Else
    '        ''''        UpdateProgressBarDuringSetUp 14, "Skipping Daily Mercury Deposition Rate Computations"
    '        ' End If

    '        'Raghu
    '        pgStatusBar.Message(0) = "Creating Point Sources Raster"
    '        UpdateProgressBarDuringSetUp(14, "Creating Point Sources Raster")
    '        ModulePointSource.CreatePointSourcesRaster()



    '        'Alvi - Nov 7, 2005
    '        ' If ComputeSedimentFlag = True Then
    '        '    UpdateProgressBarDuringSetUp 15, "Computing Sediment Constants"
    '        '    ModuleSedimentConstants.CreateMUSLEConstantRasters  'Create Grids for Sediment
    '        ''' ModuleClipper.CreateKCPLSRaster ' Uncommented at Alvi's new request
    '        'Else
    '        UpdateProgressBarDuringSetUp(15, "Skipping Sediment Constants Computations")
    '        ' End If



    '        'Alvi - Nov 7, 2005
    '        'If (ComputeMercuryFlag = True) And (NeedHgThiessen = False) Then
    '        '    UpdateProgressBarDuringSetUp 16, "Computing Hg Deposition Loads"
    '        '    ModuleMercury.SoilStep1LandAtmosphericDepLoad  'Create Grids for Mercury
    '        'Else
    '        '    UpdateProgressBarDuringSetUp 16, "Skipping Hg Deposition Load Computing"
    '        'End If

    '        If ComputeMercuryFlag = True Then
    '            pgStatusBar.Message(0) = "Computing Litter Decomposition Rate"
    '            UpdateProgressBarDuringSetUp(17, "Computing Litter Decomposition Rate")
    '            SoilStep2LitterDecompositionRate()

    '            'Writing Hg Summary - Raghu
    '            If InputDataDictionary("chkTime") = 1 Then
    '                pgStatusBar.Message(0) = "Writing Mercury Summary files"
    '                UpdateProgressBarDuringSetUp(17, "Writing Mercury Summary files")
    '                ModuleNewUtilities.HgSummary(InputDataDictionary("HgDryDepTimeSeries"), "dryHgsummary.txt")
    '                ModuleNewUtilities.HgSummary(InputDataDictionary("HgWetDepTimeSeries"), "wetHgsummary.txt")
    '            End If
    '        Else
    '            UpdateProgressBarDuringSetUp(17, "Skipping Litter Decomposition Rate Computing")
    '        End If

    '        FrmStatus.Close()
    '        'Get the end time
    '        CleanUpMemory()
    '        pgStatusBar.Message(0) = "Processed Input Grids."
    '        MsgBox("Processed Input Grids.")

    '        GoTo CleanUp

    'ShowError:
    '        '    Unload FrmStatus
    '        MsgBox("Setup Unsuccessful, " & Err.Description & Err.Number, MsgBoxStyle.Exclamation)
    'CleanUp:
    '        pgStatusBar = Nothing
    '        ''    Set pRasterSlope = Nothing
    '        ''    Set pRasterFlowDirOut = Nothing
    '        ''    Set pRasterDem = Nothing
    '        ''    Set pRasterDemLayer = Nothing
    '    End Sub

    '    Public Sub UpdateProgressBarDuringSetUp(ByRef pNext As Short, ByRef strModuleMsg As String)
    '        FrmStatus.ProgressBar.Value = pNext
    '        FrmStatus.TxtStatus.Text = strModuleMsg & " ..."
    '    End Sub

    '    Public Sub CheckLSFactorRasterDataset()
    '        On Error GoTo ErrorHandler

    '        Dim pLSFactorDS As IRasterDataset
    '        Dim pSlopeLengthDS As IRasterDataset

    '        Dim pLSFactor As IRaster
    '        Dim pCSL As IRaster

    '        pLSFactor = OpenRasterDatasetFromDisk("LSFactor")
    '        pCSL = OpenRasterDatasetFromDisk("CSL")

    '        Dim pRasterWSF As IWorkspaceFactory
    '        pRasterWSF = New RasterWorkspaceFactory
    '        Dim pRasterWS As IRasterWorkspace
    '        pRasterWS = pRasterWSF.OpenFromFile(gMapTempFolder, 0)

    '        Dim pCSLDS As IRasterDataset


    '        'IF LSFactor and slopelength are not there, then exit
    '        If (pLSFactor Is Nothing And pCSL Is Nothing) Then
    '            boolContinueLSFactor = True
    '            pLSFactorDS = Nothing
    '            pCSLDS = Nothing
    '            Exit Sub
    '        Else
    '            If (Not pLSFactor Is Nothing) Then
    '                pLSFactorDS = pRasterWS.OpenRasterDataset("LSFactor")
    '                pLSFactor = Nothing
    '            End If
    '            If (Not pCSL Is Nothing) Then
    '                pCSLDS = pRasterWS.OpenRasterDataset("CSL")
    '                pCSL = Nothing
    '            End If
    '        End If

    '        'if lsfactor is present and can be deleted then delete it
    '        Dim deleteLSFactor As Boolean

    '        Dim pRasterDataSet As IRasterDataset
    '        Dim pDataset As IDataset



    '        If Not (pLSFactorDS Is Nothing) Then
    '            'Set pRasterDataSet = pRasterWS.OpenRasterDataset("LSFactor")
    '            pDataset = pLSFactorDS
    '            If (pDataset.CanDelete) Then
    '                pDataset.Delete()
    '                deleteLSFactor = True
    '            Else
    '                deleteLSFactor = False
    '            End If
    '        End If
    '        pDataset = Nothing
    '        Dim deleteSlopeLength As Boolean
    '        If Not (pCSLDS Is Nothing) Then
    '            'Set pRasterDataSet = pRasterWS.OpenRasterDataset("CSL")
    '            pDataset = pCSLDS
    '            'Set pDataset = pSlopeLengthDS
    '            If (pDataset.CanDelete) Then
    '                pDataset.Delete()
    '                deleteSlopeLength = True
    '            Else
    '                deleteSlopeLength = False
    '            End If
    '        End If
    '        pDataset = Nothing

    '        If (deleteSlopeLength = True And deleteLSFactor = True) Then
    '            boolContinueLSFactor = True
    '        Else
    '            MsgBox("Delete SlopeLength (csl) and LSFactor Rasters from " & gMapTempFolder & " to continue.")
    '            boolContinueLSFactor = False
    '        End If
    '        GoTo CleanUp

    'ErrorHandler:
    '        MsgBox("Delete SlopeLength and LSFactor Rasters from " & gMapTempFolder & " to continue.")
    '        boolContinueLSFactor = False
    'CleanUp:
    '        ' Release memory
    '        pLSFactorDS = Nothing
    '        pSlopeLengthDS = Nothing
    '        pLSFactorDS = Nothing
    '        pCSLDS = Nothing
    '    End Sub

    '    '*****Call the subroutine to start watershed delineation -- Raghu
    '    Public Sub WatershedDelineation()
    '        'MsgBox "Begin watersheddelineation"

    '        InitializeInputDataDictionary()
    '        'Check if mask is present
    '        ModuleInit.Initialize()
    '        If Not ValidateInputDatasets Then
    '            Exit Sub
    '        End If

    '        Dim pAssessFLayer As MapWindow.Interfaces.Layer
    '        pAssessFLayer = GetInputLayer("AssessPoints")
    '        If pAssessFLayer Is Nothing Then
    '            MsgBox("Assessment Points layer is required to run the simulation." & "Run Setup with lakes layer to create lake outlets or add assessment points " & "from toolbar option.", MsgBoxStyle.Exclamation, "AssessPoints missing")
    '            Exit Sub
    '        End If

    '        '* Next subroutines creates subwatersheds and hydrograph & water balance for each subwatershed
    '        DeleteLayerFromMap("SubWatershed")
    '        ModuleSubwatershed.GenerateSubWatersheds(True, ComputeWASPFlag)
    '        CreateLocalTravelTimeForSubwatersheds()

    '        gApplication.RefreshWindow()
    '        'MsgBox "Done delineate subwatershed"

    'ExitSimulation:
    '        'Exit the simulation
    '    End Sub

    ''' <summary>
    ''' Initialize application and load InputDataDictionary with program settings
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub InitializeInputDataDictionary()

        InputDataDictionary = New Generic.Dictionary(Of String, String)

        If Not EnableHgMenu() Then Exit Sub

        DefineApplicationPath()

        'Create a new file for hydrograph
        If Not My.Computer.FileSystem.FileExists(gMapInputFolder & "\InputData.txt") Then Exit Sub

        'Create a dictionary for input data layer values
        Dim sr As New IO.StreamReader(gMapInputFolder & "\InputData.txt") ' Open file for input.
        While Not sr.EndOfStream
            Dim ar() As String = sr.ReadLine.Split(",")
            InputDataDictionary.Add(ar(0), ar(1))
        End While
        sr.Close()
        sr.Dispose()
    End Sub

    '    Public Function ValidateInputDatasets() As Boolean

    '        DefineMapFrame()
    '        Dim LayerAndTableNames(10) As String
    '        LayerAndTableNames(1) = "ClimateStation"
    '        LayerAndTableNames(2) = "DEM"
    '        LayerAndTableNames(3) = "NHD"
    '        LayerAndTableNames(4) = "Landuse"
    '        '    LayerAndTableNames(5) = "HgDry"
    '        '    LayerAndTableNames(6) = "HgWet"
    '        LayerAndTableNames(5) = "SoilMap"
    '        LayerAndTableNames(6) = "ClimateDataTextFile"
    '        LayerAndTableNames(7) = "LUcodeCNTable"
    '        LayerAndTableNames(8) = "LuLookupTable"
    '        Dim pLayerName As String
    '        Dim j As Short
    '        ValidateInputDatasets = True
    '        For j = 1 To 8
    '            pLayerName = InputDataDictionary.Item(LayerAndTableNames(j))
    '            If (FindInputDatasetInMap(pLayerName) = False) Then
    '                MsgBox(LayerAndTableNames(j) & " not found in the map. " & "Add " & LayerAndTableNames(j) & " and redefine it in the Data Management module.")
    '                ValidateInputDatasets = False
    '            End If
    '        Next
    '    End Function

    '    Public Function FindInputDatasetInMap(ByRef pDSName As String) As Boolean

    '        FindInputDatasetInMap = False
    '        'Check for layer
    '        Dim i As Short
    '        For i = 0 To gMap.LayerCount - 1
    '            If (gMap.Layer(i).name = pDSName) Then
    '                FindInputDatasetInMap = True
    '                GoTo CleanUp
    '            End If
    '        Next
    '        'Check for table
    '        Dim pTabCollection As IStandaloneTableCollection
    '        pTabCollection = gMap
    '        For i = 0 To (pTabCollection.StandaloneTableCount - 1)
    '            If (pTabCollection.StandaloneTable(i).name = pDSName) Then
    '                FindInputDatasetInMap = True
    '                GoTo CleanUp
    '            End If
    '        Next

    'CleanUp:
    '        pTabCollection = Nothing
    '    End Function

    '    Public Sub AddRasterToMap(ByRef pRaster As IRaster, ByRef pName As String)

    '        Dim pRLayer As IRasterLayer
    '        pRLayer = New RasterLayer
    '        pRLayer.CreateFromRaster(pRaster)
    '        AddLayerToMap(pRLayer, pName)
    '        pRLayer = Nothing
    '    End Sub

    '    '*** Subroutine to add a layer to the map
    '    Public Sub AddLayerToMap(ByRef pLayer As ILayer, ByRef pLayerName As String)
    '        On Error GoTo ShowError

    '        ' Set the name, make it invisible
    '        pLayer.name = pLayerName
    '        pLayer.Visible = False

    '        ' Expand the legend group for this layer.
    '        Dim pLegendInfo As ILegendInfo
    '        pLegendInfo = pLayer

    '        Dim pLegendGroup As ILegendGroup
    '        pLegendGroup = pLegendInfo.LegendGroup(0)
    '        pLegendGroup.Visible = False ' set to False to hide.

    '        ' Add the legend to map
    '        gMap.AddLayer(pLayer)
    '        GoTo CleanUp

    'ShowError:
    '        MsgBox("AddLayerToMap: " & Err.Description)

    'CleanUp:
    '        pLegendGroup = Nothing
    '        pLegendInfo = Nothing
    '    End Sub

    Public Sub DeleteLayerFromMap(ByRef pLayerName As String)
        If GisUtil.IsLayer(pLayerName) Then
            gMapWin.Layers.Remove(GisUtil.LayerIndex(pLayerName))
        End If
    End Sub


    ''' <summary>
    ''' Get input raster or vector layer
    ''' </summary>
    ''' <param name="keylayername">Either key or actual layer name</param>
    Public Function GetInputLayer(ByRef keylayername As String) As MapWindow.Interfaces.Layer
        'Get the raster layer name
        Dim fLayerName As String = GetInputLayerName(keylayername)
        If GisUtil.IsLayer(fLayerName) Then
            Return gMapWin.Layers(GisUtil.LayerIndex(fLayerName))
        Else
            Return Nothing
        End If
    End Function

    ''' <summary>
    ''' Get input raster or vector layer name
    ''' </summary>
    ''' <param name="keylayername">Either key or actual layer name</param>
    Public Function GetInputLayerName(ByRef keylayername As String) As String
        'Get the raster layer name
        If InputDataDictionary.ContainsKey(keylayername) Then
            Return InputDataDictionary(keylayername)
        Else
            Return keylayername
        End If
    End Function


    '    '*** Compare the existing raster with dem layer
    '    Public Function CompareInputRasterWithDEMExtent(ByRef fLayerName As String) As IRasterLayer

    '        CompareInputRasterWithDEMExtent = Nothing
    '        Dim i As Short
    '        Dim pRLayer As ILayer
    '        Dim boolRasterLayerFound As Boolean
    '        boolRasterLayerFound = False
    '        For i = 0 To (gMap.LayerCount - 1)
    '            pRLayer = gMap.Layer(i)
    '            If ((pRLayer.name = fLayerName) And (TypeOf pRLayer Is IRasterLayer)) Then
    '                boolRasterLayerFound = True
    '                CompareInputRasterWithDEMExtent = pRLayer 'Get the plot layer
    '                Exit Function
    '            End If
    '        Next
    '    End Function

    'following routines deal with tables attached to the map; because MW doesn't support such a feature, DBF tables will be loaded into DataTables


    ''' <summary>
    ''' Get the attributes table or stand-alone table
    ''' </summary>
    ''' <param name="TableName">Name of layer or table filename</param>
    ''' <returns>Entire contents of table or specification for attribute table assigned to layer</returns>
    ''' <remarks></remarks>
    Public Function GetDataTable(ByVal TableName As String) As DataTable
        Try
            Dim pTableName As String
            If InputDataDictionary.ContainsKey(TableName) Then
                pTableName = InputDataDictionary.Item(TableName)
            Else
                pTableName = TableName
            End If
            If gDataset.Tables.Contains(IO.Path.GetFileNameWithoutExtension(TableName)) Then
                Return gDataset.Tables(IO.Path.GetFileNameWithoutExtension(TableName))
            Else
                Dim dt As DataTable = LoadTable(pTableName)
                If dt IsNot Nothing Then gDataset.Tables.Add(dt)
                Return dt
            End If
        Catch ex As Exception
            'ErrorMsg(, ex)
            Return Nothing
        End Try
    End Function


    ''' <summary>
    ''' Get the data table from the map.
    ''' </summary>
    ''' <param name="TableName"></param>
    ''' <returns></returns>
    ''' <remarks>Because standalone tables aren't attached to the project, look in the </remarks>
    Public Function GetDataTableFileName(ByRef TableName As String) As String
        'Get the raster layer name
        Dim pTableName As String = ""

        If InputDataDictionary.ContainsKey(TableName) Then
            pTableName = InputDataDictionary(TableName)
        Else
            pTableName = TableName
        End If

        If (pTableName = "") Then ErrorMsg("No name for table")


        Dim pTabCollection As IStandaloneTableCollection
        Dim pStTable As IStandaloneTable


        Dim pMxDoc As IMxDocument
        If (gMap Is Nothing) Then
            pMxDoc = gApplication.Document
            gMap = pMxDoc.FocusMap
        End If
        pTabCollection = gMap


        Dim i As Short
        Dim pDataset As IDataset
        Dim pWrkSp As IWorkspace

        GetDataTableFileName = ""
        If pTabCollection.StandaloneTableCount > 0 Then
            For i = 0 To (pTabCollection.StandaloneTableCount - 1)
                pStTable = pTabCollection.StandaloneTable(i)
                If UCase(pStTable.name) = UCase(pTableName) Then
                    pDataset = pStTable
                    pWrkSp = pDataset.Workspace
                    GetDataTableFileName = pWrkSp.pathname & "\" & pTableName
                    Exit For
                End If
            Next
        End If
    End Function

    '*** Remove the data table from the map, and delete from disk
    Public Sub DeleteDataTable(ByRef tableDir As String, ByRef TableName As String)
        If gDataset.Tables.Contains(TableName) Then gDataset.Tables.Remove(TableName)
        Dim fn As String = String.Format("{0}\{1}.dbf", tableDir, TableName)
        If My.Computer.FileSystem.FileExists(fn) Then My.Computer.FileSystem.DeleteFile(fn)
        ''Define temporary variables
        'Dim pTabCollection As IStandaloneTableCollection
        'pTabCollection = gMap

        'Dim pStTable As IStandaloneTable
        'Dim i As Short

        'For i = 0 To (pTabCollection.StandaloneTableCount - 1)
        '    pStTable = pTabCollection.StandaloneTable(i)
        '    If (pStTable.name = TableName) Then
        '        pTabCollection.RemoveStandaloneTable(pStTable)
        '    End If
        'Next

        'Dim fso As Scripting.FileSystemObject
        'fso = CreateObject("Scripting.FileSystemObject")
        'If (fso.FileExists(tableDir & "\" & TableName & ".dbf")) Then
        '    fso.DeleteFile((tableDir & "\" & TableName & ".dbf"))
        'End If
    End Sub



    '    '****** Subroutine to read in the LuLookupTable table and create a remap table from it
    '    Public Function ReMapTableForLu_Cover(ByRef pTable As iTable, ByRef field1 As String, ByRef field2 As String, ByRef BoolInteger As Boolean) As IRaster
    '        On Error GoTo CleanUp

    '        Dim pLanduseRLayer As IRasterLayer
    '        pLanduseRLayer = GetInputRasterLayer("Landuse")

    '        Dim pDEMRLayer As IRasterLayer
    '        pDEMRLayer = GetInputRasterLayer("DEM")

    '        Dim pRasterPropsDEM As IRasterProps
    '        pRasterPropsDEM = pDEMRLayer.Raster

    '        ' Get the dataset of the newly created lucode-cn grid
    '        Dim pInputDataset As IGeoDataset
    '        pInputDataset = pLanduseRLayer.Raster

    '        Dim pFldLookup As Integer
    '        pFldLookup = pTable.FindField("LOOKUP")
    '        Dim pFieldEdit As IFieldEdit
    '        If (pFldLookup = -1) Then 'Add the field
    '            pFieldEdit = New Field
    '            With pFieldEdit
    '                .name = "LOOKUP"
    '                .IsNullable = True
    '                '          .Type = esriFieldTypeDouble
    '            End With
    '            pTable.AddField(pFieldEdit)
    '            pFldLookup = pTable.FindField("LOOKUP")
    '        End If

    '        'Define the in and out variables
    '        Dim field2Index As Integer
    '        'Get the field1 index
    '        field2Index = pTable.FindField(field1)
    '        If (field2Index = -1) Then
    '            MsgBox("LuLookupTable table should have a " & field2 & " field", MsgBoxStyle.Exclamation)
    '            Exit Function
    '        End If

    '        Dim pFieldScale As IField
    '        pFieldScale = pTable.Fields.Field(pTable.FindField(field1))
    '        pFieldEdit = pFieldScale
    '        Dim pScale As Short
    '        pScale = 10 ^ pFieldScale.Scale

    '        Dim pCursor As ICursor
    '        Dim pRow As iRow
    '        pCursor = pTable.Search(Nothing, True)
    '        pRow = pCursor.NextRow
    '        Do While Not pRow Is Nothing
    '            pRow.Value(pFldLookup) = pRow.Value(field2Index) * pScale
    '            pRow.Store()
    '            pRow = pCursor.NextRow
    '        Loop
    '        ' Create the RasterReclassOp object
    '        Dim pReclassOp As IReclassOp
    '        pReclassOp = New RasterReclassOp
    '        Dim pWS As IWorkspace
    '        Dim pWSF As IWorkspaceFactory
    '        pWSF = New RasterWorkspaceFactory
    '        pWS = pWSF.OpenFromFile(gMapTempFolder, 0)

    '        Dim penv As IRasterAnalysisEnvironment
    '        penv = pReclassOp
    '        penv.OutSpatialReference = pRasterPropsDEM.SpatialReference
    '        '    penv.SetExtent esriRasterEnvValue, pRasterPropsDEM.Extent
    '        '    penv.SetCellSize esriRasterEnvValue, pRasterPropsDEM.MeanCellSize.X
    '        penv.OutWorkspace = pWS
    '        penv.Mask = pDEMRLayer.Raster

    '        Dim pOutRaster As IRaster
    '        pOutRaster = pReclassOp.Reclass(pInputDataset, pTable, "LUCODE", "LUCODE", "LOOKUP", False)
    '        gAlgebraOp.BindRaster(pOutRaster, "OUT")
    '        If (BoolInteger = True) Then
    '            pOutRaster = gAlgebraOp.Execute("[OUT] / " & pScale)
    '        Else
    '            pOutRaster = gAlgebraOp.Execute("Float([OUT]) / " & CDbl(pScale))
    '        End If
    '        gAlgebraOp.UnbindRaster("OUT")

    '        '** Return the remap table
    '        ReMapTableForLu_Cover = pOutRaster

    'CleanUp:
    '        pLanduseRLayer = Nothing
    '        pDEMRLayer = Nothing
    '        pRasterPropsDEM = Nothing
    '        pWS = Nothing
    '        pWSF = Nothing
    '        penv = Nothing
    '        pInputDataset = Nothing
    '        pReclassOp = Nothing
    '        pOutRaster = Nothing
    '    End Function

    '    '*** Subroutine to create a unique file name
    '    Public Function CreateUniqueFileName(ByRef iDir As String, ByRef iFile As String) As String
    '        Dim fsObj As Scripting.FileSystemObject
    '        fsObj = CreateObject("Scripting.FileSystemObject")
    '        Dim filename As String
    '        filename = iDir & "\" & iFile
    '        Dim fcounter As Short
    '        fcounter = 1
    '        Do While fsObj.FolderExists(filename)
    '            filename = iDir & "\" & iFile & CStr(fcounter)
    '            fcounter = fcounter + 1
    '        Loop
    '        fsObj = Nothing
    '        'Return the unique filename
    '        CreateUniqueFileName = iFile & CStr(fcounter)
    '    End Function


    '    '*** Subroutine to create a unique raster name
    '    Public Function CreateUniqueRasterName(ByRef iDir As Object, ByRef iFileName As Object) As String
    '        Dim fsObj As Scripting.FileSystemObject
    '        fsObj = CreateObject("Scripting.FileSystemObject")
    '        Dim filename As String
    '        Dim fcounter As Short
    '        'fcounter = 1
    '        filename = iDir & iFileName & CStr(fcounter)
    '        Do While (fsObj.FileExists(filename & ".*") Or fsObj.FolderExists(filename))
    '            fcounter = fcounter + 1
    '            filename = iDir & "\" & iFileName & CStr(fcounter)
    '        Loop
    '        fsObj = Nothing
    '        'Return the unique filename
    '        CreateUniqueRasterName = iFileName & CStr(fcounter)
    '    End Function

    '    '*** Subroutine to create a unique table name
    '    Public Function CreateUniqueTableName(ByRef iDir As String, ByRef iTable As String) As String
    '        Dim fsObj As Scripting.FileSystemObject
    '        fsObj = CreateObject("Scripting.FileSystemObject")
    '        Dim filename As String
    '        Dim fcounter As Short
    '        fcounter = 1
    '        filename = iDir & "\" & iTable & CStr(fcounter) & ".dbf"
    '        Do While fsObj.FileExists(filename)
    '            fcounter = fcounter + 1
    '            filename = iDir & "\" & iTable & CStr(fcounter) & ".dbf"
    '        Loop
    '        fsObj = Nothing
    '        'Return the unique table name
    '        CreateUniqueTableName = iTable & CStr(fcounter)
    '    End Function

    '    '*** Subroutine to create raster from shapefile
    '    Public Function ConvertFeatureToRaster(ByRef pFeatureClass As IFeatureClass, ByRef pFieldName As String, ByRef pFileName As String, ByRef pQueryFilter As IQueryFilter) As IRasterDataset

    '        On Error GoTo ShowError

    '        Dim pDEMRLayer As IRasterLayer
    '        pDEMRLayer = GetInputRasterLayer("DEM")

    '        'Create a workspace
    '        Dim pWSF As IWorkspaceFactory
    '        Dim pWS As IWorkspace
    '        pWSF = New RasterWorkspaceFactory
    '        pWS = pWSF.OpenFromFile(gMapTempFolder, 0)

    '        'Select all features of the feature class
    '        Dim pSelectionSet As ISelectionSet
    '        ' Use the query filter to select features from nhd feature layer
    '        ''    Set pSelectionSet = pFeatureClass.Select(pQueryFilter, esriSelectionTypeIDSet, esriSelectionOptionNormal, Nothing)

    '        ' Define the featureclassdescriptor
    '        Dim pGeoDataDescriptor As IFeatureClassDescriptor
    '        pGeoDataDescriptor = New FeatureClassDescriptor
    '        ' Get the selection set
    '        pGeoDataDescriptor.CreateFromSelectionSet(pSelectionSet, Nothing, pFieldName)

    '        Dim pgeods As IGeoDataset
    '        pgeods = pGeoDataDescriptor
    '        ' Delete Old Files
    '        Dim fsObj As Scripting.FileSystemObject
    '        fsObj = CreateObject("Scripting.FileSystemObject")
    '        If fsObj.FolderExists(gMapTempFolder & "\" & pFileName) Then
    '            fsObj.DeleteFolder(gMapTempFolder & "\" & pFileName)
    '        End If

    '        Dim pRasterPropsDEM As IRasterProps
    '        pRasterPropsDEM = pDEMRLayer.Raster

    '        '*** Create the conversion object
    '        Dim pConvert As IConversionOp
    '        pConvert = New RasterConversionOp

    '        'Get the conversion environment
    '        Dim penv As IRasterAnalysisEnvironment
    '        Dim pCellSize As Double
    '        penv = pConvert
    '        penv.OutSpatialReference = pRasterPropsDEM.SpatialReference
    '        '    penv.SetExtent esriRasterEnvValue, pRasterPropsDEM.Extent
    '        pCellSize = pRasterPropsDEM.MeanCellSize.X
    '        '    penv.SetCellSize esriRasterEnvValue, pCellSize
    '        penv.Mask = pDEMRLayer.Raster

    '        'Create a new raster dataset
    '        Dim pConRaster As IRasterDataset
    '        pConRaster = pConvert.ToRasterDataset(pgeods, "GRID", pWS, pFileName)

    '        'Return the value
    '        ConvertFeatureToRaster = pConRaster
    '        GoTo CleanUp

    'ShowError:

    '        MsgBox(Err.Description)

    'CleanUp:

    '        pDEMRLayer = Nothing
    '        pWSF = Nothing
    '        pWS = Nothing
    '        pSelectionSet = Nothing
    '        pGeoDataDescriptor = Nothing
    '        pgeods = Nothing
    '        fsObj = Nothing
    '        pRasterPropsDEM = Nothing
    '        pConvert = Nothing
    '        penv = Nothing
    '        pConRaster = Nothing

    '    End Function


    '    'Open the shapefile and get the feature class
    '    Public Function OpenShapeFile(ByRef dir_Renamed As String, ByRef name As String) As IFeatureClass

    '        On Error GoTo ErrHandler

    '        Dim pWSFact As IWorkspaceFactory
    '        Dim connectionProperties As IPropertySet
    '        Dim pShapeWS As IFeatureWorkspace
    '        Dim isShapeWS As Boolean
    '        OpenShapeFile = Nothing
    '        pWSFact = New ShapefileWorkspaceFactory
    '        isShapeWS = pWSFact.IsWorkspace(dir_Renamed)
    '        Dim pFClass As IFeatureClass
    '        If (isShapeWS) Then
    '            connectionProperties = New PropertySet
    '            connectionProperties.SetProperty("DATABASE", dir_Renamed)
    '            pShapeWS = pWSFact.Open(connectionProperties, 0)
    '            pFClass = pShapeWS.OpenFeatureClass(name)
    '            OpenShapeFile = pFClass
    '            pFClass = Nothing
    '        End If

    '        GoTo CleanUp

    'ErrHandler:
    '        MsgBox("OpenShapeFile: " & Err.Description)

    'CleanUp:
    '        pWSFact = Nothing
    '        connectionProperties = Nothing
    '        pShapeWS = Nothing

    '    End Function


    '    'Open the shapefile and get the feature class
    '    Public Function OpenDBFTableFromDisk(ByRef pTableName As String) As iTable
    '        On Error GoTo ErrHandler

    '        Dim fso As Scripting.FileSystemObject
    '        fso = CreateObject("Scripting.FileSystemObject")
    '        If Not (fso.FileExists(gMapTempFolder & "\" & pTableName & ".dbf")) Then
    '            OpenDBFTableFromDisk = Nothing
    '            Exit Function
    '        End If

    '        Dim pFact As IWorkspaceFactory
    '        Dim pWorkspace As IWorkspace
    '        Dim pFeatWS As IFeatureWorkspace
    '        Dim pTable As iTable
    '        pFact = New ShapefileWorkspaceFactory
    '        pWorkspace = pFact.OpenFromFile(gMapTempFolder, 0)
    '        pFeatWS = pWorkspace
    '        pTable = pFeatWS.OpenTable(pTableName)
    '        OpenDBFTableFromDisk = pTable
    '        GoTo CleanUp
    'ErrHandler:
    '        MsgBox("OpenDBFTableFromDisk: " & Err.Description)

    'CleanUp:
    '        pFact = Nothing
    '        pWorkspace = Nothing
    '        pFeatWS = Nothing
    '        pTable = Nothing
    '    End Function

    '    '*** Subroutine to export text file as dbf table
    '    Public Sub ExportClimateTextFileToDBFtable()
    '        On Error GoTo ShowError

    '        Dim pTextTable As iTable
    '        pTextTable = GetDataTable("ClimateDataTextFile")

    '        Dim pDBFTable As iTable
    '        pDBFTable = OpenDBFTableFromDisk("ClimateData")
    '        If Not (pDBFTable Is Nothing) Then
    '            Exit Sub
    '        End If

    '        ' Get the dataset name for the input table
    '        Dim pDataset As IDataset
    '        Dim pDSName As IDatasetName
    '        pDataset = pTextTable
    '        pDSName = pDataset.FullName

    '        ' Get the output dataset name ready. In this case we are creating a dbf file in temp folder
    '        Dim pWkSpFactory As IWorkspaceFactory
    '        Dim pWkSp As IWorkspace
    '        Dim pWkSpDS As IDataset
    '        Dim pWkSpName As IWorkspaceName
    '        Dim pOutDSName As IDatasetName
    '        pWkSpFactory = New ShapefileWorkspaceFactory
    '        pWkSp = pWkSpFactory.OpenFromFile(gMapTempFolder, 0)
    '        pWkSpDS = pWkSp
    '        pWkSpName = pWkSpDS.FullName
    '        pOutDSName = New TableName
    '        pOutDSName.name = "ClimateData"
    '        pOutDSName.WorkspaceName = pWkSpName

    '        ' Export (Selection is ignored)
    '        Dim pExpOp As IExportOperation
    '        pExpOp = New ExportOperation
    '        pExpOp.ExportTable(pDSName, Nothing, Nothing, pOutDSName, gApplication.hwnd)

    '        GoTo CleanUp
    'ShowError:
    '        MsgBox("ExportTextFileToDBFtable: " & Err.Description)
    'CleanUp:
    '        pTextTable = Nothing
    '        pDBFTable = Nothing
    '        pDataset = Nothing
    '        pDSName = Nothing
    '        pWkSpFactory = Nothing
    '        pWkSp = Nothing
    '        pWkSpDS = Nothing
    '        pOutDSName = Nothing
    '        pExpOp = Nothing

    '    End Sub

    '    '*** Subroutine to create raster from shapefile
    '    Public Function ConvertRasterToFeature(ByRef pRaster As IRaster, ByRef pFieldName As String, ByRef pFileName As String) As MapWindow.Interfaces.Layer

    '        'On Error GoTo CleanUp:

    '        'Create a workspace
    '        Dim pWSF As IWorkspaceFactory
    '        Dim pWS As IWorkspace
    '        pWSF = New ShapefileWorkspaceFactory
    '        pWS = pWSF.OpenFromFile(gMapTempFolder, 0)

    '        ' Delete Old Files
    '        Dim fsObj As Scripting.FileSystemObject
    '        fsObj = CreateObject("Scripting.FileSystemObject")

    '        Dim pds As IDataset
    '        Dim pFClass As IFeatureClass

    '        If fsObj.FileExists(gMapTempFolder & "\" & pFileName & ".shp") Then
    '            pFClass = OpenShapeFile(gMapTempFolder, pFileName)
    '            If (Not pFClass Is Nothing) Then
    '                pds = pFClass
    '                pds.Delete()
    '            End If
    '        End If

    '        If fsObj.FileExists(gMapTempFolder & "\" & pFileName) Then
    '            fsObj.DeleteFile(gMapTempFolder & "\" & Replace(pFileName, "shp", "*"))
    '        End If

    '        ' Create RasterDecriptor
    '        Dim pRDescr As IRasterDescriptor
    '        pRDescr = New RasterDescriptor
    '        pRDescr.Create(pRaster, Nothing, pFieldName)

    '        ' Create ConversionOp
    '        Dim pConversionOp As IConversionOp
    '        pConversionOp = New RasterConversionOp

    '        ' Perform conversion
    '        Dim pOutFClass As IFeatureClass
    '        pOutFClass = pConversionOp.RasterDataToPolygonFeatureData(pRDescr, pWS, pFileName, False)

    '        ' Create a feature layer
    '        Dim pOutFLayer As MapWindow.Interfaces.Layer
    '        pOutFLayer = New FeatureLayer
    '        pOutFLayer.FeatureClass = pOutFClass

    '        ' Return the feature layer
    '        ConvertRasterToFeature = pOutFLayer

    '        GoTo CleanUp

    'CleanUp:

    '        pWSF = Nothing
    '        pWS = Nothing
    '        pds = Nothing
    '        pFClass = Nothing
    '        pRDescr = Nothing
    '        pConversionOp = Nothing
    '        pOutFClass = Nothing
    '        pOutFLayer = Nothing

    '    End Function

    ''' <summary>
    ''' Assume this means to remove then delete the gridfile
    ''' </summary>
    ''' <param name="pFeatureName"></param>
    ''' <param name="IsFullPath"></param>
    ''' <remarks></remarks>
    Public Sub DeleteRasterDataset(ByRef pRasterName As String, Optional ByRef IsFullPath As Boolean = False)
        If gMapTempFolder = "" Then
            Initialize()
            DefineApplicationPath()
        End If

        Dim FileName As String
        If IsFullPath Then
            FileName = pRasterName
        Else
            If GisUtil.IsLayer(pRasterName) Then
                FileName = GisUtil.LayerFileName(pRasterName)
                GisUtil.RemoveLayer(pRasterName)
            Else
                FileName = String.Format("{0}\{1}.tif", gMapTempFolder, pRasterName)
            End If
        End If
        MapWinGeoProc.DataManagement.DeleteGrid(FileName)
    End Sub

    ''' <summary>
    ''' Assume this means to remove then delete the shapefile, etc.
    ''' </summary>
    ''' <param name="pFeatureName"></param>
    ''' <param name="IsFullPath"></param>
    ''' <remarks></remarks>
    Public Sub DeleteFeatureDataset(ByRef pFeatureName As String, Optional ByRef IsFullPath As Boolean = False)
        If gMapTempFolder = "" Then
            Initialize()
            DefineApplicationPath()
        End If

        Dim FileName As String
        If IsFullPath Then
            FileName = pFeatureName
        Else
            If GisUtil.IsLayer(pFeatureName) Then
                FileName = GisUtil.LayerFileName(pFeatureName)
                GisUtil.RemoveLayer(pFeatureName)
            Else
                FileName = String.Format("{0}\{1}.shp", gMapTempFolder, pFeatureName)
            End If
        End If
        MapWinGeoProc.DataManagement.DeleteShapefile(pFeatureName)
    End Sub

    '    'Function to open raster dataset and read the raster from it
    '    Public Function OpenRasterDatasetFromDisk(ByRef pRasterName As String) As IRaster
    '        On Error GoTo ShowError

    '        ' check if raster dataset exist
    '        Dim fsObj As Scripting.FileSystemObject
    '        fsObj = CreateObject("Scripting.FileSystemObject")
    '        If Not fsObj.FolderExists(gMapTempFolder & "\" & pRasterName) Then
    '            OpenRasterDatasetFromDisk = Nothing
    '            Exit Function
    '        End If

    '        Dim pWF As IWorkspaceFactory
    '        pWF = New RasterWorkspaceFactory
    '        Dim pRW As IRasterWorkspace
    '        pRW = pWF.OpenFromFile(gMapTempFolder, 0)

    '        Dim pRDS As IRasterDataset
    '        pRDS = pRW.OpenRasterDataset(LCase(pRasterName))

    '        If pRDS Is Nothing Then
    '            MsgBox(pRasterName & " dataset not found on " & gMapTempFolder)
    '            GoTo CleanUp
    '        End If

    '        OpenRasterDatasetFromDisk = pRDS.CreateDefaultRaster
    '        GoTo CleanUp

    'ShowError:
    '        MsgBox("OpenRasterDatasetFromDisk: " & Err.Description)

    'CleanUp:
    '        fsObj = Nothing
    '        pWF = Nothing
    '        pRW = Nothing
    '        pRDS = Nothing
    '    End Function

    '    'Subroutine to write raster to disk
    '    Public Sub WriteRasterDatasetToDisk(ByRef pRaster As IRaster, ByRef pOutName As String)
    '        On Error GoTo ShowError

    '        ' Create a raster workspace
    '        Dim pRWS As IRasterWorkspace
    '        Dim pWSF As IWorkspaceFactory
    '        pWSF = New RasterWorkspaceFactory
    '        pRWS = pWSF.OpenFromFile(gMapTempFolder, 0)

    '        Dim pRasterDataSet As IRasterDataset
    '        Dim pDataset As IDataset

    '        'Delete the raster dataset if present on disk
    '        Dim fso As Scripting.FileSystemObject
    '        fso = CreateObject("Scripting.FileSystemObject")
    '        If fso.FolderExists(gMapTempFolder & "\" & pOutName) Then
    '            pRasterDataSet = pRWS.OpenRasterDataset(pOutName)

    '            If Not pRasterDataSet Is Nothing Then
    '                pDataset = pRasterDataSet
    '                If pDataset.CanDelete Then
    '                    pDataset.Delete()
    '                Else
    '                    MsgBox("Can not delete " & pOutName)
    '                    Exit Sub
    '                End If
    '            End If
    '        End If

    '        ' SaveAs the projected raster
    '        Dim pRasBandCol As IRasterBandCollection
    '        pRasBandCol = pRaster
    '        pRasBandCol.SaveAs(pOutName, pRWS, "GRID")
    '        GoTo CleanUp

    'ShowError:
    '        MsgBox("WriteRasterDatasetToDisk: " & Err.Description, MsgBoxStyle.Exclamation, pOutName)

    'CleanUp:
    '        pRWS = Nothing
    '        pWSF = Nothing
    '        pRasterDataSet = Nothing
    '        pDataset = Nothing
    '        fso = Nothing
    '        pRasBandCol = Nothing
    '    End Sub

    Friend Enum enumDataType
        Raster
        Feature
        Table
    End Enum

    Public Function SelectLayerOrTableData(ByVal DataType As enumDataType, Optional ByVal Titlename As String = "") As String
        Dim pDlg As New OpenFileDialog
        Try
            ' set up filters on the files that will be browsed
            With pDlg
                Select Case DataType
                    Case enumDataType.Raster
                        Dim g As New MapWinGIS.Grid
                        .Filter = g.CdlgFilter
                    Case enumDataType.Feature
                        Dim sf As New MapWinGIS.Shapefile
                        .Filter = sf.CdlgFilter
                    Case enumDataType.Table
                        .Filter = "DBF files (*.dbf)|*.dbf"
                End Select
                .InitialDirectory = GetSetting(REGAPP, "General", "DefaultDir", gMapInputFolder)
                .Filter &= IIf(.Filter = "", "", "|") & "All files (*.*)|*.*"
                .Multiselect = False
                .Title = String.Format("Select {0}Data", IIf(Titlename = "", "", Titlename & " "))

                If .ShowDialog = DialogResult.OK Then
                    'add shapefile or grid as layer and return layer name
                    'if is table, return full path to table
                    SaveSetting(REGAPP, "General", "DefaultDir", IO.Path.GetDirectoryName(.SafeFileName))
                    If DataType = "Raster" Or DataType = "Feature" Then
                        Dim LayerName As String = IO.Path.GetFileNameWithoutExtension(.FileName)
                        GisUtil.AddGroup("GBMM")
                        GisUtil.AddLayerToGroup(.FileName, LayerName, "GBMM")
                        Return LayerName
                    ElseIf DataType = "Table" Then
                        Return .FileName
                    Else
                        Return .FileName
                    End If
                End If
                Return ""
            End With
        Catch ex As Exception
            ErrorMsg(, ex)
            Return ""
        Finally
            pDlg.Dispose()
        End Try
    End Function


    '    Public Sub SoilStep2LitterDecompositionRate()

    '        On Error GoTo ShowError

    '        '* Get the landuse raster layer
    '        Dim pLanduseRLayer As IRasterLayer
    '        pLanduseRLayer = GetInputRasterLayer("Landuse")
    '        If (pLanduseRLayer Is Nothing) Then
    '            MsgBox("Map should have Landuse layer")
    '            Exit Sub
    '        End If

    '        Dim kd1 As Double
    '        Dim kd2 As Double
    '        Dim kd3 As Double
    '        kd1 = CDbl(InputDataDictionary.Item("HgLandKDcomp1"))
    '        kd2 = CDbl(InputDataDictionary.Item("HgLandKDcomp2"))
    '        kd3 = CDbl(InputDataDictionary.Item("HgLandKDcomp3"))

    '        'Create a Kcomp - litter decomposition rate raster
    '        Dim pRemap As IRemap
    '        Dim pNumberRemap As INumberRemap
    '        pNumberRemap = New NumberRemap
    '        pNumberRemap.MapValue(41, kd1 * 100000) 'Deciduous forest  - 0.0019
    '        pNumberRemap.MapValue(42, kd2 * 100000) 'Evergreen forest - 0.0005
    '        pNumberRemap.MapValue(43, kd3 * 100000) 'Mixed forest     - 0.0012
    '        pNumberRemap.MapValue(51, kd1 * 100000) 'Deciduous shrubland
    '        pNumberRemap.MapValue(52, kd2 * 100000) 'Evergreen shrubland
    '        pNumberRemap.MapValue(53, kd3 * 100000) 'Mixed shrubland
    '        pNumberRemap.MapValue(91, kd3 * 100000) 'Woody Wetland
    '        pRemap = pNumberRemap

    '        Dim pKdCompRaster As IRaster
    '        pKdCompRaster = gReclassOp.ReclassByRemap(pLanduseRLayer.Raster, pRemap, False)
    '        gAlgebraOp.BindRaster(pKdCompRaster, "Kdcomp")
    '        pKdCompRaster = gAlgebraOp.Execute("Con(IsNull([Kdcomp]), 0, Float([Kdcomp]) / 100000 )")
    '        gAlgebraOp.UnbindRaster("Kdcomp")
    '        WriteRasterDatasetToDisk(pKdCompRaster, "Kdcomp")

    '        GoTo CleanUp
    'ShowError:
    '        MsgBox("Step2LitterDecompositionLoad: " & Err.Description)
    'CleanUp:
    '        pLanduseRLayer = Nothing
    '        pRemap = Nothing
    '        pNumberRemap = Nothing
    '        pKdCompRaster = Nothing
    '    End Sub

    '    Public Function GetGridCellValue(ByRef pRaster As IRaster, ByRef lCol As Integer, ByRef lRow As Integer) As Double
    '        Dim pOrigin As IPnt
    '        pOrigin = New DblPnt
    '        pOrigin.SetCoords(lCol, lRow)

    '        Dim pSize As IPnt
    '        pSize = New DblPnt
    '        pSize.SetCoords(1, 1)

    '        Dim pPixelBlock As IPixelBlock3
    '        pPixelBlock = pRaster.CreatePixelBlock(pSize)

    '        pRaster.Read(pOrigin, pPixelBlock)
    '        GetGridCellValue = pPixelBlock.GetVal(0, 0, 0)

    'CleanUp:
    '        pOrigin = Nothing
    '        pSize = Nothing
    '        pPixelBlock = Nothing
    '    End Function

    '    Public Sub SetGridCellValue(ByRef pRaster As IRaster, ByRef lCol As Integer, ByRef lRow As Integer, ByRef newValue As Object)
    '        Dim pOrigin As IPnt
    '        pOrigin = New DblPnt
    '        pOrigin.SetCoords(lCol, lRow)

    '        Dim pSize As IPnt
    '        pSize = New DblPnt
    '        pSize.SetCoords(1, 1)

    '        Dim pPixelBlock As IPixelBlock3
    '        pPixelBlock = pRaster.CreatePixelBlock(pSize)

    '        Dim pRawPixel As IRawPixels
    '        pRawPixel = pRaster
    '        pRawPixel.Read(pOrigin, pPixelBlock)

    '        'pRaster.Read pOrigin, pPixelBlock

    '        Dim vPixelData As Object
    '        vPixelData = pPixelBlock.PixelDataByRef(0)
    '        vPixelData(0, 0) = newValue


    '        Dim pCache As Object
    '        pCache = pRawPixel.AcquireCache
    '        pRawPixel.Write(pOrigin, pPixelBlock)
    '        pRawPixel.ReturnCache(pCache)


    'CleanUp:
    '        pOrigin = Nothing
    '        pSize = Nothing
    '        pPixelBlock = Nothing
    '        pRawPixel = Nothing
    '    End Sub

    '    Public Function GetGridCellNoDataMaskVal(ByRef pRaster As IRaster, ByRef lCol As Integer, ByRef lRow As Integer) As Short
    '        Dim pOrigin As IPnt
    '        pOrigin = New DblPnt
    '        pOrigin.SetCoords(lCol, lRow)

    '        Dim pSize As IPnt
    '        pSize = New DblPnt
    '        pSize.SetCoords(1, 1)

    '        Dim pPixelBlock As IPixelBlock3
    '        pPixelBlock = pRaster.CreatePixelBlock(pSize)

    '        pRaster.Read(pOrigin, pPixelBlock)
    '        GetGridCellNoDataMaskVal = pPixelBlock.GetNoDataMaskVal(0, 0, 0)

    'CleanUp:
    '        pOrigin = Nothing
    '        pSize = Nothing
    '        pPixelBlock = Nothing
    '    End Function

    '    Public Function GetJoinedTable(ByRef pTable1 As Object, ByRef strFieldName1 As String, ByRef pTable2 As Object, ByRef strFieldName2 As String) As iTable

    '        ' ++ Create the MemoryRelationshipClass that defines what is to be joined
    '        Dim pMemRelClassFact As IMemoryRelationshipClassFactory
    '        pMemRelClassFact = New MemoryRelationshipClassFactory
    '        Dim pRelClass As IRelationshipClass
    '        '    Set pRelClass = pMemRelClassFact.Open("SAP_Assess", pTable1, _
    '        ''    strFieldName1, pTable2, strFieldName2, "forward", "backward", esriRelCardinalityOneToOne)

    '        ' ++ Perform the join
    '        Dim pRelQueryTableFact As IRelQueryTableFactory
    '        Dim pRelQueryTab As iTable
    '        pRelQueryTableFact = New RelQueryTableFactory
    '        pRelQueryTab = pRelQueryTableFact.Open(pRelClass, True, Nothing, Nothing, "", True, True)

    '        GetJoinedTable = pRelQueryTab

    '    End Function
    '    Public Sub StartEditingFeatureLayer(ByRef pFeatureLayer As MapWindow.Interfaces.Layer)

    '        Dim pID As New UID
    '        Dim TaskCount As Short

    '        '  pID = "esriEditor.Editor"
    '        g_pEditor = gApplication.FindExtensionByCLSID(pID)

    '        If (pFeatureLayer Is Nothing) Then
    '            Exit Sub
    '        End If

    '        Dim pDataset As IDataset
    '        pDataset = pFeatureLayer.FeatureClass
    '        Dim pWorkspace As IWorkspace
    '        pWorkspace = pDataset.Workspace
    '        g_pEditor.StartEditing(pWorkspace)

    '    End Sub

    '    Public Sub StopEditingFeatureLayer()
    '        If Not (g_pEditor Is Nothing) Then
    '            g_pEditor.StopEditing(True)
    '        End If
    '    End Sub

    '    '*******************************************************************************
    '    'Subroutine : StringContains
    '    'Purpose    : Checks whether a given string is contained in another one
    '    'Note       :
    '    'Arguments  :
    '    'Author     : Sabu Paul
    '    'History    :
    '    '*******************************************************************************
    '    Public Function StringContains(ByRef FindString As String, ByRef SearchString As String) As Boolean
    '        Dim TempString As String
    '        TempString = Replace(FindString, SearchString, "")
    '        If (FindString <> TempString) Then
    '            StringContains = True
    '        Else
    '            StringContains = False
    '        End If
    '    End Function

    ''' <summary>
    ''' Return lists of feature (shape) layer names, grid layer names, and table names found in default data directory
    ''' </summary>
    Friend Sub GetNameLists(ByRef FeatureLayerNames As Generic.List(Of String), ByRef RasterLayerNames As Generic.List(Of String), ByRef TableNames As Generic.List(Of String))
        FeatureLayerNames = New Generic.List(Of String)
        RasterLayerNames = New Generic.List(Of String)
        TableNames = New Generic.List(Of String)
        For i As Integer = 0 To GisUtil.NumLayers - 1
            If GisUtil.LayerType(i) = MapWindow.Interfaces.eLayerType.Grid Then
                RasterLayerNames.Add(GisUtil.LayerName(i))
            Else
                FeatureLayerNames.Add(GisUtil.LayerName(i))
            End If
        Next
        'stand-alone tables aren't assigned to GIS project; instead just get list of tables from GBMM data folder
        For Each fn As String In My.Computer.FileSystem.GetFiles(gMapDataFolder, FileIO.SearchOption.SearchTopLevelOnly, New String() {"*.dbf", "*.txt", "*.csv"})
            TableNames.Add(IO.Path.GetFileName(fn))
        Next
        'For Each tbl As DataTable In gDataset.Tables
        '    TableNames.Add(tbl.TableName)
        'Next
    End Sub

    ''' <summary>
    ''' Make sure specified table has all the required fields
    ''' </summary>
    ''' <param name="TableName">Name of table or layer</param>
    ''' <param name="ReqdFields">List of required layer names</param>
    Friend Function CheckTableFields(ByVal TableName As String, ByVal ParamArray ReqdFields() As String) As Boolean
        Dim dt As DataTable

        dt = GetDataTable(TableName)
        If dt Is Nothing Then
            WarningMsg(TableName & " file is invalid; unable to check for required fields.")
            Return False
        End If

        With dt
            For Each s As String In ReqdFields
                If Not .Columns.Contains(s) Then
                    WarningMsg(TableName & " table is missing the following required field: " & s)
                    Return False
                End If
            Next
        End With
        Return True
    End Function

End Module