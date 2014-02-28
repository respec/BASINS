Imports System.Collections.ObjectModel
Imports MapWinUtility

Public Class atcWASPSegments
    Inherits KeyedCollection(Of String, atcWASPSegment)

    Protected Overrides Function GetKeyForItem(ByVal aSegment As atcWASPSegment) As String
        Return aSegment.ID
    End Function

    Public WASPProject As atcWASPProject
    Private pNextKey As Integer = 0

    ''' <summary>
    ''' Braided networks are a problem for WASP; remove orphaned segments
    ''' </summary>
    ''' <returns>String describing any problems that occured</returns>
    Friend Function RemoveBraidedSegments() As String
        Dim lProblem As String = ""
        Dim i As Integer = Me.Count - 1
        Do While i >= 0

            Dim lSegment As atcWASPSegment = Me(i)
            If lSegment.Divergence > 0 Then
                Dim lUpExists As Boolean = False
                For Each lUpSegment As atcWASPSegment In Me
                    If lUpSegment.DownID = lSegment.ID Then
                        lUpExists = True
                        Exit For
                    End If
                Next
                If Not lUpExists Then 'remove segment and restart loop
                    Me.RemoveAt(i)
                    i = Me.Count - 1
                    Continue Do
                End If
            End If

            i -= 1
        Loop

        Return lProblem
    End Function

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
            lSegment.WaspID = 0
        Next
        pNextKey = 0

        Dim lDownstreamKey As String = DownstreamKey(lProblem)
        If lProblem.Length = 0 Then
            If lDownstreamKey.Length > 0 Then AssignWaspIdAndMoveUpstream(lDownstreamKey)

            For Each lSegment As atcWASPSegment In Me
                If lSegment.WaspID = 0 Then
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
        lDownStreamSegment.WaspID = pNextKey

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
                    If lSegment.WaspID = 0 Then 'other channel
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
                lString.Append(.WaspID.ToString.PadRight(10) & " ")
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

        'For Each lTimeseries As atcWASPTimeseries In Me.WASPProject.InputTimeseriesCollection
        '    'first write project-wide timeseries

        '    'does any segment use this?
        '    Dim lUsedBySegment As Boolean = False
        '    For Each lSegment As atcWASPSegment In Me
        '        For Each lTempTimeseries As atcWASPTimeseries In lSegment.InputTimeseriesCollection
        '            If lTempTimeseries.Type = lTimeseries.Type And lTempTimeseries.Description = lTimeseries.Description Then
        '                'yes, used by a segment 
        '                lUsedBySegment = True
        '            End If
        '        Next
        '    Next

        '    If Not lUsedBySegment Then
        '        lString.Append("<all>".PadRight(49) & " ")
        '        lString.Append(lTimeseries.Type.PadRight(16) & " ")
        '        lString.Append(lTimeseries.TimeSeries.Attributes.GetDefinedValue("Location").Value & "." & lTimeseries.Type & ".DAT")
        '        lString.Append(vbCrLf)
        '    End If
        'Next

        'For Each lSegment As atcWASPSegment In Me
        '    For Each lTimeseries As atcWASPTimeseries In lSegment.InputTimeseriesCollection
        '        lString.Append(lSegment.Name.PadRight(32) & " ")
        '        lString.Append(lSegment.ID.PadRight(16) & " ")
        '        lString.Append(lTimeseries.Type.PadRight(16) & " ")
        '        If lTimeseries.TimeSeries Is Nothing Then
        '            lString.Append(lTimeseries.Identifier & "." & lTimeseries.Type & ".DAT")
        '        Else
        '            lString.Append(lTimeseries.TimeSeries.Attributes.GetDefinedValue("Location").Value & "." & lTimeseries.Type & ".DAT")
        '        End If
        '        lString.Append(vbCrLf)
        '    Next
        'Next

        Return lString.ToString
    End Function

    Public Function Save(ByRef sw As IO.StreamWriter) As Boolean
        sw.WriteLine("Number of segments that follow:")
        sw.WriteLine(Me.Count)
        For Each lSegment As atcWASPSegment In Me
            lSegment.Save(sw)
        Next
        Return True
    End Function

    Public Function Load(ByRef sr As IO.StreamReader) As Boolean
        sr.ReadLine() 'skip text header
        Dim cnt As Integer = sr.ReadLine
        For i As Integer = 0 To cnt - 1
            Dim lSegment As New atcWASPSegment(WASPProject.WASPConstituents.Count)
            lSegment.Load(sr)
            Me.Add(lSegment)
        Next
        Return True
    End Function

    Public Sub New()

    End Sub
