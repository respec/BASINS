Imports atcControls
Imports atcData
Imports atcUtility
Imports MapWinUtility

Public Class frmSave

    Private pDataGroup As atcTimeseriesGroup
    Private pHighestDSN As Integer

    Public Function AskUser(ByVal aDataGroup As atcTimeseriesGroup, ByVal aLabel As String, ByVal aHighestDSN As Integer) As atcTimeseriesGroup
        pDataGroup = aDataGroup
        Me.lblStatus.Text = aLabel
        pHighestDSN = aHighestDSN
        agdData.Initialize(New WDMGridSource(aDataGroup))
        Me.ShowDialog()
        Return pDataGroup
    End Function

    Private Sub btnSelectAttributes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectAttributes.Click
        Logger.Msg("Attribute selection on WDM save form not yet implemented")
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        pDataGroup = Nothing
        Me.Close()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Me.Close()
    End Sub

    Private Sub frmSave_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyValue = Windows.Forms.Keys.F1 Then
            'TODO: link to documentation
            'atcUtility.ShowHelp("BASINS Details\?.html")
        End If
    End Sub
End Class

Friend Class WDMGridSource
    Inherits atcControls.atcGridSource

    Private pDataGroup As atcTimeseriesGroup
    'Private pSelected As atcCollection
    Private pDisplayAttributes As ArrayList

    ''' <summary>Names of attributes used for listing of data in UI</summary>
    Public Property DisplayAttributes() As ArrayList
        Get
            Return pDisplayAttributes
        End Get
        Set(ByVal newValue As ArrayList)
            pDisplayAttributes = newValue
        End Set
    End Property

    'Public Property SelectedItems() As atcCollection
    '    Get
    '        Return pSelected
    '    End Get
    '    Set(ByVal newValue As atcCollection)
    '        pSelected = newValue
    '    End Set
    'End Property

    Sub New(ByVal aDataGroup As atcData.atcTimeseriesGroup)
        pDataGroup = aDataGroup
        pDisplayAttributes = New ArrayList
        With pDisplayAttributes
            .Add("ID")
            .Add("New DSN")
            .Add("Scenario")
            .Add("Location")
            .Add("Constituent")
            .Add("History 1")
        End With
    End Sub

    Overrides Property Columns() As Integer
        Get
            Return pDisplayAttributes.Count
        End Get
        Set(ByVal Value As Integer)
        End Set
    End Property

    Public Overrides Property FixedRows() As Integer
        Get
            Return 1
        End Get
        Set(ByVal value As Integer)
        End Set
    End Property

    Overrides Property Rows() As Integer
        Get
            Return pDataGroup.Count + FixedRows
        End Get
        Set(ByVal Value As Integer)
        End Set
    End Property

    Overrides Property CellValue(ByVal aRow As Integer, ByVal aColumn As Integer) As String
        Get
            If aRow = FixedRows - 1 Then
                Return pDisplayAttributes(aColumn)
            Else
                Return pDataGroup(aRow - FixedRows).Attributes.GetFormattedValue(pDisplayAttributes(aColumn))
            End If
        End Get
        Set(ByVal Value As String)
            If aRow >= FixedRows Then
                pDataGroup(aRow - FixedRows).Attributes.SetValue(pDisplayAttributes(aColumn), Value)
            End If
        End Set
    End Property

    Overrides Property Alignment(ByVal aRow As Integer, ByVal aColumn As Integer) As atcControls.atcAlignment
        Get
            If aRow >= FixedRows Then
                Dim lAttributeDef As atcAttributeDefinition = atcDataAttributes.GetDefinition(pDisplayAttributes(aColumn))
                If Not lAttributeDef Is Nothing Then
                    Select Case lAttributeDef.TypeString.ToLower
                        Case "integer", "single", "double"
                            Return atcAlignment.HAlignDecimal
                    End Select
                End If
            End If
            Return atcControls.atcAlignment.HAlignLeft
        End Get
        Set(ByVal Value As atcControls.atcAlignment)
        End Set
    End Property

    'Overrides Property CellSelected(ByVal aRow As Integer, ByVal aColumn As Integer) As Boolean
    '    Get
    '        If Not pSelected Is Nothing Then
    '            If aRow < FixedRows Then
    '                Return False
    '            Else
    '                Return pSelected.Contains(pDataGroup(aRow - FixedRows))
    '            End If
    '        End If
    '        Return False
    '    End Get
    '    Set(ByVal newValue As Boolean)
    '    End Set
    'End Property

    Public Overrides Property CellEditable(ByVal aRow As Integer, ByVal aColumn As Integer) As Boolean
        Get
            If InvalidRowOrColumn(aRow, aColumn) Then
                Return False
            ElseIf aRow = 0 Then
                Return False
            Else
                Select Case pDisplayAttributes(aColumn).ToString.ToLower
                    Case "id", "dsn", "history 1" : Return False
                    Case Else : Return True
                End Select
            End If
        End Get
        Set(ByVal value As Boolean)
            MyBase.CellEditable(aRow, aColumn) = value
        End Set
    End Property
End Class
