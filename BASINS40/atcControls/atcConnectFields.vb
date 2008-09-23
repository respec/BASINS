Imports MapWinUtility
Imports atcUtility

Public Class atcConnectFields
    Private pConnectionDelimeter As String = "<->" 'TODO: allow this to change

    Public Function AddConnection(ByVal aConnectionString As String, Optional ByVal aQuiet As Boolean = True) As Boolean
        Dim lString() As String = aConnectionString.Split(" ")
        Dim lProblems As String = ""
        If lString.GetUpperBound(0) < 2 Then
            lProblems &= "Bad Connection String '" & aConnectionString & "'" & vbCrLf
        Else
            If Not lstSource.Items.Contains(lString(0)) Then
                'TODO: add an option to add as a new source?
                lProblems &= "Bad Source Field '" & lString(0) & "'" & vbCrLf
            End If
            If Not lstTarget.Items.Contains(lString(2)) Then
                lProblems &= "Bad Target Field '" & lString(2) & "'" & vbCrLf
            End If
            If lstConnections.Items.Contains(aConnectionString) Then
                lProblems &= "Connection '" & aConnectionString & "' already exists" & vbCrLf
            End If
        End If
        If lProblems.Length = 0 Then
            lstConnections.Items.Add(aConnectionString)
            Return True
        Else
            If aQuiet Then
                Logger.Dbg(lProblems.Remove(lProblems.Length - 2))
            Else
                Logger.Msg(lProblems, "Connection Add Problem")
            End If
            Return False
        End If
    End Function

    Private Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        If lstSource.SelectedIndex > -1 AndAlso lstTarget.SelectedIndex > -1 Then
            AddConnection(lstSource.SelectedItem.ToString & " " & pConnectionDelimeter & " " & lstTarget.SelectedItem.ToString, False)
        ElseIf lstSource.SelectedIndex > -1 Then
            Logger.Msg("Select a Target Field", "Connection Add Problem")
        ElseIf lstTarget.SelectedIndex > -1 Then
            Logger.Msg("Select a Source Field", "Connection Add Problem")
        Else
            Logger.Msg("Select Source and Target Fields", "Connection Add Problem")
        End If
    End Sub

    Private Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If lstConnections.Items.Count = 0 Then
            Logger.Msg("No Connections to Delete", "Connection Delete Problem")
        ElseIf lstConnections.SelectedIndex > -1 Then
            lstConnections.Items.RemoveAt(lstConnections.SelectedIndex)
        Else
            Logger.Msg("Select Connection to Delete", "Connection Delete Problem")
        End If
    End Sub

    Private Sub btnLoad_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLoad.Click
        Dim lFileSelectDialog As New OpenFileDialog
        With lFileSelectDialog
            .Title = "Load Connections"
            If .ShowDialog() = DialogResult.OK Then
                LoadConnections(.FileName)
            End If
        End With
    End Sub

    Public Function LoadConnections(ByVal aFileName As String) As Boolean
        Try
            For Each lString As String In LinesInFile(aFileName)
                If Not AddConnection(lString) Then
                    If Logger.Message("Bad Connection '" & lString & "'" & vbCrLf & "Continue?", "Load Problem", MessageBoxButtons.YesNo, Nothing, DialogResult.No) = DialogResult.No Then
                        Return False
                    End If
                End If
            Next
        Catch lEx As Exception
            Logger.Msg("Problem Loading '" & aFileName & "'" & vbCrLf & lEx.Message, "Load Problem")
            Return False
        End Try
        Return True
    End Function

    Private Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        lstConnections.Items.Clear()
    End Sub

    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim lFileSelectDialog As New SaveFileDialog
        With lFileSelectDialog
            .Title = "Save Connections"
            If .ShowDialog = DialogResult.OK Then
                Dim lStr As String = ""
                For Each lItem As Object In lstConnections.Items
                    lStr &= lItem.ToString & vbCrLf
                Next
                SaveFileString(.FileName, lStr)
            End If
        End With
    End Sub

    Public Function Connections() As atcCollection
        Dim lConnections As New atcCollection
        For Each lItem As Object In Me.lstConnections.Items
            Dim lString() As String = lItem.ToString.Split(" ")
            lConnections.Add(lString(0), lString(2))
        Next
        Return lConnections
    End Function

    Private Sub atcConnectFields_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        lstSource.Width = (Me.Width / 2) - 30
        lstTarget.Left = (Me.Width / 2) + 10
        lstTarget.Width = (Me.Width / 2) - 30
        lblTarget.Left = lstTarget.Left
        lstSource.Height = (Me.Height / 2) - 64
        lstTarget.Height = (Me.Height / 2) - 64
        lstConnections.Height = (Me.Height / 2) - 80
        lstConnections.Top = lstSource.Height + 150
        lblConnections.Top = lstConnections.Top - 20
        btnAdd.Top = lblConnections.Top - 40
        btnClear.Top = lblConnections.Top - 40
        btnDelete.Top = lblConnections.Top - 40
        btnLoad.Top = lblConnections.Top - 40
        btnSave.Top = lblConnections.Top - 40
    End Sub
End Class
