Imports System
Imports System.Windows.Forms
Imports System.Collections.Specialized
Imports Microsoft.VisualBasic
Imports MapWinGIS
Imports MapWindow.Interfaces
Imports MapWinUtility
Imports atcUtility

'GetPrecipValueFromGrid.vb 
'Created by Jack Kittle (jlkittle@aquaterra.com)
'Date 25 july 2006

Public Module GetPrecipValueFromGrid
    Private Const pDirPath As String = "G:\MetData\Prism\GridExtract"
    Private Const pBound As Integer = 2
    'Private Const pFilter As String = "us_ppt." 'all normal ppt grids
    Private Const pFilter As String = "us_ppt" 'all ppt grids
    Private Const pResultFile As String = "Results\" & pFilter & ".txt"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("GetPrecipValueFromGrid:Start")
        ChDriveDir(pDirPath)
        Logger.Dbg(" CurDir:" & CurDir)

        Dim lS As String
        lS = "Station" & vbTab & "Lat" & vbTab & "Lng" & vbTab & "Grid" & vbTab & _
             "Min" & vbTab & "Center" & vbTab & "Max" & vbCrLf                                
        SaveFileString(pResultFile,lS)

        Dim lGrid As New MapWinGIS.Grid
        Dim lGridNames As New NameValueCollection
        AddFilesInDir(lGridNames, ".", False, pFilter & "*")
        Logger.Dbg(" Count:" & lGridNames.Count & " Bound:" & pBound)

        Dim lSpecs As New Collection
        lSpecs.Add("090451:33.63:-84.44") 'ATL
        lSpecs.Add("447201:37.505:-77.3203") 'RIC

        For Each lGridName As String In lGridNames
            Logger.Dbg(" Process:" & lGridName)
            If Not (lGridName.EndsWith(".asc")) Then
                Io.File.Move(lGridName, lGridName & ".asc")
                lGridName &= ".asc"
            End If
            lGrid.Open(lGridName)
            'Logger.Dbg("    Max:" & lGrid.Maximum)
            'Logger.Dbg("    Min:" & lGrid.Minimum)

            For Each lSpec As String In lSpecs
                Dim lLat As Double, lLng As Double, lCol As Integer, lRow As Integer
                Dim lStation As String

                lStation = StrSplit(lSpec, ":", "")
                lLat = StrSplit(lSpec, ":", "")
                lLng = StrSplit(lSpec, ":", "")
                lGrid.ProjToCell(lLng, lLat, lCol, lRow)
                'Logger.Dbg("     Lat:" & lLat & ":Lng:" & lLng)
                'Logger.Dbg("     Row:" & lRow & ":Col:" & lCol)

                Dim lVal As Double, lValCnv As Double, lValCnvCtr As Double
                Dim lValCnvMin As Double = 1.0E+30, lValCnvMax As Double = -1.0E+30

                For lC As Integer = lCol - pBound To lCol + pBound
                    For lR As Integer = lRow - pBound To lRow + pBound
                        lVal = lGrid.Value(lC, lR)
                        lValCnv = lVal / 2540
                        If (lC = lCol And lR = lRow) Then lValCnvCtr = lValCnv
                        If lValCnv > lValCnvMax Then lValCnvMax = lValCnv
                        If lValCnv < lValCnvMin Then lValCnvMin = lValCnv
                        'Logger.Dbg("     Value:" & lValCnv & ":" & lC & ":" & lR & ":" & lVal)
                    Next
                Next

                lS = lStation & vbTab & _
                     DoubleToString(lLat, , "####.0000") & vbTab & _
                     DoubleToString(lLng, , "####.0000") & vbTab & _
                     FileNameOnly(lGridName) & vbTab & _
                     DoubleToString(lValCnvMin, , "###.00") & vbTab & _
                     DoubleToString(lValCnvCtr, , "###.00") & vbTab & _
                     DoubleToString(lValCnvMax, , "###.00") & vbCrLf
                AppendFileString(pResultFile, lS)
            Next

            lGrid.Close()
        Next
    End Sub
End Module