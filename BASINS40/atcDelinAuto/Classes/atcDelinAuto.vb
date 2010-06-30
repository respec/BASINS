'********************************************************************************************************
'Filename:      mwTaudemBASINSWrapper.vb
'Description:   Plugin wrapper for the modified taudem plugin to be used by BASINS
'********************************************************************************************************
'The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
'you may not use this file except in compliance with the License. You may obtain a copy of the License at 
'http://www.mozilla.org/MPL/ 
'Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
'ANY KIND, either express or implied. See the License for the specificlanguage governing rights and 
'limitations under the License. 
'
'The Original Code is MapWindow Open Source. 
'
'This class implements the MapWindow.Interfaces.Iplugin interface and all of its elements.  It defines this
'assembly as a MapWindow plug-in and provides communication between MapWindow and the mwTaudem COM dll in 
'order to form a wrapper around the regular TauDEM plugin to provide functionality requested for BASINS
'
'Contributor(s): (Open source contributors should list themselves and their modifications here). 
'Last Update:   10/18/05, ARA
'08/23/05 ARA       Modified from normal taudem wrapper
'10/18/05 ARA       Wrapping up and added mozilla comments
'16/10/09 CWG       Modified MapMouseDown section for marking outlets/inlets/reservoirs
'********************************************************************************************************

Option Explicit On

Public Class atcDelinAuto
    Implements MapWindow.Interfaces.IPlugin

    Private lstDrawPoints As New ArrayList
    Public maskShapesIdx As New ArrayList
    Public outletShapesIdx As New ArrayList

    'For drawing niftiness
    Private ReversibleDrawn As New ArrayList
    'Private ReversibleDrawn2 As New ArrayList
    Private LastStartPtX As Integer = -1
    Private LastStartPtY As Integer = -1
    Private LastEndX As Integer = -1
    Private LastEndY As Integer = -1
    Private StartPtX As Integer = -1
    Private StartPtY As Integer = -1
    Private EraseLast As Boolean = False
    Private mycolor As New System.Drawing.Color


