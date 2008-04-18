Imports atcData

Public Class atcClimateAssessmentToolPlugin
    Inherits atcData.atcDataTool

    Private Shared pForm As frmCAT
    'Private Shared pXML As String

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Analysis::Climate Assessment Tool"
        End Get
    End Property

    Public Overrides Function Show() As Object
        pForm = New frmCAT
        pForm.Initialize(Me)
        Return pForm
    End Function

    <CLSCompliant(False)> _
    Public Overrides Sub Initialize(ByVal aMapWin As MapWindow.Interfaces.IMapWin, _
                                    ByVal aParentHandle As Integer)
        MyBase.Initialize(aMapWin, aParentHandle)
        g_MapWin = aMapWin
    End Sub

    'Public Property XML() As String
    '    Get
    '        Return pXML
    '    End Get
    '    Set(ByVal newValue As String)
    '        pXML = newValue
    '    End Set
    'End Property

    'Public Overrides Sub ProjectLoading(ByVal ProjectFile As String, ByVal SettingsString As String)
    '    XML = SettingsString
    '    If Not pForm Is Nothing Then pForm.XML = XML
    'End Sub

    'Public Overrides Sub ProjectSaving(ByVal ProjectFile As String, ByRef SettingsString As String)
    '    SettingsString = XML
    'End Sub

End Class
