Imports atcUtility
Imports atcData
Imports MapWinUtility
Imports atcUCI
Imports System.Data
Public Module atcConstituentTables
    Public Land_Constituent_Table As DataTable
    Public Reach_Budget_Table As DataTable

    Public Sub LandLoadingReports(ByVal aoutfoldername As String,
                                     ByVal aBinaryData As atcDataSource,
                                     ByVal aUCI As HspfUci,
                                     ByVal aScenario As String,
                                     ByVal aRunMade As String,
                                     ByVal aBalanceType As String,
                                     aConstProperties As List(Of ConstituentProperties),
                                  aSDateJ As Double, aEDateJ As Double)

        'This Sub prepares a text report for constituents like TN and TP.
        Dim lReport As New atcReport.ReportText
        Dim lReport_Monthly As New atcReport.ReportText
        Land_Constituent_Table = New DataTable("LandConstituentTable")
        Dim Land_Constituent_Monthly_Table As New DataTable("LandConstituentMonthlyTable")
        Dim QualityConstituent As Boolean = False
        'Dim lOutflowDataTypes As String() = ConstituentList(aBalanceType, QualityConstituent)
        Dim lDataForBoxWhiskerPlot As New BoxWhiskerItem
        lDataForBoxWhiskerPlot.Constituent = aBalanceType
        lDataForBoxWhiskerPlot.Scenario = aScenario
        lDataForBoxWhiskerPlot.TimeSpan = TimeSpanAsString(aSDateJ, aEDateJ, "Analysis Period: ")
        Dim listLanduses As New List(Of String)
        Dim landUseSumAnnualValues As New atcCollection
        Dim landUseNameForTheCollection As String = ""
        Dim lUnits As String = ""

        Land_Constituent_Table = AddFirstThreeColumnsLandLoading(Land_Constituent_Table)
        Land_Constituent_Monthly_Table = AddFirstThreeColumnsLandLoading(Land_Constituent_Monthly_Table)
        Land_Constituent_Monthly_Table.Columns.Remove("Year")
        Select Case aBalanceType
#Region "Case Water"
            Case "Water"

                If aUCI.GlobalBlock.EmFg = 1 Then
                    lUnits = "In"
                Else
                    lUnits = "mm"
                End If
                lDataForBoxWhiskerPlot.Units = (lUnits & "/yr")

                Dim column As DataColumn
                Dim row As DataRow

                Land_Constituent_Monthly_Table = AddMonthlyColumnsColumns(Land_Constituent_Monthly_Table)
                column = New DataColumn()
                column.DataType = Type.GetType("System.String")
                column.ColumnName = "SUPY"
                column.Caption = "Rainfall (" & lUnits & ")"
                Land_Constituent_Table.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.String")
                column.ColumnName = "IRRAPP6"
                column.Caption = "Irrigation (" & lUnits & ")"
                Land_Constituent_Table.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.Double")
                column.ColumnName = "SURO"
                column.Caption = "Surface Outflow (" & lUnits & ")"
                Land_Constituent_Table.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.Double")
                column.ColumnName = "IFWO"
                column.Caption = "Interflow Outflow (" & lUnits & ")"
                Land_Constituent_Table.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.Double")
                column.ColumnName = "AGWO"
                column.Caption = "Groundwater Outflow (" & lUnits & ")"
                Land_Constituent_Table.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.Double")
                column.ColumnName = "TotalOutflow"
                column.Caption = "Total Outflow (" & lUnits & ")"
                Land_Constituent_Table.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.Double")
                column.ColumnName = "IGWI"
                column.Caption = "Deep Groundwater Flow (" & lUnits & ")"
                Land_Constituent_Table.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.Double")
                column.ColumnName = "AGWI"
                column.Caption = "Active Groundwater (" & lUnits & ")"
                Land_Constituent_Table.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.Double")
                column.ColumnName = "AGWLI"
                column.Caption = "Pumping (" & lUnits & ")"
                Land_Constituent_Table.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.Double")
                column.ColumnName = "PET"
                column.Caption = "Potential Evapotranspiration (" & lUnits & ")"
                Land_Constituent_Table.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.Double")
                column.ColumnName = "CEPE"
                column.Caption = "Interception Storage (" & lUnits & ")"
                Land_Constituent_Table.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.Double")
                column.ColumnName = "UZET"
                column.Caption = "Upper Zone Storage (" & lUnits & ")"
                Land_Constituent_Table.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.Double")
                column.ColumnName = "LZET"
                column.Caption = "Lower Zone Storage (" & lUnits & ")"
                Land_Constituent_Table.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.Double")
                column.ColumnName = "AGWET"
                column.Caption = "Ground Water Storage (" & lUnits & ")"
                Land_Constituent_Table.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.Double")
                column.ColumnName = "BASET"
                column.Caption = "Baseflow (" & lUnits & ")"
                Land_Constituent_Table.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.Double")
                column.ColumnName = "TAET"
                column.Caption = "Total Evapotranspiration (" & lUnits & ")"
                Land_Constituent_Table.Columns.Add(column)

                Dim RowNumber As Integer = 0

                For Each lOperation As HspfOperation In aUCI.OpnSeqBlock.Opns
                    If Not (lOperation.Name = "PERLND" OrElse lOperation.Name = "IMPLND") Then Continue For
                    Dim LocationName As String = lOperation.Name.Substring(0, 1) & ":" & lOperation.Id
                    landUseNameForTheCollection = lOperation.Name.Substring(0, 1) & ":" & lOperation.Description
                    Logger.Dbg(LocationName)
                    If Not listLanduses.Contains(landUseNameForTheCollection) Then
                        listLanduses.Add(landUseNameForTheCollection)
                    End If

                    Dim lTSNumber As Integer = 0
                    Dim lOutflowDataTypes1 As Dictionary(Of String, String) = ConstituentList(aBalanceType)
                    Dim lTS As New atcTimeseries(Nothing)
                    Dim AddTS As New atcDataGroup
                    Dim lTotalTS As New atcTimeseries(Nothing)
                    For Each lOutflowDataType As String In lOutflowDataTypes1.Keys
                        Dim lMasslinkFactor As Double = 1.0
                        If lOutflowDataType = "TotalOutflow" Then
                            lTS = lTotalTS
                            Dim lTsMonthly As atcTimeseries = Aggregate(lTS, atcTimeUnit.TUMonth, 1, atcTran.TranSumDiv)
                            Dim lSeasons As New atcSeasonsMonth
                            Dim lSeasonalAttributes As New atcDataAttributes
                            lSeasonalAttributes.SetValue("Mean", 0)
                            Dim lNewSimTSerMonthCalculatedAttributes As New atcDataAttributes
                            If lTsMonthly IsNot Nothing Then
                                lSeasons.SetSeasonalAttributes(lTsMonthly, lSeasonalAttributes, lNewSimTSerMonthCalculatedAttributes)
                            End If
                            row = Land_Constituent_Monthly_Table.NewRow

                            row("OpTypeNumber") = LocationName
                            row("OpDesc") = lOperation.Description
                            'row("Unit") = lUnits

                            For Each key As String In lNewSimTSerMonthCalculatedAttributes.ValuesSortedByName.Keys
                                row(key) = HspfTable.NumFmtRE(lNewSimTSerMonthCalculatedAttributes.GetDefinedValue(key).Value, 10)
                            Next
                            row("SumAnnual") = lTS.Attributes.GetDefinedValue("SumAnnual").Value
                            Land_Constituent_Monthly_Table.Rows.Add(row)
                        Else
                            lTS = aBinaryData.DataSets.FindData("Location", LocationName).FindData("Constituent", lOutflowDataType)(0)
                            If lTS Is Nothing Then Continue For
                            lTS = SubsetByDate(lTS, aSDateJ, aEDateJ, Nothing)
                            If ConstituentsThatNeedMassLink.Contains(lOutflowDataType) Then
                                For Each lConnection As HspfConnection In lOperation.Targets
                                    If lConnection.Target.VolName = "RCHRES" Then
                                        Dim aReach As HspfOperation = aUCI.OpnBlks("RCHRES").OperFromID(lConnection.Target.VolId)

                                        Dim lMassLinkID As Integer = lConnection.MassLink
                                        If Not lMassLinkID = 0 Then
                                            lMasslinkFactor = FindMassLinkFactor(aUCI, lMassLinkID, lOutflowDataType,
                                                                             aBalanceType, 0, 0)
                                            Exit For
                                        End If

                                    End If
                                Next lConnection
                                lMasslinkFactor *= 12 'Converting feet to inches
                            End If

                        End If
                        lTS *= lMasslinkFactor
                        Dim lTSAttributes As String = lTS.Attributes.GetDefinedValue("Constituent").Value

                        If (lTSAttributes = "SURO" Or lTSAttributes = "IFWO" Or lTSAttributes = "AGWO") Then
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

                            row = Land_Constituent_Table.NewRow
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
                                row("OpTypeNumber") = LocationName
                                row("OpDesc") = lOperation.Description
                                row("Year") = Year
                                row("SUPY") = lValue
                                Land_Constituent_Table.Rows.Add(row)
                            Else
                                Land_Constituent_Table.Rows(RowNumber - 1)(lTSAttributes) = HspfTable.NumFmtRE(lValue, 10)
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

                Dim column As DataColumn
                Dim row As DataRow
                Land_Constituent_Monthly_Table = AddMonthlyColumnsColumns(Land_Constituent_Monthly_Table)
                column = New DataColumn()
                column.DataType = Type.GetType("System.Double")
                column.ColumnName = "SO"
                column.Caption = "Surface Outflow (" & lUnits & ")"
                Land_Constituent_Table.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.Double")
                column.ColumnName = "IO"
                column.Caption = "Interflow Outflow"
                Land_Constituent_Table.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.Double")
                column.ColumnName = "AO"
                column.Caption = "Groundwater Outflow (" & lUnits & ")"
                Land_Constituent_Table.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.Double")
                column.ColumnName = "TotalOutflow"
                column.Caption = "Total Outflow (" & lUnits & ")"
                Land_Constituent_Table.Columns.Add(column)
                Dim RowNumber As Integer = 0

                For Each lOperation As HspfOperation In aUCI.OpnSeqBlock.Opns
                    If Not (lOperation.Name = "PERLND" OrElse lOperation.Name = "IMPLND") Then Continue For
                    Dim LocationName As String = lOperation.Name.Substring(0, 1) & ":" & lOperation.Id
                    landUseNameForTheCollection = lOperation.Name.Substring(0, 1) & ":" & lOperation.Description
                    If Not listLanduses.Contains(landUseNameForTheCollection) Then
                        listLanduses.Add(landUseNameForTheCollection)
                    End If

                    Dim lTSNumber As Integer = 0
                    Dim lOutflowDataTypes1 As Dictionary(Of String, String) = ConstituentList(aBalanceType)
                    Dim lTS As New atcTimeseries(Nothing)
                    Dim AddTS As New atcDataGroup
                    Dim lTotalTS As New atcTimeseries(Nothing)
                    For Each lOutflowDataType As String In lOutflowDataTypes1.Keys
                        Dim lMasslinkFactor As Double = 1.0
                        If lOutflowDataType = "TotalOutflow" Then
                            lTS = lTotalTS
                            Dim lTsMonthly As atcTimeseries = Aggregate(lTS, atcTimeUnit.TUMonth, 1, atcTran.TranSumDiv)
                            Dim lSeasons As New atcSeasonsMonth
                            Dim lSeasonalAttributes As New atcDataAttributes
                            lSeasonalAttributes.SetValue("Mean", 0)
                            Dim lNewSimTSerMonthCalculatedAttributes As New atcDataAttributes
                            If lTsMonthly IsNot Nothing Then
                                lSeasons.SetSeasonalAttributes(lTsMonthly, lSeasonalAttributes, lNewSimTSerMonthCalculatedAttributes)
                            End If
                            row = Land_Constituent_Monthly_Table.NewRow

                            row("OpTypeNumber") = LocationName
                            row("OpDesc") = lOperation.Description
                            'row("Unit") = lUnits

                            For Each key As String In lNewSimTSerMonthCalculatedAttributes.ValuesSortedByName.Keys
                                row(key) = HspfTable.NumFmtRE(lNewSimTSerMonthCalculatedAttributes.GetDefinedValue(key).Value, 10)
                            Next
                            row("SumAnnual") = lTS.Attributes.GetDefinedValue("SumAnnual").Value
                            Land_Constituent_Monthly_Table.Rows.Add(row)
                        Else
                            lTS = aBinaryData.DataSets.FindData("Location", LocationName).FindData("Constituent", lOutflowDataType)(0)
                            If lTS Is Nothing Then Continue For
                            lTS = SubsetByDate(lTS, aSDateJ, aEDateJ, Nothing)
                            If ConstituentsThatNeedMassLink.Contains(lOutflowDataType) Then
                                For Each lConnection As HspfConnection In lOperation.Targets
                                    If lConnection.Target.VolName = "RCHRES" Then
                                        Dim aReach As HspfOperation = aUCI.OpnBlks("RCHRES").OperFromID(lConnection.Target.VolId)

                                        Dim lMassLinkID As Integer = lConnection.MassLink
                                        If Not lMassLinkID = 0 Then
                                            lMasslinkFactor = FindMassLinkFactor(aUCI, lMassLinkID, lOutflowDataType,
                                                                             aBalanceType, 0, 0)
                                            Exit For
                                        End If

                                    End If
                                Next lConnection
                            End If
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

                            row = Land_Constituent_Table.NewRow
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
                                row("OpTypeNumber") = LocationName
                                row("OpDesc") = lOperation.Description
                                row("Year") = Year
                                row("SO") = lValue
                                Land_Constituent_Table.Rows.Add(row)
                            Else
                                Land_Constituent_Table.Rows(RowNumber - 1)(lTSAttributes) = HspfTable.NumFmtRE(lValue, 10)
                            End If
                        Next i
                        lTSNumber += 1
                    Next lOutflowDataType
                Next lOperation
