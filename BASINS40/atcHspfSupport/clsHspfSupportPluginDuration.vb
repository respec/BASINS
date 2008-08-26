Imports atcData

Public Class clsHspfSupportPluginDuration
    Inherits atcDataTool

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Analysis::Duration"
        End Get
    End Property

    Public Overrides Function Show() As Object
        Dim lFrmDuration As New frmDuration
        lFrmDuration.Show()
        Return lFrmDuration
    End Function

End Class
