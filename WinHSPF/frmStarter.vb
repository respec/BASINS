Imports MapWinUtility
Imports atcUCI

Public Class frmStarter

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        Me.Icon = pIcon
        Me.MinimumSize = Me.Size
        Me.MaximumSize = Me.Size

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
            pUCI.SetWDMFiles()
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
                                lOpn.DefOpnId = .CellValue(lRow, 2)
                            End If
                        Next
                    Next
                End If
            Next
        End With
    End Sub

    Private Sub cmdStarter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdStarter.Click

        OpenFileDialog1.InitialDirectory = System.Reflection.Assembly.GetEntryAssembly.Location
        OpenFileDialog1.Filter = "HSPF User Control Input Files (*.uci)"
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

            'RefreshGrid()
        End If

    End Sub
End Class