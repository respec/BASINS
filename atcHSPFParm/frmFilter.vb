Imports atcUtility
Imports MapWinUtility
Imports atcUCI

Public Class frmFilter

    Friend pType As String
    Friend pParentObj As frmHSPFParm
    Friend pLoading As Boolean

    Public Sub InitializeUI(ByVal aType As String, ByVal aFilterValues As atcCollection, ByVal aSelectedFilters As atcCollection, ByVal aParentObj As frmHSPFParm)
        pLoading = True
        Me.Text = aType & " Filter"
        pType = aType
        pParentObj = aParentObj

        clbFilter.Items.Clear()
        If aSelectedFilters.Count = 0 Then
            aSelectedFilters = aFilterValues  'let all behave same as none
        End If

        For lIndex As Integer = 0 To aFilterValues.Count - 1
            Dim lChecked As Boolean = False
            If aSelectedFilters.Contains(aFilterValues(lIndex)) Then
                lChecked = True
            End If
            clbFilter.Items.Add(aFilterValues(lIndex), lChecked)
        Next lIndex

        If aFilterValues.Count <> aSelectedFilters.Count Then
            cbxSelect.Checked = False
        End If
        pLoading = False
    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
        If pType = "Table" Then
            Dim lSelectedTableFilters As New atcCollection
            For lIndex As Integer = 0 To clbFilter.CheckedItems.Count - 1
                lSelectedTableFilters.Add(clbFilter.CheckedItems(lIndex))
            Next
            If clbFilter.CheckedItems.Count = clbFilter.Items.Count Then
                'all behaves same as none
                pParentObj.pSelectedTableFilters.Clear()
            Else
                pParentObj.pSelectedTableFilters = lSelectedTableFilters
            End If
        Else
            Dim lSelectedSegmentFilters As New atcCollection
            For lIndex As Integer = 0 To clbFilter.CheckedItems.Count - 1
                lSelectedSegmentFilters.Add(clbFilter.CheckedItems(lIndex))
            Next
            If clbFilter.CheckedItems.Count = clbFilter.Items.Count Then
                'all behaves same as none
                pParentObj.pSelectedSegmentFilters.Clear()
            Else
                pParentObj.pSelectedSegmentFilters = lSelectedSegmentFilters
            End If
        End If
        Me.Dispose()
    End Sub

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Me.Dispose()
    End Sub

    Private Sub cbxSelect_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxSelect.CheckedChanged
        If Not pLoading Then
            For lRow As Integer = 0 To clbFilter.Items.Count - 1
                clbFilter.SetItemChecked(lRow, cbxSelect.Checked)
            Next
        End If
    End Sub

    Private Sub frmFilter_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyValue = Windows.Forms.Keys.F1 Then
            ShowHelp("BASINS Details\Watershed and Instream Model Setup\HSPFParm.html")
        End If
    End Sub
End Class