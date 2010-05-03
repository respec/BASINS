Imports atcUtility
Imports atcData
Imports atcUCI
Imports atcSegmentation
Imports atcModelSetup
Imports MapWindow.Interfaces
Imports MapWinUtility
Imports MapWinGeoProc
Imports atcMwGisUtility
Imports System.Text
Imports System.Collections.Specialized
Imports System.Collections.ObjectModel
Imports System.Drawing

Module UCIBuilder
    Private pOutputPath As String = "C:\BASINS\modelout\"
    Private pInputPath As String = "C:\Projects\TT_GCRP\ProjectsBasins\070200\"
    Private pBaseOutputName As String = "UpperMS"
    Private pSubbasinThemeName As String = "UpperMS_subbasin_HSPF_V1a"
    'Private pSubbasinThemeName As String = "Test_catchments"
    Private pSubbasinFieldName As String = "SUBBASIN"
    Private pSubbasinSlopeName As String = "SLO1"
    Private pSubbasinSegmentName As String = "ModelSeg"
    Private pLandUseClassFile As String = "C:\Projects\TT_GCRP\Model Setup\GCRP_MinnRiver.dbf"
    Private pLUType As Integer = 3
    'Private pLandUseThemeName As String = "nlcd_landcover_2001"
    Private pLandUseThemeName As String = "hru_mnriv"
    Private pLandUseFieldName As String = ""
    Private pFieldNameLandUse As String = ""
    Private pMetWDM As String = pInputPath & "met\MNRV_MET.wdm"
    Private pPointThemeName As String = ""
    Private pPointYear As String = ""
    Private pStreamLayerName As String = "streams"
    Private pStreamFields() As String = {"SUBBASIN", "SUBBASINR", "LEN2", "SLO2", "WID2", "DEP2", "MINEL", "MAXEL", "SNAME"}
    Private pOutletsThemeName As String = "<none>"
    Private pPSRCustom As Boolean = False
    Private pPSRCustomFile As String = ""
    Private pPSRCalculate As Boolean = False
    Private pLUInclude() As Integer = {61, 71, 81, 91}

    Friend AtcGridMet As atcControls.atcGrid
    Friend AtcGridPervious As atcControls.atcGrid
    Friend pMetBaseDsns As atcCollection
    Friend pUniqueModelSegmentIds As atcCollection
    Friend pUniqueModelSegmentNames As atcCollection
    Friend pMetStations As atcCollection

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("Start")
        ChDriveDir(pOutputPath)  'change to the directory of the current project
        Logger.Dbg(" CurDir:" & CurDir())

        AtcGridPervious = New atcControls.atcGrid
        AtcGridPervious.Source = New atcControls.atcGridSource
        SetPerviousGrid(AtcGridPervious, pLandUseClassFile, pLUType, pLandUseThemeName, pFieldNameLandUse)
        pMetStations = New atcCollection
        pMetBaseDsns = New atcCollection
        BuildListofMetStationNames(pMetWDM, pMetStations, pMetBaseDsns)
        AtcGridMet = New atcControls.atcGrid
        AtcGridMet.Source = New atcControls.atcGridSource
        SetMetSegmentGrid(AtcGridMet, pMetStations, pUniqueModelSegmentNames, pUniqueModelSegmentIds, _
                          pSubbasinThemeName, pSubbasinFieldName, pSubbasinSegmentName)
        Dim lOutputPath As String = pOutputPath & pBaseOutputName

        If PreProcessChecking(lOutputPath, pBaseOutputName, "HSPF", pLUType, pMetStations.Count, _
                              pSubbasinThemeName, pLandUseThemeName) Then 'early checks OK
            Logger.Status("Preparing HSPF Setup")
            If SetupHSPF(AtcGridMet, 1, AtcGridPervious, _
                         pMetStations, pMetBaseDsns, _
                         pUniqueModelSegmentNames, pUniqueModelSegmentIds, _
                         lOutputPath, pBaseOutputName, _
                         pSubbasinThemeName, pSubbasinFieldName, pSubbasinSlopeName, _
                         pStreamLayerName, pStreamFields, _
                         pLUType, pLandUseThemeName, pLUInclude, _
                         pOutletsThemeName, pPointThemeName, pPointYear, _
                         pLandUseFieldName, pLandUseClassFile, _
                         pSubbasinSegmentName, _
                         pPSRCustom, pPSRCustomFile, pPSRCalculate) Then
                If CreateUCI(lOutputPath & "\" & pBaseOutputName & ".uci", pMetWDM) Then
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
