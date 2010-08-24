Imports atcControls
Imports atcUtility
Imports atcManDelin
Imports atcMwGisUtility
Imports atcModelSegmentation
Imports atcModelSetup
Imports MapWindow.Interfaces
Imports MapWinUtility

Module BatchHSPF

    'General Specs
    Private pOutputPath As String = "C:\BASINS\modelout\"
    Private pInputPath As String = "C:\BASINS\data\02060006-16\"
    Private pBaseOutputName As String = "02060006-16"    'output uci will be named this with .uci extension

    'Subbasin Specs
    Private pSubbasinLayerName As String = "Subbasins"   'name of layer as displayed on legend, or file name
    Private pSubbasinFieldName As String = "SUBBASIN"    'field containing unique integer identifier
    Private pSubbasinSlopeName As String = "SLO1"        'field containing slope of each subbasin
    Private pSubbasinSegmentName As String = ""          'field containing name of model segment associated 
    '                                                       with each subbasin, blank indicates all subbasins 
    '                                                       are associated with a single model segment

    'Land Use Specs
    Private pLUType As Integer = 0   '(0 - USGS GIRAS Shape, 1 - NLCD grid, 2 - Other shape, 3 - Other grid)
    Private pLandUseClassFile As String = "C:\BASINS\etc\giras.dbf"   'name of file that indicates classification scheme
    '                                                                    eg 21-25 = urban, 41-45 = cropland, etc
    Private pLandUseLayerName As String = ""   'name of land use layer as displayed on legend, or file name,
    '                                             does not need to be set for GIRAS land use type
    Private pLandUseFieldName As String = ""   'field containing land use classification code,
    '                                             only used for 'other shapefile' land use type
    Private pLUInclude() As Integer = {}       'special array to specify land use codes to be included
    '                                             for every subbasin, even if none exists in that subbasin,
    '                                             used to enforce a numbering convention

    'Met Data Specs
    Private pMetWDM As String = pInputPath & "met\met.wdm"   'name of met wdm file

    'Stream Specs
    Private pStreamLayerName As String = "Streams"     'name of layer as displayed on legend, or file name
    Private pStreamFields() As String = {"SUBBASIN", "SUBBASINR", "LEN2", "SLO2", "WID2", "DEP2", "MINEL", "MAXEL", "SNAME"}
    '                                                   array of field names containing required data for each stream

    'Point Source Specs
    Private pOutletsLayerName As String = "<none>"     'name of outlets layer from legend, or file name
    Private pPointFieldName As String = "PCSID"        'name of field within outlets layer containing unique 
    '                                                     point source identifier (like pcs station id)
    Private pPointYear As String = "1999"              'year of pcs data to use
    Private pPSRCustom As Boolean = False              'flag indicating use custom point source data, else pcs
    Private pPSRCustomFile As String = ""              'if using custom point source data, pass file name
    Private pPSRCalculate As Boolean = False           'flag indicating if distance on stream is to be calculated,
    '                                                   not implemented at this time

    'Water Quality Constituents to Add
    Private pWQConstituents() As String = {"NH3+NH4", "NO3", "ORTHO P", "BOD", "SEDIMENT"}

    Public Sub BatchHSPF(ByVal aSubbasinLayerName As String, ByVal aStreamLayerName As String, _
                         ByVal aLandUseLayerName As String, ByVal aElevationLayerName As String, _
                         ByVal aMetWDM As String, ByVal aNewDataDir As String, ByVal aBaseOutputName As String)
        Logger.Dbg("Start")
        pInputPath = aNewDataDir
        pOutputPath = aNewDataDir
        pSubbasinLayerName = aSubbasinLayerName
        pStreamLayerName = aStreamLayerName
        pLandUseLayerName = aLandUseLayerName
        pBaseOutputName = aBaseOutputName
        pMetWDM = aMetWDM
        ChDriveDir(pOutputPath)  'change to the directory of the current project
        Logger.Dbg(" CurDir:" & CurDir())

        'assuming we've downloaded:
        '  NHDPlus
        '  NLCD 2001
        pLUType = 1
        pLandUseClassFile = FindFile("Land Use Reclassification File", "nlcd.dbf", "dbf")
        '  DEM
        Dim lElevationUnitsName As String = "Centimeters"
        '  Met Data

        'add layers if not already on the map
        If Not GisUtil.IsLayerByFileName(aSubbasinLayerName) Then
            GisUtil.AddLayer(pSubbasinLayerName, "Catchments")
        End If
        If Not GisUtil.IsLayerByFileName(pStreamLayerName) Then
            GisUtil.AddLayer(pStreamLayerName, "Flowlines")
        End If
        If Not GisUtil.IsLayerByFileName(pLandUseLayerName) Then
            GisUtil.AddLayer(pLandUseLayerName, "NLCD 2001 Landuse")
        End If
        If Not GisUtil.IsLayerByFileName(aElevationLayerName) Then
            GisUtil.AddLayer(aElevationLayerName, "Digital Elevation Model")
        End If

        'if subbasins layer does not have subbasin id and slope fields, calculate them
        If Not GisUtil.IsField(GisUtil.LayerIndex(pSubbasinLayerName), pSubbasinFieldName) Then
            ManDelinPlugIn.CalculateSubbasinParameters(pSubbasinLayerName, aElevationLayerName, lElevationUnitsName)
        End If

        'if streams layer does not have required fields, calculate them
        If Not GisUtil.IsField(GisUtil.LayerIndex(pStreamLayerName), pStreamFields(1)) Then
            'assign subbasin numbers to each reach segment
            Dim lMinField As Integer = 9999
            ManDelinPlugIn.CalculateReachSubbasinIds(pStreamLayerName, _
                                                     pSubbasinLayerName, _
                                                     lMinField)
            'add downstream subbasin ids
            ManDelinPlugIn.CalculateReachDownstreamSubbasinIds(pStreamLayerName, _
                                                               lMinField)
            'calculate required reach parameters
            ManDelinPlugIn.CalculateReachParameters(pStreamLayerName, _
                                                    pSubbasinLayerName, _
                                                    aElevationLayerName, _
                                                    lElevationUnitsName, _
                                                    lMinField)
        End If

        'for now assume that the met wdm only has one station in it
        'if subbasins layer does not have model segment id field, add it
        'pSubbasinSegmentName = "ModelSeg"
        'Dim lMetLayerName As String = "C:\BASINS\data\02060006-16\met\met.shp"
        'ModelSegmentationPlugIn.AssignMetStationsByProximity(pSubbasinLayerName, lMetLayerName, False)

        'get land use data ready
        Dim AtcGridPervious As New atcControls.atcGrid
        AtcGridPervious.Source = New atcControls.atcGridSource
        SetPerviousGrid(AtcGridPervious, pLandUseClassFile, pLUType, pLandUseLayerName, pLandUseFieldName)

        'get met data ready
        Dim pMetStations As New atcCollection
        Dim pMetBaseDsns As New atcCollection
        Dim pMetWdmNames As New atcCollection
        BuildListofMetStationNames(pMetWdmNames, pMetStations, pMetBaseDsns)

        Dim AtcGridMet As New atcControls.atcGrid
        AtcGridMet.Source = New atcControls.atcGridSource
        Dim pUniqueModelSegmentIds As New atcCollection
        Dim pUniqueModelSegmentNames As New atcCollection
        SetMetSegmentGrid(AtcGridMet, pMetStations, pUniqueModelSegmentNames, pUniqueModelSegmentIds, _
                          pSubbasinLayerName, pSubbasinFieldName, pSubbasinSegmentName)

        Dim lOutputPath As String = pOutputPath & "HSPF"

        'now start the processing
        If PreProcessChecking(lOutputPath, pBaseOutputName, "HSPF", pLUType, pMetStations.Count, _
                              pSubbasinLayerName, pLandUseLayerName) Then 'early checks OK
            Logger.Status("Preparing HSPF Setup")
            If SetupHSPF(AtcGridPervious, _
                         pMetStations, pMetBaseDsns, _
                         pUniqueModelSegmentNames, pUniqueModelSegmentIds, _
                         lOutputPath, pBaseOutputName, _
                         pSubbasinLayerName, pSubbasinFieldName, pSubbasinSlopeName, _
                         pStreamLayerName, pStreamFields, _
                         pLUType, pLandUseLayerName, pLUInclude, _
                         pOutletsLayerName, pPointFieldName, pPointYear, _
                         pLandUseFieldName, pLandUseClassFile, _
                         pSubbasinSegmentName, _
                         pPSRCustom, pPSRCustomFile, pPSRCalculate) Then
                pMetWdmNames.Clear()
                pMetWdmNames.Add(pMetWDM)
                If CreateUCI(lOutputPath & "\" & pBaseOutputName & ".uci", pMetWdmNames, pWQConstituents) Then
                    Logger.Status("Completed HSPF Setup")
                    Logger.Dbg("UCIBuilder:  Created UCI file " & lOutputPath & "\" & pBaseOutputName & ".uci")
                Else
                    Logger.Status("HSPF Setup Failed in CreateUCI")
                End If
            Else
                Logger.Status("HSPF Setup Failed")
            End If
        Else
            Logger.Status("HSPF Setup Failed in PreProcess Checking")
        End If
        Logger.Status("")
    End Sub

End Module
