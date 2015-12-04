﻿Imports atcUtility
Imports atcData
Imports atcTimeseriesFEQ
Imports System.Drawing
Imports ZedGraph
Imports MapWinUtility
Imports System

Module GraphGenScn
    Friend Const DefaultAxisLabelFormat As String = "#,##0.###"
    Friend DefaultMajorGridColor As Color = Color.FromArgb(255, 225, 225, 225)
    Friend DefaultMinorGridColor As Color = Color.FromArgb(255, 245, 245, 245)

    Private pBasename As String
    Private Const pWorkingDirectory As String = "G:\"
    Private Const pTimeseries1Axis As String = "Aux"
    Private Const pTimeseries1IsPoint As Boolean = False
    Private Const pTimeseries2Axis As String = "Aux"
    Private Const pTimeseries2IsPoint As Boolean = False
    Private Const pTimeseries3Axis As String = "Left"
    Private Const pTimeseries3IsPoint As Boolean = False
    Private Const pTimeseries4Axis As String = "Left"
    Private Const pTimeseries4IsPoint As Boolean = True
    Private Const pTimeseries5Axis As String = "Left"
    Private Const pTimeseries5IsPoint As Boolean = True
    Private Const pTimeseries6Axis As String = "Left"
    Private Const pTimeseries6IsPoint As Boolean = True
    Private Const pTimeseries7Axis As String = "Right"
    Private Const pTimeseries7IsPoint As Boolean = False

    Private pTimeseriesConstituent, pLeftYAxisLabel, pLeftAuxAxisLabel As String
    Private pOutputDir As String = ""
    'New things
    Private pWDMFileName As String = ""
    Private pSTAFileName As String = ""
    Private pFEODataFileNames As New List(Of String)
    Private pFEODataFiles As New Dictionary(Of String, atcTimeseriesFEQ.atcTimeseriesFEQ)
    Private pTSGroup As New atcTimeseriesGroup
    Private pGraphSpec As clsGenScnGraphSpec = Nothing
    Private pTSBlockFileName As String = "" 'this is assumed to be the original GenScn .tsb files (csv)
    Private pTSBlockTab As atcTableDelimited = Nothing
    Private pSJDay As Double = 0.0
    Private pEJDay As Double = 0.0
    Private pSJDayCommon As Double = 0.0
    Private pEJDayCommon As Double = 0.0
    Private pGraphTitle As String = ""
    Private pApplyGraphSpec As Boolean
    Private pGraphWidth As Integer
    Private pGraphHeight As Integer

    Private Sub Initialize(ByVal aSTAFile As String)
        'Set control file
        'pSTAFileName = "G:\Admin\USGS_DO15_FEQ\AUTOPLOT\forecast\forecast.sta"
        pSTAFileName = aSTAFile
        Dim lStartPath As String = IO.Path.GetDirectoryName(pSTAFileName)
        Dim lSTAFileH As New IO.StreamReader(pSTAFileName)
        Dim lOneLine As String = ""
        Dim lKey As String = ""
        Dim lValue As String = ""
        While Not lSTAFileH.EndOfStream
            lOneLine = lSTAFileH.ReadLine()
            If lOneLine.StartsWith("#") Then Continue While
            lKey = StrRetRem(lOneLine)
            If lKey = "WDM" Then
                lKey = StrRetRem(lOneLine)
            End If
            lValue = lOneLine.Trim()
            Select Case lKey
                Case "WDM1"
                    pWDMFileName = AbsolutePath(lValue, lStartPath)
                    'check if file exists and quit preemptively here
                    If Not IO.File.Exists(pWDMFileName) Then
                        MsgBox("WDM file not found: " & pWDMFileName & vbCrLf & "In file: " & aSTAFile, , "File Not Found")
                        Exit Sub
                    End If
                Case "FEO"
                    Dim lFEOFileName As String = AbsolutePath(lValue, lStartPath)
                    'check if file exists and quit preemptively here
                    If Not IO.File.Exists(lFEOFileName) Then
                        MsgBox("FEO file not found: " & lFEOFileName & vbCrLf & "In file: " & aSTAFile, , "File Not Found")
                        Exit Sub
                    End If
                    pFEODataFileNames.Add(lFEOFileName)
                    If pTSGroup Is Nothing Then
                        pTSGroup = New atcTimeseriesGroup()
                    End If
                Case "TSB"
                    If lValue.Trim() <> "" Then
                        pTSBlockFileName = AbsolutePath(lValue, lStartPath)
                        If Not IO.File.Exists(pTSBlockFileName) Then
                            MsgBox("TSB file not found: " & pTSBlockFileName & vbCrLf & "In file: " & aSTAFile, , "File Not Found")
                            Exit Sub
                        End If
                    End If
                Case "GRF"
                    If lValue.Trim() <> "" Then
                        lValue = StrRetRem(lOneLine)
                        If pGraphSpec Is Nothing Then
                            pGraphSpec = New clsGenScnGraphSpec()
                            pGraphSpec.Specification = AbsolutePath(lValue, lStartPath)
                            If Not IO.File.Exists(pGraphSpec.Specification) Then
                                MsgBox("GRF file not found: " & pGraphSpec.Specification & vbCrLf & "In file: " & aSTAFile, , "File Not Found")
                                Exit Sub
                            End If
                            pGraphSpec.RetrieveSpecs()
                        End If
                        If lOneLine.Trim = "0" Then
                            pApplyGraphSpec = False
                        ElseIf lOneLine.Trim = "1" Then
                            pApplyGraphSpec = True
                        Else
                            pApplyGraphSpec = True 'default is to apply specs
                        End If
                    End If
                Case "GRAPHWIDTH"
                    If Not Integer.TryParse(lValue.Trim, pGraphWidth) Then
                        pGraphWidth = 0
                    End If
                Case "GRAPHHEIGHT"
                    If Not Integer.TryParse(lValue.Trim, pGraphHeight) Then
                        pGraphHeight = 0
                    End If
                Case "SDATETIME", "EDATETIME"
                    Dim lDate(5) As Integer
                    Dim lStrDate As String = StrRetRem(lOneLine).Trim
                    Dim lStrTime As String = lOneLine.Trim
                    Dim lValueInt As Integer
                    Try
                        Dim lArray() As String = lStrDate.Split("/")
                        For I As Integer = 0 To 2
                            lValueInt = Integer.Parse(lArray(I))
                            lDate(I) = lValueInt
                        Next
                        ReDim lArray(0)
                        lArray = lStrTime.Split(":")
                        For I As Integer = 0 To 2
                            lValueInt = Integer.Parse(lArray(I))
                            lDate(I + 3) = lValueInt
                        Next
                        If lKey = "SDATETIME" Then
                            pSJDay = Date2J(lDate)
                        Else
                            pEJDay = Date2J(lDate)
                        End If
                    Catch ex As Exception
                        If lKey = "SDATETIME" Then
                            pSJDay = 0.0
                        Else
                            pEJDay = 0.0
                        End If
                    End Try
                Case "OUTPUTDIR"
                    pOutputDir = StrRetRem(lOneLine)
                    'check if output directory exists and try to create it if not, quit preemptively here
                    Try
                        If Not IO.Directory.Exists(pOutputDir) Then
                            IO.Directory.CreateDirectory(pOutputDir)
                        End If
                    Catch ex As Exception
                        MsgBox("Create output directory failed: " & pOutputDir, MsgBoxStyle.Critical)
                        Exit Sub
                    End Try
                    'Try
                    '    Dim lSW As New IO.StreamWriter(IO.Path.Combine(foldername, "perm.txt"), False)
                    '    lSW.WriteLine("Has write permission.")
                    '    lSW.Close()
                    '    lSW = Nothing
                    'Catch ex As Exception
                    '    MsgBox("Output directory doesnot have write permission.", MsgBoxStyle.Critical)
                    '    Exit Sub
                    'End Try
            End Select
        End While

        If pGraphSpec Is Nothing Then
            pGraphSpec = New clsGenScnGraphSpec()
            pGraphSpec.Specification = ""
        End If

    End Sub

    Public Sub Main()

        'Get arguments
        Dim s() As String = System.Environment.GetCommandLineArgs()

        If s.Length < 2 Then
            Console.WriteLine("*******************************************************" & vbCrLf)
            Console.WriteLine("*GraphGenScn Usage:" & vbCrLf)
            Console.WriteLine("*     GraphGenScn STA_File_Name" & vbCrLf)
            Console.WriteLine("*" & vbCrLf)
            Console.WriteLine("*     For example:" & vbCrLf)
            Console.WriteLine("*" & vbCrLf)
            Console.WriteLine("*       GraphGenScn C:\AUTOPLOT\forecast\forecast.sta" & vbCrLf)
            Console.WriteLine("*******************************************************" & vbCrLf)
            Exit Sub
        End If

        If IO.File.Exists(s(1)) Then
            Initialize(s(1))
        Else
            Console.WriteLine("Error: File not found. " & s(1) & vbCrLf & vbCrLf)
            Exit Sub
        End If

        If pTSGroup Is Nothing Then
            Logger.Msg("No source data. Stop graphing.")
            Clear()
            Exit Sub
        End If

        If Not pTSBlockFileName = "" Then
            If IO.File.Exists(pTSBlockFileName) Then
                pTSBlockTab = New atcTableDelimited()
                With pTSBlockTab
                    .Delimiter = ","
                    .NumFields = 3
                    .FieldName(1) = "Scenario"
                    .FieldName(2) = "Location"
                    .FieldName(3) = "Constituent"
                End With
                If pTSBlockTab.OpenString("Scenario,Location,Constituent" & vbCrLf & IO.File.ReadAllText(pTSBlockFileName)) Then
                    pTSBlockTab.MoveFirst()
                Else
                    pTSBlockTab = Nothing
                End If
            End If
        Else 'If there is no TS specified, then...
            Logger.Msg("No datasets are specified. Stop graphing.")
            Clear()
            Exit Sub
        End If

        'Make sure statistics are available, for example this enables Timeseries.Attributes.GetValue("Count") to work
        Dim lStatistics As New atcTimeseriesStatistics.atcTimeseriesStatistics
        lStatistics.Initialize()

        'Get Datasets
        'Open WDM first (assuming there is only one WDM)
        Dim lWDM As New atcWDM.atcDataSourceWDM 'atcWdmVb.atcWDMfile '
        If Not lWDM.Open(pWDMFileName) Then
            Logger.Msg("Open WDM file, " & pWDMFileName, " failed. Stop graphing.")
            Clear()
            Exit Sub
        End If
        Dim lMissingCurveIndices As New ArrayList
        With pTSBlockTab
            Dim lScenario As String = ""
            Dim lLocation As String = ""
            Dim lConstituent As String = ""
            Dim lDataFound As Boolean
            While Not .EOF
                lDataFound = False
                'Search for FEOs first
                For Each lFEOFileName As String In pFEODataFileNames
                    If Not lDataFound Then
                        Dim lNewFEODataSource As atcTimeseriesFEQ.atcTimeseriesFEQ
                        If Not pFEODataFiles.ContainsKey(lFEOFileName) Then
                            lNewFEODataSource = New atcTimeseriesFEQ.atcTimeseriesFEQ
                            If lNewFEODataSource.Open(lFEOFileName) Then
                                Logger.Dbg("Found " & lNewFEODataSource.DataSets.Count & " datasets.")
                                pFEODataFiles.Add(lFEOFileName, lNewFEODataSource)
                            End If
                        Else
                            lNewFEODataSource = pFEODataFiles.Item(lFEOFileName)
                        End If
                        For Each lTs As atcTimeseries In lNewFEODataSource.DataSets
                            lScenario = lTs.Attributes.GetValue("Scenario").ToString.ToLower
                            lLocation = lTs.Attributes.GetValue("Location").ToString.ToLower
                            lConstituent = lTs.Attributes.GetValue("Constituent").ToString.ToLower
                            If .Value(1).Trim("""").ToLower = lScenario And .Value(2).Trim("""").ToLower = lLocation And .Value(3).Trim("""").ToLower = lConstituent Then
                                pTSGroup.Add(lTs)
                                lDataFound = True
                                Exit For
                            End If
                        Next
                    Else
                        Exit For
                    End If
                Next 'FEO file

                'Search WDM next
                If Not lDataFound Then
                    For Each lTS As atcTimeseries In lWDM.DataSets
                        lScenario = lTS.Attributes.GetValue("Scenario").ToString.ToLower
                        lLocation = lTS.Attributes.GetValue("Location").ToString.ToLower
                        lConstituent = lTS.Attributes.GetValue("Constituent").ToString.ToLower
                        If .Value(1).Trim("""").ToLower = lScenario And .Value(2).Trim("""").ToLower = lLocation And .Value(3).Trim("""").ToLower = lConstituent Then
                            pTSGroup.Add(lTS)
                            lDataFound = True
                            Exit For
                        End If
                    Next
                End If

                If Not lDataFound Then
                    'Logger.Msg("No data found for " & .Value(1) & "-" & .Value(2) & "-" & .Value(3) & ", Stop graphing.")
                    lMissingCurveIndices.Add(.CurrentRecord - 1)
                    'lWDM.Clear()
                    'lWDM = Nothing
                    'Clear()
                    'Exit Sub
                End If

                .MoveNext()
            End While
        End With

        Dim lAllTimeseriesAreEmpty As Boolean = True
        For Each lTsInGroup As atcTimeseries In pTSGroup
            If lTsInGroup.numValues > 1 Then
                lAllTimeseriesAreEmpty = False
                Exit For
            End If
        Next

        If pTSGroup Is Nothing OrElse pTSGroup.Count = 0 OrElse lAllTimeseriesAreEmpty Then
            Logger.Msg("No data found. Stop graphing.")
            Clear()
            Exit Sub
        Else 'Get the desired duration
            If lMissingCurveIndices.Count > 0 Then
                Dim lCrv(pGraphSpec.POSMAX) As clsGenScnGraphSpec.GSCrvType
                Dim lNewIndex As Integer = 0
                For C As Integer = 0 To pGraphSpec.Crv.Length - 1
                    If Not lMissingCurveIndices.Contains(C) Then
                        lCrv(lNewIndex).Color = pGraphSpec.Crv(C).Color
                        lCrv(lNewIndex).CurveType = pGraphSpec.Crv(C).CurveType
                        lCrv(lNewIndex).LegLbl = pGraphSpec.Crv(C).LegLbl
                        lCrv(lNewIndex).LThck = pGraphSpec.Crv(C).LThck
                        lCrv(lNewIndex).LType = pGraphSpec.Crv(C).LType
                        lCrv(lNewIndex).SType = pGraphSpec.Crv(C).SType

                        lNewIndex += 1
                    End If
                Next
                pGraphSpec.Crv = lCrv
            End If

            Dim lStart As Double
            Dim lEnd As Double
            If pSJDay = 0 OrElse pEJDay = 0 Then
                CommonDates(pTSGroup, pSJDay, pEJDay, pSJDayCommon, pEJDayCommon)
                lStart = pSJDayCommon
                lEnd = pEJDayCommon
            Else
                lStart = pSJDay
                lEnd = pEJDay
            End If
            Dim lTSGroup As New atcTimeseriesGroup
            For Each lTS As atcTimeseries In pTSGroup
                lTSGroup.Add(SubsetByDate(lTS, lStart, lEnd, Nothing))
            Next
            pTSGroup.Clear()
            pTSGroup = lTSGroup
        End If

        Dim lCrvIndex As Integer
        For I As Integer = 0 To pTSGroup.Count - 1
            Dim lCons As String = pTSGroup(I).Attributes.GetValue("Constituent")
            lCrvIndex = pGraphSpec.CurveIndex(lCons, I)
            'pTSGroup(I).Attributes.SetValue("CurveColor", Color.FromArgb(pGraphSpec.Crv(lCrvIndex).Color))
            pTSGroup(I).Attributes.SetValue("CurveColor", ToNewColor(pGraphSpec.Crv(lCrvIndex).Color))
            Dim lTimeseriesAxis As String = pTimeseries3Axis
            '1-leftY,2-rightY,3-Aux,4-X
            Select Case pGraphSpec.Crv(lCrvIndex).CurveType
                Case 1
                    lTimeseriesAxis = pTimeseries3Axis
                Case 2
                    lTimeseriesAxis = pTimeseries7Axis
                Case 3
                    lTimeseriesAxis = pTimeseries1Axis
                Case 4
                    lTimeseriesAxis = "X"
            End Select

            With pTSGroup(I)
                If lCons = "PREC" Then
                    .Attributes.SetValue("YAxis", pTimeseries1Axis)
                    .Attributes.SetValue("Point", pTimeseries1IsPoint)
                    'pLeftAuxAxisLabel = "PREC (in)"
                    pLeftAuxAxisLabel = "PREC"
                Else
                    .Attributes.SetValue("YAxis", lTimeseriesAxis)
                    .Attributes.SetValue("Point", pTimeseries3IsPoint)
                End If
            End With
        Next

        If pGraphSpec.IsReady And pApplyGraphSpec Then
            pLeftAuxAxisLabel = pGraphSpec.Axis(3).label
            pLeftYAxisLabel = pGraphSpec.Axis(1).label
        End If
        If pLeftYAxisLabel = "" Then
            pLeftYAxisLabel = pTSGroup(0).Attributes.GetValue("Constituent").ToString
        End If
        pGraphTitle = "Analysis Plot for Values"
        GraphTimeseriesBatch(pTSGroup)

        lWDM.Clear()
        lWDM = Nothing
        Clear()
    End Sub

    Private Sub Clear()
        pSJDay = 0 : pEJDay = 0 : pSJDayCommon = 0 : pEJDayCommon = 0
        pTimeseriesConstituent = "" : pLeftYAxisLabel = "" : pLeftAuxAxisLabel = "" : pOutputDir = ""
        pGraphTitle = "" : pTSBlockFileName = ""
        If pTSGroup IsNot Nothing Then
            pTSGroup.Clear()
            pTSGroup = Nothing
        End If
        If pGraphSpec IsNot Nothing Then
            pGraphSpec.Clear()
            pGraphSpec = Nothing
        End If
        pFEODataFileNames.Clear()
        If pTSBlockTab IsNot Nothing Then
            pTSBlockTab.Clear()
            pTSBlockTab = Nothing
        End If
        For Each lKey As String In pFEODataFiles.Keys
            If pFEODataFiles.Item(lKey) IsNot Nothing Then
                pFEODataFiles.Item(lKey).Clear()
            End If
        Next
        pFEODataFiles.Clear()
    End Sub


    ''' <summary>
    ''' Create a new curve for each atcTimeseries in aDataGroup and add them to aZgc
    ''' </summary>
    ''' <param name="aDataGroup">Group of timeseries to make into curves</param>
    ''' <param name="aZgc">graph control to add curves to</param>
    ''' <remarks></remarks>
    <CLSCompliant(False)> _
    Sub AddTimeseriesCurves(ByVal aDataGroup As atcTimeseriesGroup, ByVal aZgc As ZedGraphControl)
        If aZgc.IsDisposed Then Exit Sub
        Dim lPaneMain As GraphPane = aZgc.MasterPane.PaneList(aZgc.MasterPane.PaneList.Count - 1)
        Dim lCommonTimeUnits As Integer = aDataGroup.CommonAttributeValue("Time Unit", -1)
        Dim lCommonTimeStep As Integer = aDataGroup.CommonAttributeValue("Time Step", -1)

        Dim lCommonScenario As String = aDataGroup.CommonAttributeValue("Scenario", "")
        Dim lCommonLocation As String = aDataGroup.CommonAttributeValue("Location", "")
        Dim lCommonConstituent As String = aDataGroup.CommonAttributeValue("Constituent", "")
        Dim lCommonUnits As String = aDataGroup.CommonAttributeValue("Units", "")

        Dim lConstituent As String
        Dim lUnits As String

        Dim lYaxisNames As New atcCollection 'name for each item in aDataGroup

        Dim lLeftDataSets As New atcTimeseriesGroup
        Dim lRightDataSets As New atcTimeseriesGroup
        Dim lAuxDataSets As New atcTimeseriesGroup

        Dim lCommonTimeUnitName As String = TimeUnitName(lCommonTimeUnits, lCommonTimeStep)

        For Each lTimeseries As atcTimeseries In aDataGroup
            lConstituent = lTimeseries.Attributes.GetValue("Constituent", "").ToString.ToUpper
            lUnits = lTimeseries.Attributes.GetValue("Units", "").ToString.ToUpper
            Dim lYAxisName As String = lTimeseries.Attributes.GetValue("YAxis", "")
            If lYAxisName.Length = 0 Then 'Does not have a pre-assigned axis
                lYAxisName = "LEFT" 'Default to left Y axis

                'Look for existing curve with same constituent and use the same Y axis
                If GroupContainsAttribute(lLeftDataSets, "Constituent", lConstituent) OrElse _
                   GroupContainsAttribute(lLeftDataSets, "Units", lUnits) Then
                    GoTo FoundMatch
                End If
                If GroupContainsAttribute(lRightDataSets, "Constituent", lConstituent) OrElse _
                   GroupContainsAttribute(lRightDataSets, "Units", lUnits) Then
                    lYAxisName = "RIGHT"
                    GoTo FoundMatch
                End If
                If GroupContainsAttribute(lAuxDataSets, "Constituent", lConstituent) OrElse _
                   GroupContainsAttribute(lAuxDataSets, "Units", lUnits) Then
                    lYAxisName = "AUX"
                    GoTo FoundMatch
                End If

                'Precip defaults to aux when there is other data
                If lCommonConstituent.Length = 0 AndAlso lConstituent.Contains("PREC") Then
                    lYAxisName = "AUX"
                    GoTo FoundMatch
                End If

                If lYaxisNames.Contains("LEFT") Then 'Put new curve on right axis if we already have a non-matching curve on the left
                    lYAxisName = "RIGHT"
                    GoTo FoundMatch
                End If
            End If

FoundMatch:
            Select Case lYAxisName.ToUpper
                Case "AUX" : lAuxDataSets.Add(lTimeseries)
                Case "RIGHT" : lRightDataSets.Add(lTimeseries)
                Case Else : lLeftDataSets.Add(lTimeseries)
            End Select
            lYaxisNames.Add(lTimeseries.Serial, lYAxisName)
        Next

        'If all our datasets are automatically placed on the aux axis, put them all on the left Y instead and skip making the aux axis
        If lAuxDataSets.Count > 0 AndAlso lLeftDataSets.Count = 0 AndAlso lRightDataSets.Count = 0 Then
            lLeftDataSets = lAuxDataSets
            lAuxDataSets = New atcTimeseriesGroup
            lYaxisNames.Clear()
            For Each lTimeseries As atcTimeseries In lLeftDataSets
                lYaxisNames.Add(lTimeseries.Serial, "LEFT")
            Next
        End If

        Dim lMain As ZedGraph.GraphPane = Nothing
        Dim lAux As ZedGraph.GraphPane = Nothing
        If lAuxDataSets.Count > 0 Then
            EnableAuxAxis(aZgc.MasterPane, True, 0.2)
            lAux = aZgc.MasterPane.PaneList(0)
            lMain = aZgc.MasterPane.PaneList(1)
        Else
            lMain = aZgc.MasterPane.PaneList(0)
        End If

        For Each lTimeseries As atcTimeseries In aDataGroup
            Dim lCurve As ZedGraph.CurveItem = AddTimeseriesCurve(lTimeseries, aZgc, lYaxisNames.ItemByKey(lTimeseries.Serial))
            lCurve.Label.Text = TSCurveLabel(lTimeseries, lCommonTimeUnitName, lCommonScenario, lCommonConstituent, lCommonLocation, lCommonUnits)
        Next

        If lLeftDataSets.Count > 0 Then
            ScaleAxis(lLeftDataSets, lPaneMain.YAxis)
        End If
        If lRightDataSets.Count > 0 Then
            ScaleAxis(lRightDataSets, lPaneMain.Y2Axis)
            lMain.Y2Axis.MinSpace = 80
        Else
            lMain.Y2Axis.MinSpace = 20
        End If
        If lAuxDataSets.Count > 0 Then
            lAux.YAxis.MinSpace = lMain.YAxis.MinSpace
            lAux.Y2Axis.MinSpace = lMain.Y2Axis.MinSpace

            ScaleAxis(lAuxDataSets, lAux.YAxis)
            lAux.XAxis.Scale.Min = lMain.XAxis.Scale.Min
            lAux.XAxis.Scale.Max = lMain.XAxis.Scale.Max

            'Make sure both graphs line up horizontally
            'Dim lMaxX As Single = Math.Max(lAux.Rect.X, lMain.Rect.X)
            'Dim lMinRight As Single = Math.Max(lAux.Rect.Right, lMain.Rect.Right)
            'lAux.Rect = New RectangleF(lMaxX, lAux.Rect.Y, lMinRight - lMaxX, lAux.Rect.Height)
            'lMain.Rect = New RectangleF(lMaxX, lMain.Rect.Y, lMinRight - lMaxX, lMain.Rect.Height)
        End If

        AxisTitlesFromCommonAttributes(lPaneMain, lCommonTimeUnitName, lCommonScenario, lCommonConstituent, lCommonLocation, lCommonUnits)
    End Sub

    <CLSCompliant(False)> _
    Sub AxisTitlesFromCommonAttributes(ByVal aPane As GraphPane, _
                              Optional ByVal aCommonTimeUnitName As String = Nothing, _
                              Optional ByVal aCommonScenario As String = Nothing, _
                              Optional ByVal aCommonConstituent As String = Nothing, _
                              Optional ByVal aCommonLocation As String = Nothing, _
                              Optional ByVal aCommonUnits As String = Nothing)
        If Not aCommonTimeUnitName Is Nothing AndAlso aCommonTimeUnitName.Length > 0 _
           AndAlso aCommonTimeUnitName <> "<unk>" _
           AndAlso Not aPane.XAxis.Title.Text.Contains(aCommonTimeUnitName) Then
            aPane.XAxis.Title.Text &= " " & aCommonTimeUnitName
        End If

        If aCommonScenario IsNot Nothing AndAlso aCommonScenario.Length > 0 AndAlso aCommonScenario <> "<unk>" Then
            If aCommonConstituent.Length > 0 _
               AndAlso Not aPane.YAxis.Title.Text.Contains(aCommonScenario) Then
                aPane.YAxis.Title.Text &= " " & aCommonScenario
            ElseIf Not aPane.XAxis.Title.Text.Contains(aCommonScenario) Then
                aPane.XAxis.Title.Text &= " " & aCommonScenario
            End If
        End If

        If aCommonConstituent IsNot Nothing AndAlso aCommonConstituent.Length > 0 _
           AndAlso aCommonConstituent <> "<unk>" _
           AndAlso Not aPane.YAxis.Title.Text.Contains(aCommonConstituent) Then
            aPane.YAxis.Title.Text &= " " & aCommonConstituent
        End If

        If aCommonLocation IsNot Nothing AndAlso aCommonLocation.Length > 0 _
           AndAlso aCommonLocation <> "<unk>" _
           AndAlso Not aPane.XAxis.Title.Text.Contains(aCommonLocation) Then
            If aPane.XAxis.Title.Text.Length > 0 Then aPane.XAxis.Title.Text &= " at "
            aPane.XAxis.Title.Text &= aCommonLocation
        End If

        If aCommonUnits IsNot Nothing AndAlso aCommonUnits.Length > 0 _
           AndAlso aCommonUnits <> "<unk>" _
           AndAlso Not aPane.YAxis.Title.Text.Contains(aCommonUnits) Then
            If aPane.YAxis.Title.Text.Length > 0 Then
                aPane.YAxis.Title.Text &= " (" & aCommonUnits & ")"
            Else
                aPane.YAxis.Title.Text = aCommonUnits
            End If
        End If
    End Sub

    Private Function GroupContainsAttribute(ByVal aGroup As atcTimeseriesGroup, ByVal aAttribute As String, ByVal aValue As String) As Boolean
        For Each lTs As atcTimeseries In aGroup
            If String.Compare(lTs.Attributes.GetValue(aAttribute), aValue, True) = 0 Then
                Return True
            End If
        Next
        Return False
    End Function

    Public Function TimeUnitName(ByVal aTimeUnit As Integer, Optional ByVal aTimeStep As Integer = 1) As String
        Dim lName As String = ""
        Select Case aTimeStep
            Case Is > 1 : lName = aTimeStep & "-"
            Case Is < 1 : aTimeUnit = 0 'aTimeStep <= 0 means bad time step, ignore time units and return ""
        End Select
        Select Case aTimeUnit
            Case 1 : lName &= "SECOND"
            Case 2 : lName &= "MINUTE"
            Case 3 : lName &= "HOURLY"
            Case 4 : lName &= "DAILY"
            Case 5 : lName &= "MONTHLY"
            Case 6 : lName &= "YEARLY"
        End Select
        If lName = "7-DAILY" Then lName = "WEEKLY"
        Return lName
    End Function

    ''' <summary>
    ''' Create a new curve from the given atcTimeseries and add it to the ZedGraphControl
    ''' </summary>
    ''' <param name="aTimeseries">Timeseries data to turn into a curve</param>
    ''' <param name="aZgc">ZedGraphControl to add the curve to</param>
    ''' <param name="aYAxisName">Y axis to use (LEFT, RIGHT, or AUX)</param>
    ''' <remarks></remarks>
    <CLSCompliant(False)> _
    Function AddTimeseriesCurve(ByVal aTimeseries As atcTimeseries, ByVal aZgc As ZedGraphControl, ByVal aYAxisName As String, _
                        Optional ByVal aCommonTimeUnitName As String = Nothing, _
                        Optional ByVal aCommonScenario As String = Nothing, _
                        Optional ByVal aCommonConstituent As String = Nothing, _
                        Optional ByVal aCommonLocation As String = Nothing, _
                        Optional ByVal aCommonUnits As String = Nothing) As CurveItem
        Dim lScen As String = aTimeseries.Attributes.GetValue("scenario")
        Dim lLoc As String = aTimeseries.Attributes.GetValue("location")
        Dim lCons As String = aTimeseries.Attributes.GetValue("constituent")
        Dim lCurveLabel As String = TSCurveLabel(aTimeseries, aCommonTimeUnitName, aCommonScenario, aCommonConstituent, aCommonLocation, aCommonUnits)
        Dim lCurveColor As Color
        If aTimeseries.Attributes.ContainsAttribute("CurveColor") Then
            lCurveColor = aTimeseries.Attributes.GetValue("CurveColor")
        Else
            lCurveColor = GetMatchingColor(lScen & ":" & lLoc & ":" & lCons)
        End If

        Dim lPane As GraphPane = aZgc.MasterPane.PaneList(aZgc.MasterPane.PaneList.Count - 1)
        Dim lYAxis As Axis = lPane.YAxis
        Dim lCurve As LineItem = Nothing

        Select Case aYAxisName.ToUpper
            Case "AUX"
                EnableAuxAxis(aZgc.MasterPane, True, 0.2)
                lPane = aZgc.MasterPane.PaneList(0)
                lYAxis = lPane.YAxis
            Case "RIGHT"
                With lPane.YAxis
                    .MajorTic.IsOpposite = False
                    .MinorTic.IsOpposite = False
                End With
                With lPane.Y2Axis
                    .MajorTic.IsOpposite = False
                    .MinorTic.IsOpposite = False
                    .MinSpace = 80
                    aZgc.MasterPane.PaneList(0).Y2Axis.MinSpace = .MinSpace 'align right space on aux graph if present
                End With
                lYAxis = lPane.Y2Axis
        End Select

        lYAxis.IsVisible = True
        lYAxis.Scale.IsVisible = True

        With lPane
            If .XAxis.Type <> AxisType.DateDual Then
                .XAxis.Type = AxisType.DateDual
            End If
            If aTimeseries.Attributes.GetValue("point", False) Then
                lCurve = .AddCurve(lCurveLabel, New atcTimeseriesPointList(aTimeseries), lCurveColor, SymbolType.Plus)
                lCurve.Line.IsVisible = False
            Else
                lCurve = .AddCurve(lCurveLabel, New atcTimeseriesPointList(aTimeseries), lCurveColor, SymbolType.None)
                lCurve.Line.Width = 1
                Select Case aTimeseries.Attributes.GetValue("StepType", "rearwardstep").ToString.ToLower
                    Case "rearwardstep" : lCurve.Line.StepType = StepType.RearwardStep
                    Case "forwardsegment" : lCurve.Line.StepType = StepType.ForwardSegment
                    Case "forwardstep" : lCurve.Line.StepType = StepType.ForwardStep
                    Case "nonstep" : lCurve.Line.StepType = StepType.NonStep
                    Case "rearwardsegment" : lCurve.Line.StepType = StepType.RearwardSegment
                End Select
            End If

            lCurve.Tag = aTimeseries.Serial 'Make this easy to find again even if label changes

            If aYAxisName.ToUpper.Equals("RIGHT") Then lCurve.IsY2Axis = True

            'Use units as Y axis title (if this data has units and Y axis title is not set)
            If aTimeseries.Attributes.ContainsAttribute("Units") AndAlso _
               (lYAxis.Title Is Nothing OrElse lYAxis.Title.Text Is Nothing OrElse lYAxis.Title.Text.Length = 0) Then
                lYAxis.Title.Text = aTimeseries.Attributes.GetValue("Units")
                lYAxis.Title.IsVisible = True
            End If

            Dim lSJDay As Double = aTimeseries.Attributes.GetValue("SJDay")
            Dim lEJDay As Double = aTimeseries.Attributes.GetValue("EJDay")
            If .CurveList.Count = 1 Then
                If aTimeseries.numValues > 0 Then 'Set X axis to contain this date range
                    .XAxis.Scale.Min = lSJDay
                    .XAxis.Scale.Max = lEJDay
                End If
            ElseIf .CurveList.Count > 1 AndAlso Not lCurve Is Nothing Then
                'Expand time scale if needed to include all dates in new curve
                If aTimeseries.numValues > 0 Then
                    If lSJDay < .XAxis.Scale.Min Then
                        .XAxis.Scale.Min = lSJDay
                    End If
                    If lEJDay > .XAxis.Scale.Max Then
                        .XAxis.Scale.Max = lEJDay
                    End If
                End If
            End If
        End With
        Return lCurve
    End Function

    Public Function TSCurveLabel(ByVal aTimeseries As atcTimeseries, _
                        Optional ByVal aCommonTimeUnitName As String = Nothing, _
                        Optional ByVal aCommonScenario As String = Nothing, _
                        Optional ByVal aCommonConstituent As String = Nothing, _
                        Optional ByVal aCommonLocation As String = Nothing, _
                        Optional ByVal aCommonUnits As String = Nothing) As String
        With aTimeseries.Attributes
            Dim lCurveLabel As String = ""

            If (aCommonTimeUnitName Is Nothing OrElse aCommonTimeUnitName.Length = 0) _
              AndAlso aTimeseries.Attributes.ContainsAttribute("Time Unit") Then
                lCurveLabel &= TimeUnitName(aTimeseries.Attributes.GetValue("Time Unit"), _
                                            aTimeseries.Attributes.GetValue("Time Step", 1)) & " "
            Else
                lCurveLabel &= aCommonTimeUnitName & " "
            End If

            If aCommonScenario Is Nothing OrElse aCommonScenario.Length = 0 Then
                lCurveLabel &= .GetValue("Scenario", "") & " "
            ElseIf Not aCommonScenario.ToLower.Contains("<unk>") Then
                lCurveLabel &= aCommonScenario & " "
            End If
            If aCommonConstituent Is Nothing OrElse aCommonConstituent.Length = 0 Then
                lCurveLabel &= .GetValue("Constituent", "") & " "
            Else
                lCurveLabel &= aCommonConstituent & " "
            End If
            If aCommonLocation Is Nothing OrElse aCommonLocation.Length = 0 OrElse aCommonLocation.ToLower.Contains("<unk>") Then
                Dim lLocation As String = .GetValue("Location", "")
                If lLocation.Length = 0 OrElse lLocation = "<unk>" Then
                    lLocation = .GetValue("STAID", "")
                End If
                If lLocation.Length > 0 AndAlso lLocation <> "<unk>" Then
                    If lCurveLabel.Length > 0 Then lCurveLabel &= "at "
                    lCurveLabel &= lLocation
                End If
            End If
            If (aCommonUnits Is Nothing OrElse aCommonUnits.Length = 0) AndAlso .ContainsAttribute("Units") Then
                lCurveLabel &= " (" & .GetValue("Units", "") & ")"
            End If

            Return lCurveLabel.Replace("<unk>", "").Trim '.GetValue("scenario") & " " & .GetValue("constituent") & " at " & .GetValue("location")
        End With
    End Function

    <CLSCompliant(False)> _
    Public Function EnableAuxAxis(ByVal aMasterPane As ZedGraph.MasterPane, ByVal aEnable As Boolean, ByVal aAuxFraction As Single) As GraphPane
        Dim lPaneMain As GraphPane = aMasterPane.PaneList(aMasterPane.PaneList.Count - 1)
        Dim lPaneAux As GraphPane = Nothing
        If aMasterPane.PaneList.Count > 1 Then lPaneAux = aMasterPane.PaneList(0)
        Dim lDummyForm As New Windows.Forms.Form
        Dim lGraphics As Graphics = lDummyForm.CreateGraphics()
        aMasterPane.PaneList.Clear()
        If aEnable Then
            ' Main pane already exists, just needs to be shifted
            With lPaneMain
                .Margin.All = 0
                .Margin.Top = 10
                .Margin.Bottom = 10
            End With
            ' Create, format, position aux pane
            If lPaneAux Is Nothing Then lPaneAux = New ZedGraph.GraphPane
            FormatPaneWithDefaults(lPaneAux)
            With lPaneAux
                .Margin.All = 0
                .Margin.Top = 10
                With .XAxis
                    .Title.IsVisible = False
                    .Scale.IsVisible = False
                    .Scale.Max = lPaneMain.XAxis.Scale.Max
                    .Scale.Min = lPaneMain.XAxis.Scale.Min
                End With
                .X2Axis.IsVisible = False
                With .YAxis
                    .Type = AxisType.Linear
                    .MinSpace = lPaneMain.YAxis.MinSpace
                End With
                .Y2Axis.MinSpace = lPaneMain.Y2Axis.MinSpace
            End With

            With aMasterPane
                .PaneList.Add(lPaneAux)
                .PaneList.Add(lPaneMain)
                .SetLayout(lGraphics, PaneLayout.SingleColumn)
                .IsCommonScaleFactor = True
                Dim lOrigAuxHeight As Single = lPaneAux.Rect.Height
                Dim lTotalPaneHeight As Single = lPaneMain.Rect.Height + lOrigAuxHeight
                Dim lPaneX As Single = Math.Max(lPaneAux.Rect.X, lPaneMain.Rect.X)
                Dim lPaneWidth As Single = Math.Min(lPaneAux.Rect.Width, lPaneMain.Rect.Width)
                lPaneAux.Rect = New System.Drawing.Rectangle( _
                        lPaneX, lPaneAux.Rect.Y, _
                        lPaneWidth, lTotalPaneHeight * aAuxFraction)
                lPaneMain.Rect = New System.Drawing.Rectangle( _
                        lPaneX, lPaneMain.Rect.Y - lOrigAuxHeight + lPaneAux.Rect.Height, _
                        lPaneWidth, lTotalPaneHeight - lPaneAux.Rect.Height)
            End With
        Else
            aMasterPane.PaneList.Add(lPaneMain)
            aMasterPane.SetLayout(lGraphics, PaneLayout.SingleColumn)
        End If
        aMasterPane.AxisChange()
        lGraphics.Dispose()
        Return lPaneAux
    End Function

    <CLSCompliant(False)> _
    Public Function CreateZgc(Optional ByVal aZgc As ZedGraphControl = Nothing, Optional ByVal aWidth As Integer = 600, Optional ByVal aHeight As Integer = 500) As ZedGraphControl
        InitMatchingColors(FindFile("", "GraphColors.txt"))

        If aZgc Is Nothing OrElse aZgc.IsDisposed Then
            aZgc = New ZedGraphControl
        End If

        Dim lPaneMain As New GraphPane
        FormatPaneWithDefaults(lPaneMain)

        With aZgc
            .Visible = True
            .IsSynchronizeXAxes = True
            .Width = aWidth
            .Height = aHeight
            With .MasterPane
                .PaneList.Clear() 'remove default GraphPane
                .Border.IsVisible = False
                .Legend.IsVisible = False
                .Margin.All = 10
                .InnerPaneGap = 5
                .IsCommonScaleFactor = True
                .PaneList.Add(lPaneMain)
            End With
            EnableAuxAxis(.MasterPane, False, 0)
        End With
        Return aZgc
    End Function

    <CLSCompliant(False)> _
Public Sub FormatPaneWithDefaults(ByVal aPane As ZedGraph.GraphPane)
        With aPane
            .IsAlignGrids = True
            .IsFontsScaled = False
            .IsPenWidthScaled = False
            With .XAxis
                .Scale.FontSpec.Size = 14
                .Scale.FontSpec.IsBold = True
                .Scale.IsUseTenPower = False
                .Title.IsOmitMag = True
                .Scale.Mag = 0
                .MajorTic.IsOutside = False
                .MajorTic.IsInside = True
                .MajorTic.IsOpposite = True
                .MinorTic.IsOutside = False
                .MinorTic.IsInside = True
                .MinorTic.IsOpposite = True
                .Scale.Format = DefaultAxisLabelFormat
                With .MajorGrid
                    .Color = DefaultMajorGridColor
                    .DashOn = 0
                    .DashOff = 0
                    .IsVisible = True
                End With
                With .MinorGrid
                    .Color = DefaultMinorGridColor
                    .DashOn = 0
                    .DashOff = 0
                    .IsVisible = True
                End With
            End With
            With .X2Axis
                .IsVisible = False
            End With
            SetYaxisDefaults(.YAxis)
            SetYaxisDefaults(.Y2Axis)
            .YAxis.MinSpace = 80
            .Y2Axis.MinSpace = 20
            .Y2Axis.Scale.IsVisible = False 'Default to not labeling on Y2, will be turned on later if different from Y
            With .Legend
                .Position = LegendPos.Float
                .Location = New Location(0.05, 0.05, CoordType.ChartFraction, AlignH.Left, AlignV.Top)
                .IsHStack = False
                .Border.IsVisible = False
                .Fill.IsVisible = False
            End With
            .Border.IsVisible = False
        End With
    End Sub

    Private Sub SetYaxisDefaults(ByVal aYaxis As Axis)
        With aYaxis
            .Title.IsOmitMag = True
            .MajorGrid.IsVisible = True
            .MajorTic.IsOutside = False
            .MajorTic.IsInside = True
            .MinorTic.IsOutside = False
            .MinorTic.IsInside = True
            .Scale.IsUseTenPower = False
            .Scale.FontSpec.Size = 14
            .Scale.FontSpec.IsBold = True
            .Scale.Mag = 0
            .Scale.Format = DefaultAxisLabelFormat
            .Scale.Align = AlignP.Inside
            With .MajorGrid
                .Color = DefaultMajorGridColor
                .DashOn = 0
                .DashOff = 0
                .IsVisible = True
            End With
            With .MinorGrid
                .Color = DefaultMinorGridColor
                .DashOn = 0
                .DashOff = 0
                .IsVisible = True
            End With
        End With
    End Sub

    Sub GraphTimeseriesBatch(ByVal aDataGroup As atcTimeseriesGroup)
        If Not IO.Directory.Exists(pOutputDir) Then
            pOutputDir = IO.Path.GetDirectoryName(pSTAFileName)
        End If
        pBasename = IO.Path.GetFileNameWithoutExtension(pSTAFileName)
        Dim lOutFileName As String = IO.Path.Combine(pOutputDir, pBasename)
        Dim lZgc As ZedGraphControl = Nothing
        If pGraphHeight = 0 OrElse pGraphWidth = 0 Then
            pGraphWidth = 640
            pGraphHeight = 480
        End If
        lZgc = CreateZgc(, pGraphWidth, pGraphHeight)

        Dim lGrapher As New clsGraphTime(aDataGroup, lZgc)
        Dim lPaneAux As GraphPane = Nothing
        Dim lPaneMain As GraphPane = Nothing
        If lZgc.MasterPane.PaneList.Count < 2 Then
            lPaneMain = lZgc.MasterPane.PaneList(0)
        Else
            lPaneAux = lZgc.MasterPane.PaneList(0)
            lPaneMain = lZgc.MasterPane.PaneList(1)
        End If

        Dim lCurve As ZedGraph.LineItem = Nothing
        Dim lCrvIndex As Integer = 0
        If lPaneAux IsNot Nothing Then
            'Assuming only PREC is on the top Auxiliary pane
            lCurve = lPaneAux.CurveList.Item(0)
            With lCurve
                If pGraphSpec.IsReady And pApplyGraphSpec Then
                    lCrvIndex = pGraphSpec.CurveIndex("PREC", aDataGroup.Count - 1)
                    .Color = ToNewColor(pGraphSpec.Crv(lCrvIndex).Color)
                    .Line.Width = pGraphSpec.Crv(lCrvIndex).LThck
                    .Label.Text = pGraphSpec.Crv(lCrvIndex).LegLbl
                    .Line.StepType = StepType.ForwardStep

                    With .Symbol
                        If pGraphSpec.Crv(lCrvIndex).SType > 0 Then ' LType >= 0
                            .Type = pGraphSpec.Crv(lCrvIndex).SType ' Mod 5
                            .Size = 3.0F
                            .IsVisible = True
                            lCurve.Line.IsVisible = False
                        Else
                            .Type = SymbolType.None
                            .IsVisible = False
                            lCurve.Line.IsVisible = True
                        End If
                    End With
                Else
                    .Line.Width = 2
                    .Color = Drawing.Color.Red
                End If
            End With
        End If

        Dim lCurveOnMainPane As ZedGraph.LineItem = Nothing
        Dim lLastIndexOfCurvesOnMain As Integer = lPaneMain.CurveList.Count - 1
        'If lPaneAux IsNot Nothing Then
        '    lLastIndexOfCurvesOnMain -= lPaneAux.CurveList.Count
        'End If
        For i As Integer = 0 To lLastIndexOfCurvesOnMain
            lCurveOnMainPane = lPaneMain.CurveList.Item(i)
            If pGraphSpec.IsReady And pApplyGraphSpec Then
                lCrvIndex = pGraphSpec.CurveIndex(lCurveOnMainPane.Label.Text, i)
                lCurveOnMainPane.Label.Text = pGraphSpec.Crv(lCrvIndex).LegLbl
                lCurveOnMainPane.Color = ToNewColor(pGraphSpec.Crv(lCrvIndex).Color)
                lCurveOnMainPane.Line.StepType = StepType.NonStep
                lCurveOnMainPane.Line.Width = pGraphSpec.Crv(lCrvIndex).LThck
                'lPaneMain.Y2Axis.Title.Text = pGraphSpec.Axis(2).label
                With lCurveOnMainPane.Symbol
                    If pGraphSpec.Crv(i).SType > 0 Then ' LType >= 0
                        .Type = pGraphSpec.Crv(i).SType ' Mod 5
                        .Size = 3.0F
                        .IsVisible = True
                        lCurveOnMainPane.Line.IsVisible = False
                    Else
                        .Type = SymbolType.None
                        .IsVisible = False
                        lCurveOnMainPane.Line.IsVisible = True
                    End If
                End With
            Else 'No graph specs loaded
                If Not lCurveOnMainPane.Label.Text.Contains(" at ") Then
                    lCurveOnMainPane.Label.Text &= " at " & aDataGroup(i).Attributes.GetValue("Location")
                End If
                Dim constituent As String = aDataGroup(i).Attributes.GetValue("Constituent")
                lCurveOnMainPane.Line.StepType = StepType.NonStep
                lCurveOnMainPane.Line.Width = 2
                Dim lRand As New Random
                lCurveOnMainPane.Color = Drawing.Color.FromArgb(255, lRand.Next(0, 255), lRand.Next(0, 255), lRand.Next(0, 255))
            End If
        Next

        'Apply labels etc
        If lPaneAux IsNot Nothing Then lPaneAux.YAxis.Title.Text = pLeftAuxAxisLabel
        lPaneMain.YAxis.Title.Text = pLeftYAxisLabel
        lPaneMain.XAxis.Title.Text &= vbCrLf & "Analysis Plot for Values"

        If pGraphSpec.IsReady And pApplyGraphSpec Then
            lPaneMain.XAxis.Title.Text = pGraphSpec.Title.Replace("&", vbCrLf)
            lPaneMain.XAxis.Title.FontSpec.Size = 12.0
            If Not pGraphSpec.XtraText.Trim() = "" Then
                Dim lExtraTextStr As String = pGraphSpec.XtraText.Replace("&", vbCrLf)
                Dim lExtraText As ZedGraph.TextObj = Nothing
                'lExtraText = New ZedGraph.TextObj(lExtraTextStr, pGraphSpec.XTxtLoc, 0.95, CoordType.ChartFraction, AlignH.Left, AlignV.Bottom)
                lExtraText = New ZedGraph.TextObj(lExtraTextStr, pGraphSpec.XTxtLoc, pGraphSpec.YTxtLoc, CoordType.ChartFraction, AlignH.Left, AlignV.Bottom)
                With lExtraText
                    .ZOrder = ZOrder.A_InFront
                    .FontSpec.StringAlignment = Drawing.StringAlignment.Near
                    .FontSpec.Size = 12.0
                    .FontSpec.Border.IsVisible = False
                    .FontSpec.Fill = New ZedGraph.Fill(Drawing.Color.FromArgb(0, 255, 255, 255))
                    .IsVisible = True
                End With
                lPaneMain.GraphObjList.Add(lExtraText)
            End If
        End If

        'Adjust legend position
        If pGraphSpec.IsReady And pApplyGraphSpec Then
            Dim lLocLeg As Location = New Location(pGraphSpec.XLegLoc, pGraphSpec.YLegLoc - 0.95, CoordType.ChartFraction)
            lPaneMain.Legend.Location = lLocLeg
        End If

        'Adjust scale
        Dim lYAxisCross As Double = 0.0
        Dim lShowGrid As Boolean = False
        If pGraphSpec.IsReady And pApplyGraphSpec Then
            '1-leftY,2-rightY,3-Aux,4-X
            If pGraphSpec.Axis(1).label IsNot Nothing Then
                With lPaneMain.YAxis.Scale
                    .MinAuto = False
                    .MaxAuto = False
                    .Min = pGraphSpec.Axis(1).minv
                    .Max = pGraphSpec.Axis(1).maxv
                    .MajorStep = (.Max - .Min) / (pGraphSpec.Axis(1).NTic + 2)
                    lYAxisCross = .Min
                End With

                If pGraphSpec.Gridy = 1 Then
                    lShowGrid = True
                End If
                lPaneMain.YAxis.MajorGrid.IsVisible = lShowGrid
                lPaneMain.YAxis.MinorGrid.IsVisible = lShowGrid
                lPaneMain.YAxis.MinorTic.IsAllTics = lShowGrid
            End If
            lPaneMain.XAxis.CrossAuto = False
            lPaneMain.XAxis.Cross = lYAxisCross
            lPaneMain.YAxis.Scale.BaseTic = lYAxisCross
            lZgc.AxisChange()

            'If pGraphSpec.Axis(2).label IsNot Nothing Then
            '    With lPaneMain.Y2Axis.Scale
            '        .Min = pGraphSpec.Axis(2).minv
            '        .Max = pGraphSpec.Axis(2).maxv
            '        .MajorStep = (.Max - .Min) / pGraphSpec.Axis(2).NTic
            '    End With
            'End If
            If pGraphSpec.Axis(3).label IsNot Nothing AndAlso lPaneAux IsNot Nothing Then
                With lPaneAux.YAxis.Scale
                    .MinAuto = False
                    .MaxAuto = False
                    .Min = pGraphSpec.Axis(3).minv
                    .Max = pGraphSpec.Axis(3).maxv
                    .MajorStep = (.Max - .Min) / pGraphSpec.Axis(3).NTic
                End With
            End If
            'If pGraphSpec.Axis(4).label IsNot Nothing Then
            '    With lPaneMain.XAxis.Scale
            '        .Min = pGraphSpec.Axis(4).minv
            '        .Max = pGraphSpec.Axis(4).maxv
            '        .MajorStep = (.Max - .Min) / pGraphSpec.Axis(4).NTic
            '    End With
            'End If

            lShowGrid = False
            If pGraphSpec.Gridx = 1 Then
                lShowGrid = True
            End If
            lPaneMain.XAxis.MajorGrid.IsVisible = lShowGrid
            lPaneMain.XAxis.MinorGrid.IsVisible = lShowGrid
        End If

        'Make sure both graphs line up horizontally
        Dim lMaxX As Single = 0
        Dim lMinRight As Single = 0
        If lPaneAux IsNot Nothing Then
            lPaneAux.AxisChange()
            lMaxX = Math.Max(lPaneAux.Chart.Rect.X, lPaneMain.Chart.Rect.X)
            lMinRight = Math.Min(lPaneAux.Chart.Rect.Right, lPaneMain.Chart.Rect.Right)
            lPaneAux.Chart.Rect = New Drawing.RectangleF(lMaxX, lPaneAux.Chart.Rect.Y, lMinRight - lMaxX, lPaneAux.Chart.Rect.Height)
        Else
            lMaxX = lPaneMain.Chart.Rect.X
            lMinRight = lPaneMain.Chart.Rect.Right
        End If
        If lPaneMain IsNot Nothing Then lPaneMain.AxisChange()
        lPaneMain.Chart.Rect = New Drawing.RectangleF(lMaxX, lPaneMain.Chart.Rect.Y, lMinRight - lMaxX, lPaneMain.Chart.Rect.Height)

        lZgc.AxisChange()
        lZgc.Invalidate()
        lZgc.Refresh()

        Try
            System.IO.Directory.CreateDirectory(pOutputDir)
        Catch ex As Exception
            Logger.Msg("Problem accessing output directory: " & vbCrLf & pOutputDir)
            Exit Sub
        End Try

        lZgc.SaveIn(lOutFileName & ".png")
        '.emf does not seem to be saving correctly, so skip it for now: lZgc.SaveIn(lOutFileName & ".emf")
        lZgc.Dispose()
    End Sub

    Private Function ToNewColor(ByVal aOldColor As Integer) As Drawing.Color
        Return Drawing.Color.FromArgb(aOldColor And &HFF, (aOldColor And &HFF00) >> 8, (aOldColor And &HFF0000) >> 16)
    End Function

    <CLSCompliant(False)> _
    Public Sub ScaleAxis(ByVal aDataGroup As atcTimeseriesGroup, ByVal aAxis As Axis)
        Dim lDataMin As Double = 1.0E+30
        Dim lDataMax As Double = -1.0E+30
        Dim lLogFlag As Boolean = False
        If aAxis.Type = ZedGraph.AxisType.Log Then
            lLogFlag = True
        End If

        For Each lTimeseries As atcTimeseries In aDataGroup
            Try
                Dim lValue As Double = lTimeseries.Attributes.GetValue("Minimum")
                If lValue < lDataMin Then lDataMin = lValue
                lValue = lTimeseries.Attributes.GetValue("Maximum")
                If lValue > lDataMax Then lDataMax = lValue
            Catch
                'Could not get good Minimum or Maximum value
            End Try
        Next

        If lDataMin < -1.0E+20 Then
            'assume there is a bad value in here
            lDataMin = 0
        End If

        Scalit(lDataMin, lDataMax, lLogFlag, aAxis.Scale.Min, aAxis.Scale.Max)
    End Sub

    ''' <summary>
    ''' Determines an appropriate scale based on the minimum and maximum values and 
    ''' whether an arithmetic or logarithmic scale is requested. 
    ''' For log scales, the minimum and maximum must not be transformed.
    ''' </summary>
    ''' <param name="aDataMin"></param>
    ''' <param name="aDataMax"></param>
    ''' <param name="aLogScale"></param>
    ''' <param name="aScaleMin"></param>
    ''' <param name="aScaleMax"></param>
    ''' <remarks></remarks>
    Public Sub Scalit(ByVal aDataMin As Double, ByVal aDataMax As Double, ByVal aLogScale As Boolean, _
                      ByRef aScaleMin As Double, ByRef aScaleMax As Double)
        'TODO: should existing ScaleMin and ScaleMax be respected?
        If Not aLogScale Then 'arithmetic scale
            'get next lowest mult of 10
            Static lRange(15) As Double
            If lRange(1) < 0.09 Then 'need to initialze
                lRange(1) = 0.1
                lRange(2) = 0.15
                lRange(3) = 0.2
                lRange(4) = 0.4
                lRange(5) = 0.5
                lRange(6) = 0.6
                lRange(7) = 0.8
                lRange(8) = 1.0#
                lRange(9) = 1.5
                lRange(10) = 2.0#
                lRange(11) = 4.0#
                lRange(12) = 5.0#
                lRange(13) = 6.0#
                lRange(14) = 8.0#
                lRange(15) = 10.0#
            End If

            Dim lRangeIndex As Integer
            Dim lRangeInc As Integer
            Dim lDataRndlow As Double = Rndlow(aDataMax)
            If lDataRndlow > 0.0# Then
                lRangeInc = 1
                lRangeIndex = 1
            Else
                lRangeInc = -1
                lRangeIndex = 15
            End If
            Do
                aScaleMax = lRange(lRangeIndex) * lDataRndlow
                lRangeIndex += lRangeInc
            Loop While aDataMax > aScaleMax And lRangeIndex <= 15 And lRangeIndex >= 1

            If aDataMin < 0.5 * aDataMax And aDataMin >= 0.0# And aDataMin = 1 Then
                aScaleMin = 0.0#
            Else 'get next lowest mult of 10
                lDataRndlow = Rndlow(aDataMin)
                If lDataRndlow >= 0.0# Then
                    lRangeInc = -1
                    lRangeIndex = 15
                Else
                    lRangeInc = 1
                    lRangeIndex = 1
                End If
                Do
                    aScaleMin = lRange(lRangeIndex) * lDataRndlow
                    lRangeIndex += lRangeInc
                Loop While aDataMin < aScaleMin And lRangeIndex >= 1 And lRangeIndex <= 15
            End If
        Else 'logarithmic scale
            Dim lLogMin As Integer
            If aDataMin > 0.000000001 Then
                lLogMin = Fix(Math.Log10(aDataMin))
            Else
                'too small or neg value, set to -9
                lLogMin = -9
            End If
            If aDataMin < 1.0# Then
                lLogMin -= 1
            End If
            aScaleMin = 10.0# ^ lLogMin

            Dim lLogMax As Integer
            If aDataMax > 0.000000001 Then
                lLogMax = Fix(Math.Log10(aDataMax))
            Else
                'too small or neg value, set to -8
                lLogMax = -8
            End If
            If aDataMax > 1.0# Then
                lLogMax += 1
            End If
            aScaleMax = 10.0# ^ lLogMax

            If aScaleMin * 10000000.0# < aScaleMax Then
                'limit range to 7 cycles
                aScaleMin = aScaleMax / 10000000.0
            End If
        End If
    End Sub
End Module
