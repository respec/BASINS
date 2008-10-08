Imports System.Drawing
Imports MapWinUtility
Imports atcUCI
Imports atcControls
Imports atcUCIForms
Public Class frmLand
    Dim pOrigTotal As Double = 0
    Dim pAllowCheckAllSources As Boolean = True
    Dim pAllowCheckAllTargets As Boolean = True

    Public Sub New()
        Dim lTable As HspfTable
        Dim loper As HspfOperation
        Dim lConn As HspfConnection
        Dim i, j, lRow As Integer
        Dim schemfound, netfound As Boolean

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
            .Columns = 6
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
                loper = WinHSPF.pUCI.OpnSeqBlock.Opn(i)
                If loper.Name = "RCHRES" Then
                    ListTargets.Items.Add(loper.Name & " " & loper.Id, True)
                    For j = 1 To loper.Sources.Count - 1
                        lConn = loper.Sources(j)
                        If lConn.Source.VolName = "PERLND" Then
                            ListSources.Items.Add(lConn.Source.VolName & " " & lConn.Source.VolId, True)
                        End If
                    Next

                    For j = 1 To loper.Sources.Count - 1
                        lConn = loper.Sources(j)

                        If lConn.Source.VolName = "IMPLND" Then
                            ListSources.Items.Add(lConn.Source.VolName & " " & lConn.Source.VolId, True)
                        End If

                    Next j



                End If
            Next i
        End With

        RefreshGrid()


    End Sub
    Private Sub RefreshGrid()
        Dim i&, j&, s$, lName$, lId&
        Dim lHspfOperator As HspfOperation
        Dim vConn As Object
        Dim lConn As HspfConnection
        Dim t As Double
        Dim lOper As Integer

        t = 0
        With grdLand
            '.Visible = False
            '.Clear()
            '.Source.Rows = 1
            'For i = 0 To ListTargets.Items.Count - 1
            's = ListTargets.Items.Item(0)
            'lName = StrRetRem(s)
            '    lId = s
            '    lOper = GetOper(lName, lId)
            '    For Each vConn In lOper.Sources
            '        lConn = vConn
            '        If lConn.Typ = 3 Then 'schematic record
            '            If ListSources(lConn.Source.VolName & " " & lConn.Source.VolId) Then
            '                'Debug.Print lConn.Source.volname & " " & lConn.Source.volid
            '                .Source.Rows = .Source.Rows + 1
            '                .Source.CellValue(.Source.Rows, 0) = lConn.Source.VolName & " " & lConn.Source.VolId
            '                .Source.CellValue(.Source.Rows, 1) = GetOper(lConn.Source.VolName, lConn.Source.VolId).Description
            '                .Source.CellValue(.Source.Rows, 2) = lOper.Name & " " & lOper.Id
            '                .Source.CellValue(.Source.Rows, 3) = GetOper(lOper.Name, lOper.Id).Description
            '                .Source.CellValue(.Source.Rows, 4) = lConn.MFact
            '                t = t + lConn.MFact
            '            End If
            '        End If
            '    Next vConn
            'Next i
            '.SizeAllColumnsToContents()
            '.Visible = True
            'lblTotal(0) = t
            'lblTotal(1).Visible = False
            'lblTotal(2).Visible = False
            'pOrigTotal = t

            If .Source.Rows > 1 Then
                For lOper = 1 To .Source.Rows - 1
                    t = t + grdLand.Source.CellValue(lOper, 4)
                Next
            End If

            If t = pOrigTotal Then
                txtTotal.Text = CStr(Format(t, "#####0.00")).PadLeft(18)

                txtLabelOrigTotal.Visible = False
                txtLabelDifference.Visible = False
                txtOrigTotal.Visible = False
                txtDifference.Enabled = False

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
        End With

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

    Private Sub lstSou_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListSources.SelectedIndexChanged
        pAllowCheckAllSources = False
        chkAllSources.Checked = False
        pAllowCheckAllSources = True
    End Sub

    Private Sub chkAllSources_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkAllSources.CheckStateChanged
        Dim lRow As Integer

        If pAllowCheckAllSources Then
            For lRow = 0 To ListSources.Items.Count - 1
                ListSources.SetItemChecked(lRow, chkAllSources.Checked)
            Next
        End If

    End Sub

End Class