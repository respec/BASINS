Imports System
Imports atcUtility
Imports atcData
Imports atcWDM
Imports atcHspfBinOut
Imports HspfSupport
Imports atcUCI
Imports atcMetCmp
Imports CREATClimateData

Imports MapWinUtility
Imports atcGraph
Imports ZedGraph

Imports MapWindow.Interfaces
Imports System.Collections.Specialized
Imports System.Collections
Imports Microsoft.VisualBasic.Strings
Imports Microsoft.VisualBasic.DateAndTime
Imports Microsoft.VisualBasic.Constants


Module ProcessCREATData

    Dim pWdmDataPath As String = "C:\Basins\data\SomeWDMs\" '<<<change this to your wdm folder
    Private pCREATPath As String = "C:\Program Files\CREAT 2.0"
    Private pOutputPath As String = "C:\Projects\CAT_SWCalculator\CREAT"
    Private pLat As Single = 35.0
    Private pLong As Single = -80.0
    Private pTask As Integer = 0
    Private pDebug As Boolean = False
    Private Const pReportNanAsZero As Boolean = True
    Private Const pFormat As String = "##,###,##0.000"

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        pTask = 4
        Select Case pTask
            Case 0
                Test()
            Case 1
                GetRainProjections()
            Case 2
                GetPMETTempProjections()
            Case 3
                GetPMETPrecProjections()
            Case 4
                SummarizePMETAfterChange()
        End Select
    End Sub

    Private Sub GetRainProjections()
        Dim lTemplate As String = "C:\dev\StormwaterCalculator\D4EMLite\PREC_DetailsCopy.txt"

        Dim lTableTemp As New atcTableDelimited()
        With lTableTemp
            .Delimiter = vbTab
            If Not .OpenFile(lTemplate) Then Exit Sub
            .CurrentRecord = 1
            Dim lIndexStationId As Integer = .FieldNumber("StationId")
            Dim lIndexLat As Integer = .FieldNumber("Lat")
            Dim lIndexLong As Integer = .FieldNumber("Long")

            Dim lClimateData As New CREATClimateData.ClimateData
            Dim lCREATPrecData As CREATClimateData.TempPrecipData

            Dim lSW2035Hot As New IO.StreamWriter(pOutputPath & "\PREC2035Hot.txt", False)
            Dim lSW2035Med As New IO.StreamWriter(pOutputPath & "\PREC2035Med.txt", False)
            Dim lSW2035Wet As New IO.StreamWriter(pOutputPath & "\PREC2035Wet.txt", False)
            Dim lSW2060Hot As New IO.StreamWriter(pOutputPath & "\PREC2060Hot.txt", False)
            Dim lSW2060Med As New IO.StreamWriter(pOutputPath & "\PREC2060Med.txt", False)
            Dim lSW2060Wet As New IO.StreamWriter(pOutputPath & "\PREC2060Wet.txt", False)

            lSW2035Hot.WriteLine("StationId" & vbTab & "Jan" & vbTab & "Feb" & vbTab & "Mar" & vbTab & "Apr" & vbTab & "May" & vbTab & "Jun" & vbTab & "Jul" & vbTab & "Aug" & vbTab & "Sep" & vbTab & "Oct" & vbTab & "Nov" & vbTab & "Dec" & vbTab & "Ann")
            lSW2035Med.WriteLine("StationId" & vbTab & "Jan" & vbTab & "Feb" & vbTab & "Mar" & vbTab & "Apr" & vbTab & "May" & vbTab & "Jun" & vbTab & "Jul" & vbTab & "Aug" & vbTab & "Sep" & vbTab & "Oct" & vbTab & "Nov" & vbTab & "Dec" & vbTab & "Ann")
            lSW2035Wet.WriteLine("StationId" & vbTab & "Jan" & vbTab & "Feb" & vbTab & "Mar" & vbTab & "Apr" & vbTab & "May" & vbTab & "Jun" & vbTab & "Jul" & vbTab & "Aug" & vbTab & "Sep" & vbTab & "Oct" & vbTab & "Nov" & vbTab & "Dec" & vbTab & "Ann")
            lSW2060Hot.WriteLine("StationId" & vbTab & "Jan" & vbTab & "Feb" & vbTab & "Mar" & vbTab & "Apr" & vbTab & "May" & vbTab & "Jun" & vbTab & "Jul" & vbTab & "Aug" & vbTab & "Sep" & vbTab & "Oct" & vbTab & "Nov" & vbTab & "Dec" & vbTab & "Ann")
            lSW2060Med.WriteLine("StationId" & vbTab & "Jan" & vbTab & "Feb" & vbTab & "Mar" & vbTab & "Apr" & vbTab & "May" & vbTab & "Jun" & vbTab & "Jul" & vbTab & "Aug" & vbTab & "Sep" & vbTab & "Oct" & vbTab & "Nov" & vbTab & "Dec" & vbTab & "Ann")
            lSW2060Wet.WriteLine("StationId" & vbTab & "Jan" & vbTab & "Feb" & vbTab & "Mar" & vbTab & "Apr" & vbTab & "May" & vbTab & "Jun" & vbTab & "Jul" & vbTab & "Aug" & vbTab & "Sep" & vbTab & "Oct" & vbTab & "Nov" & vbTab & "Dec" & vbTab & "Ann")

            Dim lFormat As String = "#0.000"
            While Not .EOF
                pLat = Double.Parse(.Value(lIndexLat))
                pLong = Double.Parse(.Value(lIndexLong))

                Try
                    lCREATPrecData = lClimateData.GetCREATPrecip(pCREATPath, pLat, pLong, 2035)
                    lSW2035Hot.WriteLine(.Value(lIndexStationId) & vbTab & _
                                         lCREATPrecData.ModelHot.Jan & vbTab & _
                                         lCREATPrecData.ModelHot.Feb & vbTab & _
                                         lCREATPrecData.ModelHot.Mar & vbTab & _
                                         lCREATPrecData.ModelHot.Apr & vbTab & _
                                         lCREATPrecData.ModelHot.May & vbTab & _
                                         lCREATPrecData.ModelHot.Jun & vbTab & _
                                         lCREATPrecData.ModelHot.Jul & vbTab & _
                                         lCREATPrecData.ModelHot.Aug & vbTab & _
                                         lCREATPrecData.ModelHot.Sep & vbTab & _
                                         lCREATPrecData.ModelHot.Oct & vbTab & _
                                         lCREATPrecData.ModelHot.Nov & vbTab & _
                                         lCREATPrecData.ModelHot.Dec & vbTab & _
                                         lCREATPrecData.ModelHot.Annual)
                    lSW2035Med.WriteLine(.Value(lIndexStationId) & vbTab & _
                                         lCREATPrecData.ModelMedium.Jan & vbTab & _
                                         lCREATPrecData.ModelMedium.Feb & vbTab & _
                                         lCREATPrecData.ModelMedium.Mar & vbTab & _
                                         lCREATPrecData.ModelMedium.Apr & vbTab & _
                                         lCREATPrecData.ModelMedium.May & vbTab & _
                                         lCREATPrecData.ModelMedium.Jun & vbTab & _
                                         lCREATPrecData.ModelMedium.Jul & vbTab & _
                                         lCREATPrecData.ModelMedium.Aug & vbTab & _
                                         lCREATPrecData.ModelMedium.Sep & vbTab & _
                                         lCREATPrecData.ModelMedium.Oct & vbTab & _
                                         lCREATPrecData.ModelMedium.Nov & vbTab & _
                                         lCREATPrecData.ModelMedium.Dec & vbTab & _
                                         lCREATPrecData.ModelMedium.Annual)
                    lSW2035Wet.WriteLine(.Value(lIndexStationId) & vbTab & _
                                         lCREATPrecData.ModelWet.Jan & vbTab & _
                                         lCREATPrecData.ModelWet.Feb & vbTab & _
                                         lCREATPrecData.ModelWet.Mar & vbTab & _
                                         lCREATPrecData.ModelWet.Apr & vbTab & _
                                         lCREATPrecData.ModelWet.May & vbTab & _
                                         lCREATPrecData.ModelWet.Jun & vbTab & _
                                         lCREATPrecData.ModelWet.Jul & vbTab & _
                                         lCREATPrecData.ModelWet.Aug & vbTab & _
                                         lCREATPrecData.ModelWet.Sep & vbTab & _
                                         lCREATPrecData.ModelWet.Oct & vbTab & _
                                         lCREATPrecData.ModelWet.Nov & vbTab & _
                                         lCREATPrecData.ModelWet.Dec & vbTab & _
                                         lCREATPrecData.ModelWet.Annual)
                Catch ex As Exception
                    Logger.Dbg("CREAT2035 Failed(" & .Value(lIndexStationId) & "): " & ex.InnerException.Message())
                End Try

                Try
                    lCREATPrecData = lClimateData.GetCREATPrecip(pCREATPath, pLat, pLong, 2060)
                    lSW2060Hot.WriteLine(.Value(lIndexStationId) & vbTab & _
                                         lCREATPrecData.ModelHot.Jan & vbTab & _
                                         lCREATPrecData.ModelHot.Feb & vbTab & _
                                         lCREATPrecData.ModelHot.Mar & vbTab & _
                                         lCREATPrecData.ModelHot.Apr & vbTab & _
                                         lCREATPrecData.ModelHot.May & vbTab & _
                                         lCREATPrecData.ModelHot.Jun & vbTab & _
                                         lCREATPrecData.ModelHot.Jul & vbTab & _
                                         lCREATPrecData.ModelHot.Aug & vbTab & _
                                         lCREATPrecData.ModelHot.Sep & vbTab & _
                                         lCREATPrecData.ModelHot.Oct & vbTab & _
                                         lCREATPrecData.ModelHot.Nov & vbTab & _
                                         lCREATPrecData.ModelHot.Dec & vbTab & _
                                         lCREATPrecData.ModelHot.Annual)
                    lSW2060Med.WriteLine(.Value(lIndexStationId) & vbTab & _
                                         lCREATPrecData.ModelMedium.Jan & vbTab & _
                                         lCREATPrecData.ModelMedium.Feb & vbTab & _
                                         lCREATPrecData.ModelMedium.Mar & vbTab & _
                                         lCREATPrecData.ModelMedium.Apr & vbTab & _
                                         lCREATPrecData.ModelMedium.May & vbTab & _
                                         lCREATPrecData.ModelMedium.Jun & vbTab & _
                                         lCREATPrecData.ModelMedium.Jul & vbTab & _
                                         lCREATPrecData.ModelMedium.Aug & vbTab & _
                                         lCREATPrecData.ModelMedium.Sep & vbTab & _
                                         lCREATPrecData.ModelMedium.Oct & vbTab & _
                                         lCREATPrecData.ModelMedium.Nov & vbTab & _
                                         lCREATPrecData.ModelMedium.Dec & vbTab & _
                                         lCREATPrecData.ModelMedium.Annual)
                    lSW2060Wet.WriteLine(.Value(lIndexStationId) & vbTab & _
                                         lCREATPrecData.ModelWet.Jan & vbTab & _
                                         lCREATPrecData.ModelWet.Feb & vbTab & _
                                         lCREATPrecData.ModelWet.Mar & vbTab & _
                                         lCREATPrecData.ModelWet.Apr & vbTab & _
                                         lCREATPrecData.ModelWet.May & vbTab & _
                                         lCREATPrecData.ModelWet.Jun & vbTab & _
                                         lCREATPrecData.ModelWet.Jul & vbTab & _
                                         lCREATPrecData.ModelWet.Aug & vbTab & _
                                         lCREATPrecData.ModelWet.Sep & vbTab & _
                                         lCREATPrecData.ModelWet.Oct & vbTab & _
                                         lCREATPrecData.ModelWet.Nov & vbTab & _
                                         lCREATPrecData.ModelWet.Dec & vbTab & _
                                         lCREATPrecData.ModelWet.Annual)
                Catch ex As Exception
                    Logger.Dbg("CREAT2060 Failed (" & .Value(lIndexStationId) & "): " & ex.InnerException.Message())
                End Try

                .MoveNext()
            End While

            lSW2035Hot.Flush()
            lSW2035Med.Flush()
            lSW2035Wet.Flush()

            lSW2060Hot.Flush()
            lSW2060Med.Flush()
            lSW2060Wet.Flush()

            lSW2035Hot.Close()
            lSW2035Med.Close()
            lSW2035Wet.Close()

            lSW2060Hot.Close()
            lSW2060Med.Close()
            lSW2060Wet.Close()

            .Clear()
        End With
        lTableTemp = Nothing

    End Sub

    Private Sub GetPMETTempProjections()
        Dim lTemplate As String = "C:\dev\StormwaterCalculator\D4EMLite\PMET_DetailsCopy.txt"
        Dim lTotalNumStations As Integer = 5235
        Dim lTableTemp As New atcTableDelimited()
        With lTableTemp
            .Delimiter = vbTab
            If Not .OpenFile(lTemplate) Then Exit Sub
            .CurrentRecord = 1
            Dim lIndexStationId As Integer = .FieldNumber("StationId")
            Dim lIndexLat As Integer = .FieldNumber("Lat")
            Dim lIndexLong As Integer = .FieldNumber("Long")

            Dim lClimateData As New CREATClimateData.ClimateData
            Dim lCREATTempData As CREATClimateData.TempPrecipData

            Dim lSW2035Hot As New IO.StreamWriter(pOutputPath & "\PMETTemp\Temp2035Hot.txt", False)
            Dim lSW2035Med As New IO.StreamWriter(pOutputPath & "\PMETTemp\Temp2035Med.txt", False)
            Dim lSW2035Wet As New IO.StreamWriter(pOutputPath & "\PMETTemp\Temp2035Wet.txt", False)
            Dim lSW2060Hot As New IO.StreamWriter(pOutputPath & "\PMETTemp\Temp2060Hot.txt", False)
            Dim lSW2060Med As New IO.StreamWriter(pOutputPath & "\PMETTemp\Temp2060Med.txt", False)
            Dim lSW2060Wet As New IO.StreamWriter(pOutputPath & "\PMETTemp\Temp2060Wet.txt", False)

            lSW2035Hot.WriteLine("StationId" & vbTab & "Jan" & vbTab & "Feb" & vbTab & "Mar" & vbTab & "Apr" & vbTab & "May" & vbTab & "Jun" & vbTab & "Jul" & vbTab & "Aug" & vbTab & "Sep" & vbTab & "Oct" & vbTab & "Nov" & vbTab & "Dec" & vbTab & "Ann")
            lSW2035Med.WriteLine("StationId" & vbTab & "Jan" & vbTab & "Feb" & vbTab & "Mar" & vbTab & "Apr" & vbTab & "May" & vbTab & "Jun" & vbTab & "Jul" & vbTab & "Aug" & vbTab & "Sep" & vbTab & "Oct" & vbTab & "Nov" & vbTab & "Dec" & vbTab & "Ann")
            lSW2035Wet.WriteLine("StationId" & vbTab & "Jan" & vbTab & "Feb" & vbTab & "Mar" & vbTab & "Apr" & vbTab & "May" & vbTab & "Jun" & vbTab & "Jul" & vbTab & "Aug" & vbTab & "Sep" & vbTab & "Oct" & vbTab & "Nov" & vbTab & "Dec" & vbTab & "Ann")
            lSW2060Hot.WriteLine("StationId" & vbTab & "Jan" & vbTab & "Feb" & vbTab & "Mar" & vbTab & "Apr" & vbTab & "May" & vbTab & "Jun" & vbTab & "Jul" & vbTab & "Aug" & vbTab & "Sep" & vbTab & "Oct" & vbTab & "Nov" & vbTab & "Dec" & vbTab & "Ann")
            lSW2060Med.WriteLine("StationId" & vbTab & "Jan" & vbTab & "Feb" & vbTab & "Mar" & vbTab & "Apr" & vbTab & "May" & vbTab & "Jun" & vbTab & "Jul" & vbTab & "Aug" & vbTab & "Sep" & vbTab & "Oct" & vbTab & "Nov" & vbTab & "Dec" & vbTab & "Ann")
            lSW2060Wet.WriteLine("StationId" & vbTab & "Jan" & vbTab & "Feb" & vbTab & "Mar" & vbTab & "Apr" & vbTab & "May" & vbTab & "Jun" & vbTab & "Jul" & vbTab & "Aug" & vbTab & "Sep" & vbTab & "Oct" & vbTab & "Nov" & vbTab & "Dec" & vbTab & "Ann")

            Dim lFormat As String = "#0.000"
            Dim lStationCount As Integer = 1
            While Not .EOF
                pLat = Double.Parse(.Value(lIndexLat))
                pLong = Double.Parse(.Value(lIndexLong))
                Try
                    lCREATTempData = lClimateData.GetCREATTemp(pCREATPath, pLat, pLong, 2035)
                    lSW2035Hot.WriteLine(.Value(lIndexStationId) & vbTab & _
                                         lCREATTempData.ModelHot.Jan & vbTab & _
                                         lCREATTempData.ModelHot.Feb & vbTab & _
                                         lCREATTempData.ModelHot.Mar & vbTab & _
                                         lCREATTempData.ModelHot.Apr & vbTab & _
                                         lCREATTempData.ModelHot.May & vbTab & _
                                         lCREATTempData.ModelHot.Jun & vbTab & _
                                         lCREATTempData.ModelHot.Jul & vbTab & _
                                         lCREATTempData.ModelHot.Aug & vbTab & _
                                         lCREATTempData.ModelHot.Sep & vbTab & _
                                         lCREATTempData.ModelHot.Oct & vbTab & _
                                         lCREATTempData.ModelHot.Nov & vbTab & _
                                         lCREATTempData.ModelHot.Dec & vbTab & _
                                         lCREATTempData.ModelHot.Annual)
                    lSW2035Med.WriteLine(.Value(lIndexStationId) & vbTab & _
                                         lCREATTempData.ModelMedium.Jan & vbTab & _
                                         lCREATTempData.ModelMedium.Feb & vbTab & _
                                         lCREATTempData.ModelMedium.Mar & vbTab & _
                                         lCREATTempData.ModelMedium.Apr & vbTab & _
                                         lCREATTempData.ModelMedium.May & vbTab & _
                                         lCREATTempData.ModelMedium.Jun & vbTab & _
                                         lCREATTempData.ModelMedium.Jul & vbTab & _
                                         lCREATTempData.ModelMedium.Aug & vbTab & _
                                         lCREATTempData.ModelMedium.Sep & vbTab & _
                                         lCREATTempData.ModelMedium.Oct & vbTab & _
                                         lCREATTempData.ModelMedium.Nov & vbTab & _
                                         lCREATTempData.ModelMedium.Dec & vbTab & _
                                         lCREATTempData.ModelMedium.Annual)
                    lSW2035Wet.WriteLine(.Value(lIndexStationId) & vbTab & _
                                         lCREATTempData.ModelWet.Jan & vbTab & _
                                         lCREATTempData.ModelWet.Feb & vbTab & _
                                         lCREATTempData.ModelWet.Mar & vbTab & _
                                         lCREATTempData.ModelWet.Apr & vbTab & _
                                         lCREATTempData.ModelWet.May & vbTab & _
                                         lCREATTempData.ModelWet.Jun & vbTab & _
                                         lCREATTempData.ModelWet.Jul & vbTab & _
                                         lCREATTempData.ModelWet.Aug & vbTab & _
                                         lCREATTempData.ModelWet.Sep & vbTab & _
                                         lCREATTempData.ModelWet.Oct & vbTab & _
                                         lCREATTempData.ModelWet.Nov & vbTab & _
                                         lCREATTempData.ModelWet.Dec & vbTab & _
                                         lCREATTempData.ModelWet.Annual)
                    If lStationCount Mod 20 = 0 Then
                        lSW2035Hot.Flush()
                        lSW2035Med.Flush()
                        lSW2035Wet.Flush()
                    End If
                Catch ex As Exception
                    Logger.Dbg("CREAT2035 Failed(" & .Value(lIndexStationId) & "): " & ex.InnerException.Message())
                End Try

                Try
                    lCREATTempData = lClimateData.GetCREATTemp(pCREATPath, pLat, pLong, 2060)
                    lSW2060Hot.WriteLine(.Value(lIndexStationId) & vbTab & _
                                         lCREATTempData.ModelHot.Jan & vbTab & _
                                         lCREATTempData.ModelHot.Feb & vbTab & _
                                         lCREATTempData.ModelHot.Mar & vbTab & _
                                         lCREATTempData.ModelHot.Apr & vbTab & _
                                         lCREATTempData.ModelHot.May & vbTab & _
                                         lCREATTempData.ModelHot.Jun & vbTab & _
                                         lCREATTempData.ModelHot.Jul & vbTab & _
                                         lCREATTempData.ModelHot.Aug & vbTab & _
                                         lCREATTempData.ModelHot.Sep & vbTab & _
                                         lCREATTempData.ModelHot.Oct & vbTab & _
                                         lCREATTempData.ModelHot.Nov & vbTab & _
                                         lCREATTempData.ModelHot.Dec & vbTab & _
                                         lCREATTempData.ModelHot.Annual)
                    lSW2060Med.WriteLine(.Value(lIndexStationId) & vbTab & _
                                         lCREATTempData.ModelMedium.Jan & vbTab & _
                                         lCREATTempData.ModelMedium.Feb & vbTab & _
                                         lCREATTempData.ModelMedium.Mar & vbTab & _
                                         lCREATTempData.ModelMedium.Apr & vbTab & _
                                         lCREATTempData.ModelMedium.May & vbTab & _
                                         lCREATTempData.ModelMedium.Jun & vbTab & _
                                         lCREATTempData.ModelMedium.Jul & vbTab & _
                                         lCREATTempData.ModelMedium.Aug & vbTab & _
                                         lCREATTempData.ModelMedium.Sep & vbTab & _
                                         lCREATTempData.ModelMedium.Oct & vbTab & _
                                         lCREATTempData.ModelMedium.Nov & vbTab & _
                                         lCREATTempData.ModelMedium.Dec & vbTab & _
                                         lCREATTempData.ModelMedium.Annual)
                    lSW2060Wet.WriteLine(.Value(lIndexStationId) & vbTab & _
                                         lCREATTempData.ModelWet.Jan & vbTab & _
                                         lCREATTempData.ModelWet.Feb & vbTab & _
                                         lCREATTempData.ModelWet.Mar & vbTab & _
                                         lCREATTempData.ModelWet.Apr & vbTab & _
                                         lCREATTempData.ModelWet.May & vbTab & _
                                         lCREATTempData.ModelWet.Jun & vbTab & _
                                         lCREATTempData.ModelWet.Jul & vbTab & _
                                         lCREATTempData.ModelWet.Aug & vbTab & _
                                         lCREATTempData.ModelWet.Sep & vbTab & _
                                         lCREATTempData.ModelWet.Oct & vbTab & _
                                         lCREATTempData.ModelWet.Nov & vbTab & _
                                         lCREATTempData.ModelWet.Dec & vbTab & _
                                         lCREATTempData.ModelWet.Annual)
                    If lStationCount Mod 20 = 0 Then
                        lSW2060Hot.Flush()
                        lSW2060Med.Flush()
                        lSW2060Wet.Flush()
                    End If
                Catch ex As Exception
                    Logger.Dbg("CREAT2060 Failed (" & .Value(lIndexStationId) & "): " & ex.InnerException.Message())
                End Try

                Logger.Progress(lStationCount, lTotalNumStations)

                .MoveNext()
                lStationCount += 1
            End While

            lSW2035Hot.Flush()
            lSW2035Med.Flush()
            lSW2035Wet.Flush()

            lSW2060Hot.Flush()
            lSW2060Med.Flush()
            lSW2060Wet.Flush()

            lSW2035Hot.Close()
            lSW2035Med.Close()
            lSW2035Wet.Close()

            lSW2060Hot.Close()
            lSW2060Med.Close()
            lSW2060Wet.Close()

            .Clear()
        End With
        lTableTemp = Nothing
        Logger.Msg("Done retrieving CREAT Temp Monthly changes for PMET calculations.")
    End Sub

    Private Sub GetPMETPrecProjections()
        Dim lTemplate As String = "C:\dev\StormwaterCalculator\D4EMLite\PMET_DetailsCopy.txt"
        Dim lTotalNumStations As Integer = 5235
        Dim lTableTemp As New atcTableDelimited()
        With lTableTemp
            .Delimiter = vbTab
            If Not .OpenFile(lTemplate) Then Exit Sub
            .CurrentRecord = 1
            Dim lIndexStationId As Integer = .FieldNumber("StationId")
            Dim lIndexLat As Integer = .FieldNumber("Lat")
            Dim lIndexLong As Integer = .FieldNumber("Long")

            Dim lClimateData As New CREATClimateData.ClimateData
            Dim lCREATPrecData As CREATClimateData.TempPrecipData

            Dim lSW2035Hot As New IO.StreamWriter(pOutputPath & "\PMETPrec\Prec2035Hot.txt", False)
            Dim lSW2035Med As New IO.StreamWriter(pOutputPath & "\PMETPrec\Prec2035Med.txt", False)
            Dim lSW2035Wet As New IO.StreamWriter(pOutputPath & "\PMETPrec\Prec2035Wet.txt", False)
            Dim lSW2060Hot As New IO.StreamWriter(pOutputPath & "\PMETPrec\Prec2060Hot.txt", False)
            Dim lSW2060Med As New IO.StreamWriter(pOutputPath & "\PMETPrec\Prec2060Med.txt", False)
            Dim lSW2060Wet As New IO.StreamWriter(pOutputPath & "\PMETPrec\Prec2060Wet.txt", False)

            lSW2035Hot.WriteLine("StationId" & vbTab & "Jan" & vbTab & "Feb" & vbTab & "Mar" & vbTab & "Apr" & vbTab & "May" & vbTab & "Jun" & vbTab & "Jul" & vbTab & "Aug" & vbTab & "Sep" & vbTab & "Oct" & vbTab & "Nov" & vbTab & "Dec" & vbTab & "Ann")
            lSW2035Med.WriteLine("StationId" & vbTab & "Jan" & vbTab & "Feb" & vbTab & "Mar" & vbTab & "Apr" & vbTab & "May" & vbTab & "Jun" & vbTab & "Jul" & vbTab & "Aug" & vbTab & "Sep" & vbTab & "Oct" & vbTab & "Nov" & vbTab & "Dec" & vbTab & "Ann")
            lSW2035Wet.WriteLine("StationId" & vbTab & "Jan" & vbTab & "Feb" & vbTab & "Mar" & vbTab & "Apr" & vbTab & "May" & vbTab & "Jun" & vbTab & "Jul" & vbTab & "Aug" & vbTab & "Sep" & vbTab & "Oct" & vbTab & "Nov" & vbTab & "Dec" & vbTab & "Ann")
            lSW2060Hot.WriteLine("StationId" & vbTab & "Jan" & vbTab & "Feb" & vbTab & "Mar" & vbTab & "Apr" & vbTab & "May" & vbTab & "Jun" & vbTab & "Jul" & vbTab & "Aug" & vbTab & "Sep" & vbTab & "Oct" & vbTab & "Nov" & vbTab & "Dec" & vbTab & "Ann")
            lSW2060Med.WriteLine("StationId" & vbTab & "Jan" & vbTab & "Feb" & vbTab & "Mar" & vbTab & "Apr" & vbTab & "May" & vbTab & "Jun" & vbTab & "Jul" & vbTab & "Aug" & vbTab & "Sep" & vbTab & "Oct" & vbTab & "Nov" & vbTab & "Dec" & vbTab & "Ann")
            lSW2060Wet.WriteLine("StationId" & vbTab & "Jan" & vbTab & "Feb" & vbTab & "Mar" & vbTab & "Apr" & vbTab & "May" & vbTab & "Jun" & vbTab & "Jul" & vbTab & "Aug" & vbTab & "Sep" & vbTab & "Oct" & vbTab & "Nov" & vbTab & "Dec" & vbTab & "Ann")

            Dim lFormat As String = "#0.000"
            Dim lStationCount As Integer = 1
            While Not .EOF
                pLat = Double.Parse(.Value(lIndexLat))
                pLong = Double.Parse(.Value(lIndexLong))
                Try
                    lCREATPrecData = lClimateData.GetCREATPrecip(pCREATPath, pLat, pLong, 2035)
                    lSW2035Hot.WriteLine(.Value(lIndexStationId) & vbTab & _
                                         lCREATPrecData.ModelHot.Jan & vbTab & _
                                         lCREATPrecData.ModelHot.Feb & vbTab & _
                                         lCREATPrecData.ModelHot.Mar & vbTab & _
                                         lCREATPrecData.ModelHot.Apr & vbTab & _
                                         lCREATPrecData.ModelHot.May & vbTab & _
                                         lCREATPrecData.ModelHot.Jun & vbTab & _
                                         lCREATPrecData.ModelHot.Jul & vbTab & _
                                         lCREATPrecData.ModelHot.Aug & vbTab & _
                                         lCREATPrecData.ModelHot.Sep & vbTab & _
                                         lCREATPrecData.ModelHot.Oct & vbTab & _
                                         lCREATPrecData.ModelHot.Nov & vbTab & _
                                         lCREATPrecData.ModelHot.Dec & vbTab & _
                                         lCREATPrecData.ModelHot.Annual)
                    lSW2035Med.WriteLine(.Value(lIndexStationId) & vbTab & _
                                         lCREATPrecData.ModelMedium.Jan & vbTab & _
                                         lCREATPrecData.ModelMedium.Feb & vbTab & _
                                         lCREATPrecData.ModelMedium.Mar & vbTab & _
                                         lCREATPrecData.ModelMedium.Apr & vbTab & _
                                         lCREATPrecData.ModelMedium.May & vbTab & _
                                         lCREATPrecData.ModelMedium.Jun & vbTab & _
                                         lCREATPrecData.ModelMedium.Jul & vbTab & _
                                         lCREATPrecData.ModelMedium.Aug & vbTab & _
                                         lCREATPrecData.ModelMedium.Sep & vbTab & _
                                         lCREATPrecData.ModelMedium.Oct & vbTab & _
                                         lCREATPrecData.ModelMedium.Nov & vbTab & _
                                         lCREATPrecData.ModelMedium.Dec & vbTab & _
                                         lCREATPrecData.ModelMedium.Annual)
                    lSW2035Wet.WriteLine(.Value(lIndexStationId) & vbTab & _
                                         lCREATPrecData.ModelWet.Jan & vbTab & _
                                         lCREATPrecData.ModelWet.Feb & vbTab & _
                                         lCREATPrecData.ModelWet.Mar & vbTab & _
                                         lCREATPrecData.ModelWet.Apr & vbTab & _
                                         lCREATPrecData.ModelWet.May & vbTab & _
                                         lCREATPrecData.ModelWet.Jun & vbTab & _
                                         lCREATPrecData.ModelWet.Jul & vbTab & _
                                         lCREATPrecData.ModelWet.Aug & vbTab & _
                                         lCREATPrecData.ModelWet.Sep & vbTab & _
                                         lCREATPrecData.ModelWet.Oct & vbTab & _
                                         lCREATPrecData.ModelWet.Nov & vbTab & _
                                         lCREATPrecData.ModelWet.Dec & vbTab & _
                                         lCREATPrecData.ModelWet.Annual)
                    If lStationCount Mod 20 = 0 Then
                        lSW2035Hot.Flush()
                        lSW2035Med.Flush()
                        lSW2035Wet.Flush()
                    End If
                Catch ex As Exception
                    Logger.Dbg("CREAT2035 Failed(" & .Value(lIndexStationId) & "): " & ex.InnerException.Message())
                End Try

                Try
                    lCREATPrecData = lClimateData.GetCREATPrecip(pCREATPath, pLat, pLong, 2060)
                    lSW2060Hot.WriteLine(.Value(lIndexStationId) & vbTab & _
                                         lCREATPrecData.ModelHot.Jan & vbTab & _
                                         lCREATPrecData.ModelHot.Feb & vbTab & _
                                         lCREATPrecData.ModelHot.Mar & vbTab & _
                                         lCREATPrecData.ModelHot.Apr & vbTab & _
                                         lCREATPrecData.ModelHot.May & vbTab & _
                                         lCREATPrecData.ModelHot.Jun & vbTab & _
                                         lCREATPrecData.ModelHot.Jul & vbTab & _
                                         lCREATPrecData.ModelHot.Aug & vbTab & _
                                         lCREATPrecData.ModelHot.Sep & vbTab & _
                                         lCREATPrecData.ModelHot.Oct & vbTab & _
                                         lCREATPrecData.ModelHot.Nov & vbTab & _
                                         lCREATPrecData.ModelHot.Dec & vbTab & _
                                         lCREATPrecData.ModelHot.Annual)
                    lSW2060Med.WriteLine(.Value(lIndexStationId) & vbTab & _
                                         lCREATPrecData.ModelMedium.Jan & vbTab & _
                                         lCREATPrecData.ModelMedium.Feb & vbTab & _
                                         lCREATPrecData.ModelMedium.Mar & vbTab & _
                                         lCREATPrecData.ModelMedium.Apr & vbTab & _
                                         lCREATPrecData.ModelMedium.May & vbTab & _
                                         lCREATPrecData.ModelMedium.Jun & vbTab & _
                                         lCREATPrecData.ModelMedium.Jul & vbTab & _
                                         lCREATPrecData.ModelMedium.Aug & vbTab & _
                                         lCREATPrecData.ModelMedium.Sep & vbTab & _
                                         lCREATPrecData.ModelMedium.Oct & vbTab & _
                                         lCREATPrecData.ModelMedium.Nov & vbTab & _
                                         lCREATPrecData.ModelMedium.Dec & vbTab & _
                                         lCREATPrecData.ModelMedium.Annual)
                    lSW2060Wet.WriteLine(.Value(lIndexStationId) & vbTab & _
                                         lCREATPrecData.ModelWet.Jan & vbTab & _
                                         lCREATPrecData.ModelWet.Feb & vbTab & _
                                         lCREATPrecData.ModelWet.Mar & vbTab & _
                                         lCREATPrecData.ModelWet.Apr & vbTab & _
                                         lCREATPrecData.ModelWet.May & vbTab & _
                                         lCREATPrecData.ModelWet.Jun & vbTab & _
                                         lCREATPrecData.ModelWet.Jul & vbTab & _
                                         lCREATPrecData.ModelWet.Aug & vbTab & _
                                         lCREATPrecData.ModelWet.Sep & vbTab & _
                                         lCREATPrecData.ModelWet.Oct & vbTab & _
                                         lCREATPrecData.ModelWet.Nov & vbTab & _
                                         lCREATPrecData.ModelWet.Dec & vbTab & _
                                         lCREATPrecData.ModelWet.Annual)
                    If lStationCount Mod 20 = 0 Then
                        lSW2060Hot.Flush()
                        lSW2060Med.Flush()
                        lSW2060Wet.Flush()
                    End If
                Catch ex As Exception
                    Logger.Dbg("CREAT2060 Failed (" & .Value(lIndexStationId) & "): " & ex.InnerException.Message())
                End Try

                Logger.Progress(lStationCount, lTotalNumStations)

                .MoveNext()
                lStationCount += 1
            End While

            lSW2035Hot.Flush()
            lSW2035Med.Flush()
            lSW2035Wet.Flush()

            lSW2060Hot.Flush()
            lSW2060Med.Flush()
            lSW2060Wet.Flush()

            lSW2035Hot.Close()
            lSW2035Med.Close()
            lSW2035Wet.Close()

            lSW2060Hot.Close()
            lSW2060Med.Close()
            lSW2060Wet.Close()

            .Clear()
        End With
        lTableTemp = Nothing
        Logger.Msg("Done retrieving CREAT Prec Monthly changes for PMET calculations.")
    End Sub

    Private Sub SummarizePMETAfterChange()
        'Step 1. loop through PMET_Details.txt, retrieve original PREC and ATEM
        'Step 2. find corresponding monthly changes for a given WDM and apply to them
        'Step 3. calculate new PMET dataset
        'Step 4. summarize for sumannual and monthly values

        'Set the directory where all of the BASINS's met WDM files are located
        pWdmDataPath = "G:\Data\BasinsMet\WdmFinal\" 'ATest\" 
        'Dim lPMETDatasetId As Integer = 1003

        Dim lWdmFilename As String = ""
        Dim lWdmFilehandle As atcWDM.atcDataSourceWDM = Nothing

        Dim lTableTemp2035Hot As New atcTableDelimited() : lTableTemp2035Hot.Delimiter = vbTab
        Dim lTableTemp2035Med As New atcTableDelimited() : lTableTemp2035Med.Delimiter = vbTab
        Dim lTableTemp2035Wet As New atcTableDelimited() : lTableTemp2035Wet.Delimiter = vbTab
        Dim lTableTemp2060Hot As New atcTableDelimited() : lTableTemp2060Hot.Delimiter = vbTab
        Dim lTableTemp2060Med As New atcTableDelimited() : lTableTemp2060Med.Delimiter = vbTab
        Dim lTableTemp2060Wet As New atcTableDelimited() : lTableTemp2060Wet.Delimiter = vbTab

        Dim lTablePrec2035Hot As New atcTableDelimited() : lTablePrec2035Hot.Delimiter = vbTab
        Dim lTablePrec2035Med As New atcTableDelimited() : lTablePrec2035Med.Delimiter = vbTab
        Dim lTablePrec2035Wet As New atcTableDelimited() : lTablePrec2035Wet.Delimiter = vbTab
        Dim lTablePrec2060Hot As New atcTableDelimited() : lTablePrec2060Hot.Delimiter = vbTab
        Dim lTablePrec2060Med As New atcTableDelimited() : lTablePrec2060Med.Delimiter = vbTab
        Dim lTablePrec2060Wet As New atcTableDelimited() : lTablePrec2060Wet.Delimiter = vbTab

        lTableTemp2035Hot.OpenFile(pOutputPath & "\PMETTemp\Temp2035Hot.txt") : lTableTemp2035Hot.CurrentRecord = 1
        lTableTemp2035Med.OpenFile(pOutputPath & "\PMETTemp\Temp2035Med.txt") : lTableTemp2035Med.CurrentRecord = 1
        lTableTemp2035Wet.OpenFile(pOutputPath & "\PMETTemp\Temp2035Wet.txt") : lTableTemp2035Wet.CurrentRecord = 1
        lTableTemp2060Hot.OpenFile(pOutputPath & "\PMETTemp\Temp2060Hot.txt") : lTableTemp2060Hot.CurrentRecord = 1
        lTableTemp2060Med.OpenFile(pOutputPath & "\PMETTemp\Temp2060Med.txt") : lTableTemp2060Med.CurrentRecord = 1
        lTableTemp2060Wet.OpenFile(pOutputPath & "\PMETTemp\Temp2060Wet.txt") : lTableTemp2060Wet.CurrentRecord = 1

        lTablePrec2035Hot.OpenFile(pOutputPath & "\PMETPrec\Prec2035Hot.txt") : lTablePrec2035Hot.CurrentRecord = 1
        lTablePrec2035Med.OpenFile(pOutputPath & "\PMETPrec\Prec2035Med.txt") : lTablePrec2035Med.CurrentRecord = 1
        lTablePrec2035Wet.OpenFile(pOutputPath & "\PMETPrec\Prec2035Wet.txt") : lTablePrec2035Wet.CurrentRecord = 1
        lTablePrec2060Hot.OpenFile(pOutputPath & "\PMETPrec\Prec2060Hot.txt") : lTablePrec2060Hot.CurrentRecord = 1
        lTablePrec2060Med.OpenFile(pOutputPath & "\PMETPrec\Prec2060Med.txt") : lTablePrec2060Med.CurrentRecord = 1
        lTablePrec2060Wet.OpenFile(pOutputPath & "\PMETPrec\Prec2060Wet.txt") : lTablePrec2060Wet.CurrentRecord = 1

        Dim lSWPmet2035Hot As New IO.StreamWriter(pOutputPath & "\Pmet2035Hot.txt", False)
        Dim lSWPmet2035Med As New IO.StreamWriter(pOutputPath & "\Pmet2035Med.txt", False)
        Dim lSWPmet2035Wet As New IO.StreamWriter(pOutputPath & "\Pmet2035Wet.txt", False)
        Dim lSWPmet2060Hot As New IO.StreamWriter(pOutputPath & "\Pmet2060Hot.txt", False)
        Dim lSWPmet2060Med As New IO.StreamWriter(pOutputPath & "\Pmet2060Med.txt", False)
        Dim lSWPmet2060Wet As New IO.StreamWriter(pOutputPath & "\Pmet2060Wet.txt", False)

        Dim lTitleLine As String = "StationId" & vbTab & "SumAnnual" & vbTab & "Jan" & vbTab & "Feb" & vbTab & "Mar" & vbTab & "Apr" & vbTab & "May" & vbTab & "Jun" & vbTab & "Jul" & vbTab & "Aug" & vbTab & "Sep" & vbTab & "Oct" & vbTab & "Nov" & vbTab & "Dec"
        lSWPmet2035Hot.WriteLine(lTitleLine)
        lSWPmet2035Med.WriteLine(lTitleLine)
        lSWPmet2035Wet.WriteLine(lTitleLine)
        lSWPmet2060Hot.WriteLine(lTitleLine)
        lSWPmet2060Med.WriteLine(lTitleLine)
        lSWPmet2060Wet.WriteLine(lTitleLine)

        Dim lSwatWeatherStations As New SwatWeatherStations

        Dim lTemplate As String = "C:\dev\StormwaterCalculator\D4EMLite\PMET_DetailsCopy.txt"
        Dim lTotalNumStations As Integer = 5235
        Dim lTableTemp As New atcTableDelimited()
        With lTableTemp
            .Delimiter = vbTab
            If Not .OpenFile(lTemplate) Then Exit Sub
            .CurrentRecord = 1
            Dim lIndexStationId As Integer = .FieldNumber("StationId")
            Dim lIndexFileName As Integer = .FieldNumber("FileName")
            Dim lIndexSDate As Double = .FieldNumber("SDate")
            Dim lIndexEDate As Double = .FieldNumber("EDate")

            Dim lStationCount As Integer = 0
            Dim lDiffCount As Integer = 0
            While Not .EOF

                'Verify all files sync on the station id column
                If .Value(lIndexStationId) <> lTableTemp2035Hot.Value(lIndexStationId) OrElse _
                   .Value(lIndexStationId) <> lTableTemp2035Med.Value(lIndexStationId) OrElse _
                   .Value(lIndexStationId) <> lTableTemp2035Wet.Value(lIndexStationId) OrElse _
                   .Value(lIndexStationId) <> lTableTemp2060Hot.Value(lIndexStationId) OrElse _
                   .Value(lIndexStationId) <> lTableTemp2060Med.Value(lIndexStationId) OrElse _
                   .Value(lIndexStationId) <> lTableTemp2060Wet.Value(lIndexStationId) OrElse _
                   .Value(lIndexStationId) <> lTablePrec2035Hot.Value(lIndexStationId) OrElse _
                   .Value(lIndexStationId) <> lTablePrec2035Med.Value(lIndexStationId) OrElse _
                   .Value(lIndexStationId) <> lTablePrec2035Wet.Value(lIndexStationId) OrElse _
                   .Value(lIndexStationId) <> lTablePrec2060Hot.Value(lIndexStationId) OrElse _
                   .Value(lIndexStationId) <> lTablePrec2060Med.Value(lIndexStationId) OrElse _
                   .Value(lIndexStationId) <> lTablePrec2060Wet.Value(lIndexStationId) Then
                    lDiffCount += 1
                    If lDiffCount > 3 Then
                        Logger.Msg("Found a difference, need to investigage.")
                        GoTo CLEANUP
                    End If
                Else 'If all stations are matched up across all tables

                    lWdmFilename = pWdmDataPath & .Value(lIndexFileName)
                    lWdmFilehandle = New atcWDM.atcDataSourceWDM()
                    If lWdmFilehandle.Open(lWdmFilename) Then

                        Dim lTsPRECGroup0 As atcTimeseriesGroup = lWdmFilehandle.DataSets.FindData("Constituent", "PREC")
                        Dim lTsPREC As atcTimeseries = Nothing
                        If lTsPRECGroup0.Count > 0 Then
                            lTsPREC = lTsPRECGroup0(0)
                        End If
                        Dim lTsATEMGroup0 As atcTimeseriesGroup = lWdmFilehandle.DataSets.FindData("Constituent", "ATEM")
                        Dim lTsATEM As atcTimeseries = Nothing
                        If lTsATEMGroup0.Count > 0 Then
                            lTsATEM = lTsATEMGroup0(0)
                        End If
                        
                        If lTsPREC IsNot Nothing AndAlso lTsATEM IsNot Nothing Then
                            Dim lDate(5) As Integer

                            Dim ljStart As Double = StringToJdate(.Value(lIndexSDate).Trim("'"), True)
                            Dim ljEnd As Double = StringToJdate(.Value(lIndexEDate).Trim("'"), False)

                            If ljStart > 0 AndAlso ljEnd > 0 AndAlso ljStart < ljEnd Then
                                lTsPREC = SubsetByDate(lTsPREC, ljStart, ljEnd, Nothing)
                                lTsATEM = SubsetByDate(lTsATEM, ljStart, ljEnd, Nothing)

                                Dim lTsPREC2035Hot As atcTimeseries = lTsPREC.Clone
                                Dim lTsPREC2035Med As atcTimeseries = lTsPREC.Clone
                                Dim lTsPREC2035Wet As atcTimeseries = lTsPREC.Clone
                                Dim lTsPREC2060Hot As atcTimeseries = lTsPREC.Clone
                                Dim lTsPREC2060Med As atcTimeseries = lTsPREC.Clone
                                Dim lTsPREC2060Wet As atcTimeseries = lTsPREC.Clone

                                Dim lTsATEM2035Hot As atcTimeseries = lTsATEM.Clone
                                Dim lTsATEM2035Med As atcTimeseries = lTsATEM.Clone
                                Dim lTsATEM2035Wet As atcTimeseries = lTsATEM.Clone
                                Dim lTsATEM2060Hot As atcTimeseries = lTsATEM.Clone
                                Dim lTsATEM2060Med As atcTimeseries = lTsATEM.Clone
                                Dim lTsATEM2060Wet As atcTimeseries = lTsATEM.Clone

                                'Do the actual monthly adjustment for precip
                                Dim lRainX2035Hot As Double
                                Dim lRainX2035Med As Double
                                Dim lRainX2035Wet As Double
                                Dim lRainX2060Hot As Double
                                Dim lRainX2060Med As Double
                                Dim lRainX2060Wet As Double

                                Dim lTempA2035Hot As Double
                                Dim lTempA2035Med As Double
                                Dim lTempA2035Wet As Double
                                Dim lTempA2060Hot As Double
                                Dim lTempA2060Med As Double
                                Dim lTempA2060Wet As Double

                                'Sid Jan Feb Mar Apr May Jun Jul Aug Sep Oct Nov Dec Ann
                                '1   2   3   4   5   6   7   8   9   10  11  12  13  14
                                Dim lMonthlyChangeIndex As Integer
                                For I As Integer = 1 To lTsPREC.numValues
                                    J2Date(lTsPREC.Dates.Value(I - 1), lDate)
                                    Select Case lDate(1)
                                        Case 1 : lMonthlyChangeIndex = 2
                                        Case 2 : lMonthlyChangeIndex = 3
                                        Case 3 : lMonthlyChangeIndex = 4
                                        Case 4 : lMonthlyChangeIndex = 5
                                        Case 5 : lMonthlyChangeIndex = 6
                                        Case 6 : lMonthlyChangeIndex = 7
                                        Case 7 : lMonthlyChangeIndex = 8
                                        Case 8 : lMonthlyChangeIndex = 9
                                        Case 9 : lMonthlyChangeIndex = 10
                                        Case 10 : lMonthlyChangeIndex = 11
                                        Case 11 : lMonthlyChangeIndex = 12
                                        Case 12 : lMonthlyChangeIndex = 13
                                    End Select

                                    lRainX2035Hot = 1.0 + Double.Parse(lTablePrec2035Hot.Value(lMonthlyChangeIndex)) / 100.0
                                    lRainX2035Med = 1.0 + Double.Parse(lTablePrec2035Med.Value(lMonthlyChangeIndex)) / 100.0
                                    lRainX2035Wet = 1.0 + Double.Parse(lTablePrec2035Wet.Value(lMonthlyChangeIndex)) / 100.0
                                    lRainX2060Hot = 1.0 + Double.Parse(lTablePrec2060Hot.Value(lMonthlyChangeIndex)) / 100.0
                                    lRainX2060Med = 1.0 + Double.Parse(lTablePrec2060Med.Value(lMonthlyChangeIndex)) / 100.0
                                    lRainX2060Wet = 1.0 + Double.Parse(lTablePrec2060Wet.Value(lMonthlyChangeIndex)) / 100.0

                                    lTempA2035Hot = Double.Parse(lTableTemp2035Hot.Value(lMonthlyChangeIndex))
                                    lTempA2035Med = Double.Parse(lTableTemp2035Med.Value(lMonthlyChangeIndex))
                                    lTempA2035Wet = Double.Parse(lTableTemp2035Wet.Value(lMonthlyChangeIndex))
                                    lTempA2060Hot = Double.Parse(lTableTemp2060Hot.Value(lMonthlyChangeIndex))
                                    lTempA2060Med = Double.Parse(lTableTemp2060Med.Value(lMonthlyChangeIndex))
                                    lTempA2060Wet = Double.Parse(lTableTemp2060Wet.Value(lMonthlyChangeIndex))

                                    If I <= lTsPREC2035Hot.numValues Then lTsPREC2035Hot.Value(I) *= lRainX2035Hot
                                    If I <= lTsPREC2035Med.numValues Then lTsPREC2035Med.Value(I) *= lRainX2035Med
                                    If I <= lTsPREC2035Wet.numValues Then lTsPREC2035Wet.Value(I) *= lRainX2035Wet
                                    If I <= lTsPREC2060Hot.numValues Then lTsPREC2060Hot.Value(I) *= lRainX2060Hot
                                    If I <= lTsPREC2060Med.numValues Then lTsPREC2060Med.Value(I) *= lRainX2060Med
                                    If I <= lTsPREC2060Wet.numValues Then lTsPREC2060Wet.Value(I) *= lRainX2060Wet

                                    If I <= lTsATEM2035Hot.numValues Then lTsATEM2035Hot.Value(I) += lTempA2035Hot
                                    If I <= lTsATEM2035Med.numValues Then lTsATEM2035Med.Value(I) += lTempA2035Med
                                    If I <= lTsATEM2035Wet.numValues Then lTsATEM2035Wet.Value(I) += lTempA2035Wet
                                    If I <= lTsATEM2060Hot.numValues Then lTsATEM2060Hot.Value(I) += lTempA2060Hot
                                    If I <= lTsATEM2060Med.numValues Then lTsATEM2060Med.Value(I) += lTempA2060Med
                                    If I <= lTsATEM2060Wet.numValues Then lTsATEM2060Wet.Value(I) += lTempA2060Wet

                                Next

                                'Now we have the updated prec and atem, do pmet calc
                                Dim lLatitude As Double = lTsATEM.Attributes.GetValue("Latitude")
                                Dim lLongitude As Double = lTsATEM.Attributes.GetValue("Longitude")
                                Dim lNearestStations As SortedList(Of Double, PointLocation) = lSwatWeatherStations.Closest(lLatitude, lLongitude, 5)
                                Dim lNearestStation As SwatWeatherStation = lNearestStations.Values(0)

                                'TODO: get actual elevation of location rather than using station elevation
                                Dim lElevation As Double = lNearestStation.Elev / 0.3048
                                Dim lDonePMET As Boolean = True

                                Dim lTsPmet2035Hot As atcTimeseries = Nothing
                                Dim lTsPmet2035Med As atcTimeseries = Nothing
                                Dim lTsPmet2035Wet As atcTimeseries = Nothing
                                Dim lTsPmet2060Hot As atcTimeseries = Nothing
                                Dim lTsPmet2060Med As atcTimeseries = Nothing
                                Dim lTsPmet2060Wet As atcTimeseries = Nothing

                                Try
                                    lTsPmet2035Hot = PanEvaporationTimeseriesComputedByPenmanMonteith(lElevation, lTsPREC2035Hot, lTsATEM2035Hot, Nothing, lNearestStation, 0.0, , , pDebug, )
                                    lTsPmet2035Med = PanEvaporationTimeseriesComputedByPenmanMonteith(lElevation, lTsPREC2035Med, lTsATEM2035Med, Nothing, lNearestStation, 0.0, , , pDebug, )
                                    lTsPmet2035Wet = PanEvaporationTimeseriesComputedByPenmanMonteith(lElevation, lTsPREC2035Wet, lTsATEM2035Wet, Nothing, lNearestStation, 0.0, , , pDebug, )
                                    lTsPmet2060Hot = PanEvaporationTimeseriesComputedByPenmanMonteith(lElevation, lTsPREC2060Hot, lTsATEM2060Hot, Nothing, lNearestStation, 0.0, , , pDebug, )
                                    lTsPmet2060Med = PanEvaporationTimeseriesComputedByPenmanMonteith(lElevation, lTsPREC2060Med, lTsATEM2060Med, Nothing, lNearestStation, 0.0, , , pDebug, )
                                    lTsPmet2060Wet = PanEvaporationTimeseriesComputedByPenmanMonteith(lElevation, lTsPREC2060Wet, lTsATEM2060Wet, Nothing, lNearestStation, 0.0, , , pDebug, )

                                    lTsPmet2035Hot.SetInterval(atcTimeUnit.TUDay, 1)
                                    lTsPmet2035Med.SetInterval(atcTimeUnit.TUDay, 1)
                                    lTsPmet2035Wet.SetInterval(atcTimeUnit.TUDay, 1)
                                    lTsPmet2060Hot.SetInterval(atcTimeUnit.TUDay, 1)
                                    lTsPmet2060Med.SetInterval(atcTimeUnit.TUDay, 1)
                                    lTsPmet2060Wet.SetInterval(atcTimeUnit.TUDay, 1)

                                    'lTsPmet.Attributes.SetValue("ID", 9) 'lTsAtem.Attributes.GetValue("ID") + 1000) 'hard code ID9 for PMET
                                    'lTsPmet.Attributes.SetValue("description", "SWAT PM ET inches")

                                Catch ex As Exception
                                    lDonePMET = False
                                End Try

                                lSWPmet2035Hot.Write(.Value(lIndexStationId) & vbTab)
                                lSWPmet2035Med.Write(.Value(lIndexStationId) & vbTab)
                                lSWPmet2035Wet.Write(.Value(lIndexStationId) & vbTab)
                                lSWPmet2060Hot.Write(.Value(lIndexStationId) & vbTab)
                                lSWPmet2060Med.Write(.Value(lIndexStationId) & vbTab)
                                lSWPmet2060Wet.Write(.Value(lIndexStationId) & vbTab)

                                If lDonePMET Then
                                    lSWPmet2035Hot.WriteLine(DoubleToString(Double.Parse(lTsPmet2035Hot.Attributes.GetValue("SumAnnual")), , pFormat) & vbTab & PmetMonthlyAnnual(lTsPmet2035Hot))
                                    lSWPmet2035Med.WriteLine(DoubleToString(Double.Parse(lTsPmet2035Med.Attributes.GetValue("SumAnnual")), , pFormat) & vbTab & PmetMonthlyAnnual(lTsPmet2035Med))
                                    lSWPmet2035Wet.WriteLine(DoubleToString(Double.Parse(lTsPmet2035Wet.Attributes.GetValue("SumAnnual")), , pFormat) & vbTab & PmetMonthlyAnnual(lTsPmet2035Wet))
                                    lSWPmet2060Hot.WriteLine(DoubleToString(Double.Parse(lTsPmet2060Hot.Attributes.GetValue("SumAnnual")), , pFormat) & vbTab & PmetMonthlyAnnual(lTsPmet2060Hot))
                                    lSWPmet2060Med.WriteLine(DoubleToString(Double.Parse(lTsPmet2060Med.Attributes.GetValue("SumAnnual")), , pFormat) & vbTab & PmetMonthlyAnnual(lTsPmet2060Med))
                                    lSWPmet2060Wet.WriteLine(DoubleToString(Double.Parse(lTsPmet2060Wet.Attributes.GetValue("SumAnnual")), , pFormat) & vbTab & PmetMonthlyAnnual(lTsPmet2060Wet))
                                Else
                                    Dim lMessage As String = "Failed Generating Pmet"
                                    lSWPmet2035Hot.WriteLine(lMessage)
                                    lSWPmet2035Med.WriteLine(lMessage)
                                    lSWPmet2035Wet.WriteLine(lMessage)
                                    lSWPmet2060Hot.WriteLine(lMessage)
                                    lSWPmet2060Med.WriteLine(lMessage)
                                    lSWPmet2060Wet.WriteLine(lMessage)
                                End If

                                If lStationCount Mod 20 = 0 Then
                                    lSWPmet2035Hot.Flush()
                                    lSWPmet2035Med.Flush()
                                    lSWPmet2035Wet.Flush()
                                    lSWPmet2060Hot.Flush()
                                    lSWPmet2060Med.Flush()
                                    lSWPmet2060Wet.Flush()
                                End If

                                lTsPREC2035Hot.Clear() : lTsPREC2035Hot = Nothing
                                lTsPREC2035Med.Clear() : lTsPREC2035Med = Nothing
                                lTsPREC2035Wet.Clear() : lTsPREC2035Wet = Nothing
                                lTsPREC2060Hot.Clear() : lTsPREC2060Hot = Nothing
                                lTsPREC2060Med.Clear() : lTsPREC2060Med = Nothing
                                lTsPREC2060Wet.Clear() : lTsPREC2060Wet = Nothing

                                lTsATEM2035Hot.Clear() : lTsATEM2035Hot = Nothing
                                lTsATEM2035Med.Clear() : lTsATEM2035Med = Nothing
                                lTsATEM2035Wet.Clear() : lTsATEM2035Wet = Nothing
                                lTsATEM2060Hot.Clear() : lTsATEM2060Hot = Nothing
                                lTsATEM2060Med.Clear() : lTsATEM2060Med = Nothing
                                lTsATEM2060Wet.Clear() : lTsATEM2060Wet = Nothing

                                lTsPmet2035Hot.Clear() : lTsPmet2035Hot = Nothing
                                lTsPmet2035Med.Clear() : lTsPmet2035Med = Nothing
                                lTsPmet2035Wet.Clear() : lTsPmet2035Wet = Nothing
                                lTsPmet2060Hot.Clear() : lTsPmet2060Hot = Nothing
                                lTsPmet2060Med.Clear() : lTsPmet2060Med = Nothing
                                lTsPmet2060Wet.Clear() : lTsPmet2060Wet = Nothing
                                lTsPREC.Clear() : lTsPREC = Nothing
                                lTsATEM.Clear() : lTsATEM = Nothing
                            End If
                        Else 'either lTsPrec or lTsAtem is nothing
                            lSWPmet2035Hot.Write(.Value(lIndexStationId) & vbTab)
                            lSWPmet2035Med.Write(.Value(lIndexStationId) & vbTab)
                            lSWPmet2035Wet.Write(.Value(lIndexStationId) & vbTab)
                            lSWPmet2060Hot.Write(.Value(lIndexStationId) & vbTab)
                            lSWPmet2060Med.Write(.Value(lIndexStationId) & vbTab)
                            lSWPmet2060Wet.Write(.Value(lIndexStationId) & vbTab)
                            Dim lMessage As String = "Failed Generating Pmet, missing rain or temp."
                            lSWPmet2035Hot.WriteLine(lMessage)
                            lSWPmet2035Med.WriteLine(lMessage)
                            lSWPmet2035Wet.WriteLine(lMessage)
                            lSWPmet2060Hot.WriteLine(lMessage)
                            lSWPmet2060Med.WriteLine(lMessage)
                            lSWPmet2060Wet.WriteLine(lMessage)

                        End If 'lTsPrec and lTsAtem are not nothing
                        lWdmFilehandle.Clear()
                        lWdmFilehandle = Nothing
                    End If 'lWdmFilehandle.Open(lWdmFilename)
                End If

                lTableTemp2035Hot.MoveNext()
                lTableTemp2035Med.MoveNext()
                lTableTemp2035Wet.MoveNext()
                lTableTemp2060Hot.MoveNext()
                lTableTemp2060Med.MoveNext()
                lTableTemp2060Wet.MoveNext()

                lTablePrec2035Hot.MoveNext()
                lTablePrec2035Med.MoveNext()
                lTablePrec2035Wet.MoveNext()
                lTablePrec2060Hot.MoveNext()
                lTablePrec2060Med.MoveNext()
                lTablePrec2060Wet.MoveNext()
                lStationCount += 1
                .MoveNext()
                Logger.Progress(lStationCount, lTotalNumStations)
            End While

            If lDiffCount > 0 Then Logger.Msg("Diff count: " & lDiffCount)
        End With 'lTableTemp

