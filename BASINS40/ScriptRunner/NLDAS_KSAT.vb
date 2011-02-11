Imports System.Collections
Imports System.Collections.Specialized

Imports MapWindow.Interfaces
Imports MapWinUtility

Imports atcUtility
Imports atcData

Module NLDAS_KSAT
    Public pPath As String = "E:\GisData\NLDAS-Soil"
    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Dim lOriginalFolder As String = IO.Directory.GetCurrentDirectory
        Dim lOriginalLog As String = Logger.FileName

        ChDriveDir(pPath)
        Logger.StartToFile("NLDAS_KSAT.log", False, False, True)
        Logger.Dbg("NLDAS_KSAT: Start")

        Dim lFillVal As Double = -9.99
        Dim lNoDataVal As Integer = -1
        Dim lGridHeader As New MapWinGIS.GridHeader
        Dim lGridSize As Double = 1 / 8
        With lGridHeader
            .dX = lGridSize
            .dY = lGridSize
            .NodataValue = lNoDataVal
            .NumberCols = 464
            .NumberRows = 224
            .XllCenter = -125 + lGridSize
            .YllCenter = 25 + lGridSize
        End With

        Dim lFileNames As NameValueCollection = Nothing
        AddFilesInDir(lFileNames, pPath, False, "*")
        Dim lValues() As String
        For Each lFileName As String In lFileNames
            If IsNumeric(lFileName.Substring(lFileName.Length - 2)) Then
                Logger.Dbg("Process " & lFileName)
                Dim lUniqueValuesCount As New atcCollection
                Dim lValueMin As Double = 1.0E+30
                Dim lValueMax As Double = -1.0E+30
                Dim lValueCount As Integer = 0
                Dim lValueSum As Double = 0.0
                Dim lGridName As String = IO.Path.GetFileName(lFileName) & ".tif"
                Dim lGrid As New MapWinGIS.Grid
                With lGrid
                    .CreateNew("grid\" & lGridName, lGridHeader, MapWinGIS.GridDataType.ShortDataType, lNoDataVal)
                    For Each lLine As String In LinesInFile(lFileName)
                        While lLine.Contains("  ")
                            lLine = lLine.Replace("  ", " ")
                        End While
                        lValues = Split(lLine.Remove(0, 1))
                        Dim lValue As Double = lValues(9)
                        Dim lValueI As Integer
                        If Math.Abs(lValue - lFillVal) > 0.0001 Then
                            lValueI = CInt(lValue * 100000000.0) ' 1e8
                            lUniqueValuesCount.Increment("V" & lValueI.ToString.PadLeft(6, "0"))
                            If lValueI > lValueMax Then lValueMax = lValueI
                            If lValueI < lValueMin Then lValueMin = lValueI
                            lValueCount += 1
                            lValueSum += lValueI
                        Else
                            lValueI = lNoDataVal
                        End If
                        Dim lX As Integer
                        Dim lY As Integer
                        .ProjToCell(lValues(2), lValues(3), lX, lY)
                        If lX <> lValues(0) - 1 OrElse lY <> lValues(1) - 1 Then
                            Logger.Dbg("problemAt " & lX, lValues(0) - 1, lY, lValues(1) - 1)
                        End If
                        .Value(lValues(0) - 1, lValues(1) - 1) = lValueI
                    Next
                    .Save()
                    Logger.Dbg("Done " & lGridName & " Min " & lValueMin & " Max " & lValueMax & " Count " & lValueCount & " Mean " & lValueSum / lValueCount)
                    Logger.Dbg("UniqueValuesCount " & lUniqueValuesCount.Count)
                    Dim lUniqueValuesSortedList As New SortedList

                    For Each lKey As String In lUniqueValuesCount.Keys
                        lUniqueValuesSortedList.Add(lKey.Substring(1), lUniqueValuesCount.ItemByKey(lKey).ToString)
                    Next
                    For Each lKey As String In lUniqueValuesSortedList.Keys
                        Logger.Dbg("  Value " & CInt(lKey) & " Count " & lUniqueValuesCount.ItemByKey("V" & lKey).ToString)
                    Next
                End With
            Else
                Logger.Dbg("Skip " & lFileName)
            End If
        Next

        IO.Directory.SetCurrentDirectory(lOriginalFolder)
        Logger.Dbg("NLDAS_KSAT")
        Logger.StartToFile(lOriginalLog, True, , True)
        Logger.Dbg("NLDAS_KSAT")
    End Sub
End Module
