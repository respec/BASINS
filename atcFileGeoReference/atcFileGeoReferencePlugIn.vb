Imports atcdata
Imports atcMwGisUtility.GisUtil

Public Class atcFileGeoReferencePlugIn
    Inherits atcData.atcDataPlugIn

    Private pMapWin As MapWindow.Interfaces.IMapWin
    Private WithEvents pForm As frmFileGeoReference
    Private pCursorSave As MapWinGIS.tkCursor

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
        MappingObject = aMapWin 'in GisUtil
        'TODO: make this consistent, be sure remove handled correctly
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
        'This event fires when the user selects one or more shapes using the select tool in MapWindow.
        'Make sure something was passed in SelectInfo
        If Not aSelectInfo Is Nothing Then
            Try
                If Not pForm Is Nothing AndAlso Not pForm.AddingPoint Then
                    pForm.RefreshRecordInfo(aSelectInfo(0).ShapeIndex)
                End If
            Catch ex As System.Exception
                MapWinUtility.Logger.Msg(ex.Message)
            End Try
        End If
    End Sub

    Public Overrides Sub ItemClicked(ByVal aItemName As String, ByRef aHandled As Boolean)
        If aItemName = FullMenuName Then
            If pForm Is Nothing Then
                'next line commented because we can only select on one layer at a time - therefore, only one form makes sense
                'OrElse pForm.DocumentLayerIndex <> CurrentLayer Then
                pForm = New frmFileGeoReference
                pForm.PopulateLayers()
                pForm.Show()
            Else
                pForm.Focus()
            End If
            aHandled = True
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
End Class
