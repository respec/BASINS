Imports atcUtility
Imports atcData
Imports MapWinUtility

Public Class atcTimeseriesBuilder

    Private pDataSource As atcDataSource
    Private pTS As atcTimeseries
    Private pValueAttributes As atcCollection 'of atcDataAttributes
    Private pValues As Generic.List(Of Double)
    Private pDates As Generic.List(Of Double)
    Private pLastAddedIndex As Integer
    Private pLogNonNumeric As Boolean = True
    Private Shared pLogDateFormat As atcDateFormat
    Private Shared pNaN As Double = atcUtility.GetNaN
    Private Shared pNumericChars As String = ".-+eE0123456789"

    Public Sub New(ByVal aDataSource As atcDataSource)
        pDataSource = aDataSource
        Restart()
    End Sub

    ''' <summary>
    ''' Number of values that have been added to this builder so far
    ''' </summary>
    Public Function NumValues() As Integer
        Return pValues.Count
    End Function

    Public Sub Restart()
        If Not pValueAttributes Is Nothing Then pValueAttributes.Clear()
        If Not pValues Is Nothing Then pValues.Clear()
        If Not pDates Is Nothing Then pDates.Clear()

        pTS = New atcTimeseries(pDataSource)
        pValueAttributes = New atcUtility.atcCollection
        pValues = New Generic.List(Of Double)
        pDates = New Generic.List(Of Double)
    End Sub

    ''' <summary>
    ''' True to log when non-numeric values are added
    ''' </summary>
    Public Property LogNonNumeric() As Boolean
        Get
            Return pLogNonNumeric
        End Get
        Set(ByVal newValue As Boolean)
            pLogNonNumeric = newValue
        End Set
    End Property

    ''' <summary>
    ''' Append a date and value to the timeseries being built 
    ''' </summary>
    ''' <param name="aDate"></param>
    ''' <param name="aValue"></param>
    ''' <remarks></remarks>
    Public Sub AddValue(ByVal aDate As Date, ByVal aValue As Double)
        AddValue(aDate.ToOADate, aValue)
    End Sub

    ''' <summary>
    ''' Append a value and date to the timeseries being built
    ''' </summary>
    ''' <param name="aDate">Date corresponding to the value</param>
    ''' <param name="aValue">a number or non-numeric value to append</param>
    ''' <remarks></remarks>
    Public Sub AddValue(ByVal aDate As Double, ByVal aValue As String)
        Dim lValue As Double = pNaN
        Dim lFirstNumericIndex As Integer = 0
        Dim lLenNumeric As Integer = 0

        If aValue Is Nothing OrElse aValue.Trim.Length = 0 Then
            If LogNonNumeric Then
                Logger.Dbg("Missing Value at " & LogDateFormat.JDateToString(aDate))
            End If
        ElseIf IsNumeric(aValue) Then
            lValue = CDbl(aValue)
        Else
            While lFirstNumericIndex < aValue.Length AndAlso _
              Not pNumericChars.Contains(aValue.Chars(lFirstNumericIndex))
                lFirstNumericIndex += 1
            End While

            lLenNumeric = 0

            While lFirstNumericIndex + lLenNumeric < aValue.Length AndAlso _
              pNumericChars.Contains(aValue.Chars(lFirstNumericIndex + lLenNumeric))
                lLenNumeric += 1
            End While

            If lLenNumeric > 0 Then
                Try
                    lValue = CDbl(aValue.Substring(lFirstNumericIndex, lLenNumeric))
                Catch e As Exception
                    'Let lValue stay NaN
                    If LogNonNumeric Then
                        Logger.Dbg("Tried to find number but could not parse substring starting " & lFirstNumericIndex & " length " & lLenNumeric)
                    End If
                End Try
            End If

            If LogNonNumeric Then
                Logger.Dbg("NonNumericValue at " & LogDateFormat.JDateToString(aDate) & " '" & aValue & "' -> '" & lValue & "'")
            End If

        End If

        AddValue(aDate, lValue)

        If lFirstNumericIndex > 0 Then
            Me.AddValueAttribute("ValuePrefix", aValue.Substring(0, lFirstNumericIndex))
        End If

        If lLenNumeric > 0 AndAlso lFirstNumericIndex + lLenNumeric < aValue.Length Then
            Me.AddValueAttribute("ValueSuffix", aValue.Substring(lFirstNumericIndex + lLenNumeric))
        End If
    End Sub

    ''' <summary>
    ''' Append a value and date to the timeseries being built
    ''' </summary>
    ''' <param name="aDate">Date corresponding to the value</param>
    ''' <param name="aValue">a number to append to the timeseries</param>
    ''' <remarks></remarks>
    Public Sub AddValue(ByVal aDate As Double, ByVal aValue As Double)
        If pValues.Count = 0 AndAlso Not Double.IsNaN(aValue) Then
            'Value at zero is always NaN (date(0) is start of first interval for constant interval)
            pValues.Add(pNaN)
            pDates.Add(pNaN)
        End If

        Dim lOldDateIndex As Integer = pDates.Count - 1
        Dim lFoundGreaterDate As Integer = -1

        While lOldDateIndex > 0
            If Double.IsNaN(pDates(lOldDateIndex)) Then
                lOldDateIndex -= 1
            ElseIf pDates(lOldDateIndex) > aDate Then
                lFoundGreaterDate = lOldDateIndex
                lOldDateIndex -= 1
            Else
                Exit While
            End If
        End While

        If lFoundGreaterDate > -1 Then
            pLastAddedIndex = lFoundGreaterDate
            pValues.Insert(lFoundGreaterDate, aValue)
            pDates.Insert(lFoundGreaterDate, aDate)

            'Shift keys of any valueattributes at >= lFoundGreaterDate
            Dim lOldIndex As Integer
            For lIndex As Integer = pValues.Count - 1 To lFoundGreaterDate Step -1
                lOldIndex = pValueAttributes.Keys.IndexOf(pLastAddedIndex)
                If lOldIndex >= 0 Then
                    pValueAttributes.Keys(lOldIndex) += 1
                End If
            Next
        Else
            pValues.Add(aValue)
            pDates.Add(aDate)
            pLastAddedIndex = pDates.Count - 1
        End If
    End Sub

    Public ReadOnly Property Attributes() As atcDataAttributes
        Get
            Return pTS.Attributes
        End Get
    End Property

    ''' <summary>
    ''' Add an attribute to the most recently added value
    ''' </summary>
    ''' <param name="aAttributeName">Name of attribute to add</param>
    ''' <param name="aAttributeValue">Value of attribute to add</param>
    ''' <remarks></remarks>
    Public Sub AddValueAttribute(ByVal aAttributeName As String, ByVal aAttributeValue As Object)
        'pTS.ValueAttributes(pValues.Count).Add(aAttributeName, aAttributeValue)
        Dim lValueAttributes As atcDataAttributes
        Dim lIndex As Integer = pValueAttributes.Keys.IndexOf(pLastAddedIndex)
        If lIndex >= 0 Then
            lValueAttributes = pValueAttributes.ItemByIndex(lIndex)
        Else
            lValueAttributes = New atcDataAttributes
            pValueAttributes.Add(pLastAddedIndex, lValueAttributes)
        End If
        lValueAttributes.Add(aAttributeName, aAttributeValue)
    End Sub

    ''' <summary>
    ''' Add a set of attributes to the most recently added value
    ''' </summary>
    ''' <param name="aAttributes">Attributes to add</param>
    Public Sub AddValueAttributes(ByVal aAttributes As atcDataAttributes)
        Dim lValueAttributes As atcDataAttributes
        Dim lIndex As Integer = pValueAttributes.Keys.IndexOf(pLastAddedIndex)
        If lIndex >= 0 Then
            lValueAttributes = pValueAttributes.ItemByIndex(lIndex)
            For Each lAdv As atcDefinedValue In aAttributes
                If lAdv.Definition.CopiesInherit Then
                    lValueAttributes.SetValue(lAdv.Definition, lAdv.Value, lAdv.Arguments)
                End If
            Next
        Else
            lValueAttributes = aAttributes.Clone
            pValueAttributes.Add(pLastAddedIndex, lValueAttributes)
        End If
    End Sub

    Public Function CreateTimeseries() As atcTimeseries
        Dim lDates As New atcTimeseries(pDataSource)

        pTS.Values = pValues.ToArray
        lDates.Values = pDates.ToArray

        pTS.Dates = lDates

        Dim lLastValueAttribute As Integer = pValueAttributes.Count - 1
        For lValAttIndex As Integer = 0 To lLastValueAttribute
            pTS.ValueAttributes(pValueAttributes.Keys(lValAttIndex)) = pValueAttributes.ItemByIndex(lValAttIndex)
        Next

        Return pTS
    End Function

    Private ReadOnly Property LogDateFormat() As atcDateFormat
        Get
            If pLogDateFormat Is Nothing Then
                pLogDateFormat = New atcDateFormat
                pLogDateFormat.IncludeHours = True
                pLogDateFormat.IncludeMinutes = True
            End If
            Return pLogDateFormat
        End Get
    End Property
End Class
