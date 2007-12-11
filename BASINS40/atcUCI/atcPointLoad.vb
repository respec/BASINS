Imports System.Collections.ObjectModel
Imports System.IO
Imports MapWinUtility
Imports atcUtility

Public Class PointLoads
    Inherits KeyedCollection(Of String, Facility)
    Protected Overrides Function GetKeyForItem(ByVal aFacility As Facility) As String
        Dim lKey As String
        lKey = aFacility.FacilityNpdes
        Return lKey
    End Function

    Public Function Open(ByVal aFileName As String) As Integer
        'read psr file
        Dim lReturnCode As Integer = 0
        Dim lName As String = FilenameOnly(aFileName) & ".psr"

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
                    lFacility.FacilityName = StrSplit(lCurrentRecord, lDelim, lQuote)
                    lFacility.FacilityNpdes = StrSplit(lCurrentRecord, lDelim, lQuote)
                    lFacility.FacilityReach = StrSplit(lCurrentRecord, lDelim, lQuote)
                    lFacility.FacilityMile = CSng(StrSplit(lCurrentRecord, lDelim, lQuote))
                    Dim lPollutantLoads As New Collection(Of PollutantLoad)
                    lFacility.PollutantLoads = lPollutantLoads
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
                        Dim lPollutantLoad As New PollutantLoad
                        Dim lIndex As Integer = CInt(StrSplit(lCurrentRecord, lDelim, lQuote))
                        lPollutantLoad.PollutantName = StrSplit(lCurrentRecord, lDelim, lQuote)
                        lPollutantLoad.PollutantLoad = CSng(StrSplit(lCurrentRecord, lDelim, lQuote))
                        Me(lIndex).PollutantLoads.Add(lPollutantLoad)
                    End If
                Loop
            End If

            Return lReturnCode
        Catch e As ApplicationException
            Logger.Msg("Problem reading file " & lName & vbCrLf & e.Message, "Create Problem")
            lReturnCode = 1
        End Try
        Return lReturnCode
    End Function

End Class

Public Class Facility
    Public FacilityName As String
    Public FacilityNpdes As String
    Public FacilityReach As String
    Public FacilityMile As Single
    Public PollutantLoads As Collection(Of PollutantLoad)
End Class

Public Class PollutantLoad
    Public PollutantName As String
    Public PollutantLoad As Single
End Class