CLEANUP:
        lSWPmet2035Hot.Flush()
        lSWPmet2035Med.Flush()
        lSWPmet2035Wet.Flush()
        lSWPmet2060Hot.Flush()
        lSWPmet2060Med.Flush()
        lSWPmet2060Wet.Flush()

        lSWPmet2035Hot.Close()
        lSWPmet2035Med.Close()
        lSWPmet2035Wet.Close()
        lSWPmet2060Hot.Close()
        lSWPmet2060Med.Close()
        lSWPmet2060Wet.Close()

        lSWPmet2035Hot = Nothing
        lSWPmet2035Med = Nothing
        lSWPmet2035Wet = Nothing
        lSWPmet2060Hot = Nothing
        lSWPmet2060Med = Nothing
        lSWPmet2060Wet = Nothing

        lTableTemp2035Hot.Clear() : lTableTemp2035Hot = Nothing
        lTableTemp2035Med.Clear() : lTableTemp2035Med = Nothing
        lTableTemp2035Wet.Clear() : lTableTemp2035Wet = Nothing
        lTableTemp2060Hot.Clear() : lTableTemp2060Hot = Nothing
        lTableTemp2060Med.Clear() : lTableTemp2060Med = Nothing
        lTableTemp2060Wet.Clear() : lTableTemp2060Wet = Nothing

        lTablePrec2035Hot.Clear() : lTablePrec2035Hot = Nothing
        lTablePrec2035Med.Clear() : lTablePrec2035Med = Nothing
        lTablePrec2035Wet.Clear() : lTablePrec2035Wet = Nothing
        lTablePrec2060Hot.Clear() : lTablePrec2060Hot = Nothing
        lTablePrec2060Med.Clear() : lTablePrec2060Med = Nothing
        lTablePrec2060Wet.Clear() : lTablePrec2060Wet = Nothing

        lTableTemp.Clear() : lTableTemp = Nothing

        Logger.Msg("Done generating PMET scenario statistics.")
    End Sub


    Private Function PmetMonthlyAnnual(ByVal aTsPmet As atcTimeseries) As String
        Dim lSeasonsMonth As New atcSeasonsMonth
        Dim lMonthlyAnnualStr As String = ""
        If aTsPmet.Attributes.GetValue("Min") = -998.0 Then
            UpdateAccumulated(aTsPmet)
        End If

        Dim lMonthValues(12) As Double
        For Each lTimeseriesMonth As atcTimeseries In lSeasonsMonth.Split(aTsPmet, Nothing)
            Dim lMonthValue As Double = lTimeseriesMonth.Attributes.GetValue("SumAnnual")
            If pReportNanAsZero AndAlso Double.IsNaN(lMonthValue) Then
                lMonthValue = 0.0
            ElseIf lMonthValue < 0.01 Then
                lMonthValue = 0.0
            End If
            Dim lSeasonIndex As Integer = lTimeseriesMonth.Attributes.GetValue("SeasonIndex")
            Select Case lSeasonIndex
                Case 1, 3, 5, 7, 8, 10, 12
                    lMonthValues(lSeasonIndex) = lMonthValue / 31.0
                Case 2
                    lMonthValues(lSeasonIndex) = lMonthValue / 28.25
                Case Else
                    lMonthValues(lSeasonIndex) = lMonthValue / 30.0
            End Select

        Next

        'The following is for getting a sum of each monthly value before they are divided by the number of days in the month
        'I guess as a check against the original sumannual value?!
        'Dim lSumMonths As Double = 0.0
        'Dim lMonthIndex As Integer = 1
        'For lFieldIndex As Integer = .NumFields + 1 To .NumFields + 12
        '    lSumMonths += lMonthValues(lMonthIndex)
        '    lDatasetTableWithDetails.Value(lFieldIndex) = DoubleToString(lMonthValues(lMonthIndex), , pFormat)
        '    lMonthIndex += 1
        'Next
        'lDatasetTableWithDetails.Value(lDatasetTableWithDetails.NumFields - 13) = DoubleToString(lSumMonths, , pFormat)
        'lDatasetTableWithDetails.Value(lDatasetTableWithDetails.NumFields) = .Value(.NumFields)
        'lDatasetTableWithDetails.NumRecords += 1
        'Logger.Dbg("Found " & aTsPmet.Attributes.GetValue("Id").ToString.PadLeft(4) & " value " & DoubleToString(aTsPmet.Attributes.GetValue("SumAnnual"), , pFormat).PadLeft(10) & _
        '           DoubleToString(lSumMonths, , pFormat).PadLeft(10) & " in " & lWdmFileName)

        For I As Integer = 1 To 12
            lMonthlyAnnualStr &= DoubleToString(lMonthValues(I), , pFormat) & vbTab
        Next

        Return lMonthlyAnnualStr.TrimEnd(vbTab)

    End Function
    Private Sub Test()
        Dim lClimateData As New CREATClimateData.ClimateData
        Dim lCREATPrecData As CREATClimateData.TempPrecipData = lClimateData.GetCREATPrecip(pCREATPath, pLat, pLong, 2035)
        Dim lCREATPrecIntensityData As CREATClimateData.IntensePrecip = lClimateData.GetCREATIntensePrecip(pCREATPath, 1, pLat, pLong, 2035)

        Dim lSB35 As New System.Text.StringBuilder
        Dim lSB60 As New System.Text.StringBuilder
        Dim lRec As String = "Station ID" & vbTab & "JanHot" & vbTab & "FebHot" & vbTab & "MarHot" & vbTab & "AprHot" & vbTab & "MayHot" & vbTab & _
                   "JunHot" & vbTab & "JulHot" & vbTab & "AugHot" & vbTab & "SepHot" & vbTab & "OctHot" & vbTab & "NovHot" & vbTab & "DecHot" & vbTab & _
                   "JanMed" & vbTab & "FebMed" & vbTab & "MarMed" & vbTab & "AprMed" & vbTab & "MayMed" & vbTab & "JunMed" & vbTab & _
                   "JulMed" & vbTab & "AugMed" & vbTab & "SepMed" & vbTab & "OctMed" & vbTab & "NovMed" & vbTab & "DecMed" & vbTab & _
                   "JanWet" & vbTab & "FebWet" & vbTab & "MarWet" & vbTab & "AprWet" & vbTab & "MayWet" & vbTab & "JunWet" & vbTab & _
                   "JulWet" & vbTab & "AugWet" & vbTab & "SepWet" & vbTab & "OctWet" & vbTab & "NovWet" & vbTab & "DecWet"
        lSB35.AppendLine(lRec)
        lSB60.AppendLine(lRec)

        Dim lEvapStations As New D4EMLite.EvapStationLocations("BASINS-CREAT_Script")
        Dim lCREATTempData As CREATClimateData.TempPrecipData
        For Each lEvapStation As D4EMLite.EvapStationLocation In lEvapStations
            Logger.Dbg(lEvapStation.Id & " : " & lEvapStation.Latitude & " : " & lEvapStation.Longitude)
            pLat = lEvapStation.Latitude
            pLong = lEvapStation.Longitude
            lCREATTempData = lClimateData.GetCREATTemp(pCREATPath, pLat, pLong, 2035) 'getting back changes in degree F for temperature
            lRec = lEvapStation.Id & vbTab & _
                   lCREATTempData.ModelHot.Jan & vbTab & lCREATTempData.ModelHot.Feb & vbTab & lCREATTempData.ModelHot.Mar & vbTab & _
                   lCREATTempData.ModelHot.Apr & vbTab & lCREATTempData.ModelHot.May & vbTab & lCREATTempData.ModelHot.Jun & vbTab & _
                   lCREATTempData.ModelHot.Jul & vbTab & lCREATTempData.ModelHot.Aug & vbTab & lCREATTempData.ModelHot.Sep & vbTab & _
                   lCREATTempData.ModelHot.Oct & vbTab & lCREATTempData.ModelHot.Nov & vbTab & lCREATTempData.ModelHot.Dec & vbTab & _
                   lCREATTempData.ModelMedium.Jan & vbTab & lCREATTempData.ModelMedium.Feb & vbTab & lCREATTempData.ModelMedium.Mar & vbTab & _
                   lCREATTempData.ModelMedium.Apr & vbTab & lCREATTempData.ModelMedium.May & vbTab & lCREATTempData.ModelMedium.Jun & vbTab & _
                   lCREATTempData.ModelMedium.Jul & vbTab & lCREATTempData.ModelMedium.Aug & vbTab & lCREATTempData.ModelMedium.Sep & vbTab & _
                   lCREATTempData.ModelMedium.Oct & vbTab & lCREATTempData.ModelMedium.Nov & vbTab & lCREATTempData.ModelMedium.Dec & vbTab & _
                   lCREATTempData.ModelWet.Jan & vbTab & lCREATTempData.ModelWet.Feb & vbTab & lCREATTempData.ModelWet.Mar & vbTab & _
                   lCREATTempData.ModelWet.Apr & vbTab & lCREATTempData.ModelWet.May & vbTab & lCREATTempData.ModelWet.Jun & vbTab & _
                   lCREATTempData.ModelWet.Jul & vbTab & lCREATTempData.ModelWet.Aug & vbTab & lCREATTempData.ModelWet.Sep & vbTab & _
                   lCREATTempData.ModelWet.Oct & vbTab & lCREATTempData.ModelWet.Nov & vbTab & lCREATTempData.ModelWet.Dec
            lSB35.AppendLine(lRec)
            lCREATTempData = lClimateData.GetCREATTemp(pCREATPath, pLat, pLong, 2060)
            lRec = lEvapStation.Id & vbTab & _
                   lCREATTempData.ModelHot.Jan & vbTab & lCREATTempData.ModelHot.Feb & vbTab & lCREATTempData.ModelHot.Mar & vbTab & _
                   lCREATTempData.ModelHot.Apr & vbTab & lCREATTempData.ModelHot.May & vbTab & lCREATTempData.ModelHot.Jun & vbTab & _
                   lCREATTempData.ModelHot.Jul & vbTab & lCREATTempData.ModelHot.Aug & vbTab & lCREATTempData.ModelHot.Sep & vbTab & _
                   lCREATTempData.ModelHot.Oct & vbTab & lCREATTempData.ModelHot.Nov & vbTab & lCREATTempData.ModelHot.Dec & vbTab & _
                   lCREATTempData.ModelMedium.Jan & vbTab & lCREATTempData.ModelMedium.Feb & vbTab & lCREATTempData.ModelMedium.Mar & vbTab & _
                   lCREATTempData.ModelMedium.Apr & vbTab & lCREATTempData.ModelMedium.May & vbTab & lCREATTempData.ModelMedium.Jun & vbTab & _
                   lCREATTempData.ModelMedium.Jul & vbTab & lCREATTempData.ModelMedium.Aug & vbTab & lCREATTempData.ModelMedium.Sep & vbTab & _
                   lCREATTempData.ModelMedium.Oct & vbTab & lCREATTempData.ModelMedium.Nov & vbTab & lCREATTempData.ModelMedium.Dec & vbTab & _
                   lCREATTempData.ModelWet.Jan & vbTab & lCREATTempData.ModelWet.Feb & vbTab & lCREATTempData.ModelWet.Mar & vbTab & _
                   lCREATTempData.ModelWet.Apr & vbTab & lCREATTempData.ModelWet.May & vbTab & lCREATTempData.ModelWet.Jun & vbTab & _
                   lCREATTempData.ModelWet.Jul & vbTab & lCREATTempData.ModelWet.Aug & vbTab & lCREATTempData.ModelWet.Sep & vbTab & _
                   lCREATTempData.ModelWet.Oct & vbTab & lCREATTempData.ModelWet.Nov & vbTab & lCREATTempData.ModelWet.Dec
            lSB60.AppendLine(lRec)
        Next
        SaveFileString(pOutputPath & "\TempIncreaseDetails2035.txt", lSB35.ToString)
        SaveFileString(pOutputPath & "\TempIncreaseDetails2060.txt", lSB60.ToString)
    End Sub
End Module
