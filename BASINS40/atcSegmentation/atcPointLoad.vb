Imports System.Collections.ObjectModel
Imports System.IO
Imports MapWinUtility
Imports MapWinUtility.Strings
Imports atcUtility

Public Class PointLoads
    Inherits KeyedCollection(Of String, Facility)
    Private pWatershed As Watershed
    Protected Overrides Function GetKeyForItem(ByVal aFacility As Facility) As String
        Return aFacility.Npdes
    End Function

    Friend Function Open(ByVal aWatershed) As Integer
        'read psr file
        pWatershed = aWatershed
        Dim lReturnCode As Integer = 0
        Dim lName As String = aWatershed.Name & ".psr"

        Try
            Dim lDelim As String = " "
            Dim lQuote As String = """"
            Dim lCurrentRecord As String
            Dim lStreamReader As New StreamReader(lName)
            Dim lFacilityCount As Integer = CInt(lStreamReader.ReadLine) 'number of facilities 
            lCurrentRecord = lStreamReader.ReadLine 'blank line
            lCurrentRecord = lStreamReader.ReadLine 'header line
            If lFacilityCount > 0 Then
                'have some facilities
                For lIndex As Integer = 1 To lFacilityCount
                    lCurrentRecord = lStreamReader.ReadLine
                    Dim lFacility As New Facility
                    lFacility.Name = StrSplit(lCurrentRecord, lDelim, lQuote)
                    lFacility.Npdes = StrSplit(lCurrentRecord, lDelim, lQuote)
                    lFacility.Reach = StrSplit(lCurrentRecord, lDelim, lQuote)
                    lFacility.Mile = CSng(StrSplit(lCurrentRecord, lDelim, lQuote))
                    Dim lPollutants As New Collection(Of Pollutant)
                    lFacility.Pollutants = lPollutants
                    Me.Add(lFacility)
                Next lIndex
                lCurrentRecord = lStreamReader.ReadLine 'blank line
                lCurrentRecord = lStreamReader.ReadLine 'header line
                'now read pollutant loads at each facility
                Do
                    lCurrentRecord = lStreamReader.ReadLine
                    If lCurrentRecord Is Nothing Then
                        Exit Do
                    Else
                        Dim lPollutant As New Pollutant
                        Dim lIndex As Integer = CInt(StrSplit(lCurrentRecord, lDelim, lQuote))
                        lPollutant.Name = StrSplit(lCurrentRecord, lDelim, lQuote)
                        lPollutant.Load = CSng(StrSplit(lCurrentRecord, lDelim, lQuote))
                        Me(lIndex).Pollutants.Add(lPollutant)
                    End If
                Loop
            End If
        Catch e As ApplicationException
            Logger.Msg("Problem reading file " & lName & vbCrLf & e.Message, "Create Problem")
            lReturnCode = 1
        End Try
        Return lReturnCode
    End Function
End Class

Public Class Facility
    Public Name As String
    Public Npdes As String
    Public Reach As String
    Public Mile As Single
    Public Pollutants As Collection(Of Pollutant)
End Class

Public Class Pollutant
    Public Name As String
    Public Load As Single
End Class
