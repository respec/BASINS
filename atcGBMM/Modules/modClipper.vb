Module modClipper

    Private pCurrProjectPath As String

    Public Sub CreateCalibrationData2(ByRef poly As IPolygon)
        On Error GoTo ShowError

        Dim pMouseCursor As IMouseCursor
        pMouseCursor = New MouseCursor
        pMouseCursor.SetCursor(2)

        Dim pDoc As IDocument
        pDoc = gApplication.Document
        Dim pTemplates As ITemplates
        pTemplates = gApplication.Templates
        Dim lTempCount As Integer
        lTempCount = pTemplates.Count

        ' The document is always the last item
        Dim strDocPath As String
        strDocPath = pTemplates.Item(lTempCount - 1)
        pCurrProjectPath = Replace(strDocPath, pDoc.Title, "")
        pCurrProjectPath = Replace(pCurrProjectPath, ".mxd", "") 'For certain OS, ".mxd" is left
        Dim pFileSource As String
        pFileSource = pCurrProjectPath & Replace(pDoc.Title, ".mxd", "")

        pTemplates = Nothing
        pDoc = Nothing

        Dim fso As Scripting.FileSystemObject
        fso = CreateObject("Scripting.FileSystemObject")
        Dim icounter As Short
        icounter = 1

        Dim strCalibFolder As String
        Dim uniqueCalibFolder As String
        uniqueCalibFolder = "TestPrj" & icounter
        Do While fso.FolderExists(pCurrProjectPath & uniqueCalibFolder)
            icounter = icounter + 1
            uniqueCalibFolder = "TestPrj" & icounter
        Loop

        Dim strMessage As String
        strMessage = "Enter a new project name for clipped area:"
GetFolder:
        strCalibFolder = InputBox(strMessage, "Clip Area", uniqueCalibFolder)
        If (strCalibFolder = "") Then
            Exit Sub
        End If
        If (Len(strCalibFolder) > 8) Then
            MsgBox("Project Name should be less than or equal to 8 characters.")
            GoTo GetFolder
        End If

        Dim strCalibrateFolderPath As String
        strCalibrateFolderPath = pCurrProjectPath & strCalibFolder
        Dim strCalibrationDataFolderPath As String
        strCalibrationDataFolderPath = strCalibrateFolderPath & "\DATA"
        Dim strCalibrationTempFolderPath As String
        strCalibrationTempFolderPath = strCalibrateFolderPath & "\TEMP"

        'Create the input folder to put the inputdata.txt
        Dim strCalibrationInputFolderPath As String
        strCalibrationInputFolderPath = strCalibrateFolderPath & "\INPUT"

        gMapTempFolder = strCalibrationDataFolderPath

        If Not (fso.FolderExists(strCalibrateFolderPath)) Then
            fso.CreateFolder(strCalibrateFolderPath)
            fso.CreateFolder(strCalibrationDataFolderPath)
            fso.CreateFolder(strCalibrationTempFolderPath)
            fso.CreateFolder(strCalibrationInputFolderPath)
        Else
            strMessage = strCalibFolder & " exists. " & vbCr & "Enter a New Project Name"
            GoTo GetFolder
        End If

        'Load wait window
        frmProcess.Show()
        gApplication.RefreshWindow()

        ClipInputDatasetsForCalibration2(poly, strCalibrationDataFolderPath, strCalibrationTempFolderPath)
        ModuleClippingHelper.CreateMXDForClippedData(pCurrProjectPath, strCalibrationDataFolderPath, strCalibFolder)

        frmProcess.Close()
        MsgBox("You may use the newly clipped project in " & pCurrProjectPath & strCalibFolder & " folder or continue with setup.")

        'Copy the input file to new destination
        If fso.FileExists(gMapInputFolder & "\InputData.txt") Then
            fso.CopyFile(gMapInputFolder & "\InputData.txt", strCalibrationInputFolderPath & "\InputData.txt")
        End If

        GoTo CleanUp

ShowError:
        MsgBox("CreateCalibrationData: " & Err.Description)

