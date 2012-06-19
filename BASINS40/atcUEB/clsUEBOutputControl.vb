Imports atcData
Imports atcUtility
Imports MapWinUtility.Strings

Public Class clsUEBOutputControl
    Public Variables As Generic.List(Of clsUEBVariable)
    Public PointDetails As Generic.List(Of System.Drawing.Point)
    Public PointFileNames As Generic.List(Of String)
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
        PointDetails = New Generic.List(Of System.Drawing.Point)
        PointFileNames = New Generic.List(Of String)
        While lFileContents.Length > 0
            If lFileContents.ToLower.StartsWith("pointdetail") Then 'read X/Y pair
                Dim lStr As String
                Dim lPt As New System.Drawing.Point
                lStr = StrSplit(lFileContents, vbCrLf, "") 'read "pointdetail" record
                lStr = StrSplit(lFileContents, vbCrLf, "") 'read coordinate record
                lPt.X = Integer.Parse(StrRetRem(lStr))
                lPt.Y = Integer.Parse(StrRetRem(lStr))
                PointDetails.Add(lPt)
                lStr = StrSplit(lFileContents, vbCrLf, "")
                PointFileNames.Add(lStr)
            Else
                Variables.Add(clsUEBVariable.FromOutputVariableString(lFileContents))
            End If
        End While

    End Sub

    Public Function WriteOutputControlFile() As Boolean

        Dim lStr As String = ""

        If FileName.Length > 0 Then
            Try
                For Each lUEBParm As clsUEBVariable In Variables
                    lStr &= lUEBParm.OutputVariableString
                Next
                For Each lPt As System.Drawing.Point In PointDetails
                    lStr &= "pointdetail: An output point" & vbCrLf & lPt.X & " " & lPt.Y & vbCrLf
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
