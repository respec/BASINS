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
            ctmp = pfrmPoint.agdMasterPoint.Source.CellValue(i, 4)
            If Not cboPollutant.Items.Contains(ctmp) Then
                cboPollutant.Items.Add(ctmp)
            End If
        Next i

        cboFac.Items.Clear()
        'For i = 1 To pfrmPoint.cmbFac.ListCount - 1
        '  cboFac.AddItem pfrmPoint.cmbFac.List(i)
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
        Dim sen, loc, con, stanam, tstype As String
        Dim imready As Boolean
        Dim dashpos, newdsn As Integer
        Dim newwdmid As String = Nothing
        Dim longloc As String
        Dim jdates(1) As Double
        Dim rload(1) As Double

        imready = True
        'ok, check to make sure everything is filled
        If Len(txtScen.Text) = 0 Then
            MsgBox("A scenario name must be entered.", vbOKOnly, "Create Point Source Problem")
            imready = False
        End If
        If Len(cboReach.Items.Item(cboReach.SelectedIndex)) = 0 Then
            MsgBox("A reach must be selected.", vbOKOnly, "Create Point Source Problem")
            imready = False
        End If
        If cboPollutant.SelectedIndex = -1 And Len(cboPollutant.Text) = 0 Then
            MsgBox("A pollutant must be entered.", vbOKOnly, "Create Point Source Problem")
            imready = False
        End If
        If imready Then
            sen = "PT-" & UCase(Trim(txtScen.Text))
            longloc = Trim(cboReach.Items.Item(cboReach.SelectedIndex))
            dashpos = InStr(1, longloc, "-")
            loc = "RCH" & Trim(Mid(longloc, 7, dashpos - 7))
            If cboPollutant.SelectedIndex > -1 Then
                'con = Mid(Trim(cboPollutant.Items.Item(cboPollutant.SelectedIndex)), 1, 8)
                con = cboPollutant.Items.Item(cboPollutant.SelectedIndex)
            Else
                con = cboPollutant.Text
            End If
            If cboFac.SelectedIndex > -1 Then
                stanam = cboFac.Items.Item(cboFac.SelectedIndex)
            Else
                stanam = cboFac.Text
            End If
            con = UCase(con)
            stanam = UCase(stanam)
            tstype = Mid(con, 1, 4)
            If con = "FLOW" Then
                rload(1) = atxValue.Text
            Else
                rload(1) = atxValue.Text / 24.0#  'next call converts to daily
            End If
            pUCI.AddPointSourceDataSet(sen, loc, con, stanam, tstype, 0, jdates, rload, newwdmid, newdsn)
            pfrmPoint.UpdateListsForNewPointSource(sen, stanam, loc, con, newwdmid, newdsn, "RCHRES", CInt(Mid(loc, 4)), longloc)
        End If
        If imready Then
            Me.Dispose()
        End If
    End Sub
End Class