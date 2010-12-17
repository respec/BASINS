Imports System.Collections.ObjectModel
Imports System.IO
Imports MapWinUtility
Imports atcUtility
Imports System.Text
Imports System.Text.RegularExpressions

Public Class atcSWMMLanduses
    Inherits KeyedCollection(Of String, atcSWMMLanduse)
    Implements IBlock

    Private pName As String = "[LANDUSES]"
    Private pSWMMProject As atcSWMMProject

    Property Name() As String Implements IBlock.Name
        Get
            Return pName
        End Get
        Set(ByVal value As String)
            pName = value
        End Set
    End Property

    Protected Overrides Function GetKeyForItem(ByVal aLanduse As atcSWMMLanduse) As String
        Dim lKey As String = aLanduse.Name & ":" & aLanduse.Catchment.Name
        Return lKey
    End Function

    Public Sub FromString(ByVal aContents As String) Implements IBlock.FromString
        'Break it up into multiple lines
        Dim lLines() As String = aContents.Split(vbCrLf)
        Dim lSectionName As String = lLines(0)
        For I As Integer = 1 To lLines.Length - 1
            If Not lLines(I).StartsWith(";") And lLines(I).Length > 2 Then
                Dim lItems() As String = Regex.Split(lLines(I).Trim(), "\s+")
                Dim landuse As atcSWMMLanduse = Me(lItems(0))
                If landuse Is Nothing Then
                    landuse = New atcSWMMLanduse
                End If
                For J As Integer = 0 To lItems.Length - 1
                    Select Case J
                        Case 0 : landuse.Name = lItems(J)
                        Case 1
                            If lSectionName = "[LANDUSES]" Then
                                landuse.CleanInterv = Double.Parse(lItems(J))
                            ElseIf lSectionName = "[BUILDUP]" Then
                                If Not landuse.PollutBuildup.ContainsKey(lItems(J)) Then
                                    landuse.PollutBuildup.Add(lItems(J), New atcSWMMPollutBuildup)
                                End If
                            ElseIf lSectionName = "[WASHOFF]" Then
                                If Not landuse.PollutWashoff.ContainsKey(lItems(J)) Then
                                    landuse.PollutWashoff.Add(lItems(J), New atcSWMMPollutWashoff)
                                End If
                            End If
                        Case 2
                            If lSectionName = "[LANDUSES]" Then
                                landuse.FracAvail = Double.Parse(lItems(J))
                            ElseIf lSectionName = "[BUILDUP]" Then
                                landuse.PollutBuildup(lItems(1)).Func = lItems(J)
                            ElseIf lSectionName = "[WASHOFF]" Then
                                landuse.PollutWashoff(lItems(1)).Func = lItems(J)
                            End If
                        Case 3
                            If lSectionName = "[LANDUSES]" Then
                                landuse.LasCleaned = Double.Parse(lItems(J))
                            ElseIf lSectionName = "[BUILDUP]" Then
                                landuse.PollutBuildup(lItems(1)).Coeff1 = Double.Parse(lItems(J))
                            ElseIf lSectionName = "[WASHOFF]" Then
                                landuse.PollutWashoff(lItems(1)).Coeff1 = Double.Parse(lItems(J))
                            End If
                        Case 4
                            If lSectionName = "[LANDUSES]" Then
                                'nothing
                            ElseIf lSectionName = "[BUILDUP]" Then
                                landuse.PollutBuildup(lItems(1)).Coeff2 = Double.Parse(lItems(J))
                            ElseIf lSectionName = "[WASHOFF]" Then
                                landuse.PollutWashoff(lItems(1)).Coeff2 = Double.Parse(lItems(J))
                            End If
                        Case 5
                            If lSectionName = "[LANDUSES]" Then
                                'nothing
                            ElseIf lSectionName = "[BUILDUP]" Then
                                landuse.PollutBuildup(lItems(1)).Coeff3 = Double.Parse(lItems(J))
                            ElseIf lSectionName = "[WASHOFF]" Then
                                landuse.PollutWashoff(lItems(1)).CleanEffic = Double.Parse(lItems(J))
                            End If
                        Case 6
                            If lSectionName = "[BUILDUP]" Then
                                landuse.PollutBuildup(lItems(1)).Normalizer = lItems(J)
                            ElseIf lSectionName = "[WASHOFF]" Then
                                landuse.PollutWashoff(lItems(1)).BMPEffic = Double.Parse(lItems(J))
                            End If
                    End Select
                Next
                'TODO:
                'Need to check all other types to see if add too many like below
                If Not Me.Contains(lItems(0)) Then
                    Me.Add(landuse)
                    Me.ChangeItemKey(landuse, lItems(0).Trim())
                End If
            End If
        Next
    End Sub

    Public Overrides Function ToString() As String
        Dim lSB As New StringBuilder

        lSB.Append(pName & vbCrLf & _
                       ";;               Cleaning   Fraction   Last      " & vbCrLf & _
                       ";;Name           Interval   Available  Cleaned   " & vbCrLf & _
                       ";;-------------- ---------- ---------- ----------" & vbCrLf)

        'need collection of unique landuses
        Dim lUniqueLanduseNames As New atcCollection
        For Each lLanduse As atcSWMMLanduse In Me
            Dim lLanduseName As String = lLanduse.Name
            If IsNumeric(lLanduse.Name) Then
                lLanduseName = "Landuse_" & lLanduse.Name
            End If
            If Not lUniqueLanduseNames.Contains(lLanduseName) Then
                lUniqueLanduseNames.Add(lLanduseName)
            End If
        Next

        For Each lUniqueLanduseName As String In lUniqueLanduseNames
            lSB.Append(StrPad(lUniqueLanduseName, 16, " ", False))
            lSB.Append(" ")
            lSB.Append(vbCrLf)
        Next

        If Not pSWMMProject.Blocks.Contains("[COVERAGES]") Then
            lSB.Append(CoveragesToString)
        End If

        Return lSB.ToString
    End Function

    Public Function CoveragesToString() As String
        Dim lSB As New StringBuilder

        lSB.Append("[COVERAGES]" & vbCrLf & _
                   ";;Subcatchment   Land Use         Percent   " & vbCrLf & _
                   ";;-------------- ---------------- ----------" & vbCrLf)

        'need collection of unique catchments
        Dim lUniqueCatchmentNames As New atcCollection
        For Each lLanduse As atcSWMMLanduse In Me
            Dim lCatchmentName As String = lLanduse.Catchment.Name
            If Not lUniqueCatchmentNames.Contains(lCatchmentName) Then
                lUniqueCatchmentNames.Add(lCatchmentName)
            End If
        Next

        For Each lUniqueCatchment As String In lUniqueCatchmentNames
            Dim lTotalCatchmentArea As Double = 0.0
            For Each lLanduse As atcSWMMLanduse In Me
                If lLanduse.Catchment.Name = lUniqueCatchment Then
                    'a match, store areas of pervious and impervious
                    lTotalCatchmentArea += lLanduse.Area
                End If
            Next

            For Each lLanduse As atcSWMMLanduse In Me
                If lLanduse.Catchment.Name = lUniqueCatchment Then
                    'a match, write record
                    lSB.Append(StrPad(lLanduse.Catchment.Name, 16, " ", False))
                    lSB.Append(" ")
                    Dim lLanduseName As String = lLanduse.Name
                    If IsNumeric(lLanduse.Name) Then
                        lLanduseName = "Landuse_" & lLanduse.Name
                    End If
                    lSB.Append(StrPad(lLanduseName, 16, " ", False))
                    lSB.Append(" ")
                    Dim lArea As Double = 100.0 * lLanduse.Area / lTotalCatchmentArea
                    lSB.Append(StrPad(Format(lArea, "0.0"), 10, " ", False))
                    lSB.Append(" ")
                    lSB.Append(vbCrLf)
                End If
            Next
        Next

        Return lSB.ToString
    End Function
 
    Public Sub New(ByVal aSWMMPRoject As atcSWMMProject)
        pSWMMProject = aSWMMPRoject
    End Sub
End Class

Public Class atcSWMMLanduse
    Public Name As String
    Public Area As Double
    Public CleanInterv As Double 'not sure about data type
    Public FracAvail As Double 'not sure about data type
    Public LasCleaned As Double 'not sure about data type
    Public PollutBuildup As New Dictionary(Of String, atcSWMMPollutBuildup)
    Public PollutWashoff As New Dictionary(Of String, atcSWMMPollutWashoff)
    Public Catchment As atcSWMMCatchment
End Class

Public Class atcSWMMPollutBuildup
    Public Func As String
    Public Coeff1 As Double
    Public Coeff2 As Double
    Public Coeff3 As Double
    Public Normalizer As String
End Class

Public Class atcSWMMPollutWashoff
    Public Func As String
    Public Coeff1 As Double
    Public Coeff2 As Double
    Public CleanEffic As Double
    Public BMPEffic As Double
End Class