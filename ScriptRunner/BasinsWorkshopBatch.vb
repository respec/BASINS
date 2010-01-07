Imports System.Collections.Specialized
Imports MapWindow.Interfaces
Imports MapWinUtility

Imports atcUtility
Imports atcData
Imports BASINS

Module BasinsWorkshopBatch
    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Dim lOriginalDir As String = IO.Directory.GetCurrentDirectory
        Dim lOriginalDisplayMessages As Boolean = Logger.DisplayMessageBoxes
        Logger.DisplayMessageBoxes = False
        Logger.Dbg("BasinsWorkshopBatch:CurDir:" & lOriginalDir)
        Dim lBasinsProjectDataDir As String = DefaultBasinsDataDir() & "WorkshopBatch\"
        Try
            If IO.Directory.Exists(lBasinsProjectDataDir) Then
                IO.Directory.Delete(lBasinsProjectDataDir, True)
            End If
            If Not Exercise1(lBasinsProjectDataDir) Then
                Logger.Dbg("***** Exercise1 FAIL *****")
            ElseIf Not Exercise2() Then
                Logger.Dbg("***** Exercise2 FAIL *****")
            End If
        Catch lEx As Exception
            Logger.Dbg("Problem " & lEx.ToString)
        End Try
        IO.Directory.SetCurrentDirectory(lOriginalDir)
        Logger.DisplayMessageBoxes = lOriginalDisplayMessages
    End Sub

    Private Function Exercise1(ByVal aBasinsProjectDataDir As String) As Boolean
        'Adding Data to a New BASINS Project
        '  Build a New BASINS Project
        '  TODO: open National Project and select Patuxent in code
        Dim lProjection As String = "proj +proj=utm +zone=18 +ellps=GRS80 +lon_0=-75 +lat_0=0 +k=0.9996 +x_0=500000.0 +y_0=0 end "
        SaveFileString(aBasinsProjectDataDir & "prj.proj", lProjection) 'Side effect: makes data directory
        Dim lRegion As String = _
           "<region>" & _
           "   <northbc>1975392.91047589</northbc>" & _
           "   <southbc>1866978.51560156</southbc>" & _
           "   <eastbc>1684619.12695581</eastbc>" & _
           "   <westbc>1595425.21946512</westbc>" & _
           "   <projection>+proj=aea +lat_1=29.5 +lat_2=45.5 +lat_0=23 +lon_0=-96 +x_0=0 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs</projection>" & _
           "   <HUC8 status=""set by BASINS System Application"">02060006</HUC8>" & _
           "</region> "
        CreateNewProjectAndDownloadCoreData(lRegion, DefaultBasinsDataDir, aBasinsProjectDataDir, aBasinsProjectDataDir & "Patuxent.mwprj")

        '  Navigate the BASINS 4.0 GIS Environment

        '  Add Land Use Data

        '  Add NHDPlus Data

        '  Add BASINS census and TIGER line data

        '  Add BASINS Digital Elevation Model (DEM) grids

        '  Import other shapefiles

        '  Download timeseries data for use in modeling

        Return True
    End Function

    Private Function Exercise2() As Boolean
        Logger.Dbg("NotYetImplemented")
        Return False
    End Function

End Module
