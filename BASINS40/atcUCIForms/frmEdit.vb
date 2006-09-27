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
End Class