CleanUp:
        pMouseCursor.SetCursor(0)
    End Sub

    Public Sub ClipInputDatasetsForCalibration2(ByRef ipoly As IPolygon, ByRef strCalibDataFolder As String, ByRef strCalibTempFolder As String)
        On Error GoTo ShowError
        Dim fc, fso, fd, f As Object
        fso = CreateObject("Scripting.FileSystemObject")
        If fso.FolderExists(strCalibDataFolder) Then
            fd = fso.GetFolder(strCalibDataFolder)
            fc = fd.Files
            For Each f In fc
                MsgBox("Need an empty folder for outputing datasets for calibration. Folder " & strCalibDataFolder & " is not empty.")
                Exit Sub
            Next f
        Else
            fso.CreateFolder(strCalibDataFolder)
        End If

        Dim pRWSF As IWorkspaceFactory
        pRWSF = New RasterWorkspaceFactory
        Dim pRWS As IRasterWorkspace2
        pRWS = pRWSF.OpenFromFile(strCalibDataFolder, 0)
        Dim pRWSTemp As IRasterWorkspace2
        pRWSTemp = pRWSF.OpenFromFile(strCalibTempFolder, 0)

        Dim pFWSF As IWorkspaceFactory
        pFWSF = New ShapefileWorkspaceFactory
        Dim pFWS As IFeatureWorkspace
        pFWS = pFWSF.OpenFromFile(strCalibDataFolder, 0)

        Dim pEnvelope As IEnvelope
        pEnvelope = ipoly.Envelope

        ''''Raghu's new addition
        ClipAndSaveRasterLayers2("SoilMap", ipoly, pRWS, False)

        ClipAndSaveRasterLayers2("Landuse", ipoly, pRWS, False)

        ClipAndSaveRasterLayers2("DEM", ipoly, pRWS, False)

        ' Hg Dry & Wet
        If InputDataDictionary("chkGrid") = 1 Then
            If InputDataDictionary("HgDryGrid") <> "" Then ClipAndSaveRasterLayers2("HgDryGrid", ipoly, pRWS, False)
            If InputDataDictionary("HgWetGrid") <> "" Then ClipAndSaveRasterLayers2("HgWetGrid", ipoly, pRWS, False)
        End If

        'initial soil moisture
        If InputDataDictionary("optInputSoilMoisture") = True And InputDataDictionary("InitialSoilMoisture") <> "" Then
            ClipAndSaveRasterLayers2("InitialSoilMoisture", ipoly, pRWS, False)
        End If

        'initial soil mercury
        If InputDataDictionary("optionSoilHgGrid") = True And InputDataDictionary("InitialSoilHg") <> "" Then
            ClipAndSaveRasterLayers2("InitialSoilHg", ipoly, pRWS, False)
        End If

        ''Thiessen Hg Time series option
        If InputDataDictionary("chkTime") = 1 Then
            CopyTextFile("HgDryDepTimeSeries", strCalibDataFolder)
            CopyTextFile("HgWetDepTimeSeries", strCalibDataFolder)
        End If

        ClipAndSaveFeatureData2("NHD", ipoly, strCalibDataFolder, False)

        If InputDataDictionary("PointSources") <> "" Then
        End If
        ClipAndSaveFeatureData2("Lakes", ipoly, strCalibDataFolder, True)

        'No need of assessmentpoints - Nov 22, 2005
        'CopyFeatureData "AssessPoints", pFWS, True
        ClipAndSaveFeatureData2("Drain", ipoly, strCalibDataFolder, True)

        Dim pAWSF As IWorkspaceFactory
        pAWSF = New ArcInfoWorkspaceFactory
        Dim pAWS As IArcInfoWorkspace
        pAWS = pAWSF.OpenFromFile(strCalibDataFolder, 0)

        ' Process all tables
        ' Climate Data
        CopyTextFile("ClimateDataTextFile", strCalibDataFolder)
        ' Irrigation
        '    CopyTableData "Irrigation", pFWS, False

        'SoilMap Data
        ' Landuse Lookup
        ' LU Code - CN
        'NHD-Rflow table
        ' Point Source
        If InputDataDictionary("PSdataTable") <> "" Then
            CopyTextFile("PSdataTable", strCalibDataFolder)
        End If


        '* Copy flow dir, flow accu and thiessenwtr raster datasets
        ' ClipAndSaveRasterDataset "FlowDir", pEnvelope, Nothing, pRWSTemp, True
        ' ClipAndSaveRasterDataset "FlowAccu", pEnvelope, Nothing, pRWSTemp, True

        ''Alvi Nov 06, 2005
        ''  ClipAndSaveRasterDataset "ThiessenWtr", pEnvelope, Nothing, pRWSTemp, True

        GoTo CleanUp

ShowError:
        MsgBox("Clipping input datasets error. Error :" & Err.Description)

CleanUp:

        pEnvelope = Nothing
    End Sub
    Public Sub ClipAndSaveFeatureData2(ByRef strLayerName As String, ByRef pPoly As IPolygon, ByRef strWSFolder As String, ByRef bOptional As Boolean)
        On Error GoTo ShowError

        Dim pFeatureLayer As IFeatureLayer
        pFeatureLayer = GetInputFeatureLayer(strLayerName)
        If pFeatureLayer Is Nothing Then
            If bOptional Then
                GoTo CleanUp
            Else
                Err.Description = "Invalid input data for " & strLayerName
                GoTo ShowError
            End If
        End If

        Dim pGeometryBag As IGeometryBag
        pGeometryBag = New GeometryBag
        pGeometryBag.SpatialReference = pPoly.SpatialReference

        Dim pFilter As ISpatialFilter
        pFilter = New SpatialFilter

        pFilter.Geometry = pPoly
        '    pFilter.SpatialRel = esriSpatialRelIntersects



        Dim pSWSF As IScratchWorkspaceFactory
        pSWSF = New ScratchWorkspaceFactory
        Dim pSWS As IWorkspace
        pSWS = pSWSF.DefaultScratchWorkspace

        Dim pInFC As IFeatureClass
        pInFC = pFeatureLayer.FeatureClass

        ' Get the selection set
        Dim pInSelSet As ISelectionSet
        '    Set pInSelSet = pInFC.Select(pFilter, esriSelectionTypeIDSet, esriSelectionOptionNormal, pSWS)

        ' If no feature intersects with the study area, quit without creating any new dataset
        If pInSelSet.Count = 0 Then
            GoTo CleanUp
        End If

        ' Get the FeatureClassName from the featureclass
        Dim pInDataset As IDataset
        pInDataset = pInFC

        Dim pInFCName As IFeatureClassName
        pInFCName = pInDataset.FullName

        Dim pInDsName As IDatasetName
        pInDsName = pInFCName

        ' Create a new feature class name
        ' Define the output feature class name
        Dim pOutFCName As IFeatureClassName
        pOutFCName = New FeatureClassName

        '    pOutFCName.FeatureType = esriFTSimple
        '    pOutFCName.ShapeType = esriGeometryAny
        pOutFCName.ShapeFieldName = "Shape"

        Dim pWorkspaceName As IWorkspaceName
        pWorkspaceName = New WorkspaceName
        pWorkspaceName.pathname = strWSFolder
        '    pWorkspaceName.WorkspaceFactoryProgID = "esriCore.shapefileworkspacefactory.1"

        Dim pOutDSName As IDatasetName
        pOutDSName = pOutFCName

        pOutDSName.name = pFeatureLayer.name
        pOutDSName.WorkspaceName = pWorkspaceName

        'Export
        Dim pExportOp As IExportOperation
        pExportOp = New ExportOperation
        pExportOp.ExportFeatureClass(pInDsName, Nothing, pInSelSet, Nothing, pOutDSName, 0)
        GoTo CleanUp

ShowError:
        MsgBox("Error when clipping feature data for " & strLayerName & ": " & Err.Description)
CleanUp:
        pFeatureLayer = Nothing
        pGeometryBag = Nothing
        pFilter = Nothing
        pSWSF = Nothing
        pSWS = Nothing
        pInFC = Nothing
        pInSelSet = Nothing
        pInDataset = Nothing
        pInFCName = Nothing
        pInDsName = Nothing
        pOutFCName = Nothing
        pWorkspaceName = Nothing
        pOutDSName = Nothing
        pExportOp = Nothing
    End Sub

    ''' Clip raster based on polygon envelope
    Public Sub ClipAndSaveRasterLayers2(ByRef strLayerName As String, ByRef ipoly As IPolygon, ByRef pRWS As IRasterWorkspace2, ByRef bOptional As Boolean)
        On Error GoTo ShowError

        'initialize raster objects
        Dim peop As IExtractionOp
        peop = New RasterExtractionOp

        Dim pRAEnv As IRasterAnalysisEnvironment
        pRAEnv = peop

        Dim tmprasLayer As IRasterLayer
        tmprasLayer = GetInputRasterLayer(strLayerName)

        Dim pds As IDataset
        Dim pTemp As ITemporaryDataset
        Dim pOutDS As IRasterDataset
        Dim pRasBC As IRasterBandCollection
        Dim pgds As IGeoDataset
        Dim irp As IRasterProps
        If tmprasLayer Is Nothing Then
            Err.Description = "Invalid input data for " & strLayerName
            GoTo ShowError
        Else
            irp = tmprasLayer.Raster

            '        pRAEnv.SetExtent esriRasterEnvValue, ipoly.Envelope
            'pRAEnv.SetCellSize esriRasterEnvValue, irp.MeanCellSize.x
            '        pRAEnv.SetCellSize esriRasterEnvValue, gCellSize
            pRAEnv.OutSpatialReference = irp.SpatialReference

            pgds = peop.Polygon(tmprasLayer.Raster, ipoly, True)


            '-- pSAOutRaster is a Raster Object from a Spatial operation
            pRasBC = pgds

            '-- Get the dataset from the first band
            pOutDS = pRasBC.Item(0).RasterDataset

            '-- QI the dataset for ITemporaryDataset
            pTemp = pOutDS

            '-- Release the raster before making the dataset permanent
            pRasBC = Nothing
            pgds = Nothing


            pds = pTemp.MakePermanentAs(InputDataDictionary(strLayerName), pRWS, "GRID")

        End If

        GoTo CleanUp

