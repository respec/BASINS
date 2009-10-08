Imports atcClimateAssessmentTool
Imports atcUtility
Imports MapWinUtility

''' <summary>
''' batch CAT for use from command line
''' </summary>
''' <remarks></remarks>
Module modBatchCAT
    Public Function Main(ByVal args() As String) As Integer
        Try
            If args.Count = 0 Then
                Logger.Msg("ERROR - No Command Line Argument Containing CAT Scenario Found", "BatchCAT Problem")
                Logger.Dbg("ReturnWithCode 1")
                Return 1
            Else
                Dim lCatSpecFile As String = args(0)
                If IO.File.Exists(lCatSpecFile) Then
                    Dim lPath As String = IO.Path.GetDirectoryName(lCatSpecFile)
                    My.Computer.FileSystem.CurrentDirectory = lPath
                    Logger.StartToFile("logs\" & Format(Now, "yyyy-MM-dd") & "at" & Format(Now, "HH-mm") & "-BatchCAT.txt", , False)
                    Logger.AutoFlush = True
                    Initialize()

                    Dim lCat As New atcClimateAssessmentTool.clsCat
                    With lCat
                        .XML = atcUtility.WholeFileString(lCatSpecFile)
                        .RunModel = False
                        Logger.Dbg("StartRunWith " & lCat.Inputs.Count & " Inputs")
                        .StartRun("ModifyOriginal")
                    End With
                    Logger.Dbg("BatchCAT:AllDone")
                    Logger.Dbg("ReturnWithCode 0")
                    Return 0
                Else
                    Logger.Msg("ERROR - CAT specification file '" & lCatSpecFile & "' not found")
                    Logger.Dbg("ReturnWithCode 2")
                    Return 2
                End If
            End If
        Catch e As Exception
            Logger.Msg("ERROR - " & e.Message)
            Logger.Dbg("ReturnWithCode 3")
            Return 3
        End Try
        Logger.Dbg("ReturnWithCode 4")
        Return 4
    End Function

    Sub Initialize()
        atcData.atcDataManager.Clear()
        Dim lPlugIn As atcData.atcDataPlugin
        lPlugIn = New atcWDM.atcDataSourceWDM()
        lPlugIn.Initialize(Nothing, 0)
        lPlugIn = New atcTimeseriesMath.atcTimeseriesMath
        lPlugIn.Initialize(Nothing, 0)
        Logger.Dbg("SetNaN")
        Dim lNan As Double = GetNaN()
        Dim lMax As Double = GetMaxValue()
    End Sub
End Module
