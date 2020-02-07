'This project includes utilities to write WASP INP files; based on atcWASP but simplified to remove dependencies on WRDB and SQLite.
Imports atcUtility

Public Class atcWASPProject

#Region "Public Variables and class instantiation..."
    Public Segments As atcWASPSegments
    Public SDate As Date = Date.MinValue
    Public EDate As Date = Date.MaxValue
    Public ModelType As Integer = 11 '2
    Public Version As Integer = 2
    Public Name As String = ""
    Public INPFileName As String = ""
    Public WASPTimeFunctions As New atcCollection

    'list of time series pointers associated with each time function for version 2.0
    Public TimeFunctionSeries() As clsTimeSeriesSelection
    Public WASPConstituents As Generic.List(Of clsWASPConstituent)
    Public WriteErrors As String = ""

    'the following are used in creating the wasp inp file
    Friend NumFlowFunc(5) As Integer
    Friend FlowPathList As New List(Of Integer) ' subset of Headwaters that actually have flowpath defined, zero-based

    ''' <summary>
    ''' Instantiate WaspProject class
    ''' </summary>
    Public Sub New()
        Name = ""
        Segments = New atcWASPSegments
        Segments.WASPProject = Me
    End Sub

#End Region

#Region "Utility routines for dividing and combining Wasp segments..."

    ''' <summary>
    ''' Determine whether specified segment is a boundary segment
    ''' </summary>
    ''' <param name="aSegment">Wasp segment to check</param>
    ''' <returns>True if is boundary segment</returns>
    Public Function IsBoundary(ByVal aSegment As atcWASPSegment) As Boolean
        Dim lBoundary As Boolean = False

        Dim lDownBoundary As Boolean = True
        For Each lSegment As atcWASPSegment In Segments
            If aSegment.DownID = lSegment.ID Then
                'this segment connects to one downstream
                lDownBoundary = False
                Exit For
            End If
        Next

        Dim lUpBoundary As Boolean = True
        For Each lSegment As atcWASPSegment In Segments
            If lSegment.DownID = aSegment.ID Then
                'an upstream segment connects to this one
                lUpBoundary = False
                Exit For
            End If
        Next

        If lUpBoundary Or lDownBoundary Then
            lBoundary = True
        End If

        Return lBoundary
    End Function

    ''' <summary>
    ''' Determine whether specified segment is a boundary segment
    ''' </summary>
    ''' <param name="aSegNum">Wasp segment number to check</param>
    ''' <returns>True if is boundary segment</returns>
    Public Function IsBoundary(ByVal aSegNum As Integer) As Boolean
        Return IsBoundary(Segments(aSegNum))
    End Function

#End Region

#Region "Wasp INP file writing routines..."

    ''' <summary>
    ''' Write Wasp INP file that contains all input data
    ''' </summary>
    ''' <param name="aFileName">Name of INP file to write</param>
    Public Function WriteINP(ByVal aFileName As String) As Boolean
        Dim lSW As IO.StreamWriter = Nothing

        Try
            'set inp file name
            INPFileName = aFileName
            WriteErrors = ""

            'Overwrite .inp file if it exists

            lSW = New IO.StreamWriter(INPFileName, False)

            Return writeInpIntro(lSW) AndAlso
                   writeInpSegs(lSW) AndAlso
                   writeInpPath(lSW) AndAlso
                   writeInpFlowFile(lSW) AndAlso
                   writeInpDispFile(lSW) AndAlso
                   writeInpBoundFile(lSW) AndAlso
                   writeInpLoadFile(lSW) AndAlso
                   writeInpTFuncFile(lSW) AndAlso
                   writeInpParamInfoFile(lSW) AndAlso
                   writeInpConstFile(lSW) AndAlso
                   writeInpIcFile(lSW)

        Catch ex As Exception
            Throw
            Return False
        Finally
            'final flush and close it
            If lSW IsNot Nothing Then
                lSW.Flush()
                lSW.Close()
            End If
        End Try
    End Function

    Private Function writeInpIntro(ByRef aSW As IO.StreamWriter) As Boolean

        'Dim lStartDateString As String = SDate.Month.ToString.PadLeft(5) & SDate.Day.ToString.PadLeft(5) & SDate.Year.ToString.PadLeft(5)
        Dim lJulianEnd As String = EDate.Subtract(SDate).TotalDays.ToString.PadLeft(10)

        aSW.WriteLine("{0,5}{1,7}               Module type               SYSFILE", ModelType, Version)
        'aSW.WriteLine(lStartDateString & "    0    0    0     Start date and time")
        'aSW.WriteLine(lStartDateString & "    0    0    0     Skip date and time")
        aSW.WriteLine("{0,5:MM}{0,5:dd}{0,5:yyyy}    0    0    0     Start date and time", SDate)
        aSW.WriteLine("{0,5:MM}{0,5:dd}{0,5:yyyy}    0    0    0     Skip date and time", SDate)
        aSW.WriteLine("{0,10:0.00}          Julian end time", EDate.Subtract(SDate).TotalDays)
        aSW.WriteLine("{0,5}               Number of Systems", WASPConstituents.Count)
        aSW.WriteLine("    0               Mass Balance Table Output")
        aSW.WriteLine("    1               Solution Technique Option")
        aSW.WriteLine("    0               Negative Solution Option")
        aSW.WriteLine("    0               Restart Option")
        aSW.WriteLine("    1               Time Optimization Option")
        aSW.WriteLine("    0               WQ Module Linkage Option")
        aSW.WriteLine("    0.9000          TOPT Factor")
        aSW.WriteLine("0.003000  1.000     Min and Max Timestep")
        aSW.WriteLine("    0               Number print intervals")
        'aSW.WriteLine("      0.00  1.000   Time and Print Interval")
        'aSW.WriteLine(lJulianEnd & "  1.000   Time and Print Interval")
        Dim lTemp As String = " "
        For lIndex As Integer = 1 To Me.WASPConstituents.Count
            lTemp = lTemp & "  0"
        Next
        aSW.WriteLine(lTemp)
        aSW.WriteLine("    0               Number output variables")
        aSW.WriteLine("    0               Number csv files")   'new entry in INP file per Tim Wool for version 1.2
        aSW.Flush()
        Return True
    End Function

    Private Function writeInpSegs(ByRef aSW As IO.StreamWriter) As Boolean
        aSW.WriteLine(Segments.Count.ToString.PadLeft(5) & "               Number of Segments                            SEGFILE")
        aSW.WriteLine("    0               Bed Volume Option")
        aSW.WriteLine("     0.000          Bed Compaction Time Step")
        aSW.WriteLine("   1.000   1.000    Volume Scale & Conversion Factor")
        aSW.WriteLine("Segment   SegName")
        'Write out the segment id number and their names in format: FORMAT(I5,5X,A40)
        For i As Integer = 1 To Segments.Count
            For Each lSegment As atcWASPSegment In Segments
                With lSegment
                    If .WaspID = i Then
                        aSW.WriteLine("{0,5}     {1,-40}", .WaspID, .Name)
                        Exit For
                    End If
                End With
            Next
        Next

        'Write out segment geometry information
        aSW.WriteLine("   Segment    BotSeg     iType    Volume     VMult      Vexp     DMult      Dexp    Length     Slope     Width     Rough  Depth_Q0")

        'print out segments in order of ID
        Dim lsegParams(12) As String
        For i As Integer = 1 To Segments.Count
            For Each lSegment As atcWASPSegment In Segments
                With lSegment
                    If .WaspID = i Then
                        Dim vol As Double = .Length * 1000.0 * .Width * .Depth * 0.5 ' crude assumption for Volume in m3
                        aSW.WriteLine("{0,10}{1,10}{2,10}{3,15:0.00}{4,10:0.000000}{5,10:0.000000}{6,10:0.000000}" &
                                      "{7,10:0.000000}{8,10:0.00}{9,10:0.00000}{10,10:0.0000}{11,10:0.0000}{12,10:0.0000}",
                                      .WaspID, 0, 1, vol, 0, 0, .Depth,
                                      0, .Length * 1000, .Slope, .Width, .Roughness, 0.001)
                        Exit For
                    End If
                End With
            Next
        Next
        aSW.Flush()
        Return True
    End Function

    Private Function UpstreamKey(ByVal aSeg As Integer) As String
        Dim lUpstreamKey As String = String.Empty
        Dim ltargetSegID As String = Segments.Item(aSeg).ID
        For Each lSegment As atcWASPSegment In Segments
            If lSegment.DownID = ltargetSegID Then
                lUpstreamKey = lSegment.ID
                Exit For
            End If
        Next
        Return lUpstreamKey
    End Function

    Private Function GenerateFlowPaths() As Generic.Dictionary(Of Integer, String)
        FlowPathList.Clear() 'start anew
        Dim lflowpaths As New Generic.Dictionary(Of Integer, String)

        'Build a list of headwater segments
        Dim lHeadwatersIndexes As New List(Of Integer)
        For i As Integer = 0 To Segments.Count - 1
            If UpstreamKey(i) = String.Empty Then
                lHeadwatersIndexes.Add(i + 1)
            End If
        Next

        'sort the list in assending order
        lHeadwatersIndexes.Sort() ' assuming by default it is in ascending order

        'Set up a collection to hold the from-to pairs
        Dim ldoneFlowpaths As New List(Of String)
        Dim lProblem As String = String.Empty
        Dim loutletSegID As String = Segments.DownstreamKey(lProblem)
        Dim loutletSegWASPID As Integer
        Dim lOutletSegName As String = ""
        If lProblem = String.Empty Then ' getting the outlet seg succeed
            loutletSegWASPID = Segments.Item(loutletSegID).WaspID
            lOutletSegName = Segments.Item(loutletSegID).Name
        Else
            loutletSegWASPID = 1
        End If

        'Construct the main flowpath
        Dim lnumFlowFunc As Integer = 0
        Dim lnumflowroutes As Integer = 0
        Dim lflowfraction As String = "1.00"
        Dim lIndexOfTopOfMainFlowpath As Integer = lHeadwatersIndexes(0) - 1

        'need a new algorithm to find the top of the main flowpath -- (Dan river shows the need) 
        'by longest flowpath?  not necessarily
        'by largest drainage area?  not necessarily
        'by greatest number of segments?  not necessarily
        'by name -- good enough for now
        For i As Integer = 0 To lHeadwatersIndexes.Count - 1
            If Segments.Item(lHeadwatersIndexes(i) - 1).Name = lOutletSegName Then
                lIndexOfTopOfMainFlowpath = lHeadwatersIndexes(i) - 1
            End If
        Next i


        Dim lthisFlowfunction As New System.Text.StringBuilder
        lthisFlowfunction.AppendLine(Space(2) & Segments.Item(lIndexOfTopOfMainFlowpath).Name)
        Dim ltemp As New System.Text.StringBuilder
        ltemp.AppendLine("0".PadLeft(4) & Segments.Item(lIndexOfTopOfMainFlowpath).WaspID.ToString.PadLeft(4) & Space(11) & lflowfraction)
        ldoneFlowpaths.Add("0".PadLeft(4) & Segments.Item(lIndexOfTopOfMainFlowpath).WaspID.ToString.PadLeft(4))
        lnumflowroutes += 1

        Dim lend As Boolean = False
        Dim lthisSeg As atcWASPSegment = Segments.Item(lIndexOfTopOfMainFlowpath)
        Dim lthisPair As String = String.Empty
        While Not lend
            If lthisSeg.ID = loutletSegID Then
                lthisPair = lthisSeg.WaspID.ToString.PadLeft(4) & "0".PadLeft(4)
                ltemp.Append(lthisPair & Space(11) & lflowfraction)
                ldoneFlowpaths.Add(lthisPair)
                lnumflowroutes += 1
                lend = True
            Else
                Dim ldownID As String = lthisSeg.DownID
                lthisPair = lthisSeg.WaspID.ToString.PadLeft(4) & Segments.Item(ldownID).WaspID.ToString.PadLeft(4)
                ltemp.AppendLine(lthisPair & Space(11) & lflowfraction)
                lthisSeg = Segments.Item(ldownID)
                ldoneFlowpaths.Add(lthisPair)
                lnumflowroutes += 1
                lend = False
            End If
        End While

        If Not ltemp.ToString = "" Then
            'Increment the total flow function count
            'write up routes count and info
            'clear the ltemp content for subsequent flow functions, reset lnumflowroutes
            'Add this flow function to the overall list
            lnumFlowFunc += 1
            lthisFlowfunction.AppendLine(Space(3) & lnumflowroutes.ToString)
            lthisFlowfunction.Append(ltemp.ToString)
            lflowpaths.Add(lIndexOfTopOfMainFlowpath + 1, lthisFlowfunction.ToString)
            FlowPathList.Add(lIndexOfTopOfMainFlowpath)
            lnumflowroutes = 0
            ltemp = New System.Text.StringBuilder
        End If

        'Do the rest of the headwaters
        For i As Integer = 0 To lHeadwatersIndexes.Count - 1
            If lHeadwatersIndexes(i) - 1 <> lIndexOfTopOfMainFlowpath Then 'already did this one, do the rest
                lend = False
                lthisSeg = Segments.Item(lHeadwatersIndexes(i) - 1)
                ltemp.Append("0".PadLeft(4) & Segments.Item(lHeadwatersIndexes(i) - 1).WaspID.ToString.PadLeft(4) & Space(11) & lflowfraction)
                ldoneFlowpaths.Add("0".PadLeft(4) & Segments.Item(lHeadwatersIndexes(i) - 1).WaspID.ToString.PadLeft(4))
                lnumflowroutes += 1
                lthisFlowfunction = New System.Text.StringBuilder
                While Not lend
                    If lthisSeg.ID = loutletSegID Then
                        lthisPair = lthisSeg.WaspID.ToString.PadLeft(4) & "0".PadLeft(4)
                        If ldoneFlowpaths.Contains(lthisPair) Then
                            lend = True
                            Continue While
                        Else
                            ldoneFlowpaths.Add(lthisPair)
                        End If
                        ltemp.Append(lthisPair & Space(11) & lflowfraction)
                        ldoneFlowpaths.Add(lthisSeg.WaspID.ToString.PadLeft(4) & "0".PadLeft(4))
                        lnumflowroutes += 1
                        lend = True
                    Else
                        Dim ldownID As String = lthisSeg.DownID
                        lthisPair = lthisSeg.WaspID.ToString.PadLeft(4) & Segments.Item(ldownID).WaspID.ToString.PadLeft(4)
                        If ldoneFlowpaths.Contains(lthisPair) Then
                            lend = True
                            Continue While
                        Else
                            ldoneFlowpaths.Add(lthisPair)
                        End If
                        ltemp.Append(vbCrLf)
                        ltemp.Append(lthisSeg.WaspID.ToString.PadLeft(4) & Segments.Item(ldownID).WaspID.ToString.PadLeft(4) & Space(11) & lflowfraction)
                        lnumflowroutes += 1
                        lthisSeg = Segments.Item(ldownID)
                        lend = False
                    End If
                End While

                If Not ltemp.ToString = "" OrElse Not lnumflowroutes = 0 Then
                    'Increment the total flow function count
                    'write up routes count and info
                    'clear the ltemp content for subsequent flow functions
                    'Add this flow function to the overall list
                    lnumFlowFunc += 1
                    Dim lPathName As String = Segments.Item(lHeadwatersIndexes(i) - 1).Name
                    If IsNumeric(lPathName) Then
                        lPathName = "FlowPath " & lnumFlowFunc.ToString
                    End If
                    lthisFlowfunction.AppendLine(Space(2) & lPathName)
                    lthisFlowfunction.AppendLine(Space(3) & lnumflowroutes.ToString)
                    lthisFlowfunction.Append(ltemp.ToString)
                    lflowpaths.Add(lHeadwatersIndexes(i), lthisFlowfunction.ToString)
                    FlowPathList.Add(lHeadwatersIndexes(i) - 1)
                    lnumflowroutes = 0
                    ltemp = New System.Text.StringBuilder
                End If
            End If
        Next
        NumFlowFunc(0) = lnumFlowFunc ' The first flow field is always for water flow
        Return lflowpaths
    End Function

    Private Function writeInpPath(ByRef aSW As IO.StreamWriter) As Boolean
        'TODO: flow field information, need interface

        aSW.WriteLine("{0,5}               Flow Pathways                                 PATHFILE", 4)
        aSW.WriteLine("{0,5}               Number of flow fields", 6) ' this can be hardcoded here as only 6 constant fields
        aSW.WriteLine("     Flow Field   1")

        'Figure out the flowpaths:
        'this flow path is the base flow network that is used later for other things
        Dim lflowfuncfield1 As Generic.Dictionary(Of Integer, String)
        lflowfuncfield1 = GenerateFlowPaths()
        aSW.WriteLine("{0,5}               Number of Flow Functions for Flow Field", lflowfuncfield1.Count)
        For Each lString As String In lflowfuncfield1.Values
            aSW.WriteLine(lString)
        Next

        'For now the 2-6 fields' flow func can be hard-coded here, later done by functions
        For lflowField As Integer = 2 To 6
            aSW.WriteLine("     Flow Field   {0}", lflowField.ToString)
            aSW.WriteLine("{0,5}               Number of Flow Functions for Flow Field", 0)
            NumFlowFunc(lflowField - 1) = 0 'TODO: NumFlowFunc when this is done dynamically, this needs to be changed
        Next
        aSW.Flush()
        Return True
    End Function

    Private Function writeInpFlowFile(ByRef aSW As IO.StreamWriter) As Boolean
        aSW.WriteLine("{0,5}               Number of Flow Fields                          FLOWFILE", 6)
        Dim linflowCat(5) As String
        linflowCat(0) = " Surface Water Flow Field      "
        linflowCat(1) = " Porewater Flow Field          "
        linflowCat(2) = " Solids - 1                    "
        linflowCat(3) = " Solids - 2                    "
        linflowCat(4) = " Solids - 3                    "
        linflowCat(5) = " Evap/Precip Flow Field        "

        For i As Integer = 0 To 5  ' Loop through the 6 flow fields
            aSW.WriteLine(linflowCat(i))
            'Assuming inflow time-value correspond to the set up for flowpath
            'so 'NumFlowFunc' array can be used for these different, yet related, sections
            aSW.WriteLine("{0,5}               Number of inflows", NumFlowFunc(i))
            If NumFlowFunc(i) = 0 Then
                Continue For
            End If
            aSW.WriteLine("   1.000   1.000    Flow Scale & Conversion Factors")

            For j As Integer = 0 To FlowPathList.Count - 1
                Dim seg As atcWASPSegment = Segments.Item(FlowPathList(j))
                aSW.WriteLine(seg.Name.PadRight(30))
                With seg.FlowTimeSeries.GetTimeSeries(Me, seg.ID_Name, "Input Flows")
                    If seg.FlowTimeSeries.SelectionType <> clsTimeSeriesSelection.enumSelectionType.None And .Count = 0 Then
                        WriteErrors &= String.Format("Empty time series was returned for segment {0} for {1}; specification was: {2}", seg.Name, "flow", seg.FlowTimeSeries.ToFullString) & vbCr
                    End If
                    aSW.WriteLine("{0,5}               Number of time-flow values in {1}", .Count - 1, seg.FlowTimeSeries.ToFullString)
                    For t As Integer = 1 To .Count - 1
                        aSW.WriteLine(String.Format("{0,8:0.000} {1,9:0.00000}", .Keys(t - 1).Subtract(SDate).TotalDays, .Values(t)))
                    Next
                End With
            Next
        Next
        aSW.Flush()
        Return True
    End Function

    Private Function writeInpDispFile(ByRef aSW As IO.StreamWriter) As Boolean
        'TODO: deal with this part later
        aSW.WriteLine("{0,5}               Number of Exchange Fields                     DISPFILE", 0)
        aSW.Flush()
        Return True
    End Function

    Private Function writeInpBoundFile(ByRef aSW As IO.StreamWriter) As Boolean
        aSW.WriteLine("{0,5}               Number of Systems                             BOUNDFILE", WASPConstituents.Count)
        'figure out the number of boundaries
        Dim NumBound As Integer = 0
        For Each lSegment As atcWASPSegment In Segments
            If IsBoundary(lSegment) Then
                NumBound += 1
            End If
        Next
        For c As Integer = 0 To WASPConstituents.Count - 1
            aSW.WriteLine("  {0,-30}", WASPConstituents(c).Description)
            aSW.WriteLine("{0,5}               Number of boundaries", NumBound)
            aSW.WriteLine("   1.000   1.000    Boundary Scale & Conversion Factors")
            For i As Integer = 1 To Segments.Count
                For Each Seg As atcWASPSegment In Segments
                    With Seg
                        If .WaspID = i Then
                            If IsBoundary(Seg) Then
                                aSW.WriteLine("{0,5}               Boundary Segment Number", .WaspID)
                                With Seg.BoundTimeSeries(c).GetTimeSeries(Me, Seg.ID_Name, WASPConstituents(c).Description)
                                    If Seg.BoundTimeSeries(c).SelectionType <> clsTimeSeriesSelection.enumSelectionType.None And .Count = 0 Then
                                        WriteErrors &= String.Format("Empty time series was returned for segment {0} for {1}; specification was: {2}", Seg.Name, WASPConstituents(c), Seg.BoundTimeSeries(c).ToFullString) & vbCr
                                    End If
                                    If .Count < 3 Then
                                        aSW.WriteLine("{0,5}               Number of time-concentration values in {1}", 2, "--Empty Time Series--")
                                        aSW.WriteLine("{0,8:0.000} {1,9:0.00000}", 0, 0)
                                        aSW.WriteLine("{0,8:0.000} {1,9:0.00000}", EDate.Subtract(SDate).TotalDays, 0)
                                    Else
                                        aSW.WriteLine("{0,5}               Number of time-concentration values in {1}", .Count - 1, Seg.FlowTimeSeries.ToFullString)
                                        For t As Integer = 1 To .Count - 1
                                            aSW.WriteLine("{0,8:0.000} {1,9:0.00000}", .Keys(t - 1).Subtract(SDate).TotalDays, .Values(t))
                                        Next
                                    End If
                                End With
                                Exit For
                            End If
                        End If
                    End With
                Next
            Next
        Next
        aSW.Flush()
        Return True
    End Function

    Private Function writeInpLoadFile(ByRef aSW As IO.StreamWriter) As Boolean
        aSW.WriteLine("{0,5}               NPS Input Option (0=No, 1=Yes)                LOADFILE", 0)
        aSW.WriteLine("{0,5}               Number of Systems", WASPConstituents.Count)
        For c As Integer = 0 To WASPConstituents.Count - 1
            aSW.WriteLine("  {0,-30}", WASPConstituents(c).Description)
            'figure out the number of loadings
            Dim NumLoads As Integer = 0
            For Each Seg As atcWASPSegment In Segments
                If Seg.LoadTimeSeries(c).SelectionType <> clsTimeSeriesSelection.enumSelectionType.None Then NumLoads += 1
            Next
            aSW.WriteLine("{0,5}               Number of Loadings", NumLoads)
            If NumLoads > 0 Then
                aSW.WriteLine("   1.000   1.000    Loading Scale & Conversion Factors")
                For Each Seg As atcWASPSegment In Segments
                    With Seg
                        If Seg.LoadTimeSeries(c).SelectionType <> clsTimeSeriesSelection.enumSelectionType.None Then
                            aSW.WriteLine("{0,5}               Loading Segment Number", .WaspID)
                            With Seg.LoadTimeSeries(c).GetTimeSeries(Me, Seg.ID_Name, WASPConstituents(c).Description)
                                If Seg.LoadTimeSeries(c).SelectionType <> clsTimeSeriesSelection.enumSelectionType.None And .Count = 0 Then
                                    WriteErrors &= String.Format("Empty time series was returned for segment {0} for {1}; specification was: {2}", Seg.Name, WASPConstituents(c), Seg.LoadTimeSeries(c).ToFullString) & vbCr
                                End If
                                aSW.WriteLine("{0,5}               Number of time-loading values in {1}", .Count - 1, Seg.FlowTimeSeries.ToFullString)
                                For t As Integer = 1 To .Count - 1
                                    aSW.WriteLine("{0,8:0.000} {1,9:0.00000}", .Keys(t - 1).Subtract(SDate).TotalDays, .Values(t))
                                Next
                            End With
                        End If
                    End With
                Next
            End If
        Next
        aSW.Flush()
        Return True
    End Function

    Private Function writeInpTFuncFile(ByRef aSW As IO.StreamWriter) As Boolean
        Dim NumTimeFunc As Integer = 0
        For i As Integer = 0 To WASPTimeFunctions.Count - 1
            If TimeFunctionSeries(i).SelectionType <> clsTimeSeriesSelection.enumSelectionType.None Then NumTimeFunc += 1
        Next

        aSW.WriteLine("{0,5}               Number of Time Functions                      TFUNCFILE", NumTimeFunc)

        For i As Integer = 0 To WASPTimeFunctions.Count - 1
            With WASPTimeFunctions(i)
                If TimeFunctionSeries(i).SelectionType <> clsTimeSeriesSelection.enumSelectionType.None Then
                    aSW.WriteLine("{0,5}                Time Function ID Number", .FunctionID)
                    aSW.WriteLine("  {0}", .Description)  'use to write time function name
                    With TimeFunctionSeries(i).GetTimeSeries(Me, "Time Functions", .Description)
                        If TimeFunctionSeries(i).SelectionType <> clsTimeSeriesSelection.enumSelectionType.None And .Count = 0 Then
                            WriteErrors &= String.Format("Empty time series was returned for {0}; specification was: {1}", WASPTimeFunctions(i).Description, TimeFunctionSeries(i).ToFullString) & vbCr
                        End If
                        aSW.WriteLine("{0,5}               Number of time-function values in {1}", .Count, TimeFunctionSeries(i).ToFullString)
                        For t As Integer = 0 To .Count - 1
                            aSW.WriteLine("{0,8:0.000} {1,9:0.00000}", .Keys(t).Subtract(SDate).TotalDays, .Values(t))
                        Next
                    End With
                End If
            End With
        Next
        aSW.Flush()
        Return True
    End Function

    Private Function writeInpParamInfoFile(ByRef aSW As IO.StreamWriter) As Boolean
        'TODO: deal with this part later
        aSW.WriteLine("{0,5}               Number of Segments                             PARAMINFO", Segments.Count)
        aSW.WriteLine("{0,5}               Number of Segment Parameters", 0)
        aSW.Flush()
        Return True
    End Function

    Private Function writeInpConstFile(ByRef aSW As IO.StreamWriter) As Boolean
        'TODO: deal with this part later
        aSW.WriteLine("{0,5}               Number of Constants                            CONSTFILE", 0)
        aSW.Flush()
        Return True
    End Function

    Private Function writeInpIcFile(ByRef aSW As IO.StreamWriter) As Boolean
        aSW.WriteLine("{0,5}               Number of Systems                             ICFILE", WASPConstituents.Count)
        aSW.WriteLine("{0,5}               Number of Segments", Segments.Count)
        For lIndex As Integer = 1 To WASPConstituents.Count
            aSW.WriteLine("     Initial conditions for system" & lIndex.ToString.PadLeft(3) & " " & WASPConstituents(lIndex - 1).Description)
            aSW.WriteLine("    0               Solids Transport Field")
            aSW.WriteLine("     1.000          Solids Density, g/mL")
            aSW.WriteLine("     10000.0000     Maximum Allowed Concentration")
            aSW.WriteLine("  Seg   Conc   DissF")
            For lSegIndex As Integer = 1 To Segments.Count
                aSW.WriteLine("   {0}  0.000000  1.00000", lSegIndex)
            Next
        Next
        aSW.Flush()
        Return True
    End Function

#End Region

End Class

''' <summary>
''' Class to hold contents of "Systems" table in WASP database which identifies all constituents for each model type
''' Use in dictionary and select by ModelName
''' </summary>
Public Class clsWASPConstituent
    Public Description As String
    Public ConcUnits As String
    Public LoadUnits As String
    Sub New(ByVal aDescription As String, ByVal aConcUnits As String, ByVal aLoadUnits As String)
        Description = aDescription
        ConcUnits = aConcUnits
        LoadUnits = aLoadUnits
    End Sub
End Class