ShowError:
        MsgBox("Clipping input datasets error. Error :" & Err.Description)

CleanUp:
        pgds = Nothing
        '    Set pExtractedRaster = Nothing
        tmprasLayer = Nothing
        ' Set pRasterBandCollection = Nothing
    End Sub

    Public Function GetGroupValue(ByRef strLayerName As String) As IRaster
        Dim pSoilTable As iTable
        pSoilTable = GetDataTable(InputDataDictionary("SoilProperty"))

        Dim pLayerName As IRasterLayer
        pLayerName = GetInputRasterLayer(strLayerName)

        Dim pCoverRaster As IRaster
        pCoverRaster = pLayerName.Raster

        If Not (pCoverRaster Is Nothing) Then
            pCoverRaster = ReMapTableForSoil(pSoilTable, "GROUPVALUE", "VALUE", False)
            GetGroupValue = pCoverRaster
        Else
        End If
    End Function

    '''Merge soil info data
    Public Function MergeSoilInfo(ByRef strLayerName As String) As Object
        Dim pSoilTable As iTable
        pSoilTable = GetDataTable(InputDataDictionary("SoilProperty"))

        Dim pLayerName As IRasterLayer
        pLayerName = GetInputRasterLayer(strLayerName)

        Dim pCoverRaster As IRaster
        pCoverRaster = pLayerName.Raster

        Dim pchkRaster As IRaster

        If Not (pCoverRaster Is Nothing) Then

            If ComputeSedimentFlag Then
                pchkRaster = OpenRasterDatasetFromDisk("SEFactor")
                If (pchkRaster Is Nothing) Then
                    pCoverRaster = ReMapTableForSoil(pSoilTable, "KFACT", "VALUE", False)
                    'Write GETCover raster to disk
                End If

            End If

            pchkRaster = OpenRasterDatasetFromDisk("AWC")
            If (pchkRaster Is Nothing) Then
                pCoverRaster = ReMapTableForSoil(pSoilTable, "AWC", "VALUE", False)
                'Write GETCover raster to disk
            End If

            pchkRaster = OpenRasterDatasetFromDisk("BulkDensity")
            If (pchkRaster Is Nothing) Then
                pCoverRaster = ReMapTableForSoil(pSoilTable, "BD", "VALUE", False)
                'Write GETCover raster to disk
            End If

            pchkRaster = OpenRasterDatasetFromDisk("PERM")
            If (pchkRaster Is Nothing) Then
                pCoverRaster = ReMapTableForSoil(pSoilTable, "PERM", "VALUE", False)
                'Write GETCover raster to disk
            End If

            pchkRaster = OpenRasterDatasetFromDisk("CLAYPERCENT")
            If (pchkRaster Is Nothing) Then
                pCoverRaster = ReMapTableForSoil(pSoilTable, "CLAYPERC", "VALUE", False)
                'Write GETCover raster to disk
            End If

        End If

CleanUp:
        pCoverRaster = Nothing

    End Function

    'Create KCPLS layer
    Public Sub CreateKCPLSRaster()
        Dim pKRaster As IRaster
        Dim pCRaster As IRaster
        Dim pPRaster As IRaster
        Dim pLSRaster As IRaster
        Dim pKCPLSRaster As IRaster


        pKCPLSRaster = OpenRasterDatasetFromDisk("KCPLS")
        If (pKCPLSRaster Is Nothing) Then
            pKRaster = OpenRasterDatasetFromDisk("SEfactor")
            pCRaster = OpenRasterDatasetFromDisk("CMfactor")
            pPRaster = OpenRasterDatasetFromDisk("ECfactor")
            pLSRaster = OpenRasterDatasetFromDisk("LSfactor")

            If (pKRaster Is Nothing) Then
                MsgBox("SEfactor (KFACT) Raster Layer Missing !! ...", MsgBoxStyle.Exclamation)
                Exit Sub
            End If

            If (pCRaster Is Nothing) Then
                MsgBox("CMfactor Raster Layer Missing !! ...", MsgBoxStyle.Exclamation)
                Exit Sub
            End If

            If (pPRaster Is Nothing) Then
                MsgBox("ECfactor Raster Layer Missing !! ...", MsgBoxStyle.Exclamation)
                Exit Sub
            End If

            If (pLSRaster Is Nothing) Then
                MsgBox("LS factor Raster Layer Missing !! ...", MsgBoxStyle.Exclamation)
                Exit Sub
            End If

            gAlgebraOp.BindRaster(pKRaster, "K")
            gAlgebraOp.BindRaster(pCRaster, "C")
            gAlgebraOp.BindRaster(pPRaster, "P")
            gAlgebraOp.BindRaster(pLSRaster, "LS")

            pKCPLSRaster = gAlgebraOp.Execute("[K] * [C] * [P] * [LS]")

            gAlgebraOp.UnbindRaster("K")
            gAlgebraOp.UnbindRaster("C")
            gAlgebraOp.UnbindRaster("P")
            gAlgebraOp.UnbindRaster("LS")

        End If

CleanUp:
        pKRaster = Nothing
        pCRaster = Nothing
        pPRaster = Nothing
        pLSRaster = Nothing
        pKCPLSRaster = Nothing

    End Sub



    '****** Subroutine to read in the SoilTable table and create a remap table from it
    Public Function ReMapTableForSoil(ByRef pTable As iTable, ByRef field1 As String, ByRef field2 As String, ByRef BoolInteger As Boolean) As IRaster
        On Error GoTo ErrorHandler

        Dim pSoilRaster As IRaster
        'Set pSoilRaster = OpenRasterDatasetFromDisk("SoilMap")
        ''   Set pSoilRaster = OpenRasterDatasetFromDisk(InputDataDictionary.Item("SoilMap"))

        Dim pSoilRLayer As IRasterLayer
        '''Set pSoilRLayer = GetInputRasterLayer("Soilmap")
        pSoilRLayer = GetInputRasterLayer(InputDataDictionary.Item("SoilMap"))
        pSoilRaster = pSoilRLayer.Raster

        ''Set pSoilRLayer = pSoilRaster

        Dim pDEMRLayer As IRasterLayer
        pDEMRLayer = GetInputRasterLayer("DEM")

        Dim pRasterPropsDEM As IRasterProps
        pRasterPropsDEM = pDEMRLayer.Raster

        ' Get the dataset of the newly created soil grid
        Dim pInputDataset As IGeoDataset
        pInputDataset = pSoilRaster '''pSoilRLayer.Raster

        Dim pFldLookup As Integer
        pFldLookup = pTable.FindField("LOOKUP")

        Dim pFieldEdit As IFieldEdit
        If (pFldLookup = -1) Then 'Add the field
            pFieldEdit = New Field
            With pFieldEdit
                .name = "LOOKUP"
                .IsNullable = True
                '          .Type = esriFieldTypeDouble
            End With
            pTable.AddField(pFieldEdit)
            pFldLookup = pTable.FindField("LOOKUP")
        End If


        'Define the in and out variables
        Dim field2Index As Integer
        'Get the field1 index
        field2Index = pTable.FindField(field1)
        If (field2Index = -1) Then
            MsgBox("LuLookupTable table should have a " & field2 & " field", MsgBoxStyle.Exclamation)
            Exit Function
        End If

        Dim pFieldScale As IField
        pFieldScale = pTable.Fields.Field(pTable.FindField(field1))
        pFieldEdit = pFieldScale
        Dim pScale As Integer
        pScale = 10 ^ pFieldScale.Scale
        Dim pCursor As ICursor
        Dim pRow As iRow
        pCursor = pTable.Search(Nothing, True)
        pRow = pCursor.NextRow
        Do While Not pRow Is Nothing
            pRow.Value(pFldLookup) = pRow.Value(field2Index) * pScale
            pRow.Store()
            pRow = pCursor.NextRow
        Loop

        ' Create the RasterReclassOp object
        Dim pReclassOp As IReclassOp
        pReclassOp = New RasterReclassOp
        Dim pWS As IWorkspace
        Dim pWSF As IWorkspaceFactory
        pWSF = New RasterWorkspaceFactory
        pWS = pWSF.OpenFromFile(gMapTempFolder, 0)

        Dim penv As IRasterAnalysisEnvironment
        penv = pReclassOp
        penv.OutSpatialReference = pRasterPropsDEM.SpatialReference
        '    penv.SetExtent esriRasterEnvValue, pRasterPropsDEM.Extent
        '    penv.SetCellSize esriRasterEnvValue, pRasterPropsDEM.MeanCellSize.X
        penv.OutWorkspace = pWS
        penv.Mask = pDEMRLayer.Raster

        gAlgebraOp = New RasterMapAlgebraOp

        Dim pOutRaster As IRaster
        pOutRaster = pReclassOp.Reclass(pInputDataset, pTable, "VALUE", "VALUE", "LOOKUP", False)

        gAlgebraOp.BindRaster(pOutRaster, "OUT")
        If (BoolInteger = True) Then
            pOutRaster = gAlgebraOp.Execute("[OUT] / " & pScale)
        Else
            pOutRaster = gAlgebraOp.Execute("Float([OUT]) / " & CDbl(pScale))
        End If
        gAlgebraOp.UnbindRaster("OUT")

        '** Return the remap table
        ReMapTableForSoil = pOutRaster
        GoTo CleanUp
ErrorHandler:
        MsgBox("Error in RemapTableforSoil " & Err.Description)

CleanUp:
        '    Set pSoilRLayer = Nothing
        pSoilRaster = Nothing
        pDEMRLayer = Nothing
        pRasterPropsDEM = Nothing
        pWS = Nothing
        pWSF = Nothing
        penv = Nothing
        pInputDataset = Nothing
        pReclassOp = Nothing
        pOutRaster = Nothing
    End Function


    'Clip Rasters in Temp\Sub folder based on Subwatershed defined by user
    Public Sub CreateCalibrationdataNew()

        On Error GoTo ShowError

        Dim pDoc As IDocument
        pDoc = gApplication.Document
        Dim pTemplates As ITemplates
        pTemplates = gApplication.Templates
        Dim lTempCount As Integer
        lTempCount = pTemplates.Count

        Dim strDocPath As String
        strDocPath = pTemplates.Item(lTempCount - 1)
        pCurrProjectPath = Replace(strDocPath, pDoc.Title, "")
        pCurrProjectPath = Replace(pCurrProjectPath, ".mxd", "") 'For certain OS, ".mxd" is left
        Dim pFileSource As String
        pFileSource = pCurrProjectPath & Replace(pDoc.Title, ".mxd", "")
        pTemplates = Nothing
        pDoc = Nothing

        Dim fso As Scripting.FileSystemObject
        fso = CreateObject("Scripting.FileSystemObject")

        Dim gMapTempSub As String

        gMapTempSub = gMapTempFolder & "\SUB"

        If (fso.FolderExists(gMapTempSub)) Then
            fso.DeleteFolder(gMapTempSub, True)
        End If
        fso.CreateFolder(gMapTempSub)

        Dim strCalibrationDataFolderPath As String
        strCalibrationDataFolderPath = gMapTempSub

        Dim pRasterLayerSmallWS As IRasterLayer
        pRasterLayerSmallWS = GetInputRasterLayer("SubWatershed")

        If pRasterLayerSmallWS Is Nothing Then
            MsgBox("Not found Subwatershed layer.")
            GoTo CleanUp
        End If

        ClipInputDatasetsForCalibrationNew(pRasterLayerSmallWS.Raster, strCalibrationDataFolderPath) ' strCalibrationTempFolderPath

        fso = Nothing

        GoTo CleanUp

ShowError:
        MsgBox("CreateCalibrationData: " & Err.Description)

CleanUp:
        pRasterLayerSmallWS = Nothing
        CleanUpMemory()
    End Sub


    Public Sub ClipInputDatasetsForCalibrationNew(ByRef pRasterAssessment As IRaster, ByRef strCalibDataFolder As String) ' strCalibTempFolder As String
        On Error GoTo ShowError

        Dim fc, fso, fd, f As Object
        fso = CreateObject("Scripting.FileSystemObject")
        If fso.FolderExists(strCalibDataFolder) Then
            fd = fso.GetFolder(strCalibDataFolder)
            fc = fd.Files
            For Each f In fc
                MsgBox("Need an empty folder for outputing datasets for calibration. Folder " & strCalibDataFolder & " is not empty.")
                Exit Sub
            Next f
        Else
            fso.CreateFolder(strCalibDataFolder)
        End If

        Dim pRWSF As IWorkspaceFactory
        pRWSF = New RasterWorkspaceFactory
        Dim pRWS As IRasterWorkspace2
        pRWS = pRWSF.OpenFromFile(strCalibDataFolder, 0)

        Dim pEnvelope As IEnvelope
        'Set pEnvelope = GetRasterValidExtent(pRasterAssessment, pRWS)
        pEnvelope = GetRasterValidExtentNew(pRasterAssessment, pRWS)

        ' Create ConversionOp
        Dim pConversionOp As IConversionOp
        pConversionOp = New RasterConversionOp




        If InputDataDictionary("chkGrid") = 1 Then
            If InputDataDictionary("HgDryGrid") <> "" Then
            End If
            If InputDataDictionary("HgWetGrid") <> "" Then
            End If
        End If


        If InputDataDictionary("chkTime") = 1 Then
        End If








        'Alvi silenced on Jan 16, 2006

        If InputDataDictionary("PointSources") <> "" Then
        End If


        If InputDataDictionary("optInputSoilMoisture") = True And InputDataDictionary("InitialSoilMoisture") <> "" Then
            'ClipAndSaveRasterLayers "InitialSoilMoisture", pEnvelope, pRasterAssessment, pRWS, False, InputDataDictionary("InitialSoilMoisture")
        End If



        If InputDataDictionary("cbxSediment") = 1 Then
        End If

        If InputDataDictionary("cbxMercury") = 1 Then
        End If

        If InputDataDictionary("optionSoilHgGrid") = True And InputDataDictionary("InitialSoilHg") <> "" Then
            'ClipAndSaveRasterLayers "InitialSoilMoisture", pEnvelope, pRasterAssessment, pRWS, False, InputDataDictionary("InitialSoilMoisture")
        End If


        GoTo CleanUp

ShowError:
        MsgBox("ClipInputDatasetsForCalibrationNew. Error :" & Err.Description)

CleanUp:
        pRWSF = Nothing
        pRWS = Nothing
        pEnvelope = Nothing
        pConversionOp = Nothing
    End Sub

    ''' Clip raster based on polygon envelope
    Public Sub ClipAndSaveRasterLayersNew(ByRef strLayerName As String, ByRef penv As IEnvelope, ByRef pRWS As IRasterWorkspace2, ByRef bOptional As Boolean, Optional ByRef layerName As String = "")
        On Error GoTo ShowError

        'initialize raster objects
        Dim peop As IExtractionOp
        peop = New RasterExtractionOp

        Dim pRAEnv As IRasterAnalysisEnvironment
        pRAEnv = peop

        Dim tmpraster As IRaster

        Dim tmprasLayer As IRasterLayer
        If layerName = "" Then
            tmprasLayer = GetInputRasterLayer(strLayerName)
            tmpraster = tmprasLayer.Raster
        Else
            tmpraster = OpenRasterDatasetFromDisk(layerName)
        End If

        Dim pds As IDataset
        Dim pTemp As ITemporaryDataset
        Dim pOutDS As IRasterDataset
        Dim pRasBC As IRasterBandCollection
        Dim pgds As IGeoDataset
        Dim irp As IRasterProps
        If tmpraster Is Nothing Then
            Err.Description = "Invalid input data for " & strLayerName
            GoTo ShowError
        Else
            irp = tmpraster

            '        pRAEnv.SetExtent esriRasterEnvValue, penv
            'pRAEnv.SetCellSize esriRasterEnvValue, irp.MeanCellSize.x
            '        pRAEnv.SetCellSize esriRasterEnvValue, gCellSize
            pRAEnv.OutSpatialReference = irp.SpatialReference

            '        Set pgds = peop.Polygon(tmprasLayer.Raster, ipoly, True)

            pgds = peop.Rectangle(tmpraster, penv, True)

            '-- pSAOutRaster is a Raster Object from a Spatial operation
            pRasBC = pgds

            '-- Get the dataset from the first band
            pOutDS = pRasBC.Item(0).RasterDataset

            '-- QI the dataset for ITemporaryDataset
            pTemp = pOutDS

            '-- Release the raster before making the dataset permanent
            pRasBC = Nothing
            pgds = Nothing

            If (layerName = "") Then
                pds = pTemp.MakePermanentAs(InputDataDictionary(strLayerName), pRWS, "GRID")
            Else
                pds = pTemp.MakePermanentAs(layerName, pRWS, "GRID")
            End If

        End If

        GoTo CleanUp

