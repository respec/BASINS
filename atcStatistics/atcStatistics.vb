Imports atcData
Imports atcUtility
Imports MapWinUtility

Imports System.Windows.Forms

''' <summary>This class differ from atcTimeseriesStatistics in that
''' it conduct statistical analysis involving more than one timeseries
''' such as regression and curve fitting
''' </summary>
Public Class atcStatistics
    Public Shared Results As atcDataAttributes

    Private pAvailableOperations As atcDataAttributes ' atcDataGroup
    Private Shared pNaN As Double = GetNaN()
    Private Shared pMinValue As Double = GetMinValue()
    Private Shared pMaxValue As Double = GetMaxValue()

    Public Sub New()
        Results = New atcDataAttributes()
    End Sub

    ''' <summary>Definitions of statistics supported by this class.</summary>
    Public ReadOnly Property AvailableOperations() As atcDataAttributes
        Get
            If pAvailableOperations Is Nothing Then
                pAvailableOperations = New atcDataAttributes

                Dim lCategory As String = "Statistics"

                Dim defTimeSeriesOne As New atcAttributeDefinition
                With defTimeSeriesOne
                    .Name = "Data"
                    .Description = "One array of numbers"
                    .Editable = True
                    .TypeString = "Array"
                End With

                AddOperation("regression", "linear regression", defTimeSeriesOne, lCategory, "Double", pNaN)

            End If
            Return pAvailableOperations
        End Get
    End Property

    Private Sub AddOperation(ByVal aName As String, _
                             ByVal aDescription As String, _
                             ByVal aArg As atcAttributeDefinition, _
                             ByVal aCategory As String, _
                     ByVal aTypeString As String, _
                     ByVal aDefaultValue As Object)
        Dim lResult As New atcAttributeDefinition
        With lResult
            .Name = aName
            .Category = aCategory
            .Description = aDescription
            .DefaultValue = aDefaultValue
            .Editable = False
            .TypeString = aTypeString
            .Calculator = Nothing
        End With
        Dim lArguments As atcDataAttributes = New atcDataAttributes
        lArguments.SetValue(aArg, Nothing)
        pAvailableOperations.SetValue(lResult, Nothing, lArguments)

    End Sub

    'Compute all available statistics for aTimeseries and add them as attributes
    Private Sub ComputeStatistics(ByVal aTimeseries As atcTimeseries)

    End Sub

    'The only element of aArgs is an atcDataGroup or atcTimeseries
    'The attribute(s) will be set to the result(s) of calculation(s)
    Public Function Open(ByVal aOperationName As String, Optional ByVal aArgs As atcDataAttributes = Nothing) As Boolean
        Dim ltsGroup As atcTimeseriesGroup = Nothing
        If aArgs Is Nothing Then
#If BatchMode Then
#Else
            ltsGroup = atcDataManager.UserSelectData("Select data to compute statistics for")
#End If
        Else
            ltsGroup = DatasetOrGroupToGroup(aArgs.GetValue("Timeseries"))
        End If

        If ltsGroup IsNot Nothing Then
            If aOperationName.ToLower.StartsWith("regression") AndAlso ltsGroup.Count = 2 Then
                DoRegression(ltsGroup)

                'ElseIf aOperationName.StartsWith("%") Then
                '    Dim lPercentString As String = aOperationName.Substring(1)
                '    If IsNumeric(lPercentString) Then
                '        ComputePercentile(lts, CDbl(lPercentString))
                '    End If
                'ElseIf aOperationName.ToLower.Equals("bins") Then
                '    lts.Attributes.SetValue("Bins", MakeBins(lts))
                'Else
                '    ComputeStatistics(lts)
            End If
        End If
    End Function


    ' -----THIS SUBROUTINE PERFORMS LEAST-SQUARES REGRESSION TO FIND BEST-FIT ---
    ' ---------------- EQUATION OF LINEAR BASIS ( Y = A*X + B ) -----------------
    Public Shared Sub DoRegression(ByVal aTsGroup As atcTimeseriesGroup)
        Dim lA(2, 4) As Double
        Dim lRoot1 As Double = 0.0
        Dim lRoot2 As Double = 0.0
        Dim lY() As Double
        Dim lX() As Integer
        Dim lArraySize As Integer = aTsGroup(0).numValues
        ReDim lY(lArraySize)
        ReDim lX(lArraySize)
        For I As Integer = 1 To lArraySize
            lY(I) = aTsGroup(1).Value(I)
            lX(I) = aTsGroup(0).Value(I)
        Next
        For I As Integer = 1 To lArraySize
            lA(1, 1) += lY(I) ^ 2
            lA(1, 2) += lY(I)
            lA(2, 1) += lY(I)
            lRoot1 += lY(I) * lX(I)
            lRoot2 += lX(I)
        Next

        lA(2, 2) = lArraySize
        Dim lN As Integer = 2

        Inverse(lA, lN)

        Results.Add("Coefficient1", lA(1, 1) * lRoot1 + lA(1, 2) * lRoot2)
        Results.Add("Coefficient2", lA(2, 1) * lRoot1 + lA(2, 2) * lRoot2)

    End Sub

    ' -----THIS SUBROUTINE PERFORMS LEAST-SQUARES REGRESSION TO FIND BEST-FIT ---
    ' ---------------- EQUATION OF LINEAR BASIS ( Y = A*X + B ) -----------------
    Public Sub DoRegression(ByVal aX() As Double, ByVal aY() As Double, ByVal aNumOfStepsInPeriod As Integer, ByRef aCoeff1 As Double, ByRef aCoeff2 As Double)
        Dim lA(2, 4) As Double
        Dim lRoot1 As Double = 0.0
        Dim lRoot2 As Double = 0.0
        For I As Integer = 1 To aNumOfStepsInPeriod
            lA(1, 1) += aX(I) ^ 2
            lA(1, 2) += aX(I)
            lA(2, 1) += aX(I)
            lRoot1 += aX(I) * aY(I)
            lRoot2 += aY(I)
        Next

        lA(2, 2) = aNumOfStepsInPeriod
        Dim lN As Integer = 2

        Inverse(lA, lN)

        aCoeff1 = lA(1, 1) * lRoot1 + lA(1, 2) * lRoot2
        aCoeff2 = lA(2, 1) * lRoot1 + lA(2, 2) * lRoot2

        'Results.Add("Coefficient1", aCoeff1)
        'Results.Add("Coefficient2", aCoeff2)
    End Sub

    ' -----  THIS SUBROUTINE CHANGES AN N*N MATRIX [A] TO ITS INVERSE -----
    ' ---------  (Note: The matrix is actually N*(2N) internally) ---------
    Public Shared Sub Inverse(ByRef A(,) As Double, ByVal N As Integer)
        For I As Integer = 1 To N
            For J As Integer = N + 1 To 2 * N
                A(I, J) = 0.0
            Next
        Next

        For I As Integer = 1 To N
            A(I, N + I) = 1.0
        Next

        For K As Integer = 1 To N
            For I As Integer = 1 To N
                Dim lTemp As Double = A(I, K)
                For J As Integer = 1 To 2 * N
                    A(I, J) /= lTemp
                Next
            Next

            For I As Integer = 1 To N
                If I <> K Then
                    For J As Integer = 1 To 2 * N
                        A(I, J) -= A(K, J)
                    Next
                End If
            Next
        Next

        For I As Integer = 1 To N
            Dim lAP As Double = A(I, I)
            For J As Integer = 1 To 2 * N
                A(I, J) /= lAP
            Next
        Next

        For I As Integer = 1 To N
            For J As Integer = 1 To N
                A(I, J) = A(I, J + N)
            Next
        Next
    End Sub
End Class
