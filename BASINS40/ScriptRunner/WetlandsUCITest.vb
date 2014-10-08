Imports System.Collections.ObjectModel
Imports MapWindow.Interfaces
Imports atcMwGisUtility
Imports atcUCI
Imports atcUtility
Imports atcSegmentation
Imports MapWinUtility

Module WetlandsUCITest

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Dim lFolder As String = "D:\BASINS41\data\02060006-3\"
        ChDriveDir(lFolder)

        Dim lMsg As New atcUCI.HspfMsg
        lMsg.Open("hspfmsg.wdm")


        '1.	Enhance the BASINS/HSPF model setup tool user interface to allow selection of wetlands modeling and specification 
        '   of additional DEM and wetlands layers.
        Dim lDoWetlands As Boolean = True
        Dim lDEMFileName As String = "D:\BASINS41\data\02060006-3\dem\02060006demg.tif"
        Dim lWetlandsFileName As String = "D:\BASINS41\data\02060006-3\HU8_02060006_watershed\HU8_02060006_Wetlands.NAD83 - UTM zone 18N.shp"
        Dim lStreamsFileName As String = "D:\BASINS41\Predefined Delineations\West Branch\wb_strms.shp"

        '2.	Add functionality to create a Flow Direction grid based on the input DEM.  This function will leverage existing 
        '   functionality from MapWinGeoProc, the GIS processing library which underlies the BASINS GIS.  

        'set nodata value properly
        GisUtil.GridSetNoData(lDEMFileName, -100.0)

        'First Compute the Pit Fill Grid (Corrected DEM)
        Dim lPitFillDEMLayerName As String = "Corrected DEM"
        Dim lPitFillDEMLayerIndex As Integer = 0
        Dim lPitFillDEMFileName As String = ""
        If GisUtil.IsLayer(lPitFillDEMLayerName) Then
            lPitFillDEMLayerIndex = GisUtil.LayerIndex(lPitFillDEMLayerName)
            lPitFillDEMFileName = GisUtil.LayerFileName(lPitFillDEMLayerIndex)
        Else
            Logger.Status("Step 1 of X: Computing Pit Fill")
            lPitFillDEMFileName = FilenameNoExt(lDEMFileName) & "PitFill.tif"
            MapWinGeoProc.Hydrology.Fill(lDEMFileName, lPitFillDEMFileName, False)
            GisUtil.AddLayer(lPitFillDEMFileName, lPitFillDEMLayerName)
            GisUtil.SaveProject(GisUtil.ProjectFileName)
        End If

        'Flow Direction Grid
        Dim lFlowDirGridLayerName As String = "Flow Direction Grid"
        Dim lFlowDirGridLayerIndex As Integer = 0
        Dim lFlowDirGridFileName As String = ""
        Dim lSlopeGridFileName As String = ""
        If GisUtil.IsLayer(lFlowDirGridLayerName) Then
            lFlowDirGridLayerIndex = GisUtil.LayerIndex(lFlowDirGridLayerName)
            lFlowDirGridFileName = GisUtil.LayerFileName(lFlowDirGridLayerIndex)
        Else
            Logger.Status("Step 2 of X: Computing Flow Direction")
            lFlowDirGridFileName = FilenameNoExt(lDEMFileName) & "FlowDir.tif"
            lSlopeGridFileName = FilenameNoExt(lDEMFileName) & "Slope.tif"
            Dim lRet As Integer = MapWinGeoProc.Hydrology.D8(lPitFillDEMFileName, lFlowDirGridFileName, lSlopeGridFileName, 8, False, Nothing)
            If Not GisUtil.IsLayer(lFlowDirGridLayerName) Then
                GisUtil.AddLayer(lFlowDirGridFileName, lFlowDirGridLayerName)
            End If
        End If

        '      this is the assumed flow dir coding from taudem:
        '      4 3 2
        '      5   1
        '      6 7 8

        '3.	Write and incorporate an algorithm to create a 'ToWetlands' grid, using the Flow Direction grid, wetlands GIS 
        '   layer, and stream reach shapefile.  This algorithm will use a pixel-by-pixel approach to determine if each pixel 
        '   (grid cell) drains to a wetland or to a stream reach (without first passing through a wetland). As part of this 
        '   step, the wetlands GIS layer will be rasterized if necessary.  

        Dim lToWetlandsGridLayerName As String = "ToWetlands"
        Dim lToWetlandsGridLayerIndex As Integer = 0
        Dim lToWetlandsGridFileName As String = ""
        Dim lInputProjectionFileName As String = FilenameSetExt(lDEMFileName, "prj")
        If GisUtil.IsLayer(lToWetlandsGridLayerName) Then
            lToWetlandsGridLayerIndex = GisUtil.LayerIndex(lToWetlandsGridLayerName)
            lToWetlandsGridFileName = GisUtil.LayerFileName(lToWetlandsGridLayerIndex)
        Else
            Logger.Status("Step 3 of X: Computing ToWetlands Grid")
            lToWetlandsGridFileName = FilenameNoExt(lDEMFileName) & "ToWetlands.tif"
            GisUtil.ComputeToWetlands(lFlowDirGridFileName, lWetlandsFileName, lStreamsFileName, lToWetlandsGridFileName)
            If FileExists(lInputProjectionFileName) Then
                FileCopy(lInputProjectionFileName, FilenameSetExt(lToWetlandsGridFileName, "prj"))
            End If
            GisUtil.AddLayer(lToWetlandsGridFileName, lToWetlandsGridLayerName)
            GisUtil.SaveProject(GisUtil.ProjectFileName)
        End If

        '4. Using(the) 'ToWetlands' grid, enhance the BASINS/HSPF model setup tool workflow to determine the area of each 
        '   land use category contributing to each wetland reach as well as to each stream reach.  

        '5.	Enhance the section of the BASINS/HSPF model setup code responsible for creating the HSPF User Control Input 
        '   (UCI) file, so that it creates both a 'wetlands' RCHRES and a stream RCHRES within each modeled subbasin.

        '6.	Making appropriate assumptions about 'wetlands' RCHRES channel dimensions, create FTABLE for the 'wetlands' RCHRES 
        '   operations, and apply appropriate HSPF parameter values for the 'wetlands' RCHRES operations.  Note that the outlets 
        '   from the new wetlands RCHRES operations will be assumed to connect to the corresponding river RCHRES.  It will remain 
        '   up to the user to modify the connectivity and parameterization if the default assumptions are not appropriate. 




        'below is from old create UCI test script
        'Dim lDataSources As New Collection(Of atcData.atcTimeseriesSource)
        'Dim lDataSource As New atcWDM.atcDataSourceWDM
        ''lDataSource.Open("test.wdm")
        'lDataSource.Open("sdmproject.wdm")
        'lDataSources.Add(lDataSource)
        'lDataSource = New atcWDM.atcDataSourceWDM
        ''lDataSource.Open("md.wdm")
        'lDataSource.Open("met.wdm")
        'lDataSources.Add(lDataSource)

        'Dim lStarterUciName As String = "starter.uci"
        'Dim lPollutantListFileName As String = "Poltnt_2.prn"

        'Dim lMetBaseDsn As Integer = 11
        'Dim lMetWdmId As String = "WDM2"
        'Dim lWQConstituents() As String = {}

        'Dim lWatershedName As String = "UCICreation"
        'lWatershedName = "SDMProject"
        'Dim lWatershed As New Watershed
        'If lWatershed.Open(lWatershedName) = 0 Then  'everything read okay, continue
        '    Dim lHspfUci As New atcUCI.HspfUci
        '    lHspfUci.Msg = lMsg
        '    lHspfUci.CreateUciFromBASINS(lWatershed, _
        '                                 lDataSources, _
        '                                 lStarterUciName, _
        '                                 lWQConstituents, _
        '                                 lPollutantListFileName)
        '    lHspfUci.Save()
        'End If
    End Sub
End Module
