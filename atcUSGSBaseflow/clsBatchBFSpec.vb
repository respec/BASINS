Imports atcUtility
Imports atcData
Imports MapWinUtility

Public Class clsBatchBFSpec

    Public GlobalSettings As atcData.atcDataAttributes

    Public SpecFilename As String

    Public ListBatchBaseflowOpns As atcCollection
    'Public ListBatchUnits As atcCollection 'of clsBatchUnitBF, station id/location as key

    Public Delimiter As String = vbTab

    Public Message As String

    Public DownloadDataDirectory As String = ""

    Public Sub clsBatchBFSpec()

    End Sub

    Public Sub clsBatchBFSpec(ByVal aSpecFilename As String)
        SpecFilename = aSpecFilename
    End Sub

    Public Sub PopulateScenarios()
        If IO.File.Exists(SpecFilename) Then
            Dim lOneLine As String
            Dim lSR As New IO.StreamReader(SpecFilename)

            Dim lBaseflowOpnCounter As Integer = 1
            While Not lSR.EndOfStream
                lOneLine = lSR.ReadLine()
                If lOneLine.Contains("***") Then Continue While 'bypass comments
                If lOneLine.Trim() = "" Then Continue While 'bypass blank lines

                If lOneLine.StartsWith("GLOBAL") Then
                    Dim lReachedEnd As Boolean = False
                    While Not lSR.EndOfStream
                        lOneLine = lSR.ReadLine()
                        If lOneLine.Contains("***") Then Continue While
                        If lOneLine.Trim() = "" Then Continue While
                        If lOneLine.StartsWith("END GLOBAL") Then
                            lReachedEnd = True
                            Exit While
                        End If
                        SpecifyGlobal(lOneLine)
                    End While
                    If lReachedEnd Then Continue While
                End If

                If lOneLine.StartsWith("BASE-FLOW") Then
                    Dim lReachedEnd As Boolean = False
                    While Not lSR.EndOfStream
                        lOneLine = lSR.ReadLine()
                        If lOneLine.Contains("***") Then Continue While
                        If lOneLine.Trim() = "" Then Continue While
                        If lOneLine.StartsWith("END BASE-FLOW") Then
                            lReachedEnd = True
                            lBaseflowOpnCounter += 1
                            Exit While
                        End If
                        SpecifyBaseFlow(lOneLine, lBaseflowOpnCounter)
                    End While
                    If lReachedEnd Then Continue While
                End If

                If lOneLine.StartsWith("OTHER") Then
                    Dim lReachedEnd As Boolean = False
                    While Not lSR.EndOfStream
                        lOneLine = lSR.ReadLine()
                        If lOneLine.Contains("***") Then Continue While
                        If lOneLine.Trim() = "" Then Continue While
                        If lOneLine.StartsWith("END OTHER") Then
                            lReachedEnd = True
                            Exit While
                        End If
                        'SpecifyOther(lOneLine)
                    End While
                    If lReachedEnd Then Continue While
                End If
            End While
            lSR.Close()
            lSR = Nothing
        End If

        MergeSpecs()
    End Sub

    Private Sub MergeSpecs()

    End Sub
    ''' <summary>
    ''' This routine save a line of global setting
    ''' differentiate by first keyword on the line
    ''' </summary>
    ''' <param name="aSpec"></param>
    ''' <remarks></remarks>
    Private Sub SpecifyGlobal(ByVal aSpec As String)
        If GlobalSettings Is Nothing Then GlobalSettings = New atcDataAttributes()
        Dim lArr() As String = aSpec.Split(Delimiter)
        If String.IsNullOrEmpty(lArr(1).Trim()) Then Return
        Select Case lArr(0).Trim().ToLower
            Case "sdate", "startdate"
                Dim lDates(5) As Integer
                Dim lDate As DateTime
                If Date.TryParse(lArr(1), lDate) Then
                    lDates(0) = Microsoft.VisualBasic.DateAndTime.Year(lDate)
                    lDates(1) = Microsoft.VisualBasic.DateAndTime.Month(lDate)
                    lDates(2) = Microsoft.VisualBasic.DateAndTime.Day(lDate)
                    lDates(3) = 0
                    lDates(4) = 0
                    lDates(5) = 0
                    GlobalSettings.Add("SDATE", lDates)
                End If
            Case "edate", "enddate"
                Dim lDates(5) As Integer
                Dim lDate As DateTime
                If Date.TryParse(lArr(1), lDate) Then
                    lDates(0) = Microsoft.VisualBasic.DateAndTime.Year(lDate)
                    lDates(1) = Microsoft.VisualBasic.DateAndTime.Month(lDate)
                    lDates(2) = Microsoft.VisualBasic.DateAndTime.Day(lDate)
                    lDates(3) = 24
                    lDates(4) = 0
                    lDates(5) = 0
                    GlobalSettings.Add("EDATE", lDates)
                End If
            Case "bfmethod"
                Dim lMethods As atcCollection = GlobalSettings.GetValue("BFMethods")
                If lMethods Is Nothing Then
                    lMethods = New atcCollection()
                    lMethods.Add(lArr(1))
                    GlobalSettings.Add("BFMethods", lMethods)
                Else
                    If Not lMethods.Contains(lArr(1)) Then
                        lMethods.Add(lArr(1))
                    End If
                End If
            Case "datadir", "data", "datadirectory"
                If Not String.IsNullOrEmpty(lArr(1)) AndAlso IO.Directory.Exists(lArr(1)) Then
                    DownloadDataDirectory = lArr(1)
                End If
            Case Else
                GlobalSettings.Add(lArr(0), lArr(1))
        End Select
    End Sub

    ''' <summary>
    ''' Create list of BF Units
    ''' </summary>
    ''' <param name="aSpec"></param>
    ''' <remarks></remarks>
    Private Sub SpecifyBaseFlow(ByVal aSpec As String, ByVal aBaseflowOpnCount As Integer)
        Dim lArr() As String = aSpec.Split(vbTab)

        If lArr.Length < 2 Then Return
        If String.IsNullOrEmpty(lArr(0)) Then Return

        If ListBatchBaseflowOpns Is Nothing Then
            ListBatchBaseflowOpns = New atcCollection()
        End If
        Dim lListBatchUnits As atcCollection = ListBatchBaseflowOpns.ItemByKey(aBaseflowOpnCount)
        If lListBatchUnits Is Nothing Then
            lListBatchUnits = New atcCollection()
            ListBatchBaseflowOpns.Add(aBaseflowOpnCount, lListBatchUnits)
        End If

        Select Case lArr(0).ToLower
            Case "station"
                Dim lArrStation() As String = lArr(1).Split(",")
                Dim lStationId As String = lArrStation(0)
                Dim lStation As clsBatchUnitStation = lListBatchUnits.ItemByKey(lStationId)
                If lStation Is Nothing Then
                    lStation = New clsBatchUnitStation()
                    lListBatchUnits.Add(lStationId, lStation)
                End If
                If lArrStation.Length >= 2 Then
                    Dim lStationDA As Double
                    If Double.TryParse(lArrStation(1), lStationDA) AndAlso lStationDA > 0 Then
                        lStation.StationDrainageArea = lStationDA
                    End If
                End If
                If lArrStation.Length >= 3 Then
                    Dim lStationDatafilename As String = lArrStation(2)
                    If Not String.IsNullOrEmpty(lStationDatafilename) AndAlso IO.File.Exists(lStationDatafilename) Then
                        lStation.StationDataFilename = lStationDatafilename
                        lStation.NeedToDownloadData = False
                    End If
                End If
            Case "sdate", "startdate"
            Case "edate", "enddate"
            Case "bfmethod"
            Case "reportby"
            Case "bfi_plength"
            Case "bfi_turnpt"
            Case "bfi_recessconst"
            Case "outputdir"
            Case "outputprefix"
        End Select
    End Sub


    Public Sub Clear()
        
    End Sub
End Class
