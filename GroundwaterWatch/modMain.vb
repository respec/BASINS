Imports MapWinUtility
Imports atcData
Imports atcUtility
Imports atcUSGSBaseflow

Module modMain
    Dim pConsoleDebug As Boolean = False
    Sub Main()
        Dim args() As String = System.Environment.GetCommandLineArgs()
        If args.Count < 2 Then
            Exit Sub
        End If
        Dim lSpecFile As String = args(1)
        If Not IO.File.Exists(lSpecFile) Then
            Dim lcurdir As String = PathNameOnly(System.Reflection.Assembly.GetEntryAssembly.Location)
            If pConsoleDebug Then
                Console.WriteLine(lcurdir)
            End If
            lSpecFile = IO.Path.Combine(lcurdir, lSpecFile)
        End If
        If Not IO.File.Exists(lSpecFile) Then
            Exit Sub
        End If
        Dim lBasicAttributes As Generic.List(Of String) = atcDataManager.DisplayAttributes
        atcTimeseriesStatistics.atcTimeseriesStatistics.InitializeShared()
        Try
            DoBatch(lSpecFile)
        Catch ex As Exception
            Console.WriteLine("Batch run failed.")
        End Try
    End Sub
End Module