#Region "Unused Plug-in Interface Elements"


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

    Public Sub MapMouseUp(ByVal Button As Integer, ByVal Shift As Integer, ByVal x As Integer, ByVal y As Integer, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.MapMouseUp

    End Sub

    Public Sub Message(ByVal msg As String, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.Message

    End Sub

    Public Sub ProjectLoading(ByVal ProjectFile As String, ByVal SettingsString As String) Implements MapWindow.Interfaces.IPlugin.ProjectLoading
        'dpa 4/22/2005 Save the base grid file name to the MW project
        g_BaseDEM = SettingsString
        'g_Taudem.SetBASEDEM(g_BaseDEM)
    End Sub

    Public Sub ProjectSaving(ByVal ProjectFile As String, ByRef SettingsString As String) Implements MapWindow.Interfaces.IPlugin.ProjectSaving
        'dpa 4/22/2005 Save the base grid file name to the MW project
        SettingsString = g_BaseDEM
    End Sub



#End Region

#Region "Plug-in Information"
    Public ReadOnly Property Name() As String Implements MapWindow.Interfaces.IPlugin.Name
        Get
            Return "Watershed Delineation No TD"
        End Get
    End Property

    Public ReadOnly Property Description() As String Implements MapWindow.Interfaces.IPlugin.Description
        Get
            Return "atcDelinAuto: This is a plugin for terrain analysis and watershed delineation."
        End Get
    End Property

    Public ReadOnly Property Author() As String Implements MapWindow.Interfaces.IPlugin.Author
        Get
            Return "atcDecatur"
        End Get
    End Property

    Public ReadOnly Property Version() As String Implements MapWindow.Interfaces.IPlugin.Version
        Get
            Return System.Diagnostics.FileVersionInfo.GetVersionInfo(Me.GetType.Assembly.Location).FileVersion
        End Get
    End Property

    Public ReadOnly Property BuildDate() As String Implements MapWindow.Interfaces.IPlugin.BuildDate
        Get
            Return System.IO.File.GetLastWriteTime(Me.GetType.Assembly.Location)
        End Get
    End Property

    Public ReadOnly Property SerialNumber() As String Implements MapWindow.Interfaces.IPlugin.SerialNumber
        Get
            Return "O+CW7YC/96VDT+A"
            ' This is matched to the author name in a serial number issuing program Dan ran.
        End Get
    End Property

#End Region

#Region "Start and Stop Functions"
    <CLSCompliant(False)> _
    Public Sub Initialize(ByVal MapWin As MapWindow.Interfaces.IMapWin, ByVal ParentHandle As Integer) Implements MapWindow.Interfaces.IPlugin.Initialize
        g_MapWin = MapWin  '  This sets global for use elsewhere in program
        g_handle = ParentHandle

        Dim tempPtr As System.IntPtr = ParentHandle
        Dim mapFrm As System.Windows.Forms.Form = System.Windows.Forms.Control.FromHandle(tempPtr)
        mapFrm.AddOwnedForm(g_AutoForm)

        g_StatusBar = g_MapWin.StatusBar.AddPanel("", g_MapWin.StatusBar.NumPanels - 2, 10, Windows.Forms.StatusBarPanelAutoSize.Spring)
        g_StatusBar.MinWidth = 10
        g_StatusBar.AutoSize = Windows.Forms.StatusBarPanelAutoSize.Contents
        tdbChoiceList = New tdbChoices_v2
        tdbFileList = New tdbFileTypes_v2
        tdbChoiceList.SetDefaultTDchoices()
        tdbChoiceList.ConfigFileName = IO.Path.GetDirectoryName(g_MapWin.Project.ConfigFileName) + "\awd.cfg"
        tdbChoiceList.LoadConfig()

        Dim nil As Object = Nothing
        With g_MapWin.Menus
            'atcDelinAuto_BASINS main menu
            .AddMenu("btdmWatershedDelin", nil, "Watershed Delineation")
            .AddMenu("btdmAutomaticS", "btdmWatershedDelin", nil, "Automatic(Simple)")
        End With
        'MapWinGeoProc.Hydrology.ApplyJoinBasinStreamAttributes("D:\dev\zSampleData\__iron\output\irontifnet.shp", "D:\dev\zSampleData\__iron\output\irontifw.shp", "D:\dev\zSampleData\__iron\output\irontifw_merged.shp", Nothing)
        'MapWinGeoProc.Hydrology.CreateSWATFig("D:\dev\zSampleData\__iron\output\irontifw_merged.shp", "D:\dev\zSampleData\__iron\output\irontifw_merged.fig", Nothing)

        'MapWinGeoProc.Hydrology.ApplyWatershedSlopeAttribute("D:\dev\zSampleData\__iron\output\irontifw.bgd", "D:\dev\zSampleData\__iron\output\irontifw.shp", "D:\dev\zSampleData\__iron\output\irontifsd8.bgd", MapWinGeoProc.Hydrology.ElevationUnits.meters, Nothing)
        'MapWinGeoProc.Hydrology.ApplyWatershedElevationAttribute("D:\dev\zSampleData\__iron\output\irontifw.bgd", "D:\dev\zSampleData\__iron\output\irontifw_merged.shp", "D:\dev\zSampleData\__iron\output\irontiffel.bgd", Nothing)
    End Sub

    Public Sub Terminate() Implements MapWindow.Interfaces.IPlugin.Terminate
        'This sub occurs when this plugin is un-checked in MapWindow.

        If g_AutoForm IsNot Nothing AndAlso Not g_AutoForm.IsDisposed Then g_AutoForm.Close()

        'Wrap in a try block in case one of these already got removed (by ghosts?)
        Try
            g_MapWin.StatusBar.RemovePanel(g_StatusBar)
        Catch
        End Try

        'Wrap in a try block in case one of these already got removed (by ghosts?)
        Try

            g_MapWin.Menus.Remove("btdmAutomaticS")
            If g_MapWin.Menus.Item("btdmWatershedDelin").NumSubItems = 0 Then
                g_MapWin.Menus.Remove("btdmWatershedDelin")
            End If
        Catch
        End Try
    End Sub

#End Region

#Region "Used Functions"
    Public Sub ItemClicked(ByVal ItemName As String, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.ItemClicked
        'This sub fires when a menu item is clicked in MapWindow.  Here we check if the menu item
        'is one of the TauDEM related menus, and then take action accordingly. If we handle a menu 
        'click, then we set "handled=true".  This tells MapWindow not to pass the event on to any
        'other plug-ins, or to the base application (in the case that the menu being captured is a 
        'default menu such as "mnuPrint".  Note that we are using a naming convention for taudem
        'related menu items, where the name of the menu is the corresponding taudem function 
        'preceded by "btdm".  It is assumed that this will help avoid naming conflicts with other
        'plugins.  - dpa 3/18/2005

        If ItemName.StartsWith("btdm") Then
            'We're going to run a taudem function.  Let's just reset the callback events guy just in case it was lost.
            'dpa 4/22/2005
            'g_Taudem.Initialize(Me)
        End If

        Select Case ItemName
            Case "btdmAutomaticS"
                If g_AutoForm Is Nothing Or g_AutoForm.IsDisposed Then
                    g_AutoForm = New frmAutomatic_v2
                    Dim tempPtr As System.IntPtr = g_handle
                    Dim mapFrm As System.Windows.Forms.Form = System.Windows.Forms.Control.FromHandle(tempPtr)
                    mapFrm.AddOwnedForm(g_AutoForm)
                End If
                g_AutoForm.WindowState = Windows.Forms.FormWindowState.Normal
                g_AutoForm.Initialize(Me)
                g_AutoForm.Show()
        End Select
    End Sub
#End Region

#Region "SWAT compatibility Stuff"
    Public Sub InitializeAWDPaths(ByVal DEMPath As String, ByVal OutletsPath As String, ByVal RelativeOutputPath As String)
        'Progress("Add", 0, DEMPath + "|Base DEM|1")
        lastDem = "Base DEM (" + IO.Path.GetFileName(DEMPath) + ") "

        If OutletsPath <> "" Then
            'Progress("Add", 0, OutletsPath + "|Outlets Shapefile|20")
            lastOutlet = "Outlets Shapefile (" + IO.Path.GetFileName(OutletsPath) + ") "
            Dim sf As New MapWinGIS.Shapefile
            sf.Open(OutletsPath)
            g_MapWin.View.SelectedShapes.ClearSelectedShapes()
            For i As Integer = 0 To sf.NumShapes - 1
                g_MapWin.View.SelectedShapes.AddByIndex(i, Drawing.Color.Yellow)
                outletShapesIdx.Add(i)
            Next
            g_AutoForm.lblOutletSelected.Text = outletShapesIdx.Count.ToString + " selected"
            sf.Close()
            tdbChoiceList.useOutlets = True
        End If

        tdbChoiceList.OutputPath = RelativeOutputPath
    End Sub

    Public Sub InitializeAWDSettings(ByVal useDinf As Boolean, ByVal useEdgeCheck As Boolean, ByVal calcStreamFields As Boolean, ByVal calcWatershedFields As Boolean, ByVal calcMergeShedFields As Boolean, ByVal displayPitFill As Boolean, ByVal displayD8 As Boolean, ByVal displayAreaD8 As Boolean, ByVal displayDinf As Boolean, ByVal displayAreaDinf As Boolean, ByVal displayStrahlOrd As Boolean, ByVal displayNetRaster As Boolean, ByVal displayStreamOrd As Boolean, ByVal displayWatershedGrid As Boolean, ByVal displayStreamShapefile As Boolean, ByVal displayWatershedShapefile As Boolean, ByVal displayMergeWatershed As Boolean)
        tdbChoiceList.useDinf = useDinf
        tdbChoiceList.EdgeContCheck = useEdgeCheck
        tdbChoiceList.CalcSpecialStreamFields = calcStreamFields
        tdbChoiceList.CalcSpecialWshedFields = calcWatershedFields
        tdbChoiceList.calcSpecialMergeWshedFields = calcMergeShedFields
        tdbChoiceList.AddPitfillLayer = displayPitFill
        tdbChoiceList.AddD8Layer = displayD8
        tdbChoiceList.AddD8AreaLayer = displayAreaD8
        tdbChoiceList.AddDinfLayer = displayDinf
        tdbChoiceList.AddDinfAreaLayer = displayAreaDinf
        tdbChoiceList.AddGridNetLayer = displayStrahlOrd
        tdbChoiceList.AddRiverRasterLayer = displayNetRaster
        tdbChoiceList.AddOrderGridLayer = displayStreamOrd
        tdbChoiceList.AddWShedGridLayer = displayWatershedGrid
        tdbChoiceList.AddStreamShapeLayer = displayStreamShapefile
        tdbChoiceList.AddWShedShapeLayer = displayWatershedShapefile
        tdbChoiceList.AddMergedWShedShapeLayer = displayMergeWatershed
    End Sub

    Public Function HasBeenDelineated(ByVal demPath As String) As Boolean
        tdbFileList.formFileNames(demPath, "", False)
        If IO.File.Exists(tdbFileList.fel) And IO.File.Exists(tdbFileList.mergewshed) And IO.File.Exists(tdbFileList.net) Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function HasBeenDelineated(ByVal demPath As String, ByVal outputDirectory As String) As Boolean
        tdbFileList.formFileNames(demPath, outputDirectory, False)
        If IO.File.Exists(tdbFileList.fel) And IO.File.Exists(tdbFileList.mergewshed) And IO.File.Exists(tdbFileList.net) Then
            Return True
        Else
            Return False
        End If
    End Function

    Public ReadOnly Property CurrentDEMPath() As String
        Get
            Return g_AutoForm.getPathByName(lastDem)
        End Get
    End Property

    Public ReadOnly Property CurrentDEMName() As String
        Get
            Return lastDem
        End Get
    End Property

    Public ReadOnly Property CurrentOutletPath() As String
        Get
            Return lastOutlet
        End Get
    End Property

    Public ReadOnly Property CurrentOutletName() As String
        Get
            Return g_AutoForm.getPathByName(lastOutlet)
        End Get
    End Property

    Public ReadOnly Property CurrentDEMDelineated() As String
        Get
            Return outletHasRan
        End Get
    End Property

    'Public ReadOnly Property AutoForm() As atcDelinAuto.frmAdvancedOptions_v2
    '    Get
    '        Return g_AutoForm
    '    End Get
    'End Property

    Public Property AutoFormIcon() As System.Drawing.Icon
        Get
            If Not g_AutoForm Is Nothing Then
                Return g_AutoForm.Icon
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal value As System.Drawing.Icon)
            If Not g_AutoForm Is Nothing Then
                g_AutoForm.Icon = value
                g_AutoForm.ShowIcon = (Not value Is Nothing)
            End If
        End Set
    End Property

    Public ReadOnly Property FileList() As tdbFileTypes_v2
        Get
            Return tdbFileList
        End Get
    End Property
#End Region

#Region "Drawing and Selecting Events and Functions"

    Public Sub MapMouseDown(ByVal Button As Integer, ByVal Shift As Integer, ByVal x As Integer, ByVal y As Integer, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.MapMouseDown
        Dim CurrentLayerGood As Boolean = False
        Dim currPoint As New MapWinGIS.Point
        Dim locx, locy As Double

        If g_MapWin.Layers.NumLayers > 0 Then
            CurrentLayerGood = g_MapWin.Layers.Item(g_MapWin.Layers.CurrentLayer).FileName = currDrawPath
        End If

        If g_MapWin.View.CursorMode = MapWinGIS.tkCursorMode.cmSelection Then
        ElseIf g_MapWin.View.CursorMode = MapWinGIS.tkCursorMode.cmNone Then
            If g_DrawingMask And CurrentLayerGood Then
                If Button = 2 Then
                    AddPolyToShapefile()
                    ClearTempLines()
                    frmDrawSelect.disableDone(False)
                Else
                    ' Get actual location and store it in a point which is added to point list
                    g_MapWin.View.PixelToProj(x, y, locx, locy)
                    currPoint.x = locx
                    currPoint.y = locy
                    lstDrawPoints.Add(currPoint)
                    frmDrawSelect.disableDone(True)
                    Dim mydraw As MapWindow.Interfaces.Draw = g_MapWin.View.Draw
                    mydraw.DrawPoint(locx, locy, 3, Drawing.Color.Red)
                    If (LastStartPtX = -1) Then
                        LastStartPtX = System.Windows.Forms.Control.MousePosition.X
                        LastStartPtY = System.Windows.Forms.Control.MousePosition.Y
                        StartPtX = LastStartPtX
                        StartPtY = LastStartPtY
                        EraseLast = False
                    Else
                        'Reverse the one to the start place
                        System.Windows.Forms.ControlPaint.DrawReversibleLine(New System.Drawing.Point(StartPtX, StartPtY), New System.Drawing.Point(LastEndX, LastEndY), mycolor)
                        'Permanently draw line (already drawn, don't erase -- just move it)
                        ReversibleDrawn.Add(LastStartPtX)
                        ReversibleDrawn.Add(LastStartPtY)
                        ReversibleDrawn.Add(System.Windows.Forms.Control.MousePosition.X)
                        ReversibleDrawn.Add(System.Windows.Forms.Control.MousePosition.Y)

                        'Update for next loop
                        LastStartPtX = System.Windows.Forms.Control.MousePosition.X
                        LastStartPtY = System.Windows.Forms.Control.MousePosition.Y

                        EraseLast = False
                    End If
                End If
            End If

            ' this section revised by cwg 16/10/09 to avoid memory exception when 
            ' stoppping editing shapefile, and later error when outlet has null value
            ' for INLET field
            ' - explicit pointIndex and shapeIndex variables rather than pointer
            '   to count of object being added to
            ' - INLET and RES fields always added, so all values set explicitly
            If g_DrawingOutletsOrInlets And CurrentLayerGood Then
                ' Get actual location and store it in a point which is added to point list
                g_MapWin.View.PixelToProj(x, y, locx, locy)
                currPoint.x = locx
                currPoint.y = locy

                Dim tempPt As New MapWinGIS.Shape
                tempPt.Create(MapWinGIS.ShpfileType.SHP_POINT)
                Dim pointIndex As Integer = tempPt.numPoints
                tempPt.InsertPoint(currPoint, pointIndex)
                Dim sf As MapWinGIS.Shapefile = g_MapWin.Layers.Item(g_MapWin.Layers.CurrentLayer).GetObject
                Dim shapeIndex As Integer = sf.NumShapes

                If sf.StartEditingShapes() Then
                    ' Make sure we have INLET, RES and PTSOURCE fields
                    Dim inletfieldnum As Integer = -1
                    For i As Integer = 0 To sf.NumFields - 1
                        If sf.Field(i).Name.ToUpper() = "INLET" Then
                            inletfieldnum = i
                            Exit For
                        End If
                    Next i
                    If inletfieldnum = -1 Then
                        Dim inletField As New MapWinGIS.Field
                        inletField.Name = "INLET"
                        inletField.Type = MapWinGIS.FieldType.INTEGER_FIELD
                        inletfieldnum = sf.NumFields
                        sf.EditInsertField(inletField, inletfieldnum)
                    End If

                    Dim resfieldnum As Integer = -1
                    For i As Integer = 0 To sf.NumFields - 1
                        If sf.Field(i).Name.ToUpper() = "RES" Then
                            resfieldnum = i
                            Exit For
                        End If
                    Next
                    If resfieldnum = -1 Then
                        Dim resField As New MapWinGIS.Field
                        resField.Name = "RES"
                        resField.Type = MapWinGIS.FieldType.INTEGER_FIELD
                        resfieldnum = sf.NumFields
                        sf.EditInsertField(resField, resfieldnum)
                    End If

                    Dim srcfieldnum As Integer = -1
                    For i As Integer = 0 To sf.NumFields - 1
                        If sf.Field(i).Name.ToUpper() = "PTSOURCE" Then
                            srcfieldnum = i
                            Exit For
                        End If
                    Next
                    If srcfieldnum = -1 Then
                        Dim srcField As New MapWinGIS.Field
                        srcField.Name = "PTSOURCE"
                        srcField.Type = MapWinGIS.FieldType.INTEGER_FIELD
                        srcfieldnum = sf.NumFields
                        sf.EditInsertField(srcField, srcfieldnum)
                    End If

                    sf.EditInsertShape(tempPt, shapeIndex)
                    outletShapesIdx.Add(shapeIndex)

                    If g_DrawingInlets Then
                        sf.EditCellValue(inletfieldnum, shapeIndex, 1)
                    Else
                        sf.EditCellValue(inletfieldnum, shapeIndex, 0)
                    End If

                    If g_DrawingReservoir Then
                        sf.EditCellValue(resfieldnum, shapeIndex, 1)
                    Else
                        sf.EditCellValue(resfieldnum, shapeIndex, 0)
                    End If

                    If g_DrawingPointSource Then
                        sf.EditCellValue(srcfieldnum, shapeIndex, 1)
                    Else
                        sf.EditCellValue(srcfieldnum, shapeIndex, 0)
                    End If

                    ReNumberMWShapeIDs(sf)

                    sf.StopEditingShapes()
                    g_MapWin.Refresh()
                End If


            End If
        End If
    End Sub

    Public Sub MapMouseMove(ByVal ScreenX As Integer, ByVal ScreenY As Integer, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.MapMouseMove
        'mycolor = System.Drawing.Color.FromArgb(245, 230, 200)
        mycolor = Drawing.Color.White
        If g_DrawingMask Then
            If EraseLast Then
                System.Windows.Forms.ControlPaint.DrawReversibleLine(New System.Drawing.Point(LastStartPtX, LastStartPtY), New System.Drawing.Point(LastEndX, LastEndY), mycolor)
                System.Windows.Forms.ControlPaint.DrawReversibleLine(New System.Drawing.Point(StartPtX, StartPtY), New System.Drawing.Point(LastEndX, LastEndY), mycolor)
            End If

            If Not LastStartPtX = -1 Then
                LastEndX = System.Windows.Forms.Control.MousePosition.X
                LastEndY = System.Windows.Forms.Control.MousePosition.Y
                System.Windows.Forms.ControlPaint.DrawReversibleLine(New System.Drawing.Point(LastStartPtX, LastStartPtY), New System.Drawing.Point(LastEndX, LastEndY), mycolor)
                System.Windows.Forms.ControlPaint.DrawReversibleLine(New System.Drawing.Point(StartPtX, StartPtY), New System.Drawing.Point(LastEndX, LastEndY), mycolor)
                EraseLast = True
            End If
        End If
    End Sub

    Private Sub ClearTempLines()
        If (EraseLast) Then
            System.Windows.Forms.ControlPaint.DrawReversibleLine(New System.Drawing.Point(LastStartPtX, LastStartPtY), New System.Drawing.Point(LastEndX, LastEndY), mycolor)
            System.Windows.Forms.ControlPaint.DrawReversibleLine(New System.Drawing.Point(StartPtX, StartPtY), New System.Drawing.Point(LastEndX, LastEndY), mycolor)

            LastStartPtX = -1
            LastStartPtY = -1
        End If

        For i As Integer = 0 To ReversibleDrawn.Count - 1 Step 4
            System.Windows.Forms.ControlPaint.DrawReversibleLine(New System.Drawing.Point(ReversibleDrawn(i), ReversibleDrawn(i + 1)), New System.Drawing.Point(ReversibleDrawn(i + 2), ReversibleDrawn(i + 3)), mycolor)
        Next
        EraseLast = False
        ReversibleDrawn.Clear()
        lstDrawPoints.Clear()

        Dim mydraw As MapWindow.Interfaces.Draw = g_MapWin.View.Draw
        mydraw.ClearDrawings()
    End Sub

    Private Sub AddPolyToShapefile()
        ' Only try to add if the edit points has something in it
        If lstDrawPoints.Count > 1 Then
            Dim i As Integer
            ' Create poly and set it's type to polyline IMPORTANT
            Dim tempPoly As New MapWinGIS.Shape
            tempPoly.Create(MapWinGIS.ShpfileType.SHP_POLYGON)

            ' Loop the points, inserting them into new poly
            For i = 0 To lstDrawPoints.Count - 1
                tempPoly.InsertPoint(lstDrawPoints(i), tempPoly.numPoints)
            Next
            'Add the first point to complete the poly
            tempPoly.InsertPoint(lstDrawPoints(0), tempPoly.numPoints)

            Dim sf As MapWinGIS.Shapefile = g_MapWin.Layers.Item(g_MapWin.Layers.CurrentLayer).GetObject

            If sf.StartEditingShapes() Then
                maskShapesIdx.Add(sf.NumShapes)
                sf.EditInsertShape(tempPoly, sf.NumShapes)
                sf.StopEditingShapes()
                g_MapWin.Refresh()
            End If
        End If
    End Sub

    Private Sub ReNumberMWShapeIDs(ByRef sf As MapWinGIS.Shapefile)
        Dim idfieldnum As Integer = -1
        For i As Integer = 0 To sf.NumFields - 1
            If sf.Field(i).Name.ToUpper() = "MWSHAPEID" Then
                idfieldnum = i
                Exit For
            End If
        Next

        If idfieldnum <> -1 Then
            For i As Integer = 0 To sf.NumShapes - 1
                sf.EditCellValue(idfieldnum, i, i)
            Next
        End If
    End Sub

    <CLSCompliant(False)> _
    Public Sub ShapesSelected(ByVal Handle As Integer, ByVal SelectInfo As MapWindow.Interfaces.SelectInfo) Implements MapWindow.Interfaces.IPlugin.ShapesSelected
        Dim CurrentLayerGood As Boolean
        If g_SelectingMask Or g_DrawingMask Or g_SelectingOutlets Or g_DrawingOutletsOrInlets Then
            If g_MapWin.Layers.NumLayers > 0 Then
                CurrentLayerGood = g_MapWin.Layers.Item(g_MapWin.Layers.CurrentLayer).FileName = currSelectPath
            End If

            If Not CurrentLayerGood Then
                g_MapWin.View.SelectedShapes.ClearSelectedShapes()
                g_MapWin.Layers.CurrentLayer = g_AutoForm.getIndexByPath(currSelectPath)
            End If
        End If
    End Sub
#End Region

#Region "Helper Functions"
    Private Function GetColorScheme(ByVal FileType As Integer, ByVal Min As Double, ByVal MAx As Double) As MapWinGIS.GridColorScheme
        'This function generates a coloring scheme based on the Taudem filetype - dpa 4/23/2005
        Dim CS As New MapWinGIS.GridColorScheme
        Dim BR As New MapWinGIS.GridColorBreak
        Dim R1 As Integer, R2 As Integer, G1 As Integer, G2 As Integer, B1 As Integer, B2 As Integer
        Dim HighVal As Integer, StepVal As Integer, MidVal As Integer, i As Integer
        Try
            Select Case FileType
                Case 1 'Base DEM
                    CS.UsePredefined(Min, MAx, MapWinGIS.PredefinedColorScheme.SummerMountains)
                    Return CS
                Case 2 'Pit filled DEM
                    CS.UsePredefined(Min, MAx, MapWinGIS.PredefinedColorScheme.SummerMountains)
                    Return CS
                Case 3 ' D8 Slope - eight unique colors
                    BR = New MapWinGIS.GridColorBreak
                    BR.LowValue = 1 : BR.HighValue = 1
                    BR.LowColor = System.Convert.ToUInt32(RGB(46, 139, 87))
                    BR.HighColor = System.Convert.ToUInt32(RGB(46, 139, 87))
                    BR.Caption = "East"
                    BR.ColoringType = MapWinGIS.ColoringType.Random
                    CS.InsertBreak(BR)

                    BR = New MapWinGIS.GridColorBreak
                    BR.LowValue = 2 : BR.HighValue = 2
                    BR.LowColor = System.Convert.ToUInt32(RGB(0, 0, 255))
                    BR.HighColor = System.Convert.ToUInt32(RGB(0, 0, 255))
                    BR.Caption = "Northeast"
                    BR.ColoringType = MapWinGIS.ColoringType.Random
                    CS.InsertBreak(BR)

                    BR = New MapWinGIS.GridColorBreak
                    BR.LowValue = 3 : BR.HighValue = 3
                    BR.LowColor = System.Convert.ToUInt32(RGB(255, 255, 0))
                    BR.HighColor = System.Convert.ToUInt32(RGB(255, 255, 0))
                    BR.Caption = "North"
                    BR.ColoringType = MapWinGIS.ColoringType.Random
                    CS.InsertBreak(BR)

                    BR = New MapWinGIS.GridColorBreak
                    BR.LowValue = 4 : BR.HighValue = 4
                    BR.LowColor = System.Convert.ToUInt32(RGB(218, 165, 32))
                    BR.HighColor = System.Convert.ToUInt32(RGB(218, 165, 32))
                    BR.Caption = "Northwest"
                    BR.ColoringType = MapWinGIS.ColoringType.Random
                    CS.InsertBreak(BR)

                    BR = New MapWinGIS.GridColorBreak
                    BR.LowValue = 5 : BR.HighValue = 5
                    BR.LowColor = System.Convert.ToUInt32(RGB(222, 184, 135))
                    BR.HighColor = System.Convert.ToUInt32(RGB(222, 184, 135))
                    BR.Caption = "West"
                    BR.ColoringType = MapWinGIS.ColoringType.Random
                    CS.InsertBreak(BR)

                    BR = New MapWinGIS.GridColorBreak
                    BR.LowValue = 6 : BR.HighValue = 6
                    BR.LowColor = System.Convert.ToUInt32(RGB(255, 0, 0))
                    BR.HighColor = System.Convert.ToUInt32(RGB(255, 0, 0))
                    BR.Caption = "Southwest"
                    BR.ColoringType = MapWinGIS.ColoringType.Random
                    CS.InsertBreak(BR)

                    BR = New MapWinGIS.GridColorBreak
                    BR.LowValue = 7 : BR.HighValue = 7
                    BR.LowColor = System.Convert.ToUInt32(RGB(153, 50, 204))
                    BR.HighColor = System.Convert.ToUInt32(RGB(153, 50, 204))
                    BR.Caption = "South"
                    BR.ColoringType = MapWinGIS.ColoringType.Random
                    CS.InsertBreak(BR)

                    BR = New MapWinGIS.GridColorBreak
                    BR.LowValue = 8 : BR.HighValue = 8
                    BR.LowColor = System.Convert.ToUInt32(RGB(123, 104, 238))
                    BR.HighColor = System.Convert.ToUInt32(RGB(123, 104, 238))
                    BR.Caption = "Southeast"
                    BR.ColoringType = MapWinGIS.ColoringType.Random
                    CS.InsertBreak(BR)
                    Return CS

                Case 4, 6 'D8 slope, Dinf slope
                    BR = New MapWinGIS.GridColorBreak ' white to green
                    BR.LowValue = 0 : BR.HighValue = 1
                    R1 = 255 : G1 = 255 : B1 = 255 : R2 = 0 : G2 = 255 : B2 = 0
                    BR.LowColor = System.Convert.ToUInt32(RGB(R1, G1, B1))
                    BR.HighColor = System.Convert.ToUInt32(RGB(R2, G2, B2))
                    BR.ColoringType = MapWinGIS.ColoringType.Gradient
                    CS.InsertBreak(BR)

                    BR = New MapWinGIS.GridColorBreak 'green to dark green
                    BR.LowValue = 1 : BR.HighValue = MAx
                    R1 = 0 : G1 = 255 : B1 = 0 : R2 = 0 : G2 = 100 : B2 = 0
                    BR.LowColor = System.Convert.ToUInt32(RGB(R1, G1, B1))
                    BR.HighColor = System.Convert.ToUInt32(RGB(R2, G2, B2))
                    BR.ColoringType = MapWinGIS.ColoringType.Gradient
                    CS.InsertBreak(BR)
                    Return CS

                Case 5 'Dinf flow dir - white to brown
                    BR.LowValue = Min
                    BR.HighValue = MAx
                    R1 = 255 : G1 = 255 : B1 = 255 : R2 = 139 : G2 = 69 : B2 = 19
                    BR.LowColor = System.Convert.ToUInt32(RGB(R1, G1, B1))
                    BR.HighColor = System.Convert.ToUInt32(RGB(R2, G2, B2))
                    BR.ColoringType = MapWinGIS.ColoringType.Gradient
                    CS.InsertBreak(BR)
                    Return CS

                Case 7, 8 'D8 area, Dinf area - shading from white to red
                    BR = New MapWinGIS.GridColorBreak
                    BR.LowValue = Min : BR.HighValue = MAx
                    BR.LowColor = System.Convert.ToUInt32(RGB(255, 255, 255))
                    BR.HighColor = System.Convert.ToUInt32(RGB(255, 0, 0))
                    BR.ColoringType = MapWinGIS.ColoringType.Gradient
                    BR.GradientModel = MapWinGIS.GradientModel.Logorithmic
                    CS.InsertBreak(BR)
                    Return CS

                Case 9, 13 'grid order stream order - shading from yellow to green to blue
                    'yellow = RGB(255, 255, 0)
                    'green = RGB(0,255,0)
                    'blue = RGB(0,0,255)
                    HighVal = CInt(MAx)
                    If HighVal > 1 Then
                        MidVal = CInt(HighVal / 2)
                        StepVal = CInt(255 / MidVal)
                    Else
                        MidVal = 0
                        StepVal = 1
                    End If
                    For i = 1 To MidVal
                        'color breaks from yellow to green
                        R1 = 255 - StepVal * (i - 1) : G1 = 255 : B1 = 0
                        '  Guard against invalid colors DGT 10/1/05
                        If (R1 < 0) Then R1 = 0
                        If (R1 > 255) Then R1 = 255
                        BR = New MapWinGIS.GridColorBreak
                        BR.LowValue = i : BR.HighValue = i
                        BR.LowColor = System.Convert.ToUInt32(RGB(R1, G1, B1))
                        BR.HighColor = System.Convert.ToUInt32(RGB(R1, G1, B1))
                        BR.Caption = i
                        BR.ColoringType = MapWinGIS.ColoringType.Random
                        CS.InsertBreak(BR)
                    Next
                    For i = MidVal + 1 To HighVal
                        'color breaks from green to blue
                        R1 = 0 : G1 = 255 - StepVal * (i - MidVal - 1) : B1 = 0 + StepVal * (i - MidVal - 1)
                        '   Guard against invalid colors   DGT 10/1/05
                        If (G1 < 0) Then G1 = 0
                        If (B1 < 0) Then B1 = 0
                        If (G1 > 255) Then G1 = 255
                        If (B1 > 255) Then B1 = 255
                        BR = New MapWinGIS.GridColorBreak
                        BR.LowValue = i : BR.HighValue = i
                        BR.LowColor = System.Convert.ToUInt32(RGB(R1, G1, B1))
                        BR.HighColor = System.Convert.ToUInt32(RGB(R1, G1, B1))
                        BR.Caption = i
                        BR.ColoringType = MapWinGIS.ColoringType.Random
                        CS.InsertBreak(BR)
                    Next
                    CS.NoDataColor = System.Convert.ToUInt32(RGB(255, 255, 255))
                    Return CS

                Case 10, 11 'Longest upslope path and length

                    'yellow to green
                    BR = New MapWinGIS.GridColorBreak
                    BR.LowValue = Min : BR.HighValue = MAx / 10
                    BR.LowColor = System.Convert.ToUInt32(RGB(255, 255, 0))
                    BR.HighColor = System.Convert.ToUInt32(RGB(0, 255, 0))
                    BR.ColoringType = MapWinGIS.ColoringType.Gradient
                    CS.InsertBreak(BR)

                    'green to blue
                    BR = New MapWinGIS.GridColorBreak
                    BR.LowValue = MAx / 10 : BR.HighValue = MAx
                    BR.LowColor = System.Convert.ToUInt32(RGB(0, 255, 0))
                    BR.HighColor = System.Convert.ToUInt32(RGB(0, 0, 255))
                    BR.ColoringType = MapWinGIS.ColoringType.Gradient
                    CS.InsertBreak(BR)
                    Return CS

                Case 12 ' Stream raster white for 0 and blue elsewhere
                    BR = New MapWinGIS.GridColorBreak
                    CS.NoDataColor = System.Convert.ToUInt32(RGB(255, 255, 255))
                    BR.LowValue = 0 : BR.HighValue = 0
                    BR.LowColor = System.Convert.ToUInt32(RGB(255, 255, 255))
                    BR.HighColor = System.Convert.ToUInt32(RGB(255, 255, 255))
                    BR.ColoringType = MapWinGIS.ColoringType.Random
                    CS.InsertBreak(BR)

                    BR = New MapWinGIS.GridColorBreak
                    BR.LowValue = 1 : BR.HighValue = 1
                    BR.LowColor = System.Convert.ToUInt32(RGB(0, 0, 255))
                    BR.HighColor = System.Convert.ToUInt32(RGB(0, 0, 255))
                    BR.ColoringType = MapWinGIS.ColoringType.Random
                    CS.InsertBreak(BR)
                    Return CS

                Case 14  ' Watershed grid.  White for no data random colors elsewhere.
                    HighVal = CInt(MAx)
                    For i = 1 To HighVal
                        R1 = Int(Rnd() * 255) : G1 = Int(Rnd() * 255) : B1 = Int(Rnd() * 255)
                        BR = New MapWinGIS.GridColorBreak
                        BR.LowValue = i : BR.HighValue = i
                        BR.LowColor = System.Convert.ToUInt32(RGB(R1, G1, B1))
                        BR.HighColor = System.Convert.ToUInt32(RGB(R1, G1, B1))
                        BR.Caption = i
                        BR.ColoringType = MapWinGIS.ColoringType.Random
                        CS.InsertBreak(BR)
                    Next
                    CS.NoDataColor = System.Convert.ToUInt32(RGB(255, 255, 255))
                    Return CS
                Case 43 ' mask grid
                    BR.HighColor = System.Convert.ToUInt32(RGB(0, 0, 0))
                    BR.LowColor = System.Convert.ToUInt32(RGB(0, 0, 0))
                    BR.HighValue = MAx
                    BR.LowValue = Min
                    CS.InsertBreak(BR)
                    CS.NoDataColor = System.Convert.ToUInt32(RGB(255, 255, 255))
                    Return CS
                Case Else
                    BR.LowValue = Min
                    BR.HighValue = MAx
                    R1 = Int(Rnd() * 255) : R2 = Int(Rnd() * 255) : G1 = Int(Rnd() * 255) : G2 = Int(Rnd() * 255) : B1 = Int(Rnd() * 255) : B2 = Int(Rnd() * 255)
                    BR.LowColor = System.Convert.ToUInt32(RGB(R1, G1, B1))
                    BR.HighColor = System.Convert.ToUInt32(RGB(R2, G2, B2))
                    BR.ColoringType = MapWinGIS.ColoringType.Gradient
                    CS.InsertBreak(BR)
                    Return CS

            End Select
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        Return Nothing
    End Function

    Private Function GetTDGroup() As Integer
        'Returns the handle for the Taudem group.  It's looking for the first group named "Terrain Analysis"
        'dpa 4/22/05
        Dim i As Integer
        For i = 0 To g_MapWin.Layers.Groups.Count - 1
            If g_MapWin.Layers.Groups.ItemByPosition(i).Text = "Terrain Analysis" Then
                Return g_MapWin.Layers.Groups.ItemByPosition(i).Handle
            End If
        Next
        'if we get here then the group wasn't found, so we can add it and return the handle.
        Return g_MapWin.Layers.Groups.Add("Terrain Analysis")
    End Function
#End Region
End Class

