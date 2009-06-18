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
<CLSCompliant(False)> _
Public Module Sediment

    Public Const REGAPP As String = "GBMM"

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
            DeliveryMethod = enumDeliveryMethod.Area
            Modified = False
            SedimentFolder = IO.Path.GetDirectoryName(GisUtil.ProjectFileName) & "\Sediment"
            If Not My.Computer.FileSystem.DirectoryExists(SedimentFolder) Then My.Computer.FileSystem.CreateDirectory(SedimentFolder)
        End Sub

        ''' <summary>
        ''' Return the current gridsize expressed as square kilometers
        ''' </summary>
        Friend Function CellAreaKm() As Double
            Return (GridSize / DistFactor / 1000) ^ 2
        End Function

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

    Friend SedimentForm As frmSediment = Nothing
    Friend ProgressForm As frmProgress = Nothing
    Friend Project As structProject
    Public gMapWin As MapWindow.Interfaces.IMapWin

    Dim SedimentLayers() As String = {"R Factors", "K Factors", "C Factors", "LS Factors", "P Factors", "Delivery Ratio", "USLE Sheet & Rill Erosion", "USLE Sediment", "Land Use", "Elevations", "Elevations (Filled)", "Slopes", "Flow Direction", "Stream Grid"}

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
            Dim DistFile As String = modGrid.CreateGrid(Project.SedimentFolder, "Distances to Streams", Project.SubbasinLayer)
            Dim Threshold As Integer = Project.StreamThreshold / (Project.GridSize / 1000) ^ 2 'convert gridsize to sq. km., then compute number of cells
            tk.Distance(AngleFile, StreamFile, DistFile, Threshold, 0)

            'now use distances to streams to compute delivery ratios at each grid cell

            With gUSLE
                If .Open(ErosionFile) And gDist.Open(DistFile) And gDest.Open(GridFile) Then
                    With .Header
                        Dim NoDataValue As Single = .NodataValue
                        For r As Integer = 0 To .NumberRows - 1
                            Dim arU(.NumberCols - 1), arD(.NumberCols - 1), arO(.NumberCols - 1) As Single
                            gUSLE.GetRow(r, arU(0))
                            gDist.GetRow(r, arD(0))
                            For c As Integer = 0 To .NumberCols - 1
                                If arU(c) <> NoDataValue Then
                                    Dim D As Double = arD(c) / Project.DistFactor 'convert to meters
                                    Dim M As Double = arU(c) 'USLE erosion in tons/ac/yr
                                    Dim L As Double = 5.1 + 1.79 * M 'meters
                                    Dim DR As Double = Math.Max(0, (1 - 0.97 * D / L)) 'delivery ratio (Md/M)
                                    If arD(c) <> NoDataValue AndAlso arU(c) <> NoDataValue Then
                                        arO(c) = Math.Round(DR, PRECISION)
                                    Else
                                        arO(c) = NoDataValue
                                    End If
                                Else
                                    arO(c) = NoDataValue
                                End If
                            Next
                            gDest.PutRow(r, arO(0))
                            If Not ProgressForm.SetProgress(r, .NumberRows - 1) Then Return False
                        Next
                    End With
                    gDest.AssignNewProjection(GisUtil.ProjectProjection)
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

    Friend Function GenerateLoads() As Boolean
        Try
            'create a new grid using the specified gridsize, positioned over selected subbasin layer
            Dim TaskNum As Integer = 0, NumTasks As Integer = 23
            ProgressForm.SetProgressOverall("Initializing...", TaskNum, NumTasks) : TaskNum += 1

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

                If Not LayerOK("Subbasins", "General", .SubbasinLayer) Then Return False
                If Not LayerOK("Soil Type", "Soil Type", .SoilLayer, .SoilField) Then Return False
                If .LanduseType = enumLandUseType.GIRAS Then
                    If GIRASLayers(GisUtil.LayerIndex(.SubbasinLayer)).Count = 0 Then
                        Logger.Msg("No GIRAS layers were found!")
                        Return False
                    End If
                Else
                    If Not LayerOK("Land Use", "Land Use", .LandUseLayer, .LandUseField) Then Return False
                End If
                If Not LayerOK("DEM", "DEM", .DEMLayer) Then Return False
                If .UseBMP AndAlso Not LayerOK("BMP", "BMPs", .BMPLayer, .BMPField) Then Return False
                'If .DeliveryMethod <> enumDeliveryMethod.Area AndAlso Not LayerOK(.StreamLayer) Then Return False

                'create report folder
                Dim outfolder As String = String.Format("{0}\{1}", Project.SedimentFolder, Project.OutputFolder)
                If Not My.Computer.FileSystem.DirectoryExists(outfolder) Then My.Computer.FileSystem.CreateDirectory(outfolder)
            End With

            Dim ErosionFile As String = CreateGrid(Project.SedimentFolder, SedimentLayer(enumSedimentLayers.USLE_Erosion), Project.SubbasinLayer)
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
                GridFile = CreateGrid(Project.SedimentFolder, GridName, Project.SubbasinLayer)
                ProgressForm.SetProgressOverall(String.Format("Creating new grid: {0}...", GridName), TaskNum, NumTasks) : TaskNum += 1
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
                                    If Not GisUtil.AddLayerToGroup(GridFile, SedimentLayer(enumSedimentLayers.Land_Use), GroupName) Then Return False
                                Else
                                    'resample landuse grid to same resolution as USLE
                                    .LandUseGridFile = CreateGrid(Project.SedimentFolder, SedimentLayer(enumSedimentLayers.Land_Use), Project.SubbasinLayer, MapWinGIS.GridDataType.LongDataType)
                                    If Not ResampleGrid(GisUtil.LayerFileName(.LandUseLayer), .LandUseGridFile) Then Return False
                                    If Not FilterGrid(.LandUseGridFile, BasinFile) Then Return False
                                    If Not LookupGrid(.LandUseGridFile, GridFile, .dictLandUse(.LanduseType)) Then Return False
                                    If Not GisUtil.AddLayerToGroup(.LandUseGridFile, SedimentLayer(enumSedimentLayers.Land_Use), GroupName) Then Return False
                                End If
                            End If
                        Case enumSedimentLayers.LS_Factors  'LS Factors
                            'resample DEM grid to same resolution as USLE
                            Dim DEMFile As String = CreateGrid(Project.SedimentFolder, SedimentLayer(enumSedimentLayers.Elevations), Project.SubbasinLayer, MapWinGIS.GridDataType.LongDataType)
                            If Not ResampleGrid(GisUtil.LayerFileName(.DEMLayer), DEMFile) Then Return False
                            'create new pit-filled, angle, and slope grids
                            Dim tkCallback As New clsTkCallback
                            Dim PitFile As String = CreateGrid(Project.SedimentFolder, SedimentLayer(enumSedimentLayers.Elevations_Filled), Project.SubbasinLayer, MapWinGIS.GridDataType.LongDataType)
                            MapWinGeoProc.Hydrology.Fill(DEMFile, PitFile, False)
                            SlopeFile = CreateGrid(Project.SedimentFolder, SedimentLayer(enumSedimentLayers.Slopes), Project.SubbasinLayer)
                            AngleFile = CreateGrid(Project.SedimentFolder, SedimentLayer(enumSedimentLayers.Flow_Direction), Project.SubbasinLayer)
                            'note: the D8 routine used by MapWinGeoProc gives slightly different answers than the one in the TauDEM lib (MWGP has some NoData cells scattered about!)
                            'If MapWinGeoProc.Hydrology.D8(PitFile, AngleFile, SlopeFile, tkCallback) <> 0 Then Return False
                            Dim tk As New TKTAUDEMLib.TauDEM
                            If tk.D8(PitFile, AngleFile, SlopeFile, "", 0) <> 0 Then Return False
                            If Not MultiplyGrid_LS(SlopeFile, GridFile, Project.GridSize) Then Return False
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

                ProgressForm.SetProgressOverall(String.Format("Adding layer: {0} to {1} group...", GridName, GroupName), TaskNum, NumTasks) : TaskNum += 1
                If Not GisUtil.AddLayerToGroup(GridFile, GridName, GroupName) Then Return False
                ApplyColoringScheme(GridName, Choose(i, MapWinGIS.PredefinedColorScheme.Glaciers, MapWinGIS.PredefinedColorScheme.Desert, MapWinGIS.PredefinedColorScheme.FallLeaves, MapWinGIS.PredefinedColorScheme.SummerMountains, MapWinGIS.PredefinedColorScheme.DeadSea))
                GisUtil.LayerVisible(GridName) = True
                GisUtil.ZoomToLayerExtents(GridName)

                GisUtil.SaveMapAsImage(String.Format("{0}\{1}\{2}.jpg", Project.SedimentFolder, Project.OutputFolder, GridName))

                'compute source sheet and rill erosion as product of all factors
                ProgressForm.SetProgressOverall("Updating USLE grid...", TaskNum, NumTasks) : TaskNum += 1
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
            GridFile = CreateGrid(Project.SedimentFolder, GridName, Project.SubbasinLayer)
            ProgressForm.SetProgressOverall(String.Format("Creating new grid: {0}...", GridName), TaskNum, NumTasks) : TaskNum += 1

            Select Case Project.DeliveryMethod
                Case enumDeliveryMethod.Distance
                    Dim StreamFile As String = CreateGrid(Project.SedimentFolder, SedimentLayer(enumSedimentLayers.Stream_Grid), Project.SubbasinLayer)
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

            ProgressForm.SetProgressOverall(String.Format("Adding layer: {0} to {1} group...", GridName, GroupName), TaskNum, NumTasks) : TaskNum += 1
            If Not GisUtil.AddLayerToGroup(GridFile, GridName, GroupName) Then Return False
            ApplyColoringScheme(GridName, MapWinGIS.PredefinedColorScheme.Highway1)
            GisUtil.LayerVisible(GridName) = True
            GisUtil.SaveMapAsImage(String.Format("{0}\{1}\{2}.jpg", Project.SedimentFolder, Project.OutputFolder, GridName))

            Dim LayerName As String = SedimentLayer(enumSedimentLayers.USLE_Erosion)
            ProgressForm.SetProgressOverall(String.Format("Adding layer: {0} to {1} group...", LayerName, GroupName), TaskNum, NumTasks) : TaskNum += 1
            If Not GisUtil.AddLayerToGroup(ErosionFile, LayerName, GroupName) Then Return False
            ApplyColoringScheme(LayerName, MapWinGIS.PredefinedColorScheme.Meadow)
            GisUtil.LayerVisible(LayerName) = True
            GisUtil.SaveMapAsImage(String.Format("{0}\{1}\{2}.jpg", Project.SedimentFolder, Project.OutputFolder, LayerName))

            GridName = SedimentLayer(enumSedimentLayers.USLE_Sediment)
            ProgressForm.SetProgressOverall(String.Format("Creating new grid: {0}...", GridName), TaskNum, NumTasks) : TaskNum += 1
            Dim SedimentFile As String = CreateGrid(Project.SedimentFolder, GridName, Project.SubbasinLayer)

            If Not MultiplyGrid(ErosionFile, GridFile, SedimentFile) Then Return False
            ProgressForm.SetProgressOverall(String.Format("Adding layer: {0} to {1} group...", GridName, GroupName), TaskNum, NumTasks) : TaskNum += 1
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
    ''' <param name="LayerType">Text description so warning message is more descriptive if not found.</param>
    ''' <param name="TabName">Name of tab for warning message</param>
    ''' <param name="LayerName">Name of layer</param>
    ''' <param name="FieldName">Name of field (if applicable)</param>
    Private Function LayerOK(ByVal LayerType As String, ByVal TabName As String, ByVal LayerName As String, Optional ByVal FieldName As String = "") As Boolean
        Try
            If LayerName = "" OrElse Not GisUtil.IsLayer(LayerName) Then
                Logger.Message(String.Format("The {0} layer on the {1} tab named '{2}' is required but was not specified or is not available; please correct this before continuing.", LayerType, TabName, LayerName), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning, DialogResult.OK)
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
    ''' Read tab-delimited set of items 
    ''' </summary>
    Private Function ReadLine(ByVal sr As IO.StreamReader) As Object()
        Dim s As String = sr.ReadLine
        Return s.Split(vbTab)
    End Function

    ''' <summary>
    ''' Save project data to .Sediment file
    ''' </summary>
    Friend Function SaveData() As Boolean
        Dim sw As IO.StreamWriter = Nothing
        Try
            With Project
                'Dim Folder As String = "C:\BASINS\bin\Plugins\Sediment"
                'If Not My.Computer.FileSystem.DirectoryExists(Folder) Then My.Computer.FileSystem.CreateDirectory(Folder)
                If .FileName = "" Then
                    With New SaveFileDialog
                        .AddExtension = True
                        .CheckFileExists = False
                        .CheckPathExists = True
                        .DefaultExt = ".Sediment"
                        .Filter = "Sediment files (*.sediment)|*.sediment"
                        .FilterIndex = 0
                        .InitialDirectory = Project.SedimentFolder
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
                WriteLine(sw, "DEM units:", CInt(.DEMUnits))

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
    ''' After all sediment calculations are complete, echo project data, GIS map snapshots, and summary results to HTML report that will be displayed on the last tab of the main form
    ''' </summary>
    ''' <returns>Name of .htm output file that was created</returns>
    Friend Function SummaryReport() As String
        Try
            With Project
                Dim TaskNum As Integer = 0, NumTasks As Integer = 5

                ProgressForm.SetProgressOverall("Refreshing Results report...", TaskNum, NumTasks) : TaskNum += 1

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

                ProgressForm.SetProgressOverall("Appending Soil Type/Erodibility Factors table...", TaskNum, NumTasks) : TaskNum += 1
                AppendLookupTable(hb, "Soil Type/Erodibility Factors", Project.SoilLayer, Project.SoilField, Project.dictSoil)

                hb.AppendLineBreak()
                hb.AppendImage("K Factors.jpg", 75, clsHTMLBuilder.enumWidthUnits.Percent)
                hb.AppendHeading(clsHTMLBuilder.enumHeading.Level5, clsHTMLBuilder.enumAlign.Left, "K Factor Map")
                hb.AppendHorizLine()

                ProgressForm.SetProgressOverall("Appending Land Use/Cropping Factors table...", TaskNum, NumTasks) : TaskNum += 1

                If .LanduseType = enumLandUseType.GIRAS Then
                    Dim lyrList As Generic.List(Of Integer) = GIRASLayers(GisUtil.LayerIndex(.SubbasinLayer))
                    If lyrList Is Nothing Then
                        Logger.Message("The GIRAS land use index layer was not found, or is in the wrong format.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, Windows.Forms.DialogResult.OK)
                    End If
                    For Each lyr As Integer In lyrList
                        AppendLookupTable(hb, "Land Use/Cropping Factors", GisUtil.LayerName(lyr), "LUCODE", Project.dictLandUse(Project.LanduseType))
                    Next
                Else
                    AppendLookupTable(hb, "Land Use/Cropping Factors", SedimentLayer(enumSedimentLayers.Land_Use), "", Project.dictLandUse(Project.LanduseType))
                End If

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
                    ProgressForm.SetProgressOverall("Appending BMP Type/Conservation Practice Factors table...", TaskNum, NumTasks) : TaskNum += 1
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

                ProgressForm.SetProgressOverall("Appending Erosion and Sediment Calculation Results table...", TaskNum, NumTasks) : TaskNum += 1

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
        Catch ex As Exception
            ErrorMsg(, ex)
            Return ""
        End Try
    End Function

    ''' <summary>
    ''' Given a detailed shapefile or grid layer (e.g., land use), tabulate areas within each subbasin
    ''' </summary>
    ''' <param name="LayerName">Name of detailed layer</param>
    ''' <param name="FieldName">Name of shapefile field to summarize by (ignored if LayerName refers to grid file)</param>
    Friend Function TabulateAreas(ByVal LayerName As String, Optional ByVal FieldName As String = "") As Generic.SortedDictionary(Of String, Double())
        Try
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
        Catch ex As Exception
            ErrorMsg(, ex)
            Return Nothing
        End Try
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
        Try
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
        Catch ex As Exception
            ErrorMsg(, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Create HTML table containing USLE final results
    ''' </summary>
    ''' <param name="hb">Active HTMLBuilder</param>
    ''' <param name="TableName">Desired title to be placed before table</param>
    Private Sub AppendResultsTable(ByVal hb As HTMLBuilder.clsHTMLBuilder, ByVal TableName As String)
        Try
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
                    Dim NoDataValue As Single = .NodataValue
                    For r As Integer = 0 To .NumberRows - 1
                        Dim x, y As Double
                        gE.CellToProj(0, r, x, y)
                        Dim arE(.NumberCols - 1) As Single
                        Dim arS(.NumberCols - 1) As Single
                        gE.GetRow(r, arE(0))
                        gS.GetRow(r, arS(0))
                        For c As Integer = 0 To .NumberCols - 1
                            If arE(c) <> NoDataValue AndAlso sf.PointInShape(i, x, y) Then
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

            gE.AssignNewProjection(GisUtil.ProjectProjection)
            gS.AssignNewProjection(GisUtil.ProjectProjection)

            gE.Close()
            sf.EndPointInShapefile()
            sf.Close()
        Catch ex As Exception
            ErrorMsg(, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Create HTML table containing summary of USLE factors used for specified subbasin
    ''' </summary>
    ''' <param name="hb">Active HTMLBuilder</param>
    ''' <param name="SubbasinIndex">Index number of desired subbasin</param>
    Private Sub AppendUSLESummaryTable(ByVal hb As HTMLBuilder.clsHTMLBuilder, ByVal SubbasinIndex As Integer)
        Try
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
                    Dim NoDataValue As Single = .NodataValue
                    For r As Integer = 0 To .NumberRows - 1
                        Dim x, y As Double
                        g.CellToProj(0, r, x, y)
                        Dim ar(.NumberCols - 1) As Single
                        g.GetRow(r, ar(0))
                        For c As Integer = 0 To .NumberCols - 1
                            If ar(c) <> NoDataValue AndAlso ar(c) <> 0.0 AndAlso sf.PointInShape(SubbasinIndex, x, y) Then
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
        Catch ex As Exception
            ErrorMsg(, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Display error message
    ''' </summary>
    ''' <param name="ErrorText">Error text to display</param>
    ''' <param name="ex">Exception (will display traceback info)</param>
    Friend Sub ErrorMsg(Optional ByVal ErrorText As String = "", Optional ByVal ex As Exception = Nothing)
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
                Dim Folder As String = IO.Path.GetDirectoryName(Reflection.Assembly.GetEntryAssembly.Location) & "\Plugins\Sediment"
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

#Region "Get and Save Form Window Positions and Sizes and control values"

    <System.Diagnostics.DebuggerStepThrough()> _
    Public Sub GetWindowPos(ByRef RegAppName As String, ByVal FormName As String, ByRef x As Int16, ByRef y As Int16, ByRef w As Int16, ByRef h As Int16, ByRef WindowState As FormWindowState)

        Dim Index, UpperBound As Int16
        Dim maxw, maxh As Int16

        'Gets an array of all the screens connected to the system.

        Dim Screens() As System.Windows.Forms.Screen = System.Windows.Forms.Screen.AllScreens
        UpperBound = Screens.GetUpperBound(0)

        For Index = 0 To UpperBound
            With Screens(Index).WorkingArea
                maxw = Math.Max(maxw, .Right)
                maxh = Math.Max(maxh, .Bottom)
            End With
        Next

        If FormName = "" Then FormName = "PrintPreview"
        w = CInt(GetSetting(RegAppName, FormName, "W", w))
        h = CInt(GetSetting(RegAppName, FormName, "H", h))
        x = CInt(GetSetting(RegAppName, FormName, "X", x))
        y = CInt(GetSetting(RegAppName, FormName, "Y", y))
        If x + w > maxw Then x = maxw - w
        x = Math.Max(0, x)
        If y + h > maxh Then y = maxh - h
        y = Math.Max(0, y)
        If CInt(GetSetting(RegAppName, FormName, "Maximized", CStr(0))) = 1 Then WindowState = FormWindowState.Maximized
    End Sub

    <System.Diagnostics.DebuggerStepThrough()> _
    Public Sub GetWindowPos(ByRef RegAppName As String, ByRef f As System.Windows.Forms.Form)
        Dim ws As Windows.Forms.FormWindowState
        Dim dummy As Integer
        With f
            If .IsMdiChild Then 'only set size, not position
                GetWindowPos(RegAppName, .Name, dummy, dummy, .Width, .Height, ws)
            Else
                If f.FormBorderStyle = FormBorderStyle.Sizable Or f.FormBorderStyle = FormBorderStyle.SizableToolWindow Then
                    GetWindowPos(RegAppName, .Name, .Left, .Top, .Width, .Height, ws)
                Else
                    GetWindowPos(RegAppName, .Name, .Left, .Top, dummy, dummy, ws)
                End If
            End If
        End With
    End Sub

    <System.Diagnostics.DebuggerStepThrough()> _
    Public Sub SaveWindowPos(ByRef RegAppName As String, ByVal FormName As String, _
           ByVal x As Int16, ByVal y As Int16, ByVal w As Int16, ByVal h As Int16, _
           ByRef WindowState As FormWindowState)

        If FormName = "" Then FormName = "PrintPreview"

        If WindowState = vbNormal Then
            SaveSetting(RegAppName, FormName, "W", w)
            SaveSetting(RegAppName, FormName, "H", h)
            SaveSetting(RegAppName, FormName, "X", x)
            SaveSetting(RegAppName, FormName, "Y", y)
        End If
        SaveSetting(RegAppName, FormName, "Maximized", IIf(WindowState = FormWindowState.Maximized, 1, 0))
    End Sub

    <System.Diagnostics.DebuggerStepThrough()> _
    Public Sub SaveWindowPos(ByRef RegAppName As String, ByRef f As System.Windows.Forms.Form)
        With f
            SaveWindowPos(RegAppName, .Name, .Left, .Top, .Width, .Height, .WindowState)
        End With
    End Sub

    ''' <summary>
    ''' Get last (or default) value for specified control that was saved in the registry
    ''' </summary>
    ''' <param name="RegAppName">Name of application</param>
    ''' <param name="Cntl">Control to retrieve value for</param>
    ''' <param name="DefaultValue">If not already in registry, will set to this value (text, checked, or selected index)</param>
    ''' <remarks></remarks>
    Public Sub GetControlValue(ByVal RegAppName As String, ByRef Cntl As Control, ByVal DefaultValue As String)
        If Not Cntl.Enabled Then Exit Sub
        Dim Value As String = GetSetting(RegAppName, Cntl.FindForm.Name, Cntl.Name, DefaultValue)
        If TypeOf Cntl Is TextBox Then
            CType(Cntl, TextBox).Text = Value
        ElseIf TypeOf Cntl Is CheckBox Then
            If Value = "" Then Value = "False"
            CType(Cntl, CheckBox).Checked = CBool(Value)
        ElseIf TypeOf Cntl Is RadioButton Then
            If Value = "" Then Value = "False"
            If CBool(Value) Then CType(Cntl, RadioButton).Checked = True 'only set if true, to avoid triggering check-changed
        ElseIf TypeOf Cntl Is ComboBox Then
            'first, retrieve list of items
            Dim ListItems As String = GetSetting(RegAppName, Cntl.FindForm.Name, Cntl.Name & "_Items", "")
            With CType(Cntl, ComboBox)
                If .DropDownStyle = ComboBoxStyle.DropDown Then
                    .Text = Value
                Else
                    If Value = "" Then Value = "0"
                    Dim idx As Integer = Val(Value)
                    If .Items.Count = 0 Then
                        For Each s As String In ListItems.Split(";")
                            .Items.Add(s)
                        Next
                    End If
                    If idx > .Items.Count - 1 Then .SelectedIndex = -1 Else .SelectedIndex = idx
                End If
                If .Text = "" And .Items.Count > 0 Then .SelectedIndex = 0
            End With
        ElseIf TypeOf Cntl Is ListBox Then
            If Value = "" Then Value = "-1"
            Dim idx As Integer = Val(Value)
            With CType(Cntl, ListBox)
                If idx > .Items.Count - 1 Then .SelectedIndex = -1 Else .SelectedIndex = idx
            End With
        ElseIf TypeOf Cntl Is DateTimePicker Then
            With CType(Cntl, DateTimePicker)
                If IsDate(Value) Then
                    .Value = Value
                Else
                    .Value = DateTime.Now
                End If
            End With
        Else
            'not all control types are supported; if invalid control type is passed, will ignore
            'Debug.Print("Invalid control in GetControlValue: " & Cntl.GetType.ToString)
            'Debug.Assert(False)
        End If
    End Sub

    ''' <summary>
    ''' Get last values for all controls on a form that was saved in the registry (default values cannot be set explictly, will use defaults from designer)
    ''' </summary>
    ''' <param name="RegAppName">Name of application</param>
    ''' <param name= "Container">Form or control containing controls to set values for</param>
    ''' <remarks>Want to get and set in order of tag index, as there may be cascading events</remarks>
    Public Sub GetControlValues(ByVal RegAppName As String, ByRef Container As Control)
        For Indx As Integer = 0 To Container.Controls.Count - 1
            For Each Cntl As Control In Container.Controls
                If Cntl.TabIndex = Indx And Cntl.Visible Then
                    GetControlValue(RegAppName, Cntl, "")
                    GetControlValues(RegAppName, Cntl)
                End If
            Next
        Next
    End Sub

    ''' <summary>
    ''' Save value for specified control to registry
    ''' </summary>
    ''' <param name="RegAppName">Name of application</param>
    ''' <param name="Cntl">Control to set value for</param>
    ''' <remarks></remarks>
    Public Sub SaveControlValue(ByVal RegAppName As String, ByRef Cntl As Control)
        Dim Value As String
        If TypeOf Cntl Is TextBox Then
            Value = CType(Cntl, TextBox).Text
        ElseIf TypeOf Cntl Is CheckBox Then
            Value = CType(Cntl, CheckBox).Checked.ToString
        ElseIf TypeOf Cntl Is RadioButton Then
            Value = CType(Cntl, RadioButton).Checked.ToString
        ElseIf TypeOf Cntl Is ComboBox Then
            With CType(Cntl, ComboBox)
                If .DropDownStyle = ComboBoxStyle.DropDown Then
                    Value = .Text
                Else
                    Value = .SelectedIndex.ToString
                End If
                Dim ListItems As String = ""
                For Each s As String In .Items
                    ListItems &= IIf(ListItems = "", "", ";") & s
                Next
                SaveSetting(RegAppName, Cntl.FindForm.Name, Cntl.Name & "_Items", ListItems)
            End With
        ElseIf TypeOf Cntl Is ListBox Then
            Value = CType(Cntl, ListBox).SelectedIndex.ToString
        ElseIf TypeOf Cntl Is DateTimePicker Then
            Value = CType(Cntl, DateTimePicker).Value.ToString
        Else
            'Debug.Assert(False)
            Exit Sub
        End If
        SaveSetting(RegAppName, Cntl.FindForm.Name, Cntl.Name, Value)
    End Sub

    ''' <summary>
    ''' Save values for all controls on a form to registry
    ''' </summary>
    ''' <param name="RegAppName">Name of application</param>
    ''' <param name= "Container">Form or control containing controls to set values for</param>
    ''' <remarks>Want to get and set in order of tag index, as there may be cascading events</remarks>
    Public Sub SaveControlValues(ByVal RegAppName As String, ByRef Container As Control)
        For Indx As Integer = 0 To Container.Controls.Count - 1
            For Each Cntl As Control In Container.Controls
                If Cntl.TabIndex = Indx Then
                    SaveControlValue(RegAppName, Cntl)
                    SaveControlValues(RegAppName, Cntl) 'recursively look at controls that may be contained within this control
                End If
            Next
        Next
    End Sub

#End Region
End Module
