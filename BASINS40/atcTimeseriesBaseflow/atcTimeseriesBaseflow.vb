Imports atcData
Imports atcUtility
Imports MapWinUtility

Public Class atcTimeseriesBaseflow
    Inherits atcData.atcTimeseriesSource
    Private pAvailableOperations As atcDataAttributes
    Private Const pName As String = "Timeseries::Baseflow"

    Private Shared BFModel As atcAttributeDefinition
    Private Shared ClsBaseFlow As clsBaseflow

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

                'Dim defBaseflowTS As New atcAttributeDefinition
                'With defBaseflowTS
                '    .Calculator = Me
                '    .Category = Me.Category
                '    .CopiesInherit = False
                '    .Description = "Baseflow Timeseries"
                '    .Editable = False
                '    .Name = "Baseflow"
                '    .TypeString = "atcTimeseriesGroup"
                'End With
                'atcDataAttributes.AddDefinition(defBaseflowTS)

                Dim defBFModel As New atcAttributeDefinition
                With defBFModel
                    .Name = "Baseflow Separation Method"
                    .Description = "Method"
                    .DefaultValue = New String() {"HySEP-FIXED", "HySEP-SLIDE", "HySEP-LOCMIN", "PART"}
                    .Editable = True
                    .TypeString = "String"
                End With

                Dim defStations As New atcAttributeDefinition
                With defStations
                    .Name = "Station File"
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
                    .Name = "Streamflow"
                    .Description = "One daily time series"
                    .Editable = True
                    .TypeString = "atcTimeseriesGroup"
                End With

                Dim defUnit As New atcAttributeDefinition
                With defUnit
                    .Name = "EnglishUnit"
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
            .Category = "Baseflow"
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
        Dim lEnglishFlg As Boolean = True
        Dim lOperationName As String = aOperationName.ToLower
        Dim lBoundaryMonth As Integer = 10
        Dim lBoundaryDay As Integer = 1
        Dim lEndMonth As Integer = 0
        Dim lEndDay As Integer = 0
        Dim lFirstYear As Integer = 0
        Dim lLastYear As Integer = 0

        Dim lTsStreamflow As atcTimeseries = Nothing
        Dim lStartDate As Double = 0
        Dim lEndDate As Double = 0
        Dim lMethod As String = ""
        Dim lMethods As ArrayList = Nothing
        Dim lDrainageArea As Double = 0
        Dim lBFINDay As Integer = 0
        Dim lBFIFrac As Double
        Dim lBFIK1Day As Double
        Dim lBFIUseSymbol As Boolean = False
        Dim lStationFile As String = ""

        Dim lAttributeDef As atcAttributeDefinition = Nothing

        Select Case lOperationName
            Case "baseflow"
            Case Else
        End Select

        If aArgs Is Nothing Then
            'ltsGroup = atcDataManager.UserSelectData("Select data to compute statistics for")
        Else
            'ltsGroup = DatasetOrGroupToGroup(aArgs.GetValue("Timeseries"))
            lTsStreamflow = aArgs.GetValue("Streamflow")(0)
            lEnglishFlg = aArgs.GetValue("EnglishUnit", lEnglishFlg)
            lStartDate = aArgs.GetValue("Start Date")
            lEndDate = aArgs.GetValue("End Date")
            lMethod = aArgs.GetValue("Method")
            lMethods = aArgs.GetValue("Methods")
            lDrainageArea = aArgs.GetValue("Drainage Area")
            'If lMethods.Contains(BFMethods.HySEPFixed) OrElse _
            '   lMethods.Contains(BFMethods.HySEPLocMin) OrElse _
            '   lMethods.Contains(BFMethods.HySEPSlide) OrElse _
            '   lMethods.Contains(BFMethods.PART) Then

            'End If
            Dim lBFIChosen As Boolean = False
            If lMethods.Contains(BFMethods.BFIStandard) Then
                lBFIFrac = aArgs.GetValue("BFIFrac")
                lBFIChosen = True
            End If
            If lMethods.Contains(BFMethods.BFIModified) Then
                lBFIK1Day = aArgs.GetValue("BFIK1Day")
                lBFIChosen = True
            End If
            If lBFIChosen Then
                lBFINDay = aArgs.GetValue("BFINDay")
                lBFIUseSymbol = aArgs.GetValue("BFIUseSymbol")
            End If

            lStationFile = aArgs.GetValue("Station File")

            'If aArgs.ContainsAttribute("BoundaryMonth") Then
            '    lBoundaryMonth = aArgs.GetValue("BoundaryMonth")
            'Else
            '    lBoundaryMonth = 4
            'End If
            'lBoundaryDay = aArgs.GetValue("BoundaryDay", lBoundaryDay)
            'If aArgs.ContainsAttribute("EndMonth") Then lEndMonth = aArgs.GetValue("EndMonth")
            'If aArgs.ContainsAttribute("EndDay") Then lEndDay = aArgs.GetValue("EndDay")
            'If aArgs.ContainsAttribute("FirstYear") Then lFirstYear = aArgs.GetValue("FirstYear")
            'If aArgs.ContainsAttribute("LastYear") Then lLastYear = aArgs.GetValue("LastYear")
            'If aArgs.ContainsAttribute("Attribute") Then lAttributeDef = atcData.atcDataAttributes.GetDefinition(aArgs.GetValue("Attribute"), False)
        End If

        'If ltsGroup Is Nothing Then
        '    ltsGroup = atcDataManager.UserSelectData("Select data to compute statistics for")
        'End If
        If lMethods Is Nothing OrElse lMethods.Count = 0 Then
            Return False
        End If

        Dim lBFDatagroup As atcTimeseriesGroup = Nothing
        Dim lBFDataGroupFinal As New atcTimeseriesGroup

        For Each lMethod In lMethods
            Select Case lMethod
                Case BFMethods.HySEPFixed
                    ClsBaseFlow = New clsBaseflowHySep()
                    CType(ClsBaseFlow, clsBaseflowHySep).Method = BFMethods.HySEPFixed
                Case BFMethods.HySEPLocMin
                    ClsBaseFlow = New clsBaseflowHySep()
                    CType(ClsBaseFlow, clsBaseflowHySep).Method = BFMethods.HySEPLocMin
                Case BFMethods.HySEPSlide
                    ClsBaseFlow = New clsBaseflowHySep()
                    CType(ClsBaseFlow, clsBaseflowHySep).Method = BFMethods.HySEPSlide
                Case BFMethods.PART
                    ClsBaseFlow = New clsBaseflowPart()
                Case BFMethods.BFIStandard
                    ClsBaseFlow = New clsBaseflowBFI()
                    CType(ClsBaseFlow, clsBaseflowBFI).Method = BFMethods.BFIStandard
                Case BFMethods.BFIModified
                    ClsBaseFlow = New clsBaseflowBFI()
                    CType(ClsBaseFlow, clsBaseflowBFI).Method = BFMethods.BFIModified
                Case Else
            End Select
            With ClsBaseFlow

                If lMethod = BFMethods.BFIStandard Or lMethod = BFMethods.BFIModified Then
                    CType(ClsBaseFlow, clsBaseflowBFI).PartitionLengthInDays = lBFINDay
                    CType(ClsBaseFlow, clsBaseflowBFI).UseSymbols = lBFIUseSymbol
                    If lMethod = BFMethods.BFIStandard Then
                        CType(ClsBaseFlow, clsBaseflowBFI).TPTestFraction = lBFIFrac
                    ElseIf lMethod = BFMethods.BFIModified Then
                        CType(ClsBaseFlow, clsBaseflowBFI).OneDayRecessConstant = lBFIK1Day
                    End If
                End If

                'even though BFI doesn't need it, but set it nonetheless, won't hurt
                'later on reporting and graphing need it too
                .DrainageArea = lDrainageArea
                .TargetTS = lTsStreamflow
                .StartDate = lStartDate
                .EndDate = lEndDate
                If lEnglishFlg Then
                    .UnitFlag = 1
                Else
                    .UnitFlag = 2
                End If
            End With
            lBFDatagroup = ClsBaseFlow.DoBaseFlowSeparation()
            If lBFDatagroup IsNot Nothing AndAlso lBFDatagroup.Count > 0 Then
                lBFDataGroupFinal.AddRange(lBFDatagroup)
            End If
        Next

        'If Me.DataSets.Count > 0 Then
        If lBFDataGroupFinal IsNot Nothing AndAlso lBFDataGroupFinal.Count > 0 Then
            Dim lNewDef As atcAttributeDefinition
            Dim lIndex As Integer = atcDataAttributes.AllDefinitions.Keys.IndexOf("Baseflow")
            If lIndex >= 0 Then
                lNewDef = atcDataAttributes.AllDefinitions.ItemByIndex(lIndex)
            Else
                lNewDef = New atcAttributeDefinition
                With lNewDef
                    .Name = "Baseflow"
                    .Description = "Baseflow Related Timeseries"
                    .DefaultValue = ""
                    .Editable = False
                    .TypeString = "atcTimeseriesGroup"
                    .Calculator = Me
                    .Category = "Baseflow"
                    .CopiesInherit = False
                End With
            End If
            lTsStreamflow.Attributes.SetValue(lNewDef, lBFDataGroupFinal, Nothing)
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
