Imports atcUtility
Imports MapWinUtility
Imports System.Runtime.InteropServices
Imports System.Net
#If GISProvider = "DotSpatial" Then
Imports DotSpatial.Controls
Imports DotSpatial.Data
Imports DotSpatial.Symbology
Imports DotSpatial.Extensions
Imports atcMwGisUtility
#Else
#End If

Public Class frmDownload

    Public Const CancelString As String = "<Cancel>"
#If GISProvider = "DotSpatial" Then
    Private pMapWin As AppManager
#Else
    Private pMapWin As MapWindow.Interfaces.IMapWin
#End If
    Private pApplicationName As String = ""
    Private pOk As Boolean = False

    Private pRegionViewRectangle As String = "View Rectangle"
    Private pRegionExtentSelectedLayer As String = "Extent of Selected Layer"
    Private pRegionExtentSelectedShapes As String = "Extent of Selected Shapes"
    Private pRegionEnterCoordinates As String = "Enter Coordinates of Rectangle"
    Private pRegionHydrologicUnit As String = "Hydrologic Unit"
    Private pRegionStationIDs As String = "Station IDs"

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    ''' <summary>Determine if a process is running on a 64 bit Operating System but in 32 bit emulation mode (WOW64)</summary>
    ''' <param name="hProcess">A handle to the process to check</param>
    ''' <param name="Wow64Process">Output parameter. A boolean that will be set to True if the process is running in WOW64 mode</param>
    <System.Runtime.InteropServices.DllImportAttribute("kernel32.dll", EntryPoint:="IsWow64Process")>
    Public Shared Function IsWow64Process(<System.Runtime.InteropServices.InAttribute()> ByVal hProcess As System.IntPtr, <System.Runtime.InteropServices.OutAttribute()> ByRef Wow64Process As Boolean) As <System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.Bool)> Boolean
    End Function

    ''' <summary>Determines if a method exists in the specified DLL</summary>
    ''' <param name="moduleName">The DLL to look for the method in</param>
    ''' <param name="methodName">The method to look for</param>
    Public Shared Function DoesWin32MethodExist(ByVal moduleName As String, ByVal methodName As String) As Boolean
        Dim moduleHandle As IntPtr = GetModuleHandle(moduleName)
        If (moduleHandle = IntPtr.Zero) Then
            Return False
        End If
        Return (GetProcAddress(moduleHandle, methodName) <> IntPtr.Zero)
    End Function

    ''' <summary>Gets a handle to a specified DLL</summary>
    ''' <param name="moduleName">The module to return a handle for</param>
    <Runtime.ConstrainedExecution.ReliabilityContract(Runtime.ConstrainedExecution.Consistency.WillNotCorruptState, Runtime.ConstrainedExecution.Cer.MayFail), DllImport("kernel32.dll", CharSet:=CharSet.Auto, SetLastError:=True)>
    Public Shared Function GetModuleHandle(ByVal moduleName As String) As IntPtr
    End Function

    ''' <summary>Retrieves the address of an exported function or variable from the specified dynamic-link library (DLL)</summary>
    ''' <param name="hModule">A handle to the DLL to look for the method in</param>
    ''' <param name="methodName">The method to look for</param>
    <DllImport("kernel32.dll", CharSet:=CharSet.Ansi, SetLastError:=True, ExactSpelling:=True)>
    Public Shared Function GetProcAddress(ByVal hModule As IntPtr, ByVal methodName As String) As IntPtr
    End Function

    ''' <summary>Determine whether the operating system is 64 bit</summary>
    Public ReadOnly Property Is64BitOperatingSystem() As Boolean
        Get
            If IntPtr.Size = 8 Then
                Return True
            Else
                Dim Is64 As Boolean = False
                If DoesWin32MethodExist("kernel32.dll", "IsWow64Process") Then
                    IsWow64Process(Process.GetCurrentProcess.Handle, Is64)
                End If
                Return Is64
            End If
        End Get
    End Property

#If GISProvider = "DotSpatial" Then
    Public Function AskUser(ByVal aMapWin As AppManager, ByVal aParentHandle As Integer) As String
        pMapWin = aMapWin
        pApplicationName = "Hydro Toolbox"
#Else
    Public Function AskUser(ByVal aMapWin As MapWindow.Interfaces.IMapWin, ByVal aParentHandle As Integer) As String
        pMapWin = aMapWin
        pApplicationName = pMapWin.ApplicationInfo.ApplicationName
#End If
        'The following line hot-wires the form to just do met data download
        'chkBASINS_Met.Checked = True : cboRegion.SelectedIndex = 0 ': Me.Height = 141 ': Return Me.XML

#If GISProvider = "DotSpatial" Then
        Dim lExtents As Extent
        Dim lNumLayers = pMapWin.Map.Layers.Count
#Else
        Dim lExtents As MapWinGIS.Extents = Nothing
        Dim lNumLayers = pMapWin.Layers.NumLayers
#End If
        If pMapWin IsNot Nothing AndAlso lNumLayers > 0 Then
            Try
#If GISProvider = "DotSpatial" Then
                lExtents = pMapWin.Map.Extent
                If lExtents.MinX < lExtents.MaxX AndAlso lExtents.MinY < lExtents.MaxY Then
                    cboRegion.Items.Add(pRegionViewRectangle)
                End If
#Else
                lExtents = pMapWin.View.Extents
                If lExtents.xMin < lExtents.xMax AndAlso lExtents.yMin < lExtents.yMax Then
                    cboRegion.Items.Add(pRegionViewRectangle)
                End If
#End If
            Catch
            End Try

            Try
#If GISProvider = "DotSpatial" Then
                Dim lCurrentLayer As IMapFeatureLayer = pMapWin.Map.Layers.SelectedLayer
                If lCurrentLayer IsNot Nothing Then
                    lExtents = lCurrentLayer.Extent
                    If lExtents.MinX < lExtents.MaxY AndAlso lExtents.MinY < lExtents.MaxY Then
                        cboRegion.Items.Add(pRegionExtentSelectedLayer)
                    End If
                End If
#Else
                lExtents = pMapWin.Layers(pMapWin.Layers.CurrentLayer).Extents
                If lExtents.xMin < lExtents.yMax AndAlso lExtents.yMin < lExtents.yMax Then
                    cboRegion.Items.Add(pRegionExtentSelectedLayer)
                End If
#End If
            Catch
            End Try

            Try
#If GISProvider = "DotSpatial" Then
                lExtents = pMapWin.Map.Extent
                If lExtents.MinX < lExtents.MaxY AndAlso lExtents.MinY < lExtents.MaxY Then
                    cboRegion.Items.Add(pRegionExtentSelectedShapes)
                End If
#Else
                lExtents = pMapWin.View.SelectedShapes.SelectBounds
                If lExtents.xMin < lExtents.yMax AndAlso lExtents.yMin < lExtents.yMax Then
                    cboRegion.Items.Add(pRegionExtentSelectedShapes)
                End If
