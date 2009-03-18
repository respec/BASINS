Imports System.Collections.ObjectModel
Imports MapWinUtility

Public Class Segments
    Inherits KeyedCollection(Of String, Segment)
    Protected Overrides Function GetKeyForItem(ByVal aSegment As Segment) As String
        Dim lKey As String = aSegment.ID & ":" & aSegment.Name
        Return lKey
    End Function

    Public WASPProject As WASPProject
    Private pNextKey As Integer = 0

    ''' <summary>
    ''' Assign Ids to each segment starting at downstream segment
    ''' </summary>
    ''' <returns>String describing any problems that occured</returns>
    ''' <remarks></remarks>
    Public Function AssignWaspIds() As String
        Dim lProblem As String = ""
        Dim lDownstreamKey As String = ""
        For Each lSegment As Segment In Me
            lSegment.WASPID = 0
            Dim lDownExists As Boolean = False
            For Each lDownSegment As Segment In Me
                If lDownSegment.ID = lSegment.DownID Then
                    lDownExists = True
                    Exit For
                End If
            Next
            If Not lDownExists Then
                If lDownstreamKey.Length = 0 Then
                    lDownstreamKey = lSegment.ID & ":" & lSegment.Name
                Else
                    lProblem = "Multiple Exits"
                End If
            End If
        Next

        If lProblem.Length = 0 Then
            AssignWaspIdAndMoveUpstream(lDownstreamKey)

            For Each lSegment As Segment In Me
                If lSegment.WASPID = 0 Then
                    If lProblem.Length = 0 Then
                        lProblem = "ProblemSegments "
                    Else
                        lProblem &= ", "
                    End If
                    lProblem &= lSegment.ID & ":" & lSegment.Name
                End If
            Next
        End If
        Return lProblem
    End Function

    Private Sub AssignWaspIdAndMoveUpstream(ByVal aDownstreamKey As String)
        'assign WaspId
        Dim lDownStreamSegment As Segment = Me.Item(aDownstreamKey)
        pNextKey += 1
        lDownStreamSegment.WASPID = pNextKey

        'look for upsteam segments and determine largest one
        Dim lUpSteamSegments As New ArrayList
        Dim lUpSteamMaxArea As Double = 0
        Dim lUpSteamMainSegmentKey As String = ""
        For Each lSegment As Segment In Me
            If lSegment.DownID = lDownStreamSegment.ID Then
                If lSegment.CumulativeDrainageArea > lUpSteamMaxArea Then
                    lUpSteamMaxArea = lSegment.CumulativeDrainageArea
                    lUpSteamMainSegmentKey = lSegment.ID & ":" & lSegment.Name
                End If
                lUpSteamSegments.Add(lSegment)
            End If
        Next
        If lUpSteamSegments.Count > 0 Then 'main channel first
            AssignWaspIdAndMoveUpstream(lUpSteamMainSegmentKey)
            If lUpSteamSegments.Count > 1 Then
                For Each lSegment As Segment In lUpSteamSegments
                    If lSegment.WASPID = 0 Then 'other channel
                        AssignWaspIdAndMoveUpstream(lSegment.ID & ":" & lSegment.Name)
                    End If
                Next
            End If
        End If
    End Sub

    Public Sub AddRange(ByVal aEnumerable As IEnumerable)
        For Each lSegment As Segment In aEnumerable
            Try
                Me.Add(lSegment)
            Catch
            End Try
        Next
    End Sub

    Public Overrides Function ToString() As String
        Dim lString As New System.Text.StringBuilder

        lString.Append(";Name                            ID               Length     Width      Depth      Slope      Roughness  DownSegID        " & vbCrLf)
        lString.Append(";                                                 (km)       (m)        (m)                                               " & vbCrLf)
        lString.Append(";_______________________________ ________________ __________ __________ __________ __________ __________ ________________ " & vbCrLf)

        For Each lSegment As Segment In Me
            With lSegment
                lString.Append(.Name.PadRight(32) & " ")
                lString.Append(.ID.PadRight(16) & " ")
                lString.Append(Format(.Length, "0.00").PadRight(10) & " ")
                lString.Append(Format(.Width, "0.00").PadRight(10) & " ")
                lString.Append(Format(.Depth, "0.00").PadRight(10) & " ")
                lString.Append(.Slope.PadRight(10) & " ")
                lString.Append(Format(.Roughness, "0.000").PadRight(10) & " ")
                lString.Append(.DownID.PadRight(16) & " ")
                lString.Append(vbCrLf)
            End With
        Next

        Return lString.ToString
    End Function

    Public Function TimeseriesDirectoryToString() As String
        Dim lString As New System.Text.StringBuilder

        lString.Append(";Segment Name                    Segment ID       Timeseries Type  Timeseries File Name                    " & vbCrLf)
        lString.Append(";_______________________________ ________________ ________________ ________________________________________" & vbCrLf)

        For Each lSegment As Segment In Me
            For Each lTimeseries As WASPTimeseries In lSegment.InputTimeseriesCollection
                lString.Append(lSegment.Name.PadRight(32) & " ")
                lString.Append(lSegment.ID.PadRight(16) & " ")
                lString.Append(lTimeseries.Type.PadRight(16) & " ")
                lString.Append(lTimeseries.TimeSeries.Attributes.GetDefinedValue("Location").Value & "." & lTimeseries.Type & ".DAT")
                lString.Append(vbCrLf)
            Next
        Next

        Return lString.ToString
    End Function

End Class

Public Class Segment
    Public Name As String = ""
    Public ID As String = ""
    Public DownID As String = ""
    Public Length As Double = 0.0
    Public Width As Double = 0.0
    Public Depth As Double = 0.0
    Public Slope As String = "0.05"
    Public Roughness As Double = 0.05
    Public Velocity As Double = 0.0
    Public InputTimeseriesCollection As WASPTimeseriesCollection = Nothing
    Public BaseID As String = ""  'the id of the segment before breaking it up
    Public CentroidX As Double
    Public CentroidY As Double
    Public CumulativeDrainageArea As Double
    Public MeanAnnualFlow As Double
    Public WASPID As Integer

    Public Function Clone() As Segment
        Dim lNewSegment As New Segment
        lNewSegment.Depth = Me.Depth
        lNewSegment.DownID = Me.DownID
        lNewSegment.ID = Me.ID
        lNewSegment.Length = Me.Length
        lNewSegment.Name = Me.Name
        lNewSegment.Roughness = Me.Roughness
        lNewSegment.Slope = Me.Slope
        lNewSegment.Velocity = Me.Velocity
        lNewSegment.Width = Me.Width
        lNewSegment.BaseID = Me.BaseID
        lNewSegment.CentroidX = Me.CentroidX
        lNewSegment.CentroidY = Me.CentroidY
        lNewSegment.CumulativeDrainageArea = Me.CumulativeDrainageArea
        lNewSegment.MeanAnnualFlow = Me.MeanAnnualFlow
        lNewSegment.WASPID = Me.WASPID

        Dim lTimeseriesCollection As New atcWASP.WASPTimeseriesCollection
        lNewSegment.InputTimeseriesCollection = lTimeseriesCollection
        For Each lWASPTimeseries As WASPTimeseries In Me.InputTimeseriesCollection
            lNewSegment.InputTimeseriesCollection.Add(lWASPTimeseries)
        Next

        Return lNewSegment
    End Function
End Class
