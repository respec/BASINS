Imports atcUtility
Imports atcManDelin
Imports atcMwGisUtility
Imports atcModelSegmentation
Imports atcSegmentation
Imports atcModelSetup
Imports MapWindow.Interfaces
Imports MapWinUtility

Module RedoLanduseHSPF

    'General Specs
    Private pUCIName As String = "D:\BASINS41\modelout\02060006\02060006.uci"

    'Subbasin Specs
    Private pSubbasinLayerName As String = "D:\BASINS41\Predefined Delineations\West Branch\wb_subs.shp"   'name of layer as displayed on legend, or file name
    Private pSubbasinFieldName As String = "SUBBASIN"    'field containing unique integer identifier

    'Land Use Specs
    Private pLUType As Integer = 0   '(0 - USGS GIRAS Shape, 1 - NLCD grid, 2 - Other shape, 3 - Other grid)
    Private pLandUseClassFile As String = "D:\BASINS41\etc\giras.dbf"   'name of file that indicates classification scheme
    '                                                                    eg 21-25 = urban, 41-45 = cropland, etc
    Private pLandUseLayerName As String = ""   'name of land use layer as displayed on legend, or file name,
    '                                             does not need to be set for GIRAS land use type
    Private pLandUseFieldName As String = ""   'field containing land use classification code,
    '                                             only used for 'other shapefile' land use type

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("Start")
        Dim lOutputPath As String = PathNameOnly(pUCIName)
        ChDriveDir(lOutputPath)  'change to the directory of the uci
        Logger.Dbg(" CurDir:" & CurDir())

        '  for testing NLCD 2001
        'pLUType = 1
        'pLandUseClassFile = "C:\BASINS\etc\nlcd.dbf"
        'pLandUseLayerName = "C:\BASINS\data\02060006-16\NLCD\nlcd_landcover_2001.tif"

        'add layers if not already on the map
        If Not GisUtil.IsLayerByFileName(pSubbasinLayerName) Then
            GisUtil.AddLayer(pSubbasinLayerName, "Catchments")
        End If
        If Not GisUtil.IsLayerByFileName(pLandUseLayerName) Then
            GisUtil.AddLayer(pLandUseLayerName, "NLCD 2001 Landuse")
        End If

        Logger.Status("Preparing HSPF Landuse Calculations")
        If RedoLanduseHSPF(pUCIName, _
                           pSubbasinLayerName, pSubbasinFieldName, _
                           pLUType, pLandUseLayerName, _
                           pLandUseFieldName, pLandUseClassFile) Then
        Else
            Logger.Status("Redo HSPF Landuse Failed")
        End If

        Logger.Status("")
    End Sub

    'aLUType - Land use layer type (0 - USGS GIRAS Shape, 1 - NLCD grid, 2 - Other shape, 3 - Other grid)
    Public Function RedoLanduseHSPF(ByVal aUCIName As String, _
                                    ByVal aSubbasinLayerName As String, ByVal aSubbasinFieldName As String, _
                                    ByVal aLUType As Integer, ByVal aLandUseThemeName As String, _
                                    ByVal aLandUseFieldName As String, ByVal aLandUseClassFile As String) As Boolean

        Logger.Status("Preparing to process")
        Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
        Dim lBasinsFolder As String = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\AQUA TERRA Consultants\BASINS", "Base Directory", "C:\Basins")

        'get land use data ready
        Dim lGridPervious As New atcControls.atcGrid
        lGridPervious.Source = New atcControls.atcGridSource
        SetPerviousGrid(lGridPervious, pLandUseClassFile, pLUType, pLandUseLayerName, pLandUseFieldName)

        'build collection of selected subbasins 
        Dim lSubbasinsSelected As New atcCollection  'key is index, value is subbasin id
        Dim lSubbasinsSlopes As New atcCollection    'key is subbasin id, value is slope
        Dim lSubbasinId As Integer
        Dim lSubbasinSlope As Double = 0.0
        Dim lSubbasinLayerIndex As Long = GisUtil.LayerIndex(aSubbasinLayerName)
        Dim lSubbasinFieldIndex As Long = GisUtil.FieldIndex(lSubbasinLayerIndex, aSubbasinFieldName)
        For i As Integer = 1 To GisUtil.NumFeatures(lSubbasinLayerIndex)
            lSubbasinId = GisUtil.FieldValue(lSubbasinLayerIndex, i - 1, lSubbasinFieldIndex)
            lSubbasinsSelected.Add(i - 1, lSubbasinId)
            lSubbasinsSlopes.Add(lSubbasinId, lSubbasinSlope)
        Next

        'build collection of model segment ids for each subbasin
        Dim lSubbasinsModelSegmentIds As New atcCollection    'key is subbasin id, value is model segment id
        For Each lSubbasinIndex As Integer In lSubbasinsSelected.Keys
            lSubbasinId = lSubbasinsSelected.ItemByKey(lSubbasinIndex)
            lSubbasinsModelSegmentIds.Add(lSubbasinId, 1)
        Next

        'each land use code, subbasin id, and area is a single land use record
        Dim lLandUseSubbasinOverlayRecords As New Collection '(Of LandUseSubbasinOverlayRecord)
        Dim lReclassifyFileName As String = ""
        Dim lSnowOption As Integer = 0
        Dim lElevationFileName As String = ""
        Dim lElevationUnits As String = ""

        If aLUType = 0 Then
            'usgs giras is the selected land use type
            Logger.Status("Performing overlay for GIRAS landuse")
            Dim lSuccess As Boolean = CreateLanduseRecordsGIRAS(lSubbasinsSelected, lLandUseSubbasinOverlayRecords, aSubbasinLayerName, aSubbasinFieldName, _
                                                                lSnowOption, lElevationFileName, lElevationUnits)

            If lLandUseSubbasinOverlayRecords.Count = 0 Or Not lSuccess Then
                'problem occurred, get out
                Return False
                Exit Function
            End If
            'set reclassify file name for giras
            If IO.File.Exists(aLandUseClassFile) Then
                lReclassifyFileName = aLandUseClassFile
            Else
                Dim lLandUsePathName As String = PathNameOnly(GisUtil.LayerFileName(GisUtil.LayerIndex("Land Use Index"))) & "\landuse"
                Dim lBasinsBinLoc As String = PathNameOnly(System.Reflection.Assembly.GetEntryAssembly.Location)
                lReclassifyFileName = lBasinsBinLoc.Substring(0, lBasinsBinLoc.Length - 3) & "etc\"
                If IO.Directory.Exists(lReclassifyFileName) Then
                    lReclassifyFileName &= "giras.dbf"
                Else
                    lReclassifyFileName = lBasinsFolder & "\etc\giras.dbf"
                End If
            End If
        ElseIf aLUType = 1 Or aLUType = 3 Then
            'nlcd grid or other grid is the selected land use type
            Logger.Status("Overlaying Land Use and Subbasins")
            CreateLanduseRecordsGrid(lSubbasinsSelected, lLandUseSubbasinOverlayRecords, aSubbasinLayerName, aLandUseThemeName, _
                                     lSnowOption, lElevationFileName, lElevationUnits)

            If aLUType = 1 Then 'nlcd grid
                If IO.File.Exists(aLandUseClassFile) Then
                    lReclassifyFileName = aLandUseClassFile
                Else
                    'try to default if not set
                    Dim lBasinsBinLoc As String = PathNameOnly(System.Reflection.Assembly.GetEntryAssembly.Location)
                    lReclassifyFileName = lBasinsBinLoc.Substring(0, lBasinsBinLoc.Length - 3) & "etc\"
                    If IO.Directory.Exists(lReclassifyFileName) Then
                        lReclassifyFileName &= "nlcd.dbf"
                    Else
                        lReclassifyFileName = lBasinsFolder & "\etc\nlcd.dbf"
                    End If
                End If
            Else
                If aLandUseClassFile <> "<none>" Then
                    lReclassifyFileName = aLandUseClassFile
                End If
            End If

        ElseIf aLUType = 2 Then
            'other shape
            Logger.Status("Overlaying Land Use and Subbasins")
            CreateLanduseRecordsShapefile(lSubbasinsSelected, lLandUseSubbasinOverlayRecords, aSubbasinLayerName, aSubbasinFieldName, aLandUseThemeName, aLandUseFieldName, _
                                          lSnowOption, lElevationFileName, lElevationUnits)

            lReclassifyFileName = ""
            If aLandUseClassFile <> "<none>" Then
                lReclassifyFileName = aLandUseClassFile
            End If

        End If

        'convert units if the GIS layer units are feet instead of meters
        Dim lDistFactor As Double = 1.0
        If GisUtil.MapUnits.ToUpper = "FEET" Then
            lDistFactor = 3.2808
            For Each lRec As LandUseSubbasinOverlayRecord In lLandUseSubbasinOverlayRecords
                lRec.Area = lRec.Area / (lDistFactor * lDistFactor)
            Next
        End If

        Logger.Status("Completed overlay of subbasins and land use layers")

        'Create LandUses
        Dim lLandUses As LandUses = CreateLanduses(lSubbasinsSlopes, lLandUseSubbasinOverlayRecords, Nothing)
        Dim lReclassifyLanduses As LandUses = ReclassifyLandUses(lReclassifyFileName, lGridPervious, lLandUses)
        'update areas
        'save uci file

        Logger.Status("")
        Return True
    End Function

    Friend Class LandUseSubbasinOverlayRecord
        Friend LuCode As String          'land use code
        Friend SubbasinId As String      'subbasin id 
        Friend Area As Double            'area of this land use within this subbasin
        Friend MeanElevation As Single   'mean elevation of this land use within this subbasin (used for snow)
        Friend MeanLatitude As Single    'mean latitude of this land use within this subbasin  (used for snow)
    End Class
End Module
