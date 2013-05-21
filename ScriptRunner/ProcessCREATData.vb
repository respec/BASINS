Imports System
Imports atcUtility
Imports atcData
Imports atcWDM
Imports atcHspfBinOut
Imports HspfSupport
Imports atcUCI
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

    Private pTestPath As String = "C:\Program Files (x86)\CREAT 2.0"
    Private pOutputPath As String = "C:\Projects\CAT_SWCalculator\CREAT"
    Private pLat As Single = 35.0
    Private pLong As Single = -80.0

    Public Sub ScriptMain(ByRef aMapWin As IMapWin)
        Dim lClimateData As New CREATClimateData.ClimateData
        'Dim lCREATPrecData As CREATClimateData.TempPrecipData = lClimateData.GetCREATPrecip(pTestPath, pLat, pLong, 2035)
        'Dim lCREATPrecIntensityData As CREATClimateData.IntensePrecip = lClimateData.GetCREATIntensePrecip(pTestPath, 1, pLat, pLong, 2035)

        Dim lSB35 As New System.Text.StringBuilder
        Dim lSB60 As New System.Text.StringBuilder
        Dim lRec As String = "Station ID" & vbTab & "JanHot" & vbTab & "FebHot" & vbTab & "MarHot" & vbTab & "AprHot" & vbTab & "MayHot" & vbTab & _
                   "JunHot" & vbTab & "JulHot" & vbTab & "AugHot" & vbTab & "SepHot" & vbTab & "OctHot" & vbTab & "NovHot" & vbTab & "DecHot" & vbTab & _
                   "JanMed" & vbTab & "FebMed" & vbTab & "MarMed" & vbTab & "AprMed" & vbTab & "MayMed" & vbTab & "JunMed" & vbTab & _
                   "JulMed" & vbTab & "AugMed" & vbTab & "SepMed" & vbTab & "OctMed" & vbTab & "NovMed" & vbTab & "DecMed" & vbCrLf & _
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
            lCREATTempData = lClimateData.GetCREATTemp(pTestPath, pLat, pLong, 2035)
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
            lCREATTempData = lClimateData.GetCREATTemp(pTestPath, pLat, pLong, 2060)
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
