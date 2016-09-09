'OBJECTID,IUNUM,URBNAME,URBFLNM,FIMP,FCIMP,CURBDEN,URBCOEF,DIRTMX,THALF,TNCONC,TPCONC,TNO3CONC,OV_N,CN2A,CN2B,CN2C,CN2D,URBCN2
Partial Class SwatInput
    Private pUrban As clsUrban = New clsUrban(Me)
    ReadOnly Property Urban() As clsUrban
        Get
            Return pUrban
        End Get
    End Property

    Public Class clsUrbanItem
        Public IUNUM As Double
        Public URBNAME As String
        Public URBFLNM As String
        Public FIMP As Single
        Public FCIMP As Single
        Public CURBDEN As Single
        Public URBCOEF As Single
        Public DIRTMX As Single
        Public THALF As Single
        Public TNCONC As Single
        Public TPCONC As Single
        Public TNO3CONC As Single
        Public OV_N As Single
        Public CN2A As Single
        Public CN2B As Double
        Public CN2C As Double
        Public CN2D As Double
        Public URBCN2 As Single

        Public Sub New()
        End Sub

        Public Sub New(ByVal aRow As DataRow)
            With aRow
                IUNUM = .Item("IUNUM")
                URBNAME = .Item("URBNAME")
                URBFLNM = .Item("URBFLNM")
                FIMP = .Item("FIMP")
                FCIMP = .Item("FCIMP")
                CURBDEN = .Item("CURBDEN")
                URBCOEF = .Item("URBCOEF")
                DIRTMX = .Item("DIRTMX")
                THALF = .Item("THALF")
                TNCONC = .Item("TNCONC")
                TPCONC = .Item("TPCONC")
                TNO3CONC = .Item("TNO3CONC")
                OV_N = .Item("OV_N")
                CN2A = .Item("CN2A")
                CN2B = .Item("CN2B")
                CN2C = .Item("CN2C")
                CN2D = .Item("CN2D")
                URBCN2 = .Item("URBCN2")
            End With
        End Sub

        Public Function AddSQL() As String
            Return "INSERT INTO urban ( " & clsDataColumn.ColumnNames(clsHru.Columns) & " ) Values (" _
                   & IUNUM & ", " _
                   & URBNAME & ", " _
                   & URBFLNM & ", " _
                   & FIMP & ", " _
                   & FCIMP & ", " _
                   & CURBDEN & ", " _
                   & URBCOEF & ", " _
                   & DIRTMX & ", " _
                   & THALF & ", " _
                   & TNCONC & ", " _
                   & TPCONC & ", " _
                   & TNO3CONC & ", " _
                   & OV_N & ", " _
                   & CN2A & ", " _
                   & CN2B & ", " _
                   & CN2C & ", " _
                   & CN2D & ", " _
                   & URBCN2 & " )"
        End Function
    End Class

    Public Class clsUrban
        Private pSwatInput As SwatInput
        Private pTableName As String = "urban"

        Friend Shared Columns As Generic.List(Of clsDataColumn)

        Friend Sub New(ByVal aSwatInput As SwatInput)
            pSwatInput = aSwatInput
            If Columns Is Nothing Then InitColumns()
        End Sub

        Private Shared Sub InitColumns()
            Columns = New Generic.List(Of clsDataColumn)
            Columns.Add(New clsDataColumn("IUNUM", 1, "Double", "0", 3, ""))
            Columns.Add(New clsDataColumn("URBNAME", 1, "VARCHAR(4)", "%s", 5, ""))
            Columns.Add(New clsDataColumn("URBFLNM", 1, "VARCHAR(54)", "%s", 56, ""))
            Columns.Add(New clsDataColumn("FIMP", 1, "Single", "0.000", 8, ""))
            Columns.Add(New clsDataColumn("FCIMP", 1, "Single", "0.000", 8, vbCrLf))
            Columns.Add(New clsDataColumn("CURBDEN", 1, "Single", "0.000", 12, ""))
            Columns.Add(New clsDataColumn("URBCOEF", 1, "Single", "0.000", 8, ""))
            Columns.Add(New clsDataColumn("DIRTMX", 1, "Single", "0.000", 8, ""))
            Columns.Add(New clsDataColumn("THALF", 1, "Single", "0.000", 8, ""))
            Columns.Add(New clsDataColumn("TNCONC", 1, "Single", "0.000", 8, ""))
            Columns.Add(New clsDataColumn("TPCONC", 1, "Single", "0.000", 8, ""))
            Columns.Add(New clsDataColumn("TNO3CONC", 1, "Single", "0.000", 8, ""))
            Columns.Add(New clsDataColumn("OV_N", 1, "Single", "", 8, ""))
            Columns.Add(New clsDataColumn("CN2A", 1, "Single", "", 8, ""))
            Columns.Add(New clsDataColumn("CN2B", 1, "Double", "", 8, ""))
            Columns.Add(New clsDataColumn("CN2C", 1, "Double", "", 8, ""))
            Columns.Add(New clsDataColumn("CN2D", 1, "Double", "", 8, ""))
            Columns.Add(New clsDataColumn("URBCN2", 1, "Single", "0.0", 6, ""))
        End Sub

        Public Function TableCreate() As Boolean
        End Function

        Public Function Table() As DataTable
            pSwatInput.Status("Reading " & pTableName & " from database ...")
            Return pSwatInput.QueryGDB("SELECT * FROM " & pTableName & ";")
        End Function

        Public Sub Add(ByVal aItem As clsUrbanItem)
            ExecuteNonQuery(aItem.AddSQL, pSwatInput.CnSwatParm)
        End Sub

        Public Function FindUrban(ByVal aUrbanName As String, Optional ByVal aTable As DataTable = Nothing) As clsUrbanItem
            If aTable Is Nothing Then aTable = Table()
            For Each lRow As DataRow In aTable.Rows
                If lRow.Item("URBNAME").ToString = aUrbanName Then
                    Return New clsUrbanItem(lRow)
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
