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

        For iValue As Integer = 1 To aTS.numValues
            lPrevSeasonIndex = lSeasonIndex
            If lPoint Then
                lSeasonIndex = SeasonIndex(aTS.Dates.Value(iValue))
            Else '
                lSeasonIndex = SeasonIndex(aTS.Dates.Value(iValue - 1))
            End If
            lNewTS = lNewGroup.ItemByKey(lSeasonIndex)
            If lNewTS Is Nothing Then
                lNewTS = New atcTimeseries(aSource)
                CopyBaseAttributes(aTS, lNewTS)
                lNewTS.Dates = New atcTimeseries(aSource)
                lNewTS.numValues = aTS.numValues
                lNewTS.Dates.numValues = aTS.numValues
                lNewTS.Attributes.AddHistory("Split by " & ToString() & " " & SeasonName(lSeasonIndex))
                lNewTS.Attributes.Add("SeasonDefinition", Me)
                lNewTS.Attributes.Add("SeasonIndex", lSeasonIndex)
                lNewTS.Attributes.Add("SeasonName", SeasonName(lSeasonIndex))
                lNewGroup.Add(lSeasonIndex, lNewTS)
            End If

            If lPoint Then
                lNewTSvalueIndex = lNewTS.Attributes.GetValue("NextIndex", 1)
            Else
                lNewTSvalueIndex = lNewTS.Attributes.GetValue("NextIndex", 0)
                If aTS.ValueAttributesExist(lNewTSvalueIndex) Then 'TODO:finish this!
                    'lnewts.ValueAttributes(lnewtsvalueindex).SetRange(
                End If
                If lPrevSeasonIndex <> lSeasonIndex Then
                    'Insert dummy value for start of interval after skipping dates outside season
                    lNewTS.Value(lNewTSvalueIndex) = pNaN
                    lNewTS.Dates.Value(lNewTSvalueIndex) = aTS.Dates.Value(iValue - 1)
                    lNewTS.ValueAttributes(lNewTSvalueIndex).SetValue("Inserted", True)
                    lNewTSvalueIndex += 1
                End If
            End If
            lNewTS.Value(lNewTSvalueIndex) = aTS.Value(iValue)
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

    'Divide the data in aTS into a group of TS, one for selected seasons, other for data outside selected seasons
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
                If aTS.ValueAttributesExist(lNewTSvalueIndex) Then 'TODO:finish this!
                    'lnewts.ValueAttributes(lnewtsvalueindex).SetRange(
                End If
                If lNewTSvalueIndex = 0 OrElse lIsInside <> lWasInside Then
                    'Insert NaN at zero position and at start of interval after dates in other season
                    lNewTS.Values(lNewTSvalueIndex) = pNaN
                    lNewTS.Dates.Value(lNewTSvalueIndex) = aTS.Dates.Value(iValue - 1)
                    lNewTS.ValueAttributes(lNewTSvalueIndex).SetValue("Inserted", True)
                    lNewTSvalueIndex += 1
                End If
            End If
            lNewTS.Value(lNewTSvalueIndex) = aTS.Value(iValue)
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
        For Each lSeasonIndex As Integer In SeasonsSelected
            Dim lSeasonName As String = SeasonName(lSeasonIndex)
            If aXML Then
                lSeasons &= "  <Selected Name='" & lSeasonName & "'>" & lSeasonIndex & "</Selected>" & vbCrLf
            Else
                lSeasons &= lSeasonName & " "
            End If
        Next

        If aXML AndAlso lSeasons.Length > 0 Then lSeasons = "<SeasonsSelected>" & vbCrLf & lSeasons & "</SeasonsSelected>" & vbCrLf
        Return lSeasons
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

    Public Overrides Function ToString() As String
        Dim lString As String = SeasonsSelectedString()
        If lString.Length > 0 Then
            lString = " (" & lString & ")"
        End If
        'Skip first part of Name which is "Timeseries::Seasonal - "
        Return Name.Substring(23) & lString
    End Function
End Class