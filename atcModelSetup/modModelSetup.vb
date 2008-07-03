Imports atcUtility
Imports atcMwGisUtility
Imports atcData
Imports atcSegmentation
Imports MapWinUtility
Imports System.Text
Imports System.Collections.ObjectModel

Module modModelSetup

    Friend Sub StartWinHSPF(ByVal aCommand As String)
        Dim lWinHSPFexe As String

        'todo:  get this from the registry
        lWinHSPFexe = "c:\basins\models\hspf\bin\winhspf.exe"
        If Not FileExists(lWinHSPFexe) Then
            lWinHSPFexe = "d:\basins\models\hspf\bin\winhspf.exe"
        End If
        If Not FileExists(lWinHSPFexe) Then
            lWinHSPFexe = "e:\basins\models\hspf\bin\winhspf.exe"
        End If
        If Not FileExists(lWinHSPFexe) Then
            Dim lBasinsBinLoc As String = PathNameOnly(System.Reflection.Assembly.GetEntryAssembly.Location)
            lWinHSPFexe = Mid(lBasinsBinLoc, 1, Len(lBasinsBinLoc) - 3) & "models\hspf\bin\winhspf.exe"
        End If
        If Not FileExists(lWinHSPFexe) Then
            'if we cant find it in any of the common places, use findfile
            lWinHSPFexe = FindFile("Please locate WinHSPF.exe", "WinHSPF.exe")
        End If

        If FileExists(lWinHSPFexe) Then
            Logger.Dbg("StartWinHSPF:" & lWinHSPFexe & ":" & aCommand)
            Process.Start(lWinHSPFexe, aCommand)
            Logger.Dbg("WinHSPFStarted")
        Else
            Logger.Msg("Cannot find WinHSPF.exe", MsgBoxStyle.Critical, "BASINS HSPF Problem")
        End If
    End Sub

    Friend Sub StartAQUATOX(ByVal aCommand As String)
        Dim lAQUATOXexe As String

        'todo:  get this from the registry
        'AQUATOXexe = reg.RegGetString(HKEY_LOCAL_MACHINE, "SOFTWARE\Eco Modeling\AQUATOX\ExePath", "") & "\AQUATOX.exe"
        lAQUATOXexe = "\Program Files\AQUATOX_R3\Program\AQUATOX.EXE"
        If Not FileExists(lAQUATOXexe) Then
            Dim lBasinsBinLoc As String = PathNameOnly(System.Reflection.Assembly.GetEntryAssembly.Location)
            lAQUATOXexe = Left(lBasinsBinLoc, 2) & lAQUATOXexe
        End If
        If Not FileExists(lAQUATOXexe) Then
            'if we cant find it in any of the common places, use findfile
            lAQUATOXexe = FindFile("Please locate AQUATOX.exe", "AQUATOX.exe")
        End If

        If FileExists(lAQUATOXexe) Then
            Logger.Dbg("StartAQUATOX:" & lAQUATOXexe & ":" & aCommand)
            Process.Start(lAQUATOXexe, aCommand)
            Logger.Dbg("AQUATOXStarted")
        Else
            Logger.Msg("Cannot find AQUATOX.exe", MsgBoxStyle.Critical, "BASINS AQUATOX Problem")
        End If
    End Sub

    Private Function IsBASINSMetWDM(ByVal aDataSets As atcData.atcDataGroup, _
                                    ByVal aBaseDsn As Integer, _
                                    ByVal aLoc As String) As Boolean
        Dim lCheckCount As Integer = 0
        For Each lDataSet As atcData.atcTimeseries In aDataSets
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
        If FileExists(aMetWDMName) Then
            Dim lFound As Boolean = False
            For Each lBASINSDataSource As atcDataSource In atcDataManager.DataSources
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
                For Each lDataSet As atcData.atcTimeseries In lDataSource.DataSets

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
                Next
            End If
        End If
    End Sub

    Friend Function CreateUCI(ByVal aUciName As String, _
                              ByVal aMetWDMName As String) As Boolean
        ChDriveDir(PathNameOnly(aUciName))
        'get message file ready
        Dim lMsg As New atcUCI.HspfMsg
        lMsg.Open("hspfmsg.mdb")

        'get starter uci ready
        Dim lBasinsBinLoc As String = PathNameOnly(System.Reflection.Assembly.GetEntryAssembly.Location)
        Dim lStarterUciName As String = "starter.uci"
        Dim lStarterPath As String = lBasinsBinLoc.Substring(0, lBasinsBinLoc.Length - 3) & "models\hspf\bin\starter\" & lStarterUciName
        If Not FileExists(lStarterPath) Then
            lStarterPath = "\basins\models\hspf\bin\starter\" & lStarterUciName
            If Not FileExists(lStarterPath) Then
                lStarterPath = FindFile("Please locate " & lStarterUciName, lStarterUciName)
            End If
        End If
        lStarterUciName = lStarterPath

        'location master pollutant list 
        Dim lPollutantListFileName As String = "poltnt_2.prn"
        Dim lPollutantListPath As String = lBasinsBinLoc.Substring(0, lBasinsBinLoc.Length - 3) & "models\hspf\bin\" & lPollutantListFileName
        If Not FileExists(lPollutantListPath) Then
            lPollutantListPath = "\basins\models\hspf\bin\" & lPollutantListFileName
            If Not FileExists(lPollutantListPath) Then
                lPollutantListPath = FindFile("Please locate " & lPollutantListFileName, lPollutantListFileName)
            End If
        End If
        lPollutantListFileName = lPollutantListPath

        'open project wdm
        Dim lDataSources As New Collection(Of atcData.atcDataSource)
        Dim lDataSource As New atcWDM.atcDataSourceWDM
        Dim lProjectWDMName As String = IO.Path.GetFileNameWithoutExtension(aUciName) & ".wdm"
        Dim lFound As Boolean = False
        For Each lBASINSDataSource As atcDataSource In atcDataManager.DataSources
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
        For Each lBASINSDataSource As atcDataSource In atcDataManager.DataSources
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
                                       ByVal aGridPervious As Object, _
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
        For i As Integer = 1 To aGridPervious.Source.Rows
            lUniqueLugroups.Add(aGridPervious.Source.CellValue(i, 1))
        Next i
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
                'find percent perv that corresponds to this lugroup
                Dim lPercentImperv As Double
                For j As Integer = 1 To aGridPervious.Source.Rows
                    If lLandUseName = aGridPervious.Source.CellValue(j, 1) Then
                        lPercentImperv = aGridPervious.Source.CellValue(j, 2)
                        Exit For
                    End If
                Next j
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
                For j As Integer = 1 To aGridPervious.Source.Rows
                    Dim lLandUseName As String = ""
                    Dim lpos As Integer = -1
                    Dim lPercentImperv As Double
                    If aGridPervious.Source.CellValue(j, 0) <> "" Then
                        If lLandUse.Code = aGridPervious.Source.CellValue(j, 0) Then
                            'see if any of these are subbasin-specific
                            lPercentImperv = aGridPervious.Source.CellValue(j, 2)
                            Dim lMultiplier As Double
                            If IsNumeric(aGridPervious.Source.CellValue(j, 3)) Then
                                lMultiplier = CSng(aGridPervious.Source.CellValue(j, 3))
                            Else
                                lMultiplier = 1.0
                            End If
                            Dim lSubbasin As String = aGridPervious.Source.CellValue(j, 4)
                            Dim lSubbasinSpecific As Boolean = False
                            If Not lSubbasin Is Nothing Then
                                If lSubbasin.Length > 0 And lSubbasin <> "Invalid Field Number" Then
                                    lSubbasinSpecific = True
                                End If
                            End If
                            If lSubbasinSpecific Then
                                'this row is subbasin-specific
                                If lSubbasin = lLandUse.ModelID Then
                                    lLandUseName = aGridPervious.Source.CellValue(j, 1)
                                End If
                            Else
                                'make sure that no other rows of this lucode are 
                                'subbasin-specific for this subbasin and that we 
                                'should therefore not use this row
                                Dim lUseIt As Boolean = True
                                For k As Integer = 1 To aGridPervious.Source.Rows
                                    If k <> j Then
                                        If aGridPervious.Source.CellValue(k, 0) = aGridPervious.Source.CellValue(j, 0) Then
                                            'this other row has same lucode
                                            If aGridPervious.Source.CellValue(k, 1) = aGridPervious.Source.CellValue(j, 1) Then
                                                'and the same group name
                                                lSubbasin = aGridPervious.Source.CellValue(k, 4)
                                                If Not lSubbasin Is Nothing Then
                                                    If lSubbasin.Length > 0 Then
                                                        'and its subbasin-specific
                                                        If lSubbasin = lLandUse.ModelID Then
                                                            'and its specific to this subbasin
                                                            lUseIt = False
                                                        End If
                                                    End If
                                                End If
                                            End If
                                        End If
                                    End If
                                Next k
                                If lUseIt Then 'we want this one now
                                    lLandUseName = aGridPervious.Source.CellValue(j, 1)
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
                Next j
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

        Dim OutFile As Integer
        Dim i As Integer, j As Long, k As Long
        Dim pcsLayerIndex As Long
        Dim npdesIndex As Long, flowIndex As Long, cuIndex As Long, facIndex As Long
        Dim flow As Single
        Dim facname As String
        Dim huc As String
        Dim mipt As Single
        Dim dbffilename As String
        Dim dbname As String = ""
        Dim lnpdes As Object
        Dim ctemp As String
        Dim tmpDbf As IatcTable
        Dim ParmCode(0) As String, ParmName(0) As String
        Dim RowCount As Long = 0
        Dim prevdbf As String
        Dim YearField As Long, ParmField As Long
        Dim LoadField As Long, NPDESField As Long
        Dim dbrcount As Long
        Dim TableYear(0) As String
        Dim TableParm(0) As String
        Dim TableLoad(0) As String
        Dim TableNPDES(0) As String
        Dim tPoll As String
        Dim tValue As String
        Dim iFound As Boolean

        OutFile = FreeFile()
        FileOpen(OutFile, aPsrFileName, OpenMode.Output)

        Dim cNPDES As New Collection
        Dim cSubbasin As New Collection
        Dim cFlow As New Collection
        Dim cMipt As New Collection
        Dim cFacName As New Collection
        Dim cHuc As New Collection

        If aOutSubs.Count > 0 Then

            'build collection of npdes sites to output
            For i = 1 To aOutSubs.Count
                For j = 0 To aUniqueSubids.Count - 1
                    If aOutSubs(i) = aUniqueSubids(j) Then
                        'found this subbasin in selected list
                        If Len(GisUtil.FieldValue(aLayerIndex, i - 1, aPointIndex)) > 0 Then
                            cNPDES.Add(GisUtil.FieldValue(aLayerIndex, i - 1, aPointIndex))
                            cSubbasin.Add(aOutSubs(i))
                        End If
                    End If
                Next j
            Next i

            'If cNPDES.Count = 0 Then
            '  MsgBox("No point sources have been added to the outlets layer." & vbCrLf & _
            '  "To add point sources, update the outlets layer using the" & vbCrLf & _
            '  "BASINS watershed delineator or update it manually.", vbOKOnly, "BASINS HSPF Information")
            'End If

            If Not aChkCustom Then
                'use pcs data
                If GisUtil.IsLayer("Permit Compliance System") Then
                    'set pcs shape file
                    pcsLayerIndex = GisUtil.LayerIndex("Permit Compliance System")
                    npdesIndex = GisUtil.FieldIndex(pcsLayerIndex, "NPDES")
                    flowIndex = GisUtil.FieldIndex(pcsLayerIndex, "FLOW_RATE")
                    facIndex = GisUtil.FieldIndex(pcsLayerIndex, "FAC_NAME")
                    cuIndex = GisUtil.FieldIndex(pcsLayerIndex, "BCU")
                    If npdesIndex > -1 Then
                        For i = 1 To cNPDES.Count
                            flow = 0.0#
                            facname = ""
                            huc = ""
                            mipt = 0.0#
                            If Len(Trim(cNPDES(i))) > 0 Then
                                For j = 1 To GisUtil.NumFeatures(pcsLayerIndex)
                                    If GisUtil.FieldValue(pcsLayerIndex, j - 1, npdesIndex) = cNPDES(i) Then
                                        'this is the one
                                        If IsNumeric(GisUtil.FieldValue(pcsLayerIndex, j - 1, flowIndex)) Then
                                            flow = GisUtil.FieldValue(pcsLayerIndex, j - 1, flowIndex) * 1.55
                                        Else
                                            flow = 0.0
                                        End If
                                        facname = GisUtil.FieldValue(pcsLayerIndex, j - 1, facIndex)
                                        If aChkCalculate Then
                                            'calculate mile point on stream
                                            'dist = myGISTools.NearestPositionOnLineToPoint(StreamsThemeName, StreamsField, cSubbasin(i), IO.Path.GetFileNameWithoutExtension(OutletsJoinThemeName), PCSIdField, pNPDES(j))
                                            'mipt = dist / 1609.3
                                        Else
                                            mipt = 0.0#
                                        End If
                                        huc = GisUtil.FieldValue(pcsLayerIndex, j - 1, cuIndex)
                                        Exit For
                                    End If
                                Next j
                            End If
                            cFlow.Add(flow)
                            cMipt.Add(mipt)
                            cFacName.Add(facname)
                            cHuc.Add(huc)
                        Next i
                    End If
                    'check for dbf associated with each npdes point
                    i = 1
                    dbname = PathNameOnly(GisUtil.LayerFileName(pcsLayerIndex)) & "\pcs\"
                    For Each lnpdes In cNPDES
                        dbffilename = Trim(cHuc(i)) & ".dbf"
                        If Len(Dir(dbname & dbffilename)) > 0 And Len(Trim(lnpdes)) > 0 Then
                            'yes, it exists
                            i = i + 1
                        Else
                            'remove from collection
                            cNPDES.Remove(i)
                            cSubbasin.Remove(i)
                            cFlow.Remove(i)
                            cMipt.Remove(i)
                            cFacName.Remove(i)
                            cHuc.Remove(i)
                        End If
                    Next lnpdes
                Else
                    'no pcs layer, clear out
                    Do While cNPDES.Count > 0
                        cNPDES.Remove(1)
                    Loop
                End If

            Else
                'using custom table
                'must have these fields in this order:
                'pcsid (same as outlets layer)
                'facname
                'load (flow or other value) lbs/yr or cfs
                'parm (flow or other name)
                tmpDbf = atcUtility.atcTableOpener.OpenAnyTable(aLblCustom)

                i = 1
                Do While i <= cNPDES.Count
                    If aChkCalculate Then
                        'calculate mile point on stream
                        'dist = myGISTools.NearestPositionOnLineToPoint(StreamsThemeName, StreamsField, cSubbasin(i), IO.Path.GetFileNameWithoutExtension(OutletsJoinThemeName), PCSIdField, pNPDES(j))
                        'mipt = dist / 1609.3
                    Else
                        mipt = 0.0#
                    End If
                    cMipt.Add(mipt)
                    iFound = False
                    For j = 1 To tmpDbf.NumRecords
                        tmpDbf.CurrentRecord = j
                        If cNPDES(i) = tmpDbf.Value(1) Then
                            cFacName.Add(tmpDbf.Value(2))
                            iFound = True
                            Exit For
                        End If
                    Next j
                    If Not iFound Then
                        cNPDES.Remove(i)
                        cSubbasin.Remove(i)
                        cMipt.Remove(i)
                    Else
                        i = i + 1
                    End If
                Loop
            End If
        End If

        'write first part of point source file
        PrintLine(OutFile, " " & CStr(cNPDES.Count))
        PrintLine(OutFile, " ")
        WriteLine(OutFile, "Facility Name", "Npdes", "Cuseg", "Mi")
        For i = 1 To cNPDES.Count
            ctemp = Chr(34) & cFacName(i) & Chr(34) & " " & cNPDES(i) & " " & cSubbasin(i) & " " & Format(cMipt(i), "0.000000")
            PrintLine(OutFile, ctemp)
        Next i

        If Not aChkCustom Then
            'read in Permitted Discharges Parameter Table
            If cNPDES.Count > 0 Then
                'open dbf file
                tmpDbf = atcUtility.atcTableOpener.OpenAnyTable(PathNameOnly(GisUtil.LayerFileName(pcsLayerIndex)) & "\pcs3_prm.dbf")
                RowCount = tmpDbf.NumRecords
                ReDim ParmCode(RowCount)
                ReDim ParmName(RowCount)
                For i = 1 To RowCount
                    tmpDbf.CurrentRecord = i
                    ParmCode(i) = tmpDbf.Value(1)
                    ParmName(i) = tmpDbf.Value(2)
                Next i
            End If
        End If

        PrintLine(OutFile, " ")
        WriteLine(OutFile, "Ordinal Number", "Pollutant", "Load (lbs/hr)")
        If Not aChkCustom Then
            'using pcs data
            prevdbf = ""
            For j = 1 To cNPDES.Count
                'open dbf file
                dbffilename = dbname & Trim(cHuc(j)) & ".dbf"
                If Len(Dir(dbffilename)) > 0 Then
                    If dbffilename <> prevdbf Then
                        tmpDbf = atcUtility.atcTableOpener.OpenAnyTable(dbffilename)
                        prevdbf = dbffilename
                        For k = 1 To tmpDbf.NumFields
                            If UCase(tmpDbf.FieldName(k)) = "YEAR" Then
                                YearField = k
                            End If
                            If UCase(tmpDbf.FieldName(k)) = "PARM" Then
                                ParmField = k
                            End If
                            If UCase(tmpDbf.FieldName(k)) = "LOAD" Then
                                LoadField = k
                            End If
                            If UCase(tmpDbf.FieldName(k)) = "NPDES" Then
                                NPDESField = k
                            End If
                        Next k
                        dbrcount = tmpDbf.NumRecords
                        ReDim TableYear(dbrcount)
                        ReDim TableParm(dbrcount)
                        ReDim TableLoad(dbrcount)
                        ReDim TableNPDES(dbrcount)
                        For k = 1 To dbrcount
                            tmpDbf.CurrentRecord = k
                            TableYear(k) = tmpDbf.Value(YearField)
                            TableParm(k) = tmpDbf.Value(ParmField)
                            TableLoad(k) = tmpDbf.Value(LoadField)
                            TableNPDES(k) = tmpDbf.Value(NPDESField)
                        Next k
                    End If
                    For k = 1 To dbrcount
                        If TableNPDES(k) = cNPDES(j) And TableYear(k) = aYear Then
                            'found one, output it
                            tPoll = ""
                            For i = 0 To RowCount - 1
                                If TableParm(k) = ParmCode(i) Then
                                    tPoll = ParmName(i)
                                    Exit For
                                End If
                            Next i
                            tValue = TableLoad(k) / 8760 'lbs/hr
                            ctemp = CStr(j - 1) & " " & Chr(34) & Trim(tPoll) & Chr(34) & " " & Format(CSng(tValue), "0.000000")
                            PrintLine(OutFile, ctemp)
                        End If
                    Next k
                End If
            Next j
            'now output flows
            For j = 1 To cNPDES.Count
                ctemp = CStr(j - 1) & " Flow " & Format(cFlow(j), "0.000000")
                PrintLine(OutFile, ctemp)
            Next j
        Else
            'using custom data
            tmpDbf = atcUtility.atcTableOpener.OpenAnyTable(aLblCustom)
            For i = 1 To cNPDES.Count
                For j = 1 To tmpDbf.NumRecords
                    tmpDbf.CurrentRecord = j
                    If cNPDES(i) = tmpDbf.Value(1) Then
                        If UCase(tmpDbf.Value(4)) = "FLOW" Then
                            ctemp = CStr(i - 1) & " Flow " & Format(CStr(tmpDbf.Value(3)), "0.000000")
                        Else
                            tValue = CSng(tmpDbf.Value(3)) / 8760 'lbs/hr
                            ctemp = CStr(i - 1) & " " & Chr(34) & Trim(tmpDbf.Value(4)) & Chr(34) & " " & Format(CStr(tValue), "0.000000")
                        End If
                        PrintLine(OutFile, ctemp)
                    End If
                Next j
            Next i
        End If

        FileClose(OutFile)
    End Sub

    Friend Sub WriteSEGFile(ByVal aSegFileName As String, ByVal aMetSegIds As atcCollection, ByVal aMetIndices As atcCollection, ByVal aMetBaseDsns As atcCollection)

        Dim lOutFile As Integer = FreeFile()
        FileOpen(lOutFile, aSegFileName, OpenMode.Output)

        WriteLine(lOutFile, "SegID", "PrecWdmId", "PrecDsn", "PrecTstype", "PrecMFactPI", "PrecMFactR", _
                                     "AtemWdmId", "AtemDsn", "AtemTstype", "AtemMFactPI", "AtemMFactR", _
                                     "DewpWdmId", "DewpDsn", "DewpTstype", "DewpMFactPI", "DewpMFactR", _
                                     "WindWdmId", "WindDsn", "WindTstype", "WindMFactPI", "WindMFactR", _
                                     "SolrWdmId", "SolrDsn", "SolrTstype", "SolrMFactPI", "SolrMFactR", _
                                     "ClouWdmId", "ClouDsn", "ClouTstype", "ClouMFactPI", "ClouMFactR", _
                                     "PevtWdmId", "PevtDsn", "PevtTstype", "PevtMFactPI", "PevtMFactR")

        For lIndex As Integer = 0 To aMetIndices.Count - 1
            Dim lBaseDsn As Integer = aMetBaseDsns(aMetIndices(lIndex))
            PrintLine(lOutFile, CStr(aMetSegIds(lIndex)) & " WDM2 " & CStr(lBaseDsn) & " PREC 1 1" & _
                                                           " WDM2 " & CStr(lBaseDsn + 2) & " ATEM 1 1" & _
                                                           " WDM2 " & CStr(lBaseDsn + 6) & " DEWP 1 1" & _
                                                           " WDM2 " & CStr(lBaseDsn + 3) & " WIND 1 1" & _
                                                           " WDM2 " & CStr(lBaseDsn + 4) & " SOLR 1 1" & _
                                                           " WDM2 " & CStr(lBaseDsn + 7) & " CLOU 0 1" & _
                                                           " WDM2 " & CStr(lBaseDsn + 5) & " PEVT 1 1")
        Next

        FileClose(lOutFile)

    End Sub

    Friend Sub WriteMAPFile(ByVal aMapFileName As String)
        Dim lOutFile As Integer
        Dim i As Integer
        Dim lTemp As String

        lOutFile = FreeFile()
        FileOpen(lOutFile, aMapFileName, OpenMode.Output)

        lTemp = "EXT " & GisUtil.MapExtentXmin & " " & GisUtil.MapExtentYmax & " " & GisUtil.MapExtentXmax & " " & GisUtil.MapExtentYmin
        PrintLine(lOutFile, lTemp)

        For i = 0 To GisUtil.NumLayers - 1
            If GisUtil.LayerType(i) = 1 Or GisUtil.LayerType(i) = 2 Or GisUtil.LayerType(i) = 3 Then
                'shapefile
                lTemp = "LYR '" + GisUtil.LayerFileName(i) & "', " & GisUtil.LayerColor(i)
                If GisUtil.LayerType(i) = 3 Then
                    'polygon 
                    If Not GisUtil.LayerTransparent(i) Then
                        lTemp = lTemp & ",Style Transparent "
                    End If
                    lTemp = lTemp & ",Outline " & GisUtil.LayerOutlineColor(i)
                End If
                'hide the layers not turned on
                If Not GisUtil.LayerVisible(i) Then
                    lTemp = lTemp & ",Hide"
                End If
                'add theme name as caption
                lTemp = lTemp & ",Name '" & GisUtil.LayerName(i) & "'"
                PrintLine(lOutFile, lTemp)
            End If
        Next i
        FileClose(lOutFile)

    End Sub

End Module
