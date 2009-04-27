Friend Class frmAbout
    Inherits System.Windows.Forms.Form

    Private Sub cmdCancel_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdCancel.Click
        Me.Close()
    End Sub

    Private Sub frmAbout_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        lblVersion.Text = "Version: " & My.Application.Info.Version.ToString
        Dim strContribution As String
        strContribution = "Designed and Developed By: " & vbNewLine & _
            "EPA, National Exporsure Research Laboratory:" & vbNewLine & _
            Space(7) & "Robert Ambrose (ambrose.robert@epa.gov)" & vbNewLine & _
            Space(7) & "Tim Wool and Steve Kraemer" & vbNewLine & _
            "Tetra Tech, Inc.:" & vbNewLine & _
            Space(7) & "Ting Dai (ting.dai@tetratech-ffx.com)" & vbNewLine & _
            Space(7) & "Khalid Alvi, Haihong Yang, Sabu Paul, Jenny Zhen" & vbNewLine & _
            Space(7) & "and Henry Manguerra" & vbNewLine & _
            "Clayton Engineering (adapted to BASINS for AquaTerra):" & vbNewLine & _
            Space(7) & "Lloyd Chris Wilson, Ph.D., P.E. (cwilson@claytoneng.pro)"
        LabelContribution.Text = strContribution
    End Sub
End Class