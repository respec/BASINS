Friend Class atcWDMfile

    Private Structure FileDefinition
        Dim PPRBKR As Int32 ' 1 Primary backward record pointer   (always -998, was -999)
        Dim PPRFWR As Int32 ' 2 Primary forward record pointer    (always 0)
        Dim PSCBKR As Int32 ' 3 Secondary backward record pointer (always 0)
        Dim PSCFWR As Int32 ' 4 Secondary forward record pointer  (always 0)
        <VBFixedArray(23)> Public Unused05_28() As Int32 ' 5 - 28 (size 24)
        Dim LSTREC As Int32 '29
        Dim Unused30 As Int32 '30
        Dim FREREC As Int32 '31
        Dim DSCNT_Timeseries As Int32 '32
        Dim DSFST_Timeseries As Int32 '33
        Dim DSCNT_Table As Int32 '34
        Dim DSFST_Table As Int32 '35
        Dim DSCNT_Schematic As Int32 '36
        Dim DSFST_Schematic As Int32 '37
        Dim DSCNT_ProjectDescription As Int32 '38
        Dim DSFST_ProjectDescription As Int32 '39
        Dim DSCNT_Vector As Int32 '40
        Dim DSFST_Vector As Int32 '41
        Dim DSCNT_Raster As Int32 '42
        Dim DSFST_Raster As Int32 '43
        Dim DSCNT_SpaceTime As Int32 '44
        Dim DSFST_SpaceTime As Int32 '45
        Dim DSCNT_Attribute As Int32 '46
        Dim DSFST_Attribute As Int32 '47
        Dim DSCNT_Message As Int32 '48
        Dim DSFST_Message As Int32 '49
        <VBFixedArray(72)> Public Unused50_112() As Int32 '50 - 112 (size 73)
        <VBFixedArray(399)> Public DirPnt() As Int32 '113-512 (size 400)
    End Structure

    Dim pFileDefinition As FileDefinition
    Dim pFileName As String = ""

    Private Function Int32ToStringIfNotZero(ByVal aTitle As String, ByVal aValue As Int32) As String
        If aValue <> 0 Then
            Return aTitle & aValue & vbCr
        Else
            Return ""
        End If
    End Function

    Private Function FileDefinitionToString() As String
        Dim lBuilder As New Text.StringBuilder
        Dim lIndex As Integer

        lBuilder.Append("WDMFileName = " & pFileName & vbCrLf)
        lBuilder.Append("FileDefintionRecord" & vbCrLf)
        With pFileDefinition
            lBuilder.Append(Int32ToStringIfNotZero("  PPRBKR ", .PPRBKR))
            lBuilder.Append(Int32ToStringIfNotZero("  PPRFWR ", .PPRFWR))
            lBuilder.Append(Int32ToStringIfNotZero("  PSCBKR ", .PSCBKR))
            lBuilder.Append(Int32ToStringIfNotZero("  PSCFWR ", .PSCFWR))
            For lIndex = 0 To .Unused05_28.GetUpperBound(0)
                lBuilder.Append(Int32ToStringIfNotZero("  Unused(" & lIndex + 5 & ")", .Unused05_28(lIndex)))
            Next
            lBuilder.Append(Int32ToStringIfNotZero("  LSTREC ", .LSTREC))
            lBuilder.Append(Int32ToStringIfNotZero("  Unused20 ", .Unused30))
            lBuilder.Append(Int32ToStringIfNotZero("  FREREC ", .FREREC))
            lBuilder.Append(Int32ToStringIfNotZero("  DSCNT_Timeseries ", .DSCNT_Timeseries))
            lBuilder.Append(Int32ToStringIfNotZero("  DSFST_Timeseries ", .DSFST_Timeseries))
            lBuilder.Append(Int32ToStringIfNotZero("  DSCNT_Table ", .DSCNT_Table))
            lBuilder.Append(Int32ToStringIfNotZero("  DSFST_Table ", .DSFST_Table))
            lBuilder.Append(Int32ToStringIfNotZero("  DSCNT_Schematic ", .DSCNT_Schematic))
            lBuilder.Append(Int32ToStringIfNotZero("  DSFST_Schematic ", .DSFST_Schematic))
            lBuilder.Append(Int32ToStringIfNotZero("  DSCNT_ProjectDescription ", .DSCNT_ProjectDescription))
            lBuilder.Append(Int32ToStringIfNotZero("  DSFST_ProjectDescription ", .DSFST_ProjectDescription))
            lBuilder.Append(Int32ToStringIfNotZero("  DSCNT_Vector ", .DSCNT_Vector))
            lBuilder.Append(Int32ToStringIfNotZero("  DSFST_Vector ", .DSFST_Vector))
            lBuilder.Append(Int32ToStringIfNotZero("  DSCNT_Raster ", .DSCNT_Raster))
            lBuilder.Append(Int32ToStringIfNotZero("  DSFST_Raster ", .DSFST_Raster))
            lBuilder.Append(Int32ToStringIfNotZero("  DSCNT_SpaceTime ", .DSCNT_SpaceTime))
            lBuilder.Append(Int32ToStringIfNotZero("  DSFST_SpaceTime ", .DSFST_SpaceTime))
            lBuilder.Append(Int32ToStringIfNotZero("  DSCNT_Attribute ", .DSCNT_Attribute))
            lBuilder.Append(Int32ToStringIfNotZero("  DSFST_Attribute ", .DSFST_Attribute))
            lBuilder.Append(Int32ToStringIfNotZero("  DSCNT_Message ", .DSCNT_Message))
            lBuilder.Append(Int32ToStringIfNotZero("  DSFST_Message ", .DSFST_Message))
            For lIndex = 0 To .Unused50_112.GetUpperBound(0)
                lBuilder.Append(Int32ToStringIfNotZero("  Unused50_112(" & lIndex + 50 & ") = ", .Unused50_112(lIndex) & vbCrLf))
            Next
            For lIndex = 0 To .DirPnt.GetUpperBound(0)
                lBuilder.Append(Int32ToStringIfNotZero("  DirPnt(" & lIndex + 1 & ") = ", .DirPnt(lIndex)))
            Next
        End With
        Return lBuilder.ToString
    End Function

    Public Function OpenFile(ByVal aFilename As String) As Boolean
        Dim lFileHandle As Short

        If Not IO.File.Exists(aFilename) Then
            Return False 'can't open a file that doesn't exist
        Else
            pFileName = aFilename

            lFileHandle = FreeFile()
            FileOpen(lFileHandle, pFileName, OpenMode.Binary, OpenAccess.Read, OpenShare.Shared)
            FileGet(lFileHandle, pFileDefinition)
            FileClose(lFileHandle)
            Return True
        End If
    End Function

    Public Overrides Function ToString() As String
        Return FileDefinitionToString()
    End Function

End Class
