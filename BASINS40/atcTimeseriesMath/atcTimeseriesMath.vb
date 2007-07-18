Imports atcData

Imports System.Windows.Forms

Public Class atcTimeseriesMath
    Inherits atcData.atcDataSource
    Private pAvailableOperations As atcDataAttributes
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
    Public Overrides ReadOnly Property AvailableOperations() As atcDataAttributes
        Get
            If pAvailableOperations Is Nothing Then
                pAvailableOperations = New atcDataAttributes

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

                Dim defBoundaryMonth As New atcAttributeDefinition
                With defBoundaryMonth
                    .Name = "Boundary Month"
                    .Description = "Integer"
                    .DefaultValue = 10
                    .Editable = True
                    .TypeString = "Integer"
                    .Min = 1
                    .Max = 12
                End With

                Dim defBoundaryDay As New atcAttributeDefinition
                With defBoundaryDay
                    .Name = "Boundary Day"
                    .Description = "Integer"
                    .DefaultValue = 1
                    .Editable = True
                    .TypeString = "Integer"
                    .Min = 1
                    .Max = 31
                End With

                AddOperation("Add", "Add to each value", "Math", defTimeSeriesGroup, defDouble)

                AddOperation("Subtract", "Subtract from each value of first timeseries", "Math", defTimeSeriesOne, defDouble)

                AddOperation("Multiply", "Multiply each value", "Math", defTimeSeriesGroup, defDouble)

                AddOperation("Divide", "Divide each value of first timeseries", "Math", defTimeSeriesGroup, defDouble)

                AddOperation("Mean", "Arithmetic Mean of values for each date", "Math", defTimeSeriesGroup)

                AddOperation("Geometric Mean", "Geometric Mean of values for each date", "Math", defTimeSeriesGroup)

                AddOperation("Max", "Maximum value for each date", "Math", defTimeSeriesGroup, defDouble)

                AddOperation("Min", "Minimum value for each date", "Math", defTimeSeriesGroup, defDouble)

                AddOperation("Exponent", "Raise each value of first timeseries to a power", "Math", defTimeSeriesGroup, defDouble)

                AddOperation("e ^ x", "e raised to the power of each value", "Math", defTimeSeriesOne)

                AddOperation("10 ^ x", "10 raised to the power of each value", "Math", defTimeSeriesOne)

                AddOperation("Log e", "The log base e of each value", "Math", defTimeSeriesOne)

                AddOperation("Log 10", "The log base 10 of each value", "Math", defTimeSeriesOne)

                AddOperation("Absolute Value", "Change negative values to positive", "Math", defTimeSeriesOne)

                AddOperation("Celsius to F", "Celsius to Fahrenheit", "Unit Conversion", defTimeSeriesOne)

                AddOperation("F to Celsius", "Fahrenheit to Celsius", "Unit Conversion", defTimeSeriesOne)

                AddOperation("Subset by date", "Choose start and end dates", "Date", defTimeSeriesOne, defStartDate, defEndDate)

                AddOperation("Subset by date boundary", "Choose boundary month and day", "Date", defTimeSeriesOne, defBoundaryMonth, defBoundaryDay)

                AddOperation("Merge", "Choose data to merge", "Date", defTimeSeriesGroup)
            End If
            Return pAvailableOperations
        End Get
    End Property

    Private Sub AddOperation(ByVal aName As String, _
                                ByVal aDescription As String, _
                                ByVal aCategory As String, _
                                ByVal ParamArray aArgs() As atcAttributeDefinition)
        Dim lResult As New atcAttributeDefinition
        With lResult
            .Name = aName
            .Description = aDescription
            .DefaultValue = ""
            .Editable = False
            .TypeString = "atcTimeseries"
            .Calculator = Me
            .Category = aCategory
        End With
        Dim lArguments As atcDataAttributes = New atcDataAttributes
        For Each lArg As atcAttributeDefinition In aArgs
            lArguments.SetValue(lArg, Nothing)
        Next
        pAvailableOperations.SetValue(lResult, Nothing, lArguments)

    End Sub

    'Args are each usually either Double or atcTimeseries
    Public Overrides Function Open(ByVal aOperationName As String, Optional ByVal aArgs As atcDataAttributes = Nothing) As Boolean
        Dim newDataSource As New atcDataSource
        newDataSource.Specification = "Computed by " & pName
        Dim newVals() As Double
        'Dim retval As New atcTimeseries(newDataSource)
        Dim iTS As Integer
        Dim curTS As atcTimeseries = Nothing
        Dim firstTS As atcTimeseries
        Dim needToAsk As Boolean = False
        'Dim lSelectedOperation As atcDataSet
        Dim lNewTS As atcTimeseries
        Dim lNumber As Double
        Dim lHaveNumber As Boolean = False
        Dim nArgs As Integer = 0
        Dim lastValueIndex As Integer = -1
        Dim iValue As Integer
        Dim lTSgroup As atcDataGroup

        ReDim newVals(-1) ' If this gets populated, it will be turned into an atcTimeseries at the end

        DataSets.Clear() 'assume we don't want any old datasets (maybe add arg to define this???)

        If aOperationName Is Nothing OrElse aOperationName.Length = 0 Then
            'TODO: ask user which operation to perform
            aOperationName = "Add"
        End If
        Specification = aOperationName

        Dim lOperation As atcDefinedValue = AvailableOperations.GetDefinedValue(aOperationName)
        If Not lOperation Is Nothing Then
            If aArgs Is Nothing Then
                needToAsk = True
                aArgs = lOperation.Arguments.Clone
            Else
                For Each lArg As atcDefinedValue In aArgs
                    lOperation.Arguments.SetValue(lArg.Definition.Name, lArg.Value)
                Next
                aArgs = lOperation.Arguments
            End If
        End If

        For Each lArg As atcDefinedValue In aArgs
            If lArg.Value Is Nothing Then
                needToAsk = True
            ElseIf lArg.Definition.Name = "Timeseries" AndAlso lArg.GetType.Name = "atcDataGroup" AndAlso lArg.Value.Count < 1 Then
                needToAsk = True
            End If
        Next

        If needToAsk Then
            'Ask user what to do
            Dim lSpecify As New frmSpecifyComputation
            If Not lSpecify.AskUser(aArgs) Then
                Return False 'User cancelled
            End If
        End If

        If aArgs.ContainsAttribute("Number") AndAlso Not aArgs.GetValue("Number") Is Nothing Then
            lHaveNumber = True
            nArgs += 1
            lNumber = CDbl(aArgs.GetValue("Number", 0))
            Specification &= " " & lNumber
        End If

        lTSgroup = DatasetOrGroupToGroup(aArgs.GetValue("Timeseries", Nothing))
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
                ElseIf curTS Is Nothing Then
                    Err.Raise(vbObjectError + 512, Me, aOperationName & " no current Timeseries")
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
            Case "10**", "10 ^ x"
                For iValue = 0 To lastValueIndex
                    newVals(iValue) = 10 ^ (newVals(iValue))
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
                    AddDataSet(SubsetByDate(firstTS, StartDate, EndDate, Me))
                End If
                ReDim newVals(-1) 'Don't create new timeseries below

            Case "subset by date boundary"
                Dim lBoundaryMonth As Integer = aArgs.GetValue("Boundary Month")
                Dim lBoundaryDay As Integer = aArgs.GetValue("Boundary Day")
                AddDataSet(SubsetByDateBoundary(firstTS, lBoundaryMonth, lBoundaryDay, Me))
                ReDim newVals(-1) 'Don't create new timeseries below

            Case "merge"
                AddDataSet(MergeTimeseries(lTSgroup))
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
                If lTSgroup.Count = 1 Then
                    lNewTS.Attributes.SetValue("Parent Timeseries", lTSgroup.Item(0))
                Else
                    lNewTS.Attributes.SetValue("Parent Timeseries Group", lTSgroup)
                End If
            End If
            If lHaveNumber Then
                lNewTS.Attributes.SetValue("Parent Constant", lNumber)
            End If

            CopyBaseAttributes(firstTS, lNewTS, lNewTS.numValues + 1, 0, 0)

            Dim lDateNow As Date = Now
            lNewTS.Attributes.SetValue("Date Created", lDateNow)
            lNewTS.Attributes.SetValue("Date Modified", lDateNow)

            AddDataSet(lNewTS)
        End If

        If Me.DataSets.Count > 0 Then Return True
    End Function

    'Should only be used within Open routine since results of computations are all that should get added
    'Appends a new history entry to track this computation
    Public Overrides Function AddDataSet(ByVal aDataSet As atcData.atcDataSet, Optional ByVal aExistAction As atcData.atcDataSource.EnumExistAction = atcData.atcDataSource.EnumExistAction.ExistReplace) As Boolean
        aDataSet.Attributes.SetValue("Data Source", Specification)
        aDataSet.Attributes.AddHistory(Specification)
        MyBase.AddDataSet(aDataSet, aExistAction)
    End Function

End Class