End Class

Public Class atcWASPSegment
    Public Name As String = ""
    Public ID As String = ""
    Public DownID As String = ""
    Public WaspName As String = ""
    Public Length As Double = 0.0
    Public Width As Double = 0.0
    Public Depth As Double = 0.0
    Public Slope As Double = "0.05"
    Public Roughness As Double = 0.05
    Public Velocity As Double = 0.0
    Public FlowTimeSeries As clsTimeSeriesSelection     'time series of flows in reach (only if is boundary segment)
    Public BoundTimeSeries() As clsTimeSeriesSelection  'concentrations in reach (only if is boundary segment) (one item for each constituent)
    Public LoadTimeSeries() As clsTimeSeriesSelection   'loads in reach (one item for each constituent)
    Public BaseID As String = ""  'the id of the segment before breaking it up
    Public CentroidX As Double
    Public CentroidY As Double
    Public PtsX() As Double
    Public PtsY() As Double
    Public CumulativeDrainageArea As Double
    Public MeanAnnualFlow As Double
    Public WaspID As Integer
    Public Divergence As Integer = 0 'added by LCW so can eliminate braided channels (if segment has divergence>0 and no upstream connection)

    'internal variables for atcWASPProject
    Friend Removed As Boolean = False
    Friend TooShort As Boolean = False
    Friend CountAbove As Integer = 0
    Public IsBoundary As Boolean = False

    'passed when created so can dimension arrays (depends on Wasp model type)
    Private NumConstituents As Integer = 0

    Public ReadOnly Property ID_Name() As String
        Get
            Return ID & ":" & Name
        End Get
    End Property

    Public Sub Save(ByRef sw As IO.StreamWriter)
        With Me
            WriteLine(sw, "Name\tID\tWASPID\tLength\tWidth\tDepth\tSlope\tRoughness\tDownID\tD.A.\tFlow\tVelocity\tBaseID\tCentroidX\tCentroidY")
            WriteLine(sw, "{0}\t{1}\t{2}\t{3}\t{4:0.000}\t{5:0.000}\t{6:0.000}\t{7:0.00000}\t{8:0.000}\t{9}\t{10:0.0000}\t{11:0.0000}\t{12:0.0000}\t{13}\t{14:0.0000}\t{15:0.0000}", _
                          .Name, .ID, .WaspName, .WaspID, .Length, .Width, .Depth, .Slope, .Roughness, .DownID, .CumulativeDrainageArea, .MeanAnnualFlow, .Velocity, .BaseID, .CentroidX, .CentroidY)
            WriteLine(sw, "Segment X Points")
            Dim s As String = ""
            For i As Integer = 0 To PtsX.Length - 1
                s &= IIf(s = "", "", "\t") & PtsX(i).ToString("0.000")
            Next
            WriteLine(sw, s)
            WriteLine(sw, "Segment Y Points")
            s = ""
            For i As Integer = 0 To PtsY.Length - 1
                s &= IIf(s = "", "", "\t") & PtsY(i).ToString("0.000")
            Next
            WriteLine(sw, s)
            WriteLine(sw, "Flow Time Series for Boundary Reach")
            WriteLine(sw, FlowTimeSeries.ToFullString)
            WriteLine(sw, "Boundary Time Series for Each Constituent")
            For i As Integer = 0 To BoundTimeSeries.Length - 1
                WriteLine(sw, BoundTimeSeries(i).ToFullString)
            Next
            WriteLine(sw, "Load Time Series for Each Constituent")
            For i As Integer = 0 To LoadTimeSeries.Length - 1
                WriteLine(sw, LoadTimeSeries(i).ToFullString)
            Next
        End With
    End Sub

    Public Sub Load(ByRef sr As IO.StreamReader)
        With Me
            Dim lastline As String = ""
            lastline = sr.ReadLine 'skip header
            lastline = sr.ReadLine
            Try
                Dim ar() As String = lastline.Split(vbTab)
                If ar.Length = 16 Then
                    .Name = ar(0)
                    .ID = ar(1)
                    .WaspName = ar(2)
                    .WaspID = ar(3)
                    .Length = ar(4)
                    .Width = ar(5)
                    .Depth = ar(6)
                    .Slope = ar(7)
                    .Roughness = ar(8)
                    .DownID = ar(9)
                    .CumulativeDrainageArea = ar(10)
                    .MeanAnnualFlow = ar(11)
                    .Velocity = ar(12)
                    .BaseID = ar(13)
                    .CentroidX = ar(14)
                    .CentroidY = ar(15)
                Else
                    Throw New IO.InvalidDataException("Invalid number of items on line.")
                End If

                lastline = sr.ReadLine 'skip header
                lastline = sr.ReadLine
                Dim arX() As String = lastline.Split(vbTab)

                lastline = sr.ReadLine 'skip header
                lastline = sr.ReadLine
                Dim arY() As String = lastline.Split(vbTab)

                If arX.Length <> arY.Length Then Throw New IO.InvalidDataException("Number of X and Y segment points do not match.")

                ReDim .PtsX(arX.Length - 1), .PtsY(arY.Length - 1)
                For i As Integer = 0 To arX.Length - 1
                    .PtsX(i) = Val(arX(i))
                    .PtsY(i) = Val(arY(i))
                Next

                lastline = sr.ReadLine 'skip header
                lastline = sr.ReadLine
                FlowTimeSeries = New clsTimeSeriesSelection(lastline)

                lastline = sr.ReadLine 'skip header
                For i As Integer = 0 To BoundTimeSeries.Length - 1
                    lastline = sr.ReadLine
                    BoundTimeSeries(i) = New clsTimeSeriesSelection(lastline)
                Next

                lastline = sr.ReadLine 'skip header
                For i As Integer = 0 To LoadTimeSeries.Length - 1
                    lastline = sr.ReadLine
                    LoadTimeSeries(i) = New clsTimeSeriesSelection(lastline)
                Next

            Catch ex As Exception
                Throw New IO.InvalidDataException("Invalid input for input line: " & lastline, ex)
            End Try
        End With
    End Sub

    Public Function Clone() As atcWASPSegment
        NumConstituents = Me.LoadTimeSeries.GetUpperBound(0) + 1
        Dim lNewSegment As New atcWASPSegment(NumConstituents)
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
        lNewSegment.WaspName = Me.WaspName
        lNewSegment.WaspID = Me.WaspID
        lNewSegment.FlowTimeSeries = Me.FlowTimeSeries
        For i As Integer = 0 To NumConstituents - 1
            lNewSegment.LoadTimeSeries(i) = Me.LoadTimeSeries(i)
            lNewSegment.BoundTimeSeries(i) = Me.BoundTimeSeries(i)
        Next

        'Dim lTimeseriesCollection As New atcWASP.atcWASPTimeseriesCollection
        'lNewSegment.InputTimeseriesCollection = lTimeseriesCollection
        'For Each lWASPTimeseries As atcWASPTimeseries In Me.InputTimeseriesCollection
        '    lNewSegment.InputTimeseriesCollection.Add(lWASPTimeseries)
        'Next

        Return lNewSegment
    End Function

    Public Sub New(ByVal _NumConstituents As Integer)
        NumConstituents = _NumConstituents
        ReDim BoundTimeSeries(NumConstituents - 1)
        ReDim LoadTimeSeries(NumConstituents - 1)
    End Sub
End Class
