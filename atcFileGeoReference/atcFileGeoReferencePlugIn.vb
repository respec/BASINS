Imports atcdata
Imports atcMwGisUtility.GisUtil
Imports MapWinUtility

Public Class atcFileGeoReferencePlugIn
    Inherits atcData.atcDataPlugIn

    Private WithEvents pForm As frmFileGeoReference
    Private pCursorSave As MapWinGIS.tkCursor

    Private pResourceManager As Resources.ResourceManager
    Private Const ParentMenuName As String = "BasinsAnalysis"
    Private Const FullMenuName As String = ParentMenuName & "_FileGeoRef"
    Private Const ParentMenuString As String = "Analysis"

    Public Sub New()
        pResourceManager = New Resources.ResourceManager("atcFileGeoReference.Resources", System.Reflection.Assembly.GetExecutingAssembly())
    End Sub

    Public Overrides ReadOnly Property Name() As String
        Get
            Return pResourceManager.GetString("Name")
        End Get
    End Property

    Public Overrides ReadOnly Property Author() As String
        Get
            Return pResourceManager.GetString("Author")
        End Get
    End Property

    Public Overrides ReadOnly Property Description() As String
        Get
            Return pResourceManager.GetString("Description")
        End Get
    End Property

    <CLSCompliant(False)> _
    Public Overrides Sub ShapesSelected(ByVal aHandle As Integer, _
                                        ByVal aSelectInfo As MapWindow.Interfaces.SelectInfo)
        'This event fires when the user selects one or more shapes using the select tool in MapWindow.
        'Make sure something was passed in SelectInfo
        If Not aSelectInfo Is Nothing Then
            Try
                If Not pForm Is Nothing AndAlso Not pForm.AddingPoint Then
                    pForm.RefreshRecordInfo(aSelectInfo(0).ShapeIndex)
                End If
            Catch ex As System.Exception
                Logger.Msg(ex.Message)
            End Try
        End If
    End Sub

    Public Overrides Sub ItemClicked(ByVal aItemName As String, ByRef aHandled As Boolean)
        If aItemName = "BasinsAnalysis_Analysis::File Geo Reference" Then
            atcMwGisUtility.GisUtil.MappingObject = pMapWin
            If pForm Is Nothing Then
                'next line commented because we can only select on one layer at a time - therefore, only one form makes sense
                'OrElse pForm.DocumentLayerIndex <> CurrentLayer Then
                If NumLayers > 0 Then
                    pMapWin.View.HandleFileDrop = False
                    pForm = New frmFileGeoReference
                    pForm.PopulateLayers(pMapWin.Layers.Item(pMapWin.Layers.CurrentLayer).Name)
                    pForm.Show()
                Else
                    Logger.Msg("Must have a project with at least one layer to use " & Me.Name, Me.Name & " Error")
                End If
            Else
                pForm.Focus()
            End If
            aHandled = True
        End If
    End Sub

    Public Overrides Sub Message(ByVal msg As String, ByRef Handled As Boolean)
        If Not pForm Is Nothing Then
            If msg.StartsWith("FileDropEvent") Then
                Dim lMsg() As String = msg.Split("|")
                pForm.AddFile(lMsg(3), lMsg(1), lMsg(2))
            End If
        End If
    End Sub

    Public Overrides Sub MapMouseDown(ByVal Button As Integer, ByVal Shift As Integer, ByVal x As Integer, ByVal y As Integer, ByRef Handled As Boolean)
        'This event fires when the user holds a mouse button down on the map. Note that x and y are returned
        'as screen coordinates (in pixels), not map coordinates.  So if you really need the map coordinates
        'then you need to use g_MapWin.View.PixelToProj()
        If Not pForm Is Nothing AndAlso pForm.AddingPoint Then
            If Button = 1 Then 'left mouse button
                Dim lPx As Double
                Dim lPy As Double
                pMapWin.View.PixelToProj(x, y, lPx, lPy)
                AddPoint(pForm.DocumentLayerIndex, lPx, lPy)
                Handled = True
            End If
            pForm.AddingPoint = False
        End If
    End Sub

    Private Sub pForm_AddPointToggle(ByVal aAdding As Boolean) Handles pForm.AddPointToggle
        If aAdding Then
            pCursorSave = pMapWin.View.MapCursor
            pMapWin.View.MapCursor = MapWinGIS.tkCursor.crsrArrow
            'TODO: use the button rather than cursor - pMapWin.Toolbar.PressToolbarButton(whatName?)
        Else
            pMapWin.View.MapCursor = pCursorSave
        End If
    End Sub

    Private Sub pForm_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles pForm.Disposed
        pForm = Nothing
    End Sub

    ''' <summary>Create a New GeoReferencing Point Shape Layer</summary>
    Friend Sub CreateNewGeoRefLayer() Handles pForm.CreateNewGeoRefLayer
        Dim lDialog As New System.Windows.Forms.SaveFileDialog
        With lDialog
            .DefaultExt = ".shp"
            .AddExtension = True
            .Filter = "Shape files (*.shp)|*.shp|All files (*.*)|*.*"
            .FilterIndex = 0
            Try
                .FileName = IO.Path.Combine(IO.Path.GetDirectoryName(LayerFileName(0)), pForm.DefaultLayerName)
            Catch e As Exception 'Can't get directory of first layer, just use current directory
                .FileName = pForm.DefaultLayerName & .DefaultExt
            End Try
            If .ShowDialog = Windows.Forms.DialogResult.OK Then
                Dim lLayerCaption As String = IO.Path.GetFileNameWithoutExtension(.FileName)

                Dim lNewLayer As New MapWinGIS.Shapefile
                IO.Directory.CreateDirectory(IO.Path.GetDirectoryName(.FileName))
                lNewLayer.CreateNew(.FileName, MapWinGIS.ShpfileType.SHP_POINT)

                lNewLayer.StartEditingTable()
                lNewLayer.EditInsertField(NewField("file", MapWinGIS.FieldType.STRING_FIELD, 1024), 1)
                lNewLayer.EditInsertField(NewField("date", MapWinGIS.FieldType.STRING_FIELD, 10), 2)
                lNewLayer.EditInsertField(NewField(pForm.AnnotationFieldName, MapWinGIS.FieldType.STRING_FIELD, 1024), 3)
                lNewLayer.EditInsertField(NewField("latitude", MapWinGIS.FieldType.DOUBLE_FIELD), 4)
                lNewLayer.EditInsertField(NewField("longitude", MapWinGIS.FieldType.DOUBLE_FIELD), 5)
                lNewLayer.StopEditingTable()

                lNewLayer.StartEditingShapes()
                'lNewLayer.EditInsertShape(lShape, 0)
                lNewLayer.StopEditingShapes()

                lNewLayer.Projection = pMapWin.Project.ProjectProjection

                lNewLayer.SaveAs(.FileName)
                lNewLayer.Close()

                lNewLayer = Nothing

                AddLayer(.FileName, lLayerCaption)
                pForm.PopulateLayers(lLayerCaption)
            End If
        End With
    End Sub

    Private Function NewField(ByVal aFieldName As String, _
                     Optional ByVal aType As MapWinGIS.FieldType = MapWinGIS.FieldType.STRING_FIELD, _
                     Optional ByVal aWidth As Integer = 0) As MapWinGIS.Field
        Dim lNewField As New MapWinGIS.Field
        lNewField.Name = aFieldName
        lNewField.Type = aType
        If aWidth < 1 Then
            Select Case aType 'allow enough space for largest number
                Case MapWinGIS.FieldType.DOUBLE_FIELD : lNewField.Width = 16
                Case MapWinGIS.FieldType.INTEGER_FIELD : lNewField.Width = 8
                Case MapWinGIS.FieldType.STRING_FIELD : lNewField.Width = 80
            End Select
        Else
            lNewField.Width = aWidth
        End If
        Return lNewField
    End Function

    Private Sub pForm_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles pForm.FormClosing
        pMapWin.View.HandleFileDrop = True
    End Sub
End Class
