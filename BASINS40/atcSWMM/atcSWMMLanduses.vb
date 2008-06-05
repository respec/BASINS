Imports System.Collections.ObjectModel
Imports System.IO
Imports MapWinUtility
Imports atcUtility
Imports System.Text

Public Class Landuses
    Inherits KeyedCollection(Of String, Landuse)
    Protected Overrides Function GetKeyForItem(ByVal aLanduse As Landuse) As String
        Dim lKey As String = aLanduse.Name & ":" & aLanduse.Catchment.Name
        Return lKey
    End Function

    Public Overrides Function ToString() As String
        Dim lSB As New StringBuilder

        lSB.Append("[LANDUSES]" & vbCrLf & _
                       ";;               Cleaning   Fraction   Last      " & vbCrLf & _
                       ";;Name           Interval   Available  Cleaned   " & vbCrLf & _
                       ";;-------------- ---------- ---------- ----------" & vbCrLf)

        'need collection of unique landuses
        Dim lUniqueLanduseNames As New atcCollection
        For Each lLanduse As Landuse In Me
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
        For Each lLanduse As Landuse In Me
            Dim lCatchmentName As String = lLanduse.Catchment.Name
            If Not lUniqueCatchmentNames.Contains(lCatchmentName) Then
                lUniqueCatchmentNames.Add(lCatchmentName)
            End If
        Next

        For Each lUniqueCatchment As String In lUniqueCatchmentNames
            Dim lTotalCatchmentArea As Double = 0.0
            For Each lLanduse As Landuse In Me
                If lLanduse.Catchment.Name = lUniqueCatchment Then
                    'a match, store areas of pervious and impervious
                    lTotalCatchmentArea += lLanduse.Area
                End If
            Next

            For Each lLanduse As Landuse In Me
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

Public Class Landuse
    Public Name As String
    Public Area As Double
    Public Catchment As Catchment
End Class
