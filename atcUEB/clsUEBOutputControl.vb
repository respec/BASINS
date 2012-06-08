Imports atcData
Imports atcUtility
Imports MapWinUtility.Strings

Public Class clsUEBOutputControl
    Public Variables As Generic.List(Of clsUEBVariable)
    'Public PointDetails As Generic.List(Of )
    Public Header As String

    Public FileName As String

    Public Sub New(ByVal aFilename As String)

        FileName = aFilename
        Dim lFileContents As String
        If IO.File.Exists(FileName) Then
            lFileContents = WholeFileString(FileName)
        Else
            lFileContents = GetEmbeddedFileAsString("OutputControl.dat")
        End If
        'read header line in file
        Header = StrSplit(lFileContents, vbCrLf, "")

        While lFileContents.Length > 0
            If lFileContents.ToLower.StartsWith("pointdetail") Then
            Else
                Variables.Add(clsUEBVariable.FromOutputVariableString(lFileContents))
            End If
        End While
    End Sub

End Class
