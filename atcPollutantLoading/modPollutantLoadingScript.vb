Imports System.Collections.Specialized
Imports Microsoft.VisualBasic
Imports MapWindow.Interfaces
Imports MapWinUtility
Imports atcUtility
Imports atcControls
Imports atcPollutantLoading.modPollutantLoading
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
    Private Const pProject As String = "D:\Basins\data\02070009\02070009.mwprj"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("PollutantLoadingScript:Start")
        Logger.Dbg(" DirPath '" & pDirPath & "'")
        ChDriveDir(pDirPath)
        Logger.Dbg(" CurDir '" & CurDir() & "'")

        GisUtil.MappingObject = aMapWin
        Logger.Dbg(" MappingObjectSet")

        aMapWin.Project.Load(pProject)
        Logger.Dbg(" Project '" & pProject & "' Loaded, Curdir '" & CurDir() & "'")

        Dim lSubbasinLayerName As String = "CBP Phase 5 - River Segments"
        Dim lGridSource As New atcGridSource 
        lGridSource = Nothing
        Dim lUseExportCoefficent As Boolean = True
        Dim lLandUse As String = "USGS GIRAS Shapefile"
        Dim lLandUseLayer As String = ""
        Dim lLandUseId As String = ""
        Dim lPrec As Double = 40
        Dim lRatio As Double = 0.9
        Dim lConstituents As New atcCollection
        lConstituents.Add("BOD")

        GenerateLoads(lSubbasinLayerName, _
                      lGridSource, _
                      lUseExportCoefficent, _
                      lLandUse, _
                      lLandUseLayer, _
                      lLandUseId, _
                      lPrec, _
                      lRatio, _
                      lConstituents)

    End Sub
End Module
