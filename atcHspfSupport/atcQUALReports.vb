Imports atcUtility
Imports atcData
Imports MapWinUtility
Imports atcUCI
Imports System.Data.DataTable
Public Module atcQUALReports
    Private pPERLND As atcCollection
    Private pIMPLND As atcCollection
    Public QUALDataCollecion As List(Of QUALData)
    Public Function QUALReports(ByVal aUCI As HspfUci,
                                ByRef aScenariosResults As atcDataSource,
                                ByRef QUALDetails As ConstituentProperties) As List(Of ConstOutflowDatafromLand)

        Dim QualityConstituent As Boolean = False
        Dim lOutflowDataTypes As String() = ConstituentList(QUALDetails.ReportType, QualityConstituent)

        Dim ConstituentIndividualData As ConstOutflowDatafromLand
        Dim QUALNameInUCI As String = QUALDetails.ConstituentNameInUCI

        Dim lSeasonalAttributes As New atcDataAttributes
        lSeasonalAttributes.SetValue("Sum", 0)

        Dim lYearlyAttributes As New atcDataAttributes
        Dim lSeasons As atcSeasonBase
        Dim lOutputQUALData As New List(Of ConstOutflowDatafromLand)
        If aUCI.GlobalBlock.SDate(1) = 10 Then 'month Oct
            lSeasons = New atcSeasonsWaterYear
        Else
            lSeasons = New atcSeasonsCalendarYear
        End If

        pPERLND = New atcCollection
        pIMPLND = New atcCollection

        Dim lMultipleIndex As Integer = 0
        If QUALDetails.ConstNameForEXPPlus.ToLower.Contains("ref") Then
            lMultipleIndex = 1
        ElseIf QUALDetails.ConstNameForEXPPlus.Contains("lab") Then
            lMultipleIndex = 2
        End If

        For Each loperation As HspfOperation In aUCI.OpnSeqBlock.Opns
            If loperation.Name = "RCHRES" Then Continue For
            'Collecting the names of the land uses
            If loperation.Name = "PERLND" AndAlso Not pPERLND.Keys.Contains(loperation.Description) Then
                pPERLND.Add(loperation.Description)
            ElseIf loperation.Name = "IMPLND" AndAlso Not pIMPLND.Keys.Contains(loperation.Description) Then
                pIMPLND.Add(loperation.Description)
            Else Continue For
            End If
            Dim lTempOutflowData As New Dictionary(Of String, atcCollection)
            ConstituentIndividualData = New ConstOutflowDatafromLand
            ConstituentIndividualData.LandType = loperation.Name
            ConstituentIndividualData.OperationNumber = loperation.Id
            ConstituentIndividualData.OperationName = loperation.Description
            ConstituentIndividualData.LandConstituentNameInUCI = QUALDetails.ConstituentNameInUCI
            ConstituentIndividualData.LandConstituentNameForHSPEXP = QUALDetails.ConstNameForEXPPlus
            ConstituentIndividualData.Units = QUALDetails.ConstituentUnit
            Dim LocationName As String = loperation.Name.Substring(0, 1) & ":" & loperation.Id
            For Each OutflowData As String In lOutflowDataTypes
                If OutflowData.ToLower.Contains("total") Then
                    Dim TestCollection As New atcCollection
                    For Each key As String In lTempOutflowData.Keys
                        For Each YearKey As String In lTempOutflowData(key).Keys
                            TestCollection.Increment(YearKey, lTempOutflowData(key).ItemByKey(YearKey))
                        Next
                    Next
                    lTempOutflowData.Add(OutflowData, TestCollection)
                End If

                Dim lMassLinkFactor As Double = 1.0
                Dim lTS As atcTimeseries
                Dim BinaryFileConstituentName As String = ""
                If QualityConstituent Then
                    BinaryFileConstituentName = OutflowData & "-" & QUALNameInUCI
                Else
                    BinaryFileConstituentName = OutflowData
                End If

                lTS = aScenariosResults.DataSets.FindData("Constituent", BinaryFileConstituentName).FindData("Location", LocationName)(0)


                'Getting the time series of constituent and the location from the binary file data.


                If Not lTS Is Nothing Then
                    For Each lConnection As HspfConnection In loperation.Targets
                        If lConnection.Target.VolName = "RCHRES" Then
                            Dim aReach As HspfOperation = aUCI.OpnBlks("RCHRES").OperFromID(lConnection.Target.VolId)
                            Dim aConversionFactor As Double = 0.0
                            aConversionFactor = ConversionFactorfromOxygen(aUCI, QUALDetails.ReportType, aReach)
                            Dim lMassLinkID As Integer = lConnection.MassLink
                            If Not lMassLinkID = 0 Then

                                lMassLinkFactor = FindMassLinkFactor(aUCI, lMassLinkID, BinaryFileConstituentName,
                                                                     QUALDetails.ReportType, aConversionFactor, lMultipleIndex)

                                Exit For
                            End If

                        End If
                    Next


                    lSeasons.SetSeasonalAttributes(lTS, lSeasonalAttributes, lYearlyAttributes)
                    Dim lYearlyData As New atcCollection
                    lYearlyData.Increment("SumAnnual", lTS.Attributes.GetDefinedValue("SumAnnual").Value * lMassLinkFactor)
                    For i As Integer = 0 To lYearlyAttributes.Count - 1
                        lYearlyData.Increment(lYearlyAttributes.Item(i).Arguments(1).Value.ToString, lYearlyAttributes.Item(i).Value * lMassLinkFactor)
                    Next
                    lTempOutflowData.Add(OutflowData, lYearlyData) 'So for WASHQS, SCRQS etc., this list has annual sum and yearwise data.
                End If


            Next
            ConstituentIndividualData.OutflowData = lTempOutflowData

            lOutputQUALData.Add(ConstituentIndividualData)
        Next

        Return lOutputQUALData

    End Function

    Public Function PrintQUALReports(ByVal OutputQUALData As List(Of List(Of ConstOutflowDatafromLand)),
                                     ByVal aScenario As String,
                                     ByVal aRunMade As String,
                                     ByVal aBalanceType As String) As atcReport.IReport
        Dim lReport As New atcReport.ReportText
        lReport.AppendLine("   Run Made " & aRunMade)
        Dim QualityConstituent As Boolean = False
        Dim lOutflowDataTypes As String() = ConstituentList(aBalanceType, QualityConstituent)

        Dim lOutputTable As New atcTableDelimited
        Select Case aBalanceType
            Case "Water", "DO", "Heat"
                With lOutputTable
                    .NumFields = 10
                    .Delimiter = vbTab

                    Dim lField As Integer = 0
                    lField += 1 : .FieldLength(lField) = 6 : .FieldType(lField) = "C" : .FieldName(lField) = "Operation Type"
                    lField += 1 : .FieldLength(lField) = 3 : .FieldType(lField) = "N" : .FieldName(lField) = "Operation Number"
                    lField += 1 : .FieldLength(lField) = 20 : .FieldType(lField) = "C" : .FieldName(lField) = "Operation Description"
                    lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "C" : .FieldName(lField) = "Constituent Name"
                    lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "C" : .FieldName(lField) = "Year"
                    lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "C" : .FieldName(lField) = "Unit"
                    lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "N" : .FieldName(lField) = "Surface Outflow"
                    lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "N" : .FieldName(lField) = "Interflow Outflow"
                    lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "N" : .FieldName(lField) = "Active Groundwater Outflow"
                    lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "N" : .FieldName(lField) = "Total Outflow"


                    For Each ConstituentList As List(Of ConstOutflowDatafromLand) In OutputQUALData
                        For i As Integer = 0 To ConstituentList.Count - 1
                            Dim lTest As New atcCollection
                            For Each lOutflowDataType As String In lOutflowDataTypes
                                If ConstituentList(i).OutflowData.TryGetValue(lOutflowDataType, lTest) Then

                                    Exit For
                                End If
                            Next lOutflowDataType
                            'If lTest Is Nothing Then Stop

                            For Each YearName As String In lTest.Keys
                                .CurrentRecord += 1
                                lField = 0
                                lField += 1 : .Value(lField) = ConstituentList(i).LandType
                                lField += 1 : .Value(lField) = ConstituentList(i).OperationNumber
                                lField += 1 : .Value(lField) = ConstituentList(i).OperationName
                                lField += 1 : .Value(lField) = ConstituentList(i).LandConstituentNameInUCI
                                lField += 1 : .Value(lField) = YearName
                                lField += 1 : .Value(lField) = ConstituentList(i).Units

                                For Each lOutflowDataType As String In lOutflowDataTypes
                                    lTest = New atcCollection
                                    If ConstituentList(i).OutflowData.TryGetValue(lOutflowDataType, lTest) Then

                                        lField += 1 : .Value(lField) = HspfTable.NumFmtRE(lTest.ItemByKey(YearName), 10)
                                    Else
                                        lField += 1 : .Value(lField) = "NA"
                                    End If
                                Next lOutflowDataType
                            Next YearName
                        Next i

                    Next ConstituentList
                    lReport.Append(.ToString)
                End With

            Case "Sediment"
                With lOutputTable
                    .NumFields = 9
                    .Delimiter = vbTab

                    Dim lField As Integer = 0
                    lField += 1 : .FieldLength(lField) = 6 : .FieldType(lField) = "C" : .FieldName(lField) = "Operation Type"
                    lField += 1 : .FieldLength(lField) = 3 : .FieldType(lField) = "N" : .FieldName(lField) = "Operation Number"
                    lField += 1 : .FieldLength(lField) = 20 : .FieldType(lField) = "C" : .FieldName(lField) = "Operation Description"
                    lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "C" : .FieldName(lField) = "Constituent Name"
                    lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "C" : .FieldName(lField) = "Year"
                    lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "C" : .FieldName(lField) = "Unit"
                    lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "N" : .FieldName(lField) = "Washoff of Detached Sediment"
                    lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "N" : .FieldName(lField) = "Scour of Matrix Soil"
                    lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "N" : .FieldName(lField) = "Total Outflow"

                    For Each ConstituentList As List(Of ConstOutflowDatafromLand) In OutputQUALData
                        For i As Integer = 0 To ConstituentList.Count - 1


                            Dim lTest As New atcCollection
                            For Each lOutflowDataType As String In lOutflowDataTypes
                                If ConstituentList(i).OutflowData.TryGetValue(lOutflowDataType, lTest) Then

                                    Exit For
                                End If
                            Next lOutflowDataType
                            If lTest IsNot Nothing Then
                                For Each YearName As String In lTest.Keys
                                    .CurrentRecord += 1
                                    lField = 0
                                    lField += 1 : .Value(lField) = ConstituentList(i).LandType
                                    lField += 1 : .Value(lField) = ConstituentList(i).OperationNumber
                                    lField += 1 : .Value(lField) = ConstituentList(i).OperationName
                                    lField += 1 : .Value(lField) = ConstituentList(i).LandConstituentNameInUCI
                                    lField += 1 : .Value(lField) = YearName
                                    lField += 1 : .Value(lField) = ConstituentList(i).Units

                                    For Each lOutflowDataType As String In lOutflowDataTypes
                                        lTest = New atcCollection
                                        If ConstituentList(i).OutflowData.TryGetValue(lOutflowDataType, lTest) Then
                                            If (ConstituentList(i).LandType = "IMPLND" AndAlso lOutflowDataType = "SOSLD") Then lField -= 2
                                            lField += 1 : .Value(lField) = HspfTable.NumFmtRE(lTest.ItemByKey(YearName), 10)
                                        Else
                                            If Not (ConstituentList(i).LandType = "PERLND" AndAlso lOutflowDataType = "SOSLD") Then
                                                lField += 1 : .Value(lField) = "NA"
                                            End If
                                        End If
                                    Next lOutflowDataType
                                Next YearName

                            End If

                        Next i

                    Next ConstituentList
                    lReport.Append(.ToString)
                End With

            Case Else
                With lOutputTable
                    .NumFields = 12
                    .Delimiter = vbTab

                    Dim lField As Integer = 0
                    lField += 1 : .FieldLength(lField) = 6 : .FieldType(lField) = "C" : .FieldName(lField) = "Operation Type"
                    lField += 1 : .FieldLength(lField) = 3 : .FieldType(lField) = "N" : .FieldName(lField) = "Operation Number"
                    lField += 1 : .FieldLength(lField) = 20 : .FieldType(lField) = "C" : .FieldName(lField) = "Operation Description"
                    lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "C" : .FieldName(lField) = "QUALID in UCI"
                    lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "C" : .FieldName(lField) = "Constituent Name"
                    lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "C" : .FieldName(lField) = "Year"

                    lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "N" : .FieldName(lField) = "Removal of QUALSD by association with detached sediment Runoff "
                    lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "N" : .FieldName(lField) = "Removal of QUALSD with scour of matrix soil"
                    lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "N" : .FieldName(lField) = "Washoff of QUALOF from surface"
                    'lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "N" : .FieldName(lField) = "Total Outflow of QUAL from surface"
                    lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "N" : .FieldName(lField) = "Outflow of QUAL in interflow"
                    lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "N" : .FieldName(lField) = "Outflow of QUAL in active groundwater"
                    'lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "N" : .FieldName(lField) = "Total flux of QUAL from land area"
                    lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "N" : .FieldName(lField) = "Total Outflow"

                    For Each ConstituentList As List(Of ConstOutflowDatafromLand) In OutputQUALData
                        For i As Integer = 0 To ConstituentList.Count - 1

                            Dim lTest As New atcCollection
                            For Each lOutflowDataType As String In lOutflowDataTypes
                                If ConstituentList(i).OutflowData.TryGetValue(lOutflowDataType, lTest) Then

                                    Exit For
                                End If
                            Next lOutflowDataType
                            'If lTest Is Nothing Then Stop

                            For Each YearName As String In lTest.Keys
                                .CurrentRecord += 1
                                lField = 0
                                lField += 1 : .Value(lField) = ConstituentList(i).LandType
                                lField += 1 : .Value(lField) = ConstituentList(i).OperationNumber
                                lField += 1 : .Value(lField) = ConstituentList(i).OperationName
                                lField += 1 : .Value(lField) = ConstituentList(i).LandConstituentNameInUCI
                                lField += 1 : .Value(lField) = ConstituentList(i).LandConstituentNameForHSPEXP
                                lField += 1 : .Value(lField) = YearName


                                For Each lOutflowDataType As String In lOutflowDataTypes
                                    lTest = New atcCollection
                                    If ConstituentList(i).OutflowData.TryGetValue(lOutflowDataType, lTest) Then
                                        lField += 1 : .Value(lField) = HspfTable.NumFmtRE(lTest.ItemByKey(YearName), 10)

                                    Else
                                        lField += 1 : .Value(lField) = "NA"
                                    End If
                                Next lOutflowDataType
                            Next YearName
                        Next i

                    Next ConstituentList
                    lReport.Append(.ToString)
                End With
        End Select





        Return lReport
    End Function

End Module
