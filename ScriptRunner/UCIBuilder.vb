Imports atcUtility
Imports atcModelSetup
Imports MapWindow.Interfaces
Imports MapWinUtility

Module UCIBuilder

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
    Private pLandUseThemeName As String = ""   'name of land use layer as displayed on legend, or file name,
    '                                             does not need to be set for GIRAS land use type
    Private pLandUseFieldName As String = ""   'field containing land use classification code,
    '                                             only used for 'other shapefile' land use type
    Private pLUInclude() As Integer = {}       'special array to specify land use codes to be included
    '                                             for every subbasin, even if none exists in that subbasin,
    '                                             used to enforce a numbering convention

    'Met Data Specs
    Private pMetWDM As String = pInputPath & "met\met.wdm"   'name of met wdm file
    Private pSingleMetStationSelected As Integer = 3         'index of met station to use (zero based) if using a single station,
    '                                                           used only if pSubbasinSegmentName is "<none>".
    '                                                           otherwise pSubbasinSegmentName data is used 
    '                                                           to match model segments to met stations.

    'Stream Specs
    Private pStreamLayerName As String = "Streams"     'name of layer as displayed on legend, or file name
    Private pStreamFields() As String = {"SUBBASIN", "SUBBASINR", "LEN2", "SLO2", "WID2", "DEP2", "MINEL", "MAXEL", "SNAME"}
    '                                                   array of field names containing required data for each stream

    'Point Source Specs
    Private pOutletsLayerName As String = "Outlets"    'name of outlets layer from legend, or file name
    Private pPointFieldName As String = "PCSID"        'name of field within outlets layer containing unique 
    '                                                     point source identifier (like pcs station id)
    Private pPointYear As String = "1999"              'year of pcs data to use
    Private pPSRCustom As Boolean = False              'flag indicating use custom point source data, else pcs
    Private pPSRCustomFile As String = ""              'if using custom point source data, pass file name
    Private pPSRCalculate As Boolean = False           'flag indicating if distance on stream is to be calculated,
    '                                                   not implemented at this time

    'Water Quality Constituents to Add
    Private pWQConstituents() As String = {"NH3+NH4", "NO3", "ORTHO P", "BOD", "SEDIMENT"}

    'the following parameters are for the Tt GCRP Setup:
    'Private pOutputPath As String = "C:\BASINS\modelout\"
    'Private pInputPath As String = "C:\Projects\TT_GCRP\ProjectsBasins\070200\"
    'Private pBaseOutputName As String = "UpperMS"
    'Private pSubbasinLayerName As String = "UpperMS_subbasin_HSPF_V1a"
    'Private pSubbasinSegmentName As String = "ModelSeg"
    'Private pSingleMetStationSelected As Integer = 1
    'Private pLandUseClassFile As String = "C:\Projects\TT_GCRP\Model Setup\GCRP_MinnRiver.dbf"
    'Private pLUType As Integer = 3
    'Private pLandUseLayerName As String = "hru_mnriv"
    'Private pMetWDM As String = pInputPath & "met\MNRV_MET.wdm"
    'Private pLUInclude() As Integer = {61, 71, 81, 91}

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("Start")
        ChDriveDir(pOutputPath)  'change to the directory of the current project
        Logger.Dbg(" CurDir:" & CurDir())

        Dim AtcGridPervious As New atcControls.atcGrid
        AtcGridPervious.Source = New atcControls.atcGridSource
        SetPerviousGrid(AtcGridPervious, pLandUseClassFile, pLUType, pLandUseThemeName, pLandUseFieldName)

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

        Dim lOutputPath As String = pOutputPath & pBaseOutputName

        If PreProcessChecking(lOutputPath, pBaseOutputName, "HSPF", pLUType, pMetStations.Count, _
                              pSubbasinLayerName, pLandUseThemeName) Then 'early checks OK
            Logger.Status("Preparing HSPF Setup")
            Dim lMetBaseDsns As New atcCollection
            lMetBaseDsns.Add(pMetBaseDsns(0))
            Dim lMetWdmIds As New atcCollection
            lMetWdmIds.Add("WDM2")
            If SetupHSPF(AtcGridPervious, _
                         lMetBaseDsns, lMetWdmIds, _
                         pUniqueModelSegmentNames, pUniqueModelSegmentIds, _
                         lOutputPath, pBaseOutputName, _
                         pSubbasinLayerName, pSubbasinFieldName, pSubbasinSlopeName, _
                         pStreamLayerName, pStreamFields, _
                         pLUType, pLandUseThemeName, pLUInclude, _
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
