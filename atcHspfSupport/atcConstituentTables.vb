Imports atcUtility
Imports atcData
Imports MapWinUtility
Imports atcUCI
Imports System.Data
Public Module atcConstituentTables
    Public pLand_Constituent_Table As DataTable
    Public pReach_Budget_Table As DataTable

    Public Function LandLoadingReports(ByVal aoutfoldername As String,
                                       ByVal aBinaryData As atcDataSource,
                                       ByVal aUCI As HspfUci,
                                       ByVal aScenario As String,
                                       ByVal aRunMade As String,
                                       ByVal aBalanceType As String,
                                       ByVal aConstProperties As List(Of ConstituentProperties),
                                       ByVal aSDateJ As Double, ByVal aEDateJ As Double,
                                       Optional ByVal aGQALID As Integer = 0) As Data.DataTable

        'This Sub prepares a text report for constituents like TN and TP.
        Dim lReport As New atcReport.ReportText
        Dim lReport_Monthly As New atcReport.ReportText
        pLand_Constituent_Table = New DataTable("LandConstituentTable")
        Dim lLand_Constituent_Monthly_Table As New DataTable("LandConstituentMonthlyTable")
        Dim lQualityConstituent As Boolean = False
        'Dim lOutflowDataTypes As String() = ConstituentList(aBalanceType, QualityConstituent)
        Dim lDataForBoxWhiskerPlot As New BoxWhiskerItem
        lDataForBoxWhiskerPlot.Constituent = aBalanceType
        lDataForBoxWhiskerPlot.Scenario = aScenario
        lDataForBoxWhiskerPlot.TimeSpan = TimeSpanAsString(aSDateJ, aEDateJ, "Analysis Period: ")
        Dim lListLanduses As New List(Of String)
        Dim lLandUseSumAnnualValues As New atcCollection
        Dim lLandUseNameForTheCollection As String = ""
        Dim lUnits As String = ""

        pLand_Constituent_Table = AddFirstThreeColumnsLandLoading(pLand_Constituent_Table)
        lLand_Constituent_Monthly_Table = AddFirstThreeColumnsLandLoading(lLand_Constituent_Monthly_Table)
        lLand_Constituent_Monthly_Table.Columns.Remove("Year")
        Select Case aBalanceType
#Region "Case Water"
            Case "Water", "WAT"

                If aUCI.GlobalBlock.EmFg = 1 Then
                    lUnits = "in"
                Else
                    lUnits = "mm"
                End If
                lDataForBoxWhiskerPlot.Units = (lUnits & "/yr")

                Dim lColumn As DataColumn
                Dim lRow As DataRow

                lLand_Constituent_Monthly_Table = AddMonthlyColumnsColumns(lLand_Constituent_Monthly_Table)
                lColumn = New DataColumn()
                lColumn.DataType = Type.GetType("System.String")
                lColumn.ColumnName = "SUPY"
                lColumn.Caption = "Rainfall (" & lUnits & ")"
                pLand_Constituent_Table.Columns.Add(lColumn)

                lColumn = New DataColumn()
                lColumn.DataType = Type.GetType("System.String")
                lColumn.ColumnName = "IRRAPP6"
                lColumn.Caption = "Irrigation (" & lUnits & ")"
                pLand_Constituent_Table.Columns.Add(lColumn)

                lColumn = New DataColumn()
                lColumn.DataType = Type.GetType("System.Double")
                lColumn.ColumnName = "SURO"
                lColumn.Caption = "Surface Outflow (" & lUnits & ")"
                pLand_Constituent_Table.Columns.Add(lColumn)

                lColumn = New DataColumn()
                lColumn.DataType = Type.GetType("System.Double")
                lColumn.ColumnName = "IFWO"
                lColumn.Caption = "Interflow Outflow (" & lUnits & ")"
                pLand_Constituent_Table.Columns.Add(lColumn)

                lColumn = New DataColumn()
                lColumn.DataType = Type.GetType("System.Double")
                lColumn.ColumnName = "AGWO"
                lColumn.Caption = "Groundwater Outflow (" & lUnits & ")"
                pLand_Constituent_Table.Columns.Add(lColumn)

                lColumn = New DataColumn()
                lColumn.DataType = Type.GetType("System.Double")
                lColumn.ColumnName = "TotalOutflow"
                lColumn.Caption = "Total Outflow (" & lUnits & ")"
                pLand_Constituent_Table.Columns.Add(lColumn)

                lColumn = New DataColumn()
                lColumn.DataType = Type.GetType("System.Double")
                lColumn.ColumnName = "IGWI"
                lColumn.Caption = "Deep Groundwater Flow (" & lUnits & ")"
                pLand_Constituent_Table.Columns.Add(lColumn)

                lColumn = New DataColumn()
                lColumn.DataType = Type.GetType("System.Double")
                lColumn.ColumnName = "AGWI"
                lColumn.Caption = "Active Groundwater (" & lUnits & ")"
                pLand_Constituent_Table.Columns.Add(lColumn)

                lColumn = New DataColumn()
                lColumn.DataType = Type.GetType("System.Double")
                lColumn.ColumnName = "AGWLI"
                lColumn.Caption = "Pumping (" & lUnits & ")"
                pLand_Constituent_Table.Columns.Add(lColumn)

                lColumn = New DataColumn()
                lColumn.DataType = Type.GetType("System.Double")
                lColumn.ColumnName = "PET"
                lColumn.Caption = "Potential Evapotranspiration (" & lUnits & ")"
                pLand_Constituent_Table.Columns.Add(lColumn)

                lColumn = New DataColumn()
                lColumn.DataType = Type.GetType("System.Double")
                lColumn.ColumnName = "CEPE"
                lColumn.Caption = "Interception Storage (" & lUnits & ")"
                pLand_Constituent_Table.Columns.Add(lColumn)

                lColumn = New DataColumn()
                lColumn.DataType = Type.GetType("System.Double")
                lColumn.ColumnName = "UZET"
                lColumn.Caption = "Upper Zone Storage (" & lUnits & ")"
                pLand_Constituent_Table.Columns.Add(lColumn)

                lColumn = New DataColumn()
                lColumn.DataType = Type.GetType("System.Double")
                lColumn.ColumnName = "LZET"
                lColumn.Caption = "Lower Zone Storage (" & lUnits & ")"
                pLand_Constituent_Table.Columns.Add(lColumn)

                lColumn = New DataColumn()
                lColumn.DataType = Type.GetType("System.Double")
                lColumn.ColumnName = "AGWET"
                lColumn.Caption = "Ground Water Storage (" & lUnits & ")"
                pLand_Constituent_Table.Columns.Add(lColumn)

                lColumn = New DataColumn()
                lColumn.DataType = Type.GetType("System.Double")
                lColumn.ColumnName = "BASET"
                lColumn.Caption = "Baseflow (" & lUnits & ")"
                pLand_Constituent_Table.Columns.Add(lColumn)

                lColumn = New DataColumn()
                lColumn.DataType = Type.GetType("System.Double")
                lColumn.ColumnName = "TAET"
                lColumn.Caption = "Total Evapotranspiration (" & lUnits & ")"
                pLand_Constituent_Table.Columns.Add(lColumn)

                Dim RowNumber As Integer = 0

                For Each lOperation As HspfOperation In aUCI.OpnSeqBlock.Opns
                    If Not (lOperation.Name = "PERLND" OrElse lOperation.Name = "IMPLND") Then Continue For
                    Dim LocationName As String = lOperation.Name.Substring(0, 1) & ":" & lOperation.Id
                    lLandUseNameForTheCollection = lOperation.Name.Substring(0, 1) & ":" & lOperation.Description
                    Logger.Status("Generating Land Loading Reports for Water from " & LocationName)
                    Logger.Dbg(LocationName)
                    If Not lListLanduses.Contains(lLandUseNameForTheCollection) Then
                        lListLanduses.Add(lLandUseNameForTheCollection)
                    End If
                    Dim lOperationIsConnected As Boolean = False
                    Dim lTSNumber As Integer = 0
                    Dim lOutflowDataTypes1 As Dictionary(Of String, String) = ConstituentList(aBalanceType)
                    Dim lTS As New atcTimeseries(Nothing)
                    Dim lAddTS As New atcDataGroup
                    Dim lTotalTS As New atcTimeseries(Nothing)
                    For Each lOutflowDataType As String In lOutflowDataTypes1.Keys
                        Dim lMasslinkFactor As Double = 1.0
                        If lOutflowDataType = "TotalOutflow" Then
                            lTS = lTotalTS
                            If lTS Is Nothing Then Continue For
                            Dim lTsMonthly As atcTimeseries = Aggregate(lTS, atcTimeUnit.TUMonth, 1, atcTran.TranSumDiv)
                            Dim lSeasons As New atcSeasonsMonth
                            Dim lSeasonalAttributes As New atcDataAttributes
                            lSeasonalAttributes.SetValue("Mean", 0)
                            Dim lNewSimTSerMonthCalculatedAttributes As New atcDataAttributes
                            If lTsMonthly IsNot Nothing Then
                                lSeasons.SetSeasonalAttributes(lTsMonthly, lSeasonalAttributes, lNewSimTSerMonthCalculatedAttributes)
                            End If
                            lRow = lLand_Constituent_Monthly_Table.NewRow

                            lRow("OpTypeNumber") = LocationName
                            lRow("OpDesc") = lOperation.Description
                            'row("Unit") = lUnits

                            For Each key As String In lNewSimTSerMonthCalculatedAttributes.ValuesSortedByName.Keys
                                lRow(key) = HspfTable.NumFmtRE(lNewSimTSerMonthCalculatedAttributes.GetDefinedValue(key).Value, 10)
                            Next
                            lRow("SumAnnual") = lTS.Attributes.GetDefinedValue("SumAnnual").Value
                            lLand_Constituent_Monthly_Table.Rows.Add(lRow)
                        Else
                            lTS = aBinaryData.DataSets.FindData("Location", LocationName).FindData("Constituent", lOutflowDataType)(0)
                            If lTS Is Nothing Then Continue For
                            lTS = SubsetByDate(lTS, aSDateJ, aEDateJ, Nothing)

                            For Each lConnection As HspfConnection In lOperation.Targets
                                If lConnection.Target.VolName = "RCHRES" Then
                                    lOperationIsConnected = True
                                    Dim aReach As HspfOperation = aUCI.OpnBlks("RCHRES").OperFromID(lConnection.Target.VolId)
                                    If ConstituentsThatNeedMassLink.Contains(lOutflowDataType) Then
                                        Dim lMassLinkID As Integer = lConnection.MassLink
                                        If Not lMassLinkID = 0 Then
                                            lMasslinkFactor = FindMassLinkFactor(aUCI, lMassLinkID, lOutflowDataType,
                                                                             aBalanceType, 0, 0)

                                            Exit For
                                        End If
                                    End If
                                End If
                            Next lConnection
                            If Not lOperationIsConnected AndAlso ConstituentsThatNeedMassLink.Contains(lOutflowDataType) Then Exit For

                        End If
                        lTS *= lMasslinkFactor
                        Dim lTSAttributes As String = lTS.Attributes.GetDefinedValue("Constituent").Value

                        If (lTSAttributes = "SURO" Or lTSAttributes = "IFWO" Or lTSAttributes = "AGWO") Then
                            If lUnits = "in" Then lTS *= 12
                            If lTotalTS.Dates Is Nothing Then
                                lTotalTS = lTS + 0
                            Else
                                lTotalTS += lTS
                            End If
                            lTotalTS.Attributes.SetValue("Constituent", "TotalOutflow")
                        End If

                        Dim lTsYearly As atcTimeseries = Aggregate(lTS, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv)
                        Dim lSumAnnual As Double = lTsYearly.Attributes.GetDefinedValue("SumAnnual").Value

                        If lTSAttributes = "IMPEV" Then lTSAttributes = "TAET" 'Come back to this

                        If lTSNumber > 0 Then RowNumber -= (lTsYearly.numValues + 1)

                        For i As Integer = 1 To lTsYearly.numValues + 1

                            lRow = pLand_Constituent_Table.NewRow
                            Dim lDate(5) As Integer
                            Dim lYear As String = ""
                            Dim lValue As Double = 0
                            If i > lTsYearly.numValues Then
                                lYear = "SumAnnual"
                                lValue = HspfTable.NumFmtRE(lSumAnnual, 10)

                            Else
                                J2Date(lTsYearly.Dates.Values(i), lDate)
                                lYear = CStr(lDate(0))
                                lValue = HspfTable.NumFmtRE(lTsYearly.Value(i), 10)
                            End If
                            RowNumber += 1
                            If lTSNumber = 0 Then
                                lRow("OpTypeNumber") = LocationName
                                lRow("OpDesc") = lOperation.Description
                                lRow("Year") = lYear
                                lRow("SUPY") = lValue
                                pLand_Constituent_Table.Rows.Add(lRow)
                            Else
                                pLand_Constituent_Table.Rows(RowNumber - 1)(lTSAttributes) = HspfTable.NumFmtRE(lValue, 10)
                            End If

                        Next i
                        lTSNumber += 1
                    Next lOutflowDataType

                Next lOperation
#End Region

#Region "Case DO, Heat"
            Case "DO", "Heat"
                If aUCI.GlobalBlock.EmFg = 1 AndAlso aBalanceType = "DO" Then
                    lUnits = "lbs/ac"
                ElseIf aUCI.GlobalBlock.EmFg = 2 AndAlso aBalanceType = "DO" Then
                    lUnits = "kgs/ha"
                ElseIf aUCI.GlobalBlock.EmFg = 1 AndAlso aBalanceType = "Heat" Then
                    lUnits = "BTU/ac"
                ElseIf aUCI.GlobalBlock.EmFg = 2 AndAlso aBalanceType = "Heat" Then
                    lUnits = "kcal/ha"
                End If
                lDataForBoxWhiskerPlot.Units = (lUnits & "/yr")

                Dim lColumn As DataColumn
                Dim lRow As DataRow
                lLand_Constituent_Monthly_Table = AddMonthlyColumnsColumns(lLand_Constituent_Monthly_Table)
                lColumn = New DataColumn()
                lColumn.DataType = Type.GetType("System.Double")
                lColumn.ColumnName = "SO"
                lColumn.Caption = "Surface Outflow (" & lUnits & ")"
                pLand_Constituent_Table.Columns.Add(lColumn)

                lColumn = New DataColumn()
                lColumn.DataType = Type.GetType("System.Double")
                lColumn.ColumnName = "IO"
                lColumn.Caption = "Interflow Outflow"
                pLand_Constituent_Table.Columns.Add(lColumn)

                lColumn = New DataColumn()
                lColumn.DataType = Type.GetType("System.Double")
                lColumn.ColumnName = "AO"
                lColumn.Caption = "Groundwater Outflow (" & lUnits & ")"
                pLand_Constituent_Table.Columns.Add(lColumn)

                lColumn = New DataColumn()
                lColumn.DataType = Type.GetType("System.Double")
                lColumn.ColumnName = "TotalOutflow"
                lColumn.Caption = "Total Outflow (" & lUnits & ")"
                pLand_Constituent_Table.Columns.Add(lColumn)
                Dim RowNumber As Integer = 0

                For Each lOperation As HspfOperation In aUCI.OpnSeqBlock.Opns
                    If Not (lOperation.Name = "PERLND" OrElse lOperation.Name = "IMPLND") Then Continue For
                    Dim LocationName As String = lOperation.Name.Substring(0, 1) & ":" & lOperation.Id
                    lLandUseNameForTheCollection = lOperation.Name.Substring(0, 1) & ":" & lOperation.Description
                    Logger.Status("Generating Land Loading Reports for " & aBalanceType & " from " & LocationName)
                    If Not lListLanduses.Contains(lLandUseNameForTheCollection) Then
                        lListLanduses.Add(lLandUseNameForTheCollection)
                    End If
                    Dim lOperationIsConnected As Boolean = False
                    Dim lTSNumber As Integer = 0
                    Dim lOutflowDataTypes1 As Dictionary(Of String, String) = ConstituentList(aBalanceType)
                    Dim lTS As New atcTimeseries(Nothing)
                    Dim lAddTS As New atcDataGroup
                    Dim lTotalTS As New atcTimeseries(Nothing)
                    For Each lOutflowDataType As String In lOutflowDataTypes1.Keys
                        Dim lMasslinkFactor As Double = 1.0
                        If lOutflowDataType = "TotalOutflow" Then
                            lTS = lTotalTS
                            If lTS.numValues = 0 Then Continue For
                            Dim lTsMonthly As atcTimeseries = Aggregate(lTS, atcTimeUnit.TUMonth, 1, atcTran.TranSumDiv)
                            Dim lSeasons As New atcSeasonsMonth
                            Dim lSeasonalAttributes As New atcDataAttributes
                            lSeasonalAttributes.SetValue("Mean", 0)
                            Dim lNewSimTSerMonthCalculatedAttributes As New atcDataAttributes
                            If lTsMonthly IsNot Nothing Then
                                lSeasons.SetSeasonalAttributes(lTsMonthly, lSeasonalAttributes, lNewSimTSerMonthCalculatedAttributes)
                            End If
                            lRow = lLand_Constituent_Monthly_Table.NewRow

                            lRow("OpTypeNumber") = LocationName
                            lRow("OpDesc") = lOperation.Description
                            'row("Unit") = lUnits

                            For Each key As String In lNewSimTSerMonthCalculatedAttributes.ValuesSortedByName.Keys
                                lRow(key) = HspfTable.NumFmtRE(lNewSimTSerMonthCalculatedAttributes.GetDefinedValue(key).Value, 10)
                            Next
                            lRow("SumAnnual") = lTS.Attributes.GetDefinedValue("SumAnnual").Value
                            lLand_Constituent_Monthly_Table.Rows.Add(lRow)
                        Else
                            lTS = aBinaryData.DataSets.FindData("Location", LocationName).FindData("Constituent", lOutflowDataType)(0)
                            If lTS Is Nothing Then Continue For
                            lTS = SubsetByDate(lTS, aSDateJ, aEDateJ, Nothing)
                            For Each lConnection As HspfConnection In lOperation.Targets
                                If lConnection.Target.VolName = "RCHRES" Then
                                    lOperationIsConnected = True
                                    Dim aReach As HspfOperation = aUCI.OpnBlks("RCHRES").OperFromID(lConnection.Target.VolId)
                                    If ConstituentsThatNeedMassLink.Contains(lOutflowDataType) Then
                                        Dim lMassLinkID As Integer = lConnection.MassLink
                                        If Not lMassLinkID = 0 Then
                                            lMasslinkFactor = FindMassLinkFactor(aUCI, lMassLinkID, lOutflowDataType,
                                                                             aBalanceType, 0, 0)
                                            Exit For
                                        End If
                                    End If
                                End If
                            Next lConnection
                            If Not lOperationIsConnected AndAlso ConstituentsThatNeedMassLink.Contains(lOutflowDataType) Then Exit For
                        End If
                        Dim lTSAttributes As String = lTS.Attributes.GetDefinedValue("Constituent").Value
                        lTSAttributes = SafeSubstring(lTSAttributes, 0, 2)
                        If lTSAttributes = "SO" OrElse lTSAttributes = "IO" OrElse lTSAttributes = "AO" Then
                            If lTotalTS.Dates Is Nothing Then
                                lTotalTS = lTS + 0
                            Else
                                lTotalTS += lTS
                            End If
                            lTotalTS.Attributes.SetValue("Constituent", "TotalOutflow")

                        ElseIf lTSAttributes = "To" Then
                            lTSAttributes = "TotalOutflow"
                        End If

                        Dim lTsYearly As atcTimeseries = Aggregate(lTS, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv)
                        Dim lSumAnnual As Double = lTsYearly.Attributes.GetDefinedValue("SumAnnual").Value

                        If lTSNumber > 0 Then RowNumber -= (lTsYearly.numValues + 1)

                        For i As Integer = 1 To lTsYearly.numValues + 1

                            lRow = pLand_Constituent_Table.NewRow
                            Dim lDate(5) As Integer
                            Dim Year As String = ""
                            Dim lValue As Double = 0
                            If i > lTsYearly.numValues Then
                                Year = "SumAnnual"
                                lValue = HspfTable.NumFmtRE(lSumAnnual, 10)

                            Else
                                J2Date(lTsYearly.Dates.Values(i), lDate)
                                Year = CStr(lDate(0))
                                lValue = HspfTable.NumFmtRE(lTsYearly.Value(i), 10)
                            End If
                            RowNumber += 1
                            If lTSNumber = 0 Then
                                lRow("OpTypeNumber") = LocationName
                                lRow("OpDesc") = lOperation.Description
                                lRow("Year") = Year
                                lRow("SO") = lValue
                                pLand_Constituent_Table.Rows.Add(lRow)
                            Else
                                pLand_Constituent_Table.Rows(RowNumber - 1)(lTSAttributes) = HspfTable.NumFmtRE(lValue, 10)
                            End If
                        Next i
                        lTSNumber += 1
                    Next lOutflowDataType
                Next lOperation
