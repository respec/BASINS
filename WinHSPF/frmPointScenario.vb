Imports atcUCI

Public Class frmPointScenario

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        Me.Icon = pIcon
        Me.MinimumSize = Me.Size
        Me.MaximumSize = Me.Size

        cboBase.Items.Clear()
        Dim lTs As Collection = pUCI.FindTimser("", "", "")
        For lIndex As Integer = 1 To lTs.Count
            Dim lCtemp As String = lTs(lIndex).Header.sen
            If lCtemp.Length > 3 Then
                If Mid(lCtemp, 1, 3) = "PT-" Then
                    lCtemp = Mid(lCtemp, 4)
                    If Not cboBase.Items.Contains(lCtemp) Then
                        cboBase.Items.Add(lCtemp)
                    End If
                End If
            End If
        Next
        If cboBase.Items.Count > 0 Then
            cboBase.SelectedIndex = 0
        End If
        atxNew.Text = ""

    End Sub

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Me.Dispose()
    End Sub

    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
        If atxNew.Text.Length > 0 Then
            'new name has been entered
            Dim lBaseTs As New Collection
            Dim lCtemp As String = "PT-" & cboBase.SelectedItem
            Dim lTs As Collection = pUCI.FindTimser(lCtemp, "", "")
            For lIndex As Integer = 1 To lTs.Count
                'create new timeseries
                Dim lSen As String = "PT-" & UCase(atxNew.Text)
                Dim lLoc As String = lTs(lIndex).Header.loc
                Dim lCon As String = lTs(lIndex).Header.con
                Dim lTstype As String = Mid(lCon, 1, 4)
                Dim lStanam As String = lTs(lIndex).Header.Desc
                Dim lLongloc As String = lLoc
                If IsNumeric(Mid(lLoc, 4)) Then
                    Dim lId As Integer = CInt(Mid(lLoc, 4))
                    Dim lOper As HspfOperation = pUCI.OpnBlks("RCHRES").OperFromID(lId)
                    If Not lOper Is Nothing Then
                        lLongloc = "RCHRES " & lOper.Id & " - " & lOper.Description
                    End If
                End If
                Dim lRmult As Double = atxMult.Text
                Dim lJdates() As Double
                Dim lRload() As Double
                ReDim lJdates(lTs(lIndex).Dates.Summary.NVALS)
                ReDim lRload(lTs(lIndex).Dates.Summary.NVALS)
                For lValIndex As Integer = 1 To lTs(lIndex).Dates.Summary.NVALS
                    lJdates(lValIndex) = lTs(lIndex).Dates.Value(lValIndex)
                    lRload(lValIndex) = lTs(lIndex).Value(lValIndex) * lRmult
                Next lValIndex
                Dim newwdmid As Integer
                Dim newdsn As Integer
                pUCI.AddPointSourceDataSet(lSen, lLoc, lCon, lStanam, lTstype, _
                    0, lJdates, lRload, newwdmid, newdsn)
                pfrmPoint.UpdateListsForNewPointSource(lSen, lStanam, lLoc, lCon, newwdmid, _
                  newdsn, "RCHRES", CInt(Mid(lLoc, 4)), lLongloc)
            Next
            Me.Dispose()
        Else
            'no new scenario name entered
            MsgBox("A new scenario name must be entered.", vbOKOnly, "Create Scenario Problem")
        End If
    End Sub

End Class