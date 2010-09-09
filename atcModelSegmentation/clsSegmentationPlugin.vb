Imports atcMwGisUtility
Imports atcUtility
Imports mwTableEditor
Imports MapWinUtility

Public Class PlugIn
    Implements MapWindow.Interfaces.IPlugin

    Private pMapWin As MapWindow.Interfaces.IMapWin
    Private pMapWinForm As Windows.Forms.Form
    Private WithEvents pFrmModelSegmentation As frmModelSegmentation

    'TODO: get these from BASINS4 or plugInManager?
    Private Const ModelsMenuName As String = "BasinsModels"
    Private Const ModelsMenuString As String = "Models"

    Public ReadOnly Property Name() As String Implements MapWindow.Interfaces.IPlugin.Name
        Get
            Return "Model Segmentation"
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
            Return "Provides support for specifying a segmentation schema to use in a simulation model."
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
    Public Sub Initialize(ByVal aMapWin As MapWindow.Interfaces.IMapWin, _
                          ByVal aParentHandle As Integer) Implements MapWindow.Interfaces.IPlugin.Initialize
        pMapWin = aMapWin
        pMapWinForm = System.Windows.Forms.Control.FromHandle(New IntPtr(aParentHandle))
        If Not pMapWin.Plugins.PluginIsLoaded(pMapWin.Plugins.GetPluginKey("BASINS 4")) Then
            pMapWin.Menus.AddMenu(ModelsMenuName, "", Nothing, ModelsMenuString, "mnuFile")
        End If
        Dim lMenuItem As MapWindow.Interfaces.MenuItem
        lMenuItem = pMapWin.Menus.AddMenu(ModelsMenuName & "_ModelsSeparator", ModelsMenuName, "-", "Model Segmentation")
        lMenuItem = pMapWin.Menus.AddMenu(ModelsMenuName & "_Segmentation", ModelsMenuName, Nothing, "Model Segmentation")
    End Sub

    Public Sub Terminate() Implements MapWindow.Interfaces.IPlugin.Terminate
    End Sub

    Public Sub ItemClicked(ByVal aItemName As String, _
                           ByRef aHandled As Boolean) Implements MapWindow.Interfaces.IPlugin.ItemClicked
        If aItemName = ModelsMenuName & "_Segmentation" Then
            GisUtil.MappingObject = pMapWin
            If pFrmModelSegmentation Is Nothing OrElse pFrmModelSegmentation.IsDisposed Then
                pFrmModelSegmentation = New frmModelSegmentation
            End If
            pFrmModelSegmentation.Show(pMapWinForm)
            aHandled = True
        End If
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

    Public Sub MapMouseMove(ByVal ScreenX As Integer, ByVal ScreenY As Integer, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.MapMouseMove
    End Sub

    Public Sub MapMouseUp(ByVal Button As Integer, ByVal Shift As Integer, ByVal x As Integer, ByVal y As Integer, ByRef Handled As Boolean) Implements MapWindow.Interfaces.IPlugin.MapMouseUp
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

    Friend Sub OpenTableEditor(ByVal aSubbasinLayerName As String) Handles pFrmModelSegmentation.OpenTableEditor
        GisUtil.CurrentLayer = GisUtil.LayerIndex(aSubbasinLayerName)
        'TODO: make sure this layer is visible on legend
        Dim pTableEditor As New frmTableEditor(CType(pMapWin.Layers(pMapWin.Layers.CurrentLayer).GetObject, MapWinGIS.Shapefile), pMapWinForm)
        'TODO: would be nice to show only the model segment field, but requires changes to the table editor
        pTableEditor.Show()
        'pMapWin.Plugins.BroadcastMessage("TableEditorStart")
    End Sub
    Friend Sub TableEdited() Handles pFrmModelSegmentation.TableEdited
        pMapWin.Plugins.BroadcastMessage("TableEdited")
    End Sub

    Public Shared Sub AssignMetStationsByProximity(ByVal aSubbasinLayerName As String, ByVal aMetLayerName As String, ByVal aUseSelected As Boolean)

        Dim lSubbasinLayerIndex As Integer = GisUtil.LayerIndex(aSubbasinLayerName)
        Dim lMetLayerIndex As Integer = GisUtil.LayerIndex(aMetLayerName)
        Dim lModelSegFieldIndex As Integer = MetSegFieldIndex(lSubbasinLayerIndex)

        'build local collection of selected features in met station layer
        Dim lMetStationsSelected As New atcCollection
        If aUseSelected Then
            For lIndex As Integer = 1 To GisUtil.NumSelectedFeatures(lMetLayerIndex)
                lMetStationsSelected.Add(GisUtil.IndexOfNthSelectedFeatureInLayer(lIndex - 1, lMetLayerIndex))
            Next
        End If
        If lMetStationsSelected.Count = 0 Then
            'no met stations selected, act as if all are selected
            For lIndex As Integer = 1 To GisUtil.NumFeatures(lMetLayerIndex)
                lMetStationsSelected.Add(lIndex - 1)
            Next
        End If

        'loop through each subbasin and assign to nearest met station
        Dim lSubBasinX As Double, lSubBasinY As Double
        Dim lMetSegX As Double, lMetSegY As Double
        GisUtil.StartSetFeatureValue(lSubbasinLayerIndex)
        For lSubBasinIndex As Integer = 1 To GisUtil.NumFeatures(lSubbasinLayerIndex)
            'do progress
            Logger.Progress("Assigning Nearest Met Station", lSubBasinIndex, GisUtil.NumFeatures(lSubbasinLayerIndex))
            System.Windows.Forms.Application.DoEvents()
            'get centroid of this subbasin
            GisUtil.ShapeCentroid(lSubbasinLayerIndex, lSubBasinIndex - 1, lSubBasinX, lSubBasinY)
            'now find the closest met station
            Dim lShortestDistance As Double = Double.MaxValue
            Dim lClosestMetStationIndex As Integer = -1
            Dim lDistance As Double = 0.0
            For lMetStationIndex As Integer = 0 To lMetStationsSelected.Count - 1
                GisUtil.PointXY(lMetLayerIndex, lMetStationsSelected(lMetStationIndex), lMetSegX, lMetSegY)
                'calculate the distance
                'lDistance = Math.Sqrt(((Math.Abs(lSubBasinX) - Math.Abs(lMetSegX)) ^ 2) + _
                '                      ((Math.Abs(lSubBasinY) - Math.Abs(lMetSegY)) ^ 2))
                lDistance = Math.Sqrt(((lSubBasinX - lMetSegX) ^ 2) + _
                                      ((lSubBasinY - lMetSegY) ^ 2))
                If lDistance < lShortestDistance Then
                    lShortestDistance = lDistance
                    lClosestMetStationIndex = lMetStationsSelected(lMetStationIndex)
                End If
            Next
            If lClosestMetStationIndex > -1 Then 'set ModelSeg attribute
                Dim lModelSegText As String = ""
                Dim lLocationFieldIndex As Integer = -1
                If GisUtil.IsField(lMetLayerIndex, "Location") Then
                    lLocationFieldIndex = GisUtil.FieldIndex(lMetLayerIndex, "Location")
                    lModelSegText = GisUtil.FieldValue(lMetLayerIndex, lClosestMetStationIndex, lLocationFieldIndex)
                End If
                If GisUtil.IsField(lMetLayerIndex, "Stanam") Then
                    lLocationFieldIndex = GisUtil.FieldIndex(lMetLayerIndex, "Stanam")
                    If lModelSegText.Length > 0 Then
                        lModelSegText = lModelSegText & ":"
                    End If
                    lModelSegText = lModelSegText & GisUtil.FieldValue(lMetLayerIndex, lClosestMetStationIndex, lLocationFieldIndex)
                End If
                If lModelSegText.Length = 0 Then
                    lModelSegText = CStr(lClosestMetStationIndex)
                End If
                GisUtil.SetFeatureValueNoStartStop(lSubbasinLayerIndex, lModelSegFieldIndex, lSubBasinIndex - 1, lModelSegText)
            End If
        Next
        GisUtil.StopSetFeatureValue(lSubbasinLayerIndex)
    End Sub

    Public Shared Function MetSegFieldIndex(ByVal aSubbasinLayerIndex As Integer) As Integer
        'check to see if modelseg field is on subbasins layer, add if not 
        Dim lModelSegFieldIndex As Integer = -1
        If GisUtil.IsField(aSubbasinLayerIndex, "ModelSeg") Then
            lModelSegFieldIndex = GisUtil.FieldIndex(aSubbasinLayerIndex, "ModelSeg")
        Else  'need to add it
            lModelSegFieldIndex = GisUtil.AddField(aSubbasinLayerIndex, "ModelSeg", 0, 40)
        End If
        Return lModelSegFieldIndex
    End Function
End Class
