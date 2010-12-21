Imports atcdata
Imports atcutility
Imports System.Windows.Forms

Public Class frmDFLOWResults

    Private pTimeseriesGroup As atcTimeseriesGroup  ' Private copy of dataset collections
    Private pCBList As CheckedListBox   ' Private copy of user selections from dataset
    Private pInitializing As Boolean    ' Semaphore
    Private pRow As Integer = -1        ' Row last pressed down in table
    Private pColumn As Integer = -1     ' Col last pressed down in table

    Private pExcursionCountArray As New ArrayList
    Private pExcursionsArray As New ArrayList
    Private pClustersArray As New ArrayList

    Private Shared pDateFormat As New atcDateFormat

    Private pIsBatch As Boolean

    Public Sub New(Optional ByVal aTimeseriesGroup As atcData.atcTimeseriesGroup = Nothing, Optional ByVal aIsBatch As Boolean = False)

        MyBase.New()
        pInitializing = True
        pIsBatch = aIsBatch



        ' ----- Check for existence of data group (set of time series files)

        If aTimeseriesGroup Is Nothing Then
            pTimeseriesGroup = New atcTimeseriesGroup
        Else
            pTimeseriesGroup = aTimeseriesGroup
        End If

        InitializeComponent() 'required by Windows Form Designer

        ' ---- Populate Analysis menu 
        'Dim DisplayPlugins As ICollection = atcDataManager.GetPlugins(GetType(atcDataDisplay))
        'For Each lDisp As atcDataDisplay In DisplayPlugins
        '    Dim lMenuText As String = lDisp.Name
        '    If lMenuText.StartsWith("Analysis::") Then lMenuText = lMenuText.Substring(10)
        '    mnuAnalysis.MenuItems.Add(lMenuText, New EventHandler(AddressOf mnuAnalysis_Click))
        'Next


        ' ----- Ask user to select datasets for analysis

        If pTimeseriesGroup.Count = 0 Then
            pTimeseriesGroup = atcDataManager.UserSelectData("Specify data sets for DFLOW analysis", pTimeseriesGroup, Nothing, True)
        End If

        ' ----- Finish by getting DFLOW arguments

        pCBList = New CheckedListBox

        If pIsBatch Then

            pInitializing = False
            RefreshCalcs()

        Else

            If pTimeseriesGroup.Count = 0 Or Not UserSpecifyDFLOWArgs() Then

                'user declined to specify Data
                Me.DialogResult = Windows.Forms.DialogResult.Cancel

            Else

                pInitializing = False
                RefreshCalcs()

            End If

        End If

    End Sub

    Private Sub RefreshCalcs()

        ' ----- Advisory labels

        Dim pLastDay() As Integer = {31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31}
        Dim pMonth3 As String = "JanFebMarAprMayJunJulAugSepOctNovDec"
        Dim lNaN As Double = GetNaN()

        Dim pFirstyear = 1700
        If fFirstYear > 0 Then pfirstyear = fFirstYear

        Dim pLastYear = 2100
        If fLastYear > 0 Then pLastYear = fLastYear

        Dim pEndDay = fEndDay + 1
        Dim pEndMonth = fEndMonth
        If fEndDay > pLastDay(pEndMonth - 1) Then
            pEndDay = 1
            If pEndMonth = 12 Then
                pEndMonth = 1
                pLastYear = pLastYear + 1
            End If
        End If

        If (fStartMonth = fEndMonth And fStartDay = fEndDay + 1) Or _
           ((fStartMonth - fEndMonth) Mod 12 = 1 And fStartDay = 1 And fEndDay = pLastDay(fEndMonth - 1)) Then
            lblSeasons.Text = "Climatic year defined as " & pMonth3.Substring(3 * fStartMonth - 3, 3) & " " & fStartDay & " - " & pMonth3.Substring(3 * fEndMonth - 3, 3) & " " & fEndDay & "."
        Else
            lblSeasons.Text = "Season defined as " & pMonth3.Substring(3 * fStartMonth - 3, 3) & " " & fStartDay & " - " & pMonth3.Substring(3 * fEndMonth - 3, 3) & " " & fEndDay & _
                          ". Biological flow is calculated for full climatic year starting at " & pMonth3.Substring(3 * fStartMonth - 3, 3) & " " & fStartDay & "."
        End If

        If fFirstYear <= 0 And fLastYear <= 0 Then
            lblYears.Text = "All available years of data are included in analysis."
        ElseIf fFirstYear <= 0 Then
            lblYears.Text = "All available data through " & pMonth3.Substring(3 * fEndMonth - 3, 3) & " " & fEndDay & ", " & fLastYear & " are included in analysis."
        ElseIf fLastYear <= 0 Then
            If (fStartMonth < fEndMonth) Or (fStartMonth = fEndMonth And fStartDay < fEndDay) Then
                lblYears.Text = "All available data from " & pMonth3.Substring(3 * fStartMonth - 3, 3) & " " & fStartDay & ", " & fFirstYear - 1 & " are included in analysis."
            Else
                lblYears.Text = "All available data from " & pMonth3.Substring(3 * fStartMonth - 3, 3) & " " & fStartDay & ", " & fFirstYear & " are included in analysis."
            End If
        Else
            If (fStartMonth < fEndMonth) Or (fStartMonth = fEndMonth And fStartDay < fEndDay) Then
                lblYears.Text = "All available data from " & pMonth3.Substring(3 * fStartMonth - 3, 3) & " " & fStartDay & ", " & fFirstYear - 1 & _
                                " through " & pMonth3.Substring(3 * fEndMonth - 3, 3) & " " & fEndDay & ", " & fLastYear & " are included in analysis."
            Else
                lblYears.Text = "All available data from " & pMonth3.Substring(3 * fStartMonth - 3, 3) & " " & fStartDay & ", " & fFirstYear & _
                                " through " & pMonth3.Substring(3 * fEndMonth - 3, 3) & " " & fEndDay & ", " & fLastYear & " are included in analysis."
            End If
        End If


        ' ----- Count number of items checked in the listbox

        Dim lTotalItems As Integer = 0
        Dim lDSIndex As Integer
        For lDSIndex = 0 To pCBList.Items.Count - 1
            If pCBList.GetItemChecked(lDSIndex) Then
                lTotalItems = lTotalItems + 1
            End If
        Next



        ' ----- Build data source for results

        Dim ladsResults As New atcControls.atcGridSource
        With ladsResults

            .Columns = 15
            .FixedColumns = 4
            .Rows = lTotalItems + 1
            .FixedRows = 1

            .CellValue(0, 0) = "Gage"
            .CellValue(0, 1) = "Period"
            .CellValue(0, 2) = "Days in Record"
            .CellValue(0, 3) = "Zero/Missing"
            .CellValue(0, 4) = fBioPeriod & "B" & fBioYears
            .CellValue(0, 5) = "Percentile"
            .CellValue(0, 6) = "Excur per 3 yr"

            Select Case fNonBioType
                Case 0 : .CellValue(0, 7) = fAveragingPeriod & "Q" & fReturnPeriod
                Case 1 : .CellValue(0, 7) = "Explicit Q"
                Case 2 : .CellValue(0, 7) = "Percentile Q"
            End Select

            .CellValue(0, 8) = "Percentile"
            .CellValue(0, 9) = "Excur per 3 yr"
            .CellValue(0, 10) = fAveragingPeriod & "Qy Type"
            .CellValue(0, 11) = "xQy"
            .CellValue(0, 12) = "Percentile"
            .CellValue(0, 13) = "Harmonic"
            .CellValue(0, 14) = "Percentile"

            Dim lColumn As Integer
            For lColumn = 0 To 14
                .CellColor(0, lColumn) = Me.BackColor
                .Alignment(0, lColumn) = atcControls.atcAlignment.HAlignCenter
            Next

            Dim lRow As Integer
            For lRow = 1 To lTotalItems
                For lColumn = 0 To 3
                    .CellColor(lRow, lColumn) = Me.BackColor
                Next
            Next

            .Rows = lTotalItems + 1

        End With


        ' ----- Loop over the items in the listbox

        Dim lItemIdx As Integer = 0

        Dim lfrmProgress As New frmDFLOWProgress
        lfrmProgress.ProgressBar1.Value = 0
        lfrmProgress.Label1.Text = ""
        lfrmProgress.Show(Me)

        pExcursionCountArray.Clear()
        pExcursionsArray.Clear()
        pClustersArray.Clear()

        For lDSIndex = 0 To pCBList.Items.Count - 1
            If pCBList.GetItemChecked(lDSIndex) Then

                With pDateFormat
                    .IncludeHours = False
                    .IncludeMinutes = False
                    .IncludeSeconds = False
                End With

                ' ===== Quick trim 

                Dim lHydrologicTS As atcTimeseries = pTimeseriesGroup(lDSIndex)
                Dim lHydrologicTS2 As atcTimeseries = SubsetByDateBoundary(lHydrologicTS, fStartMonth, fStartDay, Nothing, pFirstyear, pLastYear, pEndMonth, pEndDay)
                Dim lFirstDate As Double = lHydrologicTS2.Attributes.GetValue("start date")
                Dim lHydrologicDS As atcDataSet = lHydrologicTS2
                Dim lSYear As Integer = (pDateFormat.JDateToString(lHydrologicDS.Attributes.GetValue("start date"))).Substring(0, 4)
                Dim lEYear As Integer = (pDateFormat.JDateToString(lHydrologicDS.Attributes.GetValue("end date"))).Substring(0, 4)
                Dim lYears As Integer = lEYear - lSYear
                Dim lTS As Double() = lHydrologicTS2.Values

                pTimeseriesGroup(lDSIndex).Attributes.SetValue("xBy start date", lFirstDate)

                ' ===== Calculate hydrologic design flow lxQy

                Dim lxQy As Double
                lfrmProgress.ProgressBar1.Value = Int(100 * (3 * lItemIdx + 1) / (3 * lTotalItems))
                Select Case fNonBioType

                    Case 0

                        lfrmProgress.Label1.Text = (1 + lItemIdx) & " of " & lTotalItems & " - " & fAveragingPeriod & "Q" & fReturnPeriod
                        Application.DoEvents()

                        If lYears >= fReturnPeriod Then
                            lxQy = xQy(fAveragingPeriod, fReturnPeriod, lHydrologicDS)
                        Else
                            lxQy = lNaN
                        End If

                    Case 1

                        lfrmProgress.Label1.Text = (1 + lItemIdx) & " of " & lTotalItems & " - explicit flow"
                        Application.DoEvents()
                        lxQy = fExplicitFlow

                    Case 2

                        lfrmProgress.Label1.Text = (1 + lItemIdx) & " of " & lTotalItems & " - " & fPercentile & "th percentile"
                        Application.DoEvents()
                        modTimeseriesMath.ComputePercentile(lHydrologicTS2, fPercentile)
                        lxQy = lHydrologicTS2.Attributes.GetValue("%" & Format(fPercentile, "00.####"))

                End Select

                ' ===== Create 4-day running average for start of xBy excursion analysis - 

                Dim lTimeSeries As atcTimeseries = pTimeseriesGroup(lDSIndex)
                Dim lTimeSeries2 As atcTimeseries = SubsetByDateBoundary(lTimeSeries, fStartMonth, fStartDay, Nothing, pFirstyear, pLastYear, pEndMonth, pEndDay)

                lTS = lTimeSeries2.Values
                lTS(0) = lNaN

                Dim lTSN As Double()
                ReDim lTSN(UBound(lTS))

                Dim lSum As Double = 0
                Dim lN As Integer = 0

                Dim lI As Integer
                For lI = 0 To UBound(lTS) - 1
                    If Double.IsNaN(lTS(lI)) Then
                        lSum = 0
                        lN = 0
                    Else
                        lSum = lSum + lTS(lI)
                        lN = lN + 1
                        If lN > 4 Then
                            lSum = lSum - lTS(lI - 4)
                        End If
                    End If

                    If lN >= 4 Then
                        lTSN(lI) = lSum / 4
                    Else
                        lTSN(lI) = lNaN
                    End If
                Next

                Dim lExcursions As New ArrayList
                Dim lClusters As New ArrayList
                Dim lExcQ = CountExcursions(lxQy, 1, 120, 5, lTSN, lExcursions, lClusters)
                lExcursions.Clear()
                lClusters.Clear()


                ' ===== Calculate xBy (only defined for full-year)

                lfrmProgress.ProgressBar1.Value = Int(100 * (3 * lItemIdx + 2) / (3 * lTotalItems))
                lfrmProgress.Label1.Text = (1 + lItemIdx) & " of " & lTotalItems & " - " & fBioPeriod & "B" & fBioYears
                Application.DoEvents()

                ' ----- 1. Create n-day running average from current time series

                lSum = 0
                lN = 0

                For lI = 0 To UBound(lTS) - 1

                    If Double.IsNaN(lTS(lI)) Then
                        lSum = 0
                        lN = 0
                    Else
                        lSum = lSum + lTS(lI)
                        lN = lN + 1
                        If lN > fBioPeriod Then
                            lSum = lSum - lTS(lI - fBioPeriod)
                        End If
                    End If

                    If lN >= fBioPeriod Then
                        lTSN(lI) = lSum / fBioPeriod
                    Else
                        lTSN(lI) = lNaN
                    End If
                Next

                ' ----- 2. Get initial guess

                Dim lxBy As Double = xQy(fBioPeriod, fBioYears, pTimeseriesGroup(lDSIndex))

                ' ----- 3. Do xBy calculation

                Dim lExcursionCount As Integer


                lxBy = xBy(lxBy, fBioPeriod, fBioYears, fBioCluster, fBioExcursions, lTSN, lExcursionCount, lExcursions, lClusters, lfrmProgress)
                Dim pAttrName As String = fBioPeriod & "B" & fBioYears
                lHydrologicTS.Attributes.SetValue(pAttrName, lxBy)

                pExcursionCountArray.Add(lExcursionCount)
                pExcursionsArray.Add(lExcursions)
                pClustersArray.Add(lClusters)


                ' ===== If appropriate, calculate equivalent xQy for this xBy

                Dim lEquivalentxQy As Double = 0
                Dim lReturnPeriod As Integer


                If fNonBioType > 0 Then

                    lEquivalentxQy = lNaN

                Else


                    lfrmProgress.ProgressBar1.Value = Int(100 * (3 * lItemIdx + 3) / (3 * lTotalItems))
                    lfrmProgress.Label1.Text = (1 + lItemIdx) & " of " & lTotalItems & " - xQy"
                    Application.DoEvents()

                    lEquivalentxQy = xQy(fAveragingPeriod, lYears, pTimeseriesGroup(lDSIndex))
                    If lEquivalentxQy > lxBy Then

                        lReturnPeriod = lYears

                    Else

                        If lxQy > lxBy Then
                            lReturnPeriod = fReturnPeriod
                        Else
                            lReturnPeriod = 1
                        End If

                        lEquivalentxQy = lxBy
                        While lEquivalentxQy >= lxBy And lReturnPeriod < lYears

                            lReturnPeriod = lReturnPeriod + 1
                            lEquivalentxQy = xQy(fAveragingPeriod, lReturnPeriod, pTimeseriesGroup(lDSIndex))

                            lfrmProgress.Label1.Text = (1 + lItemIdx) & " of " & lTotalItems & " - xQy (" & lReturnPeriod & " of up to " & lYears & ")"
                            Application.DoEvents()

                        End While

                    End If

                End If

                ' ===== Harmonic mean of flows

                Dim lHFlow As Double = 0
                Dim lNH As Integer = 0

                For lI = 0 To UBound(lTS) - 1
                    If (Not Double.IsNaN(lTS(lI))) And (lTS(lI) <> 0) Then
                        lNH = lNH + 1
                        lHFlow = lHFlow + 1 / lTS(lI)
                    End If
                Next

                If lHFlow <> 0 Then
                    lHFlow = lNH / lHFlow
                End If


                ' ===== Calculate percentiles

                Dim lNMiss As Integer = 0
                Dim lNZero As Integer = 0
                Dim lNExc As Integer = 0
                Dim lNExcB As Integer = 0
                Dim lNExcBQ As Integer = 0
                Dim lNExcHF As Integer = 0

                For lI = 0 To UBound(lTS) - 1
                    If Double.IsNaN(lTS(lI)) Then
                        lNMiss = lNMiss + 1
                    Else
                        If lTS(lI) = 0 Then
                            lNZero = lNZero + 1
                        End If
                        If lTS(lI) < lxQy Then
                            lNExc = lNExc + 1
                        End If
                        If lTS(lI) < lxBy Then
                            lNExcB = lNExcB + 1
                        End If
                        If lTS(lI) < lEquivalentxQy Then
                            lNExcBQ = lNExcBQ + 1
                        End If
                        If lTS(lI) < lHFlow Then
                            lNExcHF = lNExcHF + 1
                        End If
                    End If
                Next

                ' ===== Store results

                ladsResults.CellValue(lItemIdx + 1, 0) = lHydrologicDS.Attributes.GetFormattedValue("Location") & " - " & lHydrologicDS.Attributes.GetFormattedValue("STANAM")

                ' ----- Next three values are corrected by one day to account for current (3/2008) behavior of time series trimming

                ladsResults.CellValue(lItemIdx + 1, 1) = pDateFormat.JDateToString(1 + lHydrologicDS.Attributes.GetValue("start date")) & " - " & _
                                                         pDateFormat.JDateToString(lHydrologicDS.Attributes.GetValue("end date"))
                ladsResults.CellValue(lItemIdx + 1, 2) = Format(UBound(lTS) - 1, "#,##0") & " "
                ladsResults.Alignment(lItemIdx + 1, 2) = atcControls.atcAlignment.HAlignRight
                ladsResults.CellValue(lItemIdx + 1, 3) = Format(lNZero, "#,##0") & "/" & Format(lNMiss - 1, "#,##0") & " "
                ladsResults.Alignment(lItemIdx + 1, 3) = atcControls.atcAlignment.HAlignRight

                ladsResults.CellValue(lItemIdx + 1, 4) = Sig2(lxBy)
                If Sig2(lxBy) < 100 Then ladsResults.Alignment(lItemIdx + 1, 4) = atcControls.atcAlignment.HAlignDecimal
                ladsResults.CellValue(lItemIdx + 1, 5) = Format(lNExcB / (UBound(lTS) - lNMiss), "percent")
                ladsResults.Alignment(lItemIdx + 1, 5) = atcControls.atcAlignment.HAlignDecimal
                ladsResults.CellValue(lItemIdx + 1, 6) = Sig2(pExcursionCountArray(lItemIdx) * 3 / lYears)
                ladsResults.Alignment(lItemIdx + 1, 6) = atcControls.atcAlignment.HAlignDecimal

                ladsResults.CellValue(lItemIdx + 1, 7) = Sig2(lxQy)
                If Sig2(lxQy) < 100 Then ladsResults.Alignment(lItemIdx + 1, 7) = atcControls.atcAlignment.HAlignDecimal
                ladsResults.CellValue(lItemIdx + 1, 8) = Format(lNExc / (UBound(lTS) - lNMiss), "percent")
                ladsResults.Alignment(lItemIdx + 1, 8) = atcControls.atcAlignment.HAlignDecimal
                ladsResults.CellValue(lItemIdx + 1, 9) = Sig2(lExcQ * 3 / lYears)
                ladsResults.Alignment(lItemIdx + 1, 9) = atcControls.atcAlignment.HAlignDecimal

                ladsResults.Alignment(lItemIdx + 1, 10) = atcControls.atcAlignment.HAlignCenter
                ladsResults.Alignment(lItemIdx + 1, 11) = atcControls.atcAlignment.HAlignCenter
                ladsResults.Alignment(lItemIdx + 1, 12) = atcControls.atcAlignment.HAlignCenter

                If fNonBioType = 0 Then

                    If lEquivalentxQy > lxBy Then
                        ladsResults.CellValue(lItemIdx + 1, 10) = "> " & lReturnPeriod & " years"
                        ladsResults.CellValue(lItemIdx + 1, 11) = "N/A"
                        ladsResults.CellValue(lItemIdx + 1, 12) = "N/A"


                    Else
                        ladsResults.CellValue(lItemIdx + 1, 10) = fAveragingPeriod & "Q" & lReturnPeriod
                        ladsResults.CellValue(lItemIdx + 1, 11) = Sig2(lEquivalentxQy)
                        If Sig2(lEquivalentxQy) < 100 Then ladsResults.Alignment(lItemIdx + 1, 11) = atcControls.atcAlignment.HAlignDecimal
                        ladsResults.CellValue(lItemIdx + 1, 12) = Format(lNExcBQ / (UBound(lTS) - lNMiss), "percent")
                        ladsResults.Alignment(lItemIdx + 1, 12) = atcControls.atcAlignment.HAlignDecimal
                    End If

                Else

                    ladsResults.CellValue(lItemIdx + 1, 10) = "N/A"
                    ladsResults.CellValue(lItemIdx + 1, 11) = "N/A"
                    ladsResults.CellValue(lItemIdx + 1, 12) = "N/A"

                End If

                ladsResults.CellValue(lItemIdx + 1, 13) = Sig2(lHFlow)
                If Sig2(lHFlow) < 100 Then ladsResults.Alignment(lItemIdx + 1, 13) = atcControls.atcAlignment.HAlignDecimal

                ladsResults.CellValue(lItemIdx + 1, 14) = Format(lNExcHF / (UBound(lTS) - lNMiss), "percent")
                ladsResults.Alignment(lItemIdx + 1, 14) = atcControls.atcAlignment.HAlignDecimal
                lItemIdx = lItemIdx + 1

            End If

        Next

        lfrmProgress.Close()
        lfrmProgress.Dispose()

        agrResults.Initialize(ladsResults)

        If Not pIsBatch Then
            Me.Refresh()
        End If

        Me.DialogResult = Windows.Forms.DialogResult.OK


    End Sub

    Private Function UserSpecifyDFLOWArgs() As Boolean
        Dim lForm As New frmDFLOWArgs
        Return lForm.AskUser(pTimeseriesGroup, pCBList)
    End Function

    Private Sub SelectDataToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SelectDataToolStripMenuItem.Click

        atcDataManager.UserSelectData("Select flow time series for Design Flow calculation", pTimeseriesGroup, Nothing, False)


        If pTimeseriesGroup.Count = 0 Or Not UserSpecifyDFLOWArgs() Then

            'user declined to specify Data
            Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Else
            RefreshCalcs()
        End If
    End Sub

    Private Sub SepcifyInputsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SepcifyInputsToolStripMenuItem.Click
        If UserSpecifyDFLOWArgs() Then
            RefreshCalcs()
        End If

    End Sub

    Private Sub agrResults_MouseDownCell(ByVal aGrid As atcControls.atcGrid, ByVal aRow As System.Int32, ByVal aColumn As System.Int32) Handles agrResults.MouseDownCell
        pColumn = aColumn
        pRow = aRow
    End Sub

    Private Sub agrResults_MouseDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles agrResults.MouseDoubleClick

        If pRow = 0 Or pColumn <> 4 Then
            Beep()
        Else

            With pDateFormat

                .IncludeHours = False
                .IncludeMinutes = False
                .IncludeSeconds = False

            End With

            Dim lExcursionCount As Integer
            lExcursionCount = pExcursionCountArray(pRow - 1)

            Dim lClusters As ArrayList
            lClusters = pClustersArray(pRow - 1)

            Dim lExcursions As ArrayList
            lExcursions = pExcursionsArray(pRow - 1)

            Dim lagsExcursions As New atcControls.atcGridSource

            With lagsExcursions
                .Columns = 5
                .Rows = 1 + lExcursions.Count
                .FixedRows = 1
                .CellValue(0, 0) = "Cluster Start"
                .CellValue(0, 1) = "Excursions"
                .CellValue(0, 2) = "Period Start"
                .CellValue(0, 3) = "Duration"
                .CellValue(0, 4) = "Avg Excursion"
                Dim lColumn As Integer
                For lColumn = 0 To 4
                    .CellColor(0, lColumn) = Me.BackColor
                    .Alignment(0, lColumn) = atcControls.atcAlignment.HAlignCenter
                Next

            End With

            Dim lRow As Integer = 0
            Dim lExcursionIdx As Integer = 0
            Dim lFirstDate As Double = pTimeseriesGroup(pRow - 1).Attributes.GetValue("xBy start date")


            Dim lCluster As stCluster
            For Each lCluster In lClusters

                If lRow = 0 Then

                    lRow = 1 ' First cluster is always invalid, so it gets skipped

                Else

                    lagsExcursions.CellValue(lRow, 0) = pDateFormat.JDateToString(lFirstDate + lCluster.Start)
                    lagsExcursions.CellValue(lRow, 1) = lCluster.Excursions & " "
                    lagsExcursions.Alignment(lRow, 1) = atcControls.atcAlignment.HAlignRight

                    Dim lNExc As Integer = 0

                    Do

                        Dim lExcursion As stExcursion
                        lExcursion = lExcursions(lRow)

                        With lExcursion

                            lagsExcursions.CellValue(lRow, 2) = pDateFormat.JDateToString(lFirstDate + .Start)
                            lagsExcursions.CellValue(lRow, 3) = .Count & " "
                            lagsExcursions.CellValue(lRow, 4) = Format(.SumMag / .SumLength - 1, "Percent") & " "

                            lagsExcursions.Alignment(lRow, 3) = atcControls.atcAlignment.HAlignRight
                            lagsExcursions.Alignment(lRow, 4) = atcControls.atcAlignment.HAlignRight

                        End With

                        lNExc = lNExc + 1
                        lRow = lRow + 1

                    Loop Until lNExc >= lCluster.Events

                End If


            Next

            lagsExcursions.CellValue(lRow, 0) = "Total"
            lagsExcursions.CellValue(lRow, 1) = lExcursionCount
            lagsExcursions.Alignment(lRow, 1) = atcControls.atcAlignment.HAlignRight

            Dim lfrmExcursions As New frmDFLOWExcursions
            With lfrmExcursions

                .Text = fBioPeriod & "B" & fBioYears & " Excursion Analysis for " & pTimeseriesGroup(pRow - 1).Attributes.GetFormattedValue("Location")
                .AtcGrid1.Initialize(lagsExcursions)
                .AtcGrid1.BorderStyle = BorderStyle.FixedSingle
                .AtcGrid1.ColumnWidth(0) = 70
                .AtcGrid1.ColumnWidth(1) = 60
                .AtcGrid1.ColumnWidth(2) = 70
                .AtcGrid1.ColumnWidth(3) = 50
                .AtcGrid1.ColumnWidth(4) = 90
                .AtcGrid1.Width = 360
                For lRow = 2 To lagsExcursions.Rows - 1
                    If lagsExcursions.CellValue(lRow, 0) <> "" Then
                        .AtcGrid1.RowHeight(lRow) = 20
                    End If
                Next
                .ShowDialog()

            End With

        End If

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim lBuffer As String
        lBuffer = Me.Text & vbCrLf & vbcrlf & vbcrlf & lblYears.Text & vbcrlf & vbcrlf
        Dim lRow As Integer
        Dim lCol As Integer
        With (agrResults.Source)
            For lRow = 0 To .Rows - 1
                For lCol = 0 To .Columns - 2
                    lBuffer = lBuffer + .CellValue(lRow, lCol) + vbTab
                Next
                lBuffer = lBuffer + .CellValue(lRow, .Columns - 1) + vbCrLf
            Next
        End With
        Clipboard.SetText(lBuffer)

    End Sub

    
    Public Overrides Function ToString() As String
        Return Me.Text & vbCrLf & _
               lblSeasons.Text & vbCrLf & _
               lblYears.Text & vbCrLf & _
               agrResults.ToString
    End Function

    Private Sub SaveGridToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveGridToolStripMenuItem.Click
        Dim lSaveDialog As New System.Windows.Forms.SaveFileDialog
        With lSaveDialog
            .Title = "Save Grid As"
            .DefaultExt = ".txt"
            .FileName = ReplaceString(Me.Text, " ", "_") & "_grid.txt"
            If .ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                SaveFileString(.FileName, Me.ToString)
            End If
        End With
    End Sub

    Private Sub AboutDFLOWToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AboutDFLOWToolStripMenuItem.Click
        Dim lAboutFrm As New frmAboutDFLOW
        lAboutFrm.ShowDialog()

    End Sub

    Private Sub DFLOWHelpToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DFLOWHelpToolStripMenuItem.Click
        ShowDFLOWHelp(Replace(Application.StartupPath.ToLower, g_PathChar & "bin", g_PathChar & "docs") & g_PathChar & "dflow4.chm")
        ShowDFLOWHelp("html\dlfo6hps.htm")
    End Sub
End Class