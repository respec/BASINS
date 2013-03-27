Imports atcUtility
Imports MapWinUtility.Strings
Imports System.IO

Public Class clsUEBSiteFile

    'Public Shared NumVariables As Integer = VariableName.Length
    Public Variables As Generic.List(Of clsUEBVariable)
    Public Header As String

    Public FileName As String

    Public Sub New(ByVal aFilename As String)
        Dim lAlreadyRead As Boolean = False
        Dim lUEBVariable As clsUEBVariable

        FileName = aFilename
        Dim lFileContents As String
        lFileContents = GetEmbeddedFileAsString("SiteInit.dat")
        Variables = New Generic.List(Of clsUEBVariable)
ReadFile:

        'read header line in file
        Header = StrSplit(lFileContents, vbCrLf, "")
        'Variables = New Generic.List(Of clsUEBVariable)
        While lFileContents.Length > 0
            lUEBVariable = clsUEBVariable.FromSiteVariableString(lFileContents)
            Dim lExistingVariable As clsUEBVariable = VariableFromCode(lUEBVariable.Code)
            If lExistingVariable IsNot Nothing Then
                'just update values, but preserve embedded file Code, LongName, Description, Units
                lExistingVariable.Value = lUEBVariable.Value
                lExistingVariable.SpaceVarying = lUEBVariable.SpaceVarying
                lExistingVariable.GridFileName = lUEBVariable.GridFileName
                lExistingVariable.GridVariableName = lUEBVariable.GridVariableName
                lExistingVariable.GridXVarName = lUEBVariable.GridXVarName
                lExistingVariable.GridYVarName = lUEBVariable.GridYVarName
            Else
                Variables.Add(lUEBVariable)
            End If
        End While

        If Not lAlreadyRead AndAlso IO.File.Exists(FileName) Then
            lFileContents = WholeFileString(FileName)
            lAlreadyRead = True
            GoTo ReadFile
        End If

    End Sub

    Private Function VariableFromCode(ByVal aCode As String) As clsUEBVariable
        For Each lUEBVariable As clsUEBVariable In Variables
            If lUEBVariable.Code.ToUpper = aCode.ToUpper Then
                Return lUEBVariable
            End If
        Next
        Return Nothing
    End Function

    Public Function WriteSiteFile() As Boolean

        Dim lStr As String = Header & vbCrLf

        If FileName.Length > 0 Then
            Try
                For Each lUEBParm As clsUEBVariable In Variables
                    lStr &= lUEBParm.SiteVariableString(IO.Path.GetDirectoryName(FileName))
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
