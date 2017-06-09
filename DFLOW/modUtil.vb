Imports atcData
Imports atcUtility

Public Module modUtil

    ''' <summary>
    ''' Holds a set of inputs' names for batch run
    ''' </summary>
    ''' <remarks></remarks>
    Public Class InputNames
        'Friend fBioDefault As Boolean
        'Friend fBioType As Integer
        'Friend fBioPeriod As Integer
        'Friend fBioYears As Integer
        'Friend fBioCluster As Integer
        'Friend fBioExcursions As Integer
        'Friend Shared fBioFPArray(,) As Integer = New Integer(3, 3) {{1, 3, 120, 5}, {4, 3, 120, 5}, {30, 3, 120, 5}, {-1, -1, -1, -1}}
        'Friend fNonBioType As Integer
        'Friend fAveragingPeriod As Integer
        'Friend fReturnPeriod As Integer
        'Friend fExplicitFlow As Double
        'Friend fPercentile As Double

        'Friend fStartMonth As Integer = 4
        'Friend fStartDay As Integer = 1
        'Friend fEndMonth As Integer = 3
        'Friend fEndDay As Integer = 31
        'Friend fFirstYear As Integer = -1
        'Friend fLastYear As Integer = -1

        Public Enum EDFLOWCategory
            Biological
            NonBiological
        End Enum
        Public Shared DFLOWCategoryText As String = "DFLOWCategory"
        Public Shared BiologicalDFLOW As String = "BiologicalDFLOW"

        Public Enum EBioDFlowParamIndex
            P0FlowAveragingPeriodDays = 0
            P1AverageYearsBetweenExcursions
            P2ExcursionClusterPeriodDays
            P3AverageExcursionsPerCluster
        End Enum

        Public Shared TextTrue As String = "Yes"
        Public Shared TextFalse As String = "No"
        Public Enum EBioDFlowType
            Acute_maximum_concentration
            Chronic_continuous_concentration
            Ammonia
        End Enum
        Public Shared EBioDFLOWTypeAbbrev() As String = {"AcuteMC", "Chronic", "Ammonia"}

        Public Shared BioUseDefault As String = "Bio_Use_Default"
        Public Shared BioSelectedMethod As String = "Bio_Selected_Method"
        Public Shared BioAvgPeriod As String = "Bio_Flow_Averaging_Period_days"
        Public Shared BioReturnYears As String = "Bio_Return_Period_of_Excursions_years"
        Public Shared BioClusterDays As String = "Bio_Excursion_Clustering_Period_days"
        Public Shared BioNumExcrsnPerCluster As String = "Bio_Num_Excursions_Per_Cluster"

        Public Enum EDFlowType
            Hydrological
            Explicit_Flow_Value
            Flow_Percentile
        End Enum
        Public Shared EDFlowTypeAbbrev() As String = {"Hydrologic", "Explicit_V", "Percentile"}

        Public Shared NBioSelectedMethod As String = "NBio_Selected_Method"
        Public Shared NBioAveragingPeriod As String = "Flow_Averaging_Period_days"
        Public Shared NBioReturnPeriod As String = "Return_Period_of_Excursions_years"
        Public Shared NBioExplicitFlow As String = "Explicit_flow_value"
        Public Shared NBioFlowPercentile As String = "Flow_percentile"

        Public Shared SelectedMethodText As String = "Selected"

        Public Shared IncludeYears As String = "IncludeYears"
        Public Shared StartYear As String = "FirstYear" '"STARTYEAR"
        Public Shared EndYear As String = "LastYear" '"ENDYEAR"
        Public Shared StartMonth As String = "BoundaryMonth" '"StartMonth"
        Public Shared StartDay As String = "BoundaryDay" '"StartDay"
        Public Shared EndMonth As String = "EndMonth"
        Public Shared EndDay As String = "EndDay"
        Public Shared HighFlowSeasonStart As String = "HIGHFLOWSEASONSTART" '10/01
        Public Shared HighFlowSeasonEnd As String = "HIGHFLOWSEASONEND"     '09/30
        Public Shared LowFlowSeasonStart As String = "LOWFLOWSEASONSTART"   '04/01
        Public Shared LowFlowSeasonEnd As String = "LOWFLOWSEASONEND"       '03/31

        Public Shared StationsInfo As String = "StationsInfo"
        Public Shared Streamflow As String = "Streamflow"

        Public Shared Method As String = "METHOD"
        Public Shared Methods As String = "Methods"

        Public Shared OutputDir As String = "OUTPUTDIR"
        Public Shared OutputPrefix As String = "OutputPrefix"
        Public Shared DataDir As String = "DataDir"

        Public Shared Function BasicAttributes() As Generic.List(Of String)
            Dim lBasicAttributes As New Generic.List(Of String)
            With lBasicAttributes
                .Add("ID")
                .Add("Min")
                .Add("Max")
                .Add("Mean")
                .Add("Standard Deviation")
                .Add("Count")
                .Add("Count Missing")
            End With
            Return lBasicAttributes
        End Function

        Public Shared Function NDayAttributes() As Generic.List(Of String)
            Dim lNDayAttributes As New Generic.List(Of String)
            With lNDayAttributes
                .Add("STAID")
                .Add("STANAM")
                .Add("Constituent")
            End With
            Return lNDayAttributes
        End Function

        Public Shared Function TrendAttributes() As Generic.List(Of String)
            Dim lTrendAttributes As New Generic.List(Of String)
            With lTrendAttributes
                .Add("Original ID")
                .Add("KENTAU")
                .Add("KENPLV")
                .Add("KENSLP")
                .Add("From")
                .Add("To")
                .Add("Count")
                .Add("Not Used")
                .Add("Min")
                .Add("Max")
                .Add("Constituent")
                .Add("STAID")
            End With
            Return lTrendAttributes
        End Function

        Public Shared Function GetAbbrevParamSetName(ByVal aParamSet As atcCollection) As String
            If aParamSet Is Nothing Then Return ""
            Dim lAbbrevParamSetName As String = ""
            For Each lParamKey As String In aParamSet.Keys
                If lParamKey.ToLower().StartsWith("type_") Then
                    Dim lIndStr As String = lParamKey.Substring(lParamKey.LastIndexOf("_") + 1)
                    If lParamKey.Contains(EBioDFlowType.Acute_maximum_concentration.ToString()) Then
                        lAbbrevParamSetName = InputNames.EBioDFLOWTypeAbbrev(InputNames.EBioDFlowType.Acute_maximum_concentration)
                    ElseIf lParamKey.Contains(EBioDFlowType.Chronic_continuous_concentration.ToString()) Then
                        lAbbrevParamSetName = InputNames.EBioDFLOWTypeAbbrev(InputNames.EBioDFlowType.Chronic_continuous_concentration)
                    ElseIf lParamKey.Contains(EBioDFlowType.Ammonia.ToString()) Then
                        lAbbrevParamSetName = InputNames.EBioDFLOWTypeAbbrev(InputNames.EBioDFlowType.Ammonia)
                    ElseIf lParamKey.Contains(EDFlowType.Hydrological.ToString()) Then
                        lAbbrevParamSetName = InputNames.EDFlowTypeAbbrev(InputNames.EDFlowType.Hydrological)
                    ElseIf lParamKey.Contains(EDFlowType.Explicit_Flow_Value.ToString()) Then
                        lAbbrevParamSetName = InputNames.EDFlowTypeAbbrev(InputNames.EDFlowType.Explicit_Flow_Value)
                    ElseIf lParamKey.Contains(EDFlowType.Flow_Percentile.ToString()) Then
                        lAbbrevParamSetName = InputNames.EDFlowTypeAbbrev(InputNames.EDFlowType.Flow_Percentile)
                    End If
                    If Not String.IsNullOrEmpty(lIndStr) AndAlso IsNumeric(lIndStr) Then
                        lAbbrevParamSetName &= "_" & lIndStr
                    End If
                    Exit For
                End If
            Next
            Return lAbbrevParamSetName
        End Function

        Public Shared Sub BuildInputSet(ByRef aSpecialSet As atcDataAttributes, ByVal aCommonSet As atcDataAttributes)
            If aSpecialSet Is Nothing Then
                aSpecialSet = New atcDataAttributes()
            End If
            If aCommonSet Is Nothing Then
                With aSpecialSet
                    If .GetValue(StartDay) Is Nothing Then .SetValue(StartDay, 1)
                    If .GetValue(StartMonth) Is Nothing Then .SetValue(StartMonth, 4)
                    If .GetValue(StartYear) Is Nothing Then .SetValue(StartYear, 0)
                    If .GetValue(EndDay) Is Nothing Then .SetValue(EndDay, 31)
                    If .GetValue(EndMonth) Is Nothing Then .SetValue(EndMonth, 3)
                    If .GetValue(EndYear) Is Nothing Then .SetValue(EndYear, 0)

                    If .GetValue(BioUseDefault) Is Nothing Then
                        .SetValue(BioUseDefault, True)
                    End If
                    If .GetValue(EBioDFlowType.Acute_maximum_concentration.ToString()) Is Nothing Then
                        Dim lParams(3) As Integer
                        For I As Integer = 0 To 3
                            lParams(I) = DFLOWCalcs.fBioFPArray(EBioDFlowType.Acute_maximum_concentration, I)
                        Next
                        .SetValue(EBioDFlowType.Acute_maximum_concentration.ToString(), lParams)
                    End If
                    If .GetValue(EBioDFlowType.Chronic_continuous_concentration.ToString()) Is Nothing Then
                        Dim lParams(3) As Integer
                        For I As Integer = 0 To 3
                            lParams(I) = DFLOWCalcs.fBioFPArray(EBioDFlowType.Chronic_continuous_concentration, I)
                        Next
                        .SetValue(EBioDFlowType.Chronic_continuous_concentration.ToString(), lParams)
                    End If
                    If .GetValue(EBioDFlowType.Ammonia.ToString()) Is Nothing Then
                        Dim lParams(3) As Integer
                        For I As Integer = 0 To 3
                            lParams(I) = DFLOWCalcs.fBioFPArray(EBioDFlowType.Ammonia, I)
                        Next
                        .SetValue(EBioDFlowType.Ammonia.ToString(), lParams)
                    End If
                    If .GetValue(BioSelectedMethod) Is Nothing Then
                        .SetValue(BioSelectedMethod, EBioDFlowType.Acute_maximum_concentration)
                    End If

                    If .GetValue(EDFlowType.Hydrological.ToString()) Is Nothing Then
                        Dim lParams() As Integer = {7, 10}
                        .SetValue(EDFlowType.Hydrological.ToString(), lParams)
                    End If
                    If .GetValue(EDFlowType.Explicit_Flow_Value.ToString()) Is Nothing Then
                        .SetValue(EDFlowType.Explicit_Flow_Value.ToString(), 1.0)
                    End If
                    If .GetValue(EDFlowType.Flow_Percentile.ToString()) Is Nothing Then
                        .SetValue(EDFlowType.Flow_Percentile.ToString(), 0.1)
                    End If
                    If .GetValue(NBioSelectedMethod) Is Nothing Then
                        .SetValue(NBioSelectedMethod, EDFlowType.Hydrological)
                    End If
                End With
            Else
                With aSpecialSet
                    If .GetValue(OutputDir) Is Nothing Then .SetValue(OutputDir, aCommonSet.GetValue(OutputDir, ""))
                    If .GetValue(OutputPrefix) Is Nothing Then .SetValue(OutputPrefix, aCommonSet.GetValue(OutputPrefix, ""))
                    If .GetValue(DataDir) Is Nothing Then .SetValue(DataDir, aCommonSet.GetValue(DataDir, ""))

                    If .GetValue(StartDay) Is Nothing Then .SetValue(StartDay, aCommonSet.GetValue(StartDay, 1))
                    If .GetValue(EndDay) Is Nothing Then .SetValue(EndDay, aCommonSet.GetValue(EndDay, 31))
                    If .GetValue(StartMonth) Is Nothing Then .SetValue(StartMonth, aCommonSet.GetValue(StartMonth, 4))
                    If .GetValue(EndMonth) Is Nothing Then .SetValue(EndMonth, aCommonSet.GetValue(EndMonth, 3))
                    If .GetValue(StartYear) Is Nothing Then .SetValue(StartYear, aCommonSet.GetValue(StartYear, 0))
                    If .GetValue(EndYear) Is Nothing Then .SetValue(EndYear, aCommonSet.GetValue(EndYear, 0))

                    If .GetValue(BioUseDefault) Is Nothing Then
                        .SetValue(BioUseDefault, aCommonSet.GetValue(BioUseDefault, True))
                    End If
                    If .GetValue(EBioDFlowType.Acute_maximum_concentration.ToString()) Is Nothing Then
                        Dim lParams() As Integer = aCommonSet.GetValue(EBioDFlowType.Acute_maximum_concentration.ToString(), Nothing)
                        If lParams Is Nothing Then
                            ReDim lParams(3)
                            For I As Integer = 0 To 3
                                lParams(I) = DFLOWCalcs.fBioFPArray(EBioDFlowType.Acute_maximum_concentration, I)
                            Next
                        End If
                        Dim lParamsCopy(3) As Integer
                        For I As Integer = 0 To 3
                            lParamsCopy(I) = lParams(I)
                        Next
                        .SetValue(EBioDFlowType.Acute_maximum_concentration.ToString(), lParamsCopy)
                    End If
                    If .GetValue(EBioDFlowType.Chronic_continuous_concentration.ToString()) Is Nothing Then
                        Dim lParams() As Integer = aCommonSet.GetValue(EBioDFlowType.Chronic_continuous_concentration.ToString(), Nothing)
                        If lParams Is Nothing Then
                            ReDim lParams(3)
                            For I As Integer = 0 To 3
                                lParams(I) = DFLOWCalcs.fBioFPArray(EBioDFlowType.Chronic_continuous_concentration, I)
                            Next
                        End If
                        Dim lParamsCopy(3) As Integer
                        For I As Integer = 0 To 3
                            lParamsCopy(I) = lParams(I)
                        Next
                        .SetValue(EBioDFlowType.Chronic_continuous_concentration.ToString(), lParamsCopy)
                    End If
                    If .GetValue(EBioDFlowType.Ammonia.ToString()) Is Nothing Then
                        Dim lParams() As Integer = aCommonSet.GetValue(EBioDFlowType.Ammonia.ToString(), Nothing)
                        If lParams Is Nothing Then
                            ReDim lParams(3)
                            For I As Integer = 0 To 3
                                lParams(I) = DFLOWCalcs.fBioFPArray(EBioDFlowType.Ammonia, I)
                            Next
                        End If
                        Dim lParamsCopy(3) As Integer
                        For I As Integer = 0 To 3
                            lParamsCopy(I) = lParams(I)
                        Next
                        .SetValue(EBioDFlowType.Ammonia.ToString(), lParamsCopy)
                    End If
                    If .GetValue(BioSelectedMethod) Is Nothing Then
                        .SetValue(BioSelectedMethod, aCommonSet.GetValue(BioSelectedMethod, EBioDFlowType.Acute_maximum_concentration))
                    End If

                    If .GetValue(EDFlowType.Hydrological.ToString()) Is Nothing Then
                        Dim lParams() As Integer = aCommonSet.GetValue(EDFlowType.Hydrological.ToString(), Nothing)
                        If lParams Is Nothing Then
                            ReDim lParams(1) : lParams(0) = 7 : lParams(1) = 10
                        End If
                        Dim lParamsCopy(1) As Integer
                        For I As Integer = 0 To 1
                            lParamsCopy(I) = lParams(I)
                        Next
                        .SetValue(EDFlowType.Hydrological.ToString(), lParamsCopy)
                    End If
                    If .GetValue(EDFlowType.Explicit_Flow_Value.ToString()) Is Nothing Then
                        .SetValue(EDFlowType.Explicit_Flow_Value.ToString(), aCommonSet.GetValue(EDFlowType.Explicit_Flow_Value, 1.0))
                    End If
                    If .GetValue(EDFlowType.Flow_Percentile.ToString()) Is Nothing Then
                        .SetValue(EDFlowType.Flow_Percentile.ToString(), aCommonSet.GetValue(EDFlowType.Flow_Percentile, 0.1))
                    End If
                    If .GetValue(NBioSelectedMethod) Is Nothing Then
                        .SetValue(NBioSelectedMethod, aCommonSet.GetValue(NBioSelectedMethod, EDFlowType.Hydrological))
                    End If

                    If .GetValue(OutputDir) Is Nothing Then
                        .SetValue(OutputDir, aCommonSet.GetValue(OutputDir, ""))
                    Else
                        Dim lOutputDir As String = .GetValue(OutputDir)
                        If String.IsNullOrEmpty(lOutputDir) OrElse Not IO.Directory.Exists(lOutputDir) Then
                            .SetValue(OutputDir, aCommonSet.GetValue(OutputDir, ""))
                        End If
                    End If
                End With
            End If
        End Sub
        Public Shared Function BuildStationsInfo(ByVal aDataGroup As atcTimeseriesGroup) As ArrayList
            Dim lStationsInfo As New atcUtility.atcCollection()
            Dim lStationInfo As String = ""
            Dim loc As String
            Dim lDA As String
            Dim lFrom As String
            For Each lTser As atcTimeseries In aDataGroup
                With lTser.Attributes
                    loc = .GetValue("Location")
                    lDA = .GetValue("Drainage Area", "")
                    lFrom = .GetValue("History 1")
                    If Not String.IsNullOrEmpty(lFrom) Then
                        lFrom = lFrom.Substring("read from ".Length)
                    End If

                    lStationInfo = "Station" & vbTab & loc & "," & lDA & "," & lFrom
                    If Not lStationsInfo.Keys.Contains(loc) Then
                        lStationsInfo.Add(loc, lStationInfo)
                    End If
                End With
            Next
            Return lStationsInfo
        End Function

        Public Shared Function ParametersToText(ByVal aArgs As atcDataAttributes) As String
            If aArgs Is Nothing Then Return ""
            Dim loperation As String = aArgs.GetValue("Operation", "")
            Dim lgroupname As String = aArgs.GetValue("Group", "")
            Dim lSetGlobal As Boolean = (loperation.ToLower = "globalsetparm")

            Dim lText As New Text.StringBuilder()
            If loperation.ToLower = "groupsetparm" Then
                lText.AppendLine("DFLOW")
                Dim lStationInfo As ArrayList = aArgs.GetValue(InputNames.StationsInfo)
                If lStationInfo IsNot Nothing Then
                    For Each lstation As String In lStationInfo
                        lText.AppendLine(lstation)
                    Next
                End If
            ElseIf lSetGlobal Then
                lText.AppendLine("GLOBAL")
            End If

            'Dim lStartDate As Double = aArgs.GetValue(BFInputNames.StartDate, Date2J(2014, 8, 20, 0, 0, 0))
            'Dim lEndDate As Double = aArgs.GetValue(BFInputNames.EndDate, Date2J(2014, 8, 20, 24, 0, 0))
            'Dim lDates(5) As Integer
            'J2Date(lStartDate, lDates)
            'lText.AppendLine("STARTDATE" & vbTab & lDates(0) & "/" & lDates(1) & "/" & lDates(2))
            'J2Date(lEndDate, lDates)
            'timcnv(lDates)
            'lText.AppendLine("ENDDATE" & vbTab & lDates(0) & "/" & lDates(1) & "/" & lDates(2))

            If Not lSetGlobal Then
                If aArgs.ContainsAttribute(InputNames.IncludeYears) Then
                    Dim lDuration As String = aArgs.GetValue(InputNames.IncludeYears)
                    If Not String.IsNullOrEmpty(lDuration) Then lText.AppendLine(InputNames.IncludeYears & vbTab & lDuration)
                End If
                If aArgs.ContainsAttribute(InputNames.StartYear) Then
                    Dim lStartYear As String = aArgs.GetValue(InputNames.StartYear, 0)
                    If Not String.IsNullOrEmpty(lStartYear) Then lText.AppendLine(InputNames.StartYear & vbTab & lStartYear)
                End If
                If aArgs.ContainsAttribute(InputNames.EndYear) Then
                    Dim lEndYear As String = aArgs.GetValue(InputNames.EndYear, 0)
                    If Not String.IsNullOrEmpty(lEndYear) Then lText.AppendLine(InputNames.EndYear & vbTab & lEndYear)
                End If
            End If
            Dim lStartMonth As Integer = aArgs.GetValue(InputNames.StartMonth, 4)
            lText.AppendLine(InputNames.StartMonth & vbTab & lStartMonth)
            Dim lStartDay As Integer = aArgs.GetValue(InputNames.StartDay, 1)
            lText.AppendLine(InputNames.StartDay & vbTab & lStartDay)
            Dim lEndMonth As String = aArgs.GetValue(InputNames.EndMonth, 3)
            lText.AppendLine(InputNames.EndMonth & vbTab & lEndMonth)
            Dim lEndDay As String = aArgs.GetValue(InputNames.EndDay, 31)
            lText.AppendLine(InputNames.EndDay & vbTab & lEndDay)

            'list all analysis methods
            Dim lMethodText As String = Param_Method(aArgs, EBioDFlowType.Acute_maximum_concentration, True)
            If Not String.IsNullOrEmpty(lMethodText) Then lText.AppendLine(lMethodText)
            lMethodText = Param_Method(aArgs, EBioDFlowType.Chronic_continuous_concentration, True)
            If Not String.IsNullOrEmpty(lMethodText) Then lText.AppendLine(lMethodText)
            lMethodText = Param_Method(aArgs, EBioDFlowType.Ammonia, True)
            If Not String.IsNullOrEmpty(lMethodText) Then lText.AppendLine(lMethodText)

            lMethodText = Param_Method(aArgs, EDFlowType.Hydrological, False)
            If Not String.IsNullOrEmpty(lMethodText) Then lText.AppendLine(lMethodText)
            lMethodText = Param_Method(aArgs, EDFlowType.Explicit_Flow_Value, False)
            If Not String.IsNullOrEmpty(lMethodText) Then lText.AppendLine(lMethodText)
            lMethodText = Param_Method(aArgs, EDFlowType.Flow_Percentile, False)
            If Not String.IsNullOrEmpty(lMethodText) Then lText.AppendLine(lMethodText)

            If lSetGlobal Then
                Dim lDatadir As String = aArgs.GetValue(InputNames.DataDir, "")
                If IO.Directory.Exists(lDatadir) Then
                    lText.AppendLine(InputNames.DataDir & vbTab & lDatadir)
                End If
            End If

            Dim lOutputDir As String = aArgs.GetValue(InputNames.OutputDir, "")
            Dim lOutputPrefix As String = aArgs.GetValue(InputNames.OutputPrefix, "")
            if Not String.IsNullOrEmpty(lOutputDir) Then lText.AppendLine(InputNames.OutputDir & vbTab & lOutputDir)
            if Not String.IsNullOrEmpty(lOutputPrefix) Then lText.AppendLine(InputNames.OutputPrefix & vbTab & lOutputPrefix)

            If loperation.ToLower = "groupsetparm" Then
                lText.AppendLine("END DFLOW")
            ElseIf lSetGlobal Then
                lText.AppendLine("END GLOBAL")
            End If
            Return lText.ToString()
        End Function

        Private Shared Function Param_Method(ByVal aArgs As atcDataAttributes, ByVal aMethod As Integer, ByVal aBio As Boolean) As String
            Dim lMethodBlock As New Text.StringBuilder()
            lMethodBlock.AppendLine(Method)
            Dim lParams() As Integer = Nothing
            If aBio Then
                lMethodBlock.AppendLine(BiologicalDFLOW & vbTab & TextTrue)
                Dim lSelectedMethod As Integer = aArgs.GetValue(BioSelectedMethod, -1)
                Select Case aMethod
                    Case EBioDFlowType.Acute_maximum_concentration
                        lMethodBlock.Append("Type_" & EBioDFlowType.Acute_maximum_concentration.ToString())
                        lParams = aArgs.GetValue(EBioDFlowType.Acute_maximum_concentration.ToString(), Nothing)
                    Case EBioDFlowType.Chronic_continuous_concentration
                        lMethodBlock.Append("Type_" & EBioDFlowType.Chronic_continuous_concentration.ToString())
                        lParams = aArgs.GetValue(EBioDFlowType.Chronic_continuous_concentration.ToString(), Nothing)
                    Case EBioDFlowType.Ammonia
                        lMethodBlock.Append("Type_" & EBioDFlowType.Ammonia.ToString())
                        lParams = aArgs.GetValue(EBioDFlowType.Ammonia.ToString(), Nothing)
                End Select

                If lSelectedMethod >= 0 Then
                    Select Case lSelectedMethod
                        Case EBioDFlowType.Acute_maximum_concentration
                            lMethodBlock.AppendLine(vbTab & SelectedMethodText)
                        Case EBioDFlowType.Chronic_continuous_concentration
                            lMethodBlock.AppendLine(vbTab & SelectedMethodText)
                        Case EBioDFlowType.Ammonia
                            lMethodBlock.AppendLine(vbTab & SelectedMethodText)
                    End Select
                End If

                If lParams IsNot Nothing AndAlso lParams.Length = 4 Then
                    lMethodBlock.AppendLine(BioAvgPeriod & vbTab & lParams(0))
                    lMethodBlock.AppendLine(BioReturnYears & vbTab & lParams(1))
                    lMethodBlock.AppendLine(BioClusterDays & vbTab & lParams(2))
                    lMethodBlock.AppendLine(BioNumExcrsnPerCluster & vbTab & lParams(3))
                Else
                    Return "" 'no parameters specified for this bio dflow profile
                End If
            Else
                Dim lSetGlobal As Boolean = True
                Dim lOpn As String = aArgs.GetValue("Operation")
                If lOpn = "GroupSetParm" Then
                    lSetGlobal = False
                End If

                Dim lExFlowValue As Double
                Dim lFlowPct As Double
                lMethodBlock.AppendLine(BiologicalDFLOW & vbTab & TextFalse)
                Dim lSelectedMethod As Integer = aArgs.GetValue(NBioSelectedMethod, -1)
                Select Case aMethod
                    Case EDFlowType.Hydrological
                        If lSetGlobal OrElse aArgs.ContainsAttribute(EDFlowType.Hydrological.ToString()) Then
                            lMethodBlock.Append("Type_" & EDFlowType.Hydrological.ToString())
                            lParams = aArgs.GetValue(EDFlowType.Hydrological.ToString(), New Integer() {7, 10})
                        Else
                            Return ""
                        End If
                    Case EDFlowType.Explicit_Flow_Value
                        If lSetGlobal OrElse aArgs.ContainsAttribute(EDFlowType.Explicit_Flow_Value.ToString()) Then
                            lMethodBlock.Append("Type_" & EDFlowType.Explicit_Flow_Value.ToString())
                            lExFlowValue = aArgs.GetValue(EDFlowType.Explicit_Flow_Value.ToString(), 1.0)
                        Else
                            Return ""
                        End If
                    Case EDFlowType.Flow_Percentile
                        If lSetGlobal OrElse aArgs.ContainsAttribute(EDFlowType.Flow_Percentile.ToString()) Then
                            lMethodBlock.Append("Type_" & EDFlowType.Flow_Percentile.ToString())
                            lFlowPct = aArgs.GetValue(EDFlowType.Flow_Percentile.ToString(), 0.1)
                        Else
                            Return ""
                        End If
                End Select
                If lSelectedMethod >= 0 Then
                    Select Case lSelectedMethod
                        Case EDFlowType.Hydrological
                            lMethodBlock.AppendLine(vbTab & SelectedMethodText)
                        Case EDFlowType.Explicit_Flow_Value
                            lMethodBlock.AppendLine(vbTab & SelectedMethodText)
                        Case EDFlowType.Flow_Percentile
                            lMethodBlock.AppendLine(vbTab & SelectedMethodText)
                    End Select
                End If
                Select Case aMethod
                    Case EDFlowType.Hydrological
                        If lParams IsNot Nothing AndAlso lParams.Length = 2 Then
                            lMethodBlock.AppendLine(NBioAveragingPeriod & vbTab & lParams(0))
                            lMethodBlock.AppendLine(NBioReturnPeriod & vbTab & lParams(1))
                        End If
                    Case EDFlowType.Explicit_Flow_Value
                        lMethodBlock.AppendLine(NBioExplicitFlow & vbTab & lExFlowValue)
                    Case EDFlowType.Flow_Percentile
                        lMethodBlock.AppendLine(NBioFlowPercentile & vbTab & lFlowPct)
                End Select
            End If
            lMethodBlock.AppendLine("END " & Method)
            Return lMethodBlock.ToString()
        End Function
        Public Shared Function CalculateNDayValues(ByVal aDataGroup As atcTimeseriesGroup, _
                                           ByVal aGroupArgs As atcDataAttributes, _
                                           ByVal aNDayDbl() As Double, _
                                           ByVal aReturnPeriodDbl() As Double, _
                                           ByVal aHighFlag As Boolean) As Boolean

            'ByVal aOperationName As String, ByVal aReturnPeriods() As Double
            'for NDay calculation, no need for high/low setting
            Dim lCalculator As New atcTimeseriesNdayHighLow.atcTimeseriesNdayHighLow
            Dim lDataGroup As atcTimeseriesGroup = aDataGroup
            With aGroupArgs
                'lDataGroup = .GetValue("Timeseries", Nothing)
                If lDataGroup Is Nothing Then Exit Function
                For Each lTs As atcTimeseries In lDataGroup
                    lTs.Attributes.SetValueIfMissing("CalcEMA", True)
                Next
            End With

            Dim lCalcArgs As New atcDataAttributes
            With lCalcArgs
                .SetValue("Timeseries", lDataGroup)
                '.SetValue(NDay, aNDayDbl)
                .SetValue(NBioReturnPeriod, aReturnPeriodDbl)
            End With

            Dim lOperationName As String = "n-day high value"
            If Not aHighFlag Then
                lOperationName = "n-day low value"
            End If
            SetInputsForAnalysis(aGroupArgs, lCalcArgs, aHighFlag)
            Dim lNDayValuesCalculated As Boolean
            Try
                lNDayValuesCalculated = lCalculator.Open(lOperationName, lCalcArgs)
            Catch ex As Exception
                lNDayValuesCalculated = False
            Finally
                lCalculator.DataSets.Clear()
                lCalculator = Nothing
            End Try
            Return lNDayValuesCalculated
        End Function

        Public Shared Function CalculateNDayTser(ByVal aDataGroup As atcTimeseriesGroup, _
                                             ByVal aGroupArgs As atcDataAttributes, _
                                             ByVal aNDayDbl() As Double, _
                                             ByVal aHighFlag As Boolean) As String
            Dim lTserListing As String = ""
            If (aNDayDbl IsNot Nothing AndAlso aNDayDbl.Length > 0) AndAlso aGroupArgs IsNot Nothing Then
                Dim lCalcArgs As New atcDataAttributes()

                Dim lHighLowText As String = "high"
                If Not aHighFlag Then
                    lHighLowText = "low"
                End If

                SetInputsForAnalysis(aGroupArgs, lCalcArgs, aHighFlag)

                Dim lFirstYear As Integer = lCalcArgs.GetValue(InputNames.StartYear)
                Dim lastYear As Integer = lCalcArgs.GetValue(InputNames.EndYear)
                Dim lStartMonth As Integer = lCalcArgs.GetValue(InputNames.StartMonth)
                Dim lEndMonth As Integer = lCalcArgs.GetValue(InputNames.EndMonth)
                Dim lStartDay As Integer = lCalcArgs.GetValue(InputNames.StartDay)
                Dim lEndDay As Integer = lCalcArgs.GetValue(InputNames.EndDay)

                'Dim lRankedAnnual As atcTimeseriesGroup = _
                '   clsSWSTATPlugin.ComputeRankedAnnualTimeseries(aTimeseriesGroup:=aDataGroup, _
                '                                                 aNDay:=aNDayDbl, aHighFlag:=aHighFlag, _
                '                                                 aFirstYear:=lFirstYear, aLastYear:=lastYear, _
                '                                                 aBoundaryMonth:=lStartMonth, aBoundaryDay:=lStartDay, _
                '                                                 aEndMonth:=lEndMonth, aEndDay:=lEndDay)

                'If lRankedAnnual.Count > 0 Then
                '    For Each lTS As atcTimeseries In lRankedAnnual
                '        lTS.Attributes.SetValue("Units", "Common")
                '    Next

                '    Dim lList As New atcList.atcListForm
                '    With lList
                '        With .DateFormat
                '            .IncludeDays = False
                '            .IncludeHours = False
                '            .IncludeMinutes = False
                '            .IncludeMonths = False
                '        End With
                '        .Text = "N-Day " & lHighLowText & " Annual Time Series and Ranking"
                '        .Initialize(lRankedAnnual.Clone, NDayAttributes(), True, , False) 'show value, but not show form
                '        .DisplayValueAttributes = True
                '        lTserListing = .ToString()
                '        '.Icon = Me.Icon
                '        .Close()
                '    End With
                'End If
            Else
                'Logger.Msg("Inputs are incomplete.")
            End If
            Return lTserListing
        End Function
        Public Shared Function SetInputsForAnalysis(ByVal aSetInputArgs As atcDataAttributes, _
                                         ByRef aCalcArgs As atcDataAttributes, _
                                         Optional ByVal aHighFlow As Boolean = True) As Boolean

            Dim lStartDate As String = ""
            Dim lEndDate As String = ""
            If aCalcArgs Is Nothing Then
                Return False
            End If
            Try
                With aCalcArgs
                    '.SetValue("Timeseries", lDataGroup)
                    '.SetValue("NDay", aNDayDbl)
                    '.SetValue("Return Period", aReturnPeriodDbl)
                    Dim logCheck As Boolean = True
                    Dim logObj As Object = Nothing 'aSetInputArgs.GetValue(Logarithmic)
                    Dim lType As Type = logObj.GetType()
                    If lType.Name.ToLower() = "string" Then
                        If String.Compare(logObj.ToString, "no", True) = 0 Then
                            logCheck = False
                        End If
                    ElseIf lType.Name.ToLower() = "Boolean" Then
                        If Not logObj Then
                            logCheck = False
                        End If
                    End If
                    .SetValue("LogFlg", logCheck)
                    Dim lStartYear As String = aSetInputArgs.GetValue(StartYear)
                    Dim lEndYear As String = aSetInputArgs.GetValue(EndYear)
                    If String.IsNullOrEmpty(lStartYear) OrElse String.IsNullOrEmpty(lEndYear) Then
                        Dim lDuration As String = aSetInputArgs.GetValue(IncludeYears, "")
                        If Not String.IsNullOrEmpty(lDuration) AndAlso lDuration.ToLower.StartsWith("all") Then
                            Dim lArr() As String = lDuration.Split(" ")
                            lStartDate = lArr(1)
                            lEndDate = lArr(3)
                            Dim lSDate As DateTime
                            Dim lEDate As DateTime
                            If DateTime.TryParse(lStartDate, lSDate) Then
                                .SetValue(StartYear, lSDate.Year)
                            End If
                            If DateTime.TryParse(lEndDate, lEDate) Then
                                .SetValue(EndYear, lEDate.Year)
                            End If
                        End If
                    Else
                        Dim lStartYearD As Integer
                        Dim lEndYearD As Integer
                        If Integer.TryParse(lStartYear, lStartYearD) Then
                            .SetValue(StartYear, lStartYearD) ' pFirstYear)
                        End If
                        If Integer.TryParse(lEndYear, lEndYearD) Then
                            .SetValue(EndYear, lEndYearD) ' pLastYear)
                        End If
                    End If
                End With

                Dim lOperationName As String = ""
                Dim lsMonth As Integer
                Dim lsDay As Integer
                Dim leMonth As Integer
                Dim leDay As Integer
                If aHighFlow Then
                    lsMonth = 10
                    lsDay = 1
                    leMonth = 9
                    leDay = 30
                    lStartDate = aSetInputArgs.GetValue(HighFlowSeasonStart, "")
                    lEndDate = aSetInputArgs.GetValue(HighFlowSeasonEnd, "")
                Else
                    lsMonth = 4
                    lsDay = 1
                    leMonth = 3
                    leDay = 31
                    lStartDate = aSetInputArgs.GetValue(LowFlowSeasonStart, "")
                    lEndDate = aSetInputArgs.GetValue(LowFlowSeasonEnd, "")
                End If

                With aCalcArgs
                    If aSetInputArgs.GetValue(StartMonth) IsNot Nothing AndAlso aSetInputArgs.GetValue(StartDay) IsNot Nothing Then
                        .SetValue(StartMonth, aSetInputArgs.GetValue(StartMonth)) 'pYearStartMonth)
                        .SetValue(StartDay, aSetInputArgs.GetValue(StartDay)) 'pYearStartDay)
                    Else
                        If Not String.IsNullOrEmpty(lStartDate) AndAlso lStartDate.IndexOf("/") >= 0 Then
                            Integer.TryParse(lStartDate.Substring(0, lStartDate.IndexOf("/")), lsMonth)
                            Integer.TryParse(lStartDate.Substring(lStartDate.IndexOf("/") + 1), lsDay)
                        End If
                        .SetValue(StartMonth, lsMonth)
                        .SetValue(StartDay, lsDay)
                    End If

                    If aSetInputArgs.GetValue(EndMonth) IsNot Nothing AndAlso aSetInputArgs.GetValue(EndDay) IsNot Nothing Then
                        .SetValue(EndMonth, aSetInputArgs.GetValue(EndMonth)) 'pYearStartMonth)
                        .SetValue(EndDay, aSetInputArgs.GetValue(EndDay)) 'pYearStartDay)
                    Else
                        If Not String.IsNullOrEmpty(lEndDate) AndAlso lEndDate.IndexOf("/") >= 0 Then
                            Integer.TryParse(lEndDate.Substring(0, lEndDate.IndexOf("/")), leMonth)
                            Integer.TryParse(lEndDate.Substring(lEndDate.IndexOf("/") + 1), leDay)
                        End If
                        .SetValue(EndMonth, leMonth)
                        .SetValue(EndDay, leDay)
                    End If
                End With
                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Shared Sub DoFrequencyGraph(ByVal aDirectory As String, _
                                           ByVal aDataGroup As atcData.atcTimeseriesGroup, _
                                           ByVal aInputArgs As atcDataAttributes, _
                                           ByVal aNDaysDbl() As Double, _
                                           ByVal aReturnPeriods() As Double, _
                                           ByVal aHighFlag As Boolean)

            'Dim lHighLowText As String = "High"
            'If Not aHighFlag Then lHighLowText = "Low"
            'Dim lGraphFilePrefix As String = "FreqGraph_" & lHighLowText

            ''Calculate("n-day " & lFlowCondition & " value", clsSWSTATPlugin.ListDefaultArray("Return Period"))
            'CalculateNDayValues(aDataGroup, aInputArgs, aNDaysDbl, clsSWSTATPlugin.ListDefaultArray(InputNames.ReturnPeriod), aHighFlag)
            'Dim lGraphPlugin As New atcGraph.atcGraphPlugin
            'Dim lGraphForm As atcGraph.atcGraphForm

            ''Get station list
            'Dim lStnList As New Generic.List(Of String)
            'For Each lDataSet As atcTimeseries In aDataGroup
            '    Dim loc As String = lDataSet.Attributes.GetValue("STAID", "")
            '    If String.IsNullOrEmpty(loc) Then
            '        loc = lDataSet.Attributes.GetValue("Location", "")
            '        lDataSet.Attributes.SetValue("STAID", loc)
            '    End If
            '    If Not String.IsNullOrEmpty(loc) AndAlso Not lStnList.Contains(loc) Then
            '        lStnList.Add(loc)
            '    End If
            'Next
            ''Get Nday list
            'Dim lNDayList As New Generic.List(Of String)
            'For Each lDataSet As atcTimeseries In aDataGroup
            '    For Each lAtt As atcData.atcDefinedValue In lDataSet.Attributes
            '        If lAtt.Definition.Name.ToLower.Contains("daytimeseries") Then
            '            If Not lNDayList.Contains(lAtt.Definition.Name) Then
            '                lNDayList.Add(lAtt.Definition.Name)
            '            End If
            '        End If
            '    Next
            'Next

            'Dim lMNDayPlot As Boolean = True
            'Dim lMNDayPlotSetting As String = aInputArgs.GetValue(InputNames.MultiNDayPlot, "")
            'If lMNDayPlotSetting.ToLower() = "no" Then
            '    lMNDayPlot = False
            'End If
            'Dim lMStationPlot As Boolean = True
            'Dim lMStationPlotSetting As String = aInputArgs.GetValue(InputNames.MultiStationPlot, "")
            'If lMStationPlotSetting.ToLower() = "no" Then
            '    lMStationPlot = False
            'End If

            'If lMNDayPlot AndAlso lMStationPlot Then 'MultipleNDayPlots() And MultipleStationPlots()
            '    For Each lStaId As String In lStnList
            '        For Each lNDayTimeseriesName As String In lNDayList
            '            Dim lDataGroup As New atcData.atcTimeseriesGroup
            '            For Each lDataset As atcTimeseries In aDataGroup
            '                Dim loc As String = lDataset.Attributes.GetValue("STAID", "")
            '                If String.IsNullOrEmpty(loc) Then
            '                    loc = lDataset.Attributes.GetValue("Location", "")
            '                    lDataset.Attributes.SetValue("STAID", loc)
            '                End If
            '                If loc = lStaId Then
            '                    Dim lTs As atcTimeseries = lDataset.Attributes.GetValue(lNDayTimeseriesName)
            '                    If lTs IsNot Nothing Then
            '                        lDataGroup.Add(lTs)
            '                    End If
            '                End If
            '            Next
            '            If lDataGroup.Count > 0 Then
            '                lGraphForm = New atcGraph.atcGraphForm()
            '                With lGraphForm
            '                    Dim lGraph As New atcGraph.clsGraphFrequency(lDataGroup, .ZedGraphCtrl)
            '                    .Grapher = lGraph
            '                    Dim lGraphName As String = lGraphFilePrefix & "_" & lStaId & "_" & lNDayTimeseriesName & ".png"
            '                    .SaveGraph(IO.Path.Combine(aDirectory, lGraphName))
            '                    .Close()
            '                End With
            '            End If
            '            lDataGroup.Clear()
            '            lDataGroup = Nothing
            '        Next
            '    Next

            '    'For Each lDataSet As atcTimeseries In pDataGroup
            '    '    lGraphForm = lGraphPlugin.Show(New atcTimeseriesGroup(lDataSet), "Frequency")
            '    '    lGraphForm.Icon = Me.Icon
            '    'Next
            'ElseIf lMNDayPlot Then
            '    For Each lNDayTimeseriesName As String In lNDayList
            '        Dim lDataGroup As New atcData.atcTimeseriesGroup
            '        For Each lDataset As atcTimeseries In aDataGroup
            '            Dim lTs As atcTimeseries = lDataset.Attributes.GetValue(lNDayTimeseriesName)
            '            If lTs IsNot Nothing Then
            '                lDataGroup.Add(lTs)
            '            End If
            '        Next
            '        If lDataGroup IsNot Nothing AndAlso lDataGroup.Count > 0 Then
            '            'lGraphForm = lGraphPlugin.Show(lDataGroup, "Frequency")
            '            'lGraphForm.Icon = Me.Icon
            '            lGraphForm = New atcGraph.atcGraphForm()
            '            With lGraphForm
            '                Dim lGraph As New atcGraph.clsGraphFrequency(lDataGroup, .ZedGraphCtrl)
            '                .Grapher = lGraph
            '                Dim lGraphName As String = lGraphFilePrefix & "_" & lNDayTimeseriesName & ".png"
            '                .SaveGraph(IO.Path.Combine(aDirectory, lGraphName))
            '                .Close()
            '            End With
            '        End If
            '        lDataGroup.Clear()
            '        lDataGroup = Nothing
            '    Next
            'ElseIf lMStationPlot Then
            '    Dim lDataGroup As New atcData.atcTimeseriesGroup()
            '    For Each lStaId As String In lStnList
            '        'lDataGroup = aDataGroup.FindData("STAID", lStaId)
            '        For Each lDataset As atcTimeseries In aDataGroup
            '            Dim loc As String = lDataset.Attributes.GetValue("STAID", "")
            '            If String.IsNullOrEmpty(loc) Then
            '                loc = lDataset.Attributes.GetValue("Location", "")
            '                lDataset.Attributes.SetValue("STAID", loc)
            '            End If
            '            If loc = lStaId Then
            '                lDataGroup.Add(lDataset)
            '            End If
            '        Next

            '        If lDataGroup.Count > 0 Then
            '            'lGraphForm = lGraphPlugin.Show(lDataGroup, "Frequency")
            '            'lGraphForm.Icon = Me.Icon
            '            lGraphForm = New atcGraph.atcGraphForm()
            '            With lGraphForm
            '                Dim lGraph As New atcGraph.clsGraphFrequency(lDataGroup, .ZedGraphCtrl)
            '                .Grapher = lGraph
            '                Dim lGraphName As String = lGraphFilePrefix & "_" & lStaId & ".png"
            '                .SaveGraph(IO.Path.Combine(aDirectory, lGraphName))
            '                .Close()
            '            End With
            '        End If
            '        lDataGroup.Clear()
            '        'lDataGroup = Nothing
            '    Next
            'Else
            '    'lGraphForm = lGraphPlugin.Show(aDataGroup, "Frequency")
            '    'lGraphForm.Icon = Me.Icon
            '    lGraphForm = New atcGraph.atcGraphForm()
            '    With lGraphForm
            '        Dim lGraph As New atcGraph.clsGraphFrequency(aDataGroup, .ZedGraphCtrl)
            '        .Grapher = lGraph
            '        Dim lGraphName As String = lGraphFilePrefix & "_All.png"
            '        .SaveGraph(IO.Path.Combine(aDirectory, lGraphName))
            '        .Close()
            '    End With
            'End If

            'lNDayList.Clear()
            'lNDayList = Nothing
            'lStnList.Clear()
            'lStnList = Nothing
        End Sub

        'Public Shared Function DoFrequencyGrid(ByVal aDataGroup As atcTimeseriesGroup, _
        '                                   ByVal aGroupArgs As atcDataAttributes, _
        '                                   ByVal aNDayDbl() As Double, _
        '                                   ByVal aReturnPeriodDbl() As Double, _
        '                                   ByVal aHighFlag As Boolean) As String

        '    Try
        '        'Dim lNDayValuesHighDone As Boolean = InputNames.CalculateNDayValues(aDataGroup, aGroupArgs, aNDayDbl, aReturnPeriodDbl, aHighFlag)
        '        'Dim lFreqForm As New frmDisplayFrequencyGrid(aDataGroup:=aDataGroup, _
        '        '                                 aHigh:=aHighFlag, _
        '        '                                 aNday:=aNDayDbl, _
        '        '                                 aReturns:=aReturnPeriodDbl, _
        '        '                                 aShowForm:=False)
        '        'Return lFreqForm.ToString()
        '    Catch ex As Exception
        '        Return ""
        '    End Try
        'End Function

        Public Shared Sub GetNdayReturnPeriodAttributes(ByVal aInputs As atcDataAttributes, _
                                                        ByRef aNDay() As Double, _
                                                        ByRef aReturnPeriod() As Double)
            ReDim aNDay(0)
            ReDim aReturnPeriod(0)
            aNDay = Nothing
            aReturnPeriod = Nothing
            'When read in from spec file, the ndays and return periods are already the selected on in 
            'comma-delimited strings
            Dim listNDays As String = "" 'aInputs.GetValue(NDays, "")
            If Not String.IsNullOrEmpty(listNDays) Then
                Dim lNDaysText() As String = listNDays.Split(",")
                Dim lNDay As Double
                Dim lNDaysCol As New ArrayList
                For Each lNDayText As String In lNDaysText
                    If Double.TryParse(lNDayText, lNDay) Then
                        If Not lNDaysCol.Contains(lNDay) Then lNDaysCol.Add(lNDay)
                    End If
                Next
                ReDim aNDay(lNDaysCol.Count - 1)
                For d As Integer = 0 To lNDaysCol.Count - 1
                    aNDay(d) = lNDaysCol(d)
                Next
            End If
            'Dim lNDays As atcCollection = aInputs.GetValue(NDays, Nothing)
            'If lNDays IsNot Nothing Then
            '    Dim lNdaysCol As New ArrayList()
            '    For Each lNDay As Double In lNDays.Keys
            '        If lNDays.ItemByKey(lNDay) Then
            '            lNdaysCol.Add(lNDay)
            '        End If
            '    Next
            '    ReDim aNDay(lNdaysCol.Count - 1)
            '    For d As Integer = 0 To aNDay.Length - 1
            '        aNDay(d) = lNdaysCol(d)
            '    Next
            'End If

            Dim listRPs As String = "" 'aInputs.GetValue(ReturnPeriodText, "")
            If Not String.IsNullOrEmpty(listRPs) Then
                Dim lRPsText() As String = listRPs.Split(",")
                Dim lRP As Double
                Dim lRPsCol As New ArrayList
                For Each lRPText As String In lRPsText
                    If Double.TryParse(lRPText, lRP) Then
                        If Not lRPsCol.Contains(lRP) Then lRPsCol.Add(lRP)
                    End If
                Next
                ReDim aReturnPeriod(lRPsCol.Count - 1)
                For d As Integer = 0 To lRPsCol.Count - 1
                    aReturnPeriod(d) = lRPsCol(d)
                Next
            End If

            'Dim lRPs As atcCollection = aInputs.GetValue(ReturnPeriods, Nothing)
            'If lRPs IsNot Nothing Then
            '    Dim lRPsCol As New ArrayList()
            '    For Each lRP As Double In lRPs.Keys
            '        If lRPs.ItemByKey(lRP) Then
            '            lRPsCol.Add(lRP)
            '        End If
            '    Next
            '    ReDim aReturnPeriod(lRPsCol.Count - 1)
            '    For d As Integer = 0 To aReturnPeriod.Length - 1
            '        aReturnPeriod(d) = lRPsCol(d)
            '    Next
            'End If
        End Sub

        'Public Shared Function TrendAnalysis(ByVal aDataGroup As atcTimeseriesGroup, _
        '                                     ByVal aInputArgs As atcDataAttributes, _
        '                                     ByVal aNDays() As Double, _
        '                                     ByVal aHighFlow As Boolean) As String
        '    Dim lCalcArgs As New atcDataAttributes()
        '    'With lCalcArgs
        '    ' not needed as this is done inside computeRankedAnnualTimeseries
        '    '    .SetValue("Timeseries", aDataGroup)
        '    '    .SetValue(NDay, aNDays)
        '    'End With
        '    SetInputsForAnalysis(aInputArgs, lCalcArgs, aHighFlow)
        '    'Dim lRankedAnnual As atcTimeseriesGroup = _
        '    '       clsSWSTATPlugin.ComputeRankedAnnualTimeseries(aTimeseriesGroup:=aDataGroup, _
        '    '                                                     aNDay:=aNDays, _
        '    '                                                     aHighFlag:=aHighFlow, _
        '    '                                                     aFirstYear:=lCalcArgs.GetValue(InputNames.StartYear), _
        '    '                                                     aLastYear:=lCalcArgs.GetValue(InputNames.EndYear), _
        '    '                                                     aBoundaryMonth:=lCalcArgs.GetValue(InputNames.StartMonth), _
        '    '                                                     aBoundaryDay:=lCalcArgs.GetValue(InputNames.StartDay), _
        '    '                                                     aEndMonth:=lCalcArgs.GetValue(InputNames.EndMonth), _
        '    '                                                     aEndDay:=lCalcArgs.GetValue(InputNames.EndDay))
        '    'Dim lTrendGridText As String = ""
        '    'If lRankedAnnual.Count > 0 Then
        '    '    Dim lTrendAttributes As New Generic.List(Of String)
        '    '    Dim lDateFormat As New atcDateFormat
        '    '    With lDateFormat
        '    '        .IncludeHours = False
        '    '        .IncludeMinutes = False
        '    '        .IncludeSeconds = False
        '    '    End With
        '    '    For Each lTS As atcTimeseries In lRankedAnnual
        '    '        With lTS.Attributes
        '    '            .SetValue("Original ID", lTS.OriginalParent.Attributes.GetValue("ID"))
        '    '            .SetValue("From", lDateFormat.JDateToString(lTS.Dates.Value(1)))
        '    '            .SetValue("To", lDateFormat.JDateToString(lTS.Dates.Value(lTS.numValues)))
        '    '            .SetValue("Not Used", .GetValue("Count Missing"))
        '    '        End With
        '    '    Next
        '    '    Dim lTrendSource As New atcTimeseriesGridSource(lRankedAnnual, TrendAttributes(), True, True)
        '    '    'mnuViewValues.Checked, _
        '    '    'mnuFilterNoData.Checked)
        '    '    With lTrendSource
        '    '        .AttributeValuesEditable = True 'mnuEditAtrributeValues.Checked
        '    '        .DisplayValueAttributes = False 'mnuViewValueAttributes.Checked
        '    '        .DateFormat = lDateFormat
        '    '    End With
        '    '    Dim lList As New atcList.atcListForm
        '    '    With lList
        '    '        With .DateFormat
        '    '            .IncludeDays = False
        '    '            .IncludeHours = False
        '    '            .IncludeMinutes = False
        '    '            .IncludeMonths = False
        '    '        End With
        '    '        If aHighFlow Then
        '    '            .Text = "Trend of High Annual Time Series and Statistics"
        '    '        Else
        '    '            .Text = "Trend of Low Annual Time Series and Statistics"
        '    '        End If
        '    '        .Initialize(lRankedAnnual, TrendAttributes(), , , False)
        '    '        .SwapRowsColumns = True
        '    '        '.Icon = Me.Icon
        '    '        lTrendGridText = .ToString()
        '    '    End With
        '    '    'Return lTrendSource
        '    'Else
        '    '    'Return Nothing
        '    'End If
        '    'Return lTrendGridText
        'End Function
    End Class
End Module
