'OBJECTID,IFNUM,FERTNM,FMINN,FMINP,FORGN,FORGP,FNH3N,BACTPDB,BACTLPDB,BACTKDDB,FERTNAME,MANURE
Partial Class SwatInput
    Private pFert As clsFert = New clsFert(Me)
    ReadOnly Property Fert() As clsFert
        Get
            Return pFert
        End Get
    End Property

    Public Class clsFertItem
        Public IFNUM As Double
        Public FERTNM As String
        Public FMINN As Single
        Public FMINP As Single
        Public FORGN As Single
        Public FORGP As Single
        Public FNH3N As Single
        Public BACTPDB As Single
        Public BACTLPDB As Single
        Public BACTKDDB As Single
        Public FERTNAME As String
        Public MANURE As Integer

        Public Sub New()
        End Sub

        Public Sub New(ByVal aRow As DataRow)
            PopulateObject(aRow, Me)
        End Sub

        Public Function AddSQL() As String
            Dim lSQL As String = "INSERT INTO Fert ( " & clsDataColumn.ColumnNames(clsHru.Columns) & " ) Values (" _
                               & IFNUM & ", " _
                               & FERTNM & ", " _
                               & FMINN & ", " _
                               & FMINP & ", " _
                               & FORGN & ", " _
                               & FORGP & ", " _
                               & FNH3N & ", " _
                               & BACTPDB & ", " _
                               & BACTLPDB & ", " _
                               & BACTKDDB & ", " _
                               & FERTNAME & ", " _
                               & MANURE & " )"
            Return lSQL
        End Function
    End Class

    Public Class clsFert
        Private pSwatInput As SwatInput
        Private pTableName As String = "fert"

        Friend Shared Columns As Generic.List(Of clsDataColumn)

        Friend Sub New(ByVal aSwatInput As SwatInput)
            pSwatInput = aSwatInput
            If Columns Is Nothing Then InitColumns()
        End Sub

        Private Shared Sub InitColumns()
            Columns = New Generic.List(Of clsDataColumn)
            Columns.Add(New clsDataColumn("IFNUM", 1, "Double", "0", 4, ""))
            Columns.Add(New clsDataColumn("FERTNM", 1, "VARCHAR(8)", "%s", 9, ""))
            Columns.Add(New clsDataColumn("FMINN", 1, "Single", "0.000", 8, ""))
            Columns.Add(New clsDataColumn("FMINP", 1, "Single", "0.000", 8, ""))
            Columns.Add(New clsDataColumn("FORGN", 1, "Single", "0.000", 8, ""))
            Columns.Add(New clsDataColumn("FORGP", 1, "Single", "0.000", 8, ""))
            Columns.Add(New clsDataColumn("FNH3N", 1, "Single", "0.000", 8, ""))
            Columns.Add(New clsDataColumn("BACTPDB", 1, "Single", "0.000", 8, ""))
            Columns.Add(New clsDataColumn("BACTLPDB", 1, "Single", "0.00", 10, ""))
            Columns.Add(New clsDataColumn("BACTKDDB", 1, "Single", "0.00", 10, ""))
            Columns.Add(New clsDataColumn("FERTNAME", 1, "VARCHAR(55)", "", 8, ""))
            Columns.Add(New clsDataColumn("MANURE", 1, "Integer", "", 8, ""))
        End Sub

        Public Function TableCreate() As Boolean
        End Function

        Public Function Table() As DataTable
            pSwatInput.Status("Reading " & pTableName & " from database ...")
            Return pSwatInput.QueryGDB("SELECT * FROM " & pTableName & ";")
        End Function

        Public Sub Add(ByVal aItem As clsFertItem)
            ExecuteNonQuery(aItem.AddSQL, pSwatInput.CnSwatParm)
        End Sub

        Public Function FindFert(ByVal aFertName As String, Optional ByVal aTable As DataTable = Nothing) As clsFertItem
            If aTable Is Nothing Then aTable = Table()
            For Each lRow As DataRow In aTable.Rows
                If lRow.Item("FERTNM").ToString = aFertName Then
                    Return New clsFertItem(lRow)
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
