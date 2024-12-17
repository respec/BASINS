Imports atcUtility
Imports atcUCI
Imports System.Collections.Specialized
Imports MapWinUtility

Public Module modListUCIParameters
    Private pPERLND As atcCollection
    Private pIMPLND As atcCollection
    Private pRCHRES As atcCollection
    Private pBlockDefs As HspfBlockDefs
    Sub ListReachParameters(ByVal aHSPFUCI As HspfUci, ByVal aOutputFolder As String)
        pPERLND = New atcCollection
        pIMPLND = New atcCollection
        pRCHRES = New atcCollection

        Dim lModelName As String = IO.Path.GetFileNameWithoutExtension(aHSPFUCI.Name)
        Dim lParameterList As System.IO.StreamWriter
        Dim lNumberOfReaches As Integer = 0
        lParameterList = My.Computer.FileSystem.OpenTextFileWriter(aOutputFolder & "\" & lModelName & "_RCHRES_ParameterList.txt", False)
        lParameterList.WriteLine("Parameter list for " & lModelName)

        For Each lOperation As HspfOperation In aHSPFUCI.OpnSeqBlock.Opns
            If lOperation.Name = "RCHRES" AndAlso Not pRCHRES.Contains(lOperation.Id) Then
                pRCHRES.Add(lOperation.Id)
                lNumberOfReaches += 1
            End If
        Next

        lParameterList.WriteLine("Number Of Reaches in the UCI File = " & lNumberOfReaches)
        lParameterList.WriteLine("OperationType, OperationID, TableName, TableOccurrence, PrameterName, ParameterValue")

        For Each lOperation As HspfOperation In aHSPFUCI.OpnSeqBlock.Opns
            If lOperation.Name = "RCHRES" Then
                For Each lTable As HspfTable In lOperation.Tables
                    For Each lparm As HspfParm In lTable.Parms
                        lParameterList.WriteLine(lOperation.Name & ", " & lOperation.Id & ", " & lTable.Name & ", " &
                                                lTable.OccurIndex & ", " & lparm.Name & ", " & lparm.Value)
                    Next
                Next
            End If
        Next

        lParameterList.Close()

    End Sub

    Sub ListPERLNDParameters(ByVal aHSPFUCI As HspfUci, ByVal aOutputFolder As String)
        pPERLND = New atcCollection
        pIMPLND = New atcCollection
        pRCHRES = New atcCollection

        Dim lModelName As String = IO.Path.GetFileNameWithoutExtension(aHSPFUCI.Name)
        Dim lParameterList As System.IO.StreamWriter
        Dim lNumberOfPERLNDOperations As Integer = 0
        lParameterList = My.Computer.FileSystem.OpenTextFileWriter(aOutputFolder & "\" & lModelName & "_PERLND_ParameterList.txt", False)
        lParameterList.WriteLine("Parameter list for " & lModelName)

        For Each lOperation As HspfOperation In aHSPFUCI.OpnSeqBlock.Opns
            If lOperation.Name = "PERLND" AndAlso Not pRCHRES.Contains(lOperation.Id) Then
                pRCHRES.Add(lOperation.Id)
                lNumberOfPERLNDOperations += 1
            End If
        Next

        lParameterList.WriteLine("Number Of PERLND Operations in the UCI File = " & lNumberOfPERLNDOperations)
        lParameterList.WriteLine("OperationType, OperationID, TableName, Table Occurrence Number, Table Occurrence Count, ParameterName, ParameterValue")

        For Each lOperation As HspfOperation In aHSPFUCI.OpnSeqBlock.Opns
            If lOperation.Name = "PERLND" Then
                For Each lTable As HspfTable In lOperation.Tables
                    For Each lParm As HspfParm In lTable.Parms
                        'If lparm.Value.Contains("ORTHO") Then Stop
                        'If lTable.OccurIndex > 1 Then Stop
                        lParameterList.WriteLine(lOperation.Name & ", " & lOperation.Id & ", " &
                                                lTable.Name & "," & lTable.OccurNum & ", " & lTable.OccurCount & ", " & lParm.Name & ", " & lParm.Value)
                    Next
                Next
            End If
        Next

        lParameterList.Close()

    End Sub

    Sub ListIMPLNDParameters(ByVal aHSPFUCI As HspfUci, ByVal aOutputFolder As String)
        pPERLND = New atcCollection
        pIMPLND = New atcCollection
        pRCHRES = New atcCollection

        Dim lModelName As String = IO.Path.GetFileNameWithoutExtension(aHSPFUCI.Name)
        Dim lParameterList As System.IO.StreamWriter
        Dim lNumberOfReaches As Integer = 0
        lParameterList = My.Computer.FileSystem.OpenTextFileWriter(aOutputFolder & "\" & lModelName & "_IMPLND_ParameterList.txt", False)
        lParameterList.WriteLine("Parameter list for " & lModelName)

        For Each lOperation As HspfOperation In aHSPFUCI.OpnSeqBlock.Opns
            If lOperation.Name = "IMPLND" AndAlso Not pRCHRES.Contains(lOperation.Id) Then
                pRCHRES.Add(lOperation.Id)
                lNumberOfReaches += 1
            End If
        Next

        lParameterList.WriteLine("Number Of IMPLND Operations in the UCI File = " & lNumberOfReaches)
        lParameterList.WriteLine("OperationType, OperationID, TableName, Table Occurrence Number, Table Occurrence Count, ParameterName, ParameterValue")

        For Each lOperation As HspfOperation In aHSPFUCI.OpnSeqBlock.Opns
            If lOperation.Name = "IMPLND" Then
                For Each lTable As HspfTable In lOperation.Tables
                    For Each lparm As HspfParm In lTable.Parms
                        lParameterList.WriteLine(lOperation.Name & ", " & lOperation.Id & ", " &
                                                lTable.Name & "," & lTable.OccurNum & ", " & lTable.OccurCount & ", " & lparm.Name & ", " & lparm.Value)
                    Next
                Next
            End If
        Next

        lParameterList.Close()

    End Sub

    Sub ListReachParametersForAllUCIFiles(ByVal lFolderName As String)
        'lFolderName = "C:\Dev\Upatoi_FortBenning\FB_Model_Ext_2012"
        Dim lUCIFileNames As New NameValueCollection
        AddFilesInDir(lUCIFileNames, lFolderName, False, "*.uci")
        Dim pUci As atcUCI.HspfUci
        Dim pHspfMsg As atcUCI.HspfMsg
        atcWDM.atcDataSourceWDM.HSPFMsgFilename = "C:\BASINS45\models\HSPF\bin\hspfmsg.wdm"
        For Each lUCIFile As String In lUCIFileNames
            pUci = New atcUCI.HspfUci
            pHspfMsg = New atcUCI.HspfMsg
            pHspfMsg.Open(atcWDM.atcDataSourceWDM.HSPFMsgFilename)
            Try
                pUci.FastReadUciForStarter(pHspfMsg, lUCIFile)
            Catch ex As Exception
                Logger.Msg(ex.ToString, MsgBoxStyle.Critical, "UCI Reading Issue!")
            End Try

            ListReachParameters(pUci, lFolderName)
            ListPERLNDParameters(pUci, lFolderName)
            ListIMPLNDParameters(pUci, lFolderName)

        Next

    End Sub

End Module
