Imports atcData

Public Class clsHspfSupportPlugin
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
        pMenusAdded.Add(atcDataManager.AddMenuWithIcon(atcDataManager.AnalysisMenuName & "_Duration", atcDataManager.AnalysisMenuName, "Duration", Me.Icon, , , True))
        'pMenusAdded.Add(atcDataManager.AddMenuWithIcon(atcDataManager.AnalysisMenuName & "_Compare", atcDataManager.AnalysisMenuName, "Compare", Me.Icon, , , True))
    End Sub

    Public Overrides Sub ItemClicked(ByVal aItemName As String, ByRef aHandled As Boolean)
        Dim lFrm As Object = Nothing 'Windows.Forms.Form
        Select Case aItemName
            Case atcDataManager.AnalysisMenuName & "_Duration" : lFrm = New frmDuration
                'Case atcDataManager.AnalysisMenuName & "_Compare" : lFrm = New frmCompare
        End Select

        If lFrm IsNot Nothing Then
            If pMapWin IsNot Nothing AndAlso pMapWin.ApplicationInfo.FormIcon IsNot Nothing Then lFrm.Icon = pMapWin.ApplicationInfo.FormIcon
            Dim lTimeseriesGroup As atcTimeseriesGroup = atcDataManager.UserSelectData("Select Data For " & aItemName.Substring(atcDataManager.AnalysisMenuName.Length + 1))
            If lTimeseriesGroup.Count > 0 Then
                lFrm.Initialize(lTimeseriesGroup)
                lFrm.Show()
            End If
        End If

    End Sub
End Class
