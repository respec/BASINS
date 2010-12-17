Imports System.Collections.ObjectModel
Imports System.IO
Imports MapWinUtility
Imports atcUtility
Imports System.Text
Imports System.Text.RegularExpressions
Imports atcData

Public Class atcSWMMConduits
    Inherits KeyedCollection(Of String, atcSWMMConduit)
    Implements IBlock

    Private pName As String = "[CONDUITS]"
    Private pSWMMProject As atcSWMMProject

    Property Name() As String Implements IBlock.Name
        Get
            Return pName
        End Get
        Set(ByVal value As String)
            pName = value
        End Set
    End Property

    Protected Overrides Function GetKeyForItem(ByVal aConduit As atcSWMMConduit) As String
        Return aConduit.Name
    End Function

    Public Sub New(ByVal aSWMMPRoject As atcSWMMProject)
        pSWMMProject = aSWMMPRoject
    End Sub

    Public Sub AddRange(ByVal aEnumerable As IEnumerable)
        For Each lConduit As atcSWMMConduit In aEnumerable
            Me.Add(lConduit)
        Next
    End Sub

    Public Sub FromString(ByVal aContents As String) Implements IBlock.FromString
        'Break it up into multiple lines
        Dim lLines() As String = aContents.Split(vbCrLf)
        For I As Integer = 1 To lLines.Length - 1
            If Not lLines(I).StartsWith(";") And lLines(I).Length > 2 Then
                Dim lItems() As String = Regex.Split(lLines(I).Trim(), "\s+")
                Dim lConduit As atcSWMMConduit = Me(lItems(0))
                If lConduit Is Nothing Then
                    lConduit = New atcSWMMConduit
                End If
                For J As Integer = 0 To lItems.Length - 1
                    Select Case J
                        Case 0 : lConduit.Name = lItems(J)
                        Case 1
                            If pSWMMProject.Nodes(lItems(J)) Is Nothing Then
                                Dim lnewNode As New atcSWMMNode
                                With lnewNode
                                    .Name = lItems(J).Trim()
                                End With
                                lConduit.InletNode = lnewNode
                            Else
                                lConduit.InletNode = pSWMMProject.Nodes(lItems(J))
                            End If
                        Case 2
                            If pSWMMProject.Nodes(lItems(J)) Is Nothing Then
                                Dim lnewNode As New atcSWMMNode
                                With lnewNode
                                    .Name = lItems(J).Trim()
                                End With
                                lConduit.OutletNode = lnewNode
                            Else
                                lConduit.OutletNode = pSWMMProject.Nodes(lItems(J))
                            End If
                        Case 3
                            Dim lnewDef As New atcData.atcDefinedValue
                            With lnewDef
                                .Definition = New atcData.atcAttributeDefinition
                                .Definition.Name = lItems(0)
                                .Value = Double.Parse(lItems(J))
                            End With
                            lConduit.Length = lnewDef
                        Case 4
                            lConduit.ManningsN = Double.Parse(lItems(J))
                        Case 5
                            lConduit.InletOffset = Double.Parse(lItems(J))
                        Case 6
                            lConduit.OutletOffset = Double.Parse(lItems(J))
                        Case 7
                            lConduit.InitialFlow = Double.Parse(lItems(J))
                        Case 8
                            lConduit.MaxFlow = Double.Parse(lItems(J))
                    End Select
                Next
                If Not Me.Contains(lItems(0)) Then
                    Me.Add(lConduit)
                    Me.ChangeItemKey(lConduit, lItems(0))
                End If
            End If
        Next
    End Sub

    Public Overrides Function ToString() As String
        Dim lString As New StringBuilder
        Dim lConduit As atcSWMMConduit

        lString.Append(Name & vbCrLf & _
                       ";;               Inlet            Outlet                      Manning    Inlet      Outlet     Init.      Max.      " & vbCrLf & _
                       ";;Name           Node             Node             Length     N          Offset     Offset     Flow       Flow      " & vbCrLf)
        If Me.Count > 0 Then
            lConduit = Me.Items(0)
            Dim lIsMetric As Boolean = pSWMMProject.IsMetric
            lString.Append(";;" & Space(15) _
                                & Space(17) _
                                & Space(17) _
                                & lConduit.Length.Definition.Units.PadRight(11) & vbCrLf)
        End If
        lString.Append(";;-------------- ---------------- ---------------- ---------- ---------- ---------- ---------- ---------- ----------" & vbCrLf)

        For Each lConduit In Me
            With lConduit
                lString.Append(StrPad(.Name, 16, " ", False))
                lString.Append(" ")
                If .InletNode Is Nothing Then
                    lString.Append(StrPad(.Name & "S", 16, " ", False))
                Else
                    lString.Append(StrPad(.InletNode.Name, 16, " ", False))
                End If
                lString.Append(" ")
                If .OutletNode Is Nothing Then
                    lString.Append(StrPad(.Name & "T", 16, " ", False))
                Else
                    lString.Append(StrPad(.OutletNode.Name, 16, " ", False))
                End If
                lString.Append(" ")
                lString.Append(StrPad(Format(.Length.Value, "0.0"), 10, " ", False))
                lString.Append(" ")
                lString.Append(StrPad(Format(.ManningsN, "0.00"), 10, " ", False))
                lString.Append(" ")
                lString.Append(StrPad(Format(.InletOffset, "0.0"), 10, " ", False))
                lString.Append(" ")
                lString.Append(StrPad(Format(.OutletOffset, "0.0"), 10, " ", False))
                lString.Append(" ")
                lString.Append(StrPad(Format(.InitialFlow, "0.0"), 10, " ", False))
                lString.Append(" ")
                lString.Append(StrPad(Format(.MaxFlow, "0.0"), 10, " ", False))
                lString.Append(vbCrLf)
            End With
        Next

        lString.Append(vbCrLf)
        lString.Append("[XSECTIONS]" & vbCrLf & _
                       ";;Link           Shape        Geom1            Geom2      Geom3      Geom4      Barrels   " & vbCrLf & _
                       ";;-------------- ------------ ---------------- ---------- ---------- ---------- ----------" & vbCrLf)

        For Each lConduit In Me
            With lConduit
                lString.Append(StrPad(.Name, 16, " ", False))
                lString.Append(" ")
                lString.Append(StrPad(.Shape, 12, " ", False))
                lString.Append(" ")
                lString.Append(StrPad(Format(.Geometry1, "0.0"), 16, " ", False))
                lString.Append(" ")
                lString.Append(StrPad(Format(.Geometry2, "0.0"), 10, " ", False))
                lString.Append(" ")
                lString.Append(StrPad(Format(.Geometry3, "0.0"), 10, " ", False))
                lString.Append(" ")
                lString.Append(StrPad(Format(.Geometry4, "0.0"), 10, " ", False))
                lString.Append(" ")
                lString.Append(StrPad(.NumBarrels, 10, " ", False))
                lString.Append(" ")
                lString.Append(vbCrLf)
            End With
        Next

        Return lString.ToString
    End Function

    Public Sub VerticesFromString(ByVal aContents As String)
        'Dim lSubcatchmentName As String 'Parse this from aContents
        'Dim lConduit As atcSWMMConduit = Me(lSubcatchmentName)
        'With lConduit
        '    '.ManningsNImperv=
        '    '.ManningsNPerv=
        '    '.DepressionStorageImperv=
        '    '.PercentZeroStorage=
        '    '.RouteTo=
        '    '.PercentRouted=
        'End With
    End Sub

    Public Function VerticesToString() As String
        Dim lSB As New StringBuilder
        lSB.Append("[VERTICES]" & vbCrLf & _
                       ";;Link           X-Coord            Y-Coord           " & vbCrLf & _
                       ";;-------------- ------------------ ------------------" & vbCrLf)

        For Each lConduit As atcSWMMConduit In Me
            With lConduit
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

