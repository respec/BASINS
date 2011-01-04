'Copyright 2006 by AQUA TERRA Consultants - Royalty-free use permitted under open source license
Option Strict Off
Option Explicit On

Imports atcUtility
Imports MapWinUtility

Friend Class atcWdmFileHandle
    Implements IDisposable

    Private pBr As IO.BinaryReader
    Private Const pDsnPerDirRec As Int32 = 500

    Public Enum Wdm_Fields As Int32
        PPRBKR = 1 'Primary backward record pointer   (always -998, was -999)
        PPRFWR = 2 'Primary forward record pointer    (always 0)
        PSCBKR = 3 'Secondary backward record pointer (always 0)
        PSCFWR = 4 'Secondary forward record pointer  (always 0)
        'TODO - be sure 5-28 are 0 ???
        LSTREC = 29 'Last 2048 Byte Record 
        FREREC = 31 'First Free Recode
        DSCNT_Timeseries = 32
        DSFST_Timeseries = 33
        DSCNT_Table = 34
        DSFST_Table = 35
        DSCNT_Schematic = 36
        DSFST_Schematic = 37
        DSCNT_ProjectDescription = 38
        DSFST_ProjectDescription = 39
        DSCNT_Vector = 40
        DSFST_Vector = 41
        DSCNT_Raster = 42
        DSFST_Raster = 43
        DSCNT_SpaceTime = 44
        DSFST_SpaceTime = 45
        DSCNT_Attribute = 46
        DSFST_Attribute = 47
        DSCNT_Message = 48
        DSFST_Message = 49
        'TODO - be sure 50-112 are 0 ???
        DirPnt = 112
    End Enum

    Public Sub New(ByVal aRWCFlg As Integer, ByVal aFileName As String)
        'aRWCFlg: 0- normal open of existing WDM file,
        '         1- open WDM file as read only,
        '         2- open new WDM file
        Dim lAttr As FileAttribute
        Dim lFileName As String
        Dim lFileAccess As IO.FileAccess
        Dim lFS As IO.FileStream

        lFileName = AbsolutePath(aFileName, CurDir())

        If Not FileExists(lFileName) AndAlso aRWCFlg <> 2 Then
            Throw New ApplicationException("atcWdmFileHandle:Could not find " & aFileName)
        Else
            If aRWCFlg = 2 Then 'create an empty wdm file
                'TODO: create a wdm file
                lFileAccess = IO.FileAccess.ReadWrite
            ElseIf aRWCFlg = 1 Then 'read only
                lFileAccess = IO.FileAccess.Read
            ElseIf aRWCFlg = 0 Then
                lAttr = GetAttr(lFileName) 'if read only, change to not read only
                If (lAttr And FileAttribute.ReadOnly) <> 0 Then
                    lAttr = lAttr - FileAttribute.ReadOnly
                    SetAttr(lFileName, lAttr)
                End If
                lFileAccess = IO.FileAccess.ReadWrite
            End If

            'Logger.Dbg("atcWdmFileHandle:OpenB4:" & lFileName)
            Try
                lFS = IO.File.Open(lFileName, IO.FileMode.Open, lFileAccess, IO.FileShare.Read)
                pBr = New IO.BinaryReader(lFS)
                'TODO: writer?
                'Logger.Dbg("atcWdmFileHandle:OpenAft:" & lFileAccess)
            Catch ex As Exception
                Throw New ApplicationException("atcWdmFileHandle:Exception:" & vbCrLf & ex.ToString)
            End Try
        End If
    End Sub

    Public Sub New(ByVal aBinaryReader As IO.BinaryReader)
        pBr = aBinaryReader
    End Sub

    Public Sub Dispose() Implements System.IDisposable.Dispose
        'Logger.Dbg("atcWdmFileHandle:Dispose:")
        pBr.Close()
        GC.SuppressFinalize(Me)
    End Sub

    Protected Overrides Sub Finalize()
        Dispose()
    End Sub

    Public Function FirstRecordNumberFromDsn(ByVal aDsn As Int32) As Int32
        Dim lDirOff As Int32 = aDsn Mod pDsnPerDirRec
        If lDirOff = 0 Then
            lDirOff = pDsnPerDirRec
        End If
        Dim lDirRec As Int32 = 1 + ((aDsn - lDirOff) / pDsnPerDirRec)
        Dim lRec As Int32 = ReadInt32(Wdm_Fields.DirPnt, lDirRec)
        Me.Seek(lRec, lDirOff + 4)
        Dim lI As Int32 = pBr.ReadInt32
        Return lI
    End Function

    Public Function ReadUInt32() As UInt32
        Return pBr.ReadUInt32
    End Function

    Public Function ReadUInt16() As UInt16
        Return pBr.ReadUInt16
    End Function

    Public Function ReadInt32() As Int32
        Return pBr.ReadInt32
    End Function

    Public Function ReadInt32(ByVal aRec As Int32, ByVal aOff As Int32) As Int32
        'aOff in four byte words
        Me.Seek(aRec, aOff)
        Return pBr.ReadInt32
    End Function

    Public Function ReadInt32(ByVal aField As Wdm_Fields, Optional ByVal aOff As Int32 = 0) As Int32
        Me.Seek(1, aField + aOff)
        Return pBr.ReadInt32
    End Function

    Public Function ReadSingle() As Single
        Return pBr.ReadSingle
    End Function

    Public Function ReadByte() As Byte
        Return pBr.ReadByte
    End Function

    Public Function ReadString(ByVal aNumWords As Integer) As String 'aNumWords = number of 32-bit words
        ReadString = ""
        For lChrIndex As Integer = 1 To aNumwords
            ReadString &= Long2String(pBr.ReadInt32)
        Next
        ReadString = Trim(ReadString)
    End Function

    Public Sub Seek(ByVal aRec As Int32, ByVal aOff As Int32)
        'aOff in four byte words
        Const lReclB As Int32 = 2048 'wdm record size in bytes
        Dim lPos As Int32 = ((aRec - 1) * lReclB) + ((aOff - 1) * 4)
        pBr.BaseStream.Seek(lPos, IO.SeekOrigin.Begin)
        'Logger.Dbg(".Seek(" & lPos & ")")
    End Sub

    Friend Function Check(ByVal aVerbose As Boolean) As String
        Dim lReport As New Text.StringBuilder
        Dim lRecordInUse(Me.ReadInt32(Wdm_Fields.LSTREC)) As Int32
        Dim lDsnRecordPointer() As Int32

        SetRecordUse(lRecordInUse, 1, -3)

        If aVerbose Then
            lReport.Append(FileDefinitionToString(pBr))
        End If

        'inventory free record chain
        Dim lFreeRec As Int32 = Me.ReadInt32(Wdm_Fields.FREREC)
        While lFreeRec > 0
            SetRecordUse(lRecordInUse, lFreeRec, -2)
            lFreeRec = Me.ReadInt32(lFreeRec, 2) 'forward record pointer
        End While

        'check direcory records
        For lPos As Integer = 1 To 400 '.DirPnt.GetUpperBound(0)
            Dim lDirRecord As Integer = Me.ReadInt32(Wdm_Fields.DirPnt, lPos)
            If lDirRecord > 0 Then 'process this directory
                SetRecordUse(lRecordInUse, lDirRecord, -1)
                ReDim Preserve lDsnRecordPointer(lPos * pDsnPerDirRec)
                For lInd As Integer = 1 To 4
                    If Me.ReadInt32(lDirRecord, lInd) <> 0 Then
                        lReport.Append("Directory Record " & lDirRecord & " With NonZero Pointer " & lInd & " = " & Me.ReadInt32(lDirRecord, lInd) & vbCrLf)
                    End If
                Next
                For lInd As Integer = 1 To pDsnPerDirRec
                    lDsnRecordPointer(lInd) = Me.ReadInt32(lDirRecord, lInd + 4)
                    If lDsnRecordPointer(lInd) <> 0 Then
                        If aVerbose Then
                            lReport.Append("Directory Record " & lDirRecord & " Pointer " & lInd & " = " & lDsnRecordPointer(lInd) & vbCrLf)
                        End If
                        Dim lDsn As Int32 = ((lPos - 1) * 500) + lInd
                        Dim lDsnRec As Int32 = lDsnRecordPointer(lInd)
                        While lDsnRec > 0 'follow dsn record chain
                            SetRecordUse(lRecordInUse, lDsnRec, lDsn)
                            'next forward secondary record pointer
                            lDsnRec = Me.ReadInt32(lDsnRec, 4)
                        End While
                    End If
                Next
            End If
        Next
        For lInd As Int32 = 1 To Me.ReadInt32(Wdm_Fields.LSTREC)
            If lRecordInUse(lInd) = 0 Then 'record not in chain
                lReport.Append("Record " & lInd & " Not In a Chain" & vbCrLf)
            ElseIf aVerbose Then
                lReport.Append("Rec(" & lInd & ") use " & lRecordInUse(lInd) & vbCrLf)
            End If
        Next

        If Not aVerbose AndAlso lReport.Length > 0 Then
            Throw New Exception(lReport.ToString)
        End If

        Return lReport.ToString
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

    Private Function FileDefinitionToString(ByVal aBr As IO.BinaryReader) As String
        Dim lBuilder As New Text.StringBuilder

        Dim lItems As Array
        lItems = System.Enum.GetValues(GetType(Wdm_Fields))
        Dim lItem As Wdm_Fields
        For Each lItem In lItems
            If lItem <> Wdm_Fields.DirPnt Then
                Dim lValue As Int32 = Me.ReadInt32(lItem)
                If lValue <> 0 Then
                    Dim lName As String = System.Enum.GetName(GetType(Wdm_Fields), lItem)
                    lBuilder.Append(lName & " " & lValue & vbCrLf)
                End If
            Else
                For lInd As Int32 = 1 To 400
                    Dim lValue As Int32 = Me.ReadInt32(lItem)
                    If lValue <> 0 Then
                        Dim lName As String = System.Enum.GetName(GetType(Wdm_Fields), lItem) & "(" & lInd & ")"
                        lBuilder.Append(lName & " " & lValue & vbCrLf)
                    End If
                Next
            End If
        Next
        Return lBuilder.ToString
    End Function
End Class
