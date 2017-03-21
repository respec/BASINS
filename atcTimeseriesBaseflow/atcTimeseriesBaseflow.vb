Imports atcData
Imports atcUtility
Imports MapWinUtility
Imports System.IO

Public Class BFInputNames
    Public Shared Streamflow As String = "Streamflow"
    Public Shared EnglishUnit As String = "EnglishUnit"
    Public Shared StartDate As String = "StartDate"
    Public Shared EndDate As String = "EndDate"
    Public Shared BFMethods As String = "BFMethods"
    Public Shared DrainageArea As String = "DrainageArea"
    Public Shared BFITurnPtFrac As String = "BFI_TurnPtFrac"
    Public Shared BFIRecessConst As String = "BFI_RecessConst"
    Public Shared BFINDayScreen As String = "BFI_NDayScreen"
    Public Shared BFIUseSymbol As String = "BFI_UseSymbol"
    Public Shared BFIReportby As String = "BFI_Reportby"
    Public Shared BFIReportbyCY As String = "Calendar"
    Public Shared BFIReportbyWY As String = "Water"
    Public Shared BFLOWFilter As String = "BFLOWFilter"
    Public Shared TwoPRDFRC As String = "TwoPRDF_RC"
    Public Shared TwoPRDFBFImax As String = "TwoPRDF_BFImax"
    Public Shared TwoParamEstMethod As String = "TwoParamEstimationMethod"
    Public Shared StationFile As String = "StationFile"
