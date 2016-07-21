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
    Private Const pWizardTag As String = "Wizard:"
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
                Dim lDefinitionFilename As String = ""
                If aAttributes IsNot Nothing AndAlso aAttributes.ContainsAttribute("ScriptFileName") Then
                    lDefinitionFilename = aAttributes.GetValue("ScriptFileName")
                End If
                If Not IO.File.Exists(lDefinitionFilename) Then
                    lDefinitionFilename = SelectScript(aFileName, "Script Selection for importing " & aFileName, True)
                End If
                If IO.File.Exists(lDefinitionFilename) Then
                    Return RunSelectedScript(lDefinitionFilename, aFileName)
                ElseIf lDefinitionFilename.StartsWith(pWizardTag) Then
                    Dim lCountBefore As Integer = Me.DataSets.Count
                    ScriptInit()
                    Dim lfrmInputwizard As New frmInputWizard
                    With lfrmInputwizard
                        If .txtSample.Count = 0 Then
                            .txtSample.Add(._txtSample_0)
                        End If
                        .TserFile = Me
                        .txtDataFile.Text = aFileName
                        .txtScriptFile.Text = lDefinitionFilename.Substring(pWizardTag.Length)
                        .ReadScript()
                        .ShowDialog()
                    End With
                    Return (Me.DataSets.Count > lCountBefore)
                End If
            Catch e As Exception
                Logger.Dbg("Exception reading '" & aFileName & "': " & e.ToString)
                Return False
            End Try
        End If
    End Function

    Public Shared Function SelectScript(ByVal aDataFileName As String, ByVal aTitle As String, ByVal aWizardButtonVisible As Boolean) As String
        Try
            Dim lSelectForm As New frmSelectScript
            With lSelectForm
                .Width = GetSetting("BASINS41", "Window Positions", "SelectScriptWidth", .Width)
                .Height = GetSetting("BASINS41", "Window Positions", "SelectScriptHeight", .Height)
                .cmdWizard.Visible = aWizardButtonVisible
                .Text = aTitle
                .LoadGrid(aDataFileName)
ShowSelect:
                .ShowDialog()
                SaveSetting("BASINS41", "Window Positions", "SelectScriptWidth", .Width)
                SaveSetting("BASINS41", "Window Positions", "SelectScriptHeight", .Height)
                Dim lDefinitionFilename As String = .SelectedScript
                Select Case .ButtonPressed
                    Case .cmdCancel.Text
                        Logger.Dbg("Cancelled")
                        Logger.LastDbgText = "" 'Avoid displaying an error message
                    Case .cmdRun.Text
                        Return .SelectedScript
                    Case .cmdWizard.Text
                        Return pWizardTag & .SelectedScript
                End Select
            End With
        Catch e As Exception
            Logger.Dbg("Exception selecting script for '" & aDataFileName & "': " & e.ToString)
        End Try
        Return ""
    End Function

    Public Function RunSelectedScript(ByVal aDefinitionFilename As String, ByVal aDataFilename As String) As Boolean
        Return RunSelectedScript(aDefinitionFilename, aDataFilename, Me)
    End Function
    Public Function RunSelectedScript(ByVal aDefinitionFilename As String, ByVal aDataFilename As String, ByVal aSource As atcTimeseriesSource) As Boolean
        If String.IsNullOrEmpty(aDefinitionFilename) Then
            Logger.Msg("No script file selected", vbExclamation, "Run Script")
            Return False
        End If
        If Not IO.File.Exists(aDefinitionFilename) Then
            Logger.Msg("Could not find script file '" & aDefinitionFilename & "'", vbExclamation, "Run Script")
            Return False
        End If

        Dim Script As clsATCscriptExpression = ScriptFromString(WholeFileString(aDefinitionFilename))
        If Script Is Nothing Then
            Logger.Msg("Could not load script '" & aDefinitionFilename & "'" & vbCr & Err.Description, vbExclamation, "Run Script")
            Return False
        Else
            Dim lSourceHadDatasetCount As Integer = aSource.DataSets.Count
            Dim lMessage As String = ScriptRun(Script, aDataFilename, aSource)
            aSource.Attributes.SetValue("ScriptFileName", aDefinitionFilename)
            If aSource.DataSets.Count > lSourceHadDatasetCount Then
                lMessage &= vbCrLf & "Dataset Count = " & aSource.DataSets.Count
                If lSourceHadDatasetCount > 0 Then
                    lMessage &= vbCrLf & "Added " & aSource.DataSets.Count - lSourceHadDatasetCount & " Datasets"
                End If
                Logger.Msg(lMessage, vbOKOnly, "Ran Import Script")
                Return True
            Else
                Logger.Msg(lMessage & vbCrLf & "Did not add any datasets", vbOKOnly, "Ran Import Data Script")
                Return False
            End If
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
