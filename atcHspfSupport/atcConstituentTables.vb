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
        Land_Constituent_Table = New DataTable("LandConstituentTable")
        Dim QualityConstituent As Boolean = False
        Dim lOutflowDataTypes As String() = ConstituentList(aBalanceType, QualityConstituent)
        Dim lDataForBoxWhiskerPlot As New BoxWhiskerItem
        lDataForBoxWhiskerPlot.Constituent = aBalanceType
        lDataForBoxWhiskerPlot.Scenario = aScenario
        lDataForBoxWhiskerPlot.TimeSpan = TimeSpanAsString(aSDateJ, aEDateJ, "Analysis Period: ")
        Dim listLanduses As New List(Of String)
        Dim landUseSumAnnualValues As New atcCollection
        Dim landUseNameForTheCollection As String = ""
        Dim lUnits As String = ""

        Land_Constituent_Table = AddFirstThreeColumnsLandLoading(Land_Constituent_Table)
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
                    Dim lOutflowDataTypes1 As String() = ConstituentList(aBalanceType)
                    Dim lTS As New atcTimeseries(Nothing)
                    Dim AddTS As New atcDataGroup
                    Dim lTotalTS As New atcTimeseries(Nothing)
                    For Each lOutflowDataType As String In lOutflowDataTypes1

                        If lOutflowDataType = "TotalOutflow" Then
                            lTS = lTotalTS
                        Else
                            lTS = aBinaryData.DataSets.FindData("Location", LocationName).FindData("Constituent", lOutflowDataType)(0)
                        End If
                        If lTS IsNot Nothing Then
                            lTS = SubsetByDate(lTS, aSDateJ, aEDateJ, Nothing)
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
                        End If

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
                    Dim lOutflowDataTypes1 As String() = ConstituentList(aBalanceType)
                    Dim lTS As New atcTimeseries(Nothing)
                    Dim AddTS As New atcDataGroup
                    Dim lTotalTS As New atcTimeseries(Nothing)
                    For Each lOutflowDataType As String In lOutflowDataTypes1

                        If lOutflowDataType = "TotalOutflow" Then
                            lTS = lTotalTS
                        Else
                            lTS = aBinaryData.DataSets.FindData("Location", LocationName).FindData("Constituent", lOutflowDataType)(0)
                        End If
                        If lTS IsNot Nothing Then
                            lTS = SubsetByDate(lTS, aSDateJ, aEDateJ, Nothing)
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
                        End If

                    Next lOutflowDataType

                Next lOperation
#End Region

#Region "Case Sediment"
            Case "Sediment"
                Dim lConversionFactor As Double = 1.0
                If aUCI.GlobalBlock.EmFg = 1 Then
                    lUnits = "lbs/ac"
                    lConversionFactor = 2240
                ElseIf aUCI.GlobalBlock.EmFg = 2 Then
                    lUnits = "kgs/ha"
                    lConversionFactor = 1000
                End If
                lDataForBoxWhiskerPlot.Units = (lUnits & "/yr")

                Dim column As New DataColumn
                Dim row As DataRow

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
                    If Not (lOperation.Name = "PERLND" OrElse lOperation.Name = "IMPLND") Then Continue For
                    Dim LocationName As String = lOperation.Name.Substring(0, 1) & ":" & lOperation.Id
                    landUseNameForTheCollection = lOperation.Name.Substring(0, 1) & ":" & lOperation.Description
                    If Not listLanduses.Contains(landUseNameForTheCollection) Then
                        listLanduses.Add(landUseNameForTheCollection)
                    End If

                    Dim lTSNumber As Integer = 0
                    Dim lOutflowDataTypes1 As String() = ConstituentList(aBalanceType)
                    Dim lTS As New atcTimeseries(Nothing)
                    Dim AddTS As New atcDataGroup
                    Dim lTotalTS As New atcTimeseries(Nothing)
                    For Each lOutflowDataType As String In lOutflowDataTypes1

                        If lOutflowDataType = "TotalOutflow" Then
                            lTS = lTotalTS
                        Else
                            lTS = aBinaryData.DataSets.FindData("Location", LocationName).FindData("Constituent", lOutflowDataType)(0)

                        End If
                        If lTS IsNot Nothing Then
                            lTS = SubsetByDate(lTS, aSDateJ, aEDateJ, Nothing)
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
                        End If

                    Next lOutflowDataType

                Next lOperation
