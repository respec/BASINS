Imports atcUtility
Imports MapWinUtility
Imports System.Runtime.InteropServices

Public Class frmDownload

    Public Const CancelString As String = "<Cancel>"
    Private pMapWin As MapWindow.Interfaces.IMapWin
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
    <System.Runtime.InteropServices.DllImportAttribute("kernel32.dll", EntryPoint:="IsWow64Process")> _
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
    <Runtime.ConstrainedExecution.ReliabilityContract(Runtime.ConstrainedExecution.Consistency.WillNotCorruptState, Runtime.ConstrainedExecution.Cer.MayFail), DllImport("kernel32.dll", CharSet:=CharSet.Auto, SetLastError:=True)> _
    Public Shared Function GetModuleHandle(ByVal moduleName As String) As IntPtr
    End Function

    ''' <summary>Retrieves the address of an exported function or variable from the specified dynamic-link library (DLL)</summary>
    ''' <param name="hModule">A handle to the DLL to look for the method in</param>
    ''' <param name="methodName">The method to look for</param>
    <DllImport("kernel32.dll", CharSet:=CharSet.Ansi, SetLastError:=True, ExactSpelling:=True)> _
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

    Public Function AskUser(ByVal aMapWin As MapWindow.Interfaces.IMapWin, ByVal aParentHandle As Integer) As String
        pMapWin = aMapWin
        pApplicationName = pMapWin.ApplicationInfo.ApplicationName

        'The following line hot-wires the form to just do met data download
        'chkBASINS_Met.Checked = True : cboRegion.SelectedIndex = 0 ': Me.Height = 141 ': Return Me.XML

        Dim lExtents As MapWinGIS.Extents = Nothing
        If pMapWin IsNot Nothing AndAlso pMapWin.Layers.NumLayers > 0 Then
            Try
                lExtents = pMapWin.View.Extents
                If lExtents.xMin < lExtents.yMax AndAlso lExtents.yMin < lExtents.yMax Then
                    cboRegion.Items.Add(pRegionViewRectangle)
                End If
            Catch
            End Try

            Try
                lExtents = pMapWin.Layers(pMapWin.Layers.CurrentLayer).Extents
                If lExtents.xMin < lExtents.yMax AndAlso lExtents.yMin < lExtents.yMax Then
                    cboRegion.Items.Add(pRegionExtentSelectedLayer)
                End If
            Catch
            End Try

            Try
                lExtents = pMapWin.View.SelectedShapes.SelectBounds
                If lExtents.xMin < lExtents.yMax AndAlso lExtents.yMin < lExtents.yMax Then
                    cboRegion.Items.Add(pRegionExtentSelectedShapes)
                End If
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

        Dim lGroups As New List(Of Windows.Forms.GroupBox) '  Dictionary(Of String, Windows.Forms.GroupBox)
        If Is64BitOperatingSystem Then 'Old self-extracting archives do not work on 64 bit
            chkBASINS_DEM.Visible = False
        End If
        If pApplicationName.StartsWith("USGS GW Toolbox") Then
            lGroups.Add(grpNWISStations_GW)
            lGroups.Add(grpNWIS_GW)
            lGroups.Add(grpBASINS)
            lGroups.Add(grpNHDplus)
            lGroups.Add(grpUSGS_Seamless)

            chkBASINS_LSTORET.Visible = False
            chkBASINS_Census.Visible = False
            chkBASINS_MetStations.Left = chkBASINS_Census.Left
            chkBASINS_303d.Visible = False
            'chkBASINS_MetData.Left = chkBASINS_303d.Left
        Else
            lGroups.Add(grpBASINS)
            'lGroups.Add(grpNCDC)
            lGroups.Add(grpNHDplus)
            lGroups.Add(grpNWISStations)
            lGroups.Add(grpNWIS)
            lGroups.Add(grpUSGS_Seamless)
            lGroups.Add(grpSTORET)
            lGroups.Add(grpNLDAS)
        End If

        For Each lGroup As Windows.Forms.GroupBox In lGroups
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

        SetCheckboxVisibilityFromMapOrRegion()
        TallyPreChecked(lGroups)
        SetColorsFromAvailability()

        Do
            Me.ShowDialog(System.Windows.Forms.Control.FromHandle(New IntPtr(aParentHandle)))

            If pOk Then
                Dim lXML As String = Me.XML
                If lXML.Length = 0 Then
                    If MapWinUtility.Logger.Msg("No data was selected for download." & vbCrLf & "Return to Download window?", _
                                                         MsgBoxStyle.YesNo, "Desired Data Not Specified") = MsgBoxResult.No Then
                        Return CancelString
                    End If
                ElseIf lXML.Contains("<region>") OrElse lXML.Contains("<stationid>") Then
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
                    If MapWinUtility.Logger.Msg(lMessage & vbCrLf & "Return to Download window?", _
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
    Private PreChecked As New List(Of Windows.Forms.CheckBox)
    Private PreUnChecked As New List(Of Windows.Forms.CheckBox)
    Private Sub TallyPreChecked(aGroups As List(Of Windows.Forms.GroupBox))
        Dim lAllChecked As New List(Of Windows.Forms.CheckBox)
        For Each lGroup As Windows.Forms.GroupBox In aGroups
            For Each lChild As Windows.Forms.Control In lGroup.Controls
                If lChild.GetType.Name = "CheckBox" Then
                    Dim lChk As Windows.Forms.CheckBox = lChild
                    If lChk.Checked Then
                        PreChecked.Add(lChk)
                    Else
                        PreUnChecked.Add(lChk)
                    End If
                End If
            Next
        Next
        If PreChecked.Count > 0 Then
            For Each lChk As Windows.Forms.CheckBox In PreUnChecked
                AddHandler lChk.CheckedChanged, AddressOf PreUncheckedChanged
            Next
        End If
    End Sub

    Private Sub PreUncheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            For Each lChk As Windows.Forms.CheckBox In PreChecked
                lChk.Checked = False
            Next
            For Each lChk As Windows.Forms.CheckBox In PreUnChecked
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

    Public Function SelectedRegion() As D4EMDataManager.Region
        Dim lRegion As D4EMDataManager.Region = Nothing
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
                lRegion = New D4EMDataManager.Region(lExtents.yMax, lExtents.yMin, lExtents.xMin, lExtents.xMax, pMapWin.Project.ProjectProjection)
            End If

        Catch ex As Exception
            lRegion = frmSpecifyRegion.AskUser(Me.Icon)
        End Try

        If lRegion Is Nothing Then
            Return Nothing
        Else
            lRegion.HUC8s = HUC8s()
            lRegion.PreferredFormat = lPreferredFormat
            Return lRegion.GetProjected(D4EMDataManager.SpatialOperations.GeographicProjection)
        End If
        Return Nothing
    End Function

    'Dim pStationIDs() As String = {}

    Private Function StationsXML(ByVal aStations As Generic.List(Of String)) As String
        Dim lStationsXML As String = ""
        For Each lStation As String In aStations
            lStationsXML &= "<stationid>" & lStation & "</stationid>" & vbCrLf
        Next
        Return lStationsXML
    End Function

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

        If pApplicationName.StartsWith("USGS GW Toolbox") Then
            chkNWIS_GetNWISDailyDischarge_GW.Visible = True
            chkNWIS_GetNWISIdaDischarge_GW.Visible = True
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

        If pMapWin.View IsNot Nothing Then
            Dim lSelected As MapWindow.Interfaces.SelectInfo = pMapWin.View.SelectedShapes
            If lSelected.NumSelected > 0 Then
                Dim lLayer As MapWindow.Interfaces.Layer = pMapWin.Layers.Item(pMapWin.Layers.CurrentLayer)
                Dim lFilename As String = IO.Path.GetFileNameWithoutExtension(lLayer.FileName).ToLower

                If lFilename.StartsWith("met") Then
                    chkBASINS_MetData.ForeColor = System.Drawing.SystemColors.ControlText
                    chkBASINS_MetData.Checked = True
                    'chkNCDC_MetData.ForeColor = System.Drawing.SystemColors.ControlText
                ElseIf pApplicationName.StartsWith("USGS GW Toolbox") Then
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
                            chkNLDAS_GetNLDASParameter.Text = "Hourly Data"
                            chkNLDAS_GetNLDASParameter.Checked = True
                    End Select
                End If
            End If
        End If
        If pApplicationName.StartsWith("USGS GW Toolbox") Then
            If Not chkNWIS_GetNWISDailyDischarge_GW.Enabled AndAlso _
               Not chkNWIS_GetNWISIdaDischarge_GW.Enabled AndAlso _
               Not chkNWIS_GetNWISDailyGW_GW.Enabled AndAlso _
               Not chkNWIS_GetNWISPeriodicGW_GW.Enabled AndAlso _
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
            If Not chkNWIS_GetNWISDailyDischarge.Enabled AndAlso _
               Not chkNWIS_GetNWISIdaDischarge.Enabled AndAlso _
               Not chkNWIS_GetNWISDailyGW.Enabled AndAlso _
               Not chkNWIS_GetNWISPeriodicGW.Enabled AndAlso _
               Not chkNWIS_GetNWISWQ.Enabled AndAlso _
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
            Dim lRegion As D4EMDataManager.Region

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
            If Not pMapWin.Project Is Nothing Then
                If Not pMapWin.Project.ProjectProjection Is Nothing AndAlso pMapWin.Project.ProjectProjection.Length > 0 Then
                    lDesiredProjection = "<DesiredProjection>" & pMapWin.Project.ProjectProjection & "</DesiredProjection>" & vbCrLf
                End If
                If Not pMapWin.Project.FileName Is Nothing AndAlso pMapWin.Project.FileName.Length > 0 Then
                    lSaveFolderOnly = IO.Path.GetDirectoryName(pMapWin.Project.FileName)
                    lSaveFolder &= "<SaveIn>" & lSaveFolderOnly & "</SaveIn>" & vbCrLf
                End If
            End If

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

            For Each lControl As Windows.Forms.Control In Me.Controls
                Dim lControlName As String = lControl.Name
                If lControlName.EndsWith("_GW") Then 'Treat Groundwater version of controls the same as the BASINS version
                    lControlName = lControlName.Substring(0, lControlName.Length - 3)
                End If
                If lControlName.StartsWith("grp") AndAlso lControl.HasChildren Then
                    Dim lCheckedChildren As String = ""
                    For Each lChild As Windows.Forms.Control In lControl.Controls
                        If lChild.GetType.Name = "CheckBox" AndAlso CType(lChild, Windows.Forms.CheckBox).Checked Then
                            Dim lChildName As String = lChild.Name.Substring(lControlName.Length + 1)
                            If lChildName.EndsWith("_GW") Then
                                lChildName = lChildName.Substring(0, lChildName.Length - 3)
                            End If
                            If lChildName.ToLower.StartsWith("get") Then 'this checkbox has its own function name
                                Dim lWDMxml As String = ""
                                If pApplicationName.StartsWith("USGS") Then
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
                            lWDMxml = lWDMfrm.AskUser(Me.Icon, "Met", IO.Path.Combine(lSaveFolderOnly, "met"), _
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
        If e.KeyValue = Windows.Forms.Keys.F1 Then
            ShowHelp()
        End If
    End Sub

    Private Sub btnHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHelp.Click
        ShowHelp()
    End Sub

    Private Sub ShowHelp()
        atcUtility.ShowHelp("BASINS Details\Project Creation and Management\GIS and Time-Series Data\Download.html")
    End Sub

    Private Function RegionValid(ByVal aRegionType As String, ByRef aReason As String) As Boolean
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
End Class