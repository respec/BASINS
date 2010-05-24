Imports MapWinUtility

Public Class atcManagedList
    Public Event DefaultRequested()
    Private pRegistryName As String = "atcManagedList"

    Public Property CurrentValues() As Double()
        Get
            Dim lArray() As Double
            Dim lCollection As New ArrayList
            If lstValues.SelectedItems.Count > 0 Then
                For lIndex As Integer = 0 To lstValues.SelectedItems.Count - 1
                    If IsNumeric(lstValues.SelectedItems(lIndex)) Then
                        lCollection.Add(CDbl(lstValues.SelectedItems(lIndex)))
                    End If
                Next
            Else
                For lIndex As Integer = 0 To lstValues.Items.Count - 1
                    If IsNumeric(lstValues.Items(lIndex)) Then
                        lCollection.Add(CDbl(lstValues.Items(lIndex)))
                    End If
                Next
            End If
            ReDim lArray(lCollection.Count - 1)
            For lIndex As Integer = 0 To lCollection.Count - 1
                lArray(lIndex) = lCollection.Item(lIndex)
            Next
            Return lArray
        End Get
        Set(ByVal values As Double())
            lstValues.Items.Clear()
            For Each lNumber As Double In values
                Dim lLabel As String = Format(lNumber, "0.####")
                lstValues.Items.Add(lLabel)
            Next
        End Set
    End Property

    Private Sub btnAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAll.Click
        For index As Integer = 0 To lstValues.Items.Count - 1
            lstValues.SetSelected(index, True)
        Next
    End Sub

    Private Sub btnNone_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNone.Click
        For index As Integer = 0 To lstValues.Items.Count - 1
            lstValues.SetSelected(index, False)
        Next
    End Sub

    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        If IsNumeric(txtAdd.Text) Then
            Try
                Dim lIndex As Integer = 0
                Dim lNewValue As Double = CDbl(txtAdd.Text)
                While lIndex < lstValues.Items.Count AndAlso CDbl(lstValues.Items(lIndex)) < lNewValue
                    lIndex += 1
                End While
                lstValues.Items.Insert(lIndex, txtAdd.Text)
                lstValues.SetSelected(lIndex, True)
                SaveList()
            Catch ex As Exception
                Logger.Dbg("Exception adding Recurrence '" & txtAdd.Text & "': " & ex.Message)
            End Try
        Else
            Logger.Msg("Type a return period to add in the blank, then press the add button again")
        End If
    End Sub

    Private Sub btnRemove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRemove.Click
        Dim lRemoveThese As New ArrayList
        Dim lIndex As Integer
        If lstValues.SelectedIndices.Count > 0 Then
            For lIndex = lstValues.SelectedIndices.Count - 1 To 0 Step -1
                lRemoveThese.Add(lstValues.SelectedIndices.Item(lIndex))
            Next
        Else
            For lIndex = lstValues.Items.Count - 1 To 0 Step -1
                If lstValues.Items(lIndex) = txtAdd.Text Then
                    lRemoveThese.Add(lIndex)
                End If
            Next
        End If

        For Each lIndex In lRemoveThese
            lstValues.Items.RemoveAt(lIndex)
        Next
        SaveList()
    End Sub

    Private Sub btnDefault_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDefault.Click
        RaiseEvent DefaultRequested()
    End Sub

    Public Sub LoadListSettingsOrDefaults()
        Dim lAvailableArray As String(,) = GetAllSettings(pRegistryName, RegistrySection)
        Dim lSelected As New ArrayList
        lstValues.Items.Clear()

        If Not lAvailableArray Is Nothing AndAlso lAvailableArray.Length > 0 Then
            Try
                For lIndex As Integer = 0 To lAvailableArray.GetUpperBound(0)
                    lstValues.Items.Add(lAvailableArray(lIndex, 0))
                    If lAvailableArray(lIndex, 1) = "True" Then
                        lstValues.SetSelected(lstValues.Items.Count - 1, True)
                    End If
                Next
            Catch e As Exception
                Logger.Dbg("Error retrieving saved settings: " & e.Message)
            End Try
        Else
            RaiseEvent DefaultRequested()
        End If
    End Sub

    Private Function RegistrySection() As String
        Dim lArgName As String = Me.Name
        If Me.Tag IsNot Nothing Then
            If Me.Tag.ToString.Length > 0 Then
                lArgName = Me.Tag.ToString
            End If
        End If
        Return "List." & lArgName
    End Function

    Private Sub SaveList()
        Dim lRegistrySection As String = RegistrySection()
        SaveSetting(pRegistryName, lRegistrySection, "dummy", "")
        DeleteSetting(pRegistryName, lRegistrySection)
        For lIndex As Integer = 0 To lstValues.Items.Count - 1
            SaveSetting(pRegistryName, lRegistrySection, lstValues.Items(lIndex), lstValues.SelectedIndices.Contains(lIndex))
        Next
    End Sub

    Private Sub atcManagedList_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
        SaveList()
    End Sub

    Private Sub atcManagedList_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        LoadListSettingsOrDefaults()
    End Sub

End Class
