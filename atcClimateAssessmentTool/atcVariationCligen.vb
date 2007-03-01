Imports atcCligen
Imports atcData
Imports atcUtility
Imports MapWinUtility

Public Class VariationCligen
    Inherits atcVariation

    Public Shared CligenConstituents As String() = {"HPCP", "EVAP", "ATMP", "WIND", "DEWP", "SOLR", "CLDC"}

    Public BaseParmFileName As String = "<click to select>"
    Public ParmToVary As String = "<click to select>"
    Public StartYear As Integer = 1985
    Public NumYears As Integer = 4

    Public Sub New()
        DataSets = New atcDataGroup
        For Each lConstituent As String In CligenConstituents
            DataSets.Add(lConstituent, Nothing)
        Next
    End Sub

    Protected Overrides Function VaryData() As atcData.atcDataGroup
        Dim lHeader As String = Nothing
        Dim lFooter As String = Nothing
        Dim lTable As atcTableFixed = Nothing
        Dim lArgs As New atcDataAttributes
        Dim lCliGen As New atcCligen.atcCligen
        If ReadParmFile(BaseParmFileName, lHeader, lTable, lFooter) Then
            Dim lTempParmFileName As String = System.IO.Path.GetTempFileName()
            Dim lTempOutputFileName As String = System.IO.Path.GetTempFileName()
            lTable.FindFirst(1, ParmToVary)
            For iMon As Integer = 2 To 13
                'Vary each monthly value unless it is in a non-selected season
                If Me.Seasons Is Nothing OrElse Me.Seasons.SeasonSelected(Me.Seasons.SeasonIndex(Jday(StartYear, iMon - 1, 1, 0, 1, 0))) Then
                    UpdateParmTable(lTable, ParmToVary, iMon, CStr(CDbl(lTable.Value(iMon)) * CurrentValue))
                End If
            Next
            WriteParmFile(lTempParmFileName, lHeader, lTable, lFooter)

            lArgs.SetValue("CliGen Parm", lTempParmFileName)
            lArgs.SetValue("CliGen Out", lTempOutputFileName)
            lArgs.SetValue("Start Year", StartYear)
            lArgs.SetValue("Num Years", NumYears)
            lArgs.SetValue("Include Daily", False)
            lArgs.SetValue("Include Hourly", True)
            lCliGen.Open("Run CliGen", lArgs)
            For lDataSetIndex As Integer = 0 To Me.DataSets.Count - 1
                Dim lDataSet As atcTimeseries = Me.DataSets(lDataSetIndex)
                If Not lDataSet Is Nothing Then
                    Dim lCligenConstituent As String = CligenConstituents(lDataSetIndex)
                    For Each lCligenDataSet As atcTimeseries In lCliGen.DataSets
                        If lCligenDataSet.Attributes.GetValue("Constituent") = lCligenConstituent Then
                            'Set ID and Constituent to match what is in the WDM file
                            lCligenDataSet.Attributes.SetValue("ID", lDataSet.Attributes.GetValue("ID"))
                            lCligenDataSet.Attributes.SetValue("Constituent", lDataSet.Attributes.GetValue("Constituent"))
                            'Replace WDM dataset with Cligen dataset
                            Me.DataSets.Item(lDataSetIndex) = lCligenDataSet
                        End If
                    Next
                End If
            Next
            Try
                Kill(lTempParmFileName)
            Catch exKillParm As Exception
                Logger.Dbg("Could not delete TempParmFile '" & lTempParmFileName & "'" & vbCrLf & exKillParm.Message)
            End Try
            Try
                Kill(lTempOutputFileName)
            Catch exKillOutput As Exception
                Logger.Dbg("Could not delete TempOutputFile '" & lTempOutputFileName & "'" & vbCrLf & exKillOutput.Message)
            End Try
            Return Me.DataSets
        End If
        Return Nothing
    End Function

    Public Overrides Property XML() As String
        Get
            Dim lXML As New Chilkat.Xml
            lXML.LoadXml(MyBase.XML)
            lXML.NewChild("BaseParmFileName", BaseParmFileName)
            lXML.NewChild("ParmToVary", ParmToVary)
            lXML.NewChild("StartYear", StartYear)
            lXML.NewChild("NumYears", NumYears)
            Return lXML.GetXml
        End Get
        Set(ByVal Value As String)
            MyBase.XML = Value
            Dim lXML As New Chilkat.Xml
            lXML.LoadXml(Value)
            BaseParmFileName = lXML.GetChildWithTag("BaseParmFileName").Content
            ParmToVary = lXML.GetChildWithTag("ParmToVary").Content
            StartYear = lXML.GetChildWithTag("StartYear").Content()
            NumYears = lXML.GetChildWithTag("NumYears").Content()
        End Set
    End Property

    Public Overrides Function Clone() As atcVariation
        Dim newVariation As New VariationCligen
        With newVariation
            .BaseParmFileName = BaseParmFileName
            .ParmToVary = ParmToVary
            .StartYear = StartYear
            .NumYears = NumYears

            .Name = Name
            If Not DataSets Is Nothing Then .DataSets = DataSets.Clone()
            .ComputationSource = ComputationSource
            .Operation = Operation.Clone()
            .Seasons = Seasons 'TODO: clone Seasons of not Nothing
            .Selected = Selected
            .Min = Min
            .Max = Max
            .Increment = Increment
            .CurrentValue = CurrentValue
            .ColorAboveMax = ColorAboveMax
            .ColorBelowMin = ColorBelowMin
            .ColorDefault = ColorDefault
        End With
        Return newVariation
    End Function
End Class
