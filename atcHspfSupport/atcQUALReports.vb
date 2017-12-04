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
                                ByRef QUALDetails As PQUALProperties) As List(Of QUALData)

        Dim lOutflowDataType As String() = {"WASHQS", "SCRQS", "SOQO", "IOQUAL", "AOQUAL"}
        Dim QUALIndividualData As QUALData
        Dim QUALNameInUCI As String = QUALDetails.PQUALNameInUCI
        Dim lSeasonalAttributes As New atcDataAttributes
        lSeasonalAttributes.SetValue("Sum", 0)

        Dim lYearlyAttributes As New atcDataAttributes
        Dim lSeasons As atcSeasonBase
        Dim lOutputQUALData As New List(Of QUALData)
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
            QUALIndividualData = New QUALData
            QUALIndividualData.LandType = loperation.Name
            QUALIndividualData.OperationNumber = loperation.Id
            QUALIndividualData.OperationName = loperation.Description
            QUALIndividualData.QUALNameInUCI = QUALDetails.PQUALNameInUCI
            Dim LocationName As String = loperation.Name.Substring(0, 1) & ":" & loperation.Id
            For Each OutflowData As String In lOutflowDataType
                Dim lMassLinkFactor As Double = 1.0


                Dim lTS As atcTimeseries = aScenariosResults.DataSets.FindData("Constituent", OutflowData & "-" & QUALNameInUCI).FindData("Location", LocationName)(0)

                If Not lTS Is Nothing Then
                    For Each lConnection As HspfConnection In loperation.Targets
                        If lConnection.Target.VolName = "RCHRES" Then
                            Dim aReach As HspfOperation = aUCI.OpnBlks("RCHRES").OperFromID(lConnection.Target.VolId)
                            Dim aConversionFactor As Double = 0.0
                            aConversionFactor = ConversionFactorfromOxygen(aUCI, QUALDetails.ReportType, aReach)
                            Dim lMassLinkID As Integer = lConnection.MassLink
                            If Not lMassLinkID = 0 Then

                                lMassLinkFactor = FindMassLinkFactor(aUCI, lMassLinkID, OutflowData & "-" & QUALNameInUCI,
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
            QUALIndividualData.OutflowData = lTempOutflowData

            lOutputQUALData.Add(QUALIndividualData)
        Next

        Return lOutputQUALData

    End Function

    Public Function PrintQUALReports(ByVal OutputQUALData As List(Of QUALData), ByVal aScenario As String, ByVal aRunMade As String) As atcReport.IReport
        Dim lReport As New atcReport.ReportText
        lReport.AppendLine("   Run Made " & aRunMade)

        Dim lOutputTable As New atcTableDelimited
        With lOutputTable
            .NumFields = 10
            .Delimiter = vbTab

            Dim lField As Integer = 0
            lField += 1 : .FieldLength(lField) = 6 : .FieldType(lField) = "C" : .FieldName(lField) = "Operation Type"
            lField += 1 : .FieldLength(lField) = 3 : .FieldType(lField) = "N" : .FieldName(lField) = "Operation Number"
            lField += 1 : .FieldLength(lField) = 20 : .FieldType(lField) = "C" : .FieldName(lField) = "Operation Description"
            lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "C" : .FieldName(lField) = "QUALID"
            lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "C" : .FieldName(lField) = "Year"

            lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "N" : .FieldName(lField) = "Removal of QUALSD by association with detached sediment Runoff "
            lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "N" : .FieldName(lField) = "Removal of QUALSD with scour of matrix soil"
            lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "N" : .FieldName(lField) = "Washoff of QUALOF from surface"
            'lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "N" : .FieldName(lField) = "Total Outflow of QUAL from surface"
            lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "N" : .FieldName(lField) = "Outflow of QUAL in interflow"
            lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "N" : .FieldName(lField) = "Outflow of QUAL in active groundwater"
            'lField += 1 : .FieldLength(lField) = 10 : .FieldType(lField) = "N" : .FieldName(lField) = "Total flux of QUAL from land area"

            For i As Integer = 0 To OutputQUALData.Count - 1


                Dim lOutflowDataTypes As String() = {"WASHQS", "SCRQS", "SOQO", "IOQUAL", "AOQUAL"}
                Dim lTest As New atcCollection
                For Each lOutflowDataType As String In lOutflowDataTypes
                    If OutputQUALData(i).OutflowData.TryGetValue(lOutflowDataType, lTest) Then

                        Exit For
                    End If
                Next
                'If lTest Is Nothing Then Stop

                For Each YearName As String In lTest.Keys
                    .CurrentRecord += 1
                    lField = 0
                    lField += 1 : .Value(lField) = OutputQUALData(i).LandType
                    lField += 1 : .Value(lField) = OutputQUALData(i).OperationNumber
                    lField += 1 : .Value(lField) = OutputQUALData(i).OperationName
                    lField += 1 : .Value(lField) = OutputQUALData(i).QUALNameInUCI
                    lField += 1 : .Value(lField) = YearName

                    For Each lOutflowDataType As String In lOutflowDataTypes
                        lTest = New atcCollection
                        If OutputQUALData(i).OutflowData.TryGetValue(lOutflowDataType, lTest) Then
                            lField += 1 : .Value(lField) = HspfTable.NumFmtRE(lTest.ItemByKey(YearName), 10)
                        Else
                            lField += 1 : .Value(lField) = "NA"
                        End If

                    Next

                Next

            Next
            lReport.Append(.ToString)

        End With

        Return lReport
    End Function

End Module