#End If
            Catch
            End Try
        End If
        cboRegion.Items.Add(pRegionEnterCoordinates)
        cboRegion.Items.Add(pRegionStationIDs)
        If HUC8Layer() IsNot Nothing Then
            Dim lHUC8s As Generic.List(Of String) = HUC8s()
            If lHUC8s.Count = 1 Then
                cboRegion.Items.Add(pRegionHydrologicUnit & " " & lHUC8s(0))
            ElseIf lHUC8s.Count > 1 Then
                cboRegion.Items.Add(pRegionHydrologicUnit & "s")
            End If
        End If

        Dim lRegionTypeName As String = GetSetting("DataDownload", "Defaults", "RegionTypeName", pRegionHydrologicUnit)
        If lRegionTypeName.StartsWith(pRegionHydrologicUnit) Then
            lRegionTypeName = pRegionHydrologicUnit
        End If
        Dim lReason As String = ""
        If Not RegionValid(lRegionTypeName, lReason) Then 'Fall back on the option that is always valid
            MapWinUtility.Logger.Dbg("Could not use region type '" & lRegionTypeName & "' because '" & lReason & "' so defaulting to " & pRegionEnterCoordinates)
            lRegionTypeName = pRegionEnterCoordinates
        End If
        For lRegionIndex As Integer = 0 To cboRegion.Items.Count - 1
            If cboRegion.Items(lRegionIndex).ToString.StartsWith(lRegionTypeName) Then
                cboRegion.SelectedIndex = lRegionIndex
                Exit For
            End If
        Next
        If cboRegion.SelectedIndex < 0 Then
            cboRegion.SelectedIndex = 0
        End If

        Dim lGroupMargin As Integer = 6
        Dim lGroupY As Integer = cboRegion.Top + cboRegion.Height + lGroupMargin

        Dim lGroups As New List(Of System.Windows.Forms.GroupBox) '  Dictionary(Of String, Windows.Forms.GroupBox)
        If Is64BitOperatingSystem Then 'Old self-extracting archives do not work on 64 bit
            chkBASINS_DEM.Visible = False
        End If
        If pApplicationName.StartsWith("USGS GW Toolbox") OrElse pApplicationName.Contains("Hydro Toolbox") Then
            lGroups.Add(grpNWISStations_GW)
            lGroups.Add(grpNWIS_GW)
            'lGroups.Add(grpBASINS)
            lGroups.Add(grpNHDplus2)
            lGroups.Add(grpUSGS_Seamless)

            chkUSGS_Seamless_NLCD2001_Impervious.Visible = False
            chkUSGS_Seamless_NLCD2001_LandCover.Visible = False
            chkUSGS_Seamless_NLCD2004_LandCover.Visible = False
            chkUSGS_Seamless_NLCD2006_Impervious.Visible = False
            chkUSGS_Seamless_NLCD2006_LandCover.Visible = False
            chkUSGS_Seamless_NLCD2008_LandCover.Visible = False
            chkUSGS_Seamless_NLCD2011_Impervious.Visible = False
            chkUSGS_Seamless_NLCD2011_LandCover.Visible = False
            chkUSGS_Seamless_NLCD2013_LandCover.Visible = False

            'chkBASINS_LSTORET.Visible = False
            'chkBASINS_Census.Visible = False
            'chkBASINS_MetStations.Left = chkBASINS_Census.Left
            'chkBASINS_DEMG.Left = chkBASINS_NED.Left
            'chkBASINS_303d.Visible = False
            'chkBASINS_MetData.Left = chkBASINS_303d.Left
        Else
            lGroups.Add(grpBASINS)
            'lGroups.Add(grpNCDC)
            'lGroups.Add(grpNHDplus)
            lGroups.Add(grpNHDplus2)
            lGroups.Add(grpNWISStations)
            lGroups.Add(grpNWIS)
            lGroups.Add(grpUSGS_Seamless)
            'lGroups.Add(grpSTORET)
            lGroups.Add(grpNLDAS)
            lGroups.Add(grpSoils)
        End If

        For Each lGroup As System.Windows.Forms.GroupBox In lGroups
            lGroup.Top = lGroupY
            lGroup.Visible = True
            lGroupY += lGroup.Height + lGroupMargin
        Next

        Height = lGroupY + 85

        Dim lMinCount As String = GetSetting("DataDownload", "Defaults", "MinCount", "10")
        Dim lMinCountInt As Integer
        If Integer.TryParse(lMinCount, lMinCountInt) Then txtMinCount_GW.Text = lMinCountInt

        If GetSetting("DataDownload", "Defaults", "Clip", "").ToLower.Equals("true") Then
            chkClip.Checked = True
        End If

        If GetSetting("DataDownload", "Defaults", "Merge", "").ToLower.Equals("true") Then
            chkMerge.Checked = True
        End If

        If System.Windows.Forms.Application.ProductName = "USGSHydroToolbox" Then
            chkGetNewest.CheckState = System.Windows.Forms.CheckState.Checked
        End If

        SetCheckboxVisibilityFromMapOrRegion()
        TallyPreChecked(lGroups)
        SetColorsFromAvailability()
        lstParameters.Items.Add("Precip")   '"APCPsfc"
        lstParameters.Items.Add("PET")      '"PEVAPsfc"
        lstParameters.Items.Add("Air Temp") '"TMP2m"
        lstParameters.Items.Add("Wind")     '"UGRD10m" '"VGRD10m"
        lstParameters.Items.Add("Sol Rad")  '"DSWRFsfc"
        lstParameters.Items.Add("Cloud")    '"DSWRFsfc"
        lstParameters.Items.Add("Dewp")     '"SPFH2m"
        If lstParameters.Enabled Then
            For i As Integer = 0 To lstParameters.Items.Count - 1
                lstParameters.SetSelected(i, True)
            Next
            'show from top
            Dim ltemp As Integer = lstParameters.TopIndex
            lstParameters.TopIndex = 0   'not working???
        End If
        numEnd.Value = DateTime.Now.Year
        numEnd.Maximum = DateTime.Now.Year

        Do
            Me.ShowDialog(System.Windows.Forms.Control.FromHandle(New IntPtr(aParentHandle)))

            If pOk Then
                Dim lXML As String = Me.XML
                If lXML.Length = 0 Then
                    If MapWinUtility.Logger.Msg("No data was selected for download." & vbCrLf & "Return to Download window?",
                                                         MsgBoxStyle.YesNo, "Desired Data Not Specified") = MsgBoxResult.No Then
                        Return CancelString
                    End If
                ElseIf lXML.Contains("<region>") OrElse lXML.Contains("<stationid>") Then

                    'check to see if nhdplus desired and available
                    Dim lProblem As Boolean = False
                    'If chkNHDplus_All.Checked Or chkNHDplus_Catchment.Checked Or chkNHDplus_elev_cm.Checked Or chkNHDplus_hydrography.Checked Then
                    '    For Each lHuc8 As String In HUC8s()
                    '        If Not CheckAddress("https://s3.amazonaws.com/nhdplus/NHDPlusV1/NHDPlusExtensions/SubBasins/NHDPlus" & lHuc8.Substring(0, 2) & "/" & "NHDPlus" & lHuc8 & ".zip") Then
                    '            lProblem = True
                    '        End If
                    '    Next
                    '    If lProblem Then
                    '        If MapWinUtility.Logger.Msg("NHDPlus v1.0 data is not available for download for the selected region." & vbCrLf & "Continue anyway?",
                    '                                     MsgBoxStyle.YesNo, "Desired Data Not Available") = MsgBoxResult.No Then
                    '            Return CancelString
                    '        End If
                    '    End If
                    'End If
                    If chkNHDplus2_All.Checked Or chkNHDplus2_Catchment.Checked Or chkNHDplus2_elev_cm.Checked Or chkNHDplus2_hydrography.Checked Then
                        For Each lHuc8 As String In HUC8s()
                            'If Not CheckAddress("https://s3.amazonaws.com/nhdplus/NHDPlusV2/NHDPlusExtensions/SubBasins/NHDPlus" & lHuc8.Substring(0, 2) & "/" & "NHDPlus" & lHuc8 & ".zip") Then
                            If Not CheckAddress("ftp://newftp.epa.gov/exposure/BasinsData/NHDPlus21/NHDPlus" & lHuc8 & ".zip") Then
                                lProblem = True
                            End If
                        Next
                        If lProblem Then
                            If MapWinUtility.Logger.Msg("NHDPlus v2.1 data is not available for download for the selected region." & vbCrLf & "Continue anyway?",
                                                         MsgBoxStyle.YesNo, "Desired Data Not Available") = MsgBoxResult.No Then
                                Return CancelString
                            End If
                        End If
                    End If

                    Return lXML
                Else
                    Dim lMessage As String = "Unable to determine region to download."
                    Select Case cboRegion.SelectedItem
                        Case pRegionViewRectangle
                            lMessage = "View rectangle is not a defined region. Load a georeferenced layer before downloading."
                        Case pRegionExtentSelectedLayer
                            lMessage = "Choose a different region type or close the Download window and select a georeferenced layer."
                        Case pRegionExtentSelectedShapes
                            lMessage = "Choose a different region type or close the Download window and select one or more shapes."
                        Case Else
                    End Select
                    If MapWinUtility.Logger.Msg(lMessage & vbCrLf & "Return to Download window?",
                                                         MsgBoxStyle.YesNo, "Region Not Specified") = MsgBoxResult.No Then
                        Return CancelString
                    End If
                End If
            Else
                Return CancelString
            End If
        Loop
    End Function
