Imports MapWinUtility
Imports atcdata


Module modData
    Public Function CopyDataSet(ByVal aSourceDataSet As atcDataSet, _
                                ByVal aTargetSpecification As String, _
                                ByVal aTargetId As Integer, _
                                Optional ByVal aExistAction As atcData.atcDataSource.EnumExistAction = atcData.atcDataSource.EnumExistAction.ExistReplace) As Boolean

        'just change id in the copy
        aSourceDataSet.Attributes.SetValue("id", aTargetId)

        Dim lTargetDataSource As New atcDataSource
        If lTargetDataSource.Open(aTargetSpecification) Then
            Dim lResult As Boolean = lTargetDataSource.AddDataSet(aSourceDataSet, aExistAction)
            Logger.Dbg("CopyDataSet:Add " & aSourceDataSet.ToString & " to " & aTargetSpecification & " as ID " & aTargetId & " Result:" & lResult)
            Return lResult
        Else
            Logger.Dbg("CopyDataSet:FailedToOpenTarget " & aTargetSpecification)
            Return False
        End If

    End Function

    Public Function CopyDataSet(ByVal aSourceSpecification As String, _
                                ByVal aSourceId As Integer, _
                                ByVal aTargetSpecification As String, _
                                ByVal aTargetId As Integer, _
                                Optional ByVal aExistAction As atcData.atcDataSource.EnumExistAction = atcData.atcDataSource.EnumExistAction.ExistReplace) As Boolean

        Dim lSourceDataSource As New atcDataSource
        If lSourceDataSource.Open(aSourceSpecification) Then
            Dim lSourceDataSet As atcDataSet = lSourceDataSource.DataSets(lSourceDataSource.DataSets.IndexFromKey(aSourceId))
            If lSourceDataSet Is Nothing Then
                Logger.Dbg("CopyDataSet:FailedToOpenSourceId " & aSourceId & " from " & aSourceSpecification)
                Return False
            Else
                Return CopyDataSet(lSourceDataSet, aTargetSpecification, aTargetId, aExistAction)
            End If
        Else
            Logger.Dbg("CopyDataSet:FailedToOpenSource " & aSourceSpecification)
            Return False
        End If
    End Function
End Module
