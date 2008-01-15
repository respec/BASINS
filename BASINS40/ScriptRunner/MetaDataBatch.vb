Imports MapWinUtility
Imports atcUtility
Imports MapWindow.Interfaces
Imports mwOpenMetadataManager

Module MetaDataBatch
    Private pTestPath As String = "C:\dev\BASINS40\ScriptRunner\data\metadata\Orig"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Logger.Dbg("Start")
        ChDriveDir(pTestPath)   'change to the directory of the testdata
        Logger.Dbg(" CurDir:" & CurDir())

        Dim lMetaData As FGDCMetadata.MetadataMain

        Dim lFileNames() As String = {"cnty.shp.xml", "nhdflowline.shp.xml", "nhdflowline.shp.Orig.xml"}
        For Each lFileName As String In lFileNames
            lMetaData = New FGDCMetadata.MetadataMain
            Try
                lMetaData.Load(lFileName)
            Catch lEx As Exception
                Logger.Dbg(lEx.ToString)
            End Try
            lMetaData.Save("..\rev\" & lFileName)
        Next
        Logger.Dbg("Done")
    End Sub
End Module
