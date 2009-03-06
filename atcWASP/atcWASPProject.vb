Imports MapWinUtility
Imports System.Text
Imports atcUtility

Public Class WASPProject
    Public Segments As Segments
    Public InputTimeseriesCollection As WASPTimeseriesCollection

    Public Name As String = ""
    Public WNFFileName As String = ""

    Public Sub New()
        Name = ""
        WNFFileName = ""
        Segments = New Segments
        Segments.WASPProject = Me
        InputTimeseriesCollection = New WASPTimeseriesCollection
    End Sub

    Public Function Save(ByVal aFileName As String) As Boolean
        'write WASP network file first
        Dim lSW As New IO.StreamWriter(aFileName)
        WNFFileName = aFileName

        Dim lSegmentFileName As String = FilenameSetExt(WNFFileName, "SEG")

        lSW.WriteLine(lSegmentFileName)
        lSW.Close()

        'now write segments file
        lSW = New IO.StreamWriter(lSegmentFileName)
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
