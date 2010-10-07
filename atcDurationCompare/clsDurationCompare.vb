Imports atcData
Imports atcUtility

Public Class clsDurationComparePlugin
    Inherits atcDataPlugin

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Analysis::SWSTAT::Duration/Compare"
        End Get
    End Property

    <CLSCompliant(False)> _
    Public Overrides Sub Initialize(ByVal aMapWin As MapWindow.Interfaces.IMapWin, _
                                    ByVal aParentHandle As Integer)

        pMapWin = aMapWin
        pMapWinWindowHandle = aParentHandle
        atcDataManager.MapWindow = aMapWin

        Dim lSWSTATMenuName As String = atcDataManager.AnalysisMenuName & "_SWSTAT"

        atcDataManager.AddMenuIfMissing(atcDataManager.AnalysisMenuName, "", atcDataManager.AnalysisMenuString, atcDataManager.FileMenuName)
        atcDataManager.AddMenuIfMissing(lSWSTATMenuName, atcDataManager.AnalysisMenuName, "SWSTAT")

        pMenusAdded.Add(atcDataManager.AddMenuWithIcon(atcDataManager.AnalysisMenuName & "_DurationCompare", lSWSTATMenuName, "Duration/Compare", Me.Icon, , , True))
        pMenusAdded.Add(atcDataManager.AddMenuWithIcon(atcDataManager.AnalysisMenuName & "_DurationHydrograph", lSWSTATMenuName, "Duration Hydrograph", Me.Icon, , , True))
    End Sub

    Public Overrides ReadOnly Property Icon() As System.Drawing.Icon
        Get
            Dim lResources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAnalysis))
            Return CType(lResources.GetObject("$this.Icon"), System.Drawing.Icon)
        End Get
    End Property

    Public Overrides Sub ItemClicked(ByVal aItemName As String, ByRef aHandled As Boolean)
        Dim lFrmClassLimits As frmAnalysis = Nothing

        If aItemName = atcDataManager.AnalysisMenuName & "_DurationCompare" Then
            Dim lTimeseriesGroup As atcTimeseriesGroup = atcDataManager.UserSelectData("Select Data For Duration/Compare")
            If lTimeseriesGroup.Count > 0 Then
                lFrmClassLimits = New frmAnalysis(lTimeseriesGroup)
                If pMapWin IsNot Nothing AndAlso pMapWin.ApplicationInfo.FormIcon IsNot Nothing Then lFrmClassLimits.Icon = pMapWin.ApplicationInfo.FormIcon
                lFrmClassLimits.Show()
            End If
        ElseIf aItemName = atcDataManager.AnalysisMenuName & "_DurationHydrograph" Then
            Dim lTimeseriesGroup As atcTimeseriesGroup = atcDataManager.UserSelectData("Select Data For Duration Hydrograph")
            If lTimeseriesGroup.Count > 0 Then
                Dim lfrmDHControl As New frmDurationHydrographControl(lTimeseriesGroup)
                If pMapWin IsNot Nothing AndAlso pMapWin.ApplicationInfo.FormIcon IsNot Nothing Then lfrmDHControl.Icon = pMapWin.ApplicationInfo.FormIcon
                lfrmDHControl.Show()
            End If
        End If
    End Sub

End Class
