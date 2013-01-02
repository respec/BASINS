Imports MapWinUtility
Imports MapWindow.Interfaces
Imports System.Reflection

Public Module ScriptDriver
    Friend pMapWin As IMapWin

    Public Sub Main(ByRef aMapWin As IMapWin)
        pMapWin = aMapWin

        Dim lScriptNames As Generic.List(Of String) = ScriptNames()

        Select Case lScriptNames.Count
            Case 0
                MapWinUtility.Logger.Msg("No scripts compiled")
            Case 1
                RunScript(lScriptNames(0))
            Case Else
                Dim lSelectScript As New frmSelectScript
                For Each lScriptName As String In lScriptNames
                    lSelectScript.lstScripts.Items.Add(lScriptName)
                Next
                Dim lLastScriptName As String = My.Computer.Registry.GetValue("HKEY_CURRENT_USER\Software\VB and VBA Program Settings\ScriptDriver", "LastScript", "")
                If Not lLastScriptName Is Nothing AndAlso lLastScriptName.Length > 0 Then
                    With lSelectScript.lstScripts
                        Dim lLastScriptIndex As Integer = .Items.IndexOf(lLastScriptName)
                        If lLastScriptIndex > -1 Then
                            .SelectedIndex = lLastScriptIndex
                        End If
                    End With
                End If
                lSelectScript.ShowDialog()
        End Select
    End Sub

    Public Function ScriptNames() As Generic.List(Of String)
        Dim lScriptNames As New Generic.List(Of String)
        Dim lAssembly As Assembly = Assembly.GetExecutingAssembly
        Dim lCoClassList() As Type = lAssembly.GetTypes 'GetExportedTypes gets only public types
        For Each lCoClass As Type In lCoClassList
            Dim lMethod As MethodBase = lCoClass.GetMethod("ScriptMain")
            If Not lMethod Is Nothing Then
                Dim lName As String = lCoClass.Name
                lScriptNames.Add(lName)
            End If
        Next
        Return lScriptNames
    End Function

    Public Sub RunScript(ByVal aScriptName As String)
        My.Computer.Registry.SetValue("HKEY_CURRENT_USER\Software\VB and VBA Program Settings\ScriptDriver", "LastScript", aScriptName)
        Logger.Dbg("RunScript " & aScriptName)
        Dim lOriginalCursor As Windows.Forms.Cursor = Windows.Forms.Cursor.Current
        Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
        Dim lAssembly As Assembly = Assembly.GetExecutingAssembly
        Dim lCoClass As Type = lAssembly.GetType(lAssembly.GetName.Name & "." & aScriptName)
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
    End Sub
End Module
