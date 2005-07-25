Imports atcData

Imports System.Windows.Forms

Public Class atcTimeseriesMath
  Inherits atcDataSource
  Private pAvailableStatistics As Hashtable
  Private pAvailableTimeseriesOperations As atcDataGroup
  Private Const pName As String = "Timeseries Math"

  Private pControls As Control.ControlCollection
  Private cboTimeseries As System.Windows.Forms.ComboBox

  Public Overrides ReadOnly Property Name() As String
    Get
      Return pName
    End Get
  End Property

  Public Overrides ReadOnly Property Description() As String
    Get

      Return Name

      'Dim retval As String = Name & " ("

      'For Each def As atcDataSet In AvailableOperations
      '  retval &= def.Attributes.GetValue("Name") & ", "
      'Next

      'Return Left(retval, Len(retval) - 2) & ")" 'Replace last ", " with ")"
    End Get
  End Property

  'Opening creates new computed data rather than opening a file
  Public Overrides ReadOnly Property CanOpen() As Boolean
    Get
      Return True
    End Get
  End Property

  'Definitions of statistics supported by ComputeStatistics
  'Public Overrides ReadOnly Property AvailableStatistics() As Hashtable
  '  Get
  '    If pAvailableStatistics Is Nothing Then
  '      pAvailableStatistics = New Hashtable
  '      Dim def As atcAttributeDefinition

  '      def = New atcAttributeDefinition
  '      def.Name = "Mean"
  '      def.Description = "Sum of all values divided by number of values"
  '      def.Editable = False
  '      pAvailableStatistics.Add(def.Name, def)

  '      def = New atcAttributeDefinition
  '      def.Name = "Max"
  '      def.Description = "Maximum value"
  '      def.Editable = False
  '      pAvailableStatistics.Add(def.Name, def)

  '      def = New atcAttributeDefinition
  '      def.Name = "Min"
  '      def.Description = "Minimum value"
  '      def.Editable = False
  '      pAvailableStatistics.Add(def.Name, def)

  '      def = New atcAttributeDefinition
  '      def.Name = "GeometricMean"
  '      def.Description = "10 ^ Mean of log(each value)"
  '      def.Editable = False
  '      pAvailableStatistics.Add(def.Name, def)

  '      def = New atcAttributeDefinition
  '      def.Name = "Variance"
  '      def.Description = "Variance"
  '      def.Editable = False
  '      pAvailableStatistics.Add(def.Name, def)

  '      def = New atcAttributeDefinition
  '      def.Name = "StandardDeviation"
  '      def.Description = "Standard deviation"
  '      def.Editable = False
  '      pAvailableStatistics.Add(def.Name, def)

  '      def = New atcAttributeDefinition
  '      def.Name = "7Q10"
  '      def.Description = "Seven year 10-day low flow"
  '      def.Editable = False
  '      pAvailableStatistics.Add(def.Name, def)
  '    End If
  '    Return pAvailableStatistics
  '  End Get
  'End Property

  ''Compute all available statistics for aTimeseries and add them as attributes
  'Public Overrides Sub ComputeStatistics(ByVal aTimeseries As atcTimeseries)
  '  'TODO: SetAttribute(aTimeseries, "7Q10", "7Q10 can not yet be calculated")
  '  Dim iLastValue As Integer = aTimeseries.numValues - 1
  '  If iLastValue >= 0 Then
  '    Dim iValue As Integer
  '    Dim val As Double

  '    Dim lMax As Double = -1.0E+30
  '    Dim lMin As Double = 1.0E+30

  '    Dim lGeometricMean As Double = 0
  '    Dim lStandardDeviation As Double = 0
  '    Dim lMean As Double = 0
  '    Dim lSum As Double = 0
  '    Dim lSumSquares As Double = 0
  '    Dim lVariance As Double = 0

  '    For iValue = 0 To iLastValue
  '      val = aTimeseries.Value(iValue)
  '      If val > lMax Then lMax = val
  '      If val < lMin Then lMin = val
  '      lSum += val
  '      lSumSquares += val * val
  '      If lMin > 0 Then lGeometricMean += Math.Log(val)
  '    Next

  '    SetAttribute(aTimeseries, "Max", lMax)
  '    SetAttribute(aTimeseries, "Min", lMin)
  '    SetAttribute(aTimeseries, "Sum", lSum)

  '    iValue = aTimeseries.numValues
  '    lMean = lSum / iValue
  '    SetAttribute(aTimeseries, "Mean", lMean)

  '    If lMin > 0 Then
  '      lGeometricMean = Math.Exp(lGeometricMean / iValue)
  '      SetAttribute(aTimeseries, "GeometricMean", lGeometricMean)
  '    End If

  '    If iValue > 1 Then
  '      lVariance = (lSumSquares - (lSum * lSum) / iValue) / (iValue - 1)
  '      SetAttribute(aTimeseries, "Variance", lVariance)
  '      If lVariance > 0 Then
  '        lStandardDeviation = Math.Sqrt(lVariance)
  '        SetAttribute(aTimeseries, "StandardDeviation", lStandardDeviation)
  '      End If
  '    End If
  '  End If
  'End Sub

  'Set the named Double attribute in aTimeseries using the definition from pAvailableStatistics
  'Private Sub SetAttribute(ByVal aTimeseries As atcTimeseries, ByVal aName As String, ByVal aValue As Double)
  '  Dim def As atcAttributeDefinition = pAvailableStatistics.Item(aName)
  '  aTimeseries.Attributes.SetValue(def, aValue)
  'End Sub

  'Operations supported by ComputeTimeseries
  Public Overrides ReadOnly Property AvailableOperations() As atcDataGroup
    Get
      If pAvailableTimeseriesOperations Is Nothing Then
        pAvailableTimeseriesOperations = New atcDataGroup
        Dim lData As atcDataSet
        Dim defName As New atcAttributeDefinition
        With defName
          .Name = "Name"
          .Description = ""
          .Editable = False
          .TypeString = "String"
        End With

        Dim defDesc As New atcAttributeDefinition
        With defDesc
          .Name = "Description"
          .Description = ""
          .Editable = False
          .TypeString = "String"
        End With

        Dim defTimeSeriesOne As New atcAttributeDefinition
        With defTimeSeriesOne
          .Name = "Timeseries"
          .Description = "One time series"
          .Editable = True
          .TypeString = "atcTimeseries"
        End With

        Dim defTimeSeriesGroup As New atcAttributeDefinition
        With defTimeSeriesGroup
          .Name = "Timeseries"
          .Description = "One or more time series"
          .Editable = True
          .TypeString = "atcDataGroup"
        End With

        Dim defDouble As New atcAttributeDefinition
        With defDouble
          .Name = "Number"
          .Description = "Integer or Double Value"
          .DefaultValue = CDbl(0)
          .Editable = True
          .TypeString = "Double"
        End With

        Dim defNumDays As New atcAttributeDefinition
        With defNumDays
          .Name = "Number of Days"
          .Description = "Length of time"
          .DefaultValue = CDbl(1)
          .Editable = True
          .TypeString = "Double"
          .Min = 0
          .Max = Double.PositiveInfinity
        End With

        lData = New atcDataSet
        lData.Attributes.SetValue(defName, "Add")
        lData.Attributes.SetValue(defDesc, "Add to each value of a timeseries")
        lData.Attributes.SetValue(defTimeSeriesGroup, Nothing)
        lData.Attributes.SetValue(defDouble, Nothing)
        pAvailableTimeseriesOperations.Add(lData)

        lData = New atcDataSet
        lData.Attributes.SetValue(defName, "Subtract")
        lData.Attributes.SetValue(defDesc, "Subtract from each value of a timeseries")
        lData.Attributes.SetValue(defTimeSeriesOne, Nothing)
        lData.Attributes.SetValue(defDouble, Nothing)
        pAvailableTimeseriesOperations.Add(lData)

        lData = New atcDataSet
        lData.Attributes.SetValue(defName, "Multiply")
        lData.Attributes.SetValue(defDesc, "Multiply each value of a timeseries")
        lData.Attributes.SetValue(defTimeSeriesGroup, Nothing)
        lData.Attributes.SetValue(defDouble, Nothing)
        pAvailableTimeseriesOperations.Add(lData)

        lData = New atcDataSet
        lData.Attributes.SetValue(defName, "Divide")
        lData.Attributes.SetValue(defDesc, "Divide each value of a timeseries")
        lData.Attributes.SetValue(defTimeSeriesOne, Nothing)
        lData.Attributes.SetValue(defDouble, Nothing)
        pAvailableTimeseriesOperations.Add(lData)

        lData = New atcDataSet
        lData.Attributes.SetValue(defName, "N-day Mean")
        lData.Attributes.SetValue(defDesc, "Mean of values within N-day range")
        lData.Attributes.SetValue(defTimeSeriesGroup, Nothing)
        lData.Attributes.SetValue(defNumDays, defNumDays.DefaultValue)
        pAvailableTimeseriesOperations.Add(lData)

      End If
      Return pAvailableTimeseriesOperations
    End Get
  End Property

  'Args are each usually either Double or atcTimeseries
  Public Overrides Function Open(ByVal aOperationName As String, Optional ByVal aArgs As atcDataAttributes = Nothing) As Boolean
    Dim newDataSource As New atcDataSource
    newDataSource.DataManager = DataManager
    newDataSource.Specification = "Computed by " & pName
    Dim newVals() As Double
    'Dim retval As New atcTimeseries(newDataSource)
    Dim iTS As Integer
    Dim curTS As atcTimeseries
    Dim firstTS As atcTimeseries
    Dim needToAsk As Boolean = False
    Dim lSelectedOperation As atcDataSet
    Dim lNewTS As atcTimeseries

    If aOperationName Is Nothing OrElse aOperationName.Length = 0 Then
      'TODO: ask user which operation to perform
      aOperationName = "Add"
    End If
    Specification = aOperationName
    If aArgs Is Nothing Then
      needToAsk = True
      For Each lOperation As atcDataSet In AvailableOperations
        If lOperation.Attributes.GetValue("Name").ToString.ToLower = aOperationName.ToLower() Then
          aArgs = lOperation.Attributes.Clone
        End If
      Next
    End If

    If needToAsk Then
      'Ask user what to do
      Dim lSpecify As New frmSpecifyComputation
      If Not lSpecify.AskUser(DataManager, aArgs) Then
        Return False 'User cancelled
      End If
    End If
    'If firstTS Is Nothing Then
    '  Err.Raise("At least one Timeseries was required but none were passed to atcTimeseriesMath.Open")
    'Else
    Dim lTSgroup As atcDataGroup = aArgs.GetValue("Timeseries", Nothing)
    Dim lNumber As Double = CDbl(aArgs.GetValue("Number", 0))
    If lNumber <> 0 Then Specification &= " " & lNumber
    If lTSgroup Is Nothing OrElse lTSgroup.Count < 1 Then
      Err.Raise(aOperationName & " did not get a Timeseries argument")
    Else
      firstTS = lTSgroup.Item(0)
    End If

    Select Case aOperationName.ToLower
      Case "add", "+"
        ReDim newVals(firstTS.numValues)
        Array.Copy(firstTS.Values, newVals, firstTS.numValues + 1) 'copy values from firstTS
        For iValue As Integer = 0 To firstTS.numValues - 1
          newVals(iValue) += lNumber
          For iTS = 1 To lTSgroup.Count - 1
            curTS = lTSgroup.Item(iTS)
            newVals(iValue) += curTS.Value(iValue)
          Next
        Next
        lNewTS = New atcTimeseries(Me)
        lNewTS.Values = newVals
        lNewTS.Dates = firstTS.Dates
        lNewTS.Attributes.ChangeTo(firstTS.Attributes)
        AddDataSet(lNewTS)

      Case "subtract", "-"
        If lTSgroup.Count <> 1 Then
          Err.Raise(aOperationName & " required one timeseries but got " & lTSgroup.Count)
        Else
          ReDim newVals(firstTS.numValues)
          For iValue As Integer = 0 To firstTS.numValues - 1
            newVals(iValue) = firstTS.Value(iValue) - lNumber
          Next
          lNewTS = New atcTimeseries(Me)
          lNewTS.Values = newVals
          lNewTS.Dates = firstTS.Dates
          lNewTS.Attributes.ChangeTo(firstTS.Attributes)
          AddDataSet(lNewTS)
        End If

      Case "multiply", "*"
        ReDim newVals(firstTS.numValues)
        Array.Copy(firstTS.Values, newVals, firstTS.numValues + 1) 'copy values from firstTS
        For iValue As Integer = 0 To firstTS.numValues - 1
          newVals(iValue) *= lNumber
          For iTS = 1 To lTSgroup.Count - 1
            curTS = lTSgroup.Item(iTS)
            newVals(iValue) *= curTS.Value(iValue)
          Next
        Next
        lNewTS = New atcTimeseries(Me)
        lNewTS.Values = newVals
        lNewTS.Dates = firstTS.Dates
        lNewTS.Attributes.ChangeTo(firstTS.Attributes)
        AddDataSet(lNewTS)

      Case "divide", "/"
        If lTSgroup.Count <> 1 Then
          Err.Raise(aOperationName & " required one timeseries but got " & lTSgroup.Count)
        ElseIf Math.Abs(lNumber) < 0.000001 Then
          Err.Raise(aOperationName & " got a divisor too close to zero (" & lNumber & ")")
        Else
          ReDim newVals(firstTS.numValues)
          For iValue As Integer = 0 To firstTS.numValues - 1
            newVals(iValue) = firstTS.Value(iValue) / lNumber
          Next
          lNewTS = New atcTimeseries(Me)
          lNewTS.Values = newVals
          lNewTS.Dates = firstTS.Dates
          lNewTS.Attributes.ChangeTo(firstTS.Attributes)
          AddDataSet(lNewTS)
        End If

      Case Else
        Err.Raise(aOperationName & " not yet implemented")
    End Select
    If Me.DataSets.Count > 0 Then Return True
  End Function

  'Should only be used within Open routine since results of computations are all that should get added
  'Appends a new history entry to track this computation
  Public Overrides Function AddDataSet(ByRef t As atcData.atcDataSet, Optional ByRef ExistAction As atcData.atcDataSource.EnumExistAction = atcData.atcDataSource.EnumExistAction.ExistReplace) As Boolean
    Dim prevHistory As String
    Dim iHistory As Integer = 0
    Do
      iHistory += 1
      prevHistory = t.Attributes.GetValue("History " & iHistory, Nothing)
    Loop While Not prevHistory Is Nothing
    t.Attributes.SetValue("Data Source", Specification)
    t.Attributes.SetValue("History " & iHistory, Specification)
    MyBase.AddDataSet(t)
  End Function

  Public Overrides Sub Initialize(ByVal MapWin As MapWindow.Interfaces.IMapWin, ByVal ParentHandle As Integer)
  End Sub

  'Public Overrides Sub PopulateInterface(ByVal aOperationName As String, _
  '                             ByVal aControls As Control.ControlCollection, _
  '                             ByVal aDataManager As atcDataManager)
  '  pControls = aControls
  '  pControls.Clear()
  '  pDataManager = aDataManager
  '  cboTimeseries = New System.Windows.Forms.ComboBox
  '  cboTimeseries.Top = cboTimeseries.Height
  '  cboTimeseries.Left = cboTimeseries.Top
  '  For Each source As atcDataSource In pDataManager.DataSources
  '    For Each ts As atcTimeseries In source.DataSets
  '      cboTimeseries.Items.Add(ts.ToString)
  '    Next
  '  Next
  '  cboTimeseries.Anchor = AnchorStyles.Left + AnchorStyles.Right + AnchorStyles.Top
  '  pControls.Add(cboTimeseries)
  'End Sub

  'Public Overrides Function ExtractArgs() As ArrayList
  '  ExtractArgs = New ArrayList
  '  For Each source As atcDataSource In pDataManager.DataSources
  '    For Each ts As atcTimeseries In source.DataSets
  '      If cboTimeseries.SelectedItem = ts.ToString Then
  '        ExtractArgs.Add(ts)
  '      End If
  '    Next
  '  Next
  'End Function

End Class
