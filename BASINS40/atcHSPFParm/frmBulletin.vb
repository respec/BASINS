Public Class frmBulletin

    Public Sub New(ByVal aSummary As String)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        txtDataSummary.Text = aSummary
    End Sub

    Public Sub ClearSelection()
        txtDataSummary.SelectionStart = 0
    End Sub

    Public Sub ShowTimedDialog(ByVal aTimeInterval As Integer)
        Dim lTimer As New System.Windows.Forms.Timer
        lTimer.Interval = aTimeInterval
        AddHandler lTimer.Tick, AddressOf Timer_Tick
        lTimer.Enabled = True

        Me.Show()
    End Sub

    Private Sub Timer_Tick(ByVal Sender As Object, ByVal aEventArg As EventArgs)
        txtDataSummary.Text = ""
        Me.Close()
    End Sub
End Class