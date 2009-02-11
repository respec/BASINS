Imports System.Collections.ObjectModel
Imports System.IO
Imports MapWinUtility
Imports atcUtility
Imports System.Text

Public Class atcSWMMFieldDetails
    Inherits KeyedCollection(Of String, FieldDetail)
    Protected Overrides Function GetKeyForItem(ByVal aFieldDetail As FieldDetail) As String
        Dim lKey As String = aFieldDetail.LongName
        Return lKey
    End Function
End Class

Public Class FieldDetail
    Public LongName As String
    Public ShortName As String
    Public DefaultValue As String
    Public Width As Integer
    Public Type As Integer  'string is type 0, integer is type 1, double is type 2
End Class