ShowError:
        MsgBox("Clipping input datasets error. Error :" & Err.Description)

CleanUp:
        pgds = Nothing
        tmprasLayer = Nothing
    End Sub

    Public Function GetRasterValidExtentNew(ByRef pRaster As IRaster, ByRef pRWS As IRasterWorkspace2) As IEnvelope
        On Error GoTo ShowError

        Dim pRasterProps As IRasterProps
        pRasterProps = pRaster
        pRasterProps.PixelType = GetVBSupportedPixelType(pRasterProps.PixelType)

        ' Use a local MapAlgebraOp to prevent the conflict with the global one
        ''    Dim pMapAlgebraOp As IMapAlgebraOp
        ''    Set pMapAlgebraOp = New RasterMapAlgebraOp

        ''    Dim pRAEnv As IRasterAnalysisEnvironment
        ''    Set pRAEnv = pMapAlgebraOp
        ''
        ''    pRAEnv.SetExtent esriRasterEnvValue, pRasterProps.Extent
        ''    pRAEnv.SetCellSize esriRasterEnvValue, gCellSize    ' esriRasterEnvValue, pRasterProps.MeanCellSize.x
        ''    Set pRAEnv.OutSpatialReference = pRasterProps.SpatialReference
        ''    Set pRAEnv.OutWorkspace = pRWS

        ''    Dim pGeoDataset As IGeoDataset
        ''
        ''    Dim pRaster01 As IRaster
        ''    Set pRaster01 = pRaster

        ''    Dim pRasterBandCol As IRasterBandCollection
        ''    Set pRasterBandCol = pRaster01
        ''
        ''    Dim pRasterBand As IRasterBand
        ''    Set pRasterBand = pRasterBandCol.Item(0)
        ''
        ''    Dim pRawPixels As IRawPixels
        ''    Set pRawPixels = pRasterBand
        ''


        Dim pSize As IPnt
        pSize = New DblPnt

        Dim pOrigin As IPnt
        pOrigin = New DblPnt

        ''    pSize.SetCoords pRasterProps.Width, pRasterProps.Height
        Dim iCol As Integer
        Dim iRow As Integer
        Dim minCol As Integer
        Dim maxCol As Integer
        Dim minRow As Integer
        Dim maxRow As Integer

        minCol = pRasterProps.Width
        maxCol = -1
        minRow = pRasterProps.Height
        maxRow = -1


        Dim pPixelBlock As IPixelBlock3
        Dim vPixelData As Object

        Dim lBlockSize As Integer
        Dim lTmpBlockSize As Integer
        Dim lTileNum As Integer
        Dim lTile As Integer
        Dim lStartRow As Integer
        Dim lStartCol As Integer

