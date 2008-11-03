Public Class frmTime

    Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.MinimumSize = Me.Size
        Me.Icon = pIcon

        With agdMet
            .Source = New atcControls.atcGridSource
            .Clear()
            .AllowHorizontalScrolling = False
            .AllowNewValidValues = True
            .Visible = True
        End With

        With pUCI.GlobalBlock

            txtStartYear.Text = .SDate(0)
            txtStartMonth.Text = .SDate(1)
            txtStartDay.Text = .SDate(2)
            txtStartHour.Text = .SDate(3)
            txtStartMinute.Text = .SDate(4)

            txtEndYear.Text = .EDate(0)
            txtEndMonth.Text = .EDate(1)
            txtEndDay.Text = .EDate(2)
            txtEndHour.Text = .EDate(3)
            txtEndMinute.Text = .EDate(4)

        End With

    End Sub

    Private Sub Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel.Click
        Me.Dispose()
    End Sub

    Private Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        Dim lfrmAddMet As New frmAddMet
        lfrmAddMet.ShowDialog()
    End Sub
End Class