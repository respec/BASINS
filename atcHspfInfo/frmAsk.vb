Option Strict Off
Option Explicit On
Imports MapWinUtility

Friend Class frmAsk
	Inherits System.Windows.Forms.Form
	
    'Public IPC As ATCoIPC
	Private pProcessName As String
	
	Public Sub AskAbout(ByRef ProcessName As String)
		pProcessName = ProcessName
		Text = "Process '" & pProcessName & "'"
		lbl.Text = "Still waiting for '" & pProcessName & "' to finish."
		lbl2.Text = ""
		Show()
	End Sub
	
	Private Sub cmdEnd_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdEnd.Click
        'If Not IPC Is Nothing Then
        '          IPC.ExitProcess(pProcessName)
        'Else
        '	MsgBox("IPC not set correctly, so process cannot be ended manually", MsgBoxStyle.OKOnly, "IPC Ask")
        'End If
        Hide()
	End Sub
End Class