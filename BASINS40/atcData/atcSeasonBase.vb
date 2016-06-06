Imports atcData
Imports atcUtility
Imports MapWinUtility

Public Class atcSeasonBase

    Private pAllSeasons As Integer() = {}
    Private pSeasonsSelected As New ArrayList

    Private Shared pNaN As Double = atcUtility.GetNaN

    Public Overridable Function Clone() As atcSeasonBase
        Dim lNewSeason As New atcSeasonBase
        lNewSeason.SeasonsSelected = pSeasonsSelected.Clone
        Return lNewSeason
    End Function

    Public ReadOnly Property Name() As String
        Get
            Dim lName As String = Me.GetType.Name
            If lName.Equals("atcSeasonsBase") Then
                Return ""
            Else
                Return "Timeseries::Seasonal - " & lName.Substring(10) 'Skip prefix "atcSeasons"
            End If
        End Get
    End Property

    Public ReadOnly Property Category() As String
        Get
            Return "Seasons"
        End Get
    End Property

    Public ReadOnly Property Description() As String
        Get
            Return Name
        End Get
    End Property

    'Divide the data in aTS into a group of TS, one per season
    Public Overridable Function Split(ByVal aTS As atcTimeseries, ByVal aSource As atcTimeseriesSource) As atcTimeseriesGroup
        Dim lNewGroup As New atcTimeseriesGroup
        Dim lSeasonIndex As Integer = -1
        Dim lPrevSeasonIndex As Integer
        Dim lNewTS As atcTimeseries
        Dim lNewTSvalueIndex As Integer
        Dim lPoint As Boolean = aTS.Attributes.GetValue("point", False)
        Dim lNumValuesOriginal As Integer = aTS.numValues

        For iValue As Integer = 1 To lNumValuesOriginal
            lPrevSeasonIndex = lSeasonIndex
            If lPoint Then
                lSeasonIndex = SeasonIndex(aTS.Dates.Value(iValue))
            Else
                lSeasonIndex = SeasonIndex(aTS.Dates.Value(iValue - 1))
            End If
            lNewTS = lNewGroup.ItemByKey(lSeasonIndex)
            If lNewTS Is Nothing Then
                lNewTS = New atcTimeseries(aSource)
                CopyBaseAttributes(aTS, lNewTS)
                lNewTS.Dates = New atcTimeseries(aSource)
                'lNewTS.numValues = lNumValuesOriginal
                'lNewTS.Dates.numValues = lNumValuesOriginal
                lNewTS.Attributes.AddHistory("Split by " & ToString() & " " & SeasonName(lSeasonIndex))
                lNewTS.Attributes.Add("SeasonDefinition", Me)
                lNewTS.Attributes.Add("SeasonIndex", lSeasonIndex)
                lNewTS.Attributes.Add("SeasonName", SeasonName(lSeasonIndex))
                Dim lSeasonYearFraction As Double = SeasonYearFraction(lSeasonIndex)
                If lSeasonYearFraction > 0 Then lNewTS.Attributes.Add("SeasonYearFraction", lSeasonYearFraction)
                lNewGroup.Add(lSeasonIndex, lNewTS)
            End If

            If lPoint Then
                lNewTSvalueIndex = lNewTS.Attributes.GetValue("NextIndex", 1)
            Else
                lNewTSvalueIndex = lNewTS.Attributes.GetValue("NextIndex", 0)
                If lPrevSeasonIndex <> lSeasonIndex Then
                    'Insert dummy value for start of interval after skipping dates outside season
                    If lNewTS.numValues <= lNewTSvalueIndex Then
                        lNewTS.numValues += Math.Max(10, lNewTSvalueIndex + 1)
                    End If
                    lNewTS.Value(lNewTSvalueIndex) = pNaN
                    lNewTS.Dates.Value(lNewTSvalueIndex) = aTS.Dates.Value(iValue - 1)
                    lNewTS.ValueAttributes(lNewTSvalueIndex).SetValue("Inserted", True)
                    lNewTSvalueIndex += 1
                End If
            End If
            If lNewTS.numValues <= lNewTSvalueIndex Then
                lNewTS.numValues += Math.Max(10, lNewTSvalueIndex + 1)
            End If
            lNewTS.Value(lNewTSvalueIndex) = aTS.Value(iValue)
            If aTS.ValueAttributesExist(iValue) Then
                lNewTS.ValueAttributes(lNewTSvalueIndex) = aTS.ValueAttributes(iValue)
            End If
            lNewTS.Dates.Value(lNewTSvalueIndex) = aTS.Dates.Value(iValue)
            lNewTS.Attributes.SetValue("NextIndex", lNewTSvalueIndex + 1)
        Next

        For Each lNewTS In lNewGroup
            lNewTSvalueIndex = lNewTS.Attributes.GetValue("NextIndex", 1) - 1
            lNewTS.numValues = lNewTSvalueIndex
            With lNewTS.Attributes
                .RemoveByKey("nextindex")
                .RemoveByKey("begin_date")
                .RemoveByKey("end_date")

            End With
        Next

        If Not atcDataManager.SelectionAttributes.Contains("SeasonName") Then
            atcDataManager.SelectionAttributes.Add("SeasonName")
        End If

        If Not atcDataManager.DisplayAttributes.Contains("SeasonName") Then
            atcDataManager.DisplayAttributes.Add("SeasonName")
        End If

        Return lNewGroup
    End Function

    ''' <summary>
    ''' Divide the data in aTS into a group of TS, one for selected seasons, other for data outside selected seasons
    ''' </summary>
    ''' <param name="aTS">Timeseries to split</param>
    ''' <param name="aSource">Source to use when creating the two timeseries returned, can be Nothing</param>
    ''' <returns>Group of two timeseries: first is values inside season, second is values outside season</returns>
    ''' <remarks>To return just selected timeseries: SplitBySelected(aTS, Nothing)(0)</remarks>
    Public Overridable Function SplitBySelected(ByVal aTS As atcTimeseries, ByVal aSource As atcTimeseriesSource) As atcTimeseriesGroup
        Dim lNewGroup As New atcTimeseriesGroup
        Dim lSeasonIndex As Integer = -1
        Dim lIsInside As Boolean = False
        Dim lWasInside As Boolean = False
        Dim lInsideTS As New atcTimeseries(aSource)
        Dim lOutsideTS As New atcTimeseries(aSource)
        Dim lNewTS As atcTimeseries
        Dim lNewTSvalueIndex As Integer
        Dim lPoint As Boolean = aTS.Attributes.GetValue("point", False)

        CopyBaseAttributes(aTS, lInsideTS)
        With lInsideTS
            .Dates = New atcTimeseries(aSource)
            .numValues = aTS.numValues
            .Dates.numValues = aTS.numValues
            .Attributes.AddHistory("Split by " & ToString() & " Inside " & SeasonsSelectedString())
            .Attributes.Add("SeasonDefinition", Me)
            .Attributes.Add("SeasonName", SeasonsSelectedString())
        End With
        lNewGroup.Add(lSeasonIndex, lInsideTS)

        lSeasonIndex += 1
        CopyBaseAttributes(aTS, lOutsideTS)
        With lOutsideTS
            .Dates = New atcTimeseries(aSource)
            .numValues = aTS.numValues
            .Dates.numValues = aTS.numValues
            .Attributes.AddHistory("Split by " & ToString() & " Outside " & SeasonsSelectedString())
            .Attributes.Add("SeasonDefinition", Me)
            .Attributes.Add("SeasonName", "Not in " & Me.ToString)
        End With
        lNewGroup.Add(lSeasonIndex, lOutsideTS)

        For iValue As Integer = 1 To aTS.numValues
            If lPoint Then
                lSeasonIndex = SeasonIndex(aTS.Dates.Value(iValue))
            Else
                lSeasonIndex = SeasonIndex(aTS.Dates.Value(iValue - 1))
            End If

            lWasInside = lIsInside
            lIsInside = SeasonSelected(lSeasonIndex)

            If lIsInside Then
                lNewTS = lInsideTS
            Else
                lNewTS = lOutsideTS
            End If

            If lPoint Then
                lNewTSvalueIndex = lNewTS.Attributes.GetValue("NextIndex", 1)
            Else
                lNewTSvalueIndex = lNewTS.Attributes.GetValue("NextIndex", 0)
                If lNewTSvalueIndex = 0 OrElse lIsInside <> lWasInside Then
                    'Insert NaN at zero position and at start of interval after dates in other season
                    lNewTS.Value(lNewTSvalueIndex) = pNaN
                    lNewTS.Dates.Value(lNewTSvalueIndex) = aTS.Dates.Value(iValue - 1)
                    lNewTS.ValueAttributes(lNewTSvalueIndex).SetValue("Inserted", True)
                    lNewTSvalueIndex += 1
                End If
            End If
            lNewTS.Value(lNewTSvalueIndex) = aTS.Value(iValue)
            If aTS.ValueAttributesExist(iValue) Then
                lNewTS.ValueAttributes(lNewTSvalueIndex) = aTS.ValueAttributes(iValue)
            End If
            lNewTS.Dates.Value(lNewTSvalueIndex) = aTS.Dates.Value(iValue)
            lNewTS.Attributes.SetValue("NextIndex", lNewTSvalueIndex + 1)
        Next

        For Each lNewTS In lNewGroup
            lNewTSvalueIndex = lNewTS.Attributes.GetValue("NextIndex", 1) - 1
            lNewTS.numValues = lNewTSvalueIndex
            lNewTS.Dates.numValues = lNewTSvalueIndex
            lNewTS.Attributes.RemoveByKey("nextindex")
        Next
        Return lNewGroup
    End Function

    Public Overridable Sub SetSeasonalAttributes(ByVal aTS As atcTimeseries, _
                                     ByVal aAttributes As atcDataAttributes, _
                            Optional ByVal aCalculatedAttributes As atcDataAttributes = Nothing)

        Dim lSplit As atcTimeseriesGroup = Me.Split(aTS, Nothing)

        If aCalculatedAttributes Is Nothing Then
            aCalculatedAttributes = aTS.Attributes
        End If

        Dim lFormat As String = ""
        lFormat = lFormat.PadRight(Int(Math.Log10(lSplit.Count)) + 1, "0")

        For Each lSeasonalTS As atcTimeseries In lSplit
            Dim lSeasonIndex As Integer = lSeasonalTS.Attributes.GetValue("SeasonIndex", 0)
            Dim lSeasonName As String = SeasonName(lSeasonIndex)
            For Each lAttribute As atcDefinedValue In aAttributes
                Dim lNewAttrDefinition As atcAttributeDefinition = lAttribute.Definition.Clone _
                   (lAttribute.Definition.Name & " " & Me.ToString & " " & Format(lSeasonIndex, lFormat) & " " & lSeasonName, _
                    Me.ToString & " " & lAttribute.Definition.Description)
                Dim lNewArguments As New atcDataAttributes
                lNewArguments.SetValue("SeasonDefinition", Me)
                lNewArguments.SetValue("SeasonIndex", lSeasonIndex)
                lNewArguments.SetValue("SeasonName", lSeasonName)
                lNewAttrDefinition.Calculator = lAttribute.Definition.Calculator
                aCalculatedAttributes.SetValue(lNewAttrDefinition, lSeasonalTS.Attributes.GetValue(lAttribute.Definition.Name), lNewArguments)
            Next
        Next
    End Sub

    ''' <summary>
    ''' True if specified season index is selected, False if it is not
    ''' </summary>
    ''' <param name="aSeasonIndex">Index of season to get/set selection of</param>
    Public Overridable Property SeasonSelected(ByVal aSeasonIndex As Integer) As Boolean
        Get
            Return pSeasonsSelected.Contains(aSeasonIndex)
        End Get
        Set(ByVal aSelect As Boolean)
            If aSelect Then
                If Not pSeasonsSelected.Contains(aSeasonIndex) Then
                    pSeasonsSelected.Add(aSeasonIndex)
                End If
            Else
                While pSeasonsSelected.Contains(aSeasonIndex)
                    pSeasonsSelected.Remove(aSeasonIndex)
                End While
            End If
        End Set
    End Property

    ''' <summary>
    ''' Integer ArrayList containing SeasonIndex of selected seasons
    ''' </summary>
    Public Overridable Property SeasonsSelected() As ArrayList
        Get
            Return pSeasonsSelected.Clone
        End Get
        Set(ByVal newValue As ArrayList)
            pSeasonsSelected = newValue.Clone
            'pSeasonsSelected.Clear()
            'For Each lIndex As Integer In newValue
            '    SeasonSelected(lIndex) = True
            'Next
        End Set
    End Property

    ''' <summary>
    ''' List of selected season names
    ''' </summary>
    ''' <param name="aXML">True to get list formatted as XML, False for plain text</param>
    Public Overridable Function SeasonsSelectedString(Optional ByVal aXML As Boolean = False) As String
        Dim lSeasons As String = ""
        Dim lSeasonsSelected As ArrayList = SeasonsSelected

        'Shorten the string by replacing contiguous seasons between start and end with a dash
        'For example "Jun Jul Aug Sep" becomes "Jun - Sep"
        If Not aXML AndAlso lSeasonsSelected.Count > 3 Then
            Dim lContiguous As Boolean = True
            Dim lPrevSeason As Integer = -1
            Dim lContiguousStart As Integer = -1
            Dim lAllSeasons() As Integer = AllSeasons()
            Dim lSeasonSelectedIndex As Integer
            For lSeasonSelectedIndex = 0 To lSeasonsSelected.Count - 1
                Dim lSeasonSelected As Integer = lSeasonsSelected(lSeasonSelectedIndex)
                If lPrevSeason > -1 AndAlso lSeasonSelected <> lPrevSeason + 1 Then
                    If lAllSeasons.GetUpperBound(0) > 1 AndAlso lPrevSeason = lAllSeasons(lAllSeasons.GetUpperBound(0)) AndAlso lSeasonSelected = lAllSeasons(0) Then
                        'Wrapping around to start, still contiguous
                    Else
                        lContiguousStart = lSeasonSelectedIndex
                        lContiguous = False
                        Exit For
                    End If
                End If
                lPrevSeason = lSeasonSelected
            Next
            If lContiguous Then
                Return SeasonName(lSeasonsSelected(0)) & " - " & SeasonName(lSeasonsSelected(lSeasonsSelected.Count - 1))
            End If

            'See if they really are contiguous one more time in case SeasonsSelected starts in the middle of wrapping around
            lContiguous = True
            lSeasonSelectedIndex = lContiguousStart
            lPrevSeason = -1
            Do
                Dim lSeasonSelected As Integer = lSeasonsSelected(lSeasonSelectedIndex)
                If lPrevSeason > -1 AndAlso lSeasonSelected <> lPrevSeason + 1 Then
                    If lAllSeasons.GetUpperBound(0) > 1 AndAlso lPrevSeason = lAllSeasons(lAllSeasons.GetUpperBound(0)) AndAlso lSeasonSelected = lAllSeasons(0) Then
                        'Wrapping around to start, still contiguous
                    Else
                        lContiguous = False
                        Exit Do
                    End If
                End If
                lPrevSeason = lSeasonSelected

                lSeasonSelectedIndex += 1
                If lSeasonSelectedIndex = lSeasonsSelected.Count Then
                    lSeasonSelectedIndex = 0
                End If
            Loop While lSeasonSelectedIndex <> lContiguousStart

            If lContiguous Then
                Return SeasonName(lSeasonsSelected(lContiguousStart)) & " - " & SeasonName(lSeasonsSelected(lContiguousStart - 1))
            End If

        End If

        For Each lSeasonIndex As Integer In lSeasonsSelected
            Dim lSeasonName As String = SeasonName(lSeasonIndex)
            If aXML Then
                lSeasons &= "  <Selected Name='" & lSeasonName & "'>" & lSeasonIndex & "</Selected>" & vbCrLf
            Else
                lSeasons &= lSeasonName & " "
            End If
        Next

        If aXML AndAlso lSeasons.Length > 0 Then lSeasons = "<SeasonsSelected>" & vbCrLf & lSeasons & "</SeasonsSelected>" & vbCrLf
        Return lSeasons.TrimEnd(" ")
    End Function

    Public Overridable Property SeasonsSelectedXML() As String
        Get
            Return SeasonsSelectedString(True)
        End Get
        Set(ByVal newValue As String)
            Try
                pSeasonsSelected.Clear()
                Dim lNextSelected As Integer = newValue.IndexOf("<Selected")
                While lNextSelected > -1
                    lNextSelected = newValue.IndexOf(">", lNextSelected + 10) + 1
                    Dim lEndSelected As Integer = newValue.IndexOf("</Selected>", lNextSelected)
                    SeasonSelected(CInt(newValue.Substring(lNextSelected, lEndSelected - lNextSelected))) = True
                    lNextSelected = newValue.IndexOf("<Selected", lNextSelected)
                End While
            Catch ex As Exception
                Logger.Dbg("Unable to read SeasonsSelectedXML" & vbCrLf & newValue)
            End Try
        End Set
    End Property

    ''' <summary>
    ''' Returns an ArrayList containing all the unique season indexes which correspond to dates in the given array
    ''' </summary>
    ''' <param name="aDates">Dates to search for seasons</param>
    Public Function AllSeasonsInDates(ByVal aDates As Double()) As Integer()
        Dim lAllSeasons As New ArrayList
        Dim lSeasonIndex As Integer
        Dim lLastSeason As Integer = Integer.MinValue
        For Each lDateValue As Double In aDates
            If Not Double.IsNaN(lDateValue) Then
                lSeasonIndex = SeasonIndex(lDateValue)
                If lSeasonIndex <> lLastSeason Then
                    lLastSeason = lSeasonIndex
                    If Not lAllSeasons.Contains(lSeasonIndex) Then
                        lAllSeasons.Add(lSeasonIndex)
                    End If
                End If
            End If
        Next
        Return lAllSeasons.ToArray(GetType(Integer))
    End Function

    Public Overridable Function AllSeasons() As Integer()
        Return pAllSeasons
    End Function

    Public Overridable Function AllSeasonNames() As String()
        Dim lIndexArray As Integer() = AllSeasons()
        Dim lSeasonNames As String()
        Dim lLastSeason As Integer = lIndexArray.GetUpperBound(0)
        ReDim lSeasonNames(lLastSeason)
        For i As Integer = 0 To lLastSeason
            lSeasonNames(i) = SeasonName(lIndexArray(i))
        Next
        Return lSeasonNames
    End Function

    Public Overridable Function SeasonIndex(ByVal aDate As Double) As Integer
        Return -1
    End Function

    Public Overridable Overloads Function SeasonName(ByVal aDate As Double) As String
        Return SeasonName(SeasonIndex(aDate))
    End Function

    Public Overridable Overloads Function SeasonName(ByVal aIndex As Integer) As String
        Return CStr(aIndex)
    End Function

    Public Overridable Overloads Function SeasonYearFraction(ByVal aIndex As Integer) As Double
        Return 0
    End Function

    Public Overrides Function ToString() As String
        Dim lString As String = SeasonsSelectedString()
        If lString.Length > 0 Then
            lString = " (" & lString & ")"
        End If
        Return Name.Replace("Timeseries::Seasonal - ", "") & lString
    End Function

    Public Shared ReadOnly Property AllSeasonTypes() As Generic.List(Of Type)
        Get
            Dim lAllTypes As New Generic.List(Of Type)
            Try
                Dim lAssembly As Reflection.Assembly = Reflection.Assembly.GetAssembly(GetType(atcSeasonBase))
                Dim lAssemblyTypes As Type() = lAssembly.GetTypes()
                For Each lType As Type In lAssemblyTypes
                    If lType.Name.StartsWith("atcSeasons") Then
                        lAllTypes.Add(lType)
                    End If
                Next
            Catch e As Exception
                Logger.Dbg("AllSeasonTypes: " & e.Message & vbCrLf & e.StackTrace)
            End Try
            Return lAllTypes
        End Get
    End Property

    Public Shared Function CreateSeasonObject(ByVal aSeasonTypeName As String) As atcSeasonBase
        Dim lMatchName As String = aSeasonTypeName.Replace(" ", "").ToLower
        For Each typ As Type In AllSeasonTypes
            If typ.Name.ToLower.Equals(lMatchName) Then
                Try
                    Return typ.InvokeMember(Nothing, Reflection.BindingFlags.CreateInstance, Nothing, Nothing, New Object() {})
                Catch e As Exception
                    Logger.Dbg("Exception creating " & aSeasonTypeName & ": " & e.Message)
                End Try
            End If
        Next
        Logger.Dbg("Could not find season type: " & aSeasonTypeName)
        Return Nothing
    End Function

    Public Shared Function SeasonClassNameToLabel(ByVal aName As String) As String
        If aName.StartsWith("atc") Then
            Dim lCamelCase As String = aName.Substring(10)
            If lCamelCase.Equals("AMorPM") Then
                Return "AM or PM"
            Else
                Dim lReturn As String = lCamelCase.Substring(0, 1)
                For iCh As Integer = 1 To lCamelCase.Length - 1
                    Dim lCurChar As String = lCamelCase.Substring(iCh, 1)
                    If lCurChar.ToUpper.Equals(lCurChar) Then
                        lReturn &= " "
                    End If
                    lReturn &= lCurChar
                Next
                Return lReturn
            End If
        Else
            Return ""
        End If
    End Function

End Class