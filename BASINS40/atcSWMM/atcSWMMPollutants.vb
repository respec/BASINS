Imports System.Collections.ObjectModel
Imports System.IO
Imports MapWinUtility
Imports atcUtility
Imports System.Text
Imports System.Text.RegularExpressions

Public Class atcSWMMPollutants
    Inherits KeyedCollection(Of String, atcSWMMPollutant)
    Implements IBlock

    Private pName As String
    Private pSWMMProject As atcSWMMProject

    Property Name() As String Implements IBlock.Name
        Get
            Return pName
        End Get
        Set(ByVal value As String)
            pName = value
        End Set
    End Property

    Protected Overrides Function GetKeyForItem(ByVal aPollutant As atcSWMMPollutant) As String
        Dim lKey As String = aPollutant.Name
        Return lKey
    End Function

    Public Sub New(ByVal aSWMMPRoject As atcSWMMProject)
        Name = "[POLLUTANTS]"
        pSWMMProject = aSWMMPRoject
    End Sub

    Public Sub AddRange(ByVal aEnumerable As IEnumerable)
        For Each lPollutant As atcSWMMPollutant In aEnumerable
            Me.Add(lPollutant)
        Next
    End Sub

    Public Sub FromString(ByVal aContents As String) Implements IBlock.FromString
        'Break it up into multiple lines
        Dim lLines() As String = aContents.Split(vbCrLf)
        Dim lSectionName As String = lLines(0)
        For I As Integer = 1 To lLines.Length - 1
            If Not lLines(I).StartsWith(";") And lLines(I).Length > 2 Then
                Dim lPollutant As New atcSWMMPollutant
                Dim lItems As Generic.List(Of String) = atcSWMMProject.SplitSpaceDelimitedWithQuotes(lLines(I).Trim())
                For J As Integer = 0 To lItems.Count - 1
                    Select Case J
                        Case 0 : lPollutant.Name = lItems(J)
                        Case 1 : lPollutant.MassUnits = lItems(J)
                        Case 2 : lPollutant.RainConcentration = Double.Parse(lItems(J))
                        Case 3 : lPollutant.GWConcentration = Double.Parse(lItems(J))
                        Case 4 : lPollutant.IIConcentration = Double.Parse(lItems(J))
                        Case 5 : lPollutant.DecayCoefficient = Double.Parse(lItems(J))
                        Case 6 : lPollutant.SnowOnly = lItems(J)
                        Case 7 : lPollutant.CoPollutName = lItems(J)
                        Case 8 : lPollutant.CoPollutFrac = Double.Parse(lItems(J))
                        Case 9 : lPollutant.DWFConcentration = Double.Parse(lItems(J))
                    End Select
                Next
                Me.Add(lPollutant)
                Me.ChangeItemKey(lPollutant, lItems(0).Trim())
            End If
        Next
    End Sub

    Public Overrides Function ToString() As String
        Dim lSB As New StringBuilder

        lSB.Append(Name & vbCrLf & _
";;               Mass   Rain       GW         I&I        Decay      Snow  Co-Pollut.       Co-Pollut. DWF       " & vbCrLf & _
";;Name           Units  Concen.    Concen.    Concen.    Coeff.     Only  Name             Fraction   Concen.   " & vbCrLf & _
";;-------------- ------ ---------- ---------- ---------- ---------- ----- ---------------- ---------- ----------" & vbCrLf)

        For Each lPollutant As atcSWMMPollutant In Me
            With lPollutant
                lSB.Append(StrPad(.Name, 16, " ", False))
                lSB.Append(" ")
                lSB.Append(StrPad(.MassUnits, 6, " ", False))
                lSB.Append(" ")
                lSB.Append(StrPad(Format(.RainConcentration, "0.0"), 10, " ", False))
                lSB.Append(" ")
                lSB.Append(StrPad(Format(.GWConcentration, "0.0"), 10, " ", False))
                lSB.Append(" ")
                lSB.Append(StrPad(Format(.IIConcentration, "0.0"), 10, " ", False))
                lSB.Append(" ")
                lSB.Append(StrPad(Format(.DecayCoefficient, "0.0"), 10, " ", False))
                lSB.Append(" ")
                lSB.Append(StrPad(.SnowOnly, 5, " ", False))
                lSB.Append(" ")
                lSB.Append(StrPad(.CoPollutName, 16, " ", False))
                lSB.Append(" ")
                lSB.Append(StrPad(Format(.CoPollutFrac, "0.0"), 10, " ", False))
                lSB.Append(" ")
                lSB.Append(StrPad(Format(.DWFConcentration, "0.0"), 10, " ", False))
                lSB.Append(vbCrLf)
            End With
        Next

        Return lSB.ToString
    End Function
End Class

Public Class atcSWMMPollutant
    Public Name As String
    Public MassUnits As String = String.Empty
    Public RainConcentration As Double = 0.0
    Public GWConcentration As Double = 0.0
    Public IIConcentration As Double = 0.0
    Public DecayCoefficient As Double = 0.0
    Public SnowOnly As String = "NO"
    Public CoPollutName As String = "*"
    Public CoPollutFrac As Double = 0.0
    Public DWFConcentration As Double = 0.0
End Class

