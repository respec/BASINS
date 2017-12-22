Imports atcUtility
Imports atcData
Imports MapWinUtility
Imports atcUCI
Imports System.Data.DataTable
Imports System.Data
Public Module atcQUALReports
    Private pPERLND As atcCollection
    Private pIMPLND As atcCollection
    Public QUALDataCollecion As List(Of ConstOutflowDatafromLand)
    'This function reads a UCI file, Binary files and the list of constituents
    'The output result is a list of a class ConstOutflowDatafromLand
    'ConstOutflowDataFromLand is a class where each individual land operation's outflow data is stored.

    Public Function QUALReports(ByVal aUCI As HspfUci,
                                ByRef aScenariosResults As atcDataSource,
                                ByRef QUALDetails As List(Of ConstituentProperties)) As List(Of ConstOutflowDatafromLand)

        Dim lOutputQUALData As New List(Of ConstOutflowDatafromLand)
        For Each loperation As HspfOperation In aUCI.OpnSeqBlock.Opns
            'Starting through each opertaion
            'If loperation.Name = "IMPLND" Then Stop
            If loperation.Name = "RCHRES" Then Continue For
            Dim LocationName As String = loperation.Name.Substring(0, 1) & ":" & loperation.Id
            For Each constituent As ConstituentProperties In QUALDetails
                'QUALDetails is a list of constituent like NO3, NH4, Ref-OrgN, lab-OrgN
                'Going through each constituent

                Dim ConstituentIndividualData As New ConstOutflowDatafromLand
                ConstituentIndividualData.LandType = loperation.Name
                ConstituentIndividualData.OperationNumber = loperation.Id
                ConstituentIndividualData.OperationName = loperation.Description
                Dim lOutflowDataTypes As String() = ConstituentList(constituent.ReportType, constituent.ConstituentNameInUCI)
                Dim QUALNameInUCI As String = constituent.ConstituentNameInUCI

                Dim lMultipleIndex As Integer = 0
                If constituent.ConstNameForEXPPlus.ToLower.Contains("ref") Then
                    lMultipleIndex = 1
                ElseIf constituent.ConstNameForEXPPlus.Contains("lab") Then
                    lMultipleIndex = 2
                End If

                ConstituentIndividualData.LandConstituentNameInUCI = constituent.ConstituentNameInUCI
                ConstituentIndividualData.LandConstituentNameForHSPEXP = constituent.ConstNameForEXPPlus
                ConstituentIndividualData.Units = constituent.ConstituentUnit
                Dim lTSGroup As New atcTimeseriesGroup
                Dim lTotalTS As New atcTimeseries(Nothing)
                For Each OutflowData As String In lOutflowDataTypes

                    Dim lMassLinkFactor As Double = 1.0
                    Dim lTS As atcTimeseries = aScenariosResults.DataSets.FindData("Constituent", OutflowData).FindData("Location", LocationName)(0)

                    'Getting the time series of constituent and the location from the binary file data.

                    If Not lTS Is Nothing Then
                        For Each lConnection As HspfConnection In loperation.Targets
                            If lConnection.Target.VolName = "RCHRES" Then
                                Dim aReach As HspfOperation = aUCI.OpnBlks("RCHRES").OperFromID(lConnection.Target.VolId)
                                Dim aConversionFactor As Double = 0.0
                                aConversionFactor = ConversionFactorfromOxygen(aUCI, constituent.ReportType, aReach)
                                Dim lMassLinkID As Integer = lConnection.MassLink
                                If Not lMassLinkID = 0 Then
                                    lMassLinkFactor = FindMassLinkFactor(aUCI, lMassLinkID, OutflowData,
                                                                             constituent.ReportType, aConversionFactor, lMultipleIndex)
                                    Exit For
                                End If

                            End If
                        Next lconnection

                        lTS = lTS * lMassLinkFactor
                        lTSGroup.Add(lTS)

                    ElseIf OutflowData.ToLower.Contains("total outflow") Then
                        'lTotalTS = lTSGroup(0) * 0
                        For Each AddTS As atcTimeseries In lTSGroup
                            If lTotalTS.Dates Is Nothing Then
                                lTotalTS = AddTS + 0
                            Else
                                lTotalTS += AddTS
                            End If

                        Next

                        lTotalTS.Attributes.SetValue("Constituent", "total")
                        lTSGroup.Add(lTotalTS)
                    End If

                    If OutflowData.Contains("-") AndAlso OutflowData.Split("-")(1).ToLower.Contains("total") AndAlso Not OutflowData.Split("-")(0).ToLower.Contains("total") Then

                        Dim lTotalTS2 As New atcTimeseries(Nothing)
                        For i As Integer = 0 To lOutputQUALData.Count - 1
                            Dim ConstituentName As String = OutflowData.Split("-")(0) & "-" & lOutputQUALData(i).LandConstituentNameInUCI

                            For Each AddTS As atcTimeseries In lOutputQUALData(i).MonthlyTimeSeriesOutflowData.FindData("constituent", ConstituentName).FindData("Location", LocationName)
                                If lTotalTS2.Dates Is Nothing Then
                                    lTotalTS2 = AddTS + 0
                                Else
                                    lTotalTS2 += AddTS
                                End If

                            Next AddTS

                        Next i
                        If lTotalTS2.Dates IsNot Nothing Then
                            lTotalTS2.Attributes.SetValue("Constituent", OutflowData)
                            lTSGroup.Add(lTotalTS2)
                        End If

                    End If
                    'The group of time series for each constituent and each operation get added here.
                Next OutflowData
                ConstituentIndividualData.MonthlyTimeSeriesOutflowData = lTSGroup
                'The individudal object ConstOutflowDatafromLand gets added to the list lOutputQUALData.
                lOutputQUALData.Add(ConstituentIndividualData)

            Next constituent
        Next loperation

        Return lOutputQUALData

    End Function

    Public Function PrintQUALReports(ByVal OutputQUALData As List(Of ConstOutflowDatafromLand),
                                     ByVal aScenario As String,
                                     ByVal aRunMade As String,
                                     ByVal aBalanceType As String) As atcReport.IReport
        'A list of ConstOutflowData contains inromation about individual constituents (like NO3, or NH4)
        'A list of list ConstOutflowData will contain information about TN, TP etc.
        'This function prepares a text report for constituents like TN and TP.
        Dim lReport As New atcReport.ReportText

        Dim QualityConstituent As Boolean = False
        Dim lOutflowDataTypes As String() = ConstituentList(aBalanceType, QualityConstituent)

        Select Case aBalanceType
            Case "Water", "DO", "Heat"

                Dim lDataForBoxWhiskerPlot As New BoxWhiskerItem

                Dim Water_Do_Heat As New DataTable("ConstituentTable")
                Dim column As DataColumn
                Dim row As DataRow
                column = New DataColumn()
                column.DataType = Type.GetType("System.String")
                column.ColumnName = "OpType"
                column.Caption = "Operation Type"
                Water_Do_Heat.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.Int32")
                column.ColumnName = "OpNum"
                column.Caption = "Operation Number"
                Water_Do_Heat.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.String")
                column.ColumnName = "OpDesc"
                column.Caption = "Operation Description"
                Water_Do_Heat.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.String")
                column.ColumnName = "ConstName"
                column.Caption = "Constituent Name"
                Water_Do_Heat.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.String")
                column.ColumnName = "Year"
                column.Caption = "Year"
                Water_Do_Heat.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.String")
                column.ColumnName = "Unit"
                column.Caption = "Unit"
                Water_Do_Heat.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.Double")
                column.ColumnName = "SurfOutflow"
                column.Caption = "Surface Outflow"
                Water_Do_Heat.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.Double")
                column.ColumnName = "InterOutflow"
                column.Caption = "Interflow Outflow"
                Water_Do_Heat.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.Double")
                column.ColumnName = "GroundOutflow"
                column.Caption = "Groundwater Outflow"
                Water_Do_Heat.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.Double")
                column.ColumnName = "TotalOutflow"
                column.Caption = "Total Outflow"
                Water_Do_Heat.Columns.Add(column)
                Dim RowNumber As Integer = 0
                Dim listLanduses As New List(Of String)
                For Each lOperation As ConstOutflowDatafromLand In OutputQUALData
                    listLanduses.Add(SafeSubstring(lOperation.LandType, 0, 1) & ":" & lOperation.OperationName)
                    Dim NumberOfFields As Integer = 0
                    Dim lTSNumber As Integer = 0
                    For Each lTS As atcTimeseries In lOperation.MonthlyTimeSeriesOutflowData

                        Dim lTsYearly As atcTimeseries = Aggregate(lTS, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv)
                        Dim lSumAnnual As Double = lTsYearly.Attributes.GetDefinedValue("SumAnnual").Value
                        Dim lTSAttributes As String = lTsYearly.Attributes.GetDefinedValue("Constituent").Value
                        If lTSNumber > 0 Then RowNumber -= (lTsYearly.numValues + 1)

                        For i As Integer = 1 To lTsYearly.numValues + 1

                            row = Water_Do_Heat.NewRow
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
                                row("OpType") = lOperation.LandType
                                row("OpNum") = lOperation.OperationNumber
                                row("OpDesc") = lOperation.OperationName
                                row("ConstName") = lOperation.LandConstituentNameInUCI
                                row("Year") = Year
                                row("Unit") = lOperation.Units
                                row("SurfOutflow") = lValue
                                Water_Do_Heat.Rows.Add(row)
                            ElseIf lTSAttributes.StartsWith("I") Then
                                Water_Do_Heat.Rows(RowNumber - 1)("InterOutflow") = lValue
                            ElseIf lTSAttributes.StartsWith("A") Then
                                Water_Do_Heat.Rows(RowNumber - 1)("GroundOutflow") = lValue
                            ElseIf lTSAttributes.Contains("total") Then
                                Water_Do_Heat.Rows(RowNumber - 1)("TotalOutflow") = lValue

                            End If

                        Next i
                        lTSNumber += 1
                    Next lTS

                Next lOperation

                Dim TextToWrite As String = ""
                For Each TableColumn As DataColumn In Water_Do_Heat.Columns
                    TextToWrite &= TableColumn.Caption & vbTab
                Next
                lReport.AppendLine(TextToWrite)

                Dim BoxWhiskerDataSet As New BoxWhiskerItem

                For Each TableRow As DataRow In Water_Do_Heat.Rows
                    TextToWrite = ""
                    For Each TableColumn As DataColumn In Water_Do_Heat.Columns
                        TextToWrite &= TableRow(TableColumn) & vbTab

                    Next TableColumn

                    lReport.AppendLine(TextToWrite)
                Next TableRow
                lReport.AppendLine("Run Made " & aRunMade)

                For Each item As String In listLanduses
                    Dim OpType1 As String = ""
                    If item.StartsWith("P") Then
                        OpType1 = "PERLND"
                    Else
                        OpType1 = "IMPLND"
                    End If
                    Dim LU As String = SafeSubstring(item, 2)
                    Dim Values As New List(Of Double)
                    For Each TableRow As DataRow In Water_Do_Heat.Rows
                        If TableRow("OpType") = OpType1 AndAlso
                                TableRow("OpDesc") = LU AndAlso
                                TableRow("Year") = "SumAnnual" Then
                            Values.Add(TableRow("Total Outflow"))

                        End If

                        lReport.AppendLine(TextToWrite)
                    Next TableRow

                Next




            Case "Sediment"

                Dim SedConstTable As New DataTable("ConstituentTable")
                Dim column As DataColumn
                Dim row As DataRow
                column = New DataColumn()
                column.DataType = Type.GetType("System.String")
                column.ColumnName = "OpType"
                column.Caption = "Operation Type"
                SedConstTable.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.Int32")
                column.ColumnName = "OpNum"
                column.Caption = "Operation Number"
                SedConstTable.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.String")
                column.ColumnName = "OpDesc"
                column.Caption = "Operation Description"
                SedConstTable.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.String")
                column.ColumnName = "ConstName"
                column.Caption = "Constituent Name"
                SedConstTable.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.String")
                column.ColumnName = "Year"
                column.Caption = "Year"
                SedConstTable.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.String")
                column.ColumnName = "Unit"
                column.Caption = "Unit"
                SedConstTable.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.Double")
                column.ColumnName = "WSSD"
                column.Caption = "Wash Off of detached Sediment"
                SedConstTable.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.Double")
                column.ColumnName = "SCRSD"
                column.Caption = "Scour of Matrix Soil"
                SedConstTable.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.Double")
                column.ColumnName = "TotalOutflow"
                column.Caption = "Total Outflow"
                SedConstTable.Columns.Add(column)
                Dim RowNumber As Integer = 0
                For Each lOperation As ConstOutflowDatafromLand In OutputQUALData
                    Dim NumberOfFields As Integer = 0
                    Dim lTSNumber As Integer = 0
                    For Each lTS As atcTimeseries In lOperation.MonthlyTimeSeriesOutflowData

                        Dim lTsYearly As atcTimeseries = Aggregate(lTS, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv)
                        Dim lSumAnnual As Double = lTsYearly.Attributes.GetDefinedValue("SumAnnual").Value
                        Dim lTSAttributes As String = lTsYearly.Attributes.GetDefinedValue("Constituent").Value
                        If lTSNumber > 0 Then RowNumber -= (lTsYearly.numValues + 1)

                        For i As Integer = 1 To lTsYearly.numValues + 1

                            row = SedConstTable.NewRow
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
                                row("OpType") = lOperation.LandType
                                row("OpNum") = lOperation.OperationNumber
                                row("OpDesc") = lOperation.OperationName
                                row("ConstName") = lOperation.LandConstituentNameInUCI
                                row("Year") = Year
                                row("Unit") = lOperation.Units
                                row("WSSD") = lValue
                                SedConstTable.Rows.Add(row)
                            ElseIf lTSAttributes.StartsWith("SCRSD") Then
                                SedConstTable.Rows(RowNumber - 1)("SCRSD") = lValue
                            ElseIf lTSAttributes.Contains("total") Then
                                SedConstTable.Rows(RowNumber - 1)("TotalOutflow") = lValue
                            End If

                        Next i

                        lTSNumber += 1
                    Next lTS

                Next lOperation

                Dim TextToWrite As String = ""
                For Each TableColumn As DataColumn In SedConstTable.Columns
                    TextToWrite &= TableColumn.Caption & vbTab
                Next
                lReport.AppendLine(TextToWrite)
                For Each TableRow As DataRow In SedConstTable.Rows
                    TextToWrite = ""
                    For Each TableColumn As DataColumn In SedConstTable.Columns

                        TextToWrite &= TableRow(TableColumn) & vbTab

                    Next TableColumn

                    lReport.AppendLine(TextToWrite)
                Next TableRow
                lReport.AppendLine("Run Made " & aRunMade)

            Case Else
                Dim QUALConstTable As New DataTable("QUALConstituentTable")
                Dim column As DataColumn
                Dim row As DataRow
                column = New DataColumn()
                column.DataType = Type.GetType("System.String")
                column.ColumnName = "OpType"
                column.Caption = "Operation Type"
                QUALConstTable.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.Int32")
                column.ColumnName = "OpNum"
                column.Caption = "Operation Number"
                QUALConstTable.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.String")
                column.ColumnName = "OpDesc"
                column.Caption = "Operation Description"
                QUALConstTable.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.String")
                column.ColumnName = "ConstName"
                column.Caption = "Constituent Name"
                QUALConstTable.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.String")
                column.ColumnName = "ConstNameEXP"
                column.Caption = "Constituent Name in EXP+"
                QUALConstTable.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.String")
                column.ColumnName = "Year"
                column.Caption = "Year"
                QUALConstTable.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.String")
                column.ColumnName = "Unit"
                column.Caption = "Unit"
                QUALConstTable.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.Double")
                column.ColumnName = "WASHQS"
                column.Caption = "Removal of QUALSD by association with detached sediment Runoff"
                QUALConstTable.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.Double")
                column.ColumnName = "SCRQS"
                column.Caption = "Removal of QUALSD with scour of matrix soil"
                QUALConstTable.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.Double")
                column.ColumnName = "SOQO"
                column.Caption = "Washoff of QUALOF from surface"
                QUALConstTable.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.Double")
                column.ColumnName = "IOQUAL"
                column.Caption = "Outflow of QUAL in interflow"
                QUALConstTable.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.Double")
                column.ColumnName = "AOQUAL"
                column.Caption = "Outflow of QUAL in Groundwater flow"
                QUALConstTable.Columns.Add(column)

                column = New DataColumn()
                column.DataType = Type.GetType("System.Double")
                column.ColumnName = "TotalOutflow"
                column.Caption = "Total Outflow"
                QUALConstTable.Columns.Add(column)
                Dim RowNumber As Integer = 0
                For Each lOperation As ConstOutflowDatafromLand In OutputQUALData
                    Dim lOperationTotal As New ConstOutflowDatafromLand
                    Dim lTSNumber As Integer = 0
                    For Each lTS As atcTimeseries In lOperation.MonthlyTimeSeriesOutflowData
                        Dim TotalMonthlyTimeSeries As New atcTimeseriesGroup



                        Dim lTsYearly As atcTimeseries = Aggregate(lTS, atcTimeUnit.TUYear, 1, atcTran.TranSumDiv)
                        Dim lSumAnnual As Double = lTsYearly.Attributes.GetDefinedValue("SumAnnual").Value
                        Dim lTSAttributes As String = lTsYearly.Attributes.GetDefinedValue("Constituent").Value
                        If lTSNumber > 0 Then RowNumber -= (lTsYearly.numValues + 1)

                        For i As Integer = 1 To lTsYearly.numValues + 1

                            row = QUALConstTable.NewRow
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
                                row("OpType") = lOperation.LandType
                                row("OpNum") = lOperation.OperationNumber
                                row("OpDesc") = lOperation.OperationName
                                row("ConstName") = lOperation.LandConstituentNameInUCI
                                row("ConstNameEXP") = lOperation.LandConstituentNameForHSPEXP
                                row("Year") = Year
                                row("Unit") = lOperation.Units
                                row("WASHQS") = lValue
                                QUALConstTable.Rows.Add(row)
                            ElseIf lTSAttributes.StartsWith("SCRQS") Then
                                QUALConstTable.Rows(RowNumber - 1)("SCRQS") = lValue
                            ElseIf lTSAttributes.StartsWith("SOQO") Then
                                QUALConstTable.Rows(RowNumber - 1)("SOQO") = lValue
                            ElseIf lTSAttributes.StartsWith("IOQUAL") Then
                                QUALConstTable.Rows(RowNumber - 1)("IOQUAL") = lValue
                            ElseIf lTSAttributes.StartsWith("AOQUAL") Then
                                QUALConstTable.Rows(RowNumber - 1)("AOQUAL") = lValue
                            ElseIf lTSAttributes.Contains("total") Then
                                QUALConstTable.Rows(RowNumber - 1)("TotalOutflow") = lValue
                            End If

                        Next i

                        lTSNumber += 1
                    Next lTS

                Next lOperation

                For Each TableRow As DataRow In QUALConstTable.Rows
                    Dim UniqueComboVariable As String = ""
                    For Each TableColumn As DataColumn In QUALConstTable.Columns
                        'UniqueComboVariable = TableRow("OpType") & "_" & TableRow("OpNum") & 


                    Next


                Next

                'Dim QueryExpression As String = "Select OpType, OpNum, OPDesc, null as ConstName, 'TN' as ConstNameExp, Year, Unit, SUM(WASHQS), SUM(SCRQS), SUM(SOQO), SUM(IOQUAL), SUM(AOQUAL), SUM(TotalOutflow),
                '        from QUALConstTable group by OPType, OPNum, OPDesc, null as ConstName, 'TN' as ConstNameInExp, Year, Unit UNION ALL Select * from QUALConstTable"
                'Dim distinctDT As DataRow() = QUALConstTable.Select(QueryExpression)

                Dim TextToWrite As String = ""
                For Each TableColumn As DataColumn In QUALConstTable.Columns
                    TextToWrite &= TableColumn.Caption & vbTab
                Next
                lReport.AppendLine(TextToWrite)
                For Each TableRow As DataRow In QUALConstTable.Rows
                    TextToWrite = ""
                    For Each TableColumn As DataColumn In QUALConstTable.Columns
                        TextToWrite &= TableRow(TableColumn) & vbTab
                    Next TableColumn
                    lReport.AppendLine(TextToWrite)
                Next TableRow
                lReport.AppendLine("Run Made " & aRunMade)

        End Select

        Return lReport
    End Function

End Module
