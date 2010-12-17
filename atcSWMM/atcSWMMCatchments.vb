Imports System.Collections.ObjectModel
Imports System.IO
Imports MapWinUtility
Imports atcUtility
Imports System.Text
Imports System.Text.RegularExpressions

Public Class atcSWMMCatchments
    Inherits KeyedCollection(Of String, atcSWMMCatchment)
    Implements IBlock

    Private pName As String = "[SUBCATCHMENTS]"
    Private pSWMMProject As atcSWMMProject

    Property Name() As String Implements IBlock.Name
        Get
            Return pName
        End Get
        Set(ByVal value As String)
            pName = value
        End Set
    End Property

    Protected Overrides Function GetKeyForItem(ByVal aCatchment As atcSWMMCatchment) As String
        Dim lKey As String = aCatchment.Name
        Return lKey
    End Function

    Public Sub New(ByVal aSWMMPRoject As atcSWMMProject)
        pSWMMProject = aSWMMPRoject
    End Sub

    Public Sub AddRange(ByVal aEnumerable As IEnumerable)
        For Each lCatchment As atcSWMMCatchment In aEnumerable
            Me.Add(lCatchment)
        Next
    End Sub

    Public Sub FromString(ByVal aContents As String) Implements IBlock.FromString
        'Break it up into multiple lines
        Dim lLines() As String = aContents.Split(vbCrLf)
        Dim lSectionName As String = lLines(0)
        For I As Integer = 1 To lLines.Length - 1
            If Not lLines(I).StartsWith(";") And lLines(I).Length > 2 Then
                Dim lItems() As String = Regex.Split(lLines(I).Trim(), "\s+")
                Dim lSub As atcSWMMCatchment = Me(lItems(0))
                If lSub Is Nothing Then
                    lSub = New atcSWMMCatchment
                End If
                For J As Integer = 0 To lItems.Length - 1
                    Select Case J
                        Case 0 : lSub.Name = lItems(J)
                        Case 1
                            If lSectionName = "[SUBCATCHMENTS]" Then
                                lSub.RainGage = pSWMMProject.RainGages(lItems(J))
                            ElseIf lSectionName = "[SUBAREAS]" Then
                                lSub.ManningsNImperv = Double.Parse(lItems(J))
                            ElseIf lSectionName = "[INFILTRATION]" Then
                                lSub.MaxInfiltRate = Double.Parse(lItems(J))
                            ElseIf lSectionName = "[COVERAGES]" Then
                                If Not lSub.Landuses.ContainsKey(lItems(J)) Then
                                    lSub.Landuses.Add(lItems(J), 0.0)
                                End If
                            ElseIf lSectionName = "[LOADINGS]" Then
                                If Not lSub.PollutantLoads.ContainsKey(lItems(J)) Then
                                    lSub.PollutantLoads.Add(lItems(J), 0.0)
                                End If
                            End If
                        Case 2
                            If lSectionName = "[SUBCATCHMENTS]" Then
                                lSub.OutletNode = pSWMMProject.Nodes(lItems(J))
                            ElseIf lSectionName = "[SUBAREAS]" Then
                                lSub.ManningsNPerv = Double.Parse(lItems(J))
                            ElseIf lSectionName = "[INFILTRATION]" Then
                                lSub.MinInfiltRate = Double.Parse(lItems(J))
                            ElseIf lSectionName = "[COVERAGES]" Then
                                lSub.Landuses(lItems(1)) = Double.Parse(lItems(J))
                            ElseIf lSectionName = "[LOADINGS]" Then
                                lSub.PollutantLoads(lItems(1)) = Double.Parse(lItems(J))
                            End If
                        Case 3
                            If lSectionName = "[SUBCATCHMENTS]" Then
                                lSub.Area = Double.Parse(lItems(J))
                            ElseIf lSectionName = "[SUBAREAS]" Then
                                lSub.SlopeImperv = Double.Parse(lItems(J))
                            ElseIf lSectionName = "[INFILTRATION]" Then
                                lSub.DecayRateConstant = Double.Parse(lItems(J))
                            End If
                        Case 4
                            If lSectionName = "[SUBCATCHMENTS]" Then
                                lSub.DepressionStorageImperv = Double.Parse(lItems(J))
                            ElseIf lSectionName = "[SUBAREAS]" Then
                                lSub.SlopePerv = Double.Parse(lItems(J))
                            ElseIf lSectionName = "[INFILTRATION]" Then
                                lSub.DryTime = Double.Parse(lItems(J))
                            End If
                        Case 5
                            If lSectionName = "[SUBCATCHMENTS]" Then
                                lSub.Width = Double.Parse(lItems(J))
                            ElseIf lSectionName = "[SUBAREAS]" Then
                                lSub.PercentZeroStorage = Double.Parse(lItems(J))
                            ElseIf lSectionName = "[INFILTRATION]" Then
                                lSub.MaxInfiltRate = Double.Parse(lItems(J))
                            End If
                        Case 6
                            If lSectionName = "[SUBCATCHMENTS]" Then
                                lSub.Slope = Double.Parse(lItems(J))
                            ElseIf lSectionName = "[SUBAREAS]" Then
                                lSub.OutletNodeID = lItems(J) 'Route To??
                            End If
                        Case 7
                            If lSectionName = "[SUBCATCHMENTS]" Then
                                lSub.CurbLength = Double.Parse(lItems(J))
                            ElseIf lSectionName = "[SUBAREAS]" Then
                                lSub.PercentRouted = Double.Parse(lItems(J))
                            End If
                        Case 8 : lSub.SnowPackName = lItems(J)
                    End Select
                Next
                If Not Me.Contains(lItems(0).Trim()) Then
                    Me.Add(lSub)
                    Me.ChangeItemKey(lSub, lItems(0).Trim())
                End If
            End If
        Next
    End Sub

    Public Overrides Function ToString() As String
        Dim lSB As New StringBuilder

        lSB.Append(Name & vbCrLf & _
                       ";;                                                 Total    Pcnt.             Pcnt.    Curb     Snow    " & vbCrLf & _
                       ";;Name           Raingage         Outlet           Area     Imperv   Width    Slope    Length   Pack    " & vbCrLf & _
                       ";;-------------- ---------------- ---------------- -------- -------- -------- -------- -------- --------" & vbCrLf)

        For Each lCatchment As atcSWMMCatchment In Me
            With lCatchment
                lSB.Append(StrPad(.Name, 16, " ", False))
                lSB.Append(" ")
                If Not .RainGage Is Nothing Then
                    lSB.Append(StrPad(.RainGage.Name, 16, " ", False))
                Else
                    lSB.Append(StrPad("R1", 16, " ", False))
                End If
                lSB.Append(" ")
                If Not .OutletNode Is Nothing Then
                    lSB.Append(StrPad(.OutletNode.Name, 16, " ", False))
                Else
                    lSB.Append(StrPad("J1", 16, " ", False))
                End If
                lSB.Append(" ")
                lSB.Append(StrPad(Format(.Area, "0.0"), 8, " ", False))
                lSB.Append(" ")
                lSB.Append(StrPad(Format(.PercentImpervious, "0.0"), 8, " ", False))
                lSB.Append(" ")
                lSB.Append(StrPad(Format(.Width, "0.0"), 8, " ", False))
                lSB.Append(" ")
                lSB.Append(StrPad(Format(.Slope, "0.0"), 8, " ", False))
                lSB.Append(" ")
                lSB.Append(StrPad(Format(.CurbLength, "0.0"), 8, " ", False))
                lSB.Append(" ")
                lSB.Append(.SnowPackName)
                lSB.Append(vbCrLf)
            End With
        Next

        If Not pSWMMProject.Blocks.Contains("[SUBAREAS]") Then
            lSB.Append(SubareasToString)
        End If
        If Not pSWMMProject.Blocks.Contains("[INFILTRATION]") Then
            lSB.Append(InfiltrationToString)
        End If

        Return lSB.ToString
    End Function

    Public Sub SubareaFromString(ByVal aContents As String)
        'Dim lSubcatchmentName As String 'Parse this from aContents
        'Dim lCatchment As atcSWMMCatchment = Me(lSubcatchmentName)
        'With lCatchment
        '    '.ManningsNImperv=
        '    '.ManningsNPerv=
        '    '.DepressionStorageImperv=
        '    '.PercentZeroStorage=
        '    '.RouteTo=
        '    '.PercentRouted=
        'End With
    End Sub

    Public Function SubareasToString() As String
        Dim lSB As New StringBuilder

        lSB.Append("[SUBAREAS]" & vbCrLf & _
                   ";;Subcatchment   N-Imperv   N-Perv     S-Imperv   S-Perv     PctZero    RouteTo    PctRouted " & vbCrLf & _
                   ";;-------------- ---------- ---------- ---------- ---------- ---------- ---------- ----------" & vbCrLf)

        For Each lCatchment As atcSWMMCatchment In Me
            With lCatchment
                lSB.Append(StrPad(.Name, 16, " ", False))
                lSB.Append(" ")
                lSB.Append(StrPad(Format(.ManningsNImperv, "0.00"), 10, " ", False))
                lSB.Append(" ")
                lSB.Append(StrPad(Format(.ManningsNPerv, "0.00"), 10, " ", False))
                lSB.Append(" ")
                lSB.Append(StrPad(Format(.DepressionStorageImperv, "0.00"), 10, " ", False))
                lSB.Append(" ")
                lSB.Append(StrPad(Format(.DepressionStoragePerv, "0.00"), 10, " ", False))
                lSB.Append(" ")
                lSB.Append(StrPad(Format(.PercentZeroStorage, "0.0"), 10, " ", False))
                lSB.Append(" ")
                lSB.Append(StrPad(.RouteTo, 10, " ", False))
                lSB.Append(" ")
                If .PercentRouted < 100.0 Then
                    lSB.Append(StrPad(Format(.PercentRouted, "0.0"), 10, " ", False))
                Else
                    lSB.Append("          ")
                End If
                lSB.Append(vbCrLf)
            End With
        Next

        Return lSB.ToString
    End Function

    Public Sub InfiltrationFromString(ByVal aContents As String)
        'Dim lSubcatchmentName As String 'Parse this from aContents
        'Dim lCatchment As atcSWMMCatchment = Me(lSubcatchmentName)
        'With lCatchment
        '    'Three different methods set different fields
        '    'HORTON:
        '    '.MaxInfiltRate=
        '    '.MinInfiltRate=
        '    '.DecayRateConstant=
        '    '.DryTime=
        '    '.MaxInfiltVolume=
        '    'GREEN_AMPT
        '    'Suction=
        '    'Conductivity
        '    'InitialDeficit
        '    'CURVE_NUMBER
        '    '.CurveNumber=
        '    '.Conductivity
        '    '.DryTime
        'End With
    End Sub

    Public Function InfiltrationToString() As String
        Dim lSB As New StringBuilder

        If pSWMMProject.Options.InfiltrationMethod = "HORTON" Then

            lSB.Append("[INFILTRATION]" & vbCrLf & _
                       ";;Subcatchment   MaxRate    MinRate    Decay      DryTime    MaxInfil  " & vbCrLf & _
                       ";;-------------- ---------- ---------- ---------- ---------- ----------" & vbCrLf)

            For Each lCatchment As atcSWMMCatchment In Me
                With lCatchment
                    lSB.Append(StrPad(.Name, 16, " ", False))
                    lSB.Append(" ")
                    lSB.Append(StrPad(Format(.MaxInfiltRate, "0.00"), 10, " ", False))
                    lSB.Append(" ")
                    lSB.Append(StrPad(Format(.MinInfiltRate, "0.00"), 10, " ", False))
                    lSB.Append(" ")
                    lSB.Append(StrPad(Format(.DecayRateConstant, "0.00"), 10, " ", False))
                    lSB.Append(" ")
                    lSB.Append(StrPad(Format(.DryTime, "0.00"), 10, " ", False))
                    lSB.Append(" ")
                    lSB.Append(StrPad(Format(.MaxInfiltVolume, "0.00"), 10, " ", False))
                    lSB.Append(vbCrLf)
                End With
            Next

        ElseIf pSWMMProject.Options.InfiltrationMethod = "GREEN_AMPT" Then

            lSB.Append("[INFILTRATION]" & vbCrLf & _
                       ";;Subcatchment   Suction    HydCon     IMDmax    " & vbCrLf & _
                       ";;-------------- ---------- ---------- ----------" & vbCrLf)

            For Each lCatchment As atcSWMMCatchment In Me
                With lCatchment
                    lSB.Append(StrPad(.Name, 16, " ", False))
                    lSB.Append(" ")
                    lSB.Append(StrPad(Format(.Suction, "0.00"), 10, " ", False))
                    lSB.Append(" ")
                    lSB.Append(StrPad(Format(.Conductivity, "0.00"), 10, " ", False))
                    lSB.Append(" ")
                    lSB.Append(StrPad(Format(.InitialDeficit, "0.00"), 10, " ", False))
                    lSB.Append(vbCrLf)
                End With
            Next

        ElseIf pSWMMProject.Options.InfiltrationMethod = "CURVE_NUMBER" Then

            lSB.Append("[INFILTRATION]" & vbCrLf & _
                       ";;Subcatchment   CurveNum   HydCon     DryTime   " & vbCrLf & _
                       ";;-------------- ---------- ---------- ----------" & vbCrLf)

            For Each lCatchment As atcSWMMCatchment In Me
                With lCatchment
                    lSB.Append(StrPad(.Name, 16, " ", False))
                    lSB.Append(" ")
                    lSB.Append(StrPad(Format(.CurveNumber, "0.00"), 10, " ", False))
                    lSB.Append(" ")
                    lSB.Append(StrPad(Format(.Conductivity, "0.00"), 10, " ", False))
                    lSB.Append(" ")
                    lSB.Append(StrPad(Format(.DryTime, "0.00"), 10, " ", False))
                    lSB.Append(vbCrLf)
                End With
            Next

        End If
        Return lSB.ToString
    End Function

    Public Sub PolygonsFromString(ByVal aContents As String)
        'Dim lSubcatchmentName As String 'Parse this from aContents
        'Dim lCatchment As atcSWMMCatchment = Me(lSubcatchmentName)
        'With lCatchment
        '    '.X(lIndex)=
        '    '.Y(lIndex)=
        'End With
    End Sub

    Public Function PolygonsToString() As String
        Dim lSB As New StringBuilder
        lSB.Append("[Polygons]" & vbCrLf & _
                       ";;Subcatchment   X-Coord            Y-Coord           " & vbCrLf & _
                       ";;-------------- ------------------ ------------------" & vbCrLf)

        For Each lCatchment As atcSWMMCatchment In Me
            With lCatchment
                For lIndex As Integer = 0 To .X.GetUpperBound(0) - 1
                    lSB.Append(StrPad(.Name, 16, " ", False))
                    lSB.Append(" ")
                    lSB.Append(StrPad(Format(.X(lIndex), "0.000"), 18, " ", False))
                    lSB.Append(" ")
                    lSB.Append(StrPad(Format(.Y(lIndex), "0.000"), 18, " ", False))
                    lSB.Append(vbCrLf)
                Next
            End With
        Next

        Return lSB.ToString
    End Function
