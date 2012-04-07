Imports atcUtility
Imports System.Data

Public Class frmWatershedDetails
    Friend pWatershedIDs As atcCollection
    Friend pCurrentIndex As Integer
    Friend pGridSource As atcControls.atcGridSource

    Public Sub InitializeUI(ByVal aIds As atcCollection, ByVal aFirstIndex As Integer, ByVal aGridSource As atcControls.atcGridSource)
        pWatershedIDs = aIds
        If aFirstIndex = -1 Then
            aFirstIndex = 1
        End If
        pCurrentIndex = aFirstIndex
        pGridSource = aGridSource
        LoadWatershed(pWatershedIDs(pCurrentIndex - 1))
    End Sub

    Private Sub LoadWatershed(ByVal aWatershedID As Integer)
        With pGridSource
            For lRow As Integer = 1 To pGridSource.Rows - 1
                If .CellValue(lRow, 0) = aWatershedID Then
                    TextBox1.Text = .CellValue(lRow, 0)  'ID
                    TextBox2.Text = .CellValue(lRow, 1)  'Project Name
                    TextBox3.Text = .CellValue(lRow, 2)  'HUC
                    TextBox4.Text = .CellValue(lRow, 3)  'Location
                    TextBox5.Text = .CellValue(lRow, 4)  'Drainage Area
                    TextBox6.Text = .CellValue(lRow, 5)  'Comments
                    TextBox7.Text = .CellValue(lRow, 6)  'Physiographic Setting
                    TextBox8.Text = .CellValue(lRow, 7)  'Weather Regime
                    Exit For
                End If
            Next
        End With
    End Sub

    Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        'todo: save back to database?
        Me.Close()
    End Sub

    Private Sub cmdFirst_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdFirst.Click
        pCurrentIndex = 1
        LoadWatershed(pWatershedIDs(0))
    End Sub

    Private Sub cmdPrev_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPrev.Click
        If pCurrentIndex > 1 Then
            pCurrentIndex -= 1
            LoadWatershed(pWatershedIDs(pCurrentIndex - 1))
        End If
    End Sub

    Private Sub cmdNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdNext.Click
        If pCurrentIndex < pWatershedIDs.Count Then
            pCurrentIndex += 1
            LoadWatershed(pWatershedIDs(pCurrentIndex - 1))
        End If
    End Sub

    Private Sub cmdLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdLast.Click
        pCurrentIndex = pWatershedIDs.Count
        LoadWatershed(pWatershedIDs(pWatershedIDs.Count - 1))
    End Sub
End Class