#End Region

#Region "Case Else"
            Case Else

                Dim column As DataColumn
                Dim row As DataRow
                column = New DataColumn()

                column = New DataColumn()
                column.DataType = Type.GetType("System.String")
                column.ColumnName = "ConstName"
                column.Caption = "Constituent Name"
                Land_Constituent_Table.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.String")
                column.ColumnName = "ConstNameEXP"
                column.Caption = "Constituent Name in EXP+"
                Land_Constituent_Table.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.String")
                column.ColumnName = "Unit"
                column.Caption = "Unit"
                Land_Constituent_Table.Columns.Add(column)

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
                    If Not (lOperation.Name = "PERLND" OrElse lOperation.Name = "IMPLND") Then Continue For
                    'If lOperation.Name = "IMPLND" Then Stop
                    Dim LocationName As String = lOperation.Name.Substring(0, 1) & ":" & lOperation.Id
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
                        ElseIf constituent.ConstNameForEXPPlus.Contains("lab") Then
                            lMultipleIndex = 2
                        End If

                        If Not lConstituentNames.Contains(constituent.ConstNameForEXPPlus) Then
                            lConstituentNames.Add(constituent.ConstNameForEXPPlus)
                        End If

                        Dim lOutflowDataTypes1 As String() = ConstituentList(aBalanceType, constituent.ConstituentNameInUCI)
                        Dim lTSNumber As Integer = 0
                        Dim lTS As New atcTimeseries(Nothing)
                        Dim AddTS As New atcDataGroup
                        Dim lTotalTS As New atcTimeseries(Nothing)

                        For Each lOutflowDataType As String In lOutflowDataTypes1

                            If lOutflowDataType.StartsWith("TotalOutflow") And lTotalTS.Dates IsNot Nothing Then
                                lTS = lTotalTS
                            Else
                                lTS = aBinaryData.DataSets.FindData("Location", LocationName).FindData("Constituent", lOutflowDataType)(0)
                            End If
                            Dim lMassLinkFactor As Double = 1.0
                            If lTS Is Nothing Then Continue For
                            lTS = SubsetByDate(lTS, aSDateJ, aEDateJ, Nothing)
                            For Each lConnection As HspfConnection In lOperation.Targets
                                If lConnection.Target.VolName = "RCHRES" Then
                                    Dim aReach As HspfOperation = aUCI.OpnBlks("RCHRES").OperFromID(lConnection.Target.VolId)
                                    Dim aConversionFactor As Double = 0.0
                                    aConversionFactor = ConversionFactorfromOxygen(aUCI, constituent.ReportType, aReach)
                                    Dim lMassLinkID As Integer = lConnection.MassLink
                                    If Not lMassLinkID = 0 Then
                                        lMassLinkFactor = FindMassLinkFactor(aUCI, lMassLinkID, lOutflowDataType,
                                                                             constituent.ReportType, aConversionFactor, lMultipleIndex)
                                        Exit For
                                    End If

                                End If
                            Next lConnection

                            If Not lOutflowDataType.StartsWith("TotalOutflow") Then lTS *= lMassLinkFactor
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
                                    row("WASHQS") = lValue
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
                            row("OpTypeNumber") = foundRows(0)("OpTypeNumber")
                            row("OpDesc") = foundRows(0)("OpDesc")
                            row("Year") = foundRows(0)("Year")
                            row("ConstName") = aBalanceType
                            row("ConstNameEXP") = aBalanceType
                            row("Unit") = foundRows(0)("Unit")

                            For Each foundrow As DataRow In foundRows
                                For i As Integer = 6 To foundrow.ItemArray.Length - 1
                                    If IsDBNull(row(i)) And Not IsDBNull(foundrow(i)) Then
                                        row(i) = foundrow(i)
                                    ElseIf Not (IsDBNull(row(i)) AndAlso IsDBNull(foundrow(i))) Then
                                        row(i) += foundrow(i)
                                    End If
                                Next i
                            Next foundrow
                            Land_Constituent_Table.Rows.Add(row)
                        Next lYear
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
        lReport.AppendLine("Run Made " & aRunMade)
        lReport.AppendLine(TimeSpanAsString(aUCI.GlobalBlock.SDateJ, aUCI.GlobalBlock.EdateJ, "Analysis Period: "))
        SaveFileString(aoutfoldername & aBalanceType & "Land_Tabular.txt", lReport.ToString)

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

                    lDataForBoxWhiskerPlot.Units = "(" & foundRows(0)("Unit") & "/yr)"
                    landUseSumAnnualValues.Add(item, Values.ToArray)
                Next item
                lDataForBoxWhiskerPlot.LabelValueCollection = landUseSumAnnualValues
                CreateGraph_BoxAndWhisker(lDataForBoxWhiskerPlot, aoutfoldername & Constituent.ConstNameForEXPPlus & "_BoxWhisker.png")
                landUseSumAnnualValues.Clear()
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
                landUseSumAnnualValues.Add(item, Values.ToArray)
            Next item
            lDataForBoxWhiskerPlot.Units = "(" & lUnits & "/yr)"
            lDataForBoxWhiskerPlot.LabelValueCollection = landUseSumAnnualValues
            CreateGraph_BoxAndWhisker(lDataForBoxWhiskerPlot, aoutfoldername & aBalanceType & "_BoxWhisker.png")
            landUseSumAnnualValues.Clear()
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
                    Dim lDiversion As Double = CalculateDiversion(lReach, lUpstreamInflows, lDownstreamReachID, lOutflow)
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
                                row(ColumnName) = HspfTable.NumFmtRE(CalculatePSLoad(aUCI, lReach, aSDateJ, aEDateJ, aBalanceType), 10)
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
                lReport.AppendLine("Run Made " & aRunMade)
                lReport.AppendLine(TimeSpanAsString(aUCI.GlobalBlock.SDateJ, aUCI.GlobalBlock.EdateJ, "Analysis Period: "))
                SaveFileString(aoutfoldername & aBalanceType & "Reach_Tabular.txt", lReport.ToString)
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
                    Dim lDiversion As Double = CalculateDiversion(lReach, lUpstreamInflows, lDownstreamReachID, lOutflow)
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
                                row(ColumnName) = HspfTable.NumFmtRE(CalculatePSLoad(aUCI, lReach, aSDateJ, aEDateJ, aBalanceType), 10)
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
                lReport.AppendLine("Run Made " & aRunMade)
                lReport.AppendLine(TimeSpanAsString(aUCI.GlobalBlock.SDateJ, aUCI.GlobalBlock.EdateJ, "Analysis Period: "))
                SaveFileString(aoutfoldername & aBalanceType & "Reach_Tabular.txt", lReport.ToString)
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
                    Dim lDiversion As Double = CalculateDiversion(lReach, lUpstreamInflows, lDownstreamReachID, lOutflow)
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
                lReport.AppendLine("Run Made " & aRunMade)
                lReport.AppendLine(TimeSpanAsString(aUCI.GlobalBlock.SDateJ, aUCI.GlobalBlock.EdateJ, "Analysis Period: "))
                SaveFileString(aoutfoldername & aBalanceType & "Reach_Tabular.txt", lReport.ToString)
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
                            Dim lDiversion As Double = CalculateDiversion(lReach, lUpstreamInflows, lDownstreamReachID, lOutflow)
                            Dim lGENERLoad As Double = CalculateGENERLoad(aUCI, lReach, aBalanceType, aSDateJ, aEDateJ)
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
                        lReport.AppendLine("Run Made " & aRunMade)
                        lReport.AppendLine(TimeSpanAsString(aUCI.GlobalBlock.SDateJ, aUCI.GlobalBlock.EdateJ, "Analysis Period: "))
                        SaveFileString(aoutfoldername & lReachConstituent & "Reach_Tabular.txt", lReport.ToString)
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
                            Dim lDiversion As Double = CalculateDiversion(lReach, lUpstreamInflows, lDownstreamReachID, lOutflow)
                            Dim lGENERLoad As Double = CalculateGENERLoad(aUCI, lReach, aBalanceType, aSDateJ, aEDateJ)
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
                        lReport.AppendLine("Run Made " & aRunMade)
                        lReport.AppendLine(TimeSpanAsString(aUCI.GlobalBlock.SDateJ, aUCI.GlobalBlock.EdateJ, "Analysis Period: "))
                        SaveFileString(aoutfoldername & lReachConstituent & "Reach_Tabular.txt", lReport.ToString)
                    End If
                Next lConstituent
