'OBJECTID,IPNUM,PNAME,SKOC,WOF,HLIFE_F,HLIFE_S,AP_EF,WSOL,HENRY,PESTNAME
Partial Class SwatInput
    Private pPest As clsPest = New clsPest(Me)
    ReadOnly Property Pest() As clsPest
        Get
            Return pPest
        End Get
    End Property

    Public Class clsPestItem
        Public IPNUM As Integer
        Public PNAME As String '17
        Public SKOC As Double
        Public WOF As Single
        Public HLIFE_F As Single
        Public HLIFE_S As Single
        Public AP_EF As Single
        Public WSOL As Double
        Public HENRY As Double
        Public PESTNAME As String '30

        Public Sub New()
        End Sub

        Sub New(ByVal aRow As DataRow)
            PopulateObject(aRow, Me)
        End Sub

        Public Function AddSQL() As String
            Return "INSERT INTO Pest ( " & clsDataColumn.ColumnNames(clsHru.Columns) & " ) Values (" _
                   & IPNUM & ", " _
                   & PNAME & ", " _
                   & SKOC & ", " _
                   & WOF & ", " _
                   & HLIFE_F & ", " _
                   & HLIFE_S & ", " _
                   & AP_EF & ", " _
                   & WSOL & ", " _
                   & HENRY & ", " _
                   & PESTNAME & " )"
        End Function
    End Class

    Public Class clsPest
        Private pSwatInput As SwatInput
        Private pTableName As String = "Pest"

        Friend Shared Columns As Generic.List(Of clsDataColumn)

        Friend Sub New(ByVal aSwatInput As SwatInput)
            pSwatInput = aSwatInput
            If Columns Is Nothing Then InitColumns()
        End Sub

        Private Shared Sub InitColumns()
            Columns = New Generic.List(Of clsDataColumn)

            Columns.Add(New clsDataColumn("IPNUM", 1, "Integer", "0", 3, ""))
            Columns.Add(New clsDataColumn("PNAME", 1, "VARCHAR(17)", "%s", 17, ""))
            Columns.Add(New clsDataColumn("SKOC", 1, "Double", "0.0", 10, ""))
            Columns.Add(New clsDataColumn("WOF", 1, "Single", "0.00", 5, ""))
            Columns.Add(New clsDataColumn("HLIFE_F", 1, "Single", "0.0", 8, ""))
            Columns.Add(New clsDataColumn("HLIFE_S", 1, "Single", "0.0", 8, ""))
            Columns.Add(New clsDataColumn("AP_EF", 1, "Single", "0.00", 5, ""))
            Columns.Add(New clsDataColumn("WSOL", 1, "Double", "0.000", 11, ""))
            Columns.Add(New clsDataColumn("HENRY", 1, "Double", "", 8, ""))
            Columns.Add(New clsDataColumn("PESTNAME", 1, "VARCHAR(30)", "", 8, ""))
        End Sub

        Public Function TableCreate() As Boolean
        End Function

        Public Function Table() As DataTable
            pSwatInput.Status("Reading " & pTableName & " from database ...")
            Return pSwatInput.QueryGDB("SELECT * FROM " & pTableName & ";")
        End Function

        Public Sub Add(ByVal aItem As clsPestItem)
            ExecuteNonQuery(aItem.AddSQL, pSwatInput.CnSwatParm)
        End Sub

        Public Function FindPest(ByVal aPestName As String, Optional ByVal aTable As DataTable = Nothing) As clsPestItem
            If aTable Is Nothing Then aTable = Table()
            For Each lRow As DataRow In aTable.Rows
                If lRow.Item("PNAME").ToString = aPestName Then
                    Return New clsPestItem(lRow)
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
