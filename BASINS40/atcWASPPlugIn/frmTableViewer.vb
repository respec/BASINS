Public Class frmTableViewer
    Private dgv As WRDB.Controls.DGVEditor

    Private Sub frmTableViewer_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        SaveWindowPos(REGAPPNAME, Me)
    End Sub

    Private Sub frmTableViewer_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GetWindowPos(REGAPPNAME, Me)
        'dgv = New WRDB.Controls.DGVEditor(dgTable)
        'cannot display blob column properly in grid; add a link column for object type to perform operations
        With dgTable
            For c As Integer = 0 To .Columns.Count - 1
                If .Columns(c).Name = "Object_Type" Then
                    dgv.SetColumnFormat(WRDB.Controls.DGVEditor.enumColumnTypes.Link, c)
                End If
            Next
        End With
    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        dgv = New WRDB.Controls.DGVEditor(dgTable)
    End Sub
End Class