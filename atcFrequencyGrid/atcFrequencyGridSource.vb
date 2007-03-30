Imports atcData
Imports atcUtility

Friend Class atcFrequencyGridSource
    Inherits atcControls.atcGridSource

    Private pDataGroup As atcDataGroup
    Private pNdays As SortedList
    Private pRecurrence As SortedList
    Private pHigh As Boolean

    Private pCalculatedNdays As New ArrayList
    Private pCalculatedRecurrence As New ArrayList

    Sub New(ByVal aDataGroup As atcData.atcDataGroup)
        pDataGroup = aDataGroup
        pRecurrence = New SortedList
        pNdays = New SortedList
        Dim lKey As String
        MyBase.ColorCells = True
        For Each lData As atcDataSet In pDataGroup
            For Each lAttribute As atcDefinedValue In lData.Attributes
                If Not lAttribute.Arguments Is Nothing Then
                    If lAttribute.Arguments.ContainsAttribute("Nday") Then
                        Dim lNdays As String = lAttribute.Arguments.GetFormattedValue("Nday")
                        lKey = Format(lAttribute.Arguments.GetValue("Nday"), "00000.0000")
                        If Not pNdays.ContainsKey(lKey) Then
                            pNdays.Add(lKey, lNdays)
                        End If
                    End If
                    If lAttribute.Arguments.ContainsAttribute("Return Period") Then
                        Dim lNyears As String = lAttribute.Arguments.GetFormattedValue("Return Period")
                        lKey = Format(lAttribute.Arguments.GetValue("Return Period"), "00000.0000")
                        If Not pRecurrence.ContainsKey(lKey) Then
                            pRecurrence.Add(lKey, lNyears)
                        End If
                    End If
                End If
            Next
        Next
    End Sub

    Public Property High() As Boolean
        Get
            Return pHigh
        End Get
        Set(ByVal newValue As Boolean)
            pHigh = newValue
        End Set
    End Property

    Overrides Property Columns() As Integer
        Get
            If pNdays Is Nothing Then
                Return 3
            Else
                Return pNdays.Count + 2
            End If
        End Get
        Set(ByVal Value As Integer)
        End Set
    End Property

    Overrides Property Rows() As Integer
        Get
            Try
                Return pDataGroup.Count * pRecurrence.Count + 1
            Catch
                Return 1
            End Try
        End Get
        Set(ByVal Value As Integer)
        End Set
    End Property

    Public Function DataSetAt(ByVal aRow As Integer) As atcDataSet
        Return pDataGroup((aRow - 1) \ pRecurrence.Count)
    End Function

    Public Function NdaysAt(ByVal aColumn As Integer) As String
        Return pNdays.GetByIndex(aColumn - 2)
    End Function

    Public Function RecurrenceAt(ByVal aRow As Integer) As String
        'remove any thousands commas in return period
        Return ReplaceString(pRecurrence.GetByIndex((aRow - 1) Mod pRecurrence.Count), ",", "")
    End Function

    Overrides Property CellValue(ByVal aRow As Integer, ByVal aColumn As Integer) As String
        Get
            If aRow = 0 Then
                Select Case aColumn
                    Case 0 : Return "Data Set"
                    Case 1 : Return "Return Period"
                    Case Else : Return NdaysAt(aColumn)
                End Select
            Else
                Select Case aColumn
                    Case 0 : Return DataSetAt(aRow).ToString
                    Case 1 : Return RecurrenceAt(aRow)
                    Case Else
                        Dim lDataSet As atcDataSet = DataSetAt(aRow)
                        Dim lAttrName As String = NdaysAt(aColumn)
                        If pHigh Then lAttrName &= "High" Else lAttrName &= "Low"
                        lAttrName &= RecurrenceAt(aRow)

                        If Not lDataSet.Attributes.ContainsAttribute(lAttrName) Then
                            Try
                                Dim lCalculator As New atcTimeseriesNdayHighLow.atcTimeseriesNdayHighLow
                                Dim lArgs As New atcDataAttributes
                                Dim lOperationName As String
                                Dim lNdays(pNdays.Count) As Double
                                Dim lNextNdays As Integer = 1
                                Dim lReturns(pRecurrence.Count) As Double
                                Dim lNextReturns As Double = 1
                                Dim lValue As Double

                                If pHigh Then
                                    lOperationName = "n-day high value"
                                Else
                                    lOperationName = "n-day low value"
                                End If

                                lArgs.SetValue("Timeseries", lDataSet)

                                lNdays(0) = NdaysAt(aColumn)
                                For Each lNday As DictionaryEntry In pNdays
                                    lValue = CDbl(lNday.Value)
                                    If lValue <> lNdays(0) AndAlso Not pCalculatedNdays.Contains(lValue) Then
                                        pCalculatedNdays.Add(lValue)
                                        lNdays(lNextNdays) = lValue
                                        lNextNdays += 1
                                    End If
                                Next

                                lReturns(0) = RecurrenceAt(aColumn)
                                For Each lReturn As DictionaryEntry In pRecurrence
                                    lValue = CDbl(ReplaceString(lReturn.Value, ",", ""))
                                    If lValue <> lReturns(0) AndAlso Not pCalculatedRecurrence.Contains(lValue) Then
                                        pCalculatedRecurrence.Add(lValue)
                                        lReturns(lNextReturns) = lValue
                                        lNextReturns += 1
                                    End If
                                Next

                                ReDim Preserve lNdays(lNextNdays - 1)
                                ReDim Preserve lReturns(lNextReturns - 1)

                                lArgs.SetValue("NDay", lNdays)
                                lArgs.SetValue("Return Period", lReturns)

                                lCalculator.Open(lOperationName, lArgs)
                            Catch e As Exception
                                'LogDbg(Me.Name & " Could not calculate value at row " & aRow & ", col " & aColumn & ". " & e.ToString)
                            End Try
                        End If

                        CellValue = lDataSet.Attributes.GetFormattedValue(lAttrName)
                        If CellValue = "NaN" Then CellValue = ""

                End Select
            End If
        End Get
        Set(ByVal Value As String)
        End Set
    End Property

    Overrides Property Alignment(ByVal aRow As Integer, ByVal aColumn As Integer) As atcControls.atcAlignment
        Get
            If aColumn > 0 Then
                Return atcControls.atcAlignment.HAlignDecimal
            Else
                Return atcControls.atcAlignment.HAlignLeft
            End If
        End Get
        Set(ByVal Value As atcControls.atcAlignment)
        End Set
    End Property

    Overrides Property CellColor(ByVal aRow As Integer, ByVal aColumn As Integer) As System.Drawing.Color
        Get
            If aColumn > 1 AndAlso aRow > 0 Then
                Return System.Drawing.SystemColors.Window
            Else
                Return System.Drawing.SystemColors.Control
            End If
        End Get
        Set(ByVal Value As System.Drawing.Color)
        End Set
    End Property
End Class
