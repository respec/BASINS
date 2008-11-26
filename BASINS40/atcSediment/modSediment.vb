Imports System.Drawing
Imports MapWinUtility
Imports atcMwGisUtility
Imports atcControls
Imports atcUtility
Imports System.Windows.Forms
Imports HTMLBuilder

''' <summary>
''' This class is used when I call TauDEM functions which need a callback function
''' </summary>
Public Class clsTkCallback
    Implements TKTAUDEMLib.ItkCallback

    Public Sub [Error](ByVal Key As String, ByVal ErrorCode As Integer, ByVal ErrorMsg As String) Implements TKTAUDEMLib.ItkCallback.Error
        If ProgressForm IsNot Nothing Then
            ProgressForm.Status = ErrorMsg
            ProgressForm.IsCancelled = True
            Logger.Message(ErrorMsg, "TauDEM Error", Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Error, Windows.Forms.DialogResult.OK)
        End If
    End Sub

    Public Sub Progress(ByVal Key As String, ByVal Percent As Integer, ByVal Message As String) Implements TKTAUDEMLib.ItkCallback.Progress
        ProgressForm.SetProgress(Percent, 100)
    End Sub
End Class

''' <summary>
''' This module contains all computational routines for the sediment utility
''' </summary>
Public Module Sediment

    Friend Class clsLookup
        Friend Description As String
        Friend Factor As Double
        Sub New(ByVal _Description As String, ByVal _Factor As Double)
            Description = _Description
            Factor = _Factor
        End Sub
    End Class

    Friend Structure structProject
        Dim FileName As String
        Dim OutputFolder As String
        Dim GridSize As Double
        Dim SubbasinLayer As String
        Dim SoilLayer, SoilField As String
        Dim LanduseType As enumLandUseType
        Dim LandUseLayer, LandUseField As String
        Dim LandUseGridFile As String
        Dim RoadLayer, RoadField As String
        Dim DEMLayer, DEMField As String, DEMUnits As enumDEMUnits
        Dim BMPLayer, BMPField As String
        Dim StreamLayer As String
        Dim StreamThreshold As Double
        Dim R_Factor As Double
        Dim dictSoil, dictLandUse(), dictBMP As Generic.Dictionary(Of String, clsLookup)
        Dim UseBMP As Boolean
        Dim DeliveryMethod As enumDeliveryMethod
        Dim DistFactor As Double 'conversion factor for distance (units/meter)
        Dim ElevFactor As Double 'conversion factor dem grid elevations (units/meter)
        Dim Modified As Boolean
        Dim SedimentFolder As String

        Sub Initialize()
            FileName = ""
            OutputFolder = "Project 1"
            GridSize = 30
            LanduseType = enumLandUseType.GIRAS
            R_Factor = 300
            UseBMP = False
            DeliveryMethod = enumDeliveryMethod.Distance
            dictSoil = New Generic.Dictionary(Of String, clsLookup)
            ReDim dictLandUse(enumLandUseType.UserGrid)
            For i As enumLandUseType = enumLandUseType.GIRAS To enumLandUseType.UserGrid
                dictLandUse(i) = New Generic.Dictionary(Of String, clsLookup)
            Next
            DEMUnits = enumDEMUnits.Meters
            dictBMP = New Generic.Dictionary(Of String, clsLookup)
            DistFactor = 1.0
            ElevFactor = 1.0
            StreamThreshold = 500
            Modified = False
            SedimentFolder = IO.Path.GetDirectoryName(GisUtil.ProjectFileName) & "\Sediment"
            If Not My.Computer.FileSystem.DirectoryExists(SedimentFolder) Then My.Computer.FileSystem.CreateDirectory(SedimentFolder)
        End Sub
    End Structure

    Friend Enum enumDeliveryMethod
        Distance
        Distance_Relief
        Area
        WEPP
    End Enum

    Friend Enum enumDEMUnits
        Meters
        Centimeters
        Feet
    End Enum

    Friend Enum enumLandUseType
        GIRAS
        NLCD_1992
        NLCD_2001
        UserShapefile
        UserGrid
    End Enum

    Private Enum enumSedimentLayers
        R_Factors
        K_Factors
        C_Factors
        LS_Factors
        P_Factors
        Delivery_Ratio
        USLE_Erosion
        USLE_Sediment
        Land_Use
        Elevations
        Elevations_Filled
        Slopes
        Flow_Direction
        Stream_Grid
    End Enum

    Const PRECISION As Integer = 4 'this is number of decimal places to round real numbers off to when storing in grids

    Friend ProgressForm As frmProgress = Nothing
    Friend Project As structProject

    Dim SedimentLayers() As String = {"R Factors", "K Factors", "C Factors", "LS Factors", "P Factors", "Delivery Ratio", "USLE Sheet & Rill Erosion", "USLE Sediment", "Land Use", "Elevations", "Elevations (Filled)", "Slopes", "Flow Direction", "Stream Grid"}

    ''' <summary>
    ''' Given high-resolution source grid, average all values to a destination grid
    ''' The source and destination grids must already exist (the destination should already be filtered for the desired area)
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function AverageGrid(ByVal SourceFile As String, ByVal DestFile As String) As Boolean
        Try
            Dim gSource, gDest As New MapWinGIS.Grid
            If Not gSource.Open(SourceFile, , False) Then Return False
            If Not gDest.Open(DestFile, , False) Then Return False

            'create temp array so can average slope values (will initially be populated with zeroes)
            Dim arSum(,) As Double, arNum(,) As Integer
            With gDest.Header
                ReDim arSum(.NumberCols - 1, .NumberRows - 1)
                ReDim arNum(.NumberCols - 1, .NumberRows - 1)
            End With

            With gSource.Header
                'get average value within the larger destination grid from the smaller source grid
                For r As Integer = 0 To .NumberRows - 1
                    Dim x As Double, y As Double, cc As Integer, rr As Integer
                    gSource.CellToProj(0, r, x, y)
                    Dim ar(.NumberCols - 1) As Single
                    gSource.GetRow(r, ar(0))
                    For c As Integer = 0 To .NumberCols - 1
                        If ar(c) <> .NodataValue Then
                            gDest.ProjToCell(x, y, cc, rr)
                            If cc >= 0 AndAlso cc <= arSum.GetUpperBound(0) AndAlso rr >= 0 AndAlso rr <= arSum.GetUpperBound(1) Then
                                arSum(cc, rr) += ar(c)
                                arNum(cc, rr) += 1
                            End If
                        End If
                        x += .dX
                    Next
                    If Not ProgressForm.SetProgress(r, .NumberRows - 1) Then Return False
                Next
            End With
            gSource.Close()

            With gDest.Header
                For r As Integer = 0 To .NumberRows - 1
                    Dim ar(.NumberCols - 1) As Single
                    gDest.GetRow(r, ar(0))
                    For c As Integer = 0 To .NumberCols - 1
                        If arNum(c, r) > 0 Then ar(c) = Math.Round(arSum(c, r) / arNum(c, r), PRECISION)
                    Next
                    gDest.PutRow(r, ar(0))
                    If Not ProgressForm.SetProgress(r, .NumberRows - 1) Then Return False
                Next
            End With
            gDest.Save()
            gDest.Close()
            Return True

        Catch ex As Exception
            ErrorMsg(, ex)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Create a new grid and return new grid file name; will remove layer and delete grid first
    ''' </summary>
    ''' <param name="GridName">Name of layer to create</param>
    ''' <returns>Grid filename</returns>
    ''' <remarks></remarks>
    Private Function CreateGrid(ByVal GridName As String, Optional ByVal gTemplate As MapWinGIS.Grid = Nothing, Optional ByVal DataType As MapWinGIS.GridDataType = MapWinGIS.GridDataType.FloatDataType) As String
        Dim hdr As New MapWinGIS.GridHeader
        With hdr
            If gTemplate Is Nothing Then
                .dX = Math.Round(Project.GridSize * Project.DistFactor, 2) 'convert meters to project units
                .dY = .dX
                .NodataValue = -1 'note: cannot do direct comparison to this value (round-off) instead compare to >=0.0
                Dim xmax, xmin, ymax, ymin As Double
                GisUtil.ExtentsOfLayer(GisUtil.LayerIndex(Project.SubbasinLayer), xmax, xmin, ymax, ymin)
                .NumberCols = (xmax - xmin) / .dX
                .NumberRows = (ymax - ymin) / .dY
                .XllCenter = xmin + .dX / 2
                .YllCenter = ymin + .dY / 2
                .Projection = GisUtil.ProjectProjection
            Else
                .CopyFrom(gTemplate.Header)
            End If
        End With

        Dim GridFile As String = String.Format("{0}\{1}.tif", Project.SedimentFolder, GridName)
        Do While GisUtil.IsLayerByFileName(GridFile)
            GisUtil.RemoveLayer(GisUtil.LayerIndex(GridFile))
        Loop
        MapWinGeoProc.DataManagement.DeleteGrid(GridFile)

        Dim g As New MapWinGIS.Grid
        If Not g.CreateNew(GridFile, hdr, DataType, hdr.NodataValue, False, MapWinGIS.GridFileType.GeoTiff) Then GridFile = ""
        g.Close()

        Return GridFile
    End Function

    Private Function Delivery_Area(ByVal GridFile As String) As Boolean
        Try
            'assign delivery ratio to all grid points within a subbasin using Area method
            'DR = 0.417762 * A ^ (-0.134958) - 0.127097 where: DR is the sediment delivery ratio and A is area in square miles

            'see: http://www.gaepd.org/Files_PDF/techguide/wpb/TMDL/Chattahoochee/EPA_Chattahoochee_River_Basin_Sediment_TMDL.pdf

            ProgressForm.Status = "Computing sediment delivery ratios..."

            With Project
                Dim lyr As Integer = GisUtil.LayerIndex(.SubbasinLayer)
                For f As Integer = 0 To GisUtil.NumFeatures(lyr) - 1
                    Dim area As Double = GisUtil.FeatureArea(lyr, f) / (.DistFactor ^ 2) / 2589988.1103 'convert from map units to square meters then to square miles
                    Dim dr As Double = 0.417762 * area ^ (-0.134958) - 0.127097
                    If Not SetGrid(GridFile, GisUtil.LayerFileName(.SubbasinLayer), f, dr) Then Return False
                Next
            End With
            Return True
        Catch ex As Exception
            ErrorMsg(, ex)
            Return False
        End Try
    End Function

    Private Function Delivery_Distance(ByVal ErosionFile As String, ByVal AngleFile As String, ByVal GridFile As String, ByVal StreamFile As String) As Boolean
        '        If Not MultiplyGrid_DR(ErosionFile, DistFile, GridFile) Then Return False
        '        'assign delivery ratio to all grid points within a subbasin using Distance method
        '        'Md = M * (1 - 0.97 * D / L); 
        '        'L = 5.1 + 1.79 * M
        '        'where Md is the mass moved from each cell to the closest stream network (US tons/acre/yr);
        '        '       D is the least cost distance (feet) from a cell to the nearest stream network; and
        '        '       L is the maximum distance (feet) that sediment with mass M (US ton) may travel

        '        'see: http://www.srs.fs.usda.gov/pubs/ja/uncaptured/ja_sun009.pdf

        Dim gUSLE, gDist, gDest As New MapWinGIS.Grid

        Try
            Dim tk As New TKTAUDEMLib.TauDEM

            ProgressForm.Status = "Computing stream grid..."
            If tk.Aread8(AngleFile, StreamFile, 0, 0, 0, 1, "", 0, 0) <> 0 Then Return False

            ProgressForm.Status = "Computing distances to streams..."
            Dim DistFile As String = CreateGrid("Distances to Streams")
            Dim Threshold As Integer = Project.StreamThreshold / (Project.GridSize / 1000) ^ 2 'convert gridsize to sq. km., then compute number of cells
            tk.Distance(AngleFile, StreamFile, DistFile, Threshold, 0)

            'now use distances to streams to compute delivery ratios at each grid cell

            With gUSLE
                If .Open(ErosionFile) And gDist.Open(DistFile) And gDest.Open(GridFile) Then
                    With .Header
                        For r As Integer = 0 To .NumberRows - 1
                            Dim arU(.NumberCols - 1), arD(.NumberCols - 1), arO(.NumberCols - 1) As Single
                            gUSLE.GetRow(r, arU(0))
                            gDist.GetRow(r, arD(0))
                            For c As Integer = 0 To .NumberCols - 1
                                If arU(c) <> .NodataValue Then
                                    Dim D As Double = arD(c) / Project.DistFactor 'convert to meters
                                    Dim M As Double = arU(c) 'USLE erosion in tons/ac/yr
                                    Dim L As Double = 5.1 + 1.79 * M 'meters
                                    Dim DR As Double = Math.Max(0, (1 - 0.97 * D / L)) 'delivery ratio (Md/M)
                                    If arD(c) <> .NodataValue AndAlso arU(c) <> .NodataValue Then
                                        arO(c) = Math.Round(DR, PRECISION)
                                    Else
                                        arO(c) = .NodataValue
                                    End If
                                Else
                                    arO(c) = .NodataValue
                                End If
                            Next
                            gDest.PutRow(r, arO(0))
                            If Not ProgressForm.SetProgress(r, .NumberRows - 1) Then Return False
                        Next
                    End With
                    gDest.Save()
                Else
                    Return False
                End If
                Return True
            End With
        Catch ex As Exception
            ErrorMsg(, ex)
            Return False
        Finally
            gUSLE.Close()
            gDist.Close()
            gDest.Close()
        End Try
    End Function

    ''' <summary>
    ''' Given a grid over a large extent, set all values that fall outside of the filter layer shapes to NoData
    ''' </summary>
    Private Function FilterGrid(ByVal GridFile As String, ByVal FilterFile As String) As Boolean
        Dim sfFilter As New MapWinGIS.Shapefile
        Dim g As New MapWinGIS.Grid
        Try
            sfFilter.Open(GisUtil.LayerFileName(GisUtil.LayerIndex(FilterFile)))
            sfFilter.BeginPointInShapefile()
            g.Open(GridFile)
            With g.Header
                For r As Integer = 0 To .NumberRows - 1
                    Dim x, y As Double
                    g.CellToProj(0, r, x, y)
                    Dim ar(.NumberCols - 1) As Single
                    If Not g.GetRow(r, ar(0)) Then Return False
                    For c As Integer = 0 To .NumberCols - 1
                        If sfFilter.PointInShapefile(x, y) = -1 Then ar(c) = .NodataValue
                        x += .dX
                    Next
                    g.PutRow(r, ar(0))
                    If Not ProgressForm.SetProgress(r, .NumberRows - 1) Then Return False
                Next
            End With
            Return True
        Catch ex As Exception
            ErrorMsg(, ex)
            Return False
        Finally
            sfFilter.EndPointInShapefile()
            sfFilter.Close()
            g.Save()
            g.Close()
            g = Nothing
        End Try
    End Function

    Friend Function GenerateLoads() As Boolean
        Try
            'create a new grid using the specified gridsize, positioned over selected subbasin layer

            With Project
                Select Case GisUtil.MapUnits.ToUpper
                    Case "METERS"
                        .DistFactor = 1.0
                    Case "FEET"
                        If Logger.Message("The current project mapping units are set to 'feet', however BASINS is normally set up to user 'meters'. You you want to proceed anyway?", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, DialogResult.Cancel) = DialogResult.Cancel Then Return False
                        .DistFactor = 3.2808
                    Case Else
                        Logger.Message("The current BASINS project units are not compatible with this tool: " & GisUtil.MapUnits, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, Windows.Forms.DialogResult.OK)
                        Return False
                End Select

                Select Case .DEMUnits
                    Case enumDEMUnits.Meters : .ElevFactor = 1.0
                    Case enumDEMUnits.Centimeters : .ElevFactor = 100
                    Case enumDEMUnits.Feet : .ElevFactor = 3.2808
                End Select

                If Not LayerOK(.SubbasinLayer) Then Return False
                If Not LayerOK(.SoilLayer, .SoilField) Then Return False
                If .LanduseType = enumLandUseType.GIRAS Then
                    If GIRASLayers(GisUtil.LayerIndex(.SubbasinLayer)).Count = 0 Then Return False
                Else
                    If Not LayerOK(.LandUseLayer, .LandUseField) Then Return False
                End If
                If Not LayerOK(.DEMLayer) Then Return False
                If .UseBMP And Not LayerOK(.BMPLayer, .BMPField) Then Return False
                If .DeliveryMethod <> enumDeliveryMethod.Area AndAlso Not LayerOK(.StreamLayer) Then Return False

            End With

            Dim ErosionFile As String = CreateGrid(SedimentLayer(enumSedimentLayers.USLE_Erosion))
            Dim GroupName As String = "Sediment Analysis"

            Dim BasinFile As String = GisUtil.LayerFileName(Project.SubbasinLayer)

            For i As enumSedimentLayers = enumSedimentLayers.R_Factors To enumSedimentLayers.Stream_Grid
                If GisUtil.IsLayer(SedimentLayer(i)) Then GisUtil.RemoveLayer(GisUtil.LayerIndex(SedimentLayer(i)))
            Next

            'With GisUtil.MappingObject.Layers
            '    For i As Integer = .NumLayers - 1 To 0 Step -1
            '        If .IsValidHandle(i) AndAlso .Item(i).GroupHandle = GisUtil.GroupHandle(GroupName) Then .Remove(i)
            '    Next
            'End With

            GisUtil.AddGroup(GroupName)

            'create grids for each component of USLE equation...
            Dim GridName, GridFile As String
            Dim SlopeFile As String = "", AngleFile As String = ""

            For i As enumSedimentLayers = enumSedimentLayers.R_Factors To enumSedimentLayers.P_Factors
                GridName = SedimentLayer(i)
                GridFile = CreateGrid(GridName)
                ProgressForm.Status = String.Format("Creating new grid: {0}...", GridName)
                With Project
                    Select Case i
                        Case enumSedimentLayers.R_Factors  'R Factors are constant
                            If Not SetGrid(GridFile, .R_Factor) Then Return False
                        Case enumSedimentLayers.K_Factors  'K Factors based on shapefile
                            If Not LookupGrid(GisUtil.LayerFileName(.SoilLayer), .SoilField, GridFile, .dictSoil) Then Return False
                        Case enumSedimentLayers.C_Factors  'C Factors
                            If Project.LanduseType = enumLandUseType.GIRAS Then
                                For Each idx As Integer In GIRASLayers(GisUtil.LayerIndex(Project.SubbasinLayer))
                                    If Not LookupGrid(GisUtil.LayerFileName(idx), "LUCODE", GridFile, .dictLandUse(.LanduseType)) Then Return False
                                Next
                            Else
                                If GisUtil.LayerType(.LandUseLayer) = MapWindow.Interfaces.eLayerType.PolygonShapefile Then
                                    If Not LookupGrid(GisUtil.LayerFileName(.LandUseLayer), .LandUseField, GridFile, .dictLandUse(.LanduseType)) Then Return False
                                Else
                                    'resample landuse grid to same resolution as USLE
                                    .LandUseGridFile = CreateGrid(SedimentLayer(enumSedimentLayers.Land_Use), , MapWinGIS.GridDataType.LongDataType)
                                    If Not ResampleGrid(GisUtil.LayerFileName(.LandUseLayer), .LandUseGridFile) Then Return False
                                    If Not FilterGrid(.LandUseGridFile, BasinFile) Then Return False
                                    If Not LookupGrid(.LandUseGridFile, GridFile, .dictLandUse(.LanduseType)) Then Return False
                                    If Not GisUtil.AddLayerToGroup(.LandUseGridFile, SedimentLayer(enumSedimentLayers.Land_Use), GroupName) Then Return False
                                End If
                            End If
                        Case enumSedimentLayers.LS_Factors  'LS Factors
                            'resample DEM grid to same resolution as USLE
                            Dim DEMFile As String = CreateGrid(SedimentLayer(enumSedimentLayers.Elevations), , MapWinGIS.GridDataType.LongDataType)
                            If Not ResampleGrid(GisUtil.LayerFileName(.DEMLayer), DEMFile) Then Return False
                            'create new pit-filled, angle, and slope grids
                            Dim tkCallback As New clsTkCallback
                            Dim PitFile As String = CreateGrid(SedimentLayer(enumSedimentLayers.Elevations_Filled), , MapWinGIS.GridDataType.LongDataType)
                            MapWinGeoProc.Hydrology.Fill(DEMFile, PitFile, False)
                            SlopeFile = CreateGrid(SedimentLayer(enumSedimentLayers.Slopes))
                            AngleFile = CreateGrid(SedimentLayer(enumSedimentLayers.Flow_Direction))
                            'note: the D8 routine used by MapWinGeoProc gives slightly different answers than the one in the TauDEM lib (MWGP has some NoData cells scattered about!)
                            Dim tk As New TKTAUDEMLib.TauDEM
                            If tk.D8(PitFile, AngleFile, SlopeFile, "", 0) <> 0 Then Return False
                            If Not MultiplyGrid_LS(SlopeFile, GridFile) Then Return False
                        Case enumSedimentLayers.P_Factors  'P Factors
                            If .UseBMP Then
                                If Not SetGrid(GridFile, 1.0) Then Return False
                                If Not LookupGrid(GisUtil.LayerFileName(.BMPLayer), .BMPField, GridFile, .dictBMP) Then Return False
                            Else
                                If Not SetGrid(GridFile, 1.0) Then Return False
                            End If
                    End Select
                End With

                If Not FilterGrid(GridFile, BasinFile) Then Return False

                ProgressForm.Status = String.Format("Adding layer: {0} to {1} group...", GridName, GroupName)
                If Not GisUtil.AddLayerToGroup(GridFile, GridName, GroupName) Then Return False
                ApplyColoringScheme(GridName, Choose(i, MapWinGIS.PredefinedColorScheme.Glaciers, MapWinGIS.PredefinedColorScheme.Desert, MapWinGIS.PredefinedColorScheme.FallLeaves, MapWinGIS.PredefinedColorScheme.SummerMountains, MapWinGIS.PredefinedColorScheme.DeadSea))
                GisUtil.LayerVisible(GridName) = True
                GisUtil.ZoomToLayerExtents(GridName)

                GisUtil.SaveMapAsImage(String.Format("{0}\{1}\{2}.jpg", Project.SedimentFolder, Project.OutputFolder, GridName))

                'compute source sheet and rill erosion as product of all factors
                ProgressForm.Status = "Updating USLE grid..."
                If i = enumSedimentLayers.R_Factors Then
                    If Not MultiplyGrid(GridFile, 1.0, ErosionFile) Then Return False
                Else
                    If Not MultiplyGrid(ErosionFile, GridFile) Then Return False
                End If
            Next

            'create road erosion grid here...

            'note: methodology from original WCS sediment tool for road erosion is not available at this time--skip

            'compute sediment delivery

            'Compute Sediment Delivery Ratios

            GridName = SedimentLayer(enumSedimentLayers.Delivery_Ratio)
            GridFile = CreateGrid(GridName)
            ProgressForm.Status = String.Format("Creating new grid: {0}...", GridName)

            Select Case Project.DeliveryMethod
                Case enumDeliveryMethod.Distance
                    Dim StreamFile As String = CreateGrid(SedimentLayer(enumSedimentLayers.Stream_Grid))
                    If Not Delivery_Distance(ErosionFile, AngleFile, GridFile, StreamFile) Then Return False
                    If Not FilterGrid(StreamFile, BasinFile) Then Return False
                    If Not GisUtil.AddLayerToGroup(StreamFile, SedimentLayer(enumSedimentLayers.Stream_Grid), GroupName) Then Return False
                    ApplyColoringScheme(SedimentLayer(enumSedimentLayers.Stream_Grid), "Upland", Color.LightYellow, "Stream", Color.Blue, Project.StreamThreshold)
                    GisUtil.LayerVisible(SedimentLayer(enumSedimentLayers.Stream_Grid)) = True
                    GisUtil.SaveMapAsImage(String.Format("{0}\{1}\{2}.jpg", Project.SedimentFolder, Project.OutputFolder, SedimentLayer(enumSedimentLayers.Stream_Grid)))
                Case enumDeliveryMethod.Distance_Relief
                Case enumDeliveryMethod.Area
                    If Not Delivery_Area(GridFile) Then Return False
                Case enumDeliveryMethod.WEPP
            End Select

            ProgressForm.Status = String.Format("Adding layer: {0} to {1} group...", GridName, GroupName)
            If Not GisUtil.AddLayerToGroup(GridFile, GridName, GroupName) Then Return False
            ApplyColoringScheme(GridName, MapWinGIS.PredefinedColorScheme.Highway1)
            GisUtil.LayerVisible(GridName) = True
            GisUtil.SaveMapAsImage(String.Format("{0}\{1}\{2}.jpg", Project.SedimentFolder, Project.OutputFolder, GridName))

            Dim LayerName As String = SedimentLayer(enumSedimentLayers.USLE_Erosion)
            ProgressForm.Status = String.Format("Adding layer: {0} to {1} group...", LayerName, GroupName)
            If Not GisUtil.AddLayerToGroup(ErosionFile, LayerName, GroupName) Then Return False
            ApplyColoringScheme(LayerName, MapWinGIS.PredefinedColorScheme.Meadow)
            GisUtil.LayerVisible(LayerName) = True
            GisUtil.SaveMapAsImage(String.Format("{0}\{1}\{2}.jpg", Project.SedimentFolder, Project.OutputFolder, LayerName))

            GridName = SedimentLayer(enumSedimentLayers.USLE_Sediment)
            ProgressForm.Status = String.Format("Creating new grid: {0}...", GridName)
            Dim SedimentFile As String = CreateGrid(GridName)

            If Not MultiplyGrid(ErosionFile, GridFile, SedimentFile) Then Return False
            ProgressForm.Status = String.Format("Adding layer: {0} to {1} group...", GridName, GroupName)
            If Not GisUtil.AddLayerToGroup(SedimentFile, GridName, GroupName) Then Return False
            ApplyColoringScheme(LayerName, MapWinGIS.PredefinedColorScheme.ValleyFires)
            GisUtil.LayerVisible(GridName) = True
            GisUtil.SaveMapAsImage(String.Format("{0}\{1}\{2}.jpg", Project.SedimentFolder, Project.OutputFolder, GridName))
            Return True
        Catch ex As Exception
            ErrorMsg(, ex)
            Return False
        End Try
        Logger.Dbg("Complete")
    End Function

    ''' <summary>
    ''' GIRAS land use consists of multiple layers; these shapefiles are contained as field values in the "Land Use Index" layer
    ''' This routine will return the list of shapefiles contained in the GIRAS landuse coverages
    ''' Will return empty list if an error occurs
    ''' </summary>
    ''' <param name="BasinLayerIndex">Layer index associated with subbasins</param>
    Friend Function GIRASLayers(ByVal BasinLayerIndex As Integer) As Generic.List(Of Integer)
        Try
            Const LayerName As String = "Land Use Index", FieldName As String = "COVNAME"
            Dim ShapeFiles As New Generic.List(Of Integer)

            If GisUtil.IsLayer(LayerName) Then
                Dim LayerIndex As Integer = GisUtil.LayerIndex(LayerName)
                If GisUtil.IsField(LayerIndex, FieldName) Then
                    Dim FieldIndex As Integer = GisUtil.FieldIndex(LayerIndex, FieldName)
                    For i As Integer = 0 To GisUtil.NumFeatures(LayerIndex) - 1
                        Dim LyrName As String = GisUtil.FieldValue(LayerIndex, i, FieldIndex)
                        Dim LayerFileName As String = IO.Path.GetDirectoryName(GisUtil.LayerFileName(LayerIndex)) & "\landuse\" & LyrName & ".shp"
                        For j As Integer = 0 To GisUtil.NumFeatures(BasinLayerIndex) - 1
                            If GisUtil.OverlappingPolygons(LayerIndex, i, BasinLayerIndex, j) Then
                                If GisUtil.IsLayer(LayerFileName) Then ShapeFiles.Add(GisUtil.LayerIndex(LayerFileName))
                                Exit For
                            End If
                        Next j
                    Next
                End If
            End If
            Return ShapeFiles
        Catch ex As Exception
            ErrorMsg(, ex)
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Determine if layer and/or field are available
    ''' </summary>
    ''' <param name="LayerName">Name of layer</param>
    ''' <param name="FieldName">Name of field</param>
    Private Function LayerOK(ByVal LayerName As String, Optional ByVal FieldName As String = "") As Boolean
        Try
            If LayerName = "" OrElse Not GisUtil.IsLayer(LayerName) Then
                Logger.Message(String.Format("The layer named '{0}' was not specified or is not available; please correct this before continuing.", LayerName), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning, DialogResult.OK)
                Return False
            Else
                If FieldName <> "" Then
                    If Not GisUtil.IsField(GisUtil.LayerIndex(LayerName), FieldName) Then
                        Logger.Message(String.Format("The field named '{0}' for layer '{1}' was not specified or is invalid; please correct this before continuing.", FieldName, LayerName), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning, DialogResult.OK)
                        Return False
                    End If
                End If
            End If
            Return True
        Catch ex As Exception
            ErrorMsg(, ex)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Set values in grid to factor taken from ID in grid file using lookup table
    ''' </summary>
    ''' <param name="SourceFile">Name of source grid file </param>
    ''' <param name="DestFile">Name of destination grid file (must already exist)</param>
    Private Function LookupGrid(ByVal SourceFile As String, ByVal DestFile As String, ByVal dictLookup As Generic.Dictionary(Of String, clsLookup)) As Boolean
        Dim gSource As MapWinGIS.Grid = Nothing
        Dim gDest As MapWinGIS.Grid = Nothing

        Try
            gSource = New MapWinGIS.Grid
            If Not gSource.Open(SourceFile, , False) Then Return False
            gDest = New MapWinGIS.Grid
            If Not gDest.Open(DestFile, , False) Then Return False
            Dim Lookup As clsLookup = Nothing
            With gSource.Header
                For r As Integer = 0 To .NumberRows - 1
                    Dim arL(.NumberCols - 1), arV(.NumberCols - 1) As Single
                    gSource.GetRow(r, arL(0))
                    For c As Integer = 0 To .NumberCols - 1
                        Dim ID As String = arL(c)
                        If ID <> "" AndAlso dictLookup.TryGetValue(ID, Lookup) Then arV(c) = Lookup.Factor Else arV(c) = .NodataValue
                    Next
                    gDest.PutRow(r, arV(0))
                    If Not ProgressForm.SetProgress(r, .NumberRows - 1) Then Return False
                Next
            End With
            gDest.Save()
            Return True
        Catch ex As Exception
            ErrorMsg(, ex)
            Return False
        Finally
            If gSource IsNot Nothing Then gSource.Close()
            If gDest IsNot Nothing Then gDest.Close()
        End Try
    End Function

    ''' <summary>
    ''' Set values in grid to factor taken from ID in shape file using lookup table
    ''' </summary>
    ''' <param name="SourceFile">Name of source shape file </param>
    ''' <param name="FieldName">Name of field containing lookup value</param>
    ''' <param name="DestFile">Name of destination grid file (must already exist)</param>
    Private Function LookupGrid(ByVal SourceFile As String, ByVal FieldName As String, ByVal DestFile As String, ByVal dictLookup As Generic.Dictionary(Of String, clsLookup)) As Boolean
        Dim sfSource As MapWinGIS.Shapefile = Nothing
        Dim gDest As MapWinGIS.Grid = Nothing

        Try
            sfSource = New MapWinGIS.Shapefile
            If Not sfSource.Open(SourceFile) Then Return False
            sfSource.BeginPointInShapefile()

            gDest = New MapWinGIS.Grid
            If Not gDest.Open(DestFile, , True) Then Return False
            Dim Lookup As clsLookup = Nothing
            Dim lyrIndex As Integer = GisUtil.LayerIndex(SourceFile)
            Dim fldIndex As Integer = GisUtil.FieldIndex(lyrIndex, FieldName)
            With gDest.Header
                For r As Integer = 0 To .NumberRows - 1
                    Dim x, y As Double
                    gDest.CellToProj(0, r, x, y)
                    Dim arV(.NumberCols - 1) As Single
                    gDest.GetRow(r, arV(0))
                    For c As Integer = 0 To .NumberCols - 1
                        Dim shpIndex As Integer = sfSource.PointInShapefile(x, y)
                        If shpIndex <> -1 Then
                            Dim ID As String = GisUtil.FieldValue(lyrIndex, shpIndex, fldIndex)
                            If dictLookup.TryGetValue(ID, Lookup) Then
                                arV(c) = Lookup.Factor
                            Else
                                arV(c) = .NodataValue
                            End If
                        End If
                        x += .dX
                    Next
                    gDest.PutRow(r, arV(0))
                    If Not ProgressForm.SetProgress(r, .NumberRows - 1) Then Return False
                Next
            End With
            gDest.Save()
            Return True
        Catch ex As Exception
            ErrorMsg(, ex)
            Return False
        Finally
            If sfSource IsNot Nothing Then sfSource.Close()
            If gDest IsNot Nothing Then gDest.Close()
        End Try
    End Function

    ''' <summary>
    ''' Multiply two grids and put result in third grid (or back into source grid)
    ''' </summary>
    ''' <param name="SourceFile">Name of source grid file</param>
    ''' <param name="DestFile">Name of destination grid file that will be created (if blank, will put results back in source grid)</param>
    ''' <returns>True if successful</returns>
    Private Function MultiplyGrid(ByVal SourceFile As String, ByVal Multiplier As Double, Optional ByVal DestFile As String = "") As Boolean
        Try
            Dim gSource As New MapWinGIS.Grid
            Dim gDest As MapWinGIS.Grid = Nothing
            With gSource
                If .Open(SourceFile) Then
                    If DestFile <> "" Then
                        gDest = New MapWinGIS.Grid
                        If Not gDest.Open(DestFile) Then Return False
                    End If
                    With .Header
                        Dim arS(.NumberCols - 1), arD(.NumberCols - 1) As Single
                        For r As Integer = 0 To .NumberRows - 1
                            gSource.GetRow(r, arS(0))
                            For c As Integer = 0 To .NumberCols - 1
                                If arS(c) <> .NodataValue Then arD(c) = Math.Round(arS(c) * Multiplier, PRECISION) Else arD(c) = .NodataValue
                            Next
                            If gDest Is Nothing Then gSource.PutRow(r, arD(0)) Else gDest.PutRow(r, arD(0))
                            If Not ProgressForm.SetProgress(r, .NumberRows - 1) Then Return False
                        Next
                    End With
                    If gDest Is Nothing Then
                        .Save()
                    Else
                        gDest.Save()
                        gDest.Close()
                    End If
                    .Close()
                Else
                    Throw New Exception(String.Format("Unable to open {0} grid file in MultiplyGrid; error message was: {1}", SourceFile, .ErrorMsg(.LastErrorCode)))
                    Return False
                End If
            End With
            Return True
        Catch ex As Exception
            ErrorMsg(, ex)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Multiply two grids and put result in third grid (or back into source grid)
    ''' </summary>
    ''' <param name="SourceFile">Name of source grid file</param>
    ''' <param name="MultFile">Name of grid file containing multipliers</param>
    ''' <param name="DestFile">Name of destination grid file that will be created (if blank, will put results back in source grid)</param>
    ''' <returns>True if successful</returns>
    Private Function MultiplyGrid(ByVal SourceFile As String, ByVal MultFile As String, Optional ByVal DestFile As String = "") As Boolean
        Try
            Dim gSource As New MapWinGIS.Grid
            Dim gMult As New MapWinGIS.Grid
            Dim gDest As MapWinGIS.Grid = Nothing
            With gSource
                If .Open(SourceFile) AndAlso gMult.Open(MultFile) Then
                    If DestFile <> "" Then
                        gDest = New MapWinGIS.Grid
                        If Not gDest.Open(DestFile) Then Return False
                    End If
                    With .Header
                        For r As Integer = 0 To .NumberRows - 1
                            Dim arS(.NumberCols - 1), arD(.NumberCols - 1), arM(.NumberCols - 1) As Single
                            gSource.GetRow(r, arS(0))
                            gMult.GetRow(r, arM(0))
                            For c As Integer = 0 To .NumberCols - 1
                                If arS(c) <> .NodataValue And arM(c) <> .NodataValue Then arD(c) = Math.Round(arS(c) * arM(c), PRECISION) Else arD(c) = .NodataValue
                            Next
                            If gDest Is Nothing Then gSource.PutRow(r, arD(0)) Else gDest.PutRow(r, arD(0))
                            If Not ProgressForm.SetProgress(r, .NumberRows - 1) Then Return False
                        Next
                    End With
                    If gDest Is Nothing Then
                        .Save()
                    Else
                        gDest.Save()
                        gDest.Close()
                    End If
                    .Close()
                    gMult.Close()
                Else
                    Throw New Exception(String.Format("Unable to open {0} and/or {1} grid files in MultiplyGrid.", SourceFile, MultFile))
                    Return False
                End If
                Return True
            End With
        Catch ex As Exception
            ErrorMsg(, ex)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Multiply two grids and put result in third grid (or back into source grid) (Special version to compute LS factor)
    ''' </summary>
    ''' <param name="SourceFile">Name of source grid file (slopes)</param>
    ''' <param name="DestFile">Name of destination grid file that will be created</param>
    ''' <returns>True if successful</returns>
    Private Function MultiplyGrid_LS(ByVal SourceFile As String, ByVal DestFile As String) As Boolean
        Try
            Dim gSource As New MapWinGIS.Grid
            Dim gDest As MapWinGIS.Grid = Nothing
            With gSource
                If .Open(SourceFile) Then
                    gDest = New MapWinGIS.Grid
                    If Not gDest.CreateNew(DestFile, .Header, .DataType, .Header.NodataValue, False) Then
                        Throw New Exception(String.Format("Unable to create new grid file {0} in MultiplyGrid; error message was: {1}", DestFile, gDest.ErrorMsg(gDest.LastErrorCode)))
                        Return False
                    End If
                    With .Header
                        For r As Integer = 0 To .NumberRows - 1
                            Dim arS(.NumberCols - 1), arD(.NumberCols - 1), arM(.NumberCols - 1) As Single
                            gSource.GetRow(r, arS(0))
                            For c As Integer = 0 To .NumberCols - 1
                                If arS(c) <> .NodataValue Then
                                    Dim LS As Double
                                    Dim Slope As Double = arS(c)
                                    Dim SlopeLength As Double = Project.GridSize 'in meters
                                    Dim NN As Double
                                    If Slope < 0.01 Then
                                        NN = 0.2
                                    ElseIf Slope < 0.03 Then
                                        NN = 0.3
                                    ElseIf Slope < 0.05 Then
                                        NN = 0.4
                                    Else
                                        NN = 0.5
                                    End If
                                    'note: formula taken from http://www.omafra.gov.on.ca/english/engineer/facts/00-001.htm
                                    'this is equivalent to equation used in WCS, except WCS used constant value of 0.5 for NN and used English units
                                    LS = (0.065 + 4.56 * Slope + 65.41 * Slope ^ 2) * (SlopeLength / 22.1) ^ NN
                                    arD(c) = Math.Round(LS, PRECISION)
                                Else
                                    arD(c) = .NodataValue
                                End If
                            Next
                            If gDest Is Nothing Then gSource.PutRow(r, arD(0)) Else gDest.PutRow(r, arD(0))
                            If Not ProgressForm.SetProgress(r, .NumberRows - 1) Then Return False
                        Next
                    End With
                    If gDest Is Nothing Then
                        .Save()
                    Else
                        gDest.Save()
                        gDest.Close()
                    End If
                    .Close()
                Else
                    Throw New Exception(String.Format("Unable to open {0} grid file in MultiplyGrid_LS.", SourceFile))
                    Return False
                End If
                Return True
            End With
        Catch ex As Exception
            ErrorMsg(, ex)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Read tab-delimited set of items 
    ''' </summary>
    Private Function ReadLine(ByVal sr As IO.StreamReader) As Object()
        Dim s As String = sr.ReadLine
        Return s.Split(vbTab)
    End Function

    ''' <summary>
    ''' Take high-resolution source grid and create lower resolution destination grid
    ''' The source and destination grids must already exist (the destination should already be filtered for the desired area)
    ''' </summary>
    Private Function ResampleGrid(ByVal SourceFile As String, ByVal DestFile As String) As Boolean
        Try
            Dim gSource, gDest As New MapWinGIS.Grid
            If Not gSource.Open(SourceFile, , False) Then Return False
            If Not gDest.Open(DestFile, , False) Then Return False

            Dim SourceCols, SourceRows As Integer
            With gSource.Header
                SourceCols = .NumberCols
                SourceRows = .NumberRows
            End With
            With gDest.Header
                'get average value within the larger destination grid from the smaller source grid
                Dim ar(.NumberCols - 1) As Single
                For r As Integer = 0 To .NumberRows - 1
                    Dim x As Double, y As Double, cc As Integer, rr As Integer
                    gDest.CellToProj(0, r, x, y)
                    For c As Integer = 0 To .NumberCols - 1
                        gSource.ProjToCell(x, y, cc, rr)
                        'If cc >= 0 AndAlso cc <= gSource.Header.NumberCols - 1 AndAlso rr >= 0 AndAlso rr <= gSource.Header.NumberRows - 1 Then
                        If cc >= 0 AndAlso cc <= SourceCols - 1 AndAlso rr >= 0 AndAlso rr <= SourceRows - 1 Then
                            ar(c) = gSource.Value(cc, rr)
                        Else
                            ar(c) = .NodataValue
                        End If
                        x += .dX
                    Next
                    gDest.PutRow(r, ar(0))
                    'Debug.Print(CInt(My.Computer.Info.AvailablePhysicalMemory / 1000000.0))
                    If Not ProgressForm.SetProgress(r, .NumberRows - 1) Then Return False
                Next
            End With
            gSource.Close()
            gDest.Save()
            gDest.Close()
            Return True
        Catch ex As Exception
            ErrorMsg(, ex)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Save project data to .Sediment file
    ''' </summary>
    Friend Function SaveData() As Boolean
        Dim sw As IO.StreamWriter = Nothing
        Try
            With Project
                Dim Folder As String = "C:\BASINS\bin\Plugins\Sediment"
                If Not My.Computer.FileSystem.DirectoryExists(Folder) Then My.Computer.FileSystem.CreateDirectory(Folder)
                If .FileName = "" Then
                    With New SaveFileDialog
                        .AddExtension = True
                        .CheckFileExists = False
                        .CheckPathExists = True
                        .DefaultExt = ".Sediment"
                        .Filter = "Sediment files (*.sediment)|*.sediment"
                        .FilterIndex = 0
                        .InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\BASINS\Sediment"
                        If Not My.Computer.FileSystem.DirectoryExists(.InitialDirectory) Then My.Computer.FileSystem.CreateDirectory(.InitialDirectory)
                        .Title = "Save Sediment File"
                        If .ShowDialog <> Windows.Forms.DialogResult.OK Then .Dispose() : Return False
                        Project.FileName = .FileName
                        .Dispose()
                    End With
                End If
                sw = New IO.StreamWriter(.FileName)
                WriteLine(sw, "Version", 1.0)
                WriteLine(sw, "Output name:", .OutputFolder)
                WriteLine(sw, "Grid cell size:", .GridSize)
                WriteLine(sw, "Subbasins layer:", .SubbasinLayer)

                WriteLine(sw, "Soil layer:", .SoilLayer)
                WriteLine(sw, "Soil type ID:", .SoilField)

                WriteLine(sw, "Landuse type:", CInt(.LanduseType))
                WriteLine(sw, "Landuse layer:", .LandUseLayer)
                WriteLine(sw, "Landuse ID:", .LandUseField)
                WriteLine(sw, "Road layer:", .RoadLayer)
                WriteLine(sw, "Road ID:", .RoadField)

                WriteLine(sw, "DEM layer:", .DEMLayer)
                WriteLine(sw, "DEM units:", .DEMUnits)

                WriteLine(sw, "Rainfall factor:", .R_Factor)

                For i As Integer = 1 To 3
                    For j As Integer = 0 To Choose(i, 0, enumLandUseType.UserGrid, 0)
                        Dim dict As Generic.Dictionary(Of String, clsLookup) = Choose(i, .dictSoil, .dictLandUse(j), .dictBMP)
                        WriteLine(sw, String.Format("Num {0}:", Choose(i, "Erodibility", "Cropping-" & Choose(j + 1, "GIRAS", "NLCD-1992", "NLCD-2001", "USER-SHAPEFILE", "USER-GRID"), "BMP")), dict.Count)
                        For Each s As String In dict.Keys
                            With dict.Item(s)
                                WriteLine(sw, s, .Description, .Factor)
                            End With
                        Next
                    Next
                Next
                WriteLine(sw, "Use BMPs?", .UseBMP)
                WriteLine(sw, "BMP Layer:", .BMPLayer)
                WriteLine(sw, "BMP ID:", .BMPField)

                WriteLine(sw, "Sediment Method:", CInt(.DeliveryMethod))
                WriteLine(sw, "Stream Grid Layer:", .StreamLayer)
                WriteLine(sw, "Stream Threshold:", .StreamThreshold)
            End With
            Return True
        Catch ex As Exception
            ErrorMsg("Error while trying to save data.", ex)
            Return False
        Finally
            If sw IsNot Nothing Then
                sw.Close()
                sw.Dispose()
            End If
        End Try
    End Function

    ''' <summary>
    ''' Return name of predefined layer used for sediment utility
    ''' </summary>
    ''' <param name="SedLayer">Type of sediment layer</param>
    ''' <returns>Name of layer</returns>
    ''' <remarks>This helper routine written to give intellisense help for returning standard layer names</remarks>
    Private Function SedimentLayer(ByVal SedLayer As enumSedimentLayers) As String
        Return SedimentLayers(SedLayer)
    End Function

    ''' <summary>
    ''' Set all grid values to fixed value 
    ''' </summary>
    Private Function SetGrid(ByVal DestFile As String, ByVal Value As Double) As Boolean
        Dim gDest As New MapWinGIS.Grid
        Try
            With gDest
                .Open(DestFile, , False)
                With .Header
                    For r As Integer = 0 To .NumberRows - 1
                        Dim ar(.NumberCols - 1) As Single
                        For c As Integer = 0 To .NumberCols - 1
                            ar(c) = Value
                        Next c
                        gDest.PutRow(r, ar(0))
                        If Not ProgressForm.SetProgress(r, .NumberRows - 1) Then Return False
                    Next
                End With
                .Save()
            End With
            Return True
        Catch ex As Exception
            ErrorMsg(, ex)
            Return False
        Finally
            gDest.Close()
            gDest = Nothing
        End Try
    End Function

    ''' <summary>
    ''' Given a grid over a large extent, set all values that fall inside a filter layer shape to specified value
    ''' </summary>
    Private Function SetGrid(ByVal GridFile As String, ByVal FilterFile As String, ByVal ShapeIndex As Integer, ByVal Value As Double) As Boolean
        Dim sfFilter As New MapWinGIS.Shapefile
        Dim g As New MapWinGIS.Grid
        Try
            sfFilter.Open(GisUtil.LayerFileName(GisUtil.LayerIndex(FilterFile)))
            sfFilter.BeginPointInShapefile()
            g.Open(GridFile, , False)
            With g.Header
                For r As Integer = 0 To .NumberRows - 1
                    Dim x, y As Double
                    g.CellToProj(0, r, x, y)
                    Dim ar(.NumberCols - 1) As Single
                    If Not g.GetRow(r, ar(0)) Then Return False
                    For c As Integer = 0 To .NumberCols - 1
                        If sfFilter.PointInShapefile(x, y) = ShapeIndex Then ar(c) = Value
                        x += .dX
                    Next
                    g.PutRow(r, ar(0))
                    If Not ProgressForm.SetProgress(r, .NumberRows - 1) Then Return False
                Next
            End With
            sfFilter.EndPointInShapefile()
            g.Save()
            Return True
        Catch ex As Exception
            ErrorMsg(, ex)
            Return False
        Finally
            sfFilter.Close()
            g.Close()
        End Try
    End Function

    ''' <summary>
    ''' After all sediment calculations are complete, echo project data, GIS map snapshots, and summary results to HTML report that will be displayed on the last tab of the main form
    ''' </summary>
    ''' <returns>Name of .htm output file that was created</returns>
    Friend Function SummaryReport() As String
        With Project
            Dim hb As New clsHTMLBuilder
            hb.AppendHeading(clsHTMLBuilder.enumHeading.Level2, clsHTMLBuilder.enumAlign.Center, "BASINS Clean Sediment Tool")

            hb.AppendTable(100, clsHTMLBuilder.enumWidthUnits.Percent, clsHTMLBuilder.enumBorderStyle.none, , clsHTMLBuilder.enumDividerStyle.None)
            hb.AppendTableColumn("", , , 200, clsHTMLBuilder.enumWidthUnits.Pixels)
            hb.AppendTableColumn("")

            hb.AppendTableRow("Filename:", Project.FileName)
            hb.AppendTableRowEmpty()
            hb.AppendTableRow("Output Name:", .OutputFolder)
            hb.AppendTableRow("Grid Size (m):", .GridSize)
            hb.AppendTableRow("Subbasins Layer:", .SubbasinLayer)
            hb.AppendTableRowEmpty()
            hb.AppendTableRow("Soil Type Layer:", .SoilLayer)
            hb.AppendTableRow("Soil Type ID Field:", .SoilField)
            hb.AppendTableRowEmpty()

            hb.AppendTableRow("Land Use Layer Type:", .LanduseType.ToString)
            If .LanduseType <> enumLandUseType.GIRAS Then
                hb.AppendTableRow("Land Use Layer:", .LandUseLayer)
                If .LanduseType = enumLandUseType.UserShapefile Then
                    hb.AppendTableRow("Land Use Field:", .LandUseField)
                End If
            End If
            hb.AppendTableRowEmpty()

            hb.AppendTableRow("DEM Layer:", .DEMLayer)
            hb.AppendTableRow("DEM Elevation Units:", .DEMUnits.ToString)
            hb.AppendTableRowEmpty()

            hb.AppendTableRow("R Factor:", .R_Factor)
            hb.AppendTableRowEmpty()
            hb.AppendTableEnd()

            hb.AppendImage("R Factors.jpg", 75, clsHTMLBuilder.enumWidthUnits.Percent)
            hb.AppendHeading(clsHTMLBuilder.enumHeading.Level5, clsHTMLBuilder.enumAlign.Left, "R Factor Map")
            hb.AppendHorizLine()

            AppendLookupTable(hb, "Soil Type/Erodibility Factors", Project.SoilLayer, Project.SoilField, Project.dictSoil)

            hb.AppendLineBreak()
            hb.AppendImage("K Factors.jpg", 75, clsHTMLBuilder.enumWidthUnits.Percent)
            hb.AppendHeading(clsHTMLBuilder.enumHeading.Level5, clsHTMLBuilder.enumAlign.Left, "K Factor Map")
            hb.AppendHorizLine()

            AppendLookupTable(hb, "Land Use/Cropping Factors", "Land Use", "", Project.dictLandUse(Project.LanduseType))

            hb.AppendLineBreak()
            hb.AppendImage("C Factors.jpg", 75, clsHTMLBuilder.enumWidthUnits.Percent)
            hb.AppendHeading(clsHTMLBuilder.enumHeading.Level5, clsHTMLBuilder.enumAlign.Left, "C Factor Map")
            hb.AppendHorizLine()

            hb.AppendImage("LS Factors.jpg", 75, clsHTMLBuilder.enumWidthUnits.Percent)
            hb.AppendHeading(clsHTMLBuilder.enumHeading.Level5, clsHTMLBuilder.enumAlign.Left, "LS Factor Map")
            hb.AppendHorizLine()

            hb.AppendTable(100, clsHTMLBuilder.enumWidthUnits.Percent, clsHTMLBuilder.enumBorderStyle.none, , clsHTMLBuilder.enumDividerStyle.None)
            hb.AppendTableColumn("", , , 200, clsHTMLBuilder.enumWidthUnits.Pixels)
            hb.AppendTableColumn("")
            hb.AppendTableRowEmpty()
            hb.AppendTableRow("Use BMPs?", IIf(.UseBMP, "Yes", "No"))
            hb.AppendTableRowEmpty()

            If .UseBMP Then
                hb.AppendTableRow("BMP Layer:", .BMPLayer)
                hb.AppendTableRow("BMP Type Field:", .BMPField)
                hb.AppendTableEnd()
                AppendLookupTable(hb, "BMP Type/Conservation Practice Factors", Project.BMPLayer, Project.BMPField, Project.dictBMP)
                hb.AppendPara(clsHTMLBuilder.enumAlign.Left, "")
                hb.AppendImage("P Factors.jpg", 75, clsHTMLBuilder.enumWidthUnits.Percent)
                hb.AppendHeading(clsHTMLBuilder.enumHeading.Level5, clsHTMLBuilder.enumAlign.Left, "P Factor Map")
            End If
            hb.AppendHorizLine()

            hb.AppendTable(100, clsHTMLBuilder.enumWidthUnits.Percent, clsHTMLBuilder.enumBorderStyle.none, , clsHTMLBuilder.enumDividerStyle.None)
            hb.AppendTableColumn("", , , 200, clsHTMLBuilder.enumWidthUnits.Pixels)
            hb.AppendTableColumn("")

            hb.AppendTableRowEmpty()
            hb.AppendTableRow("Sediment Delivery Method:", .DeliveryMethod.ToString)
            If .DeliveryMethod <> enumDeliveryMethod.Area Then hb.AppendTableRow("Area Threshold (km):", .StreamThreshold)
            hb.AppendTableRowEmpty()
            hb.AppendTableEnd()

            If .DeliveryMethod <> enumDeliveryMethod.Area Then
                hb.AppendImage("Stream Grid.jpg", 75, clsHTMLBuilder.enumWidthUnits.Percent)
                hb.AppendHeading(clsHTMLBuilder.enumHeading.Level5, clsHTMLBuilder.enumAlign.Left, "Stream Grid Map")
            End If

            hb.AppendHorizLine()
            hb.AppendImage("Delivery Ratio.jpg", 75, clsHTMLBuilder.enumWidthUnits.Percent)
            hb.AppendHeading(clsHTMLBuilder.enumHeading.Level5, clsHTMLBuilder.enumAlign.Left, "Delivery Ratio Map")

            hb.AppendHorizLine()
            hb.AppendImage("USLE Sheet & Rill Erosion.jpg", 75, clsHTMLBuilder.enumWidthUnits.Percent)
            hb.AppendHeading(clsHTMLBuilder.enumHeading.Level5, clsHTMLBuilder.enumAlign.Left, "USLE Sheet & Rill Erosion Map")

            hb.AppendHorizLine()
            hb.AppendImage("USLE Sediment.jpg", 75, clsHTMLBuilder.enumWidthUnits.Percent)
            hb.AppendHeading(clsHTMLBuilder.enumHeading.Level5, clsHTMLBuilder.enumAlign.Left, "USLE Sediment Map")
            hb.AppendHorizLine()

            AppendResultsTable(hb, "Erosion and Sediment Calculation Results")

            For i As Integer = 0 To GisUtil.NumFeatures(GisUtil.LayerIndex(Project.SubbasinLayer)) - 1
                hb.AppendHorizLine()
                AppendUSLESummaryTable(hb, i)
            Next

            Dim OutputFolder As String = .SedimentFolder & "\" & .OutputFolder
            If Not My.Computer.FileSystem.DirectoryExists(OutputFolder) Then My.Computer.FileSystem.CreateDirectory(OutputFolder)
            Dim OutputFile As String = OutputFolder & "\Sediment.htm"
            hb.Save(OutputFile)
            Return OutputFile
        End With
    End Function

    ''' <summary>
    ''' Given a detailed shapefile or grid layer (e.g., land use), tabulate areas within each subbasin
    ''' </summary>
    ''' <param name="LayerName">Name of detailed layer</param>
    ''' <param name="FieldName">Name of shapefile field to summarize by (ignored if LayerName refers to grid file)</param>
    Friend Function TabulateAreas(ByVal LayerName As String, Optional ByVal FieldName As String = "") As Generic.SortedDictionary(Of String, Double())
        Dim SourceLayerIndex As Integer = GisUtil.LayerIndex(LayerName)
        Dim BasinLayerIndex As Integer = GisUtil.LayerIndex(Project.SubbasinLayer)
        Dim d As Generic.SortedDictionary(Of String, Double())
        If GisUtil.LayerType(SourceLayerIndex) = MapWindow.Interfaces.eLayerType.PolygonShapefile Then
            Dim SourceFieldIndex As Integer = GisUtil.FieldIndex(SourceLayerIndex, FieldName)
            d = GisUtil.TabulatePolygonAreas(SourceLayerIndex, SourceFieldIndex, BasinLayerIndex)
        Else 'grid
            d = GisUtil.TabulateAreas(SourceLayerIndex, BasinLayerIndex)
        End If
        'convert to acres
        For Each k As String In d.Keys
            For i As Integer = 0 To d.Item(k).Length - 1
                d.Item(k)(i) /= (Project.DistFactor ^ 2 * 4046.86) 'convert to meters then square meters to acres
            Next
        Next
        Return d
    End Function

    ''' <summary>
    ''' Create HTML table containing Erosion factors and areas
    ''' </summary>
    ''' <param name="hb">Active HTMLBuilder</param>
    ''' <param name="TableName">Desired title to be placed before table</param>
    ''' <param name="LayerName">GIS layer name (e.g., soil shapfile layer)</param>
    ''' <param name="LayerField">If layer is shapefile, fieldname used for lookup</param>
    ''' <param name="LookupDict">Dictionary that related lookup ID with USLE factors</param>
    Private Sub AppendLookupTable(ByVal hb As HTMLBuilder.clsHTMLBuilder, ByVal TableName As String, ByVal LayerName As String, ByVal LayerField As String, ByVal LookupDict As Generic.Dictionary(Of String, clsLookup))

        ProgressForm.Status = String.Format("Tabulating {0} areas...", TableName)

        hb.AppendHeading(clsHTMLBuilder.enumHeading.Level4, clsHTMLBuilder.enumAlign.Left, TableName)
        hb.AppendTable()
        For c As Integer = 0 To 3 + GisUtil.NumFeatures(GisUtil.LayerIndex(Project.SubbasinLayer)) - 1
            If c = 0 Then
                hb.AppendTableColumn("ID", clsHTMLBuilder.enumAlign.Center)
            ElseIf c = 1 Then
                hb.AppendTableColumn("Description", clsHTMLBuilder.enumAlign.Left)
            ElseIf c = 2 Then
                hb.AppendTableColumn("Factor", clsHTMLBuilder.enumAlign.Right)
            Else
                Dim AreaNum As String = ""
                If GisUtil.NumFeatures(GisUtil.LayerIndex(Project.SubbasinLayer)) > 1 Then AreaNum = " " & c - 2
                hb.AppendTableColumn(String.Format("Area{0}\n(ac)", AreaNum), , clsHTMLBuilder.enumAlign.Right)
            End If
        Next
        Dim dictArea As Generic.SortedDictionary(Of String, Double()) = TabulateAreas(LayerName, LayerField)
        For Each key As String In dictArea.Keys
            hb.AppendTableRow()
            hb.AppendTableCell(key)
            If LookupDict.ContainsKey(key) Then
                hb.AppendTableCell(LookupDict(key).Description)
                hb.AppendTableCell(LookupDict(key).Factor.ToString("0.0000"))
            Else
                hb.AppendTableCell("")
                hb.AppendTableCell("")
            End If
            For c As Integer = 0 To dictArea.Item(key).Length - 1
                hb.AppendTableCell(String.Format("{0:0.0}", dictArea.Item(key)(c)))
            Next
            hb.AppendTableCellEnd()
        Next
        hb.AppendTableRowEnd()
        hb.AppendTableEnd()
    End Sub

    ''' <summary>
    ''' Create HTML table containing USLE final results
    ''' </summary>
    ''' <param name="hb">Active HTMLBuilder</param>
    ''' <param name="TableName">Desired title to be placed before table</param>
    Private Sub AppendResultsTable(ByVal hb As HTMLBuilder.clsHTMLBuilder, ByVal TableName As String)
        ProgressForm.Status = String.Format("Tabulating {0} summary...", TableName)

        hb.AppendHeading(clsHTMLBuilder.enumHeading.Level4, clsHTMLBuilder.enumAlign.Left, TableName)
        hb.AppendTable()
        hb.AppendTableColumn("Watershed", clsHTMLBuilder.enumAlign.Center)
        hb.AppendTableColumn("Area\n(ac)", clsHTMLBuilder.enumAlign.Center)
        hb.AppendTableColumn("Erosion\n(t/ac/yr)", clsHTMLBuilder.enumAlign.Center)
        hb.AppendTableColumn("Delivery\nRatio", clsHTMLBuilder.enumAlign.Center)
        hb.AppendTableColumn("Sediment\n(t/ac/yr)", clsHTMLBuilder.enumAlign.Center)

        Dim sf As New MapWinGIS.Shapefile
        sf.Open(GisUtil.LayerFileName(Project.SubbasinLayer))
        sf.BeginPointInShapefile()

        Dim gE As New MapWinGIS.Grid
        gE.Open(GisUtil.LayerFileName(SedimentLayer(enumSedimentLayers.USLE_Erosion)))

        Dim gS As New MapWinGIS.Grid
        gS.Open(GisUtil.LayerFileName(SedimentLayer(enumSedimentLayers.USLE_Sediment)))

        For i As Integer = 0 To GisUtil.NumFeatures(GisUtil.LayerIndex(Project.SubbasinLayer)) - 1
            'tabulate total area and weighted loads
            Dim TotalArea As Single = 0, TotalErosion As Single = 0, TotalSediment As Single = 0
            With gE.Header
                For r As Integer = 0 To .NumberRows - 1
                    Dim x, y As Double
                    gE.CellToProj(0, r, x, y)
                    Dim arE(.NumberCols - 1) As Single
                    Dim arS(.NumberCols - 1) As Single
                    gE.GetRow(r, arE(0))
                    gS.GetRow(r, arS(0))
                    For c As Integer = 0 To .NumberCols - 1
                        If arE(c) <> .NodataValue AndAlso sf.PointInShape(i, x, y) Then
                            TotalArea += 1
                            TotalErosion += arE(c)
                            TotalSediment += arS(c)
                        End If
                        x += .dX
                    Next
                Next
                TotalErosion /= TotalArea 'this gives weighted average in t/ac/yr
                TotalSediment /= TotalArea
                TotalArea *= .dX * .dY / (Project.DistFactor ^ 2 * 4046.86) 'convert to meters then square meters to acres
            End With
            hb.AppendTableRow()
            hb.AppendTableCell("Subbasin " & i + 1)
            hb.AppendTableCell(TotalArea.ToString("0.0"))
            hb.AppendTableCell(TotalErosion.ToString("0.00"))
            hb.AppendTableCell((TotalSediment / TotalErosion).ToString("0.00"))
            hb.AppendTableCell(TotalSediment.ToString("0.00"))
            hb.AppendTableCellEnd()
            hb.AppendTableRowEnd()
        Next
        hb.AppendTableEnd()

        gE.Close()
        sf.EndPointInShapefile()
        sf.Close()
    End Sub

    ''' <summary>
    ''' Create HTML table containing summary of USLE factors used for specified subbasin
    ''' </summary>
    ''' <param name="hb">Active HTMLBuilder</param>
    ''' <param name="SubbasinIndex">Index number of desired subbasin</param>
    Private Sub AppendUSLESummaryTable(ByVal hb As HTMLBuilder.clsHTMLBuilder, ByVal SubbasinIndex As Integer)
        Dim TableName As String = String.Format("Subbasin {0} USLE Parameters", SubbasinIndex + 1)

        ProgressForm.Status = String.Format("Tabulating {0} summary...", TableName)

        hb.AppendHeading(clsHTMLBuilder.enumHeading.Level4, clsHTMLBuilder.enumAlign.Left, TableName)
        hb.AppendTable()
        hb.AppendTableColumn("Item", clsHTMLBuilder.enumAlign.Center)
        hb.AppendTableColumn("Min", clsHTMLBuilder.enumAlign.Center)
        hb.AppendTableColumn("Max", clsHTMLBuilder.enumAlign.Center)
        hb.AppendTableColumn("Mean", clsHTMLBuilder.enumAlign.Center)

        Dim sf As New MapWinGIS.Shapefile
        sf.Open(GisUtil.LayerFileName(Project.SubbasinLayer))
        sf.BeginPointInShapefile()

        For i As enumSedimentLayers = enumSedimentLayers.R_Factors To enumSedimentLayers.USLE_Sediment
            Dim g As New MapWinGIS.Grid
            g.Open(GisUtil.LayerFileName(SedimentLayer(i)))

            Dim CellCount As Single = 0, MeanFactor As Single = 0, MinFactor As Single = Single.MaxValue, MaxFactor As Single = Single.MinValue
            With g.Header
                For r As Integer = 0 To .NumberRows - 1
                    Dim x, y As Double
                    g.CellToProj(0, r, x, y)
                    Dim ar(.NumberCols - 1) As Single
                    g.GetRow(r, ar(0))
                    For c As Integer = 0 To .NumberCols - 1
                        If ar(c) <> .NodataValue AndAlso ar(c) <> 0.0 AndAlso sf.PointInShape(SubbasinIndex, x, y) Then
                            CellCount += 1
                            MeanFactor += ar(c)
                            MinFactor = Math.Min(MinFactor, ar(c))
                            MaxFactor = Math.Max(MaxFactor, ar(c))
                        End If
                        x += .dX
                    Next
                Next
                MeanFactor /= CellCount 'this gives weighted average 
                hb.AppendTableRow()
                hb.AppendTableCell(clsHTMLBuilder.enumAlign.Center, SedimentLayer(i))
                If CellCount > 0 Then
                    hb.AppendTableCell(MinFactor.ToString("0.00"))
                    hb.AppendTableCell(MaxFactor.ToString("0.00"))
                    hb.AppendTableCell(MeanFactor.ToString("0.00"))
                End If
                hb.AppendTableCellEnd()
                hb.AppendTableRowEnd()
            End With
            g.Close()
        Next
        hb.AppendTableEnd()

        sf.EndPointInShapefile()
        sf.Close()
    End Sub

    ''' <summary>
    ''' Apply predefined coloring scheme to grid layer
    ''' </summary>
    ''' <param name="LayerName">Name of layer</param>
    ''' <param name="ColoringScheme">Predefined coloring scheme</param>
    Private Sub ApplyColoringScheme(ByVal LayerName As String, ByVal ColoringScheme As MapWinGIS.PredefinedColorScheme)
        Dim g As New MapWinGIS.Grid
        If Not g.Open(GisUtil.LayerFileName(LayerName), , False) Then Exit Sub

        Dim scheme As New MapWinGIS.GridColorScheme
        With scheme
            .UsePredefined(g.Minimum * 0.999, g.Maximum * 1.001, ColoringScheme) 'prevent round-off errors from causing empty cells
        End With
        g.Close()

        GisUtil.ColoringScheme(GisUtil.LayerIndex(LayerName)) = scheme
    End Sub

    ''' <summary>
    ''' Apply custom coloring scheme to grid layer (two ranges, each with separate color)
    ''' </summary>
    ''' <param name="LayerName">Name of layer</param>
    ''' <param name="Caption1">Caption assigned to lower range</param>
    ''' <param name="Color1">Color assigned to lower range</param>
    ''' <param name="Caption2">Caption assigned to upper range</param>
    ''' <param name="Color2">Color assigned to upper range</param>
    ''' <param name="BreakValue">Grid value that separates two ranges</param>
    Private Sub ApplyColoringScheme(ByVal LayerName As String, ByVal Caption1 As String, ByVal Color1 As System.Drawing.Color, ByVal Caption2 As String, ByVal Color2 As System.Drawing.Color, ByVal BreakValue As Double)
        Dim g As New MapWinGIS.Grid
        g.Open(GisUtil.LayerFileName(LayerName), , False)

        Dim scheme As New MapWinGIS.GridColorScheme
        Dim b1 As New MapWinGIS.GridColorBreak
        With b1
            .Caption = Caption1
            .LowValue = g.Minimum
            .HighValue = BreakValue
            .ColoringType = MapWinGIS.ColoringType.Gradient
            .LowColor = System.Convert.ToUInt32(RGB(Color1.R, Color1.G, Color1.B))
            .HighColor = .LowColor
        End With
        scheme.InsertBreak(b1)

        Dim b2 As New MapWinGIS.GridColorBreak
        With b2
            .Caption = Caption2
            .LowValue = BreakValue * 1.0001
            .HighValue = g.Maximum * 1.0001
            .ColoringType = MapWinGIS.ColoringType.Gradient
            .LowColor = System.Convert.ToUInt32(RGB(Color2.R, Color2.G, Color2.B))
            .HighColor = .LowColor
        End With
        scheme.InsertBreak(b2)

        g.Close()

        GisUtil.ColoringScheme(GisUtil.LayerIndex(LayerName)) = scheme
    End Sub

    ''' <summary>
    ''' Display error message
    ''' </summary>
    ''' <param name="ErrorText">Error text to display</param>
    ''' <param name="ex">Exception (will display traceback info)</param>
    Private Sub ErrorMsg(Optional ByVal ErrorText As String = "", Optional ByVal ex As Exception = Nothing)
        If ErrorText = "" Then ErrorText = "An unhandled error has occurred in the BASINS Sediment tool."
        If ex IsNot Nothing Then ErrorText &= vbCr & vbCr & "The detailed error message was:" & vbCr & vbCr & ex.ToString
        Logger.Message(ErrorText, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, DialogResult.OK)
    End Sub

    ''' <summary>
    ''' Load all settings and coefficients from data file; if blank will load from default file
    ''' </summary>
    ''' <param name="Filename"></param>
    ''' <remarks></remarks>
    Friend Sub LoadData(Optional ByVal Filename As String = "")
        Dim sr As IO.StreamReader = Nothing
        Try
            With Project
                Dim Folder As String = "C:\BASINS\bin\Plugins\Sediment"
                Dim DefaultFilename As String = Folder & "\Default.sediment"
                If Filename = "" Then Filename = DefaultFilename
                If Not My.Computer.FileSystem.FileExists(Filename) Then 'create the default file from the file in resources
                    Dim sw As New IO.StreamWriter(Filename)
                    sw.Write(My.Resources.Defaults)
                    sw.Close()
                    sw.Dispose()
                End If
                sr = New IO.StreamReader(Filename)
                Dim Version As Single = 0.0
                .Initialize()
                If Filename <> DefaultFilename Then .FileName = Filename
                While Not sr.EndOfStream
                    Dim ar() As String = ReadLine(sr)
                    Dim Key As String = ar(0).ToLower, Value As String = ar(1)
                    Select Case Key
                        Case "version" : Version = Val(Value)
                        Case "output name:" : .OutputFolder = Value
                        Case "grid cell size:" : .GridSize = Val(Value)
                        Case "subbasins layer:" : .SubbasinLayer = Value
                        Case "soil layer:" : .SoilLayer = Value
                        Case "soil type id:" : .SoilField = Value
                        Case "landuse type:" : .LanduseType = Val(Value)
                        Case "landuse layer:" : .LandUseLayer = Value
                        Case "landuse id:" : .LandUseField = Value
                        Case "road layer:" : .RoadLayer = Value
                        Case "road id:" : .RoadField = Value
                        Case "dem layer:" : .DEMLayer = Value
                        Case "dem units:" : .DEMUnits = Val(Value)
                        Case "rainfall factor:" : .R_Factor = Val(Value)
                        Case "use bmps?" : .UseBMP = Value
                        Case "bmp layer:" : .BMPLayer = Value
                        Case "bmp id:" : .BMPField = Value
                        Case "sediment method:" : .DeliveryMethod = Val(Value)
                        Case "stream grid layer:" : .StreamLayer = Value
                        Case "stream threshold:" : .StreamThreshold = Val(Value)
                        Case Else
                            If Key.StartsWith("num") Then
                                Dim num As Integer = Val(Value)
                                Dim dict As Generic.Dictionary(Of String, clsLookup)
                                If Key.Contains("erodibility") Then
                                    dict = .dictSoil
                                ElseIf Key.Contains("giras") Then
                                    dict = .dictLandUse(enumLandUseType.GIRAS)
                                ElseIf Key.Contains("nlcd-1992") Then
                                    dict = .dictLandUse(enumLandUseType.NLCD_1992)
                                ElseIf Key.Contains("nlcd-2001") Then
                                    dict = .dictLandUse(enumLandUseType.NLCD_2001)
                                ElseIf Key.Contains("shapefile") Then
                                    dict = .dictLandUse(enumLandUseType.UserShapefile)
                                ElseIf Key.Contains("grid") Then
                                    dict = .dictLandUse(enumLandUseType.UserGrid)
                                ElseIf Key.Contains("bmp") Then
                                    dict = .dictBMP
                                Else
                                    dict = Nothing
                                End If
                                For j As Integer = 0 To num - 1
                                    Dim arDict() As String = ReadLine(sr)
                                    If Not dict.ContainsKey(ar(0)) Then dict.Add(arDict(0), New clsLookup(arDict(1), arDict(2)))
                                Next
                            Else
                                Logger.Message("Error while trying to load data; unrecognized keyword: " & Key, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, Windows.Forms.DialogResult.OK)
                                Exit Sub
                            End If
                    End Select

                End While
            End With
        Catch ex As Exception
            ErrorMsg("Error while trying to load data.", ex)
        Finally
            If sr IsNot Nothing Then
                sr.Close()
                sr.Dispose()
            End If
        End Try
    End Sub

    ''' <summary>
    ''' Write tab-delimited set of items
    ''' </summary>
    ''' <param name="sw"></param>
    ''' <param name="item"></param>
    ''' <remarks></remarks>
    Private Sub WriteLine(ByVal sw As IO.StreamWriter, ByVal ParamArray item() As Object)
        For i As Integer = 0 To item.Length - 1
            If i > 0 Then sw.Write(vbTab)
            sw.Write(item(i))
        Next
        sw.WriteLine()
    End Sub
End Module
