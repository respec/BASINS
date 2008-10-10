Imports System.Drawing
Imports MapWinUtility
Imports atcUCI
Imports atcControls
Imports atcUCIForms
Public Class frmLand
    Dim pOrigTotal As Double = 0


    Public Sub New()
        Dim lTable As HspfTable
        Dim lHspfOperator As HspfOperation
        Dim lConn As HspfConnection
        Dim i, j, lRow As Integer
        Dim schemfound, netfound As Boolean
        Dim lAddSourceNameAndId As String

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.Text = "WinHSPF - LandUse Editor"
        Me.Icon = pIcon

        CheckSchematic(schemfound, netfound)
        If netfound And Not schemfound Then
            i = Logger.Msg("The Land Use Editor requires a Schematic Block." & vbCrLf & _
                   "Would you like to convert the Network Block to a Schematic Block?", MsgBoxStyle.YesNo, _
                   "WinHSPF - Land Use Editor")
            If i = 1 Then
                'ConvertNetworkToSchematic()
                WinHSPF.pUCI.MaxAreaByLand2Stream = 0
            End If
        End If

        With grdLand
            .Source = New atcControls.atcGridSource
            .Clear()
            .AllowHorizontalScrolling = False
            .AllowNewValidValues = True
            .Visible = True
        End With

        With grdLand.Source
            .Rows = 1
            .Columns = 5
            .FixedRows = 1
            .CellValue(0, 0) = "Source ID"
            .CellValue(0, 1) = "Source Description"
            .CellValue(0, 2) = "Target ID"
            .CellValue(0, 3) = "Target Description"
            If WinHSPF.pUCI.GlobalBlock.EmFg = 1 Then
                .CellValue(0, 4) = "Area (Acres)"
            Else
                .CellValue(0, 4) = "Area (Hectares)"
            End If

            If .Rows > 1 Then
                For lRow = 1 To .Rows - 1
                    .CellEditable(lRow, 4) = True
                Next
            End If

            For i = 1 To WinHSPF.pUCI.OpnSeqBlock.Opns.Count - 1
                lHspfOperator = WinHSPF.pUCI.OpnSeqBlock.Opn(i)
                If lHspfOperator.Name = "RCHRES" Then
                    ListTargets.Items.Add(lHspfOperator.Name & " " & lHspfOperator.Id, True)

                    For j = 0 To lHspfOperator.Sources.Count - 1
                        lConn = lHspfOperator.Sources(j)
                        lAddSourceNameAndId = lConn.Source.VolName & " " & lConn.Source.VolId
                        If (lConn.Source.VolName = "PERLND" Or lConn.Source.VolName = "IMPLND") And Not ListSources.Items.Contains(lAddSourceNameAndId) Then
                            ListSources.Items.Add(lConn.Source.VolName & " " & lConn.Source.VolId, True)
                        End If
                    Next
                End If
            Next i

        End With

        ListSources.SelectionMode = SelectionMode.One
        AddHandler chkAllSources.CheckStateChanged, AddressOf chkAllSources_CheckedChanged
        AddHandler ListSources.ItemCheck, AddressOf ListSources_IndividualCheckChanged
        AddHandler ListTargets.ItemCheck, AddressOf ListTargets_IndividualCheckChanged
        AddHandler chkAllTargets.CheckStateChanged, AddressOf chkAllTargets_CheckedChanged

        RefreshGrid()


    End Sub
    Private Sub RefreshGrid()
        Dim i, lRow As Integer
        Dim lTargetName As String
        Dim lHspfOperator As HspfOperation
        Dim lHspfConnection As HspfConnection
        Dim t As Double
        Dim vConn As Object
        Dim lTargetIndex As Integer
        Dim lSourceID As String
        Dim lTargetID As String

        With grdLand
            .Clear()
            .Source.Rows = 1
        End With

        lTargetName = "RCHRES"
        lTargetIndex = 3

        For i = 1 To WinHSPF.pUCI.OpnSeqBlock.Opns.Count - 1
            lHspfOperator = WinHSPF.pUCI.OpnSeqBlock.Opn(i)

            For Each vConn In lHspfOperator.Sources
                lHspfConnection = vConn
                lSourceID = lHspfConnection.Source.VolName & " " & lHspfConnection.Source.VolId
                lTargetID = lHspfOperator.Name & " " & lHspfOperator.Id
                lRow = grdLand.Source.Rows
                With ListSources
                    If lHspfOperator.Name <> "COPY" AndAlso .Items.Contains(lSourceID) AndAlso .GetItemChecked(.FindStringExact(lSourceID)) AndAlso ListTargets.GetItemChecked(ListTargets.FindStringExact(lTargetID)) Then
                        grdLand.Source.CellValue(lRow, 0) = lSourceID
                        grdLand.Source.CellValue(lRow, 1) = lHspfConnection.Source.Opn.Description
                        grdLand.Source.CellValue(lRow, 2) = lTargetID
                        grdLand.Source.CellValue(lRow, 3) = WinHSPF.pUCI.OpnSeqBlock.Opns(i).Description
                        grdLand.Source.CellValue(lRow, 4) = lHspfConnection.MFact
                    End If
                End With
            Next
        Next


        grdLand.SizeAllColumnsToContents(grdLand.Width, True)

        With grdLand

            If .Source.Rows > 1 Then
                For lRow = 0 To .Source.Rows - 1
                    .Source.Alignment(lRow, 4) = atcAlignment.HAlignRight
                Next
            End If

            .Refresh()

            t = 0

            If .Source.Rows > 1 Then
                For lRow = 1 To .Source.Rows - 1
                    t = t + grdLand.Source.CellValue(lRow, 4)
                Next
            End If

            pOrigTotal = t

        End With

        If t = pOrigTotal Then
            txtTotal.Text = CStr(Format(t, "#####0.00")).PadLeft(18)

            txtLabelOrigTotal.Visible = False
            txtLabelDifference.Visible = False
            txtOrigTotal.Visible = False
            txtDifference.Visible = False

            '.NET conversion: Switch Differnece label font to ControlText (un-do Red coloring)
            txtDifference.ForeColor = System.Drawing.SystemColors.ControlText
            txtLabelDifference.ForeColor = System.Drawing.SystemColors.ControlText
        Else
            txtLabelOrigTotal.Enabled = True
            txtLabelDifference.Enabled = True
            txtOrigTotal.Enabled = True
            txtDifference.Enabled = True

            txtTotal.Text = CStr(Format(t, "#####0.00")).PadLeft(18)
            txtOrigTotal.Text = CStr(Format(pOrigTotal, "#####0.00")).PadLeft(18)
            txtDifference.Text = CStr(Format(t - pOrigTotal, "#####0.00")).PadLeft(18)

            txtDifference.ForeColor = Color.Red
            txtLabelDifference.ForeColor = Color.Red
        End If

    End Sub

    Public Sub CheckSchematic(ByVal schemfound As Boolean, ByVal netfound As Boolean)
        Dim vConn As Object, lConn As HspfConnection

        schemfound = False
        netfound = False
        For Each vConn In WinHSPF.pUCI.Connections
            lConn = vConn
            If lConn.Typ = 3 Then
                schemfound = True
            ElseIf lConn.Typ = 2 Then
                netfound = True
            End If
        Next vConn
    End Sub

    Private Sub ListSources_IndividualCheckChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckEventArgs)

        RemoveHandler chkAllSources.CheckStateChanged, AddressOf chkAllSources_CheckedChanged

        chkAllSources.Checked = False

        AddHandler chkAllSources.CheckStateChanged, AddressOf chkAllSources_CheckedChanged

        RemoveHandler ListSources.ItemCheck, AddressOf ListSources_IndividualCheckChanged
        ListSources.SetItemChecked(e.Index, e.NewValue)
        AddHandler ListSources.ItemCheck, AddressOf ListSources_IndividualCheckChanged

        RefreshGrid()
    End Sub

    Private Sub chkAllSources_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim lRow As Integer
        RemoveHandler ListSources.ItemCheck, AddressOf ListSources_IndividualCheckChanged

        For lRow = 0 To ListSources.Items.Count - 1
            ListSources.SetItemChecked(lRow, chkAllSources.Checked)
        Next

        AddHandler ListSources.ItemCheck, AddressOf ListSources_IndividualCheckChanged

        RefreshGrid()
    End Sub

    Private Sub ListTargets_IndividualCheckChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckEventArgs)

        RemoveHandler chkAllTargets.CheckStateChanged, AddressOf chkAllTargets_CheckedChanged

        chkAllSources.Checked = False

        AddHandler chkAllTargets.CheckStateChanged, AddressOf chkAllTargets_CheckedChanged

        RemoveHandler ListTargets.ItemCheck, AddressOf ListTargets_IndividualCheckChanged
        ListTargets.SetItemChecked(e.Index, e.NewValue)
        AddHandler ListTargets.ItemCheck, AddressOf ListTargets_IndividualCheckChanged

        RefreshGrid()
    End Sub

    Private Sub chkAllTargets_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim lRow As Integer
        RemoveHandler ListTargets.ItemCheck, AddressOf ListTargets_IndividualCheckChanged

        For lRow = 0 To ListTargets.Items.Count - 1
            ListTargets.SetItemChecked(lRow, chkAllTargets.Checked)
        Next

        AddHandler ListTargets.ItemCheck, AddressOf ListTargets_IndividualCheckChanged

        RefreshGrid()
    End Sub

    Private Sub grdTable_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles grdLand.Resize
        grdLand.SizeAllColumnsToContents(grdLand.Width, True)
    End Sub

End Class