#End Region

#Region "Case Sediment"
            Case "Sediment", "SED"
                Dim lConversionFactor As Double = 1.0
                If aUCI.GlobalBlock.EmFg = 1 Then
                    lUnits = "lbs/ac"
                    lConversionFactor = 2000 'English tons to lbs - US needs to move to SI units
                ElseIf aUCI.GlobalBlock.EmFg = 2 Then
                    lUnits = "kgs/ha"
                    lConversionFactor = 1000
                End If
                lDataForBoxWhiskerPlot.Units = (lUnits & "/yr")

                Dim lColumn As New DataColumn
                Dim lRow As DataRow
                lLand_Constituent_Monthly_Table = AddMonthlyColumnsColumns(lLand_Constituent_Monthly_Table)
                lColumn = New DataColumn()
                lColumn.DataType = Type.GetType("System.Double")
                lColumn.ColumnName = "WSSD"
                lColumn.Caption = "Wash Off of detached Sediment (" & lUnits & ")"
                pLand_Constituent_Table.Columns.Add(lColumn)

                lColumn = New DataColumn()
                lColumn.DataType = Type.GetType("System.Double")
                lColumn.ColumnName = "SCRSD"
                lColumn.Caption = "Scour of Matrix Soil (" & lUnits & ")"
                pLand_Constituent_Table.Columns.Add(lColumn)

                lColumn = New DataColumn()
                lColumn.DataType = Type.GetType("System.Double")
                lColumn.ColumnName = "TotalOutflow"
                lColumn.Caption = "Total Outflow (" & lUnits & ")"
                pLand_Constituent_Table.Columns.Add(lColumn)

                Dim lRowNumber As Integer = 0
                For Each lOperation As HspfOperation In aUCI.OpnSeqBlock.Opns
                    'If lOperation.Name = "IMPLND" Then Stop

                    If Not ((lOperation.Name = "PERLND" AndAlso lOperation.Tables("ACTIVITY").Parms("SEDFG").Value = "1") OrElse
                            (lOperation.Name = "IMPLND" AndAlso lOperation.Tables("ACTIVITY").Parms("SLDFG").Value = "1")) Then Continue For
                    'If lOperation.Name = "IMPLND" Then Stop
                    Dim lLocationName As String = lOperation.Name.Substring(0, 1) & ":" & lOperation.Id
                    lLandUseNameForTheCollection = lOperation.Name.Substring(0, 1) & ":" & lOperation.Description
                    Logger.Status("Generating Land Loading Reports for " & aBalanceType & " from " & lLocationName)
                    If Not lListLanduses.Contains(lLandUseNameForTheCollection) Then
                        lListLanduses.Add(lLandUseNameForTheCollection)
                    End If
                    Dim lOperationIsConnected As Boolean = False
                    Dim lTSNumber As Integer = 0
                    Dim lOutflowDataTypes1 As Dictionary(Of String, String) = ConstituentList(aBalanceType)
                    Dim lTS As New atcTimeseries(Nothing)
                    Dim lAddTS As New atcDataGroup
                    Dim lTotalTS As New atcTimeseries(Nothing)
                    For Each lOutflowDataType As String In lOutflowDataTypes1.Keys
                        Dim lMasslinkFactor As Double = 1.0
                        If lOutflowDataType = "TotalOutflow" Then
                            lTS = lTotalTS
                            If lTS.numValues = 0 Then Continue For
                            Dim lTsMonthly As atcTimeseries = Aggregate(lTS, atcTimeUnit.TUMonth, 1, atcTran.TranSumDiv)
                            Dim lSeasons As New atcSeasonsMonth
                            Dim lSeasonalAttributes As New atcDataAttributes
                            lSeasonalAttributes.SetValue("Mean", 0)
                            Dim lNewSimTSerMonthCalculatedAttributes As New atcDataAttributes
                            If lTsMonthly IsNot Nothing Then
                                lTsMonthly *= lConversionFactor
                                lSeasons.SetSeasonalAttributes(lTsMonthly, lSeasonalAttributes, lNewSimTSerMonthCalculatedAttributes)
                            End If
                            lRow = lLand_Constituent_Monthly_Table.NewRow

                            lRow("OpTypeNumber") = lLocationName
                            lRow("OpDesc") = lOperation.Description
                            'row("Unit") = lUnits

                            For Each key As String In lNewSimTSerMonthCalculatedAttributes.ValuesSortedByName.Keys
                                lRow(key) = HspfTable.NumFmtRE(lNewSimTSerMonthCalculatedAttributes.GetDefinedValue(key).Value, 10)
                            Next
                            lRow("SumAnnual") = lTS.Attributes.GetDefinedValue("SumAnnual").Value
                            lLand_Constituent_Monthly_Table.Rows.Add(lRow)
                        Else
                            lTS = aBinaryData.DataSets.FindData("Location", lLocationName).FindData("Constituent", lOutflowDataType)(0)
                            If lTS Is Nothing Then Continue For
                            lTS = SubsetByDate(lTS, aSDateJ, aEDateJ, Nothing)
                            For Each lConnection As HspfConnection In lOperation.Targets
                                If lConnection.Target.VolName = "RCHRES" Or lConnection.Target.VolName = "BMPRAC" Then
                                    lOperationIsConnected = True
                                    Dim aReach As HspfOperation = aUCI.OpnBlks(lConnection.Target.VolName).OperFromID(lConnection.Target.VolId)
                                    If ConstituentsThatNeedMassLink.Contains(lOutflowDataType) Then
                                        Dim lMassLinkID As Integer = lConnection.MassLink
                                        If Not lMassLinkID = 0 Then
                                            lMasslinkFactor = FindMassLinkFactor(aUCI, lMassLinkID, lOutflowDataType,
                                                                             aBalanceType, 0, 0)
                                            Exit For
                                        End If
                                    End If
                                End If
                            Next lConnection
                            If Not lOperationIsConnected AndAlso ConstituentsThatNeedMassLink.Contains(lOutflowDataType) Then Exit For
                        End If

                        'Logger.Dbg(lTS.Attributes.GetDefinedValue("Constituent").Value)
                        Dim lTSAttributes As String = lTS.Attributes.GetDefinedValue("Constituent").Value
                        If lTSAttributes = "WSSD" OrElse lTSAttributes = "SCRSD" OrElse lTSAttributes = "SOSLD" Then
                            If lTotalTS.Dates Is Nothing Then
                                lTotalTS = lTS + 0
                            Else
                                lTotalTS += lTS
                            End If
                            lTotalTS.Attributes.SetValue("Constituent", "TotalOutflow")
                        End If
                        lTS *= lConversionFactor
                        Dim lTsYearly As atcTimeseries = Aggregate(lTS, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv)
                        Dim lSumAnnual As Double = lTsYearly.Attributes.GetDefinedValue("SumAnnual").Value

                        If lTSNumber > 0 Then lRowNumber -= (lTsYearly.numValues + 1)
                        If lTSAttributes = "SOSLD" Then lTSAttributes = "WSSD"
                        For i As Integer = 1 To lTsYearly.numValues + 1
                            lRow = pLand_Constituent_Table.NewRow
                            Dim lDate(5) As Integer
                            Dim lYear As String = ""
                            Dim lValue As Double = 0
                            If i > lTsYearly.numValues Then
                                lYear = "SumAnnual"
                                lValue = HspfTable.NumFmtRE(lSumAnnual, 10)
                            Else
                                J2Date(lTsYearly.Dates.Values(i), lDate)
                                lYear = CStr(lDate(0))
                                lValue = HspfTable.NumFmtRE(lTsYearly.Value(i), 10)
                            End If
                            lRowNumber += 1
                            If lTSNumber = 0 Then
                                lRow("OpTypeNumber") = lLocationName
                                lRow("OpDesc") = lOperation.Description
                                lRow("Year") = lYear
                                lRow("WSSD") = lValue
                                pLand_Constituent_Table.Rows.Add(lRow)
                            Else
                                pLand_Constituent_Table.Rows(lRowNumber - 1)(lTSAttributes) = HspfTable.NumFmtRE(lValue, 10)
                            End If
                        Next i
                        lTSNumber += 1
                    Next lOutflowDataType
                Next lOperation
#End Region

#Region "Case Else"
            Case Else

                Dim lColumn As DataColumn
                Dim lRow As DataRow

                lColumn = New DataColumn()
                lColumn.DataType = Type.GetType("System.String")
                lColumn.ColumnName = "ConstName"
                lColumn.Caption = "Constituent Name"
                pLand_Constituent_Table.Columns.Add(lColumn)

                lColumn = New DataColumn()
                lColumn.DataType = Type.GetType("System.String")
                lColumn.ColumnName = "ConstName"
                lColumn.Caption = "Constituent Name"
                lLand_Constituent_Monthly_Table.Columns.Add(lColumn)

                lColumn = New DataColumn()
                lColumn.DataType = Type.GetType("System.String")
                lColumn.ColumnName = "ConstNameEXP"
                lColumn.Caption = "Constituent Name in EXP+"
                pLand_Constituent_Table.Columns.Add(lColumn)

                lColumn = New DataColumn()
                lColumn.DataType = Type.GetType("System.String")
                lColumn.ColumnName = "ConstNameEXP"
                lColumn.Caption = "Constituent Name in EXP+"
                lLand_Constituent_Monthly_Table.Columns.Add(lColumn)

                lColumn = New DataColumn()
                lColumn.DataType = Type.GetType("System.String")
                lColumn.ColumnName = "Unit"
                lColumn.Caption = "Unit"
                pLand_Constituent_Table.Columns.Add(lColumn)

                lColumn = New DataColumn()
                lColumn.DataType = Type.GetType("System.String")
                lColumn.ColumnName = "Unit"
                lColumn.Caption = "Unit"
                lLand_Constituent_Monthly_Table.Columns.Add(lColumn)

                lLand_Constituent_Monthly_Table = AddMonthlyColumnsColumns(lLand_Constituent_Monthly_Table)

                lColumn = New DataColumn()
                lColumn.DataType = Type.GetType("System.Double")
                lColumn.ColumnName = "WASHQS"
                lColumn.Caption = "Removal of QUALSD by association with detached sediment Runoff"
                pLand_Constituent_Table.Columns.Add(lColumn)

                lColumn = New DataColumn()
                lColumn.DataType = Type.GetType("System.Double")
                lColumn.ColumnName = "SCRQS"
                lColumn.Caption = "Removal of QUALSD with scour of matrix soil"
                pLand_Constituent_Table.Columns.Add(lColumn)

                lColumn = New DataColumn()
                lColumn.DataType = Type.GetType("System.Double")
                lColumn.ColumnName = "SOQO"
                lColumn.Caption = "Washoff of QUALOF from surface"
                pLand_Constituent_Table.Columns.Add(lColumn)

                lColumn = New DataColumn()
                lColumn.DataType = Type.GetType("System.Double")
                lColumn.ColumnName = "IOQUAL"
                lColumn.Caption = "Outflow of QUAL in interflow"
                pLand_Constituent_Table.Columns.Add(lColumn)

                lColumn = New DataColumn()
                lColumn.DataType = Type.GetType("System.Double")
                lColumn.ColumnName = "AOQUAL"
                lColumn.Caption = "Outflow of QUAL in Groundwater flow"
                pLand_Constituent_Table.Columns.Add(lColumn)

                lColumn = New DataColumn()
                lColumn.DataType = Type.GetType("System.Double")
                lColumn.ColumnName = "TotalOutflow"
                lColumn.Caption = "Total Outflow"
                pLand_Constituent_Table.Columns.Add(lColumn)
                Dim lRowNumber As Integer = 0
                Dim lConstituentNames As New List(Of String)
                Dim lOperationNameNumber As New List(Of String)
                Dim lYears As New List(Of String)
                For Each lOperation As HspfOperation In aUCI.OpnSeqBlock.Opns
                    If Not ((lOperation.Name = "PERLND" AndAlso lOperation.Tables("ACTIVITY").Parms("PQALFG").Value = "1") OrElse
                        (lOperation.Name = "IMPLND" AndAlso lOperation.Tables("ACTIVITY").Parms("IQALFG").Value = "1")) Then Continue For
                    'If lOperation.Name = "IMPLND" Then Stop
                    Dim lLocationName As String = lOperation.Name.Substring(0, 1) & ":" & lOperation.Id
                    Logger.Status("Generating Land Loading Reports for " & aBalanceType & " from " & lLocationName)
                    Dim lOperationIsConnected As Boolean = False
                    'If lOperation.Tables("ACTIVITY").Parms("PQUALFG").Value = "0" Then Continue For

                    lOperationNameNumber.Add(lLocationName)

                    lLandUseNameForTheCollection = lOperation.Name.Substring(0, 1) & ":" & lOperation.Description

                    If Not lListLanduses.Contains(lLandUseNameForTheCollection) Then
                        lListLanduses.Add(lLandUseNameForTheCollection)
                    End If
                    'If lOperation.Id = 319 Then Stop
                    For Each lConstituent As ConstituentProperties In aConstProperties
                        Dim lMultipleIndex As Integer = 0
                        If lConstituent.ConstNameForEXPPlus.ToLower.Contains("ref") Then
                            lMultipleIndex = 1
                        ElseIf lConstituent.ConstNameForEXPPlus.ToLower.Contains("lab") Then
                            lMultipleIndex = 2
                        End If

                        If Not lConstituentNames.Contains(lConstituent.ConstNameForEXPPlus) Then
                            lConstituentNames.Add(lConstituent.ConstNameForEXPPlus)
                        End If

                        Dim lOutflowDataTypes1 As Dictionary(Of String, String) = ConstituentList(aBalanceType, lConstituent.ConstituentNameInUCI,
                                                                                                  lConstituent.ConstNameForEXPPlus, False, lOperation.Name)
                        Dim lTSNumber As Integer = 0
                        Dim lTS As New atcTimeseries(Nothing)
                        Dim lAddTS As New atcDataGroup
                        Dim lTotalTS As New atcTimeseries(Nothing)
                        For Each lOutflowDataType As String In lOutflowDataTypes1.Keys
                            Dim lMassLinkFactor As Double = 1.0
                            If lOutflowDataType.StartsWith("TotalOutflow") And lTotalTS.Dates IsNot Nothing Then
                                lTS = lTotalTS
                                If lTS.numValues = 0 Then Continue For
                                'Start doing the montly calculations here.
                                Dim lTsMonthly As atcTimeseries = Aggregate(lTS, atcTimeUnit.TUMonth, 1, atcTran.TranSumDiv)
                                Dim lSeasons As New atcSeasonsMonth
                                Dim lSeasonalAttributes As New atcDataAttributes
                                lSeasonalAttributes.SetValue("Mean", 0)
                                Dim lNewSimTSerMonthCalculatedAttributes As New atcDataAttributes
                                If lTsMonthly IsNot Nothing Then
                                    lSeasons.SetSeasonalAttributes(lTsMonthly, lSeasonalAttributes, lNewSimTSerMonthCalculatedAttributes)
                                End If
                                lRow = lLand_Constituent_Monthly_Table.NewRow

                                lRow("OpTypeNumber") = lLocationName
                                lRow("OpDesc") = lOperation.Description
                                lRow("ConstName") = lConstituent.ConstituentNameInUCI
                                lRow("ConstNameEXP") = lConstituent.ConstNameForEXPPlus
                                lRow("Unit") = lConstituent.ConstituentUnit

                                For Each key As String In lNewSimTSerMonthCalculatedAttributes.ValuesSortedByName.Keys
                                    lRow(key) = HspfTable.NumFmtRE(lNewSimTSerMonthCalculatedAttributes.GetDefinedValue(key).Value, 10)
                                Next
                                lRow("SumAnnual") = lTS.Attributes.GetDefinedValue("SumAnnual").Value
                                lLand_Constituent_Monthly_Table.Rows.Add(lRow)
                            Else
                                lTS = aBinaryData.DataSets.FindData("Location", lLocationName).FindData("Constituent", lOutflowDataTypes1(lOutflowDataType))(0)

                                If lTS Is Nothing Then Continue For
                                If lTS.Attributes.GetDefinedValue("Sum").Value = 0 Then Continue For
                                lTS = SubsetByDate(lTS, aSDateJ, aEDateJ, Nothing)
                                For Each lConnection As HspfConnection In lOperation.Targets
                                    If lConnection.Target.VolName = "RCHRES" Then
                                        Dim lReach As HspfOperation = aUCI.OpnBlks("RCHRES").OperFromID(lConnection.Target.VolId)
                                        If lReach IsNot Nothing Then
                                            lOperationIsConnected = True

                                            Dim aConversionFactor As Double = 0.0
                                            If aBalanceType = "TN" Or aBalanceType = "TP" Then
                                                aConversionFactor = ConversionFactorfromOxygen(aUCI, lConstituent.ReportType, lReach)
                                            End If
                                            Dim lMassLinkID As Integer = lConnection.MassLink
                                            If Not lMassLinkID = 0 Then
                                                lMassLinkFactor = FindMassLinkFactor(aUCI, lMassLinkID, lOutflowDataType,
                                                                             lConstituent.ReportType, aConversionFactor, lMultipleIndex, aGQALID)
                                                Exit For
                                            End If
                                        End If
                                    End If
                                Next lConnection
                                If Not lOperationIsConnected Then Exit For
                            End If

                            'If Not lOutflowDataType.StartsWith("TotalOutflow") Then 
                            lTS *= lMassLinkFactor
                            If lTotalTS.Dates Is Nothing Then
                                lTotalTS = lTS + 0
                            Else
                                lTotalTS += lTS
                            End If
                            lTotalTS.Attributes.SetValue("Constituent", "TotalOutflow")

                            Dim lTSAttributes As String = lTS.Attributes.GetDefinedValue("Constituent").Value
                            Dim lTsYearly As atcTimeseries = Aggregate(lTS, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv)
                            Dim lSumAnnual As Double = lTsYearly.Attributes.GetDefinedValue("SumAnnual").Value

                            If lTSNumber > 0 Then lRowNumber -= (lTsYearly.numValues + 1)

                            For i As Integer = 1 To lTsYearly.numValues + 1

                                lRow = pLand_Constituent_Table.NewRow
                                Dim lDate(5) As Integer
                                Dim lYear As String = ""
                                Dim lValue As Double = 0
                                If i > lTsYearly.numValues Then
                                    lYear = "SumAnnual"
                                    lValue = HspfTable.NumFmtRE(lSumAnnual, 10)

                                Else
                                    J2Date(lTsYearly.Dates.Values(i), lDate)
                                    lYear = CStr(lDate(0))
                                    lValue = HspfTable.NumFmtRE(lTsYearly.Value(i), 10)
                                End If
                                If Not lYears.Contains(lYear) Then
                                    lYears.Add(lYear)
                                End If
                                lRowNumber += 1

                                If lTSNumber = 0 Then
                                    lRow("OpTypeNumber") = lLocationName
                                    lRow("OpDesc") = lOperation.Description
                                    lRow("Year") = lYear
                                    lRow("ConstName") = lConstituent.ConstituentNameInUCI
                                    lRow("ConstNameEXP") = lConstituent.ConstNameForEXPPlus
                                    lRow("Unit") = lConstituent.ConstituentUnit
                                    'row("WASHQS") = lValue
                                    If lTSAttributes.Split("-")(0) = "SOQS" Then 'For IMPLND, WASHQS is not a constituent, but Anurag wants to put this value in that column
                                        lRow("WASHQS") = lValue
                                    Else
                                        lRow(lTSAttributes.Split("-")(0)) = lValue
                                    End If
                                    pLand_Constituent_Table.Rows.Add(lRow)
                                Else
                                    pLand_Constituent_Table.Rows(lRowNumber - 1)(lTSAttributes.Split("-")(0)) = HspfTable.NumFmtRE(lValue, 10)
                                End If
                            Next i
                            lTSNumber += 1
                        Next lOutflowDataType
                    Next lConstituent

                Next lOperation
