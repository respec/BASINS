Imports System.Collections.ObjectModel

Public Interface IBlock
    Property Name As String
    Sub FromString(ByVal aContents As String)
End Interface

Public Class Blocks
    Inherits KeyedCollection(Of String, IBlock)
    Protected Overrides Function GetKeyForItem(ByVal aBlock As IBlock) As String
        Return aBlock.Name
    End Function
End Class

Public Class Block
    Implements IBlock

    Property Name As String Implements IBlock.Name
    Private Contents As String

    Public Sub New(ByVal aName As String, ByVal aContents As String)
        Name = aName
        Contents = aContents
    End Sub

    Public Sub FromString(ByVal aContents As String) Implements IBlock.FromString
        Contents = aContents
    End Sub

    Public Overrides Function ToString() As String
        Return Name & vbCrLf & Contents & vbCrLf
    End Function
End Class