#Region "ManagePreCheckedBoxes"
    'Keep track of which check box(es) were checked automatically at startup (for example because a station was selected on the map)
    'Automatically un-check when another is manually checked since that indicates user wanted different data
    Private PreChecked As New List(Of System.Windows.Forms.CheckBox)
    Private PreUnChecked As New List(Of System.Windows.Forms.CheckBox)
    Private Sub TallyPreChecked(aGroups As List(Of System.Windows.Forms.GroupBox))
        Dim lAllChecked As New List(Of System.Windows.Forms.CheckBox)
        For Each lGroup As System.Windows.Forms.GroupBox In aGroups
            For Each lChild As System.Windows.Forms.Control In lGroup.Controls
                If lChild.GetType.Name = "CheckBox" Then
                    Dim lChk As System.Windows.Forms.CheckBox = lChild
                    If lChk.Checked Then
                        PreChecked.Add(lChk)
                    Else
                        PreUnChecked.Add(lChk)
                    End If
                End If
            Next
        Next
        If PreChecked.Count > 0 Then
            For Each lChk As System.Windows.Forms.CheckBox In PreUnChecked
                AddHandler lChk.CheckedChanged, AddressOf PreUncheckedChanged
            Next
        End If
    End Sub

    Private Sub PreUncheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            For Each lChk As System.Windows.Forms.CheckBox In PreChecked
                lChk.Checked = False
            Next
            For Each lChk As System.Windows.Forms.CheckBox In PreUnChecked
                RemoveHandler lChk.CheckedChanged, AddressOf PreUncheckedChanged
            Next
        Catch
        End Try
    End Sub
#End Region

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        pOk = False
        Me.Close()
    End Sub

    Private Sub btnDownload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDownload.Click
        Try
            If cboRegion.SelectedItem IsNot Nothing Then SaveSetting("DataDownload", "Defaults", "RegionTypeName", cboRegion.SelectedItem)
            Dim lMinCountInt As Integer
            If Integer.TryParse(txtMinCount_GW.Text, lMinCountInt) Then SaveSetting("DataDownload", "Defaults", "MinCount", lMinCountInt)
            SaveSetting("DataDownload", "Defaults", "Clip", chkClip.Checked.ToString)
            SaveSetting("DataDownload", "Defaults", "Merge", chkMerge.Checked.ToString)
        Catch ex As Exception
            MapWinUtility.Logger.Dbg(ex.Message, ex.StackTrace)
        End Try

        pOk = True
        Me.Close()
    End Sub

#If GISProvider = "DotSpatial" Then
    Public Function SelectedRegion() As atcD4EMLauncher.Region
        Dim lRegion As atcD4EMLauncher.Region = Nothing
        Dim lPreferredFormat As String = "box"
        Try
            Dim lProjection As DotSpatial.Projections.ProjectionInfo = Nothing
            Dim lProjectionString As String = ""
#If GISProvider = "DotSpatial" Then
            If pMapWin.Map.Layers.Count > 0 Then
                lProjectionString = pMapWin.Map.GetLayers()(0).ProjectionString
            End If
            Dim lMapExtent = pMapWin.Map.Extent
#Else
            If pMapWin.Layers.Count() > 0 Then
                lProjectionString = pMapWin.Layers()(0).ProjectionString
            End If
            Dim lMapExtent = pMapWin.Extent
#End If
            Dim lExtents As Extent = Nothing
            Select Case cboRegion.SelectedItem
                Case pRegionViewRectangle
                    lExtents = lMapExtent 'pMapWin.View.Extents
                Case pRegionExtentSelectedLayer
                    lExtents = lMapExtent 'pMapWin.Layers(pMapWin.Layers.CurrentLayer).Extents
                Case pRegionExtentSelectedShapes
                    lExtents = lMapExtent 'pMapWin.View.SelectedShapes.SelectBounds
                Case pRegionEnterCoordinates
                    lRegion = frmSpecifyRegion.AskUser(Me.Icon)
                    'Case pRegionStationIDs
                    'lRegion = frmSpecifyStations.AskUser(Me.Icon, StationsFromMap)
                Case Else
                    Dim lHuc8Layer As IMapFeatureLayer = HUC8Layer()
                    If lHuc8Layer IsNot Nothing Then
                        lExtents = lHuc8Layer.Extent
                        lPreferredFormat = "huc8"
                    End If
            End Select

            If lExtents IsNot Nothing Then
                lRegion = New atcD4EMLauncher.Region(lExtents.MaxY, lExtents.MinY, lExtents.MinX, lExtents.MaxX, lProjectionString)
            End If

        Catch ex As Exception
            lRegion = frmSpecifyRegion.AskUser(Me.Icon)
        End Try

        If lRegion Is Nothing Then
            Return Nothing
        Else
            lRegion.HUC8s = HUC8s()
            lRegion.PreferredFormat = lPreferredFormat
#If GISProvider = "DotSpatial" Then
            Return lRegion
#Else
            Return lRegion.GetProjected(atcD4EMLauncher.SpatialOperations.GeographicProjection)
#End If
        End If
        Return Nothing
    End Function
#Else
    Public Function SelectedRegion() As atcD4EMLauncher.Region
        Dim lRegion As atcD4EMLauncher.Region = Nothing
        Dim lPreferredFormat As String = "box"
        Try
            Dim lExtents As MapWinGIS.Extents = Nothing
            Select Case cboRegion.SelectedItem
                Case pRegionViewRectangle
                    lExtents = pMapWin.View.Extents
                Case pRegionExtentSelectedLayer
                    lExtents = pMapWin.Layers(pMapWin.Layers.CurrentLayer).Extents
                Case pRegionExtentSelectedShapes
                    lExtents = pMapWin.View.SelectedShapes.SelectBounds
                Case pRegionEnterCoordinates
                    lRegion = frmSpecifyRegion.AskUser(Me.Icon)
                    'Case pRegionStationIDs
                    'lRegion = frmSpecifyStations.AskUser(Me.Icon, StationsFromMap)
                Case Else
                    Dim lHuc8Layer As MapWindow.Interfaces.Layer = HUC8Layer()
                    If lHuc8Layer IsNot Nothing Then
                        lExtents = lHuc8Layer.Extents
                        lPreferredFormat = "huc8"
                    End If
            End Select

            If lExtents IsNot Nothing Then
                lRegion = New atcD4EMLauncher.Region(lExtents.yMax, lExtents.yMin, lExtents.xMin, lExtents.xMax, pMapWin.Project.ProjectProjection)
            End If

        Catch ex As Exception
            lRegion = frmSpecifyRegion.AskUser(Me.Icon)
        End Try

        If lRegion Is Nothing Then
            Return Nothing
        Else
            lRegion.HUC8s = HUC8s()
            lRegion.PreferredFormat = lPreferredFormat
            Return lRegion.GetProjected(atcD4EMLauncher.SpatialOperations.GeographicProjection)
        End If
        Return Nothing
    End Function

#End If
    'Dim pStationIDs() As String = {}

    Private Function StationsXML(ByVal aStations As Generic.List(Of String)) As String
        Dim lStationsXML As String = ""
        For Each lStation As String In aStations
            lStationsXML &= "<stationid>" & lStation & "</stationid>" & vbCrLf
        Next
        Return lStationsXML
    End Function

