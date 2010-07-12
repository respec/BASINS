Imports System.Collections.ObjectModel
Imports System.IO
Imports MapWinUtility
Imports atcUtility
Imports System.Text

Public Class MetConstituents
    Inherits KeyedCollection(Of String, MetConstituent)
    Implements IBlock

    Protected Overrides Function GetKeyForItem(ByVal aMetConstituent As MetConstituent) As String
        Dim lKey As String = aMetConstituent.Type
        Return lKey
    End Function

    Private pName As String
    Private pSWMMProject As SWMMProject

    Property Name() As String Implements IBlock.Name
        Get
            Return pName
        End Get
        Set(ByVal value As String)
            pName = value
        End Set
    End Property

    Public Sub New(ByVal aSWMMPRoject As SWMMProject, ByVal aName As String)
        pSWMMProject = aSWMMPRoject
        Name = aName
    End Sub

    Public Sub New(ByVal aSWMMPRoject As SWMMProject, ByVal aName As String, ByVal aContents As String)
        pSWMMProject = aSWMMPRoject
        Name = aName
        FromString(aContents)
    End Sub

    Public Sub FromString(ByVal aContents As String) Implements IBlock.FromString
        'TODO: fill this in
    End Sub

    Public Overrides Function ToString() As String
        Dim lSB As New StringBuilder

        Dim lFoundEvap As Boolean = False
        For Each lMetConstituent As MetConstituent In Me
            If lMetConstituent.Type = "EVAP" Or lMetConstituent.Type = "PEVT" Then
                If Not lMetConstituent.TimeSeries Is Nothing Then
                    lSB.Append("[EVAPORATION]" & vbCrLf & _
                               ";;Type       Parameters" & vbCrLf & _
                               ";;---------- ----------" & vbCrLf)

                    With lMetConstituent
                        lSB.Append(StrPad("TIMESERIES", 12, " ", False))
                        lSB.Append(" ")
                        lSB.Append(StrPad(.TimeSeries.Attributes.GetValue("Location") & ":E", 10, " ", False))
                        lSB.Append(" ")
                        lSB.Append(vbCrLf)
                    End With
                    lFoundEvap = True
                    Exit For
                End If
            End If
        Next

        For Each lMetConstituent As MetConstituent In Me
            If lMetConstituent.Type = "ATEM" Or lMetConstituent.Type = "ATMP" Then
                If Not lMetConstituent.TimeSeries Is Nothing Then
                    If lFoundEvap Then
                        lSB.Append(vbCrLf)
                    End If

                    lSB.Append("[TEMPERATURE]" & vbCrLf)

                    With lMetConstituent
                        lSB.Append(StrPad("TIMESERIES", 12, " ", False))
                        lSB.Append(" ")
                        lSB.Append(StrPad(.TimeSeries.Attributes.GetValue("Location") & ":T", 10, " ", False))
                        lSB.Append(" ")
                        lSB.Append(vbCrLf)
                    End With
                    Exit For
                End If
            End If
        Next

        Return lSB.ToString
    End Function

    Public Function TimeSeriesHeaderToString() As String
        Dim lSB As New StringBuilder
        lSB.Append("[TIMESERIES]" & vbCrLf & _
                   ";;Name           Date       Time       Value     " & vbCrLf & _
                   ";;-------------- ---------- ---------- ----------")
        Return lSB.ToString
    End Function

    Public Function TimeSeriesToStream(ByVal aSW As IO.StreamWriter) As String

        Dim lFoundEvap As Boolean = False
        For Each lMetConstituent As MetConstituent In Me
            If lMetConstituent.Type = "EVAP" Or lMetConstituent.Type = "PEVT" Then
                If Not lMetConstituent.TimeSeries Is Nothing Then
                    aSW.Write(";EVAPORATION" & vbCrLf)
                    Me.pSWMMProject.TimeSeriesToStream(lMetConstituent.TimeSeries, lMetConstituent.TimeSeries.Attributes.GetValue("Location") & ":E", aSW)
                    lFoundEvap = True
                    Exit For
                End If
            End If
        Next

        For Each lMetConstituent As MetConstituent In Me
            If lMetConstituent.Type = "ATEM" Or lMetConstituent.Type = "ATMP" Then
                If Not lMetConstituent.TimeSeries Is Nothing Then
                    If lFoundEvap Then
                        aSW.Write(vbCrLf)
                    End If
                    aSW.Write(";TEMPERATURE" & vbCrLf)
                    Me.pSWMMProject.TimeSeriesToStream(lMetConstituent.TimeSeries, lMetConstituent.TimeSeries.Attributes.GetValue("Location") & ":T", aSW)
                End If
            End If
        Next

        Return aSW.ToString
    End Function

    Public Function TimeSeriesFileNamesToString() As String
        Dim lSB As New StringBuilder

        Dim lFileName As String = ""
        For Each lMetConstituent As MetConstituent In Me
            If lMetConstituent.Type = "EVAP" Or lMetConstituent.Type = "PEVT" Then
                If lFileName.Length > 0 Then lSB.Append(vbCrLf)
                lFileName = PathNameOnly(Me.pSWMMProject.FileName) & g_PathChar & lMetConstituent.TimeSeries.Attributes.GetValue("Location") & "E.DAT"
                lSB.Append(StrPad(lMetConstituent.TimeSeries.Attributes.GetValue("Location") & ":E", 16, " ", False))
                lSB.Append(" FILE " & lFileName)
            ElseIf lMetConstituent.Type = "ATEM" Or lMetConstituent.Type = "ATMP" Then
                If lFileName.Length > 0 Then lSB.Append(vbCrLf)
                lFileName = PathNameOnly(Me.pSWMMProject.FileName) & g_PathChar & lMetConstituent.TimeSeries.Attributes.GetValue("Location") & "T.DAT"
                lSB.Append(StrPad(lMetConstituent.TimeSeries.Attributes.GetValue("Location") & ":T", 16, " ", False))
                lSB.Append(" FILE " & lFileName)
            End If
        Next

        Return lSB.ToString
    End Function

    Public Function TimeSeriesToFile() As Boolean

        For Each lMetConstituent As MetConstituent In Me
            If lMetConstituent.Type = "EVAP" Or lMetConstituent.Type = "PEVT" Then
                Dim lFileName As String = PathNameOnly(Me.pSWMMProject.FileName) & g_PathChar & lMetConstituent.TimeSeries.Attributes.GetValue("Location") & "E.DAT"
                Dim lSB As New StringBuilder
                lSB.Append(Me.pSWMMProject.TimeSeriesToString(lMetConstituent.TimeSeries, lMetConstituent.TimeSeries.Attributes.GetValue("Location") & ":E", lMetConstituent.Type))
                SaveFileString(lFileName, lSB.ToString)
            ElseIf lMetConstituent.Type = "ATEM" Or lMetConstituent.Type = "ATMP" Then
                Dim lFileName As String = PathNameOnly(Me.pSWMMProject.FileName) & g_PathChar & lMetConstituent.TimeSeries.Attributes.GetValue("Location") & "T.DAT"
                Dim lSB As New StringBuilder
                lSB.Append(Me.pSWMMProject.TimeSeriesToString(lMetConstituent.TimeSeries, lMetConstituent.TimeSeries.Attributes.GetValue("Location") & ":T", lMetConstituent.Type))
                SaveFileString(lFileName, lSB.ToString)
            End If
        Next

    End Function
End Class

Public Class MetConstituent
    Public Type As String 'Evap or Temp
    Public TimeSeries As atcData.atcTimeseries
End Class
