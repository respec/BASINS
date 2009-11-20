Imports atcData
Imports atcUtility

Imports System.Reflection

Public Class atcEventPlugin
    Inherits atcData.atcTimeseriesSource

    Private pAvailableOperations As atcDataAttributes ' atcDataGroup
    Private pName As String = "Timeseries::Events"

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Timeseries::Event"
        End Get
    End Property

    Public Overrides ReadOnly Property Category() As String
        Get
            Return "Events"
        End Get
    End Property

    Public Overrides ReadOnly Property Description() As String
        Get
            Return Name
        End Get
    End Property

    Public Overrides ReadOnly Property CanOpen() As Boolean
        Get
            Return True
        End Get
    End Property

    'The only element of aArgs is an atcDataGroup or atcTimeseries
    'The attribute(s) will be set to the result(s) of calculation(s)
    Public Overrides Function Open(ByVal aOperationName As String, _
                          Optional ByVal aArgs As atcDataAttributes = Nothing) As Boolean
        Dim ltsGroup As atcTimeseriesGroup = Nothing
        Dim lThresh As Double = GetNaN()
        Dim lDaysGapAllowed As Double = 0
        Dim lHigh As Boolean
        Dim lOk As Boolean
        Dim lExtreme As Double

        If Not aArgs Is Nothing Then
            ltsGroup = DatasetOrGroupToGroup(aArgs.GetValue("Timeseries"))
            lThresh = aArgs.GetValue("Threshold", lThresh)
            lDaysGapAllowed = aArgs.GetValue("DaysGapAllowed", 0)
            lHigh = aArgs.GetValue("High", True)
        End If
        If ltsGroup Is Nothing OrElse ltsGroup.Count = 0 Then
            ltsGroup = atcDataManager.UserSelectData("Select data for " & aOperationName)
        End If
        If ltsGroup.Count > 0 AndAlso Double.IsNaN(lThresh) Then
            Dim lForm As New frmSpecifyEventAttributes
            lOk = lForm.AskUser(lHigh, lThresh, lDaysGapAllowed, MinMaxLabel(ltsGroup))
        End If
        If lOk Then
            For Each lts As atcTimeseries In ltsGroup
                Dim lEvents As atcTimeseriesGroup = EventSplit(lts, Me, lThresh, lDaysGapAllowed, lHigh)
                Select Case aOperationName
                    Case "Split"
                        MyBase.DataSets.AddRange(lEvents)
                    Case "Event Attributes"
                        lts.Attributes.SetValue(EventAttDef(lThresh, lHigh, "Count"), lEvents.Count, aArgs)
                        Dim lDurCnt As Integer = 0
                        Dim lSum As Double = 0
                        Dim lAve As Double = 0
                        If lHigh Then
                            lExtreme = GetMinValue()
                        Else
                            lExtreme = GetMaxValue()
                        End If
                        For Each lETs As atcTimeseries In lEvents
                            lDurCnt += lETs.numValues
                            lSum += lETs.Attributes.GetValue("Sum")
                            lAve += lETs.Attributes.GetValue("Mean")
                            If lHigh AndAlso lETs.Attributes.GetValue("Max") > lExtreme Then
                                lExtreme = lETs.Attributes.GetValue("Max")
                            ElseIf Not lHigh AndAlso lETs.Attributes.GetValue("Min") < lExtreme Then
                                lExtreme = lETs.Attributes.GetValue("Min")
                            End If
                        Next
                        lts.Attributes.SetValue(EventAttDef(lThresh, lHigh, "Duration"), lDurCnt, aArgs)
                        lts.Attributes.SetValue(EventAttDef(lThresh, lHigh, "Sum"), lSum, aArgs)
                        lts.Attributes.SetValue(EventAttDef(lThresh, lHigh, "Mean"), lAve / lEvents.Count, aArgs)
                        If lHigh Then
                            lts.Attributes.SetValue(EventAttDef(lThresh, lHigh, "Max"), lExtreme, aArgs)
                        Else
                            lts.Attributes.SetValue(EventAttDef(lThresh, lHigh, "Min"), lExtreme, aArgs)
                        End If
                End Select
            Next
            If MyBase.DataSets.Count > 0 Then Return True
        End If

    End Function

    Private Function EventAttDef(ByVal aThresh As Double, ByVal aHigh As Boolean, ByVal aName As String) As atcAttributeDefinition
        Dim lFullName As String
        If aHigh Then
            lFullName = "EventHigh" & aThresh & aName
        Else
            lFullName = "EventLow" & aThresh & aName
        End If
        EventAttDef = atcDataAttributes.GetDefinition(lFullName)
        If EventAttDef Is Nothing Then
            EventAttDef = New atcAttributeDefinition
            With EventAttDef
                .Name = lFullName
                If aHigh Then
                    .Description = aName & " of values above " & aThresh
                Else
                    .Description = aName & " of values below " & aThresh
                End If
                .Editable = False
                .TypeString = "Double"
                .Calculator = Me
            End With
        End If
    End Function

    'Public Overrides Sub Initialize(ByVal MapWin As MapWindow.Interfaces.IMapWin, _
    '                                ByVal ParentHandle As Integer)
    'End Sub

    Public Overrides Function ToString() As String
        Return "Event"
    End Function

    Public Overloads ReadOnly Property AvailableOperations(ByVal aIncludeSplits As Boolean, _
                                                           ByVal aIncludeAttributes As Boolean) As atcDataAttributes
        Get
            Dim lOperations As atcDataAttributes
            If aIncludeSplits AndAlso aIncludeAttributes AndAlso Not pAvailableOperations Is Nothing Then
                lOperations = pAvailableOperations
            Else
                lOperations = New atcDataAttributes
                Dim lArguments As atcDataAttributes
                Dim defTimeSeriesOne As New atcAttributeDefinition
                With defTimeSeriesOne
                    .Name = "Timeseries"
                    .Description = "One time series"
                    .Editable = True
                    .TypeString = "atcTimeseries"
                End With

                Dim defThresh As New atcAttributeDefinition
                With defThresh
                    .Name = "Threshold"
                    .Description = "Event Threshold"
                    .DefaultValue = 0
                    .Editable = True
                    .TypeString = "Double"
                End With

                Dim defHigh As New atcAttributeDefinition
                With defHigh
                    .Name = "High"
                    .Description = "High event flag"
                    .DefaultValue = True
                    .Editable = True
                    .TypeString = "Boolean"
                End With

                If aIncludeAttributes Then
                    Dim lAttributes As New atcAttributeDefinition
                    With lAttributes
                        .Name = "Event Attributes"
                        .Category = "" 'Me.ToString
                        .Description = "Attribute values calculated from events"
                        .Editable = False
                        .TypeString = "atcDataAttributes"
                        .Calculator = Me
                    End With
                    lArguments = New atcDataAttributes
                    lArguments.SetValue(defTimeSeriesOne, Nothing)
                    lArguments.SetValue("Attributes", Nothing)

                    lOperations.SetValue(lAttributes, Nothing, lArguments)
                End If

                If aIncludeSplits Then
                    Dim lSplit As New atcAttributeDefinition
                    With lSplit
                        .Name = "Split"
                        .Category = "" '"Split"
                        .Description = "Split a timeseries into events"
                        .Editable = False
                        .TypeString = "atcDataGroup"
                        .Calculator = Me
                    End With

                    lArguments = New atcDataAttributes
                    lArguments.SetValue(defTimeSeriesOne, Nothing)

                    lOperations.SetValue(lSplit, Nothing, lArguments)
                End If
            End If

            If aIncludeSplits AndAlso aIncludeAttributes Then
                pAvailableOperations = lOperations
            End If

            Return lOperations
        End Get
    End Property

    Private Function SeasonClassNameToLabel(ByVal aName As String) As String
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
    End Function

    Public Overloads Overrides ReadOnly Property AvailableOperations() As atcDataAttributes
        Get
            Return AvailableOperations(True, True)
        End Get
    End Property

    Private Function MinMaxLabel(ByVal aTSGroup As atcTimeseriesGroup) As String
        Dim lMin As Double = GetMaxValue()
        Dim lMax As Double = GetMinValue()
        Dim lMinStr As String = ""
        Dim lMaxStr As String = ""
        For Each lts As atcTimeseries In aTSGroup
            If lts.Attributes.GetValue("Min") < lMin Then
                lMin = lts.Attributes.GetValue("Min")
                lMinStr = lts.Attributes.GetFormattedValue("Min")
            End If
            If lts.Attributes.GetValue("Max") > lMax Then
                lMax = lts.Attributes.GetValue("Max")
                lMaxStr = lts.Attributes.GetFormattedValue("Max")
            End If
        Next
        Return lMinStr & " - " & lMaxStr
    End Function
End Class