#If GISProvider = "DotSpatial" Then
    Private Function StationsFromMap() As Generic.List(Of String)
        Dim lStations As New Generic.List(Of String)
        Dim layers As List(Of IMapFeatureLayer) = GisUtilDS.GetFeatureLayers(FeatureType.Point)
        For Each lyr As IMapFeatureLayer In layers
            If lyr.IsSelected Then
                If lyr.Selection().Count > 0 Then
                    Dim lKeyFld As DataColumn = Nothing
                    Dim lKeyFldIndex As Integer = -1
                    Dim lColumnArray = lyr.DataSet.GetColumns()
                    For Each Fld As DataColumn In lColumnArray
                        Select Case Fld.ColumnName.ToLower()
                            Case "site_no", "locid", "location"
                                lKeyFld = Fld
                                lKeyFldIndex = Array.IndexOf(lColumnArray, lKeyFld)
                                Exit For 'TODO: use list of ID fields for layers from layers.dbf
                        End Select
                    Next
                    If lKeyFld IsNot Nothing Then
                        For Each lFeat As IFeature In lyr.Selection().ToFeatureList()
                            lStations.Add(lFeat.DataRow.Item(lKeyFldIndex).ToString())
                        Next
                    End If
                End If
            End If
        Next
        Return lStations
    End Function
#Else
    Private Function StationsFromMap() As Generic.List(Of String)
        Dim lStations As New Generic.List(Of String)
        If pMapWin.View IsNot Nothing Then
            Dim lSelected As MapWindow.Interfaces.SelectInfo = pMapWin.View.SelectedShapes
            If lSelected.NumSelected > 0 Then
                Dim lLayer As MapWindow.Interfaces.Layer = pMapWin.Layers.Item(pMapWin.Layers.CurrentLayer)
                Dim lLayerShapefile As MapWinGIS.Shapefile = lLayer.GetObject
                Dim lKeyField As Integer
                For lKeyField = 0 To lLayerShapefile.NumFields - 1
                    Select Case lLayerShapefile.Field(lKeyField).Name.ToLower
                        Case "site_no", "locid", "location" : Exit For 'TODO: use list of ID fields for layers from layers.dbf
                    End Select
                Next
                If lKeyField < lLayerShapefile.NumFields Then
                    For lShapeIndex As Integer = 0 To lSelected.NumSelected - 1
                        lStations.Add(lLayerShapefile.CellValue(lKeyField, lSelected.Item(lShapeIndex).ShapeIndex))
                    Next
                End If
            End If
        End If
        Return lStations
    End Function
#End If

    Private Sub SetCheckboxVisibilityFromMapOrRegion()
        chkBASINS_MetData.ForeColor = System.Drawing.SystemColors.GrayText
        'chkNCDC_MetData.ForeColor = System.Drawing.SystemColors.GrayText
        Dim lStationsRegion As Boolean = False
        If cboRegion.Text = pRegionStationIDs Then
            lStationsRegion = True
            panelNWISnoStations.Visible = False
            panelNWISnoStations_GW.Visible = False
            chkBASINS_MetData.ForeColor = System.Drawing.SystemColors.ControlText
            'chkNCDC_MetData.ForeColor = System.Drawing.SystemColors.ControlText
        End If

        If pApplicationName.StartsWith("USGS GW Toolbox") OrElse pApplicationName.Contains("Hydro Toolbox") Then
            chkNWIS_GetNWISDailyDischarge_GW.Visible = True
            chkNWIS_GetNWISIdaDischarge_GW.Visible = False
            chkNWIS_GetNWISDailyGW_GW.Visible = True
            chkNWIS_GetNWISPeriodicGW_GW.Visible = True
            chkNWIS_GetNWISPrecipitation_GW.Visible = True

            chkNWIS_GetNWISDailyDischarge_GW.Enabled = lStationsRegion
            chkNWIS_GetNWISIdaDischarge_GW.Enabled = lStationsRegion
            chkNWIS_GetNWISDailyGW_GW.Enabled = lStationsRegion
            chkNWIS_GetNWISPeriodicGW_GW.Enabled = lStationsRegion
            chkNWIS_GetNWISPrecipitation_GW.Enabled = lStationsRegion
        Else
            chkNWIS_GetNWISDailyDischarge.Visible = True
            chkNWIS_GetNWISIdaDischarge.Visible = True
            chkNWIS_GetNWISDailyGW.Visible = True
            chkNWIS_GetNWISPeriodicGW.Visible = True
            chkNWIS_GetNWISWQ.Visible = True
            chkNWIS_GetNWISMeasurements.Visible = True

            chkNWIS_GetNWISDailyDischarge.Enabled = lStationsRegion
            chkNWIS_GetNWISIdaDischarge.Enabled = lStationsRegion
            chkNWIS_GetNWISDailyGW.Enabled = lStationsRegion
            chkNWIS_GetNWISPeriodicGW.Enabled = lStationsRegion
            chkNWIS_GetNWISWQ.Enabled = lStationsRegion
            chkNWIS_GetNWISMeasurements.Enabled = lStationsRegion
        End If

        Dim lHasMapWin As Boolean
#If GISProvider = "DotSpatial" Then
        If pMapWin Is Nothing Then
            lHasMapWin = False
        Else
            lHasMapWin = True
        End If
#Else
        If pMapWin.View Is Nothing Then
            lHasMapWin = False
        Else
            lHasMapWin = True
        End If
#End If
        If lHasMapWin Then
#If GISProvider = "DotSpatial" Then
            Dim lLayer As ILayer = CurrentLayer()
            Dim lFilename As String = ""
            Dim lSelectedCount As Integer = 0
            If lLayer IsNot Nothing Then
                lFilename = IO.Path.GetFileNameWithoutExtension(lLayer.DataSet.Filename).ToLower
                lSelectedCount = CType(lLayer, IMapFeatureLayer).Selection().Count
            Else
                lFilename = ""
            End If
#Else
            Dim lLayer As MapWindow.Interfaces.Layer = pMapWin.Layers.Item(pMapWin.Layers.CurrentLayer)
            Dim lFilename As String = IO.Path.GetFileNameWithoutExtension(lLayer.FileName).ToLower
            Dim lSelected As MapWindow.Interfaces.SelectInfo = pMapWin.View.SelectedShapes
            Dim lSelectedCount As Integer = lSelected.NumSelected