Public Class atcSWMMConduit
    Public Name As String = ""
    Public InletNode As atcSWMMNode
    Public OutletNode As atcSWMMNode
    Public InletNodeName As String = ""
    Public OutletNodeName As String = ""
    Public DownConduitID As String = ""
    Public Length As New atcDefinedValue
    Private Shared LengthDefinition As atcAttributeDefinition
    Public MeanWidth As Double = -1.0
    Public MeanDepth As Double = -1.0
    Public ElevationHigh As Double = -1.0
    Public ElevationLow As Double = -1.0
    Public ManningsN As Double = -1.0
    Public InletOffset As Double = -1.0
    Public OutletOffset As Double = -1.0
    Public InitialFlow As Double = -1.0
    Public MaxFlow As Double = -1.0
    Public Shape As String = ""
    Public Geometry1 As Double = -1 'full height
    Public Geometry2 As Double = -1 'base width
    Public Geometry3 As Double = -1 'left slope
    Public Geometry4 As Double = -1 'right slope
    Public NumBarrels As Integer = -1
    Public X() As Double
    Public Y() As Double
    Private Shared pIsMetric As Boolean

    Public Shared Property IsMetric() As Boolean
        Get
            Return pIsMetric
        End Get
        Set(ByVal aIsMetric As Boolean)
            If LengthDefinition Is Nothing Then
                LengthDefinition = New atcAttributeDefinition
            End If
            With LengthDefinition
                If aIsMetric Then
                    .Units = "meters"
                Else
                    .Units = "feet"
                End If
            End With
            pIsMetric = aIsMetric
        End Set
    End Property

    Public Sub New()
        Length.Definition = LengthDefinition
    End Sub
End Class
