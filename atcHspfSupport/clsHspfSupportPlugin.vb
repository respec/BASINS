Imports atcData
Public Class clsHspfSupportPlugin
    Inherits atcDataTool

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Analysis::Compare"
        End Get
    End Property

    Public Overrides Function Show() As Object
        Dim lFrm As New frmCompare
        lFrm.Show()
        Return lFrm
    End Function

End Class