LeftMost:
        ' Get leftmost valid cell

        'vertical tiles
        lBlockSize = CInt(1048576 / pRasterProps.Height)
        If lBlockSize < 1 Then lBlockSize = 1
        lTileNum = CInt(pRasterProps.Width / lBlockSize)
        If lTileNum * lBlockSize < pRasterProps.Width Then lTileNum = lTileNum + 1

        For lTile = 0 To lTileNum - 1
            lStartCol = lTile * lBlockSize
            If lStartCol + lBlockSize > pRasterProps.Width Then
                lTmpBlockSize = pRasterProps.Width - lStartCol
            Else
                lTmpBlockSize = lBlockSize
            End If
            pOrigin.SetCoords(lStartCol, 0)
            pSize.SetCoords(lTmpBlockSize, pRasterProps.Height)

            pPixelBlock = pRaster.CreatePixelBlock(pSize)
            pRaster.Read(pOrigin, pPixelBlock)
            vPixelData = pPixelBlock.PixelData(0)

            For iCol = 0 To lTmpBlockSize - 1
                For iRow = 0 To pRasterProps.Height - 1
                    If pPixelBlock.GetNoDataMaskVal(0, iCol, iRow) = 1 Then
                        'minCol = iCol
                        minCol = lStartCol + iCol
                        GoTo RightMost
                    End If
                Next
            Next
        Next
