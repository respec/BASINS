Imports atcData
Imports atcUtility
Imports MapWinUtility
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

    Private pIsBatch As Boolean = False
    Private pRemote As Boolean = False

    Public Sub New(Optional ByVal aTimeseriesGroup As atcData.atcTimeseriesGroup = Nothing, _
                   Optional ByVal aIsBatch As Boolean = False, _
                   Optional ByVal aRemote As Boolean = False)

        MyBase.New()
        pInitializing = True
        pIsBatch = aIsBatch
        pRemote = aRemote

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

        If pIsBatch OrElse aRemote Then
            pInitializing = False
            RefreshCalcs()
        Else
            If pTimeseriesGroup.Count = 0 Or Not UserSpecifyDFLOWArgs() Then
                'user declined to specify Data
                Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Else
                pInitializing = False
                RefreshCalcs()
            End If
        End If
    End Sub

    Private Sub RefreshCalcs()

        ' ----- Advisory labels

        Dim lNaN As Double = GetNaN()

        Dim pFirstyear = 1700
        If DFLOWCalcs.fFirstYear > 0 Then pFirstyear = DFLOWCalcs.fFirstYear

        Dim pLastYear = 2100
        If DFLOWCalcs.fLastYear > 0 Then pLastYear = DFLOWCalcs.fLastYear

        Dim pEndDay = DFLOWCalcs.fEndDay + 1
        Dim pEndMonth = DFLOWCalcs.fEndMonth
        If DFLOWCalcs.fEndDay > DFLOWCalcs.fLastDay(pEndMonth - 1) Then
            pEndDay = 1
            If pEndMonth = 12 Then
                pEndMonth = 1
                pLastYear = pLastYear + 1
            End If
        End If

        If (DFLOWCalcs.fStartMonth = DFLOWCalcs.fEndMonth And DFLOWCalcs.fStartDay = DFLOWCalcs.fEndDay + 1) Or _
           ((DFLOWCalcs.fStartMonth - DFLOWCalcs.fEndMonth) Mod 12 = 1 And DFLOWCalcs.fStartDay = 1 And DFLOWCalcs.fEndDay = DFLOWCalcs.fLastDay(DFLOWCalcs.fEndMonth - 1)) Then
            lblSeasons.Text = "Climatic year defined as " & DFLOWCalcs.fMonth3.Substring(3 * DFLOWCalcs.fStartMonth - 3, 3) & " " & DFLOWCalcs.fStartDay & " - " & DFLOWCalcs.fMonth3.Substring(3 * DFLOWCalcs.fEndMonth - 3, 3) & " " & DFLOWCalcs.fEndDay & "."
        Else
            lblSeasons.Text = "Season defined as " & DFLOWCalcs.fMonth3.Substring(3 * DFLOWCalcs.fStartMonth - 3, 3) & " " & DFLOWCalcs.fStartDay & " - " & DFLOWCalcs.fMonth3.Substring(3 * DFLOWCalcs.fEndMonth - 3, 3) & " " & DFLOWCalcs.fEndDay & _
                          ". Biological flow is calculated for full climatic year starting at " & DFLOWCalcs.fMonth3.Substring(3 * DFLOWCalcs.fStartMonth - 3, 3) & " " & DFLOWCalcs.fStartDay & "."
        End If

        If DFLOWCalcs.fFirstYear <= 0 And DFLOWCalcs.fLastYear <= 0 Then
            lblYears.Text = "All available years of data are included in analysis."
        ElseIf DFLOWCalcs.fFirstYear <= 0 Then
            lblYears.Text = "All available data through " & DFLOWCalcs.fMonth3.Substring(3 * DFLOWCalcs.fEndMonth - 3, 3) & " " & DFLOWCalcs.fEndDay & ", " & DFLOWCalcs.fLastYear & " are included in analysis."
        ElseIf DFLOWCalcs.fLastYear <= 0 Then
            If (DFLOWCalcs.fStartMonth < DFLOWCalcs.fEndMonth) Or (DFLOWCalcs.fStartMonth = DFLOWCalcs.fEndMonth And DFLOWCalcs.fStartDay < DFLOWCalcs.fEndDay) Then
                lblYears.Text = "All available data from " & DFLOWCalcs.fMonth3.Substring(3 * DFLOWCalcs.fStartMonth - 3, 3) & " " & DFLOWCalcs.fStartDay & ", " & DFLOWCalcs.fFirstYear - 1 & " are included in analysis."
            Else
                lblYears.Text = "All available data from " & DFLOWCalcs.fMonth3.Substring(3 * DFLOWCalcs.fStartMonth - 3, 3) & " " & DFLOWCalcs.fStartDay & ", " & DFLOWCalcs.fFirstYear & " are included in analysis."
            End If
        Else
            If (DFLOWCalcs.fStartMonth < DFLOWCalcs.fEndMonth) Or (DFLOWCalcs.fStartMonth = DFLOWCalcs.fEndMonth And DFLOWCalcs.fStartDay < DFLOWCalcs.fEndDay) Then
                lblYears.Text = "All available data from " & DFLOWCalcs.fMonth3.Substring(3 * DFLOWCalcs.fStartMonth - 3, 3) & " " & DFLOWCalcs.fStartDay & ", " & DFLOWCalcs.fFirstYear - 1 & _
                                " through " & DFLOWCalcs.fMonth3.Substring(3 * DFLOWCalcs.fEndMonth - 3, 3) & " " & DFLOWCalcs.fEndDay & ", " & DFLOWCalcs.fLastYear & " are included in analysis."
            Else
                lblYears.Text = "All available data from " & DFLOWCalcs.fMonth3.Substring(3 * DFLOWCalcs.fStartMonth - 3, 3) & " " & DFLOWCalcs.fStartDay & ", " & DFLOWCalcs.fFirstYear & _
                                " through " & DFLOWCalcs.fMonth3.Substring(3 * DFLOWCalcs.fEndMonth - 3, 3) & " " & DFLOWCalcs.fEndDay & ", " & DFLOWCalcs.fLastYear & " are included in analysis."
            End If
        End If


        ' ----- Count number of items checked in the listbox
        Dim lTotalItems As Integer = 0
        Dim lDSIndex As Integer
        If pRemote AndAlso pTimeseriesGroup IsNot Nothing Then
            lTotalItems = pTimeseriesGroup.Count
        Else
            For lDSIndex = 0 To pCBList.Items.Count - 1
                If pCBList.GetItemChecked(lDSIndex) Then
                    lTotalItems = lTotalItems + 1
                End If
            Next
        End If

        ' ----- Build data source for results

        Dim ladsResults As New atcControls.atcGridSource
        With ladsResults

            .Columns = 14 + DFLOWCalcs.fNonBioType.Count
            .FixedColumns = 4
            .Rows = lTotalItems + 1
            .FixedRows = 1

            Dim lColumn As Integer = 0
            .CellValue(0, lColumn) = "Gage"
            lColumn += 1
            .CellValue(0, lColumn) = "Period"
            lColumn += 1
            .CellValue(0, lColumn) = "Days in Record"
            lColumn += 1
            .CellValue(0, lColumn) = "Zero/Missing"
            lColumn += 1
            .CellValue(0, lColumn) = DFLOWCalcs.fBioPeriod & "B" & DFLOWCalcs.fBioYears
            lColumn += 1
            .CellValue(0, lColumn) = "Percentile"
            lColumn += 1
            .CellValue(0, lColumn) = "Excur per 3 yr"
            lColumn += 1

            For Each lNonBioType As Integer In DFLOWCalcs.fNonBioType
                Select Case lNonBioType
                    Case 0 : .CellValue(0, lColumn) = DFLOWCalcs.fAveragingPeriod & "Q" & DFLOWCalcs.fReturnPeriod
                    Case 1 : .CellValue(0, lColumn) = "Explicit Q"
                    Case 2 : .CellValue(0, lColumn) = "Percentile Q"
                End Select
                lColumn += 1
            Next

            .CellValue(0, lColumn) = "Percentile"
            lColumn += 1
            .CellValue(0, lColumn) = "Excur per 3 yr"
            lColumn += 1
            .CellValue(0, lColumn) = DFLOWCalcs.fAveragingPeriod & "Qy Type"
            lColumn += 1
            .CellValue(0, lColumn) = "xQy"
            lColumn += 1
            .CellValue(0, lColumn) = "Percentile"
            lColumn += 1
            .CellValue(0, lColumn) = "Harmonic"
            lColumn += 1
            .CellValue(0, lColumn) = "Percentile"

            For lColumn = 0 To .Columns - 1
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

        For lDSIndex = 0 To lTotalItems - 1 'pCBList.Items.Count - 1
            If (Not pRemote AndAlso pCBList.GetItemChecked(lDSIndex)) OrElse pTimeseriesGroup(lDSIndex) IsNot Nothing Then

                With pDateFormat
                    .IncludeHours = False
                    .IncludeMinutes = False
                    .IncludeSeconds = False
                End With

                For Each lNonBioType As Integer In DFLOWCalcs.fNonBioType


                    ' ===== Quick trim 

                    Dim lHydrologicTS As atcTimeseries = pTimeseriesGroup(lDSIndex)
                    Dim lHydrologicTS2 As atcTimeseries = SubsetByDateBoundary(lHydrologicTS, DFLOWCalcs.fStartMonth, DFLOWCalcs.fStartDay, Nothing, pFirstyear, pLastYear, pEndMonth, pEndDay)
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

                    Select Case lNonBioType

                        Case 0

                            lfrmProgress.Label1.Text = (1 + lItemIdx) & " of " & lTotalItems & " - " & DFLOWCalcs.fAveragingPeriod & "Q" & DFLOWCalcs.fReturnPeriod
                            Application.DoEvents()

                            If lYears >= DFLOWCalcs.fReturnPeriod Then
                                lxQy = DFLOWCalcs.xQy(DFLOWCalcs.fAveragingPeriod, DFLOWCalcs.fReturnPeriod, lHydrologicDS)
                            Else
                                lxQy = lNaN
                            End If

                        Case 1

                            lfrmProgress.Label1.Text = (1 + lItemIdx) & " of " & lTotalItems & " - explicit flow"
                            Application.DoEvents()
                            lxQy = DFLOWCalcs.fExplicitFlow

                        Case 2

                            lfrmProgress.Label1.Text = (1 + lItemIdx) & " of " & lTotalItems & " - " & DFLOWCalcs.fPercentile & "th percentile"
                            Application.DoEvents()
                            modTimeseriesMath.ComputePercentile(lHydrologicTS2, DFLOWCalcs.fPercentile)
                            lxQy = lHydrologicTS2.Attributes.GetValue("%" & Format(DFLOWCalcs.fPercentile, "00.####"))

                    End Select

                    ' ===== Create 4-day running average for start of xBy excursion analysis - 

                    Dim lTimeSeries As atcTimeseries = pTimeseriesGroup(lDSIndex)
                    Dim lTimeSeries2 As atcTimeseries = SubsetByDateBoundary(lTimeSeries, DFLOWCalcs.fStartMonth, DFLOWCalcs.fStartDay, Nothing, pFirstyear, pLastYear, pEndMonth, pEndDay)

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
                    Dim lExcQ = DFLOWCalcs.CountExcursions(lxQy, 1, 120, 5, lTSN, lExcursions, lClusters)
                    lExcursions.Clear()
                    lClusters.Clear()


                    ' ===== Calculate xBy (only defined for full-year)

                    lfrmProgress.ProgressBar1.Value = Int(100 * (3 * lItemIdx + 2) / (3 * lTotalItems))
                    lfrmProgress.Label1.Text = (1 + lItemIdx) & " of " & lTotalItems & " - " & DFLOWCalcs.fBioPeriod & "B" & DFLOWCalcs.fBioYears
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
                            If lN > DFLOWCalcs.fBioPeriod Then
                                lSum = lSum - lTS(lI - DFLOWCalcs.fBioPeriod)
                            End If
                        End If

                        If lN >= DFLOWCalcs.fBioPeriod Then
                            lTSN(lI) = lSum / DFLOWCalcs.fBioPeriod
                        Else
                            lTSN(lI) = lNaN
                        End If
                    Next

                    ' ----- 2. Get initial guess

                    Dim lxBy As Double = DFLOWCalcs.xQy(DFLOWCalcs.fBioPeriod, DFLOWCalcs.fBioYears, pTimeseriesGroup(lDSIndex))

                    ' ----- 3. Do xBy calculation

                    Dim lExcursionCount As Integer


                    lxBy = DFLOWCalcs.xBy(lxBy, DFLOWCalcs.fBioPeriod, DFLOWCalcs.fBioYears, DFLOWCalcs.fBioCluster, DFLOWCalcs.fBioExcursions, lTSN, lExcursionCount, lExcursions, lClusters, lfrmProgress)
                    Dim pAttrName As String = DFLOWCalcs.fBioPeriod & "B" & DFLOWCalcs.fBioYears
                    lHydrologicTS.Attributes.SetValue(pAttrName, lxBy)

                    pExcursionCountArray.Add(lExcursionCount)
                    pExcursionsArray.Add(lExcursions)
                    pClustersArray.Add(lClusters)


                    ' ===== If appropriate, calculate equivalent xQy for this xBy

                    Dim lEquivalentxQy As Double = 0
                    Dim lReturnPeriod As Integer


                    If lNonBioType > 0 Then

                        lEquivalentxQy = lNaN

                    Else


                        lfrmProgress.ProgressBar1.Value = Int(100 * (3 * lItemIdx + 3) / (3 * lTotalItems))
                        lfrmProgress.Label1.Text = (1 + lItemIdx) & " of " & lTotalItems & " - xQy"
                        Application.DoEvents()

                        lEquivalentxQy = DFLOWCalcs.xQy(DFLOWCalcs.fAveragingPeriod, lYears, pTimeseriesGroup(lDSIndex))
                        If lEquivalentxQy > lxBy Then

                            lReturnPeriod = lYears

                        Else

                            If lxQy > lxBy Then
                                lReturnPeriod = DFLOWCalcs.fReturnPeriod
                            Else
                                lReturnPeriod = 1
                            End If

                            lEquivalentxQy = lxBy
                            While lEquivalentxQy >= lxBy And lReturnPeriod < lYears

                                lReturnPeriod = lReturnPeriod + 1
                                lEquivalentxQy = DFLOWCalcs.xQy(DFLOWCalcs.fAveragingPeriod, lReturnPeriod, pTimeseriesGroup(lDSIndex))

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

                    ladsResults.CellValue(lItemIdx + 1, 1) = pDateFormat.JDateToString(1 + lHydrologicDS.Attributes.GetValue("start date")) & " - " &
                                                             pDateFormat.JDateToString(lHydrologicDS.Attributes.GetValue("end date"))
                    ladsResults.CellValue(lItemIdx + 1, 2) = Format(UBound(lTS) - 1, "#,##0") & " "
                    ladsResults.Alignment(lItemIdx + 1, 2) = atcControls.atcAlignment.HAlignRight
                    ladsResults.CellValue(lItemIdx + 1, 3) = Format(lNZero, "#,##0") & "/" & Format(lNMiss - 1, "#,##0") & " "
                    ladsResults.Alignment(lItemIdx + 1, 3) = atcControls.atcAlignment.HAlignRight

                    ladsResults.CellValue(lItemIdx + 1, 4) = DFLOWCalcs.Sig2(lxBy)
                    If DFLOWCalcs.Sig2(lxBy) < 100 Then ladsResults.Alignment(lItemIdx + 1, 4) = atcControls.atcAlignment.HAlignDecimal
                    ladsResults.CellValue(lItemIdx + 1, 5) = Format(lNExcB / (UBound(lTS) - lNMiss), "percent")
                    ladsResults.Alignment(lItemIdx + 1, 5) = atcControls.atcAlignment.HAlignDecimal
                    ladsResults.CellValue(lItemIdx + 1, 6) = DFLOWCalcs.Sig2(pExcursionCountArray(lItemIdx) * 3 / lYears)
                    ladsResults.Alignment(lItemIdx + 1, 6) = atcControls.atcAlignment.HAlignDecimal

                    ladsResults.CellValue(lItemIdx + 1, 7) = DFLOWCalcs.Sig2(lxQy)
                    If DFLOWCalcs.Sig2(lxQy) < 100 Then ladsResults.Alignment(lItemIdx + 1, 7) = atcControls.atcAlignment.HAlignDecimal
                    ladsResults.CellValue(lItemIdx + 1, 8) = Format(lNExc / (UBound(lTS) - lNMiss), "percent")
                    ladsResults.Alignment(lItemIdx + 1, 8) = atcControls.atcAlignment.HAlignDecimal
                    ladsResults.CellValue(lItemIdx + 1, 9) = DFLOWCalcs.Sig2(lExcQ * 3 / lYears)
                    ladsResults.Alignment(lItemIdx + 1, 9) = atcControls.atcAlignment.HAlignDecimal

                    ladsResults.Alignment(lItemIdx + 1, 10) = atcControls.atcAlignment.HAlignCenter
                    ladsResults.Alignment(lItemIdx + 1, 11) = atcControls.atcAlignment.HAlignCenter
                    ladsResults.Alignment(lItemIdx + 1, 12) = atcControls.atcAlignment.HAlignCenter

                    If lNonBioType = 0 Then
                        'TODO: put in correct column for which NonBioType we are on, assess where the NonBioType For loop goes
                        If lEquivalentxQy > lxBy Then
                            ladsResults.CellValue(lItemIdx + 1, 10) = "> " & lReturnPeriod & " years"
                            ladsResults.CellValue(lItemIdx + 1, 11) = "N/A"
                            ladsResults.CellValue(lItemIdx + 1, 12) = "N/A"


                        Else
                            ladsResults.CellValue(lItemIdx + 1, 10) = DFLOWCalcs.fAveragingPeriod & "Q" & lReturnPeriod
                            ladsResults.CellValue(lItemIdx + 1, 11) = DFLOWCalcs.Sig2(lEquivalentxQy)
                            If DFLOWCalcs.Sig2(lEquivalentxQy) < 100 Then ladsResults.Alignment(lItemIdx + 1, 11) = atcControls.atcAlignment.HAlignDecimal
                            ladsResults.CellValue(lItemIdx + 1, 12) = Format(lNExcBQ / (UBound(lTS) - lNMiss), "percent")
                            ladsResults.Alignment(lItemIdx + 1, 12) = atcControls.atcAlignment.HAlignDecimal
                        End If

                    Else

                        ladsResults.CellValue(lItemIdx + 1, 10) = "N/A"
                        ladsResults.CellValue(lItemIdx + 1, 11) = "N/A"
                        ladsResults.CellValue(lItemIdx + 1, 12) = "N/A"

                    End If

                    ladsResults.CellValue(lItemIdx + 1, 13) = DFLOWCalcs.Sig2(lHFlow)
                    If DFLOWCalcs.Sig2(lHFlow) < 100 Then ladsResults.Alignment(lItemIdx + 1, 13) = atcControls.atcAlignment.HAlignDecimal

                    ladsResults.CellValue(lItemIdx + 1, 14) = Format(lNExcHF / (UBound(lTS) - lNMiss), "percent")
                    ladsResults.Alignment(lItemIdx + 1, 14) = atcControls.atcAlignment.HAlignDecimal
                    lItemIdx = lItemIdx + 1
                Next
            End If
        Next
        lfrmProgress.Close()
        lfrmProgress.Dispose()

        agrResults.Initialize(ladsResults)

        If Not pIsBatch Then
            Me.Refresh()
        End If

        If pRemote Then
            Me.Text = "Design Flow Results (" & DateTime.Now & ")"
            Me.Show()
        Else
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
        End If
    End Sub

    Private Function UserSpecifyDFLOWArgs() As Boolean
        Dim lForm As New frmDFLOWArgs
        Return lForm.AskUser(pTimeseriesGroup, pCBList)
    End Function

    Private Sub SelectDataToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SelectDataToolStripMenuItem.Click

        atcDataManager.UserSelectData("Select flow time series for Design Flow calculation", pTimeseriesGroup, Nothing, False)


        If pTimeseriesGroup.Count = 0 Or Not UserSpecifyDFLOWArgs() Then

            'user declined to specify Data
            Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
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


            Dim lCluster As DFLOWCalcs.stCluster
            For Each lCluster In lClusters

                If lRow = 0 Then

                    lRow = 1 ' First cluster is always invalid, so it gets skipped

                Else

                    lagsExcursions.CellValue(lRow, 0) = pDateFormat.JDateToString(lFirstDate + lCluster.Start)
                    lagsExcursions.CellValue(lRow, 1) = lCluster.Excursions & " "
                    lagsExcursions.Alignment(lRow, 1) = atcControls.atcAlignment.HAlignRight

                    Dim lNExc As Integer = 0

                    Do

                        Dim lExcursion As DFLOWCalcs.stExcursion
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

                .Text = DFLOWCalcs.fBioPeriod & "B" & DFLOWCalcs.fBioYears & " Excursion Analysis for " & pTimeseriesGroup(pRow - 1).Attributes.GetFormattedValue("Location")
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
            If FileExists(IO.Path.GetDirectoryName(.FileName), True, False) Then
                .InitialDirectory = IO.Path.GetDirectoryName(.FileName)
            End If
            If .ShowDialog(Me) = System.Windows.Forms.DialogResult.OK Then
                SaveFileString(.FileName, Me.ToString)
            End If
        End With
    End Sub

    Private Sub AboutDFLOWToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AboutDFLOWToolStripMenuItem.Click
        Dim lAboutFrm As New frmAboutDFLOW
        lAboutFrm.ShowDialog()

    End Sub

    Private Sub DFLOWHelpToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DFLOWHelpToolStripMenuItem.Click
        DFLOWCalcs.ShowDFLOWHelp(Replace(Application.StartupPath.ToLower, g_PathChar & "bin", g_PathChar & "docs") & g_PathChar & "dflow4.chm")
        DFLOWCalcs.ShowDFLOWHelp("html\dlfo6hps.htm")
    End Sub
End Class