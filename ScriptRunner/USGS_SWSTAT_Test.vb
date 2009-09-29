Imports atcData
Imports atcUtility
Imports atcWDM
Imports atcswstat
Imports MapWindow.Interfaces
Imports MapWinUtility

Public Module Script_USGS_SWSTAT_Test
    Private pBaseDrive As String = "c:"
    Private pBaseFolder As String = pBaseDrive & "\Dev\BASINS40\ScriptRunner\Data\USGS\SWSTAT\Current"


    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Dim lOrigCurrentDirectory As String = My.Computer.FileSystem.CurrentDirectory
        My.Computer.FileSystem.CurrentDirectory = pBaseFolder
        Dim lOrigLogFileName As String = Logger.FileName
        Logger.Dbg("StartingScript_USGS_SWSTAT_Test")
        Logger.StartToFile("Script_USGS_SWSTAT_Test.log", , , True)
        Logger.AutoFlush = True

        Dim lFileName As String = "test.wdm"
        IO.File.Copy("..\base\" & lFileName, lFileName, True)
        Logger.Dbg("FileCopied " & lFileName)

        Dim lWdmFile As New atcWDM.atcDataSourceWDM
        If lWdmFile.Open(lFileName) Then
            Logger.Dbg("FileOpened " & lFileName)
            Dim lSB As New Text.StringBuilder
            Dim lCalculator As New atcTimeseriesNdayHighLow.atcTimeseriesNdayHighLow
            Dim lArgs As New atcDataAttributes

            'data for test 1 & 2
            Dim lTimeseriesGroup As atcTimeseriesGroup = lWdmFile.DataSets.FindData("ID", 3)
            Dim lTimeseries As atcTimeseries = lTimeseriesGroup.ItemByIndex(0)

            'test 1
            lArgs.SetValue("Timeseries", lTimeseriesGroup)
            Dim lNDay As Double() = {7}
            lArgs.SetValue("NDay", lNDay)
            Dim lRecurrence As Double() = {100, 50, 20, 10, 5, 3, 2, 1.25, 1.11, 1.04, 1.02, 1.01}
            lArgs.SetValue("Return Period", lRecurrence)
            lArgs.SetValue("LogFlg", True)
            lArgs.SetValue("BoundaryMonth", 6)
            lArgs.SetValue("BoundaryDay", 15)
            lArgs.SetValue("EndMonth", 9)
            lArgs.SetValue("EndDay", 15)
            lArgs.SetValue("FirstYear", 1960)
            lArgs.SetValue("LastYear", 1970)
            lCalculator.Clear()
            lCalculator.Open("n-day Low value", lArgs)
            lCalculator.DataSets.Clear()
            Dim lFrequencyGridSource As New atcFrequencyGridSource(lTimeseriesGroup)
            With lFrequencyGridSource
                .High = False
                lSB.Append(.CreateReport)
            End With

            'test 2
            lArgs.Clear()
            lTimeseries.Attributes.DiscardCalculated()
            lArgs.SetValue("Timeseries", lTimeseriesGroup)
            lArgs.SetValue("NDay", lNDay)
            lRecurrence = New Double() {200, 100, 50, 25, 10, 5, 3, 2, 1.25, 1.11, 1.05, 1.01}
            lArgs.SetValue("Return Period", lRecurrence)
            lArgs.SetValue("LogFlg", True)
            lArgs.SetValue("BoundaryMonth", 6)
            lArgs.SetValue("BoundaryDay", 15)
            lArgs.SetValue("EndMonth", 9)
            lArgs.SetValue("EndDay", 15)
            lArgs.SetValue("FirstYear", 1960)
            lArgs.SetValue("LastYear", 1970)
            lCalculator.Clear()
            lCalculator.Open("n-day High value", lArgs)
            lCalculator.DataSets.Clear()
            lFrequencyGridSource = New atcFrequencyGridSource(lTimeseriesGroup)
            With lFrequencyGridSource
                .High = True
                lSB.Append(.CreateReport)
            End With

            'data for test 3
            lTimeseriesGroup.Clear()
            lTimeseriesGroup = lWdmFile.DataSets.FindData("ID", 15)
            lTimeseries = lTimeseriesGroup.ItemByIndex(0)

            'test 3
            lArgs.Clear()
            lArgs.SetValue("Timeseries", lTimeseriesGroup)
            lArgs.SetValue("NDay", lNDay)
            lRecurrence = New Double() {100, 50, 20, 10, 5, 3, 2, 1.25, 1.11, 1.04, 1.02, 1.01}
            lArgs.SetValue("Return Period", lRecurrence)
            lArgs.SetValue("LogFlg", True)
            lArgs.SetValue("BoundaryMonth", 4)
            lArgs.SetValue("BoundaryDay", 1)
            lArgs.SetValue("EndMonth", 3)
            lArgs.SetValue("EndDay", 31)
            lArgs.SetValue("FirstYear", 1950)
            lArgs.SetValue("LastYear", 1956)

            lCalculator.Clear()
            lCalculator.Open("n-day Low value", lArgs)
            lCalculator.DataSets.Clear()
            lFrequencyGridSource = New atcFrequencyGridSource(lTimeseriesGroup)
            With lFrequencyGridSource
                .High = False
                lSB.Append(.CreateReport)
            End With

            'data for test 4
            lTimeseriesGroup.Clear()
            lTimeseriesGroup = lWdmFile.DataSets.FindData("ID", 16)
            lTimeseries = lTimeseriesGroup.ItemByIndex(0)

            'test 3
            lArgs.Clear()
            lArgs.SetValue("Timeseries", lTimeseriesGroup)
            lArgs.SetValue("NDay", lNDay)
            lRecurrence = New Double() {100, 50, 20, 10, 5, 3, 2, 1.25, 1.11, 1.04, 1.02, 1.01}
            lArgs.SetValue("Return Period", lRecurrence)
            lArgs.SetValue("LogFlg", True)
            lArgs.SetValue("BoundaryMonth", 4)
            lArgs.SetValue("BoundaryDay", 1)
            lArgs.SetValue("EndMonth", 3)
            lArgs.SetValue("EndDay", 31)
            lArgs.SetValue("FirstYear", 1929)
            lArgs.SetValue("LastYear", 1947)

            lCalculator.Clear()
            lCalculator.Open("n-day Low value", lArgs)
            lCalculator.DataSets.Clear()
            lFrequencyGridSource = New atcFrequencyGridSource(lTimeseriesGroup)
            With lFrequencyGridSource
                .High = False
                lSB.Append(.CreateReport)
            End With

            SaveFileString("test7.ot2", lSB.ToString)
        Else
            Logger.Dbg("FileNotOpened " & lFileName)
        End If

        My.Computer.FileSystem.CurrentDirectory = lOrigCurrentDirectory
        Logger.StartToFile(lOrigLogFileName, True, , True)
        Logger.Dbg("CompletedScript_USGS_SWSTAT_Test")
    End Sub
End Module
