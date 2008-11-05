Imports System.Drawing
Imports MapWinUtility
Imports atcUCI
Imports atcControls
Imports atcUCIForms
Imports System.ComponentModel

Public Class frmPoint

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        Me.Icon = pIcon
        lstSources.SelectionMode = SelectionMode.One
        ExpandedView(False)

        'temporarty fill of list.
        For lOper As Integer = 1 To 10
            lstSources.Items.Add(lOper)
        Next

        grpDetails.Text = "Details of " & lstSources.SelectedItem

    End Sub

    Private Sub ExpandedView(ByVal aExpand As Boolean)
        If aExpand Then
            Me.Size = New Size(800, 450)
            cmdDetailsHide.Visible = True
            cmdDetailsShow.Visible = False
            grpDetails.Visible = True
        Else
            Me.Size = New Size(280, 450)
            cmdDetailsHide.Visible = False
            cmdDetailsShow.Visible = True
            grpDetails.Visible = False
        End If

    End Sub

    Private Sub lstSources_SelectionChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstSources.SelectedIndexChanged
        grpDetails.Text = "Details of " & lstSources.SelectedItem
    End Sub
    Private Sub cmdShowDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDetailsShow.Click
        ExpandedView(True)
    End Sub

    Private Sub cmdDetailsHide_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDetailsHide.Click
        ExpandedView(False)
    End Sub

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Me.Dispose()
    End Sub

    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
        Me.Dispose()
    End Sub
End Class