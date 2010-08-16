Imports atcUtility
Imports MapWinUtility
Imports System.Text.RegularExpressions

Public Class atcSWMMOptions
    Implements IBlock

    Private pName As String

    Public FlowUnits As String = "CFS"
    Public InfiltrationMethod As String = "HORTON"
    Public FlowRouting As String = "KINWAVE"
    Public SJDate As Double = 0.0
    Public EJDate As Double = 0.0
    Public SweepStart As String = "1/1"
    Public SweepEnd As String = "12/31"
    Public DryDays As Integer = 0
    Public ReportStep As String = "00:15:00"
    Public WetStep As String = "00:15:00"
    Public DryStep As String = "01:00:00"
    Public RoutingStep As String = "0:00:30"
    Public AllowPonding As String = "NO"
    Public InertialDamping As String = "PARTIAL"
    Public VariableStep As Double = 0.75
    Public LengtheningStep As Integer = 0
    Public MinSurfArea As Integer = 0
    Public NormalFlowLimited As String = "BOTH"
    Public SkipSteadyState As String = "NO"
    Public IgnoreRainfall As String = "NO"
    Public ForceMainEquation As String = "H-W"
    Public LinkOffsets As String = "DEPTH"
    Public MinSlope As Double = 0

    Property Name() As String Implements IBlock.Name
        Get
            Return pName
        End Get
        Set(ByVal value As String)
            pName = value
        End Set
    End Property

    Public Sub FromString(ByVal aContents As String) Implements IBlock.FromString
        Dim lSDate(6) As Integer
        J2Date(SJDate, lSDate)
        Dim lEDate(6) As Integer
        J2Date(EJDate, lEDate)

        Dim lStartDateString As String = lSDate(1) & "/" & lSDate(2) & "/" & lSDate(0)
        Dim lStartTimeString As String = Format(lSDate(3), "00") & ":" & Format(lSDate(4), "00") & ":00"
        Dim lEndDateString As String = lEDate(1) & "/" & lEDate(2) & "/" & lEDate(0)
        Dim lEndTimeString As String = Format(lEDate(3), "00") & ":" & Format(lEDate(4), "00") & ":00"

        For Each lLine As String In aContents.Split(vbCrLf)
            lLine = lLine.Trim()
            Dim lItems() As String = Regex.Split(lLine, "\s+")
            If lItems.GetUpperBound(0) = 1 Then
                Dim lOption As String = lItems(0).ToUpper.PadRight(19)
                Dim lValue As String = lItems(1)
                Select Case lOption
                    Case "FLOW_UNITS         " : FlowUnits = lValue
                    Case "INFILTRATION       " : InfiltrationMethod = lValue
                    Case "FLOW_ROUTING       " : FlowRouting = lValue
                    Case "START_DATE         " : lStartDateString = lValue
                    Case "START_TIME         " : lStartTimeString = lValue
                    Case "REPORT_START_DATE  " 'lStartDateString = lValue
                    Case "REPORT_START_TIME  " 'lStartTimeString = lValue
                    Case "END_DATE           " : lEndDateString = lValue
                    Case "END_TIME           " : lEndTimeString = lValue
                    Case "SWEEP_START        " : SweepStart = lValue
                    Case "SWEEP_END          " : SweepEnd = lValue
                    Case "DRY_DAYS           " : DryDays = Integer.Parse(lValue)
                    Case "REPORT_STEP        " : ReportStep = lValue
                    Case "WET_STEP           " : WetStep = lValue
                    Case "DRY_STEP           " : DryStep = lValue
                    Case "ROUTING_STEP       " : RoutingStep = lValue
                    Case "ALLOW_PONDING      " : AllowPonding = lValue
                    Case "INERTIAL_DAMPING   " : InertialDamping = lValue
                    Case "VARIABLE_STEP      " : VariableStep = lValue
                    Case "LENGTHENING_STEP   " : LengtheningStep = lValue
                    Case "MIN_SURFAREA       " : MinSurfArea = lValue
                    Case "NORMAL_FLOW_LIMITED" : NormalFlowLimited = lValue
                    Case "SKIP_STEADY_STATE  " : SkipSteadyState = lValue
                    Case "IGNORE_RAINFALL    " : IgnoreRainfall = lValue
                    Case "FORCE_MAIN_EQUATION" : ForceMainEquation = lValue
                    Case "LINK_OFFSETS       " : LinkOffsets = lValue
                    Case "MIN_SLOPE          " : MinSlope = lValue
                    Case Else
                        Logger.Dbg("Option '" & lOption & "' is unknown")
                End Select
            End If
        Next

        SJDate = ParseDateTime(lStartDateString, lStartTimeString)
        EJDate = ParseDateTime(lEndDateString, lEndTimeString)
    End Sub

    Private Function ParseDateTime(ByVal aDate As String, ByVal aTime As String) As Double
        Dim lDateArray(6) As Integer
        Dim lMDY() As String
        lMDY = aDate.Split("/")
        If lMDY.Length = 3 Then
            lDateArray(1) = lMDY(0)
            lDateArray(2) = lMDY(1)
            lDateArray(0) = lMDY(2)
        End If

        Dim lHMS() As String
        lHMS = aTime.Split(":")
        If lHMS.Length = 3 Then
            lDateArray(3) = lHMS(0)
            lDateArray(4) = lHMS(1)
            lDateArray(5) = lHMS(2)
        End If
        Return Date2J(lDateArray)
    End Function

    Private Function FormatDate(ByVal aJulianDate As Double) As String
        Dim lDateArray(6) As Integer
        J2Date(aJulianDate, lDateArray)
        Return lDateArray(1) & "/" & lDateArray(2) & "/" & lDateArray(0)
    End Function

    Private Function FormatTime(ByVal aJulianDate As Double) As String
        Dim lDateArray(6) As Integer
        J2Date(aJulianDate, lDateArray)
        Return Format(lDateArray(3), "00") & ":" & Format(lDateArray(4), "00") & ":" & Format(lDateArray(5), "00")
    End Function

    Public Overrides Function ToString() As String
        Dim lStartDateString As String = FormatDate(SJDate)
        Dim lStartTimeString As String = FormatTime(SJDate)
        Dim lEndDateString As String = FormatDate(EJDate)
        Dim lEndTimeString As String = FormatTime(EJDate)

        Return "[OPTIONS]" & vbCrLf _
            & "FLOW_UNITS           " & FlowUnits & vbCrLf _
            & "INFILTRATION         " & InfiltrationMethod & vbCrLf _
            & "FLOW_ROUTING         " & FlowRouting & vbCrLf _
            & "START_DATE           " & lStartDateString & vbCrLf _
            & "START_TIME           " & lStartTimeString & vbCrLf _
            & "REPORT_START_DATE    " & lStartDateString & vbCrLf _
            & "REPORT_START_TIME    " & lStartTimeString & vbCrLf _
            & "END_DATE             " & lEndDateString & vbCrLf _
            & "END_TIME             " & lEndTimeString & vbCrLf _
            & "SWEEP_START          " & SweepStart & vbCrLf _
            & "SWEEP_END            " & SweepEnd & vbCrLf _
            & "DRY_DAYS             " & DryDays & vbCrLf _
            & "REPORT_STEP          " & ReportStep & vbCrLf _
            & "WET_STEP             " & WetStep & vbCrLf _
            & "DRY_STEP             " & DryStep & vbCrLf _
            & "ROUTING_STEP         " & RoutingStep & vbCrLf _
            & "ALLOW_PONDING        " & AllowPonding & vbCrLf _
            & "INERTIAL_DAMPING     " & InertialDamping & vbCrLf _
            & "VARIABLE_STEP        " & VariableStep & vbCrLf _
            & "LENGTHENING_STEP     " & LengtheningStep & vbCrLf _
            & "MIN_SURFAREA         " & MinSurfArea & vbCrLf _
            & "NORMAL_FLOW_LIMITED  " & NormalFlowLimited & vbCrLf _
            & "SKIP_STEADY_STATE    " & SkipSteadyState & vbCrLf _
            & "IGNORE_RAINFALL      " & IgnoreRainfall & vbCrLf _
            & "FORCE_MAIN_EQUATION  " & ForceMainEquation & vbCrLf _
            & "LINK_OFFSETS         " & LinkOffsets & vbCrLf _
            & "MIN_SLOPE            " & MinSlope & vbCrLf
    End Function
End Class
