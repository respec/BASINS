﻿Imports atcData
Imports atcUtility
Imports MapWinUtility.Strings

Public Class clsUEBInputControl

    'Public FileName As String
    'Public SJDate As Double
    'Public TimeStep As Integer
    'Public NSkip As Integer
    'Public InitialEnergy As Double
    'Public InitialH2OEquiv As Double
    'Public InitialSnowAge As Double
    'Public ATempTS As atcTimeseries
    'Public PrecipTS As atcTimeseries
    'Public WindTS As atcTimeseries
    'Public RelHTS As atcTimeseries
    'Public ATempRngTS As atcTimeseries
    'Public ShortRadTS As atcTimeseries
    'Public NetRadTS As atcTimeseries

    Public Variables As Generic.List(Of clsUEBVariable)
    Public Header As String
    Public SDate(4) As Integer
    Public EDate(4) As Integer
    Public TimeStep As Integer
    Public UTCOffset As Double

    Public FileName As String

    Public Sub New(ByVal aFilename As String)

        Dim i As Integer
        Dim lStr As String
        Dim lChrSep() As String = {" ", ".", vbTab}
        Dim lStrArray() As String
        Dim lAlreadyRead As Boolean = False
        Dim lUEBVariable As clsUEBVariable

        FileName = aFilename
        Dim lFileContents As String
        lFileContents = GetEmbeddedFileAsString("InputControl.dat")
        Variables = New Generic.List(Of clsUEBVariable)
ReadFile:

        'read header line in file
        Header = StrSplit(lFileContents, vbCrLf, "")
        'read start/end dates
        lStr = StrSplit(lFileContents, vbCrLf, "")
        lStr = ReplaceRepeats(lStr, " ") 'remove extra blanks
        lStrArray = lStr.Split(lChrSep, StringSplitOptions.None)
        For i = 0 To 4
            Integer.TryParse(lStrArray(i), SDate(i))
        Next
        lStr = StrSplit(lFileContents, vbCrLf, "")
        lStr = ReplaceRepeats(lStr, " ") 'remove extra blanks
        lStrArray = lStr.Split(lChrSep, StringSplitOptions.None)
        For i = 0 To 4
            Integer.TryParse(lStrArray(i), EDate(i))
        Next
        'read time step and UTC offset
        lStr = StrSplit(lFileContents, vbCrLf, "")
        TimeStep = CInt(StrSplit(lStr, " ", ""))
        lStr = StrSplit(lFileContents, vbCrLf, "")
        UTCOffset = CDbl(StrSplit(lStr, " ", ""))

        While lFileContents.Length > 0
            lUEBVariable = clsUEBVariable.FromInputVariableString(lFileContents)
            Dim lExistingVariable As clsUEBVariable = VariableFromDescription(lUEBVariable.Description)
            If lExistingVariable IsNot Nothing Then
                Variables.Insert(Variables.IndexOf(lExistingVariable), lUEBVariable)
                Variables.Remove(lExistingVariable)
            Else
                Variables.Add(lUEBVariable)
            End If
        End While

        If Not lAlreadyRead AndAlso IO.File.Exists(FileName) Then
            lFileContents = WholeFileString(FileName)
            lAlreadyRead = True
            GoTo ReadFile
        End If
    End Sub

    Private Function VariableFromDescription(ByVal aDescription As String) As clsUEBVariable
        For Each lUEBVariable As clsUEBVariable In Variables
            If lUEBVariable.Description = aDescription Then
                Return lUEBVariable
            End If
        Next
        Return Nothing
    End Function

    Private Function ReplaceRepeats(ByVal aSource As String, ByVal aReplace As String) As String
        Dim lRepeat As String = aReplace & aReplace
        While aSource.Contains(lRepeat)
            aSource = aSource.Replace(lRepeat, aReplace)
        End While
        Return aSource
    End Function

    Public Function WriteInputControlFile() As Boolean

        Dim lStr As String = Header & vbCrLf

        If FileName.Length > 0 Then
            Try
                lStr &= SDate(0) & " " & SDate(1) & " " & SDate(2) & " " & SDate(3) & "." & SDate(4) & vbCrLf
                lStr &= EDate(0) & " " & EDate(1) & " " & EDate(2) & " " & EDate(3) & "." & EDate(4) & vbCrLf
                lStr &= TimeStep & vbCrLf & UTCOffset & vbCrLf
                For Each lUEBParm As clsUEBVariable In Variables
                    lStr &= lUEBParm.InputVariableString
                Next
                SaveFileString(FileName, lStr)
                Return True
            Catch ex As Exception
                Return False
            End Try
        Else
            Return False
        End If
    End Function

    Public Sub ReadWeatherFile()

        'Dim lSR As New IO.StreamReader(FileName)
        'Dim lStr As String
        'Dim lCnt As Integer = 9
        'Dim lDate(5) As Integer
        'Dim lCurDate As Double
        'Dim lStrArray(7) As String
        'Dim lChrSep() As String = {" ", vbTab, vbCrLf}

        'lStr = lSR.ReadLine
        'lStrArray = lStr.Split(lChrSep, lCnt, StringSplitOptions.None)
        'lDate(1) = CInt(lStrArray(0))
        'lDate(2) = CInt(lStrArray(1))
        'lDate(0) = CInt(lStrArray(2))
        'If lDate(0) < 100 Then
        '    lDate(0) += 1900
        'End If
        'lDate(3) = CInt(lStrArray(3))
        'SJDate = Date2J(lDate)
        'TimeStep = CInt(lStrArray(4))
        'InitialEnergy = CDbl(lStrArray(5))
        'InitialH2OEquiv = CDbl(lStrArray(6))
        'InitialSnowAge = CDbl(lStrArray(7))

        'lCurDate = SJDate
        'lStr = lSR.ReadLine
        'lStrArray = lStr.Split(lChrSep, lCnt, StringSplitOptions.None)
        'NSkip = lStrArray(0)
        'If NSkip > 0 Then
        '    For i As Integer = 1 To NSkip
        '        lStr = lSR.ReadLine
        '    Next
        '    lCurDate += TimeStep * NSkip / 24
        'End If

        ''TODO: read weather data into TSers

    End Sub

End Class
