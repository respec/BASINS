Public Class frmDataSummary
    Public Sub New(ByVal aSummary As String)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        txtDataSummary.Text = aSummary
    End Sub
End Class