Imports atcUtility
Imports atcManDelin
Imports atcMwGisUtility
Imports atcModelSegmentation
Imports atcSegmentation
Imports atcModelSetup
Imports atcUCI
Imports MapWindow.Interfaces
Imports MapWinUtility

Module RedoLanduseHSPF

    ''General Specs
    'Private pUCIName As String = "D:\BASINS41\modelout\02060006\02060006.uci"

    ''Subbasin Specs
    'Private pSubbasinLayerName As String = "D:\BASINS41\Predefined Delineations\West Branch\wb_subs.shp"   'name of layer as displayed on legend, or file name
    'Private pSubbasinFieldName As String = "SUBBASIN"    'field containing unique integer identifier

    ''Land Use Specs
    'Private pLUType As Integer = 0   '(0 - USGS GIRAS Shape, 1 - NLCD grid, 2 - Other shape, 3 - Other grid)
    'Private pLandUseClassFile As String = "D:\BASINS41\etc\giras.dbf"   'name of file that indicates classification scheme
    ''                                                                    eg 21-25 = urban, 41-45 = cropland, etc
    'Private pLandUseLayerName As String = ""   'name of land use layer as displayed on legend, or file name,
    ''                                             does not need to be set for GIRAS land use type
    'Private pLandUseFieldName As String = ""   'field containing land use classification code,
    ''                                             only used for 'other shapefile' land use type

    'General Specs
    Private pUCIName As String = "D:\Atkins-SARA\LanduseAdjustmentTool\UpperSAR_HSPF10_85test.uci"

    'Subbasin Specs
    Private pSubbasinLayerName As String = "D:\Atkins-SARA\LanduseAdjustmentTool\SAR_SubBasin_Project.shp"   'name of layer as displayed on legend, or file name
    Private pSubbasinFieldName As String = "SubBasinID"    'field containing unique integer identifier

    'Land Use Specs
    Private pLUType As Integer = 2   '(0 - USGS GIRAS Shape, 1 - NLCD grid, 2 - Other shape, 3 - Other grid)
    Private pLandUseClassFile As String = "D:\Atkins-SARA\LanduseAdjustmentTool\reclass.dbf"   'name of file that indicates classification scheme
    '                                                                    eg 21-25 = urban, 41-45 = cropland, etc
    Private pLandUseLayerName As String = "D:\Atkins-SARA\LanduseAdjustmentTool\BexarFutureLandUseDFIRM_subset.shp"   'name of land use layer as displayed on legend, or file name,
    '                                             does not need to be set for GIRAS land use type
    Private pLandUseFieldName As String = "GRIDCODE"   'field containing land use classification code,
    '                                                   only used for 'other shapefile' land use type

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("Start")
        Dim lOutputPath As String = PathNameOnly(pUCIName)
        ChDriveDir(lOutputPath)  'change to the directory of the uci
        Logger.Dbg(" CurDir:" & CurDir())

        Dim lMsg As New atcUCI.HspfMsg
        lMsg.Open("hspfmsg.wdm")
        Dim lHspfUci As New atcUCI.HspfUci
        'open this uci
        lHspfUci.FastReadUciForStarter(lMsg, pUCIName)
        lHspfUci.Msg = lMsg

        'add layers if not already on the map
        If Not GisUtil.IsLayerByFileName(pSubbasinLayerName) Then
            GisUtil.AddLayer(pSubbasinLayerName, "Catchments")
        End If
        If Not GisUtil.IsLayerByFileName(pLandUseLayerName) Then
            GisUtil.AddLayer(pLandUseLayerName, "NLCD 2001 Landuse")
        End If

        Logger.Status("Preparing HSPF Landuse Calculations")
        If RedoLanduseHSPF(lHspfUci, _
                           pSubbasinLayerName, pSubbasinFieldName, _
                           pLUType, pLandUseLayerName, _
                           pLandUseFieldName, pLandUseClassFile) Then
        Else
            Logger.Status("Redo HSPF Landuse Failed")
        End If

        Logger.Status("")
    End Sub

    'aLUType - Land use layer type (0 - USGS GIRAS Shape, 1 - NLCD grid, 2 - Other shape, 3 - Other grid)
    Public Function RedoLanduseHSPF(ByVal aUCI As HspfUci, _
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
            For Each lRec As Object In lLandUseSubbasinOverlayRecords
                lRec.Area = lRec.Area / (lDistFactor * lDistFactor)
            Next
        End If

        Logger.Status("Completed overlay of subbasins and land use layers")

        'Create LandUses
        Dim lLandUses As LandUses = CreateLanduses(lSubbasinsSlopes, lLandUseSubbasinOverlayRecords, Nothing)
        Dim lReclassifyLanduses As LandUses = ReclassifyLandUses(lReclassifyFileName, lGridPervious, lLandUses)
        AddZeroAreaLandUses(aUCI, lReclassifyLanduses) 'Add zero area records for land uses not found in overlay operation

        'update areas
        For Each lLandUse As LandUse In lReclassifyLanduses
            Dim lDesc As String = lLandUse.Description.ToUpper
            If lDesc.Length > 20 Then
                lDesc = lDesc.Substring(0, 20)
            End If
            'update corresponding record in uci, add or delete if needed
            'pervious
            Dim lArea As Double = lLandUse.Area * (1 - lLandUse.ImperviousFraction) / 4046.8564
            PutSchematicRecord(aUCI, "PERLND", lDesc, "RCHRES", lLandUse.Reach.Id, lArea)
            'impervious
            lArea = lLandUse.Area * (lLandUse.ImperviousFraction) / 4046.8564
            PutSchematicRecord(aUCI, "IMPLND", lDesc, "RCHRES", lLandUse.Reach.Id, lArea)
        Next lLandUse

        'do some checking to make sure no area added or lost

        'save uci file
        aUCI.Name = aUCI.Name & "new"
        aUCI.Save()

        Logger.Status("")
        Return True
    End Function

    Private Sub PutSchematicRecord(ByVal aUCI As HspfUci, ByVal aSName As String, ByVal aSDesc As String, _
                                   ByVal aTName As String, ByVal aTId As Integer, ByVal aMultFact As Double)

        Dim lSId As Integer = 0
        If aSName = "RCHRES" And aTName = "BMPRAC" Then 'dont do rchres to bmp connections
        Else
            If aSName = "RCHRES" And aTName = "RCHRES" Then
                aMultFact = 1.0#
            End If
            Dim lAddIt As Boolean = True
            Dim lDeleteIt As Boolean = False
            Dim lDeleteIndex As Integer = 0
            For lIndex As Integer = 0 To aUCI.Connections.Count - 1
                Dim lConn As HspfConnection = aUCI.Connections(lIndex)
                If lConn.Typ = 3 Then 'schematic
                    If lConn.Target.Opn.Id = aTId And _
                       lConn.Target.Opn.Name = aTName And _
                       lConn.Source.Opn.Description.ToUpper = aSDesc And _
                       lConn.Source.Opn.Name = aSName Then
                        lAddIt = False
                        lConn.MFact = aMultFact
                        If Math.Abs(aMultFact) < 0.00000001 Then
                            lDeleteIt = True
                            lDeleteIndex = lIndex
                            lSId = lConn.Source.Opn.Id
                        End If
                    End If
                End If
            Next lIndex
            If lAddIt And Math.Abs(aMultFact) > 0.00000001 Then 'need to add the connection
                Dim lConn = New HspfConnection
                Dim lOpnBlk As HspfOpnBlk = aUCI.OpnBlks(aSName)
                Dim lSourceOpn As New HspfOperation  'figure out which opn this is adding 
                Dim lMatchFound As Boolean = False
                For Each lTempOpn As HspfOperation In lOpnBlk.Ids
                    If lTempOpn.Description.ToUpper = aSDesc Then
                        lSourceOpn = lTempOpn
                        lMatchFound = True
                        Exit For
                    End If
                Next
                If Not lMatchFound Then
                    Logger.Msg("Could not find a matching operation for '" & aSDesc & "'")
                Else
                    lConn.Source.Opn = lSourceOpn
                    lConn.Source.volname = lSourceOpn.Name
                    lConn.Source.volid = lSourceOpn.Id
                    lConn.Typ = 3
                    lConn.MFact = aMultFact
                    lOpnBlk = aUCI.OpnBlks(aTName)
                    Dim lOpn As HspfOperation = lOpnBlk.OperFromID(aTId)
                    lConn.Target.Opn = lOpn
                    lConn.Target.volname = lOpn.Name
                    lConn.Target.volid = lOpn.Id
                    Dim lMLId As Integer = 0
                    GetMassLinkID(aUCI, aSName, aTName, lMLId)
                    If lMLId = 0 Then
                        AddMassLink(aUCI, aSName, aTName, lMLId)
                    End If
                    lConn.MassLink = lMLId
                    lConn.Uci = aUCI
                    'add targets to source opn
                    lSourceOpn.Targets.Add(lConn)
                    lConn.Source.Opn = lSourceOpn
                    'add sources to target opn
                    lOpnBlk = aUCI.OpnBlks(aTName)
                    lOpn = lOpnBlk.OperFromID(aTId)
                    lOpn.Sources.Add(lConn)
                    lConn.Target.Opn = lOpn
                    'add to collection of connections
                    aUCI.Connections.Add(lConn)
                End If
            ElseIf lDeleteIt Then 'need to delete the connection
                aUCI.Connections.RemoveAt(lDeleteIndex)
                'remove target from source opn
                Dim lOpnBlk As HspfOpnBlk = aUCI.OpnBlks(aSName)
                Dim lOpn As HspfOperation = lOpnBlk.OperFromID(lSId)
                Dim lIndex As Integer = 1
                Do While lIndex < lOpn.Targets.Count
                    Dim lConn As HspfConnection = lOpn.Targets(lIndex)
                    If lConn.Target.VolName = aTName And _
                           lConn.Target.VolId = aTId Then
                        lOpn.Targets.RemoveAt(lIndex)
                    Else
                        lIndex = lIndex + 1
                    End If
                Loop
                'remove source from target opn
                lOpnBlk = aUCI.OpnBlks(aTName)
                lOpn = lOpnBlk.OperFromID(aTId)
                lIndex = 1
                Do While lIndex < lOpn.Sources.Count
                    Dim lConn As HspfConnection = lOpn.Sources(lIndex)
                    If lConn.Source.VolName = aSName And _
                       lConn.Source.VolId = lSId Then
                        lOpn.Sources.RemoveAt(lIndex)
                    Else
                        lIndex = lIndex + 1
                    End If
                Loop
            End If
        End If
    End Sub

    Private Sub GetMassLinkID(ByVal aUCI As HspfUci, ByVal aSName As String, ByVal aTName As String, ByRef aMLId As Integer)
        Dim lConn As HspfConnection

        'determine mass link number
        aMLId = 0
        For Each lConn In aUCI.Connections
            If lConn.Typ = 3 Then
                If lConn.Source.VolName = aSName And lConn.Target.VolName = aTName Then
                    aMLId = lConn.MassLink
                End If
            End If
        Next lConn
    End Sub

    Private Sub AddMassLink(ByVal aUCI As HspfUci, ByVal aSName As String, ByVal aTName As String, ByRef aMLId As Integer)
        'need to add masslink, find an unused number
        Dim lFound As Boolean = True
        aMLId = 1
        Do Until lFound = False
            lFound = False
            For Each lMassLink As HspfMassLink In aUCI.MassLinks
                If lMassLink.MassLinkId = aMLId Then
                    aMLId = aMLId + 1
                    lFound = True
                    Exit For
                End If
            Next lMassLink
        Loop
        'find id of masslink to copy
        Dim lCopyId As Integer = 0
        If aSName = "BMPRAC" And aTName = "RCHRES" Then
            'copy from perlnd to rchres masslink
            GetMassLinkID(aUCI, "PERLND", aTName, lCopyId)
        ElseIf aSName = "PERLND" And aTName = "BMPRAC" Then
            'copy from perlnd to rchres masslink
            GetMassLinkID(aUCI, aSName, "RCHRES", lCopyId)
        ElseIf aSName = "IMPLND" And aTName = "BMPRAC" Then
            'copy from implnd to rchres masslink
            GetMassLinkID(aUCI, aSName, "RCHRES", lCopyId)
        End If
        If aMLId > 0 And lCopyId > 0 Then
            'now copy masslink
            For lIndex As Integer = 0 To aUCI.MassLinks.Count - 1
                Dim lcMassLink As HspfMassLink = aUCI.MassLinks(lIndex)
                If lcMassLink.MassLinkId = lCopyId Then
                    'copy this record
                    Dim lMassLink As New HspfMassLink
                    lMassLink.Uci = aUCI
                    lMassLink.MassLinkId = aMLId
                    lMassLink.Source.VolName = aSName
                    lMassLink.Source.VolId = 0
                    lMassLink.Source.Group = lcMassLink.Source.Group
                    lMassLink.Source.Member = lcMassLink.Source.Member
                    lMassLink.Source.MemSub1 = lcMassLink.Source.MemSub1
                    lMassLink.Source.MemSub2 = lcMassLink.Source.MemSub2
                    lMassLink.MFact = lcMassLink.MFact
                    lMassLink.Tran = lcMassLink.Tran
                    lMassLink.Target.VolName = aTName
                    lMassLink.Target.VolId = 0
                    lMassLink.Target.Group = lcMassLink.Target.Group
                    lMassLink.Target.Member = lcMassLink.Target.Member
                    lMassLink.Target.MemSub1 = lcMassLink.Target.MemSub1
                    lMassLink.Target.MemSub2 = lcMassLink.Target.MemSub2

                    If (aSName = "PERLND" Or aSName = "IMPLND") And _
                      aTName = "BMPRAC" Then  'special cases
                        If lcMassLink.Target.Member = "OXIF" Then
                            lMassLink.Target.Member = "IOX"
                        ElseIf lcMassLink.Target.Member = "NUIF1" Then
                            lMassLink.Target.Member = "IDNUT"
                        ElseIf lcMassLink.Target.Member = "NUIF2" Then
                            lMassLink.Target.Member = "ISNUT"
                        ElseIf lcMassLink.Target.Member = "PKIF" Then
                            lMassLink.Target.Member = "IPLK"
                        End If
                    End If

                    If aSName = "BMPRAC" And aTName = "RCHRES" Then
                        'special cases
                        lMassLink.Source.Group = "ROFLOW"
                        lMassLink.MFact = 1.0#
                        If lcMassLink.Target.Member = "IVOL" Then
                            lMassLink.Source.Member = "ROVOL"
                        ElseIf lcMassLink.Target.Member = "CIVOL" Then
                            lMassLink.Source.Member = "CROVOL"
                        ElseIf lcMassLink.Target.Member = "ICON" Then
                            lMassLink.Source.Member = "ROCON"
                        ElseIf lcMassLink.Target.Member = "IHEAT" Then
                            lMassLink.Source.Member = "ROHEAT"
                        ElseIf lcMassLink.Target.Member = "ISED" Then
                            lMassLink.Source.Member = "ROSED"
                        ElseIf lcMassLink.Target.Member = "IDQAL" Then
                            lMassLink.Source.Member = "RODQAL"
                        ElseIf lcMassLink.Target.Member = "ISQAL" Then
                            lMassLink.Source.Member = "ROSQAL"
                        ElseIf lcMassLink.Target.Member = "OXIF" Then
                            lMassLink.Source.Member = "ROOX"
                        ElseIf lcMassLink.Target.Member = "NUIF1" Then
                            lMassLink.Source.Member = "RODNUT"
                        ElseIf lcMassLink.Target.Member = "NUIF2" Then
                            lMassLink.Source.Member = "ROSNUT"
                        ElseIf lcMassLink.Target.Member = "PKIF" Then
                            lMassLink.Source.Member = "ROPLK"
                        ElseIf lcMassLink.Target.Member = "PHIF" Then
                            lMassLink.Source.Member = "ROPH"
                        End If
                        lMassLink.Source.MemSub1 = lcMassLink.Target.MemSub1
                        lMassLink.Source.MemSub2 = lcMassLink.Target.MemSub2
                    End If

                    Dim lExists As Boolean = False
                    For Each lML As HspfMassLink In aUCI.MassLinks
                        If lML.MassLinkId = lMassLink.MassLinkId AndAlso _
                           lML.MFact = lMassLink.MFact AndAlso _
                           lML.Source.VolName = lMassLink.Source.VolName AndAlso _
                           lML.Source.Group = lMassLink.Source.Group AndAlso _
                           lML.Source.Member = lMassLink.Source.Member AndAlso _
                           lML.Source.MemSub1 = lMassLink.Source.MemSub1 AndAlso _
                           lML.Source.MemSub2 = lMassLink.Source.MemSub2 AndAlso _
                           lML.Target.VolName = lMassLink.Target.VolName AndAlso _
                           lML.Target.Group = lMassLink.Target.Group AndAlso _
                           lML.Target.Member = lMassLink.Target.Member AndAlso _
                           lML.Target.MemSub1 = lMassLink.Target.MemSub1 AndAlso _
                           lML.Target.MemSub2 = lMassLink.Target.MemSub2 Then
                            lExists = True
                        End If
                    Next
                    If Not lExists Then
                        'if this masslink record does not already exist, add it
                        aUCI.MassLinks.Add(lMassLink)
                    End If
                End If
            Next
        End If

    End Sub

    Public Sub AddZeroAreaLandUses(ByVal aUci As HspfUci, _
                                   ByRef aLandUses As LandUses)
        For Each lPerOper As HspfOperation In aUci.OpnBlks("PERLND").Ids
            For Each lRchOper As HspfOperation In aUci.OpnBlks("RCHRES").Ids
                'is this combo in aLanduses?
                Dim lFound As Boolean = False
                For Each lLanduse As LandUse In aLandUses
                    Dim lDesc As String = lLanduse.Description.ToUpper
                    If lDesc.Length > 20 Then
                        lDesc = lDesc.Substring(0, 20)
                    End If
                    If lDesc = lPerOper.Description.ToUpper And lLanduse.Reach.Id = lRchOper.Id Then
                        lFound = True
                    End If
                Next
                If Not lFound Then
                    'this combo is not there, add it
                    Dim lNewLanduse As New LandUse
                    Dim lNewReach As New Reach
                    With lNewLanduse
                        .Description = lPerOper.Description
                        .Area = 0.0
                        .ImperviousFraction = 0.5
                        .ModelID = 0
                        .Reach = lNewReach
                        .Reach.Id = lRchOper.Id
                        .Type = "COMPOSITE"
                    End With
                    aLandUses.Add(lNewLanduse)
                End If
            Next
        Next
    End Sub

End Module
