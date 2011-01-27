Imports System
Imports System.Collections
Imports System.Collections.Specialized
Imports MapWindow.Interfaces
Imports MapWinUtility

Imports atcUtility
Imports atcData
Imports atcManDelin
Imports BASINS
Imports D4EMDataManager

Imports AODL
Imports AODL.Document
Imports AODL.Document.SpreadsheetDocuments
Imports AODL.Document.Content
Imports AODL.Document.Content.Tables
Imports AODL.Document.Styles

Module MetWdmBuilder
    Private pMapWin As IMapWin
    Private pDrive As String = "G:"
    Private pBaseFolder As String = pDrive & "\Projects\TT_GCRP\MetData\"
    Private pProjectFolder As String = pBaseFolder & "Projects\"
    Private pCacheFolder As String = pBaseFolder & "Cache\"
    Private pSpreadsheetName As String = "Initial MetSta selection for WDM builds.ods"
    Private WithEvents pZip As New ICSharpCode.SharpZipLib.Zip.FastZip

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        pMapWin = aMapWin
        Dim lOriginalFolder As String = IO.Directory.GetCurrentDirectory
        Dim lOriginalLog As String = Logger.FileName
        Dim lOriginalDisplayMessages As Boolean = Logger.DisplayMessageBoxes
        IO.Directory.SetCurrentDirectory(pBaseFolder)

        If IO.File.Exists(pSpreadsheetName) Then
            If IO.Directory.Exists(pProjectFolder) Then
                IO.Directory.Delete(pProjectFolder, True)
                Logger.Dbg("ExistingProjectFolderDeleted:" & pProjectFolder)
            End If
            If Not IO.Directory.Exists(pCacheFolder) Then
                IO.Directory.CreateDirectory(pCacheFolder)
                Logger.Dbg("CacheFolderCreated:" & pCacheFolder)
            End If
            Logger.StartToFile(pCacheFolder & "MetWdmBuilder.log", , , True)
            Logger.DisplayMessageBoxes = False

            Dim lPlugins As New ArrayList
            For lPluginIndex As Integer = 0 To pMapWin.Plugins.Count
                Try
                    If Not pMapWin.Plugins.Item(lPluginIndex) Is Nothing Then
                        lPlugins.Add(pMapWin.Plugins.Item(lPluginIndex))
                    End If
                Catch lEx As Exception
                    Logger.Dbg(lPluginIndex.ToString & " Problem:" & lEx.ToString)
                End Try
            Next
            Dim lDownloadManager As New D4EMDataManager.DataManager(lPlugins)

            Dim lProjects As New atcCollection
            Dim lStations As atcCollection = Nothing
            Dim lProjectPrevious As String = ""
            Dim lSpreadsheetDocument As New SpreadsheetDocument
            With lSpreadsheetDocument
                .Load(pSpreadsheetName)
                Debug.Print(.IsLoadedFile & " " & .Content.Count)
                For lIndex As Integer = 0 To .Content.Count - 1
                    If Not .Content(lIndex) Is Nothing AndAlso _
                       Not .Content(lIndex).StyleName Is Nothing AndAlso _
                       .Content(lIndex).StyleName.Length > 0 Then
                        Dim lTable As Table = .Content(lIndex)
                        Debug.Print("Rows:" & lTable.Rows.Count)
                        For lRowIndex As Integer = 1 To lTable.Rows.Count - 1
                            Try
                                Dim lRow As Row = lTable.Rows(lRowIndex)
                                Dim lCell As Cell = lRow.Cells(2)
                                Dim lParagraph As AODL.Document.Content.Text.Paragraph = lCell.Content(0)
                                Dim lProjectName As String = lParagraph.TextContent(0).Text
                                If lProjectName <> lProjectPrevious Then
                                    lStations = New atcCollection
                                    lProjects.Add(lProjectName, lStations)
                                    lProjectPrevious = lProjectName
                                End If
                                Dim lLocation As String = CType(lRow.Cells(0).Content(0), AODL.Document.Content.Text.Paragraph).TextContent(0).Text
                                Dim lStaNam As String = CType(lRow.Cells(1).Content(0), AODL.Document.Content.Text.Paragraph).TextContent(0).Text
                                lStations.Add(lStaNam, lLocation)
                            Catch lEx As Exception
                                Debug.Print("Exception" & lEx.ToString)
                            End Try
                        Next
                    End If
                Next
            End With

            Dim lStartDate() As Integer = {1950, 10, 1, 0, 0, 0}
            Dim lStartDateJ As Double = Date2J(lStartDate)

            Dim lEndDate() As Integer = {2006, 10, 1, 0, 0, 0}
            Dim lEndDateJ As Double = Date2J(lEndDate)

            For Each lProject As String In lProjects.Keys
                DownloadMetData(lDownloadManager, lProject, lProjects.ItemByKey(lProject))
                Dim lWdm As New atcWDM.atcDataSourceWDM
                If atcDataManager.OpenDataSource(lWdm, pProjectFolder & lProject & "\met\met.wdm", Nothing) Then
                    ScriptSummarizeTimeseriesBatch.ScriptMain(aMapWin, lStartDateJ, lEndDateJ, pProjectFolder & lProject & "\met\Summary")
                    lWdm.Clear()
                    lWdm = Nothing
                End If
                System.GC.Collect()
                System.GC.WaitForPendingFinalizers()
                Windows.Forms.Application.DoEvents()

                Try
                    pZip.CreateZip(lProject & ".zip", pProjectFolder & lProject, True, "", "")
                Catch lEx As Exception
                    Logger.Dbg("Problem " & lEx.ToString)
                End Try
                System.GC.Collect()
                System.GC.WaitForPendingFinalizers()
            Next
        End If

        IO.Directory.SetCurrentDirectory(lOriginalFolder)
        Logger.Dbg("MetWdmBuilderDone")
        Logger.StartToFile(lOriginalLog, True, , True)
        Logger.Dbg("MetWdmBuilderBatch")
        Logger.DisplayMessageBoxes = lOriginalDisplayMessages
    End Sub

    Sub DownloadMetData(ByVal aDownloadManager As D4EMDataManager.DataManager, ByVal aProject As String, ByVal aStations As atcCollection)
        Logger.Dbg("DownloadMetData " & aProject)

        Dim lQueryMetData As String = "<function name='GetBASINS'> <arguments> <DataType>Met</DataType>" & _
                                      "<SaveWDM>" & pProjectFolder & aProject & "\met\met.wdm</SaveWDM>" & _
                                      "<SaveIn>" & pProjectFolder & aProject & "</SaveIn>" & _
                                      "<CacheFolder>" & pCacheFolder & "</CacheFolder>" & _
                                      "<DesiredProjection>+proj=aea +lat_1=29.5 +lat_2=45.5 +lat_0=23 +lon_0=-96 +x_0=0 +y_0=0 +ellps=GRS80 +datum=NAD83 +units=m +no_defs</DesiredProjection> <region> <northbc>39.3668056784273</northbc> <southbc>38.257273329737</southbc> <eastbc>-76.4009709099128</eastbc> <westbc>-77.2070255407762</westbc>  <projection>+proj=latlong +datum=NAD83</projection> </region> "
        For Each lStation As String In aStations
            lQueryMetData &= "<stationid>" & lStation & "</stationid> "
        Next
        lQueryMetData &= "<clip>False</clip> <merge>False</merge> <joinattributes>true</joinattributes> </arguments> </function>  "
        Dim lResultMetData As String = aDownloadManager.Execute(lQueryMetData)
        If lResultMetData Is Nothing OrElse lResultMetData.Length = 0 Then
            'Nothing to report, no success or error
        ElseIf lResultMetData.StartsWith("<success>") Then
            BASINS.ProcessDownloadResults(lResultMetData)
        Else
            Logger.Msg(atcUtility.ReadableFromXML(lResultMetData), "MetDataDownload Result")
        End If
    End Sub
End Module
