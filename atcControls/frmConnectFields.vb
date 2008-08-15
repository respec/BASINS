Imports atcUtility

Public Class frmConnectFields
    Public Function Connections() As atcCollection
        Return Me.ctlConnectFields.Connections
    End Function

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
    End Sub
End Class