RightMost:


        ' Get rightmost valid cell
        For lTile = lTileNum - 1 To 0 Step -1
            lStartCol = lTile * lBlockSize
            If lStartCol + lBlockSize > pRasterProps.Width Then
                lTmpBlockSize = pRasterProps.Width - lStartCol
            Else
                lTmpBlockSize = lBlockSize
            End If
            pOrigin.SetCoords(lStartCol, 0)
            pSize.SetCoords(lTmpBlockSize, pRasterProps.Height)

            pPixelBlock = pRaster.CreatePixelBlock(pSize)
            pRaster.Read(pOrigin, pPixelBlock)
            vPixelData = pPixelBlock.PixelData(0)

            For iCol = lTmpBlockSize - 1 To 0 Step -1
                For iRow = 0 To pRasterProps.Height - 1
                    If pPixelBlock.GetNoDataMaskVal(0, iCol, iRow) = 1 Then
                        'maxCol = iCol
                        maxCol = lStartCol + iCol
                        GoTo BottomMost
                    End If
                Next
            Next
        Next
BottomMost:
        'Horizontal tiles
        lBlockSize = CInt(1048576 / pRasterProps.Width)
        If lBlockSize < 1 Then lBlockSize = 1
        lTileNum = CInt(pRasterProps.Height / lBlockSize)
        If lTileNum * lBlockSize < pRasterProps.Height Then lTileNum = lTileNum + 1

        For lTile = 0 To lTileNum - 1
            lStartRow = lTile * lBlockSize
            If lStartRow + lBlockSize > pRasterProps.Height Then
                lTmpBlockSize = pRasterProps.Height - lStartRow
            Else
                lTmpBlockSize = lBlockSize
            End If
            pOrigin.SetCoords(0, lStartRow)
            pSize.SetCoords(pRasterProps.Width, lTmpBlockSize)

            pPixelBlock = pRaster.CreatePixelBlock(pSize)
            pRaster.Read(pOrigin, pPixelBlock)
            vPixelData = pPixelBlock.PixelData(0)

            ' Get topmost valid cell
            For iRow = 0 To lTmpBlockSize - 1 ' pRasterProps.Height - 1
                For iCol = 0 To pRasterProps.Width - 1
                    If pPixelBlock.GetNoDataMaskVal(0, iCol, iRow) = 1 Then
                        'minRow = iRow
                        minRow = lStartRow + iRow
                        GoTo TopMost
                    End If
                Next
            Next
        Next

