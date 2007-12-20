Imports atcUtility
Imports atcMwGisUtility
Imports MapWinUtility
Imports atcData
Imports System.Collections.ObjectModel
Imports atcSegmentation

Module modModelSetup

    Friend Sub StartWinHSPF(ByVal aCommand As String)
        Dim WinHSPFexe As String

        'todo:  get this from the registry
        WinHSPFexe = "c:\basins\models\hspf\bin\winhspf.exe"
        If Not FileExists(WinHSPFexe) Then
            WinHSPFexe = "d:\basins\models\hspf\bin\winhspf.exe"
        End If
        If Not FileExists(WinHSPFexe) Then
            WinHSPFexe = "e:\basins\models\hspf\bin\winhspf.exe"
        End If
        If Not FileExists(WinHSPFexe) Then
            Dim lBasinsBinLoc As String = PathNameOnly(System.Reflection.Assembly.GetEntryAssembly.Location)
            WinHSPFexe = Mid(lBasinsBinLoc, 1, Len(lBasinsBinLoc) - 3) & "models\hspf\bin\winhspf.exe"
        End If
        If Not FileExists(WinHSPFexe) Then
            WinHSPFexe = FindFile("Please locate WinHSPF.exe", "WinHSPF.exe")
        End If
        If FileExists(WinHSPFexe) Then
            Process.Start(WinHSPFexe, aCommand)
        Else
            MsgBox("Cannot find WinHSPF.exe", MsgBoxStyle.Critical, "BASINS HSPF Problem")
        End If
    End Sub

    Friend Sub StartAQUATOX(ByVal aCommand As String)
        Dim AQUATOXexe$
        'Dim reg As New ATCoRegistry

        'todo:  get this from the registry
        'AQUATOXexe = reg.RegGetString(HKEY_LOCAL_MACHINE, "SOFTWARE\Eco Modeling\AQUATOX\ExePath", "") & "\AQUATOX.exe"
        AQUATOXexe = "\basins\models\AQUATOX\AQUATOX.exe"

        If FileExists(AQUATOXexe) Then
            Process.Start(AQUATOXexe, aCommand)
        Else
            MsgBox("Cannot find AQUATOX.exe", MsgBoxStyle.Critical, "BASINS AQUATOX Problem")
        End If
    End Sub

    Friend Function IsBASINSMetWDM(ByVal aDataSets As atcData.atcDataGroup, ByVal aBaseDsn As Integer, ByVal aLoc As String) As Boolean
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

        If lCheckCount = 7 Then
            Return True
        Else
            Return False
        End If

    End Function

    Friend Sub BuildListofMetStationNames(ByRef aMetWDMName As String, ByVal aMetStations As atcCollection, ByVal aMetBaseDsns As atcCollection)
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
            If Not lFound Then
                If lDataSource.Open(aMetWDMName) Then
                    'need to open it here
                    lFound = True
                End If
            End If
            If lFound Then
                For Each lDataSet As atcData.atcTimeseries In lDataSource.DataSets
                    If lDataSet.Attributes.GetValue("Scenario") = "OBSERVED" And lDataSet.Attributes.GetValue("Constituent") = "PREC" Then
                        Dim lLoc As String = lDataSet.Attributes.GetValue("Location")
                        Dim lStanam As String = lDataSet.Attributes.GetValue("Stanam")
                        Dim lDsn As Integer = lDataSet.Attributes.GetValue("Id")
                        'get the common dates from prec and pevt at this location
                        Dim lSJDay As Double = lDataSet.Dates.Value(0)
                        Dim lEJDay As Double = lDataSet.Dates.Value(lDataSet.Dates.numValues)
                        'find pevt dataset at the same location
                        Dim lPevtFound As Boolean = False
                        For Each lDataSet2 As atcData.atcTimeseries In lDataSource.DataSets
                            If lDataSet2.Attributes.GetValue("Constituent") = "PEVT" And _
                               lDataSet2.Attributes.GetValue("Location") = lLoc Then
                                Dim lSJDay2 As Double = lDataSet2.Dates.Value(0)
                                Dim lEJDay2 As Double = lDataSet2.Dates.Value(lDataSet2.Dates.numValues)
                                If lSJDay2 > lSJDay Then lSJDay = lSJDay2
                                If lEJDay2 < lEJDay Then lEJDay = lEJDay2
                                lPevtFound = True
                                Exit For
                            End If
                        Next
                        If lPevtFound Then
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

    Friend Function CreateUCI(ByVal aUciName As String, ByVal aMetWDMName As String) As Boolean
        CreateUCI = False

        ChDriveDir(PathNameOnly(aUciName))
        'get message file ready
        Dim lMsg As New atcUCI.HspfMsg
        lMsg.Open("hspfmsg.mdb")

        'get starter uci ready
        Dim lBasinsBinLoc As String = PathNameOnly(System.Reflection.Assembly.GetEntryAssembly.Location)
        Dim lStarterUciName As String = "starter.uci"
        Dim lStarterPath As String = Mid(lBasinsBinLoc, 1, Len(lBasinsBinLoc) - 3) & "models\hspf\bin\starter\" & lStarterUciName
        If Not FileExists(lStarterPath) Then
            lStarterPath = "\basins\models\hspf\bin\starter\" & lStarterUciName
            If Not FileExists(lStarterPath) Then
                lStarterPath = FindFile("Please locate " & lStarterUciName, lStarterUciName)
            End If
        End If
        lStarterUciName = lStarterPath

        'location master pollutant list 
        Dim lPollutantListFileName As String = "poltnt_2.prn"
        Dim lPollutantListPath As String = Mid(lBasinsBinLoc, 1, Len(lBasinsBinLoc) - 3) & "models\hspf\bin\" & lPollutantListFileName
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
        Dim lProjectWDMName As String = FilenameOnly(aUciName) & ".wdm"
        Dim lFound As Boolean = False
        For Each lBASINSDataSource As atcDataSource In atcDataManager.DataSources
            If lBASINSDataSource.Specification.ToUpper = lProjectWDMName.ToUpper Then
                'found it in the BASINS data sources
                lDataSource = lBASINSDataSource
                lFound = True
                Exit For
            End If
        Next
        If Not lFound Then
            If lDataSource.Open(lProjectWDMName) Then
                'need to open it here
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
        If Not lFound Then
            If lDataSource.Open(aMetWDMName) Then
                'need to open it here
                lFound = True
            End If
        End If
        lDataSources.Add(lDataSource)

        ChDriveDir(PathNameOnly(aUciName))

        Dim lWatershedName As String = FilenameOnly(aUciName)
        Dim lWatershed As New Watershed
        If lWatershed.Open(lWatershedName) = 0 Then  'everything read okay, continue
            Dim lHspfUci As New atcUCI.HspfUci
            lHspfUci.Msg = lMsg
            lHspfUci.CreateUciFromBASINS(lWatershed, _
                                         lDataSources, _
                                         lStarterUciName, lPollutantListFileName)
            lHspfUci.Save()
            Return True
        End If

    End Function

    Friend Sub WriteWSDFile(ByVal aWsdFileName As String, ByVal aArea As Collection, ByVal aLucode As Collection, _
                            ByVal aSubid As Collection, ByVal aSubslope As Collection, ByVal aReclassifyFile As String, _
                            ByVal aGridPervious As Object)

        Dim OutFile As Integer
        Dim i As Integer, j As Integer, k As Integer
        Dim incollection As Boolean
        Dim percentimperv As Double
        Dim tarea As Double, stype As String
        Dim tmpDbf As IatcTable
        Dim PerArea(,) As Single
        Dim ImpArea(,) As Single
        Dim length() As Single
        Dim slope() As Single
        Dim UseSimpleGrid As Boolean
        Dim spos As Long
        Dim lpos As Long
        Dim luname As String = ""
        Dim multiplier As Single
        Dim subbasin As String
        Dim useit As Boolean

        'if simple reclassifyfile exists, read it in
        Dim cRcode As New Collection
        Dim cRname As New Collection
        UseSimpleGrid = False
        If Len(aReclassifyFile) > 0 And aGridPervious.ColumnWidth(0) = 0 Then
            'have the simple percent pervious grid, need to know which 
            'lucodes correspond to which lugroups
            UseSimpleGrid = True
            'open dbf file
            tmpDbf = atcUtility.atcTableOpener.OpenAnyTable(aReclassifyFile)
            For i = 1 To tmpDbf.NumRecords
                tmpDbf.CurrentRecord = i
                cRcode.Add(tmpDbf.Value(1))
                cRname.Add(tmpDbf.Value(2))
            Next i
        End If

        'create summary array 
        '  area of each land use group in each subbasin

        'build collection of unique subbasin ids
        Dim cUniqueSubids As New Collection
        For i = 1 To aSubid.Count
            incollection = False
            For j = 1 To cUniqueSubids.Count
                If cUniqueSubids(j) = aSubid(i) Then
                    incollection = True
                    Exit For
                End If
            Next j
            If Not incollection Then
                cUniqueSubids.Add(aSubid(i))
            End If
        Next i

        'build collection of unique landuse groups
        Dim cUniqueLugroups As New Collection
        For i = 1 To aGridPervious.Source.Rows
            incollection = False
            For j = 1 To cUniqueLugroups.Count
                If cUniqueLugroups(j) = aGridPervious.Source.CellValue(i, 1) Then
                    incollection = True
                    Exit For
                End If
            Next j
            If Not incollection Then
                cUniqueLugroups.Add(aGridPervious.Source.CellValue(i, 1))
            End If
        Next i

        ReDim PerArea(cUniqueSubids.Count, cUniqueLugroups.Count)
        ReDim ImpArea(cUniqueSubids.Count, cUniqueLugroups.Count)
        ReDim length(cUniqueSubids.Count)
        ReDim slope(cUniqueSubids.Count)

        'loop through each polygon (or grid subid/lucode combination)
        'and populate array with area values
        If UseSimpleGrid Then
            For i = 1 To aSubid.Count

                'find subbasin position in the area array
                For j = 1 To cUniqueSubids.Count
                    If aSubid(i) = cUniqueSubids(j) Then
                        spos = j
                        Exit For
                    End If
                Next j
                'find lugroup that corresponds to this lucode
                For j = 1 To cRcode.Count
                    If aLucode(i) = cRcode(j) Then
                        luname = cRname(j)
                        Exit For
                    End If
                Next j
                'find percent perv that corresponds to this lugroup
                For j = 1 To aGridPervious.Source.Rows
                    If luname = aGridPervious.Source.CellValue(j, 1) Then
                        percentimperv = aGridPervious.Source.CellValue(j, 2)
                        Exit For
                    End If
                Next j
                'find lugroup position in the area array
                For j = 1 To cUniqueLugroups.Count
                    If luname = cUniqueLugroups(j) Then
                        lpos = j
                        Exit For
                    End If
                Next j

                PerArea(spos, lpos) = PerArea(spos, lpos) + (aArea(i) * (100 - percentimperv) / 100)
                ImpArea(spos, lpos) = ImpArea(spos, lpos) + (aArea(i) * percentimperv / 100)
                length(spos) = 0.0
                slope(spos) = aSubslope(i) / 100.0

            Next i

        Else
            'using custom table for landuse classification
            For i = 1 To aSubid.Count
                'loop through each polygon (or grid subid/lucode combination)

                'find subbasin position in the area array
                For j = 1 To cUniqueSubids.Count
                    If aSubid(i) = cUniqueSubids(j) Then
                        spos = j
                        Exit For
                    End If
                Next j

                'find lugroup that corresponds to this lucode, could be multiple matches
                For j = 1 To aGridPervious.Source.Rows
                    luname = ""
                    lpos = -1
                    If aGridPervious.Source.CellValue(j, 0) <> "" Then
                        If aLucode(i) = aGridPervious.Source.CellValue(j, 0) Then
                            'see if any of these are subbasin-specific
                            percentimperv = aGridPervious.Source.CellValue(j, 2)
                            If IsNumeric(aGridPervious.Source.CellValue(j, 3)) Then
                                multiplier = CSng(aGridPervious.Source.CellValue(j, 3))
                            Else
                                multiplier = 1.0
                            End If
                            subbasin = aGridPervious.Source.CellValue(j, 4)
                            If Len(subbasin) > 0 And subbasin <> "Invalid Field Number" Then
                                'this row is subbasin-specific
                                If subbasin = aSubid(i) Then
                                    'we want this one now
                                    luname = aGridPervious.Source.CellValue(j, 1)
                                End If
                            Else
                                'make sure that no other rows of this lucode are 
                                'subbasin-specific for this subbasin and that we 
                                'should therefore not use this row
                                useit = True
                                For k = 1 To aGridPervious.Source.Rows
                                    If k <> j Then
                                        If aGridPervious.Source.CellValue(k, 0) = aGridPervious.Source.CellValue(j, 0) Then
                                            'this other row has same lucode
                                            If aGridPervious.Source.CellValue(k, 1) = aGridPervious.Source.CellValue(j, 1) Then
                                                'and the same group name
                                                subbasin = aGridPervious.Source.CellValue(k, 4)
                                                If Len(subbasin) > 0 Then
                                                    'and its subbasin-specific
                                                    If subbasin = aSubid(i) Then
                                                        'and its specific to this subbasin
                                                        useit = False
                                                    End If
                                                End If
                                            End If
                                        End If
                                    End If
                                Next k
                                If useit Then
                                    'we want this one now
                                    luname = aGridPervious.Source.CellValue(j, 1)
                                End If
                            End If

                            If Len(luname) > 0 Then
                                'find lugroup position in the area array
                                For k = 1 To cUniqueLugroups.Count
                                    If luname = cUniqueLugroups(k) Then
                                        lpos = k
                                        Exit For
                                    End If
                                Next k
                            End If

                            If lpos > 0 Then
                                PerArea(spos, lpos) = PerArea(spos, lpos) + (aArea(i) * multiplier * (100 - percentimperv) / 100)
                                ImpArea(spos, lpos) = ImpArea(spos, lpos) + (aArea(i) * multiplier * percentimperv / 100)
                                length(spos) = 0.0 'were not computing lsur since winhspf does that
                                slope(spos) = aSubslope(i) / 100.0
                            End If

                        End If
                    End If
                Next j

            Next i
        End If

        OutFile = FreeFile()
        FileOpen(OutFile, aWsdFileName, OpenMode.Output)
        WriteLine(OutFile, "LU Name", "Type (1=Impervious, 2=Pervious)", "Watershd-ID", "Area", "Slope", "Distance")

        'now write
        For i = 1 To cUniqueSubids.Count
            For j = 1 To cUniqueLugroups.Count
                stype = "2"
                tarea = PerArea(i, j) / 4046.8564
                If CInt(tarea) > 0 Then
                    PrintLine(OutFile, Chr(34) & cUniqueLugroups(j) & Chr(34), " " & stype & " ", cUniqueSubids(i), Format(tarea, "0."), Format(slope(i), "0.000000"), Format(length(i), "0.0000"))
                End If
                stype = "1"
                tarea = ImpArea(i, j) / 4046.8564
                If CInt(tarea) > 0 Then
                    PrintLine(OutFile, Chr(34) & cUniqueLugroups(j) & Chr(34), " " & stype & " ", cUniqueSubids(i), Format(tarea, "0."), Format(slope(i), "0.000000"), Format(length(i), "0.0000"))
                End If
            Next j
        Next i

        FileClose(OutFile)
    End Sub

    Friend Sub WriteRCHFile(ByVal aRchFileName As String, ByVal aUniqueSubids As Collection, ByVal aSubbasinLayerIndex As Integer, _
                            ByVal aStreamsIndex As Integer, ByVal aStreamsRIndex As Integer, ByVal aLen2Index As Integer, _
                            ByVal aSlo2Index As Integer, ByVal aWid2Index As Integer, ByVal aDep2Index As Integer, _
                            ByVal aMinelIndex As Integer, ByVal aMaxelIndex As Integer, ByVal aSnameIndex As Integer)

        Dim OutFile As Integer, OutFile2 As Integer
        Dim PtfFileName As String
        Dim sname As String
        Dim cDOWN$, cLENGTH#, cDEPTH#, cWIDTH#, cSLOPE#, cMINEL#, cMAXEL#, cELEV#
        Dim i As Long, j As Long

        OutFile = FreeFile()
        FileOpen(OutFile, aRchFileName, OpenMode.Output)
        WriteLine(OutFile, "Rivrch", "Pname", "Watershed-ID", "HeadwaterFlag", _
                  "Exits", "Milept", "Stream/Resevoir Type", "Segl", _
                  "Delth", "Elev", "Ulcsm", "Urcsm", "Dscsm", "Ccsm", _
                  "Mnflow", "Mnvelo", "Svtnflow", "Svtnvelo", "Pslope", _
                  "Pdepth", "Pwidth", "Pmile", "Ptemp", "Pph", "Pk1", _
                  "Pk2", "Pk3", "Pmann", "Psod", "Pbgdo", _
                  "Pbgnh3", "Pbgbod5", "Pbgbod", "Level")

        OutFile2 = FreeFile()
        PtfFileName = Mid(aRchFileName, 1, Len(aRchFileName) - 3) & "ptf"
        FileOpen(OutFile2, PtfFileName, OpenMode.Output)
        WriteLine(OutFile2, "Reach Number", "Length(ft)", _
            "Mean Depth(ft)", "Mean Width (ft)", _
            "Mannings Roughness Coeff.", "Long. Slope", _
            "Type of x-section", "Side slope of upper FP left", _
            "Side slope of lower FP left", "Zero slope FP width left(ft)", _
            "Side slope of channel left", "Side slope of channel right", _
            "Zero slope FP width right(ft)", "Side slope lower FP right", _
            "Side slope upper FP right", "Channel Depth(ft)", _
            "Flood side slope change at depth", "Max. depth", _
            "No. of exits", "Fraction of flow through exit 1", _
            "Fraction of flow through exit 2", "Fraction of flow through exit 3", _
            "Fraction of flow through exit 4", "Fraction of flow through exit 5")

        For i = 1 To GisUtil.NumFeatures(aSubbasinLayerIndex)
            'is this subbasin in the list?
            For j = 1 To aUniqueSubids.Count
                GisUtil.FieldValue(aSubbasinLayerIndex, i - 1, aStreamsIndex)
                If aUniqueSubids(j) = GisUtil.FieldValue(aSubbasinLayerIndex, i - 1, aStreamsIndex) Then
                    'in list, output it
                    sname = GisUtil.FieldValue(aSubbasinLayerIndex, i - 1, aSnameIndex)
                    If Len(Trim(sname)) = 0 Then
                        sname = "STREAM " + aUniqueSubids(j)
                    End If
                    cDOWN = GisUtil.FieldValue(aSubbasinLayerIndex, i - 1, aStreamsRIndex)
                    If IsNumeric(GisUtil.FieldValue(aSubbasinLayerIndex, i - 1, aLen2Index)) Then
                        cLENGTH = (CSng(GisUtil.FieldValue(aSubbasinLayerIndex, i - 1, aLen2Index)) * 3.28) / 5280
                    Else
                        cLENGTH = 0.0#
                    End If
                    If IsNumeric(GisUtil.FieldValue(aSubbasinLayerIndex, i - 1, aSlo2Index)) Then
                        cSLOPE = CSng(GisUtil.FieldValue(aSubbasinLayerIndex, i - 1, aSlo2Index)) / 100
                    Else
                        cSLOPE = 0.0#
                    End If
                    If IsNumeric(GisUtil.FieldValue(aSubbasinLayerIndex, i - 1, aWid2Index)) Then
                        cWIDTH = CSng(GisUtil.FieldValue(aSubbasinLayerIndex, i - 1, aWid2Index)) * 3.28
                    Else
                        cWIDTH = 0.0#
                    End If
                    If IsNumeric(GisUtil.FieldValue(aSubbasinLayerIndex, i - 1, aDep2Index)) Then
                        cDEPTH = CSng(GisUtil.FieldValue(aSubbasinLayerIndex, i - 1, aDep2Index)) * 3.28
                    Else
                        cDEPTH = 0.0#
                    End If
                    If IsNumeric(GisUtil.FieldValue(aSubbasinLayerIndex, i - 1, aMinelIndex)) Then
                        cMINEL = CSng(GisUtil.FieldValue(aSubbasinLayerIndex, i - 1, aMinelIndex)) * 3.28
                    Else
                        cMINEL = 0.0#
                    End If
                    If IsNumeric(GisUtil.FieldValue(aSubbasinLayerIndex, i - 1, aMaxelIndex)) Then
                        cMAXEL = CSng(GisUtil.FieldValue(aSubbasinLayerIndex, i - 1, aMaxelIndex)) * 3.28
                    Else
                        cMAXEL = 0.0#
                    End If
                    cELEV = ((cMAXEL + cMINEL) / 2)
                    PrintLine(OutFile, aUniqueSubids(j) & " " & Chr(34) & sname & Chr(34) & " " & aUniqueSubids(j) & " " & _
                           " 0 1 0 S " & Format(cLENGTH, "0.00") & " " & Format(Math.Abs(cMAXEL - cMINEL), "0.00") & " " & _
                           Format(cELEV, "0.") & " 0 0 " & cDOWN & " 0 0 0 0 0 " & _
                           Format(cSLOPE, "0.000000") & " " & Format(cDEPTH, "0.0000") & " " & Format(cWIDTH, "0.000") & _
                           " 0 0 0 0 0 0 0 0 0 0 0 0 0")
                    PrintLine(OutFile2, aUniqueSubids(j) & " " & Format(cLENGTH * 5280.0#, "0.") & " " & _
                           Format(cDEPTH, "0.00000") & " " & Format(cWIDTH, "0.00000") & " 0.05 " & _
                           Format(cSLOPE, "0.00000") & " " & "Trapezoidal" & " " & _
                           "0.5 0.5 " & Format(cWIDTH, "0.000") & " 1 1 " & Format(cWIDTH, "0.000") & _
                           " 0.5 0.5 " & Format(cDEPTH * 1.25, "0.0000") & " " & Format(cDEPTH * 1.875, "0.0000") & " " & _
                           Format(cDEPTH * 62.5, "0.000") & " 1 1 0 0 0 0")
                    If (2 * cDEPTH) > cWIDTH Then
                        'problem
                        MsgBox("The depth and width values specified for Reach " & aUniqueSubids(j) & ", coupled with the trapezoidal" & vbCrLf & _
                               "cross section assumptions of WinHSPF, indicate a physical imposibility." & vbCrLf & _
                               "(Given 1:1 side slopes, the depth of the channel cannot be more than half the width.)" & vbCrLf & vbCrLf & _
                               "This problem can be corrected in WinHSPF by revising the FTABLE or by " & vbCrLf & _
                               "importing the ptf with modifications to the width and depth values." & vbCrLf & _
                               "See the WinHSPF manual for more information.", vbOKOnly, "Channel Problem")
                    End If
                End If
            Next j
        Next i
        FileClose(OutFile)
        FileClose(OutFile2)
    End Sub

    Friend Sub WritePSRFile(ByVal aPsrFileName As String, ByVal aUniqueSubids As Collection, ByVal aOutSubs As Collection, _
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
                For j = 1 To aUniqueSubids.Count
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
                                            'dist = myGISTools.NearestPositionOnLineToPoint(StreamsThemeName, StreamsField, cSubbasin(i), FilenameOnly(OutletsJoinThemeName), PCSIdField, pNPDES(j))
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
                        'dist = myGISTools.NearestPositionOnLineToPoint(StreamsThemeName, StreamsField, cSubbasin(i), FilenameOnly(OutletsJoinThemeName), PCSIdField, pNPDES(j))
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

    Friend Sub WriteSEGFile(ByVal aSegFileName As String, ByVal aMetIndices As Collection, ByVal aMetBaseDsns As atcCollection)

        Dim lOutFile As Integer = FreeFile()
        FileOpen(lOutFile, aSegFileName, OpenMode.Output)

        WriteLine(lOutFile, "SegID", "PrecWdmId", "PrecDsn", "PrecTstype", "PrecMFactPI", "PrecMFactR", _
                                     "AtemWdmId", "AtemDsn", "AtemTstype", "AtemMFactPI", "AtemMFactR", _
                                     "DewpWdmId", "DewpDsn", "DewpTstype", "DewpMFactPI", "DewpMFactR", _
                                     "WindWdmId", "WindDsn", "WindTstype", "WindMFactPI", "WindMFactR", _
                                     "SolrWdmId", "SolrDsn", "SolrTstype", "SolrMFactPI", "SolrMFactR", _
                                     "ClouWdmId", "ClouDsn", "ClouTstype", "ClouMFactPI", "ClouMFactR", _
                                     "PevtWdmId", "PevtDsn", "PevtTstype", "PevtMFactPI", "PevtMFactR")

        For Each lIndex As Integer In aMetIndices
            Dim lBaseDsn As Integer = aMetBaseDsns(lIndex)
            PrintLine(lOutFile, CStr(lIndex + 1) & " WDM2 " & CStr(lBaseDsn) & " PREC 1 1" & _
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
