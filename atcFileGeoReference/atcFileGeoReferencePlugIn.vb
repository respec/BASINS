Imports atcdata
Imports atcMwGisUtility

Public Class atcFileGeoReferencePlugIn
    Inherits atcData.atcDataPlugIn

    Private pMapWin As MapWindow.Interfaces.IMapWin
    Private pLayer As MapWinGIS.Shapefile
    Private pNewPoint As MapWinGIS.Point
    Private pForm As frmFileGeoReference

    Private pResourceManager As Resources.ResourceManager
    Private Const ParentMenuName As String = "BasinsAnalysis"
    Private Const FullMenuName As String = ParentMenuName & "_FileGeoRef"
    Private Const ParentMenuString As String = "Analysis"

    Public Sub New()
        pResourceManager = New Resources.ResourceManager("atcFileGeoReference.Resources", System.Reflection.Assembly.GetExecutingAssembly())
    End Sub

    <CLSCompliant(False)> _
    Public Overrides Sub Initialize(ByVal aMapWin As MapWindow.Interfaces.IMapWin, _
                                    ByVal aParentHandle As Integer)
        pMapWin = aMapWin
        'TODO: make this consitant, be sure remove handled appropriatey
        pMapWin.Menus.AddMenu(ParentMenuName, "", Nothing, ParentMenuString, "mnuFile")
        pMapWin.Menus.AddMenu(FullMenuName, ParentMenuName, Nothing, "File Geo Reference")
        MyBase.Initialize(aMapWin, aParentHandle)
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
        'This event fires when the user selects one or more shapes using the select tool in MapWindow. Handle is the 

        'make sure something was returned in SelectInfo
        If aSelectInfo Is Nothing Then Exit Sub
        Try
            If Not pForm Is Nothing Then
                pForm.RefreshRecordInfo(aSelectInfo(0).ShapeIndex + 1)
            End If
            ''get the shapefile object on which a shape was selected
            'Dim lSf As MapWinGIS.Shapefile = pMapWin.Layers(aHandle).GetObject

            ''TODO: convience function for this?
            ''Check to see if this shapefile has an attribute field "FileOrURL"
            'Dim lFileOrURL As String = ""
            'For lField As Integer = 0 To lSf.NumFields - 1
            '    Dim lFieldName As String = lSf.Field(lField).Name.ToLower
            '    If lFieldName = "fileorurl" Or _
            '       lFieldName = "file" Or _
            '       lFieldName = "url" Then 'save value in first selected record for this field
            '        lFileOrURL = lSf.CellValue(lField, aSelectInfo(0).ShapeIndex)
            '        Exit For
            '    End If
            'Next
            'If lFileOrURL.Length > 0 Then
            '    MapWinUtility.Logger.Dbg("FileGeoReference: Launch File or URL: " & lFileOrURL)
            '    Dim lNewProcess As New Process
            '    lNewProcess.StartInfo.FileName = lFileOrURL
            '    lNewProcess.Start()
            'End If
        Catch ex As System.Exception
            MapWinUtility.Logger.Msg(ex.Message)
        End Try
    End Sub

    Public Overrides Sub ItemClicked(ByVal aItemName As String, ByRef aHandled As Boolean)
        If aItemName = FullMenuName Then
            pForm = New frmFileGeoReference

            For lLayerIndex As Integer = 0 To pMapWin.Layers.NumLayers - 1
                If pMapWin.Layers(lLayerIndex).LayerType = MapWindow.Interfaces.eLayerType.PointShapefile Then
                    Dim lLayerName As String = pMapWin.Layers(lLayerIndex).Name
                    pForm.cboLayer.Items.Add(lLayerName)
                    If InStr(lLayerName.ToLower, "photo") Then
                        pForm.cboLayer.SelectedIndex = pForm.cboLayer.Items.Count - 1
                    End If
                End If
            Next
            If pForm.cboLayer.Items.Count > 0 Then
                pForm.Show()
            Else
                MapWinUtility.Logger.Msg("No Point Layers Available") 'TODO: Create One?
            End If
            aHandled = True
        End If
    End Sub

    Public Overrides Sub MapMouseDown(ByVal Button As Integer, ByVal Shift As Integer, ByVal x As Integer, ByVal y As Integer, ByRef Handled As Boolean)
        'This event fires when the user holds a mouse button down on the map. Note that x and y are returned
        'as screen coordinates (in pixels), not map coordinates.  So if you really need the map coordinates
        'then you need to use g_MapWin.View.PixelToProj()
        If pForm.AddingPoint Then
            If Button = 1 Then 'left mouse button
                Dim lPx As Double
                Dim lPy As Double
                pMapWin.View.PixelToProj(x, y, lPx, lPy)
                GisUtil.AddPoint(GisUtil.CurrentLayer, lPx, lPy)
                Handled = True
            End If
            pForm.AddingPoint = False
        End If
    End Sub

End Class
