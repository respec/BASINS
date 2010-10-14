Option Strict Off
Option Explicit On

Imports atcData
Imports atcUtility
Imports MapWinUtility
Imports MapWinUtility.Strings

''' <summary>
''' Reads USGS rdb files containing daily values
''' </summary>
''' <remarks>
''' Would need to change pJulianInterval, ts and tu for non-daily values
''' Does not read provisional values into timeseries
''' </remarks>
Public Class atcTimeseriesScriptPlugin
    Inherits atcTimeseriesSource

    Private Shared pFilter As String = "Any Data File (*.*)|*.*"
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
            aFileName = FindFile("Select data file to open", , , Filter, True, , 1)
        End If

        If Not IO.File.Exists(aFileName) Then
            pErrorDescription = "File '" & aFileName & "' not found"
        Else
            Me.Specification = aFileName
            Try
                Dim lSelectForm As New frmSelectScript
                With lSelectForm
                    .Width = GetSetting("BASINS4", "Window Positions", "SelectScriptWidth", .Width)
                    .Height = GetSetting("BASINS4", "Window Positions", "SelectScriptHeight", .Height)
                    .Text = "Script Selection for importing " & aFileName
                    .LoadGrid(aFileName)
ShowSelect:
                    .ShowDialog()
                    SaveSetting("BASINS4", "Window Positions", "SelectScriptWidth", .Width)
                    SaveSetting("BASINS4", "Window Positions", "SelectScriptHeight", .Height)
                    Dim lDefinitionFilename As String = .SelectedScript
                    Select Case .ButtonPressed
                        Case .cmdCancel.Text : Exit Function
                        Case .cmdRun.Text
                            Return RunSelectedScript(lDefinitionFilename, aFileName)
                        Case .cmdTest.Text
                            DebuggingScript = True
                            RunSelectedScript(lDefinitionFilename, aFileName)
                            DebuggingScript = False
                        Case .cmdWizard.Text
                            Dim lfrmInputwizard As New frmInputWizard
                            With lfrmInputwizard
                                If .txtSample.Count = 0 Then
                                    .txtSample.Add(._txtSample_0)
                                End If
                                .TserFile = Me
                                .txtDataFile.Text = aFileName
                                .txtScriptFile.Text = lDefinitionFilename
                                .ReadScript()
                                .ShowDialog()
                            End With

                    End Select
                End With
            Catch e As Exception
                Logger.Dbg("Exception reading '" & aFileName & "': " & e.ToString)
                Return False
            End Try
        End If
    End Function

    Public Function RunSelectedScript(ByVal aDefinitionFilename As String, ByVal aDataFilename As String) As Boolean
        Dim Script As clsATCscriptExpression

        Script = ScriptFromString(WholeFileString(aDefinitionFilename))
        If Script Is Nothing Then
            Logger.Msg("Could not load script " & aDefinitionFilename & vbCr & Err.Description, vbExclamation, "Run Script")
            Return False
        Else
            ScriptRun(Script, aDataFilename, Me)
            Return (Me.DataSets.Count > 0)
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
