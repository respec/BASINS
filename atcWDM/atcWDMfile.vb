'Copyright 2006 by AQUA TERRA Consultants - Royalty-free use permitted under open source license
Option Strict Off
Option Explicit On

Imports atcUtility
Imports MapWinUtility

Friend Class atcWDMfile
    Private pFileName As String
    Private pFileDefinition As clsFileDefinition
    Private pDsnRecordPointer() As Int32
    Private Const pDsnPerDirRec As Int32 = 500

    Public Function OpenFile(ByVal aFilename As String) As Boolean
        pFileName = aFilename
        Dim lWdmFileHandle As New atcWdmFileHandle(1, pFileName, pFileDefinition)
        lWdmFileHandle.Dispose()
        Return True
    End Function

    Public Overrides Function ToString() As String
        Dim lBuilder As New Text.StringBuilder
        lBuilder.Append("WDMFileName = " & pFileName & vbCr)
        lBuilder.Append("FileDefintionRecord" & vbCr)
        lBuilder.Append(pFileDefinition.ToString())
        lBuilder.Append("Datasets" & vbCr)
        'For lInd As Integer = 1 To pDsnRecordPointer.GetUpperBound(0)
        'lBuilder.Append(Int32ToStringIfNotZero("Dsn(" & lInd & ") at ", pDsnRecordPointer(lInd)))
        'Next
        lBuilder.Append("RecordUsage" & vbCr)
        Dim lRecordInUse As Int32() = RecordUsageMap()
        For lInd As Integer = 1 To lRecordInUse.GetUpperBound(0)
            lBuilder.Append(Int32ToStringIfNotZero("Rec(" & lInd & ") use ", lRecordInUse(lInd)) & vbCr)
        Next
        Return lBuilder.ToString
    End Function

    Friend Function RecordUsageMap() As Int32()
        Dim lWdmFileHandle As New atcWdmFileHandle(1, pFileName, pFileDefinition)
        Dim lBr As IO.BinaryReader = lWdmFileHandle.BinaryReader
        Dim lRecordInUse(pFileDefinition.LSTREC) As Int32
        SetRecordUse(lRecordInUse, 1, -3)
        'inventory free record chain
        Dim lFreeRec As Int32 = pFileDefinition.FREREC
        While lFreeRec > 0
            SetRecordUse(lRecordInUse, lFreeRec, -2)
            lFreeRec = ReadWdmInt32(lBR, lFreeRec, 2) 'forward record pointer
        End While
        'check direcory records
        For lPos As Integer = 1 To pFileDefinition.DirPnt.GetUpperBound(0)
            Dim lDirRecord As Integer = pFileDefinition.DirPnt(lPos)
            If lDirRecord > 0 Then 'process this directory
                SetRecordUse(lRecordInUse, lDirRecord, -1)
                ReDim Preserve pDsnRecordPointer(lPos * pDsnPerDirRec)
                For lInd As Integer = 1 To 4
                    If ReadWdmInt32(lBR, lDirRecord, lInd) <> 0 Then
                        Throw New Exception("Directory Record " & lDirRecord & " With NonZero Pointers at" & lInd)
                    End If
                Next
                For lInd As Integer = 1 To pDsnPerDirRec
                    pDsnRecordPointer(lInd) = ReadWdmInt32(lBR, lDirRecord, lInd + 4)
                    If pDsnRecordPointer(lInd) <> 0 Then
                        Dim lDsn As Int32 = ((lPos - 1) * 500) + lInd
                        Dim lDsnRec As Int32 = pDsnRecordPointer(lInd)
                        While lDsnRec > 0 'follow dsn record chain
                            SetRecordUse(lRecordInUse, lDsnRec, lDsn)
                            'next forward secondary record pointer
                            lDsnRec = ReadWdmInt32(lBR, lDsnRec, 4)
                        End While
                    End If
                Next
            End If
        Next
        For lInd As Int32 = 1 To pFileDefinition.LSTREC
            If lRecordInUse(lInd) = 0 Then 'record not in chain
                Throw New Exception("Record " & lInd & " Not In a Chain")
            End If
        Next
        lWdmFileHandle.Dispose()
        Return lRecordInUse
    End Function

    Private Sub SetRecordUse(ByRef aRecordInUse() As Int32, ByVal aRecord As Int32, ByVal aUse As Int32)
        If aRecordInUse(aRecord) = 0 Then 'not in use
            aRecordInUse(aRecord) = aUse
        Else
            Throw New Exception("WDM record " & aRecord & _
                                " already in use with code " & aRecordInUse(aRecord) & _
                                " can not use with new code " & aUse)
        End If
    End Sub

    Private Class clsFileDefinition
        Dim PPRBKR As Int32 ' 1 Primary backward record pointer   (always -998, was -999)
        Dim PPRFWR As Int32 ' 2 Primary forward record pointer    (always 0)
        Dim PSCBKR As Int32 ' 3 Secondary backward record pointer (always 0)
        Dim PSCFWR As Int32 ' 4 Secondary forward record pointer  (always 0)
        Dim Unused05_28(24) As Byte ' 5 - 28 
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
        Dim UnusedB(63) As Int32 '50 - 112 
        Friend DirPnt(400) As Int32 '113- 512 

        Friend Sub New(ByVal aBR As IO.BinaryReader) 'could use handle here
            PPRBKR = aBR.ReadInt32() ' Primary backward record pointer   (always -998, was -999)
            'TODO: exception if not -998
            PPRFWR = aBR.ReadInt32() ' Primary forward record pointer    (always 0)
            PSCBKR = aBR.ReadInt32() ' Secondary backward record pointer (always 0)
            PSCFWR = aBR.ReadInt32() ' Secondary forward record pointer  (always 0)
            For lPos As Integer = 1 To Unused05_28.GetUpperBound(0)
                Unused05_28(lPos) = aBR.ReadInt32  ' 5 - 28 
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
            For lPos As Integer = 1 To UnusedB.GetUpperBound(0)
                UnusedB(lPos) = aBR.ReadInt32  '50 - 112 
            Next
            For lPos As Integer = 1 To DirPnt.GetUpperBound(0)
                DirPnt(lPos) = aBR.ReadInt32  '113 - 512 
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
                For lIndex = 1 To .Unused05_28.GetUpperBound(0)
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
                For lIndex = 1 To .UnusedB.GetUpperBound(0)
                    lBuilder.Append(Int32ToStringIfNotZero("  Unused50_112(" & lIndex + 50 & ") = ", .UnusedB(lIndex) & vbCrLf))
                Next
                For lIndex = 1 To .DirPnt.GetUpperBound(0)
                    lBuilder.Append(Int32ToStringIfNotZero("  DirPnt(" & lIndex + 1 & ") = ", .DirPnt(lIndex)))
                Next
            End With
            Return lBuilder.ToString
        End Function
    End Class

    Private Class atcWdmFileHandle
        Implements IDisposable

        Dim pBr As IO.BinaryReader

        Public ReadOnly Property BinaryReader() As IO.BinaryReader
            Get
                Return pBr
            End Get
        End Property

        Public Sub New(ByVal aRWCFlg As Integer, ByVal aFileName As String, ByRef aFileDefinition As clsFileDefinition)
            'aRWCFlg: 0- normal open of existing WDM file,
            '         1- open WDM file as read only,
            '         2- open new WDM file
            Dim lAttr As FileAttribute
            Dim lFileName As String
            Dim lFS As IO.FileStream

            lFileName = AbsolutePath(aFileName, CurDir())

            If Not FileExists(lFileName) AndAlso aRWCFlg <> 2 Then
                Throw New ApplicationException("atcWdmFileHandle:Could not find " & aFileName)
            Else
                If aRWCFlg = 0 Then
                    lAttr = GetAttr(lFileName) 'if read only, change to not read only
                    If (lAttr And FileAttribute.ReadOnly) <> 0 Then
                        lAttr = lAttr - FileAttribute.ReadOnly
                        SetAttr(lFileName, lAttr)
                    End If
                End If

                Logger.Dbg("atcWdmFileHandle:OpenB4:" & lFileName)
                Try
                    lFS = IO.File.Open(lFileName, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.Read)
                    pBr = New IO.BinaryReader(lFS)
                    aFileDefinition = New clsFileDefinition(pBr)
                    Logger.Dbg("atcWdmFileHandle:OpenAft")
                Catch ex As Exception
                    Throw New ApplicationException("atcWdmFileHandle:Exception:" & vbCrLf & ex.ToString)
                End Try
            End If
        End Sub

        Public Sub Dispose() Implements System.IDisposable.Dispose
            Logger.Dbg("atcWdmFileHandle:Dispose:")
            pBr.Close()
            GC.SuppressFinalize(Me)
        End Sub

        Protected Overrides Sub Finalize()
            Dispose()
        End Sub
    End Class

    Private Function ReadWdmInt32(ByVal aBr As IO.BinaryReader, ByRef aRec As Int32, ByRef aOff As Int32) As Int32
        'aOff in four byte words
        SeekWdm(aBr.BaseStream, aRec, aOff)
        Dim lI As Int32 = aBr.ReadInt32
        Return lI
    End Function
    Private Sub SeekWdm(ByVal aFs As IO.FileStream, ByVal aRec As Int32, ByVal aOff As Int32)
        'aOff in four byte words
        Const lReclB As Int32 = 2048 'wdm record size in bytes
        aFs.Seek(((aRec - 1) * lReclB) + ((aOff - 1) * 4), IO.SeekOrigin.Begin)
    End Sub

    Private Shared Function Int32ToStringIfNotZero(ByVal aTitle As String, ByVal aValue As Int32) As String
        If aValue <> 0 Then
            Return aTitle & aValue & vbCr
        Else
            Return ""
        End If
    End Function
End Class