#End If
            If lSelectedCount > 0 Then
                If lFilename.StartsWith("met") Then
                    chkBASINS_MetData.ForeColor = System.Drawing.SystemColors.ControlText
                    chkBASINS_MetData.Checked = True
                    'chkNCDC_MetData.ForeColor = System.Drawing.SystemColors.ControlText
                ElseIf pApplicationName.StartsWith("USGS GW Toolbox") OrElse pApplicationName.Contains("Hydro Toolbox") Then
                    Select Case lFilename
                        Case "nwis_stations_discharge"
                            chkNWIS_GetNWISDailyDischarge_GW.Enabled = True
                            chkNWIS_GetNWISDailyDischarge_GW.Checked = True
                            chkNWIS_GetNWISIdaDischarge_GW.Enabled = True
                        Case "nwis_stations_gw_daily"
                            chkNWIS_GetNWISDailyGW_GW.Enabled = True
                            chkNWIS_GetNWISDailyGW_GW.Checked = True
                        Case "nwis_stations_gw_periodic"
                            chkNWIS_GetNWISPeriodicGW_GW.Enabled = True
                            chkNWIS_GetNWISPeriodicGW_GW.Checked = True
                        Case "nwis_stations_precipitation"
                            chkNWIS_GetNWISPrecipitation_GW.Enabled = True
                            chkNWIS_GetNWISPrecipitation_GW.Checked = True
                    End Select
                Else
                    Select Case lFilename
                        Case "nwis_stations_discharge"
                            chkNWIS_GetNWISDailyDischarge.Enabled = True
                            chkNWIS_GetNWISDailyDischarge.Checked = True
                            chkNWIS_GetNWISIdaDischarge.Enabled = True
                        Case "nwis_stations_qw"
                            chkNWIS_GetNWISWQ.Enabled = True
                            chkNWIS_GetNWISWQ.Checked = True
                        Case "nwis_stations_measurements"
                            chkNWIS_GetNWISMeasurements.Enabled = True
                            chkNWIS_GetNWISMeasurements.Checked = True
                        Case "nwis_stations_gw_daily"
                            chkNWIS_GetNWISDailyGW.Enabled = True
                            chkNWIS_GetNWISDailyGW.Checked = True
                        Case "nwis_stations_gw_periodic"
                            chkNWIS_GetNWISPeriodicGW.Enabled = True
                            chkNWIS_GetNWISPeriodicGW.Checked = True
                        Case "nwis_stations_peak" 'download of these data not yet implemented

                        Case "storet"
                            chkSTORET_Results.Enabled = True
                            chkSTORET_Results.Text = "Results"
                            chkSTORET_Results.Checked = True
                        Case "nldas_grid", "nldas_grid_center"
                            chkNLDAS_GetNLDASParameter.Enabled = True
                            txtTimeZone.Enabled = True
                            lblTimeZone.Enabled = True
                            lstParameters.Enabled = True
                            numStart.Enabled = True
                            numEnd.Enabled = True
                            lblConstituents.Enabled = True
                            lblStart.Enabled = True
                            lblEnd.Enabled = True
                            chkNLDAS_GetNLDASParameter.Text = "Hourly Data"
                            chkNLDAS_GetNLDASParameter.Checked = True
                    End Select
                End If
            End If
        End If
        If pApplicationName.StartsWith("USGS GW Toolbox") OrElse pApplicationName.Contains("Hydro Toolbox") Then
            If Not chkNWIS_GetNWISDailyDischarge_GW.Enabled AndAlso
               Not chkNWIS_GetNWISIdaDischarge_GW.Enabled AndAlso
               Not chkNWIS_GetNWISDailyGW_GW.Enabled AndAlso
               Not chkNWIS_GetNWISPeriodicGW_GW.Enabled AndAlso
               Not chkNWIS_GetNWISPrecipitation_GW.Enabled Then
                panelNWISnoStations_GW.Visible = True
                panelNWISnoStations_GW.BringToFront()

                chkNWIS_GetNWISDailyDischarge_GW.Visible = False
                chkNWIS_GetNWISIdaDischarge_GW.Visible = False
                chkNWIS_GetNWISDailyGW_GW.Visible = False
                chkNWIS_GetNWISPeriodicGW_GW.Visible = False
                chkNWIS_GetNWISPrecipitation_GW.Visible = False
            End If
        Else
            If Not chkNWIS_GetNWISDailyDischarge.Enabled AndAlso
               Not chkNWIS_GetNWISIdaDischarge.Enabled AndAlso
               Not chkNWIS_GetNWISDailyGW.Enabled AndAlso
               Not chkNWIS_GetNWISPeriodicGW.Enabled AndAlso
               Not chkNWIS_GetNWISWQ.Enabled AndAlso
               Not chkNWIS_GetNWISMeasurements.Enabled Then
                panelNWISnoStations.Visible = True
                panelNWISnoStations.BringToFront()

                chkNWIS_GetNWISDailyDischarge.Visible = False
                chkNWIS_GetNWISIdaDischarge.Visible = False
                chkNWIS_GetNWISMeasurements.Visible = False
                chkNWIS_GetNWISDailyGW.Visible = False
                chkNWIS_GetNWISPeriodicGW.Visible = False
                chkNWIS_GetNWISWQ.Visible = False
            End If
        End If
    End Sub

    Private Sub SetColorsFromAvailability()
        'Dim lStatusFilename As String = IO.Path.Combine(IO.Path.Combine(My.Computer.FileSystem.SpecialDirectories.MyDocuments, "BASINS"), "status.html")
        'If IO.File.Exists(lStatusFilename) Then
        '    Dim lColor As Drawing.Color
        '    Dim lGoodColor As Drawing.Color = Drawing.Color.FromArgb(255, 140, 216, 140)
        '    Dim lBadColor As Drawing.Color = Drawing.Color.FromArgb(255, 206, 150, 150)
        '    For Each lStatusLine As String In IO.File.ReadAllLines(lStatusFilename)
        '        If lStatusLine.StartsWith("<tr><td>") Then
        '            Dim lDataType As String = lStatusLine.Substring(8)
        '            Dim lEndField As Integer = lDataType.IndexOf("<")
        '            lDataType = lDataType.Substring(0, lEndField)
        '            If lStatusLine.Contains("a href") Then
        '                lColor = lBadColor
        '            Else
        '                lColor = lGoodColor
        '            End If
        '            Select Case lDataType
        '                Case "BASINS.303d" : chkBASINS_303d.BackColor = lColor
        '                Case "BASINS.Census" : chkBASINS_Census.BackColor = lColor
        '                Case "BASINS.DEM" : chkBASINS_DEM.BackColor = lColor
        '                Case "BASINS.DEMG" : chkBASINS_DEMG.BackColor = lColor
        '                Case "BASINS.GIRAS" : chkBASINS_GIRAS.BackColor = lColor
        '                Case "BASINS.LSTORET" : chkBASINS_LSTORET.BackColor = lColor
        '                Case "BASINS.MetStations" : chkBASINS_MetStations.BackColor = lColor
        '                Case "BASINS.MetData" : chkBASINS_MetData.BackColor = lColor
        '                Case "BASINS.NED" : chkBASINS_NED.BackColor = lColor
        '                Case "BASINS.NHD" : chkBASINS_NHD.BackColor = lColor
        '                Case "NHDPlus.All" : grpNHDplus.BackColor = lColor
        '                Case "NWIS.DischargeStations" : chkNWISStations_discharge.BackColor = lColor
        '                Case "NWIS.WaterQualityStations" : chkNWISStations_qw.BackColor = lColor
        '                Case "NWIS.MeasurementStations" : chkNWISStations_measurement.BackColor = lColor
        '                Case "NWIS.GroundwaterStations" : chkNWISStations_gw_daily.BackColor = lColor
        '                Case "NWIS.DischargeData" : chkNWIS_GetNWISDailyDischarge.BackColor = lColor
        '                Case "NWIS.IdaDischarge" : chkNWIS_GetNWISIdaDischarge.BackColor = lColor
        '                Case "NWIS.WaterQualityData" : chkNWIS_GetNWISWQ.BackColor = lColor
        '                Case "NWIS.MeasurementData" : chkNWIS_GetNWISMeasurements.BackColor = lColor
        '                Case "Seamless.NLCD1992LandCover" : chkNLCD2001_1992.BackColor = lColor
        '                Case "Seamless.NLCD2001Canopy" : chkNLCD2001_Canopy.BackColor = lColor
        '                Case "Seamless.NLCD2001Impervious" : chkNLCD2001_Impervious.BackColor = lColor
        '                Case "Seamless.NLCD2001LandCover" : chkNLCD2001_LandCover.BackColor = lColor
        '                Case "STORET.Stations" : chkSTORET_Stations.BackColor = lColor
        '                Case "STORET.Data" : chkSTORET_Results.BackColor = lColor
        '                Case Else
        '                    Debug.WriteLine("Case """ & lDataType & """ : chk" & lDataType.Replace(".", "_") & ".BackColor = lColor")
        '            End Select
        '        End If
        '    Next
        'End If
    End Sub

    Public Property XML() As String
        Get
            Dim lXML As String = ""
            Dim lDesiredProjection As String = ""
            Dim lRegionXML As String = ""
            Dim lStationsXML As String
            Dim lRegion As atcD4EMLauncher.Region

            If cboRegion.SelectedItem = pRegionStationIDs Then
                lRegion = Nothing
                Dim lStations As Generic.List(Of String) = frmSpecifyStations.AskUser(Me.Icon, StationsFromMap)
                If lStations Is Nothing OrElse lStations.Count = 0 Then
                    Return ""
                End If
                lStationsXML = StationsXML(lStations)
            Else
                lRegion = Me.SelectedRegion
                lStationsXML = StationsXML(StationsFromMap)
            End If

            If lRegion IsNot Nothing Then lRegionXML = lRegion.XML

            Dim lCacheFolder As String = "<CacheFolder>" & BASINS.g_CacheDir & "</CacheFolder>" & vbCrLf

            Dim lCacheBehavior As String = ""
            If chkCacheOnly.Checked Then
                lCacheBehavior = "<CacheOnly>" & chkCacheOnly.Checked & "</CacheOnly>" & vbCrLf
            End If
            If chkGetNewest.Checked Then
                lCacheBehavior = "<GetEvenIfCached>" & chkGetNewest.Checked & "</GetEvenIfCached>" & vbCrLf
            End If

            Dim lCount As String = ""
            Dim lMinCountInt As Integer
            If Integer.TryParse(txtMinCount_GW.Text, lMinCountInt) Then lCount = "<MinCount>" & txtMinCount_GW.Text.Trim & "</MinCount>" & vbCrLf

            Dim lSaveFolderOnly As String = ""
            Dim lSaveFolder As String = ""
