Friend Class atcWDMfile

    Private Structure FileDefinition
        Dim PPRBKR As Int32 'Primary backward record pointer   (always -998, was -999)
        Dim PPRFWR As Int32 'Primary forward record pointer    (always 0)
        Dim PSCBKR As Int32 'Secondary backward record pointer (always 0)
        Dim PSCFWR As Int32 'Secondary forward record pointer  (always 0)
        <VBFixedArray(24)> Public Unused05_28() As Int32
        Dim LSTREC As Int32
        Dim Unused30 As Int32
        Dim FREREC As Int32
        Dim DSCNT_Timeseries As Int32
        Dim DSFST_Timeseries As Int32
        Dim DSCNT_Table As Int32
        Dim DSFST_Table As Int32
        Dim DSCNT_Schematic As Int32
        Dim DSFST_Schematic As Int32
        Dim DSCNT_ProjectDescription As Int32
        Dim DSFST_ProjectDescription As Int32
        Dim DSCNT_Vector As Int32
        Dim DSFST_Vector As Int32
        Dim DSCNT_Raster As Int32
        Dim DSFST_Raster As Int32
        Dim DSCNT_SpaceTime As Int32
        Dim DSFST_SpaceTime As Int32
        Dim DSCNT_Attribute As Int32
        Dim DSFST_Attribute As Int32
        Dim DSCNT_Message As Int32
        Dim DSFST_Message As Int32
        <VBFixedArray(63)> Public Unused50_112() As Int32
        <VBFixedArray(99)> Public DirPnt() As Int32
    End Structure

    Dim pFileDefinition As FileDefinition
    Dim pFileName As String = ""

    Private Function FileDefinitionAsString() As String
        Dim lBuilder As New Text.StringBuilder
        Dim lIndex As Integer
        With pFileDefinition
            lBuilder.Append("PPRBKR " & .PPRBKR & vbCrLf)
            lBuilder.Append("PPRFWR " & .PPRFWR & vbCrLf)
            lBuilder.Append("PSCBKR " & .PSCBKR & vbCrLf)
            lBuilder.Append("PSCFWR " & .PSCFWR & vbCrLf)
            For lIndex = 0 To .Unused05_28.GetUpperBound(0)
                If .Unused05_28(lIndex) <> 0 Then
                    lBuilder.Append("Unused05_28(" & lIndex & ") = " & .Unused05_28(lIndex) & vbCrLf)
                End If
            Next
            lBuilder.Append("LSTREC " & .LSTREC & vbCrLf)
            lBuilder.Append("Unused30 " & .Unused30 & vbCrLf)
            lBuilder.Append("FREREC " & .FREREC & vbCrLf)
            lBuilder.Append("DSCNT_Timeseries " & .DSCNT_Timeseries & vbCrLf)
            lBuilder.Append("DSFST_Timeseries " & .DSFST_Timeseries & vbCrLf)
            lBuilder.Append("DSCNT_Table " & .DSCNT_Table & vbCrLf)
            lBuilder.Append("DSFST_Table " & .DSFST_Table & vbCrLf)
            lBuilder.Append("DSCNT_Schematic " & .DSCNT_Schematic & vbCrLf)
            lBuilder.Append("DSFST_Schematic " & .DSFST_Schematic & vbCrLf)
            lBuilder.Append("DSCNT_ProjectDescription " & .DSCNT_ProjectDescription & vbCrLf)
            lBuilder.Append("DSFST_ProjectDescription " & .DSFST_ProjectDescription & vbCrLf)
            lBuilder.Append("DSCNT_Vector " & .DSCNT_Vector & vbCrLf)
            lBuilder.Append("DSFST_Vector " & .DSFST_Vector & vbCrLf)
            lBuilder.Append("DSCNT_Raster " & .DSCNT_Raster & vbCrLf)
            lBuilder.Append("DSFST_Raster " & .DSFST_Raster & vbCrLf)
            lBuilder.Append("DSCNT_SpaceTime " & .DSCNT_SpaceTime & vbCrLf)
            lBuilder.Append("DSFST_SpaceTime " & .DSFST_SpaceTime & vbCrLf)
            lBuilder.Append("DSCNT_Attribute " & .DSCNT_Attribute & vbCrLf)
            lBuilder.Append("DSFST_Attribute " & .DSFST_Attribute & vbCrLf)
            lBuilder.Append("DSCNT_Message " & .DSCNT_Message & vbCrLf)
            lBuilder.Append("DSFST_Message " & .DSFST_Message & vbCrLf)
            For lIndex = 0 To .Unused50_112.GetUpperBound(0)
                If .Unused50_112(lIndex) <> 0 Then
                    lBuilder.Append("Unused50_112(" & lIndex & ") = " & .Unused50_112(lIndex) & vbCrLf)
                End If
            Next
            For lIndex = 0 To .DirPnt.GetUpperBound(0)
                If .DirPnt(lIndex) <> 0 Then
                    lBuilder.Append("DirPnt(" & lIndex & ") = " & .DirPnt(lIndex) & vbCrLf)
                End If
            Next
        End With
        Return lBuilder.ToString
    End Function

    Public Function OpenFile(ByVal aFilename As String) As Boolean
        Dim inFile As Short

        If Not IO.File.Exists(aFilename) Then
            Return False 'can't open a file that doesn't exist
        End If

        pFilename = aFilename

        inFile = FreeFile()
        FileOpen(inFile, aFilename, OpenMode.Binary, OpenAccess.Read, OpenShare.Shared)
        FileGet(inFile, pFileDefinition)

        Return True
    End Function


End Class
