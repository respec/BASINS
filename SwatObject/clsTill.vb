'OBJECTID,ITNUM,TILLNM,EFTMIX,DEPTIL,OPNAME,OPNUM
Partial Class SwatInput
    Private pTill As clsTill = New clsTill(Me)
    ReadOnly Property Till() As clsTill
        Get
            Return pTill
        End Get
    End Property

    Public Class clsTillItem
        Public ITNUM As Double
        Public TILLNM As String '8
        Public EFTMIX As Single
        Public DEPTIL As Single
        Public OPNAME As String '40
        Public OPNUM As String '4

        Public Sub New()
        End Sub

        Public Sub New(ByVal aRow As DataRow)
            With aRow
                ITNUM = .Item("ITNUM")
                TILLNM = .Item("TILLNM")
                EFTMIX = .Item("EFTMIX")
                DEPTIL = .Item("DEPTIL")
                OPNAME = .Item("OPNAME")
                OPNUM = .Item("OPNUM")
            End With
        End Sub

        Public Function AddSQL() As String
            Return "INSERT INTO Till ( " & clsDataColumn.ColumnNames(clsHru.Columns) & " ) Values (" _
                   & ITNUM & ", " _
                   & TILLNM & ", " _
                   & EFTMIX & ", " _
                   & DEPTIL & ", " _
                   & OPNAME & ", " _
                   & OPNUM & " )"
        End Function
    End Class

    Public Class clsTill
        Private pSwatInput As SwatInput
        Private pTableName As String = "Till"

        Friend Shared Columns As Generic.List(Of clsDataColumn)

        Friend Sub New(ByVal aSwatInput As SwatInput)
            pSwatInput = aSwatInput
            If Columns Is Nothing Then InitColumns()
        End Sub

        Private Shared Sub InitColumns()
            Columns = New Generic.List(Of clsDataColumn)

            Columns.Add(New clsDataColumn("ITNUM", 1, "Double", "0", 4, ""))
            Columns.Add(New clsDataColumn("TILLNM", 1, "VARCHAR(8)", "%s", 12, ""))
            Columns.Add(New clsDataColumn("EFTMIX", 1, "Single", "0.000", 16, ""))
            Columns.Add(New clsDataColumn("DEPTIL", 1, "Single", "0.000", 16, ""))
            Columns.Add(New clsDataColumn("OPNAME", 1, "VARCHAR(40)", "", 8, ""))
            Columns.Add(New clsDataColumn("OPNUM", 1, "VARCHAR(4)", "", 8, ""))

        End Sub

        Public Function TableCreate() As Boolean
        End Function

        Public Function Table() As DataTable
            pSwatInput.Status("Reading " & pTableName & " from database ...")
            Return pSwatInput.QueryGDB("SELECT * FROM " & pTableName & ";")
        End Function

        Public Sub Add(ByVal aItem As clsTillItem)
            ExecuteNonQuery(aItem.AddSQL, pSwatInput.CnSwatParm)
        End Sub

        Public Function FindTill(ByVal aTillName As String, Optional ByVal aTable As DataTable = Nothing) As clsTillItem
            If aTable Is Nothing Then aTable = Table()
            For Each lRow As DataRow In aTable.Rows
                If lRow.Item("TILLNM").ToString = aTillName Then
                    Return New clsTillItem(lRow)
                End If
            Next
            Return Nothing
        End Function

        Public Sub Save(Optional ByVal aTable As DataTable = Nothing)
            pSwatInput.Status("Writing " & pTableName & " text ...")
            If aTable Is Nothing Then aTable = Table()
            SaveTableAsText(aTable, Columns, pSwatInput.TxtInOutFolder & "\" & pTableName & ".dat")
        End Sub
    End Class
End Class