#If GISProvider = "DotSpatial" Then
            If pMapWin IsNot Nothing Then
                If pMapWin.Map.Layers.Count > 0 Then
                    Dim lprojinfo As DotSpatial.Projections.ProjectionInfo = pMapWin.Map.Projection
                    If lprojinfo.ToProj4String.Length() > 0 Then
                        lDesiredProjection = "<DesiredProjection>" & lprojinfo.ToProj4String & "</DesiredProjection>" & vbCrLf
                    End If
                Else
                    lDesiredProjection = "<DesiredProjection>+proj=aea +lat_1=29.5 +lat_2=45.5 +lat_0=23 +lon_0=-96 +x_0=0 +y_0=0 +ellps=GRS80 +towgs84=0,0,0,0,0,0,0 +units=m +no_defs </DesiredProjection>" & vbCrLf
                End If

                If Not DownloadDataPlugin.DSProject.CurrentProjectFile Is Nothing AndAlso DownloadDataPlugin.DSProject.CurrentProjectFile.Length > 0 Then
                    lSaveFolderOnly = IO.Path.GetDirectoryName(DownloadDataPlugin.DSProject.CurrentProjectFile)
                    lSaveFolder &= "<SaveIn>" & lSaveFolderOnly & "</SaveIn>" & vbCrLf
                Else
                    lSaveFolder &= "<SaveIn>" & IO.Path.Combine(BASINS.g_CacheDir, "DataDownload") & "</SaveIn>" & vbCrLf
                End If
            End If
#Else
            If Not pMapWin.Project Is Nothing Then
                If Not pMapWin.Project.ProjectProjection Is Nothing AndAlso pMapWin.Project.ProjectProjection.Length > 0 Then
                    lDesiredProjection = "<DesiredProjection>" & pMapWin.Project.ProjectProjection & "</DesiredProjection>" & vbCrLf
                End If
                If Not pMapWin.Project.FileName Is Nothing AndAlso pMapWin.Project.FileName.Length > 0 Then
                    lSaveFolderOnly = IO.Path.GetDirectoryName(pMapWin.Project.FileName)
                    lSaveFolder &= "<SaveIn>" & lSaveFolderOnly & "</SaveIn>" & vbCrLf
                End If
            End If
#End If


            Dim lXMLcommon As String = lSaveFolder _
                                     & lCacheFolder _
                                     & lDesiredProjection _
                                     & lRegionXML _
                                     & lStationsXML _
                                     & lCacheBehavior _
                                     & lCount _
                                     & "<clip>" & chkClip.Checked & "</clip>" & vbCrLf _
                                     & "<merge>" & chkMerge.Checked & "</merge>" & vbCrLf _
                                     & "<joinattributes>true</joinattributes>" & vbCrLf _
                                     & "</arguments>" & vbCrLf _
                                     & "</function>" & vbCrLf

            For Each lControl As System.Windows.Forms.Control In Me.Controls
                Dim lControlName As String = lControl.Name
                If lControlName.EndsWith("_GW") Then 'Treat Groundwater version of controls the same as the BASINS version
                    lControlName = lControlName.Substring(0, lControlName.Length - 3)
                End If
                If lControlName.StartsWith("grp") AndAlso lControl.HasChildren Then
                    Dim lCheckedChildren As String = ""
                    For Each lChild As System.Windows.Forms.Control In lControl.Controls
                        If lChild.GetType.Name = "CheckBox" AndAlso CType(lChild, System.Windows.Forms.CheckBox).Checked Then
                            Dim lChildName As String = lChild.Name.Substring(lControlName.Length + 1)
                            If lChildName.EndsWith("_GW") Then
                                lChildName = lChildName.Substring(0, lChildName.Length - 3)
                            End If
                            If lChildName.ToLower.StartsWith("get") Then 'this checkbox has its own function name
                                Dim lWDMxml As String = ""
                                If pApplicationName.StartsWith("USGS") OrElse pApplicationName.Contains("Hydro") Then
                                    'Don't offer to save in WDM for USGS versions, always add as individual files
                                Else
                                    If lChild Is chkNWIS_GetNWISDailyDischarge Then
                                        Dim lWDMfrm As New frmWDM
                                        lWDMxml = lWDMfrm.AskUser(Me.Icon, "Flow", IO.Path.Combine(lSaveFolderOnly, "nwis"), lChild.Text & " Processing Options")
                                    ElseIf lChild Is chkNWIS_GetNWISIdaDischarge Then
                                        'Dim lWDMfrm As New frmWDM    'don't add to wdm, just add rdb file to project
                                        'lWDMxml = lWDMfrm.AskUser(Me.Icon, "Flow", IO.Path.Combine(lSaveFolderOnly, "nwis"), _
                                        '                          lChild.Text & " Processing Options")
                                    ElseIf lChild Is chkNWIS_GetNWISDailyGW Then
                                        Dim lWDMfrm As New frmWDM
                                        lWDMxml = lWDMfrm.AskUser(Me.Icon, "Groundwater", IO.Path.Combine(lSaveFolderOnly, "nwis"), lChild.Text & " Processing Options")
                                    ElseIf lChild Is chkNLDAS_GetNLDASParameter Then
                                        Dim lWDMfrm As New frmWDM
                                        lWDMxml = lWDMfrm.AskUser(Me.Icon, "NLDAS", IO.Path.Combine(lSaveFolderOnly, "nldas"), "NLDAS Processing Options")
                                        lWDMxml &= "<TimeZoneShift>" & txtTimeZone.Text & "</TimeZoneShift>" & vbCrLf
                                        If lstParameters.SelectedItems.Count <> 7 And lstParameters.SelectedItems.Count <> 0 Then
                                            'user doens't want all the parameters
                                            If lstParameters.SelectedItems.Contains("Precip") Then
                                                lWDMxml &= "<datatype>APCPsfc</datatype>" & vbCrLf
                                            End If
                                            If lstParameters.SelectedItems.Contains("PET") Then
                                                lWDMxml &= "<datatype>PEVAPsfc</datatype>" & vbCrLf
                                            End If
                                            If lstParameters.SelectedItems.Contains("Air Temp") Then
                                                lWDMxml &= "<datatype>TMP2m</datatype>" & vbCrLf
                                            End If
                                            If lstParameters.SelectedItems.Contains("Wind") Then
                                                lWDMxml &= "<datatype>UGRD10m</datatype>" & vbCrLf
                                                lWDMxml &= "<datatype>VGRD10m</datatype>" & vbCrLf
                                            End If
                                            If lstParameters.SelectedItems.Contains("Sol Rad") Then
                                                lWDMxml &= "<datatype>DSWRFsfc</datatype>" & vbCrLf
                                            End If
                                            If lstParameters.SelectedItems.Contains("Cloud") Then
                                                lWDMxml &= "<datatype>DSWRFsfc</datatype>" & vbCrLf
                                            End If
                                            If lstParameters.SelectedItems.Contains("Dewp") Then
                                                lWDMxml &= "<datatype>SPFH2m</datatype>" & vbCrLf
                                            End If
                                        End If
                                        If numStart.Value > 1979 Or numEnd.Value < DateTime.Now.Year Then
                                            'user doesn't want the whole available span, add one hour for start of day 
                                            lWDMxml &= "<startdate>1/1/" & numStart.Value & " 1:00:00</startdate>" & vbCrLf
                                            lWDMxml &= "<enddate>1/1/" & numEnd.Value + 1 & "</enddate>" & vbCrLf
                                        End If
                                    End If
                                    'If lChild Is chkNWIS_GetNWISPrecipitation Then
                                    '    Dim lWDMfrm As New frmWDM
                                    '    lWDMxml = lWDMfrm.AskUser(Me.Icon, "Precipitation", IO.Path.Combine(lSaveFolderOnly, "nwis"), _
                                    '                              lChild.Text & " Processing Options")
                                    'End If
                                End If
                                If lWDMxml IsNot Nothing Then
                                    lXML &= "<function name='" & lChildName & "'>" & vbCrLf _
                                         & "<arguments>" & vbCrLf _
                                         & lWDMxml _
                                         & lXMLcommon & vbCrLf
                                End If
                            Else 'This checkbox adds a data type to the parent function
                                lCheckedChildren &= "<DataType>" & lChildName & "</DataType>" & vbCrLf
                            End If
                        End If
                    Next

                    If lCheckedChildren.Length > 0 Then

                        Dim lWDMxml As String = ""
                        If lCheckedChildren.Contains("<DataType>MetData</DataType>") Then
                            Dim lWDMfrm As New frmWDM
                            lWDMxml = lWDMfrm.AskUser(Me.Icon, "Met", IO.Path.Combine(lSaveFolderOnly, "met"),
                                                      "Met Data Processing Options")
                        End If
                        If lWDMxml IsNot Nothing Then
                            lXML &= "<function name='Get" & lControlName.Substring(3) & "'>" & vbCrLf _
                                 & "<arguments>" & vbCrLf _
                                 & lCheckedChildren _
                                 & lWDMxml _
                                 & lXMLcommon & vbCrLf
                        End If
                    End If
                End If
            Next
            Return lXML
        End Get
        Set(ByVal value As String)

        End Set
    End Property