#End Region

        End Select
    End Sub

    Private Function CalculateNPSLoad(ByVal aUCI As HspfUci, ByVal aReach As HspfOperation, ByVal aConstituentName As String) As Double
        Dim NPSLoad As Double = 0.0
        Dim SelectExpression As String = ""
        For Each lReachSource As HspfConnection In aReach.Sources
            If Not (lReachSource.Source.VolName = "PERLND" OrElse lReachSource.Source.VolName = "IMPLND") Then Continue For
            If lReachSource.Source.Opn Is Nothing Then Continue For
            Dim lConnectionArea As Double = lReachSource.MFact
            Dim lOperationTypeNumber As String = SafeSubstring(lReachSource.Source.VolName, 0, 1) & ":" & lReachSource.Source.VolId
            If aConstituentName = "NO3" Or aConstituentName = "TAM" Or aConstituentName = "PO4" Then
                SelectExpression = "OpTypeNumber= '" & lOperationTypeNumber & "' And Year = 'SumAnnual' And ConstNameEXP = '" & aConstituentName & "'"
            Else
                SelectExpression = "OpTypeNumber= '" & lOperationTypeNumber & "' And Year = 'SumAnnual'"
            End If

            Dim foundRows() As DataRow = Land_Constituent_Table.Select(SelectExpression)
            NPSLoad += lConnectionArea * foundRows(0)("TotalOutflow")

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
                                PSLoad += ltimeseries.Attributes.GetDefinedValue("Sum").Value * lMfact / YearCount(aSDateJ, aEDateJ)
                                PSLoad *= MultiFactorForPointSource(ltimeseries.Attributes.GetDefinedValue("Time Step").Value, ltimeseries.Attributes.GetDefinedValue("Time Unit").Value.ToString,
                                                                                            TimeSeriesTransformaton, aUCI.OpnSeqBlock.Delt)
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
                                PSLoad += ltimeseries.Attributes.GetDefinedValue("Sum").Value * lMfact / YearCount(aSDateJ, aEDateJ)
                                PSLoad *= MultiFactorForPointSource(ltimeseries.Attributes.GetDefinedValue("Time Step").Value, ltimeseries.Attributes.GetDefinedValue("Time Unit").Value.ToString,
                                                                                            TimeSeriesTransformaton, aUCI.OpnSeqBlock.Delt)
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
                                PSLoad += ltimeseries.Attributes.GetDefinedValue("Sum").Value * lMfact / YearCount(aSDateJ, aEDateJ)
                                PSLoad *= MultiFactorForPointSource(ltimeseries.Attributes.GetDefinedValue("Time Step").Value, ltimeseries.Attributes.GetDefinedValue("Time Unit").Value.ToString,
                                                                                            TimeSeriesTransformaton, aUCI.OpnSeqBlock.Delt)
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
                                PSLoad += ltimeseries.Attributes.GetDefinedValue("Sum").Value * lMfact / YearCount(aSDateJ, aEDateJ)
                                PSLoad *= MultiFactorForPointSource(ltimeseries.Attributes.GetDefinedValue("Time Step").Value, ltimeseries.Attributes.GetDefinedValue("Time Unit").Value.ToString,
                                                                                            TimeSeriesTransformaton, aUCI.OpnSeqBlock.Delt)
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
                                PSLoad += ltimeseries.Attributes.GetDefinedValue("Sum").Value * lMfact / YearCount(aSDateJ, aEDateJ)
                                PSLoad *= MultiFactorForPointSource(ltimeseries.Attributes.GetDefinedValue("Time Step").Value, ltimeseries.Attributes.GetDefinedValue("Time Unit").Value.ToString,
                                                                                            TimeSeriesTransformaton, aUCI.OpnSeqBlock.Delt)
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
    Private Function CalculateDiversion(ByVal aReach As HspfOperation, ByRef aUpstreamInflows As atcCollection,
                                ByVal aDownstreamReachID As Integer, ByVal aOutflow As Double) As Double
        Dim lDiversion As Double = 0.0
        Try
            If aReach.Tables("GEN-INFO").Parms("NEXITS").Value = 1 Then
                Logger.Dbg(aReach.Id)
                aUpstreamInflows.Increment(aDownstreamReachID, aOutflow)
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
            Case "Heat"
                For Each lSource As HspfConnection In aReach.Sources
                    If lSource.Source.VolName = "GENER" AndAlso lSource.Target.Group = "INFLOW" AndAlso lSource.Target.Member = "IHEAT" Then

                        Dim lGENERID As Integer = lSource.Source.VolId
                        Dim lMfact As Double = lSource.MFact
                        Dim lGENEROperationisOutputtoWDM As Boolean = False
                        Dim lGENEROperation As HspfOperation = lSource.Source.Opn
                        For Each EXTTarget As HspfConnection In lGENEROperation.Targets

                            If EXTTarget.Target.VolName.Contains("WDM") Then
                                lGENEROperationisOutputtoWDM = True
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
                                        lGENERLoad += ltimeseries.Attributes.GetDefinedValue("Sum").Value * lMfact / YearCount(aSDateJ, aEDateJ)

                                    End If
                                Next

                            End If

                        Next EXTTarget
                        If Not lGENEROperationisOutputtoWDM Then
                            Logger.Dbg("GENER Loadings Issue: The RCHRES operation " & aReach.Id & " has loadings input for the constituent " & aConstituentName & " from GENER connections in the Network Block. Please make sure that these GENER operations output to a WDM dataset for accurate source accounting.")


                        End If

                    End If
                Next lSource

            Case "Sediment"
                For Each lSource As HspfConnection In aReach.Sources
                    If lSource.Source.VolName = "GENER" AndAlso lSource.Target.Group = "INFLOW" AndAlso lSource.Target.Member = "ISED" Then

                        Dim lGENERID As Integer = lSource.Source.VolId
                        Dim lMfact As Double = lSource.MFact
                        Dim lGENEROperationisOutputtoWDM As Boolean = False
                        Dim lGENEROperation As HspfOperation = lSource.Source.Opn
                        For Each EXTTarget As HspfConnection In lGENEROperation.Targets

                            If EXTTarget.Target.VolName.Contains("WDM") Then
                                lGENEROperationisOutputtoWDM = True
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
                                        lGENERLoad += ltimeseries.Attributes.GetDefinedValue("Sum").Value * lMfact / YearCount(aSDateJ, aEDateJ)

                                    End If
                                Next

                            End If
                        Next EXTTarget
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
        column.ColumnName = "Diversion"
        column.Caption = "Diversion (" & aUnits & ")"
        aDataTable.Columns.Add(column)

        column = New DataColumn()
        column.DataType = Type.GetType("System.Double")
        column.ColumnName = "MassBalance"
        column.Caption = "Mass Balance (" & aUnits & ")"
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
End Module
