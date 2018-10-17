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

        Return lNewSegment
    End Function

    Public Sub New(ByVal _NumConstituents As Integer)
        NumConstituents = _NumConstituents
        ReDim BoundTimeSeries(NumConstituents - 1)
        ReDim LoadTimeSeries(NumConstituents - 1)
    End Sub
End Class
