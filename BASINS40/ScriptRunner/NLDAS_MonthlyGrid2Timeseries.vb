Imports System.Collections
Imports System.Collections.Specialized

Imports MapWindow.Interfaces
Imports MapWinUtility

Imports atcUtility
Imports atcData
Imports atcSeasons

Module NLDAS_MonthlyGrid2Timeseries
    Public pPath As String = "E:\Data\NLDAS_Converted"
    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Dim lOriginalFolder As String = IO.Directory.GetCurrentDirectory
        Dim lOriginalLog As String = Logger.FileName

        ChDriveDir(pPath)
        Logger.StartToFile("NLDAS_MonthlyGrid2Timeseries.log", False, False, True)
        Logger.Dbg("NLDAS_MonthlyGrid2Timeseries: Start")

        Dim lXMax As Integer = 64
        Dim lYMax As Integer = 24
        Dim lTimeseries(lXMax, lYMax) As atcTimeseries
        Dim lDates As New atcTimeseries(Nothing)

        Dim lFirstTime As Boolean = True
        Dim lFileNames As NameValueCollection = Nothing
        AddFilesInDir(lFileNames, pPath, True, "*.txt")
        lDates.numValues = lFileNames.Count
        Dim lFileIndex As Integer = 0
        Dim lValue As Double
        For Each lFileName As String In lFileNames
            Dim lX As Integer = -1
            Dim lY As Integer = 0
            For Each lLine As String In LinesInFile(lFileName)
                If lX >= 0 Then
                    If lY > lYMax Then
                        Logger.Dbg("ArrayProblem " & lLine)
                    End If
                    If lFirstTime Then lTimeseries(lX, lY) = New atcTimeseries(Nothing)
                    If Double.TryParse(lLine, lValue) Then
                        With lTimeseries(lX, lY)
                            If lFirstTime Then
                                .numValues = lFileNames.Count
                                .Dates = lDates
                            End If
                            If lX = 0 AndAlso lY = 0 Then
                                lDates.Value(lFileIndex) = Date2J(lFileName.Substring(40, 4), lFileName.Substring(44, 2), 1)
                            End If
                            .Values(lFileIndex) = lValue
                        End With
                    Else
                        Logger.Dbg("ParseProblem " & lLine & " at " & lFileIndex)
                    End If
                End If
                lX += 1
                If lX > lXMax Then
                    lY += 1
                    lX = 0
                End If
            Next
            lFileIndex += 1
            lFirstTime = False
        Next

        IO.Directory.SetCurrentDirectory(lOriginalFolder)
        Logger.Dbg("NLDAS_MonthlyGrid2Timeseries")
        Logger.StartToFile(lOriginalLog, True, , True)
        Logger.Dbg("NLDAS_MonthlyGrid2Timeseries")
    End Sub

End Module
