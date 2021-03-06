﻿Imports atcwdm
Imports atcUCI
Imports atcUtility
Imports MapWinUtility

''' <summary>
''' updates HSPF PERLND parameter tables with values extracted from an ATC standard spreadsheet of parameter values.
''' </summary>
''' <remarks></remarks>
Module modHSPFTableBuilder
    Friend g_MetSegmentBuild As Boolean = True
    Friend g_BaseDrive As String = "G"
    Friend g_Debug As Boolean = False
    Friend g_Project As String = "Susq" ''"CentralAZ" '_SALT" '"Willamette" 
    Friend g_SubProject As String = "020503" '"020503" '020501
    Friend g_BaseFolder As String
    Friend g_ObsDataWDM As String
    Friend g_OutputWDMCreate As Boolean = True
    Friend g_LandSurfaceSegmentRepeat As Integer = 25 'TODO: hardcoded for TT_GCRP - make generic

    Private pMsg As New HspfMsg("hspfmsg.mdb")
    Private pUci As New HspfUci
    Private pDefUci As New HspfUci

    Sub Initialize()
        g_BaseFolder = g_BaseDrive & ":\Projects\TT_GCRP\ProjectsTT\" & g_Project & "\"
        Select Case g_SubProject
            Case "020501"
                g_ObsDataWDM = g_BaseFolder & "parms" & g_SubProject & "\Danville.wdm"
            Case "020502"
                g_ObsDataWDM = g_BaseFolder & "parms" & g_SubProject & "\westbrsusq.wdm"
            Case "020503"
                g_ObsDataWDM = g_BaseFolder & "parms" & g_SubProject & "\marietta.wdm"
        End Select
    End Sub

    Sub main()
        Initialize()
        My.Computer.FileSystem.CurrentDirectory = g_BaseFolder
        Logger.StartToFile("logs\" & Format(Now, "yyyy-MM-dd") & "at" & Format(Now, "HH-mm") & "-HSPFTableBuilderLog.txt", , False)

        If g_Project.ToLower.Contains("susq") Then
            'ParmSummary()
        End If

        Dim lTransWdmName As String = ""
        Dim lConnectionTable As New atcTableDelimited
        Dim lTargetCons() As String = {"FVOL", "SED1", "SED2", "SED3", "DQAL1", "DQAL2"}

        If IO.File.Exists("parms\" & g_Project & "Trans.txt") Then
            lConnectionTable.Delimiter = ","
            lConnectionTable.OpenFile("parms\" & g_Project & "Trans.txt")
            Dim lHspfTimeseriesTransferDetails As ArrayList = lConnectionTable.PopulateObjects((New HspfTimeseriesTransferDetails).GetType)

            lTransWdmName = "parms\" & g_Project & "Trans.wdm"
            If Not IO.File.Exists(lTransWdmName) Then
                Dim lTransWdm As New atcWDM.atcDataSourceWDM
                Dim lTransDataset As New atcData.atcTimeseries(Nothing)
                lTransDataset.Attributes.SetValue("TU", 3)
                lTransDataset.Attributes.SetValue("TS", 1)

                If lTransWdm.Open(lTransWdmName) Then
                    Logger.Dbg("OpenedTransferWdmFile " & lTransWdm.Name)
                Else
                    Logger.Dbg("*** ProblemOpeningTransferWdmFile " & lTransWdmName)
                End If
                For Each lHspfTimeseriesTransferDetail As HspfTimeseriesTransferDetails In lHspfTimeseriesTransferDetails
                    Dim lId As Integer = lHspfTimeseriesTransferDetail.BaseDsn
                    For Each lTargetCon As String In lTargetCons
                        Dim lTransdatasetToAdd As atcData.atcTimeseries = lTransDataset.Clone
                        lTransdatasetToAdd.Attributes.SetValue("ID", lId)
                        lTransdatasetToAdd.Attributes.SetValue("Constituent", lTargetCon)
                        lTransWdm.AddDataset(lTransdatasetToAdd)
                        lId += 1
                    Next
                Next
            End If
        End If

        pUci.FastReadUciForStarter(pMsg, "parms" & g_SubProject & "\" & g_Project & g_SubProject & "starter.uci")
        Dim lError As String = pUci.ErrorDescription
        If lError.Length > 0 Then
            Logger.Dbg("UciReadError " & lError)
            Exit Sub
        End If
        Logger.Dbg("UCI " & pUci.Name & " Opened")

        pUci.Name = FilenameNoExt(pUci.Name).Replace("starter", "")
        pUci.Name = FilenameNoExt(pUci.Name) & "Aft.uci"

        If g_MetSegmentBuild Then
            FixMetSegments(pUci)
            pUci.Name = FilenameNoExt(pUci.Name) & "Met.uci"
            pUci.Save()
        End If

        'update tables based on contents of starter
        pDefUci.FastReadUciForStarter(pMsg, "parms\starter.uci")
        lError = pDefUci.ErrorDescription
        If lError.Length > 0 Then
            Logger.Dbg("StarterUciReadError " & lError)
            Exit Sub
        End If
        Dim lOperationNames() As String = {"PERLND", "IMPLND", "RCHRES"}
        For Each lOperationName As String In lOperationNames
            Dim lOpnBlkStarter As HspfOpnBlk = pDefUci.OpnBlks(lOperationName)
            Dim lOpnBlk As HspfOpnBlk = pUci.OpnBlks(lOperationName)
            For Each lTable As HspfTable In lOpnBlkStarter.Tables
                Dim lTableName As String = lTable.Name
                If lTable.OccurNum > 1 Then
                    lTableName &= ":" & lTable.OccurNum
                End If
                If Not lOpnBlk.TableExists(lTableName) Then
                    lOpnBlk.AddTableForAll(lTableName, lOperationName, lTable.OccurIndex)
                End If
                If lTableName <> "HYDR-INIT" Then 'TODO: others too?
                    SetDefaultsForTable(pUci, pDefUci, lOperationName, lTableName, False)
                End If
            Next
        Next

        If pUci.MonthData.MonthDataTables.Count = 0 AndAlso pDefUci.MonthData.MonthDataTables.Count > 0 Then
            For Each lMonthData As HspfMonthDataTable In pDefUci.MonthData.MonthDataTables
                pUci.MonthData.MonthDataTables.Add(lMonthData)
            Next
        End If

        pUci.Name = FilenameNoExt(pUci.Name) & "Starter.uci"
        pUci.Save()
        Logger.Dbg("Save " & pUci.Name)

        'update PERLND/PWATER parameters based on values from ATC standard spreadsheet 
        Dim lPrmUpdTable As New atcTableDelimited
        lPrmUpdTable.Delimiter = vbTab
        If lPrmUpdTable.OpenFile("parms\parms.txt") Then
            Dim lReclassifyTable As New atcTableDelimited
            lReclassifyTable.Delimiter = vbTab
            Dim lReclassifyTableName As String = "hrus\HruSummarizeSubBasin.txt"
            If Not IO.File.Exists(lReclassifyTableName) Then
                lReclassifyTableName = "parms\HruSummarizeSubBasin.txt"
            End If
            lReclassifyTable.OpenFile(lReclassifyTableName)
            Dim lSlpRecFieldNumber(1) As Integer
            lSlpRecFieldNumber(0) = lReclassifyTable.FieldNumber("SubBasin")
            lSlpRecFieldNumber(1) = lReclassifyTable.FieldNumber("LandUse")
            Dim lSlopeReclassifyValueField As Integer = lReclassifyTable.FieldNumber("SlopeReclass")

            Dim lSlpRecFieldOperation() As String = {"=", "="}
            Dim lSlpRecFieldValue(1) As String
            Dim lPrmUpdFieldNumber() As Integer = {1, 4, 5}
            Dim lPrmUpdFieldOperation() As String = {"=", "=", "="}
            Dim lPrmUpdFieldValue(2) As String
            For Each lOperation As atcUCI.HspfOperation In pUci.OpnBlks("PERLND").Ids
                Dim lMetSegmentComment As String = lOperation.MetSeg.Comment
                Dim lMetSegmentName As String = lMetSegmentComment.Substring(lMetSegmentComment.Length - 8)
                Dim lLandUseName As String = lOperation.Tables("GEN-INFO").Parms(0).Value
                Dim lSlopeReclassValue As Integer = 1
                lSlpRecFieldValue(0) = lMetSegmentName
                lSlpRecFieldValue(1) = lLandUseName
                If lReclassifyTable.FindMatch(lSlpRecFieldNumber, lSlpRecFieldOperation, lSlpRecFieldValue) Then
                    lSlopeReclassValue = lReclassifyTable.Value(lSlopeReclassifyValueField)
                    If g_Debug Then Logger.Dbg("ID,Met,LU,SlopeReclass:" & lOperation.Id & ":" & lMetSegmentName & ":" & lLandUseName & ":" & lSlopeReclassValue)
                    lPrmUpdFieldValue(0) = "PERLND"
                    lPrmUpdFieldValue(1) = lLandUseName
                    lPrmUpdFieldValue(2) = lSlopeReclassValue
                    Dim lRecordStart As Integer = 1
                    Dim lRecordsFound As Integer = 0
                    While lPrmUpdTable.FindMatch(lPrmUpdFieldNumber, lPrmUpdFieldOperation, lPrmUpdFieldValue, , lRecordStart)
                        lRecordsFound += 1
                        Dim lTableName As String = lPrmUpdTable.Value(2)
                        Dim lParmName As String = lPrmUpdTable.Value(3)
                        Dim lParmValue As String = lPrmUpdTable.Value(6)
                        lRecordStart = lPrmUpdTable.CurrentRecord + 1
                        For Each lTable As HspfTable In lOperation.Tables
                            If lTable.Name = lTableName Then
                                For Each lParm As HspfParm In lTable.Parms
                                    If lParmName = lParm.Name Then
                                        If g_Debug Then Logger.Dbg("Update:" & lTableName & ":" & lParmName & ":" & lParm.Value & ":" & lParmValue)
                                        lParm.Value = lParmValue
                                        Exit For
                                    End If
                                Next
                                Exit For
                            End If
                        Next
                    End While
                    If lRecordsFound = 0 Then
                        Logger.Dbg("NoParmUpdatesFor " & lMetSegmentName & ":" & lLandUseName)
                    End If
                Else
                    Logger.Dbg("NoSlopeReclassFor " & lMetSegmentName & ":" & lLandUseName)
                End If
            Next lOperation
            'Next
            pUci.Name = FilenameNoExt(pUci.Name) & "Hydparmupd.uci"
            pUci.Save()
        End If

        'open or create a wdm file for output as needed
        Dim lDefaultOutputWdmFileName As String = "parms" & g_SubProject & "\" & g_Project & g_SubProject & ".wdm"
        If g_OutputWDMCreate AndAlso IO.File.Exists(lDefaultOutputWdmFileName) Then
            IO.File.Delete(lDefaultOutputWdmFileName)
        End If
        Dim lWdmOutput As New atcWDM.atcDataSourceWDM
        lWdmOutput.Open(lDefaultOutputWdmFileName)
        For Each lConnection As HspfConnection In pUci.Connections
            If lConnection.Target.VolName.Contains("WDM") Then
                Dim lDataset As New atcData.atcTimeseries(Nothing)
                With lDataset.Attributes
                    .SetValue("ID", lConnection.Target.VolId)
                    .SetValue("Scenario", IO.Path.GetFileNameWithoutExtension(pUci.Name))
                    .SetValue("Constituent", lConnection.Target.Member)
                    .SetValue("Location", lConnection.Source.Opn.Name.Substring(0, 1) & ":" & lConnection.Source.Opn.Id)
                    .SetValue("TU", 4)
                    .SetValue("TS", 1)
                    .SetValue("TSTYPE", lConnection.Target.Member)
                End With
                Dim lTsDate As atcData.atcTimeseries = New atcData.atcTimeseries(Nothing)
                lDataset.Dates = lTsDate
                Dim lAddedDsn As Boolean = lWdmOutput.AddDataset(lDataset)
            End If
        Next

        Dim pExpertSystemLocsFileName As String = "parms" & g_SubProject & "\ExpertSystemLocs.txt"
        If IO.File.Exists(pExpertSystemLocsFileName) Then
            Dim lOstr(28) As String
            Dim lDsns(28) As Integer
            For Each lRecord As String In LinesInFile(pExpertSystemLocsFileName)
                Dim lParms() As String = lRecord.Split(",") 'reachId, location, base dsn, upstreamArea(including this uci)
                Dim lUpstreamArea As Double = 0.0
                If lParms.GetUpperBound(0) > 2 Then
                    lUpstreamArea = lParms(3)
                End If
                pUci.AddExpertSystem(lParms(0), lParms(1), lWdmOutput, lParms(2), lDsns, lOstr, lUpstreamArea)
            Next
            pUci.Name = FilenameNoExt(pUci.Name) & "Expert.uci"
            pUci.Save()

            If g_ObsDataWDM.Length > 0 Then
                Dim lWdmObserved As New atcWDM.atcDataSourceWDM
                lWdmObserved.Open(g_ObsDataWDM)
                For Each lDataSet As atcData.atcTimeseries In lWdmObserved.DataSets
                    lWdmOutput.AddDataset(lDataSet.Clone, atcData.atcDataSource.EnumExistAction.ExistRenumber)
                Next
            End If
        End If

        IO.File.Copy(pUci.Name, "parms" & g_SubProject & "\" & g_Project & g_SubProject & ".uci", True)
        Logger.Dbg("AllDone")
    End Sub

    Friend Class HspfTimeseriesTransferDetails
        Public SourceHuc As Integer
        Public SourceId As Integer
        Public TargetHuc As Integer
        Public TargetId As Integer
        Public BaseDsn As Integer
    End Class
    '
    'TODO: remaining code from WinHSPF - modWinHSPF - need to refactor both and move to atcUCI
    '
    Public Sub SetDefaultsForTable(ByVal aUCI As HspfUci, ByVal aDefUCI As HspfUci, ByVal aOpName As String, ByVal TableName As String, Optional ByVal aWinHspf As Boolean = False)
        If aUCI.OpnBlks(aOpName).Count > 0 Then
            Dim lOptyp As HspfOpnBlk = aUCI.OpnBlks(aOpName)
            For Each lOpn As HspfOperation In lOptyp.Ids
                Dim lId As Integer = DefaultOpnId(lOpn, aDefUCI)
                If lId > 0 Then
                    Dim lDOpn As HspfOperation = aDefUCI.OpnBlks(lOpn.Name).OperFromID(lId)
                    If Not lDOpn Is Nothing Then
                        If lOpn.TableExists(TableName) Then
                            Dim lTab As HspfTable = lOpn.Tables(TableName)
                            If DefaultThisTable(lOptyp.Name, lTab.Name, aWinHspf) Then
                                If lDOpn.TableExists(TableName) Then
                                    Dim lDTab As HspfTable = lDOpn.Tables(TableName)
                                    For Each lPar As HspfParm In lTab.Parms
                                        If DefaultThisParameter(lOptyp.Name, TableName, lPar.Name, aWinHspf) Then
                                            If lPar.Value <> lPar.Name Then
                                                lPar.Value = lDTab.Parms(lPar.Name).Value
                                            End If
                                        End If
                                    Next
                                End If
                            End If
                        End If
                    End If
                End If
            Next
        End If
    End Sub

    Public Function DefaultOpnId(ByVal aOpn As HspfOperation, ByVal aDefUCI As HspfUci) As Long
        If aOpn.DefOpnId <> 0 Then
            DefaultOpnId = aOpn.DefOpnId
        Else
            Dim lDOpn As HspfOperation = matchOperWithDefault(aOpn.Name, aOpn.Description, aDefUCI)
            If lDOpn Is Nothing Then
                DefaultOpnId = 0
            Else
                DefaultOpnId = lDOpn.Id
            End If
        End If
    End Function

    Private Function DefaultThisTable(ByVal aOperName As String, ByVal aTableName As String, Optional ByVal aWinHSPF As Boolean = True) As Boolean
        If aOperName = "PERLND" Or aOperName = "IMPLND" Then
            If (aWinHSPF AndAlso aTableName = "ACTIVITY") Or _
               aTableName = "PRINT-INFO" Or _
               aTableName = "GEN-INFO" Or _
               aTableName = "PWAT-PARM5" Then
                DefaultThisTable = False
            ElseIf aWinHSPF AndAlso aTableName.StartsWith("QUAL") Then
                DefaultThisTable = False
            Else
                DefaultThisTable = True
            End If
        ElseIf aOperName = "RCHRES" Then
            If (aWinHSPF AndAlso aTableName = "ACTIVITY") Or _
               aTableName = "PRINT-INFO" Or _
               aTableName = "GEN-INFO" Or _
               aTableName = "HYDR-PARM1" Then
                DefaultThisTable = False
            ElseIf aWinHSPF AndAlso aTableName.StartsWith("GQ-") Then
                DefaultThisTable = False
            Else
                DefaultThisTable = True
            End If
        Else
            DefaultThisTable = False
        End If
    End Function

    Private Function DefaultThisParameter(ByVal aOperName As String, ByVal aTableName As String, ByVal aParmName As String, Optional ByVal aWinHSPF As Boolean = True) As Boolean
        DefaultThisParameter = True
        If aOperName = "PERLND" Then
            If aTableName = "PWAT-PARM2" Then
                If aParmName = "SLSUR" Or aParmName = "LSUR" Then
                    DefaultThisParameter = False
                End If
            ElseIf aWinHSPF AndAlso aTableName = "NQUALS" Then
                If aParmName = "NQUAL" Then
                    DefaultThisParameter = False
                End If
            End If
        ElseIf aOperName = "IMPLND" Then
            If aTableName = "IWAT-PARM2" Then
                If aParmName = "SLSUR" Or aParmName = "LSUR" Then
                    DefaultThisParameter = False
                End If
            ElseIf aWinHSPF AndAlso aTableName = "NQUALS" Then
                If aParmName = "NQUAL" Then
                    DefaultThisParameter = False
                End If
            End If
        ElseIf aOperName = "RCHRES" Then
            If aTableName = "HYDR-PARM2" Then
                If aParmName = "LEN" Or _
                   aParmName = "DELTH" Or _
                   aParmName = "FTBUCI" Then
                    DefaultThisParameter = False
                End If
            ElseIf aWinHSPF AndAlso aTableName = "GQ-GENDATA" Then
                If aParmName = "NGQUAL" Then
                    DefaultThisParameter = False
                End If
            End If
        End If
    End Function

    Public Function matchOperWithDefault(ByVal aOpTypName As String, ByVal aOpnDesc As String, ByVal aDefUCI As HspfUci) As HspfOperation
        For Each lOpn As HspfOperation In aDefUCI.OpnBlks(aOpTypName).Ids
            If lOpn.Description = aOpnDesc Then
                matchOperWithDefault = lOpn
                Exit Function
            End If
        Next
        'a complete match not found, look for partial
        Dim lTempString As String
        For Each lOpn As HspfOperation In aDefUCI.OpnBlks(aOpTypName).Ids
            If Len(lOpn.Description) > Len(aOpnDesc) Then
                lTempString = Microsoft.VisualBasic.Left(lOpn.Description, Len(aOpnDesc))
                If lTempString = aOpnDesc Then
                    matchOperWithDefault = lOpn
                    Exit Function
                End If
            ElseIf Len(lOpn.Description) < Len(aOpnDesc) Then
                lTempString = Microsoft.VisualBasic.Left(aOpnDesc, Len(lOpn.Description))
                If lOpn.Description = lTempString Then
                    matchOperWithDefault = lOpn
                    Exit Function
                End If
            End If
            If Len(aOpnDesc) > 4 And Len(lOpn.Description) > 4 Then
                lTempString = Microsoft.VisualBasic.Left(aOpnDesc, 4)
                If Microsoft.VisualBasic.Left(lOpn.Description, 4) = lTempString Then
                    matchOperWithDefault = lOpn
                    Exit Function
                End If
            End If
        Next
        'not found, use first one
        If aDefUCI.OpnBlks(aOpTypName).Count > 0 Then
            matchOperWithDefault = aDefUCI.OpnBlks(aOpTypName).Ids(0)
        Else
            matchOperWithDefault = Nothing
        End If
    End Function
End Module
