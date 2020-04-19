''' <summary>Base class for data.</summary>
Public Class atcDataSet
    Implements IEquatable(Of atcDataSet)

    Private Shared pNextSerial As Integer = 0 'Next serial number to be assigned
    Private Shared pStringFormat As String = "{0} {1} {2}"
    Private Shared pStringAttributeNames() As String = {"Scenario", "Location", "Constituent"}

    Private pSerial As Integer 'Serial number of this object

    Private pAttributes As atcDataAttributes

    ''' <summary>Attributes associated with the whole data set(location, constituent, etc.)</summary>
    Public ReadOnly Property Attributes() As atcDataAttributes
        Get
            Return pAttributes
        End Get
    End Property

    ''' <summary>Reset data attributes to an empty collection</summary>
    Public Overridable Sub Clear()
        If pAttributes Is Nothing Then
            pAttributes = New atcDataAttributes
        ElseIf pAttributes.Count > 0 Then
            'MapWinUtility.Logger.Dbg(pSerial & " Clearing Attributes: " & pAttributes.ToString)
            pAttributes.Clear()
        End If
        pAttributes.Owner = Me
    End Sub

    ''' <summary>Create a new data set with identical attributes to current one</summary>
    Public Overridable Function Clone() As atcDataSet
        Dim lClone As New atcDataSet
        lClone.Attributes.ChangeTo(pAttributes)
        Return lClone
    End Function

    ''' <summary>Create a new data set</summary>
    Public Sub New()
        pSerial = System.Threading.Interlocked.Increment(pNextSerial)
        Clear()
    End Sub

    ''' <summary>Unique serial number assigned when data object is created</summary>
    Public ReadOnly Property Serial() As Integer
        Get
            Return pSerial
        End Get
    End Property

    Public Function EqualDatasets(ByVal otherDS As atcDataSet) As Boolean Implements IEquatable(Of atcDataSet).Equals
        If otherDS Is Nothing Then
            Return False
        End If
        Return Me.Serial = otherDS.Serial
    End Function

    Public Overrides Function Equals(obj As Object) As Boolean
        Return EqualDatasets(obj)
    End Function

    Public Overrides Function GetHashCode() As Integer
        Return (Serial).GetHashCode()
    End Function

    ''' <summary>
    ''' Test for equality by testing whether these datasets have the same serial number
    ''' </summary>
    ''' <param name="aArg1">One dataset to compare to the other</param>
    ''' <param name="aArg2">The other dataset to compare to the one</param>
    ''' <returns>True if datasets have same serial number</returns>
    ''' <remarks>False even if values in datasets (other than serial number) match each other exactly</remarks>
    Public Shared Operator =(ByVal aArg1 As atcDataSet, ByVal aArg2 As atcDataSet) As Boolean
        If aArg1 Is Nothing Then
            Return aArg2 Is Nothing
        ElseIf aArg2 Is Nothing Then
            Return False
        Else
            Return aArg1.Serial = aArg2.Serial
        End If
    End Operator

    ''' <summary>Exact opposite of the operator = </summary>
    Public Shared Operator <>(ByVal aArg1 As atcDataSet, ByVal aArg2 As atcDataSet) As Boolean
        Return Not (aArg1 = aArg2)
    End Operator

    ''' <summary>String describing this DataSet</summary>
    Public Overrides Function ToString() As String
        Try
            Dim lNumHaveValue As Integer = 0
            Dim lLastAttribute As Integer = pStringAttributeNames.GetUpperBound(0)
            Dim lAttrValues(lLastAttribute) As String
            For iArg As Integer = 0 To lLastAttribute
                lAttrValues(iArg) = pAttributes.GetFormattedValue(pStringAttributeNames(iArg), "")
                If lAttrValues(iArg) = "<unk>" Then lAttrValues(iArg) = ""
                If lAttrValues(iArg).Length > 0 Then lNumHaveValue += 1
            Next
            If lNumHaveValue > 0 Then
                Return String.Format(pStringFormat, lAttrValues)
            End If
        Catch ex As Exception
        End Try
        Return "# " & Serial
    End Function

    ''' <summary>Build a default format string with all arguments separated by 
    ''' spaces</summary>
    Public Shared Sub SetStringFormat(ByVal aAttributeNames() As String, _
                             Optional ByVal aFormat As String = "")

        If aFormat.Length = 0 Then 'Create a format containing all the named attributes
            For iArg As Integer = 0 To aAttributeNames.GetUpperBound(0)
                aFormat &= "{" & iArg & "} "
            Next
            aFormat = RTrim(aFormat) 'remove trailing space
        End If
        pStringFormat = aFormat
        pStringAttributeNames = aAttributeNames
    End Sub

End Class
