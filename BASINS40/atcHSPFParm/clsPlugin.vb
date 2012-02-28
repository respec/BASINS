Imports atcMwGisUtility
Imports atcData.atcDataManager

Public Class PlugIn
    Inherits atcData.atcDataPlugin

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
    End Sub

    Public Overrides Sub Terminate()
        atcData.atcDataManager.RemoveMenuIfEmpty(ModelsMenuName & "_HSPFParm")
        atcData.atcDataManager.RemoveMenuIfEmpty(ModelsMenuName)
    End Sub

    Public Overrides Sub ItemClicked(ByVal ItemName As String, ByRef Handled As Boolean)
        If ItemName = ModelsMenuName & "_HSPFParm" Then
            Dim lFormHspfParm As New frmHSPFParm
            GisUtil.MappingObject = pMapWin
            lFormHspfParm.InitializeUI()
            lFormHspfParm.Show()
            Handled = True
        End If
    End Sub
End Class