#If GISProvider = "DotSpatial" Then
    Private Function CurrentLayer() As ILayer
        Try
            If pMapWin.Map.Layers.Count > 0 Then
                Return pMapWin.Map.Layers.SelectedLayer
            Else
                Return Nothing
            End If
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Private Function HUC8Layer() As ILayer
        Dim layers As List(Of IMapFeatureLayer) = GisUtilDS.GetFeatureLayers(FeatureType.Polygon)
        If layers.Count > 0 Then
            For Each lyr As IMapFeatureLayer In layers
                If lyr.DataSet.Filename.ToLower.EndsWith(g_PathChar & "cat.shp") Then
                    Return lyr
                End If
            Next
        Else
            Return Nothing
        End If
        Return Nothing
    End Function

    Private Function HUC8s() As Generic.List(Of String)
        'First check for a cat layer that contains the list of HUC-8s
        Dim lcatLayer As IMapFeatureLayer = HUC8Layer()
        If lcatLayer Is Nothing Then Return Nothing
        Dim lHUC8s As New Generic.List(Of String)
        If pMapWin IsNot Nothing AndAlso lcatLayer IsNot Nothing Then
            Dim lCatDbfName As String = IO.Path.Combine(IO.Path.GetDirectoryName(lcatLayer.DataSet.Filename), "cat.dbf")
            If IO.File.Exists(lCatDbfName) Then
                Dim lCatDbf As New atcUtility.atcTableDBF
                lCatDbf.OpenFile(lCatDbfName)
                Dim lHucField As Integer = lCatDbf.FieldNumber("CU")
                If lHucField > 0 Then
                    For lRecord As Integer = 1 To lCatDbf.NumRecords
                        lCatDbf.CurrentRecord = lRecord
                        lHUC8s.Add(lCatDbf.Value(lHucField))
                    Next
                End If
            End If
        End If
        Return lHUC8s
    End Function

#Else
    ''' <summary>
    ''' Returns index in pMapWin.Layers of the HUC8 layer cat.shp, or -1 if not found
    ''' </summary>
    Private Function HUC8Layer() As MapWindow.Interfaces.Layer
        If Not pMapWin Is Nothing AndAlso Not pMapWin.Layers Is Nothing Then
            Dim lIndex As Integer = 0
            For Each lLayer As Object In pMapWin.Layers
                If lLayer IsNot Nothing AndAlso lLayer.FileName.ToLower.EndsWith(g_PathChar & "cat.shp") Then
                    Return lLayer
                End If
            Next
        End If
        Return Nothing
    End Function

    Private Function HUC8s() As Generic.List(Of String)
        'First check for a cat layer that contains the list of HUC-8s
        Dim lHUC8s As New Generic.List(Of String)
        If pMapWin IsNot Nothing AndAlso pMapWin.Project IsNot Nothing AndAlso pMapWin.Project.FileName IsNot Nothing Then
            Dim lCatDbfName As String = IO.Path.Combine(IO.Path.GetDirectoryName(pMapWin.Project.FileName), "cat.dbf")
            If IO.File.Exists(lCatDbfName) Then
                Dim lCatDbf As New atcUtility.atcTableDBF
                lCatDbf.OpenFile(lCatDbfName)
                Dim lHucField As Integer = lCatDbf.FieldNumber("CU")
                If lHucField > 0 Then
                    For lRecord As Integer = 1 To lCatDbf.NumRecords
                        lCatDbf.CurrentRecord = lRecord
                        lHUC8s.Add(lCatDbf.Value(lHucField))
                    Next
                End If
            End If
        End If
        Return lHUC8s
    End Function
#End If

    '''' <summary>
    '''' Synchronous HTTP download
    '''' </summary>
    'Private Function URLtoString(ByVal Url As String, Optional ByVal aCookieContainer As CookieContainer = Nothing) As String
    '    Dim BytesProcessed As Integer = 0
    '    Debug.Assert(Url.StartsWith("http://"))
    '    Dim DownloadStartTime As Date = DateTime.Now

    '    ' create content stream from memory or file
    '    Dim ContentStream As New Text.StringBuilder

    '    ' Create the request object.
    '    Dim request As HttpWebRequest = HttpWebRequest.Create(Url)
    '    request.CookieContainer = aCookieContainer        
    '    Dim response As HttpWebResponse = request.GetResponse()
    '    ' only if server responds 200 OK
    '    If (response.StatusCode = HttpStatusCode.OK) Then
    '        Dim readBuffer As Byte()
    '        ReDim readBuffer(2048)
    '        Dim responseStream As IO.Stream = response.GetResponseStream()
    '        Dim iByte As Integer
    '        While (True)
    '            Dim bytesRead As Integer = responseStream.Read(readBuffer, 0, readBuffer.Length)
    '            If (bytesRead <= 0) Then Exit While
    '            iByte = 0
    '            While iByte < bytesRead
    '                ContentStream.Append(Chr(readBuffer(iByte)))
    '                iByte += 1
    '            End While
    '            BytesProcessed += bytesRead
    '        End While
    '    End If
    '    Return ContentStream.ToString
    'End Function

    'Private Function DownloadGeocache() As String
    '    Dim cookieContainer As New System.Net.CookieContainer()
    '    ' create the web request
    '    Dim req As HttpWebRequest = HttpWebRequest.Create("http://www.geocaching.com/login/default.aspx")
    '    req.CookieContainer = cookieContainer
    '    req.Method = "POST"
    '    ' create post data
    '    Dim bytePostData As Byte() = System.Text.Encoding.GetEncoding(1252).GetBytes("myUsername=&myPassword=&cookie=on&Button1=Login")
    '    ' create stream object
    '    Dim streamPostData As System.IO.Stream = req.GetRequestStream()
    '    ' write to the stream
    '    streamPostData.Write(bytePostData, 0, bytePostData.Length)
    '    ' close the stream
    '    streamPostData.Close()
    '    ' wait for the response
    '    Dim resp As HttpWebResponse = req.GetResponse()
    '    ' initialize header collection
    '    Dim coll As WebHeaderCollection = resp.Headers
    '    ' get site minder cookie data
    '    Dim sessionId As String = coll.Get("Set-Cookie")
    '    ' get current location
    '    Dim location As String = coll.Get("Location")
    '    ' close response stream
    '    resp.GetResponseStream().Close()
    '    ' create new cookie container object
    '    ' add cookie headers to cookie container
    '    'cookieContainer.SetCookies(New System.Uri("http://www.geocaching.com/"), sessionId)

    '    ' Print the properties of each cookie.
    '    Dim cook As Cookie
    '    For Each cook In resp.Cookies
    '        Console.WriteLine("Cookie:")
    '        Console.WriteLine("{0} = {1}", cook.Name, cook.Value)
    '        Console.WriteLine("Domain: {0}", cook.Domain)
    '        Console.WriteLine("Path: {0}", cook.Path)
    '        Console.WriteLine("Port: {0}", cook.Port)
    '        Console.WriteLine("Secure: {0}", cook.Secure)

    '        Console.WriteLine("When issued: {0}", cook.TimeStamp)
    '        Console.WriteLine("Expires: {0} (expired? {1})", cook.Expires, cook.Expired)
    '        Console.WriteLine("Don't save: {0}", cook.Discard)
    '        Console.WriteLine("Comment: {0}", cook.Comment)
    '        Console.WriteLine("Uri for comments: {0}", cook.CommentUri)
    '        Console.WriteLine("Version: RFC {0}", IIf(cook.Version = 1, "2109", "2965"))

    '        ' Show the string representation of the cookie.
    '        Console.WriteLine("String: {0}", cook.ToString())
    '    Next cook

    '    Return URLtoString("http://www.geocaching.com/seek/nearest.aspx?lat=33.775&lon=-84.295", cookieContainer)

    'End Function

    Private Sub frmDownload_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyValue = System.Windows.Forms.Keys.F1 Then
            ShowHelp()
        End If
    End Sub

    Private Sub btnHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHelp.Click
        ShowHelp()
    End Sub

    Private Sub ShowHelp()
        If pApplicationName = "Hydro Toolbox" Then
            atcUtility.ShowHelp("Getting Started (File, Project, and Data Menus)/GIS and Time-Series Data/Download and Manage Data.html")
        Else
            atcUtility.ShowHelp("BASINS Details\Project Creation and Management\GIS and Time-Series Data\Download.html")
        End If
    End Sub

    Private Function RegionValid(ByVal aRegionType As String, ByRef aReason As String) As Boolean
