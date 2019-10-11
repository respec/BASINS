Imports System
Imports System.Windows.Forms
Imports DotSpatial.Controls
Imports DotSpatial.Controls.Docking
Imports DotSpatial.Controls.Header

Public Class USGSBaseflowPlugin
    Inherits Extension

    Public Overrides Sub Activate()
        App.HeaderControl.Add(New SimpleActionItem("Baseflow Separation", AddressOf USGSBaseflowTool_Clicked))
        MyBase.Activate()
    End Sub

    Public Overrides Sub Deactivate()
        If frmInteractive IsNot Nothing AndAlso Not frmInteractive.IsDisposed Then frmInteractive.Close()
        App.HeaderControl.RemoveAll()
        MyBase.Deactivate()
    End Sub

    Private Sub DisplayMessage(ByVal message As String)
        App.ProgressHandler.Progress(Nothing, 0, message)
    End Sub

    Private Sub USGSBaseflowTool_Clicked(ByVal sender As Object, ByVal e As EventArgs)
        If frmInteractive Is Nothing Or frmInteractive.IsDisposed Then
            frmInteractive = New frmUSGSBaseflow()
        End If
        frmInteractive.WindowState = System.Windows.Forms.FormWindowState.Normal
        frmInteractive.InitializeDS(Me)
        frmInteractive.Initialize()
        frmInteractive.Show()
    End Sub
End Class
