Imports MapWinUtility
Imports atcUCI
Imports atcUtility

Public Class frmStarter

    Dim pVScrollColumnOffset As Integer = 16
    Dim pCurrentSelectedColumn As Integer
    Dim pCurrentSelectedRow As Integer

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        Me.Icon = pIcon
        Me.MinimumSize = Me.Size
        Me.MaximumSize = Me.Size

        agdStarter.Source = New atcControls.atcGridSource
        RefreshGrid()

    End Sub

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Me.Dispose()
    End Sub

    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
        SaveSpecs()
        Me.Dispose()
    End Sub

    Private Sub cmdApply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdApply.Click
        If Logger.Msg("All parameters will be set to the values specified in 'starter.uci'." & vbCrLf & vbCrLf & _
                              "Are you sure you want to continue?", MsgBoxStyle.YesNo, _
                              "Apply Starter Values") = vbYes Then
            SaveSpecs()
            pUCI.SetDefault(pDefUCI)
        End If
    End Sub

    Private Sub SaveSpecs()
        With agdStarter.Source
            Dim lOpTyps As New Collection
            lOpTyps.Add("PERLND")
            lOpTyps.Add("IMPLND")
            lOpTyps.Add("RCHRES")

            For Each lOpTypName As String In lOpTyps
                If pUCI.OpnBlks(lOpTypName).Count > 0 Then
                    Dim lOpTyp As HspfOpnBlk = pUCI.OpnBlks(lOpTypName)
                    For Each lOpn As HspfOperation In lOpTyp.Ids
                        Dim lDesc As String
                        If Len(Trim(lOpn.Description)) > 0 Then
                            lDesc = lOpn.Description & " (" & lOpn.Name & ")"
                        Else
                            lDesc = "Unspecified" & " (" & lOpn.Name & ")"
                        End If
                        For lRow As Integer = 1 To .Rows
                            If .CellValue(lRow, 0) = lDesc Then
                                Dim lDefDesc = .CellValue(lRow, 1)
                                Dim lParPos As Integer = InStr(lDefDesc, "(")
                                Dim lSpacePos As Integer = InStr(Mid(lDefDesc, lParPos), " ")
                                Dim lDefOpId As String = Mid(lDefDesc, lParPos + lSpacePos, lDefDesc.length - lParPos - lSpacePos)
                                If IsNumeric(lDefOpId) Then
                                    lOpn.DefOpnId = CInt(lDefOpId)
                                End If
                            End If
                        Next
                    Next
                End If
            Next
        End With
    End Sub

    Private Sub cmdStarter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdStarter.Click

        OpenFileDialog1.InitialDirectory = System.Reflection.Assembly.GetEntryAssembly.Location
        OpenFileDialog1.Filter = "HSPF User Control Input Files (*.uci)|*.uci"
        OpenFileDialog1.FileName = "*.uci"
        OpenFileDialog1.Title = "Select Starter UCI File"

        Dim lFileName As String = ""
        If OpenFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            lFileName = OpenFileDialog1.FileName
        End If

        If lFileName.Length > 0 Then
            pDefUCI = New HspfUci
            pDefUCI.FastReadUciForStarter(pMsg, lFileName)

            'unset the default oper ids
            Dim lOpTyps As New Collection
            lOpTyps.Add("PERLND")
            lOpTyps.Add("IMPLND")
            lOpTyps.Add("RCHRES")

            Dim lOpTyp As HspfOpnBlk
            For Each lOpTypName As String In lOpTyps
                If pUCI.OpnBlks(lOpTypName).Count > 0 Then
                    lOpTyp = pUCI.OpnBlks(lOpTypName)
                    For Each lOpn As HspfOperation In lOpTyp.Ids
                        lOpn.DefOpnId = 0
                    Next
                End If
            Next

            RefreshGrid()
        End If

    End Sub

    Private Sub RefreshGrid()

        With agdStarter
            .Clear()
            .AllowHorizontalScrolling = False
            .AllowNewValidValues = True
            .Visible = True
            .Source.FixedRows = 1
        End With

        With agdStarter.Source
            .Columns = 1
            .Rows = 1
            .CellValue(0, 0) = "Project Operations"
            .CellValue(0, 1) = "Mapped From Starter Operation"

            Dim lOpTyps As New Collection
            lOpTyps.Add("PERLND")
            lOpTyps.Add("IMPLND")
            lOpTyps.Add("RCHRES")

            Dim lOpTyp As HspfOpnBlk
            Dim lRow As Integer = 0
            Dim lDesc As String
            For Each lOpTypName As String In lOpTyps
                If pUCI.OpnBlks(lOpTypName).Count > 0 Then
                    Dim lDescList As New atcCollection
                    lOpTyp = pUCI.OpnBlks(lOpTypName)
                    For Each lOpn As HspfOperation In lOpTyp.Ids
                        If Len(Trim(lOpn.Description)) > 0 Then
                            lDesc = lOpn.Description
                        Else
                            lDesc = "Unspecified"
                        End If

                        If Not lDescList.Contains(lDesc) Then
                            lDescList.Add(lDesc)

                            'add a row for this one
                            lRow = lRow + 1
                            '.Rows = lRow
                            .CellValue(lRow, 0) = lDesc & " (" & lOpn.Name & ")"
                            If lOpn.DefOpnId <> 0 Then
                                Dim ldOpn As HspfOperation = pDefUCI.OpnBlks(lOpn.Name).OperFromID(lOpn.DefOpnId)
                                lDesc = ldOpn.Description
                                .CellValue(lRow, 1) = lDesc & " (" & ldOpn.Name & " " & ldOpn.Id & ")"
                            Else
                                Dim lId As Integer = pUCI.DefaultOpnId(lOpn, pDefUCI)
                                Dim ldOpn As HspfOperation = pDefUCI.OpnBlks(lOpn.Name).OperFromID(lId)
                                If Not ldOpn Is Nothing Then
                                    lDesc = ldOpn.Description
                                    .CellValue(lRow, 1) = lDesc & " (" & ldOpn.Name & " " & ldOpn.Id & ")"
                                Else
                                    .CellValue(lRow, 1) = "None available"
                                End If
                            End If
                        End If
                    Next
                End If
            Next

            'make column 1 editable
            For lRowIndex As Integer = 1 To .Rows - 1
                .CellEditable(lRowIndex, 1) = True
            Next

        End With

        agdStarter.AllowNewValidValues = False
        agdStarter.SizeAllColumnsToContents(agdStarter.Width - pVScrollColumnOffset, True)
        agdStarter.Refresh()

    End Sub

    Private Sub DoLimits()

        If pCurrentSelectedColumn = 1 Then
            Dim lDesc As String = agdStarter.Source.CellValue(pCurrentSelectedRow, 0)
            Dim lParPos As Integer = InStr(lDesc, "(")
            Dim lEndParPos As Integer = InStr(Mid(lDesc, lParPos), ")")
            Dim lOpType As String = Mid(lDesc, lParPos + 1, lEndParPos - 2)
            Dim lValidValues As New Collection
            If pDefUCI.OpnBlks(lOpType).Count > 0 Then
                For Each lOpn As HspfOperation In pDefUCI.OpnBlks(lOpType).Ids
                    lDesc = lOpn.Description
                    lValidValues.Add(lDesc & " (" & lOpn.Name & " " & lOpn.Id & ")")
                Next
            End If
            agdStarter.ValidValues = lValidValues
            agdStarter.Refresh()
        End If

    End Sub

    Private Sub agdStarter_MouseDownCell(ByVal aGrid As atcControls.atcGrid, ByVal aRow As Integer, ByVal aColumn As Integer) Handles agdStarter.MouseDownCell
        pCurrentSelectedColumn = aColumn
        pCurrentSelectedRow = aRow
        DoLimits()
    End Sub

    Private Sub frmStarter_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyValue = Windows.Forms.Keys.F1 Then
            ShowHelp(pWinHSPFManualName)
            ShowHelp("User's Guide\Detailed Functions\Starter Mapping.html")
        End If
    End Sub
End Class