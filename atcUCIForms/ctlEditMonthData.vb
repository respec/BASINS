Imports System.Drawing
Imports MapWinUtility
Imports atcUCI
Imports atcControls

Public Class ctlEditMonthData
    Implements ctlEdit

    Dim pVScrollColumnOffset As Integer = 16
    Dim pHspfMonthData As New HspfMonthData
    Dim pCurrentMonthDataTable As New HspfMonthDataTable
    Dim pCurrentMonthDataTableIndex As Integer = 0
    Dim pCurrentID As Double
    Dim pChanged As Boolean
    Dim pInRefresh As Boolean
    Dim pEdited As Boolean = False
    Public Event Change(ByVal aChange As Boolean) Implements ctlEdit.Change

    Public ReadOnly Property Caption() As String Implements ctlEdit.Caption
        Get
            Return "Edit Month Data"
        End Get
    End Property

    Private Sub grdMonthValues_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles grdMonthValues.Resize
        grdMonthValues.SizeAllColumnsToContents(grdMonthValues.Width - pVScrollColumnOffset, True)
    End Sub

    Public Sub New(ByVal aHspfMonthData As Object, ByVal aParent As Windows.Forms.Form)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Data = aHspfMonthData
    End Sub

    Public Sub Add() Implements ctlEdit.Add
        'not needed 
    End Sub

    Public Sub Help() Implements ctlEdit.Help
        'TODO: add this code
    End Sub

    Public Sub Remove() Implements ctlEdit.Remove
        'not be needed
    End Sub

    Public Property Changed() As Boolean Implements ctlEdit.Changed
        Get
            Return pChanged
        End Get
        Set(ByVal aChanged As Boolean)
            If aChanged <> pChanged Then
                pChanged = aChanged
                RaiseEvent Change(aChanged)
            End If
        End Set
    End Property

    Public Property Data() As Object Implements ctlEdit.Data
        Get
            Return pHspfMonthData
        End Get

        Set(ByVal aHspfMonthData As Object)
            Dim lRow, lCol As Integer
            Dim lMonthsList() As String = {"Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"}

            pHspfMonthData = aHspfMonthData

            'set combo box to non-editable and give special cursor
            cboID.Cursor = Windows.Forms.Cursors.Hand
            cboID.DropDownStyle = Windows.Forms.ComboBoxStyle.DropDownList

            With grdMonthValues
                .Source = New atcControls.atcGridSource
                .Clear()
                .AllowHorizontalScrolling = False
                .AllowNewValidValues = True
                .Visible = True
                .Source.FixedRows = 1
            End With

            With grdMonthValues.Source
                .Rows = 2
                For lCol = 0 To 11
                    .CellValue(0, lCol) = lMonthsList(lCol)

                    For lRow = 1 To .Rows - 1
                        .CellEditable(lRow, lCol) = True
                    Next

                Next
            End With

            If pHspfMonthData.MonthDataTables.Count > 0 Then
                pCurrentMonthDataTable = pHspfMonthData.MonthDataTables(0)
                pCurrentID = pHspfMonthData.MonthDataTables(0).Id
                pCurrentMonthDataTableIndex = 0
            End If

            grdMonthValues.SizeAllColumnsToContents(grdMonthValues.Width - pVScrollColumnOffset, True)
            grdMonthValues.Refresh()

            RefreshMonthData()

        End Set
    End Property

    Private Sub RefreshMonthData()
        Dim lOper As Integer

        For lOper = 0 To pHspfMonthData.MonthDataTables.Count - 1

            cboID.Items.Add(pHspfMonthData.MonthDataTables(lOper).Id)

            If pCurrentID = pHspfMonthData.MonthDataTables(lOper).Id Then
                cboID.SelectedIndex = lOper
            End If

        Next

        If pHspfMonthData.MonthDataTables.Count > 0 Then
            With pCurrentMonthDataTable
                lblRefCount.Text = "is referenced " & .ReferencedBy.Count & " times"
                For lOper = 0 To 11
                    grdMonthValues.Source.CellValue(1, lOper) = .MonthValue(lOper + 1)
                Next
            End With
        End If

        pEdited = False

    End Sub

    Private Sub cboID_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboID.Click

    End Sub

    Private Sub grdMonthValues_CommitChange(ByVal ChangeFromRow As Long, ByVal ChangeToRow As Long, ByVal ChangeFromCol As Long, ByVal ChangeToCol As Long)

    End Sub
    Public Sub Save() Implements ctlEdit.Save
        Dim lCol As Integer

        For lCol = 1 To 12
            pCurrentMonthDataTable.MonthValue(lCol) = grdMonthValues.Source.CellValue(1, lCol - 1)
        Next
        pHspfMonthData.MonthDataTables.Add(pCurrentMonthDataTable)

    End Sub

    Private Sub cboID_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboID.SelectedIndexChanged
        Dim discard As Integer

        If CDbl(cboID.SelectedValue) <> pCurrentID Then 'need to change table
            If pEdited Then
                discard = MsgBox("Changes to current Month Data Table have not been saved. Discard them?", vbYesNo)
            Else
                discard = vbYes 'no changes
            End If
            If discard = vbYes Then
                Refresh()
            Else
                cboID.SelectedValue = pCurrentID
            End If
        End If

    End Sub
End Class
