Imports atcData
Imports atcUtility
Imports MapWindow.Interfaces
Imports MapWinUtility
Imports atcMwGisUtility

Imports Microsoft.VisualBasic
Imports System

Public Module ScriptMapDataLocations
    Private Const pBasePath As String = "c:\Basins\data\03130003\"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)

        'Build a map of data locations as specified in the script

        Logger.Dbg("Start")

        'change to the directory of the current project
        ChDriveDir(pBasePath)
        Logger.Dbg(" CurDir:" & CurDir())

        'set the mapping object
        GisUtil.MappingObject = aMapWin

        'add base map layers
        GisUtil.AddLayer(pBasePath & "st.shp", "State Boundaries")
        GisUtil.AddLayer(pBasePath & "cnty.shp", "County Boundaries")
        GisUtil.AddLayer(pBasePath & "FortBenning\installation_area_boundary.shp", "Installation Area Boundary")
        GisUtil.AddLayer(pBasePath & "FortBenning\wshed6th.shp", "6th Order Watersheds")
        GisUtil.AddLayer(pBasePath & "FortBenning\wshed5th.shp", "5th Order Watersheds")
        'GisUtil.AddLayer(pBasePath & "FortBenning\wshed4th.shp", "4th Order Watersheds")
        'GisUtil.AddLayer(pBasePath & "FortBenning\wshed3rd_clip.shp", "3rd Order Watersheds")
        'GisUtil.AddLayer(pBasePath & "FortBenning\wshed2nd_clip.shp", "2nd Order Watersheds")
        GisUtil.AddLayer(pBasePath & "FortBenning\majorroads.shp", "Major Roads")
        GisUtil.AddLayer(pBasePath & "FortBenning\streams2.shp", "Streams")

        GisUtil.AddLayer(pBasePath & "met\prec.shp", "Precip Stations") 
        'GisUtil.AddLayer(pBasePath & "met\evap.shp", "Evap Stations")
        'GisUtil.AddLayer(pBasePath & "met\atem.shp", "Air Temp Stations")
        'GisUtil.AddLayer(pBasePath & "met\dewpwindsolrclou.shp", "Dewpoint, Wind, Solr, CloudCover Stations")
        'GisUtil.AddLayer(pBasePath & "flow\flow.shp", "USGS Flow Stations")

        'make layers visible
        For lIndex As Integer = 0 To GisUtil.NumLayers - 1
            GisUtil.LayerVisible(lIndex) = True
        Next

        'zoom to layer?
        Dim lLayer As MapWindow.Interfaces.Layer = aMapWin.Layers(aMapWin.Layers.CurrentLayer)
        lLayer.ZoomTo()
        lLayer.Expanded = True

        'label points

        'save to jpg


    End Sub

End Module
