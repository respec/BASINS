Imports System
Imports System.Windows.Forms
Imports Microsoft.VisualBasic
Imports MapWinGIS
Imports MapWindow.Interfaces
Imports MapWinUtility
Imports atcUtility

'GetPrecipValueFromGrid.vb 
'Created by Jack Kittle (jlkittle@aquaterra.com)
'Date 25 july 2006

Public Module GetPrecipValueFromGrid
  Private Const pDirPath As String = "D:\Basins\data\prism\"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("GetPrecipValueFromGrid:Start")
        ChDriveDir(pDirPath)
        Logger.Dbg(" CurDir:" & CurDir)

        Dim lGrid As New MapWinGIS.Grid

        Dim lGridNames As New Collection
        lGridNames.Add ("us_ppt_800m_1971_2000.01.asc")
       
        Dim lGridName As String
        For Each lGridName In lGridNames
            Logger.Dbg(" Process:" & lGridName)
            lGrid.Open(lGridName)
            Logger.Dbg("    Max:" & lGrid.Maximum)
            Logger.Dbg("    Min:" & lGrid.Minimum)

            Dim lLat As Double, lLng As Double, lCol As Integer, lRow As Integer
            lLat = 33.63
            lLng = -84.44
            lGrid.ProjToCell(lLng, lLat, lCol, lRow)
            Logger.Dbg("     Lat:" & lLat & ":Lng:" & lLng)
            Logger.Dbg("     Row:" & lRow & ":Col:" & lCol)

            Dim lValue As Double
            lValue = lGrid.Value(lCol, lRow)
            Logger.Dbg("     Value:" & lValue)

            lValue /= 2540
            Logger.Dbg("    Value(Inches):" & lValue)
            lGrid.Close()
        Next
    End Sub
End Module