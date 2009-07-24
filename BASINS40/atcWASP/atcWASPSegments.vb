Imports System.Collections.ObjectModel
Imports MapWinUtility

Public Class atcWASPSegments
    Inherits KeyedCollection(Of String, atcWASPSegment)
    Protected Overrides Function GetKeyForItem(ByVal aSegment As atcWASPSegment) As String
        Return aSegment.ID
    End Function

    Public WASPProject As atcWASPProject
    Private pNextKey As Integer = 0

    Friend Function DownstreamKey(ByRef aProblem As String) As String
        Dim lDownstreamKey As String = ""
        For Each lSegment As atcWASPSegment In Me
            Dim lDownExists As Boolean = False
            For Each lDownSegment As atcWASPSegment In Me
                If lDownSegment.ID = lSegment.DownID Then
                    lDownExists = True
                    Exit For
                End If
            Next
            If Not lDownExists Then
                If lDownstreamKey.Length = 0 Then
                    lDownstreamKey = lSegment.ID
                Else
                    aProblem = "Multiple Exits"
                End If
            End If
        Next
        Return lDownstreamKey
    End Function

    ''' <summary>
    ''' Assign Ids to each segment starting at downstream segment
    ''' </summary>
    ''' <returns>String describing any problems that occured</returns>
    ''' <remarks></remarks>
    Public Function AssignWaspIds() As String
        Dim lProblem As String = ""

        For Each lSegment As atcWASPSegment In Me
            lSegment.WASPID = 0
        Next

        Dim lDownstreamKey As String = DownstreamKey(lProblem)
        If lProblem.Length = 0 Then
            If lDownstreamKey.Length > 0 Then AssignWaspIdAndMoveUpstream(lDownstreamKey)

            For Each lSegment As atcWASPSegment In Me
                If lSegment.WASPID = 0 Then
                    If lProblem.Length = 0 Then
                        lProblem = "ProblemSegments "
                    Else
                        lProblem &= ", "
                    End If
                    lProblem &= lSegment.ID
                End If
            Next
        End If
        Return lProblem
    End Function

    Private Sub AssignWaspIdAndMoveUpstream(ByVal aDownstreamKey As String)
        'assign WaspId
        Dim lDownStreamSegment As atcWASPSegment = Me.Item(aDownstreamKey)
        pNextKey += 1
        lDownStreamSegment.WASPID = pNextKey

        'look for upsteam segments and determine largest one
        Dim lUpSteamSegments As New ArrayList
        Dim lUpSteamMaxArea As Double = 0
        Dim lUpSteamMainSegmentKey As String = ""
        For Each lSegment As atcWASPSegment In Me
            If lSegment.DownID = lDownStreamSegment.ID Then
                If lSegment.CumulativeDrainageArea > lUpSteamMaxArea Then
                    lUpSteamMaxArea = lSegment.CumulativeDrainageArea
                    lUpSteamMainSegmentKey = lSegment.ID
                End If
                lUpSteamSegments.Add(lSegment)
            End If
        Next
        If lUpSteamSegments.Count > 0 Then 'main channel first
            AssignWaspIdAndMoveUpstream(lUpSteamMainSegmentKey)
            If lUpSteamSegments.Count > 1 Then
                For Each lSegment As atcWASPSegment In lUpSteamSegments
                    If lSegment.WASPID = 0 Then 'other channel
                        AssignWaspIdAndMoveUpstream(lSegment.ID)
                    End If
                Next
            End If
        End If
    End Sub

    Public Sub AddRange(ByVal aEnumerable As IEnumerable)
        For Each lSegment As atcWASPSegment In aEnumerable
            Try
                Me.Add(lSegment)
            Catch
            End Try
        Next
    End Sub

    Public Overrides Function ToString() As String
        Dim lString As New System.Text.StringBuilder

        lString.Append(";Name                            ID               WASP ID    Length     Width      Depth      Slope      Roughness  DownSegID        " & vbCrLf)
        lString.Append(";                                                            (km)       (m)        (m)                                               " & vbCrLf)
        lString.Append(";_______________________________ ________________ __________ __________ __________ __________ __________ __________ ________________ " & vbCrLf)

        For Each lSegment As atcWASPSegment In Me
            With lSegment
                lString.Append(.Name.PadRight(32) & " ")
                lString.Append(.ID.PadRight(16) & " ")
                lString.Append(.WASPID.ToString.PadRight(10) & " ")
                lString.Append(Format(.Length, "0.00").PadRight(10) & " ")
                lString.Append(Format(.Width, "0.00").PadRight(10) & " ")
                lString.Append(Format(.Depth, "0.00").PadRight(10) & " ")
                lString.Append(Format(.Slope, "0.0000").PadRight(10) & " ")
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

        For Each lTimeseries As atcWASPTimeseries In Me.WASPProject.InputTimeseriesCollection
            'first write project-wide timeseries

            'does any segment use this?
            Dim lUsedBySegment As Boolean = False
            For Each lSegment As atcWASPSegment In Me
                For Each lTempTimeseries As atcWASPTimeseries In lSegment.InputTimeseriesCollection
                    If lTempTimeseries.Type = lTimeseries.Type And lTempTimeseries.Description = lTimeseries.Description Then
                        'yes, used by a segment 
                        lUsedBySegment = True
                    End If
                Next
            Next

            If Not lUsedBySegment Then
                lString.Append("<all>".PadRight(49) & " ")
                lString.Append(lTimeseries.Type.PadRight(16) & " ")
                lString.Append(lTimeseries.TimeSeries.Attributes.GetDefinedValue("Location").Value & "." & lTimeseries.Type & ".DAT")
                lString.Append(vbCrLf)
            End If
        Next

        For Each lSegment As atcWASPSegment In Me
            For Each lTimeseries As atcWASPTimeseries In lSegment.InputTimeseriesCollection
                lString.Append(lSegment.Name.PadRight(32) & " ")
                lString.Append(lSegment.ID.PadRight(16) & " ")
                lString.Append(lTimeseries.Type.PadRight(16) & " ")
                If lTimeseries.TimeSeries Is Nothing Then
                    lString.Append(lTimeseries.Identifier & "." & lTimeseries.Type & ".DAT")
                Else
                    lString.Append(lTimeseries.TimeSeries.Attributes.GetDefinedValue("Location").Value & "." & lTimeseries.Type & ".DAT")
                End If
                lString.Append(vbCrLf)
            Next
        Next

        Return lString.ToString
    End Function

End Class

Public Class atcWASPSegment
    Public Name As String = ""
    Public ID As String = ""
    Public DownID As String = ""
    Public Length As Double = 0.0
    Public Width As Double = 0.0
    Public Depth As Double = 0.0
    Public Slope As Double = "0.05"
    Public Roughness As Double = 0.05
    Public Velocity As Double = 0.0
    Public InputTimeseriesCollection As atcWASPTimeseriesCollection = Nothing
    Public BaseID As String = ""  'the id of the segment before breaking it up
    Public CentroidX As Double
    Public CentroidY As Double
    Public PtsX() As Double
    Public PtsY() As Double
    Public CumulativeDrainageArea As Double
    Public MeanAnnualFlow As Double
    Public WASPID As Integer
    'internal variables for atcWASPProject
    Friend Removed As Boolean = False
    Friend TooShort As Boolean = False
    Friend CountAbove As Integer = 0

    Public Function Clone() As atcWASPSegment
        Dim lNewSegment As New atcWASPSegment
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
        lNewSegment.PtsX = Me.PtsX
        lNewSegment.PtsY = Me.PtsY
        lNewSegment.CumulativeDrainageArea = Me.CumulativeDrainageArea
        lNewSegment.MeanAnnualFlow = Me.MeanAnnualFlow
        lNewSegment.WASPID = Me.WASPID

        Dim lTimeseriesCollection As New atcWASP.atcWASPTimeseriesCollection
        lNewSegment.InputTimeseriesCollection = lTimeseriesCollection
        For Each lWASPTimeseries As atcWASPTimeseries In Me.InputTimeseriesCollection
            lNewSegment.InputTimeseriesCollection.Add(lWASPTimeseries)
        Next

        Return lNewSegment
    End Function
End Class
