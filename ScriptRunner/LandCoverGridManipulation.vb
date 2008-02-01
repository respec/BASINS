Imports atcUtility
Imports MapWindow.Interfaces
Imports MapWinUtility
Imports MapWinGeoProc
Imports atcMwGisUtility
Imports System.Collections.Specialized

Module ScriptLandCoverGridManipulation
    Private pTestPath As String = "F:\GisData\SERDP\LandUseProcess\"
    
    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("Start")
        ChDriveDir(pTestPath)  'change to the directory of the current project
        Logger.Dbg(" CurDir:" & CurDir())

        Dim lNLCDGrid As New MapWinGIS.Grid
        lNLCDGrid.Open(pTestPath & "NLCD2001Test.tif")

        Dim lSEMPGrid As New MapWinGIS.Grid
        lSEMPGrid.Open(pTestPath & "Landcover2004Test.tif")

        Dim lTotalCellCount As Integer = lNLCDGrid.Header.NumberCols * lNLCDGrid.Header.NumberRows
        Dim lCellCount As Integer = 0
        Dim lNLCDValue As Integer
        Dim lXPos As Double
        Dim lYPos As Double
        Dim lSEMPCol As Integer
        Dim lSEMPRow As Integer
        Dim lSEMPValue As Integer

        For lRow As Integer = 0 To lNLCDGrid.Header.NumberRows - 1
            For lCol As Integer = 0 To lNLCDGrid.Header.NumberCols - 1
                lNLCDValue = lNLCDGrid.Value(lCol, lRow)

                'find corresponding value in SEMP Grid
                lNLCDGrid.CellToProj(lCol, lRow, lXPos, lYPos)
                lSEMPGrid.ProjToCell(lXPos, lYPos, lSEMPCol, lSEMPRow)
                lSEMPValue = lSEMPGrid.Value(lSEMPCol, lSEMPRow)

                If lNLCDValue > 20 And lNLCDValue < 25 Then
                    'this is developed, see if it is cantonment in semp grid
                    If lSEMPValue = 11 Then
                        'add 100 to show cantonment 
                        lNLCDGrid.Value(lCol, lRow) = 100 + lNLCDGrid.Value(lCol, lRow)
                    End If
                ElseIf lNLCDValue = 42 Then
                    'this is evergreen, see if it is evergreen planted in semp grid
                    If lSEMPValue = 2 Then
                        'add 100 to show evergreen planted
                        lNLCDGrid.Value(lCol, lRow) = 100 + lNLCDGrid.Value(lCol, lRow)
                    End If
                End If

                If lSEMPValue = 9 Then
                    'paved roads in semp layer
                    lNLCDGrid.Value(lCol, lRow) = 100
                End If

                lCellCount += 1
                Logger.Progress(lCellCount, lTotalCellCount)
            Next lCol
        Next lRow

        lNLCDGrid.Save(pTestPath & "NLCD2001SEMP2004Hybrid.tif")
        lNLCDGrid.Close()
    End Sub
End Module
