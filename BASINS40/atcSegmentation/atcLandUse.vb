Imports System.Collections.ObjectModel
Imports System.IO
Imports MapWinUtility
Imports MapWinUtility.Strings
Imports atcUtility

Public Class LandUses
    Inherits KeyedCollection(Of String, LandUse)
    Private pWatershed As Watershed
    Protected Overrides Function GetKeyForItem(ByVal aLandUse As LandUse) As String
        'Dim lKey As String = aLandUse.Description & ":" & aLandUse.Type & ":" & aLandUse.Reach.Id
        Dim lKey As String = aLandUse.Description & ":" & aLandUse.Reach.Id
        Return lKey
    End Function

    Friend Function Open(ByVal aWatershed As Watershed) As Integer
        pWatershed = aWatershed
        'read wsd file
        Dim lReturnCode As Integer = 0
        Dim lName As String = pWatershed.Name & ".wsd"
        Try
            Dim lDelim As String = " "
            Dim lQuote As String = """"
            Dim lCurrentRecord As String
            Dim lStreamReader As New StreamReader(lName)
            lCurrentRecord = lStreamReader.ReadLine 'first line is header
            Do
                lCurrentRecord = lStreamReader.ReadLine
                If lCurrentRecord Is Nothing Then
                    Exit Do
                Else
                    'count the number of fields in this string
                    Dim lStrTemp As String = lCurrentRecord
                    Dim lFieldCount As Integer = 0
                    Do While StrSplit(lStrTemp, lDelim, lQuote).Length > 0
                        lFieldCount += 1
                    Loop
                    Dim lLandUse As New LandUse
                    If lFieldCount = 6 Then
                        'this is the normal way
                        lLandUse.Description = StrSplit(lCurrentRecord, lDelim, lQuote)
                        lLandUse.Type = CInt(StrSplit(lCurrentRecord, lDelim, lQuote))
                        Dim lReachId As String = StrSplit(lCurrentRecord, lDelim, lQuote)
                        lLandUse.Reach = pWatershed.Reaches(lReachId)
                        lLandUse.Area = CDbl(StrSplit(lCurrentRecord, lDelim, lQuote))
                        lLandUse.Slope = CDbl(StrSplit(lCurrentRecord, lDelim, lQuote))
                        lLandUse.Distance = CDbl(StrSplit(lCurrentRecord, lDelim, lQuote))
                    Else 'if coming from old delineator might not be space delimited
                        lLandUse.Description = StrSplit(lCurrentRecord, lDelim, lQuote)
                        If lCurrentRecord.Length > 23 Then
                            lLandUse.Distance = CSng(Mid(lCurrentRecord, lCurrentRecord.Length - 7, 8))
                            lLandUse.Slope = CSng(Mid(lCurrentRecord, lCurrentRecord.Length - 15, 8))
                            lLandUse.Area = CSng(Mid(lCurrentRecord, lCurrentRecord.Length - 23, 8))
                        End If
                        lLandUse.Type = CInt(StrSplit(lCurrentRecord, lDelim, lQuote))
                        Dim lReachId As String = StrSplit(lCurrentRecord, lDelim, lQuote)
                        lLandUse.Reach = pWatershed.Reaches(lReachId)
                    End If
                    Select Case lLandUse.Type
                        Case 1
                            lLandUse.Type = "IMPLND"
                            lLandUse.ImperviousFraction = 1.0
                        Case 2
                            lLandUse.Type = "PERLND"
                            lLandUse.ImperviousFraction = 0.0
                        Case Else
                            lLandUse.Type = "COMPOSITE"
                    End Select

                    Dim lNewKey As String = Me.GetKeyForItem(lLandUse)
                    If Me.Contains(lNewKey) Then 'adjust existing
                        Dim lLandUseExisting As LandUse = Me.Item(lNewKey)
                        With lLandUseExisting
                            Dim lAreaExistingImpervious As Double = .Area * .ImperviousFraction
                            .Area += lLandUse.Area
                            If .Area > 0 Then
                                Select Case lLandUse.Type
                                    Case "PERLND"
                                        .ImperviousFraction = (.Area - lLandUse.Area) / .Area
                                    Case "IMPLND"
                                        .ImperviousFraction = (lAreaExistingImpervious + lLandUse.Area) / .Area
                                End Select
                            End If
                            .Type = "COMPOSITE"
                        End With
                    Else 'a new landuse
                        Me.Add(lLandUse)
                    End If
                End If
            Loop
        Catch e As ApplicationException
            Logger.Msg("Problem reading file " & lName & vbCrLf & e.Message, "Create Problem")
            lReturnCode = 1
        End Try
        Return lReturnCode
    End Function
End Class

Public Class LandUse
    Public Description As String  'Description like Urban, Forest, etc.
    Public Code As String         'could be integer version of description, or just description
    Public Type As String         'PERLND(2) or IMPLND(1)
    Public Reach As Reach         'Reach that this land use type contributes to
    Public ImperviousFraction As Double 'Fraction of Area that is Directly Connected Impervious
    Public Area As Double         'Area of this land use contributing to this reach
    Public Slope As Double        'Average slope of this subbasin, not land use specific
    Public Distance As Double     'Overland flow distance
    Public ModelID As Integer     'Assigned id for this land use type, e.g. 101 for Forest land uses 
End Class
