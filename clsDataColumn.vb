Public Class clsDataColumn
    Public Name As String
    Public Count As Integer
    Public TypeString As String
    Public TextFormat As String
    Public TextPad As Integer
    Public TextSuffix As String

    Public Sub New(ByVal aName As String, ByVal aCount As Integer, ByVal aTypeString As String, ByVal aTextFormat As String, ByVal aTextPad As Integer, ByVal aTextSuffix As String)
        Name = aName
        Count = aCount
        TypeString = aTypeString
        TextFormat = aTextFormat
        TextPad = aTextPad
        TextSuffix = aTextSuffix
    End Sub

    Shared Function ColumnNames(ByVal aDataColumns As Generic.List(Of clsDataColumn)) As String
        Dim lSB As New Text.StringBuilder
        For Each lField As clsDataColumn In aDataColumns
            With lField
                If .Count > 1 Then
                    For lColumnCountIndex As Integer = 1 To .Count
                        lSB.Append(.Name & lColumnCountIndex & ", ")
                    Next
                Else
                    lSB.Append(.Name & ", ")
                End If
            End With
        Next
        Return lSB.ToString.Substring(0, lSB.Length - 2)
    End Function
End Class
