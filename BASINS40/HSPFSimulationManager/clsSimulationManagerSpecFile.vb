Imports atcUtility
Imports MapWinUtility

Public Class clsSimulationManagerSpecFile
    Friend Shared Function Open(ByVal aFileName As String) As IconCollection
        Dim lAllModels As New IconCollection
        Dim lReadingModel As clsSchematicIcon = Nothing
        Dim lBasePath As String = String.Empty
        For Each lLine As String In LinesInFile(aFileName)
            Dim lFields() As String = lLine.Split(vbTab)
            If lFields.Length = 2 Then
                Select lFields(0).ToUpperInvariant()
                    Case "BASEPATH" : lBasePath = lFields(1)
                    Case "MODELUCI"
                        lReadingModel = FindOrAddIcon(lAllModels, IO.Path.Combine(lBasePath, lFields(1)))
                        lReadingModel.UciFileName = IO.Path.Combine(lBasePath, lFields(1))
                    Case "MODELNAME"
                        lReadingModel.Label = lFields(1)
                    Case "DOWNSTREAMUCI"
                        lReadingModel.DownstreamIcons.Add(FindOrAddIcon(lAllModels, IO.Path.Combine(lBasePath, lFields(1))))
                    Case "WATERSHEDIMAGE"
                        lReadingModel.BackgroundImage = Drawing.Image.FromFile(IO.Path.Combine(lBasePath, lFields(1)))
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

                        'lReadingModel.Size = New Point(lNewWidth, lNewHeight)
                End Select
            End If
        Next

        Dim lOutlets As New IconCollection

        Return lAllModels 'lOutlets
    End Function

    Friend Shared Function FindOrAddIcon(ByVal aIconCollection As IconCollection, ByVal aUciFilename As String) As clsSchematicIcon
        Dim lIcon As clsSchematicIcon
        If aIconCollection.Contains(aUciFilename.ToLowerInvariant) Then
            lIcon = aIconCollection.Item(aUciFilename.ToLowerInvariant)
        Else
            lIcon = New clsSchematicIcon
            lIcon.UciFileName = aUciFilename
            aIconCollection.Add(lIcon)
        End If
        Return lIcon
    End Function
End Class
