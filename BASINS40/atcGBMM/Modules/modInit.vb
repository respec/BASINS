Module modInit

    Public gApplicationPath As String
    Public gMapTempFolder As String
    Public gMapInputFolder As String
    Public gMapDataFolder As String
    Public gMapOutputFolder As String
    Public gMapOutputRunFolder As String
    Public gModelWASPOutputFolder As String
    Public gModelWASPOutputRunFolder As String
    Public gModelWhAEMFolder As String
    Public gModelWhAEMRunFolder As String
    'Public gOutputSpatialReference As ISpatialReference

    'Public gApplication As IApplication
    'Public gMxDoc As IMxDocument
    'Public gMap As IMap
    Public gDEMRaster As MapWinGIS.Grid
    Public gCellSize As Double

    'Public gReclassOp As IReclassOp
    'Public gNeighborhoodOp As INeighborhoodOp
    'Public gAlgebraOp As IMapAlgebraOp
    'Public gHydrologyOp As IHydrologyOp

    '    Public UsPointList() As Short
    '    Public UsPointTimeLag() As Short
    '    Public Vinitial() As Double
    '    Public Sinitial() As Double
    '    Public Minitial() As Double
    '    Public pSubWaterHourlyRunoff() As Double
    '    Public pSubWaterLandTypeRunoff() As Double
    '    Public pSubWaterHourlySediment() As Double
    '    Public pSubWaterLandTypeSediment() As Double
    '    Public pSubWaterHourlyMercury() As Double
    '    Public pSubWaterLandTypeMercury() As Double
    '    'Public pEvaporationDict As Scripting.Dictionary
    '    'Public pLakeSurfaceAreaDict As Scripting.Dictionary
    '    'Public pSubWaterSurfaceAreaDict As Scripting.Dictionary
    '    'Public pRunoffPointSourceDict As Scripting.Dictionary
    '    'Public pSedimentPointSourceDict As Scripting.Dictionary
    '    'Public pMercuryPointSourceDict As Scripting.Dictionary

    '    Public RoutingCollection As Collection
    '    'Public pAssessPointTypeDictionary As Scripting.Dictionary
    '    'Public pAssessPointLakeDepthDictionary As Scripting.Dictionary
    '    'Public pAssessPointWaterDepthDictionary As Scripting.Dictionary
    '    'Public pAssessPointInitialSedConcDictionary As Scripting.Dictionary
    '    'Public pAssessPointInitialHgBenthicDictionary As Scripting.Dictionary
    '    'Public pAssessPointInitialHgWaterDictionary As Scripting.Dictionary
    '    'Public DownstreamPointDictionary As Scripting.Dictionary
    '    'Public pAssessPointTravelTimeDictionary As Scripting.Dictionary

    '    'Public pLUNameDict As Scripting.Dictionary
    '    'Public pLUIndexDict As Scripting.Dictionary

    '    Public Sub DefineMapFrame()
    '        gMxDoc = gApplication.Document
    '        gMap = gMxDoc.FocusMap
    '    End Sub

    Public Function Initialize() As Boolean
        Try
            Dim pDEMRasterLayer As MapWindow.Interfaces.Layer = GetInputLayer("DEM")
            gDEMRaster = pDEMRasterLayer.GetGridObject

            'Get the raster cell size
            gCellSize = (gDEMRaster.Header.dX + gDEMRaster.Header.dY) / 2

            'don't know what this stuff is for!!!!

            'Dim pWSF As IWorkspaceFactory
            'pWSF = New RasterWorkspaceFactory
            'Dim pRWS As IRasterWorkspace2
            'pRWS = pWSF.OpenFromFile(gMapTempFolder, 0)
            'Dim pRAEnv As IRasterAnalysisEnvironment

            '' Create the global gReclassOp object
            'gReclassOp = New RasterReclassOp
            'pRAEnv = gReclassOp
            ''    pRAEnv.SetExtent esriRasterEnvValue, pDEMRasterProps.Extent
            ''    pRAEnv.SetCellSize esriRasterEnvValue, pDEMRasterProps.MeanCellSize.X
            'pRAEnv.OutSpatialReference = pDEMRasterProps.SpatialReference
            'pRAEnv.OutWorkspace = pRWS
            'pRAEnv.Mask = gDEMRaster

            '' Create the global gNeighborhoodOp object
            'gNeighborhoodOp = New RasterNeighborhoodOp
            'pRAEnv = gNeighborhoodOp
            ''    pRAEnv.SetExtent esriRasterEnvValue, pDEMRasterProps.Extent
            ''    pRAEnv.SetCellSize esriRasterEnvValue, pDEMRasterProps.MeanCellSize.X
            'pRAEnv.OutSpatialReference = pDEMRasterProps.SpatialReference
            'pRAEnv.OutWorkspace = pRWS
            'pRAEnv.Mask = gDEMRaster

            '' Create the global gAlgebraOp object
            'gAlgebraOp = New RasterMapAlgebraOp
            'pRAEnv = gAlgebraOp
            ''    pRAEnv.SetExtent esriRasterEnvValue, pDEMRasterProps.Extent
            ''    pRAEnv.SetCellSize esriRasterEnvValue, pDEMRasterProps.MeanCellSize.X
            'pRAEnv.OutSpatialReference = pDEMRasterProps.SpatialReference
            'pRAEnv.OutWorkspace = pRWS
            'pRAEnv.Mask = gDEMRaster

            '' Create the global gHydrologyOp object
            'gHydrologyOp = New RasterHydrologyOp
            'pRAEnv = gHydrologyOp
            ''    pRAEnv.SetExtent esriRasterEnvValue, pDEMRasterProps.Extent
            ''    pRAEnv.SetCellSize esriRasterEnvValue, pDEMRasterProps.MeanCellSize.X
            'pRAEnv.OutSpatialReference = pDEMRasterProps.SpatialReference
            'pRAEnv.OutWorkspace = pRWS
            'pRAEnv.Mask = gDEMRaster
            Return True
        Catch ex As Exception
            ErrorMsg(, ex)
            Return False
        End Try
    End Function

    Public Sub InitializeAlgebraOperator()
        'gAlgebraOp = Nothing

        'Dim pWSF As IWorkspaceFactory
        'pWSF = New RasterWorkspaceFactory
        'Dim pRWS As IRasterWorkspace2
        'pRWS = pWSF.OpenFromFile(gMapTempFolder, 0)
        'Dim pRAEnv As IRasterAnalysisEnvironment

        'Dim pDEMRasterProps As IRasterProps
        'pDEMRasterProps = gDEMRaster

        '' Create the global gAlgebraOp object
        'gAlgebraOp = New RasterMapAlgebraOp
        'pRAEnv = gAlgebraOp
        ''    pRAEnv.SetExtent esriRasterEnvValue, pDEMRasterProps.Extent
        ''    pRAEnv.SetCellSize esriRasterEnvValue, pDEMRasterProps.MeanCellSize.X
        'pRAEnv.OutSpatialReference = pDEMRasterProps.SpatialReference
        'pRAEnv.OutWorkspace = pRWS
        'pRAEnv.Mask = gDEMRaster
    End Sub

    Public Sub CleanUpMemory()
        gDEMRaster = Nothing
    End Sub

    Public Sub DefineApplicationPath()
        'not sure if these are global input data paths that don't change or project-specific
        'for now, assign all to data folder; otherwise would get from plugins folder
        gApplicationPath = IO.Path.GetDirectoryName(GisUtil.ProjectFileName) & "\GBMM"

        gMapTempFolder = gApplicationPath & "\TEMP"
        gMapInputFolder = gApplicationPath & "\INPUT"
        gMapDataFolder = gApplicationPath & "\DATA"
        gModelWASPOutputFolder = gMapInputFolder 'Redirected the WASP output to INPUT folder

        '* Create each folder if not present
        If Not My.Computer.FileSystem.DirectoryExists(gApplicationPath) Then
            My.Computer.FileSystem.CreateDirectory(gApplicationPath)
        End If
        If Not My.Computer.FileSystem.DirectoryExists(gMapTempFolder) Then
            My.Computer.FileSystem.CreateDirectory(gMapTempFolder)
        End If
        If Not My.Computer.FileSystem.DirectoryExists(gMapInputFolder) Then
            My.Computer.FileSystem.CreateDirectory(gMapInputFolder)
        End If
        If Not My.Computer.FileSystem.DirectoryExists(gMapDataFolder) Then
            My.Computer.FileSystem.CreateDirectory(gMapDataFolder)
        End If
    End Sub
End Module