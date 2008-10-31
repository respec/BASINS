Imports atcUtility
Imports atcUCIForms
Imports atcUCI
Imports atcControls


Public Class frmActivityAll

    Dim pVScrollColumnOffset As Integer = 16

    Dim pPChanged As Boolean
    Dim pIChanged As Boolean
    Dim pRChanged As Boolean
    Dim pPGrid, pIGrid, pRGrid As atcGrid
    Private WithEvents lPerlndActivityControl As New ctlEditTable(pUCI.OpnBlks("PERLND").Tables("ACTIVITY"), Me)
    Private WithEvents lImplndActivityControl As New ctlEditTable(pUCI.OpnBlks("IMPLND").Tables("ACTIVITY"), Me)
    Private WithEvents lRchresActivityControl As New ctlEditTable(pUCI.OpnBlks("RCHRES").Tables("ACTIVITY"), Me)

    Private Sub lPerlndActivityControl_grdTableEdited(ByVal aChange As Boolean) Handles lPerlndActivityControl.Change
        pPChanged = True
        cmdApply.Enabled = True
    End Sub
    Private Sub lImplndActivityControl_grdTableEdited(ByVal aChange As Boolean) Handles lImplndActivityControl.Change
        pIChanged = True
        cmdApply.Enabled = True
    End Sub
    Private Sub lRchresActivityControl_grdTableEdited(ByVal aChange As Boolean) Handles lRchresActivityControl.Change
        pRChanged = True
        cmdApply.Enabled = True
    End Sub

    Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.MinimumSize = Me.Size
        Me.Icon = pIcon

        pPChanged = False
        pIChanged = False
        pRChanged = False

        TabPage1.Controls.Add(lPerlndActivityControl)
        TabPage2.Controls.Add(lImplndActivityControl)
        TabPage3.Controls.Add(lRchresActivityControl)

        pPGrid = lPerlndActivityControl.Controls.Item("grdTable")
        pIGrid = lImplndActivityControl.Controls.Item("grdTable")
        pRGrid = lRchresActivityControl.Controls.Item("grdTable")

        RefreshTableGeometry()

        cmdApply.Enabled = False
        AddHandler Me.Resize, AddressOf frmActivityAll_Resize

    End Sub

    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
        If pPChanged Or pIChanged Or pRChanged Then
            Save()
        End If
        Me.Dispose()
    End Sub
    Private Sub cmdApply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdApply.Click
        If pPChanged Or pIChanged Or pRChanged Then
            Save()
        End If
    End Sub
    Private Sub Save()
        If pPChanged Then
            lPerlndActivityControl.Save()
        End If
        If pIChanged Then
            lImplndActivityControl.Save()
        End If
        If pRChanged Then
            lRchresActivityControl.Save()
        End If
        If pPChanged Or pIChanged Or pRChanged Then
            cmdApply.Enabled = False
        Else 'should not get here
            MsgBox("No Changes have been made.")
        End If

        cmdApply.Enabled = False

        pPChanged = False
        pIChanged = False
        pRChanged = False
    End Sub
    Private Sub frmActivityAll_Resize(ByVal sender As Object, ByVal e As System.EventArgs)
        RefreshTableGeometry()
    End Sub

    Private Sub SSTabPIR_ChangeTab(ByVal sender As Object, ByVal e As System.EventArgs) Handles SSTabPIR.SelectedIndexChanged
        RefreshTableGeometry()
    End Sub

    Private Sub RefreshTableGeometry()

        lPerlndActivityControl.Size = lPerlndActivityControl.Parent.Size
        lImplndActivityControl.Size = lImplndActivityControl.Parent.Size
        lRchresActivityControl.Size = lRchresActivityControl.Parent.Size

        pPGrid.SizeAllColumnsToContents(pPGrid.Width - pVScrollColumnOffset, True)
        pIGrid.SizeAllColumnsToContents(pIGrid.Width - pVScrollColumnOffset, True)
        pRGrid.SizeAllColumnsToContents(pRGrid.Width - pVScrollColumnOffset, True)

    End Sub
    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Me.Dispose()
    End Sub

    Private Sub cmdHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdHelp.Click

    End Sub
End Class