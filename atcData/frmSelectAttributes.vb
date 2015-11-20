Imports atcUtility

Public Class frmSelectAttributes

    Private pAllAttributes As Generic.List(Of String)
    Private pCalculatedAttributes As Generic.List(Of String)
    Private pNotCalculatedAttributes As Generic.List(Of String)

    Public Function AskUser(ByVal aTsGroup As atcTimeseriesGroup, ByVal aSelected As Generic.List(Of String)) As Boolean
        If aTsGroup Is Nothing Then
            Throw New Exception("Nothing Available in frmSelectAttributes::AskUser")
        ElseIf aTsGroup.Count < 1 Then
            Throw New Exception("Zero items Available in frmSelectAttributes::AskUser")
        ElseIf aSelected Is Nothing Then
            Throw New Exception("Selected = Nothing - Must be assigned before calling frmSelectAttributes::AskUser")
        End If

        pCalculatedAttributes = New Generic.List(Of String)
        pNotCalculatedAttributes = New Generic.List(Of String)

        Dim lAllDefinitions As atcCollection = atcDataAttributes.AllDefinitions
        Dim lName As String
        If lAllDefinitions IsNot Nothing Then
            For Each lDef As atcAttributeDefinition In lAllDefinitions
                If lDef.Displayable Then
                    Dim lInclude As Boolean = False
                    lName = lDef.Name
                    If aTsGroup.Count < 200 Then
                        For Each lTs As atcTimeseries In aTsGroup
                            If lTs.Attributes.ContainsAttribute(lName) Then
                                lInclude = True
                                Exit For
                            End If
                        Next
                    Else
                        lInclude = True
                    End If
                    If lInclude Then
                        If lDef.Calculated Then
                            pCalculatedAttributes.Add(lName)
                        Else
                            pNotCalculatedAttributes.Add(lName)
                        End If
                    End If
                End If
            Next
            pCalculatedAttributes.Sort()
            pNotCalculatedAttributes.Sort()
            pAllAttributes = New Generic.List(Of String)
            pAllAttributes.AddRange(pCalculatedAttributes)
            pAllAttributes.AddRange(pNotCalculatedAttributes)
            pAllAttributes.Sort()
            RefreshAvailable()
            For Each lSelected As String In aSelected
                ctlSelect.RightItem(ctlSelect.RightItems.Count) = lSelected
            Next
            If Me.ShowDialog() = Windows.Forms.DialogResult.OK Then
                aSelected.Clear()
                For Each lName In ctlSelect.RightItems
                    aSelected.Add(lName)
                Next
                Return True
            Else
                Return False
            End If
        End If
    End Function

    Private Sub chkCalculated_CheckedChanged(sender As Object, e As EventArgs) Handles chkCalculated.CheckedChanged, chkNotCalculated.CheckedChanged
        RefreshAvailable()
    End Sub

    Private Sub RefreshAvailable()
        ctlSelect.ClearLeft()
        If chkCalculated.Checked AndAlso pCalculatedAttributes IsNot Nothing AndAlso chkNotCalculated.Checked AndAlso pNotCalculatedAttributes IsNot Nothing Then
            For Each lItem As String In pAllAttributes
                ctlSelect.LeftItemFastAdd(lItem)
            Next
        ElseIf chkCalculated.Checked AndAlso pCalculatedAttributes IsNot Nothing Then
            For Each lItem As String In pCalculatedAttributes
                ctlSelect.LeftItemFastAdd(lItem)
            Next
        ElseIf chkNotCalculated.Checked AndAlso pNotCalculatedAttributes IsNot Nothing Then
            For Each lItem As String In pNotCalculatedAttributes
                ctlSelect.LeftItemFastAdd(lItem)
            Next
        End If
    End Sub
End Class