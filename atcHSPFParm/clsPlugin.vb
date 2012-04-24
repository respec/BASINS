Imports atcMwGisUtility
Imports atcData.atcDataManager

Public Class PlugIn
    Inherits atcData.atcDataPlugin

    Private pFrmHSPFParm As frmHSPFParm
    Private pInitialized As Boolean
    Friend Shared pHSPFParmDB As HSPFParmDB = Nothing

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "HSPFParm - Parameter Database for HSPF"
        End Get
    End Property

    Public Overrides ReadOnly Property Description() As String
        Get
            Return "Parameter Database for HSPF"
        End Get
    End Property

    <CLSCompliant(False)> _
    Public Overrides Sub Initialize(ByVal MapWin As MapWindow.Interfaces.IMapWin, ByVal ParentHandle As Integer)
        pMapWin = MapWin
        atcData.atcDataManager.AddMenuIfMissing(ModelsMenuName, "", ModelsMenuString, "mnuFile")
        atcData.atcDataManager.AddMenuIfMissing(ModelsMenuName & "_HSPFParm", ModelsMenuName, "HSPFParm")
        If pHSPFParmDB Is Nothing Then
            pHSPFParmDB = New HSPFParmDB("")
        End If
        pInitialized = False
    End Sub

    Public Overrides Sub Terminate()
        atcData.atcDataManager.RemoveMenuIfEmpty(ModelsMenuName & "_HSPFParm")
        atcData.atcDataManager.RemoveMenuIfEmpty(ModelsMenuName)
    End Sub

    Public Overrides Sub ItemClicked(ByVal ItemName As String, ByRef Handled As Boolean)
        If ItemName = ModelsMenuName & "_HSPFParm" Then
            GisUtil.MappingObject = pMapWin

            Dim lPath As String = ""
            If Not pHSPFParmDB Is Nothing Then
                lPath = IO.Path.GetDirectoryName(pHSPFParmDB.Name)
            End If

            Dim lDBName As String = ""
            If Not pHSPFParmDB Is Nothing Then
                lDBName = pHSPFParmDB.Name
            End If

            Dim lCreateNew As Boolean = True
            If Not pFrmHSPFParm Is Nothing Then
                If pFrmHSPFParm.Visible = True Then
                    pFrmHSPFParm.BringToFront()
                    lCreateNew = False
                End If
            End If
            If lCreateNew Then
                pFrmHSPFParm = New frmHSPFParm
                pFrmHSPFParm.InitializeUI(lPath, lDBName, pMapWin)
                pFrmHSPFParm.Show()
                pInitialized = True
                Handled = True
            End If
        End If
    End Sub

    <CLSCompliant(False)> _
    Public Overrides Sub ShapesSelected(ByVal aHandle As Integer, ByVal aSelectInfo As MapWindow.Interfaces.SelectInfo)
        MyBase.ShapesSelected(aHandle, aSelectInfo)
        If pInitialized Then
            pFrmHSPFParm.MapSelectedChanged()
        End If
    End Sub
End Class
