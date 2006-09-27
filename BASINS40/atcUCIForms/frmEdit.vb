Imports MapWinUtility

Public Class frmEdit
    Private WithEvents pEditControl As ctlEdit
    Private pParent As Windows.Forms.Form

    Friend Property EditControl() As Windows.Forms.Control
        Get
            Return pEditControl
        End Get
        Set(ByVal aControl As Windows.Forms.Control)
            panelEdit.Controls.Add(aControl)
            aControl.Dock = Windows.Forms.DockStyle.Fill
            pEditControl = aControl
        End Set
    End Property

    Private Sub cmdOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdOk.Click
        pEditControl.Save()
        Me.Dispose()
    End Sub

    Private Sub cmdApply_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdApply.Click
        pEditControl.Save()
    End Sub

    Private Sub cmdCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        If pEditControl.Changed Then
            If Logger.Msg("Changes have been made.  Discard them?", _
                        Microsoft.VisualBasic.MsgBoxStyle.YesNo, _
                        "Discard Changes") = Microsoft.VisualBasic.MsgBoxResult.Yes Then
                Me.Dispose()
            End If
        Else
            Me.Dispose()
        End If
    End Sub

    Private Sub cmdAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        pEditControl.Add()
    End Sub

    Public Sub New(ByVal aParent As Windows.Forms.Form)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        pParent = aParent
    End Sub

    Private Sub pEditControl_Change(ByVal aChange As Boolean) Handles pEditControl.Change
        Me.Text = pEditControl.Caption
        If aChange Then
            Me.Text &= " *"
        End If
    End Sub
End Class