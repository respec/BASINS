Imports atcData
Imports atcUtility
Imports MapWinUtility

Public Class atcTimeseriesBaseflow
    Inherits atcData.atcTimeseriesSource
    Private pAvailableOperations As atcDataAttributes
    Private Const pName As String = "Timeseries::Baseflow"

    Private Shared BFModel As atcAttributeDefinition

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
            Return "Calculate Baseflow"
        End Get
    End Property

    'Opening creates new computed data rather than opening a file
    Public Overrides ReadOnly Property CanOpen() As Boolean
        Get
            Return True
        End Get
    End Property

    'Definitions of the type of baseflow calculations supported by ComputeBaseflow
    Public Overrides ReadOnly Property AvailableOperations() As atcDataAttributes
        Get
            If pAvailableOperations Is Nothing Then
                pAvailableOperations = New atcDataAttributes

                Dim defBaseflowTS As New atcAttributeDefinition
                With defBaseflowTS
                    .Calculator = Me
                    .Category = Me.Category
                    .CopiesInherit = False
                    .Description = "Baseflow Timeseries"
                    .Editable = False
                    .Name = "BaseflowTimeseries"
                    .TypeString = "atcTimeseries"
                End With
                atcDataAttributes.AddDefinition(defBaseflowTS)

                Dim defBFModel As New atcAttributeDefinition
                With defBFModel
                    .Name = "Baseflow Model"
                    .Description = "Baseflow Model"
                    .DefaultValue = New String() {"HySEP-FIXED", "HySEP-SLIDE", "HySEP-LOCMIN", "PART"}
                    .Editable = True
                    .TypeString = "String"
                End With

                Dim defStations As New atcAttributeDefinition
                With defStations
                    .Name = "Station Information"
                    .Description = "Station Information File (default Station.txt)"
                    .DefaultValue = "Station.txt"
                    .Editable = True
                    .TypeString = "String"
                End With

                Dim defDrainageArea As New atcAttributeDefinition
                With defDrainageArea
                    .Name = "Drainage Area"
                    .Description = "Drainage Area (default unit sq mi)"
                    .DefaultValue = 0.0
                    .Editable = True
                    .TypeString = "Double"
                End With

                Dim defTimeSeriesDaily As New atcAttributeDefinition
                With defTimeSeriesDaily
                    .Name = "Timeseries"
                    .Description = "One daily time series"
                    .Editable = True
                    .TypeString = "atcTimeseries"
                End With

                Dim defUnit As New atcAttributeDefinition
                With defUnit
                    .Name = "UnitFlag"
                    .Description = "English (True) or Metric (False)"
                    .Editable = True
                    .TypeString = "Boolean"
                End With

                AddOperation("Baseflow", "Baseflow separation", _
                             "Double", defTimeSeriesDaily, defBFModel, defUnit, defDrainageArea, defStations)
            End If
            Return pAvailableOperations
        End Get
    End Property

    Private Sub AddOperation(ByVal aName As String, _
                                  ByVal aDescription As String, _
                                  ByVal aTypeString As String, _
                                  ByVal ParamArray aArgs() As atcAttributeDefinition)
        Dim lResult As New atcAttributeDefinition
        With lResult
            .Name = aName
            .Description = aDescription
            .DefaultValue = Nothing
            .Editable = False
            .TypeString = aTypeString
            .Calculator = Me
            .Category = "N-day and Frequency"
        End With
        Dim lArguments As atcDataAttributes = New atcDataAttributes
        For Each lArg As atcAttributeDefinition In aArgs
            lArguments.SetValue(lArg, Nothing)
        Next
        pAvailableOperations.SetValue(lResult, Nothing, lArguments)

    End Sub

    'first element of aArgs is atcData object whose attribute(s) will be set to the result(s) of calculation(s)
    'remaining aArgs are expected to follow the args required for the specified operation
    Public Overrides Function Open(ByVal aOperationName As String, Optional ByVal aArgs As atcDataAttributes = Nothing) As Boolean
        'Dim ltsGroup As atcTimeseriesGroup = Nothing
        'Dim lTs As atcTimeseries
        'Dim lTsB As atcTimeseries
        'Dim lLogFlg As Boolean = True
        'Dim lOperationName As String = aOperationName.ToLower
        'Dim lNDay As Object = 1
        'Dim lReturn As Object = 0
        'Dim lHigh As Boolean = True
        'Dim lHighLowWord As String = "high"
        'Dim lBoundaryMonth As Integer = 10
        'Dim lBoundaryDay As Integer = 1
        'Dim lEndMonth As Integer = 0
        'Dim lEndDay As Integer = 0
        'Dim lFirstYear As Integer = 0
        'Dim lLastYear As Integer = 0
        'Dim lAttributeDef As atcAttributeDefinition = Nothing

        'Select Case lOperationName
        '    Case "7q10"
        '        lNDay = 7
        '        lReturn = 10
        '        lOperationName = "n-day low value"
        '        'lLogFlg = False
        '    Case "1hi100", "1high100"
        '        lNDay = 1
        '        lReturn = 100
        '        lOperationName = "n-day high value"
        '    Case Else
        '        lNDay = StrFirstInt(lOperationName)
        '        Dim lPos As Integer = lOperationName.Length
        '        While IsNumeric(Mid(lOperationName, lPos, 1))
        '            lPos -= 1
        '        End While
        '        If lPos < lOperationName.Length Then
        '            lReturn = lOperationName.Substring(lPos)
        '        End If
        'End Select

        ''Change default from True to False if operation name contains "low"
        'If lOperationName.Contains("low") Then
        '    lHigh = False
        '    lHighLowWord = "low"
        'End If

        'If lNDay > 0 AndAlso lReturn > 0 Then
        '    lOperationName = "n-day " & lHighLowWord & " value"
        'ElseIf lNDay > 0 Then 'See if we can find an attribute name instead of a return period
        '    Dim lAttStart As Integer = -1
        '    If lHigh Then
        '        lAttStart = lOperationName.IndexOf("high")
        '        If lAttStart >= 0 Then
        '            lAttStart += 4
        '        Else
        '            lAttStart = lOperationName.IndexOf("hi")
        '            If lAttStart >= 0 Then lAttStart += 2
        '        End If
        '    Else
        '        lAttStart = lOperationName.IndexOf("low")
        '        If lAttStart >= 0 Then lAttStart += 3
        '    End If

        '    If lAttStart >= 0 Then
        '        lAttributeDef = atcData.atcDataAttributes.GetDefinition(lOperationName.Substring(lAttStart), False)
        '        If lAttributeDef IsNot Nothing Then
        '            lOperationName = "n-day " & lHighLowWord & " attribute"
        '        End If
        '    End If
        'End If

        'If aArgs Is Nothing Then
        '    ltsGroup = atcDataManager.UserSelectData("Select data to compute statistics for")
        '    Dim lForm As New frmSpecifyYearsSeasons
        '    If lHigh Then
        '        lBoundaryMonth = 10
        '        If Not lForm.AskUser("High", ltsGroup, lBoundaryMonth, lBoundaryDay, lEndMonth, lEndDay, lFirstYear, lLastYear, lNDay) Then
        '            Return False
        '        End If
        '    Else
        '        lBoundaryMonth = 4
        '        If Not lForm.AskUser("Low", ltsGroup, lBoundaryMonth, lBoundaryDay, lEndMonth, lEndDay, lFirstYear, lLastYear, lNDay) Then
        '            Return False
        '        End If
        '    End If
        'Else
        '    ltsGroup = DatasetOrGroupToGroup(aArgs.GetValue("Timeseries"))
        '    lLogFlg = aArgs.GetValue("LogFlg", lLogFlg)
        '    lNDay = aArgs.GetValue("NDay", lNDay)
        '    lReturn = aArgs.GetValue("Return Period", lReturn)
        '    lHigh = aArgs.GetValue("HighFlag", lHigh)
        '    If aArgs.ContainsAttribute("BoundaryMonth") Then
        '        lBoundaryMonth = aArgs.GetValue("BoundaryMonth")
        '    ElseIf lHigh Then
        '        lBoundaryMonth = 10
        '    Else
        '        lBoundaryMonth = 4
        '    End If
        '    lBoundaryDay = aArgs.GetValue("BoundaryDay", lBoundaryDay)

        '    If aArgs.ContainsAttribute("EndMonth") Then lEndMonth = aArgs.GetValue("EndMonth")
        '    If aArgs.ContainsAttribute("EndDay") Then lEndDay = aArgs.GetValue("EndDay")
        '    If aArgs.ContainsAttribute("FirstYear") Then lFirstYear = aArgs.GetValue("FirstYear")
        '    If aArgs.ContainsAttribute("LastYear") Then lLastYear = aArgs.GetValue("LastYear")
        '    If aArgs.ContainsAttribute("Attribute") Then lAttributeDef = atcData.atcDataAttributes.GetDefinition(aArgs.GetValue("Attribute"), False)
        'End If

        'If ltsGroup Is Nothing Then
        '    ltsGroup = atcDataManager.UserSelectData("Select data to compute statistics for")
        'End If

        'For Each lTs In ltsGroup
        '    Dim lNDayTsGroup As atcTimeseriesGroup = Nothing 'atcTimeseries
        '    If lTs.Attributes.GetValue("Time Unit") = atcTimeUnit.TUYear Then
        '        lTsB = lTs
        '    Else
        '        lTsB = SubsetByDateBoundary(lTs, lBoundaryMonth, lBoundaryDay, Nothing, lFirstYear, lLastYear, lEndMonth, lEndDay)
        '    End If
        '    Select Case lOperationName
        '        Case "n-day low value", "n-day high value"
        '            ComputeFreq(lTsB, lNDay, lHigh, lReturn, lLogFlg, lTs.Attributes, lNDayTsGroup, lEndMonth, lEndDay)
        '            Me.DataSets.AddRange(lNDayTsGroup)
        '        Case "n-day low timeseries", "n-day high timeseries"
        '            lNDayTsGroup = HighOrLowTimeseries(lTsB, lNDay, lHigh, lTs.Attributes, lEndMonth, lEndDay)
        '            Me.DataSets.AddRange(lNDayTsGroup)
        '        Case "n-day low attribute", "n-day high attribute"
        '            If lAttributeDef IsNot Nothing Then
        '                lNDayTsGroup = HighOrLowTimeseries(lTsB, lNDay, lHigh, lTs.Attributes, lEndMonth, lEndDay)
        '                lTs.Attributes.SetValue(CInt(lNDay) & lHighLowWord & lAttributeDef.Name, lNDayTsGroup(0).Attributes.GetValue(lAttributeDef.Name))
        '            End If
        '        Case "kendall tau"
        '            ComputeTau(lTsB, lNDay, lHigh, lTs.Attributes, lEndMonth, lEndDay)
        '    End Select
        'Next

        If Me.DataSets.Count > 0 Then
            Return True 'todo: error checks
        Else
            Return False 'no datasets added, not a data source
        End If
    End Function

    <CLSCompliant(False)> _
    Public Overrides Sub Initialize(ByVal aMapWin As MapWindow.Interfaces.IMapWin, ByVal aParentHandle As Integer)
        MyBase.Initialize(aMapWin, aParentHandle)
        For Each lOperation As atcDefinedValue In AvailableOperations
            atcDataAttributes.AddDefinition(lOperation.Definition)
        Next
    End Sub
End Class
