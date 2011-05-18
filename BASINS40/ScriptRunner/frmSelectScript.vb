Imports MapWinUtility
Imports System.Reflection

Partial Class frmSelectScript
    Inherits System.Windows.Forms.Form
    Private Sub cmdRun_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRun.Click
        If lstScripts.SelectedIndex >= -1 Then
            Dim lScriptName As String = lstScripts.Items(lstScripts.SelectedIndex)
            My.Computer.Registry.SetValue("HKEY_CURRENT_USER\Software\VB and VBA Program Settings\ScriptDriver", "LastScript", lScriptName)
            Logger.Dbg("RunScript " & lScriptName)
            Dim lOriginalCursor As Windows.Forms.Cursor = Windows.Forms.Cursor.Current
            Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
            Dim lAssembly As Assembly = Assembly.GetExecutingAssembly
            Dim lCoClass As Type = lAssembly.GetType(lAssembly.GetName.Name & "." & lScriptName)
            Dim lMethod As MethodInfo = lCoClass.GetMethod("ScriptMain")
            Dim lArgs(0) As Object
            lArgs(0) = CObj(pMapWin)
            Dim lResult As Boolean
            Try
                lResult = lMethod.Invoke(Nothing, lArgs)
            Catch lEx As Exception
                Logger.Dbg(vbCrLf & "Exception: " & lEx.ToString & vbCrLf)
                If lEx.InnerException IsNot Nothing Then
                    Logger.Dbg(vbCrLf & "Inner Exception: " & lEx.InnerException.Message & vbCrLf & lEx.InnerException.StackTrace & vbCrLf)
                End If
                Logger.Flush()
                lResult = False
            End Try
            Logger.Dbg("  Done:Result:" & lResult)
            Windows.Forms.Cursor.Current = lOriginalCursor
            Me.Dispose()
        Else
            Logger.Msg("Select a Script to Run", vbOK, "Script Runner")
        End If
    End Sub

    Private Sub lstScripts_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstScripts.DoubleClick
        cmdRun_Click(sender, e)
    End Sub
End Class