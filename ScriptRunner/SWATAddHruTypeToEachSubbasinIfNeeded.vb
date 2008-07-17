Imports MapWindow.Interfaces
Imports MapWinUtility
Imports atcUtility
Imports SwatObject

Module SWATAddHruTypeToEachSubbasinIfNeeded
    Private pDrive As String = "C:"
    'Private pDrive As String = "G:"
    Private pBaseFolder As String = pDrive & "\project\UMRB\baseline90"
    Private pSWATGDB As String = "C:\Program Files\SWAT\ArcSWAT\Databases\SWAT2005.mdb"
    'Private pSWATGDB As String = "C:\Program Files\SWAT 2005 Editor\Databases\SWAT2005.mdb"
    Private pInGDB As String = "baseline90.mdb"
    Private pOutGDB As String = "baseline90X.mdb"
    Private pLogsFolder As String
    Private pScenario As String = "AddHruAsNeeded"
    Private pCropToCreate As String = "CRP"
    Private pMaxHRUsToChange As Integer = 5

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        ChDriveDir(pBaseFolder)
        Dim lLogFileName As String = Logger.FileName
        pLogsFolder = IO.Path.Combine(pBaseFolder, "logs")
        Logger.StartToFile(IO.Path.Combine(pLogsFolder, pScenario & ".log"), , , True)

        Dim lOutGDB As String = IO.Path.Combine(pBaseFolder, pOutGDB)
        If IO.File.Exists(lOutGDB) Then
            Logger.Dbg("DeleteExisting " & lOutGDB)
            IO.File.Delete(lOutGDB)
        End If
        IO.Directory.CreateDirectory(IO.Path.GetDirectoryName(lOutGDB))
        IO.File.Copy(IO.Path.Combine(pBaseFolder, pInGDB), lOutGDB)
        Logger.Dbg("Copied " & pOutGDB & " from " & pInGDB)

        Dim lHUC8ToSubBasin As New atcCollection
        Dim lHUC8TableToSubBasin As New atcTableDelimited
        With lHUC8TableToSubBasin
            .Delimiter = ","
            .NumHeaderRows = 0
            .OpenFile(IO.Path.Combine(pBaseFolder, "flowfig.csv"))
            For lRowIndex As Integer = 1 To .NumRecords
                lHUC8ToSubBasin.Add(.Value(7), .Value(5))
                .MoveNext()
            Next
            .Clear()
        End With
        Dim lHuc8Crp As New atcCollection
        Dim lHuc8CrpTable As New atcTableDelimited
        With lHuc8CrpTable
            .Delimiter = vbTab
            .NumHeaderRows = 0
            .OpenFile(IO.Path.Combine(pBaseFolder, "crp2005.txt"))
            For lRowIndex As Integer = 1 To .NumRecords
                Dim lHuc8 As String = .Value(1).ToString.PadLeft(8, "0")
                Dim lSubBasin As String = lHUC8ToSubBasin.ItemByKey(lHuc8)
                lHuc8Crp.Add(lSubBasin, .Value(2))
                'Logger.Dbg("Huc8 " & lHuc8 & " SubBasin " & lSubBasin & " CropAcres " & .Value(2))
                .MoveNext()
            Next
            .Clear()
        End With

        Logger.Dbg("InitializeSwatInput")
        Dim lSwatInput As New SwatInput(pSWATGDB, lOutGDB, pBaseFolder, pscenario)

        Dim lSubBasinTable As DataTable = lSwatInput.SubBsn.Table
        For Each lSubBasinRow As DataRow In lSubBasinTable.Rows
            Dim lSubBasinId As String = lSubBasinRow.Item(1).ToString
            Dim lWhereClause As String = "(subbasin=" & lSubBasinId & " AND landuse='" & pCropToCreate & "')"
            Dim lSubBasinHruTableCrp As DataTable = lSwatInput.QueryInputDB("SELECT * FROM hru WHERE " & lWhereClause & " ORDER BY HRU;")
            Dim lSubBasinHruCrpArea As Double = 0.0
            Dim lSubBasinArea As Double = lSubBasinRow.Item("SUB_KM")
            For Each lCrp As DataRow In lSubBasinHruTableCrp.Rows
                lSubBasinHruCrpArea += lSubBasinArea * lCrp.Item("HRU_FR")
            Next
            Dim lHuc8CrpNeed As Double = lHuc8Crp.ItemByKey(lSubBasinId) / 247.0 'acre -> km2
            If lSubBasinHruCrpArea >= lHuc8CrpNeed Then
                Logger.Dbg("SubBasin " & lSubBasinId & " has " & lSubBasinHruCrpArea & " km2 of " & pCropToCreate & " only need " & lHuc8CrpNeed)
            Else
                If lSubBasinHruTableCrp.Rows.Count = 0 Then
                    lWhereClause = "subbasin=" & lSubBasinId
                    Dim lSubBasinHruTable As DataTable = lSwatInput.QueryInputDB("SELECT * FROM hru WHERE " & lWhereClause & " ORDER BY hru_fr;")
                    Dim LUsToConvert() As String = {"AGRR", "RNGE", "PAST"}
                    Dim lSlopeCodes() As String = {"4-9999"}
                    Dim lChanged As Boolean = False
                    'first try converting only hi-slope row crop, range, and pasture
                    ConvertHRUsToCRP(lSubBasinId, LUsToConvert, lSlopeCodes, lHuc8CrpNeed, lSwatInput, lSubBasinArea, lSubBasinHruTable, lChanged)
                    If Not lChanged Then
                        'next try other hi-slope land uses
                        Dim LUsToConvert2() As String = {"URHD", "URLD", "URMD", "ALFA", "HAY", "FRSD", "FRSE", "FRST"}
                        ConvertHRUsToCRP(lSubBasinId, LUsToConvert2, lSlopeCodes, lHuc8CrpNeed, lSwatInput, lSubBasinArea, lSubBasinHruTable, lChanged, pMaxHRUsToChange)
                        If Not lChanged Then
                            'finally try lower slope land uses
                            Dim LUsToConvert3() As String = {"URHD", "URLD", "URMD", "ALFA", "HAY", "FRSD", "FRSE", "FRST", "WETF"}
                            Dim lSlopeCodes2() As String = {"2-4", "4-9999"}
                            ConvertHRUsToCRP(lSubBasinId, LUsToConvert3, lSlopeCodes2, lHuc8CrpNeed, lSwatInput, lSubBasinArea, lSubBasinHruTable, lChanged, pMaxHRUsToChange)
                        End If
                    End If
                    If Not lChanged Then
                        Logger.Dbg("Need to convert something else for " & lSubBasinId)
                    End If
                Else
                    Logger.Dbg("SubBasin " & lSubBasinId & " has " & lSubBasinHruTableCrp.Rows.Count & " HRUs of " & pCropToCreate _
                              & " (" & lSubBasinHruCrpArea & " km2, need " & lHuc8CrpNeed & ")")
                End If
            End If
        Next

        'back to basins log
        Logger.StartToFile(lLogFileName, True, False, True)
    End Sub

    Private Sub ConvertHRUsToCRP(ByVal aSubBasinID As String, ByVal aLUsToConvert As Array, ByVal aSlopeCodes As Array, ByVal aCRPAreaNeeded As Double, _
                                 ByVal aSwatInput As SwatInput, ByVal aSubBasinArea As Double, ByVal aSubBasinHRUTable As DataTable, _
                                 ByRef aChanged As Boolean, Optional ByVal aMaxHRUsToChange As Integer = 0)
        Dim lMaxHRUsToChange As Integer = 1000000
        If aMaxHRUsToChange > 0 Then lMaxHRUsToChange = aMaxHRUsToChange
        Dim lHRUsChanged As Integer = 0
        Dim lHruId As String
        Dim lHruLandUse As String
        Dim lHruSlope As String
        Dim lHruFraction As Double
        Dim lHruArea As Double
        For Each lHruRow As DataRow In aSubBasinHRUTable.Rows
            lHruId = lHruRow.Item("HRU")
            lHruLandUse = lHruRow.Item("LANDUSE")
            lHruSlope = lHruRow.Item("SLOPE_CD")
            lHruFraction = lHruRow.Item("HRU_FR")
            lHruArea = lHruFraction * aSubBasinArea
            If lHruArea > aCRPAreaNeeded And Not aChanged Then
                If lHRUsChanged = 0 Then
                    Dim lLUString As String = ""
                    For Each lstr As String In aLUsToConvert
                        lLUString &= lstr & ", "
                    Next
                    lLUString = Left(lLUString, lLUString.Length - 2)
                    Logger.Dbg("SubBasin " & aSubBasinID & ": " & lLUString & " HRUs too big (only need " & aCRPAreaNeeded & ") - convert other HRUs")
                End If
                Exit For
            Else
                If Array.IndexOf(aLUsToConvert, lHruLandUse) >= 0 AndAlso _
                   Array.IndexOf(aSlopeCodes, lHruSlope) >= 0 AndAlso _
                   lHruArea < aCRPAreaNeeded Then
                    aCRPAreaNeeded -= lHruArea
                    Logger.Dbg("SubBasin " & aSubBasinID & " Change " & lHruLandUse & " to " & pCropToCreate & " for " & lHruArea & " need " & aCRPAreaNeeded)
                    UpdateLandUse(aSwatInput, aSubBasinID, lHruId, lHruLandUse, pCropToCreate)
                    lHRUsChanged += 1
                End If
            End If
        Next
        If lHRUsChanged > 0 Then
            aChanged = True
        Else
            aChanged = False
        End If

    End Sub

    Private Sub UpdateLandUse(ByVal aSwatInput As SwatInput, _
                              ByVal aSubBasinId As String, ByVal aHruId As String, _
                              ByVal aHruLandUse As String, ByVal aLandUseNew As String)
        Dim lWhereClause As String = " (subbasin=" & aSubBasinId & " AND hru=" & aHruId & ")"
        Dim lTablesToUpdate As String() = {"hru", "chm", "gw", "mgt1", "sol"}
        For Each lTable As String In lTablesToUpdate
            aSwatInput.UpdateInputDB(lTable, lWhereClause, "LANDUSE", aLandUseNew)
        Next
        'update other parameters in mgt2 
        'TODO: dont make so tied to the specific needs of UMRB project !!!!
        Dim lSql As String = "SELECT * FROM mgt2 WHERE " & lWhereClause & ";"
        Dim lDataTable As DataTable = aSwatInput.QueryInputDB(lSql)
        Logger.Dbg("SubBasin " & aSubBasinId & " HRU " & aHruId & " MGT2Count " & lDataTable.Rows.Count)
        Dim lMgtFound(20) As Boolean
        For Each lRow As DataRow In lDataTable.Rows
            Dim lMgtOp As Integer = lRow.Item("MGT_OP")
            lMgtFound(lMgtOp) = True
            If lMgtOp = 1 Or lMgtOp = 5 Then
                aSwatInput.UpdateInputDB("mgt2", "oid", lRow.Item(0), "landuse", aLandUseNew)
                aSwatInput.UpdateInputDB("mgt2", "oid", lRow.Item(0), "crop", aLandUseNew)
            Else
                aSwatInput.DeleteRowInputDB("mgt2", "oid", lRow.Item(0))
            End If
        Next
        If Not lMgtFound(1) Then
            Logger.Dbg("Missing MGT1")
        End If
        If Not lMgtFound(5) Then
            Logger.Dbg("Missing MGT5")
        End If
    End Sub

End Module
