Imports atcData
Imports atcUtility
Imports MapWinUtility

Public Class atcSeasonBase

    Private pAllSeasons As Integer() = {}
    Private pSeasonsSelected As New BitArray(0)
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
    Public Overridable Function Split(ByVal aTS As atcTimeseries, ByVal aSource As atcDataSource) As atcDataGroup
        Dim lNewGroup As New atcDataGroup
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

    'Divide the data in aTS into a group of TS, one for selected seasons, other for data outside selected seasons
    Public Overridable Function SplitBySelected(ByVal aTS As atcTimeseries, ByVal aSource As atcDataSource) As atcDataGroup
        Dim lNewGroup As New atcDataGroup
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

        Dim lSplit As atcDataGroup = Me.Split(aTS, Nothing)

        If aCalculatedAttributes Is Nothing Then
            aCalculatedAttributes = aTS.Attributes
        End If

        Dim lFormat As String = ""
        lFormat = lFormat.PadRight(Int(Log10(lSplit.Count)) + 1, "0")

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

    'Seasons that have been selected are set to True
    Public Overridable Property SeasonSelected(ByVal aSeasonIndex As Integer) As Boolean
        Get
            If aSeasonIndex >= 0 AndAlso aSeasonIndex < pSeasonsSelected.Count Then
                Return pSeasonsSelected(aSeasonIndex)
            Else
                Return False
            End If
        End Get
        Set(ByVal newValue As Boolean)
            If pSeasonsSelected.Length < aSeasonIndex + 1 Then
                pSeasonsSelected.Length = aSeasonIndex + 1
            End If
            pSeasonsSelected(aSeasonIndex) = newValue
        End Set
    End Property

    'Seasons that have been selected are set to True
    Protected Overridable Property SeasonsSelected() As BitArray
        Get
            Dim lNumSeasons As Integer = AllSeasons.GetLength(0)
            If pSeasonsSelected.Length < lNumSeasons Then pSeasonsSelected.Length = lNumSeasons
            Return pSeasonsSelected
        End Get
        Set(ByVal newValue As BitArray)
            pSeasonsSelected = newValue
        End Set
    End Property

    Public Function SeasonsSelectedString(Optional ByVal aXML As Boolean = False) As String
        Dim lSeasons As String = ""
        Dim lIndexArray As Integer() = AllSeasons()
        Dim lLastSeason As Integer = lIndexArray.GetUpperBound(0)
        For i As Integer = 0 To lLastSeason
            Dim lSeasonIndex As Integer = lIndexArray(i)
            Dim lSeasonName As String = SeasonName(lSeasonIndex)
            If SeasonSelected(lSeasonIndex) Then
                If aXML Then
                    lSeasons &= "  <Selected Name='" & lSeasonName & "'>" & lSeasonIndex & "</Selected>" & vbCrLf
                Else
                    lSeasons &= lSeasonName & " "
                End If
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
                pSeasonsSelected.SetAll(False) 'TODO: not all season types use pSeasonsSelected, need to clear selection another way
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