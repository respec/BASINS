Option Strict Off
Option Explicit On

Imports atcData
Imports atcUtility
Imports MapWinUtility

''' <summary>
''' Reads USGS rdb files containing daily values
''' </summary>
''' <remarks>
''' Would need to change pJulianInterval, ts and tu for non-daily values
''' Does not read provisional values into timeseries
''' </remarks>
Public Class atcTimeseriesScriptPlugin
    Inherits atcDataSource

    Private Shared pFilter As String = "Any Data File (*.*)|(*.*)"
    Private pErrorDescription As String

    Public Sub New()
        Filter = pFilter
    End Sub

    Public Overrides ReadOnly Property Description() As String
        Get
            Return "Read Data With Script"
        End Get
    End Property

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Timeseries::Script"
        End Get
    End Property

    Public Overrides ReadOnly Property Category() As String
        Get
            Return "File"
        End Get
    End Property

    Public Overrides ReadOnly Property CanOpen() As Boolean
        Get
            Return True
        End Get
    End Property

    Public Overrides Function Open(ByVal aFileName As String, _
                      Optional ByVal aAttributes As atcData.atcDataAttributes = Nothing) As Boolean
        If aFileName Is Nothing OrElse aFileName.Length = 0 OrElse Not IO.File.Exists(aFileName) Then
            aFileName = FindFile("Select " & Name & " file to open", , , Filter, True, , 1)
        End If

        If Not IO.File.Exists(aFileName) Then
            pErrorDescription = "File '" & aFileName & "' not found"
        Else
            Me.Specification = aFileName
            Try
                Dim lSelectForm As New frmSelectScript
                lSelectForm.ShowDialog()

            Catch e As Exception
                Logger.Dbg("Exception reading '" & aFileName & "': " & e.ToString)
                Return False
            End Try
        End If
    End Function

    'Private Function ScriptFileNames() As String(,)
    '    Dim lScriptsToShow As String(,) = GetAllSettings("atcTimeseriesScript", "Scripts")
    '    If lScriptsToShow.Length = 0 Then
    '        Dim lAllScriptFileNames As New Generic.List(Of String)
    '        Dim lScriptFilename As String = FindFile("TimeseriesScriptCSV.vb")
    '        If IO.File.Exists(lScriptFilename) Then
    '            For Each lFilename As String In IO.Directory.GetFiles(IO.Path.GetDirectoryName(lScriptFilename))
    '                Select Case IO.Path.GetExtension(lFilename).ToLower
    '                    Case ".vb", ".cs" : lAllScriptFileNames.Add(lFilename)
    '                End Select
    '            Next
    '        End If
    '        If lAllScriptFileNames.Count > 0 Then
    '            ReDim lScriptsToShow(lAllScriptFileNames.Count, 2)

    '        End If
    '    End If
    '    '        SaveSetting("atcTimeseriesScript", "Scripts", ScriptFilename, ScriptDescription)

    'End Function

End Class
