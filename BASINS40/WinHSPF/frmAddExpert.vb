Imports atcUCI
Imports atcData
Imports atcUtility


Public Class frmAddExpert

    Dim pCheckedRadioIndex As Integer

    Sub New(ByVal aCheckedRadioIndex As Integer)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.Icon = pIcon
        Me.MinimumSize = Me.Size
        Me.MaximumSize = Me.Size

        pCheckedRadioIndex = aCheckedRadioIndex

        Select Case pCheckedRadioIndex
            Case 1 'Hydrology Calibration
                ListBox1.Size = New Size(350, 173)
                ListBox2.Visible = False
                txtDefine.Visible = False
                optH.Visible = False
                optD.Visible = False
                Label2.Visible = False
            Case 2 'Flow
                ListBox1.Size = New Size(350, 173)
                ListBox2.Visible = False
                txtDefine.Visible = False
                Label2.Visible = False
            Case 3 'AQUATOX Linkage
                ListBox1.Size = New Size(350, 173)
                ListBox2.Visible = False
                txtDefine.Visible = False
                Label2.Visible = False
        End Select

        'formerly

        Dim lHspfOperation As HspfOperation
        Dim i, lIndex2 As Integer
        Dim S As String
        Dim lFoundFlag As Boolean
        Dim lListBox1DataItems As New Collection

        lListBox1DataItems.Clear()

        If aCheckedRadioIndex = 4 Then 'other types
            ListBox2.Enabled = False
            Label2.Enabled = False
            For i = 1 To pUCI.OpnSeqBlock.Opns.Count
                lHspfOperation = pUCI.OpnSeqBlock.Opn(i)
                If lHspfOperation.Name = "RCHRES" Or lHspfOperation.Name = "PERLND" Or lHspfOperation.Name = "IMPLND" Or lHspfOperation.Name = "BMPRAC" Then
                    S = lHspfOperation.Name & " " & lHspfOperation.Id & " (" & lHspfOperation.Description & ")"
                    ListBox1.Items.Add(S)
                    lListBox1DataItems.Add(lHspfOperation.Id)
                End If
            Next i
        ElseIf aCheckedRadioIndex = 1 Or aCheckedRadioIndex = 2 Or aCheckedRadioIndex = 5 Then 'calib or flow or aquatox
            ListBox2.Enabled = False
            If aCheckedRadioIndex = 5 Then
                'aquatox type
            End If
            For i = 1 To pUCI.OpnSeqBlock.Opns.Count
                lHspfOperation = pUCI.OpnSeqBlock.Opn(i)
                If lHspfOperation.Name = "RCHRES" Then
                    S = lHspfOperation.Name & " " & lHspfOperation.Id & " (" & lHspfOperation.Description & ")"
                    ListBox1.Items.Add(S)
                    lListBox1DataItems.Add(lHspfOperation.Id)
                End If
            Next i
        ElseIf aCheckedRadioIndex = 4 Then 'copy
            ListBox1.Items.Clear()
            ListBox2.Enabled = False

            For i = 1 To frmOutput.agdOutput.Source.Rows

                lFoundFlag = False
                For lIndex2 = 0 To ListBox1.Items.Count
                    If pfrmOutput.agdOutput.Source.CellValue(i, 0) = ListBox1.Items.Item(lIndex2) Then
                        lFoundFlag = True
                        Exit For
                    End If
                Next

                If Not lFoundFlag Then
                    ListBox1.Items.Add(pfrmOutput.agdOutput.Source.CellValue(i, 0))
                End If

            Next

        End If

        If aCheckedRadioIndex = 2 Or aCheckedRadioIndex = 3 Or aCheckedRadioIndex = 5 Then
            'give option of output timeunits if hourly run, otherwise always daily
            If pUCI.OpnSeqBlock.Delt <= 60 Then
                optH.Enabled = True
                optD.Enabled = True
            End If
        End If

        'txtLoc.Text = "<none>"


    End Sub

    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
        Me.Dispose()
    End Sub

    Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        Me.Dispose()
    End Sub
End Class