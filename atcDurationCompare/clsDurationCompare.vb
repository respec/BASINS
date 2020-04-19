Imports atcData
Imports atcUtility

Public Class clsDurationComparePlugin
    Inherits atcDataDisplay

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Analysis::USGS Surface Water Statistics (SWSTAT)::Duration/Compare"
        End Get
    End Property
#If GISProvider = "DotSpatial" Then
    Public Function ShowDH(ByVal aTimeseriesGroup As atcData.atcDataGroup) As Object
        Dim lTimeseriesGroup = aTimeseriesGroup
        If aTimeseriesGroup Is Nothing OrElse aTimeseriesGroup.Count = 0 Then
            lTimeseriesGroup = atcDataManager.UserSelectData("Select Data For Duration Hydrograph",
                                          Nothing, Nothing, True, True, Me.Icon)
        End If
        If lTimeseriesGroup.Count > 0 Then
            Dim lfrmDHControl As New frmDurationHydrographControl(lTimeseriesGroup)
            lfrmDHControl.Show()
            Return lfrmDHControl
        End If
        Return Nothing
    End Function
#Else
    <CLSCompliant(False)> _
    Public Overrides Sub Initialize(ByVal aMapWin As MapWindow.Interfaces.IMapWin, _
                                    ByVal aParentHandle As Integer)

        pMapWin = aMapWin
        pMapWinWindowHandle = aParentHandle
        atcDataManager.MapWindow = aMapWin

        'Dim lSWSTATMenuName As String = atcDataManager.AnalysisMenuName & "_USGS Surface Water Statistics (SWSTAT)"

        atcDataManager.AddMenuIfMissing(atcDataManager.AnalysisMenuName, "", atcDataManager.AnalysisMenuString, atcDataManager.FileMenuName)
        'atcDataManager.AddMenuWithIcon(lSWSTATMenuName, atcDataManager.AnalysisMenuName, "USGS Surface Water Statistics (SWSTAT)", Me.Icon)

        pMenusAdded.Add(atcDataManager.AddMenuWithIcon(atcDataManager.AnalysisMenuName & "_DurationCompare", atcDataManager.AnalysisMenuName, "Duration/Compare", Me.Icon, , , True))
        pMenusAdded.Add(atcDataManager.AddMenuWithIcon(atcDataManager.AnalysisMenuName & "_DurationHydrograph", atcDataManager.AnalysisMenuName, "Duration Hydrograph", Me.Icon, , , True))
    End Sub

    Public Overrides ReadOnly Property Icon() As System.Drawing.Icon
        Get
            Dim lResources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAnalysis))
            Return CType(lResources.GetObject("$this.Icon"), System.Drawing.Icon)
        End Get
    End Property

    Public Overrides Sub ItemClicked(ByVal aItemName As String, ByRef aHandled As Boolean)
         If aItemName = atcDataManager.AnalysisMenuName & "_DurationCompare" Then
            Dim lTimeseriesGroup As atcTimeseriesGroup = _
            atcDataManager.UserSelectData("Select Data For Duration/Compare", _
                                          Nothing, Nothing, True, True, Me.Icon)
            If lTimeseriesGroup.Count > 0 Then
                Dim lFrmAnalysis As frmAnalysis = New frmAnalysis(lTimeseriesGroup)
                lFrmAnalysis.Show()
            End If
        ElseIf aItemName = atcDataManager.AnalysisMenuName & "_DurationHydrograph" Then
            Dim lTimeseriesGroup As atcTimeseriesGroup = _
            atcDataManager.UserSelectData("Select Data For Duration Hydrograph", _
                                          Nothing, Nothing, True, True, Me.Icon)
            If lTimeseriesGroup.Count > 0 Then
                Dim lfrmDHControl As New frmDurationHydrographControl(lTimeseriesGroup)
                lfrmDHControl.Show()
            End If
        End If
    End Sub
#End If

    Public Overrides Function Show(ByVal aTimeseriesGroup As atcData.atcDataGroup) As Object
        Dim lTimeseriesGroup = aTimeseriesGroup
        If aTimeseriesGroup Is Nothing OrElse aTimeseriesGroup.Count = 0 Then
            lTimeseriesGroup = atcDataManager.UserSelectData("Select Data For Duration/Compare",
                                          Nothing, Nothing, True, True, Me.Icon)
        End If
        If lTimeseriesGroup IsNot Nothing AndAlso lTimeseriesGroup.Count > 0 Then
            Dim lFrmAnalysis As frmAnalysis = New frmAnalysis(lTimeseriesGroup)
            lFrmAnalysis.Show()
            Return lFrmAnalysis
        End If
        Return Nothing
    End Function
End Class
