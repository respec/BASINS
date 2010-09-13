Imports atcData
Imports atcUtility

Public Class clsDurationComparePlugin
    Inherits atcDataPlugin

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Analysis::Duration/Compare"
        End Get
    End Property

    <CLSCompliant(False)> _
    Public Overrides Sub Initialize(ByVal aMapWin As MapWindow.Interfaces.IMapWin, _
                                    ByVal aParentHandle As Integer)

        pMapWin = aMapWin
        pMapWinWindowHandle = aParentHandle
        atcDataManager.MapWindow = aMapWin

        atcDataManager.AddMenuIfMissing(atcDataManager.AnalysisMenuName, "", atcDataManager.AnalysisMenuString, atcDataManager.FileMenuName)
        'pMenusAdded.Add(atcDataManager.AddMenuWithIcon(atcDataManager.AnalysisMenuName & "_Duration", atcDataManager.AnalysisMenuName, "Duration", Me.Icon, , , True))
        'pMenusAdded.Add(atcDataManager.AddMenuWithIcon(atcDataManager.AnalysisMenuName & "_Compare", atcDataManager.AnalysisMenuName, "Compare", Me.Icon, , , True))
        pMenusAdded.Add(atcDataManager.AddMenuWithIcon(atcDataManager.AnalysisMenuName & "_DurationCompare", atcDataManager.AnalysisMenuName, "Duration/Compare", Me.Icon, , , True))
        pMenusAdded.Add(atcDataManager.AddMenuWithIcon(atcDataManager.AnalysisMenuName & "_DurationHydrograph", atcDataManager.AnalysisMenuName, "Duration Hydrograph", Me.Icon, , , True))
    End Sub

    'Public Overrides Sub ItemClicked(ByVal aItemName As String, ByRef aHandled As Boolean)
    '    Dim lFrmClassLimts As frmAnalysis = Nothing
    '    Dim lFrm As Object = Nothing 'Windows.Forms.Form
    '    Select Case aItemName
    '        Case atcDataManager.AnalysisMenuName & "_Duration"
    '            lFrm = New frmDuration
    '        Case atcDataManager.AnalysisMenuName & "_Compare"
    '            lFrm = New frmCompare
    '    End Select

    '    If lFrm IsNot Nothing Then
    '        Dim DisplayPlugins As ICollection = atcDataManager.GetPlugins(GetType(atcDataDisplay))
    '        For Each lDisp As atcDataDisplay In DisplayPlugins
    '            Dim lMenuText As String = lDisp.Name
    '            If lMenuText.StartsWith("Analysis::") Then lMenuText = lMenuText.Substring(10)
    '            lFrm.mnuAnalysis.MenuItems.Add(lMenuText, New EventHandler(AddressOf lFrm.mnuAnalysis_Click))
    '        Next
    '        If pMapWin IsNot Nothing AndAlso pMapWin.ApplicationInfo.FormIcon IsNot Nothing Then lFrm.Icon = pMapWin.ApplicationInfo.FormIcon
    '        Dim lTimeseriesGroup As atcTimeseriesGroup = atcDataManager.UserSelectData("Select Data For " & aItemName.Substring(atcDataManager.AnalysisMenuName.Length + 1))
    '        If lTimeseriesGroup.Count > 0 Then
    '            lFrmClassLimts = New frmAnalysis(lTimeseriesGroup, lFrm)
    '            lFrmClassLimts.Show()
    '            'lFrm.Initialize(lTimeseriesGroup)
    '            'lFrm.Show()
    '        End If
    '    End If
    'End Sub

    Public Overrides Sub ItemClicked(ByVal aItemName As String, ByRef aHandled As Boolean)
        Dim lFrmClassLimits As frmAnalysis = Nothing

        If aItemName = atcDataManager.AnalysisMenuName & "_DurationCompare" Then
            Dim lTimeseriesGroup As atcTimeseriesGroup = atcDataManager.UserSelectData("Select Data For " & aItemName.Substring(atcDataManager.AnalysisMenuName.Length + 1))
            If lTimeseriesGroup.Count > 0 Then
                lFrmClassLimits = New frmAnalysis(lTimeseriesGroup)
                If pMapWin IsNot Nothing AndAlso pMapWin.ApplicationInfo.FormIcon IsNot Nothing Then lFrmClassLimits.Icon = pMapWin.ApplicationInfo.FormIcon
                lFrmClassLimits.Show()
            End If
        ElseIf aItemName = atcDataManager.AnalysisMenuName & "_DurationHydrograph" Then
            Dim lTimeseriesGroup As atcTimeseriesGroup = atcDataManager.UserSelectData("Select Data For " & aItemName.Substring(atcDataManager.AnalysisMenuName.Length + 1))
            If lTimeseriesGroup.Count > 0 Then
                Dim lfrmDHControl As New frmDurationHydrographControl(lTimeseriesGroup)
                lfrmDHControl.Show()
            End If
        End If
    End Sub

End Class
