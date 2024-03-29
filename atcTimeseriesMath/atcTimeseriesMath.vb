Imports atcData
Imports atcUtility

Imports System.Windows.Forms

Public Class atcTimeseriesMath
    Inherits atcTimeseriesSource
    Implements IDataMemory
    Private pAvailableOperations As atcDataAttributes
    Private Const pName As String = "Timeseries::Math"
    Friend Shared pIcon As System.Drawing.Icon = Nothing

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

    Private _sharedates As Boolean = True
    Public Property ShareDates() As Boolean Implements IDataMemory.ShareDates
        Get
            Return _sharedates
        End Get
        Set(value As Boolean)
            _sharedates = value
        End Set
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
                    .Name = "OneOrMoreTimeseries"
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
                    .Description = "Boundary Month"
                    .DefaultValue = 10
                    .Editable = True
                    .TypeString = "Integer"
                    .Min = 1
                    .Max = 12
                End With

                Dim defBoundaryDay As New atcAttributeDefinition
                With defBoundaryDay
                    .Name = "Boundary Day"
                    .Description = "Boundary Day"
                    .DefaultValue = 1
                    .Editable = True
                    .TypeString = "Integer"
                    .Min = 1
                    .Max = 31
                End With

                AddOperation("Add", "Add to each value", "Math", defTimeSeriesGroup, defDouble)

                AddOperation("Subtract", "Subtract from each value of first timeseries", "Math", defTimeSeriesGroup, defDouble)

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
        Me.DataSets.Clear() 'clear out any old datasets from previous computations

        If String.IsNullOrEmpty(aOperationName) Then
            'TODO: ask user which operation to perform
            aOperationName = "Add"
        End If
        Specification = aOperationName

        Dim lNeedToAsk As Boolean = False
        Dim lArgs As New atcDataAttributes
        If aArgs Is Nothing Then
            Dim lOperation As atcDefinedValue = AvailableOperations.GetDefinedValue(aOperationName)
            If lOperation Is Nothing Then
                Throw New ApplicationException("Unknown operation and no arguments for " & aOperationName)
            Else
                lNeedToAsk = True
                lArgs = lOperation.Arguments.Clone
            End If
        Else
            lArgs = aArgs.Clone
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
#If BatchMode Then
            Throw New ApplicationException("TimeseriesMath: Arguments not specified to Open")
#Else
            Select Case aOperationName
                Case "Subtract", "Divide"
                    Dim lSpecify As New frmSpecifySubtract
                    If pIcon IsNot Nothing Then lSpecify.Icon = pIcon
                    lSpecify.Text = Me.Category & ": " & aOperationName
                    If aOperationName = "Divide" Then
                        lSpecify.radioNumberMinusTS.Text = lSpecify.radioNumberMinusTS.Text.Replace("-", "/")
                        lSpecify.radioTS1MinusTS2.Text = lSpecify.radioTS1MinusTS2.Text.Replace("-", "/")
                        lSpecify.radioTsMinusNumber.Text = lSpecify.radioTsMinusNumber.Text.Replace("-", "/")
                    End If
                    If Not lSpecify.AskUser(lArgs) Then
                        Return False 'User cancelled
                    End If
                Case Else
                    Dim lSpecify As New frmSpecifyComputation
                    If pIcon IsNot Nothing Then lSpecify.Icon = pIcon
                    lSpecify.Text = Me.Category & ": " & aOperationName
                    If Not lSpecify.AskUser(lArgs) Then
                        Return False 'User cancelled
                    End If
            End Select
#End If
        End If

        Try
            lArgs.Add("ShareDates", Me.ShareDates)
            Dim lNewTS As atcTimeseries = DoMath(aOperationName, lArgs)
            If lNewTS IsNot Nothing Then
                AddDataSet(lNewTS)
                Return True
            End If
        Catch lex As Exception
            MapWinUtility.Logger.Msg(lex.ToString, "Could not perform time series math")
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

    Public Sub New()
    End Sub

    Public Sub New(ByVal aIcon As System.Drawing.Icon)
        If aIcon IsNot Nothing Then
            pIcon = aIcon
        End If
    End Sub
End Class
