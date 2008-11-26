Imports MapWinUtility
Imports MapWindow.Interfaces
Imports atcUtility
Imports atcClimateAssessmentTool

Module CatRunner
    Private pBaseDrive As String = "C:"
    Private pBaseFolder As String
    Private pBaseFolders As New ArrayList
    'Private pCatXMLFile As String = "VaryPrecTempHbnPrepare4.xml"
    Private pCatXMLFile As String = "VaryPrecTempHbnPreparehl.xml"
    'Private pCatXMLFile As String = "zVaryPrecTempHbnPrepare_short.xml"
    'Private pCatXMLFile As String = "VaryPrecTempHbnPrepareFlow2.xml"
    Private WithEvents pCat As New atcClimateAssessmentTool.clsCat
    Private pRunModel As Boolean = False
    Private pEdit As Boolean = False

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)

        'Add scenario directories
        'pBaseFolders.Add(pBaseDrive & "\mono_luChange\output\lu2030a2")
        'pBaseFolders.Add(pBaseDrive & "\mono_luChange\output\lu2030b2")
        'pBaseFolders.Add(pBaseDrive & "\mono_luChange\output\lu2090a2")
        'pBaseFolders.Add(pBaseDrive & "\mono_luChange\output\lu2090b2")
        pBaseFolders.Add(pBaseDrive & "\mono_luChange\output\lu2090a2bmp")
        pBaseFolders.Add(pBaseDrive & "\mono_luChange\output\lu2090b2bmp")

        'pBaseFolders.Add(pBaseDrive & "\mono_luChange\output\Mono_10")
        'pBaseFolders.Add(pBaseDrive & "\mono_luChange\output\Mono_70")

        'pBaseFolders.Add(pBaseDrive & "\mono_luChange\output\lu2030a2bmp")
        'pBaseFolders.Add(pBaseDrive & "\mono_luChange\output\lu2030b2bmp")
        'pBaseFolders.Add(pBaseDrive & "\mono_luChange\output\Mono10bmp")
        'pBaseFolders.Add(pBaseDrive & "\mono_luChange\output\Mono70bmp")
        'pBaseFolders.Add(pBaseDrive & "\mono_luChange\output\Mono70bmpDbg")

        For Each pBaseFolder In pBaseFolders
            ChDriveDir(pBaseFolder)
            Logger.StartToFile("CatRunner.Log", , , True)
            Logger.DisplayMessageBoxes = False
            Dim lResultsFileName As String = "CatRunnerResults70echmF10.txt"
            With pCat
                Dim lFrmCat As frmCAT = Nothing
                If pEdit Then
                    lFrmCat = New frmCAT
                    lFrmCat.Initialize(Nothing, pCat)
                    lFrmCat.NoExecutionAllowed()
                End If
                .XML = IO.File.ReadAllText(pCatXMLFile)
                While lFrmCat IsNot Nothing AndAlso lFrmCat.Visible = True
                    Windows.Forms.Application.DoEvents()
                End While
                If Not pRunModel Then
                    .RunModel = False
                    lResultsFileName = "CatRunnerResultsLowHighFlow.txt"
                End If
                .StartRun("Modified")
                Logger.Dbg("RunsComplete")
                IO.File.WriteAllText(lResultsFileName, .ResultsGrid.ToString)
                .Inputs.Clear()
                .Endpoints.Clear()
                .PreparedInputs.Clear()
                System.GC.Collect()
                System.GC.WaitForPendingFinalizers()
            End With
            Logger.Dbg("ResultsStored")
            Logger.Flush()
        Next
    End Sub

    Private Sub UpdateStatusLabel(ByVal aIteration As Integer) Handles pCat.StartIteration
        pCat.StartIterationMessage(aIteration)
    End Sub

    Private Sub UpdateResults(ByVal aResultsFilename As String) Handles pCat.UpdateResults
        Try
            Windows.Forms.Application.DoEvents()
        Catch
            'stop
        End Try
        SaveFileString(aResultsFilename, pCat.ResultsGrid.ToString)
    End Sub
End Module
