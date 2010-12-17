Imports System.Collections.ObjectModel

Public Interface IBlock
    Property Name As String
    Sub FromString(ByVal aContents As String)
End Interface

Public Class atcSWMMBlocks
    Inherits KeyedCollection(Of String, IBlock)
    Protected Overrides Function GetKeyForItem(ByVal aBlock As IBlock) As String
        Return aBlock.Name
    End Function
End Class

Public Class atcSWMMBlock
    Implements IBlock
    Private pName As String
    Private pContents As String

    Property Name() As String Implements IBlock.Name
        Get
            Return pName
        End Get
        Set(ByVal value As String)
            pName = value
        End Set
    End Property

    Property Content() As String
        Get
            Return pContents
        End Get
        Set(ByVal value As String)
            pContents = value
        End Set
    End Property

    Public Sub New(ByVal aName As String, ByVal aContents As String)
        Name = aName
        pContents = aContents
    End Sub

    Public Sub FromString(ByVal aContents As String) Implements IBlock.FromString
        pContents = aContents
    End Sub

    Public Overrides Function ToString() As String
        Dim lTempString As String
        If pContents.Length = 0 Then
            lTempString = Name & vbCrLf
        Else
            lTempString = Name & vbCrLf & pContents & vbCrLf
        End If
        Return lTempString
    End Function
End Class
