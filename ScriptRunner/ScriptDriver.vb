Imports MapWindow.Interfaces
Imports System.Reflection

Public Module ScriptDriver
    Friend pMapWin As IMapWin

    Public Sub Main(ByRef aMapWin As IMapWin)
        pMapWin = aMapWin
        Dim lSelectScript As New frmSelectScript
        Dim lAssembly As Assembly = Assembly.GetExecutingAssembly
        Dim lCoClassList() As Type = lAssembly.GetTypes 'GetExportedTypes gets only public types
        For Each lCoClass As Type In lCoClassList
            Dim lMethod As MethodBase = lCoClass.GetMethod("ScriptMain")
            If Not lMethod Is Nothing Then
                Dim lName As String = lCoClass.Name
                lSelectScript.lstScripts.Items.Add(lName)
            End If
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

        lSelectScript.Show()
    End Sub
End Module
