Public Class frmEdit
    Private pEditControl As Windows.Forms.Control

    Friend Property EditControl() As Windows.Forms.Control
        Get
            Return pEditControl
        End Get
        Set(ByVal aControl As Windows.Forms.Control)
            pEditControl = aControl
            panelEdit.Controls.Add(pEditControl)
            With pEditControl
                .Dock = Windows.Forms.DockStyle.Fill
            End With
        End Set
    End Property

    Private Sub cmdOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdOk.Click
        Dim lEditControl As ctlEdit = pEditControl
        lEditControl.Save()
    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
    End Sub
End Class