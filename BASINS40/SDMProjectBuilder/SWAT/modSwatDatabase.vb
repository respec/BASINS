Imports MapWinUtility
Imports atcUtility
Imports atcData
Imports D4EMDataManager
Imports SwatObject
Imports System.Data

Friend Module modSwatDatabase
    Private SWATParamsXmlDoc As Xml.XmlDocument

    Friend Enum WhichSoilIndex
        USGS
        SWATSTATSGO
    End Enum

    Friend Sub BuildSwatDatabase(ByVal aCacheFolder As String, _
                                 ByVal aProjectFolder As String, _
                                 ByVal aFlowlinesFilename As String, _
                                 ByVal aHuc12 As String, _
                                 ByVal aSwatDatabaseName As String, _
                                 ByVal aParamTable As atcTable, _
                                 ByVal aSubBasinToParamIndex As atcCollection, _
                                 ByVal aHruTable As clsHruTable, _
                                 ByVal aWhichSoilIndex As WhichSoilIndex)

        If aHruTable Is Nothing Then
            Logger.Dbg("BuildSwatDatabase: HRU table not present, aborting")
        ElseIf aHruTable.Count = 0 Then
            Logger.Dbg("BuildSwatDatabase: HRU table is empty, aborting")
        Else
            Dim lProjectDataBaseName As String = IO.Path.Combine(aProjectFolder, aHuc12 & ".mdb")
            If IO.File.Exists(lProjectDataBaseName) Then
                IO.File.Delete(lProjectDataBaseName)
            End If

            Try
                Dim lSwatInput As New SwatInput(aSwatDatabaseName, lProjectDataBaseName, aProjectFolder, aHuc12)
                With lSwatInput
                    Dim lTxtInOutFolder As String = IO.Path.Combine(aProjectFolder, "Scenarios\" & aHuc12 & "\TxtInOut")

                    Dim lComIdToBsnId As atcCollection = CreateSWATFig(aFlowlinesFilename, IO.Path.Combine(lTxtInOutFolder, "fig.fig"))

                    Dim lFlowlineTable As New atcTableDelimited
                    lFlowlineTable.Delimiter = vbTab
                    lFlowlineTable.OpenFile(IO.Path.Combine(aProjectFolder, "Flowlines.txt"))

                    'Dim lHruTable As New atcTableDelimited
                    'lHruTable.Delimiter = vbTab
                    'lHruTable.OpenFile(IO.Path.Combine(aProjectFolder, "HRUsRev.txt"))
                    Dim lHruCount As Integer = 0

                    Dim lCatchmentTable As New atcTableDelimited
                    lCatchmentTable.Delimiter = vbTab
                    lCatchmentTable.OpenFile(IO.Path.Combine(aProjectFolder, "Catchments.txt"))

                    lSwatInput.CIO.Add(CioDefault)
                    lSwatInput.Wwq.Add(WwqDefault)
                    lSwatInput.Bsn.Add(BsnDefault)

                    Dim lSolItem As SwatInput.clsSolItem = Nothing
                    Dim lSoilClassOld As String = ""

                    Dim lWeatherGenAllDataTable As DataTable = lSwatInput.QueryGDB("SELECT * FROM weatherstations")
                    Logger.Dbg("WeatherStationCount " & lWeatherGenAllDataTable.Rows.Count)

                    Dim lLandUseMissing As New atcCollection
                    For lIndex As Integer = 0 To lComIdToBsnId.Count - 1
                        Dim lSubId As Integer = lComIdToBsnId.ItemByIndex(lIndex)
                        Dim lComId As Integer = lComIdToBsnId.Keys(lIndex)
                        Dim lHaveParams As Boolean = False
                        If aParamTable IsNot Nothing Then
                            Dim lSubBasinParamIndex As Integer = aSubBasinToParamIndex.IndexFromKey(CDbl(lComId))
                            If lSubBasinParamIndex >= 0 Then
                                lHaveParams = True
                                aParamTable.CurrentRecord = aSubBasinToParamIndex.ItemByIndex(lSubBasinParamIndex) + 1
                            End If
                        End If
                        Dim lHruTableStartSearchAt As Integer = 1
                        Dim lHruSubCount As Integer = 0
                        Dim lSubBasinAreaTotal As Double = 0
                        Logger.Dbg("Index " & lIndex & " SubId " & lSubId & " ComID " & lComId)
                        Dim lSubBasinHrus As New Generic.List(Of SwatInput.clsHruItem)
                        Dim lHruComIdIndex As Integer = aHruTable.Tags.IndexOf("SubBasin")
                        Dim lHruLandUseIndex As Integer = aHruTable.Tags.IndexOf("LandUse")
                        Dim lHruSoilIndex As Integer = aHruTable.Tags.IndexOf("Soil")
                        Dim lHruSlopeCdIndex As Integer = aHruTable.Tags.IndexOf("SlopeReclass")
                        For Each lHru As clsHru In aHruTable
                            If lHru.Ids(lHruComIdIndex) = lComId Then
                                Dim lSlopeId As Double = lHru.Ids(lHruSlopeCdIndex)
                                If Double.IsNaN(lSlopeId) Then
                                    Logger.Dbg("Skip " & lHru.Key & ":" & lHru.Area)
                                Else
                                    Dim lSoilId As String = lHru.Ids(lHruSoilIndex)
                                    Dim lSoilClass As String = SoilId2Str(lSwatInput, lSoilId, aWhichSoilIndex)
                                    If lSoilClass = "<unk>" Then
                                        'Throw New ApplicationException("CouldNotConvertSoilId " & lSoilId & " SubId " & lSubId & " HRU " & lHru.ToString)
                                        Logger.Dbg("CouldNotConvertSoilId " & lSoilId & " SubId " & lSubId & " HRU " & lHru.Key & ":" & lHru.Area)
                                    Else
                                        lHruSubCount += 1
                                        Dim lSlope_CdStr As String = SlopeId2Str((lSlopeId))
                                        Dim lLuIdStr As String = lHru.Ids(lHruLandUseIndex)
                                        Dim lLuId As Integer
                                        If Not (Integer.TryParse(lLuIdStr, lLuId)) Then
                                            lLuId = -1
                                        End If

                                        Dim lLandUseStr As String = LuId2Str(lLuId)
                                        'Dim lLuId As Integer = lHru.Ids(lHruLandUseIndex)



                                        'Check the new soil class with the old soil class
                                        If lSolItem Is Nothing OrElse lSoilClassOld <> lSoilClass Then
                                            Dim lState As String = lSoilClass.Substring(0, 1) & lSoilClass.Substring(1, 1).ToLower
                                            Dim lUSSoil As DataTable = lSwatInput.QuerySoils("SELECT * FROM( " & lState & "MUIDs) WHERE MUID = '" & lSoilClass & "' ORDER BY CMPPCT DESC;")
                                            If lUSSoil.Rows.Count = 0 Then
                                                Logger.Dbg("DummySoil " & lSoilId)
                                                lUSSoil = lSwatInput.QuerySoils("SELECT * FROM( " & "Mn" & "MUIDs) WHERE MUID = '" & "MN009" & "' ORDER BY CMPPCT DESC;")
                                            End If
                                            lSolItem = New SwatInput.clsSolItem(lUSSoil.Rows(0))
                                        End If
                                        With lSolItem
                                            .SUBBASIN = lSubId
                                            .HRU = lHruSubCount
                                            .LANDUSE = lLandUseStr
                                            .SOIL = lSoilClass
                                            .SLOPE_CD = lSlope_CdStr
                                        End With
                                        lSwatInput.Sol.Add(lSolItem)
                                        lSoilClassOld = lSoilClass

                                        Dim lHruItem As New SwatInput.clsHruItem(lSubId, lHruSubCount, lLandUseStr, lSoilClass, lSlope_CdStr)
                                        With lHruItem
                                            .HRU_FR = lHru.Area
                                            .HRU_SLP = lHru.SlopeMean / 100
                                            .OV_N = 0.1
                                            'from OpenSWAT:createHRUGeneralTable
                                            'Select Case lHru.SlopeMean
                                            '    Case 0 To 0.1 : .SLSUBBSN = 121
                                            '    Case 0.1 To 2.0 : .SLSUBBSN = 90
                                            '    Case 2.0 To 3.0 : .SLSUBBSN = 75
                                            '    Case 3.0 To 4.0 : .SLSUBBSN = 60
                                            '    Case 4.0 To 5.0 : .SLSUBBSN = 45
                                            '    Case Else : .SLSUBBSN = 25
                                            'End Select
                                            'New slope values from Srini: 
                                            'for slope length if the slope of the HRU is:
                                            '< 1% then use 120m
                                            ' 1-2% - 100m
                                            ' 2-3% - 90m 
                                            ' 3-5% - 60m
                                            ' 5-8% - 30m
                                            ' 8-15% - 15m
                                            ' > 15% - 9m
                                            Select Case lHru.SlopeMean
                                                Case Is < 1.0 : .SLSUBBSN = 120
                                                Case 1.0 To 2.0 : .SLSUBBSN = 100
                                                Case 2.0 To 3.0 : .SLSUBBSN = 90
                                                Case 3.0 To 5.0 : .SLSUBBSN = 60
                                                Case 5.0 To 8.0 : .SLSUBBSN = 30
                                                Case 8.0 To 15.0 : .SLSUBBSN = 15
                                                Case Else : .SLSUBBSN = 9
                                            End Select

                                            lSubBasinAreaTotal += lHru.Area
                                        End With
                                        lSubBasinHrus.Add(lHruItem)

                                        Dim lChmItem As New SwatInput.clsChmItem(lSubId, lHruSubCount, lLandUseStr, lSoilClass, lSlope_CdStr)
                                        lSwatInput.Chm.Add(lChmItem)

                                        Dim lGwItem As SwatInput.clsGwItem = GwDefault(lSubId, lHruSubCount, lLandUseStr, lSoilClass, lSlope_CdStr)
                                        If lHaveParams Then UpdateFromTable(lGwItem, aParamTable)
                                        lSwatInput.Gw.Add(lGwItem)

                                        Dim lMgtItem1 As New SwatInput.clsMgtItem1(lSubId, lHruSubCount, lLandUseStr, lSoilClass, lSlope_CdStr)
                                        Dim lCrop As SwatInput.clsCropItem = lSwatInput.Crop.FindCrop(lLandUseStr)

                                        Dim lUrban As SwatInput.clsUrbanItem = lSwatInput.Urban.FindUrban(lLandUseStr)

                                        Dim lMgtItem2 As New SwatInput.clsMgtItem2(lSubId, lHruSubCount, lLandUseStr, lSoilClass, lSlope_CdStr)
                                        If g_UseMgtCropFile Then
                                            CropInfoFromXml(aProjectFolder, lSwatInput, lMgtItem1, lCrop, lUrban, lMgtItem2, lSolItem)
                                        Else
                                            With lMgtItem1
                                                .BIOMIX = 0.2
                                                If Not lCrop Is Nothing Then
                                                    Select Case lSolItem.HYDGRP
                                                        Case "A" : .CN2 = lCrop.CN2A
                                                        Case "B" : .CN2 = lCrop.CN2B
                                                        Case "C" : .CN2 = lCrop.CN2C
                                                        Case "D" : .CN2 = lCrop.CN2D
                                                    End Select
                                                    'planting
                                                    lMgtItem2 = New SwatInput.clsMgtItem2(lSubId, lHruSubCount, lLandUseStr, lSoilClass, lSlope_CdStr)
                                                    With lMgtItem2
                                                        .CROP = lCrop.CPNM
                                                        .HUSC = 0.15
                                                        .MGT_OP = 1
                                                        .PLANT_ID = lCrop.ICNUM
                                                        'use the .SUBBASIN --> weather stn and get the x, y
                                                        'need to have a subbasin centroid x, y look up table
                                                        'lCatchmentTable.FindFirst(3, lLat)
                                                        'lCatchmentTable.FindFirst(4, lLong)
                                                        .HEATUNITS = HeatUnits(lCrop.CPNM)
                                                        '.HEATUNITS = HeatUnits(lCrop.CPNM, lLat, lLong, aProjectFolder)
                                                    End With
                                                    lSwatInput.Mgt.Add2(lMgtItem2)
                                                    If lCrop.CPNM = "AGRR" Then
                                                        'autofert
                                                        lMgtItem2 = New SwatInput.clsMgtItem2(lSubId, lHruSubCount, lLandUseStr, lSoilClass, lSlope_CdStr)
                                                        With lMgtItem2
                                                            .CROP = lCrop.CPNM
                                                            .MGT_OP = 11
                                                            .HUSC = 0.16
                                                        End With
                                                        lSwatInput.Mgt.Add2(lMgtItem2)
                                                    End If
                                                    'harvest
                                                    lMgtItem2 = New SwatInput.clsMgtItem2(lSubId, lHruSubCount, lLandUseStr, lSoilClass, lSlope_CdStr)
                                                    With lMgtItem2
                                                        .CROP = lCrop.CPNM
                                                        .MGT_OP = 5
                                                        .HUSC = 1.2
                                                    End With
                                                    lSwatInput.Mgt.Add2(lMgtItem2)
                                                    .NROT = 1
                                                ElseIf Not lUrban Is Nothing Then
                                                    Select Case lSolItem.HYDGRP
                                                        Case "A" : .CN2 = lUrban.CN2A
                                                        Case "B" : .CN2 = lUrban.CN2B
                                                        Case "C" : .CN2 = lUrban.CN2C
                                                        Case "D" : .CN2 = lUrban.CN2D
                                                    End Select
                                                    .IURBAN = 1
                                                    .URBLU = 4
                                                    'planting
                                                    lCrop = lSwatInput.Crop.FindCrop("BERM") 'TODO-this is grass-other types?
                                                    lMgtItem2 = New SwatInput.clsMgtItem2(lSubId, lHruSubCount, lLandUseStr, lSoilClass, lSlope_CdStr)
                                                    With lMgtItem2
                                                        .CROP = lCrop.CPNM
                                                        .HUSC = 0.15
                                                        .MGT_OP = 1
                                                        .PLANT_ID = lCrop.ICNUM
                                                        .HEATUNITS = HeatUnits(lCrop.CPNM)
                                                    End With
                                                    lSwatInput.Mgt.Add2(lMgtItem2)
                                                    'harvest
                                                    lMgtItem2 = New SwatInput.clsMgtItem2(lSubId, lHruSubCount, lLandUseStr, lSoilClass, lSlope_CdStr)
                                                    With lMgtItem2
                                                        .CROP = lCrop.CPNM
                                                        .MGT_OP = 5
                                                        .HUSC = 1.2
                                                    End With
                                                    lSwatInput.Mgt.Add2(lMgtItem2)
                                                    .NROT = 1
                                                Else
                                                    .CN2 = 66
                                                    If lLandUseMissing.IndexFromKey(lLandUseStr) = -1 Then
                                                        Logger.Dbg("MissingLU:" & lLandUseStr)
                                                        lLandUseMissing.Add(lLandUseStr)
                                                        .NROT = 0
                                                    End If
                                                End If
                                                'TODO: more from crop??
                                                .USLE_P = 1
                                                .HUSC = 0
                                            End With
                                            lSwatInput.Mgt.Add1(lMgtItem1)
                                        End If
                                        End If
                                End If
                            End If
                        Next
                        For Each lHruItem As SwatInput.clsHruItem In lSubBasinHrus
                            lHruItem.HRU_FR /= lSubBasinAreaTotal
                            lSwatInput.Hru.Add(lHruItem)
                        Next

                        lHruCount += lHruSubCount

                        Dim lSubBasin As New SwatInput.clsSubBsnItem(lSubId)
                        With lSubBasin
                            .COMID = lComId
                            .SUB_KM = lSubBasinAreaTotal / 1000000.0 'm2 -> km2

                            lFlowlineTable.FindFirst(2, lComId)
                            'TODO: this seems to be how OpenSWAT does this!!!!!
                            .CH_L1 = lFlowlineTable.Value(6) 'TODO: need trib values here!!!
                            .CH_S1 = lFlowlineTable.Value(5) 'TODO: need trib values here!!!
                            .CH_W1 = lFlowlineTable.Value(3) 'TODO: need trib values here!!!
                            .HRUTOT = lHruSubCount
                            .FCST_REG = 1
                            .CH_K1 = 0.5
                            .CH_N1 = 0.014

                            lCatchmentTable.FindFirst(2, lComId)
                            .SUB_LAT = lCatchmentTable.Value(3)
                            .SUB_ELEV = lCatchmentTable.Value(5)
                            Dim lSubLng As Double = lCatchmentTable.Value(4)
                            Dim lMinDistance As Double = 1.0E+30
                            Dim lRowClose As DataRow = Nothing
                            Dim lFieldLat As Integer = lWeatherGenAllDataTable.Columns.IndexOf("WLATITUDE")
                            Dim lFieldLng As Integer = lWeatherGenAllDataTable.Columns.IndexOf("WLONGITUDE")
                            Dim lFieldStn As Integer = lWeatherGenAllDataTable.Columns.IndexOf("STATION")
                            For Each lWeatherRow As DataRow In lWeatherGenAllDataTable.Rows
                                Dim lDistance = Math.Sqrt((lWeatherRow(lFieldLat) - .SUB_LAT) ^ 2 + _
                                                          (lWeatherRow(lFieldLng) - lSubLng) ^ 2)
                                If lDistance < lMinDistance Then
                                    lRowClose = lWeatherRow
                                    lMinDistance = lDistance
                                End If
                            Next

                            If lRowClose IsNot Nothing Then
                                lSwatInput.Wgn.Add(New SwatInput.clsWgnItem(.SUBBASIN, lRowClose))
                            Else
                                Logger.Dbg("Could not find weather station for " & .SUBBASIN)
                            End If
                        End With
                        lSwatInput.SubBsn.Add(lSubBasin)

                        lSwatInput.Swq.Add(SwqDefault(lSubId))
                        Dim lWusItem As New SwatInput.clsWusItem(lSubId)
                        lSwatInput.Wus.Add(lWusItem)
                        lSwatInput.Pnd.Add(PndDefault(lSubId))
                        Dim lRteItem As New SwatInput.clsRteItem(lSubId)
                        With lRteItem
                            .CH_L2 = lFlowlineTable.Value(6)
                            .CH_S2 = lFlowlineTable.Value(5)
                            .CH_D = lFlowlineTable.Value(4)
                            .CH_W2 = lFlowlineTable.Value(3)
                            .CH_N2 = 0.014
                            If .CH_D > 0 Then
                                .CH_WDR = .CH_W2 / .CH_D
                            Else
                                .CH_WDR = 0
                            End If
                        End With
                        lSwatInput.Rte.Add(lRteItem)
                    Next
                    Logger.Dbg("SWATDatabaseBuilt S " & lComIdToBsnId.Count & " H " & lHruCount)

                    If lLandUseMissing.Count > 0 Then
                        For Each lLandUseStr As String In lLandUseMissing
                            Logger.Msg("***** FatalError:MissingLandUse " & lLandUseStr & vbCrLf & "Update 'crop.dat' or reclassification in LuId2Str")
                        Next
                        Stop
                    End If
                    Dim lCioItem As SwatInput.clsCIOItem = lSwatInput.CIO.Item
                    Dim lDate(5) As Integer
                    lDate(0) = lCioItem.IYR
                    lDate(1) = 1
                    lDate(2) = 1
                    Dim lDateStart As Double = Date2J(lDate)
                    lDate(0) += lCioItem.NBYR
                    Dim lDateEnd As Double = Date2J(lDate)
                    Logger.Dbg("Writing " & timdifJ(lDateStart, lDateEnd, 6, 1) & " years of data " & MemUsage())

                    WriteSwatMetInput(aCacheFolder, aProjectFolder, lTxtInOutFolder, lDateStart, lDateEnd)

                    'TODO: allow multiple gages
                    For Each lSubBsn As SwatInput.clsSubBsnItem In lSwatInput.SubBsn.Items
                        With lSubBsn
                            .IRGAGE = 1
                            .ITGAGE = 1
                            .IWGAGE = 1
                            .ISGAGE = 1
                        End With
                        lSwatInput.SubBsn.Update(lSubBsn)
                    Next
                    Logger.Dbg("MetDataWritten " & MemUsage())

                    .SaveAllTextInput()
                    Logger.Dbg("SWATInputSaved")
                End With
            Catch lEx As Exception
                Logger.Dbg(lEx.Message)
            End Try
        End If
    End Sub

    Private Sub UpdateFromTable(ByVal aUpdateMe As Object, ByVal aTable As atcUtility.atcTable)
        For lFieldIndex As Integer = 1 To aTable.NumFields
            If atcUtility.SetSomething(aUpdateMe, aTable.FieldName(lFieldIndex), aTable.Value(lFieldIndex), False).Length = 0 Then
                MapWinUtility.Logger.Dbg("UpdateFromTable: " & aTable.FieldName(lFieldIndex) & " = " & aTable.Value(lFieldIndex))
            End If
        Next
    End Sub

    Private Function HeatUnits(ByVal aCropName As String) As Double
        'TODO: use following function from OpenSWAT when all arguments and datafiles are understood
        Dim lHeatUnits As Double = 0

        'Return calHeatUnits(aCropName, aX, aY, aDataDirectory)

        Select Case aCropName
            Case "AGRR" : lHeatUnits = 1841.061
            Case "FRSD" : lHeatUnits = 2234.698
            Case "FRSE" : lHeatUnits = 4572.529
            Case "HAY", "URLD" : lHeatUnits = 1454.7
            Case "WETF" : lHeatUnits = 2151.05
            Case Else
                lHeatUnits = 2000.0
        End Select
        Return lHeatUnits
    End Function

    Private Function HeatUnits(ByVal aCropName As String, ByVal aLat As Single, ByVal along As Single, ByVal aDataDirectory As String) As Double
        'TODO: use following function from OpenSWAT when all arguments and datafiles are understood
        Return Nothing 'calHeatUnits(aCropName, along, aLat, aDataDirectory)
    End Function

    Private Function SlopeId2Str(ByVal aSlopeId As Integer) As String
        Dim lSlopeId2Str As String = ""
        Select Case aSlopeId
            Case 0 : lSlopeId2Str = "<un>"
            Case 1 : lSlopeId2Str = "0-0.5"
            Case 2 : lSlopeId2Str = "0.5-2"
            Case 3 : lSlopeId2Str = "2-9999"
            Case Else : lSlopeId2Str = "<un>"
        End Select
        Return lSlopeId2Str
    End Function

    Friend Function SoilId2Str(ByVal aSwatInput As SwatInput, ByVal aSoilId As String, _
                               ByVal aWhichSoilIndex As WhichSoilIndex) As String
        aSoilId = aSoilId.Trim
        If Not IsNumeric(aSoilId) Then
            Return aSoilId 'Must already be the string version with state abbreviation
        End If
        Dim lNumericSTMUID As String
        Dim lStateString As String = ""

        Select Case aWhichSoilIndex
            Case WhichSoilIndex.USGS
                Select Case aSoilId.Substring(0, 2)
                    Case "27" : lStateString = "MI"
                    Case "28" : lStateString = "MN"
                    Case "58" : lStateString = "WI"
                End Select
                If lStateString.Length = 2 Then
                    Return lStateString & aSoilId.Substring(2)
                End If
            Case WhichSoilIndex.SWATSTATSGO
                Dim lFindMUID As DataTable = aSwatInput.QuerySoils("Select STMUID From Stats_grd_lu Where Value_=" & aSoilId & ";")
                If lFindMUID.Rows.Count > 0 Then
                    lNumericSTMUID = lFindMUID.Rows(0).Item(0)
                Else
                    lNumericSTMUID = aSoilId
                End If
                Dim lFindStateTxt As DataTable = aSwatInput.QuerySoils("Select StateTxt From tblStmuidLu Where StateNum='" & SafeSubstring(lNumericSTMUID, 0, 2) & "';")
                If lFindStateTxt.Rows.Count > 0 Then
                    Return lFindStateTxt.Rows(0).Item(0) & lNumericSTMUID.Substring(2)
                End If
        End Select

        Return "<unk>"
    End Function

    Friend Function LuId2Str(ByVal aLuId As Integer) As String
        Dim lLuId2Str As String = ""
        Select Case aLuId
            Case 11 : lLuId2Str = "WATR"
            Case 21 : lLuId2Str = "URLD"
            Case 22 : lLuId2Str = "URML"
            Case 23 : lLuId2Str = "URMD"
            Case 24 : lLuId2Str = "URHD"
            Case 31 : lLuId2Str = "RNGB" 'barren land -> range / brush
            Case 41 : lLuId2Str = "FRSD"
            Case 42 : lLuId2Str = "FRSE"
            Case 43 : lLuId2Str = "FRST"
            Case 52 : lLuId2Str = "RNGB" 'scrub -> range / brush
            Case 71 : lLuId2Str = "PAST"
            Case 81 : lLuId2Str = "HAY"
            Case 82 : lLuId2Str = "AGRR" 'generic row crop
            Case 90 : lLuId2Str = "WETL"
            Case 95 : lLuId2Str = "WETF"
                'ADDED for APES
                'KLW July 14, 2009
            Case 100 : lLuId2Str = "CORN" 'Corn, all
            Case 200 : lLuId2Str = "COTS" 'Cotton() - 2 types
            Case 400 : lLuId2Str = "SGHY" 'Sorghum() - sorghum hay
            Case 500 : lLuId2Str = "SOYB" 'Soybeans()
            Case 1000 : lLuId2Str = "PNUT" 'Peanuts()
            Case 1100 : lLuId2Str = "TOBC" 'Tobacco()
            Case 2100 : lLuId2Str = "BARL" 'Barley() - spring barley
            Case 2400 : lLuId2Str = "WWHT" 'Winter(Wheat)
            Case 2500 : lLuId2Str = "HAY" 'Other(Grains / Hay)
            Case 2600 : lLuId2Str = "CORN" 'Wheat/Soybeans Double Cropped
            Case 2700 : lLuId2Str = "RYE" 'Rye()
            Case 2800 : lLuId2Str = "OATS" 'Oats()
            Case 3600 : lLuId2Str = "ALFA" 'Alfalfa()
            Case 4300 : lLuId2Str = "POTA" 'Potatoes()
            Case 4400 : lLuId2Str = "AGRR" 'Other(Crops)
            Case 4600 : lLuId2Str = "SPOT" 'Sweet(Potatoes)
            Case 5000 : lLuId2Str = "AGRR" 'Cucumbers (NC); Other Crops (VA)
            Case 5100 : lLuId2Str = "AGGR" 'Processed(Vegetables(VA))
            Case 5800 : lLuId2Str = "POTA" 'Potatoes(VA)
            Case 7100 : lLuId2Str = "ORCD" 'State722, Cottonwood Tree, Orchard, Other Fruits & Nuts - orchard

            Case Else
                Dim lMissingLandUse As String = "RNGB"
                Logger.Dbg("MissingLandUseCode " & aLuId & " defaulting to " & lMissingLandUse)
                lLuId2Str = lMissingLandUse
        End Select
        Return lLuId2Str
    End Function

    Private Function CropInfoFromXml(ByVal aProjectFolder As String, _
                                     ByVal lSwatInput As SwatInput, _
                                     ByVal lMgtItem1 As SwatInput.clsMgtItem1, _
                                     ByVal lCrop As SwatInput.clsCropItem, _
                                     ByVal lUrban As SwatInput.clsUrbanItem, _
                                     ByVal lMgtItem2In As SwatInput.clsMgtItem2, _
                                     ByVal lSolItem As SwatInput.clsSolItem) As String

        Dim cropNode As Xml.XmlNode
        cropNode = Nothing
        Dim msgException As String = ""
        Dim heatUnit As Single
        Dim lMgtItem2 As SwatInput.clsMgtItem2

        Try

            If Not (lCrop Is Nothing) Then

                If (SWATParamsXmlDoc Is Nothing) Then
                    SWATParamsXmlDoc = New Xml.XmlDocument
                    Dim paramsFile = IO.Path.Combine(aProjectFolder, "CropLandcover\SWATParams.xml")
                    SWATParamsXmlDoc.Load(paramsFile)
                End If

                cropNode = SWATParamsXmlDoc.SelectSingleNode("/SWATData/Parameterset[@name='APES']/Crop[@name='" + lCrop.CPNM + "']")
                If (cropNode Is Nothing) Then
                    cropNode = SWATParamsXmlDoc.SelectSingleNode("/SWATData/Parameterset[@name='APES']/Crop[@name='DEFAULT']")
                End If

                Dim mgt1Node As Xml.XmlNode
                Dim nodeVal As Xml.XmlNode
                mgt1Node = SWATParamsXmlDoc.SelectSingleNode("/SWATData/Parameterset[@name='APES']/MGT1")

                With lMgtItem1
                    'Try

                    nodeVal = mgt1Node.SelectSingleNode("BIOMIX")
                    If Not (nodeVal Is Nothing) Then
                        .BIOMIX = Convert.ToSingle(nodeVal.InnerText)
                    End If

                    nodeVal = mgt1Node.SelectSingleNode("USLE_P")
                    If Not (nodeVal Is Nothing) Then
                        .USLE_P = Convert.ToSingle(nodeVal.InnerText)
                    End If

                    nodeVal = mgt1Node.SelectSingleNode("HUSC")
                    If Not (nodeVal Is Nothing) Then
                        .HUSC = Convert.ToSingle(nodeVal.InnerText)
                    End If

                    nodeVal = mgt1Node.SelectSingleNode("NROT")
                    If Not (nodeVal Is Nothing) Then
                        .NROT = Convert.ToSingle(nodeVal.InnerText)
                    End If

                    'Catch ex As Exception
                    '    Dim msg As String
                    '    msg = ex.Message
                    'End Try

                    'Try

                    Select Case lSolItem.HYDGRP
                        Case "A" : .CN2 = lCrop.CN2A
                        Case "B" : .CN2 = lCrop.CN2B
                        Case "C" : .CN2 = lCrop.CN2C
                        Case "D" : .CN2 = lCrop.CN2D
                    End Select

                    nodeVal = cropNode.SelectSingleNode("HeatUnit")
                    If Not (nodeVal Is Nothing) Then
                        heatUnit = Convert.ToSingle(nodeVal.InnerText)
                    End If

                    'Catch ex As Exception
                    '    Dim msg As String
                    '    msg = ex.Message
                    'End Try

                    Dim mgt2Nodes As Xml.XmlNodeList
                    Dim mgt2Node As Xml.XmlNode
                    Dim attr As Xml.XmlAttribute
                    Dim mgtType As String
                    mgt2Nodes = cropNode.SelectNodes("MGT2")
                    For Each mgt2Node In mgt2Nodes

                        'Try
                        lMgtItem2 = lMgtItem2In.Clone()
                        attr = mgt2Node.Attributes("type")

                        If Not (attr Is Nothing) Then
                            mgtType = attr.InnerText
                            If (String.Compare(mgtType, "planting", True) = 0) Then
                                lMgtItem2.HEATUNITS = heatUnit
                                lMgtItem2.PLANT_ID = lCrop.ICNUM
                            End If
                        End If

                        If Not (mgt2Node.SelectSingleNode("HUSC") Is Nothing) Then
                            lMgtItem2.HUSC = Convert.ToSingle(mgt2Node.SelectSingleNode("HUSC").InnerText)
                        End If
                        If Not (mgt2Node.SelectSingleNode("MGT_OP") Is Nothing) Then
                            lMgtItem2.MGT_OP = Convert.ToInt32(mgt2Node.SelectSingleNode("MGT_OP").InnerText)
                        End If
                        lMgtItem2.CROP = lCrop.CPNM

                        lSwatInput.Mgt.Add2(lMgtItem2)
                        'Catch ex As Exception
                        '    Dim msg As String
                        '    msg = ex.Message

                        'End Try
                    Next
                End With

            ElseIf Not (lUrban Is Nothing) Then
                'Try
                With lMgtItem1
                    Select Case lSolItem.HYDGRP
                        Case "A" : .CN2 = lUrban.CN2A
                        Case "B" : .CN2 = lUrban.CN2B
                        Case "C" : .CN2 = lUrban.CN2C
                        Case "D" : .CN2 = lUrban.CN2D
                    End Select
                    .IURBAN = 1
                    .URBLU = 4
                    'planting
                    lCrop = lSwatInput.Crop.FindCrop("BERM") 'TODO-this is grass-other types?
                    'Dim lMgtItem2 As New SwatInput.clsMgtItem2(lSubId, lHruSubCount, lLandUseStr, lSoilClass, lSlope_CdStr)
                    lMgtItem2 = lMgtItem2In.Clone()
                    With lMgtItem2
                        .CROP = lCrop.CPNM
                        .HUSC = 0.15
                        .MGT_OP = 1
                        .PLANT_ID = lCrop.ICNUM
                        .HEATUNITS = HeatUnits(lCrop.CPNM)
                    End With
                    lSwatInput.Mgt.Add2(lMgtItem2)
                    'harvest
                    lMgtItem2 = lMgtItem2In.Clone()
                    With lMgtItem2
                        .CROP = lCrop.CPNM
                        .MGT_OP = 5
                        .HUSC = 1.2
                    End With
                    lSwatInput.Mgt.Add2(lMgtItem2)
                    'Catch ex As Exception
                    '    Dim msg As String
                    '    msg = ex.Message
                    'End Try
                End With

            Else
                With lMgtItem1
                    .CN2 = 66
                End With

                'If lLandUseMissing.IndexFromKey(lLandUseStr) = -1 Then
                'Logger.Dbg("MissingLU:" & lLandUseStr)
                'lLandUseMissing.Add(lLandUseStr)
            End If

            lSwatInput.Mgt.Add1(lMgtItem1)

            Return ""

        Catch ex As Exception
            Dim src As String
            msgException = ex.Message
            src = ex.Source

        Finally
            If (msgException.Length > 0) Then
                Throw New Exception("Error in CropInfoFromXml: " & msgException)
            End If

        End Try
        Return ""
    End Function

End Module
