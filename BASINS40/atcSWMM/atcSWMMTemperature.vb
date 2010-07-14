Imports System.Collections.ObjectModel
Imports System.IO
Imports MapWinUtility
Imports atcUtility
Imports System.Text

Public Class atcSWMMTemperature
    Implements IBlock

    Private pName As String = "[TEMPERATURE]"
    Private pSWMMProject As atcSWMMProject
    Public Timeseries As atcData.atcTimeseries

    Property Name() As String Implements IBlock.Name
        Get
            Return pName
        End Get
        Set(ByVal value As String)
            pName = value
        End Set
    End Property

    Public Sub New(ByVal aSWMMPRoject As atcSWMMProject)
        pSWMMProject = aSWMMPRoject
    End Sub

    Public Sub New(ByVal aSWMMPRoject As atcSWMMProject, ByVal aContents As String)
        pSWMMProject = aSWMMPRoject
        FromString(aContents)
    End Sub

    Public Sub FromString(ByVal aContents As String) Implements IBlock.FromString
        'TODO: fill this in
    End Sub

    Public Overrides Function ToString() As String
        Dim lSB As New StringBuilder

        If TimeSeries IsNot Nothing Then
            lSB.AppendLine(pName)
            lSB.Append(StrPad("TIMESERIES", 12, " ", False))
            lSB.Append(" ")
            lSB.Append(StrPad(Timeseries.Attributes.GetValue("Location") & ":T", 10, " ", False))
            lSB.Append(" ")
            lSB.AppendLine()
        End If
        
        Return lSB.ToString
    End Function

    Public Shared Function TimeSeriesHeaderToString() As String
        Return "[TIMESERIES]" & vbCrLf & _
               ";;Name           Date       Time       Value     " & vbCrLf & _
               ";;-------------- ---------- ---------- ----------"
    End Function

    Public Sub TimeSeriesToStream(ByVal aSW As IO.StreamWriter)
        If Timeseries IsNot Nothing Then
            aSW.Write(";TEMPERATURE" & vbCrLf)
            Me.pSWMMProject.TimeSeriesToStream(Timeseries, Timeseries.Attributes.GetValue("Location") & ":T", aSW)
        End If
    End Sub

    Public Function TimeSeriesFileNamesToString() As String
        Dim lSB As New StringBuilder

        If Timeseries IsNot Nothing Then
            Dim lFileName As String = PathNameOnly(Me.pSWMMProject.FileName) & g_PathChar & Timeseries.Attributes.GetValue("Location") & "T.DAT"
            lSB.Append(StrPad(Timeseries.Attributes.GetValue("Location") & ":T", 16, " ", False))
            lSB.Append(" FILE " & lFileName)
        End If

        Return lSB.ToString
    End Function

    Public Function TimeSeriesToFile() As Boolean

        If Timeseries IsNot Nothing Then
            Dim lFileName As String = PathNameOnly(Me.pSWMMProject.FileName) & g_PathChar & Timeseries.Attributes.GetValue("Location") & "T.DAT"
            Dim lSB As New StringBuilder
            lSB.Append(Me.pSWMMProject.TimeSeriesToString(Timeseries, Timeseries.Attributes.GetValue("Location") & ":T", "ATEM"))
            SaveFileString(lFileName, lSB.ToString)
        End If

    End Function
End Class
