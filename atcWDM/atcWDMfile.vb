Friend Class atcWDMfile
    Const pReclB As Int32 = 2048
    Dim pFileName As String
    Dim pFileDefinition As clsFileDefinition
    Dim pDsnRecordPointer() As Int32
    Dim pRecordInUse() As Int32

    Public Function OpenFile(ByVal aFilename As String) As Boolean
        If Not IO.File.Exists(aFilename) Then
            Return False 'can't open a file that doesn't exist
        Else
            pFileName = aFilename
            Dim lFS As IO.FileStream
            lFS = IO.File.Open(pFileName, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.Read)
            lFS.Seek(0, IO.SeekOrigin.Begin)
            Dim lBR As New IO.BinaryReader(lFS)
            pFileDefinition = New clsFileDefinition(lBR)
            With pFileDefinition
                ReDim pRecordInUse(.LSTREC)
                SetRecordUse(1, -3)
                'TODO inventory free record chain

                'check direcory records
                For lPos As Integer = 0 To .DirPnt.GetUpperBound(0)
                    Dim lDirRecord As Integer = pFileDefinition.DirPnt(lPos)
                    If lDirRecord > 0 Then 'process this directory
                        SetRecordUse(lDirRecord, -1)
                        Dim lSize As Integer = (lPos + 1) * 500
                        ReDim Preserve pDsnRecordPointer(lSize)
                        lFS.Seek(((lDirRecord - 1) * pReclB), IO.SeekOrigin.Begin)
                        For lInd As Integer = 1 To 4
                            If lBR.ReadInt32 <> 0 Then
                                Throw New Exception("Directory Record " & lDirRecord & " With NonZero Pointers at" & lInd)
                            End If
                        Next
                        For lInd As Integer = lSize - 499 To lSize
                            pDsnRecordPointer(lInd) = lBR.ReadInt32
                            If pDsnRecordPointer(lInd) <> 0 Then
                                SetRecordUse(pDsnRecordPointer(lInd), lInd)
                            End If
                            'need to follow dsn chain
                        Next
                    End If
                Next
            End With
            lFS.Close()
            Return True
        End If
    End Function

    Public Overrides Function ToString() As String
        Dim lBuilder As New Text.StringBuilder
        lBuilder.Append("WDMFileName = " & pFileName & vbCr)
        lBuilder.Append("FileDefintionRecord" & vbCr)
        lBuilder.Append(pFileDefinition.ToString())
        lBuilder.Append("Datasets" & vbCr)
        For lInd As Integer = 1 To pDsnRecordPointer.GetUpperBound(0)
            lBuilder.Append(Int32ToStringIfNotZero("Dsn(" & lInd & ") at ", pDsnRecordPointer(lInd)))
        Next
        lBuilder.Append("RecordUsage" & vbCr)
        For lInd As Integer = 1 To pRecordInUse.GetUpperBound(0)
            lBuilder.Append(Int32ToStringIfNotZero("Rec(" & lInd & ") use ", pRecordInUse(lInd)))
        Next
        Return lBuilder.ToString
    End Function

    Private Class clsFileDefinition
        Dim PPRBKR As Int32 ' 1 Primary backward record pointer   (always -998, was -999)
        Dim PPRFWR As Int32 ' 2 Primary forward record pointer    (always 0)
        Dim PSCBKR As Int32 ' 3 Secondary backward record pointer (always 0)
        Dim PSCFWR As Int32 ' 4 Secondary forward record pointer  (always 0)
        Dim Unused05_28(23) As Byte ' 5 - 28 
        Friend LSTREC As Int32 '29
        Dim UnusedA As Int32 '30
        Friend FREREC As Int32 '31
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
        Dim UnusedB(62) As Int32 '50 - 112 (size 63)
        Friend DirPnt(399) As Int32 '113- 512 (size 400)

        Friend Sub New(ByVal aBR As IO.BinaryReader) 'could use handle here
            PPRBKR = aBR.ReadInt32() ' Primary backward record pointer   (always -998, was -999)
            PPRFWR = aBR.ReadInt32() ' Primary forward record pointer    (always 0)
            PSCBKR = aBR.ReadInt32() ' Secondary backward record pointer (always 0)
            PSCFWR = aBR.ReadInt32() ' Secondary forward record pointer  (always 0)
            For lPos As Integer = 0 To Unused05_28.GetUpperBound(0)
                Unused05_28(lPos) = aBR.ReadInt32  ' 5 - 28 (size 24)
            Next
            LSTREC = aBR.ReadInt32() '29
            UnusedA = aBR.ReadInt32() '30
            FREREC = aBR.ReadInt32() '31
            DSCNT_Timeseries = aBR.ReadInt32() '32
            DSFST_Timeseries = aBR.ReadInt32() '33
            DSCNT_Table = aBR.ReadInt32() '34
            DSFST_Table = aBR.ReadInt32() '35
            DSCNT_Schematic = aBR.ReadInt32() '36
            DSFST_Schematic = aBR.ReadInt32() '37
            DSCNT_ProjectDescription = aBR.ReadInt32() '38
            DSFST_ProjectDescription = aBR.ReadInt32() '39
            DSCNT_Vector = aBR.ReadInt32() '40
            DSFST_Vector = aBR.ReadInt32() '41
            DSCNT_Raster = aBR.ReadInt32() '42
            DSFST_Raster = aBR.ReadInt32() '43
            DSCNT_SpaceTime = aBR.ReadInt32() '44
            DSFST_SpaceTime = aBR.ReadInt32() '45
            DSCNT_Attribute = aBR.ReadInt32() '46
            DSFST_Attribute = aBR.ReadInt32() '47
            DSCNT_Message = aBR.ReadInt32() '48
            DSFST_Message = aBR.ReadInt32() '49
            For lPos As Integer = 0 To UnusedB.GetUpperBound(0)
                UnusedB(lPos) = aBR.ReadInt32  '50 - 112 (size 63 four byte integers)
            Next
            For lPos As Integer = 0 To DirPnt.GetUpperBound(0)
                DirPnt(lPos) = aBR.ReadInt32  '113-512 (size 400)
            Next
        End Sub

        Overrides Function ToString() As String
            Dim lBuilder As New Text.StringBuilder
            Dim lIndex As Integer

            With Me
                lBuilder.Append(Int32ToStringIfNotZero("  PPRBKR ", .PPRBKR))
                lBuilder.Append(Int32ToStringIfNotZero("  PPRFWR ", .PPRFWR))
                lBuilder.Append(Int32ToStringIfNotZero("  PSCBKR ", .PSCBKR))
                lBuilder.Append(Int32ToStringIfNotZero("  PSCFWR ", .PSCFWR))
                For lIndex = 0 To .Unused05_28.GetUpperBound(0)
                    lBuilder.Append(Int32ToStringIfNotZero("  Unused(" & lIndex + 5 & ")", .Unused05_28(lIndex)))
                Next
                lBuilder.Append(Int32ToStringIfNotZero("  LSTREC ", .LSTREC))
                lBuilder.Append(Int32ToStringIfNotZero("  Unused30 ", .UnusedA))
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
                For lIndex = 0 To .UnusedB.GetUpperBound(0)
                    lBuilder.Append(Int32ToStringIfNotZero("  Unused50_112(" & lIndex + 50 & ") = ", .UnusedB(lIndex) & vbCrLf))
                Next
                For lIndex = 0 To .DirPnt.GetUpperBound(0)
                    lBuilder.Append(Int32ToStringIfNotZero("  DirPnt(" & lIndex + 1 & ") = ", .DirPnt(lIndex)))
                Next
            End With
            Return lBuilder.ToString
        End Function
    End Class

    Friend Sub SetRecordUse(ByVal aRecord As Integer, ByVal aUse As Integer)
        If pRecordInUse(aRecord) = 0 Then 'not in use
            pRecordInUse(aRecord) = aUse
        Else
            Throw New Exception("WDM record (" & aRecord & ") already in use with code " & aUse)
        End If
    End Sub

    Friend Shared Function Int32ToStringIfNotZero(ByVal aTitle As String, ByVal aValue As Int32) As String
        If aValue <> 0 Then
            Return aTitle & aValue & vbCr
        Else
            Return ""
        End If
    End Function
End Class