End Class

Public Class atcSWMMCatchment
    Public Name As String
    Public RainGage As atcSWMMRainGage
    Public OutletNode As atcSWMMNode
    Public OutletNodeID As String
    Public Area As Double = -1.0 'in acres or hectares
    Public PercentImpervious As Double = -1.0
    Public Width As Double = -1.0 'in feet or meters
    Public Slope As Double = -1.0 'percent
    Public CurbLength As Double = -1.0
    Public SnowPackName As String = "" 'blank if none

    Public ManningsNImperv As Double = -1.0
    Public ManningsNPerv As Double = -1.0
    Public DepressionStorageImperv As Double = -1.0 'inches or mm
    Public DepressionStoragePerv As Double = -1.0 'inches or mm
    Public PercentZeroStorage As Double = -1.0
    Public RouteTo As String = ""
    Public PercentRouted As Double = -1.0

    Public MaxInfiltRate As Double = -1.0 'inches/hr or mm/hr
    Public MinInfiltRate As Double = -1.0 'inches/hr or mm/hr
    Public DecayRateConstant As Double = -1.0
    Public DryTime As Double = -1.0 'days (or 4 days if using curve number)
    Public MaxInfiltVolume As Double = -1.0 'inches or mm
    Public Suction As Double = -1.0 'inches or mm              'used if Infiltration Method is "GREEN_AMPT"
    Public Conductivity As Double = -1.0 'inches/hr or mm/hr   'used if Infiltration Method is "GREEN_AMPT" or "CURVE_NUMBER"
    Public InitialDeficit As Double = -1.0                     'used if Infiltration Method is "GREEN_AMPT"
    Public CurveNumber As Double = -1.0                        'used if Infiltration Method is "CURVE_NUMBER"

    Public SlopeImperv As Double = 0.05
    Public SlopePerv As Double = 0.05
    Public Landuses As New Dictionary(Of String, Double)
    Public PollutantLoads As New Dictionary(Of String, Double)
    Public X() As Double
    Public Y() As Double
End Class





