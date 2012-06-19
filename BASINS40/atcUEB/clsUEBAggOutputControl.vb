Imports atcUtility
Imports MapWinUtility.Strings
Imports System.IO

Public Class clsUEBAggOutputControl
    Public Variables As Generic.List(Of clsUEBVariable)
    Public Header As String

    Public FileName As String

    Public Sub New(ByVal aFilename As String)

        FileName = aFilename
        Dim lFileContents As String
        If IO.File.Exists(FileName) Then
            lFileContents = WholeFileString(FileName)
        Else
            lFileContents = ""
        End If
        'read header line in file
        Header = StrSplit(lFileContents, vbCrLf, "")
        Variables = New Generic.List(Of clsUEBVariable)
        While lFileContents.Length > 0
            Variables.Add(clsUEBVariable.FromAggOutputVariableString(lFileContents))
        End While
    End Sub

    Public Function WriteAggOutputControlFile() As Boolean

        Dim lStr As String = ""

        If FileName.Length > 0 Then
            Try
                For Each lUEBParm As clsUEBVariable In Variables
                    lStr &= lUEBParm.Description & vbCrLf
                Next
                SaveFileString(FileName, lStr)
                Return True
            Catch ex As Exception
                Return False
            End Try
        Else
            Return False
        End If
    End Function

End Class
