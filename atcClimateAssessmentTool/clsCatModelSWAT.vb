Imports atcData
Imports atcUtility
Imports MapWinUtility

Public Class clsCatModelSWAT
    Implements clsCatModel

    Public SWATProgramBase As String = "C:\Program Files\SWAT 2005 Editor\"
    Public SWATDatabaseName As String = SWATProgramBase & "\Databases\SWAT2005.mdb"

    Public Event BaseScenarioSet(ByVal aBaseScenario As String) Implements clsCatModel.BaseScenarioSet

    Private pBaseScenario As String = ""

    Public Property BaseScenario() As String Implements clsCatModel.BaseScenario
        Get
            Return pBaseScenario
        End Get
        Set(ByVal newValue As String)
            OpenBaseScenario(newValue)
        End Set
    End Property

    Friend Sub OpenBaseScenario(Optional ByVal aFilename As String = "")
        If Not aFilename Is Nothing AndAlso Not IO.File.Exists(aFilename) Then
            If IO.File.Exists(aFilename & ".mdb") Then aFilename &= ".mdb"
        End If

        If aFilename Is Nothing OrElse Not IO.File.Exists(aFilename) Then
            Dim cdlg As New Windows.Forms.OpenFileDialog
            cdlg.Title = "Open SWAT file containing base scenario"
            cdlg.Filter = "SWAT mdb files|*.mdb|All Files|*.*"
            If cdlg.ShowDialog = Windows.Forms.DialogResult.OK Then
                aFilename = cdlg.FileName
            End If
        End If

        If aFilename.ToLower <> pBaseScenario.ToLower AndAlso IO.File.Exists(aFilename) Then
            Dim lFolder As String = PathNameOnly(aFilename)
            ChDriveDir(lFolder)
            pBaseScenario = aFilename
            RaiseEvent BaseScenarioSet(aFilename)
        End If
    End Sub

    Public Function ScenarioRun(ByVal aNewScenarioName As String, _
                                ByVal aModifiedData As atcData.atcTimeseriesGroup, _
                                ByVal aPreparedInput As String, _
                                ByVal aRunModel As Boolean, _
                                ByVal aShowProgress As Boolean, _
                                ByVal aKeepRunning As Boolean) As atcUtility.atcCollection _
                                                        Implements clsCatModel.ScenarioRun
        Dim lProjectFolder As String = IO.Path.GetTempPath & aNewScenarioName
        Dim lSwatInput As New SwatObject.SwatInput(SWATDatabaseName, pBaseScenario, lProjectFolder, aNewScenarioName)
        lSwatInput.SaveAllTextInput()
        'TODO: write modified data
        If aRunModel Then
            Dim lInputFilePath As String = IO.Path.Combine(lProjectFolder, "Scenarios\" & aNewScenarioName & "\TxtInOut")
            Logger.Dbg("StartModel")
            Dim lSWATexePath As String = IO.Path.Combine(SWATProgramBase, "Swat2005.exe")
            If Not IO.File.Exists(lSWATexePath) Then
                lSWATexePath = FindFile("Please Locate Swat2005.exe", lSWATexePath)
                If IO.File.Exists(lSWATexePath) Then
                    SWATProgramBase = IO.Path.GetDirectoryName(lSWATexePath)
                End If
            End If
            If IO.File.Exists(lSWATexePath) Then
                LaunchProgram(lSWATexePath, lInputFilePath)
                Logger.Dbg("DoneModelRun")
            Else
                Logger.Dbg("SWAT exe not found, skipping model run")
            End If
        End If
        Dim lModified As New atcCollection
        'TODO: read written data for endpoints
        Return lModified
    End Function

    Public Property XML() As String Implements clsCatModel.XML
        Get
            Dim lXML As String = ""
            lXML &= "<SWAT>" & vbCrLf
            lXML &= "  <FileName>" & pBaseScenario & "</FileName>" & vbCrLf
            lXML &= "</SWAT>" & vbCrLf
            Return lXML
        End Get
        Set(ByVal value As String)

        End Set
    End Property
End Class
