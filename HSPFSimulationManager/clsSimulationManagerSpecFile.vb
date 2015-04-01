Imports atcUtility
Imports MapWinUtility

Public Class clsSimulationManagerSpecFile
    Private Const FileHeader As String = "SARA HSPF Simulation Manager Configuration v1.0"
    Private Shared pDelimiters() As Char = {" "c, Chr(9)}

    Friend Shared Function Open(ByRef aWindowSize As Drawing.Size, ByRef aIconSize As Drawing.Size, ByVal aFileName As String) As IconCollection
        Dim lAllModels As New IconCollection
        Dim lReadingModel As clsIcon = Nothing
        Dim lBasePath As String = String.Empty
        Dim lCheckHeader As Boolean = True
        For Each lLine As String In LinesInFile(aFileName)
            If lCheckHeader Then
                If Not lLine.StartsWith(FileHeader) Then
                    Throw New ApplicationException("File header expected: '" & FileHeader & "'" & vbCrLf & "File header found: '" & lLine & "'")
                End If
                lCheckHeader = False
            End If
            Dim lDelimPos As Integer = lLine.IndexOfAny(pDelimiters)
            If lDelimPos > 0 Then
                Dim lArgument As String = lLine.Substring(lDelimPos + 1).Trim
                If lArgument.Length > 0 Then
                    Dim lKeyword As String = lLine.Substring(0, lDelimPos).Trim.ToUpperInvariant()
                    Select Case lKeyword
                        Case "BASEPATH" : lBasePath = lArgument
                        Case "WATERSHEDNAME"
                            lReadingModel = lAllModels.FindOrAddIcon(lArgument)
                        Case "SCENARIO"
                            lReadingModel.Scenario = New clsUciScenario(lArgument.Trim, lBasePath)
                        Case "SCENARIOS"
                            For Each lUciScenarioString As String In lArgument.Split(",")
                                lReadingModel.ScenariosAdd(New clsUciScenario(lUciScenarioString, lBasePath))
                            Next
                        Case "DOWNSTREAMNAME"
                            Select Case lArgument.Trim.ToLowerInvariant
                                Case "", "none" 'No downstream model
                                Case Else
                                    lReadingModel.DownstreamIcon = lAllModels.FindOrAddIcon(lArgument)
                                    lReadingModel.DownstreamIcon.UpstreamIcons.Add(lReadingModel)
                            End Select
                        Case "WATERSHEDIMAGE"
                            lReadingModel.WatershedImageFilename = IO.Path.Combine(lBasePath, lArgument)
                            Try
                                lReadingModel.WatershedImage = Drawing.Image.FromFile(lReadingModel.WatershedImageFilename)
                                ' Dim lNewWidth As Integer = lReadingModel.BackgroundImage.Width * 1.1
                                ' Dim lNewHeight As Integer = lReadingModel.BackgroundImage.Height * 1.1
                                'Dim lImageScale As Double = 1
                                'If lNewWidth > 200 Then
                                '    lImageScale = 200.0 / lNewWidth
                                '    lNewHeight *= lImageScale
                                '    lNewWidth = 200
                                'End If
                                'If lNewHeight > 200 Then
                                '    lImageScale *= 200.0 / lNewHeight
                                '    lNewWidth *= lImageScale
                                '    lNewHeight = 200
                                'End If
                                lReadingModel.BackgroundImageLayout = ImageLayout.Zoom
                            Catch
                            End Try

                            'lReadingModel.Size = New Point(lNewWidth, lNewHeight)
                        Case "MODELXY"
                            Dim lXY() As String = lArgument.Replace(" ", "").Split(",")
                            If lXY.Length = 2 Then
                                lReadingModel.Location = New Drawing.Point(CInt(lXY(0)), CInt(lXY(1)))
                            End If
                        Case "WINDOWSIZE"
                            Dim lWidthHeight() As String = lArgument.Replace(" ", "").Split(",")
                            If lWidthHeight.Length = 2 Then
                                aWindowSize = New Drawing.Size(CInt(lWidthHeight(0)), CInt(lWidthHeight(1)))
                            End If
                        Case "ICONSIZE"
                            Dim lWidthHeight() As String = lArgument.Replace(" ", "").Split(",")
                            If lWidthHeight.Length = 2 Then
                                aIconSize = New Drawing.Size(CInt(lWidthHeight(0)), CInt(lWidthHeight(1)))
                            End If
                    End Select
                End If
            End If
        Next
        Return lAllModels
    End Function

    Friend Shared Sub Save(ByVal aSaveIcons As IconCollection, ByVal aWindowSize As Drawing.Size, ByVal aIconSize As Drawing.Size, ByVal aFileName As String)
        Dim lWriter As New IO.StreamWriter(aFileName)
        lWriter.WriteLine(FileHeader)
        lWriter.WriteLine("WindowSize" & vbTab & aWindowSize.Width & "," & aWindowSize.Height)
        lWriter.WriteLine("IconSize" & vbTab & aIconSize.Width & "," & aIconSize.Height)
        Try
            Dim lBasePath As String = IO.Path.GetDirectoryName(aSaveIcons(0).UciFileName)
            If lBasePath.Length > 0 Then lBasePath &= IO.Path.DirectorySeparatorChar

            For Each lIcon In aSaveIcons
                If lBasePath.Length > 0 Then
                    For Each lScenario As clsUciScenario In lIcon.Scenarios
                        Dim lThisPath As String = IO.Path.GetDirectoryName(lScenario.UciFileName)
                        If lThisPath.Length > 0 Then
                            lThisPath &= IO.Path.DirectorySeparatorChar
                            Dim lLastChar As Integer = lBasePath.Length
                            If lThisPath.Length < lLastChar Then
                                lLastChar = lThisPath.Length
                                lBasePath = lBasePath.Substring(0, lLastChar)
                            End If
                            lLastChar -= 1
                            For lCharIndex As Integer = 0 To lLastChar
                                If lBasePath.Substring(lCharIndex, 1).ToLowerInvariant() <> lThisPath.Substring(lCharIndex, 1).ToLowerInvariant() Then
                                    lBasePath = lBasePath.Substring(0, lCharIndex)
                                    Exit For
                                End If
                            Next
                        End If
                    Next
                End If
            Next
            'Make sure lBasePath ends with a complete folder name, discard any partial match
            While lBasePath.Length > 0 AndAlso Not lBasePath.EndsWith(IO.Path.DirectorySeparatorChar)
                lBasePath = lBasePath.Substring(0, lBasePath.Length - 1)
            End While
            If lBasePath.Length > 0 Then
                lWriter.WriteLine("BasePath" & vbTab & lBasePath)
            End If

            For Each lIcon In aSaveIcons
                lWriter.WriteLine()
                lWriter.WriteLine("WatershedName" & vbTab & lIcon.WatershedName)

                If lIcon.DownstreamIcon Is Nothing Then
                    lWriter.WriteLine("DownstreamName" & vbTab & "none")
                Else
                    lWriter.WriteLine("DownstreamName" & vbTab & lIcon.DownstreamIcon.WatershedName)
                End If

                lWriter.WriteLine("Scenario" & vbTab & lIcon.Scenario.ScenarioName & "|" & RemoveBasePath(lIcon.Scenario.UciFileName, lBasePath))

                If lIcon.Scenarios.Count > 1 Then
                    Dim lScenariosString As String = ""
                    For Each lScenario As clsUciScenario In lIcon.Scenarios
                        lScenariosString &= lScenario.ScenarioName & "|" & RemoveBasePath(lScenario.UciFileName, lBasePath) & ","
                    Next
                    lWriter.WriteLine("Scenarios" & vbTab & lScenariosString.TrimEnd(","))
                End If

                If IO.File.Exists(lIcon.WatershedImageFilename) Then
                    lWriter.WriteLine("WatershedImage" & vbTab & RemoveBasePath(lIcon.WatershedImageFilename, lBasePath))
                End If
                lWriter.WriteLine("ModelXY" & vbTab & lIcon.Location.X & "," & lIcon.Location.Y)
            Next
        Catch ex As Exception
            Logger.Msg(ex.ToString, MsgBoxStyle.Exclamation, "Error Saving " & aFileName)
        Finally
            Try
                lWriter.Close()
            Catch
            End Try
        End Try
    End Sub

    Private Shared Function RemoveBasePath(ByVal aFullPath As String, ByVal aBasePath As String) As String
        If aFullPath.ToLowerInvariant().StartsWith(aBasePath.ToLowerInvariant()) Then
            Return aFullPath.Substring(aBasePath.Length)
        Else
            Return aFullPath
        End If
    End Function

End Class
