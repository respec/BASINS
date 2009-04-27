Friend Class frmSediment
    Inherits System.Windows.Forms.Form

    Private Sub cmdCancel_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdCancel.Click
        Me.Close()
    End Sub
    Private Sub cmdSave_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSave.Click
        WritetoDict(Me)
        SaveDict()
        Close()
    End Sub

    Private Sub frmSediment_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        Filer()
        LoadForm(Me)
    End Sub
End Class