'Copyright 2006 by AQUA TERRA Consultants - Royalty-free use permitted under open source license
Option Strict Off
Option Explicit On

Imports atcData
Imports atcData.atcDataSource.EnumExistAction
Imports atcUtility
Imports MapWinUtility

Public Class atcWDMfile
    Inherits atcData.atcDataSource

    Private Const pDsnPerDirRec As Int32 = 500
    Private Const pFileFilter As String = "WDM Files (*.wdm)|*.wdm"

    Public Overrides ReadOnly Property Category() As String
        Get
            Return "File"
        End Get
    End Property

    Public Overrides ReadOnly Property Description() As String
        Get
            Return "WDM Time Series VB"
        End Get
    End Property

    Public Overrides ReadOnly Property Name() As String
        Get
            Return "Timeseries::WDM VB"
        End Get
    End Property

    Public Overrides ReadOnly Property CanOpen() As Boolean
        Get
            Return True
        End Get
    End Property

    Public Overrides ReadOnly Property CanSave() As Boolean
        Get
            Return False 'TODO: change when this can save
        End Get
    End Property

    Public Overrides Function Open(ByVal aFileName As String, Optional ByVal aAttributes As atcDataAttributes = Nothing) As Boolean
        If aFileName Is Nothing OrElse aFileName.Length = 0 Then
            Dim cdlg As New Windows.Forms.OpenFileDialog
            With cdlg
                .Title = "Select WDM file to open"
                .FileName = aFileName
                .Filter = pFileFilter
                .CheckFileExists = False
                If .ShowDialog() = Windows.Forms.DialogResult.OK Then
                    aFileName = AbsolutePath(.FileName, CurDir)
                Else 'cancel
                    Logger.Dbg("atcWDMfile:Open:User Cancelled File Dialogue for Open WDM file")
                    Open = False
                End If
            End With
        End If

        Dim lWdmFileHandle As atcWdmFileHandle = Nothing
        If FileExists(aFileName) Then
            lWdmFileHandle = New atcWdmFileHandle(0, aFileName)
        ElseIf FilenameNoPath(aFileName).Length > 0 Then
            Logger.Dbg("atcWDMfile:Open:WDM file " & aFileName & " does not exist - it will be created")
            MkDirPath(PathNameOnly(aFileName))
            lWdmFileHandle = New atcWdmFileHandle(2, aFileName)
        Else
            Logger.Dbg("atcWDMfile:Open:File does not exist and cannot create '" & aFileName & "'")
            Open = False
        End If

        If Not lWdmFileHandle Is Nothing Then
            Specification = aFileName
            Open = True 'assume the best
            'do some basic checks
            Dim lVal As Int32 = ReadWdmInt32(lWdmFileHandle.BinaryReader, Wdm_Fields.PPRBKR)
            If lVal <> -998 Then
                Logger.Dbg("atcWDMfile:Open:PrimaryBackwardRecordPointer for FileDefinitionRecord:" & lVal & " should be -998")
                Open = False
            End If
            Refresh(lWdmFileHandle.BinaryReader)
            lWdmFileHandle.Dispose()
            Return True 'Successfully opened
        Else
            Return False
        End If
    End Function

    Private Sub Refresh(ByVal aBr As IO.BinaryReader)
        Dim lProg As Integer = 0
        Dim lProgPrev As Integer

        DataSets.Clear()

        'pDates = Nothing
        'pDates = New ArrayList

        Dim lDsn As Int32 = ReadWdmInt32(aBr, Wdm_Fields.DSFST_Timeseries)
        While lDsn > 0
            Dim lRec As Int32 = FirstRecordNumberFromDsn(aBr, lDsn)
            Logger.Dbg("Dsn: " & lDsn & " Rec: " & lRec)
            DataSets.Add(lDsn, DataSetFromWdm(aBr, lDsn))
            'RefreshDsn(aBr, lDsn Or lRec)
            lDsn = ReadWdmInt32(aBr, lRec, 2)
            If lDsn = 0 Then
                Logger.Progress("WDM Refresh Complete", 100, 0)
            Else 'try the next dsn
                lRec = FirstRecordNumberFromDsn(aBr, lDsn)
                lProgPrev = lProg
                lProg = (100 * lDsn) / 32000
                Logger.Progress("WDM Refresh", lProg, lProgPrev)
            End If
        End While
    End Sub

    Private Function DataSetFromWdm(ByVal aBr As IO.BinaryReader, ByVal aDsn As Int32) As atcTimeseries
        Dim lDataSet As New atcTimeseries(Me)
        lDataSet.Attributes.Add("DSN", aDsn)
        Return lDataSet
    End Function

    Public Overrides Function ToString() As String
        Dim lBuilder As New Text.StringBuilder
        lBuilder.Append("WDMFileName = " & Specification & vbCrLf)
        'lBuilder.Append("Datasets" & vbCr)
        'For lInd As Integer = 1 To pDsnRecordPointer.GetUpperBound(0)
        'lBuilder.Append(Int32ToStringIfNotZero("Dsn(" & lInd & ") at ", pDsnRecordPointer(lInd)))
        'Next
        lBuilder.Append("Check" & vbCrLf)
        lBuilder.Append(Check(True))
        'Dim lRecordInUse As Int32() = RecordUsageMap()
        'For lInd As Integer = 1 To lRecordInUse.GetUpperBound(0)
        '    lBuilder.Append(Int32ToStringIfNotZero("Rec(" & lInd & ") use ", lRecordInUse(lInd)) & vbCr)
        'Next
        Return lBuilder.ToString
    End Function

    Friend Function Check(ByVal aVerbose As Boolean) As String
        Dim lReport As New Text.StringBuilder
        Dim lWdmFileHandle As New atcWdmFileHandle(1, Specification)
        Dim lBr As IO.BinaryReader = lWdmFileHandle.BinaryReader
        Dim lRecordInUse(ReadWdmInt32(lBr, Wdm_Fields.LSTREC)) As Int32
        Dim lDsnRecordPointer() As Int32

        SetRecordUse(lRecordInUse, 1, -3)

        If aVerbose Then
            lReport.Append(FileDefinitionToString(lBr))
        End If

        'inventory free record chain
        Dim lFreeRec As Int32 = ReadWdmInt32(lBr, Wdm_Fields.FREREC)
        While lFreeRec > 0
            SetRecordUse(lRecordInUse, lFreeRec, -2)
            lFreeRec = ReadWdmInt32(lBr, lFreeRec, 2) 'forward record pointer
        End While

        'check direcory records
        For lPos As Integer = 1 To 400 '.DirPnt.GetUpperBound(0)
            Dim lDirRecord As Integer = ReadWdmInt32(lBr, Wdm_Fields.DirPnt, lPos)
            If lDirRecord > 0 Then 'process this directory
                SetRecordUse(lRecordInUse, lDirRecord, -1)
                ReDim Preserve lDsnRecordPointer(lPos * pDsnPerDirRec)
                For lInd As Integer = 1 To 4
                    If ReadWdmInt32(lBr, lDirRecord, lInd) <> 0 Then
                        lReport.Append("Directory Record " & lDirRecord & " With NonZero Pointer " & lInd & " = " & ReadWdmInt32(lBr, lDirRecord, lInd) & vbCrLf)
                    End If
                Next
                For lInd As Integer = 1 To pDsnPerDirRec
                    lDsnRecordPointer(lInd) = ReadWdmInt32(lBr, lDirRecord, lInd + 4)
                    If lDsnRecordPointer(lInd) <> 0 Then
                        If aVerbose Then
                            lReport.Append("Directory Record " & lDirRecord & " Pointer " & lInd & " = " & lDsnRecordPointer(lInd) & vbCrLf)
                        End If
                        Dim lDsn As Int32 = ((lPos - 1) * 500) + lInd
                        Dim lDsnRec As Int32 = lDsnRecordPointer(lInd)
                        While lDsnRec > 0 'follow dsn record chain
                            SetRecordUse(lRecordInUse, lDsnRec, lDsn)
                            'next forward secondary record pointer
                            lDsnRec = ReadWdmInt32(lBr, lDsnRec, 4)
                        End While
                    End If
                Next
            End If
        Next
        For lInd As Int32 = 1 To ReadWdmInt32(lBr, Wdm_Fields.LSTREC)
            If lRecordInUse(lInd) = 0 Then 'record not in chain
                lReport.Append("Record " & lInd & " Not In a Chain" & vbCrLf)
            ElseIf aVerbose Then
                lReport.Append("Rec(" & lInd & ") use " & lRecordInUse(lInd) & vbCrLf)
            End If
        Next
        lWdmFileHandle.Dispose()

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

    Private Enum Wdm_Fields As Int32
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

    Private Function FileDefinitionToString(ByVal aBr As IO.BinaryReader) As String
        Dim lBuilder As New Text.StringBuilder

        Dim lItems As Array
        lItems = System.Enum.GetValues(GetType(Wdm_Fields))
        Dim lItem As Wdm_Fields
        For Each lItem In lItems
            If lItem <> Wdm_Fields.DirPnt Then
                Dim lValue As Int32 = ReadWdmInt32(aBr, lItem)
                If lValue <> 0 Then
                    Dim lName As String = System.Enum.GetName(GetType(Wdm_Fields), lItem)
                    lBuilder.Append(lName & " " & lValue & vbCrLf)
                End If
            Else
                For lInd As Int32 = 1 To 400
                    Dim lValue As Int32 = ReadWdmInt32(aBr, lItem)
                    If lValue <> 0 Then
                        Dim lName As String = System.Enum.GetName(GetType(Wdm_Fields), lItem) & "(" & lInd & ")"
                        lBuilder.Append(lName & " " & lValue & vbCrLf)
                    End If
                Next
            End If
        Next
        Return lBuilder.ToString
    End Function

    Private Function FirstRecordNumberFromDsn(ByVal aBr As IO.BinaryReader, ByVal aDsn As Int32) As Int32
        Dim lDirOff As Int32 = aDsn Mod pDsnPerDirRec
        If lDirOff = 0 Then
            lDirOff = pDsnPerDirRec
        End If
        Dim lDirRec As Int32 = 1 + ((aDsn - lDirOff) / pDsnPerDirRec)
        Dim lRec As Int32 = ReadWdmInt32(aBr, Wdm_Fields.DirPnt, lDirRec)
        SeekWdm(aBr.BaseStream, lRec, lDirOff + 4)
        Dim lI As Int32 = aBr.ReadInt32
        Return lI
    End Function
    Private Function ReadWdmInt32(ByVal aBr As IO.BinaryReader, ByVal aRec As Int32, ByVal aOff As Int32) As Int32
        'aOff in four byte words
        SeekWdm(aBr.BaseStream, aRec, aOff)
        Dim lI As Int32 = aBr.ReadInt32
        Return lI
    End Function
    Private Function ReadWdmInt32(ByVal aBr As IO.BinaryReader, ByVal aField As Wdm_Fields, Optional ByVal aOff As Int32 = 0) As Int32
        SeekWdm(aBr.BaseStream, 1, aField + aOff)
        Dim lI As Int32 = aBr.ReadInt32
        Return lI
    End Function

    Private Sub SeekWdm(ByVal aFs As IO.FileStream, ByVal aRec As Int32, ByVal aOff As Int32)
        'aOff in four byte words
        Const lReclB As Int32 = 2048 'wdm record size in bytes
        aFs.Seek(((aRec - 1) * lReclB) + ((aOff - 1) * 4), IO.SeekOrigin.Begin)
    End Sub

    Private Class atcWdmFileHandle
        Implements IDisposable

        Dim pBr As IO.BinaryReader

        Public ReadOnly Property BinaryReader() As IO.BinaryReader
            Get
                Return pBr
            End Get
        End Property

        Public Sub New(ByVal aRWCFlg As Integer, ByVal aFileName As String)
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
End Class