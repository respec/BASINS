Public Class frmAddExpert

    Dim pCheckedRadioIndex As Integer

    Sub New(ByVal aCheckedRadioIndex As Integer)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.Icon = pIcon
        Me.MinimumSize = Me.Size
        Me.MaximumSize = Me.Size

        pCheckedRadioIndex = aCheckedRadioIndex

        Select Case pCheckedRadioIndex
            Case 1 'Hydrology Calibration
                ListBox1.Size = New Size(350, 173)
                ListBox2.Visible = False
                txtDefine.Visible = False
                optH.Visible = False
                optD.Visible = False
                lblGroup.Visible = False
            Case 2 'Flow
                ListBox1.Size = New Size(350, 173)
                ListBox2.Visible = False
                txtDefine.Visible = False
                lblGroup.Visible = False
            Case 3 'AQUATOX Linkage
                ListBox1.Size = New Size(350, 173)
                ListBox2.Visible = False
                txtDefine.Visible = False
                lblGroup.Visible = False
        End Select



    End Sub


    Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        Me.Dispose()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.Dispose()
    End Sub
End Class