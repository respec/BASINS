Imports atcUtility
Imports atcUCI
Imports System.Collections.Specialized
Imports MapWinUtility

Public Module modListUCIParameters
    Private pPERLND As atcCollection
    Private pIMPLND As atcCollection
    Private pRCHRES As atcCollection
    Private pBlockDefs As HspfBlockDefs
    Sub ListReachParameters(ByVal aHSPFUCI As HspfUci, ByVal lOutputFolder As String)
        pPERLND = New atcCollection
        pIMPLND = New atcCollection
        pRCHRES = New atcCollection
        Dim s As String = ""
        Dim ModelName As String = IO.Path.GetFileNameWithoutExtension(aHSPFUCI.Name)
        Dim ParameterList As System.IO.StreamWriter
        Dim NumberOfReaches As Integer = 0
        ParameterList = My.Computer.FileSystem.OpenTextFileWriter(lOutputFolder & "\" & ModelName & "_RCHRES_ParameterList.txt", False)
        ParameterList.WriteLine("Parameter list for " & ModelName)

        For Each lOperation As HspfOperation In aHSPFUCI.OpnSeqBlock.Opns
            If lOperation.Name = "RCHRES" AndAlso Not pRCHRES.Contains(lOperation.Id) Then
                pRCHRES.Add(lOperation.Id)
                NumberOfReaches += 1
            End If
        Next

        ParameterList.WriteLine("Number Of Reaches in the UCI File = " & NumberOfReaches)
        ParameterList.WriteLine("OperationType, OperationID, TableName, PrameterName, ParameterValue")

        For Each lOperation As HspfOperation In aHSPFUCI.OpnSeqBlock.Opns
            If lOperation.Name = "RCHRES" Then
                For Each lTable As HspfTable In lOperation.Tables
                    For Each lparm As HspfParm In lTable.Parms
                        ParameterList.WriteLine(lOperation.Name & ", " & lOperation.Id & ", " & lTable.Name & ", " & lparm.Name & ", " & lparm.Value)
                    Next
                Next
            End If
        Next

        ParameterList.Close()


    End Sub
    Sub ListPERLNDParameters(ByVal aHSPFUCI As HspfUci, ByVal lOutputFolder As String)
        pPERLND = New atcCollection
        pIMPLND = New atcCollection
        pRCHRES = New atcCollection
        Dim s As String = ""
        Dim ModelName As String = IO.Path.GetFileNameWithoutExtension(aHSPFUCI.Name)
        Dim ParameterList As System.IO.StreamWriter
        Dim NumberOfPERLNDOperations As Integer = 0
        ParameterList = My.Computer.FileSystem.OpenTextFileWriter(lOutputFolder & "\" & ModelName & "_PERLND_ParameterList.txt", False)
        ParameterList.WriteLine("Parameter list for " & ModelName)

        For Each lOperation As HspfOperation In aHSPFUCI.OpnSeqBlock.Opns
            If lOperation.Name = "PERLND" AndAlso Not pRCHRES.Contains(lOperation.Id) Then
                pRCHRES.Add(lOperation.Id)
                NumberOfPERLNDOperations += 1
            End If
        Next

        ParameterList.WriteLine("Number Of PERLND Operations in the UCI File = " & NumberOfPERLNDOperations)
        ParameterList.WriteLine("OperationType, OperationID, TableName, PrameterName, Table Occurrence Index, ParameterValue")

        For Each lOperation As HspfOperation In aHSPFUCI.OpnSeqBlock.Opns
            If lOperation.Name = "PERLND" Then
                For Each lTable As HspfTable In lOperation.Tables
                    For Each lparm As HspfParm In lTable.Parms
                        'If lTable.OccurIndex > 1 Then Stop
                        ParameterList.WriteLine(lOperation.Name & ", " & lOperation.Id & ", " & lTable.Name & "," & lTable.OccurIndex & ", " & lparm.Name & ", " & lparm.Value)
                    Next
                Next
            End If
        Next

        ParameterList.Close()


    End Sub
    Sub ListIMPLNDParameters(ByVal aHSPFUCI As HspfUci, ByVal lOutputFolder As String)
        pPERLND = New atcCollection
        pIMPLND = New atcCollection
        pRCHRES = New atcCollection
        Dim s As String = ""
        Dim ModelName As String = IO.Path.GetFileNameWithoutExtension(aHSPFUCI.Name)
        Dim ParameterList As System.IO.StreamWriter
        Dim NumberOfReaches As Integer = 0
        ParameterList = My.Computer.FileSystem.OpenTextFileWriter(lOutputFolder & "\" & ModelName & "_IMPLND_ParameterList.txt", False)
        ParameterList.WriteLine("Parameter list for " & ModelName)

        For Each lOperation As HspfOperation In aHSPFUCI.OpnSeqBlock.Opns
            If lOperation.Name = "IMPLND" AndAlso Not pRCHRES.Contains(lOperation.Id) Then
                pRCHRES.Add(lOperation.Id)
                NumberOfReaches += 1
            End If
        Next

        ParameterList.WriteLine("Number Of IMPLND Operations in the UCI File = " & NumberOfReaches)
        ParameterList.WriteLine("OperationType, OperationID, TableName, PrameterName, ParameterValue")

        For Each lOperation As HspfOperation In aHSPFUCI.OpnSeqBlock.Opns
            If lOperation.Name = "IMPLND" Then
                For Each lTable As HspfTable In lOperation.Tables
                    For Each lparm As HspfParm In lTable.Parms
                        ParameterList.WriteLine(lOperation.Name & ", " & lOperation.Id & ", " & lTable.Name & "," & lTable.OccurIndex & ", " & lparm.Name & ", " & lparm.Value)
                    Next
                Next
            End If
        Next

        ParameterList.Close()


    End Sub

    Sub ListReachParametersForAllUCIFiles(ByVal lFolderName As String)
        lFolderName = "C:\Dropbox (RESPEC)\Basins\data\DO_TMDL_Research\UCI_Files\CTWM_UCIs"
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

            'ListReachParameters(pUci, lFolderName)
            ListPERLNDParameters(pUci, lFolderName)
            ListIMPLNDParameters(pUci, lFolderName)

        Next

    End Sub

End Module
