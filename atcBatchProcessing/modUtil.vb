Imports atcData
Imports atcUtility
Imports MapWinUtility
Public Module modUtil

    Public Class Inputs
        Public Shared IncludeYears As String = "IncludeYears"
        Public Shared StartYear As String = "FirstYear" '"STARTYEAR"
        Public Shared EndYear As String = "LastYear" '"ENDYEAR"
        Public Shared StartMonth As String = "BoundaryMonth" '"StartMonth"
        Public Shared StartDay As String = "BoundaryDay" '"StartDay"
        Public Shared EndMonth As String = "EndMonth"
        Public Shared EndDay As String = "EndDay"
        Public Shared HighFlowSeasonStart As String = "HIGHFLOWSEASONSTART" '10/01
        Public Shared HighFlowSeasonEnd As String = "HIGHFLOWSEASONEND"     '09/30
        Public Shared LowFlowSeasonStart As String = "LOWFLOWSEASONSTART"   '04/01
        Public Shared LowFlowSeasonEnd As String = "LOWFLOWSEASONEND"       '03/31

        Public Shared StationsInfo As String = "StationsInfo"
        Public Shared Streamflow As String = "Streamflow"
        Public Shared Method As String = "METHOD"
        Public Shared Methods As String = "Methods"

        Public Shared OutputDir As String = "OUTPUTDIR"
        Public Shared OutputPrefix As String = "OutputPrefix"
        Public Shared DataDir As String = "DataDir"

        Public Shared Function BasicAttributes() As Generic.List(Of String)
            Dim lBasicAttributes As New Generic.List(Of String)
            With lBasicAttributes
                .Add("ID")
                .Add("Min")
                .Add("Max")
                .Add("Mean")
                .Add("Standard Deviation")
                .Add("Count")
                .Add("Count Missing")
            End With
            Return lBasicAttributes
        End Function

        Public Shared Function NDayAttributes() As Generic.List(Of String)
            Dim lNDayAttributes As New Generic.List(Of String)
            With lNDayAttributes
                .Add("STAID")
                .Add("STANAM")
                .Add("Constituent")
            End With
            Return lNDayAttributes
        End Function

        Public Shared Function TrendAttributes() As Generic.List(Of String)
            Dim lTrendAttributes As New Generic.List(Of String)
            With lTrendAttributes
                .Add("Original ID")
                .Add("KENTAU")
                .Add("KENPLV")
                .Add("KENSLP")
                .Add("From")
                .Add("To")
                .Add("Count")
                .Add("Not Used")
                .Add("Min")
                .Add("Max")
                .Add("Constituent")
                .Add("STAID")
            End With
            Return lTrendAttributes
        End Function
    End Class
End Module
