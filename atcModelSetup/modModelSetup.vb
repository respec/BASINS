Imports atcUtility
Imports atcMwGisUtility
Imports atcData
Imports atcSegmentation
Imports MapWinUtility
Imports System.Text
Imports System.Collections.ObjectModel

Module modModelSetup

    Friend Sub StartWinHSPF(ByVal aCommand As String)
        Dim lWinHSPFexe As String = FindFile("Please locate WinHSPF.exe", "WinHSPF.exe")

        If IO.File.Exists(lWinHSPFexe) Then
            Logger.Dbg("StartWinHSPF:" & lWinHSPFexe & ":" & aCommand)
            Process.Start(lWinHSPFexe, aCommand)
            Logger.Dbg("WinHSPFStarted")
        Else
            Logger.Msg("Cannot find WinHSPF.exe", MsgBoxStyle.Critical, "BASINS HSPF Problem")
        End If
    End Sub

    Friend Sub StartAQUATOX(ByVal aCommand As String)
        Dim lAQUATOXexe As String = FindFile("Please locate AQUATOX.exe", "AQUATOX.exe")

        If IO.File.Exists(lAQUATOXexe) Then
            Logger.Dbg("StartAQUATOX:" & lAQUATOXexe & ":" & aCommand)
            Process.Start(lAQUATOXexe, aCommand)
            Logger.Dbg("AQUATOXStarted")
        Else
            Logger.Msg("Cannot find AQUATOX.exe", MsgBoxStyle.Critical, "BASINS AQUATOX Problem")
        End If
    End Sub

    Private Function IsBASINSMetWDM(ByVal aDataSets As atcData.atcTimeseriesGroup, _
                                    ByVal aBaseDsn As Integer, _
                                    ByVal aLoc As String) As Boolean
        Dim lCheckCount As Integer = 0
        For Each lDataSet As atcTimeseries In aDataSets
            If lDataSet.Attributes.GetValue("Location") = aLoc Then
                If lDataSet.Attributes.GetValue("ID") = aBaseDsn Then
                    If lDataSet.Attributes.GetValue("TSTYPE") = "PREC" Then
                        lCheckCount += 1
                    End If
                ElseIf lDataSet.Attributes.GetValue("ID") = aBaseDsn + 2 Then
                    If lDataSet.Attributes.GetValue("TSTYPE") = "ATEM" Then
                        lCheckCount += 1
                    End If
                ElseIf lDataSet.Attributes.GetValue("ID") = aBaseDsn + 3 Then
                    If lDataSet.Attributes.GetValue("TSTYPE") = "WIND" Then
                        lCheckCount += 1
                    End If
                ElseIf lDataSet.Attributes.GetValue("ID") = aBaseDsn + 4 Then
                    If lDataSet.Attributes.GetValue("TSTYPE") = "SOLR" Then
                        lCheckCount += 1
                    End If
                ElseIf lDataSet.Attributes.GetValue("ID") = aBaseDsn + 5 Then
                    If lDataSet.Attributes.GetValue("TSTYPE") = "PEVT" Then
                        lCheckCount += 1
                    End If
                ElseIf lDataSet.Attributes.GetValue("ID") = aBaseDsn + 6 Then
                    If lDataSet.Attributes.GetValue("TSTYPE") = "DEWP" Then
                        lCheckCount += 1
                    End If
                ElseIf lDataSet.Attributes.GetValue("ID") = aBaseDsn + 7 Then
                    If lDataSet.Attributes.GetValue("TSTYPE") = "CLOU" Then
                        lCheckCount += 1
                    End If
                End If
            End If
        Next

        If lCheckCount = 7 Then 'needed datasets found
            Return True
        Else
            Return False
        End If
    End Function

    Friend Sub BuildListofMetStationNames(ByRef aMetWDMName As String, _
                                          ByVal aMetStations As atcCollection, _
                                          ByVal aMetBaseDsns As atcCollection)
        aMetStations.Clear()
        aMetBaseDsns.Clear()
        Dim lDataSource As New atcWDM.atcDataSourceWDM
        If IO.File.Exists(aMetWDMName) Then
            Dim lFound As Boolean = False
            For Each lBASINSDataSource As atcTimeseriesSource In atcDataManager.DataSources
                If lBASINSDataSource.Specification.ToUpper = aMetWDMName.ToUpper Then
                    'found it in the BASINS data sources
                    lDataSource = lBASINSDataSource
                    lFound = True
                    Exit For
                End If
            Next

            If Not lFound Then 'need to open it here
                If lDataSource.Open(aMetWDMName) Then
                    lFound = True
                End If
            End If

            If lFound Then
                Dim lCounter As Integer = 0
                For Each lDataSet As atcTimeseries In lDataSource.DataSets
                    lCounter += 1
                    Logger.Progress(lCounter, lDataSource.DataSets.Count)

                    If (lDataSet.Attributes.GetValue("Scenario") = "OBSERVED" Or lDataSet.Attributes.GetValue("Scenario") = "COMPUTED") _
                        And lDataSet.Attributes.GetValue("Constituent") = "PREC" Then
                        Dim lLoc As String = lDataSet.Attributes.GetValue("Location")
                        Dim lStanam As String = lDataSet.Attributes.GetValue("Stanam")
                        Dim lDsn As Integer = lDataSet.Attributes.GetValue("Id")
                        'get the common dates from prec and pevt at this location
                        Dim lSJDay As Double = lDataSet.Dates.Value(0)
                        Dim lEJDay As Double = lDataSet.Dates.Value(lDataSet.Dates.numValues)
                        'find pevt dataset at the same location
                        Dim lAddIt As Boolean = False
                        For Each lDataSet2 As atcData.atcTimeseries In lDataSource.DataSets
                            If lDataSet2.Attributes.GetValue("Constituent") = "PEVT" And _
                               lDataSet2.Attributes.GetValue("Location") = lLoc Then
                                Dim lSJDay2 As Double = lDataSet2.Dates.Value(0)
                                Dim lEJDay2 As Double = lDataSet2.Dates.Value(lDataSet2.Dates.numValues)
                                If lSJDay2 > lSJDay Then lSJDay = lSJDay2
                                If lEJDay2 < lEJDay Then lEJDay = lEJDay2
                                lAddIt = True
                                Exit For
                            End If
                        Next
                        'if this one is computed and observed also exists at same location, just use observed
                        If lDataSet.Attributes.GetValue("Scenario") = "COMPUTED" Then
                            For Each lDataSet2 As atcData.atcTimeseries In lDataSource.DataSets
                                If lDataSet2.Attributes.GetValue("Constituent") = "PREC" And _
                                   lDataSet2.Attributes.GetValue("Scenario") = "OBSERVED" And _
                                   lDataSet2.Attributes.GetValue("Location") = lLoc Then
                                    lAddIt = False
                                    Exit For
                                End If
                            Next
                        End If
                        If lAddIt Then
                            Dim lLeadingChar As String = ""
                            If IsBASINSMetWDM(lDataSource.DataSets, lDsn, lLoc) Then
                                'full set available here
                                lLeadingChar = "*"
                            End If
                            Dim lSdate(6) As Integer
                            Dim lEdate(6) As Integer
                            J2Date(lSJDay, lSdate)
                            J2Date(lEJDay, lEdate)
                            Dim lDateString As String = "(" & lSdate(0) & "/" & lSdate(1) & "/" & lSdate(2) & "-" & lEdate(0) & "/" & lEdate(1) & "/" & lEdate(2) & ")"
                            aMetStations.Add(lLeadingChar & lLoc & ":" & lStanam & " " & lDateString)
                            aMetBaseDsns.Add(lDsn)
                        End If
                    End If
                    'set valuesneedtoberead so that the dates and values will be forgotten, to free up memory
                    lDataSet.ValuesNeedToBeRead = True
                Next
            End If
        End If
    End Sub

    Friend Function CreateUCI(ByVal aUciName As String, _
                              ByVal aMetWDMName As String) As Boolean
        ChDriveDir(PathNameOnly(aUciName))
        'get message file ready
        Dim lMsg As New atcUCI.HspfMsg("hspfmsg.mdb")

        'get starter uci ready
        Dim lBasinsBinLoc As String = PathNameOnly(System.Reflection.Assembly.GetEntryAssembly.Location)
        Dim lStarterUciName As String = "starter.uci"
        Dim lStarterPath As String = lBasinsBinLoc.Substring(0, lBasinsBinLoc.Length - 3) & "models\hspf\bin\starter\" & lStarterUciName
        If Not IO.File.Exists(lStarterPath) Then
            lStarterPath = "\basins\models\hspf\bin\starter\" & lStarterUciName
            If Not IO.File.Exists(lStarterPath) Then
                lStarterPath = FindFile("Please locate " & lStarterUciName, lStarterUciName)
            End If
        End If
        lStarterUciName = lStarterPath

        'location master pollutant list 
        Dim lPollutantListFileName As String = "poltnt_2.prn"
        Dim lPollutantListPath As String = lBasinsBinLoc.Substring(0, lBasinsBinLoc.Length - 3) & "models\hspf\bin\" & lPollutantListFileName
        If Not IO.File.Exists(lPollutantListPath) Then
            lPollutantListPath = "\basins\models\hspf\bin\" & lPollutantListFileName
            If Not FileExists(lPollutantListPath) Then
                lPollutantListPath = FindFile("Please locate " & lPollutantListFileName, lPollutantListFileName)
            End If
        End If
        lPollutantListFileName = lPollutantListPath

        'open project wdm
        Dim lDataSources As New Collection(Of atcData.atcTimeseriesSource)
        Dim lDataSource As New atcWDM.atcDataSourceWDM
        Dim lProjectWDMName As String = IO.Path.GetFileNameWithoutExtension(aUciName) & ".wdm"
        Dim lFound As Boolean = False
        For Each lBASINSDataSource As atcTimeseriesSource In atcDataManager.DataSources
            If lBASINSDataSource.Specification.ToUpper = lProjectWDMName.ToUpper Then
                'found it in the BASINS data sources
                lDataSource = lBASINSDataSource
                lFound = True
                Exit For
            End If
        Next

        If Not lFound Then 'need to open it here
            If lDataSource.Open(lProjectWDMName) Then
                lFound = True
            End If
        End If
        lDataSources.Add(lDataSource)

        'open met wdm
        lDataSource = New atcWDM.atcDataSourceWDM
        lFound = False
        For Each lBASINSDataSource As atcTimeseriesSource In atcDataManager.DataSources
            If lBASINSDataSource.Specification.ToUpper = aMetWDMName.ToUpper Then
                'found it in the BASINS data sources
                lDataSource = lBASINSDataSource
                lFound = True
                Exit For
            End If
        Next

        If Not lFound Then 'need to open it here
            If lDataSource.Open(aMetWDMName) Then
                lFound = True
            End If
        End If
        lDataSources.Add(lDataSource)

        ChDriveDir(PathNameOnly(aUciName))

        Dim lWatershedName As String = IO.Path.GetFileNameWithoutExtension(aUciName)
        Dim lWatershed As New Watershed
        Dim lCreateUCI As Boolean = False
        If lWatershed.Open(lWatershedName) = 0 Then  'everything read okay, continue
            Dim lHspfUci As New atcUCI.HspfUci
            lHspfUci.Msg = lMsg
            lHspfUci.CreateUciFromBASINS(lWatershed, _
                                         lDataSources, _
                                         lStarterUciName, lPollutantListFileName)
            lHspfUci.Save()
            lCreateUCI = True
        End If
        Return lCreateUCI
    End Function

    Friend Function ReclassifyLandUses(ByVal aReclassifyFile As String, _
                                       ByVal aGridPervious As atcControls.atcGrid, _
                                       ByVal aLandUses As LandUses) As LandUses
        Dim lReclassifyLandUses As New LandUses
        'if simple reclassifyfile exists, read it in
        Dim lRcode As New atcCollection
        Dim lUseSimpleGrid As Boolean = False
        If aReclassifyFile.Length > 0 And aGridPervious.ColumnWidth(0) = 0 Then
            'have the simple percent pervious grid, need to know which 
            'lucodes correspond to which lugroups
            lUseSimpleGrid = True
            'open dbf file
            Dim lTable As IatcTable = atcTableOpener.OpenAnyTable(aReclassifyFile)
            For lTableRecordIndex As Integer = 1 To lTable.NumRecords
                lTable.CurrentRecord = lTableRecordIndex
                lRcode.Add(lTable.Value(1), lTable.Value(2))
            Next lTableRecordIndex
        End If
        Logger.Dbg("ReclassifyFile:'" & aReclassifyFile & "' Count" & lRcode.Count)

        'create summary array 
        '  area of each land use group in each subbasin

        'build collection of unique subbasin ids
        Dim lUniqueSubids As New atcCollection
        For Each lLandUse As LandUse In aLandUses
            lUniqueSubids.Add(lLandUse.ModelID)
        Next
        Logger.Dbg("LandUseCount:" & aLandUses.Count & " UniqueSubidCount:" & lUniqueSubids.Count)

        'build collection of unique landuse groups
        Dim lUniqueLugroups As New atcCollection
        For lRowIndex As Integer = 1 To aGridPervious.Source.Rows
            lUniqueLugroups.Add(aGridPervious.Source.CellValue(lRowIndex, 1))
        Next lRowIndex
        Logger.Dbg("GridRowCount:" & aGridPervious.Source.Rows & " UniqueLugroupCount:" & lUniqueLugroups.Count)

        Dim lPerArea(lUniqueSubids.Count, lUniqueLugroups.Count) As Double
        Dim lImpArea(lUniqueSubids.Count, lUniqueLugroups.Count) As Double
        Dim lLength(lUniqueSubids.Count) As Double
        Dim lSlope(lUniqueSubids.Count) As Single

        'loop through each polygon (or grid subid/lucode combination)
        'and populate array with area values
        If lUseSimpleGrid Then
            For Each lLandUse As LandUse In aLandUses
                'find subbasin position in the area array
                Dim spos As Integer
                For j As Integer = 0 To lUniqueSubids.Count - 1
                    If lLandUse.ModelID = lUniqueSubids(j) Then
                        spos = j
                        Exit For
                    End If
                Next j
                'find lugroup that corresponds to this lucode
                Dim lLandUseName As String = lRcode.ItemByKey(lLandUse.Code.ToString)
                If lLandUseName IsNot Nothing Then
                    'find percent perv that corresponds to this lugroup
                    Dim lPercentImperv As Double
                    For lRowIndex As Integer = 1 To aGridPervious.Source.Rows
                        If lLandUseName = aGridPervious.Source.CellValue(lRowIndex, 1) Then
                            If Double.TryParse(aGridPervious.Source.CellValue(lRowIndex, 2), lPercentImperv) Then
                                Exit For
                            Else
                                Logger.Dbg("Warning: non-parsable percent impervious value at row " & lRowIndex & " '" & aGridPervious.Source.CellValue(lRowIndex, 2) & "' for land use name " & lLandUseName)
                            End If
                        End If
                    Next lRowIndex
                    'find lugroup position in the area array
                    Dim lpos As Long
                    For j As Integer = 0 To lUniqueLugroups.Count - 1
                        If lLandUseName = lUniqueLugroups(j) Then
                            lpos = j
                            Exit For
                        End If
                    Next j

                    With lLandUse
                        lPerArea(spos, lpos) += (.Area * (100 - lPercentImperv) / 100)
                        lImpArea(spos, lpos) += (.Area * lPercentImperv / 100)
                        lLength(spos) = 0.0
                        lSlope(spos) = .Slope / 100.0
                    End With
                End If
            Next lLandUse
        Else 'using custom table for landuse classification
            For Each lLandUse As LandUse In aLandUses
                'loop through each polygon (or grid subid/lucode combination)
                'find subbasin position in the area array
                Dim spos As Integer
                For j As Integer = 1 To lUniqueSubids.Count
                    If lLandUse.ModelID = lUniqueSubids(j - 1) Then
                        spos = j - 1
                        Exit For
                    End If
                Next j

                'find lugroup that corresponds to this lucode, could be multiple matches
                For lSourceRowIndex As Integer = 1 To aGridPervious.Source.Rows
                    Dim lLandUseName As String = ""
                    Dim lpos As Integer = -1
                    Dim lPercentImperv As Double
                    If aGridPervious.Source.CellValue(lSourceRowIndex, 0) <> "" Then
                        If lLandUse.Code = aGridPervious.Source.CellValue(lSourceRowIndex, 0) Then
                            'see if any of these are subbasin-specific
                            If Not Double.TryParse(aGridPervious.Source.CellValue(lSourceRowIndex, 2), lPercentImperv) Then
                                Logger.Dbg("Warning: non-parsable percent impervious value at row " & lSourceRowIndex & " '" & aGridPervious.Source.CellValue(lSourceRowIndex, 2) & "' for land use code " & lLandUse.Code)
                            Else
                                Dim lMultiplier As Double
                                If Not Double.TryParse(aGridPervious.Source.CellValue(lSourceRowIndex, 3), lMultiplier) Then
                                    lMultiplier = 1.0
                                End If
                                Dim lSubbasin As String = aGridPervious.Source.CellValue(lSourceRowIndex, 4)
                                Dim lSubbasinSpecific As Boolean = False
                                If Not lSubbasin Is Nothing Then
                                    If lSubbasin.Length > 0 And lSubbasin <> "Invalid Field Number" Then
                                        lSubbasinSpecific = True
                                    End If
                                End If
                                If lSubbasinSpecific Then
                                    'this row is subbasin-specific
                                    If lSubbasin = lLandUse.ModelID Then
                                        lLandUseName = aGridPervious.Source.CellValue(lSourceRowIndex, 1)
                                    End If
                                Else
                                    'make sure that no other rows of this lucode are 
                                    'subbasin-specific for this subbasin and that we 
                                    'should therefore not use this row
                                    Dim lUseIt As Boolean = True
                                    For k As Integer = 1 To aGridPervious.Source.Rows
                                        If k <> lSourceRowIndex Then
                                            If aGridPervious.Source.CellValue(k, 0) = aGridPervious.Source.CellValue(lSourceRowIndex, 0) Then
                                                'this other row has same lucode
                                                If aGridPervious.Source.CellValue(k, 1) = aGridPervious.Source.CellValue(lSourceRowIndex, 1) Then
                                                    'and the same group name
                                                    lSubbasin = aGridPervious.Source.CellValue(k, 4)
                                                    If lSubbasin IsNot Nothing AndAlso IsNumeric(lSubbasin) Then
                                                        'and its subbasin-specific
                                                        If lSubbasin = lLandUse.ModelID Then
                                                            'and its specific to this subbasin
                                                            lUseIt = False
                                                        End If
                                                    End If
                                                End If
                                            End If
                                        End If
                                    Next k
                                    If lUseIt Then 'we want this one now
                                        lLandUseName = aGridPervious.Source.CellValue(lSourceRowIndex, 1)
                                    End If
                                End If

                                If lLandUseName.Length > 0 Then 'find lugroup position in the area array
                                    For k As Integer = 1 To lUniqueLugroups.Count
                                        If lLandUseName = lUniqueLugroups(k - 1) Then
                                            lpos = k - 1
                                            Exit For
                                        End If
                                    Next k
                                End If

                                If lpos > -1 Then
                                    With lLandUse
                                        lPerArea(spos, lpos) += (.Area * lMultiplier * (100 - lPercentImperv) / 100)
                                        lImpArea(spos, lpos) += (.Area * lMultiplier * lPercentImperv / 100)
                                        lLength(spos) = 0.0 'were not computing lsur since winhspf does that
                                        lSlope(spos) = .Slope / 100.0
                                    End With
                                End If
                            End If
                        End If
                    End If
                Next lSourceRowIndex
            Next lLandUse
        End If

        For spos As Integer = 0 To lUniqueSubids.Count - 1
            For lpos As Integer = 0 To lUniqueLugroups.Count - 1
                Dim lArea As Double = lPerArea(spos, lpos) + lImpArea(spos, lpos)
                If lArea > 0 Then
                    Dim lLandUse As New LandUse
                    With lLandUse
                        .Description = lUniqueLugroups(lpos)
                        .Distance = lLength(spos)
                        .Slope = lSlope(spos)
                        .Area = lArea
                        .Code = lpos
                        .ImperviousFraction = lImpArea(spos, lpos) / .Area
                        .ModelID = lUniqueSubids(spos)
                        For Each lOrigLandUse As LandUse In aLandUses
                            If lOrigLandUse.ModelID = .ModelID Then
                                .Reach = lOrigLandUse.Reach
                                Exit For
                            End If
                        Next
                        .Type = "COMPOSITE"
                    End With
                    lReclassifyLandUses.Add(lLandUse)
                End If
            Next
        Next

        Return lReclassifyLandUses
    End Function

    Friend Sub WriteWSDFile(ByVal aWsdFileName As String, _
                            ByVal aLandUses As LandUses)

        Dim lSB As New StringBuilder
        lSB.AppendLine("""LU Name""" & "," & """Type (1=Impervious, 2=Pervious)""" & "," & """Watershd-ID""" & "," & _
                       """Area""" & "," & """Slope""" & "," & """Distance""")
        For Each lLandUse As LandUse In aLandUses
            Dim lType As String = "2"
            Dim lArea As Double = lLandUse.Area * (1 - lLandUse.ImperviousFraction) / 4046.8564
            If lArea > 0 Then 'or CInt(lArea)
                lSB.AppendLine(Chr(34) & lLandUse.Description & Chr(34) & "     " & _
                               lType & "     " & _
                               lLandUse.ModelID & "     " & _
                               Format(lArea, "0.0") & "     " & _
                               Format(lLandUse.Slope, "0.000000") & "     " & _
                               Format(lLandUse.Distance, "0.0000"))
            End If
            lType = "1"
            lArea = lLandUse.Area * lLandUse.ImperviousFraction / 4046.8564
            If lArea > 0 Then 'or CInt(lArea)
                lSB.AppendLine(Chr(34) & lLandUse.Description & Chr(34) & "     " & _
                               lType & "     " & _
                               lLandUse.ModelID & "     " & _
                               Format(lArea, "0.0") & "     " & _
                               Format(lLandUse.Slope, "0.000000") & "     " & _
                               Format(lLandUse.Distance, "0.0000"))
            End If
        Next lLandUse
        SaveFileString(aWsdFileName, lSB.ToString)
    End Sub

    Friend Sub WriteRCHFile(ByVal aRchFileName As String, _
                            ByVal aReaches As Reaches)

        Dim lSBRch As New StringBuilder

        lSBRch.AppendLine("""Rivrch""" & "," & """Pname""" & "," & """Watershed-ID""" & "," & """HeadwaterFlag""" & "," & _
                  """Exits""" & "," & """Milept""" & "," & """Stream/Resevoir Type""" & "," & """Segl""" & "," & _
                  """Delth""" & "," & """Elev""" & "," & """Ulcsm""" & "," & """Urcsm""" & "," & """Dscsm""" & "," & """Ccsm""" & "," & _
                  """Mnflow""" & "," & """Mnvelo""" & "," & """Svtnflow""" & "," & """Svtnvelo""" & "," & """Pslope""" & "," & _
                  """Pdepth""" & "," & """Pwidth""" & "," & """Pmile""" & "," & """Ptemp""" & "," & """Pph""" & "," & """Pk1""" & "," & _
                  """Pk2""" & "," & """Pk3""" & "," & """Pmann""" & "," & """Psod""" & "," & """Pbgdo""" & "," & _
                  """Pbgnh3""" & "," & """Pbgbod5""" & "," & """Pbgbod""" & "," & """Level""" & "," & """ModelSeg""")

        Dim lSlope As Double
        For Each lReach As Reach In aReaches
            With lReach
                lSlope = Math.Abs(.DeltH / (.Length * 5280))
                If lSlope < 0.00001 Then
                    lSlope = 0.001
                End If
                lSBRch.AppendLine(.Id & " " & Chr(34) & .Name & Chr(34) & " " & .Id & " " & _
                       " 0 1 0 S " & Format(.Length, "0.00") & " " & Format(Math.Abs(.DeltH), "0.00") & " " & _
                       Format(.Elev, "0.") & " 0 0 " & .DownID & " 0 0 0 0 0 " & _
                       Format(lSlope, "0.000000") & " " & Format(.Depth, "0.0000") & " " & Format(.Width, "0.000") & _
                       " 0 0 0 0 0 0 0 0 0 0 0 0 0 " & .SegmentId)
                If (2 * .Depth) > .Width Then 'problem
                    Logger.Msg("The depth and width values specified for Reach " & lReach.Id & ", coupled with the trapezoidal" & vbCrLf & _
                           "cross section assumptions of WinHSPF, indicate a physical imposibility." & vbCrLf & _
                           "(Given 1:1 side slopes, the depth of the channel cannot be more than half the width.)" & vbCrLf & vbCrLf & _
                           "This problem can be corrected in WinHSPF by revising the FTABLE or by " & vbCrLf & _
                           "importing the ptf with modifications to the width and depth values." & vbCrLf & _
                           "See the WinHSPF manual for more information.", vbOKOnly, "Channel Problem")
                End If
            End With
        Next

        SaveFileString(aRchFileName, lSBRch.ToString)
    End Sub

    Friend Sub WritePTFFile(ByVal aPtfFileName As String, _
                            ByVal aChannels As Channels)
        Dim lSBPtf As New StringBuilder

        lSBPtf.AppendLine("""Reach Number""" & "," & """Length(ft)""" & "," & _
            """Mean Depth(ft)""" & "," & """Mean Width (ft)""" & "," & _
            """Mannings Roughness Coeff.""" & "," & """Long. Slope""" & "," & _
            """Type of x-section""" & "," & """Side slope of upper FP left""" & "," & _
            """Side slope of lower FP left""" & "," & """Zero slope FP width left(ft)""" & "," & _
            """Side slope of channel left""" & "," & """Side slope of channel right""" & "," & _
            """Zero slope FP width right(ft)""" & "," & """Side slope lower FP right""" & "," & _
            """Side slope upper FP right""" & "," & """Channel Depth(ft)""" & "," & _
            """Flood side slope change at depth""" & "," & """Max. depth""" & "," & _
            """No. of exits""" & "," & """Fraction of flow through exit 1""" & "," & _
            """Fraction of flow through exit 2""" & "," & """Fraction of flow through exit 3""" & "," & _
            """Fraction of flow through exit 4""" & "," & """Fraction of flow through exit 5""")

        For Each lChannel As Channel In aChannels
            With lChannel
                lSBPtf.AppendLine(.Reach.Id & " " & Format(.Length, "0.") & " " & _
                       Format(.DepthMean, "0.00000") & " " & Format(.WidthMean, "0.00000") & " " & _
                       Format(.ManningN, "0.00") & " " & Format(.SlopeProfile, "0.00000") & " " & "Trapezoidal" & " " & _
                       Format(.SlopeSideUpperFPLeft, "0.0") & " " & Format(.SlopeSideLowerFPLeft, "0.0") & " " & _
                       Format(.WidthZeroSlopeLeft, "0.000") & " " & .SlopeSideLeft & " " & .SlopeSideRight & " " & _
                       Format(.WidthZeroSlopeRight, "0.000") & " " & _
                       Format(.SlopeSideLowerFPRight, "0.0") & " " & Format(.SlopeSideUpperFPRight, "0.0") & " " & _
                       Format(.DepthChannel, "0.0000") & " " & Format(.DepthSlopeChange, "0.0000") & " " & _
                       Format(.DepthMax, "0.000") & " 1 1 0 0 0 0")
            End With
        Next

        SaveFileString(aPtfFileName, lSBPtf.ToString)
    End Sub

    Friend Sub WritePSRFile(ByVal aPsrFileName As String, ByVal aUniqueSubids As atcCollection, ByVal aOutSubs As Collection, _
                            ByVal aLayerIndex As Integer, ByVal aPointIndex As Integer, ByVal aChkCustom As Boolean, _
                            ByVal aLblCustom As String, ByVal aChkCalculate As Boolean, ByVal aYear As String)
        Dim lSB As New StringBuilder

        Dim lNPDESSites As New Collection
        Dim lSubbasins As New Collection
        Dim lFlows As New Collection
        Dim lMipts As New Collection
        Dim lFacNames As New Collection
        Dim lHucs As New Collection

        Dim lDbName As String = ""
        Dim lRowCount As Long = 0
        Dim lPcsLayerIndex As Integer
        If aOutSubs.Count > 0 Then 'build collection of npdes sites to output
            For i As Integer = 1 To aOutSubs.Count
                For j As Integer = 0 To aUniqueSubids.Count - 1
                    If aOutSubs(i) = aUniqueSubids(j) Then 'found this subbasin in selected list
                        If GisUtil.FieldValue(aLayerIndex, i - 1, aPointIndex).Length > 0 Then
                            lNPDESSites.Add(GisUtil.FieldValue(aLayerIndex, i - 1, aPointIndex))
                            lSubbasins.Add(aOutSubs(i))
                        End If
                    End If
                Next j
            Next i

            'If cNPDES.Count = 0 Then
            '  MsgBox("No point sources have been added to the outlets layer." & vbCrLf & _
            '  "To add point sources, update the outlets layer using the" & vbCrLf & _
            '  "BASINS watershed delineator or update it manually.", vbOKOnly, "BASINS HSPF Information")
            'End If

            If Not aChkCustom Then 'use pcs data
                If GisUtil.IsLayer("Permit Compliance System") Then
                    'set pcs shape file
                    lPcsLayerIndex = GisUtil.LayerIndex("Permit Compliance System")
                    Dim lNpdesFieldIndex As Integer = GisUtil.FieldIndex(lPcsLayerIndex, "NPDES")
                    Dim lFlowFieldIndex As Integer = GisUtil.FieldIndex(lPcsLayerIndex, "FLOW_RATE")
                    Dim lFacFieldIndex As Integer = GisUtil.FieldIndex(lPcsLayerIndex, "FAC_NAME")
                    Dim lCuFieldIndex As Integer = GisUtil.FieldIndex(lPcsLayerIndex, "BCU")
                    If lNpdesFieldIndex > -1 Then
                        For lNpdesIndex As Integer = 1 To lNPDESSites.Count
                            Dim lFlow As Double = 0.0
                            Dim lFacName As String = ""
                            Dim lHuc As String = ""
                            Dim lMipt As Single = 0.0#
                            If lNPDESSites(lNpdesIndex).ToString.Trim.Length > 0 Then
                                For j As Integer = 1 To GisUtil.NumFeatures(lPcsLayerIndex)
                                    If GisUtil.FieldValue(lPcsLayerIndex, j - 1, lNpdesFieldIndex) = lNPDESSites(lNpdesIndex) Then
                                        'this is the one
                                        If IsNumeric(GisUtil.FieldValue(lPcsLayerIndex, j - 1, lFlowFieldIndex)) Then
                                            lFlow = GisUtil.FieldValue(lPcsLayerIndex, j - 1, lFlowFieldIndex) * 1.55
                                        Else
                                            lFlow = 0.0
                                        End If
                                        lFacName = GisUtil.FieldValue(lPcsLayerIndex, j - 1, lFacFieldIndex)
                                        If aChkCalculate Then
                                            'calculate mile point on stream
                                            'dist = myGISTools.NearestPositionOnLineToPoint(StreamsThemeName, StreamsField, cSubbasin(i), IO.Path.GetFileNameWithoutExtension(OutletsJoinThemeName), PCSIdField, pNPDES(j))
                                            'mipt = dist / 1609.3
                                        Else
                                            lMipt = 0.0#
                                        End If
                                        lHuc = GisUtil.FieldValue(lPcsLayerIndex, j - 1, lCuFieldIndex)
                                        Exit For
                                    End If
                                Next j
                            End If
                            lFlows.Add(lFlow)
                            lMipts.Add(lMipt)
                            lFacNames.Add(lFacName)
                            lHucs.Add(lHuc)
                        Next lNpdesIndex
                    End If
                    'check for dbf associated with each npdes point
                    Dim i As Integer = 1
                    lDbName = PathNameOnly(GisUtil.LayerFileName(lPcsLayerIndex)) & "\pcs\"
                    For Each lNpdesSite As Object In lNPDESSites
                        Dim lDbfFileName As String = lHucs(i).ToString.Trim & ".dbf"
                        If IO.File.Exists(lDbName & lDbfFileName) And lNpdesSite.ToString.Trim.Length > 0 Then
                            'yes, it exists
                            i += 1
                        Else 'remove from collection
                            lNPDESSites.Remove(i)
                            lSubbasins.Remove(i)
                            lFlows.Remove(i)
                            lMipts.Remove(i)
                            lFacNames.Remove(i)
                            lHucs.Remove(i)
                        End If
                    Next lNpdesSite
                Else
                    'no pcs layer, clear out
                    Do While lNPDESSites.Count > 0
                        lNPDESSites.Remove(1)
                    Loop
                End If
            Else
                'using custom table
                'must have these fields in this order:
                'pcsid (same as outlets layer)
                'facname
                'load (flow or other value) lbs/yr or cfs
                'parm (flow or other name)
                Dim lDbf As IatcTable = atcUtility.atcTableOpener.OpenAnyTable(aLblCustom)

                Dim i As Integer = 1
                Do While i <= lNPDESSites.Count
                    Dim lMipt As Single = 0.0#
                    If aChkCalculate Then
                        'calculate mile point on stream
                        'dist = myGISTools.NearestPositionOnLineToPoint(StreamsThemeName, StreamsField, cSubbasin(i), IO.Path.GetFileNameWithoutExtension(OutletsJoinThemeName), PCSIdField, pNPDES(j))
                        'mipt = dist / 1609.3
                    Else
                        lMipt = 0.0#
                    End If
                    lMipts.Add(lMipt)
                    Dim lFound As Boolean = False
                    For j As Integer = 1 To lDbf.NumRecords
                        lDbf.CurrentRecord = j
                        If lNPDESSites(i) = lDbf.Value(1) Then
                            lFacNames.Add(lDbf.Value(2))
                            lFound = True
                            Exit For
                        End If
                    Next j
                    If Not lFound Then
                        lNPDESSites.Remove(i)
                        lSubbasins.Remove(i)
                        lMipts.Remove(i)
                    Else
                        i += 1
                    End If
                Loop
            End If
        End If

        'write first part of point source file
        lSB.AppendLine(" " & lNPDESSites.Count.ToString)
        lSB.AppendLine(" ")
        lSB.AppendLine("FacilityName Npdes Cuseg Mi")
        Dim lStr As String
        For lNpdesSiteIndex As Integer = 1 To lNPDESSites.Count
            lStr = Chr(34) & lFacNames(lNpdesSiteIndex) & Chr(34) & " " & lNPDESSites(lNpdesSiteIndex) & " " & lSubbasins(lNpdesSiteIndex) & " " & Format(lMipts(lNpdesSiteIndex), "0.000000")
            lSB.AppendLine(lStr)
        Next lNpdesSiteIndex

        Dim lParmCode(0) As String, lParmName(0) As String
        If Not aChkCustom Then 'read in Permitted Discharges Parameter Table
            If lNPDESSites.Count > 0 Then 'open dbf file
                Dim lDbf As IatcTable = atcUtility.atcTableOpener.OpenAnyTable(PathNameOnly(GisUtil.LayerFileName(lPcsLayerIndex)) & "\pcs3_prm.dbf")
                lRowCount = lDbf.NumRecords
                ReDim lParmCode(lRowCount)
                ReDim lParmName(lRowCount)
                For i As Integer = 1 To lRowCount
                    lDbf.CurrentRecord = i
                    lParmCode(i) = lDbf.Value(1)
                    lParmName(i) = lDbf.Value(2)
                Next i
            End If
        End If

        lSB.AppendLine(" ")
        lSB.AppendLine("OrdinalNumber Pollutant Load(lbs/hr)")
        Dim lValue As String
        If Not aChkCustom Then  'using pcs data
            Dim lPrevDbf As String = ""
            Dim lTableYear(0) As String
            Dim lTableParm(0) As String
            Dim lTableLoad(0) As String
            Dim lTableNPDES(0) As String
            For lNpdesSiteIndex As Integer = 1 To lNPDESSites.Count
                'open dbf file
                Dim lDbfFileName As String = lDbName & lHucs(lNpdesSiteIndex).ToString.Trim & ".dbf"
                Dim lDbfRowCount As Long
                If IO.File.Exists(lDbfFileName) Then
                    If lDbfFileName <> lPrevDbf Then
                        Dim lDbf As IatcTable = atcUtility.atcTableOpener.OpenAnyTable(lDbfFileName)
                        lPrevDbf = lDbfFileName
                        Dim lYearField As Integer, lParmField As Integer
                        Dim lLoadField As Integer, lNPDESField As Integer
                        For lFieldIndex As Integer = 1 To lDbf.NumFields
                            If lDbf.FieldName(lFieldIndex).ToUpper = "YEAR" Then
                                lYearField = lFieldIndex
                            End If
                            If lDbf.FieldName(lFieldIndex).ToUpper = "PARM" Then
                                lParmField = lFieldIndex
                            End If
                            If lDbf.FieldName(lFieldIndex).ToUpper = "LOAD" Then
                                lLoadField = lFieldIndex
                            End If
                            If lDbf.FieldName(lFieldIndex).ToUpper = "NPDES" Then
                                lNPDESField = lFieldIndex
                            End If
                        Next lFieldIndex
                        lDbfRowCount = lDbf.NumRecords
                        ReDim lTableYear(lDbfRowCount)
                        ReDim lTableParm(lDbfRowCount)
                        ReDim lTableLoad(lDbfRowCount)
                        ReDim lTableNPDES(lDbfRowCount)
                        For lRowIndex As Integer = 1 To lDbfRowCount
                            lDbf.CurrentRecord = lRowIndex
                            lTableYear(lRowIndex) = lDbf.Value(lYearField)
                            lTableParm(lRowIndex) = lDbf.Value(lParmField)
                            lTableLoad(lRowIndex) = lDbf.Value(lLoadField)
                            lTableNPDES(lRowIndex) = lDbf.Value(lNPDESField)
                        Next lRowIndex
                    End If
                    For lDbfRowIndex As Integer = 1 To lDbfRowCount
                        If lTableNPDES(lDbfRowIndex) = lNPDESSites(lNpdesSiteIndex) And lTableYear(lDbfRowIndex) = aYear Then
                            'found one, output it
                            Dim lPollutantName As String = ""
                            For lRowIndex As Integer = 0 To lRowCount - 1
                                If lTableParm(lDbfRowIndex) = lParmCode(lRowIndex) Then
                                    lPollutantName = lParmName(lRowIndex)
                                    Exit For
                                End If
                            Next lRowIndex
                            lValue = lTableLoad(lDbfRowIndex) / 8760 'lbs/hr
                            lStr = CStr(lNpdesSiteIndex - 1) & " " & Chr(34) & Trim(lPollutantName) & Chr(34) & " " & Format(CSng(lValue), "0.000000")
                            lSB.AppendLine(lStr)
                        End If
                    Next lDbfRowIndex
                End If
            Next lNpdesSiteIndex
            'now output flows
            For lNpdesSiteIndex As Integer = 1 To lNPDESSites.Count
                lStr = CStr(lNpdesSiteIndex - 1) & " Flow " & Format(lFlows(lNpdesSiteIndex), "0.000000")
                lSB.AppendLine(lStr)
            Next lNpdesSiteIndex
        Else 'using custom data
            Dim lDbf As IatcTable = atcUtility.atcTableOpener.OpenAnyTable(aLblCustom)
            For i As Integer = 1 To lNPDESSites.Count
                For j As Integer = 1 To lDbf.NumRecords
                    lDbf.CurrentRecord = j
                    If lNPDESSites(i) = lDbf.Value(1) Then
                        If lDbf.Value(4).ToUpper = "FLOW" Then
                            lStr = CStr(i - 1) & " Flow " & Format(CStr(lDbf.Value(3)), "0.000000")
                        Else
                            lValue = CSng(lDbf.Value(3)) / 8760 'lbs/hr
                            lStr = CStr(i - 1) & " " & Chr(34) & Trim(lDbf.Value(4)) & Chr(34) & " " & Format(CStr(lValue), "0.000000")
                        End If
                        lSB.AppendLine(lStr)
                    End If
                Next j
            Next i
        End If

        SaveFileString(aPsrFileName, lSB.ToString)
    End Sub

    Friend Sub WriteSEGFile(ByVal aSegFileName As String, _
                            ByVal aMetSegIds As atcCollection, _
                            ByVal aMetIndices As atcCollection, _
                            ByVal aMetBaseDsns As atcCollection)
        Dim lSB As New StringBuilder
        lSB.AppendLine("SegID" & vbTab & _
                       "PrecWdmId PrecDsn PrecTstype PrecMFactPI PrecMFactR " & _
                       "AtemWdmId AtemDsn AtemTstype AtemMFactPI AtemMFactR " & _
                       "DewpWdmId DewpDsn DewpTstype DewpMFactPI DewpMFactR " & _
                       "WindWdmId WindDsn WindTstype WindMFactPI WindMFactR " & _
                       "SolrWdmId SolrDsn SolrTstype SolrMFactPI SolrMFactR " & _
                       "ClouWdmId ClouDsn ClouTstype ClouMFactPI ClouMFactR " & _
                       "PevtWdmId PevtDsn PevtTstype PevtMFactPI PevtMFactR")
        For lIndex As Integer = 0 To aMetIndices.Count - 1
            Dim lBaseDsn As Integer = aMetBaseDsns(aMetIndices(lIndex))
            lSB.AppendLine(CStr(aMetSegIds(lIndex)) & " WDM2 " & lBaseDsn.ToString & " PREC 1 1" & _
                                                      " WDM2 " & (lBaseDsn + 2).ToString & " ATEM 1 1" & _
                                                      " WDM2 " & (lBaseDsn + 6).ToString & " DEWP 1 1" & _
                                                      " WDM2 " & (lBaseDsn + 3).ToString & " WIND 1 1" & _
                                                      " WDM2 " & (lBaseDsn + 4).ToString & " SOLR 1 1" & _
                                                      " WDM2 " & (lBaseDsn + 7).ToString & " CLOU 0 1" & _
                                                      " WDM2 " & (lBaseDsn + 5).ToString & " PEVT 1 1")
        Next
        SaveFileString(aSegFileName, lSB.ToString)
    End Sub

    Friend Sub WriteMAPFile(ByVal aMapFileName As String)
        Dim lSB As New StringBuilder
        lSB.AppendLine("EXT " & GisUtil.MapExtentXmin & _
                          " " & GisUtil.MapExtentYmax & _
                          " " & GisUtil.MapExtentXmax & _
                          " " & GisUtil.MapExtentYmin)
        For lLayerIndex As Integer = 0 To GisUtil.NumLayers - 1
            If GisUtil.LayerType(lLayerIndex) = 1 Or _
               GisUtil.LayerType(lLayerIndex) = 2 Or _
               GisUtil.LayerType(lLayerIndex) = 3 Then
                'shapefile
                Dim lTemp As String = "LYR '" + GisUtil.LayerFileName(lLayerIndex) & "', " & GisUtil.LayerColor(lLayerIndex)
                If GisUtil.LayerType(lLayerIndex) = 3 Then
                    'polygon 
                    If Not GisUtil.LayerTransparent(lLayerIndex) Then
                        lTemp &= ",Style Transparent "
                    End If
                    lTemp &= ",Outline " & GisUtil.LayerOutlineColor(lLayerIndex)
                End If
                'hide the layers not turned on
                If Not GisUtil.LayerVisible(lLayerIndex) Then
                    lTemp &= ",Hide"
                End If
                'add theme name as caption
                lTemp &= ",Name '" & GisUtil.LayerName(lLayerIndex) & "'"
                lSB.AppendLine(lTemp)
            End If
        Next lLayerIndex
        SaveFileString(aMapFileName, lSB.ToString)
    End Sub
End Module
