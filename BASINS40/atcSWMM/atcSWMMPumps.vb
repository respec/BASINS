Imports System.Collections.ObjectModel
Imports System.IO
Imports MapWinUtility
Imports atcUtility
Imports System.Text

Public Class atcSWMMPumps
    Inherits KeyedCollection(Of String, atcSWMMPump)
    Implements IBlock

    Private pName As String
    Private pSWMMProject As atcSWMMProject

    Property Name() As String Implements IBlock.Name
        Get
            Return pName
        End Get
        Set(ByVal value As String)
            pName = value
        End Set
    End Property

    Protected Overrides Function GetKeyForItem(ByVal aPump As atcSWMMPump) As String
        Dim lKey As String = aPump.Name
        Return lKey
    End Function

    Public Sub New(ByVal aSWMMPRoject As atcSWMMProject)
        Name = "[PUMPS]"
        pSWMMProject = aSWMMPRoject
    End Sub

    Public Sub AddRange(ByVal aEnumerable As IEnumerable)
        For Each lPump As atcSWMMPump In aEnumerable
            Me.Add(lPump)
        Next
    End Sub

    'Temporarily just save the text for now
    Private pText As String
    Public Property Text() As String
        Get
            Return pText
        End Get
        Set(ByVal value As String)
            pText = value
        End Set
    End Property
    Public Sub FromString(ByVal aContents As String) Implements IBlock.FromString
        'TODO
        Text = aContents
    End Sub

    Public Overrides Function ToString() As String
        Return Text()
    End Function
End Class

Public Class atcSWMMPump
    Public Name As String
    Public Content As String
End Class