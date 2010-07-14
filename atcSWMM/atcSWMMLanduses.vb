Imports System.Collections.ObjectModel
Imports System.IO
Imports MapWinUtility
Imports atcUtility
Imports System.Text

Public Class atcSWMMLanduses
    Inherits KeyedCollection(Of String, atcSWMMLanduse)
    Implements IBlock

    Private pName As String = "[LANDUSES]"

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
        'TODO: fill this in
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
End Class

Public Class atcSWMMLanduse
    Public Name As String
    Public Area As Double
    Public Catchment As atcSWMMCatchment
End Class
