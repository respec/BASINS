Imports atcData
Imports atcUtility
Imports MapWinUtility

Imports System.Windows.Forms

Public Class frmFilterData

    Private pTranSumDivLabel As String = "Accumulate/Divide"
    Private pTranAverSameLabel As String = "Average/Same"
    Private pTranMaxLabel As String = "Maximum"
    Private pTranMinLabel As String = "Minimum"
    Private pTranCountMissingLabel As String = "Count Missing"

    Private WithEvents pSelectedGroup As atcTimeseriesGroup

    Private pAllDates As atcTimeseries

    Private pInitializing As Boolean = True
    Private pSelectedOK As Boolean = False
    Private pAsking As Boolean = False

    'Private pEventsPlugin As atcTimeseriesSource
    Private pMathOpnNames() As String = {"add", "subtract", "multiply", "divide", "exponent", "e ^ x", "10 ^ x", "log 10", "log e", "absolute value", "celsius to f", "f to celsius"}
    Private Class MathOpn
        Public DataGroup As atcTimeseriesGroup
        Public MathOpn As String
        Public Constant As Double
        Public AttributeName As String
        Public Selected As Boolean = False
        Public ReadOnly Property IncludeAllTimeseries(ByVal aTotal As Integer) As Boolean
            Get
                If DataGroup Is Nothing Then
                    Return False
                Else
                    Return (DataGroup.Count = aTotal)
                End If
            End Get
        End Property
        Public Function EqualTo(ByVal aMathOpn As MathOpn) As Boolean
            If aMathOpn Is Nothing Then Return False
            If MathOpn <> aMathOpn.MathOpn Then Return False
            If Double.IsNaN(Constant) AndAlso Double.IsNaN(aMathOpn.Constant) Then
            ElseIf Constant = aMathOpn.Constant Then
            Else
                Return False
            End If
            If AttributeName <> aMathOpn.AttributeName Then Return False
            If DataGroup IsNot Nothing AndAlso aMathOpn.DataGroup IsNot Nothing AndAlso DataGroup.Count <> aMathOpn.DataGroup.Count Then
                Return False
            End If

            For Each lTser As atcTimeseries In DataGroup
                Dim loc As String = lTser.Attributes.GetValue("Location")
                Dim lcon As String = lTser.Attributes.GetValue("Constituent")
                Dim lTu As atcTimeUnit = lTser.Attributes.GetValue("tu")
                Dim lFoundMatch As Boolean = False
                For Each lTser1 As atcTimeseries In aMathOpn.DataGroup
                    With lTser1.Attributes
                        If .GetValue("Location") = loc AndAlso _
                            .GetValue("tu") = lTu AndAlso _
                            .GetValue("Constituent") = lcon Then
                            lFoundMatch = True
                            Exit For
                        End If
                    End With
                Next
                If Not lFoundMatch Then
                    Return False
                End If
            Next
            Return True
        End Function
        Public Shadows Function ToString(Optional ByVal aIncludeAllTimeseries As Boolean = False) As String
            Dim lbl As String = ""
            If DataGroup Is Nothing OrElse DataGroup.Count = 0 Then
                Return ""
            Else
                If aIncludeAllTimeseries Then
                    lbl = "All Timeseries "
                Else
                    lbl = DataGroup(0).Attributes.GetValue("Location") & "...(" & DataGroup.Count & ") "
                End If
            End If
            lbl &= MathOpn & " "
            If Not Double.IsNaN(Constant) Then
                lbl &= Constant
            Else
                lbl &= AttributeName
            End If
            Return lbl
        End Function
    End Class
    Private pMathOpns As Generic.List(Of MathOpn)

    Public Function AskUser(Optional ByVal aGroup As atcTimeseriesGroup = Nothing, Optional ByVal aModal As Boolean = True) As atcTimeseriesGroup
        pInitializing = True 'Gets set back to False in Populate below

        FindEventsPlugin()

        If aGroup Is Nothing Then
            pSelectedGroup = New atcTimeseriesGroup
        Else
            pSelectedGroup = aGroup
        End If

        atcSelectedDates.DataGroup = pSelectedGroup

        InitSeasons()

        InitTimeStep()

        InitMath()

        Me.Show()
        pAsking = True

        pInitializing = False
        If aModal Then
            While pAsking AndAlso Application.OpenForms.Count > 1
                Application.DoEvents()
                Threading.Thread.Sleep(10)
            End While
            If pSelectedOK Then

            Else 'User clicked Cancel or closed dialog

            End If
            Try
                Me.Close()
            Catch
            End Try
            Return pSelectedGroup
        Else
            Return Nothing
        End If
    End Function

    Private Sub FindEventsPlugin()
        'For Each lPlugin As atcDataPlugin In atcDataManager.GetPlugins(GetType(atcTimeseriesSource))
        '    If (lPlugin.Name = "Timeseries::Events") Then
        '        pEventsPlugin = lPlugin
        '        Exit For
        '    End If
        'Next
        'If pEventsPlugin Is Nothing Then Me.TabPage1.Hide()
    End Sub

    'Public Function CreateSelectedGroupWithTimeStep() As atcTimeseriesGroup
    '    If pSelectedGroup Is Nothing Then
    '        'nothing to aggregate, return empty group
    '        Return New atcTimeseriesGroup
    '    ElseIf Not chkEnableChangeTimeStep.Checked Then
    '        'No need to aggregate
    '        Return pSelectedGroup
    '    Else
    '        Dim lAggregatedData As New atcTimeseriesGroup

    '        Dim lTimeStep As Integer

    '        Dim lTU As atcTimeUnit = atcTimeUnit.TUUnknown
    '        For Each lTimeUnit As atcTimeUnit In [Enum].GetValues(lTU.GetType)
    '            If [Enum].GetName(lTU.GetType, lTimeUnit) = "TU" & cboTimeUnits.Text Then
    '                lTU = lTimeUnit
    '            End If
    '        Next

    '        Dim lTran As atcTran = atcTran.TranNative
    '        Select Case cboAggregate.Text
    '            Case pTranSumDivLabel : lTran = atcTran.TranSumDiv
    '            Case pTranAverSameLabel : lTran = atcTran.TranAverSame
    '            Case pTranMaxLabel : lTran = atcTran.TranMax
    '            Case pTranMinLabel : lTran = atcTran.TranMin
    '            Case pTranCountMissingLabel : lTran = atcTran.TranCountMissing
    '            Case Else : lTran = atcTran.TranNative
    '        End Select

    '        If Not Integer.TryParse(txtTimeStep.Text, lTimeStep) Then
    '            Logger.Msg("Time step must be specified as an integer.", "Time Step Not Specified")
    '        ElseIf lTimeStep < 1 Then
    '            Logger.Msg("Time step must be >= 1.", "Time Step Less Than One")
    '        ElseIf lTU = atcTimeUnit.TUUnknown Then
    '            Logger.Msg("Time Units must be selected to change time step.", "Time Units Not Selected")
    '        ElseIf lTran = atcTran.TranNative Then
    '            Logger.Msg("Aggregation type must be selected to change time step.", "Type of Aggregation Not Selected")
    '        Else
    '            lAggregatedData = New atcTimeseriesGroup
    '            For Each lTimeseries As atcTimeseries In pSelectedGroup
    '                lAggregatedData.Add(Aggregate(lTimeseries, lTU, lTimeStep, lTran))
    '            Next
    '        End If
    '        Return lAggregatedData
    '    End If
    'End Function

    Private Sub frmFilterData_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        atcSelectedDates.Text = ""


        'chkEnableSubsetByDate.Checked = False
        chkEnableSeasons.Checked = False
        chkEnableChangeTimeStep.Checked = False
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        pSelectedOK = False
        Me.Visible = False
        pAsking = False
    End Sub

    Private Sub TimeUnits_Changed(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboTimeUnits.SelectedIndexChanged, _
                                                                                                      cboAggregate.SelectedIndexChanged, _
                                                                                                      txtTimeStep.TextChanged
        If Not pInitializing AndAlso Not chkEnableChangeTimeStep.Checked Then chkEnableChangeTimeStep.Checked = True
    End Sub

    Private Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Try
            Dim lWasModified As Boolean = False
            Dim lModifiedGroup As atcTimeseriesGroup = pSelectedGroup.Clone

            If Not atcSelectedDates.SelectedAll Then 'Change to date subset if needed
                'Note that ChangeTo uses the atcTimeseriesGroup already inside atcSelectedDates,
                'so this has to come before other filters that modify lModifiedGroup
                lModifiedGroup.ChangeTo(atcSelectedDates.CreateSelectedDataGroupSubset)
                lWasModified = True
            End If

            'Timeseries Math
            If pMathOpns IsNot Nothing AndAlso pMathOpns.Count > 0 Then
                For I As Integer = 0 To clbMathOpns.Items.Count - 1
                    CType(pMathOpns(I), MathOpn).Selected = clbMathOpns.GetItemChecked(I)
                Next
                For Each lModifiedTser As atcTimeseries In lModifiedGroup
                    For Each lMathOp As MathOpn In pMathOpns
                        If Not String.IsNullOrEmpty(lMathOp.MathOpn) AndAlso lMathOp.Selected Then
                            For Each lMathDataset As atcTimeseries In lMathOp.DataGroup
                                If lModifiedTser.Attributes.GetValue("Location") = lMathDataset.Attributes.GetValue("Location") AndAlso _
                                   lModifiedTser.Attributes.GetValue("Constituent") = lMathDataset.Attributes.GetValue("Constituent") AndAlso _
                                   lModifiedTser.Attributes.GetValue("tu") = lMathDataset.Attributes.GetValue("tu") Then
                                    'Now work this modified version of tser
                                    Dim lArgs As New atcDataAttributes()
                                    Dim lNumber As Double = GetNaN()
                                    If Not String.IsNullOrEmpty(lMathOp.AttributeName) AndAlso _
                                       lModifiedTser.Attributes.ContainsAttribute(lMathOp.AttributeName) Then
                                        lNumber = lModifiedTser.Attributes.GetValue(lMathOp.AttributeName)
                                    ElseIf Not Double.IsNaN(lMathOp.Constant) Then
                                        lNumber = lMathOp.Constant
                                    Else
                                    End If
                                    lArgs.SetValue("Timeseries", lModifiedTser)
                                    'Number should be defined as second to have the desired effect of Timeseries operator Number
                                    If Not Double.IsNaN(lNumber) Then
                                        lArgs.SetValue("Number", lNumber)
                                    End If

                                    Dim lResult As atcTimeseries = DoMath(lMathOp.MathOpn, lArgs)
                                    'we know we are not doing subset dates etc, so just do a straightup value copy
                                    For I As Integer = 1 To lModifiedTser.numValues
                                        lModifiedTser.Value(I) = lResult.Value(I)
                                    Next
                                    Array.Clear(lResult.Values, 0, lResult.numValues)
                                    lResult = Nothing
                                    lArgs.Clear()
                                    lArgs = Nothing
                                    lWasModified = True
                                    Exit For
                                End If
                            Next
                        End If
                    Next
                Next
            End If

            If chkEnableSeasons.Checked Then
                If lstSeasons.SelectedIndices.Count = 0 Then
                    Throw New ApplicationException("Split Into Seasons requested, but no Seasons to include selected.")
                End If
                If radioSeasonsCombine.Checked Then
                    SaveSetting("atcSeasons", "SplitType", "Split", "Combine")
                End If
                If radioSeasonsSeparate.Checked Then
                    SaveSetting("atcSeasons", "SplitType", "Split", "Separate")
                End If
                Dim lNumToGroup As Integer = 0
                If radioSeasonsGroup.Checked Then
                    Integer.TryParse(txtGroupSeasons.Text, lNumToGroup)
                    If lNumToGroup < 1 Then
                        Throw New ApplicationException("Grouping selected, but number to group not specified")
                    End If
                    SaveSetting("atcSeasons", "SplitType", "Split", txtGroupSeasons.Text)
                End If
                Try 'Try saving first so DeleteSetting will not throw an exception
                    SaveSetting("atcSeasons", "Split" & cboSeasons.Text, "X", "X")
                    DeleteSetting("atcSeasons", "Split" & cboSeasons.Text)
                Catch
                End Try
                Dim lSelectedSeasons As System.Windows.Forms.ListBox.SelectedObjectCollection = lstSeasons.SelectedItems
                If lSelectedSeasons.Count < lstSeasons.Items.Count Then 'Only save selection if fewer than all were selected
                    For Each lSeason As Object In lSelectedSeasons
                        SaveSetting("atcSeasons", "Split" & cboSeasons.Text, lSeason.ToString, "True")
                    Next
                End If
                Dim lSeasonalDatasets As New atcTimeseriesGroup
                DoSplit(CurrentSeason, lSelectedSeasons, lModifiedGroup, radioSeasonsCombine.Checked, radioSeasonsSeparate.Checked, lNumToGroup, lSeasonalDatasets)
                lModifiedGroup = lSeasonalDatasets
                lWasModified = True
            End If

            If chkEvents.Checked Then 'AndAlso pEventsPlugin IsNot Nothing Then
                'Dim lType As System.Type = pEventsPlugin.GetType()
                'Dim lAssembly As System.Reflection.Assembly = System.Reflection.Assembly.GetAssembly(lType)
                'Dim lNewTimeseriesSource As atcTimeseriesSource = lAssembly.CreateInstance(lType.FullName)
                'Dim lNewArguments As New atcDataAttributes
                'lNewArguments.SetValue("Timeseries", lModifiedGroup)
                'lNewTimeseriesSource.Open("Split", lNewArguments)
                Dim lMinValue As Double
                Dim lMaxValue As Double
                Dim lEnforceMin As Boolean = Double.TryParse(txtValueMinimum.Text, lMinValue)
                Dim lEnforceMax As Boolean = Double.TryParse(txtValueMaximum.Text, lMaxValue)
                If lEnforceMin OrElse lEnforceMax Then
                    Dim lFilteredValuesGroup As New atcTimeseriesGroup
                    For Each lSearchTS As atcTimeseries In lModifiedGroup
                        'If we can tell that all values are already in limits, just use it as is
                        If (Not lEnforceMin OrElse lSearchTS.Attributes.GetValue("Minimum") >= lMinValue) AndAlso _
                           (Not lEnforceMax OrElse lSearchTS.Attributes.GetValue("Maximum") <= lMaxValue) Then
                            lFilteredValuesGroup.Add(lSearchTS)
                        Else
                            Dim lFilteredValuesBuilder As New atcTimeseriesBuilder(Nothing)
                            For lValueIndex As Integer = 1 To lSearchTS.numValues
                                Dim lValue As Double = lSearchTS.Value(lValueIndex)
                                If (Not lEnforceMin OrElse lValue >= lMinValue) AndAlso _
                                   (Not lEnforceMax OrElse lValue <= lMaxValue) Then
                                    lFilteredValuesBuilder.AddValue(lSearchTS.Dates.Value(lValueIndex), lValue)
                                End If
                            Next
                            Dim lFilteredTS As atcTimeseries = lFilteredValuesBuilder.CreateTimeseries()
                            lFilteredTS.Attributes.ChangeTo(lSearchTS.Attributes.Clone())
                            lFilteredTS.Attributes.DiscardCalculated()
                            If lEnforceMin Then
                                lFilteredTS.Attributes.AddHistory("Filtered >= " & DoubleToString(lMinValue))
                            End If
                            If lEnforceMax Then
                                lFilteredTS.Attributes.AddHistory("Filtered <= " & DoubleToString(lMaxValue))
                            End If
                            lFilteredValuesGroup.Add(lFilteredTS)
                        End If
                    Next

                    lModifiedGroup = lFilteredValuesGroup
                    lWasModified = True
                End If
            End If

            If chkEnableChangeTimeStep.Checked Then
                Dim lAggregatedGroup As New atcTimeseriesGroup
                Dim lTimeStep As Integer

                Dim lTU As atcTimeUnit = atcTimeUnit.TUUnknown
                For Each lTimeUnit As atcTimeUnit In [Enum].GetValues(lTU.GetType)
                    If [Enum].GetName(lTU.GetType, lTimeUnit) = "TU" & cboTimeUnits.Text Then
                        lTU = lTimeUnit
                    End If
                Next

                Dim lTran As atcTran = atcTran.TranNative
                Select Case cboAggregate.Text
                    Case pTranSumDivLabel : lTran = atcTran.TranSumDiv
                    Case pTranAverSameLabel : lTran = atcTran.TranAverSame
                    Case pTranMaxLabel : lTran = atcTran.TranMax
                    Case pTranMinLabel : lTran = atcTran.TranMin
                    Case pTranCountMissingLabel : lTran = atcTran.TranCountMissing
                    Case Else : lTran = atcTran.TranNative
                End Select

                If Not Integer.TryParse(txtTimeStep.Text, lTimeStep) Then
                    Throw New ApplicationException("Time step must be specified as an integer.")
                ElseIf lTimeStep < 1 Then
                    Throw New ApplicationException("Time step must be >= 1.")
                ElseIf lTU = atcTimeUnit.TUUnknown Then
                    Throw New ApplicationException("Time Units must be selected to change time step.")
                ElseIf lTran = atcTran.TranNative Then
                    Throw New ApplicationException("Aggregation type must be selected to change time step.")
                Else
                    For Each lTimeseries As atcTimeseries In lModifiedGroup
                        lAggregatedGroup.Add(Aggregate(lTimeseries, lTU, lTimeStep, lTran))
                    Next
                    SaveSetting("BASINS", "Select Data", "TimeUnits", cboTimeUnits.Text)
                    SaveSetting("BASINS", "Select Data", "TimeStep", lTimeStep)
                    SaveSetting("BASINS", "Select Data", "Transformation", cboAggregate.Text)
                End If

                lModifiedGroup = lAggregatedGroup
                lWasModified = True
            End If

            If lWasModified Then
                pSelectedGroup.ChangeTo(lModifiedGroup)
                pSelectedOK = True
                pAsking = False
            Else
                Logger.Msg("Select at least one kind of filter." & vbCrLf & "Most options have a checkbox at the top that needs to be checked to activate.")
            End If
        Catch ex As Exception
            Logger.Msg(ex.Message, "Error Filtering Data")
        End Try
    End Sub

    Private Sub cboSeasons_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboSeasons.SelectedIndexChanged
        lstSeasons.Items.Clear()
        SaveSetting("atcSeasons", "SeasonType", "Split", cboSeasons.Text)
        Dim lType As Type = CurrentSeason()
        If lType IsNot Nothing Then
            Dim lSeasonType As atcSeasonBase = lType.InvokeMember(Nothing, Reflection.BindingFlags.CreateInstance, Nothing, Nothing, New Object() {})
            lstSeasons.Items.Clear()
            Dim lSeasonName As String
            Dim lNumSelected As Integer = 0

            For Each lSeasonIndex As Integer In lSeasonType.AllSeasonsInDates(pAllDates.Values)
                lSeasonName = lSeasonType.SeasonName(lSeasonIndex)
                lstSeasons.Items.Add(lSeasonName)
                If GetSetting("atcSeasons", "Split" & cboSeasons.Text, lSeasonName) = "True" Then
                    lNumSelected += 1
                    lstSeasons.SetSelected(lstSeasons.Items.Count - 1, True)
                End If
            Next

            If lNumSelected = 0 Then 'If none were selected in saved settings, default to selecting all
                For lSeasonIndex As Integer = lstSeasons.Items.Count - 1 To 0 Step -1
                    lstSeasons.SetSelected(lSeasonIndex, True)
                Next
            End If
        End If
    End Sub

    Private Sub InitTimeStep()
        With cboTimeUnits.Items
            .Clear()
            Dim lDefaultUnit As String = "TU" & GetSetting("BASINS", "Select Data", "TimeUnits", "Day")
            For Each lUnitName As String In [Enum].GetNames(GetType(atcTimeUnit))
                If lUnitName <> "TUUnknown" Then
                    .Add(lUnitName.Substring(2))
                    If lUnitName = lDefaultUnit Then
                        cboTimeUnits.SelectedItem = cboTimeUnits.Items(cboTimeUnits.Items.Count - 1)
                    End If
                End If
            Next
        End With

        With cboAggregate.Items
            Dim lDefaultTransformation As String = GetSetting("BASINS", "Select Data", "Transformation", pTranSumDivLabel)
            .Clear()
            .Add(pTranSumDivLabel)
            .Add(pTranAverSameLabel)
            .Add(pTranMaxLabel)
            .Add(pTranMinLabel)
            .Add(pTranCountMissingLabel)

            For lIndex As Integer = 0 To .Count - 1
                If .Item(lIndex).ToString = lDefaultTransformation Then
                    cboAggregate.SelectedItem = cboAggregate.Items(lIndex)
                    Exit For
                End If
            Next
        End With

        txtTimeStep.Text = GetSetting("BASINS", "Select Data", "TimeStep", "1")
    End Sub

    Private Sub InitSeasons()
        cboSeasons.Items.Clear()
        lstSeasons.Items.Clear()

        For Each typ As Type In atcData.atcSeasonBase.AllSeasonTypes
            Dim SeasonTypeLabel As String = atcSeasonBase.SeasonClassNameToLabel(typ.Name)
            If SeasonTypeLabel <> "Year Subset" Then
                cboSeasons.Items.Add(SeasonTypeLabel)
            End If
        Next
        pAllDates = MergeDates(pSelectedGroup)

        cboSeasons.SelectedItem = GetSetting("atcSeasons", "SeasonType", "Split", "Month")
        Dim lTypeSetting As String = GetSetting("atcSeasons", "SplitType", "Split", "Separate")
        Select Case lTypeSetting
            Case "Combine"
                radioSeasonsCombine.Checked = True
                radioSeasonsSeparate.Checked = False
                radioSeasonsGroup.Checked = False
            Case "Separate"
                radioSeasonsCombine.Checked = False
                radioSeasonsSeparate.Checked = True
                radioSeasonsGroup.Checked = False
            Case Else
                If IsNumeric(lTypeSetting) Then
                    txtGroupSeasons.Text = lTypeSetting
                    radioSeasonsCombine.Checked = False
                    radioSeasonsSeparate.Checked = False
                    radioSeasonsGroup.Checked = True
                End If
        End Select
    End Sub

    Private Function TserLbl(ByVal aTser As atcTimeseries) As String
        If aTser Is Nothing Then Return ""
        With aTser.Attributes
            Dim ltu_text As String = ""
            If .GetValue("tu") Then
                ltu_text = .GetValue("tu").ToString()
            End If
            Dim lItem As String = .GetValue("Location") & "," & ltu_text & " " & .GetValue("Constituent")
            Return lItem
        End With
    End Function

    Private Sub InitMath()
        cboMathOp.Items.Clear()
        With cboMathOp.Items
            For Each lOpn As String In pMathOpnNames
                .Add(lOpn)
            Next
        End With

        clbTimeseries.Items.Clear()
        clbTimeseries.Items.Add("All Timeseries")
        Dim lAttribs As New ArrayList()
        For Each lTs As atcTimeseries In pSelectedGroup
            clbTimeseries.Items.Add(TserLbl(lTs))
            With lTs
                For Each lDef As atcDefinedValue In .Attributes
                    If lDef.Definition.IsNumeric Then
                        If Not lAttribs.Contains(lDef.Definition.Name) Then
                            lAttribs.Add(lDef.Definition.Name)
                        End If
                    End If
                Next
            End With
        Next

        cboConstant.Items.Clear()
        cboConstant.Items.AddRange(lAttribs.ToArray())

        clbMathOpns.Items.Clear()
    End Sub

    Private Function CurrentSeason() As Type
        For Each typ As Type In atcData.atcSeasonBase.AllSeasonTypes
            If atcSeasonBase.SeasonClassNameToLabel(typ.Name) = cboSeasons.Text Then
                Return typ
            End If
        Next
        Return Nothing
    End Function

    Private Function DoSplit(ByVal aSeasonType As Type, _
                             ByVal aSeasonsSelected As Windows.Forms.ListBox.SelectedObjectCollection, _
                             ByVal aTimseriesGroup As atcTimeseriesGroup, _
                             ByVal aCombineAllSelected As Boolean, _
                             ByVal aEachSelected As Boolean, _
                             ByVal aGroupEveryN As Integer, _
                             ByVal aNewDatasets As atcTimeseriesGroup) As atcTimeseriesGroup
        If aSeasonType IsNot Nothing Then
            Dim lSeasonType As atcSeasonBase = aSeasonType.InvokeMember(Nothing, Reflection.BindingFlags.CreateInstance, Nothing, Nothing, New Object() {})
            Dim lSeasonIndexes() As Integer = lSeasonType.AllSeasonsInDates(pAllDates.Values)
            Dim lSeasonIndexesSelected As New Generic.List(Of Integer)
            For Each lSeasonIndex As Integer In lSeasonIndexes
                If lstSeasons.SelectedItems.Contains(lSeasonType.SeasonName(lSeasonIndex)) Then
                    lSeasonType.SeasonSelected(lSeasonIndex) = True
                    lSeasonIndexesSelected.Add(lSeasonIndex)
                Else
                    lSeasonType.SeasonSelected(lSeasonIndex) = False
                End If
            Next

            If aCombineAllSelected Then
                For Each lTimeseries As atcTimeseries In aTimseriesGroup
                    Dim lSplitTS As atcTimeseriesGroup = lSeasonType.SplitBySelected(lTimeseries, Nothing)
                    aNewDatasets.Add(lSplitTS(0))
                Next
            End If
            If aEachSelected Then
                For Each lTimeseries As atcTimeseries In aTimseriesGroup
                    For Each lSplitTS As atcTimeseries In lSeasonType.Split(lTimeseries, Nothing)
                        If lSeasonIndexesSelected.Contains(lSplitTS.Attributes.GetValue("SeasonIndex")) Then
                            aNewDatasets.Add(lSplitTS)
                        End If
                    Next
                Next
            End If
            If aGroupEveryN > 0 Then
                Dim lGroup As New atcTimeseriesGroup
                Dim lGroupSeasonName As String = ""
                For Each lTimeseries As atcTimeseries In aTimseriesGroup
                    For Each lSplitTS As atcTimeseries In lSeasonType.Split(lTimeseries, Nothing)
                        If lSeasonIndexesSelected.Contains(lSplitTS.Attributes.GetValue("SeasonIndex")) Then
                            lGroup.Add(lSplitTS)
                            Select Case lGroup.Count
                                Case 1
                                    lGroupSeasonName = lSplitTS.Attributes.GetValue("SeasonName")
                                Case aGroupEveryN
                                    lGroupSeasonName &= " - " & lSplitTS.Attributes.GetValue("SeasonName")
                                    Dim lMergedTS As atcTimeseries = MergeTimeseries(lGroup)
                                    lMergedTS.Attributes.SetValue("SeasonName", lGroupSeasonName)
                                    lMergedTS.Attributes.SetValue("SeasonDefinition", lSplitTS.Attributes.GetValue("SeasonDefinition"))
                                    aNewDatasets.Add(lMergedTS)
                                    lGroup.Clear()

                            End Select
                        End If
                    Next
                Next
                If lGroup.Count > 0 Then
                    If lGroup.Count > 1 Then
                        lGroupSeasonName &= " - " & lGroup(lGroup.Count - 1).Attributes.GetValue("SeasonName")
                    End If
                    Dim lMergedTS As atcTimeseries = MergeTimeseries(lGroup)
                    lMergedTS.Attributes.SetValue("SeasonName", lGroupSeasonName)
                    lMergedTS.Attributes.SetValue("SeasonDefinition", lGroup(0).Attributes.GetValue("SeasonDefinition"))
                    aNewDatasets.Add(lMergedTS)
                    lGroup.Clear()
                End If
            End If
        End If
        Return aNewDatasets
    End Function

    Private Sub btnSeasonsAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSeasonsAll.Click
        For index As Integer = 0 To lstSeasons.Items.Count - 1
            lstSeasons.SetSelected(index, True)
        Next
    End Sub

    Private Sub btnSeasonsNone_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSeasonsNone.Click
        For index As Integer = 0 To lstSeasons.Items.Count - 1
            lstSeasons.SetSelected(index, False)
        Next
    End Sub

    Private Sub lstSeasons_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstSeasons.SelectedIndexChanged
        chkEnableSeasons.Checked = lstSeasons.SelectedIndices.Count > 0
    End Sub

    Private Sub txtValue_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtValueMinimum.TextChanged, txtValueMaximum.TextChanged
        chkEvents.Checked = IsNumeric(txtValueMaximum.Text) OrElse IsNumeric(txtValueMinimum.Text)
    End Sub

    Private Sub btnAddMathOp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddMathOp.Click
        If pMathOpns Is Nothing Then
            pMathOpns = New Generic.List(Of MathOpn)
        End If
        Dim lNewOpn As New MathOpn()
        If clbTimeseries.CheckedIndices.Contains(0) Then
            lNewOpn.DataGroup = pSelectedGroup
        Else
            lNewOpn.DataGroup = New atcTimeseriesGroup()
            For Each lCheckedItem As String In clbTimeseries.CheckedItems
                Dim loc As String = lCheckedItem.Substring(0, lCheckedItem.IndexOf(","))
                Dim lDatagroup As atcTimeseriesGroup = pSelectedGroup.FindData("Location", loc)
                If lDatagroup IsNot Nothing AndAlso lDatagroup.Count > 0 Then
                    lNewOpn.DataGroup.Add(lDatagroup(0))
                End If
            Next
        End If
        If lNewOpn.DataGroup Is Nothing OrElse lNewOpn.DataGroup.Count = 0 Then
            Exit Sub
        End If

        If cboMathOp.SelectedItem Is Nothing Then
            Exit Sub
        End If
        lNewOpn.MathOpn = cboMathOp.SelectedItem

        If cboConstant.SelectedItem IsNot Nothing Then
            lNewOpn.AttributeName = cboConstant.SelectedItem
            lNewOpn.Constant = GetNaN()
        Else
            lNewOpn.AttributeName = ""
            If Not Double.TryParse(cboConstant.Text, lNewOpn.Constant) Then
                lNewOpn.Constant = GetNaN()
            End If
        End If

        For Each lMathOpn As MathOpn In pMathOpns
            If lMathOpn.EqualTo(lNewOpn) Then
                Exit Sub
            End If
        Next
        pMathOpns.Add(lNewOpn)

        If lNewOpn.IncludeAllTimeseries(pSelectedGroup.Count) Then
            clbMathOpns.Items.Add(lNewOpn.ToString(True))
        Else
            clbMathOpns.Items.Add(lNewOpn.ToString())
        End If
        clbMathOpns.SetItemChecked(clbMathOpns.Items.Count - 1, True)
    End Sub

    Private Sub cboMathOp_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboMathOp.SelectedIndexChanged
        If cboMathOp.SelectedItem IsNot Nothing Then
            Dim lSelMathOp As String = cboMathOp.SelectedItem
            Select Case lSelMathOp
                Case pMathOpnNames(0), pMathOpnNames(1), pMathOpnNames(2), pMathOpnNames(3)
                    cboConstant.Enabled = True
                Case Else
                    cboConstant.SelectedIndex = -1
                    cboConstant.Text = ""
                    cboConstant.Enabled = False
            End Select
        End If
    End Sub

    Private Sub clbMathOpns_ItemCheck(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckEventArgs) Handles clbMathOpns.ItemCheck
        'Ctype(pMathOpns(e.Index), MathOpn).Selected = clbMathOpns.GetItemChecked(e.Index)
    End Sub
End Class