#If GISProvider = "DotSpatial" Then
        Return True
#Else
                Try
            Select Case cboRegion.SelectedItem
                Case pRegionEnterCoordinates, pRegionStationIDs
                    Return True 'Always an option
            End Select

            If pMapWin.Project.ProjectProjection.Length = 0 Then
                aReason = "Need a projection set in File/Settings/Project Projection" & vbCrLf _
                        & "and 'Use Projection Info' = True"
                Return False
            End If

            Dim lExtents As MapWinGIS.Extents = Nothing

            Select Case aRegionType
                Case pRegionViewRectangle
                    If pMapWin Is Nothing OrElse pMapWin.View Is Nothing OrElse pMapWin.View.Extents Is Nothing Then
                        aReason = "Current view does not have extents defined"
                        Return False
                    End If
                    lExtents = pMapWin.View.Extents
                Case pRegionExtentSelectedLayer
                    If pMapWin Is Nothing OrElse pMapWin.Layers Is Nothing OrElse pMapWin.Layers.CurrentLayer < 0 Then
                        aReason = "No layer is selected"
                        Return False
                    Else
                        lExtents = pMapWin.Layers(pMapWin.Layers.CurrentLayer).Extents
                        aReason = "Current layer does not have extents"
                    End If
                Case pRegionExtentSelectedShapes
                    If pMapWin Is Nothing OrElse pMapWin.View Is Nothing OrElse pMapWin.View.SelectedShapes Is Nothing OrElse pMapWin.View.SelectedShapes.NumSelected = 0 Then
                        aReason = "No shapes are selected"
                        Return False
                    Else
                        lExtents = pMapWin.View.SelectedShapes.SelectBounds
                        aReason = "Selected shapes do not have extents"
                    End If
                Case Else
                    If aRegionType.StartsWith(pRegionHydrologicUnit) Then
                        If HUC8Layer() Is Nothing Then
                            aReason = "Could not find hydrologic unit layer 'cat.shp' in project"
                            Return False
                        Else
                            Return True
                        End If
                    End If
            End Select

            If lExtents Is Nothing OrElse (lExtents.yMax = 0 AndAlso lExtents.yMin = 0) OrElse (lExtents.xMax = 0 AndAlso lExtents.xMin = 0) Then
                aReason = "Region is not valid"
                Return False
            End If

            Return True
        Catch e As Exception
            aReason = e.Message
            Return False
        End Try
#End If
    End Function

    Private Sub cboRegion_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboRegion.SelectedIndexChanged
        Dim lReason As String = ""
        If RegionValid(cboRegion.SelectedItem, lReason) Then
            SetCheckboxVisibilityFromMapOrRegion()
        Else
            MapWinUtility.Logger.Msg("Cannot use " & cboRegion.SelectedItem & ":" & vbCrLf & lReason, MsgBoxStyle.OkOnly, "Region Type Not Valid")
            cboRegion.SelectedItem = pRegionEnterCoordinates
        End If
    End Sub

    Private Sub chkBASINS_MetData_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkBASINS_MetData.CheckedChanged
        If chkBASINS_MetData.ForeColor = System.Drawing.SystemColors.GrayText Then
            If chkBASINS_MetData.Checked Then
                MapWinUtility.Logger.Msg("1. Download BASINS Met Stations" & vbCrLf _
                                       & "2. In the map legend, choose the Weather Station Sites layer" & vbCrLf _
                                       & "3. On the map, select the stations to get data from" & vbCrLf _
                                       & "4. Download BASINS Met Data", "One or more Met Stations must be selected")
                chkBASINS_MetData.Checked = False
            End If
        End If
    End Sub

    'Private Sub chkNCDC_MetData_CheckedChanged(sender As Object, e As EventArgs) Handles chkNCDC_MetData.CheckedChanged
    '    If chkNCDC_MetData.ForeColor = System.Drawing.SystemColors.GrayText Then
    '        If chkNCDC_MetData.Checked Then
    '            MapWinUtility.Logger.Msg("1. Download BASINS Met Stations" & vbCrLf _
    '                                   & "2. In the map legend, choose the Weather Station Sites layer" & vbCrLf _
    '                                   & "3. On the map, select the stations to get data from" & vbCrLf _
    '                                   & "4. Download NCDC Met Data", "One or more Met Stations must be selected")
    '            chkNCDC_MetData.Checked = False
    '        End If
    '    End If
    'End Sub

    Public Function CheckAddress(ByVal URL As String) As Boolean
        Try
            Logger.Dbg("CheckAddress " & URL)
            Dim request As WebRequest = WebRequest.Create(URL)
            Dim response As WebResponse = request.GetResponse()
        Catch ex As Exception
            Logger.Dbg("CheckAddress Failed " & ex.ToString)
            Return False
        End Try
        Return True
    End Function

    Private Sub numEnd_ValueChanged(sender As Object, e As EventArgs) Handles numEnd.ValueChanged
        numStart.Maximum = numEnd.Value
    End Sub

    Private Sub numStart_ValueChanged(sender As Object, e As EventArgs) Handles numStart.ValueChanged
        numEnd.Minimum = numStart.Value
    End Sub
End Class