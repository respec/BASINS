Imports atcData
Imports atcUtility

Imports System.Windows.Forms

Public Class atcTimeseriesMath
    Inherits atcTimeseriesSource
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

                AddOperation("Mean Each Date", "Arithmetic Mean of values for each date", "Math", defTimeSeriesGroup)

                AddOperation("Geometric Mean Each Date", "Geometric Mean of values for each date", "Math", defTimeSeriesGroup)

                AddOperation("Max Each Date", "Maximum value at each date", "Math", defTimeSeriesGroup, defDouble)

                AddOperation("Min Each Date", "Minimum value at each date", "Math", defTimeSeriesGroup, defDouble)

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

                AddOperation("Running Sum", "Accumulate Values", "Math", defTimeSeriesOne)

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

    Public Shared Function Compute(ByVal aOperationName As String, ByVal ParamArray aArgs() As Object) As atcTimeseries
        Dim lDataAttributes As New atcDataAttributes
        Dim lDataGroup As New atcTimeseriesGroup

        For Each lArg As Object In aArgs
            Select Case lArg.GetType.Name
                Case "atcTimeseries"
                    lDataGroup.Add(lArg)
                Case "atcDataGroup", "atcTimeseriesGroup"
                    lDataGroup.AddRange(lArg)
                Case "Double", "Single", "Integer"
                    lDataAttributes.Add("Number", lArg)
                Case Else 'TODO: more types
            End Select
        Next
        If lDataGroup.Count > 0 Then
            lDataAttributes.SetValue("timeseries", lDataGroup)
        End If

        Dim lInstance As New atcTimeseriesMath
        If lInstance.Open(aOperationName, lDataAttributes) Then
            Return lInstance.DataSets(0)
        Else
            lInstance.Clear()
            Return Nothing
        End If
    End Function

    'Args are each usually either Double or atcTimeseries
    Public Overrides Function Open(ByVal aOperationName As String, _
                          Optional ByVal aArgs As atcDataAttributes = Nothing) As Boolean
        Me.DataSets.Clear() 'assume we don't want any old datasets (maybe add arg to define this???)

        If aOperationName Is Nothing OrElse aOperationName.Length = 0 Then
            'TODO: ask user which operation to perform
            aOperationName = "Add"
        End If
        Specification = aOperationName

        Dim lNeedToAsk As Boolean = False
        Dim lOperation As atcDefinedValue = AvailableOperations.GetDefinedValue(aOperationName)
        If Not lOperation Is Nothing Then
            If aArgs Is Nothing Then
                lNeedToAsk = True
                aArgs = lOperation.Arguments.Clone
            Else
                For Each lArg As atcDefinedValue In aArgs
                    lOperation.Arguments.SetValue(lArg.Definition.Name, lArg.Value)
                Next
                aArgs = lOperation.Arguments
            End If
        End If

        'This loop checks to see if any arguments are missing, but this does not take into account optional arguments so it is commented out
        'For Each lArg As atcDefinedValue In aArgs
        '    If lArg.Value Is Nothing Then
        '        lNeedToAsk = True
        '    ElseIf lArg.Definition.Name = "Timeseries" AndAlso (lArg.GetType.Name = "atcDataGroup" OrElse lArg.GetType.Name = "atcTimeseriesGroup") AndAlso lArg.Value.Count < 1 Then
        '        lNeedToAsk = True
        '    End If
        'Next

        If lNeedToAsk Then 'Ask user what to do
            Dim lSpecify As New frmSpecifyComputation
            lSpecify.Text = Me.Category & ": " & aOperationName
            If Not lSpecify.AskUser(aArgs) Then
                Return False 'User cancelled
            End If
        End If

        Try
            Dim lNewTS As atcTimeseries = DoMath(aOperationName, aArgs)
            If lNewTS IsNot Nothing Then
                AddDataSet(lNewTS)
                Return True
            End If
        Catch lex As Exception
            Debug.Print(lex.ToString)
        End Try
        Return False
    End Function

    'Should only be used within Open routine since results of computations are all that should get added
    'Appends a new history entry to track this computation
    Public Overrides Function AddDataSet(ByVal aDataSet As atcData.atcDataSet, Optional ByVal aExistAction As atcData.atcTimeseriesSource.EnumExistAction = atcData.atcTimeseriesSource.EnumExistAction.ExistReplace) As Boolean
        aDataSet.Attributes.SetValue("Data Source", Specification)
        aDataSet.Attributes.AddHistory(Specification)
        Return MyBase.AddDataSet(aDataSet, aExistAction)
    End Function

End Class
