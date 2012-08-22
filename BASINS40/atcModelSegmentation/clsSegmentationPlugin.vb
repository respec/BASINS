Imports atcMwGisUtility
Imports atcUtility
Imports mwTableEditor
Imports MapWinUtility
Imports atcData.atcDataManager

Public Class PlugIn
    Inherits atcData.atcDataPlugin

    Private pMapWinForm As Windows.Forms.Form
    Private WithEvents pFrmModelSegmentation As frmModelSegmentation

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Model Segmentation"
        End Get
    End Property

    Public Overrides ReadOnly Property Description() As String
        Get
            Return "Provides support for specifying a segmentation schema to use in a simulation model."
        End Get
    End Property

    <CLSCompliant(False)> _
    Public Overrides Sub Initialize(ByVal MapWin As MapWindow.Interfaces.IMapWin, ByVal ParentHandle As Integer)
        pMapWin = MapWin
        pMapWinForm = System.Windows.Forms.Control.FromHandle(New IntPtr(ParentHandle))

        atcData.atcDataManager.AddMenuIfMissing(ModelsMenuName, "", ModelsMenuString, "mnuFile")
        Dim lMenuItem As MapWindow.Interfaces.MenuItem
        lMenuItem = pMapWin.Menus.AddMenu(ModelsMenuName & "_ModelsSeparator", ModelsMenuName, "-", "Model Segmentation")
        atcData.atcDataManager.AddMenuIfMissing(ModelsMenuName & "_Segmentation", ModelsMenuName, "Model Segmentation")
    End Sub

    Public Overrides Sub Terminate()
        atcData.atcDataManager.RemoveMenuIfEmpty(ModelsMenuName & "_Segmentation")
        atcData.atcDataManager.RemoveMenuIfEmpty(ModelsMenuName & "_ModelsSeparator")
        atcData.atcDataManager.RemoveMenuIfEmpty(ModelsMenuName)
    End Sub

    Public Overrides Sub ItemClicked(ByVal ItemName As String, ByRef Handled As Boolean)
        If ItemName = ModelsMenuName & "_Segmentation" Then
            GisUtil.MappingObject = pMapWin
            If pFrmModelSegmentation Is Nothing OrElse pFrmModelSegmentation.IsDisposed Then
                pFrmModelSegmentation = New frmModelSegmentation
                pFrmModelSegmentation.Show(pMapWinForm)
            Else
                pFrmModelSegmentation.BringToFront()
            End If
            Handled = True
        End If
    End Sub

    Friend Sub OpenTableEditor(ByVal aSubbasinLayerName As String) Handles pFrmModelSegmentation.OpenTableEditor
        GisUtil.CurrentLayer = GisUtil.LayerIndex(aSubbasinLayerName)
        'TODO: make sure this layer is visible on legend
        'Dim pTableEditor As New frmTableEditor(CType(pMapWin.Layers(pMapWin.Layers.CurrentLayer).GetObject, MapWinGIS.Shapefile), pMapWinForm)  'used to work prior to MapWindow update
        Dim pTableEditor As New frmTableEditor(pMapWin.Layers(pMapWin.Layers.CurrentLayer).Handle, pMapWinForm)
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
