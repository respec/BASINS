Imports atcControls
Imports atcData
Imports atcUCI
Imports atcUCIForms
Imports atcUtility
Imports MapWinUtility
Imports WinHSPF
Imports System.Collections.ObjectModel

Public Class frmAddPoint

    Public Sub New()
        Dim lOper As HspfOperation
        Dim lOpnBlk As HspfOpnBlk
        Dim vpol As Object
        Dim i&, ctmp$

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        Me.Icon = pIcon
        Me.MinimumSize = Me.Size
        Me.MaximumSize = Me.Size

        cboReach.Items.Clear()
        lOpnBlk = pUCI.OpnBlks("RCHRES")
        For i = 1 To lOpnBlk.Count
            lOper = lOpnBlk.OperFromID(i)
            cboReach.Items.Add("RCHRES " & lOper.Id & " - " & lOper.Description)
        Next i
        cboReach.SelectedIndex = 0

        cboPollutant.Items.Clear()
        For Each vpol In pPollutantList
            cboPollutant.Items.Add(vpol)
        Next vpol

        For i = 1 To pfrmPoint.agdMasterPoint.Source.Rows()
            ctmp = frmPoint.agdMasterPoint.Source.CellValue(i, 4)
            If Not cboPollutant.Items.Contains(ctmp) Then
                cboPollutant.Items.Add(ctmp)
            End If
        Next i

        cboFac.Items.Clear()
        'For i = 1 To frmPoint.cmbFac.ListCount - 1
        '  cboFac.AddItem frmPoint.cmbFac.List(i)
        'Next i
        For i = 1 To pfrmPoint.agdMasterPoint.Source.Rows - 1
            ctmp = pfrmPoint.agdMasterPoint.Source.CellValue(i, 3)
            If Not cboFac.Items.Contains(ctmp) Then
                cboFac.Items.Add(ctmp)
            End If
        Next i
    End Sub

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Me.Dispose()
    End Sub

    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
        Me.Dispose()
    End Sub
End Class