#End Region

#Region "Case Sediment"
            Case "Sediment"
                Dim lConversionFactor As Double = 1.0
                If aUCI.GlobalBlock.EmFg = 1 Then
                    lUnits = "lbs/ac"
                    lConversionFactor = 2000 'English tons to lbs - US needs to move to SI units
                ElseIf aUCI.GlobalBlock.EmFg = 2 Then
                    lUnits = "kgs/ha"
                    lConversionFactor = 1000
                End If
                lDataForBoxWhiskerPlot.Units = (lUnits & "/yr")

                Dim column As New DataColumn
                Dim row As DataRow
                Land_Constituent_Monthly_Table = AddMonthlyColumnsColumns(Land_Constituent_Monthly_Table)
                column = New DataColumn()
                column.DataType = Type.GetType("System.Double")
                column.ColumnName = "WSSD"
                column.Caption = "Wash Off of detached Sediment (" & lUnits & ")"
                Land_Constituent_Table.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.Double")
                column.ColumnName = "SCRSD"
                column.Caption = "Scour of Matrix Soil (" & lUnits & ")"
                Land_Constituent_Table.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.Double")
                column.ColumnName = "TotalOutflow"
                column.Caption = "Total Outflow (" & lUnits & ")"
                Land_Constituent_Table.Columns.Add(column)

                Dim RowNumber As Integer = 0
                For Each lOperation As HspfOperation In aUCI.OpnSeqBlock.Opns
                    'If lOperation.Name = "IMPLND" Then Stop

                    If Not ((lOperation.Name = "PERLND" AndAlso lOperation.Tables("ACTIVITY").Parms("SEDFG").Value = "1") OrElse
                            (lOperation.Name = "IMPLND" AndAlso lOperation.Tables("ACTIVITY").Parms("SLDFG").Value = "1")) Then Continue For
                    'If lOperation.Name = "IMPLND" Then Stop
                    Dim LocationName As String = lOperation.Name.Substring(0, 1) & ":" & lOperation.Id
                    landUseNameForTheCollection = lOperation.Name.Substring(0, 1) & ":" & lOperation.Description
                    If Not listLanduses.Contains(landUseNameForTheCollection) Then
                        listLanduses.Add(landUseNameForTheCollection)
                    End If

                    Dim lTSNumber As Integer = 0
                    Dim lOutflowDataTypes1 As Dictionary(Of String, String) = ConstituentList(aBalanceType)
                    Dim lTS As New atcTimeseries(Nothing)
                    Dim AddTS As New atcDataGroup
                    Dim lTotalTS As New atcTimeseries(Nothing)
                    For Each lOutflowDataType As String In lOutflowDataTypes1.Keys
                        Dim lMasslinkFactor As Double = 1.0
                        If lOutflowDataType = "TotalOutflow" Then
                            lTS = lTotalTS
                            Dim lTsMonthly As atcTimeseries = Aggregate(lTS, atcTimeUnit.TUMonth, 1, atcTran.TranSumDiv)
                            Dim lSeasons As New atcSeasonsMonth
                            Dim lSeasonalAttributes As New atcDataAttributes
                            lSeasonalAttributes.SetValue("Mean", 0)
                            Dim lNewSimTSerMonthCalculatedAttributes As New atcDataAttributes
                            If lTsMonthly IsNot Nothing Then
                                lSeasons.SetSeasonalAttributes(lTsMonthly, lSeasonalAttributes, lNewSimTSerMonthCalculatedAttributes)
                            End If
                            row = Land_Constituent_Monthly_Table.NewRow

                            row("OpTypeNumber") = LocationName
                            row("OpDesc") = lOperation.Description
                            'row("Unit") = lUnits

                            For Each key As String In lNewSimTSerMonthCalculatedAttributes.ValuesSortedByName.Keys
                                row(key) = HspfTable.NumFmtRE(lNewSimTSerMonthCalculatedAttributes.GetDefinedValue(key).Value, 10)
                            Next
                            row("SumAnnual") = lTS.Attributes.GetDefinedValue("SumAnnual").Value
                            Land_Constituent_Monthly_Table.Rows.Add(row)
                        Else
                            lTS = aBinaryData.DataSets.FindData("Location", LocationName).FindData("Constituent", lOutflowDataType)(0)
                            If lTS Is Nothing Then Continue For
                            lTS = SubsetByDate(lTS, aSDateJ, aEDateJ, Nothing)
                            If ConstituentsThatNeedMassLink.Contains(lOutflowDataType) Then
                                For Each lConnection As HspfConnection In lOperation.Targets
                                    If lConnection.Target.VolName = "RCHRES" Then
                                        Dim aReach As HspfOperation = aUCI.OpnBlks("RCHRES").OperFromID(lConnection.Target.VolId)

                                        Dim lMassLinkID As Integer = lConnection.MassLink
                                        If Not lMassLinkID = 0 Then
                                            lMasslinkFactor = FindMassLinkFactor(aUCI, lMassLinkID, lOutflowDataType,
                                                                             aBalanceType, 0, 0)
                                            Exit For
                                        End If

                                    End If
                                Next lConnection

                            End If

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

                        If lTSNumber > 0 Then RowNumber -= (lTsYearly.numValues + 1)
                        If lTSAttributes = "SOSLD" Then lTSAttributes = "WSSD"
                        For i As Integer = 1 To lTsYearly.numValues + 1
                            row = Land_Constituent_Table.NewRow
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
                                row("OpTypeNumber") = LocationName
                                row("OpDesc") = lOperation.Description
                                row("Year") = Year
                                row("WSSD") = lValue
                                Land_Constituent_Table.Rows.Add(row)
                            Else
                                Land_Constituent_Table.Rows(RowNumber - 1)(lTSAttributes) = HspfTable.NumFmtRE(lValue, 10)
                            End If
                        Next i
                        lTSNumber += 1
                    Next lOutflowDataType
                Next lOperation
#End Region

#Region "Case Else"
            Case Else

                Dim column As DataColumn
                'Dim columnMonthly As DataColumn
                Dim row As DataRow

                column = New DataColumn()
                column.DataType = Type.GetType("System.String")
                column.ColumnName = "ConstName"
                column.Caption = "Constituent Name"
                Land_Constituent_Table.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.String")
                column.ColumnName = "ConstName"
                column.Caption = "Constituent Name"
                Land_Constituent_Monthly_Table.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.String")
                column.ColumnName = "ConstNameEXP"
                column.Caption = "Constituent Name in EXP+"
                Land_Constituent_Table.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.String")
                column.ColumnName = "ConstNameEXP"
                column.Caption = "Constituent Name in EXP+"
                Land_Constituent_Monthly_Table.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.String")
                column.ColumnName = "Unit"
                column.Caption = "Unit"
                Land_Constituent_Table.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.String")
                column.ColumnName = "Unit"
                column.Caption = "Unit"
                Land_Constituent_Monthly_Table.Columns.Add(column)

                Land_Constituent_Monthly_Table = AddMonthlyColumnsColumns(Land_Constituent_Monthly_Table)
                'column = New DataColumn()
                'column.DataType = Type.GetType("System.Double")
                'column.ColumnName = "ATDEP"
                'column.Caption = "Atmospheric Deposition"
                'QUALConstTable.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.Double")
                column.ColumnName = "WASHQS"
                column.Caption = "Removal of QUALSD by association with detached sediment Runoff"
                Land_Constituent_Table.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.Double")
                column.ColumnName = "SCRQS"
                column.Caption = "Removal of QUALSD with scour of matrix soil"
                Land_Constituent_Table.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.Double")
                column.ColumnName = "SOQO"
                column.Caption = "Washoff of QUALOF from surface"
                Land_Constituent_Table.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.Double")
                column.ColumnName = "IOQUAL"
                column.Caption = "Outflow of QUAL in interflow"
                Land_Constituent_Table.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.Double")
                column.ColumnName = "AOQUAL"
                column.Caption = "Outflow of QUAL in Groundwater flow"
                Land_Constituent_Table.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.Double")
                column.ColumnName = "TotalOutflow"
                column.Caption = "Total Outflow"
                Land_Constituent_Table.Columns.Add(column)
                Dim RowNumber As Integer = 0
                Dim lConstituentNames As New List(Of String)
                Dim lOperationNameNumber As New List(Of String)
                Dim lYears As New List(Of String)
                For Each lOperation As HspfOperation In aUCI.OpnSeqBlock.Opns
                    If Not ((lOperation.Name = "PERLND" AndAlso lOperation.Tables("ACTIVITY").Parms("PQALFG").Value = "1") OrElse
                        (lOperation.Name = "IMPLND" AndAlso lOperation.Tables("ACTIVITY").Parms("IQALFG").Value = "1")) Then Continue For
                    'If lOperation.Name = "IMPLND" Then Stop
                    Dim LocationName As String = lOperation.Name.Substring(0, 1) & ":" & lOperation.Id

                    'If lOperation.Tables("ACTIVITY").Parms("PQUALFG").Value = "0" Then Continue For

                    lOperationNameNumber.Add(LocationName)

                    landUseNameForTheCollection = lOperation.Name.Substring(0, 1) & ":" & lOperation.Description
                    'Look at this. Do not want operation id with this
                    If Not listLanduses.Contains(landUseNameForTheCollection) Then
                        listLanduses.Add(landUseNameForTheCollection)
                    End If

                    For Each constituent As ConstituentProperties In aConstProperties
                        Dim lMultipleIndex As Integer = 0
                        If constituent.ConstNameForEXPPlus.ToLower.Contains("ref") Then
                            lMultipleIndex = 1
                        ElseIf constituent.ConstNameForEXPPlus.ToLower.Contains("lab") Then
                            lMultipleIndex = 2
                        End If

                        If Not lConstituentNames.Contains(constituent.ConstNameForEXPPlus) Then
                            lConstituentNames.Add(constituent.ConstNameForEXPPlus)
                        End If

                        Dim lOutflowDataTypes1 As Dictionary(Of String, String) = ConstituentList(aBalanceType, constituent.ConstituentNameInUCI, constituent.ConstNameForEXPPlus)
                        Dim lTSNumber As Integer = 0
                        Dim lTS As New atcTimeseries(Nothing)
                        Dim AddTS As New atcDataGroup
                        Dim lTotalTS As New atcTimeseries(Nothing)
                        For Each lOutflowDataType As String In lOutflowDataTypes1.Keys
                            Dim lMassLinkFactor As Double = 1.0
                            If lOutflowDataType.StartsWith("TotalOutflow") And lTotalTS.Dates IsNot Nothing Then
                                lTS = lTotalTS
                                'Start doing the montly calculations here.
                                Dim lTsMonthly As atcTimeseries = Aggregate(lTS, atcTimeUnit.TUMonth, 1, atcTran.TranSumDiv)
                                Dim lSeasons As New atcSeasonsMonth
                                Dim lSeasonalAttributes As New atcDataAttributes
                                lSeasonalAttributes.SetValue("Mean", 0)
                                Dim lNewSimTSerMonthCalculatedAttributes As New atcDataAttributes
                                If lTsMonthly IsNot Nothing Then
                                    lSeasons.SetSeasonalAttributes(lTsMonthly, lSeasonalAttributes, lNewSimTSerMonthCalculatedAttributes)
                                End If
                                row = Land_Constituent_Monthly_Table.NewRow

                                row("OpTypeNumber") = LocationName
                                row("OpDesc") = lOperation.Description
                                row("ConstName") = constituent.ConstituentNameInUCI
                                row("ConstNameEXP") = constituent.ConstNameForEXPPlus
                                row("Unit") = constituent.ConstituentUnit

                                For Each key As String In lNewSimTSerMonthCalculatedAttributes.ValuesSortedByName.Keys
                                    row(key) = HspfTable.NumFmtRE(lNewSimTSerMonthCalculatedAttributes.GetDefinedValue(key).Value, 10)
                                Next
                                row("SumAnnual") = lTS.Attributes.GetDefinedValue("SumAnnual").Value
                                Land_Constituent_Monthly_Table.Rows.Add(row)
                            Else
                                lTS = aBinaryData.DataSets.FindData("Location", LocationName).FindData("Constituent", lOutflowDataTypes1(lOutflowDataType))(0)

                                If lTS Is Nothing Then Continue For
                                lTS = SubsetByDate(lTS, aSDateJ, aEDateJ, Nothing)
                                For Each lConnection As HspfConnection In lOperation.Targets
                                    If lConnection.Target.VolName = "RCHRES" Then
                                        Dim aReach As HspfOperation = aUCI.OpnBlks("RCHRES").OperFromID(lConnection.Target.VolId)
                                        Dim aConversionFactor As Double = 0.0
                                        If aBalanceType = "TotalN" Or aBalanceType = "TotalP" Then
                                            aConversionFactor = ConversionFactorfromOxygen(aUCI, constituent.ReportType, aReach)

                                            Dim lMassLinkID As Integer = lConnection.MassLink
                                            If Not lMassLinkID = 0 Then
                                                lMassLinkFactor = FindMassLinkFactor(aUCI, lMassLinkID, lOutflowDataType,
                                                                             constituent.ReportType, aConversionFactor, lMultipleIndex)
                                                Exit For
                                            End If
                                        End If
                                    End If
                                Next lConnection
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

                            If lTSNumber > 0 Then RowNumber -= (lTsYearly.numValues + 1)

                            For i As Integer = 1 To lTsYearly.numValues + 1

                                row = Land_Constituent_Table.NewRow
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
                                If Not lYears.Contains(Year) Then
                                    lYears.Add(Year)
                                End If
                                RowNumber += 1
                                If lTSNumber = 0 Then
                                    row("OpTypeNumber") = LocationName
                                    row("OpDesc") = lOperation.Description
                                    row("Year") = Year
                                    row("ConstName") = constituent.ConstituentNameInUCI
                                    row("ConstNameEXP") = constituent.ConstNameForEXPPlus
                                    row("Unit") = constituent.ConstituentUnit
                                    'row("WASHQS") = lValue
                                    row(lTSAttributes.Split("-")(0)) = lValue
                                    Land_Constituent_Table.Rows.Add(row)
                                Else
                                    Land_Constituent_Table.Rows(RowNumber - 1)(lTSAttributes.Split("-")(0)) = HspfTable.NumFmtRE(lValue, 10)
                                End If
                            Next i
                            lTSNumber += 1
                        Next lOutflowDataType
                    Next constituent

                Next lOperation