#End Region
                If aConstProperties.Count > 1 Then
                    For Each lOperation As String In lOperationNameNumber 'Summing constituents of TN and TP
                        For Each lYear As String In lYears
                            Dim lSelectExpression As String = "OpTypeNumber = '" & lOperation & "' And Year = '" & lYear & "'"
                            Dim lFoundRows() As DataRow = pLand_Constituent_Table.Select(lSelectExpression)
                            If lFoundRows.Length = 0 Then Continue For
                            lRow = pLand_Constituent_Table.NewRow
                            'Logger.Dbg(SelectExpression)
                            lRow("OpTypeNumber") = lFoundRows(0)("OpTypeNumber")
                            lRow("OpDesc") = lFoundRows(0)("OpDesc")
                            lRow("Year") = lFoundRows(0)("Year")
                            lRow("ConstName") = aBalanceType
                            lRow("ConstNameEXP") = aBalanceType
                            lRow("Unit") = lFoundRows(0)("Unit")

                            For Each lFoundrow As DataRow In lFoundRows
                                For i As Integer = 6 To lFoundrow.ItemArray.Length - 1
                                    If IsDBNull(lRow(i)) AndAlso Not IsDBNull(lFoundrow(i)) Then
                                        lRow(i) = lFoundrow(i)

                                    ElseIf Not IsDBNull(lRow(i)) AndAlso Not IsDBNull(lFoundrow(i)) Then
                                        lRow(i) += lFoundrow(i)
                                        'ElseIf IsDBNull(foundrow(i) AndAlso Not IsDBNull(row(i))) Then

                                    End If
                                Next i
                            Next lFoundrow
                            pLand_Constituent_Table.Rows.Add(lRow)
                        Next lYear

                        Dim lSelectExpressionMonthly As String = "OpTypeNumber = '" & lOperation & "'"
                        Dim lFoundRowsMonthly() As DataRow = lLand_Constituent_Monthly_Table.Select(lSelectExpressionMonthly)
                        If lFoundRowsMonthly.Length = 0 Then Continue For
                        lRow = lLand_Constituent_Monthly_Table.NewRow

                        lRow("OpTypeNumber") = lFoundRowsMonthly(0)("OpTypeNumber")
                        lRow("OpDesc") = lFoundRowsMonthly(0)("OpDesc")
                        lRow("ConstName") = aBalanceType
                        lRow("ConstNameEXP") = aBalanceType
                        lRow("Unit") = lFoundRowsMonthly(0)("Unit")

                        For Each lFoundRow As DataRow In lFoundRowsMonthly
                            For i As Integer = 5 To lFoundRow.ItemArray.Length - 1
                                If IsDBNull(lRow(i)) AndAlso Not IsDBNull(lFoundRow(i)) Then
                                    lRow(i) = lFoundRow(i)

                                ElseIf Not IsDBNull(lRow(i)) AndAlso Not IsDBNull(lFoundRow(i)) Then
                                    lRow(i) += lFoundRow(i)
                                    'ElseIf IsDBNull(foundrow(i) AndAlso Not IsDBNull(row(i))) Then

                                End If
                            Next i
                        Next lFoundRow
                        lLand_Constituent_Monthly_Table.Rows.Add(lRow)
                    Next lOperation
                End If


        End Select

        Dim lTextToWrite As String = ""
        For Each TableColumn As DataColumn In pLand_Constituent_Table.Columns 'Writing the table headings
            lTextToWrite &= TableColumn.Caption & vbTab
        Next
        lReport.AppendLine(lTextToWrite)
        For Each TableRow As DataRow In pLand_Constituent_Table.Rows 'Writing the table contents
            lTextToWrite = ""
            For Each TableColumn As DataColumn In pLand_Constituent_Table.Columns
                lTextToWrite &= TableRow(TableColumn) & vbTab
            Next TableColumn
            lReport.AppendLine(lTextToWrite)
        Next TableRow
        lReport.AppendLine()
        lReport.AppendLine("Tabular Report of Land Loading of all the Land Operations.")
        lReport.AppendLine(aUCI.GlobalBlock.Caption)
        lReport.AppendLine("Run Made " & aRunMade)
        lReport.AppendLine(TimeSpanAsString(aUCI.GlobalBlock.SDateJ, aUCI.GlobalBlock.EdateJ, "Analysis Period: "))
        SaveFileString(aoutfoldername & aBalanceType & "_Land_Loadings.txt", lReport.ToString)

        lTextToWrite = ""
        For Each lTableColumn As DataColumn In lLand_Constituent_Monthly_Table.Columns 'Writing the table headings
            lTextToWrite &= lTableColumn.Caption & vbTab
        Next
        lReport_Monthly.AppendLine(lTextToWrite)
        For Each lTableRow As DataRow In lLand_Constituent_Monthly_Table.Rows 'Writing the table contents
            lTextToWrite = ""
            For Each TableColumn As DataColumn In lLand_Constituent_Monthly_Table.Columns
                lTextToWrite &= lTableRow(TableColumn) & vbTab
            Next TableColumn
            lReport_Monthly.AppendLine(lTextToWrite)
        Next lTableRow
        lReport_Monthly.AppendLine()
        lReport_Monthly.AppendLine("Tabular Report of Monthly Land Loading of all the Land Operations.")
        lReport_Monthly.AppendLine("   " & aUCI.GlobalBlock.RunInf.Value)
        lReport_Monthly.AppendLine("   Run Made " & aRunMade)
        lReport_Monthly.AppendLine("   " & TimeSpanAsString(aSDateJ, aEDateJ, "Analysis Period: "))
        SaveFileString(aoutfoldername & "\MonthlyLoadings\" & aBalanceType & "_Monthly_Land_Loadings.txt", lReport_Monthly.ToString)

        Dim lMonthNames() As String = {"Mean Month 01 Jan", "Mean Month 02 Feb", "Mean Month 03 Mar", "Mean Month 04 Apr",
                                           "Mean Month 05 May", "Mean Month 06 Jun", "Mean Month 07 Jul", "Mean Month 08 Aug",
                                           "Mean Month 09 Sep", "Mean Month 10 Oct", "Mean Month 11 Nov", "Mean Month 12 Dec"}
        If Not aConstProperties.Count = 0 Then
            For Each lConstituent As ConstituentProperties In aConstProperties
                lDataForBoxWhiskerPlot.Constituent = lConstituent.ConstNameForEXPPlus
                For Each lItem As String In lListLanduses
                    Dim lOpType1 As String = lItem.Split("-")(0)
                    Dim lSelectExpression As String = "OpTypeNumber Like '" & lItem.Split(":")(0) & "%' And Year = 'SumAnnual' And OpDesc ='" & lItem.Split(":")(1) & "' And ConstNameEXP = '" & lConstituent.ConstNameForEXPPlus & "'"
                    Dim lFoundRows() As DataRow = pLand_Constituent_Table.Select(lSelectExpression)
                    Dim lValues As New List(Of Double)
                    If lFoundRows.Length > 0 Then
                        For Each lFoundRow As DataRow In lFoundRows
                            Try
                                lValues.Add(lFoundRow("TotalOutflow"))
                            Catch
                            End Try
                        Next lFoundRow

                        If lValues.Count > 0 Then
                            lDataForBoxWhiskerPlot.Units = "(" & lFoundRows(0)("Unit") & "/yr)"
                            lLandUseSumAnnualValues.Add(lItem, lValues.ToArray)
                        End If
                    End If
                Next lItem
                lDataForBoxWhiskerPlot.LabelValueCollection = lLandUseSumAnnualValues
                CreateGraph_BoxAndWhisker(lDataForBoxWhiskerPlot, aoutfoldername & lConstituent.ConstNameForEXPPlus & "_BoxWhisker.png")
                lLandUseSumAnnualValues.Clear()
            Next

            For Each lConstituent As ConstituentProperties In aConstProperties
                lDataForBoxWhiskerPlot.Constituent = lConstituent.ConstNameForEXPPlus
                For Each lItem As String In lListLanduses
                    Dim lOpType1 As String = lItem.Split("-")(0)
                    Dim lSelectExpression As String = "OpTypeNumber Like '" & lItem.Split(":")(0) & "%' And OpDesc ='" & lItem.Split(":")(1) & "' And ConstNameEXP = '" & lConstituent.ConstNameForEXPPlus & "'"
                    Dim lFoundRows() As DataRow = lLand_Constituent_Monthly_Table.Select(lSelectExpression)
                    If lFoundRows.Length > 0 Then
                        For Each lMonth As String In lMonthNames
                            Dim lValues As New List(Of Double)
                            For Each MonthRow As DataRow In lFoundRows
                                Try
                                    lValues.Add(MonthRow(lMonth))
                                Catch
                                End Try
                            Next
                            If lValues.Count > 0 Then
                                lLandUseSumAnnualValues.Add(Right(lMonth, 3), lValues.ToArray)
                            End If
                        Next

                        lDataForBoxWhiskerPlot.LabelValueCollection = lLandUseSumAnnualValues

                        If lLandUseSumAnnualValues.Count > 0 Then
                            lDataForBoxWhiskerPlot.Units = "(" & lFoundRows(0)("Unit") & ")"

                            CreateGraph_BoxAndWhisker(lDataForBoxWhiskerPlot, AbsolutePath(System.IO.Path.Combine("MonthlyLoadings\" & lConstituent.ConstNameForEXPPlus & "_" & lItem.Split(":")(0) & "_" & lItem.Split(":")(1) & "_BoxWhisker.png"), aoutfoldername),
                                                      "Monthly Loading Rate from Land Use " & lItem & "")
                            lLandUseSumAnnualValues.Clear()
                        End If
                    End If
                Next lItem

            Next

        Else
            lDataForBoxWhiskerPlot.Constituent = aBalanceType
            For Each lItem As String In lListLanduses
                Dim SelectExpression As String = "OpTypeNumber Like '" & lItem.Split(":")(0) & "%' And Year = 'SumAnnual' And OpDesc ='" & lItem.Split(":")(1) & "'"
                'Logger.Dbg(SelectExpression)
                Dim lFoundRows() As DataRow = pLand_Constituent_Table.Select(SelectExpression)
                Dim lValues As New List(Of Double)
                If lFoundRows.Length > 0 Then
                    For Each foundrow As DataRow In lFoundRows
                        Try
                            lValues.Add(foundrow("TotalOutflow"))
                        Catch
                        End Try
                    Next foundrow
                    If lValues.Count > 0 Then
                        lLandUseSumAnnualValues.Add(lItem, lValues.ToArray)
                    End If
                End If

            Next lItem
            If lLandUseSumAnnualValues.Count > 0 Then
                lDataForBoxWhiskerPlot.Units = "(" & lUnits & "/yr)"
                lDataForBoxWhiskerPlot.LabelValueCollection = lLandUseSumAnnualValues
                CreateGraph_BoxAndWhisker(lDataForBoxWhiskerPlot, aoutfoldername & aBalanceType & "_BoxWhisker.png")
                lLandUseSumAnnualValues.Clear()

                lDataForBoxWhiskerPlot.Constituent = aBalanceType
                For Each lItem As String In lListLanduses
                    Dim lOpType1 As String = lItem.Split("-")(0)
                    Dim lSelectExpression As String = "OpTypeNumber Like '" & lItem.Split(":")(0) & "%' And OpDesc ='" & lItem.Split(":")(1) & "'"
                    Dim lFoundRows() As DataRow = lLand_Constituent_Monthly_Table.Select(lSelectExpression)
                    If lFoundRows.Length > 0 Then
                        For Each lMonth As String In lMonthNames
                            Dim lValues As New List(Of Double)
                            For Each lMonthRow As DataRow In lFoundRows
                                Try
                                    lValues.Add(lMonthRow(lMonth))
                                Catch
                                End Try
                            Next
                            lLandUseSumAnnualValues.Add(Right(lMonth, 3), lValues.ToArray)
                        Next

                        lDataForBoxWhiskerPlot.LabelValueCollection = lLandUseSumAnnualValues
                        lDataForBoxWhiskerPlot.Units = "(" & lUnits & ")"

                        CreateGraph_BoxAndWhisker(lDataForBoxWhiskerPlot, AbsolutePath(System.IO.Path.Combine("MonthlyLoadings\" & aBalanceType & "_" & lItem.Split(":")(0) & "_" & lItem.Split(":")(1) & "_BoxWhisker.png"), aoutfoldername),
                                                  "Monthly Loading Rate from Land Use " & lItem & "")
                    End If
                    lLandUseSumAnnualValues.Clear()
                Next
            End If

        End If
        Return pLand_Constituent_Table

    End Function

    Public Sub ReachBudgetReports(ByVal aOutFolderName As String,
                                     ByVal aBinaryData As atcDataSource,
                                     ByVal aUCI As HspfUci,
                                     ByVal aScenario As String,
                                     ByVal aRunMade As String,
                                     ByVal aBalanceType As String,
                                     ByVal aConstProperties As List(Of ConstituentProperties),
                                     ByVal aSDateJ As Double, ByVal aEDateJ As Double, Optional aGQALID As Integer = 0)
        Dim lReport As New atcReport.ReportText
        Dim lUpstreamInflows As New atcCollection
        Dim lCumulativePointNonpointColl As New atcCollection

        pReach_Budget_Table = New DataTable("ReachBudgetTable")

        Dim lUnits As String = ""

        Select Case aBalanceType
            Case "Water", "Sediment"
                'not generating reach budget report for water and sediment
#Region "DO Case"
            Case "DO"

                If aUCI.GlobalBlock.EmFg = 1 Then
                    lUnits = "lbs"
                Else
                    lUnits = "kgs"
                End If
                pReach_Budget_Table = AddFirstSixColumnsReachBudget(pReach_Budget_Table, lUnits)
                Dim lRow As DataRow
                Dim lColumn As DataColumn

                lColumn = New DataColumn()
                lColumn.DataType = Type.GetType("System.Double")
                lColumn.ColumnName = "DOXIN-PREC"
                lColumn.Caption = "DO Input In Precip (" & lUnits & ")"
                pReach_Budget_Table.Columns.Add(lColumn)

                lColumn = New DataColumn()
                lColumn.DataType = Type.GetType("System.Double")
                lColumn.ColumnName = "DOXIN"
                lColumn.Caption = "Total DO Input (" & lUnits & ")"
                pReach_Budget_Table.Columns.Add(lColumn)

                lColumn = New DataColumn()
                lColumn.DataType = Type.GetType("System.Double")
                lColumn.ColumnName = "DOXFLUX-TOT"
                lColumn.Caption = "Total DO Process Flux (" & lUnits & ")"
                pReach_Budget_Table.Columns.Add(lColumn)

                lColumn = New DataColumn()
                lColumn.DataType = Type.GetType("System.Double")
                lColumn.ColumnName = "DOXFLUX-REAER"
                lColumn.Caption = "DO Reaeration (" & lUnits & ")"
                pReach_Budget_Table.Columns.Add(lColumn)

                lColumn = New DataColumn()
                lColumn.DataType = Type.GetType("System.Double")
                lColumn.ColumnName = "DOXFLUX-BODDEC"
                lColumn.Caption = "DO BOD Decay (" & lUnits & ")"
                pReach_Budget_Table.Columns.Add(lColumn)

                lColumn = New DataColumn()
                lColumn.DataType = Type.GetType("System.Double")
                lColumn.ColumnName = "DOXFLUX-BENTHAL"
                lColumn.Caption = "DO Benthal (" & lUnits & ")"
                pReach_Budget_Table.Columns.Add(lColumn)

                lColumn = New DataColumn()
                lColumn.DataType = Type.GetType("System.Double")
                lColumn.ColumnName = "DOXFLUX-NITR"
                lColumn.Caption = "DO Nitrification (" & lUnits & ")"
                pReach_Budget_Table.Columns.Add(lColumn)

                lColumn = New DataColumn()
                lColumn.DataType = Type.GetType("System.Double")
                lColumn.ColumnName = "DOXFLUX-PHYTO"
                lColumn.Caption = "DO Phytoplankton (" & lUnits & ")"
                pReach_Budget_Table.Columns.Add(lColumn)

                lColumn = New DataColumn()
                lColumn.DataType = Type.GetType("System.Double")
                lColumn.ColumnName = "DOXFLUX-BENTHIC"
                lColumn.Caption = "DO Benthic Algae (" & lUnits & ")"
                pReach_Budget_Table.Columns.Add(lColumn)

                lColumn = New DataColumn()
                lColumn.DataType = Type.GetType("System.Double")
                lColumn.ColumnName = "DOXFLUX-ZOO"
                lColumn.Caption = "DO Zooplankton (" & lUnits & ")"
                pReach_Budget_Table.Columns.Add(lColumn)

                lColumn = New DataColumn()
                lColumn.DataType = Type.GetType("System.Double")
                lColumn.ColumnName = "DOXOUTTOT"
                lColumn.Caption = "Total DO Output (" & lUnits & ")"
                pReach_Budget_Table.Columns.Add(lColumn)

                For Each lReach As HspfOperation In aUCI.OpnSeqBlock.Opns
                    lRow = pReach_Budget_Table.NewRow
                    If Not lReach.Name = "RCHRES" Then Continue For
                    If Not lReach.Tables("ACTIVITY").Parms("OXFG").Value = "1" Then Continue For
                    Dim LocationName As String = lReach.Name.Substring(0, 1) & ":" & lReach.Id
                    Logger.Status("Generating Reach Budget Report for DO from " & LocationName)
                    'Dim lOutflowDataTypes1 As String() = ConstituentListRCHRES(aBalanceType)
                    Dim lTS As New atcTimeseries(Nothing)
                    Dim AddTS As New atcDataGroup
                    Dim lTotalTS As New atcTimeseries(Nothing)
                    'If lReach.Id = 103 Then Stop
                    Dim lDownstreamReachID As Integer = lReach.DownOper("RCHRES")
                    Dim lUpstreamIn As Double = 0.0
                    If lUpstreamInflows.Keys.Contains(lReach.Id) Then
                        lUpstreamIn = lUpstreamInflows.ItemByKey(lReach.Id)
                    End If
                    Dim lNPSLoad As Double = CalculateNPSLoad(aUCI, lReach, aBalanceType)
                    Dim lPSLoad As Double = CalculatePSLoad(aUCI, lReach, aSDateJ, aEDateJ, aBalanceType)
                    Dim lOutflow As Double = SafeSumAnnual(aBinaryData, LocationName, "DOXOUTTOT", aSDateJ, aEDateJ)
                    Dim lTotalIn As Double = SafeSumAnnual(aBinaryData, LocationName, "DOXIN", aSDateJ, aEDateJ)
                    Dim lPrecIn As Double = 0
                    Try
                        lPrecIn = SafeSumAnnual(aBinaryData, LocationName, "DOXIN-PREC", aSDateJ, aEDateJ)
                        If lPrecIn < -99 Then
                            lPrecIn = 0
                        End If
                    Catch
                        Logger.Dbg("Precipitation does not contain DO in this model.")
                    End Try
                    Dim lDiversion As Double = CalculateDiversion(aUCI, aBinaryData, lReach, lUpstreamInflows, lDownstreamReachID,
                                                                  lOutflow, aBalanceType)
                    Dim lGENERLoad As Double = CalculateGENERLoad(aUCI, lReach, aBalanceType, aSDateJ, aEDateJ)
                    Dim lMassBalance As Double = lTotalIn - lNPSLoad - lUpstreamIn - lPSLoad - lGENERLoad - lPrecIn
                    For Each lColumnValue As DataColumn In pReach_Budget_Table.Columns
                        Dim lColumnName As String = lColumnValue.ColumnName
                        Select Case lColumnName
                            Case "OpTypeNumber"
                                lRow(lColumnName) = LocationName
                            Case "OpDesc"
                                lRow(lColumnName) = lReach.Description
                            Case "NPSLoad"
                                lRow(lColumnName) = HspfTable.NumFmtRE(lNPSLoad, 10)
                            Case "PSLoad"
                                lRow(lColumnName) = HspfTable.NumFmtRE(lPSLoad, 10)
                            Case "GENERLoad"
                                lRow(lColumnName) = HspfTable.NumFmtRE(lGENERLoad, 10)
                            Case "Diversion"
                                lRow(lColumnName) = HspfTable.NumFmtRE(lDiversion, 10)
                            Case "MassBalance"
                                lRow(lColumnName) = HspfTable.NumFmtRE(lMassBalance, 10)
                            Case "UpstreamIn"
                                lRow(lColumnName) = HspfTable.NumFmtRE(lUpstreamIn, 10)
                            Case Else
                                Dim lTest As atcTimeseries = aBinaryData.DataSets.FindData("Location", LocationName).FindData("Constituent", lColumnName)(0)
                                If lTest IsNot Nothing Then
                                    lRow(lColumnName) = HspfTable.NumFmtRE(SubsetByDate(lTest,
                                                                  aSDateJ, aEDateJ, Nothing).Attributes.GetDefinedValue("SumAnnual").Value, 10)
                                End If

                        End Select
                    Next lColumnValue

                    pReach_Budget_Table.Rows.Add(lRow)
                Next lReach
                Dim lTextToWrite As String = ""
                For Each TableColumn As DataColumn In pReach_Budget_Table.Columns 'Writing the table headings
                    lTextToWrite &= TableColumn.Caption & vbTab
                Next
                lReport.AppendLine(lTextToWrite)
                For Each TableRow As DataRow In pReach_Budget_Table.Rows 'Writing the table contents
                    lTextToWrite = ""
                    For Each TableColumn As DataColumn In pReach_Budget_Table.Columns
                        lTextToWrite &= TableRow(TableColumn) & vbTab
                    Next TableColumn
                    lReport.AppendLine(lTextToWrite)
                Next TableRow
                lReport.AppendLine()
                lReport.AppendLine("Tabular Report of Average Annual Reach Budget for all the Reach Operations.")
                lReport.AppendLine("   " & aUCI.GlobalBlock.RunInf.Value)
                lReport.AppendLine("   Run Made " & aRunMade)
                lReport.AppendLine("   " & TimeSpanAsString(aSDateJ, aEDateJ, "Analysis Period: "))
                SaveFileString(aOutFolderName & aBalanceType & "_Reach_Budget.txt", lReport.ToString)
#End Region
#Region "Heat Case"
            Case "Heat"
                Dim lUnits2 As String = ""
                If aUCI.GlobalBlock.EmFg = 1 Then
                    lUnits = "BTU"
                    lUnits2 = "BTU/FT2"
                Else
                    lUnits = "kcal"
                    lUnits2 = "kcal/M2"
                End If
                Dim lColumn As DataColumn
                Dim lRow As DataRow
                pReach_Budget_Table = AddFirstSixColumnsReachBudget(pReach_Budget_Table, lUnits)
                lColumn = New DataColumn()
                lColumn.DataType = Type.GetType("System.Double")
                lColumn.ColumnName = "IHEAT"
                lColumn.Caption = "Total Heat Inflow (" & lUnits & ")"
                pReach_Budget_Table.Columns.Add(lColumn)

                lColumn = New DataColumn()
                lColumn.DataType = Type.GetType("System.Double")
                lColumn.ColumnName = "HTEXCH"
                lColumn.Caption = "Atmospheric Heat Exchange (" & lUnits & ")"
                pReach_Budget_Table.Columns.Add(lColumn)

                lColumn = New DataColumn()
                lColumn.DataType = Type.GetType("System.Double")
                lColumn.ColumnName = "ROHEAT"
                lColumn.Caption = "Heat Outflow (" & lUnits & ")"
                pReach_Budget_Table.Columns.Add(lColumn)

                lColumn = New DataColumn()
                lColumn.DataType = Type.GetType("System.Double")
                lColumn.ColumnName = "QTOTAL"
                lColumn.Caption = "Total Heat Balance (" & lUnits2 & ")"
                pReach_Budget_Table.Columns.Add(lColumn)

                lColumn = New DataColumn()
                lColumn.DataType = Type.GetType("System.Double")
                lColumn.ColumnName = "QSOLAR"
                lColumn.Caption = "Solar Radiation (" & lUnits2 & ")"
                pReach_Budget_Table.Columns.Add(lColumn)

                lColumn = New DataColumn()
                lColumn.DataType = Type.GetType("System.Double")
                lColumn.ColumnName = "QLONGW"
                lColumn.Caption = "Longwave Radiation (" & lUnits2 & ")"
                pReach_Budget_Table.Columns.Add(lColumn)

                lColumn = New DataColumn()
                lColumn.DataType = Type.GetType("System.Double")
                lColumn.ColumnName = "QEVAP"
                lColumn.Caption = "Evaporation (" & lUnits2 & ")"
                pReach_Budget_Table.Columns.Add(lColumn)

                lColumn = New DataColumn()
                lColumn.DataType = Type.GetType("System.Double")
                lColumn.ColumnName = "QCON"
                lColumn.Caption = "Convection/Conduction (" & lUnits2 & ")"
                pReach_Budget_Table.Columns.Add(lColumn)

                lColumn = New DataColumn()
                lColumn.DataType = Type.GetType("System.Double")
                lColumn.ColumnName = "QPREC"
                lColumn.Caption = "Precipitation (" & lUnits2 & ")"
                pReach_Budget_Table.Columns.Add(lColumn)

                lColumn = New DataColumn()
                lColumn.DataType = Type.GetType("System.Double")
                lColumn.ColumnName = "QBED"
                lColumn.Caption = "Bed Conduction (" & lUnits2 & ")"
                pReach_Budget_Table.Columns.Add(lColumn)

                For Each lReach As HspfOperation In aUCI.OpnSeqBlock.Opns
                    lRow = pReach_Budget_Table.NewRow
                    If Not lReach.Name = "RCHRES" Then Continue For
                    If Not lReach.Tables("ACTIVITY").Parms("HTFG").Value = "1" Then Continue For
                    Dim LocationName As String = lReach.Name.Substring(0, 1) & ":" & lReach.Id
                    Logger.Status("Generating Reach Budget Report for Heat from " & LocationName)
                    'Dim lOutflowDataTypes1 As String() = ConstituentListRCHRES(aBalanceType)
                    Dim lTS As New atcTimeseries(Nothing)
                    Dim lAddTS As New atcDataGroup
                    Dim lTotalTS As New atcTimeseries(Nothing)
                    Dim lDownstreamReachID As Integer = lReach.DownOper("RCHRES")
                    Dim lUpstreamIn As Double = 0.0
                    If lUpstreamInflows.Keys.Contains(lReach.Id) Then
                        lUpstreamIn = lUpstreamInflows.ItemByKey(lReach.Id)
                    End If
                    Dim lNPSLoad As Double = CalculateNPSLoad(aUCI, lReach, aBalanceType)
                    Dim lPSLoad As Double = CalculatePSLoad(aUCI, lReach, aSDateJ, aEDateJ, aBalanceType)
                    Dim lOutflow As Double = SafeSumAnnual(aBinaryData, LocationName, "ROHEAT", aSDateJ, aEDateJ)
                    Dim lTotalIn As Double = SafeSumAnnual(aBinaryData, LocationName, "IHEAT", aSDateJ, aEDateJ)
                    Dim lDiversion As Double = CalculateDiversion(aUCI, aBinaryData, lReach, lUpstreamInflows, lDownstreamReachID, lOutflow, aBalanceType)
                    Dim lGENERLoad As Double = CalculateGENERLoad(aUCI, lReach, aBalanceType, aSDateJ, aEDateJ)
                    Dim lMassBalance As Double = lTotalIn - lNPSLoad - lUpstreamIn - lPSLoad - lGENERLoad
                    For Each lColumnValue As DataColumn In pReach_Budget_Table.Columns
                        Dim lColumnName As String = lColumnValue.ColumnName
                        Select Case lColumnName
                            Case "OpTypeNumber"
                                lRow(lColumnName) = LocationName
                            Case "OpDesc"
                                lRow(lColumnName) = lReach.Description
                            Case "NPSLoad"
                                lRow(lColumnName) = HspfTable.NumFmtRE(lNPSLoad, 10)
                            Case "PSLoad"
                                lRow(lColumnName) = HspfTable.NumFmtRE(lPSLoad, 10)
                            Case "GENERLoad"
                                lRow(lColumnName) = HspfTable.NumFmtRE(lGENERLoad, 10)
                            Case "Diversion"
                                lRow(lColumnName) = HspfTable.NumFmtRE(lDiversion, 10)
                            Case "MassBalance"
                                lRow(lColumnName) = HspfTable.NumFmtRE(lMassBalance, 10)
                            Case "UpstreamIn"
                                lRow(lColumnName) = HspfTable.NumFmtRE(lUpstreamIn, 10)
                            Case Else
                                Dim lTest As atcTimeseries = aBinaryData.DataSets.FindData("Location", LocationName).FindData("Constituent", lColumnName)(0)
                                If lTest IsNot Nothing Then
                                    lRow(lColumnName) = HspfTable.NumFmtRE(SubsetByDate(lTest,
                                                                  aSDateJ, aEDateJ, Nothing).Attributes.GetDefinedValue("SumAnnual").Value, 10)
                                End If
                        End Select
                    Next lColumnValue

                    pReach_Budget_Table.Rows.Add(lRow)
                Next lReach
                Dim lTextToWrite As String = ""
                For Each lTableColumn As DataColumn In pReach_Budget_Table.Columns 'Writing the table headings
                    lTextToWrite &= lTableColumn.Caption & vbTab
                Next
                lReport.AppendLine(lTextToWrite)
                For Each lTableRow As DataRow In pReach_Budget_Table.Rows 'Writing the table contents
                    lTextToWrite = ""
                    For Each TableColumn As DataColumn In pReach_Budget_Table.Columns
                        lTextToWrite &= lTableRow(TableColumn) & vbTab
                    Next TableColumn
                    lReport.AppendLine(lTextToWrite)
                Next lTableRow
                lReport.AppendLine()
                lReport.AppendLine("Tabular Report of Average Annual Reach Budget for all the Reach Operations.")
                lReport.AppendLine("   " & aUCI.GlobalBlock.RunInf.Value)
                lReport.AppendLine("   Run Made " & aRunMade)
                lReport.AppendLine("   " & TimeSpanAsString(aSDateJ, aEDateJ, "Analysis Period: "))
                SaveFileString(aOutFolderName & aBalanceType & "_Reach_Budget.txt", lReport.ToString)
#End Region
#Region "BOD-Labile Case"
            Case "BOD-Labile"
                Dim lUnits2 As String = ""
                If aUCI.GlobalBlock.EmFg = 1 Then
                    lUnits = "lbs"
                Else
                    lUnits = "kg"

                End If
                pReach_Budget_Table = AddFirstSixColumnsReachBudget(pReach_Budget_Table, lUnits)
                Dim lRow As DataRow
                Dim lColumn As DataColumn
                lColumn = New DataColumn()
                lColumn.DataType = Type.GetType("System.Double")
                lColumn.ColumnName = "BODIN"
                lColumn.Caption = "Total BOD Inflow (" & lUnits & ")"
                pReach_Budget_Table.Columns.Add(lColumn)

                lColumn = New DataColumn()
                lColumn.DataType = Type.GetType("System.Double")
                lColumn.ColumnName = "BODFLUX-TOT"
                lColumn.Caption = "Total BOD Flux (" & lUnits & ")"
                pReach_Budget_Table.Columns.Add(lColumn)

                lColumn = New DataColumn()
                lColumn.DataType = Type.GetType("System.Double")
                lColumn.ColumnName = "BODFLUX-BODDEC"
                lColumn.Caption = "BOD Decay (" & lUnits & ")"
                pReach_Budget_Table.Columns.Add(lColumn)

                lColumn = New DataColumn()
                lColumn.DataType = Type.GetType("System.Double")
                lColumn.ColumnName = "BODFLUX-SINK"
                lColumn.Caption = "BOD Sink (" & lUnits & ")"
                pReach_Budget_Table.Columns.Add(lColumn)

                lColumn = New DataColumn()
                lColumn.DataType = Type.GetType("System.Double")
                lColumn.ColumnName = "BODFLUX-BENTHAL"
                lColumn.Caption = "BOD Benthal (" & lUnits & ")"
                pReach_Budget_Table.Columns.Add(lColumn)

                lColumn = New DataColumn()
                lColumn.DataType = Type.GetType("System.Double")
                lColumn.ColumnName = "BODFLUX-BENTHIC"
                lColumn.Caption = "BOD Benthic (" & lUnits & ")"
                pReach_Budget_Table.Columns.Add(lColumn)

                lColumn = New DataColumn()
                lColumn.DataType = Type.GetType("System.Double")
                lColumn.ColumnName = "BODFLUX-DENITR"
                lColumn.Caption = "BOD Denitrification (" & lUnits & ")"
                pReach_Budget_Table.Columns.Add(lColumn)

                lColumn = New DataColumn()
                lColumn.DataType = Type.GetType("System.Double")
                lColumn.ColumnName = "BODFLUX-PHYTO"
                lColumn.Caption = "BOD Phytoplankton (" & lUnits & ")"
                pReach_Budget_Table.Columns.Add(lColumn)

                lColumn = New DataColumn()
                lColumn.DataType = Type.GetType("System.Double")
                lColumn.ColumnName = "BODFLUX-ZOO"
                lColumn.Caption = "BOD Zooplankton (" & lUnits & ")"
                pReach_Budget_Table.Columns.Add(lColumn)

                lColumn = New DataColumn()
                lColumn.DataType = Type.GetType("System.Double")
                lColumn.ColumnName = "BODOUTTOT"
                lColumn.Caption = "BOD Outflow (" & lUnits & ")"
                pReach_Budget_Table.Columns.Add(lColumn)

                For Each lReach As HspfOperation In aUCI.OpnSeqBlock.Opns
                    lRow = pReach_Budget_Table.NewRow
                    If Not lReach.Name = "RCHRES" Then Continue For
                    Dim LocationName As String = lReach.Name.Substring(0, 1) & ":" & lReach.Id
                    Logger.Status("Generating Reach Budget Report for BOD from " & LocationName)

                    'Dim lOutflowDataTypes1 As String() = ConstituentListRCHRES(aBalanceType)
                    Dim lTS As New atcTimeseries(Nothing)
                    Dim lAddTS As New atcDataGroup
                    Dim lTotalTS As New atcTimeseries(Nothing)
                    Dim lDownstreamReachID As Integer = lReach.DownOper("RCHRES")
                    Dim lUpstreamIn As Double = 0.0
                    If lUpstreamInflows.Keys.Contains(lReach.Id) Then
                        lUpstreamIn = lUpstreamInflows.ItemByKey(lReach.Id)
                    End If
                    Dim lNPSLoad As Double = CalculateNPSLoad(aUCI, lReach, aBalanceType)
                    Dim lPSLoad As Double = CalculatePSLoad(aUCI, lReach, aSDateJ, aEDateJ, aBalanceType)
                    Dim lOutflow As Double = SafeSumAnnual(aBinaryData, LocationName, "BODOUTTOT", aSDateJ, aEDateJ)
                    Dim lTotalIn As Double = SafeSumAnnual(aBinaryData, LocationName, "BODIN", aSDateJ, aEDateJ)
                    Dim lDiversion As Double = CalculateDiversion(aUCI, aBinaryData, lReach, lUpstreamInflows, lDownstreamReachID, lOutflow, aBalanceType)
                    Dim lGENERLoad As Double = CalculateGENERLoad(aUCI, lReach, aBalanceType, aSDateJ, aEDateJ)
                    Dim lMassBalance As Double = lTotalIn - lNPSLoad - lUpstreamIn - lPSLoad - lGENERLoad

                    For Each lColumnValue As DataColumn In pReach_Budget_Table.Columns
                        Dim lColumnName As String = lColumnValue.ColumnName
                        Select Case lColumnName
                            Case "OpTypeNumber"
                                lRow(lColumnName) = LocationName
                            Case "OpDesc"
                                lRow(lColumnName) = lReach.Description
                            Case "NPSLoad"
                                lRow(lColumnName) = HspfTable.NumFmtRE(lNPSLoad, 10)

                            Case "PSLoad"
                                lRow(lColumnName) = HspfTable.NumFmtRE(lPSLoad, 10)
                            Case "GENERLoad"
                                lRow(lColumnName) = HspfTable.NumFmtRE(lGENERLoad, 10)
                            Case "Diversion"
                                lRow(lColumnName) = HspfTable.NumFmtRE(lDiversion, 10)
                            Case "MassBalance"
                                lRow(lColumnName) = HspfTable.NumFmtRE(lMassBalance, 10)
                            Case "UpstreamIn"
                                lRow(lColumnName) = HspfTable.NumFmtRE(lUpstreamIn, 10)
                            Case Else
                                Dim lTest As atcTimeseries = aBinaryData.DataSets.FindData("Location", LocationName).FindData("Constituent", lColumnName)(0)
                                If lTest IsNot Nothing Then
                                    lRow(lColumnName) = HspfTable.NumFmtRE(SubsetByDate(lTest,
                                                                  aSDateJ, aEDateJ, Nothing).Attributes.GetDefinedValue("SumAnnual").Value, 10)
                                End If
                        End Select
                    Next lColumnValue

                    pReach_Budget_Table.Rows.Add(lRow)
                Next lReach
                Dim lTextToWrite As String = ""
                For Each TableColumn As DataColumn In pReach_Budget_Table.Columns 'Writing the table headings
                    lTextToWrite &= TableColumn.Caption & vbTab
                Next
                lReport.AppendLine(lTextToWrite)
                For Each TableRow As DataRow In pReach_Budget_Table.Rows 'Writing the table contents
                    lTextToWrite = ""
                    For Each TableColumn As DataColumn In pReach_Budget_Table.Columns
                        lTextToWrite &= TableRow(TableColumn) & vbTab
                    Next TableColumn
                    lReport.AppendLine(lTextToWrite)
                Next TableRow
                lReport.AppendLine()
                lReport.AppendLine("Tabular Report of Average Annual Reach Budget for all the Reach Operations.")
                lReport.AppendLine("   " & aUCI.GlobalBlock.RunInf.Value)
                lReport.AppendLine("   Run Made " & aRunMade)
                lReport.AppendLine("   " & TimeSpanAsString(aSDateJ, aEDateJ, "Analysis Period: "))
                SaveFileString(aOutFolderName & aBalanceType & "_Reach_Budget.txt", lReport.ToString)
#End Region
#Region "TotalN Case"
            Case "TN"
                For Each lConstituent As ConstituentProperties In aConstProperties
                    pReach_Budget_Table = New DataTable
                    Dim lReachConstituent As String = lConstituent.ConstNameForEXPPlus
                    If lReachConstituent = "NO3" Or lReachConstituent = "TAM" Then
                        If aUCI.GlobalBlock.EmFg = 1 Then
                            lUnits = "lbs"
                        Else
                            lUnits = "kgs"
                        End If
                        lUpstreamInflows = New atcCollection
                        pReach_Budget_Table = AddFirstSixColumnsReachBudget(pReach_Budget_Table, lUnits)
                        Dim lRow As DataRow
                        Dim lColumn As DataColumn
                        lColumn = New DataColumn()
                        lColumn.DataType = Type.GetType("System.Double")
                        lColumn.ColumnName = lReachConstituent & "-INTOT"
                        lColumn.Caption = "Total " & lReachConstituent & " Inflow (" & lUnits & ")"
                        pReach_Budget_Table.Columns.Add(lColumn)

                        lColumn = New DataColumn()
                        lColumn.DataType = Type.GetType("System.Double")
                        lColumn.ColumnName = lReachConstituent & "-PROCFLUX-TOT"
                        lColumn.Caption = "Total " & lReachConstituent & " Process Fluxes (" & lUnits & ")"
                        pReach_Budget_Table.Columns.Add(lColumn)

                        lColumn = New DataColumn()
                        lColumn.DataType = Type.GetType("System.Double")
                        lColumn.ColumnName = lReachConstituent & "-ADSDES-TOT"
                        lColumn.Caption = "Total " & lReachConstituent & " Adsorption/Desorption (" & lUnits & ")"
                        pReach_Budget_Table.Columns.Add(lColumn)

                        lColumn = New DataColumn()
                        lColumn.DataType = Type.GetType("System.Double")
                        lColumn.ColumnName = lReachConstituent & "-SCOURDEP-TOT"
                        lColumn.Caption = "Total " & lReachConstituent & " Scour/Deposition (" & lUnits & ")"
                        pReach_Budget_Table.Columns.Add(lColumn)

                        lColumn = New DataColumn()
                        lColumn.DataType = Type.GetType("System.Double")
                        lColumn.ColumnName = lReachConstituent & "-ATMDEPTOT"
                        lColumn.Caption = "Total " & lReachConstituent & " Atmospheric Deposition (" & lUnits & ")"
                        pReach_Budget_Table.Columns.Add(lColumn)

                        lColumn = New DataColumn()
                        lColumn.DataType = Type.GetType("System.Double")
                        lColumn.ColumnName = lReachConstituent & "-OUTTOT"
                        lColumn.Caption = "Total " & lReachConstituent & " Outflow (" & lUnits & ")"
                        pReach_Budget_Table.Columns.Add(lColumn)

                        For Each lReach As HspfOperation In aUCI.OpnSeqBlock.Opns
                            lRow = pReach_Budget_Table.NewRow
                            If Not lReach.Name = "RCHRES" Then Continue For
                            Dim lLocationName As String = lReach.Name.Substring(0, 1) & ":" & lReach.Id
                            Logger.Status("Generating Reach Budget Report for " & lReachConstituent & " from " & lLocationName)

                            'Dim lOutflowDataTypes1 As String() = ConstituentListRCHRES(aBalanceType)
                            Dim lTS As New atcTimeseries(Nothing)
                            Dim lAddTS As New atcDataGroup
                            Dim lTotalTS As New atcTimeseries(Nothing)
                            Dim lDownstreamReachID As Integer = lReach.DownOper("RCHRES")
                            Dim lUpstreamIn As Double = 0.0
                            If lUpstreamInflows.Keys.Contains(lReach.Id) Then
                                lUpstreamIn = lUpstreamInflows.ItemByKey(lReach.Id)
                            End If
                            Dim lNPSLoad As Double = CalculateNPSLoad(aUCI, lReach, lReachConstituent)
                            Dim lPSLoad As Double = CalculatePSLoad(aUCI, lReach, aSDateJ, aEDateJ, lReachConstituent)
                            Dim lOutflow As Double = SafeSumAnnual(aBinaryData, lLocationName, lReachConstituent & "-OUTTOT", aSDateJ, aEDateJ)
                            Dim lTotalIn As Double = SafeSumAnnual(aBinaryData, lLocationName, lReachConstituent & "-INTOT", aSDateJ, aEDateJ)
                            Dim lTotalAtmDep As Double = 0.0
                            lTotalAtmDep = SafeSumAnnual(aBinaryData, lLocationName, lReachConstituent & "-ATMDEPTOT", aSDateJ, aEDateJ)
                            If lTotalAtmDep < -99 Then
                                lTotalAtmDep = 0.0
                            End If
                            Dim lProcFluxTot As Double = SafeSumAnnual(aBinaryData, lLocationName, lReachConstituent & "-PROCFLUX-TOT", aSDateJ, aEDateJ)
                            If lReachConstituent = "NO3" Then
                                Try
                                    lOutflow += SafeSumAnnual(aBinaryData, lLocationName, "NO2-OUTTOT", aSDateJ, aEDateJ)
                                    lTotalIn += SafeSumAnnual(aBinaryData, lLocationName, "NO2-INTOT", aSDateJ, aEDateJ)
                                    lProcFluxTot += SafeSumAnnual(aBinaryData, lLocationName, "NO2-PROCFLUX-TOT", aSDateJ, aEDateJ)
                                Catch
                                End Try

                            End If

                            Dim lDiversion As Double = CalculateDiversion(aUCI, aBinaryData, lReach, lUpstreamInflows, lDownstreamReachID, lOutflow, lReachConstituent)
                            Dim lGENERLoad As Double = CalculateGENERLoad(aUCI, lReach, lReachConstituent, aSDateJ, aEDateJ)
                            Dim lMassBalance As Double = lTotalIn - lNPSLoad - lUpstreamIn - lPSLoad - lGENERLoad - lTotalAtmDep
                            For Each lColumnValue As DataColumn In pReach_Budget_Table.Columns
                                Try
                                    Dim lColumnName As String = lColumnValue.ColumnName
                                    Select Case lColumnName
                                        Case "OpTypeNumber"
                                            lRow(lColumnName) = lLocationName
                                        Case "OpDesc"
                                            lRow(lColumnName) = lReach.Description
                                        Case "NPSLoad"
                                            lRow(lColumnName) = HspfTable.NumFmtRE(lNPSLoad, 10)
                                        Case "PSLoad"
                                            lRow(lColumnName) = HspfTable.NumFmtRE(lPSLoad, 10)
                                        Case "GENERLoad"
                                            lRow(lColumnName) = HspfTable.NumFmtRE(lGENERLoad, 10)
                                        Case "Diversion"
                                            lRow(lColumnName) = HspfTable.NumFmtRE(lDiversion, 10)
                                        Case "MassBalance"
                                            lRow(lColumnName) = HspfTable.NumFmtRE(lMassBalance, 10)
                                        Case "UpstreamIn"
                                            lRow(lColumnName) = HspfTable.NumFmtRE(lUpstreamIn, 10)
                                        Case lReachConstituent & "-INTOT"
                                            lRow(lColumnName) = HspfTable.NumFmtRE(lTotalIn, 10)
                                        Case lReachConstituent & "-PROCFLUX-TOT"
                                            lRow(lColumnName) = lProcFluxTot
                                        Case lReachConstituent & "-OUTTOT"
                                            lRow(lColumnName) = lOutflow
                                        Case Else
                                            Dim lTest As atcTimeseries = aBinaryData.DataSets.FindData("Location", lLocationName).FindData("Constituent", lColumnName)(0)
                                            If lTest IsNot Nothing Then
                                                lRow(lColumnName) = HspfTable.NumFmtRE(SubsetByDate(lTest,
                                                                              aSDateJ, aEDateJ, Nothing).Attributes.GetDefinedValue("SumAnnual").Value, 10)
                                            End If
                                    End Select
                                Catch
                                End Try

                            Next lColumnValue

                            pReach_Budget_Table.Rows.Add(lRow)
                        Next lReach


                        Dim lTextToWrite As String = ""
                        For Each lTableColumn As DataColumn In pReach_Budget_Table.Columns 'Writing the table headings
                            lTextToWrite &= lTableColumn.Caption & vbTab
                        Next
                        lReport = New atcReport.ReportText
                        lReport.AppendLine(lTextToWrite)
                        For Each TableRow As DataRow In pReach_Budget_Table.Rows 'Writing the table contents
                            lTextToWrite = ""
                            For Each TableColumn As DataColumn In pReach_Budget_Table.Columns
                                lTextToWrite &= TableRow(TableColumn) & vbTab
                            Next TableColumn
                            lReport.AppendLine(lTextToWrite)
                        Next TableRow
                        lReport.AppendLine()
                        lReport.AppendLine("Tabular Report of Average Annual Reach Budget for all the Reach Operations.")
                        lReport.AppendLine("   " & aUCI.GlobalBlock.RunInf.Value)
                        lReport.AppendLine("   Run Made " & aRunMade)
                        lReport.AppendLine("   " & TimeSpanAsString(aSDateJ, aEDateJ, "Analysis Period: "))
                        SaveFileString(aOutFolderName & lConstituent.ConstNameForEXPPlus & "_Reach_Budget.txt", lReport.ToString)
                    End If
                Next lConstituent
#End Region

#Region "TotalP Case"
            Case "TP"
                For Each lConstituent As ConstituentProperties In aConstProperties
                    pReach_Budget_Table = New DataTable
                    Dim lReachConstituent As String = lConstituent.ConstNameForEXPPlus
                    If lReachConstituent = "PO4" Then
                        If aUCI.GlobalBlock.EmFg = 1 Then
                            lUnits = "lbs"
                        Else
                            lUnits = "kgs"
                        End If
                        lUpstreamInflows = New atcCollection
                        pReach_Budget_Table = AddFirstSixColumnsReachBudget(pReach_Budget_Table, lUnits)
                        Dim lRow As DataRow
                        Dim lColumn As DataColumn
                        lColumn = New DataColumn()
                        lColumn.DataType = Type.GetType("System.Double")
                        lColumn.ColumnName = lReachConstituent & "-INTOT"
                        lColumn.Caption = "Total " & lReachConstituent & " Inflow (" & lUnits & ")"
                        pReach_Budget_Table.Columns.Add(lColumn)

                        lColumn = New DataColumn()
                        lColumn.DataType = Type.GetType("System.Double")
                        lColumn.ColumnName = lReachConstituent & "-PROCFLUX-TOT"
                        lColumn.Caption = "Total " & lReachConstituent & " Process Fluxes (" & lUnits & ")"
                        pReach_Budget_Table.Columns.Add(lColumn)

                        lColumn = New DataColumn()
                        lColumn.DataType = Type.GetType("System.Double")
                        lColumn.ColumnName = lReachConstituent & "-ADSDES-TOT"
                        lColumn.Caption = "Total " & lReachConstituent & " Adsorption/Desorption (" & lUnits & ")"
                        pReach_Budget_Table.Columns.Add(lColumn)

                        lColumn = New DataColumn()
                        lColumn.DataType = Type.GetType("System.Double")
                        lColumn.ColumnName = lReachConstituent & "-SCOURDEP-TOT"
                        lColumn.Caption = "Total " & lReachConstituent & " Scour/Deposition (" & lUnits & ")"
                        pReach_Budget_Table.Columns.Add(lColumn)

                        lColumn = New DataColumn()
                        lColumn.DataType = Type.GetType("System.Double")
                        lColumn.ColumnName = lReachConstituent & "-ATMDEPTOT"
                        lColumn.Caption = "Total " & lReachConstituent & " Atmospheric Deposition (" & lUnits & ")"
                        pReach_Budget_Table.Columns.Add(lColumn)

                        lColumn = New DataColumn()
                        lColumn.DataType = Type.GetType("System.Double")
                        lColumn.ColumnName = lReachConstituent & "-OUTTOT"
                        lColumn.Caption = "Total " & lReachConstituent & " Outflow (" & lUnits & ")"
                        pReach_Budget_Table.Columns.Add(lColumn)

                        For Each lReach As HspfOperation In aUCI.OpnSeqBlock.Opns
                            lRow = pReach_Budget_Table.NewRow
                            If Not lReach.Name = "RCHRES" Then Continue For
                            Dim LocationName As String = lReach.Name.Substring(0, 1) & ":" & lReach.Id
                            Logger.Status("Generating Reach Budget Report for " & lReachConstituent & " from " & LocationName)

                            'Dim lOutflowDataTypes1 As String() = ConstituentListRCHRES(aBalanceType)
                            Dim lTS As New atcTimeseries(Nothing)
                            Dim lAddTS As New atcDataGroup
                            Dim lTotalTS As New atcTimeseries(Nothing)
                            Dim lDownstreamReachID As Integer = lReach.DownOper("RCHRES")
                            Dim lUpstreamIn As Double = 0.0
                            If lUpstreamInflows.Keys.Contains(lReach.Id) Then
                                lUpstreamIn = lUpstreamInflows.ItemByKey(lReach.Id)
                            End If
                            Dim lNPSLoad As Double = CalculateNPSLoad(aUCI, lReach, lReachConstituent)
                            Dim lPSLoad As Double = CalculatePSLoad(aUCI, lReach, aSDateJ, aEDateJ, lReachConstituent)
                            Dim lOutflow As Double = SafeSumAnnual(aBinaryData, LocationName, lReachConstituent & "-OUTTOT", aSDateJ, aEDateJ)
                            Dim lTotalIn As Double = SafeSumAnnual(aBinaryData, LocationName, lReachConstituent & "-INTOT", aSDateJ, aEDateJ)
                            Dim lTotalAtmDep As Double = 0.0
                            'can't be certain Atm Dep is in use, so check to see if data exists before retrieving SumAnnual
                            lTS = aBinaryData.DataSets.FindData("Location", LocationName).FindData("Constituent", lReachConstituent & "-ATMDEPTOT")(0)
                            If lTS IsNot Nothing Then lTotalAtmDep = SubsetByDate(lTS, aSDateJ, aEDateJ, Nothing).Attributes.GetDefinedValue("SumAnnual").Value

                            Dim lDiversion As Double = CalculateDiversion(aUCI, aBinaryData, lReach, lUpstreamInflows, lDownstreamReachID, lOutflow, lReachConstituent)

                            Dim lGENERLoad As Double = CalculateGENERLoad(aUCI, lReach, lReachConstituent, aSDateJ, aEDateJ)
                            Dim lMassBalance As Double = lTotalIn - lNPSLoad - lUpstreamIn - lPSLoad - lGENERLoad - lTotalAtmDep
                            For Each lColumnValue As DataColumn In pReach_Budget_Table.Columns
                                Try
                                    Dim lColumnName As String = lColumnValue.ColumnName
                                    Select Case lColumnName
                                        Case "OpTypeNumber"
                                            lRow(lColumnName) = LocationName
                                        Case "OpDesc"
                                            lRow(lColumnName) = lReach.Description
                                        Case "NPSLoad"
                                            lRow(lColumnName) = HspfTable.NumFmtRE(lNPSLoad, 10)
                                        Case "PSLoad"
                                            lRow(lColumnName) = HspfTable.NumFmtRE(lPSLoad, 10)
                                        Case "GENERLoad"
                                            lRow(lColumnName) = HspfTable.NumFmtRE(lGENERLoad, 10)
                                        Case "Diversion"
                                            lRow(lColumnName) = HspfTable.NumFmtRE(lDiversion, 10)
                                        Case "MassBalance"
                                            lRow(lColumnName) = HspfTable.NumFmtRE(lMassBalance, 10)
                                        Case "UpstreamIn"
                                            lRow(lColumnName) = HspfTable.NumFmtRE(lUpstreamIn, 10)
                                        Case Else
                                            Dim lTest As atcTimeseries = aBinaryData.DataSets.FindData("Location", LocationName).FindData("Constituent", lColumnName)(0)
                                            If lTest IsNot Nothing Then
                                                lRow(lColumnName) = HspfTable.NumFmtRE(SubsetByDate(lTest,
                                                                              aSDateJ, aEDateJ, Nothing).Attributes.GetDefinedValue("SumAnnual").Value, 10)
                                            End If
                                    End Select
                                Catch
                                End Try
                            Next lColumnValue

                            pReach_Budget_Table.Rows.Add(lRow)
                        Next lReach


                        Dim lTextToWrite As String = ""
                        For Each TableColumn As DataColumn In pReach_Budget_Table.Columns 'Writing the table headings
                            lTextToWrite &= TableColumn.Caption & vbTab
                        Next
                        lReport = New atcReport.ReportText
                        lReport.AppendLine(lTextToWrite)
                        For Each TableRow As DataRow In pReach_Budget_Table.Rows 'Writing the table contents
                            lTextToWrite = ""
                            For Each TableColumn As DataColumn In pReach_Budget_Table.Columns
                                lTextToWrite &= TableRow(TableColumn) & vbTab
                            Next TableColumn
                            lReport.AppendLine(lTextToWrite)
                        Next TableRow
                        lReport.AppendLine()
                        lReport.AppendLine("Tabular Report of Average Annual Reach Budget for all the Reach Operations.")
                        lReport.AppendLine("   " & aUCI.GlobalBlock.RunInf.Value)
                        lReport.AppendLine("   Run Made " & aRunMade)
                        lReport.AppendLine("   " & TimeSpanAsString(aSDateJ, aEDateJ, "Analysis Period: "))
                        SaveFileString(aOutFolderName & lConstituent.ConstNameForEXPPlus & "_Reach_Budget.txt", lReport.ToString)
                    End If
                Next lConstituent
#End Region

#Region "Else Case"
            Case Else
                'For GQAL Constituents
                For Each lConstituent As ConstituentProperties In aConstProperties

                    lUnits = GQualUnits(aUCI, lConstituent.ConstituentNameInUCI)

                    Dim lColumn As DataColumn
                    Dim lRow As DataRow
                    pReach_Budget_Table = AddFirstSixColumnsReachBudget(pReach_Budget_Table, lUnits)
                    lColumn = New DataColumn()
                    lColumn.DataType = Type.GetType("System.Double")
                    lColumn.ColumnName = aBalanceType & "-TIQAL"
                    lColumn.Caption = "Total " & aBalanceType & " Inflow (" & lUnits & ")"
                    pReach_Budget_Table.Columns.Add(lColumn)

                    lColumn = New DataColumn()
                    lColumn.DataType = Type.GetType("System.Double")
                    lColumn.ColumnName = aBalanceType & "-PDQAL"
                    lColumn.Caption = "Input of " & aBalanceType & " due to decay of parents (" & lUnits & ")"
                    pReach_Budget_Table.Columns.Add(lColumn)

                    lColumn = New DataColumn()
                    lColumn.DataType = Type.GetType("System.Double")
                    lColumn.ColumnName = aBalanceType & "-GQADEP"
                    lColumn.Caption = "Total Atmospheric deposition of " & aBalanceType & " (" & lUnits & ")"
                    pReach_Budget_Table.Columns.Add(lColumn)

                    lColumn = New DataColumn()
                    lColumn.DataType = Type.GetType("System.Double")
                    lColumn.ColumnName = aBalanceType & "-DDQAL-TOT"
                    lColumn.Caption = "Decay of Dissolved " & aBalanceType & " (" & lUnits & ")"
                    pReach_Budget_Table.Columns.Add(lColumn)

                    lColumn = New DataColumn()
                    lColumn.DataType = Type.GetType("System.Double")
                    lColumn.ColumnName = aBalanceType & "-DSQAL-TOT"
                    lColumn.Caption = "Dep(+)/Scour(-) of " & aBalanceType & " (" & lUnits & ")"
                    pReach_Budget_Table.Columns.Add(lColumn)

                    lColumn = New DataColumn()
                    lColumn.DataType = Type.GetType("System.Double")
                    lColumn.ColumnName = aBalanceType & "-ADQAL-TOT"
                    lColumn.Caption = "Adsorption(+)/Desorption(-) of " & aBalanceType & "(" & lUnits & ")"
                    pReach_Budget_Table.Columns.Add(lColumn)

                    lColumn = New DataColumn()
                    lColumn.DataType = Type.GetType("System.Double")
                    lColumn.ColumnName = aBalanceType & "-TROQAL"
                    lColumn.Caption = "Total Outflow of " & aBalanceType & " (" & lUnits & ")"
                    pReach_Budget_Table.Columns.Add(lColumn)


                    For Each lReach As HspfOperation In aUCI.OpnSeqBlock.Opns
                        lRow = pReach_Budget_Table.NewRow
                        If Not lReach.Name = "RCHRES" Then Continue For
                        If Not lReach.Tables("ACTIVITY").Parms("GQALFG").Value = "1" Then Continue For
                        Dim lLocationName As String = lReach.Name.Substring(0, 1) & ":" & lReach.Id
                        Logger.Status("Generating Reach Budget Report for " & lConstituent.ConstituentNameInUCI & " from " & lLocationName)
                        'If lReach.Id = 106 Then Stop
                        Dim lTS As New atcTimeseries(Nothing)
                        Dim lAddTS As New atcDataGroup
                        Dim lTotalTS As New atcTimeseries(Nothing)
                        Dim lDownstreamReachID As Integer = lReach.DownOper("RCHRES")
                        Dim lUpstreamIn As Double = 0.0
                        If lUpstreamInflows.Keys.Contains(lReach.Id) Then
                            lUpstreamIn = lUpstreamInflows.ItemByKey(lReach.Id)
                        End If
                        Dim lNPSLoad As Double = CalculateNPSLoad(aUCI, lReach, aBalanceType, aGQALID)
                        Dim lPSLoad As Double = CalculatePSLoad(aUCI, lReach, aSDateJ, aEDateJ, aBalanceType, aGQALID)
                        Dim lOutflow As Double = SafeSumAnnual(aBinaryData, lLocationName, aBalanceType & "-TROQAL", aSDateJ, aEDateJ)
                        Dim lTotalIn As Double = SafeSumAnnual(aBinaryData, lLocationName, aBalanceType & "-TIQAL", aSDateJ, aEDateJ)
                        Dim lDiversion As Double = CalculateDiversion(aUCI, aBinaryData, lReach, lUpstreamInflows, lDownstreamReachID, lOutflow, aBalanceType, aGQALID)
                        Dim lGENERLoad As Double = CalculateGENERLoad(aUCI, lReach, aBalanceType, aSDateJ, aEDateJ, aGQALID)
                        Dim lMassBalance As Double = lTotalIn - lNPSLoad - lUpstreamIn - lPSLoad - lGENERLoad
                        For Each lColumnValue As DataColumn In pReach_Budget_Table.Columns
                            Dim ColumnName As String = lColumnValue.ColumnName
                            Select Case ColumnName
                                Case "OpTypeNumber"
                                    lRow(ColumnName) = lLocationName
                                Case "OpDesc"
                                    lRow(ColumnName) = lReach.Description
                                Case "NPSLoad"
                                    lRow(ColumnName) = HspfTable.NumFmtRE(lNPSLoad, 10)
                                Case "PSLoad"
                                    lRow(ColumnName) = HspfTable.NumFmtRE(lPSLoad, 10)
                                Case "GENERLoad"
                                    lRow(ColumnName) = HspfTable.NumFmtRE(lGENERLoad, 10)
                                Case "Diversion"
                                    lRow(ColumnName) = HspfTable.NumFmtRE(lDiversion, 10)
                                Case "MassBalance"
                                    lRow(ColumnName) = HspfTable.NumFmtRE(lMassBalance, 10)
                                Case "UpstreamIn"
                                    lRow(ColumnName) = HspfTable.NumFmtRE(lUpstreamIn, 10)
                                Case Else
                                    Try
                                        Dim lTest As atcTimeseries = aBinaryData.DataSets.FindData("Location", lLocationName).FindData("Constituent", ColumnName)(0)
                                        If lTest IsNot Nothing Then
                                            lRow(ColumnName) = HspfTable.NumFmtRE(SubsetByDate(lTest,
                                                                          aSDateJ, aEDateJ, Nothing).Attributes.GetDefinedValue("SumAnnual").Value, 10)
                                        End If
                                    Catch
                                        'row(ColumnName)
                                    End Try
                            End Select
                        Next lColumnValue

                        pReach_Budget_Table.Rows.Add(lRow)
                    Next lReach
                    Dim lTextToWrite As String = ""
                    For Each TableColumn As DataColumn In pReach_Budget_Table.Columns 'Writing the table headings
                        lTextToWrite &= TableColumn.Caption & vbTab
                    Next
                    lReport.AppendLine(lTextToWrite)
                    For Each lTableRow As DataRow In pReach_Budget_Table.Rows 'Writing the table contents
                        lTextToWrite = ""
                        For Each TableColumn As DataColumn In pReach_Budget_Table.Columns
                            lTextToWrite &= lTableRow(TableColumn) & vbTab
                        Next TableColumn
                        lReport.AppendLine(lTextToWrite)
                    Next lTableRow
                    lReport.AppendLine()
                    lReport.AppendLine("Tabular Report of Average Annual Reach Budget for all the Reach Operations.")
                    lReport.AppendLine("   " & aUCI.GlobalBlock.RunInf.Value)
                    lReport.AppendLine("   Run Made " & aRunMade)
                    lReport.AppendLine("   " & TimeSpanAsString(aSDateJ, aEDateJ, "Analysis Period: "))
                    SaveFileString(aOutFolderName & aBalanceType & "_Reach_Budget.txt", lReport.ToString)
                Next lConstituent
#End Region


        End Select
    End Sub

    Private Function SafeSumAnnual(ByVal aBinaryData As atcDataSource, ByVal aLocationName As String, ByVal aReachConstituent As String, ByVal aSDateJ As Double, ByVal aEDateJ As Double) As Double
        Dim lValue As Double = -999.0
        Try
            lValue = SubsetByDate(aBinaryData.DataSets.FindData("Location", aLocationName).FindData("Constituent", aReachConstituent)(0),
                                  aSDateJ, aEDateJ, Nothing).Attributes.GetDefinedValue("SumAnnual").Value
            Return lValue
        Catch
            Return lValue
        End Try
    End Function

    Private Function CalculateNPSLoad(ByVal aUCI As HspfUci, ByVal aReach As HspfOperation, ByVal aConstituentName As String, Optional ByVal aGQALID As Integer = 0) As Double
        Dim lNPSLoad As Double = 0.0
        Dim lSelectExpression As String = ""
        For Each lReachSource As HspfConnection In aReach.Sources
            Try
                If lReachSource.Source.Opn Is Nothing OrElse lReachSource.Source.Opn.Name = "RCHRES" Then Continue For
                'If Not ((lReachSource.Source.Opn.Name = "PERLND" AndAlso lReachSource.Source.Opn.Tables("ACTIVITY").Parms("PQALFG").Value = "1") OrElse
                '           (lReachSource.Source.Opn.Name = "IMPLND" AndAlso lReachSource.Source.Opn.Tables("ACTIVITY").Parms("IQALFG").Value = "1")) Then Continue For
                Dim lConnectionArea As Double = lReachSource.MFact
                Dim lOperationTypeNumber As String = SafeSubstring(lReachSource.Source.VolName, 0, 1) & ":" & lReachSource.Source.VolId
                If aConstituentName = "NO3" Or aConstituentName = "TAM" Or aConstituentName = "PO4" Then
                    lSelectExpression = "OpTypeNumber= '" & lOperationTypeNumber & "' And Year = 'SumAnnual' And ConstNameEXP = '" & aConstituentName & "'"
                Else
                    lSelectExpression = "OpTypeNumber= '" & lOperationTypeNumber & "' And Year = 'SumAnnual'"
                End If

                Dim lFoundRows() As DataRow = pLand_Constituent_Table.Select(lSelectExpression)
                If lFoundRows.Length = 0 Then Continue For
                lNPSLoad += lConnectionArea * lFoundRows(0)("TotalOutflow")
            Catch
            End Try

        Next lReachSource

        Return lNPSLoad
    End Function
    Private Function CalculatePSLoad(ByVal aUCI As HspfUci, ByVal aReach As HspfOperation,
                                     ByVal aSDateJ As Double,
                                      ByVal aEDateJ As Double, ByVal aConstituentName As String,
                                     Optional ByVal aGQALID As Integer = 0) As Double
        Dim lPSLoad As Double = 0.0
        Select Case aConstituentName
            Case "DO"
                For Each lSource As HspfPointSource In aReach.PointSources
                    If lSource.Target.Group = "INFLOW" AndAlso lSource.Target.Member = "OXIF" AndAlso lSource.Target.MemSub1 = 1 Then
                        Dim lTimeSeriesTransformaton As String = lSource.Tran.ToString
                        Dim lVolName As String = lSource.Source.VolName
                        Dim lDSN As Integer = lSource.Source.VolId
                        Dim lMfact As Double = lSource.MFact
                        Dim lTransformationMultFact As Double = 0
                        For i As Integer = 0 To aUCI.FilesBlock.Count
                            If aUCI.FilesBlock.Value(i).Typ = lVolName Then
                                Dim lFileName As String = AbsolutePath(aUCI.FilesBlock.Value(i).Name.Trim, CurDir())
                                Dim lDataSource As atcDataSource = atcDataManager.DataSourceBySpecification(lFileName)
                                If lDataSource Is Nothing Then
                                    If atcDataManager.OpenDataSource(lFileName) Then
                                        lDataSource = atcDataManager.DataSourceBySpecification(lFileName)
                                    End If
                                End If
                                Dim lTimeseries As atcTimeseries = lDataSource.DataSets.FindData("ID", lDSN)(0)
                                If Not lTimeseries Is Nothing Then
                                    lTimeseries = SubsetByDate(lTimeseries, aSDateJ, aEDateJ, Nothing)
                                    lTransformationMultFact = MultiFactorForPointSource(lTimeseries.Attributes.GetDefinedValue("Time Step").Value, lTimeseries.Attributes.GetDefinedValue("Time Unit").Value.ToString,
                                                                                            lTimeSeriesTransformaton, aUCI.OpnSeqBlock.Delt)
                                End If
                                lPSLoad += lTimeseries.Attributes.GetDefinedValue("Sum").Value * lMfact * lTransformationMultFact / YearCount(aSDateJ, aEDateJ)
                            End If
                        Next

                    End If
                Next lSource
            Case "BOD-Labile"
                For Each lSource As HspfPointSource In aReach.PointSources
                    If lSource.Target.Group = "INFLOW" AndAlso lSource.Target.Member = "OXIF" AndAlso lSource.Target.MemSub1 = 2 Then
                        Dim lTimeSeriesTransformaton As String = lSource.Tran.ToString
                        Dim lVolName As String = lSource.Source.VolName
                        Dim lDSN As Integer = lSource.Source.VolId
                        Dim lMfact As Double = lSource.MFact
                        Dim lTransformationMultFact As Double = 0
                        For i As Integer = 0 To aUCI.FilesBlock.Count
                            If aUCI.FilesBlock.Value(i).Typ = lVolName Then
                                Dim lFileName As String = AbsolutePath(aUCI.FilesBlock.Value(i).Name.Trim, CurDir())
                                Dim lDataSource As atcDataSource = atcDataManager.DataSourceBySpecification(lFileName)
                                If lDataSource Is Nothing Then
                                    If atcDataManager.OpenDataSource(lFileName) Then
                                        lDataSource = atcDataManager.DataSourceBySpecification(lFileName)
                                    End If
                                End If
                                Dim lTimeseries As atcTimeseries = lDataSource.DataSets.FindData("ID", lDSN)(0)
                                If Not lTimeseries Is Nothing Then
                                    lTimeseries = SubsetByDate(lTimeseries, aSDateJ, aEDateJ, Nothing)
                                    lTransformationMultFact = MultiFactorForPointSource(lTimeseries.Attributes.GetDefinedValue("Time Step").Value, lTimeseries.Attributes.GetDefinedValue("Time Unit").Value.ToString,
                                                                                            lTimeSeriesTransformaton, aUCI.OpnSeqBlock.Delt)
                                End If
                                lPSLoad += lTimeseries.Attributes.GetDefinedValue("Sum").Value * lMfact * lTransformationMultFact / YearCount(aSDateJ, aEDateJ)

                            End If
                        Next

                    End If
                Next lSource

            Case "Heat"
                For Each lSource As HspfPointSource In aReach.PointSources
                    If lSource.Target.Group = "INFLOW" AndAlso lSource.Target.Member = "IHEAT" Then
                        Dim lTimeSeriesTransformaton As String = lSource.Tran.ToString
                        Dim lVolName As String = lSource.Source.VolName
                        Dim lDSN As Integer = lSource.Source.VolId
                        Dim lMfact As Double = lSource.MFact
                        Dim lTransformationMultFact As Double = 0
                        For i As Integer = 0 To aUCI.FilesBlock.Count
                            If aUCI.FilesBlock.Value(i).Typ = lVolName Then
                                Dim lFileName As String = AbsolutePath(aUCI.FilesBlock.Value(i).Name.Trim, CurDir())
                                Dim lDataSource As atcDataSource = atcDataManager.DataSourceBySpecification(lFileName)
                                If lDataSource Is Nothing Then
                                    If atcDataManager.OpenDataSource(lFileName) Then
                                        lDataSource = atcDataManager.DataSourceBySpecification(lFileName)
                                    End If
                                End If
                                Dim ltimeseries As atcTimeseries = lDataSource.DataSets.FindData("ID", lDSN)(0)
                                If Not ltimeseries Is Nothing Then
                                    ltimeseries = SubsetByDate(ltimeseries, aSDateJ, aEDateJ, Nothing)
                                    lTransformationMultFact = MultiFactorForPointSource(ltimeseries.Attributes.GetDefinedValue("Time Step").Value, ltimeseries.Attributes.GetDefinedValue("Time Unit").Value.ToString,
                                                                                            lTimeSeriesTransformaton, aUCI.OpnSeqBlock.Delt)
                                End If
                                lPSLoad += ltimeseries.Attributes.GetDefinedValue("Sum").Value * lMfact * lTransformationMultFact / YearCount(aSDateJ, aEDateJ)
                            End If
                        Next

                    End If
                Next lSource

            Case "NO3"
                For Each lSource As HspfPointSource In aReach.PointSources
                    If (lSource.Target.Group = "INFLOW" AndAlso lSource.Target.Member = "NUIF1" AndAlso lSource.Target.MemSub1 = 1) OrElse
                        (lSource.Target.Group = "INFLOW" AndAlso lSource.Target.Member = "NUIF1" AndAlso lSource.Target.MemSub1 = 3) Then
                        Dim lTimeSeriesTransformaton As String = lSource.Tran.ToString
                        Dim lVolName As String = lSource.Source.VolName
                        Dim lDSN As Integer = lSource.Source.VolId
                        Dim lMfact As Double = lSource.MFact
                        Dim lTransformationMultFact As Double = 0
                        For i As Integer = 0 To aUCI.FilesBlock.Count
                            If aUCI.FilesBlock.Value(i).Typ = lVolName Then
                                Dim lFileName As String = AbsolutePath(aUCI.FilesBlock.Value(i).Name.Trim, CurDir())
                                Dim lDataSource As atcDataSource = atcDataManager.DataSourceBySpecification(lFileName)
                                If lDataSource Is Nothing Then
                                    If atcDataManager.OpenDataSource(lFileName) Then
                                        lDataSource = atcDataManager.DataSourceBySpecification(lFileName)
                                    End If
                                End If
                                Dim lTimeseries As atcTimeseries = lDataSource.DataSets.FindData("ID", lDSN)(0)
                                If Not lTimeseries Is Nothing Then
                                    lTimeseries = SubsetByDate(lTimeseries, aSDateJ, aEDateJ, Nothing)
                                    lTransformationMultFact = MultiFactorForPointSource(lTimeseries.Attributes.GetDefinedValue("Time Step").Value, lTimeseries.Attributes.GetDefinedValue("Time Unit").Value.ToString,
                                                                                            lTimeSeriesTransformaton, aUCI.OpnSeqBlock.Delt)
                                End If
                                lPSLoad += lTimeseries.Attributes.GetDefinedValue("Sum").Value * lMfact * lTransformationMultFact / YearCount(aSDateJ, aEDateJ)
                            End If
                        Next

                    End If
                Next lSource

            Case "TAM"
                For Each lSource As HspfPointSource In aReach.PointSources
                    If (lSource.Target.Group = "INFLOW" AndAlso lSource.Target.Member = "NUIF1" AndAlso lSource.Target.MemSub1 = 2) OrElse
                       (lSource.Target.Group = "INFLOW" AndAlso lSource.Target.Member = "NUIF2" AndAlso lSource.Target.MemSub1 = 1 AndAlso lSource.Target.MemSub2 = 1) OrElse
                       (lSource.Target.Group = "INFLOW" AndAlso lSource.Target.Member = "NUIF2" AndAlso lSource.Target.MemSub1 = 1 AndAlso lSource.Target.MemSub2 = 2) OrElse
                       (lSource.Target.Group = "INFLOW" AndAlso lSource.Target.Member = "NUIF2" AndAlso lSource.Target.MemSub1 = 1 AndAlso lSource.Target.MemSub2 = 3) Then

                        Dim lTimeSeriesTransformaton As String = lSource.Tran.ToString
                        Dim lVolName As String = lSource.Source.VolName
                        Dim lDSN As Integer = lSource.Source.VolId
                        Dim lMfact As Double = lSource.MFact
                        Dim lTransformationMultFact As Double = 0
                        For i As Integer = 0 To aUCI.FilesBlock.Count
                            If aUCI.FilesBlock.Value(i).Typ = lVolName Then
                                Dim lFileName As String = AbsolutePath(aUCI.FilesBlock.Value(i).Name.Trim, CurDir())
                                Dim lDataSource As atcDataSource = atcDataManager.DataSourceBySpecification(lFileName)
                                If lDataSource Is Nothing Then
                                    If atcDataManager.OpenDataSource(lFileName) Then
                                        lDataSource = atcDataManager.DataSourceBySpecification(lFileName)
                                    End If
                                End If
                                Dim lTimeseries As atcTimeseries = lDataSource.DataSets.FindData("ID", lDSN)(0)
                                If Not lTimeseries Is Nothing Then
                                    lTimeseries = SubsetByDate(lTimeseries, aSDateJ, aEDateJ, Nothing)
                                    lTransformationMultFact = MultiFactorForPointSource(lTimeseries.Attributes.GetDefinedValue("Time Step").Value, lTimeseries.Attributes.GetDefinedValue("Time Unit").Value.ToString,
                                                                                            lTimeSeriesTransformaton, aUCI.OpnSeqBlock.Delt)
                                End If
                                lPSLoad += lTimeseries.Attributes.GetDefinedValue("Sum").Value * lMfact * lTransformationMultFact / YearCount(aSDateJ, aEDateJ)
                            End If
                        Next

                    End If
                Next lSource

            Case "PO4"
                For Each lSource As HspfPointSource In aReach.PointSources
                    If (lSource.Target.Group = "INFLOW" AndAlso lSource.Target.Member = "NUIF1" AndAlso lSource.Target.MemSub1 = 4) OrElse
                       (lSource.Target.Group = "INFLOW" AndAlso lSource.Target.Member = "NUIF2" AndAlso lSource.Target.MemSub1 = 2 AndAlso lSource.Target.MemSub2 = 1) OrElse
                       (lSource.Target.Group = "INFLOW" AndAlso lSource.Target.Member = "NUIF2" AndAlso lSource.Target.MemSub1 = 2 AndAlso lSource.Target.MemSub2 = 2) OrElse
                       (lSource.Target.Group = "INFLOW" AndAlso lSource.Target.Member = "NUIF2" AndAlso lSource.Target.MemSub1 = 2 AndAlso lSource.Target.MemSub2 = 3) Then
                        Dim lTimeSeriesTransformaton As String = lSource.Tran.ToString
                        Dim lVolName As String = lSource.Source.VolName
                        Dim lDSN As Integer = lSource.Source.VolId
                        Dim lMfact As Double = lSource.MFact
                        Dim lTransformationMultFact As Double = 0
                        For i As Integer = 0 To aUCI.FilesBlock.Count
                            If aUCI.FilesBlock.Value(i).Typ = lVolName Then
                                Dim lFileName As String = AbsolutePath(aUCI.FilesBlock.Value(i).Name.Trim, CurDir())
                                Dim lDataSource As atcDataSource = atcDataManager.DataSourceBySpecification(lFileName)
                                If lDataSource Is Nothing Then
                                    If atcDataManager.OpenDataSource(lFileName) Then
                                        lDataSource = atcDataManager.DataSourceBySpecification(lFileName)
                                    End If
                                End If
                                Dim lTimeseries As atcTimeseries = lDataSource.DataSets.FindData("ID", lDSN)(0)
                                If Not lTimeseries Is Nothing Then
                                    lTimeseries = SubsetByDate(lTimeseries, aSDateJ, aEDateJ, Nothing)
                                    lTransformationMultFact = MultiFactorForPointSource(lTimeseries.Attributes.GetDefinedValue("Time Step").Value, lTimeseries.Attributes.GetDefinedValue("Time Unit").Value.ToString,
                                                                                            lTimeSeriesTransformaton, aUCI.OpnSeqBlock.Delt)
                                End If
                                lPSLoad += lTimeseries.Attributes.GetDefinedValue("Sum").Value * lMfact * lTransformationMultFact / YearCount(aSDateJ, aEDateJ)
                            End If
                        Next

                    End If
                Next lSource
            Case Else
                For Each lSource As HspfPointSource In aReach.PointSources
                    If (lSource.Target.Group = "INFLOW" AndAlso lSource.Target.Member = "IDQAL" AndAlso lSource.Target.MemSub1 = aGQALID) OrElse
                        (lSource.Target.Group = "INFLOW" AndAlso lSource.Target.Member = "ISQAL" AndAlso lSource.Target.MemSub2 = aGQALID) Then
                        Dim lTimeSeriesTransformaton As String = lSource.Tran.ToString
                        Dim lVolName As String = lSource.Source.VolName
                        Dim lDSN As Integer = lSource.Source.VolId
                        Dim lMfact As Double = lSource.MFact
                        Dim lTransformationMultFact As Double = 0
                        For i As Integer = 0 To aUCI.FilesBlock.Count
                            If aUCI.FilesBlock.Value(i).Typ = lVolName Then
                                Dim lFileName As String = AbsolutePath(aUCI.FilesBlock.Value(i).Name.Trim, CurDir())
                                Dim lDataSource As atcDataSource = atcDataManager.DataSourceBySpecification(lFileName)
                                If lDataSource Is Nothing Then
                                    If atcDataManager.OpenDataSource(lFileName) Then
                                        lDataSource = atcDataManager.DataSourceBySpecification(lFileName)
                                    End If
                                End If
                                Dim lTimeseries As atcTimeseries = lDataSource.DataSets.FindData("ID", lDSN)(0)
                                If Not lTimeseries Is Nothing Then
                                    lTimeseries = SubsetByDate(lTimeseries, aSDateJ, aEDateJ, Nothing)
                                    lTransformationMultFact = MultiFactorForPointSource(lTimeseries.Attributes.GetDefinedValue("Time Step").Value, lTimeseries.Attributes.GetDefinedValue("Time Unit").Value.ToString,
                                                                                            lTimeSeriesTransformaton, aUCI.OpnSeqBlock.Delt)
                                End If
                                lPSLoad += lTimeseries.Attributes.GetDefinedValue("Sum").Value * lMfact * lTransformationMultFact / YearCount(aSDateJ, aEDateJ)
                            End If
                        Next

                    End If
                Next lSource

        End Select



        Return lPSLoad
    End Function

    Private Function MultiFactorForPointSource(ByVal aTStep As Integer, ByVal aTimeUnit As String, ByVal aTransformation As String,
                                               ByVal aDelta As atcTimeUnit) As Double
        Dim lMultiFactor As Double = 0.0
        If Trim(aTransformation) = "DIV" Then
            lMultiFactor = 1.0
        Else
            Select Case aTransformation
                Case "SAME"
                    If aDelta / 60 = 1 AndAlso aTimeUnit = "TUDay" AndAlso aTStep = 1 Then
                        lMultiFactor = 24.0
                    ElseIf aDelta / 60 = 1 AndAlso aTimeUnit = "TUHour" AndAlso aTStep = 1 Then
                        lMultiFactor = 1
                    End If
            End Select
        End If

        Return lMultiFactor
    End Function
    Private Function CalculateDiversion(ByVal aUCI As HspfUci, ByVal aBinaryDataSource As atcDataSource, ByVal aReach As HspfOperation, ByRef aUpstreamInflows As atcCollection,
                                ByVal aDownstreamReachID As Integer, ByVal aOutflow As Double, ByVal aConstituent As String, Optional ByVal aGQALID As Integer = 0) As Double
        Dim lDiversion As Double = 0.0
        Dim lTimeSeries As New atcTimeseries(Nothing)
        Try
            If aReach.Tables("GEN-INFO").Parms("NEXITS").Value = 1 Then
                'Logger.Dbg(aReach.Id)
                aUpstreamInflows.Increment(aDownstreamReachID, aOutflow)
            Else
                Dim lExitNUmber As Integer = 0
                FindDownStreamExitNumber(aUCI, aReach, lExitNUmber)
                Dim lExitFlowConstituent As String = ""
                Dim lTotalOutFlow As Double = 0.0
                Select Case aConstituent
                    Case "DO"
                        lExitFlowConstituent = "DOXOUT-EXIT" & lExitNUmber
                        lTimeSeries = aBinaryDataSource.DataSets.FindData("Location", "R:" & aReach.Id).
                            FindData("Constituent", lExitFlowConstituent)(0)
                        If Not lTimeSeries Is Nothing Then
                            lTotalOutFlow = lTimeSeries.Attributes.GetDefinedValue("SumAnnual").Value
                        End If
                        lDiversion = aOutflow - lTotalOutFlow
                    Case "BOD-Labile"
                        lExitFlowConstituent = "BODOUT-EXIT" & lExitNUmber
                        lTimeSeries = aBinaryDataSource.DataSets.FindData("Location", "R:" & aReach.Id).
                            FindData("Constituent", lExitFlowConstituent)(0)
                        If Not lTimeSeries Is Nothing Then
                            lTotalOutFlow = lTimeSeries.Attributes.GetDefinedValue("SumAnnual").Value
                        End If
                        lDiversion = aOutflow - lTotalOutFlow
                    Case "Heat"
                        lExitFlowConstituent = "OHEAT - EXIT-" & lExitNUmber
                        lTimeSeries = aBinaryDataSource.DataSets.FindData("Location", "R:" & aReach.Id).
                            FindData("Constituent", lExitFlowConstituent)(0)
                        If Not lTimeSeries Is Nothing Then
                            lTotalOutFlow = lTimeSeries.Attributes.GetDefinedValue("SumAnnual").Value
                        End If
                        lDiversion = aOutflow - lTotalOutFlow

                    Case "NO3"

                        lExitFlowConstituent = "NO3-OUTDIS-EXIT" & lExitNUmber
                        lTimeSeries = aBinaryDataSource.DataSets.FindData("Location", "R:" & aReach.Id).
                            FindData("Constituent", lExitFlowConstituent)(0)
                        If Not lTimeSeries Is Nothing Then
                            lTotalOutFlow = lTimeSeries.Attributes.GetDefinedValue("SumAnnual").Value
                        End If
                        lExitFlowConstituent = "NO2-OUTDIS-EXIT" & lExitNUmber
                        lTimeSeries = aBinaryDataSource.DataSets.FindData("Location", "R:" & aReach.Id).
                            FindData("Constituent", lExitFlowConstituent)(0)
                        If Not lTimeSeries Is Nothing Then
                            lTotalOutFlow += lTimeSeries.Attributes.GetDefinedValue("SumAnnual").Value
                        End If
                        lDiversion = aOutflow - lTotalOutFlow

                    Case "TAM"

                        lExitFlowConstituent = "TAM-OUTDIS-EXIT" & lExitNUmber
                        lTimeSeries = aBinaryDataSource.DataSets.FindData("Location", "R:" & aReach.Id).
                            FindData("Constituent", lExitFlowConstituent)(0)
                        If Not lTimeSeries Is Nothing Then
                            lTotalOutFlow = lTimeSeries.Attributes.GetDefinedValue("SumAnnual").Value
                        End If
                        lExitFlowConstituent = "TAM-OUTPART-TOT-EXIT" & lExitNUmber
                        lTimeSeries = aBinaryDataSource.DataSets.FindData("Location", "R:" & aReach.Id).
                            FindData("Constituent", lExitFlowConstituent)(0)
                        If Not lTimeSeries Is Nothing Then
                            lTotalOutFlow += lTimeSeries.Attributes.GetDefinedValue("SumAnnual").Value
                        End If
                        lDiversion = aOutflow - lTotalOutFlow

                    Case "PO4"

                        lExitFlowConstituent = "PO4-OUTDIS-EXIT" & lExitNUmber
                        lTimeSeries = aBinaryDataSource.DataSets.FindData("Location", "R:" & aReach.Id).
                            FindData("Constituent", lExitFlowConstituent)(0)
                        If Not lTimeSeries Is Nothing Then
                            lTotalOutFlow = lTimeSeries.Attributes.GetDefinedValue("SumAnnual").Value
                        End If
                        lExitFlowConstituent = "PO4-OUTPART-TOT-EXIT" & lExitNUmber
                        lTimeSeries = aBinaryDataSource.DataSets.FindData("Location", "R:" & aReach.Id).
                            FindData("Constituent", lExitFlowConstituent)(0)
                        If Not lTimeSeries Is Nothing Then
                            lTotalOutFlow += lTimeSeries.Attributes.GetDefinedValue("SumAnnual").Value
                        End If
                        lDiversion = aOutflow - lTotalOutFlow

                    Case Else
                        lExitFlowConstituent = aConstituent & "-OSQAL-TOT-" & lExitNUmber

                        lTimeSeries = aBinaryDataSource.DataSets.FindData("Location", "R:" & aReach.Id).
                            FindData("Constituent", lExitFlowConstituent)(0)
                        If Not lTimeSeries Is Nothing Then
                            lTotalOutFlow = lTimeSeries.Attributes.GetDefinedValue("SumAnnual").Value
                        End If
                        lExitFlowConstituent = aConstituent & "-TOSQAL-EXIT" & lExitNUmber
                        lTimeSeries = aBinaryDataSource.DataSets.FindData("Location", "R:" & aReach.Id).
                            FindData("Constituent", lExitFlowConstituent)(0)
                        If Not lTimeSeries Is Nothing Then
                            lTotalOutFlow += lTimeSeries.Attributes.GetDefinedValue("SumAnnual").Value
                        End If
                        lDiversion = aOutflow - lTotalOutFlow

                End Select
                aUpstreamInflows.Increment(aDownstreamReachID, lTotalOutFlow)
            End If

        Catch ex As Exception
            Logger.Msg("Trouble reading the parameters of RCHRES " & aReach.Id & ". Constituent Reports will not be generated.", MsgBoxStyle.Critical, "RCHRES Parameter Issue.")
            Return Nothing

        End Try

        Return lDiversion
    End Function
    Private Function CalculateGENERLoad(ByVal aUCI As HspfUci, ByVal aReach As HspfOperation, ByVal aConstituentName As String,
                                        ByVal aSDateJ As Double, ByVal aEDateJ As Double, Optional ByVal aGQALID As Integer = 0) As Double
        Dim lGENERLoad As Double = 0.0

        Select Case aConstituentName
            Case "PO4"
                lGENERLoad = 0
                'If aReach.Id = 157 Then Stop
                For Each lSource As HspfConnection In aReach.Sources
                    Dim lGENERSum As Double = 0.0
                    Dim lMfact As Double = 0.0
                    If lSource.Source.VolName = "GENER" Then
                        Dim lGENEROperationisOutputtoWDM As Boolean = False
                        With GetGENERSum(aUCI, lSource, aSDateJ, aEDateJ)
                            lGENERSum = .Item1
                            lGENEROperationisOutputtoWDM = .Item2
                        End With
                        If lSource.MassLink > 0 Then
                            lGENERSum *= lSource.MFact
                            For Each lMassLink As HspfMassLink In aUCI.MassLinks
                                If lMassLink.MassLinkId = lSource.MassLink AndAlso lMassLink.Target.Member = "NUIF1" AndAlso lMassLink.Target.MemSub1 = 4 Then
                                    lGENERSum *= lMassLink.MFact
                                    lGENERLoad += lGENERSum
                                    Exit For
                                End If
                            Next lMassLink
                        ElseIf lSource.Target.Group = "INFLOW" AndAlso lSource.Target.Member = "NUIF1" AndAlso lSource.Target.MemSub1 = 4 Then
                            lGENERSum *= lSource.MFact
                            lGENERLoad += lGENERSum
                        End If
                        If Not lGENEROperationisOutputtoWDM Then
                            Logger.Dbg("GENER Loadings Issue: The RCHRES operation " & aReach.Id & " has loadings input for the constituent " & aConstituentName & " from GENER connections in the Network Block. Please make sure that these GENER operations output to a WDM dataset for accurate source accounting.")
                        End If
                    End If

                Next lSource
            Case "TAM"
                lGENERLoad = 0
                'If aReach.Id = 157 Then Stop
                For Each lSource As HspfConnection In aReach.Sources
                    Dim lGENERSum As Double = 0.0
                    Dim lMfact As Double = 0.0
                    If lSource.Source.VolName = "GENER" Then
                        Dim lGENEROperationisOutputtoWDM As Boolean = False
                        With GetGENERSum(aUCI, lSource, aSDateJ, aEDateJ)
                            lGENERSum = .Item1
                            lGENEROperationisOutputtoWDM = .Item2
                        End With
                        If lSource.MassLink > 0 Then
                            lGENERSum *= lSource.MFact
                            For Each lMassLink As HspfMassLink In aUCI.MassLinks
                                If lMassLink.MassLinkId = lSource.MassLink AndAlso lMassLink.Target.Member = "NUIF1" AndAlso lMassLink.Target.MemSub1 = 2 Then
                                    lGENERSum *= lMassLink.MFact
                                    lGENERLoad += lGENERSum
                                    Exit For
                                End If
                            Next lMassLink
                        ElseIf lSource.Target.Group = "INFLOW" AndAlso lSource.Target.Member = "NUIF1" AndAlso lSource.Target.MemSub1 = 2 Then
                            lGENERSum *= lSource.MFact
                            lGENERLoad += lGENERSum
                        End If
                        If Not lGENEROperationisOutputtoWDM Then
                            Logger.Dbg("GENER Loadings Issue: The RCHRES operation " & aReach.Id & " has loadings input for the constituent " & aConstituentName & " from GENER connections in the Network Block. Please make sure that these GENER operations output to a WDM dataset for accurate source accounting.")
                        End If
                    End If

                Next lSource
            Case "NO3"
                lGENERLoad = 0
                'If aReach.Id = 157 Then Stop
                For Each lSource As HspfConnection In aReach.Sources
                    Dim lGENERSum As Double = 0.0
                    Dim lMfact As Double = 0.0
                    If lSource.Source.VolName = "GENER" Then
                        Dim lGENEROperationisOutputtoWDM As Boolean = False
                        With GetGENERSum(aUCI, lSource, aSDateJ, aEDateJ)
                            lGENERSum = .Item1
                            lGENEROperationisOutputtoWDM = .Item2
                        End With
                        If lSource.MassLink > 0 Then
                            lGENERSum *= lSource.MFact
                            For Each lMassLink As HspfMassLink In aUCI.MassLinks
                                If lMassLink.MassLinkId = lSource.MassLink AndAlso lMassLink.Target.Member = "NUIF1" AndAlso lMassLink.Target.MemSub1 = 1 Then
                                    lGENERSum *= lMassLink.MFact
                                    lGENERLoad += lGENERSum
                                ElseIf lMassLink.MassLinkId = lSource.MassLink AndAlso lMassLink.Target.Member = "NUIF1" AndAlso lMassLink.Target.MemSub1 = 3 Then
                                    lGENERSum *= lMassLink.MFact
                                    lGENERLoad += lGENERSum
                                End If
                            Next lMassLink
                        ElseIf lSource.Target.Group = "INFLOW" AndAlso lSource.Target.Member = "NUIF1" AndAlso lSource.Target.MemSub1 = 1 Then
                            lGENERSum *= lSource.MFact
                            lGENERLoad += lGENERSum
                        ElseIf lSource.Target.Group = "INFLOW" AndAlso lSource.Target.Member = "NUIF1" AndAlso lSource.Target.MemSub1 = 3 Then
                            lGENERSum *= lSource.MFact
                            lGENERLoad += lGENERSum
                        End If
                        If Not lGENEROperationisOutputtoWDM Then
                            Logger.Dbg("GENER Loadings Issue: The RCHRES operation " & aReach.Id & " has loadings input for the constituent " & aConstituentName & " from GENER connections in the Network Block. Please make sure that these GENER operations output to a WDM dataset for accurate source accounting.")
                        End If
                    End If

                Next lSource
            Case "Heat"
                lGENERLoad = 0
                'If aReach.Id = 157 Then Stop
                For Each lSource As HspfConnection In aReach.Sources
                    Dim lGENERSum As Double = 0.0
                    Dim lMfact As Double = 0.0
                    If lSource.Source.VolName = "GENER" Then
                        Dim lGENEROperationisOutputtoWDM As Boolean = False
                        With GetGENERSum(aUCI, lSource, aSDateJ, aEDateJ)
                            lGENERSum = .Item1
                            lGENEROperationisOutputtoWDM = .Item2
                        End With
                        If lSource.MassLink > 0 Then
                            lGENERSum *= lSource.MFact
                            For Each lMassLink As HspfMassLink In aUCI.MassLinks
                                If lMassLink.MassLinkId = lSource.MassLink AndAlso lMassLink.Target.Member = "IHEAT" Then
                                    lGENERSum *= lMassLink.MFact
                                    lGENERLoad += lGENERSum
                                    Exit For
                                End If
                            Next lMassLink
                        ElseIf lSource.Target.Group = "INFLOW" AndAlso lSource.Target.Member = "IHEAT" Then
                            lGENERSum *= lSource.MFact
                            lGENERLoad += lGENERSum
                        End If
                        If Not lGENEROperationisOutputtoWDM Then
                            Logger.Dbg("GENER Loadings Issue: The RCHRES operation " & aReach.Id & " has loadings input for the constituent " & aConstituentName & " from GENER connections in the Network Block. Please make sure that these GENER operations output to a WDM dataset for accurate source accounting.")
                        End If
                    End If

                Next lSource
            Case "DO"
                lGENERLoad = 0
                'If aReach.Id = 157 Then Stop
                For Each lSource As HspfConnection In aReach.Sources
                    Dim lGENERSum As Double = 0.0
                    Dim lMfact As Double = 0.0
                    If lSource.Source.VolName = "GENER" Then
                        Dim lGENEROperationisOutputtoWDM As Boolean = False
                        With GetGENERSum(aUCI, lSource, aSDateJ, aEDateJ)
                            lGENERSum = .Item1
                            lGENEROperationisOutputtoWDM = .Item2
                        End With
                        If lSource.MassLink > 0 Then
                            lGENERSum *= lSource.MFact
                            For Each lMassLink As HspfMassLink In aUCI.MassLinks
                                If lMassLink.MassLinkId = lSource.MassLink AndAlso lMassLink.Target.Member = "OXIF" AndAlso lMassLink.Target.MemSub1 = 1 Then
                                    lGENERSum *= lMassLink.MFact
                                    lGENERLoad += lGENERSum
                                    Exit For
                                End If
                            Next lMassLink
                        ElseIf lSource.Target.Group = "INFLOW" AndAlso lSource.Target.Member = "OXIF" AndAlso lSource.Target.MemSub1 = 1 Then
                            lGENERSum *= lSource.MFact
                            lGENERLoad += lGENERSum
                        End If
                        If Not lGENEROperationisOutputtoWDM Then
                            Logger.Dbg("GENER Loadings Issue: The RCHRES operation " & aReach.Id & " has loadings input for the constituent " & aConstituentName & " from GENER connections in the Network Block. Please make sure that these GENER operations output to a WDM dataset for accurate source accounting.")
                        End If
                    End If

                Next lSource
            Case "BOD-Labile"
                lGENERLoad = 0
                'If aReach.Id = 157 Then Stop
                For Each lSource As HspfConnection In aReach.Sources
                    Dim lGENERSum As Double = 0.0
                    Dim lMfact As Double = 0.0
                    If lSource.Source.VolName = "GENER" Then
                        Dim lGENEROperationisOutputtoWDM As Boolean = False
                        With GetGENERSum(aUCI, lSource, aSDateJ, aEDateJ)
                            lGENERSum = .Item1
                            lGENEROperationisOutputtoWDM = .Item2
                        End With
                        If lSource.MassLink > 0 Then
                            lGENERSum *= lSource.MFact
                            For Each lMassLink As HspfMassLink In aUCI.MassLinks
                                If lMassLink.MassLinkId = lSource.MassLink AndAlso lMassLink.Target.Member = "OXIF" AndAlso lMassLink.Target.MemSub1 = 2 Then
                                    lGENERSum *= lMassLink.MFact
                                    lGENERLoad += lGENERSum
                                    Exit For
                                End If
                            Next lMassLink
                        ElseIf lSource.Target.Group = "INFLOW" AndAlso lSource.Target.Member = "OXIF" AndAlso lSource.Target.MemSub1 = 2 Then
                            lGENERSum *= lSource.MFact
                            lGENERLoad += lGENERSum
                        End If
                        If Not lGENEROperationisOutputtoWDM Then
                            Logger.Dbg("GENER Loadings Issue: The RCHRES operation " & aReach.Id & " has loadings input for the constituent " & aConstituentName & " from GENER connections in the Network Block. Please make sure that these GENER operations output to a WDM dataset for accurate source accounting.")
                        End If
                    End If

                Next lSource

            Case "Sediment"
                lGENERLoad = 0
                'If aReach.Id = 157 Then Stop
                For Each lSource As HspfConnection In aReach.Sources
                    Dim lGENERSum As Double = 0.0
                    Dim lMfact As Double = 0.0
                    If lSource.Source.VolName = "GENER" Then
                        Dim lGENEROperationisOutputtoWDM As Boolean = False
                        With GetGENERSum(aUCI, lSource, aSDateJ, aEDateJ)
                            lGENERSum = .Item1
                            lGENEROperationisOutputtoWDM = .Item2
                        End With
                        If lSource.MassLink > 0 Then
                            lGENERSum *= lSource.MFact
                            For Each lMassLink As HspfMassLink In aUCI.MassLinks
                                If lMassLink.MassLinkId = lSource.MassLink AndAlso lMassLink.Target.Member = "ISED" Then
                                    lGENERSum *= lMassLink.MFact
                                    lGENERLoad += lGENERSum
                                    Exit For
                                End If
                            Next lMassLink
                        ElseIf lSource.Target.Group = "INFLOW" AndAlso lSource.Target.Member = "ISED" Then
                            lGENERSum *= lSource.MFact
                            lGENERLoad += lGENERSum
                        End If
                        If Not lGENEROperationisOutputtoWDM Then
                            Logger.Dbg("GENER Loadings Issue: The RCHRES operation " & aReach.Id & " has loadings input for the constituent " & aConstituentName & " from GENER connections in the Network Block. Please make sure that these GENER operations output to a WDM dataset for accurate source accounting.")
                        End If
                    End If

                Next lSource

            Case "Water"
                lGENERLoad = 0
                'If aReach.Id = 157 Then Stop
                For Each lSource As HspfConnection In aReach.Sources
                    Dim lGENERSum As Double = 0.0
                    Dim lMfact As Double = 0.0
                    If lSource.Source.VolName = "GENER" Then
                        Dim lGENEROperationisOutputtoWDM As Boolean = False
                        With GetGENERSum(aUCI, lSource, aSDateJ, aEDateJ)
                            lGENERSum = .Item1
                            lGENEROperationisOutputtoWDM = .Item2
                        End With
                        If lSource.MassLink > 0 Then
                            lGENERSum *= lSource.MFact
                            For Each lMassLink As HspfMassLink In aUCI.MassLinks
                                If lMassLink.MassLinkId = lSource.MassLink AndAlso lMassLink.Target.Member = "IVOL" Then
                                    lGENERSum *= lMassLink.MFact
                                    lGENERLoad += lGENERSum
                                    Exit For
                                End If
                            Next lMassLink
                        ElseIf lSource.Target.Group = "INFLOW" AndAlso lSource.Target.Member = "IVOL" Then
                            lGENERSum *= lSource.MFact
                            lGENERLoad += lGENERSum
                        End If
                        If Not lGENEROperationisOutputtoWDM Then
                            Logger.Dbg("GENER Loadings Issue: The RCHRES operation " & aReach.Id & " has loadings input for the constituent " & aConstituentName & " from GENER connections in the Network Block. Please make sure that these GENER operations output to a WDM dataset for accurate source accounting.")
                        End If
                    End If

                Next lSource
            Case Else
                lGENERLoad = 0

                For Each lSource As HspfConnection In aReach.Sources
                    Dim lGENERSum As Double = 0.0
                    Dim lMfact As Double = 0.0
                    If lSource.Source.VolName = "GENER" Then
                        Dim lGENEROperationisOutputtoWDM As Boolean = False
                        With GetGENERSum(aUCI, lSource, aSDateJ, aEDateJ)
                            lGENERSum = .Item1
                            lGENEROperationisOutputtoWDM = .Item2
                        End With
                        If lSource.MassLink > 0 Then
                            lGENERSum *= lSource.MFact
                            For Each lMassLink As HspfMassLink In aUCI.MassLinks
                                If (lMassLink.MassLinkId = lSource.MassLink AndAlso lMassLink.Target.Member = "IDQAL" AndAlso lMassLink.Target.MemSub1 = aGQALID) OrElse
                                    (lMassLink.MassLinkId = lSource.MassLink AndAlso lMassLink.Target.Member = "ISQAL" AndAlso lMassLink.Target.MemSub2 = aGQALID) Then
                                    lGENERSum *= lMassLink.MFact
                                    lGENERLoad += lGENERSum
                                    Exit For
                                End If
                            Next lMassLink
                        ElseIf (lSource.Target.Group = "INFLOW" AndAlso lSource.Target.Member = "IDQAL" AndAlso lSource.Target.MemSub1 = aGQALID) OrElse
                                    (lSource.Target.Group = "INFLOW" AndAlso lSource.Target.Member = "ISQAL" AndAlso lSource.Target.MemSub2 = aGQALID) Then
                            lGENERSum *= lSource.MFact
                            lGENERLoad += lGENERSum
                        End If
                        If Not lGENEROperationisOutputtoWDM Then
                            Logger.Dbg("GENER Loadings Issue: The RCHRES operation " & aReach.Id & " has loadings input for the constituent " & aConstituentName & " from GENER connections in the Network Block. Please make sure that these GENER operations output to a WDM dataset for accurate source accounting.")
                        End If
                    End If

                Next lSource
        End Select

        Return lGENERLoad
    End Function

    Private Function AddFirstSixColumnsReachBudget(ByRef aDataTable As Data.DataTable, ByRef aUnits As String) As DataTable
        Dim lColumn As DataColumn
        lColumn = New DataColumn()
        lColumn.DataType = Type.GetType("System.String")
        lColumn.ColumnName = "OpTypeNumber"
        lColumn.Caption = "Operation Type & Number"
        aDataTable.Columns.Add(lColumn)

        lColumn = New DataColumn()
        lColumn.DataType = Type.GetType("System.String")
        lColumn.ColumnName = "OpDesc"
        lColumn.Caption = "Operation Description"
        aDataTable.Columns.Add(lColumn)

        lColumn = New DataColumn()
        lColumn.DataType = Type.GetType("System.Double")
        lColumn.ColumnName = "NPSLoad"
        lColumn.Caption = "Nonpoint Source Loads (" & aUnits & ")"
        aDataTable.Columns.Add(lColumn)

        lColumn = New DataColumn()
        lColumn.DataType = Type.GetType("System.Double")
        lColumn.ColumnName = "PSLoad"
        lColumn.Caption = "Point Source Loads (" & aUnits & ")"
        aDataTable.Columns.Add(lColumn)

        lColumn = New DataColumn()
        lColumn.DataType = Type.GetType("System.Double")
        lColumn.ColumnName = "GENERLoad"
        lColumn.Caption = "GENER Loads (" & aUnits & ")"
        aDataTable.Columns.Add(lColumn)

        lColumn = New DataColumn()
        lColumn.DataType = Type.GetType("System.Double")
        lColumn.ColumnName = "MassBalance"
        lColumn.Caption = "Mass Balance (" & aUnits & ")"
        aDataTable.Columns.Add(lColumn)

        lColumn = New DataColumn()
        lColumn.DataType = Type.GetType("System.Double")
        lColumn.ColumnName = "Diversion"
        lColumn.Caption = "Diversion (" & aUnits & ")"
        aDataTable.Columns.Add(lColumn)

        lColumn = New DataColumn()
        lColumn.DataType = Type.GetType("System.Double")
        lColumn.ColumnName = "UpstreamIn"
        lColumn.Caption = "Upstream Load (" & aUnits & ")"
        aDataTable.Columns.Add(lColumn)

        Return aDataTable
    End Function

    Private Function AddFirstThreeColumnsLandLoading(ByRef aDataTable As Data.DataTable) As DataTable
        Dim lColumn As DataColumn
        lColumn = New DataColumn()
        lColumn.DataType = Type.GetType("System.String")
        lColumn.ColumnName = "OpTypeNumber"
        lColumn.Caption = "Operation Type & Number"
        aDataTable.Columns.Add(lColumn)

        lColumn = New DataColumn()
        lColumn.DataType = Type.GetType("System.String")
        lColumn.ColumnName = "OpDesc"
        lColumn.Caption = "Operation Description"
        aDataTable.Columns.Add(lColumn)

        lColumn = New DataColumn()
        lColumn.DataType = Type.GetType("System.String")
        lColumn.ColumnName = "Year"
        lColumn.Caption = "Year"
        aDataTable.Columns.Add(lColumn)

        Return aDataTable
    End Function

    Private Function AddMonthlyColumnsColumns(ByRef aDataTable As Data.DataTable) As DataTable
        Dim lColumn As DataColumn
        lColumn = New DataColumn()
        lColumn.DataType = Type.GetType("System.Double")
        lColumn.ColumnName = "Mean Month 01 Jan"
        lColumn.Caption = "Jan"
        aDataTable.Columns.Add(lColumn)

        lColumn = New DataColumn()
        lColumn.DataType = Type.GetType("System.Double")
        lColumn.ColumnName = "Mean Month 02 Feb"
        lColumn.Caption = "Feb"
        aDataTable.Columns.Add(lColumn)

        lColumn = New DataColumn()
        lColumn.DataType = Type.GetType("System.Double")
        lColumn.ColumnName = "Mean Month 03 Mar"
        lColumn.Caption = "Mar"
        aDataTable.Columns.Add(lColumn)

        lColumn = New DataColumn()
        lColumn.DataType = Type.GetType("System.Double")
        lColumn.ColumnName = "Mean Month 04 Apr"
        lColumn.Caption = "Apr"
        aDataTable.Columns.Add(lColumn)

        lColumn = New DataColumn()
        lColumn.DataType = Type.GetType("System.Double")
        lColumn.ColumnName = "Mean Month 05 May"
        lColumn.Caption = "May"
        aDataTable.Columns.Add(lColumn)

        lColumn = New DataColumn()
        lColumn.DataType = Type.GetType("System.Double")
        lColumn.ColumnName = "Mean Month 06 Jun"
        lColumn.Caption = "Jun"
        aDataTable.Columns.Add(lColumn)

        lColumn = New DataColumn()
        lColumn.DataType = Type.GetType("System.Double")
        lColumn.ColumnName = "Mean Month 07 Jul"
        lColumn.Caption = "Jul"
        aDataTable.Columns.Add(lColumn)

        lColumn = New DataColumn()
        lColumn.DataType = Type.GetType("System.Double")
        lColumn.ColumnName = "Mean Month 08 Aug"
        lColumn.Caption = "Aug"
        aDataTable.Columns.Add(lColumn)

        lColumn = New DataColumn()
        lColumn.DataType = Type.GetType("System.Double")
        lColumn.ColumnName = "Mean Month 09 Sep"
        lColumn.Caption = "Sep"
        aDataTable.Columns.Add(lColumn)

        lColumn = New DataColumn()
        lColumn.DataType = Type.GetType("System.Double")
        lColumn.ColumnName = "Mean Month 10 Oct"
        lColumn.Caption = "Oct"
        aDataTable.Columns.Add(lColumn)

        lColumn = New DataColumn()
        lColumn.DataType = Type.GetType("System.Double")
        lColumn.ColumnName = "Mean Month 11 Nov"
        lColumn.Caption = "Nov"
        aDataTable.Columns.Add(lColumn)

        lColumn = New DataColumn()
        lColumn.DataType = Type.GetType("System.Double")
        lColumn.ColumnName = "Mean Month 12 Dec"
        lColumn.Caption = "Dec"
        aDataTable.Columns.Add(lColumn)

        lColumn = New DataColumn()
        lColumn.DataType = Type.GetType("System.Double")
        lColumn.ColumnName = "SumAnnual"
        lColumn.Caption = "Sum Annual"
        aDataTable.Columns.Add(lColumn)
        Return aDataTable
    End Function

    Private Function GetGENERSum(ByVal aUCI As HspfUci, ByVal aSource As HspfConnection, ByVal aSDateJ As Double, ByVal aEDateJ As Double) As Tuple(Of Double, Boolean)
        Dim aGenerSum As Double = 0
        Dim aGENERID As Integer = aSource.Source.VolId
        Dim aGENEROperationisOutputtoWDM As Boolean = False
        Dim aGENEROperation As HspfOperation = aSource.Source.Opn
        If Not aGENEROperation Is Nothing Then
            For Each lEXTTarget As HspfConnection In aGENEROperation.Targets
                If lEXTTarget.Target.VolName.Contains("WDM") Then
                    aGENEROperationisOutputtoWDM = True
                    Dim lWDMFile As String = lEXTTarget.Target.VolName.ToString
                    Dim lDSN As Integer = lEXTTarget.Target.VolId
                    For i As Integer = 0 To aUCI.FilesBlock.Count
                        If aUCI.FilesBlock.Value(i).Typ = lWDMFile Then
                            Dim lFileName As String = AbsolutePath(aUCI.FilesBlock.Value(i).Name.Trim, CurDir())
                            Dim lDataSource As atcDataSource = atcDataManager.DataSourceBySpecification(lFileName)
                            If lDataSource Is Nothing Then
                                If atcDataManager.OpenDataSource(lFileName) Then
                                    lDataSource = atcDataManager.DataSourceBySpecification(lFileName)
                                End If
                            End If
                            Dim lTimeseries As atcTimeseries = lDataSource.DataSets.FindData("ID", lDSN)(0)
                            lTimeseries = SubsetByDate(lTimeseries, aSDateJ, aEDateJ, Nothing)
                            aGenerSum = lTimeseries.Attributes.GetDefinedValue("Sum").Value / YearCount(aSDateJ, aEDateJ)

                        End If
                    Next
                End If
            Next lEXTTarget
        End If

        Return New Tuple(Of Double, Boolean)(aGenerSum, aGENEROperationisOutputtoWDM)
    End Function

End Module
