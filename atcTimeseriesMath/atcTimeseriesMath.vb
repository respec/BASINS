Imports atcData

Imports System.Windows.Forms

Public Class atcTimeseriesMath
  Inherits atcDataSource
  Private pAvailableTimeseriesOperations As atcDataGroup
  Private Const pName As String = "Timeseries::Math"

  Private pControls As Control.ControlCollection
  Private cboTimeseries As System.Windows.Forms.ComboBox

  Public Overrides ReadOnly Property Name() As String
    Get
      Return pName
    End Get
  End Property

  Public Overrides ReadOnly Property Category() As String
    Get
      Return "Generate Timeseries"
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

  'Operations supported
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

        Dim defCategory As New atcAttributeDefinition
        With defCategory
          .Name = "Category"
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
          .DefaultValue = ""
          .Editable = True
          .TypeString = "Double"
        End With

        Dim defDate As New atcAttributeDefinition
        With defDate
          .Name = "Date"
          .Description = "Date"
          .DefaultValue = ""
          .Editable = True
          .TypeString = "Date"
        End With
        Dim defStartDate As atcAttributeDefinition = defDate.Clone
        Dim defEndDate As atcAttributeDefinition = defDate.Clone
        defStartDate.Name = "Start Date"
        defEndDate.Name = "End Date"

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
        lData.Attributes.SetValue(defDesc, "Add to each value")
        lData.Attributes.SetValue(defTimeSeriesGroup, Nothing)
        lData.Attributes.SetValue(defDouble, 0)
        'lData.Attributes.SetValue(defBeforeAfter, Nothing)
        pAvailableTimeseriesOperations.Add(lData)

        lData = New atcDataSet
        lData.Attributes.SetValue(defName, "Subtract")
        lData.Attributes.SetValue(defDesc, "Subtract from each value of first timeseries")
        lData.Attributes.SetValue(defTimeSeriesOne, Nothing)
        lData.Attributes.SetValue(defDouble, 0)
        pAvailableTimeseriesOperations.Add(lData)

        lData = New atcDataSet
        lData.Attributes.SetValue(defName, "Multiply")
        lData.Attributes.SetValue(defDesc, "Multiply each value")
        lData.Attributes.SetValue(defTimeSeriesGroup, Nothing)
        lData.Attributes.SetValue(defDouble, 1)
        pAvailableTimeseriesOperations.Add(lData)

        lData = New atcDataSet
        lData.Attributes.SetValue(defName, "Divide")
        lData.Attributes.SetValue(defDesc, "Divide each value of first timeseries")
        lData.Attributes.SetValue(defTimeSeriesGroup, Nothing)
        lData.Attributes.SetValue(defDouble, 1)
        pAvailableTimeseriesOperations.Add(lData)

        lData = New atcDataSet
        lData.Attributes.SetValue(defName, "Mean")
        lData.Attributes.SetValue(defDesc, "Arithmetic Mean of values for each date")
        lData.Attributes.SetValue(defTimeSeriesGroup, Nothing)
        'lData.Attributes.SetValue(defBeforeAfter, Nothing)
        pAvailableTimeseriesOperations.Add(lData)

        lData = New atcDataSet
        lData.Attributes.SetValue(defName, "Geometric Mean")
        lData.Attributes.SetValue(defDesc, "Geometric Mean of values for each date")
        lData.Attributes.SetValue(defTimeSeriesGroup, Nothing)
        'lData.Attributes.SetValue(defBeforeAfter, Nothing)
        pAvailableTimeseriesOperations.Add(lData)

        lData = New atcDataSet
        lData.Attributes.SetValue(defName, "Max")
        lData.Attributes.SetValue(defDesc, "Maximum value for each date")
        lData.Attributes.SetValue(defTimeSeriesGroup, Nothing)
        lData.Attributes.SetValue(defDouble, "")
        pAvailableTimeseriesOperations.Add(lData)

        lData = New atcDataSet
        lData.Attributes.SetValue(defName, "Min")
        lData.Attributes.SetValue(defDesc, "Minimum value for each date")
        lData.Attributes.SetValue(defTimeSeriesGroup, Nothing)
        lData.Attributes.SetValue(defDouble, "")
        pAvailableTimeseriesOperations.Add(lData)

        lData = New atcDataSet
        lData.Attributes.SetValue(defName, "Exponent")
        lData.Attributes.SetValue(defDesc, "Raise each value of first timeseries to a power")
        lData.Attributes.SetValue(defTimeSeriesGroup, Nothing)
        lData.Attributes.SetValue(defDouble, "")
        pAvailableTimeseriesOperations.Add(lData)

        lData = New atcDataSet
        lData.Attributes.SetValue(defName, "e ^ x")
        lData.Attributes.SetValue(defDesc, "e raised to the power of each value")
        lData.Attributes.SetValue(defTimeSeriesOne, Nothing)
        pAvailableTimeseriesOperations.Add(lData)

        lData = New atcDataSet
        lData.Attributes.SetValue(defName, "Log e")
        lData.Attributes.SetValue(defDesc, "The log base e of each value")
        lData.Attributes.SetValue(defTimeSeriesOne, Nothing)
        pAvailableTimeseriesOperations.Add(lData)

        lData = New atcDataSet
        lData.Attributes.SetValue(defName, "Log 10")
        lData.Attributes.SetValue(defDesc, "The log base 10 of each value")
        lData.Attributes.SetValue(defTimeSeriesOne, Nothing)
        pAvailableTimeseriesOperations.Add(lData)

        lData = New atcDataSet
        lData.Attributes.SetValue(defName, "Absolute Value")
        lData.Attributes.SetValue(defDesc, "Change negative values to positive")
        lData.Attributes.SetValue(defTimeSeriesOne, Nothing)
        pAvailableTimeseriesOperations.Add(lData)

        lData = New atcDataSet
        lData.Attributes.SetValue(defName, "Celsius to F")
        lData.Attributes.SetValue(defDesc, "Celsius to Fahrenheit")
        lData.Attributes.SetValue(defCategory, "Unit Conversion")
        lData.Attributes.SetValue(defTimeSeriesOne, Nothing)
        pAvailableTimeseriesOperations.Add(lData)

        lData = New atcDataSet
        lData.Attributes.SetValue(defName, "F to Celsius")
        lData.Attributes.SetValue(defDesc, "Fahrenheit to Celsius")
        lData.Attributes.SetValue(defCategory, "Unit Conversion")
        lData.Attributes.SetValue(defTimeSeriesOne, Nothing)
        pAvailableTimeseriesOperations.Add(lData)

        lData = New atcDataSet
        lData.Attributes.SetValue(defName, "Subset by date")
        lData.Attributes.SetValue(defDesc, "Choose start and end dates")
        lData.Attributes.SetValue(defCategory, "Date")
        lData.Attributes.SetValue(defTimeSeriesOne, Nothing)
        lData.Attributes.SetValue(defStartDate, "")
        lData.Attributes.SetValue(defEndDate, "")
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
    Dim lNumber As Double
    Dim lHaveNumber As Boolean = False
    Dim nArgs As Integer = 0
    Dim lastValueIndex As Integer = -1
    Dim iValue As Integer

    ReDim newVals(-1) ' If this gets populated, it will be turned into an atcTimeseries at the end

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

    If aArgs.ContainsAttribute("Number") AndAlso Not aArgs.GetValue("Number") Is Nothing Then
      lHaveNumber = True
      nArgs += 1
      lNumber = CDbl(aArgs.GetValue("Number", 0))
      Specification &= " " & lNumber
    End If

    Dim lTSgroup As atcDataGroup = aArgs.GetValue("Timeseries", Nothing)
    If lTSgroup Is Nothing OrElse lTSgroup.Count < 1 Then
      Err.Raise(vbObjectError + 512, Me, aOperationName & " did not get a Timeseries argument")
    End If

    firstTS = lTSgroup.Item(0)
    If lTSgroup.Count > 1 Then
      curTS = lTSgroup.Item(1) 'default the current ts to the one after the first
    End If

    lastValueIndex = firstTS.numValues
    ReDim newVals(lastValueIndex)
    Array.Copy(firstTS.Values, newVals, lastValueIndex + 1) 'copy values from firstTS
    nArgs += lTSgroup.Count

    'TODO: check here for number of arguments instead of in each case?

    Select Case aOperationName.ToLower
      Case "add", "+"
        For iValue = 0 To lastValueIndex
          If lHaveNumber Then newVals(iValue) += lNumber
          For iTS = 1 To lTSgroup.Count - 1
            curTS = lTSgroup.Item(iTS)
            newVals(iValue) += curTS.Value(iValue)
          Next
        Next

      Case "subtract", "-"
        If nArgs <> 2 Then
          Err.Raise(vbObjectError + 512, Me, aOperationName & " required two arguments but got " & nArgs)
        ElseIf lHaveNumber Then
          For iValue = 0 To lastValueIndex
            newVals(iValue) -= lNumber
          Next
        Else
          For iValue = 0 To lastValueIndex
            newVals(iValue) -= curTS.Value(iValue)
          Next
        End If

      Case "multiply", "*"
        For iValue = 0 To lastValueIndex
          If lHaveNumber Then newVals(iValue) *= lNumber
          For iTS = 1 To lTSgroup.Count - 1
            curTS = lTSgroup.Item(iTS)
            newVals(iValue) *= curTS.Value(iValue)
          Next
        Next

      Case "divide", "/"
        If nArgs <> 2 Then
          Err.Raise(vbObjectError + 512, Me, aOperationName & " required two arguments but got " & nArgs)
        ElseIf lHaveNumber Then
          If Math.Abs(lNumber) < 0.000001 Then
            Err.Raise(vbObjectError + 512, Me, aOperationName & " got a divisor too close to zero (" & lNumber & ")")
          Else
            For iValue = 0 To lastValueIndex
              newVals(iValue) /= lNumber
            Next
          End If
        Else
          For iValue = 0 To lastValueIndex
            newVals(iValue) /= curTS.Value(iValue)
          Next
        End If

      Case "mean"
        For iValue = 0 To lastValueIndex
          If lHaveNumber Then newVals(iValue) += lNumber
          For iTS = 1 To lTSgroup.Count - 1
            curTS = lTSgroup.Item(iTS)
            newVals(iValue) += curTS.Value(iValue)
          Next
          newVals(iValue) /= nArgs
        Next

      Case "geometric mean"
        For iValue = 0 To lastValueIndex
          newVals(iValue) = Math.Log10(newVals(iValue))
          If lHaveNumber Then newVals(iValue) += Math.Log10(lNumber)
          For iTS = 1 To lTSgroup.Count - 1
            curTS = lTSgroup.Item(iTS)
            newVals(iValue) += Math.Log10(curTS.Value(iValue))
          Next
          newVals(iValue) = 10 ^ (newVals(iValue) / nArgs)
        Next

      Case "min"
        For iValue = 0 To lastValueIndex
          If lHaveNumber Then
            If lNumber < newVals(iValue) Then newVals(iValue) = lNumber
          End If
          For iTS = 1 To lTSgroup.Count - 1
            curTS = lTSgroup.Item(iTS)
            If curTS.Value(iValue) < newVals(iValue) Then
              newVals(iValue) = curTS.Value(iValue)
            End If
          Next
        Next
      Case "max"
        For iValue = 0 To lastValueIndex
          If lHaveNumber Then
            If lNumber > newVals(iValue) Then newVals(iValue) = lNumber
          End If
          For iTS = 1 To lTSgroup.Count - 1
            curTS = lTSgroup.Item(iTS)
            If curTS.Value(iValue) > newVals(iValue) Then
              newVals(iValue) = curTS.Value(iValue)
            End If
          Next
        Next
      Case "exponent", "exp", "^", "**"
        If nArgs <> 2 Then
          Err.Raise(vbObjectError + 512, Me, aOperationName & " required two arguments but got " & nArgs)
        ElseIf lHaveNumber Then
          For iValue = 0 To lastValueIndex
            newVals(iValue) ^= lNumber
          Next
        Else
          For iValue = 0 To lastValueIndex
            newVals(iValue) ^= curTS.Value(iValue)
          Next
        End If
      Case "e**", "e ^ x"
        For iValue = 0 To lastValueIndex
          newVals(iValue) = Math.Exp(newVals(iValue))
        Next
      Case "log 10"
        For iValue = 0 To lastValueIndex
          newVals(iValue) = Math.Log10(newVals(iValue))
        Next
      Case "log e"
        For iValue = 0 To lastValueIndex
          newVals(iValue) = Math.Log(newVals(iValue))
        Next
        'Case "line"
        '  For valNum = 1 To NVALS
        '    argNum = 1
        '    GoSub SetCurArgVal
        '    dataval(valNum) = curArgVal
        '    argNum = 2
        '    GoSub SetCurArgVal
        '    dataval(valNum) = dataval(valNum) * curArgVal
        '    argNum = 3
        '    GoSub SetCurArgVal
        '    dataval(valNum) = dataval(valNum) + curArgVal
        '  Next
      Case "abs", "absolute value"
        For iValue = 0 To lastValueIndex
          newVals(iValue) = Math.Abs(newVals(iValue))
        Next
      Case "ctof", "celsiustofahrenheit", "celsius to fahrenheit", "celsius to f"
        For iValue = 0 To lastValueIndex
          newVals(iValue) = newVals(iValue) * 9 / 5 + 32
        Next
      Case "ftoc", "fahrenheittocelsius", "fahrenheit to celsius", "f to celsius"
        For iValue = 0 To lastValueIndex
          newVals(iValue) = (newVals(iValue) - 32) * 5 / 9
        Next
      Case "subset by date"
        If aArgs.ContainsAttribute("Start Date") AndAlso Not aArgs.GetValue("Start Date") Is Nothing _
        AndAlso aArgs.ContainsAttribute("End Date") AndAlso Not aArgs.GetValue("End Date") Is Nothing Then
          Dim lArg As Object
          lArg = aArgs.GetValue("Start Date")
          If TypeOf (lArg) Is String Then
            lArg = System.DateTime.Parse(lArg).ToOADate
          End If
          Dim StartDate As Double = CDbl(lArg)
          lArg = aArgs.GetValue("End Date")
          If TypeOf (lArg) Is String Then
            lArg = System.DateTime.Parse(lArg).ToOADate
          End If
          Dim EndDate As Double = CDbl(lArg)
          AddDataSet(SubsetByDate(firstTS, StartDate, EndDate))
        End If
        ReDim newVals(-1) 'Don't create new timeseries below

        '  '      Case "running sum"
        '  '        valNum = 1
        '  '        GoSub SetCurArgVal
        '  '        dataval(valNum) = curArgVal
        '  '        For valNum = 2 To NVALS
        '  '          GoSub SetCurArgVal
        '  '          dataval(valNum) = dataval(valNum - 1) + curArgVal
        '  '        Next
        'Case "weight"
        '  For valNum = 1 To NVALS
        '    dataval(valNum) = 0
        '    argNum = 1
        '    While argNum < Nargs
        '      GoSub SetCurArgVal
        '      weightVal = curArgVal
        '      argNum = argNum + 1
        '      GoSub SetCurArgVal
        '      dataval(valNum) = dataval(valNum) + curArgVal * weightVal
        '      argNum = argNum + 1
        '    End While
        '  Next
        'Case "interpolate"

      Case Else
          ReDim newVals(-1) 'Don't create new timeseries
          Err.Raise(vbObjectError + 512, Me, aOperationName & " not implemented")
    End Select

    If newVals.GetUpperBound(0) >= 0 Then
      lNewTS = New atcTimeseries(Me)
      lNewTS.Values = newVals

      If Not firstTS Is Nothing Then
        lNewTS.Dates = firstTS.Dates
      Else
        Err.Raise(vbObjectError + 512, Me, "Did not get dates for new computed timeseries " & aOperationName)
      End If

      If Not lTSgroup Is Nothing AndAlso lTSgroup.Count > 0 Then
        lNewTS.Attributes.SetValue("Parent Timeseries", lTSgroup)
      End If
      If lHaveNumber Then
        lNewTS.Attributes.SetValue("Parent Constant", lNumber)
      End If
      AddDataSet(lNewTS)
    End If
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

  'WARNING: cousin copy in atcTimeseriesNdayHighLow
  Private Function SubsetByDate(ByVal aTimeseries As atcTimeseries, _
                              ByVal aStartDate As Double, _
                              ByVal aEndDate As Double) As atcTimeseries
    'TODO: boundary conditions...
    Dim iStart As Integer = 0
    Dim iEnd As Integer = aTimeseries.numValues
    Dim numNewValues As Integer = iEnd + 1

    'TODO: binary search for iStart and iEnd could be faster
    While iStart < iEnd AndAlso aTimeseries.Dates.Value(iStart) < aStartDate
      iStart += 1
    End While

    While iEnd > iStart AndAlso aTimeseries.Dates.Value(iEnd) > aEndDate
      iEnd -= 1
    End While

    numNewValues = iEnd - iStart + 1
    Dim newValues(numNewValues) As Double
    Dim newDates(numNewValues) As Double

    If iStart > 0 Then
      System.Array.Copy(aTimeseries.Values, iStart - 1, newValues, 0, numNewValues + 1)
      System.Array.Copy(aTimeseries.Dates.Values, iStart - 1, newDates, 0, numNewValues + 1)
    Else
      System.Array.Copy(aTimeseries.Values, iStart, newValues, 0, numNewValues)
      System.Array.Copy(aTimeseries.Dates.Values, iStart, newDates, 0, numNewValues)
    End If
    Dim lnewTS As New atcTimeseries(Me)
    lnewTS.Dates = New atcTimeseries(Me)
    lnewTS.Values = newValues
    lnewTS.Dates.Values = newDates

    lnewTS.Attributes.ChangeTo(aTimeseries.Attributes)

    'TODO: fix me
    'For Each lAttribute As atcDefinedValue In lnewTS.Attributes
    'If lAttribute.Definition.Calculated Then
    'lnewTS.Attributes.Remove(lAttribute)
    'End If
    'Next

    lnewTS.Attributes.SetValue("Parent Timeseries", aTimeseries)
    Return lnewTS
  End Function
End Class
