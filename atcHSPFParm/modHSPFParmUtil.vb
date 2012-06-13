Imports atcUtility
Imports atcUCI
Imports atcMwGisUtility
Imports MapWinUtility
Imports System.Data

Module modHSPFParmUtil

    Friend Class Scn
        Public UCIFilename As String
        Public LandUseType As String
        Public Channels As String
        Public WQConst As String
        Public ChemSrc As String
        Public Purpose As String
        Public Version As String
        Public Ref As String
        Public ContactName As String
        Public ContactOrg As String
        Public ContactNumber As String
        Public Comments As String
        Public ReqStartDate As String
        Public ReqEndDate As String
        Public ReqNumReaches As String
        Public ReqNumSegments As String
        Public ReqUnits As String
        Public ScnShortName As String
        Public ScnType As String = ""
    End Class

    Friend Class Prj
        Public PRJName As String
        Public HUC As String
        Public Loc As String
        Public DrainageArea As String
        Public Comments As String
        Public PhysiographicSetting As String
        Public Weather As String
        Public Latitude As String
        Public Longitude As String
        Public DBId As Integer
        Public Scenarios As Generic.List(Of Scn)
        Public Sub New()
            Scenarios = New Generic.List(Of Scn)
        End Sub
        Public Sub Clear()
            Scenarios.Clear()
            Scenarios = Nothing
        End Sub
    End Class

    Public Sub AddParmData(ByVal aInpFilename As String, ByVal aParmDB As atcMDB)

        If aParmDB Is Nothing Then
            Logger.Msg("No database is available, abort.")
            Exit Sub
        End If
        If Not IO.File.Exists(aInpFilename) Then
            Logger.Msg("HSPF Parm batch input file doesn't exist, " & vbCrLf & aInpFilename)
            Exit Sub
        End If

        'Dim lPath As String = IO.Path.GetDirectoryName(aInpFilename)
        'Dim lMDBFilename As String = IO.Path.Combine(lPath, "HSPFParmV2.mdb")
        'If Not IO.File.Exists(lMDBFilename) Then
        '    lMDBFilename = FindFile("Locate HSPFParmV2", lMDBFilename, "mdb")
        'End If
        'If Not IO.File.Exists(lMDBFilename) Then
        '    'TODO: Create a new MDB
        '    'CreateNewHSPFParmMDB()
        '    Logger.Msg("No database is found. Abort.")
        '    Exit Sub
        'End If

        Dim lPrjsToAdd As New atcCollection()

        Dim lSR As New IO.StreamReader(aInpFilename)
        Dim lOneLine As String = lSR.ReadLine()
        While Not lSR.EndOfStream

            If lOneLine.StartsWith("#") Then
                lOneLine = lSR.ReadLine()
                Continue While
            End If

            If lOneLine.StartsWith("PRJ") Then
                Dim lPrj As New Prj()
                Dim lArr() As String = lOneLine.Split(",")
                With lPrj
                    .PRJName = lArr(1).Replace("'", "").Trim()
                    .HUC = lArr(2).Replace("'", "").Trim()
                    .Loc = lArr(3).Replace("'", "").Trim()
                    .DrainageArea = lArr(4).Replace("'", "").Trim()
                    .Comments = lArr(5).Replace("'", "").Trim()
                    .PhysiographicSetting = lArr(6).Replace("'", "").Trim()
                    .Weather = lArr(7).Replace("'", "").Trim()
                    .Latitude = lArr(8).Replace("'", "").Trim()
                    .Longitude = lArr(9).Replace("'", "").Trim()
                End With
                lPrjsToAdd.Add(lPrj)
                While Not lSR.EndOfStream
                    lOneLine = lSR.ReadLine()
                    If lOneLine.StartsWith("SCN") Then
                        lArr = lOneLine.Split(",")
                        If lArr.Length >= 2 Then
                            Dim lScn As New Scn()
                            With lScn
                                .UCIFilename = lArr(1).Replace("'", "").Trim()

                                If lArr.Length >= 3 Then .LandUseType = lArr(2).Replace("'", "").Trim()
                                If lArr.Length >= 4 Then .Channels = lArr(3).Replace("'", "").Trim()
                                If lArr.Length >= 5 Then .WQConst = lArr(4).Replace("'", "").Trim()
                                If lArr.Length >= 6 Then .ChemSrc = lArr(5).Replace("'", "").Trim()
                                If lArr.Length >= 7 Then .Purpose = lArr(6).Replace("'", "").Trim()
                                If lArr.Length >= 8 Then .Version = lArr(7).Replace("'", "").Trim()
                                If lArr.Length >= 9 Then .Ref = lArr(8).Replace("'", "").Trim()
                                If lArr.Length >= 10 Then .ContactName = lArr(9).Replace("'", "").Trim()
                                If lArr.Length >= 11 Then .ContactOrg = lArr(10).Replace("'", "").Trim()
                                If lArr.Length >= 12 Then .ContactNumber = lArr(11).Replace("'", "").Trim()
                                If lArr.Length >= 13 Then .Comments = lArr(12).Replace("'", "").Trim()
                            End With
                            lPrj.Scenarios.Add(lScn)
                        End If


                    ElseIf lOneLine.StartsWith("PRJ") Then
                        Exit While
                    Else
                        lOneLine = lSR.ReadLine()
                        Exit While
                    End If
                End While
            End If
        End While
        lSR.Close() : lSR = Nothing

        'Add to DB
        For Each lPrj As Prj In lPrjsToAdd
            AddWatershed(lPrj, aParmDB)
            For Each lScn As Scn In lPrj.Scenarios
                Dim lHspfUci As HspfUci = SetScenario(lScn)
                AddScenario(lScn, lPrj, lHspfUci, aParmDB, True)
            Next
        Next
    End Sub

    Public Sub AddWatershed(ByRef aPrj As Prj, ByVal aParmDB As atcMDB)
        Dim lProject As String = aPrj.PRJName
        Dim lHuc As String = aPrj.HUC
        Dim lArea As String = aPrj.DrainageArea
        Dim lComments As String = aPrj.Comments
        If lComments.Length = 0 Then lComments = " "
        Dim lLocation As String = aPrj.Loc
        Dim lSetting As String = aPrj.PhysiographicSetting
        Dim lWeather As String = aPrj.Weather
        Dim lLatitude As String = aPrj.Latitude
        Dim lLongitude As String = aPrj.Longitude
        'check values
        If lProject.Trim.Length = 0 Or lHuc.Trim.Length = 0 Or lArea.Trim.Length = 0 Or lLocation.Trim.Length = 0 Or lSetting.Trim.Length = 0 Or lWeather.Trim.Length = 0 Then
            Logger.Msg("One or more required fields does not have a value.", MsgBoxStyle.OkOnly, "BASINS HSPFParm")
            Exit Sub
        End If
        If Not IsNumeric(lLatitude) Or Not IsNumeric(lLongitude) Then
            Logger.Msg("Latitude and Longitude fields must contain numeric values.", MsgBoxStyle.OkOnly, "BASINS HSPFParm")
            Exit Sub
        End If
        Dim lX As Double = CDbl(lLongitude)
        Dim lY As Double = CDbl(lLatitude)
        GisUtil.ProjectPoint(lX, lY, "+proj=longlat +datum=NAD83", GisUtil.ProjectProjection)
        'find next available id number 
        Dim lTable As DataTable = aParmDB.GetTable("WatershedData")
        Dim lMaxId As Integer = 0
        Dim lTmpId As Integer = 0
        For lRow As Integer = 0 To lTable.Rows.Count - 1
            lTmpId = lTable.Rows(lRow).Item(0).ToString
            If lTmpId > lMaxId Then
                lMaxId = lTmpId
            End If
        Next
        'save to database
        Dim lValues As New Collection
        lValues.Add(lMaxId + 1)
        lValues.Add("'" & lProject & "'")
        lValues.Add("'" & lLocation & "'")
        lValues.Add("'" & lSetting & "'")
        lValues.Add("'" & lWeather & "'")
        lValues.Add("'" & lArea & "'")
        lValues.Add("'" & lHuc & "'")
        lValues.Add(lLatitude)
        lValues.Add(lLongitude)
        lValues.Add(lX)
        lValues.Add(lY)
        lValues.Add("'" & lComments & "'")
        Try
            aParmDB.InsertRowIntoTable("WatershedData", lValues)
            'find the id number for this newly added watershed
            Dim lSQL As String = "SELECT ID FROM WatershedData ORDER BY ID DESC;"
            Dim lTempTable As DataTable = aParmDB.GetTable(lSQL)
            Dim lParmDataId As Integer = lTempTable.Rows(0).Item(0)
            lTempTable.Clear()
            lTempTable = Nothing
            aPrj.DBId = lParmDataId
        Catch ex As Exception
            Logger.Msg("Failed to add a new watershed, " & lProject)
            Exit Sub
        End Try

        Try
            'save to shapefile
            If GisUtil.IsLayer("Watershed") Then
                Dim lLayerIndex As Integer = GisUtil.LayerIndex("Watershed")
                GisUtil.AddPoint(lLayerIndex, lX, lY)
                Dim lNumFeature As Integer = GisUtil.NumFeatures(lLayerIndex) - 1
                GisUtil.SetFeatureValue(lLayerIndex, GisUtil.FieldIndex(lLayerIndex, "ID"), lNumFeature, lMaxId + 1)
                GisUtil.SetFeatureValue(lLayerIndex, GisUtil.FieldIndex(lLayerIndex, "WATERSNAME"), lNumFeature, lProject)
                GisUtil.SetFeatureValue(lLayerIndex, GisUtil.FieldIndex(lLayerIndex, "HUC"), lNumFeature, lHuc)
                GisUtil.SetFeatureValue(lLayerIndex, GisUtil.FieldIndex(lLayerIndex, "LOCATION"), lNumFeature, lLocation)
                GisUtil.SetFeatureValue(lLayerIndex, GisUtil.FieldIndex(lLayerIndex, "DRAINAGEAR"), lNumFeature, lArea)
                GisUtil.SetFeatureValue(lLayerIndex, GisUtil.FieldIndex(lLayerIndex, "COMMENTS"), lNumFeature, lComments)
                GisUtil.SetFeatureValue(lLayerIndex, GisUtil.FieldIndex(lLayerIndex, "PHYS"), lNumFeature, lSetting)
                GisUtil.SetFeatureValue(lLayerIndex, GisUtil.FieldIndex(lLayerIndex, "WEATHER"), lNumFeature, lWeather)
            End If
        Catch ex As Exception
            'doesn't matter
        End Try
    End Sub

    Private Sub AddScenario(ByVal aScn As Scn, ByVal aPrj As Prj, ByVal aUci As HspfUci, ByVal aParmDB As atcMDB, Optional ByVal aCheckRange As Boolean = False)
        'get values from user interface
        Dim lUciName As String = aScn.UCIFilename
        Dim lName As String = aScn.ScnShortName
        Dim lType As String = aScn.ScnType
        If lType Is Nothing OrElse lType.Length = 0 Then lType = " "
        Dim lWatershedId As String = aPrj.DBId.ToString
        Dim lStartDate As String = aScn.ReqStartDate
        If lStartDate.Length = 0 Then lStartDate = " "
        Dim lEndDate As String = aScn.ReqEndDate
        If lEndDate.Length = 0 Then lEndDate = " "
        Dim lUnits As String = aScn.ReqUnits
        If lUnits.Length = 0 Then lUnits = " "
        Dim lNumSegments As String = aScn.ReqNumSegments
        If lNumSegments.Length = 0 Then lNumSegments = " "
        Dim lNumReaches As String = aScn.ReqNumReaches
        If lNumReaches.Length = 0 Then lNumReaches = " "
        Dim lLandUseType As String = aScn.LandUseType
        If lLandUseType Is Nothing OrElse lLandUseType.Length = 0 Then lLandUseType = " "
        Dim lChannels As String = aScn.Channels
        If lChannels Is Nothing OrElse lChannels.Length = 0 Then lChannels = " "
        Dim lWQConstituents As String = aScn.WQConst
        If lWQConstituents Is Nothing OrElse lWQConstituents.Length = 0 Then lWQConstituents = " "
        Dim lSources As String = aScn.ChemSrc
        If lSources Is Nothing OrElse lSources.Length = 0 Then lSources = " "
        Dim lPurpose As String = aScn.Purpose
        If lPurpose Is Nothing OrElse lPurpose.Length = 0 Then lPurpose = " "
        Dim lVersion As String = aScn.Version
        If lVersion Is Nothing OrElse lVersion.Length = 0 Then lVersion = " "
        Dim lReference As String = aScn.Ref
        If lReference Is Nothing OrElse lReference.Length = 0 Then lReference = " "
        Dim lContactName As String = aScn.ContactName
        If lContactName Is Nothing OrElse lContactName.Length = 0 Then lContactName = " "
        Dim lOrganization As String = aScn.ContactOrg
        If lOrganization Is Nothing OrElse lOrganization.Length = 0 Then lOrganization = " "
        Dim lPhone As String = aScn.ContactNumber
        If lPhone Is Nothing OrElse lPhone.Length = 0 Then lPhone = " "
        Dim lComments As String = aScn.Comments
        If lComments Is Nothing OrElse lComments.Length = 0 Then lComments = " "

        'check values
        If lUciName.Trim.Length = 0 Or lName.Trim.Length = 0 Or lWatershedId.Trim.Length = 0 Then
            Logger.Msg("One or more required fields does not have a value.", MsgBoxStyle.OkOnly, "BASINS HSPFParm")
            Exit Sub
        End If

        Logger.Status("SHOW")
        Logger.Status("Adding Scenario to database...")
        'find next available id number 
        Dim lTable As DataTable = aParmDB.GetTable("ScenarioData")
        Dim lMaxId As Integer = 0
        Dim lTmpId As Integer = 0
        For lRow As Integer = 0 To lTable.Rows.Count - 1
            lTmpId = lTable.Rows(lRow).Item(0).ToString
            If lTmpId > lMaxId Then
                lMaxId = lTmpId
            End If
        Next
        Dim lNewScenarioId As Integer = lMaxId + 1

        'save to database
        Dim lValues As New Collection
        lValues.Add(lNewScenarioId)
        lValues.Add("'" & lName & "'")
        lValues.Add("'" & lType & "'")
        lValues.Add("'" & lUciName & "'")
        lValues.Add(lWatershedId)
        lValues.Add("'" & lStartDate & "'")
        lValues.Add("'" & lEndDate & "'")
        lValues.Add(lUnits)
        lValues.Add(lNumSegments)
        lValues.Add(lNumReaches)
        lValues.Add("'" & lLandUseType & "'")
        lValues.Add("'" & lChannels & "'")
        lValues.Add("'" & lWQConstituents & "'")
        lValues.Add("'" & lSources & "'")
        lValues.Add("'" & lPurpose & "'")
        lValues.Add("'" & lVersion & "'")
        lValues.Add("'" & lReference & "'")
        lValues.Add("'" & lContactName & "'")
        lValues.Add("'" & lOrganization & "'")
        lValues.Add("'" & lPhone & "'")
        lValues.Add("'" & lComments & "'")
        aParmDB.InsertRowIntoTable("ScenarioData", lValues)

        Dim lRangeCheck As Boolean = aCheckRange
        If Not aUci Is Nothing Then
            'look for perlnd, implnd, rchres operation types
            If aUci.OpnBlks("PERLND").Count > 0 Then
                'this operation type exists
                Logger.Status("Adding PERLND to database...")
                GetOperInfo(aParmDB, aUci, "PERLND", lNewScenarioId, lRangeCheck)
            End If
            If aUci.OpnBlks("IMPLND").Count > 0 Then
                'this operation type exists
                Logger.Status("Adding IMPLND to database...")
                GetOperInfo(aParmDB, aUci, "IMPLND", lNewScenarioId, lRangeCheck)
            End If
            If aUci.OpnBlks("RCHRES").Count > 0 Then
                'this operation type exists
                Logger.Status("Adding RCHRES to database...")
                GetOperInfo(aParmDB, aUci, "RCHRES", lNewScenarioId, lRangeCheck)
            End If
        End If
        Logger.Status("")
        Logger.Status("HIDE")

        If FileExists(aUci.Name) Then 'make a copy of the UCI file just added as a new scenario
            Dim lArchiveFolder As String = IO.Path.Combine(IO.Path.GetDirectoryName(aParmDB.Name), "Archive")
            If Not IO.Directory.Exists(lArchiveFolder) Then MkDir(lArchiveFolder)
            TryCopy(aUci.Name, IO.Path.Combine(lArchiveFolder, IO.Path.GetFileName(aUci.Name)))
        End If

    End Sub

    Public Sub GetOperInfo(ByVal aParmDB As atcMDB, ByVal aUci As HspfUci, ByVal aOpName As String, ByVal aScenarioID As Integer, ByVal aRangeCheck As Boolean)
        'find next available id number 
        Dim lTable As DataTable = aParmDB.GetTable("SegData")
        Dim lNewSegId As Integer = 0
        Dim lTmpId As Integer = 0
        lNewSegId = lTable.Rows(lTable.Rows.Count - 1).Item(0).ToString

        'find next available parm data id number 
        Dim lSQL As String = "SELECT ID FROM ParmData ORDER BY ID DESC;"
        Dim lTempTable As DataTable = aParmDB.GetTable(lSQL)
        Dim lParmDataId As Integer = lTempTable.Rows(0).Item(0) + 1
        lTempTable.Clear()
        lTempTable = Nothing

        Dim lParmDataTable As DataTable = aParmDB.GetTable("ParmData")
        'lParmDataId = lParmDataTable.Rows(lParmDataTable.Rows.Count - 1).Item(0).ToString

        Dim lOpTypId As Integer = 0
        If aOpName = "PERLND" Then
            lOpTypId = 1
        ElseIf aOpName = "IMPLND" Then
            lOpTypId = 2
        ElseIf aOpName = "RCHRES" Then
            lOpTypId = 3
        End If

        Dim lProgressTotal As Integer = 0
        For Each lOpn As HspfOperation In aUci.OpnBlks(aOpName).Ids
            lProgressTotal += lOpn.Tables.Count
        Next
        Dim lProgressCount As Integer = 0
        'find information about hspf operation type
        Dim lMsgBuilder As Text.StringBuilder = Nothing
        For Each lOpn As HspfOperation In aUci.OpnBlks(aOpName).Ids
            lNewSegId += 1
            Dim lValues As New Collection
            lValues.Add(lNewSegId)
            Dim lTmp As String = lOpn.Name & lOpn.Id.ToString.PadLeft(8)
            lValues.Add("'" & lTmp & "'")
            lValues.Add("'" & lOpn.Description & "'")
            lValues.Add(lOpTypId)
            lValues.Add(aScenarioID)
            aParmDB.InsertRowIntoTable("SegData", lValues)

            lMsgBuilder = New Text.StringBuilder
            For Each lTab As HspfTable In lOpn.Tables
                lProgressCount += 1
                Logger.Progress(lProgressCount, lProgressTotal)
                Dim lOccur As Integer = 1
                If lTab.OccurCount > 1 Or lTab.OccurIndex > 1 Then
                    'get occurrance number 
                    If lTab.OccurIndex > 0 Then
                        lOccur = lTab.OccurIndex
                    Else
                        lOccur = lTab.OccurNum
                    End If
                End If
                Dim lTableId As Integer = TableIDFromTableName(aParmDB, lTab.Name, lOpTypId)   'like PRINT-INFO 1(PERLND) returns 2
                If lTableId > 0 Then
                    For Each lParm As HspfParm In lTab.Parms
                        lParmDataId += 1
                        'myParmData = myDB.OpenRecordset("ParmData", dbOpenDynaset)
                        'ID	    ParmID	SegID	Occur	Value
                        '1	    1	    1	    1	    0
                        '157	13	    1	    1	    4
                        '545033	600	    1364	5	    1.4E3
                        Dim lParmId As Integer = ParmIDFromParmName(aParmDB, lParm.Name, lTableId)   'like AIRTPR 2 returns 13
                        If lParmId > 0 Then
                            If aRangeCheck AndAlso lParm.Def.Typ > 1 Then
                                'check here to see if this real number is within the normal range
                                Dim lMax As Single = 0.0 'max in DB currently
                                Dim lMin As Single = 0.0 'min in DB currently
                                Dim lTMax As Single 'typical max
                                Dim lTMin As Single 'typical min
                                Dim lPMax As Single 'possible max
                                Dim lPMin As Single 'possible min
                                Dim lVal As Single = 0.0
                                Dim lHSPFMax As Single = lParm.Def.Max
                                Dim lHSPFMin As Single = lParm.Def.Min
                                If aUci.GlobalBlock.EmFg = 2 Then
                                    lHSPFMax = lParm.Def.MetricMax
                                    lHSPFMin = lParm.Def.MetricMin
                                End If
                                ParmMinMax(aParmDB, lParmId, lParm.Name, lParm.Parent.Name, lMin, lMax, lTMin, lTMax, lPMin, lPMax)
                                If IsNumeric(lParm.Value) Then
                                    lVal = CSng(lParm.Value)
                                    'Dim lMsg As String = "The value (" & lVal & ") for parameter " & lParm.Name & " (Operation: " & lOpn.Name & ", " & lOpn.Id & ")" & " is outside the ranges below:"
                                    Dim lMsg As String = "The value (" & lVal & ") for parameter " & lParm.Name & " is outside the ranges below:"
                                    Dim lOutOfRange As Boolean = False
                                    If lTMin < lTMax Then
                                        If lVal > lTMax OrElse lVal < lTMin Then
                                            lOutOfRange = True
                                            lMsg &= vbCrLf & "    Typical Min: " & lTMin & " ~ Typical Max: " & lTMax
                                        End If
                                    End If
                                    If lPMin < lPMax Then
                                        If lVal > lPMax OrElse lVal < lPMin Then
                                            lOutOfRange = True
                                            lMsg &= vbCrLf & "    Possible Min: " & lPMin & " ~ Possible Max: " & lPMax
                                        End If
                                    End If
                                    'If lVal > lMax Then
                                    '    Logger.Msg("The value for parameter " & lParm.Name & " is greater than the greatest value of this parameter " & _
                                    '               "in the HSPFParm database." & vbCrLf & vbCrLf & "Operation: " & lOpn.Name & " " & lOpn.Id & _
                                    '               "   Value: " & lVal & "   Max: " & lMax & "   HSPF Max: " & lHSPFMax, MsgBoxStyle.Critical, _
                                    '               "HSPFParm Value Beyond Normal Range")
                                    'End If
                                    'If lVal < lMin Then
                                    '    Logger.Msg("The value for parameter " & lParm.Name & " is lower than the lowest value of this parameter " & _
                                    '               "in the HSPFParm database." & vbCrLf & vbCrLf & "Operation: " & lOpn.Name & " " & lOpn.Id & _
                                    '               "   Value: " & lVal & "   Min: " & lMin & "   HSPF Min: " & lHSPFMin, MsgBoxStyle.Critical, _
                                    '               "HSPFParm Value Beyond Normal Range")
                                    'End If
                                    If lMin < lMax AndAlso (lVal > lMax OrElse lVal < lMin) Then
                                        lOutOfRange = True
                                        lMsg &= vbCrLf & "    Database Min: " & lMin & " ~ Database Max: " & lMax
                                    End If
                                    If lOutOfRange Then
                                        lMsgBuilder.AppendLine(lMsg)
                                        'Logger.Msg(lMsg, MsgBoxStyle.Critical, "HSPFParm Value Beyond Normal Range")
                                    End If
                                End If
                            End If
                            Dim lParmValues As New Collection
                            lParmValues.Add(lParmDataId) 'no need as it is managed by access
                            lParmValues.Add(lParmId)
                            lParmValues.Add(lNewSegId)
                            lParmValues.Add(lOccur)
                            lParmValues.Add("'" & lParm.Value & "'")
                            aParmDB.InsertRowIntoTable("ParmData", lParmValues)
                        End If
                    Next 'param
                End If
            Next 'hspftable
            If lMsgBuilder.Length > 0 Then
                'Logger.Msg(lMsgBuilder.ToString(), MsgBoxStyle.Information, "HSPFParm Values " & " (Operation: " & lOpn.Name & ", " & lOpn.Id & ")" & " Beyond Normal Range")
                Logger.Dbg(lMsgBuilder.ToString())
                Dim lfrmMsg As New frmBulletin(lMsgBuilder.ToString())
                lfrmMsg.Text = "HSPFParm Values " & " (Operation: " & lOpn.Name & ", " & lOpn.Id & ")" & " Beyond Normal Range"
                lfrmMsg.ClearSelection()
                'lfrmMsg.ShowDialog()
                lfrmMsg.ShowTimedDialog(6000)
            End If
            lMsgBuilder.Length = 0
        Next 'operation
    End Sub

    Public Function TableIDFromTableName(ByVal aParmDB As atcMDB, ByVal aTableName As String, ByVal aOpTypId As Integer) As Integer
        'myParmTableDefn = myDB.OpenRecordset("ParmTableDefn", dbOpenDynaset)
        'ID	Name	OpnTypID	Alias	TableNumber	Definition
        '1	ACTIVITY	1	FALSE	1	
        '222	GQ-VALUES	3	FALSE	55	
        Dim lTableId As Integer = 0
        Dim lCrit As String = " Name = '" & aTableName & "' AND " & " OpnTypID = " & aOpTypId
        Dim lStr As String = "SELECT DISTINCTROW ParmTableDefn.ID " & _
                                                "From ParmTableDefn " & _
                                                "WHERE (" & lCrit & ")"
        Dim lTable As DataTable = aParmDB.GetTable(lStr)
        If lTable.Rows.Count > 0 Then
            lTableId = lTable.Rows(0).Item(0).ToString
        End If
        Return lTableId
    End Function

    Public Function ParmIDFromParmName(ByVal aParmDB As atcMDB, ByVal aParmName As String, ByVal aTableId As Integer) As Integer
        'myParmDefn = myDB.OpenRecordset("ParmDefn", dbOpenDynaset)
        'ID	Name	Assoc	AssocID	ParmTypeID	ParmTableID	Min	Max	Def	StartCol	Width	Definition
        '1	AIRTFG	 	    0	    2	        1	0	1	0	11	5	
        '2	SNOWFG	 	    0	    2	        1	0	1	0	16	5	
        '13	AIRTPR	 	    0	    2	        2	2	6	4	11	5	
        '1478	BEDDEP	 	0	    3	        204	0	<none>	0	11	10	
        '1479	SANDFR	 	0	    3	        204	9.999999E-05	1	1	21	10	
        Dim lParmId As Integer = 0
        Dim lCrit As String = " Name = '" & aParmName & "' AND " & " ParmTableID = " & aTableId
        Dim lStr As String = "SELECT DISTINCTROW ParmDefn.ID " & _
                                                "From ParmDefn " & _
                                                "WHERE (" & lCrit & ")"
        Dim lTable As DataTable = aParmDB.GetTable(lStr)
        If lTable.Rows.Count > 0 Then
            lParmId = lTable.Rows(0).Item(0).ToString
        End If
        Return lParmId
    End Function

    Public Sub ParmMinMax(ByVal aParmDB As atcMDB, _
                   ByVal aParmId As Integer, ByVal aParmName As String, ByVal aParmTablename As String, _
                   ByRef aMin As Single, ByRef aMax As Single, _
                   ByRef aTMin As Single, ByRef aTMax As Single, _
                   ByRef aPMin As Single, ByRef aPMax As Single)
        'find min and max values for this parameter
        Dim lParmCrit As String = "ParmID = " & aParmId & " OR AssocID = " & aParmId
        Dim lStr As String = "SELECT DISTINCTROW ParmTableData.SegID, " & _
                                                "ParmTableData.OpnTypID, " & _
                                                "ParmTableData.Name, " & _
                                                "ParmTableData.ParmID, " & _
                                                "ParmTableData.Value, " & _
                                                "ParmTableData.Table, " & _
                                                "ParmTableData.Occur, " & _
                                                "ParmTabledata.AliasInfo " & _
                                                "From ParmTableData " & _
                                                "WHERE (" & lParmCrit & ")"
        Dim lTable As DataTable = aParmDB.GetTable(lStr)
        Dim lTmp As Single
        If lTable IsNot Nothing AndAlso lTable.Rows.Count > 0 Then
            aMin = lTable.Rows(0).Item(4)
            aMax = lTable.Rows(0).Item(4)
            For lRow As Integer = 1 To lTable.Rows.Count - 1
                If Single.TryParse(lTable.Rows(lRow).Item(4).ToString, lTmp) Then
                    If lTmp > aMax Then
                        aMax = lTmp
                    End If
                    If lTmp < aMin Then
                        aMin = lTmp
                    End If
                End If
            Next
        End If
        lTable.Clear()
        lTable = Nothing

        'find typical and possible range
        aTMin = -9999
        aTMax = -9999
        aPMin = -9999
        aPMax = -9999
        lStr = "SELECT Name, Unit, TMin, TMax, PMin, PMax, HSPFOPN, HSPFTable " & _
               "FROM ParmRange " & _
               "WHERE Name = '" & aParmName.ToUpper() & "'"
        lTable = aParmDB.GetTable(lStr)
        If lTable IsNot Nothing Then
            With lTable
                If .Rows.Count = 1 Then
                    SetParmRange(.Rows(0), aTMin, aTMax, aPMin, aPMax)
                ElseIf lTable.Rows.Count > 1 Then
                    Dim lTablenameInDB As String
                    For lRow As Integer = 0 To .Rows.Count - 1
                        lTablenameInDB = .Rows(lRow).Item("HSPFTable").ToString.Replace(" ", "").Trim()
                        If lTablenameInDB.ToUpper() = aParmTablename.Replace(" ", "").Trim().ToUpper() Then
                            SetParmRange(.Rows(lRow), aTMin, aTMax, aPMin, aPMax)
                            Exit For
                        End If
                    Next
                End If
            End With 'lTable
        End If
        lTable.Clear()
        lTable = Nothing
    End Sub

    Public Sub SetParmRange(ByVal aDataRow As DataRow, ByRef aTMin As Single, ByRef aTMax As Single, ByRef aPMin As Single, ByRef aPMax As Single)
        Dim lRangeTMin As String = aDataRow.Item("TMin").ToString.Replace("""", "").Trim()
        Dim lRangeTMax As String = aDataRow.Item("TMax").ToString.Replace("""", "").Trim()
        Dim lRangePMin As String = aDataRow.Item("PMin").ToString.Replace("""", "").Trim()
        Dim lRangePMax As String = aDataRow.Item("PMax").ToString.Replace("""", "").Trim()

        Dim lArr() As String
        If Not Single.TryParse(lRangeTMin, aTMin) Then
            If lRangeTMin.Contains(" ") Then
                lArr = System.Text.RegularExpressions.Regex.Split(lRangeTMin, "\s+")
                'If lArr.Length >= 2 AndAlso IsNumeric(lArr(0)) AndAlso IsNumeric(lArr(1)) Then
                '    aTMin = (Single.Parse(lArr(0)) + Single.Parse(lArr(1))) / 2.0
                'End If
                If lArr.Length >= 2 Then
                    If IsNumeric(lArr(0)) AndAlso IsNumeric(lArr(1)) Then
                        aTMin = Math.Min(Single.Parse(lArr(0)), Single.Parse(lArr(1)))
                    Else
                        If Not Single.TryParse(lArr(0), aTMin) Then Single.TryParse(lArr(1), aTMin)
                    End If
                End If
                ReDim lArr(0)
            End If
        End If

        If Not Single.TryParse(lRangeTMax, aTMax) Then
            If lRangeTMax.Contains(" ") Then
                lArr = System.Text.RegularExpressions.Regex.Split(lRangeTMax, "\s+")
                'If lArr.Length >= 2 AndAlso IsNumeric(lArr(0)) AndAlso IsNumeric(lArr(1)) Then
                '    aTMax = (Single.Parse(lArr(0)) + Single.Parse(lArr(1))) / 2.0
                'End If
                If lArr.Length >= 2 Then
                    If IsNumeric(lArr(0)) AndAlso IsNumeric(lArr(1)) Then
                        aTMax = Math.Max(Single.Parse(lArr(0)), Single.Parse(lArr(1)))
                    Else
                        If Not Single.TryParse(lArr(0), aTMax) Then Single.TryParse(lArr(1), aTMax)
                    End If
                End If
                ReDim lArr(0)
            End If
        End If

        If Not Single.TryParse(lRangePMin, aPMin) Then
            If lRangePMin.Contains(" ") Then
                lArr = System.Text.RegularExpressions.Regex.Split(lRangePMin, "\s+")
                'If lArr.Length >= 2 AndAlso IsNumeric(lArr(0)) AndAlso IsNumeric(lArr(1)) Then
                '    aPMin = (Single.Parse(lArr(0)) + Single.Parse(lArr(1))) / 2.0
                'End If
                If IsNumeric(lArr(0)) AndAlso IsNumeric(lArr(1)) Then
                    aPMin = Math.Min(Single.Parse(lArr(0)), Single.Parse(lArr(1)))
                Else
                    If Not Single.TryParse(lArr(0), aPMin) Then Single.TryParse(lArr(1), aPMin)
                End If
                ReDim lArr(0)
            End If
        End If

        If Not Single.TryParse(lRangePMax, aPMax) Then
            If lRangePMax.Contains(" ") Then
                lArr = System.Text.RegularExpressions.Regex.Split(lRangePMax, "\s+")
                'If lArr.Length >= 2 AndAlso IsNumeric(lArr(0)) AndAlso IsNumeric(lArr(1)) Then
                '    aPMax = (Single.Parse(lArr(0)) + Single.Parse(lArr(1))) / 2.0
                'End If
                If lArr.Length >= 2 Then
                    If IsNumeric(lArr(0)) AndAlso IsNumeric(lArr(1)) Then
                        aPMax = Math.Max(Single.Parse(lArr(0)), Single.Parse(lArr(1)))
                    Else
                        If Not Single.TryParse(lArr(0), aPMax) Then Single.TryParse(lArr(1), aPMax)
                    End If
                End If
                ReDim lArr(0)
            End If
        End If
    End Sub

    Private Function SetScenario(ByRef aScn As Scn) As HspfUci
        Dim lUci As New HspfUci()
        Dim pMsg As New HspfMsg
        pMsg.Open("hspfmsg.mdb")

        Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
        Logger.Status("SHOW")
        Logger.Status("Reading Scenario UCI: " & aScn.UCIFilename)
        lUci.FastReadUciForStarter(pMsg, aScn.UCIFilename)
        Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
        'Logger.Status("")

        aScn.ScnShortName = FilenameNoExt(FilenameNoPath(aScn.UCIFilename))
        aScn.ReqStartDate = DumpDate(Date2J(lUci.GlobalBlock.SDate(0), lUci.GlobalBlock.SDate(1), lUci.GlobalBlock.SDate(2), lUci.GlobalBlock.SDate(3), lUci.GlobalBlock.SDate(4)))
        aScn.ReqEndDate = DumpDate(Date2J(lUci.GlobalBlock.EDate(0), lUci.GlobalBlock.EDate(1), lUci.GlobalBlock.EDate(2), lUci.GlobalBlock.EDate(3), lUci.GlobalBlock.EDate(4)))
        aScn.ReqNumReaches = lUci.OpnBlks("RCHRES").Count
        aScn.ReqNumSegments = lUci.MetSegs.Count
        aScn.ReqUnits = lUci.GlobalBlock.EmFg
        Return lUci
    End Function

End Module
