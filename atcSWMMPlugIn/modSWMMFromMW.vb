Imports atcMwGisUtility
Imports atcSWMM
Imports atcData
Imports MapWinUtility
Imports atcUtility

Friend Module modSWMMFromMW

    Public Function CompleteCatchmentsFromShapefile(ByVal aCatchmentShapefileName As String, _
                                                    ByVal aPrecGageNamesByCatchment As Collection, _
                                                    ByVal aSWMMProject As atcSWMMProject, _
                                                    ByRef aCatchments As atcSWMMCatchments) As Boolean

        Logger.Dbg("ShapefileName " & aCatchmentShapefileName)
        If Not GisUtil.IsLayerByFileName(aCatchmentShapefileName) Then
            If GisUtil.AddLayer(aCatchmentShapefileName, "Catchments") Then
                Logger.Dbg("LayerAdded")
            Else
                Logger.Msg("ProblemAddingCatchmentLayer") 'Exception?
            End If
        End If
        Dim lLayerIndex As Integer = GisUtil.LayerIndex(aCatchmentShapefileName)

        For lFeatureIndex As Integer = 0 To GisUtil.NumFeatures(lLayerIndex) - 1
            Dim lCatchment As atcSWMMCatchment = aCatchments(lFeatureIndex)

            If Not lCatchment.Name Is Nothing Then
                If lCatchment.Name.Length = 0 Then
                    lCatchment.Name = "S" & CStr(lFeatureIndex + 1)
                Else
                    If IsNumeric(lCatchment.Name) Then
                        lCatchment.Name = "S" & CStr(CInt(lCatchment.Name))
                    End If
                End If
            End If

            If aSWMMProject.RainGages.Count > 0 Then
                'assign each catchment to the selected raingage 
                If aPrecGageNamesByCatchment.Count > 1 Then
                    lCatchment.RainGage = aSWMMProject.RainGages(aPrecGageNamesByCatchment(lFeatureIndex + 1))
                Else 'using one raingage for all
                    lCatchment.RainGage = aSWMMProject.RainGages(aPrecGageNamesByCatchment(1))
                End If
                Logger.Dbg("Assigned Rain Gage " & lCatchment.RainGage.Name)
            Else
                Logger.Dbg("No Rain Gage available to assign")
            End If

            'find associated outlet node
            If lCatchment.OutletNodeID Is Nothing Then
                Logger.Dbg("No Outlet Node")
            Else
                If aSWMMProject.Nodes.Contains(lCatchment.OutletNodeID) Then
                    lCatchment.OutletNode = aSWMMProject.Nodes(lCatchment.OutletNodeID)
                    Logger.Dbg("Outlet Node " & lCatchment.OutletNode.Name)
                End If
            End If

            If lCatchment.OutletNode Is Nothing Then
                'find node through associated conduit
                For Each lConduit As atcSWMMConduit In aSWMMProject.Conduits
                    If lConduit.Name.Substring(1) = lCatchment.Name Then
                        lCatchment.OutletNode = lConduit.OutletNode
                        Exit For
                    End If
                Next
            End If

            If lCatchment.Area <= 0.0 Then
                lCatchment.Area = GisUtil.FeatureArea(lLayerIndex, lFeatureIndex) / 4047.0  'convert m2 to acres
            End If

            'lCatchment.PercentImpervious()  'this is computed later
            If lCatchment.Width <= 0.0 Then
                lCatchment.Width = Math.Sqrt(lCatchment.Area * 43560)
            End If

            GisUtil.PointsOfLine(lLayerIndex, lFeatureIndex, lCatchment.X, lCatchment.Y)
        Next
        Logger.Dbg("Completed " & GisUtil.NumFeatures(lLayerIndex) & " CatchmentsFromShapeFile")
    End Function

    Public Function CompleteConduitsFromShapefile(ByVal aConduitShapefilename As String, _
                                                  ByVal aSWMMProject As atcSWMMProject, _
                                                  ByRef aConduits As atcSWMMConduits) As Boolean
        Logger.Dbg("ShapefileName " & aConduitShapefilename)

        Dim lNeedNodes As Boolean = False
        If aSWMMProject.Nodes.Count = 0 Then
            lNeedNodes = True
        End If

        If Not GisUtil.IsLayerByFileName(aConduitShapefilename) Then
            If GisUtil.AddLayer(aConduitShapefilename, "Conduits") Then
                Logger.Dbg("LayerAdded")
            Else
                Logger.Msg("ProblemAddingConduitLayer") 'Exception?
            End If
        End If
        Dim lLayerIndex As Integer = GisUtil.LayerIndex(aConduitShapefilename)

        For lFeatureIndex As Integer = 0 To GisUtil.NumFeatures(lLayerIndex) - 1
            Dim lConduit As atcSWMMConduit = aConduits(lFeatureIndex)

            'calculate the actual feature length
            If lConduit.Length.Value Is Nothing Then
                lConduit.Length.Value = GisUtil.FeatureLength(lLayerIndex, lFeatureIndex) * 3.281 'need to convert meters to feet
            End If

            If lConduit.Geometry1 < 0 Then
                lConduit.Geometry1 = lConduit.MeanDepth * 1.25 'full height = mean depth * 1.25
            End If
            If lConduit.Geometry2 < 0 Then
                lConduit.Geometry2 = lConduit.MeanWidth - (lConduit.MeanDepth / lConduit.Geometry3) - (lConduit.MeanDepth / lConduit.Geometry4) 'base width
            End If

            If lNeedNodes Then
                'nodes not specified, create them at end points of conduits

                If lConduit.ElevationHigh < lConduit.ElevationLow Then
                    'something is wrong, switch places
                    Dim lTemp As Double = lConduit.ElevationHigh
                    lConduit.ElevationHigh = lConduit.ElevationLow
                    lConduit.ElevationLow = lTemp
                End If

                Dim lXup As Double
                Dim lYup As Double
                Dim lXdown As Double
                Dim lYdown As Double
                GisUtil.EndPointsOfLine(lLayerIndex, lFeatureIndex, lXup, lYup, lXdown, lYdown)
                'todo: may need to verify which way the line is digitized

                'create node at upstream end
                Dim lUpNode As New atcSWMMNode
                lUpNode.Name = "J" & lConduit.Name
                lUpNode.Type = "JUNCTION"
                lUpNode.InvertElevation = lConduit.ElevationHigh
                lUpNode.XPos = lXup
                lUpNode.YPos = lYup
                If Not aSWMMProject.Nodes.Contains(lUpNode.Name) Then
                    aSWMMProject.Nodes.Add(lUpNode)
                    lConduit.InletNode = lUpNode
                Else
                    lConduit.InletNode = aSWMMProject.Nodes(lUpNode.Name)
                End If

                'create node at downstream end
                Dim lNode As New atcSWMMNode
                If lConduit.DownConduitID.Length > 0 AndAlso CInt(lConduit.DownConduitID) > 0 Then
                    lNode.Name = "J" & lConduit.DownConduitID
                    lNode.Type = "JUNCTION"
                Else
                    lNode.Name = "O1"
                    lNode.Type = "OUTFALL"
                End If
                lNode.InvertElevation = lConduit.ElevationLow
                lNode.XPos = lXdown
                lNode.YPos = lYdown
                If Not aSWMMProject.Nodes.Contains(lNode.Name) Then
                    aSWMMProject.Nodes.Add(lNode)
                    lConduit.OutletNode = lNode
                Else
                    lConduit.OutletNode = aSWMMProject.Nodes(lNode.Name)
                    'make sure coordinates correspond with downstream end
                    lConduit.OutletNode.XPos = lNode.XPos
                    lConduit.OutletNode.YPos = lNode.YPos
                End If
            Else
                'use nodes from shapefile 
                If aSWMMProject.Nodes.Contains(lConduit.InletNodeName) Then
                    lConduit.InletNode = aSWMMProject.Nodes(lConduit.InletNodeName)
                End If

                If aSWMMProject.Nodes.Contains(lConduit.OutletNodeName) Then
                    lConduit.OutletNode = aSWMMProject.Nodes(lConduit.OutletNodeName)
                End If
            End If

            'if it doesnt have a name yet, assign it one
            If lConduit.Name.Length = 0 Then
                lConduit.Name = "C" & CStr(lFeatureIndex + 1)
            ElseIf IsNumeric(lConduit.Name) Then
                lConduit.Name = "C" & lConduit.Name
            End If

            'find the coordinates
            GisUtil.PointsOfLine(lLayerIndex, lFeatureIndex, lConduit.X, lConduit.Y)
        Next
        Logger.Dbg("Completed " & GisUtil.NumFeatures(lLayerIndex) & " ConduitsFromShapeFile")
    End Function

    Public Function CompleteNodesFromShapefile(ByVal aNodeShapefileName As String, _
                                               ByRef aNodes As atcSWMMNodes) As Boolean

        Logger.Dbg("ShapefileName " & aNodeShapefileName & " NodeCount " & aNodes.Count)
        If aNodes.Count > 0 Then
            If Not GisUtil.IsLayerByFileName(aNodeShapefileName) Then
                If GisUtil.AddLayer(aNodeShapefileName, "Nodes") Then
                    Logger.Dbg("LayerAdded")
                Else
                    Logger.Msg("ProblemAddingNodeLayer") 'Exception?
                End If
            End If
            Dim lLayerIndex As Integer = GisUtil.LayerIndex(aNodeShapefileName)

            For lFeatureIndex As Integer = 0 To GisUtil.NumFeatures(lLayerIndex) - 1
                Dim lNode As atcSWMMNode = aNodes(lFeatureIndex)
                'if it doesnt have a name yet, assign it one
                If lNode.Name.Length = 0 Then
                    lNode.Name = "N" & CInt(lFeatureIndex)
                End If
                'find the coordinates
                GisUtil.PointXY(lLayerIndex, lFeatureIndex, lNode.XPos, lNode.YPos)
            Next
        End If

    End Function

    Public Function CreateRaingageFromShapefile(ByVal aShapefileName As String, _
                                                ByVal aGageId As String, _
                                                ByRef aRainGages As atcSWMMRainGages) As Boolean
        Logger.Dbg("ShapefileName " & aShapefileName & " GageID " & aGageId)
        If Not GisUtil.IsLayerByFileName(aShapefileName) Then
            If GisUtil.AddLayer(aShapefileName, "Raingages") Then
                Logger.Dbg("LayerAdded")
            Else
                Logger.Msg("ProblemAddingRaingageLayer") 'Exception?
            End If
        End If

        Dim lLayerIndex As Integer = GisUtil.LayerIndex(aShapefileName)
        Dim lGageIDFieldIndex As Integer = GisUtil.FieldIndex(lLayerIndex, "LOCATION")
        Dim lWDMFileName As String = FilenameNoExt(aShapefileName) & ".wdm"
        For lFeatureIndex As Integer = 0 To GisUtil.NumFeatures(lLayerIndex) - 1
            If GisUtil.FieldValue(lLayerIndex, lFeatureIndex, lGageIDFieldIndex) = aGageId Then
                Dim lRaingage As New atcSWMMRainGage
                lRaingage.Name = aGageId
                'lRaingage.Form
                'lRaingage.Interval()
                'lRaingage.SnowCatchFactor()
                'lRaingage.Type
                'lRaingage.Units
                lRaingage.TimeSeries = GetTimeseries(lWDMFileName, "OBSERVED", aGageId, "PREC")
                If lRaingage.TimeSeries = Nothing Then
                    lRaingage.TimeSeries = GetTimeseries(lWDMFileName, "COMPUTED", aGageId, "PREC")
                End If
                GisUtil.PointXY(lLayerIndex, lFeatureIndex, lRaingage.XPos, lRaingage.YPos)
                If Not aRainGages.Contains(lRaingage.Name) Then
                    aRainGages.Add(lRaingage)
                End If
            End If
        Next
        Logger.Dbg("CreateRaingageFromShapefile Count " & aRainGages.Count)
    End Function

    Friend Function CreateLandusesFromGrid(ByVal aLanduseGridFileName As String, _
                                           ByVal aSubbasinShapefileName As String, _
                                           ByVal aCatchments As atcSWMMCatchments, _
                                           ByRef aLanduses As atcSWMMLanduses) As Boolean
        Logger.Dbg("GridFile " & aLanduseGridFileName & _
                   " Catchment " & aSubbasinShapefileName & _
                   " CatchmentCount " & aCatchments.Count)
        aLanduses.Clear()

        If Not GisUtil.IsLayerByFileName(aSubbasinShapefileName) Then
            If GisUtil.AddLayer(aSubbasinShapefileName, "Catchments") Then
                Logger.Dbg("CatchmentLayerAdded")
            Else
                Logger.Msg("ProblemCatchmentsLayer") 'Exception?
            End If
        End If
        Dim lSubbasinLayerIndex As Integer = GisUtil.LayerIndex(aSubbasinShapefileName)

        If Not GisUtil.IsLayerByFileName(aLanduseGridFileName) Then
            If GisUtil.AddLayer(aLanduseGridFileName, "Landuse Grid") Then
                Logger.Dbg("LayerLanduseGridAdded")
            Else
                Logger.Msg("ProblemLanduseGridLayer") 'Exception?
            End If
        End If
        Dim lLanduseLayerIndex As Integer = GisUtil.LayerIndex(aLanduseGridFileName)

        Dim lMaxLanduseCategory As Integer = Convert.ToInt32(GisUtil.GridLayerMaximum(lLanduseLayerIndex))
        Dim lAreaLS(lMaxLanduseCategory, GisUtil.NumFeatures(lSubbasinLayerIndex)) As Double
        GisUtil.TabulateAreas(lLanduseLayerIndex, lSubbasinLayerIndex, lAreaLS)

        For lCatchmentIndex As Integer = 0 To aCatchments.Count - 1
            For lLanduseCategory As Integer = 1 To lMaxLanduseCategory
                If lAreaLS(lLanduseCategory, lCatchmentIndex) > 0 Then
                    Dim lLanduse As New atcSWMMLanduse
                    lLanduse.Area = lAreaLS(lLanduseCategory, lCatchmentIndex)
                    lLanduse.Name = lLanduseCategory
                    lLanduse.Catchment = aCatchments(lCatchmentIndex)
                    aLanduses.Add(lLanduse)
                End If
            Next
        Next

    End Function

    Friend Function CreateLandusesFromGIRAS(ByVal aSubbasinShapefileName As String, _
                                            ByVal aSubbasinFieldName As String, _
                                            ByVal aCatchments As atcSWMMCatchments, _
                                            ByRef aLanduses As atcSWMMLanduses) As Boolean
        'perform overlay for GIRAS shapefiles
        Logger.Dbg("Begin")
        aLanduses.Clear()

        If Not GisUtil.IsLayerByFileName(aSubbasinShapefileName) Then
            GisUtil.AddLayer(aSubbasinShapefileName, "Catchments")
        End If
        Dim lSubbasinLayerIndex As Integer = GisUtil.LayerIndex(aSubbasinShapefileName)
        Dim lNumSubbasins As Integer = GisUtil.NumFeatures(lSubbasinLayerIndex)

        'set land use index layer
        Dim lLandUseThemeName As String = "Land Use Index"
        Dim lLanduseFieldName As String = "COVNAME"
        Dim lLanduseLayerIndex As Integer = GisUtil.LayerIndex(lLandUseThemeName)
        Dim lLandUseFieldIndex As Integer = GisUtil.FieldIndex(lLanduseLayerIndex, lLanduseFieldName)
        Dim lLandUsePathName As String = PathNameOnly(GisUtil.LayerFileName(lLanduseLayerIndex)) & "\landuse"

        'figure out which land use tiles to overlay
        Dim lLandUseTiles As New atcCollection
        For lLanduseTileIndex As Integer = 1 To GisUtil.NumFeatures(lLanduseLayerIndex)
            'loop thru each shape of land use index shapefile
            For lSubbasinIndex As Integer = 1 To lNumSubbasins
                'loop thru each subbasin 
                If GisUtil.OverlappingPolygons(lLanduseLayerIndex, lLanduseTileIndex - 1, lSubbasinLayerIndex, lSubbasinIndex - 1) Then
                    'add this to collection of tiles we'll need
                    lLandUseTiles.Add(GisUtil.FieldValue(lLanduseLayerIndex, lLanduseTileIndex - 1, lLandUseFieldIndex))
                End If
            Next
        Next

        'add tiles if not already on map
        'figure out how many polygons to overlay, for status message
        Dim lTotalPolygonCount As Integer = 0
        Dim lTileFileNames As New atcCollection
        For Each lLandUseTile As String In lLandUseTiles
            Dim lNewFileName As String = lLandUsePathName & g_PathChar & lLandUseTile & ".shp"
            lTileFileNames.Add(lNewFileName)
            If Not GisUtil.IsLayerByFileName(lNewFileName) Then
                If Not GisUtil.AddLayer(lNewFileName, lLandUseTile) Then
                    Logger.Msg("The GIRAS Landuse Shapefile " & lNewFileName & "does not exist." & _
                                vbCrLf & "Run the Download tool to bring this data into your project.", vbOKOnly, "SWMM Setup Problem")
                    Exit Function
                End If
            End If
            lTotalPolygonCount += GisUtil.NumFeatures(GisUtil.LayerIndex(lNewFileName))
        Next
        lTotalPolygonCount *= lNumSubbasins

        lLanduseFieldName = "LUCODE"
        Dim lFirst As Boolean = True
        Dim lTileIndex As Integer = 0
        For Each lTileFileName As String In lTileFileNames
            lTileIndex += 1
            'lblStatus.Text = "Overlaying Land Use and Subbasins (Tile " & lTileIndex & " of " & lTileFileNames.Count & ")"
            'Me.Refresh()
            'do overlay
            GisUtil.Overlay(lTileFileName, lLanduseFieldName, aSubbasinShapefileName, aSubbasinFieldName, _
                            lLandUsePathName & "\overlay.shp", lFirst)
            lFirst = False
        Next

        Dim lTable As IatcTable = atcUtility.atcTableOpener.OpenAnyTable(lLandUsePathName & "\overlay.dbf")
        For lRecordIndex As Integer = 1 To lTable.NumRecords
            lTable.CurrentRecord = lRecordIndex
            Dim lLanduse As New atcSWMMLanduse
            lLanduse.Area = CDbl(lTable.Value(3))
            lLanduse.Name = lTable.Value(1)
            'find associated catchment
            For Each lCatchment As atcSWMMCatchment In aCatchments
                If lCatchment.Name = lTable.Value(2) Or lCatchment.Name = "S" & lTable.Value(2) Then
                    lLanduse.Catchment = lCatchment
                End If
            Next
            Dim lKey As String = lLanduse.Name & ":" & lLanduse.Catchment.Name
            If aLanduses.Contains(lKey) Then
                aLanduses(lKey).Area += lLanduse.Area
            Else
                aLanduses.Add(lLanduse)
            End If

        Next lRecordIndex
    End Function

    Friend Function CreateLandusesFromShapefile(ByVal aLanduseShapefileName As String, _
                                                ByVal aLanduseFieldName As String, _
                                                ByVal aSubbasinShapefileName As String, _
                                                ByVal aSubbasinFieldName As String, _
                                                ByVal aCatchments As atcSWMMCatchments, _
                                                ByRef aLanduses As atcSWMMLanduses) As Boolean
        'perform overlay for other shapefiles (not GIRAS) 
        Logger.Dbg("Begin")
        aLanduses.Clear()

        If Not GisUtil.IsLayerByFileName(aSubbasinShapefileName) Then
            GisUtil.AddLayer(aSubbasinShapefileName, "Catchments")
        End If
        Dim lSubbasinLayerIndex As Integer = GisUtil.LayerIndex(aSubbasinShapefileName)

        If Not GisUtil.IsLayerByFileName(aLanduseShapefileName) Then
            GisUtil.AddLayer(aLanduseShapefileName, "Landuse Shapefile")
        End If
        Dim lLanduseLayerIndex As Integer = GisUtil.LayerIndex(aLanduseShapefileName)
        Dim lLandUsePathName As String = PathNameOnly(GisUtil.LayerFileName(lLanduseLayerIndex))

        'do overlay
        GisUtil.Overlay(aLanduseShapefileName, aLanduseFieldName, aSubbasinShapefileName, aSubbasinFieldName, _
                        lLandUsePathName & "\overlay.shp", True)

        Dim lTable As IatcTable = atcUtility.atcTableOpener.OpenAnyTable(lLandUsePathName & "\overlay.dbf")
        For lRecordIndex As Integer = 1 To lTable.NumRecords
            lTable.CurrentRecord = lRecordIndex
            Dim lLanduse As New atcSWMMLanduse
            lLanduse.Area = CDbl(lTable.Value(3))
            lLanduse.Name = lTable.Value(1)
            'find associated catchment
            For Each lCatchment As atcSWMMCatchment In aCatchments
                If lCatchment.Name = lTable.Value(2) Or lCatchment.Name = "S" & lTable.Value(2) Then
                    lLanduse.Catchment = lCatchment
                End If
            Next
            Dim lKey As String = lLanduse.Name & ":" & lLanduse.Catchment.Name
            If aLanduses.Contains(lKey) Then
                aLanduses(lKey).Area += lLanduse.Area
            Else
                aLanduses.Add(lLanduse)
            End If
        Next lRecordIndex
    End Function

    Public Function FindTimeseries(ByVal aWDMFileName As String, _
                                   ByVal aGageId As String, _
                                   ByVal aConstituentID As String) As atcTimeseries
        Logger.Dbg("Begin")
        Dim lMetConstituent As atcData.atcTimeseries = GetTimeseries(aWDMFileName, "OBSERVED", aGageId, aConstituentID)
        If lMetConstituent Is Nothing Then
            lMetConstituent = GetTimeseries(aWDMFileName, "COMPUTED", aGageId, aConstituentID)
        End If
        Return lMetConstituent
    End Function

    Private Function GetTimeseries(ByRef aMetWDMName As String, _
                                   ByVal aScenario As String, _
                                   ByVal aLocation As String, _
                                   ByVal aConstituent As String) As atcData.atcTimeseries
        Logger.Dbg("Begin " & aScenario & " Location " & aLocation & " Constituent " & aConstituent)
        Dim lGetTimeseries As atcData.atcTimeseries = Nothing

        Dim lDataSource As atcWDM.atcDataSourceWDM
        If FileExists(aMetWDMName) Then
            lDataSource = atcDataManager.DataSourceBySpecification(aMetWDMName)

            If lDataSource Is Nothing Then 'need to open it here
                lDataSource = New atcWDM.atcDataSourceWDM
                If Not lDataSource.Open(aMetWDMName) Then
                    lDataSource = Nothing
                End If
            End If

            If lDataSource IsNot Nothing Then
                For Each lDataSet As atcData.atcTimeseries In lDataSource.DataSets
                    If (lDataSet.Attributes.GetValue("Scenario") = aScenario And _
                        lDataSet.Attributes.GetValue("Constituent") = aConstituent And _
                        lDataSet.Attributes.GetValue("Location") = aLocation) Then
                        lGetTimeseries = lDataSet
                        Exit For
                    End If
                Next
            End If
        End If
        Return lGetTimeseries
    End Function

    Friend Function ReclassifyLandUses(ByVal aReclassificationRecords As atcCollection, _
                                       ByVal aLandUses As atcSWMMLanduses, _
                                       ByVal aSWMMProject As atcSWMMProject) As atcSWMMLanduses
        Logger.Dbg("Begin")
        Dim lReclassifyLandUses As New atcSWMMLanduses(aSWMMProject)

        'build collection of unique subbasin ids
        Dim lUniqueSubids As New atcCollection
        For Each lLandUse As atcSWMMLanduse In aLandUses
            lUniqueSubids.Add(lLandUse.Catchment.Name)
        Next

        'build collection of unique landuse groups
        Dim lUniqueLugroups As New atcCollection
        For Each lDetail As LanduseReclassificationDetails In aReclassificationRecords
            lUniqueLugroups.Add(lDetail.GroupDescription)
        Next

        'create summary arrays
        Dim lPerArea(lUniqueSubids.Count, lUniqueLugroups.Count) As Double
        Dim lImpArea(lUniqueSubids.Count, lUniqueLugroups.Count) As Double

        For Each lLandUse As atcSWMMLanduse In aLandUses
            'loop through each polygon (or grid subid/lucode combination)
            'find subbasin position in the area array
            Dim lSpos As Integer
            For j As Integer = 0 To lUniqueSubids.Count - 1
                If lLandUse.Catchment.Name = lUniqueSubids(j) Then
                    lSpos = j
                    Exit For
                End If
            Next j

            'find lugroup that corresponds to this lucode, could be multiple matches
            For Each lDetail As LanduseReclassificationDetails In aReclassificationRecords
                Dim lLandUseName As String = ""
                Dim lLpos As Integer = -1
                Dim lPercentImperv As Double
                If lDetail.Code.Trim.Length > 0 Then
                    If lLandUse.Name = lDetail.Code Then
                        'see if any of these are subbasin-specific
                        If Not Double.TryParse(lDetail.ImperviousPercent, lPercentImperv) Then
                            Logger.Dbg("Warning: non-parsable percent impervious value '" & lDetail.ImperviousPercent & "' for land use name " & lLandUse.Name)
                        Else
                            Dim lMultiplier As Double
                            If Not Double.TryParse(lDetail.Multiplier, lMultiplier) Then
                                lMultiplier = 1.0
                            End If
                            Dim lSubbasin As String = lDetail.Subbasin
                            Dim lSubbasinSpecific As Boolean = False
                            If Not lSubbasin Is Nothing Then
                                If lSubbasin.Trim.Length > 0 Then
                                    lSubbasinSpecific = True
                                End If
                            End If
                            If lSubbasinSpecific Then
                                'this row is subbasin-specific
                                If lSubbasin = lLandUse.Catchment.Name Then
                                    lLandUseName = lDetail.GroupDescription
                                End If
                            Else
                                'make sure that no other rows of this lucode are 
                                'subbasin-specific for this subbasin and that we 
                                'should therefore not use this row
                                Dim lUseIt As Boolean = True
                                For Each lDetail2 As LanduseReclassificationDetails In aReclassificationRecords
                                    If Not lDetail2.Equals(lDetail) Then
                                        If lDetail2.Code = lDetail.Code And lDetail2.GroupDescription = lDetail.GroupDescription Then
                                            'this other row has same lucode and group description
                                            lSubbasin = lDetail2.Subbasin
                                            If lSubbasin IsNot Nothing AndAlso IsNumeric(lSubbasin) Then
                                                'and its subbasin-specific
                                                If lSubbasin = lLandUse.Catchment.Name Then
                                                    'and its specific to this subbasin
                                                    lUseIt = False
                                                End If
                                            End If
                                        End If
                                    End If
                                Next
                                If lUseIt Then 'we want this one now
                                    lLandUseName = lDetail.GroupDescription
                                End If
                            End If

                            lLpos = -1
                            If lLandUseName.Length > 0 Then 'find lugroup position in the area array
                                For j As Integer = 0 To lUniqueLugroups.Count - 1
                                    If lLandUseName = lUniqueLugroups(j) Then
                                        lLpos = j
                                        Exit For
                                    End If
                                Next j
                            End If

                            If lLpos > -1 Then
                                With lLandUse
                                    lPerArea(lSpos, lLpos) += (.Area * lMultiplier * (100 - lPercentImperv) / 100)
                                    lImpArea(lSpos, lLpos) += (.Area * lMultiplier * lPercentImperv / 100)
                                End With
                            End If
                        End If
                    End If
                End If
            Next
        Next lLandUse

        For lSpos As Integer = 0 To lUniqueSubids.Count - 1
            Dim lPerAreaBySubbasin As Double = 0
            Dim lImpAreaBySubbasin As Double = 0
            For lLpos As Integer = 0 To lUniqueLugroups.Count - 1
                lPerAreaBySubbasin = lPerAreaBySubbasin + lPerArea(lSpos, lLpos)
                lImpAreaBySubbasin = lImpAreaBySubbasin + lImpArea(lSpos, lLpos)
                Dim lArea As Double = lPerArea(lSpos, lLpos) + lImpArea(lSpos, lLpos)
                If lArea > 0 Then
                    Dim lLandUse As New atcSWMMLanduse
                    With lLandUse
                        .Name = lUniqueLugroups(lLpos)
                        .Area = lArea
                        For Each lOrigLandUse As atcSWMMLanduse In aLandUses
                            If lOrigLandUse.Catchment.Name = lUniqueSubids(lSpos) Then
                                .Catchment = lOrigLandUse.Catchment
                                .Catchment.PercentImpervious = lImpAreaBySubbasin / (lPerAreaBySubbasin + lImpAreaBySubbasin) * 100
                                Exit For
                            End If
                        Next
                        'swmm file is space delimited, must not use spaces or commas in land use names
                        .Name = ReplaceString(.Name, " ", "")
                        .Name = ReplaceString(.Name, ",", "_")
                    End With
                    lReclassifyLandUses.Add(lLandUse)
                End If
            Next
        Next

        Return lReclassifyLandUses
    End Function

    Friend Sub BuildListofValidStationNames(ByRef aMetWDMName As String, _
                                            ByRef aMetConstituent As String, _
                                            ByVal aStations As atcCollection)
        Logger.Dbg("MetWDMName " & aMetWDMName & " Constituent " & aMetConstituent)
        aStations.Clear()
        Dim lDataSource As atcWDM.atcDataSourceWDM = Nothing
        If FileExists(aMetWDMName) Then
            lDataSource = atcDataManager.DataSourceBySpecification(aMetWDMName)

            If lDataSource Is Nothing Then 'need to open it here
                lDataSource = New atcWDM.atcDataSourceWDM
                If Not lDataSource.Open(aMetWDMName) Then
                    lDataSource = Nothing
                End If
            End If

            If lDataSource Is Nothing Then
                Logger.Dbg("WDMFile Not found") 'FindFile?
            Else
                Dim lCounter As Integer = 0
                Logger.Dbg("LookThrough " & lDataSource.DataSets.Count & " Datasets")
                For Each lDataSet As atcData.atcTimeseries In lDataSource.DataSets
                    lCounter += 1
                    Logger.Progress("Building list of valid station names...", lCounter, lDataSource.DataSets.Count)

                    If (lDataSet.Attributes.GetValue("Scenario") = "OBSERVED" Or lDataSet.Attributes.GetValue("Scenario") = "COMPUTED") _
                        And lDataSet.Attributes.GetValue("Constituent") = aMetConstituent Then
                        Dim lLoc As String = lDataSet.Attributes.GetValue("Location")
                        Dim lStanam As String = lDataSet.Attributes.GetValue("Stanam")
                        Dim lDsn As Integer = lDataSet.Attributes.GetValue("Id")
                        Dim lSJDay As Double
                        Dim lEJDay As Double
                        lSJDay = lDataSet.Attributes.GetValue("Start Date", 0)
                        lEJDay = lDataSet.Attributes.GetValue("End Date", 0)
                        If lSJDay = 0 Then
                            lSJDay = lDataSet.Dates.Value(0)
                        End If
                        If lEJDay = 0 Then
                            lEJDay = lDataSet.Dates.Value(lDataSet.Dates.numValues)
                        End If
                        Dim lAddIt As Boolean = True

                        If aMetConstituent = "PEVT" Then
                            'special case, see if ATEM dataset exists at the same location
                            lAddIt = False
                            For Each lDataSet2 As atcData.atcTimeseries In lDataSource.DataSets
                                If lDataSet2.Attributes.GetValue("Constituent") = "ATEM" And _
                                   lDataSet2.Attributes.GetValue("Location") = lLoc Then
                                    Dim lSJDay2 As Double
                                    Dim lEJDay2 As Double
                                    lSJDay2 = lDataSet2.Attributes.GetValue("Start Date", 0)
                                    lEJDay2 = lDataSet2.Attributes.GetValue("End Date", 0)
                                    If lSJDay2 = 0 Then
                                        lSJDay2 = lDataSet2.Dates.Value(0)
                                    End If
                                    If lEJDay2 = 0 Then
                                        lEJDay2 = lDataSet2.Dates.Value(lDataSet2.Dates.numValues)
                                    End If
                                    If lSJDay2 > lSJDay Then lSJDay = lSJDay2
                                    If lEJDay2 < lEJDay Then lEJDay = lEJDay2
                                    lAddIt = True
                                    'set valuesneedtoberead so that the dates and values will be forgotten, to free up memory
                                    lDataSet2.ValuesNeedToBeRead = True
                                    Exit For
                                End If
                            Next
                        End If

                        'if this one is computed and observed also exists at same location, just use observed
                        If lDataSet.Attributes.GetValue("Scenario") = "COMPUTED" Then
                            For Each lDataSet2 As atcData.atcTimeseries In lDataSource.DataSets
                                If lDataSet2.Attributes.GetValue("Constituent") = aMetConstituent And _
                                   lDataSet2.Attributes.GetValue("Scenario") = "OBSERVED" And _
                                   lDataSet2.Attributes.GetValue("Location") = lLoc Then
                                    lAddIt = False
                                    Exit For
                                End If
                            Next
                        End If

                        If lAddIt Then
                            Dim lSdate(6) As Integer
                            Dim lEdate(6) As Integer
                            J2Date(lSJDay, lSdate)
                            J2Date(lEJDay, lEdate)
                            Dim lDateString As String = "(" & lSdate(0) & "/" & lSdate(1) & "/" & lSdate(2) & "-" & lEdate(0) & "/" & lEdate(1) & "/" & lEdate(2) & ")"
                            Dim lStationDetails As New StationDetails
                            lStationDetails.Name = lLoc
                            lStationDetails.StartJDate = lSJDay
                            lStationDetails.EndJDate = lEJDay
                            lStationDetails.Description = lLoc & ":" & lStanam & " " & lDateString
                            aStations.Add(lStationDetails.Description, lStationDetails)
                            'Logger.Dbg("Added " & lStationDetails.Description)
                        End If
                    End If
                    'set valuesneedtoberead so that the dates and values will be forgotten, to free up memory
                    lDataSet.ValuesNeedToBeRead = True
                Next
            End If
        End If
        lDataSource = Nothing
        Logger.Dbg("Found " & aStations.Count & " Stations")
    End Sub

    Friend Sub GetLanduseReclassificationDetails(ByVal aReclassifyFile As String, _
                                                 ByVal aCodesVisible As Boolean, _
                                                 ByRef aGridSource As atcControls.atcGridSource, _
                                                 ByVal aLanduseReclassificationDetails As atcCollection)

        If aReclassifyFile.Length > 0 And aReclassifyFile <> "<none>" And Not aCodesVisible Then
            'have the simple percent pervious grid, need to know which 
            'lucodes correspond to which lugroups from dbf file
            Dim lTable As IatcTable = atcTableOpener.OpenAnyTable(aReclassifyFile)
            For lTableRecordIndex As Integer = 1 To lTable.NumRecords
                lTable.CurrentRecord = lTableRecordIndex
                'lRcode.Add(lTable.Value(1), lTable.Value(2))
                Dim lLanduseReclassificationDetail As New LanduseReclassificationDetails
                With lLanduseReclassificationDetail
                    .Code = lTable.Value(1)
                    If .Code Is Nothing Then .Code = ""
                    .GroupDescription = lTable.Value(2)
                End With
                aLanduseReclassificationDetails.Add(lLanduseReclassificationDetail)
            Next lTableRecordIndex
            'now fill in the rest from the grid
            For lRow As Integer = 1 To aGridSource.Rows
                For Each lDetail As LanduseReclassificationDetails In aLanduseReclassificationDetails
                    With lDetail
                        If .GroupDescription = aGridSource.CellValue(lRow, 1) Then
                            'descriptions match, fill in the rest
                            .ImperviousPercent = aGridSource.CellValue(lRow, 2)
                            .Multiplier = aGridSource.CellValue(lRow, 3)
                            If .Multiplier Is Nothing Then .Multiplier = ""
                            .Subbasin = aGridSource.CellValue(lRow, 4)
                            If .Subbasin Is Nothing Then .Subbasin = ""
                        End If
                    End With
                Next
            Next
        Else
            'the more complex type, fill in from the grid
            For lRow As Integer = 1 To aGridSource.Rows
                Dim lLanduseReclassificationDetail As New LanduseReclassificationDetails
                With lLanduseReclassificationDetail
                    .Code = aGridSource.CellValue(lRow, 0)
                    If .Code Is Nothing Then .Code = ""
                    .GroupDescription = aGridSource.CellValue(lRow, 1)
                    .ImperviousPercent = aGridSource.CellValue(lRow, 2)
                    .Multiplier = aGridSource.CellValue(lRow, 3)
                    If .Multiplier Is Nothing Then .Multiplier = ""
                    .Subbasin = aGridSource.CellValue(lRow, 4)
                    If .Subbasin Is Nothing Then .Subbasin = ""
                End With
                aLanduseReclassificationDetails.Add(lLanduseReclassificationDetail)
            Next
        End If

    End Sub

    Public Sub UseDefaultsForAttributes(ByVal aFieldDetails As atcSWMMFieldDetails, ByRef aEntities As Object)
        'for any value that didn't come from the attribute table, use user-supplied default
        For Each lEntity As Object In aEntities
            For Each lField As System.Reflection.FieldInfo In lEntity.GetType.GetFields()
                If lField.FieldType.Name = "String" Then
                    If lField.GetValue(lEntity) Is Nothing OrElse lField.GetValue(lEntity).ToString.Length = 0 Then
                        'the value is blank, use default
                        If aFieldDetails.Contains(lField.Name) Then
                            If aFieldDetails(lField.Name).DefaultValue.Length > 0 Then
                                lField.SetValue(lEntity, aFieldDetails(lField.Name).DefaultValue)
                            End If
                        End If
                    End If
                ElseIf lField.FieldType.Name = "Integer" Or lField.FieldType.Name = "Int32" Then
                    If lField.GetValue(lEntity) Is Nothing OrElse lField.GetValue(lEntity) < 0 Then
                        'the value is less than zero, use default
                        If aFieldDetails.Contains(lField.Name) Then
                            If IsNumeric(aFieldDetails(lField.Name).DefaultValue) AndAlso CInt(aFieldDetails(lField.Name).DefaultValue) >= 0 Then
                                lField.SetValue(lEntity, CInt(aFieldDetails(lField.Name).DefaultValue))
                            End If
                        End If
                    End If
                ElseIf lField.FieldType.Name = "Double" Then
                    If lField.GetValue(lEntity) Is Nothing OrElse lField.GetValue(lEntity) < 0 Then
                        'the value is less than zero, use default
                        If aFieldDetails.Contains(lField.Name) Then
                            If IsNumeric(aFieldDetails(lField.Name).DefaultValue) AndAlso CDbl(aFieldDetails(lField.Name).DefaultValue) >= 0 Then
                                lField.SetValue(lEntity, CDbl(aFieldDetails(lField.Name).DefaultValue))
                            End If
                        End If
                    End If
                End If
            Next
        Next
    End Sub

    Friend Class StationDetails
        Public Name As String
        Public StartJDate As Double
        Public EndJDate As Double
        Public Description As String
    End Class

    Friend Class LanduseReclassificationDetails
        Public Code As String
        Public GroupDescription As String
        Public ImperviousPercent As String
        Public Multiplier As String
        Public Subbasin As String
    End Class

End Module