#End Region
                If aConstProperties.Count > 1 Then
                    For Each lOperation As String In lOperationNameNumber 'Summing constituents of TN and TP
                        For Each lYear As String In lYears
                            Dim SelectExpression As String = "OpTypeNumber = '" & lOperation & "' And Year = '" & lYear & "'"
                            Dim foundRows() As DataRow = Land_Constituent_Table.Select(SelectExpression)
                            row = Land_Constituent_Table.NewRow
                            'Logger.Dbg(SelectExpression)
                            row("OpTypeNumber") = foundRows(0)("OpTypeNumber")
                            row("OpDesc") = foundRows(0)("OpDesc")
                            row("Year") = foundRows(0)("Year")
                            row("ConstName") = aBalanceType
                            row("ConstNameEXP") = aBalanceType
                            row("Unit") = foundRows(0)("Unit")

                            For Each foundrow As DataRow In foundRows
                                For i As Integer = 6 To foundrow.ItemArray.Length - 1
                                    If IsDBNull(row(i)) AndAlso Not IsDBNull(foundrow(i)) Then
                                        row(i) = foundrow(i)

                                    ElseIf Not IsDBNull(row(i)) AndAlso Not IsDBNull(foundrow(i)) Then
                                        row(i) += foundrow(i)
                                        'ElseIf IsDBNull(foundrow(i) AndAlso Not IsDBNull(row(i))) Then

                                    End If
                                Next i
                            Next foundrow
                            Land_Constituent_Table.Rows.Add(row)
                        Next lYear

                        Dim SelectExpressionMonthly As String = "OpTypeNumber = '" & lOperation & "'"
                        Dim foundRowsMonthly() As DataRow = Land_Constituent_Monthly_Table.Select(SelectExpressionMonthly)
                        row = Land_Constituent_Monthly_Table.NewRow

                        row("OpTypeNumber") = foundRowsMonthly(0)("OpTypeNumber")
                        row("OpDesc") = foundRowsMonthly(0)("OpDesc")
                        row("ConstName") = aBalanceType
                        row("ConstNameEXP") = aBalanceType
                        row("Unit") = foundRowsMonthly(0)("Unit")

                        For Each foundrow As DataRow In foundRowsMonthly
                            For i As Integer = 5 To foundrow.ItemArray.Length - 1
                                If IsDBNull(row(i)) AndAlso Not IsDBNull(foundrow(i)) Then
                                    row(i) = foundrow(i)

                                ElseIf Not IsDBNull(row(i)) AndAlso Not IsDBNull(foundrow(i)) Then
                                    row(i) += foundrow(i)
                                    'ElseIf IsDBNull(foundrow(i) AndAlso Not IsDBNull(row(i))) Then

                                End If
                            Next i
                        Next foundrow
                        Land_Constituent_Monthly_Table.Rows.Add(row)
                    Next lOperation
                End If


        End Select

        Dim TextToWrite As String = ""
        For Each TableColumn As DataColumn In Land_Constituent_Table.Columns 'Writing the table headings
            TextToWrite &= TableColumn.Caption & vbTab
        Next
        lReport.AppendLine(TextToWrite)
        For Each TableRow As DataRow In Land_Constituent_Table.Rows 'Writing the table contents
            TextToWrite = ""
            For Each TableColumn As DataColumn In Land_Constituent_Table.Columns
                TextToWrite &= TableRow(TableColumn) & vbTab
            Next TableColumn
            lReport.AppendLine(TextToWrite)
        Next TableRow
        lReport.AppendLine()
        lReport.AppendLine("Tabular Report of Land Loading of all the Land Operations.")
        lReport.AppendLine(aUCI.GlobalBlock.Caption)
        lReport.AppendLine("Run Made " & aRunMade)
        lReport.AppendLine(TimeSpanAsString(aUCI.GlobalBlock.SDateJ, aUCI.GlobalBlock.EdateJ, "Analysis Period: "))
        SaveFileString(aoutfoldername & aBalanceType & "_Land_Loadings.txt", lReport.ToString)

        TextToWrite = ""
        For Each TableColumn As DataColumn In Land_Constituent_Monthly_Table.Columns 'Writing the table headings
            TextToWrite &= TableColumn.Caption & vbTab
        Next
        lReport_Monthly.AppendLine(TextToWrite)
        For Each TableRow As DataRow In Land_Constituent_Monthly_Table.Rows 'Writing the table contents
            TextToWrite = ""
            For Each TableColumn As DataColumn In Land_Constituent_Monthly_Table.Columns
                TextToWrite &= TableRow(TableColumn) & vbTab
            Next TableColumn
            lReport_Monthly.AppendLine(TextToWrite)
        Next TableRow
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
            For Each Constituent As ConstituentProperties In aConstProperties
                lDataForBoxWhiskerPlot.Constituent = Constituent.ConstNameForEXPPlus
                For Each item As String In listLanduses
                    Dim OpType1 As String = item.Split("-")(0)
                    Dim SelectExpression As String = "OpTypeNumber Like '" & item.Split(":")(0) & "%' And Year = 'SumAnnual' And OpDesc ='" & item.Split(":")(1) & "' And ConstNameEXP = '" & Constituent.ConstNameForEXPPlus & "'"
                    Dim foundRows() As DataRow = Land_Constituent_Table.Select(SelectExpression)
                    Dim Values As New List(Of Double)
                    For Each foundrow As DataRow In foundRows
                        Values.Add(foundrow("TotalOutflow"))
                    Next foundrow

                    If Values.Count > 0 Then
                        lDataForBoxWhiskerPlot.Units = "(" & foundRows(0)("Unit") & "/yr)"
                        landUseSumAnnualValues.Add(item, Values.ToArray)
                    End If
                Next item
                lDataForBoxWhiskerPlot.LabelValueCollection = landUseSumAnnualValues
                CreateGraph_BoxAndWhisker(lDataForBoxWhiskerPlot, aoutfoldername & Constituent.ConstNameForEXPPlus & "_BoxWhisker.png")
                landUseSumAnnualValues.Clear()
            Next

            For Each Constituent As ConstituentProperties In aConstProperties
                lDataForBoxWhiskerPlot.Constituent = Constituent.ConstNameForEXPPlus
                For Each item As String In listLanduses
                    Dim OpType1 As String = item.Split("-")(0)
                    Dim SelectExpression As String = "OpTypeNumber Like '" & item.Split(":")(0) & "%' And OpDesc ='" & item.Split(":")(1) & "' And ConstNameEXP = '" & Constituent.ConstNameForEXPPlus & "'"
                    Dim foundRows() As DataRow = Land_Constituent_Monthly_Table.Select(SelectExpression)

                    For Each month As String In lMonthNames
                        Dim Values As New List(Of Double)
                        For Each MonthRow As DataRow In foundRows
                            Values.Add(MonthRow(month))
                        Next
                        If Values.Count > 0 Then
                            landUseSumAnnualValues.Add(Right(month, 3), Values.ToArray)
                        End If
                    Next

                    lDataForBoxWhiskerPlot.LabelValueCollection = landUseSumAnnualValues

                    If landUseSumAnnualValues.Count > 0 Then
                        lDataForBoxWhiskerPlot.Units = "(" & foundRows(0)("Unit") & ")"

                        CreateGraph_BoxAndWhisker(lDataForBoxWhiskerPlot, AbsolutePath(System.IO.Path.Combine("MonthlyLoadings\" & Constituent.ConstNameForEXPPlus & "_" & item.Split(":")(0) & "_" & item.Split(":")(1) & "_BoxWhisker.png"), aoutfoldername),
                                                  "Monthly Loading Rate from Land Use " & item & "")
                        landUseSumAnnualValues.Clear()
                    End If



                Next item


            Next



        Else
            lDataForBoxWhiskerPlot.Constituent = aBalanceType
            For Each item As String In listLanduses
                Dim OpType1 As String = item.Split("-")(0)
                Dim SelectExpression As String = "OpTypeNumber Like '" & item.Split(":")(0) & "%' And Year = 'SumAnnual' And OpDesc ='" & item.Split(":")(1) & "'"
                Dim foundRows() As DataRow = Land_Constituent_Table.Select(SelectExpression)
                Dim Values As New List(Of Double)
                For Each foundrow As DataRow In foundRows
                    Values.Add(foundrow("TotalOutflow"))
                Next foundrow
                If Values.Count > 0 Then
                    landUseSumAnnualValues.Add(item, Values.ToArray)
                End If
            Next item
            If landUseSumAnnualValues.Count > 0 Then
                lDataForBoxWhiskerPlot.Units = "(" & lUnits & "/yr)"
                lDataForBoxWhiskerPlot.LabelValueCollection = landUseSumAnnualValues
                CreateGraph_BoxAndWhisker(lDataForBoxWhiskerPlot, aoutfoldername & aBalanceType & "_BoxWhisker.png")
                landUseSumAnnualValues.Clear()


                lDataForBoxWhiskerPlot.Constituent = aBalanceType
                For Each item As String In listLanduses
                    Dim OpType1 As String = item.Split("-")(0)
                    Dim SelectExpression As String = "OpTypeNumber Like '" & item.Split(":")(0) & "%' And OpDesc ='" & item.Split(":")(1) & "'"
                    Dim foundRows() As DataRow = Land_Constituent_Monthly_Table.Select(SelectExpression)

                    For Each month As String In lMonthNames
                        Dim Values As New List(Of Double)
                        For Each MonthRow As DataRow In foundRows
                            Values.Add(MonthRow(month))
                        Next
                        landUseSumAnnualValues.Add(Right(month, 3), Values.ToArray)
                    Next

                    lDataForBoxWhiskerPlot.LabelValueCollection = landUseSumAnnualValues
                    lDataForBoxWhiskerPlot.Units = "(" & lUnits & ")"

                    CreateGraph_BoxAndWhisker(lDataForBoxWhiskerPlot, AbsolutePath(System.IO.Path.Combine("MonthlyLoadings\" & aBalanceType & "_" & item.Split(":")(0) & "_" & item.Split(":")(1) & "_BoxWhisker.png"), aoutfoldername),
                                                  "Monthly Loading Rate from Land Use " & item & "")
                    landUseSumAnnualValues.Clear()
                Next
            End If

        End If


    End Sub

    Public Sub ReachBudgetReports(ByVal aoutfoldername As String,
                                     ByVal aBinaryData As atcDataSource,
                                     ByVal aUCI As HspfUci,
                                     ByVal aScenario As String,
                                     ByVal aRunMade As String,
                                     ByVal aBalanceType As String,
                                     ByVal aConstProperties As List(Of ConstituentProperties),
                                     ByVal aSDateJ As Double, ByVal aEDateJ As Double)
        Dim lReport As New atcReport.ReportText
        Dim lUpstreamInflows As New atcCollection
        Dim lCumulativePointNonpointColl As New atcCollection

        Reach_Budget_Table = New DataTable("ReachBudgetTable")

        Dim lUnits As String = ""

        Select Case aBalanceType
#Region "DO Case"
            Case "DO"

                If aUCI.GlobalBlock.EmFg = 1 Then
                    lUnits = "lbs"
                Else
                    lUnits = "kgs"
                End If
                Reach_Budget_Table = AddFirstSixColumnsReachBudget(Reach_Budget_Table, lUnits)
                Dim row As DataRow
                Dim column As DataColumn

                column = New DataColumn()
                column.DataType = Type.GetType("System.Double")
                column.ColumnName = "DOXIN-PREC"
                column.Caption = "DO Input In Precip (" & lUnits & ")"
                Reach_Budget_Table.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.Double")
                column.ColumnName = "DOXIN"
                column.Caption = "Total DO Input (" & lUnits & ")"
                Reach_Budget_Table.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.Double")
                column.ColumnName = "DOXFLUX-TOT"
                column.Caption = "Total DO Flux (" & lUnits & ")"
                Reach_Budget_Table.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.Double")
                column.ColumnName = "DOXFLUX-REAER"
                column.Caption = "DO Reaeration (" & lUnits & ")"
                Reach_Budget_Table.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.Double")
                column.ColumnName = "DOXFLUX-BODDEC"
                column.Caption = "DO BOD Decay (" & lUnits & ")"
                Reach_Budget_Table.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.Double")
                column.ColumnName = "DOXFLUX-BENTHAL"
                column.Caption = "DO Benthal (" & lUnits & ")"
                Reach_Budget_Table.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.Double")
                column.ColumnName = "DOXFLUX-NITR"
                column.Caption = "DO Nitrification (" & lUnits & ")"
                Reach_Budget_Table.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.Double")
                column.ColumnName = "DOXFLUX-PHYTO"
                column.Caption = "DO Phytoplankton (" & lUnits & ")"
                Reach_Budget_Table.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.Double")
                column.ColumnName = "DOXFLUX-BENTHIC"
                column.Caption = "DO Phytoplankton (" & lUnits & ")"
                Reach_Budget_Table.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.Double")
                column.ColumnName = "DOXFLUX-ZOO"
                column.Caption = "DO Zooplankton (" & lUnits & ")"
                Reach_Budget_Table.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.Double")
                column.ColumnName = "DOXOUTTOT"
                column.Caption = "Total DO Output (" & lUnits & ")"
                Reach_Budget_Table.Columns.Add(column)

                For Each lReach As HspfOperation In aUCI.OpnSeqBlock.Opns
                    row = Reach_Budget_Table.NewRow
                    If Not lReach.Name = "RCHRES" Then Continue For
                    Dim LocationName As String = lReach.Name.Substring(0, 1) & ":" & lReach.Id
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
                    Dim lOutflow As Double = SubsetByDate(aBinaryData.DataSets.FindData("Location", LocationName).FindData("Constituent", "DOXOUTTOT")(0),
                                                          aSDateJ, aEDateJ, Nothing).Attributes.GetDefinedValue("SumAnnual").Value
                    Dim lTotalIn As Double = SubsetByDate(aBinaryData.DataSets.FindData("Location", LocationName).FindData("Constituent", "DOXIN")(0),
                                                          aSDateJ, aEDateJ, Nothing).Attributes.GetDefinedValue("SumAnnual").Value
                    Dim lPrecIn As Double = 0
                    Try
                        lPrecIn = SubsetByDate(aBinaryData.DataSets.FindData("Location", LocationName).FindData("Constituent", "DOXIN-PREC")(0),
                                                          aSDateJ, aEDateJ, Nothing).Attributes.GetDefinedValue("SumAnnual").Value
                    Catch
                        Logger.Dbg("Precipitation does not contain DO in this model.")
                    End Try
                    Dim lDiversion As Double = CalculateDiversion(aUCI, aBinaryData, lReach, lUpstreamInflows, lDownstreamReachID,
                                                                  lOutflow, aBalanceType)
                    Dim lGENERLoad As Double = CalculateGENERLoad(aUCI, lReach, aBalanceType, aSDateJ, aEDateJ)
                    Dim lMassBalance As Double = lTotalIn - lNPSLoad - lUpstreamIn - lPSLoad - lGENERLoad - lPrecIn
                    For Each columnValue As DataColumn In Reach_Budget_Table.Columns
                        Dim ColumnName As String = columnValue.ColumnName
                        Select Case ColumnName
                            Case "OpTypeNumber"
                                row(ColumnName) = LocationName
                            Case "OpDesc"
                                row(ColumnName) = lReach.Description
                            Case "NPSLoad"
                                row(ColumnName) = HspfTable.NumFmtRE(lNPSLoad, 10)
                            Case "PSLoad"
                                row(ColumnName) = HspfTable.NumFmtRE(lPSLoad, 10)
                            Case "GENERLoad"
                                row(ColumnName) = HspfTable.NumFmtRE(lGENERLoad, 10)
                            Case "Diversion"
                                row(ColumnName) = HspfTable.NumFmtRE(lDiversion, 10)
                            Case "MassBalance"
                                row(ColumnName) = HspfTable.NumFmtRE(lMassBalance, 10)
                            Case "UpstreamIn"
                                row(ColumnName) = HspfTable.NumFmtRE(lUpstreamIn, 10)
                            Case Else
                                Dim lTest As atcTimeseries = aBinaryData.DataSets.FindData("Location", LocationName).FindData("Constituent", ColumnName)(0)
                                If lTest IsNot Nothing Then
                                    row(ColumnName) = HspfTable.NumFmtRE(SubsetByDate(lTest,
                                                                  aSDateJ, aEDateJ, Nothing).Attributes.GetDefinedValue("SumAnnual").Value, 10)
                                End If

                        End Select
                    Next columnValue

                    Reach_Budget_Table.Rows.Add(row)
                Next lReach
                Dim TextToWrite As String = ""
                For Each TableColumn As DataColumn In Reach_Budget_Table.Columns 'Writing the table headings
                    TextToWrite &= TableColumn.Caption & vbTab
                Next
                lReport.AppendLine(TextToWrite)
                For Each TableRow As DataRow In Reach_Budget_Table.Rows 'Writing the table contents
                    TextToWrite = ""
                    For Each TableColumn As DataColumn In Reach_Budget_Table.Columns
                        TextToWrite &= TableRow(TableColumn) & vbTab
                    Next TableColumn
                    lReport.AppendLine(TextToWrite)
                Next TableRow
                lReport.AppendLine()
                lReport.AppendLine("Tabular Report of Average Annual Reach Budget for all the Reach Operations.")
                lReport.AppendLine("   " & aUCI.GlobalBlock.RunInf.Value)
                lReport.AppendLine("   Run Made " & aRunMade)
                lReport.AppendLine("   " & TimeSpanAsString(aSDateJ, aEDateJ, "Analysis Period: "))
                SaveFileString(aoutfoldername & aBalanceType & "_Reach_Budget.txt", lReport.ToString)
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
                Dim column As DataColumn
                Dim row As DataRow
                Reach_Budget_Table = AddFirstSixColumnsReachBudget(Reach_Budget_Table, lUnits)
                column = New DataColumn()
                column.DataType = Type.GetType("System.Double")
                column.ColumnName = "IHEAT"
                column.Caption = "Total Heat Inflow (" & lUnits & ")"
                Reach_Budget_Table.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.Double")
                column.ColumnName = "HTEXCH"
                column.Caption = "Atmospheric Heat Exchange (" & lUnits & ")"
                Reach_Budget_Table.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.Double")
                column.ColumnName = "ROHEAT"
                column.Caption = "Heat Outflow (" & lUnits & ")"
                Reach_Budget_Table.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.Double")
                column.ColumnName = "QTOTAL"
                column.Caption = "Total Heat Balance (" & lUnits2 & ")"
                Reach_Budget_Table.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.Double")
                column.ColumnName = "QSOLAR"
                column.Caption = "Solar Radiation (" & lUnits2 & ")"
                Reach_Budget_Table.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.Double")
                column.ColumnName = "QLONGW"
                column.Caption = "Longwave Radiation (" & lUnits2 & ")"
                Reach_Budget_Table.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.Double")
                column.ColumnName = "QEVAP"
                column.Caption = "Evaporation (" & lUnits2 & ")"
                Reach_Budget_Table.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.Double")
                column.ColumnName = "QCON"
                column.Caption = "Convection/Conduction (" & lUnits2 & ")"
                Reach_Budget_Table.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.Double")
                column.ColumnName = "QPREC"
                column.Caption = "Precipitation (" & lUnits2 & ")"
                Reach_Budget_Table.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.Double")
                column.ColumnName = "QBED"
                column.Caption = "Bed Conduction (" & lUnits2 & ")"
                Reach_Budget_Table.Columns.Add(column)

                For Each lReach As HspfOperation In aUCI.OpnSeqBlock.Opns
                    row = Reach_Budget_Table.NewRow
                    If Not lReach.Name = "RCHRES" Then Continue For
                    Dim LocationName As String = lReach.Name.Substring(0, 1) & ":" & lReach.Id
                    'Dim lOutflowDataTypes1 As String() = ConstituentListRCHRES(aBalanceType)
                    Dim lTS As New atcTimeseries(Nothing)
                    Dim AddTS As New atcDataGroup
                    Dim lTotalTS As New atcTimeseries(Nothing)
                    Dim lDownstreamReachID As Integer = lReach.DownOper("RCHRES")
                    Dim lUpstreamIn As Double = 0.0
                    If lUpstreamInflows.Keys.Contains(lReach.Id) Then
                        lUpstreamIn = lUpstreamInflows.ItemByKey(lReach.Id)
                    End If
                    Dim lNPSLoad As Double = CalculateNPSLoad(aUCI, lReach, aBalanceType)
                    Dim lPSLoad As Double = CalculatePSLoad(aUCI, lReach, aSDateJ, aEDateJ, aBalanceType)
                    Dim lOutflow As Double = SubsetByDate(aBinaryData.DataSets.FindData("Location", LocationName).FindData("Constituent", "ROHEAT")(0),
                                                                  aSDateJ, aEDateJ, Nothing).Attributes.GetDefinedValue("SumAnnual").Value
                    Dim lTotalIn As Double = SubsetByDate(aBinaryData.DataSets.FindData("Location", LocationName).FindData("Constituent", "IHEAT")(0),
                                                                  aSDateJ, aEDateJ, Nothing).Attributes.GetDefinedValue("SumAnnual").Value
                    Dim lDiversion As Double = CalculateDiversion(aUCI, aBinaryData, lReach, lUpstreamInflows, lDownstreamReachID, lOutflow, aBalanceType)
                    Dim lGENERLoad As Double = CalculateGENERLoad(aUCI, lReach, aBalanceType, aSDateJ, aEDateJ)
                    Dim lMassBalance As Double = lTotalIn - lNPSLoad - lUpstreamIn - lPSLoad - lGENERLoad
                    For Each columnValue As DataColumn In Reach_Budget_Table.Columns
                        Dim ColumnName As String = columnValue.ColumnName
                        Select Case ColumnName
                            Case "OpTypeNumber"
                                row(ColumnName) = LocationName
                            Case "OpDesc"
                                row(ColumnName) = lReach.Description
                            Case "NPSLoad"
                                row(ColumnName) = HspfTable.NumFmtRE(lNPSLoad, 10)
                            Case "PSLoad"
                                row(ColumnName) = HspfTable.NumFmtRE(lPSLoad, 10)
                            Case "GENERLoad"
                                row(ColumnName) = HspfTable.NumFmtRE(lGENERLoad, 10)
                            Case "Diversion"
                                row(ColumnName) = HspfTable.NumFmtRE(lDiversion, 10)
                            Case "MassBalance"
                                row(ColumnName) = HspfTable.NumFmtRE(lMassBalance, 10)
                            Case "UpstreamIn"
                                row(ColumnName) = HspfTable.NumFmtRE(lUpstreamIn, 10)
                            Case Else
                                row(ColumnName) = HspfTable.NumFmtRE(SubsetByDate(aBinaryData.DataSets.FindData("Location", LocationName).FindData("Constituent", ColumnName)(0),
                                                                  aSDateJ, aEDateJ, Nothing).Attributes.GetDefinedValue("SumAnnual").Value, 10)
                        End Select
                    Next columnValue

                    Reach_Budget_Table.Rows.Add(row)
                Next lReach
                Dim TextToWrite As String = ""
                For Each TableColumn As DataColumn In Reach_Budget_Table.Columns 'Writing the table headings
                    TextToWrite &= TableColumn.Caption & vbTab
                Next
                lReport.AppendLine(TextToWrite)
                For Each TableRow As DataRow In Reach_Budget_Table.Rows 'Writing the table contents
                    TextToWrite = ""
                    For Each TableColumn As DataColumn In Reach_Budget_Table.Columns
                        TextToWrite &= TableRow(TableColumn) & vbTab
                    Next TableColumn
                    lReport.AppendLine(TextToWrite)
                Next TableRow
                lReport.AppendLine()
                lReport.AppendLine("Tabular Report of Average Annual Reach Budget for all the Reach Operations.")
                lReport.AppendLine("   " & aUCI.GlobalBlock.RunInf.Value)
                lReport.AppendLine("   Run Made " & aRunMade)
                lReport.AppendLine("   " & TimeSpanAsString(aSDateJ, aEDateJ, "Analysis Period: "))
                SaveFileString(aoutfoldername & aBalanceType & "_Reach_Budget.txt", lReport.ToString)
#End Region
#Region "BOD-Labile Case"
            Case "BOD-Labile"
                Dim lUnits2 As String = ""
                If aUCI.GlobalBlock.EmFg = 1 Then
                    lUnits = "lbs"
                Else
                    lUnits = "kg"

                End If
                Reach_Budget_Table = AddFirstSixColumnsReachBudget(Reach_Budget_Table, lUnits)
                Dim row As DataRow
                Dim column As DataColumn
                column = New DataColumn()
                column.DataType = Type.GetType("System.Double")
                column.ColumnName = "BODIN"
                column.Caption = "Total BOD Inflow (" & lUnits & ")"
                Reach_Budget_Table.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.Double")
                column.ColumnName = "BODFLUX-TOT"
                column.Caption = "Total BOD Flux (" & lUnits & ")"
                Reach_Budget_Table.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.Double")
                column.ColumnName = "BODFLUX-BODDEC"
                column.Caption = "BOD Decay (" & lUnits & ")"
                Reach_Budget_Table.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.Double")
                column.ColumnName = "BODFLUX-SINK"
                column.Caption = "BOD Sink (" & lUnits & ")"
                Reach_Budget_Table.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.Double")
                column.ColumnName = "BODFLUX-BENTHAL"
                column.Caption = "BOD Benthal (" & lUnits & ")"
                Reach_Budget_Table.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.Double")
                column.ColumnName = "BODFLUX-BENTHIC"
                column.Caption = "BOD Benthic (" & lUnits & ")"
                Reach_Budget_Table.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.Double")
                column.ColumnName = "BODFLUX-DENITR"
                column.Caption = "BOD Denitrification (" & lUnits & ")"
                Reach_Budget_Table.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.Double")
                column.ColumnName = "BODFLUX-PHYTO"
                column.Caption = "BOD Phytoplankton (" & lUnits & ")"
                Reach_Budget_Table.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.Double")
                column.ColumnName = "BODFLUX-ZOO"
                column.Caption = "BOD Zooplankton (" & lUnits & ")"
                Reach_Budget_Table.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.Double")
                column.ColumnName = "BODOUTTOT"
                column.Caption = "BOD Outflow (" & lUnits & ")"
                Reach_Budget_Table.Columns.Add(column)

                For Each lReach As HspfOperation In aUCI.OpnSeqBlock.Opns
                    row = Reach_Budget_Table.NewRow
                    If Not lReach.Name = "RCHRES" Then Continue For
                    Dim LocationName As String = lReach.Name.Substring(0, 1) & ":" & lReach.Id

                    'Dim lOutflowDataTypes1 As String() = ConstituentListRCHRES(aBalanceType)
                    Dim lTS As New atcTimeseries(Nothing)
                    Dim AddTS As New atcDataGroup
                    Dim lTotalTS As New atcTimeseries(Nothing)
                    Dim lDownstreamReachID As Integer = lReach.DownOper("RCHRES")
                    Dim lUpstreamIn As Double = 0.0
                    If lUpstreamInflows.Keys.Contains(lReach.Id) Then
                        lUpstreamIn = lUpstreamInflows.ItemByKey(lReach.Id)
                    End If
                    Dim lNPSLoad As Double = CalculateNPSLoad(aUCI, lReach, aBalanceType)
                    Dim lPSLoad As Double = CalculatePSLoad(aUCI, lReach, aSDateJ, aEDateJ, aBalanceType)
                    Dim lOutflow As Double = SubsetByDate(aBinaryData.DataSets.FindData("Location", LocationName).FindData("Constituent", "BODOUTTOT")(0),
                                                                  aSDateJ, aEDateJ, Nothing).Attributes.GetDefinedValue("SumAnnual").Value
                    Dim lTotalIn As Double = SubsetByDate(aBinaryData.DataSets.FindData("Location", LocationName).FindData("Constituent", "BODIN")(0),
                                                                  aSDateJ, aEDateJ, Nothing).Attributes.GetDefinedValue("SumAnnual").Value
                    Dim lDiversion As Double = CalculateDiversion(aUCI, aBinaryData, lReach, lUpstreamInflows, lDownstreamReachID, lOutflow, aBalanceType)
                    Dim lGENERLoad As Double = CalculateGENERLoad(aUCI, lReach, aBalanceType, aSDateJ, aEDateJ)
                    Dim lMassBalance As Double = lTotalIn - lNPSLoad - lUpstreamIn - lPSLoad - lGENERLoad

                    For Each columnValue As DataColumn In Reach_Budget_Table.Columns
                        Dim ColumnName As String = columnValue.ColumnName
                        Select Case ColumnName
                            Case "OpTypeNumber"
                                row(ColumnName) = LocationName
                            Case "OpDesc"
                                row(ColumnName) = lReach.Description
                            Case "NPSLoad"
                                row(ColumnName) = HspfTable.NumFmtRE(lNPSLoad, 10)

                            Case "PSLoad"
                                row(ColumnName) = HspfTable.NumFmtRE(lPSLoad, 10)
                            Case "GENERLoad"
                                row(ColumnName) = HspfTable.NumFmtRE(lGENERLoad, 10)
                            Case "Diversion"
                                row(ColumnName) = HspfTable.NumFmtRE(lDiversion, 10)
                            Case "MassBalance"
                                row(ColumnName) = HspfTable.NumFmtRE(lMassBalance, 10)
                            Case "UpstreamIn"
                                row(ColumnName) = HspfTable.NumFmtRE(lUpstreamIn, 10)
                            Case Else
                                row(ColumnName) = HspfTable.NumFmtRE(SubsetByDate(aBinaryData.DataSets.FindData("Location", LocationName).FindData("Constituent", ColumnName)(0),
                                                                  aSDateJ, aEDateJ, Nothing).Attributes.GetDefinedValue("SumAnnual").Value, 10)
                        End Select
                    Next columnValue

                    Reach_Budget_Table.Rows.Add(row)
                Next lReach
                Dim TextToWrite As String = ""
                For Each TableColumn As DataColumn In Reach_Budget_Table.Columns 'Writing the table headings
                    TextToWrite &= TableColumn.Caption & vbTab
                Next
                lReport.AppendLine(TextToWrite)
                For Each TableRow As DataRow In Reach_Budget_Table.Rows 'Writing the table contents
                    TextToWrite = ""
                    For Each TableColumn As DataColumn In Reach_Budget_Table.Columns
                        TextToWrite &= TableRow(TableColumn) & vbTab
                    Next TableColumn
                    lReport.AppendLine(TextToWrite)
                Next TableRow
                lReport.AppendLine()
                lReport.AppendLine("Tabular Report of Average Annual Reach Budget for all the Reach Operations.")
                lReport.AppendLine("   " & aUCI.GlobalBlock.RunInf.Value)
                lReport.AppendLine("   Run Made " & aRunMade)
                lReport.AppendLine("   " & TimeSpanAsString(aSDateJ, aEDateJ, "Analysis Period: "))
                SaveFileString(aoutfoldername & aBalanceType & "_Reach_Budget.txt", lReport.ToString)
#End Region
#Region "TotalN Case"
            Case "TotalN"
                For Each lConstituent As ConstituentProperties In aConstProperties
                    Reach_Budget_Table = New DataTable
                    Dim lReachConstituent As String = lConstituent.ConstNameForEXPPlus
                    If lReachConstituent = "NO3" Or lReachConstituent = "TAM" Then
                        If aUCI.GlobalBlock.EmFg = 1 Then
                            lUnits = "lbs"
                        Else
                            lUnits = "kgs"
                        End If
                        lUpstreamInflows = New atcCollection
                        Reach_Budget_Table = AddFirstSixColumnsReachBudget(Reach_Budget_Table, lUnits)
                        Dim row As DataRow
                        Dim column As DataColumn
                        column = New DataColumn()
                        column.DataType = Type.GetType("System.Double")
                        column.ColumnName = lReachConstituent & "-INTOT"
                        column.Caption = "Total " & lReachConstituent & " Inflow (" & lUnits & ")"
                        Reach_Budget_Table.Columns.Add(column)

                        column = New DataColumn()
                        column.DataType = Type.GetType("System.Double")
                        column.ColumnName = lReachConstituent & "-PROCFLUX-TOT"
                        column.Caption = "Total " & lReachConstituent & " Process Fluxes (" & lUnits & ")"
                        Reach_Budget_Table.Columns.Add(column)

                        column = New DataColumn()
                        column.DataType = Type.GetType("System.Double")
                        column.ColumnName = lReachConstituent & "-ATMDEPTOT"
                        column.Caption = "Total " & lReachConstituent & " Atmospheric Deposition (" & lUnits & ")"
                        Reach_Budget_Table.Columns.Add(column)

                        column = New DataColumn()
                        column.DataType = Type.GetType("System.Double")
                        column.ColumnName = lReachConstituent & "-OUTTOT"
                        column.Caption = "Total " & lReachConstituent & " Outflow (" & lUnits & ")"
                        Reach_Budget_Table.Columns.Add(column)

                        For Each lReach As HspfOperation In aUCI.OpnSeqBlock.Opns
                            row = Reach_Budget_Table.NewRow
                            If Not lReach.Name = "RCHRES" Then Continue For
                            Dim LocationName As String = lReach.Name.Substring(0, 1) & ":" & lReach.Id

                            'Dim lOutflowDataTypes1 As String() = ConstituentListRCHRES(aBalanceType)
                            Dim lTS As New atcTimeseries(Nothing)
                            Dim AddTS As New atcDataGroup
                            Dim lTotalTS As New atcTimeseries(Nothing)
                            Dim lDownstreamReachID As Integer = lReach.DownOper("RCHRES")
                            Dim lUpstreamIn As Double = 0.0
                            If lUpstreamInflows.Keys.Contains(lReach.Id) Then
                                lUpstreamIn = lUpstreamInflows.ItemByKey(lReach.Id)
                            End If
                            Dim lNPSLoad As Double = CalculateNPSLoad(aUCI, lReach, lReachConstituent)
                            Dim lPSLoad As Double = CalculatePSLoad(aUCI, lReach, aSDateJ, aEDateJ, lReachConstituent)
                            Dim lOutflow As Double = SubsetByDate(aBinaryData.DataSets.FindData("Location", LocationName).FindData("Constituent", lReachConstituent & "-OUTTOT")(0),
                                                                          aSDateJ, aEDateJ, Nothing).Attributes.GetDefinedValue("SumAnnual").Value

                            Dim lTotalIn As Double = SubsetByDate(aBinaryData.DataSets.FindData("Location", LocationName).FindData("Constituent", lReachConstituent & "-INTOT")(0),
                                                                          aSDateJ, aEDateJ, Nothing).Attributes.GetDefinedValue("SumAnnual").Value
                            Dim lTotalAtmDep As Double = SubsetByDate(aBinaryData.DataSets.FindData("Location", LocationName).FindData("Constituent", lReachConstituent & "-ATMDEPTOT")(0),
                                                                          aSDateJ, aEDateJ, Nothing).Attributes.GetDefinedValue("SumAnnual").Value
                            Dim lProcFluxTot As Double = SubsetByDate(aBinaryData.DataSets.FindData("Location", LocationName).FindData("Constituent", lReachConstituent & "-PROCFLUX-TOT")(0),
                                                                          aSDateJ, aEDateJ, Nothing).Attributes.GetDefinedValue("SumAnnual").Value
                            If lReachConstituent = "NO3" Then
                                Try
                                    lOutflow += SubsetByDate(aBinaryData.DataSets.FindData("Location", LocationName).FindData("Constituent", "NO2-OUTTOT")(0),
                                                                          aSDateJ, aEDateJ, Nothing).Attributes.GetDefinedValue("SumAnnual").Value
                                    lTotalIn += SubsetByDate(aBinaryData.DataSets.FindData("Location", LocationName).FindData("Constituent", "NO2-INTOT")(0),
                                                                          aSDateJ, aEDateJ, Nothing).Attributes.GetDefinedValue("SumAnnual").Value
                                    lProcFluxTot += SubsetByDate(aBinaryData.DataSets.FindData("Location", LocationName).FindData("Constituent", "NO2-PROCFLUX-TOT")(0),
                                                                              aSDateJ, aEDateJ, Nothing).Attributes.GetDefinedValue("SumAnnual").Value
                                Catch
                                End Try

                            End If

                            Dim lDiversion As Double = CalculateDiversion(aUCI, aBinaryData, lReach, lUpstreamInflows, lDownstreamReachID, lOutflow, lReachConstituent)
                            Dim lGENERLoad As Double = CalculateGENERLoad(aUCI, lReach, lReachConstituent, aSDateJ, aEDateJ)
                            Dim lMassBalance As Double = lTotalIn - lNPSLoad - lUpstreamIn - lPSLoad - lGENERLoad - lTotalAtmDep
                            For Each columnValue As DataColumn In Reach_Budget_Table.Columns
                                Dim ColumnName As String = columnValue.ColumnName
                                Select Case ColumnName
                                    Case "OpTypeNumber"
                                        row(ColumnName) = LocationName
                                    Case "OpDesc"
                                        row(ColumnName) = lReach.Description
                                    Case "NPSLoad"
                                        row(ColumnName) = HspfTable.NumFmtRE(lNPSLoad, 10)
                                    Case "PSLoad"
                                        row(ColumnName) = HspfTable.NumFmtRE(lPSLoad, 10)
                                    Case "GENERLoad"
                                        row(ColumnName) = HspfTable.NumFmtRE(lGENERLoad, 10)
                                    Case "Diversion"
                                        row(ColumnName) = HspfTable.NumFmtRE(lDiversion, 10)
                                    Case "MassBalance"
                                        row(ColumnName) = HspfTable.NumFmtRE(lMassBalance, 10)
                                    Case "UpstreamIn"
                                        row(ColumnName) = HspfTable.NumFmtRE(lUpstreamIn, 10)
                                    Case lReachConstituent & "-INTOT"
                                        row(ColumnName) = HspfTable.NumFmtRE(lTotalIn, 10)
                                    Case lReachConstituent & "-PROCFLUX-TOT"
                                        row(ColumnName) = lProcFluxTot
                                    Case lReachConstituent & "-OUTTOT"
                                        row(ColumnName) = lOutflow
                                    Case Else
                                        row(ColumnName) = HspfTable.NumFmtRE(SubsetByDate(aBinaryData.DataSets.FindData("Location", LocationName).FindData("Constituent", ColumnName)(0),
                                                                          aSDateJ, aEDateJ, Nothing).Attributes.GetDefinedValue("SumAnnual").Value, 10)

                                End Select
                            Next columnValue

                            Reach_Budget_Table.Rows.Add(row)
                        Next lReach


                        Dim TextToWrite As String = ""
                        For Each TableColumn As DataColumn In Reach_Budget_Table.Columns 'Writing the table headings
                            TextToWrite &= TableColumn.Caption & vbTab
                        Next
                        lReport = New atcReport.ReportText
                        lReport.AppendLine(TextToWrite)
                        For Each TableRow As DataRow In Reach_Budget_Table.Rows 'Writing the table contents
                            TextToWrite = ""
                            For Each TableColumn As DataColumn In Reach_Budget_Table.Columns
                                TextToWrite &= TableRow(TableColumn) & vbTab
                            Next TableColumn
                            lReport.AppendLine(TextToWrite)
                        Next TableRow
                        lReport.AppendLine()
                        lReport.AppendLine("Tabular Report of Average Annual Reach Budget for all the Reach Operations.")
                        lReport.AppendLine("   " & aUCI.GlobalBlock.RunInf.Value)
                        lReport.AppendLine("   Run Made " & aRunMade)
                        lReport.AppendLine("   " & TimeSpanAsString(aSDateJ, aEDateJ, "Analysis Period: "))
                        SaveFileString(aoutfoldername & lConstituent.ConstNameForEXPPlus & "_Reach_Budget.txt", lReport.ToString)
                    End If
                Next lConstituent
#End Region

#Region "TotalP Case"
            Case "TotalP"
                For Each lConstituent As ConstituentProperties In aConstProperties
                    Reach_Budget_Table = New DataTable
                    Dim lReachConstituent As String = lConstituent.ConstNameForEXPPlus
                    If lReachConstituent = "PO4" Then
                        If aUCI.GlobalBlock.EmFg = 1 Then
                            lUnits = "lbs"
                        Else
                            lUnits = "kgs"
                        End If
                        lUpstreamInflows = New atcCollection
                        Reach_Budget_Table = AddFirstSixColumnsReachBudget(Reach_Budget_Table, lUnits)
                        Dim row As DataRow
                        Dim column As DataColumn
                        column = New DataColumn()
                        column.DataType = Type.GetType("System.Double")
                        column.ColumnName = lReachConstituent & "-INTOT"
                        column.Caption = "Total " & lReachConstituent & " Inflow (" & lUnits & ")"
                        Reach_Budget_Table.Columns.Add(column)

                        column = New DataColumn()
                        column.DataType = Type.GetType("System.Double")
                        column.ColumnName = lReachConstituent & "-PROCFLUX-TOT"
                        column.Caption = "Total " & lReachConstituent & " Process Fluxes (" & lUnits & ")"
                        Reach_Budget_Table.Columns.Add(column)

                        column = New DataColumn()
                        column.DataType = Type.GetType("System.Double")
                        column.ColumnName = lReachConstituent & "-ATMDEPTOT"
                        column.Caption = "Total " & lReachConstituent & " Atmospheric Deposition (" & lUnits & ")"
                        Reach_Budget_Table.Columns.Add(column)

                        column = New DataColumn()
                        column.DataType = Type.GetType("System.Double")
                        column.ColumnName = lReachConstituent & "-OUTTOT"
                        column.Caption = "Total " & lReachConstituent & " Outflow (" & lUnits & ")"
                        Reach_Budget_Table.Columns.Add(column)

                        For Each lReach As HspfOperation In aUCI.OpnSeqBlock.Opns
                            row = Reach_Budget_Table.NewRow
                            If Not lReach.Name = "RCHRES" Then Continue For
                            Dim LocationName As String = lReach.Name.Substring(0, 1) & ":" & lReach.Id

                            'Dim lOutflowDataTypes1 As String() = ConstituentListRCHRES(aBalanceType)
                            Dim lTS As New atcTimeseries(Nothing)
                            Dim AddTS As New atcDataGroup
                            Dim lTotalTS As New atcTimeseries(Nothing)
                            Dim lDownstreamReachID As Integer = lReach.DownOper("RCHRES")
                            Dim lUpstreamIn As Double = 0.0
                            If lUpstreamInflows.Keys.Contains(lReach.Id) Then
                                lUpstreamIn = lUpstreamInflows.ItemByKey(lReach.Id)
                            End If
                            Dim lNPSLoad As Double = CalculateNPSLoad(aUCI, lReach, lReachConstituent)
                            Dim lPSLoad As Double = CalculatePSLoad(aUCI, lReach, aSDateJ, aEDateJ, lReachConstituent)
                            Dim lOutflow As Double = SubsetByDate(aBinaryData.DataSets.FindData("Location", LocationName).FindData("Constituent", lReachConstituent & "-OUTTOT")(0),
                                                                          aSDateJ, aEDateJ, Nothing).Attributes.GetDefinedValue("SumAnnual").Value
                            Dim lTotalIn As Double = SubsetByDate(aBinaryData.DataSets.FindData("Location", LocationName).FindData("Constituent", lReachConstituent & "-INTOT")(0),
                                                                          aSDateJ, aEDateJ, Nothing).Attributes.GetDefinedValue("SumAnnual").Value
                            Dim lTotalAtmDep As Double = SubsetByDate(aBinaryData.DataSets.FindData("Location", LocationName).FindData("Constituent", lReachConstituent & "-ATMDEPTOT")(0),
                                                                          aSDateJ, aEDateJ, Nothing).Attributes.GetDefinedValue("SumAnnual").Value

                            Dim lDiversion As Double = CalculateDiversion(aUCI, aBinaryData, lReach, lUpstreamInflows, lDownstreamReachID, lOutflow, lReachConstituent)

                            Dim lGENERLoad As Double = CalculateGENERLoad(aUCI, lReach, lReachConstituent, aSDateJ, aEDateJ)
                            Dim lMassBalance As Double = lTotalIn - lNPSLoad - lUpstreamIn - lPSLoad - lGENERLoad - lTotalAtmDep
                            For Each columnValue As DataColumn In Reach_Budget_Table.Columns
                                Dim ColumnName As String = columnValue.ColumnName
                                Select Case ColumnName
                                    Case "OpTypeNumber"
                                        row(ColumnName) = LocationName
                                    Case "OpDesc"
                                        row(ColumnName) = lReach.Description
                                    Case "NPSLoad"
                                        row(ColumnName) = HspfTable.NumFmtRE(lNPSLoad, 10)
                                    Case "PSLoad"
                                        row(ColumnName) = HspfTable.NumFmtRE(lPSLoad, 10)
                                    Case "GENERLoad"
                                        row(ColumnName) = HspfTable.NumFmtRE(lGENERLoad, 10)
                                    Case "Diversion"
                                        row(ColumnName) = HspfTable.NumFmtRE(lDiversion, 10)
                                    Case "MassBalance"
                                        row(ColumnName) = HspfTable.NumFmtRE(lMassBalance, 10)
                                    Case "UpstreamIn"
                                        row(ColumnName) = HspfTable.NumFmtRE(lUpstreamIn, 10)
                                    Case Else
                                        row(ColumnName) = HspfTable.NumFmtRE(SubsetByDate(aBinaryData.DataSets.FindData("Location", LocationName).FindData("Constituent", ColumnName)(0),
                                                                          aSDateJ, aEDateJ, Nothing).Attributes.GetDefinedValue("SumAnnual").Value, 10)
                                End Select
                            Next columnValue

                            Reach_Budget_Table.Rows.Add(row)
                        Next lReach


                        Dim TextToWrite As String = ""
                        For Each TableColumn As DataColumn In Reach_Budget_Table.Columns 'Writing the table headings
                            TextToWrite &= TableColumn.Caption & vbTab
                        Next
                        lReport = New atcReport.ReportText
                        lReport.AppendLine(TextToWrite)
                        For Each TableRow As DataRow In Reach_Budget_Table.Rows 'Writing the table contents
                            TextToWrite = ""
                            For Each TableColumn As DataColumn In Reach_Budget_Table.Columns
                                TextToWrite &= TableRow(TableColumn) & vbTab
                            Next TableColumn
                            lReport.AppendLine(TextToWrite)
                        Next TableRow
                        lReport.AppendLine()
                        lReport.AppendLine("Tabular Report of Average Annual Reach Budget for all the Reach Operations.")
                        lReport.AppendLine("   " & aUCI.GlobalBlock.RunInf.Value)
                        lReport.AppendLine("   Run Made " & aRunMade)
                        lReport.AppendLine("   " & TimeSpanAsString(aSDateJ, aEDateJ, "Analysis Period: "))
                        SaveFileString(aoutfoldername & lConstituent.ConstNameForEXPPlus & "_Reach_Budget.txt", lReport.ToString)
                    End If
                Next lConstituent
#End Region

        End Select
    End Sub

    Private Function CalculateNPSLoad(ByVal aUCI As HspfUci, ByVal aReach As HspfOperation, ByVal aConstituentName As String) As Double
        Dim NPSLoad As Double = 0.0
        Dim SelectExpression As String = ""
        For Each lReachSource As HspfConnection In aReach.Sources
            Try
                If lReachSource.Source.Opn Is Nothing OrElse lReachSource.Source.Opn.Name = "RCHRES" Then Continue For
                'If Not ((lReachSource.Source.Opn.Name = "PERLND" AndAlso lReachSource.Source.Opn.Tables("ACTIVITY").Parms("PQALFG").Value = "1") OrElse
                '           (lReachSource.Source.Opn.Name = "IMPLND" AndAlso lReachSource.Source.Opn.Tables("ACTIVITY").Parms("IQALFG").Value = "1")) Then Continue For
                Dim lConnectionArea As Double = lReachSource.MFact
                Dim lOperationTypeNumber As String = SafeSubstring(lReachSource.Source.VolName, 0, 1) & ":" & lReachSource.Source.VolId
                If aConstituentName = "NO3" Or aConstituentName = "TAM" Or aConstituentName = "PO4" Then
                    SelectExpression = "OpTypeNumber= '" & lOperationTypeNumber & "' And Year = 'SumAnnual' And ConstNameEXP = '" & aConstituentName & "'"
                Else
                    SelectExpression = "OpTypeNumber= '" & lOperationTypeNumber & "' And Year = 'SumAnnual'"
                End If

                Dim foundRows() As DataRow = Land_Constituent_Table.Select(SelectExpression)
                If foundRows.Length = 0 Then Continue For
                NPSLoad += lConnectionArea * foundRows(0)("TotalOutflow")
            Catch
            End Try

        Next lReachSource

        Return NPSLoad
    End Function
    Private Function CalculatePSLoad(ByVal aUCI As HspfUci, ByVal aReach As HspfOperation,
                                     ByVal aSDateJ As Double,
                                      ByVal aEDateJ As Double, ByVal aConstituentName As String) As Double
        Dim PSLoad As Double = 0.0
        Select Case aConstituentName
            Case "DO"
                For Each lSource As HspfPointSource In aReach.PointSources
                    If lSource.Target.Group = "INFLOW" AndAlso lSource.Target.Member = "OXIF" AndAlso lSource.Target.MemSub1 = 1 Then
                        Dim TimeSeriesTransformaton As String = lSource.Tran.ToString
                        Dim VolName As String = lSource.Source.VolName
                        Dim lDSN As Integer = lSource.Source.VolId
                        Dim lMfact As Double = lSource.MFact
                        Dim TransformationMultFact As Double = 0
                        For i As Integer = 0 To aUCI.FilesBlock.Count
                            If aUCI.FilesBlock.Value(i).Typ = VolName Then
                                Dim lFileName As String = AbsolutePath(aUCI.FilesBlock.Value(i).Name.Trim, CurDir())
                                Dim lDataSource As atcDataSource = atcDataManager.DataSourceBySpecification(lFileName)
                                If lDataSource Is Nothing Then
                                    If atcDataManager.OpenDataSource(lFileName) Then
                                        lDataSource = atcDataManager.DataSourceBySpecification(lFileName)
                                    End If
                                End If
                                Dim ltimeseries As atcTimeseries = lDataSource.DataSets.FindData("ID", lDSN)(0)
                                ltimeseries = SubsetByDate(ltimeseries, aSDateJ, aEDateJ, Nothing)
                                TransformationMultFact = MultiFactorForPointSource(ltimeseries.Attributes.GetDefinedValue("Time Step").Value, ltimeseries.Attributes.GetDefinedValue("Time Unit").Value.ToString,
                                                                                            TimeSeriesTransformaton, aUCI.OpnSeqBlock.Delt)
                                PSLoad += ltimeseries.Attributes.GetDefinedValue("Sum").Value * lMfact * TransformationMultFact / YearCount(aSDateJ, aEDateJ)
                            End If
                        Next

                    End If
                Next lSource
            Case "BOD-Labile"
                For Each lSource As HspfPointSource In aReach.PointSources
                    If lSource.Target.Group = "INFLOW" AndAlso lSource.Target.Member = "OXIF" AndAlso lSource.Target.MemSub1 = 2 Then
                        Dim TimeSeriesTransformaton As String = lSource.Tran.ToString
                        Dim VolName As String = lSource.Source.VolName
                        Dim lDSN As Integer = lSource.Source.VolId
                        Dim lMfact As Double = lSource.MFact
                        Dim TransformationMultFact As Double = 0
                        For i As Integer = 0 To aUCI.FilesBlock.Count
                            If aUCI.FilesBlock.Value(i).Typ = VolName Then
                                Dim lFileName As String = AbsolutePath(aUCI.FilesBlock.Value(i).Name.Trim, CurDir())
                                Dim lDataSource As atcDataSource = atcDataManager.DataSourceBySpecification(lFileName)
                                If lDataSource Is Nothing Then
                                    If atcDataManager.OpenDataSource(lFileName) Then
                                        lDataSource = atcDataManager.DataSourceBySpecification(lFileName)
                                    End If
                                End If
                                Dim ltimeseries As atcTimeseries = lDataSource.DataSets.FindData("ID", lDSN)(0)
                                ltimeseries = SubsetByDate(ltimeseries, aSDateJ, aEDateJ, Nothing)
                                TransformationMultFact = MultiFactorForPointSource(ltimeseries.Attributes.GetDefinedValue("Time Step").Value, ltimeseries.Attributes.GetDefinedValue("Time Unit").Value.ToString,
                                                                                            TimeSeriesTransformaton, aUCI.OpnSeqBlock.Delt)
                                PSLoad += ltimeseries.Attributes.GetDefinedValue("Sum").Value * lMfact * TransformationMultFact / YearCount(aSDateJ, aEDateJ)

                            End If
                        Next

                    End If
                Next lSource

            Case "Heat"
                For Each lSource As HspfPointSource In aReach.PointSources
                    If lSource.Target.Group = "INFLOW" AndAlso lSource.Target.Member = "IHEAT" Then
                        Dim TimeSeriesTransformaton As String = lSource.Tran.ToString
                        Dim VolName As String = lSource.Source.VolName
                        Dim lDSN As Integer = lSource.Source.VolId
                        Dim lMfact As Double = lSource.MFact
                        Dim TransformationMultFact As Double = 0
                        For i As Integer = 0 To aUCI.FilesBlock.Count
                            If aUCI.FilesBlock.Value(i).Typ = VolName Then
                                Dim lFileName As String = AbsolutePath(aUCI.FilesBlock.Value(i).Name.Trim, CurDir())
                                Dim lDataSource As atcDataSource = atcDataManager.DataSourceBySpecification(lFileName)
                                If lDataSource Is Nothing Then
                                    If atcDataManager.OpenDataSource(lFileName) Then
                                        lDataSource = atcDataManager.DataSourceBySpecification(lFileName)
                                    End If
                                End If
                                Dim ltimeseries As atcTimeseries = lDataSource.DataSets.FindData("ID", lDSN)(0)
                                ltimeseries = SubsetByDate(ltimeseries, aSDateJ, aEDateJ, Nothing)
                                TransformationMultFact = MultiFactorForPointSource(ltimeseries.Attributes.GetDefinedValue("Time Step").Value, ltimeseries.Attributes.GetDefinedValue("Time Unit").Value.ToString,
                                                                                            TimeSeriesTransformaton, aUCI.OpnSeqBlock.Delt)
                                PSLoad += ltimeseries.Attributes.GetDefinedValue("Sum").Value * lMfact * TransformationMultFact / YearCount(aSDateJ, aEDateJ)
                            End If
                        Next

                    End If
                Next lSource

            Case "NO3"
                For Each lSource As HspfPointSource In aReach.PointSources
                    If (lSource.Target.Group = "INFLOW" AndAlso lSource.Target.Member = "NUIF1" AndAlso lSource.Target.MemSub1 = 1) OrElse
                        (lSource.Target.Group = "INFLOW" AndAlso lSource.Target.Member = "NUIF1" AndAlso lSource.Target.MemSub1 = 3) Then
                        Dim TimeSeriesTransformaton As String = lSource.Tran.ToString
                        Dim VolName As String = lSource.Source.VolName
                        Dim lDSN As Integer = lSource.Source.VolId
                        Dim lMfact As Double = lSource.MFact
                        Dim TransformationMultFact As Double = 0
                        For i As Integer = 0 To aUCI.FilesBlock.Count
                            If aUCI.FilesBlock.Value(i).Typ = VolName Then
                                Dim lFileName As String = AbsolutePath(aUCI.FilesBlock.Value(i).Name.Trim, CurDir())
                                Dim lDataSource As atcDataSource = atcDataManager.DataSourceBySpecification(lFileName)
                                If lDataSource Is Nothing Then
                                    If atcDataManager.OpenDataSource(lFileName) Then
                                        lDataSource = atcDataManager.DataSourceBySpecification(lFileName)
                                    End If
                                End If
                                Dim ltimeseries As atcTimeseries = lDataSource.DataSets.FindData("ID", lDSN)(0)
                                ltimeseries = SubsetByDate(ltimeseries, aSDateJ, aEDateJ, Nothing)
                                TransformationMultFact = MultiFactorForPointSource(ltimeseries.Attributes.GetDefinedValue("Time Step").Value, ltimeseries.Attributes.GetDefinedValue("Time Unit").Value.ToString,
                                                                                            TimeSeriesTransformaton, aUCI.OpnSeqBlock.Delt)
                                PSLoad += ltimeseries.Attributes.GetDefinedValue("Sum").Value * lMfact * TransformationMultFact / YearCount(aSDateJ, aEDateJ)
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

                        Dim TimeSeriesTransformaton As String = lSource.Tran.ToString
                        Dim VolName As String = lSource.Source.VolName
                        Dim lDSN As Integer = lSource.Source.VolId
                        Dim lMfact As Double = lSource.MFact
                        Dim TransformationMultFact As Double = 0
                        For i As Integer = 0 To aUCI.FilesBlock.Count
                            If aUCI.FilesBlock.Value(i).Typ = VolName Then
                                Dim lFileName As String = AbsolutePath(aUCI.FilesBlock.Value(i).Name.Trim, CurDir())
                                Dim lDataSource As atcDataSource = atcDataManager.DataSourceBySpecification(lFileName)
                                If lDataSource Is Nothing Then
                                    If atcDataManager.OpenDataSource(lFileName) Then
                                        lDataSource = atcDataManager.DataSourceBySpecification(lFileName)
                                    End If
                                End If
                                Dim ltimeseries As atcTimeseries = lDataSource.DataSets.FindData("ID", lDSN)(0)
                                ltimeseries = SubsetByDate(ltimeseries, aSDateJ, aEDateJ, Nothing)
                                TransformationMultFact = MultiFactorForPointSource(ltimeseries.Attributes.GetDefinedValue("Time Step").Value, ltimeseries.Attributes.GetDefinedValue("Time Unit").Value.ToString,
                                                                                            TimeSeriesTransformaton, aUCI.OpnSeqBlock.Delt)
                                PSLoad += ltimeseries.Attributes.GetDefinedValue("Sum").Value * lMfact * TransformationMultFact / YearCount(aSDateJ, aEDateJ)
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
                        Dim TimeSeriesTransformaton As String = lSource.Tran.ToString
                        Dim VolName As String = lSource.Source.VolName
                        Dim lDSN As Integer = lSource.Source.VolId
                        Dim lMfact As Double = lSource.MFact
                        Dim TransformationMultFact As Double = 0
                        For i As Integer = 0 To aUCI.FilesBlock.Count
                            If aUCI.FilesBlock.Value(i).Typ = VolName Then
                                Dim lFileName As String = AbsolutePath(aUCI.FilesBlock.Value(i).Name.Trim, CurDir())
                                Dim lDataSource As atcDataSource = atcDataManager.DataSourceBySpecification(lFileName)
                                If lDataSource Is Nothing Then
                                    If atcDataManager.OpenDataSource(lFileName) Then
                                        lDataSource = atcDataManager.DataSourceBySpecification(lFileName)
                                    End If
                                End If
                                Dim ltimeseries As atcTimeseries = lDataSource.DataSets.FindData("ID", lDSN)(0)
                                ltimeseries = SubsetByDate(ltimeseries, aSDateJ, aEDateJ, Nothing)
                                TransformationMultFact = MultiFactorForPointSource(ltimeseries.Attributes.GetDefinedValue("Time Step").Value, ltimeseries.Attributes.GetDefinedValue("Time Unit").Value.ToString,
                                                                                            TimeSeriesTransformaton, aUCI.OpnSeqBlock.Delt)
                                PSLoad += ltimeseries.Attributes.GetDefinedValue("Sum").Value * lMfact * TransformationMultFact / YearCount(aSDateJ, aEDateJ)
                            End If
                        Next

                    End If
                Next lSource
        End Select



        Return PSLoad
    End Function

    Private Function MultiFactorForPointSource(ByVal aTStep As Integer, ByVal aTimeUnit As String, ByVal aTransformation As String,
                                               ByVal aDelta As atcTimeUnit) As Double
        Dim MultiFactor As Double = 0.0
        If Trim(aTransformation) = "DIV" Then
            MultiFactor = 1.0
        Else
            Select Case aTransformation
                Case "SAME"
                    If aDelta / 60 = 1 AndAlso aTimeUnit = "TUDay" AndAlso aTStep = 1 Then
                        MultiFactor = 24.0
                    End If
            End Select
        End If

        Return MultiFactor
    End Function
    Private Function CalculateDiversion(ByVal aUCI As HspfUci, ByVal aBinaryDataSource As atcDataSource, ByVal aReach As HspfOperation, ByRef aUpstreamInflows As atcCollection,
                                ByVal aDownstreamReachID As Integer, ByVal aOutflow As Double, ByVal aConstituent As String) As Double
        Dim lDiversion As Double = 0.0
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
                        lTotalOutFlow = (aBinaryDataSource.DataSets.FindData("Location", "R:" & aReach.Id).
                            FindData("Constituent", lExitFlowConstituent)(0).Attributes.GetDefinedValue("SumAnnual").Value)
                        lDiversion = aOutflow - lTotalOutFlow

                    Case "BOD-Labile"
                        lExitFlowConstituent = "BODOUT-EXIT" & lExitNUmber
                        lTotalOutFlow = (aBinaryDataSource.DataSets.FindData("Location", "R:" & aReach.Id).
                            FindData("Constituent", lExitFlowConstituent)(0).Attributes.GetDefinedValue("SumAnnual").Value)
                        lDiversion = aOutflow - lTotalOutFlow

                    Case "Heat"
                        lExitFlowConstituent = "OHEAT - EXIT-" & lExitNUmber
                        lTotalOutFlow = (aBinaryDataSource.DataSets.FindData("Location", "R:" & aReach.Id).
                            FindData("Constituent", lExitFlowConstituent)(0).Attributes.GetDefinedValue("SumAnnual").Value)
                        lDiversion = aOutflow - lTotalOutFlow


                    Case "NO3"

                        lExitFlowConstituent = "NO3-OUTDIS-EXIT" & lExitNUmber
                        lTotalOutFlow = (aBinaryDataSource.DataSets.FindData("Location", "R:" & aReach.Id).
                            FindData("Constituent", lExitFlowConstituent)(0).Attributes.GetDefinedValue("SumAnnual").Value)
                        lExitFlowConstituent = "NO2-OUTDIS-EXIT" & lExitNUmber
                        lTotalOutFlow += (aBinaryDataSource.DataSets.FindData("Location", "R:" & aReach.Id).
                            FindData("Constituent", lExitFlowConstituent)(0).Attributes.GetDefinedValue("SumAnnual").Value)
                        lDiversion = aOutflow - lTotalOutFlow

                    Case "TAM"

                        lExitFlowConstituent = "TAM-OUTDIS-EXIT" & lExitNUmber
                        lTotalOutFlow = (aBinaryDataSource.DataSets.FindData("Location", "R:" & aReach.Id).
                            FindData("Constituent", lExitFlowConstituent)(0).Attributes.GetDefinedValue("SumAnnual").Value)
                        lExitFlowConstituent = "TAM-OUTPART-TOT-EXIT" & lExitNUmber
                        lTotalOutFlow += (aBinaryDataSource.DataSets.FindData("Location", "R:" & aReach.Id).
                            FindData("Constituent", lExitFlowConstituent)(0).Attributes.GetDefinedValue("SumAnnual").Value)
                        lDiversion = aOutflow - lTotalOutFlow

                    Case "PO4"

                        lExitFlowConstituent = "PO4-OUTDIS-EXIT" & lExitNUmber
                        lTotalOutFlow = (aBinaryDataSource.DataSets.FindData("Location", "R:" & aReach.Id).
                            FindData("Constituent", lExitFlowConstituent)(0).Attributes.GetDefinedValue("SumAnnual").Value)
                        lExitFlowConstituent = "PO4-OUTPART-TOT-EXIT" & lExitNUmber
                        lTotalOutFlow += (aBinaryDataSource.DataSets.FindData("Location", "R:" & aReach.Id).
                            FindData("Constituent", lExitFlowConstituent)(0).Attributes.GetDefinedValue("SumAnnual").Value)
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
                                        ByVal aSDateJ As Double, ByVal aEDateJ As Double) As Double
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


        End Select

        Return lGENERLoad
    End Function

    Private Function AddFirstSixColumnsReachBudget(ByRef aDataTable As Data.DataTable, ByRef aUnits As String) As DataTable
        Dim column As DataColumn
        column = New DataColumn()
        column.DataType = Type.GetType("System.String")
        column.ColumnName = "OpTypeNumber"
        column.Caption = "Operation Type & Number"
        aDataTable.Columns.Add(column)

        column = New DataColumn()
        column.DataType = Type.GetType("System.String")
        column.ColumnName = "OpDesc"
        column.Caption = "Operation Description"
        aDataTable.Columns.Add(column)

        column = New DataColumn()
        column.DataType = Type.GetType("System.Double")
        column.ColumnName = "NPSLoad"
        column.Caption = "Nonpoint Source Loads (" & aUnits & ")"
        aDataTable.Columns.Add(column)

        column = New DataColumn()
        column.DataType = Type.GetType("System.Double")
        column.ColumnName = "PSLoad"
        column.Caption = "Point Source Loads (" & aUnits & ")"
        aDataTable.Columns.Add(column)

        column = New DataColumn()
        column.DataType = Type.GetType("System.Double")
        column.ColumnName = "GENERLoad"
        column.Caption = "GENER Loads (" & aUnits & ")"
        aDataTable.Columns.Add(column)

        column = New DataColumn()
        column.DataType = Type.GetType("System.Double")
        column.ColumnName = "MassBalance"
        column.Caption = "Mass Balance (" & aUnits & ")"
        aDataTable.Columns.Add(column)

        column = New DataColumn()
        column.DataType = Type.GetType("System.Double")
        column.ColumnName = "Diversion"
        column.Caption = "Diversion (" & aUnits & ")"
        aDataTable.Columns.Add(column)



        column = New DataColumn()
        column.DataType = Type.GetType("System.Double")
        column.ColumnName = "UpstreamIn"
        column.Caption = "Upstream Load (" & aUnits & ")"
        aDataTable.Columns.Add(column)



        Return aDataTable
    End Function
    Private Function AddFirstThreeColumnsLandLoading(ByRef aDataTable As Data.DataTable) As DataTable
        Dim column As DataColumn
        column = New DataColumn()
        column.DataType = Type.GetType("System.String")
        column.ColumnName = "OpTypeNumber"
        column.Caption = "Operation Type & Number"
        aDataTable.Columns.Add(column)

        column = New DataColumn()
        column.DataType = Type.GetType("System.String")
        column.ColumnName = "OpDesc"
        column.Caption = "Operation Description"
        aDataTable.Columns.Add(column)

        column = New DataColumn()
        column.DataType = Type.GetType("System.String")
        column.ColumnName = "Year"
        column.Caption = "Year"
        aDataTable.Columns.Add(column)
        Return aDataTable
    End Function

    Private Function AddMonthlyColumnsColumns(ByRef aDataTable As Data.DataTable) As DataTable
        Dim column As DataColumn
        column = New DataColumn()
        column.DataType = Type.GetType("System.Double")
        column.ColumnName = "Mean Month 01 Jan"
        column.Caption = "Jan"
        aDataTable.Columns.Add(column)

        column = New DataColumn()
        column.DataType = Type.GetType("System.Double")
        column.ColumnName = "Mean Month 02 Feb"
        column.Caption = "Feb"
        aDataTable.Columns.Add(column)

        column = New DataColumn()
        column.DataType = Type.GetType("System.Double")
        column.ColumnName = "Mean Month 03 Mar"
        column.Caption = "Mar"
        aDataTable.Columns.Add(column)

        column = New DataColumn()
        column.DataType = Type.GetType("System.Double")
        column.ColumnName = "Mean Month 04 Apr"
        column.Caption = "Apr"
        aDataTable.Columns.Add(column)

        column = New DataColumn()
        column.DataType = Type.GetType("System.Double")
        column.ColumnName = "Mean Month 05 May"
        column.Caption = "May"
        aDataTable.Columns.Add(column)

        column = New DataColumn()
        column.DataType = Type.GetType("System.Double")
        column.ColumnName = "Mean Month 06 Jun"
        column.Caption = "Jun"
        aDataTable.Columns.Add(column)

        column = New DataColumn()
        column.DataType = Type.GetType("System.Double")
        column.ColumnName = "Mean Month 07 Jul"
        column.Caption = "Jul"
        aDataTable.Columns.Add(column)

        column = New DataColumn()
        column.DataType = Type.GetType("System.Double")
        column.ColumnName = "Mean Month 08 Aug"
        column.Caption = "Aug"
        aDataTable.Columns.Add(column)

        column = New DataColumn()
        column.DataType = Type.GetType("System.Double")
        column.ColumnName = "Mean Month 09 Sep"
        column.Caption = "Sep"
        aDataTable.Columns.Add(column)

        column = New DataColumn()
        column.DataType = Type.GetType("System.Double")
        column.ColumnName = "Mean Month 10 Oct"
        column.Caption = "Oct"
        aDataTable.Columns.Add(column)

        column = New DataColumn()
        column.DataType = Type.GetType("System.Double")
        column.ColumnName = "Mean Month 11 Nov"
        column.Caption = "Nov"
        aDataTable.Columns.Add(column)

        column = New DataColumn()
        column.DataType = Type.GetType("System.Double")
        column.ColumnName = "Mean Month 12 Dec"
        column.Caption = "Dec"
        aDataTable.Columns.Add(column)

        column = New DataColumn()
        column.DataType = Type.GetType("System.Double")
        column.ColumnName = "SumAnnual"
        column.Caption = "Sum Annual"
        aDataTable.Columns.Add(column)
        Return aDataTable
    End Function
    Private Function GetGENERSum(ByVal aUCI As HspfUci, ByVal aSource As HspfConnection, ByVal aSDateJ As Double, ByVal aEDateJ As Double) As Tuple(Of Double, Boolean)
        Dim aGenerSum As Double = 0
        Dim aGENERID As Integer = aSource.Source.VolId
        Dim aGENEROperationisOutputtoWDM As Boolean = False
        Dim aGENEROperation As HspfOperation = aSource.Source.Opn
        For Each EXTTarget As HspfConnection In aGENEROperation.Targets
            If EXTTarget.Target.VolName.Contains("WDM") Then
                aGENEROperationisOutputtoWDM = True
                Dim lWDMFile As String = EXTTarget.Target.VolName.ToString
                Dim lDSN As Integer = EXTTarget.Target.VolId
                For i As Integer = 0 To aUCI.FilesBlock.Count
                    If aUCI.FilesBlock.Value(i).Typ = lWDMFile Then
                        Dim lFileName As String = AbsolutePath(aUCI.FilesBlock.Value(i).Name.Trim, CurDir())
                        Dim lDataSource As atcDataSource = atcDataManager.DataSourceBySpecification(lFileName)
                        If lDataSource Is Nothing Then
                            If atcDataManager.OpenDataSource(lFileName) Then
                                lDataSource = atcDataManager.DataSourceBySpecification(lFileName)
                            End If
                        End If
                        Dim ltimeseries As atcTimeseries = lDataSource.DataSets.FindData("ID", lDSN)(0)
                        ltimeseries = SubsetByDate(ltimeseries, aSDateJ, aEDateJ, Nothing)
                        aGenerSum = ltimeseries.Attributes.GetDefinedValue("Sum").Value / YearCount(aSDateJ, aEDateJ)

                    End If
                Next
            End If
        Next EXTTarget

        Return New Tuple(Of Double, Boolean)(aGenerSum, aGENEROperationisOutputtoWDM)
    End Function


End Module
