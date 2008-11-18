Imports MapWinUtility
Imports System.Text
Imports atcUtility

Public Class WASPProject
    Public Segments As Segments

    Public Name As String = ""
    Public FileName As String = ""

    Public Sub New()
        Name = ""
        FileName = ""
    End Sub

    Public Function Save(ByVal aFileName As String) As Boolean
        Dim lSW As New IO.StreamWriter(aFileName)
        FileName = aFileName

        lSW.WriteLine(Name)
        lSW.WriteLine("")

        'write segments to file
        lSW.WriteLine(Segments.ToString)

        lSW.Close()

        Return True
    End Function

    Public Sub Run(ByVal aInputFileName As String)
        If IO.File.Exists(aInputFileName) Then
            Dim lWASPexe As String = atcUtility.FindFile("Please locate the EPA WASP Executable", "\Program Files\EPA WASP\WASP.exe")
            If IO.File.Exists(lWASPexe) Then
                LaunchProgram(lWASPexe, IO.Path.GetDirectoryName(aInputFileName), "/f " & aInputFileName, False)
                Logger.Dbg("WASP launched with input " & aInputFileName)
            Else
                Logger.Msg("Cannot find the EPA WASP Executable", MsgBoxStyle.Critical, "BASINS WASP Problem")
            End If
        Else
            Logger.Msg("Cannot find WASP Input File " & aInputFileName)
        End If
    End Sub
End Class
