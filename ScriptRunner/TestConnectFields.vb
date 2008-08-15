Imports MapWinUtility
Imports MapWindow.Interfaces
Imports atcControls
Imports atcUtility

Module TestConnectFields
    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Dim lForm As New frmConnectFields
        With lForm.ctlConnectFields
            Dim lSources As New atcCollection
            With lSources
                .Add("srcField1")
                .Add("srcField2")
                .Add("srcField3")
            End With
            .lstSource.Items.AddRange(lSources.ToArray)
            With .lstTarget.Items
                .Add("tarField1")
                .Add("tarField2")
                .Add("tarField3")
                .Add("tarField4")
            End With
            .AddConnection("srcField1 <-> tarField4")
            .AddConnection("x <-> y")
            .AddConnection("z <-> a", False)
            Dim lResult As System.Windows.Forms.DialogResult = lForm.ShowDialog
        End With
        For lIndex As Integer = 0 To lForm.Connections.Count - 1
            With lForm.Connections
                Logger.Dbg(.Keys(lIndex) & ":" & .ItemByIndex(lIndex))
            End With
        Next
    End Sub
End Module
