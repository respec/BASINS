Imports atcControls
Imports atcData
Imports atcUCI
Imports atcUtility
Imports MapWinUtility

Public Class frmAddPoint

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        Me.Icon = pIcon
        Me.MinimumSize = Me.Size
        Me.MaximumSize = Me.Size

        cboReach.Items.Clear()
        Dim lOpnBlk As HspfOpnBlk = pUCI.OpnBlks("RCHRES")
        For Each lOper As HspfOperation In lOpnBlk.Ids
            cboReach.Items.Add("RCHRES " & lOper.Id & " - " & lOper.Description)
        Next lOper
        cboReach.SelectedIndex = 0

        cboPollutant.Items.Clear()
        For lOper As Integer = 1 To pfrmPoint.agdMasterPoint.Source.Rows()
            Dim lTempString As String = pfrmPoint.agdMasterPoint.Source.CellValue(lOper, 4)
            If Not cboPollutant.Items.Contains(lTempString) Then
                cboPollutant.Items.Add(lTempString)
            End If
        Next lOper

        cboFac.Items.Clear()
        For lOper As Integer = 1 To pfrmPoint.agdMasterPoint.Source.Rows - 1
            Dim lTempString As String = pfrmPoint.agdMasterPoint.Source.CellValue(lOper, 3)
            If Not cboFac.Items.Contains(lTempString) Then
                cboFac.Items.Add(lTempString)
            End If
        Next lOper
    End Sub

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Me.Dispose()
    End Sub

    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
        Dim lScenario, lLocation, lConstituent, lStationName, lTimeSeriesType As String
        Dim lReadyForCloseFlag As Boolean
        Dim lDashPosition, lNewDsn As Integer
        Dim lNewWdmId As Integer = 0
        Dim lLongLocation As String
        Dim lJDates(1) As Double
        Dim lLoadValues(1) As Double

        lReadyForCloseFlag = True
        'ok, check to make sure everything is filled
        If Len(txtScen.Text) = 0 Then
            Logger.Message("A pollutant must be entered.", "Create Point Source Problem", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, Windows.Forms.DialogResult.OK)
            lReadyForCloseFlag = False
        End If
        If Len(cboReach.Items.Item(cboReach.SelectedIndex)) = 0 Then
            Logger.Message("A reach must be selected.", "Create Point Source Problem", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, Windows.Forms.DialogResult.OK)
            lReadyForCloseFlag = False
        End If
        If cboPollutant.SelectedIndex = -1 And Len(cboPollutant.Text) = 0 Then
            Logger.Message("A pollutant must be entered.", "Create Point Source Problem", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, Windows.Forms.DialogResult.OK)
            lReadyForCloseFlag = False
        End If
        If lReadyForCloseFlag Then
            lScenario = "PT-" & UCase(Trim(txtScen.Text))
            lLongLocation = Trim(cboReach.Items.Item(cboReach.SelectedIndex))
            lDashPosition = InStr(1, lLongLocation, "-")
            lLocation = "RCH" & Trim(Mid(lLongLocation, 7, lDashPosition - 7))
            If cboPollutant.SelectedIndex > -1 Then
                'lConstituent = Mid(Trim(cboPollutant.Items.Item(cboPollutant.SelectedIndex)), 1, 8)
                lConstituent = cboPollutant.Items.Item(cboPollutant.SelectedIndex)
            Else
                lConstituent = cboPollutant.Text
            End If
            If cboFac.SelectedIndex > -1 Then
                lStationName = cboFac.Items.Item(cboFac.SelectedIndex)
            Else
                lStationName = cboFac.Text
            End If
            lConstituent = UCase(lConstituent)
            lStationName = UCase(lStationName)
            lTimeSeriesType = Mid(lConstituent, 1, 4)
            If lConstituent = "FLOW" Then
                lLoadValues(1) = atxValue.Text
            Else
                lLoadValues(1) = atxValue.Text / 24.0#  'next call converts to daily
            End If
            pUCI.AddPointSourceDataSet(lScenario, lLocation, lConstituent, lStationName, lTimeSeriesType, 0, lJDates, lLoadValues, lNewWdmId, lNewDsn)
            pfrmPoint.UpdateListsForNewPointSource(lScenario, lStationName, lLocation, lConstituent, "WDM" & lNewWdmId, lNewDsn, "RCHRES", CInt(Mid(lLocation, 4)), lLongLocation)
        End If
        If lReadyForCloseFlag Then
            Me.Dispose()
        End If
    End Sub

    Private Sub frmAddPoint_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyValue = Windows.Forms.Keys.F1 Then
            ShowHelp(pWinHSPFManualName)
            ShowHelp("User's Guide\Detailed Functions\Point Sources.html")
        End If
    End Sub
End Class