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

    'Public AuxiParms As New atcData.atcDataAttributes
    Public AuxiParms As New Dictionary(Of String, String)

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
        Timeseries = New atcData.atcTimeseries(pSWMMProject)
        Timeseries.Attributes.SetValue("Location", "")
        Timeseries.Attributes.SetValue("Constituent", "ATEM")

        'TODO: populate Timeseries
        'Need to do a delayed action here

        'Break it up into multiple lines
        Dim lLines() As String = aContents.Split(vbCrLf)
        Dim lWord As String = "TIMESERIES"
        Dim laTSFile As String = String.Empty
        For I As Integer = 0 To lLines.Length - 1
            If Not lLines(I).StartsWith(";") Then
                laTSFile = lLines(I).Substring(lWord.Length).Trim()
                'Assuming there is only one TS for Temp
                If laTSFile.Length > 0 And laTSFile.EndsWith("T") Then
                    Timeseries.Attributes.SetValue("Location", laTSFile)
                Else
                    'Dim lDef As New atcData.atcDefinedValue
                    'With lDef
                    '    .Definition = New atcData.atcAttributeDefinition()
                    '    .Definition.Name = lparms(0) 'Parm name such as WINDSPEED
                    '    If lparms.Length > 2 Then
                    '        .Definition.Description = lparms(1) 'specification eg monthly
                    '        .Value = lparms(2) 'Parm values as a string
                    '    Else
                    '        .Value = lparms(1) 'sometimes, there is no description
                    '    End If
                    '    .Definition.TypeString = "String"
                    'End With
                    'AuxiParms.Add(lDef)

                    Dim lLine As String = lLines(I)
                    Dim laKey As String = StrSplit(lLine, " ", "")
                    If AuxiParms.ContainsKey(laKey.Trim()) Then
                        AuxiParms.Item(laKey) &= vbCrLf & lLine.Trim()
                    Else
                        AuxiParms.Add(laKey, lLine.Trim())
                    End If
                End If
            End If
        Next
    End Sub

    Public Sub ReadDataExternal(ByVal aFilename As String, ByVal aTS As atcData.atcTimeseries)

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