TopMost:


        For lTile = lTileNum - 1 To 0 Step -1
            lStartRow = lTile * lBlockSize
            If lStartRow + lBlockSize > pRasterProps.Height Then
                lTmpBlockSize = pRasterProps.Height - lStartRow
            Else
                lTmpBlockSize = lBlockSize
            End If
            pOrigin.SetCoords(0, lStartRow)
            pSize.SetCoords(pRasterProps.Width, lTmpBlockSize)

            pPixelBlock = pRaster.CreatePixelBlock(pSize)
            pRaster.Read(pOrigin, pPixelBlock)
            vPixelData = pPixelBlock.PixelData(0)

            ' Get bottommost valid cell
            For iRow = lTmpBlockSize - 1 To 0 Step -1
                For iCol = 0 To pRasterProps.Width - 1
                    If pPixelBlock.GetNoDataMaskVal(0, iCol, iRow) = 1 Then
                        'maxRow = iRow
                        maxRow = lStartRow + iRow
                        GoTo SetEnvelope
                    End If
                Next
            Next
        Next

SetEnvelope:
        ' test any edge to see if some valid cell is found
        If maxCol = -1 Then
            MsgBox("No valid cells in the source raster.")
            GoTo CleanUp
        End If

        Dim pEnvelope As IEnvelope
        pEnvelope = New Envelope

        pEnvelope.XMin = minCol * pRasterProps.MeanCellSize.X + pRasterProps.Extent.XMin
        pEnvelope.XMax = (maxCol + 1) * pRasterProps.MeanCellSize.X + pRasterProps.Extent.XMin 'pRasterProps.Extent.XMax - (pRasterProps.Width - 1 - maxCol) * pRasterProps.MeanCellSize.x
        pEnvelope.YMin = pRasterProps.Extent.YMax - (1 + maxRow) * pRasterProps.MeanCellSize.Y '(pRasterProps.Height - 1 - maxRow) * pRasterProps.MeanCellSize.y + pRasterProps.Extent.YMin
        pEnvelope.YMax = pRasterProps.Extent.YMax - minRow * pRasterProps.MeanCellSize.Y

        'MsgBox "from valid extent -> minx = " & pEnvelope.XMin & " maxx = " & pEnvelope.XMax & " miny = " & pEnvelope.YMin & " maxy = " & pEnvelope.YMax

        GetRasterValidExtentNew = pEnvelope
        GoTo CleanUp

ShowError:
        MsgBox("Error: " & Err.Description)

CleanUp:
        pRasterProps = Nothing
        'Set pMapAlgebraOp = Nothing
        'Set pRAEnv = Nothing
        'Set pRaster01 = Nothing
        'Set pRasterBandCol = Nothing
        'Set pRasterBand = Nothing
        'Set pRawPixels = Nothing
        pSize = Nothing
        pPixelBlock = Nothing
        pOrigin = Nothing
        vPixelData = Nothing
        pEnvelope = Nothing
    End Function
End Module