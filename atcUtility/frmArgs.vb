Public Class frmArgs

    Private pOk As Boolean
    Private pLeft As Integer
    Private pTop As Integer
    Private pRowHeight As Integer
    Private pLeftColumnWidth As Integer
    Private pRightColumnItems As ArrayList

    Public Function AskUser(ByVal aTitle As String, ByRef aArgs As atcCollection) As Boolean
        Text = aTitle

        pOk = False
        pLeft = btnOk.Left
        pTop = pLeft
        pRowHeight = CInt(btnOk.Height * 1.5)
        pLeftColumnWidth = btnOk.Width
        pRightColumnItems = New ArrayList

        Dim lArgIndex As Integer = 0
        While lArgIndex < aArgs.Count
            Select Case aArgs.Keys(lArgIndex).GetType.Name
                Case "String"
                    Dim lKey As String = aArgs.Keys(lArgIndex).ToString
                    Dim lArgValue As Object = GetSetting("BASINS4", aTitle, lKey, aArgs.ItemByIndex(lArgIndex).ToString)
                    If lKey.Substring(1, 1) = ":" Then
                        Select Case lKey.Substring(0, 1).ToUpper
                            Case "L" 'Just a label
                                AddLabel(lKey.Substring(2), "lbl" & lArgIndex)
                            Case "T" 'Label and text box
                                Dim lLabel As Windows.Forms.Label = AddLabel(lKey.Substring(2), "lbl" & lArgIndex)
                                If lLabel.Width > pLeftColumnWidth Then pLeftColumnWidth = lLabel.Width
                                lArgIndex += 1
                                AddTextbox(aArgs(lArgIndex).ToString, "txt" & lArgIndex)
                            Case "C" 'Label and checkbox
                                Dim lLabel As Windows.Forms.Label = AddLabel(lKey.Substring(2), "lbl" & lArgIndex)
                                If lLabel.Width > pLeftColumnWidth Then pLeftColumnWidth = lLabel.Width
                                lArgIndex += 1
                                AddCheckbox(CBool(aArgs(lArgIndex)), "chk" & lArgIndex)
                            Case Else
                                Stop
                        End Select
                    Else
                        Dim lLabel As Windows.Forms.Label = AddLabel(lKey, "lbl" & lArgIndex)
                        If lLabel.Width > pLeftColumnWidth Then pLeftColumnWidth = lLabel.Width
                        Select Case aArgs.ItemByIndex(lArgIndex).GetType.Name
                            Case "Boolean" : AddCheckbox(lArgValue, "chk" & lArgIndex)
                            Case Else : AddTextbox(lArgValue, "txt" & lArgIndex)
                        End Select
                    End If
            End Select
            lArgIndex += 1
        End While

        Me.Width = (pLeftColumnWidth + pLeft) * 4 + pLeft

        For Each lItem As Windows.Forms.Control In pRightColumnItems
            lItem.Left = pLeftColumnWidth + pLeft * 2
            lItem.Width = Me.Width - lItem.Left - pLeft
        Next

        Me.Height = pTop + Me.Height - btnOk.Top
        'pLabelClicked = "Cancel" 'If form closes without user clicking a button, default to "Cancel"
        Me.ShowDialog() 'Block until form closes
        If pOk Then
            lArgIndex = 0
            While lArgIndex < aArgs.Count
                Dim lKey As String = aArgs.Keys(lArgIndex)
                Select Case aArgs(lArgIndex).GetType.Name
                    'Case "String"
                    '    Dim lStr As String = aArgs(lArgIndex)
                    '    If lStr.Substring(1, 1) = ":" Then
                    '        Select Case lStr.Substring(0, 1).ToUpper
                    '            Case "L" 'Just a label
                    '            Case "T" 'Label and text box
                    '                'Dim lLabel As Windows.Forms.Label = Me.Controls.Item(Me.Controls.IndexOfKey("lbl" & lArgIndex))
                    '                lArgIndex += 1
                    '                Dim lTextbox As Windows.Forms.TextBox = Me.Controls.Item(Me.Controls.IndexOfKey("txt" & lArgIndex))
                    '                aArgs.ItemByIndex(lArgIndex) = lTextbox.Text
                    '            Case "C" 'Label and checkbox
                    '                'Dim lLabel As Windows.Forms.Label = AddLabel(lStr.Substring(2), "lbl" & lArgIndex)
                    '                lArgIndex += 1
                    '                Dim lCheckbox As Windows.Forms.CheckBox = Me.Controls.Item(Me.Controls.IndexOfKey("chk" & lArgIndex))
                    '                aArgs.ItemByIndex(lArgIndex) = lCheckbox.Checked
                    '            Case Else
                    '                Stop
                    '        End Select
                    '    Else
                    '        Select Case aArgs.ItemByIndex(lArgIndex).GetType.Name
                    '            Case "Boolean"
                    '                Dim lCheckbox As Windows.Forms.CheckBox = Me.Controls.Item(Me.Controls.IndexOfKey("chk" & lArgIndex))
                    '                aArgs.ItemByIndex(lArgIndex) = lCheckbox.Checked
                    '            Case Else
                    '                Dim lTextbox As Windows.Forms.TextBox = Me.Controls.Item(Me.Controls.IndexOfKey("txt" & lArgIndex))
                    '                aArgs.ItemByIndex(lArgIndex) = lTextbox.Text
                    '        End Select
                    '    End If
                    Case "Boolean"
                        Dim lCheckbox As Windows.Forms.CheckBox = Me.Controls.Item(Me.Controls.IndexOfKey("chk" & lArgIndex))
                        aArgs.ItemByIndex(lArgIndex) = lCheckbox.Checked
                    Case Else
                        Dim lTextbox As Windows.Forms.TextBox = Me.Controls.Item(Me.Controls.IndexOfKey("txt" & lArgIndex))
                        aArgs.ItemByIndex(lArgIndex) = lTextbox.Text
                End Select
                SaveSetting("BASINS4", aTitle, lKey, aArgs.ItemByIndex(lArgIndex))
                lArgIndex += 1
            End While
        End If
        Return pOk
    End Function

    Private Function AddLabel(ByVal aText As String, ByVal aName As String) As Windows.Forms.Label
        Dim lAddLabel As New Windows.Forms.Label
        With lAddLabel
            .AutoSize = True
            .Text = aText
            .Name = aName
            .Top = pTop
            pTop += pRowHeight
            .Left = pLeft
            .Anchor = Windows.Forms.AnchorStyles.Left Or Windows.Forms.AnchorStyles.Top
            Me.Controls.Add(lAddLabel)
        End With
        Return lAddLabel
    End Function

    Private Function AddTextbox(ByVal aText As String, ByVal aName As String) As Windows.Forms.TextBox
        Dim lAddTextbox As New Windows.Forms.TextBox
        With lAddTextbox
            .Text = aText
            .Name = aName
            .Top = pTop - pRowHeight
            .Anchor = Windows.Forms.AnchorStyles.Top Or Windows.Forms.AnchorStyles.Left Or Windows.Forms.AnchorStyles.Right
            Me.Controls.Add(lAddTextbox)
            pRightColumnItems.Add(lAddTextbox)
        End With
        Return lAddTextbox
    End Function

    Private Function AddCheckbox(ByVal aChecked As Boolean, ByVal aName As String) As Windows.Forms.CheckBox
        Dim lAddCheckbox As New Windows.Forms.CheckBox
        With lAddCheckbox
            .Checked = aChecked
            .Name = aName
            .Top = pTop - pRowHeight
            .Anchor = Windows.Forms.AnchorStyles.Top Or Windows.Forms.AnchorStyles.Left Or Windows.Forms.AnchorStyles.Right
            Me.Controls.Add(lAddCheckbox)
            pRightColumnItems.Add(lAddCheckbox)
        End With
        Return lAddCheckbox
    End Function

    'Private Sub AddButton()
    '    Dim btn As Windows.Forms.Button = New Windows.Forms.Button
    '    btn.Text = curLabel
    '    btn.Top = lTop
    '    btn.Left = 0
    '    btn.Width = Me.ClientSize.Width
    '    btn.Anchor = Windows.Forms.AnchorStyles.Left Or Windows.Forms.AnchorStyles.Right Or Windows.Forms.AnchorStyles.Top
    '    lTop += btn.Height

    '    Me.Controls.Add(btn)
    '    AddHandler btn.Click, AddressOf btnClick
    'End Sub

    'Private Sub btnClick(ByVal sender As Object, ByVal e As System.EventArgs)
    '    pLabelClicked = sender.text
    '    Me.Hide()
    'End Sub

    Private Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
        pOk = True
        Me.Hide()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Hide()
    End Sub
End Class