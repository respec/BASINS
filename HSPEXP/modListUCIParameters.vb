Imports atcUtility
Imports atcUCI
Imports System.Collections.Specialized

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
        ParameterList = My.Computer.FileSystem.OpenTextFileWriter(lOutputFolder & "\" & ModelName & "_RCHRES_ParameterList.txt", False)
        ParameterList.WriteLine("Parameter list for " & ModelName)
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

    Sub ListReachParametersForAllUCIFiles(ByVal lFolderName As String)
        lFolderName = "C:\Dropbox (RESPEC)\Basins\data\DO_TMDL_Research\UCI_Files"
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

            End Try

            ListReachParameters(pUci, lFolderName)

        Next

    End Sub

End Module
