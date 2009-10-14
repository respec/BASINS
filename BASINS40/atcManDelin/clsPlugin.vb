Imports atcUtility
Imports atcMwGisUtility
Imports MapWinUtility

Public Class PlugIn
    Implements MapWindow.Interfaces.IPlugin

    Private pMapWin As MapWindow.Interfaces.IMapWin
    Private pFrmManDelin As frmManDelin
    Private pInitialized As Boolean

    'TODO: get these 3 from BASINS4 or plugInManager?
    Private Const DelineateMenuName As String = "btdmWatershedDelin"
    Private Const DelineateMenuString As String = "Watershed Delineation"

    Public ReadOnly Property Name() As String Implements MapWindow.Interfaces.IPlugin.Name
        Get
            Return "Manual Delineation"
        End Get
    End Property

    Public ReadOnly Property Author() As String Implements MapWindow.Interfaces.IPlugin.Author
        Get
            Return "AQUA TERRA Consultants"
        End Get
    End Property

    Public ReadOnly Property SerialNumber() As String Implements MapWindow.Interfaces.IPlugin.SerialNumber
        Get
            Return "G14R/KCU1FOWVVI"
        End Get
    End Property

    Public ReadOnly Property Description() As String Implements MapWindow.Interfaces.IPlugin.Description
        Get
            Return "This plug-in provides an interface for manually delineating watersheds."
        End Get
    End Property

    Public ReadOnly Property BuildDate() As String Implements MapWindow.Interfaces.IPlugin.BuildDate
        Get
            Return System.IO.File.GetLastWriteTime(Me.GetType().Assembly.Location)
        End Get
    End Property

    Public ReadOnly Property Version() As String Implements MapWindow.Interfaces.IPlugin.Version
        Get
            Return System.Diagnostics.FileVersionInfo.GetVersionInfo(Me.GetType().Assembly.Location).FileVersion
        End Get
    End Property

    <CLSCompliant(False)> _
    Public Sub Initialize(ByVal MapWin As MapWindow.Interfaces.IMapWin, ByVal ParentHandle As Integer) Implements MapWindow.Interfaces.IPlugin.Initialize
        pMapWin = MapWin
        pMapWin.Menus.AddMenu(DelineateMenuName, "", Nothing, DelineateMenuString, "mnuFile")
        Dim lMenuItem As MapWindow.Interfaces.MenuItem
        lMenuItem = pMapWin.Menus.AddMenu(DelineateMenuName & "_ManDelin", DelineateMenuName, Nothing, "&Manual")
        pInitialized = False
    End Sub

    Public Sub ItemClicked(ByVal ItemName As String, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.ItemClicked
        If ItemName = DelineateMenuName & "_ManDelin" Then
            Dim lCreateNew As Boolean = True
            If Not pFrmManDelin Is Nothing Then
                If pFrmManDelin.Visible = True Then
                    pFrmManDelin.BringToFront()
                    lCreateNew = False
                End If
            End If
            If lCreateNew Then
                pFrmManDelin = New frmManDelin
                pFrmManDelin.Initialize(pMapWin)
                pFrmManDelin.Show()
                pInitialized = True
                Handled = True
            End If
        End If
    End Sub

    Public Sub TestClip()
        Dim lLineShapeFile As New MapWinGIS.Shapefile
        If lLineShapeFile.Open("c:\temp\temp1.shp") Then
            Dim lLineShape As MapWinGIS.Shape = lLineShapeFile.Shape(0)
            Dim lPolyShapefile As New MapWinGIS.Shapefile
            If lPolyShapefile.Open("c:\temp\temp2.shp") Then
                Dim lPolyShape As MapWinGIS.Shape = lPolyShapefile.Shape(0)
                Dim lSuccess As Boolean = MapWinGeoProc.SpatialOperations.ClipPolygonWithLine(lPolyShape, lLineShape, "c:\temp\temp.shp")
            End If
        End If
    End Sub

    Public Sub MapMouseMove(ByVal ScreenX As Integer, ByVal ScreenY As Integer, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.MapMouseMove
        If pInitialized Then
            Dim lPixelX, lPixelY As Double
            pMapWin.View.PixelToProj(ScreenX, ScreenY, lPixelX, lPixelY)
            pFrmManDelin.MouseDrawingMove(lPixelX, lPixelY)
        End If
    End Sub

    Public Sub MapMouseUp(ByVal Button As Integer, ByVal Shift As Integer, ByVal x As Integer, ByVal y As Integer, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.MapMouseUp
        If pInitialized Then
            Dim lPixelX, lPixelY As Double
            pMapWin.View.PixelToProj(x, y, lPixelX, lPixelY)
            pFrmManDelin.MouseButtonClickUp(lPixelX, lPixelY, Button)
        End If
    End Sub

    Public Sub Terminate() Implements MapWindow.Interfaces.IPlugin.Terminate
    End Sub

    Public Sub LayerRemoved(ByVal Handle As Integer) Implements MapWindow.Interfaces.IPlugin.LayerRemoved
    End Sub

    <CLSCompliant(False)> _
    Public Sub LayersAdded(ByVal Layers() As MapWindow.Interfaces.Layer) Implements MapWindow.Interfaces.IPlugin.LayersAdded
    End Sub

    Public Sub LayersCleared() Implements MapWindow.Interfaces.IPlugin.LayersCleared
    End Sub

    Public Sub LayerSelected(ByVal Handle As Integer) Implements MapWindow.Interfaces.IPlugin.LayerSelected
    End Sub

    <CLSCompliant(False)> _
    Public Sub LegendDoubleClick(ByVal Handle As Integer, ByVal Location As MapWindow.Interfaces.ClickLocation, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.LegendDoubleClick
    End Sub

    <CLSCompliant(False)> _
    Public Sub LegendMouseDown(ByVal Handle As Integer, ByVal Button As Integer, ByVal Location As MapWindow.Interfaces.ClickLocation, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.LegendMouseDown
    End Sub

    <CLSCompliant(False)> _
    Public Sub LegendMouseUp(ByVal Handle As Integer, ByVal Button As Integer, ByVal Location As MapWindow.Interfaces.ClickLocation, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.LegendMouseUp
    End Sub

    Public Sub MapDragFinished(ByVal Bounds As System.Drawing.Rectangle, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.MapDragFinished
    End Sub

    Public Sub MapExtentsChanged() Implements MapWindow.Interfaces.IPlugin.MapExtentsChanged
    End Sub

    Public Sub MapMouseDown(ByVal Button As Integer, ByVal Shift As Integer, ByVal x As Integer, ByVal y As Integer, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.MapMouseDown
    End Sub

    Public Sub Message(ByVal msg As String, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.Message
    End Sub

    Public Sub ProjectLoading(ByVal ProjectFile As String, ByVal SettingsString As String) Implements MapWindow.Interfaces.IPlugin.ProjectLoading
    End Sub

    Public Sub ProjectSaving(ByVal ProjectFile As String, ByRef SettingsString As String) Implements MapWindow.Interfaces.IPlugin.ProjectSaving
    End Sub

    <CLSCompliant(False)> _
    Public Sub ShapesSelected(ByVal Handle As Integer, ByVal SelectInfo As MapWindow.Interfaces.SelectInfo) Implements MapWindow.Interfaces.IPlugin.ShapesSelected
    End Sub
End Class

Module ManDelin
    Public Sub CalculateSubbasinParameters(ByVal aSubBasinThemeName As String, ByVal aElevationThemeName As String, _
                                           Optional ByVal aElevUnits As String = "Meters")
        Logger.Status("Calculating...")
        Dim lSubbasinLayerIndex As Integer = GisUtil.LayerIndex(aSubBasinThemeName)
        Dim lElevationLayerIndex As Integer = GisUtil.LayerIndex(aElevationThemeName)

        'calculate average elev -- this is not actually used anywhere, so why calculate it
        'does mean elev field exist on subbasin shapefile?
        'MeanElevationFieldIndex = GisUtil.FieldIndex(SubbasinLayerIndex, "MEANELEV")
        'If MeanElevationFieldIndex = -1 Then
        '  'need to add it
        '  MeanElevationFieldIndex = GisUtil.AddField(SubbasinLayerIndex, "MEANELEV", 2, 10)
        'End If
        'If GisUtil.LayerType(ElevationLayerIndex) = 3 Then
        '  'shapefile
        '  ElevationFieldIndex = GisUtil.FieldIndex(ElevationLayerIndex, "ELEV_M")
        '  For i = 1 To GisUtil.NumFeatures(SubbasinLayerIndex)
        '    subbasinArea = 0
        '    weightedElev = 0
        '    For j = 1 To GisUtil.NumFeatures(ElevationLayerIndex)
        '      If GisUtil.OverlappingPolygons(ElevationLayerIndex, j - 1, SubbasinLayerIndex, i - 1) Then
        '        subbasinArea = subbasinArea + GisUtil.AreaNthFeatureInLayer(ElevationLayerIndex, j - 1)
        '        weightedElev = weightedElev + (subbasinArea * GisUtil.FieldValue(ElevationLayerIndex, ElevationFieldIndex, j - 1))
        '      End If
        '    Next j
        '    weightedElev = weightedElev / subbasinArea
        '    'store in mean elevation field
        '    GisUtil.SetFeatureValue(SubbasinLayerIndex, MeanElevationFieldIndex, i - 1, weightedElev)
        '  Next i
        'Else
        '  'grid
        'End If

        Dim lSubbasinFieldIndex As Integer
        If GisUtil.IsField(lSubbasinLayerIndex, "SUBBASIN") Then
            lSubbasinFieldIndex = GisUtil.FieldIndex(lSubbasinLayerIndex, "SUBBASIN")
        Else 'need to add it
            lSubbasinFieldIndex = GisUtil.AddField(lSubbasinLayerIndex, "SUBBASIN", 0, 10)
            Logger.Status("Assigning Subbasin Numbers")
            For lSubBasinIndex As Integer = 1 To GisUtil.NumFeatures(lSubbasinLayerIndex)
                GisUtil.SetFeatureValue(lSubbasinLayerIndex, lSubbasinFieldIndex, lSubBasinIndex - 1, lSubBasinIndex)
            Next lSubBasinIndex
        End If

        'calculate slope
        Dim lSlopeFieldIndex As Integer
        If GisUtil.IsField(lSubbasinLayerIndex, "SLO1") Then
            lSlopeFieldIndex = GisUtil.FieldIndex(lSubbasinLayerIndex, "SLO1")
        Else 'need to add it
            lSlopeFieldIndex = GisUtil.AddField(lSubbasinLayerIndex, "SLO1", 2, 10)
        End If

        Dim lSlope As Double
        If GisUtil.LayerType(lElevationLayerIndex) = 3 Then 'shapefile
            Logger.Status("Calculating Slope from Elevation Shapefile")
            Dim lSubbasinCount As Integer = GisUtil.NumFeatures(lSubbasinLayerIndex)
            Dim lElevationShapeCount As Integer = GisUtil.NumFeatures(lElevationLayerIndex)
            Dim lElevationFieldIndex As Integer = GisUtil.FieldIndex(lElevationLayerIndex, "ELEV_M")
            Dim lElevationIndex(lElevationShapeCount) As Integer
            GisUtil.AssignContainingPolygons(lElevationLayerIndex, lSubbasinLayerIndex, lElevationIndex)
            For lSubbasinIndex As Integer = 1 To lSubbasinCount
                Logger.Progress(lSubbasinIndex, lSubbasinCount)
                Dim lElevMin As Double = 99999999
                Dim lElevMax As Double = -99999999
                Dim lElev As Double
                For lElevationShapeIndex As Integer = 1 To lElevationShapeCount
                    'npercent = 100 * i * j / ntot
                    'If npercent > lastpercent Then
                    '  lblCalc.Text = "Calculating (" & npercent & "%)"
                    '  lastpercent = npercent
                    '  Me.Refresh()
                    'End If
                    If lElevationIndex(lElevationShapeIndex) = lSubbasinIndex - 1 Then
                        lElev = GisUtil.FieldValue(lElevationLayerIndex, lElevationShapeIndex - 1, lElevationFieldIndex)
                        If lElev > lElevMax Then
                            lElevMax = lElev
                        End If
                        If lElev < lElevMin Then
                            lElevMin = lElev
                        End If
                    End If
                Next lElevationShapeIndex
                'store in slope field as percent
                'estimate slope as the difference between max and min elevations / square root of subbasin area -- better approx?
                lSlope = (lElevMax - lElevMin) / ((GisUtil.FeatureArea(lSubbasinLayerIndex, lSubbasinIndex - 1)) ^ 0.5)
                If aElevUnits = "Meters" Then
                    lSlope *= 100
                ElseIf aElevUnits = "Feet" Then
                    lSlope = lSlope * 100 / 3.281
                End If
                GisUtil.SetFeatureValue(lSubbasinLayerIndex, lSlopeFieldIndex, lSubbasinIndex - 1, lSlope)
            Next lSubbasinIndex
        Else 'grid
            Logger.Status("Calculating Slope from Elevation Grid")
            Dim lSubbasinCount As Integer = GisUtil.NumFeatures(lSubbasinLayerIndex)
            For lSubbasinIndex As Integer = 1 To lSubbasinCount
                Logger.Status("Calculating Slope from Elevation Grid")
                Logger.Progress(lSubbasinIndex, lSubbasinCount)
                'store in slope field as percent
                If GisUtil.FieldValue(lSubbasinLayerIndex, lSubbasinIndex - 1, lSlopeFieldIndex) <= 0 Then
                    lSlope = GisUtil.GridSlopeInPolygon(lElevationLayerIndex, lSubbasinLayerIndex, lSubbasinIndex - 1)
                    If aElevUnits = "Meters" Then
                        lSlope *= 100
                    ElseIf aElevUnits = "Feet" Then
                        lSlope = lSlope * 100 / 3.281
                    End If
                    GisUtil.SetFeatureValue(lSubbasinLayerIndex, lSlopeFieldIndex, lSubbasinIndex - 1, lSlope)
                End If
            Next lSubbasinIndex
        End If

        'calculate length of overland flow plane
        'this is computed in WinHSPF based on slope, no need to compute here
        'Dim sl As Double

        'If GisUtil.IsField(SubbasinLayerIndex, "LEN1") Then
        '  LengthFieldIndex = GisUtil.FieldIndex(SubbasinLayerIndex, "LEN1")
        'Else
        '  'need to add it
        '  LengthFieldIndex = GisUtil.AddField(SubbasinLayerIndex, "LEN1", 2, 10)
        'End If
        'For i = 1 To GisUtil.NumFeatures(SubbasinLayerIndex)
        '  slope = GisUtil.FieldValue(SubbasinLayerIndex, i - 1, SlopeFieldIndex)
        '  'Slope Length from old autodelin
        '  If ((slope > 0) And (slope < 2.0)) Then
        '    sl = 400 / 3.28
        '  ElseIf ((slope >= 2.0) And (slope < 5.0)) Then
        '    sl = 300 / 3.28
        '  ElseIf ((slope >= 5.0) And (slope < 8.0)) Then
        '    sl = 200 / 3.28
        '  ElseIf ((slope >= 8) And (slope < 10.0)) Then
        '    sl = 200 / 3.28
        '  ElseIf ((slope >= 10) And (slope < 12.0)) Then
        '    sl = 120.0 / 3.28
        '  ElseIf ((slope >= 12) And (slope < 16.0)) Then
        '    sl = 80.0 / 3.28
        '  ElseIf ((slope >= 16) And (slope < 20.0)) Then
        '    sl = 60.0 / 3.28
        '  ElseIf ((slope >= 20) And (slope < 25.0)) Then
        '    sl = 50.0 / 3.28
        '  Else
        '    sl = 0.05  '30.0/3.28      
        '  End If
        '  GisUtil.SetFeatureValue(SubbasinLayerIndex, LengthFieldIndex, i - 1, sl)
        'Next i

        'set area of each subbasin
        Logger.Status("Calculating Areas")
        Dim lAreaAcresFieldIndex As Integer
        If GisUtil.IsField(lSubbasinLayerIndex, "AREAACRES") Then
            lAreaAcresFieldIndex = GisUtil.FieldIndex(lSubbasinLayerIndex, "AREAACRES")
        Else 'need to add it
            lAreaAcresFieldIndex = GisUtil.AddField(lSubbasinLayerIndex, "AREAACRES", 2, 10)
        End If
        Dim lAreaMi2FieldIndex As Integer
        If GisUtil.IsField(lSubbasinLayerIndex, "AREAMI2") Then
            lAreaMi2FieldIndex = GisUtil.FieldIndex(lSubbasinLayerIndex, "AREAMI2")
        Else 'need to add it
            lAreaMi2FieldIndex = GisUtil.AddField(lSubbasinLayerIndex, "AREAMI2", 2, 10)
        End If
        For lSubbasinsIndex As Integer = 1 To GisUtil.NumFeatures(lSubbasinLayerIndex)
            Dim lArea As Double = GisUtil.FeatureArea(lSubbasinLayerIndex, lSubbasinsIndex - 1)
            GisUtil.SetFeatureValue(lSubbasinLayerIndex, lAreaAcresFieldIndex, lSubbasinsIndex - 1, lArea / 4046.86)
            GisUtil.SetFeatureValue(lSubbasinLayerIndex, lAreaMi2FieldIndex, lSubbasinsIndex - 1, lArea / 2589988)
        Next lSubbasinsIndex
        Logger.Status("")
    End Sub

    Sub CalculateReaches(ByVal aSubbasinThemeName As String, ByVal aReachThemeName As String, _
                         ByVal aElevationThemeName As String, ByVal aPCS As Boolean, ByVal aCombine As Boolean, _
                         ByRef aOutletThemeName As String, Optional ByVal aElevUnits As String = "Meters")
        Logger.Status("Calculating...")

        'find the level field
        Dim lReachLayerIndex As Integer = GisUtil.LayerIndex(aReachThemeName)
        Dim lLevelFieldIndex As Integer = -1
        If GisUtil.LayerFileName(lReachLayerIndex).EndsWith("rf1.shp") Then
            If GisUtil.IsField(lReachLayerIndex, "LEV") Then
                lLevelFieldIndex = GisUtil.FieldIndex(lReachLayerIndex, "LEV")
            End If
        Else
            If GisUtil.IsField(lReachLayerIndex, "LEVEL") Then
                lLevelFieldIndex = GisUtil.FieldIndex(lReachLayerIndex, "LEVEL")
            End If
            If lLevelFieldIndex = -1 Then
                If GisUtil.IsField(lReachLayerIndex, "STREAMLEVE") Then
                    lLevelFieldIndex = GisUtil.FieldIndex(lReachLayerIndex, "STREAMLEVE")
                End If
            End If
        End If
        Dim i As Integer
        If lLevelFieldIndex = -1 Then
            'if level field does not exist, add it with every segment set to level 1
            lLevelFieldIndex = GisUtil.FieldIndexAddIfMissing(lReachLayerIndex, "LEVEL", 1, 10)
            GisUtil.StartSetFeatureValue(lReachLayerIndex)
            For i = 1 To GisUtil.NumFeatures(lReachLayerIndex)
                GisUtil.SetFeatureValueNoStartStop(lReachLayerIndex, lLevelFieldIndex, i - 1, 1)
            Next i
            GisUtil.StopSetFeatureValue(lReachLayerIndex)
        End If

        Logger.Status("Clipping...")
        'clip reach layer to subbasin boundaries
        Dim lSubbasinLayerIndex As Integer = GisUtil.LayerIndex(aSubbasinThemeName)
        Dim lOutputReachShapefileName As String = GisUtil.ClipShapesWithPolygon(lReachLayerIndex, lSubbasinLayerIndex)

        'add output reach shapefile to the view
        If GisUtil.IsLayer("Streams") Then
            GisUtil.RemoveLayer(GisUtil.LayerIndex("Streams"))
            'if layer removed, need to obtain new indexes 
            lSubbasinLayerIndex = GisUtil.LayerIndex(aSubbasinThemeName)
            lReachLayerIndex = GisUtil.LayerIndex(aReachThemeName)
        End If
        GisUtil.AddLayer(lOutputReachShapefileName, "Streams")
        Dim lStreamsLayerIndex As Integer = GisUtil.LayerIndex("Streams")
        GisUtil.LayerVisible(lStreamsLayerIndex) = True

        Logger.Status("Indexing...")

        Dim lMinField As Integer = 9999
        'identify which fields contain the upstream and downstream reach ids
        Dim lReachField As Integer
        Dim lDownReachField As Integer
        If GisUtil.IsField(lStreamsLayerIndex, "RIVRCH") Then
            lReachField = GisUtil.FieldIndex(lStreamsLayerIndex, "RIVRCH")
        ElseIf GisUtil.IsField(lStreamsLayerIndex, "RCHID") Then
            lReachField = GisUtil.FieldIndex(lStreamsLayerIndex, "RCHID")
        ElseIf GisUtil.IsField(lStreamsLayerIndex, "COMID") Then
            lReachField = GisUtil.FieldIndex(lStreamsLayerIndex, "COMID")
        ElseIf GisUtil.IsField(lStreamsLayerIndex, "SUBBASIN") Then
            lReachField = GisUtil.FieldIndex(lStreamsLayerIndex, "SUBBASIN")
        End If
        If GisUtil.IsField(lStreamsLayerIndex, "DSCSM") Then
            lDownReachField = GisUtil.FieldIndex(lStreamsLayerIndex, "DSCSM")
        ElseIf GisUtil.IsField(lStreamsLayerIndex, "DSRCHID") Then
            lDownReachField = GisUtil.FieldIndex(lStreamsLayerIndex, "DSRCHID")
        ElseIf GisUtil.IsField(lStreamsLayerIndex, "TOCOMID") Then
            lDownReachField = GisUtil.FieldIndex(lStreamsLayerIndex, "TOCOMID")
        ElseIf GisUtil.IsField(lStreamsLayerIndex, "SUBBASINR") Then
            lDownReachField = GisUtil.FieldIndex(lStreamsLayerIndex, "SUBBASINR")
        End If

        'temporarily flag segments that have been clipped -- we'll want to know this later
        Dim lClippedFieldIndex As Integer = GisUtil.FieldIndexAddIfMissing(lStreamsLayerIndex, "CLIPPED", 0, 10)
        GisUtil.StartSetFeatureValue(lStreamsLayerIndex)
        For lStreamIndex As Integer = 1 To GisUtil.NumFeatures(lStreamsLayerIndex)
            Dim lSearchVal As String = GisUtil.FieldValue(lStreamsLayerIndex, lStreamIndex - 1, lReachField)
            For lStreamIndexMatch As Integer = 1 To GisUtil.NumFeatures(lStreamsLayerIndex)
                If GisUtil.FieldValue(lStreamsLayerIndex, lStreamIndexMatch - 1, lReachField) = lSearchVal And lStreamIndex <> lStreamIndexMatch Then
                    'this record has the same reach id, mark as a clipped segment
                    GisUtil.SetFeatureValueNoStartStop(lStreamsLayerIndex, lClippedFieldIndex, lStreamIndex - 1, "clipped")
                    GisUtil.SetFeatureValueNoStartStop(lStreamsLayerIndex, lClippedFieldIndex, lStreamIndexMatch - 1, "clipped")
                    Dim lLen1 As Double = GisUtil.FeatureLength(lStreamsLayerIndex, lStreamIndex - 1)
                    Dim lLen2 As Double = GisUtil.FeatureLength(lStreamsLayerIndex, lStreamIndexMatch - 1)
                    If lLen1 < 0.05 * lLen2 Then
                        'this is just a nub, mark it for deletion
                        GisUtil.SetFeatureValueNoStartStop(lStreamsLayerIndex, lLevelFieldIndex, lStreamIndex - 1, 998)
                    End If
                    If lLen2 < 0.05 * lLen1 Then
                        'this is just a nub, mark it for deletion
                        GisUtil.SetFeatureValueNoStartStop(lStreamsLayerIndex, lLevelFieldIndex, lStreamIndexMatch - 1, 998)
                    End If
                End If
            Next lStreamIndexMatch
        Next lStreamIndex
        GisUtil.StopSetFeatureValue(lStreamsLayerIndex)

        'assign subbasin numbers to each reach segment
        Dim aIndex(GisUtil.NumFeatures(lStreamsLayerIndex)) As Integer
        GisUtil.AssignContainingPolygons(lStreamsLayerIndex, lSubbasinLayerIndex, aIndex)
        Dim ReachSubbasinFieldIndex As Integer
        Dim SubbasinFieldIndex As Integer
        SubbasinFieldIndex = GisUtil.FieldIndex(lSubbasinLayerIndex, "SUBBASIN")
        ReachSubbasinFieldIndex = GisUtil.FieldIndexAddIfMissing(lStreamsLayerIndex, "SUBBASIN", 1, 10)
        If ReachSubbasinFieldIndex < lMinField Then lMinField = ReachSubbasinFieldIndex
        GisUtil.StartSetFeatureValue(lStreamsLayerIndex)
        For i = 1 To GisUtil.NumFeatures(lStreamsLayerIndex)
            Dim j As Integer
            If aIndex(i) > -1 Then
                j = GisUtil.FieldValue(lSubbasinLayerIndex, aIndex(i), SubbasinFieldIndex)
            Else
                j = aIndex(i)
            End If
            GisUtil.SetFeatureValueNoStartStop(lStreamsLayerIndex, ReachSubbasinFieldIndex, i - 1, j)
        Next i
        GisUtil.StopSetFeatureValue(lStreamsLayerIndex)

        'clean out segments that are not within any subbasin, fix to clean up outliers in containing polygons
        GisUtil.StartRemoveFeature(lStreamsLayerIndex)
        i = 0
        Do While i < GisUtil.NumFeatures(lStreamsLayerIndex)
            If GisUtil.FieldValue(lStreamsLayerIndex, i, ReachSubbasinFieldIndex) < 0 Then
                'remove this feature
                GisUtil.RemoveFeatureNoStartStop(lStreamsLayerIndex, i)
            Else
                i += 1
            End If
        Loop
        GisUtil.StopRemoveFeature(lStreamsLayerIndex)

        Logger.Status("Filtering...")

        'find lowest reach level in each subbasin
        For lSubBasinIndex As Integer = 1 To GisUtil.NumFeatures(lSubbasinLayerIndex)
            Logger.Progress(lSubBasinIndex, GisUtil.NumFeatures(lSubbasinLayerIndex))
            System.Windows.Forms.Application.DoEvents()
            Dim lLowestLevel As Integer = 999999
            For i = 1 To GisUtil.NumFeatures(lStreamsLayerIndex)
                If GisUtil.FieldValue(lStreamsLayerIndex, i - 1, ReachSubbasinFieldIndex) = GisUtil.FieldValue(lSubbasinLayerIndex, lSubBasinIndex - 1, SubbasinFieldIndex) Then
                    'this is in the subbasin of interest
                    Dim lLevel As Integer = GisUtil.FieldValue(lStreamsLayerIndex, i - 1, lLevelFieldIndex)
                    If lLevel < lLowestLevel And lLevel > 0 Then
                        lLowestLevel = lLevel
                    End If
                End If
            Next i

            'save only segments of the lowest level in this subbasin
            GisUtil.StartRemoveFeature(lStreamsLayerIndex)
            i = 0
            Do While i < GisUtil.NumFeatures(lStreamsLayerIndex)
                If GisUtil.FieldValue(lStreamsLayerIndex, i, ReachSubbasinFieldIndex) = GisUtil.FieldValue(lSubbasinLayerIndex, lSubBasinIndex - 1, SubbasinFieldIndex) Then
                    'this is in the subbasin of interest
                    Dim lLevel As Integer = GisUtil.FieldValue(lStreamsLayerIndex, i, lLevelFieldIndex)
                    If lLevel <> lLowestLevel Then 'remove this feature
                        GisUtil.RemoveFeatureNoStartStop(lStreamsLayerIndex, i)
                    Else
                        i += 1
                    End If
                Else
                    i += 1
                End If
            Loop
            GisUtil.StopRemoveFeature(lStreamsLayerIndex)
        Next lSubBasinIndex

        Logger.Status("Merging...")

        'add downstream subbasin ids
        Dim lDownstreamFieldIndex As Integer = GisUtil.FieldIndexAddIfMissing(lStreamsLayerIndex, "SUBBASINR", 1, 10)
        If lDownstreamFieldIndex < lMinField Then lMinField = lDownstreamFieldIndex

        Dim rval As String
        Dim dval As String
        Dim dsubbasin As String
        Dim rsubbasin As String
        GisUtil.StartSetFeatureValue(lStreamsLayerIndex)
        'populate the downstream subbasin ids
        For i = 1 To GisUtil.NumFeatures(lStreamsLayerIndex)
            Logger.Progress(i, GisUtil.NumFeatures(lStreamsLayerIndex))
            System.Windows.Forms.Application.DoEvents()
            dval = GisUtil.FieldValue(lStreamsLayerIndex, i - 1, lDownReachField)
            'find what is downstream of rval
            For lSteamIndexDown As Integer = 1 To GisUtil.NumFeatures(lStreamsLayerIndex)
                rval = GisUtil.FieldValue(lStreamsLayerIndex, lSteamIndexDown - 1, lReachField)
                If rval = dval Then
                    'this is the downstream segment
                    dsubbasin = GisUtil.FieldValue(lStreamsLayerIndex, lSteamIndexDown - 1, ReachSubbasinFieldIndex)
                    rsubbasin = GisUtil.FieldValue(lStreamsLayerIndex, i - 1, ReachSubbasinFieldIndex)
                    'if the downstream subbasin id is different that this subbasin id
                    'set it, and make the same change to all segments of this subbasin id
                    If dsubbasin <> rsubbasin Then
                        GisUtil.SetFeatureValueNoStartStop(lStreamsLayerIndex, lDownstreamFieldIndex, i - 1, dsubbasin)
                        'make another pass to set each stream within a subbasin to the same subbasinr
                        For lStreamIndex As Integer = 1 To GisUtil.NumFeatures(lStreamsLayerIndex)
                            If GisUtil.FieldValue(lStreamsLayerIndex, lStreamIndex - 1, ReachSubbasinFieldIndex) = rsubbasin Then
                                GisUtil.SetFeatureValueNoStartStop(lStreamsLayerIndex, lDownstreamFieldIndex, lStreamIndex - 1, dsubbasin)
                            End If
                        Next lStreamIndex
                    End If
                    'exit once we found what is downstream of this segment
                    Exit For
                End If
            Next lSteamIndexDown
        Next i
        For i = 1 To GisUtil.NumFeatures(lStreamsLayerIndex)
            Logger.Progress(i, GisUtil.NumFeatures(lStreamsLayerIndex))
            System.Windows.Forms.Application.DoEvents()
            dval = GisUtil.FieldValue(lStreamsLayerIndex, i - 1, lDownstreamFieldIndex)
            If dval = 0 Then
                GisUtil.SetFeatureValueNoStartStop(lStreamsLayerIndex, lDownstreamFieldIndex, i - 1, -999)
            End If
        Next i
        Logger.Progress(GisUtil.NumFeatures(lStreamsLayerIndex), GisUtil.NumFeatures(lStreamsLayerIndex))
        GisUtil.StopSetFeatureValue(lStreamsLayerIndex)

        'merge reach segments together within subbasin
        GisUtil.MergeFeaturesBasedOnAttribute(lStreamsLayerIndex, ReachSubbasinFieldIndex, aCombine)

        'create and populate fields
        Logger.Status("Calculating attributes...")

        'set length of stream reach
        Dim lLengthFieldIndex As Integer = GisUtil.FieldIndexAddIfMissing(lStreamsLayerIndex, "LEN2", 2, 10)
        If lLengthFieldIndex < lMinField Then lMinField = lLengthFieldIndex
        Dim r As Double
        For i = 1 To GisUtil.NumFeatures(lStreamsLayerIndex)
            r = GisUtil.FeatureLength(lStreamsLayerIndex, i - 1)
            GisUtil.SetFeatureValue(lStreamsLayerIndex, lLengthFieldIndex, i - 1, r)
        Next i

        'set local contributing area of stream reach
        Dim AreaFieldIndex As Integer = GisUtil.FieldIndexAddIfMissing(lStreamsLayerIndex, "LAREA", 2, 10)
        If AreaFieldIndex < lMinField Then lMinField = AreaFieldIndex
        For i = 1 To GisUtil.NumFeatures(lStreamsLayerIndex)
            rval = GisUtil.FieldValue(lStreamsLayerIndex, i - 1, ReachSubbasinFieldIndex)
            For lSubbasinIndex As Integer = 1 To GisUtil.NumFeatures(lSubbasinLayerIndex)
                dval = GisUtil.FieldValue(lSubbasinLayerIndex, lSubbasinIndex - 1, SubbasinFieldIndex)
                If dval = rval Then
                    r = GisUtil.FeatureArea(lSubbasinLayerIndex, lSubbasinIndex - 1)
                    GisUtil.SetFeatureValue(lStreamsLayerIndex, AreaFieldIndex, i - 1, r)
                    Exit For
                End If
            Next lSubbasinIndex
        Next i

        'set total contributing area of stream reach
        Dim bfound As Boolean
        Dim r2 As Double
        Dim tAreaFieldIndex As Integer
        tAreaFieldIndex = GisUtil.FieldIndexAddIfMissing(lStreamsLayerIndex, "TAREA", 2, 20)
        If tAreaFieldIndex < lMinField Then lMinField = tAreaFieldIndex
        For i = 1 To GisUtil.NumFeatures(lStreamsLayerIndex)
            r = GisUtil.FieldValue(lStreamsLayerIndex, i - 1, AreaFieldIndex)
            GisUtil.SetFeatureValue(lStreamsLayerIndex, tAreaFieldIndex, i - 1, r)
        Next i
        For i = 1 To GisUtil.NumFeatures(lStreamsLayerIndex)
            Logger.Progress(i, GisUtil.NumFeatures(lStreamsLayerIndex))
            System.Windows.Forms.Application.DoEvents()
            r2 = GisUtil.FieldValue(lStreamsLayerIndex, i - 1, AreaFieldIndex)                        'local area of this one
            rval = GisUtil.FieldValue(lStreamsLayerIndex, i - 1, ReachSubbasinFieldIndex)
            'Logger.Dbg("ManDelin:adding area from feature " & rval)
            'is there anything downstream of this one?
            dval = GisUtil.FieldValue(lStreamsLayerIndex, i - 1, lDownstreamFieldIndex)
            Do While dval > 0
                'Logger.Dbg("ManDelin:" & dval & " downstream of " & rval)
                bfound = False
                For lStreamIndexDownstream As Integer = 1 To GisUtil.NumFeatures(lStreamsLayerIndex)
                    rval = GisUtil.FieldValue(lStreamsLayerIndex, lStreamIndexDownstream - 1, ReachSubbasinFieldIndex)
                    If rval = dval Then 'this is the one
                        r = GisUtil.FieldValue(lStreamsLayerIndex, lStreamIndexDownstream - 1, tAreaFieldIndex)   'total area of downstream one
                        GisUtil.SetFeatureValue(lStreamsLayerIndex, tAreaFieldIndex, lStreamIndexDownstream - 1, r + r2)
                        'Logger.Dbg("ManDelin:" & rval & " area now " & r + r2)
                        dval = GisUtil.FieldValue(lStreamsLayerIndex, lStreamIndexDownstream - 1, lDownstreamFieldIndex)
                        bfound = True
                        Exit For
                    End If
                Next lStreamIndexDownstream
                If Not bfound Then
                    dval = 0
                End If
            Loop
        Next i
        'add total contributing area in acres and square miles
        Dim AreaAcresFieldIndex As Integer = GisUtil.FieldIndexAddIfMissing(lStreamsLayerIndex, "TAREAACRES", 2, 20)
        Dim AreaMi2FieldIndex As Integer = GisUtil.FieldIndexAddIfMissing(lStreamsLayerIndex, "TAREAMI2", 2, 10)
        For i = 1 To GisUtil.NumFeatures(lStreamsLayerIndex)
            r = GisUtil.FieldValue(lStreamsLayerIndex, i - 1, tAreaFieldIndex)
            GisUtil.SetFeatureValue(lStreamsLayerIndex, AreaAcresFieldIndex, i - 1, r / 4046.86)
            GisUtil.SetFeatureValue(lStreamsLayerIndex, AreaMi2FieldIndex, i - 1, r / 2589988)
        Next i

        'set stream width based on upstream area
        Dim lFieldIndex As Integer = GisUtil.FieldIndexAddIfMissing(lStreamsLayerIndex, "WID2", 2, 10)
        If lFieldIndex < lMinField Then lMinField = lFieldIndex
        For i = 1 To GisUtil.NumFeatures(lStreamsLayerIndex)
            r = GisUtil.FieldValue(lStreamsLayerIndex, i - 1, tAreaFieldIndex)
            r2 = (1.29) * ((r / 1000000) ^ (0.6))
            GisUtil.SetFeatureValue(lStreamsLayerIndex, lFieldIndex, i - 1, r2)
        Next i

        'set depth based on upstream area
        lFieldIndex = GisUtil.FieldIndexAddIfMissing(lStreamsLayerIndex, "DEP2", 2, 10)
        If lFieldIndex < lMinField Then lMinField = lFieldIndex
        For i = 1 To GisUtil.NumFeatures(lStreamsLayerIndex)
            r = GisUtil.FieldValue(lStreamsLayerIndex, i - 1, tAreaFieldIndex)
            r2 = (0.13) * ((r / 1000000) ^ (0.4))
            GisUtil.SetFeatureValue(lStreamsLayerIndex, lFieldIndex, i - 1, r2)
        Next i

        'set min elev
        Dim lMinElevFieldIndex As Integer = GisUtil.FieldIndexAddIfMissing(lStreamsLayerIndex, "MINEL", 1, 10)
        If lMinElevFieldIndex < lMinField Then lMinField = lMinElevFieldIndex
        'set max elev
        Dim lMaxElevFieldIndex As Integer = GisUtil.FieldIndexAddIfMissing(lStreamsLayerIndex, "MAXEL", 1, 10)
        If lMaxElevFieldIndex < lMinField Then lMinField = lMaxElevFieldIndex

        Dim x1 As Double
        Dim x2 As Double
        Dim y1 As Double
        Dim y2 As Double
        Dim gmin As Integer
        Dim gmax As Integer
        Dim gtemp As Integer
        Dim lElevationLayerIndex As Integer = GisUtil.LayerIndex(aElevationThemeName)
        For i = 1 To GisUtil.NumFeatures(lStreamsLayerIndex)
            'return end points of stream segment
            GisUtil.EndPointsOfLine(lStreamsLayerIndex, i - 1, x1, y1, x2, y2)
            If GisUtil.LayerType(lElevationLayerIndex) = 3 Then
                'get shapefile value at point
                Dim lElevation As Integer = GisUtil.PointInPolygonXY(x1, y1, lElevationLayerIndex)
                Dim lElevationFieldIndex As Integer = GisUtil.FieldIndex(lElevationLayerIndex, "ELEV_M")
                gmin = GisUtil.FieldValue(lElevationLayerIndex, lElevation, lElevationFieldIndex)
                lElevation = GisUtil.PointInPolygonXY(x2, y2, lElevationLayerIndex)
                gmax = GisUtil.FieldValue(lElevationLayerIndex, lElevation, lElevationFieldIndex)
            Else 'get grid value at point
                gmin = GisUtil.GridValueAtPoint(lElevationLayerIndex, x1, y1)
                gmax = GisUtil.GridValueAtPoint(lElevationLayerIndex, x2, y2)
            End If
            If aElevUnits = "Centimeters" Then
                gmin = gmin / 100  'this is an ned grid (in cm), convert to meters
                gmax = gmax / 100
            ElseIf aElevUnits = "Feet" Then
                gmin = gmin / 3.281  'this is a grid in ft, convert to meters
                gmax = gmax / 3.281
            End If
            If gmax < gmin Then
                gtemp = gmin
                gmin = gmax
                gmax = gtemp
            End If
            GisUtil.SetFeatureValue(lStreamsLayerIndex, lMinElevFieldIndex, i - 1, gmin)
            GisUtil.SetFeatureValue(lStreamsLayerIndex, lMaxElevFieldIndex, i - 1, gmax)
        Next i

        'set slope of stream reach
        lFieldIndex = GisUtil.FieldIndexAddIfMissing(lStreamsLayerIndex, "SLO2", 2, 10)
        If lFieldIndex < lMinField Then lMinField = lFieldIndex
        For i = 1 To GisUtil.NumFeatures(lStreamsLayerIndex)
            gmin = GisUtil.FieldValue(lStreamsLayerIndex, i - 1, lMinElevFieldIndex)
            gmax = GisUtil.FieldValue(lStreamsLayerIndex, i - 1, lMaxElevFieldIndex)
            gtemp = GisUtil.FieldValue(lStreamsLayerIndex, i - 1, lLengthFieldIndex)
            GisUtil.SetFeatureValue(lStreamsLayerIndex, lFieldIndex, i - 1, (gmax - gmin) * 100 / gtemp)
        Next i

        'set name of each stream reach
        lFieldIndex = GisUtil.FieldIndexAddIfMissing(lStreamsLayerIndex, "SNAME", 0, 20)
        If lFieldIndex < lMinField Then lMinField = lFieldIndex
        Dim NameFieldIndex As Integer
        Dim Name As String
        If GisUtil.IsField(lStreamsLayerIndex, "PNAME") Then
            NameFieldIndex = GisUtil.FieldIndex(lStreamsLayerIndex, "PNAME")
        ElseIf GisUtil.IsField(lStreamsLayerIndex, "NAME") Then
            NameFieldIndex = GisUtil.FieldIndex(lStreamsLayerIndex, "NAME")
        ElseIf GisUtil.IsField(lStreamsLayerIndex, "GNIS_NAME") Then
            NameFieldIndex = GisUtil.FieldIndex(lStreamsLayerIndex, "GNIS_NAME")
        End If
        If NameFieldIndex > -1 Then
            For i = 1 To GisUtil.NumFeatures(lStreamsLayerIndex)
                Name = GisUtil.FieldValue(lStreamsLayerIndex, i - 1, NameFieldIndex)
                GisUtil.SetFeatureValue(lStreamsLayerIndex, lFieldIndex, i - 1, Name)
            Next i
        End If
        'add name to subbasin layer as well
        NameFieldIndex = GisUtil.FieldIndexAddIfMissing(lSubbasinLayerIndex, "BNAME", 0, 20)
        For i = 1 To GisUtil.NumFeatures(lSubbasinLayerIndex)
            dval = GisUtil.FieldValue(lSubbasinLayerIndex, i - 1, SubbasinFieldIndex)
            For lSteamIndex As Integer = 1 To GisUtil.NumFeatures(lStreamsLayerIndex)
                rval = GisUtil.FieldValue(lStreamsLayerIndex, lSteamIndex - 1, ReachSubbasinFieldIndex)
                If rval = dval Then
                    'this is the one
                    If Len(Trim(GisUtil.FieldValue(lSubbasinLayerIndex, i - 1, NameFieldIndex))) = 0 Then
                        GisUtil.SetFeatureValue(lSubbasinLayerIndex, NameFieldIndex, i - 1, GisUtil.FieldValue(lStreamsLayerIndex, lSteamIndex - 1, lFieldIndex))
                        Exit For
                    End If
                End If
            Next lSteamIndex
        Next i

        'remove unwanted fields
        For i = 1 To lMinField
            GisUtil.RemoveField(lStreamsLayerIndex, 0)
        Next i

        'now add outlets
        Logger.Status("Creating outlets...")

        'create new outlets shapefile
        i = 1
        Dim outputpath As String
        Dim success As Boolean
        outputpath = PathNameOnly(GisUtil.LayerFileName(lStreamsLayerIndex))
        aOutletThemeName = outputpath & "\outlets" & i & ".shp"
        Do While FileExists(aOutletThemeName)
            i += 1
            aOutletThemeName = outputpath & "\outlets" & i & ".shp"
        Loop
        'add points to the shapefile
        Dim lShapefile As New MapWinGIS.Shapefile
        success = lShapefile.CreateNew(aOutletThemeName, MapWinGIS.ShpfileType.SHP_POINT)
        Dim lInputProjectionFileName As String = FilenameSetExt(GisUtil.LayerFileName(lStreamsLayerIndex), "prj")
        If FileExists(lInputProjectionFileName) Then
            FileCopy(lInputProjectionFileName, FilenameSetExt(aOutletThemeName, "prj"))
        End If
        success = lShapefile.StartEditingShapes(True)

        'Add ID Field 
        Dim [of] As New MapWinGIS.Field
        [of].Name = "ID"
        [of].Type = MapWinGIS.FieldType.INTEGER_FIELD
        [of].Width = 10
        success = lShapefile.EditInsertField([of], lShapefile.NumFields)
        [of] = Nothing
        'Add PCSID Field 
        Dim of2 As New MapWinGIS.Field
        of2.Name = "PCSID"
        of2.Type = MapWinGIS.FieldType.STRING_FIELD
        of2.Width = 10
        success = lShapefile.EditInsertField(of2, lShapefile.NumFields)
        'Add Xpr Field 
        Dim of3 As New MapWinGIS.Field
        of3.Name = "Xpr"
        of3.Type = MapWinGIS.FieldType.DOUBLE_FIELD
        of3.Width = 10
        success = lShapefile.EditInsertField(of3, lShapefile.NumFields)
        'Add Ypr Field 
        Dim of4 As New MapWinGIS.Field
        of4.Name = "Ypr"
        of4.Type = MapWinGIS.FieldType.DOUBLE_FIELD
        of4.Width = 10
        success = lShapefile.EditInsertField(of4, lShapefile.NumFields)

        'add points at each stream outlet
        For i = 1 To GisUtil.NumFeatures(lStreamsLayerIndex)
            Dim lShape As New MapWinGIS.Shape
            Dim lPoint As New MapWinGIS.Point
            lShape.Create(MapWinGIS.ShpfileType.SHP_POINT)
            GisUtil.EndPointsOfLine(lStreamsLayerIndex, i - 1, x1, y1, x2, y2)
            If GisUtil.FieldName(lReachField, lReachLayerIndex) = "COMID" Then
                'cheap way to see if this is nhdplus way of digitizing
                lPoint.x = x2
                lPoint.y = y2
            Else
                lPoint.x = x1
                lPoint.y = y1
            End If
            lShape.InsertPoint(lPoint, 0)
            success = lShapefile.EditInsertShape(lShape, lShapefile.NumShapes)
            success = lShapefile.EditCellValue(2, lShapefile.NumShapes - 1, lPoint.x)
            success = lShapefile.EditCellValue(3, lShapefile.NumShapes - 1, lPoint.y)
            lPoint = Nothing
            lShape = Nothing
        Next i

        'add pcs points if checked
        Dim pcsLayerIndex As Integer
        Dim pcsFieldindex As Integer
        Dim pcsid As String
        If aPCS Then
            pcsLayerIndex = GisUtil.LayerIndex("Permit Compliance System")
            pcsFieldindex = GisUtil.FieldIndex(pcsLayerIndex, "NPDES")
            For i = 1 To GisUtil.NumFeatures(pcsLayerIndex)
                GisUtil.PointXY(pcsLayerIndex, i - 1, x1, y1)
                If GisUtil.PointInPolygonXY(x1, y1, lSubbasinLayerIndex) > -1 Then
                    pcsid = GisUtil.FieldValue(pcsLayerIndex, i - 1, pcsFieldindex)
                    Dim lShape As New MapWinGIS.Shape
                    Dim lPoint As New MapWinGIS.Point
                    lShape.Create(MapWinGIS.ShpfileType.SHP_POINT)
                    lPoint.x = x1
                    lPoint.y = y1
                    lShape.InsertPoint(lPoint, 0)
                    success = lShapefile.EditInsertShape(lShape, lShapefile.NumShapes)
                    success = lShapefile.EditCellValue(1, lShapefile.NumShapes - 1, pcsid)
                    success = lShapefile.EditCellValue(2, lShapefile.NumShapes - 1, x1)
                    success = lShapefile.EditCellValue(3, lShapefile.NumShapes - 1, y1)
                    lPoint = Nothing
                    lShape = Nothing
                End If
            Next i
        End If

        'Populate ID field
        For i = 1 To lShapefile.NumShapes
            success = lShapefile.EditCellValue(0, i - 1, i)
        Next i
        success = lShapefile.StopEditingShapes(True, True)
        success = lShapefile.Close()

        Logger.Status("")
    End Sub

End Module

Public Class clsProgressStatus
    Implements MapWinUtility.IProgressStatus

    Private pLabel As Windows.Forms.Label
    Private pForm As Windows.Forms.Form
    Private pMessage As String
    Private pProgressStatusOther As MapWinUtility.IProgressStatus

    Friend WriteOnly Property ProgressLabel() As Windows.Forms.Label
        Set(ByVal aLabel As Windows.Forms.Label)
            pLabel = aLabel
            pForm = pLabel.TopLevelControl
        End Set
    End Property

    Friend WriteOnly Property ProgressStatusOther() As MapWinUtility.IProgressStatus
        Set(ByVal aProgressStatusOther As MapWinUtility.IProgressStatus)
            pProgressStatusOther = aProgressStatusOther
        End Set
    End Property

    ''' <summary>
    ''' Log the progress of a long-running task
    ''' </summary>
    ''' <param name="aCurrentPosition">Current position/item of task</param>
    ''' <param name="aLastPosition">Final position/item of task</param>
    ''' <remarks>
    ''' A final call when the task is done with aCurrent = aLast 
    ''' indicates completion and should clear the progress display.
    ''' </remarks>
    Public Sub Progress(ByVal aCurrentPosition As Integer, ByVal aLastPosition As Integer) Implements MapWinUtility.IProgressStatus.Progress
        pLabel.Text = pMessage & "(" & aCurrentPosition & " of " & aLastPosition & ")"
        pLabel.Refresh()
        If Not pProgressStatusOther Is Nothing Then
            pProgressStatusOther.Progress(aCurrentPosition, aLastPosition)
        End If
    End Sub

    ''' <summary>
    ''' Update the current status message
    ''' </summary>
    ''' <param name="aStatusMessage">Description of current processing status</param>
    Public Sub Status(ByVal aStatusMessage As String) Implements MapWinUtility.IProgressStatus.Status
        If aStatusMessage.Length > 0 Then
            Logger.Dbg(aStatusMessage)
            pForm.Cursor = Windows.Forms.Cursors.WaitCursor
            pLabel.Visible = True
        Else
            pForm.Cursor = Windows.Forms.Cursors.Default
            pLabel.Visible = False
        End If
        pMessage = aStatusMessage
        pLabel.Text = aStatusMessage
        pLabel.Refresh()
    End Sub
End Class