End Class

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
            'Return "Generate Timeseries"
            Return "File"
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

    Private pBF_Message As String = ""
    Public ReadOnly Property BF_Message() As String
        Get
            Return pBF_Message
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

    '{
    ''' <summary>
    ''' Read base-flow analysis output persisted in the form of .csv file, namely, the *_fullspan_Daily.csv
    ''' </summary>
    ''' <param name="aFileName">the name of the CSV file</param>
    ''' <param name="aAttributes">additional conditions</param>
    ''' <returns>True: success; False: failure</returns>
    Public Overrides Function Open(ByVal aFileName As String,
                          Optional ByVal aAttributes As atcData.atcDataAttributes = Nothing) As Boolean
        If MyBase.Open(aFileName, aAttributes) Then
            If Not IO.File.Exists(Specification) Then
                Logger.Dbg("Opening new file " & Specification)
                Return True
            ElseIf IO.Path.GetFileName(Specification).ToLower.StartsWith("nwis_stations") Then
                Throw New ApplicationException("Station file does not contain timeseries data: " & IO.Path.GetFileName(Specification))
            Else
                Try
                    Dim lTimeStartOpen As Date = Now
                    Logger.Dbg("OpenStartFor " & Specification)

                    Dim lDataGroup As New atcTimeseriesGroup()
                    Dim location As String = ""
                    Dim lDA As Double = Double.NaN
                    Dim lMethods As New ArrayList()
                    Dim lDateStart As Double = Double.NaN
                    Dim lDateEnd As Double = Double.NaN
                    Dim lBFI_N As Integer = -1
                    Dim lBFI_TurnFac As Double = Double.NaN
                    Dim lBFI_K As Double = Double.NaN
                    Dim lDF1ParamAlpha As Double = Double.NaN
                    Dim lDF2ParamRC As Double = Double.NaN
                    Dim lDF2ParamBFImax As Double = Double.NaN
                    Dim lMethod As BFMethods = Nothing

                    Dim lFirstLine As Boolean = True
                    Dim lAttributes As New atcDataAttributes()
                    lAttributes.AddHistory("Read from " & Specification)

                    Dim lArr() As String
                    Dim lContinueToData As Boolean = False
                    Dim lHeaderLines As Integer = 0
                    Dim lSR As New StreamReader(Specification)
                    Dim lCurLine As String = lSR.ReadLine()
                    lHeaderLines += 1
                    Dim lDays As Integer = 0
                    While Not lSR.EndOfStream
                        If lCurLine.StartsWith("Station:") Then
                            lArr = lCurLine.Split(" ")
                            If IsNumeric(lArr(1)) Then
                                lAttributes.SetValue("Location", lArr(1))
                            End If
                            Dim lStnName As String = lCurLine.Substring(("Station: " & lArr(1)).Length + 1)
                            If Not String.IsNullOrEmpty(lStnName) Then
                                lAttributes.SetValue("StaNam", lStnName)
                            End If
                        ElseIf lCurLine.Contains("Drainage") Then
                            lArr = lCurLine.Split(":")
                            If lArr.Length > 1 AndAlso Not String.IsNullOrEmpty(lArr(1).Trim()) Then
                                Dim lArr1() As String = lArr(1).Trim().Split(" ")
                                lAttributes.SetValue("Drainage Area", lArr1(0))
                            End If
                        ElseIf lCurLine.Contains("BFI:") Then
                            lArr = lCurLine.Split(";")
                            If lArr.Length > 0 Then
                                lAttributes.SetValue("BFI_N", lArr(0).Substring(lArr(0).LastIndexOf(":") + 1))
                            End If
                            If lArr.Length > 1 Then
                                lAttributes.SetValue("BFI_TurnPtFac", lArr(1).Substring(lArr(1).LastIndexOf(":") + 1))
                            End If
                            If lArr.Length > 2 Then
                                lAttributes.SetValue("BFI_K", lArr(2).Substring(lArr(2).LastIndexOf(":") + 1))
                            End If
                        ElseIf lCurLine.Contains("DF1Param:") Then
                            lAttributes.SetValue("DF1Param_Alpha", lCurLine.Substring(lCurLine.LastIndexOf(":") + 1))
                        ElseIf lCurLine.Contains("DF2Param:") Then
                            lArr = lCurLine.Split(";")
                            lAttributes.SetValue("DF2Param_RC", lArr(0).Substring(lArr(0).LastIndexOf(":") + 1))
                            lAttributes.SetValue("DF2Param_BFImax", lArr(1).Substring(lArr(1).LastIndexOf(":") + 1))
                        ElseIf lCurLine.Contains("BFIStandard") OrElse lCurLine.Contains("BFIModified") OrElse
                               lCurLine.Contains("BFLOW") OrElse lCurLine.Contains("TwoPRDF") OrElse
                               lCurLine.Contains("PART") OrElse lCurLine.Contains("HySEP-") Then
                            lArr = lCurLine.Split(",")
                            For I As Integer = 0 To lArr.Length - 1
                                If Not String.IsNullOrEmpty(lArr(I)) Then
                                    If lArr(I).StartsWith("HySEP") Then
                                        If lArr(I).Contains("Fix") Then
                                            lMethod = BFMethods.HySEPFixed
                                            lAttributes.SetValue("Index_BF_" & BFMethods.HySEPFixed.ToString(), I - 2)
                                            lAttributes.SetValue("Index_RO_" & BFMethods.HySEPFixed.ToString(), I)
                                            lAttributes.SetValue("Index_BFPCT_" & BFMethods.HySEPFixed.ToString(), I + 2)
                                        ElseIf lArr(I).Contains("Min") Then
                                            lMethod = BFMethods.HySEPLocMin
                                            lAttributes.SetValue("Index_BF_" & BFMethods.HySEPLocMin.ToString(), I - 2)
                                            lAttributes.SetValue("Index_RO_" & BFMethods.HySEPLocMin.ToString(), I)
                                            lAttributes.SetValue("Index_BFPCT_" & BFMethods.HySEPLocMin.ToString(), I + 2)
                                        ElseIf lArr(I).Contains("Slide") Then
                                            lMethod = BFMethods.HySEPSlide
                                            lAttributes.SetValue("Index_BF_" & BFMethods.HySEPSlide.ToString(), I - 2)
                                            lAttributes.SetValue("Index_RO_" & BFMethods.HySEPSlide.ToString(), I)
                                            lAttributes.SetValue("Index_BFPCT_" & BFMethods.HySEPSlide.ToString(), I + 2)
                                        End If
                                    ElseIf lArr(I).StartsWith("PART") Then
                                        lMethod = BFMethods.PART
                                        lAttributes.SetValue("Index_BF_" & BFMethods.PART.ToString(), I - 2)
                                        lAttributes.SetValue("Index_RO_" & BFMethods.PART.ToString(), I)
                                        lAttributes.SetValue("Index_BFPCT_" & BFMethods.PART.ToString(), I + 2)
                                    ElseIf lArr(I).StartsWith("BFLOW") Then
                                        lMethod = BFMethods.BFLOW
                                        lAttributes.SetValue("Index_BF_" & BFMethods.BFLOW.ToString(), I - 2)
                                        lAttributes.SetValue("Index_RO_" & BFMethods.BFLOW.ToString(), I)
                                        lAttributes.SetValue("Index_BFPCT_" & BFMethods.BFLOW.ToString(), I + 2)
                                    ElseIf lArr(I).StartsWith("Two") Then
                                        lMethod = BFMethods.TwoPRDF
                                        lAttributes.SetValue("Index_BF_" & BFMethods.TwoPRDF.ToString(), I - 2)
                                        lAttributes.SetValue("Index_RO_" & BFMethods.TwoPRDF.ToString(), I)
                                        lAttributes.SetValue("Index_BFPCT_" & BFMethods.TwoPRDF.ToString(), I + 2)
                                    ElseIf lArr(I).StartsWith("BFIStandard") Then
                                        lMethod = BFMethods.BFIStandard
                                        lAttributes.SetValue("Index_BF_" & BFMethods.BFIStandard.ToString(), I - 2)
                                        lAttributes.SetValue("Index_RO_" & BFMethods.BFIStandard.ToString(), I)
                                        lAttributes.SetValue("Index_BFPCT_" & BFMethods.BFIStandard.ToString(), I + 2)
                                    ElseIf lArr(I).StartsWith("BFIModified") Then
                                        lMethod = BFMethods.BFIModified
                                        lAttributes.SetValue("Index_BF_" & BFMethods.BFIModified.ToString(), I - 2)
                                        lAttributes.SetValue("Index_RO_" & BFMethods.BFIModified.ToString(), I)
                                        lAttributes.SetValue("Index_BFPCT_" & BFMethods.BFIModified.ToString(), I + 2)
                                    End If
                                    If Not lMethods.Contains(lMethod) Then
                                        lMethods.Add(lMethod)
                                    End If
                                End If
                            Next
                        ElseIf lCurLine.Contains("Day") AndAlso lCurLine.Contains("Date") Then
                            While Not lSR.EndOfStream
                                lCurLine = lSR.ReadLine()
                                lHeaderLines += 1
                                If Not String.IsNullOrEmpty(lCurLine) Then
                                    If lCurLine.StartsWith("1") Then
                                        lHeaderLines -= 1
                                        'Exit While
                                        'Start count days
                                        lDays += 1
                                        lArr = lCurLine.Split(",")
                                        If lArr.Length > 1 AndAlso Not String.IsNullOrEmpty(lArr(1)) Then
                                            Dim lDateTime As New DateTime()
                                            If DateTime.TryParse(lArr(1), lDateTime) Then
                                                lDateStart = Date2J(lDateTime.Year, lDateTime.Month, lDateTime.Day, 0, 0, 0)
                                            End If
                                        End If
                                        While Not lSR.EndOfStream
                                            lCurLine = lSR.ReadLine()
                                            If Not String.IsNullOrEmpty(lCurLine) AndAlso IsNumeric(lCurLine.Substring(0, lCurLine.IndexOf(","))) Then
                                                lDays += 1
                                            End If
                                        End While
                                        Exit While
                                    End If
                                End If
                            End While
                            If lMethods.Count > 0 Then
                                lContinueToData = True
                            End If
                            Exit While
                        End If
                        lCurLine = lSR.ReadLine()
                        lHeaderLines += 1
                    End While

                    If lContinueToData Then
                        'Now read data
                        Dim lTsAttributes As atcDataAttributes = lAttributes.Copy()
                        Dim lRmDefs As New ArrayList()
                        For Each lDef As atcDefinedValue In lTsAttributes
                            If lDef.Definition.Name.StartsWith("Index") Then
                                lRmDefs.Add(lDef)
                            ElseIf lDef.Definition.Name.StartsWith("DF") Then
                                lRmDefs.Add(lDef)
                            ElseIf lDef.Definition.Name.StartsWith("BFI") Then
                                lRmDefs.Add(lDef)
                            End If
                        Next
                        For Each lDef As atcDefinedValue In lRmDefs
                            lTsAttributes.Remove(lDef)
                        Next
                        lDateEnd = lDateStart + JulianHour * 24 * lDays
                        For Each lMethod In lMethods
                            Dim lTsRO As atcTimeseries = NewTimeseries(lDateStart, lDateEnd, atcTimeUnit.TUDay, 1, Me, Double.NaN)
                            Dim lTsBF As atcTimeseries = NewTimeseries(lDateStart, lDateEnd, atcTimeUnit.TUDay, 1, Me, Double.NaN)
                            Dim lTsBFPCT As atcTimeseries = NewTimeseries(lDateStart, lDateEnd, atcTimeUnit.TUDay, 1, Me, Double.NaN)
                            With lTsBF.Attributes
                                .ChangeTo(lTsAttributes)
                                If lMethod = BFMethods.BFIStandard Then
                                    .SetValue("BFI_N", lAttributes.GetValue("BFI_N"))
                                    .SetValue("BFI_TurnPtFac", lAttributes.GetValue("BFI_TurnPtFac"))
                                ElseIf lMethod = BFMethods.BFIModified Then
                                    .SetValue("BFI_N", lAttributes.GetValue("BFI_N"))
                                    .SetValue("BFI_K", lAttributes.GetValue("BFI_K"))
                                ElseIf lMethod = BFMethods.BFLOW Then
                                    .SetValue("DF1Param_Alpha", lAttributes.GetValue("DF1Param_Alpha"))
                                ElseIf lMethod = BFMethods.TwoPRDF Then
                                    .SetValue("DF2Param_RC", lAttributes.GetValue("DF2Param_RC"))
                                    .SetValue("DF2Param_BFImax", lAttributes.GetValue("DF2Param_BFImax"))
                                End If
                                .SetValue("Constituent", "BF_" & lMethod.ToString())
                                .SetValue("Description", "baseflow cfs")
                                .SetValue("Units", "cubic feet per second")
                            End With
                            With lTsRO.Attributes
                                .ChangeTo(lTsAttributes)
                                If lMethod = BFMethods.BFIStandard Then
                                    .SetValue("BFI_N", lAttributes.GetValue("BFI_N"))
                                    .SetValue("BFI_TurnPtFac", lAttributes.GetValue("BFI_TurnPtFac"))
                                ElseIf lMethod = BFMethods.BFIModified Then
                                    .SetValue("BFI_N", lAttributes.GetValue("BFI_N"))
                                    .SetValue("BFI_K", lAttributes.GetValue("BFI_K"))
                                ElseIf lMethod = BFMethods.BFLOW Then
                                    .SetValue("DF1Param_Alpha", lAttributes.GetValue("DF1Param_Alpha"))
                                ElseIf lMethod = BFMethods.TwoPRDF Then
                                    .SetValue("DF2Param_RC", lAttributes.GetValue("DF2Param_RC"))
                                    .SetValue("DF2Param_BFImax", lAttributes.GetValue("DF2Param_BFImax"))
                                End If
                                .SetValue("Constituent", "RO_" & lMethod.ToString())
                                .SetValue("Description", "runoff cfs")
                                .SetValue("Units", "cubic feet per second")
                            End With
                            With lTsBFPCT.Attributes
                                .ChangeTo(lTsAttributes)
                                If lMethod = BFMethods.BFIStandard Then
                                    .SetValue("BFI_N", lAttributes.GetValue("BFI_N"))
                                    .SetValue("BFI_TurnPtFac", lAttributes.GetValue("BFI_TurnPtFac"))
                                ElseIf lMethod = BFMethods.BFIModified Then
                                    .SetValue("BFI_N", lAttributes.GetValue("BFI_N"))
                                    .SetValue("BFI_K", lAttributes.GetValue("BFI_K"))
                                ElseIf lMethod = BFMethods.BFLOW Then
                                    .SetValue("DF1Param_Alpha", lAttributes.GetValue("DF1Param_Alpha"))
                                ElseIf lMethod = BFMethods.TwoPRDF Then
                                    .SetValue("DF2Param_RC", lAttributes.GetValue("DF2Param_RC"))
                                    .SetValue("DF2Param_BFImax", lAttributes.GetValue("DF2Param_BFImax"))
                                End If
                                .SetValue("Constituent", "BFPCT_" & lMethod.ToString())
                                .SetValue("Description", "baseflow%")
                                .SetValue("Units", "percent")
                            End With
                            lDataGroup.Add("BF_" & lMethod.ToString(), lTsBF)
                            lDataGroup.Add("RO_" & lMethod.ToString(), lTsRO)
                            lDataGroup.Add("BFPCT_" & lMethod.ToString(), lTsBFPCT)
                        Next
                        lSR.BaseStream.Seek(0, SeekOrigin.Begin)
                        For I As Integer = 1 To lHeaderLines
                            lCurLine = lSR.ReadLine()
                        Next
                        Dim lDayCtr As Integer = 1
                        Dim lVal As Double
                        While Not lSR.EndOfStream
                            lCurLine = lSR.ReadLine()
                            If String.IsNullOrEmpty(lCurLine) Then
                                Exit While
                            End If
                            lArr = lCurLine.Split(",")
                            If String.IsNullOrEmpty(lArr(0)) Then
                                Exit While
                            End If
                            For Each lMethod In lMethods
                                If Double.TryParse(lArr(lAttributes.GetValue("Index_BF_" & lMethod.ToString())), lVal) Then
                                Else
                                    lVal = Double.NaN
                                End If
                                lDataGroup.ItemByKey("BF_" & lMethod.ToString()).Value(lDayCtr) = lVal
                                If Double.TryParse(lArr(lAttributes.GetValue("Index_RO_" & lMethod.ToString())), lVal) Then
                                Else
                                    lVal = Double.NaN
                                End If
                                lDataGroup.ItemByKey("RO_" & lMethod.ToString()).Value(lDayCtr) = lVal
                                If Double.TryParse(lArr(lAttributes.GetValue("Index_BFPCT_" & lMethod.ToString())), lVal) Then
                                Else
                                    lVal = Double.NaN
                                End If
                                lDataGroup.ItemByKey("BFPCT_" & lMethod.ToString()).Value(lDayCtr) = lVal
                            Next
                            lDayCtr += 1
                            If lDayCtr > lDays Then
                                Exit While
                            End If
                        End While
                    End If
                    Try
                        If lSR IsNot Nothing Then
                            lSR.DiscardBufferedData()
                            lSR.Close()
                            lSR = Nothing
                        End If
                    Catch
                    End Try
                    Dim lMissingVal As Double = -999
                    Dim lTSIndex As Integer = 0
                    For Each lData In lDataGroup
                        lTSIndex = lData.Attributes.GetValue("Count")
                        If lData.numValues <> lTSIndex Then
                            lData.numValues = lTSIndex
                        End If
                        lData.Attributes.RemoveByKey("DataKey")
                        'further processing could be done here, eg filling missing/gaps etc
                        DataSets.Add(lData)
                    Next
                    'lDataGroup.Clear()
                    Return True
                Catch lException As Exception
                    Throw New ApplicationException("Exception reading '" & Specification & "': " & lException.Message, lException)
                End Try
            End If
        End If
    End Function '}

    'first element of aArgs is atcData object whose attribute(s) will be set to the result(s) of calculation(s)
    'remaining aArgs are expected to follow the args required for the specified operation
    Public Function Calculate(ByVal aOperationName As String, Optional ByVal aArgs As atcDataAttributes = Nothing) As Boolean
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
        Dim lBFIYearBasis As String = ""
        Dim lStationFile As String = ""
        Dim lBatchRun As Boolean = False
        Dim lDFBeta As Double = Double.NaN
        Dim lDFRC As Double = Double.NaN
        Dim lDFBFImax As Double = Double.NaN
        Dim lTwoDFParamEstMethod As clsBaseflow2PRDF.ETWOPARAMESTIMATION = clsBaseflow2PRDF.ETWOPARAMESTIMATION.NONE

        Dim lAttributeDef As atcAttributeDefinition = Nothing

        Select Case lOperationName
            Case "baseflow"
            Case Else
        End Select

        If aArgs Is Nothing Then
            'ltsGroup = atcDataManager.UserSelectData("Select data to compute statistics for")
        Else
            'ltsGroup = DatasetOrGroupToGroup(aArgs.GetValue("Timeseries"))
            lTsStreamflow = aArgs.GetValue(BFInputNames.Streamflow)(0)
            Me.Specification = "Base-Flow-" & lTsStreamflow.Attributes.GetValue("Location")
            lEnglishFlg = aArgs.GetValue(BFInputNames.EnglishUnit, lEnglishFlg)
            lStartDate = aArgs.GetValue(BFInputNames.StartDate)
            lEndDate = aArgs.GetValue(BFInputNames.EndDate)
            lMethod = aArgs.GetValue("Method")
            lMethods = aArgs.GetValue(BFInputNames.BFMethods)
            lDrainageArea = aArgs.GetValue(BFInputNames.DrainageArea)
            'If lMethods.Contains(BFMethods.HySEPFixed) OrElse _
            '   lMethods.Contains(BFMethods.HySEPLocMin) OrElse _
            '   lMethods.Contains(BFMethods.HySEPSlide) OrElse _
            '   lMethods.Contains(BFMethods.PART) Then

            'End If
            Dim lBFIChosen As Boolean = False
            If lMethods.Contains(BFMethods.BFIStandard) Then
                lBFIFrac = aArgs.GetValue(BFInputNames.BFITurnPtFrac) '"BFIFrac"
                lBFIChosen = True
            End If
            If lMethods.Contains(BFMethods.BFIModified) Then
                lBFIK1Day = aArgs.GetValue(BFInputNames.BFIRecessConst) '"BFIK1Day"
                lBFIChosen = True
            End If
            If lBFIChosen Then
                lBFINDay = aArgs.GetValue(BFInputNames.BFINDayScreen) '"BFINDay"
                lBFIUseSymbol = aArgs.GetValue(BFInputNames.BFIUseSymbol) '"BFIUseSymbol"
                lBFIYearBasis = aArgs.GetValue(BFInputNames.BFIReportby) '"BFIReportby"
            End If
            If lMethods.Contains(BFMethods.BFLOW) Then
                lDFBeta = aArgs.GetValue(BFInputNames.BFLOWFilter, Double.NaN)
            End If
            If lMethods.Contains(BFMethods.TwoPRDF) Then
                lDFRC = aArgs.GetValue(BFInputNames.TwoPRDFRC, Double.NaN)
                lDFBFImax = aArgs.GetValue(BFInputNames.TwoPRDFBFImax, Double.NaN)
                lTwoDFParamEstMethod = aArgs.GetValue(BFInputNames.TwoParamEstMethod, clsBaseflow2PRDF.ETWOPARAMESTIMATION.NONE)
            End If

            lStationFile = aArgs.GetValue(BFInputNames.StationFile) '"Station File"
            lBatchRun = aArgs.GetValue("BatchRun", False)

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
                Case BFMethods.BFLOW
                    ClsBaseFlow = New clsBaseflowBFLOW()
                    'Below is for testing on using original time series including gaps
                    'ClsBaseFlow.TargetTS = aArgs.GetValue("OriginalFlow", Nothing)
                Case BFMethods.TwoPRDF
                    ClsBaseFlow = New clsBaseflow2PRDF()
                Case Else
            End Select
            With ClsBaseFlow
                .gBatchRun = lBatchRun
                If lMethod = BFMethods.BFIStandard Or lMethod = BFMethods.BFIModified Then
                    CType(ClsBaseFlow, clsBaseflowBFI).PartitionLengthInDays = lBFINDay
                    CType(ClsBaseFlow, clsBaseflowBFI).UseSymbols = lBFIUseSymbol
                    CType(ClsBaseFlow, clsBaseflowBFI).YearBasis = lBFIYearBasis
                    If lMethod = BFMethods.BFIStandard Then
                        CType(ClsBaseFlow, clsBaseflowBFI).TPTestFraction = lBFIFrac
                    ElseIf lMethod = BFMethods.BFIModified Then
                        CType(ClsBaseFlow, clsBaseflowBFI).OneDayRecessConstant = lBFIK1Day
                    End If
                ElseIf lMethod = BFMethods.BFLOW AndAlso Not Double.IsNaN(lDFBeta) Then
                    CType(ClsBaseFlow, clsBaseflowBFLOW).FP1 = lDFBeta
                ElseIf lMethod = BFMethods.TwoPRDF Then
                    If Not Double.IsNaN(lDFRC) Then
                        CType(ClsBaseFlow, clsBaseflow2PRDF).RC = lDFRC
                    End If
                    If Not Double.IsNaN(lDFBFImax) Then
                        CType(ClsBaseFlow, clsBaseflow2PRDF).BFImax = lDFBFImax
                    End If
                    CType(ClsBaseFlow, clsBaseflow2PRDF).ParamEstimationMethod = lTwoDFParamEstMethod
                End If

                'even though BFI doesn't need it, but set it nonetheless, won't hurt
                'later on reporting and graphing need it too
                .DrainageArea = lDrainageArea
                If .TargetTS Is Nothing Then .TargetTS = lTsStreamflow
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
            pBF_Message &= ClsBaseFlow.gError & vbCrLf
        Next

        'If Me.DataSets.Count > 0 Then
        If lBFDataGroupFinal IsNot Nothing AndAlso lBFDataGroupFinal.Count > 0 Then
            Me.DataSets.AddRange(lBFDataGroupFinal)
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
            If ClsBaseFlow IsNot Nothing AndAlso ClsBaseFlow.gError <> "" Then
                Logger.Msg(ClsBaseFlow.gError)
                ClsBaseFlow.gError = ""
            End If
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
