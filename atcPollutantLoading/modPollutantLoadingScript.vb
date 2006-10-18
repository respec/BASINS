Imports System.Collections.Specialized
Imports Microsoft.VisualBasic
Imports MapWindow.Interfaces
Imports MapWinUtility
Imports atcUtility
Imports atcControls
Imports atcPollutantLoading
Imports atcMwGisUtility

''' <summary>
''' Pollutant Loading Script
''' </summary>
''' <remarks>
''' Created by Jack Kittle (jlkittle@aquaterra.com)
''' Date 12 Oct 2006
''' </remarks>
Module modPollutantLoadingScript
    Private Const pDirPath As String = "c:\test\atcPollutantLoading"
    Private Const pBasinsDir As String = "D:\Basins\"
    Private Const pGridValuesSourceFilename As String = pBasinsDir & "etc\pload\ecgiras.dbf"

    Private Const pProject As String = pBasinsDir & "data\02070009\02070009.mwprj"
    Private Const pSubbasinLayerName As String = "CBP Phase 5 - River Segments"
    Private Const pLandUse As String = "USGS GIRAS Shapefile"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("PollutantLoadingScript:Start")
        Logger.Dbg(" DirPath '" & pDirPath & "'")
        ChDriveDir(pDirPath)
        Logger.Dbg(" CurDir '" & CurDir() & "'")

        GisUtil.MappingObject = aMapWin
        Logger.Dbg(" MappingObjectSet")

        aMapWin.Project.Load(pProject)
        Logger.Dbg(" Project '" & pProject & "' Loaded, Curdir '" & CurDir() & "'")

        Dim lUseExportCoefficent As Boolean = True
        Dim lGridSource As New atcGridSource
        SetGridValuesSource(pGridValuesSourceFileName, lGridSource)
        Dim lLandUseLayer As String = ""
        Dim lLandUseId As String = ""
        Dim lPrec As Double = 40
        Dim lRatio As Double = 0.9
        Dim lConstituents As New atcCollection
        lConstituents.Add("BOD")

        Dim lBmps As PollutantLoadingBMPs = Nothing
        'Dim lBMPLayerName As String = ""
        'Dim lBMPAreaField As String = ""
        'Dim lBMPTypefield As String = ""
        'Dim lBMPGridSource As New atcGridSource

        Dim lPointLoads As PollutantLoadingPointLoads = Nothing
        'Dim lPointLayerName As String = ""
        'Dim lPointIDField As String = ""
        'Dim lPointGridSource As New atcGridSource
        Dim lStreamBankLoads As PollutantLoadingStreamBankLoads = Nothing

        PollutantLoading.GenerateLoads( _
            pSubbasinLayerName, _
            lGridSource, _
            lUseExportCoefficent, _
            pLandUse, _
            lLandUseLayer, _
            lLandUseId, _
            lPrec, _
            lRatio, _
            lConstituents, _
            lBmps, _
            lPointLoads, _
            lStreamBankLoads)
    End Sub
End Module
