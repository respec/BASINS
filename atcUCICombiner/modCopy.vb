Imports MapWinUtility
Imports atcdata


Module modData
    Public Function CopyDataSet(ByVal aDataManager As atcDataManager, _
                                ByVal aSourceDataSet As atcDataSet, _
                                ByVal aTargetType As String, _
                                ByVal aTargetSpecification As String, _
                                ByVal aTargetId As Integer, _
                                Optional ByVal aExistAction As atcData.atcDataSource.EnumExistAction = atcData.atcDataSource.EnumExistAction.ExistReplace) As Boolean

        'just change id in the copy
        aSourceDataSet.Attributes.SetValue("id", aTargetId)
        Dim lTargetDataSource As New atcWDM.atcDataSourceWDM

        If lTargetDataSource.Open(aTargetSpecification) Then
            Dim lResult As Boolean = lTargetDataSource.AddDataset(aSourceDataSet, aExistAction)
            Logger.Dbg("CopyDataSet:Add " & aSourceDataSet.ToString & " to " & aTargetSpecification & " as ID " & aTargetId & " Result:" & lResult)
            Return lResult
        Else
            Logger.Dbg("CopyDataSet:FailedToOpenTarget " & aTargetSpecification)
            Return False
        End If

    End Function

    Public Function CopyDataSet(ByVal aDataManager As atcDataManager, _
                                ByVal aSourceType As String, _
                                ByVal aSourceSpecification As String, _
                                ByVal aSourceId As Integer, _
                                ByVal aTargetType As String, _
                                ByVal aTargetSpecification As String, _
                                ByVal aTargetId As Integer, _
                                Optional ByVal aExistAction As atcData.atcDataSource.EnumExistAction = atcData.atcDataSource.EnumExistAction.ExistReplace) As Boolean

        'Select Case aSourceType
        '    Case "wdm"
        '        Dim lSourceDataSource As New atcWDM.atcDataSourceWDM
        '    Case Else
        '        Dim lsourcedatasource As New atcData.atcDataSource
        'End Select
        Dim lSourceDataSource As New atcWDM.atcDataSourceWDM
        If aDataManager.OpenDataSource((lSourceDataSource), _
                                    aSourceSpecification, Nothing) Then
            Dim lSourceDataSet As atcDataSet = lSourceDataSource.DataSets(lSourceDataSource.DataSets.IndexFromKey(aSourceId))
            If lSourceDataSet Is Nothing Then
                Logger.Dbg("CopyDataSet:FailedToOpenSourceId " & aSourceId & " from " & aSourceSpecification)
                Return False
            Else
                Return CopyDataSet(aDataManager, lSourceDataSet, aTargetType, aTargetSpecification, aTargetId, aExistAction)
            End If
        Else
            Logger.Dbg("CopyDataSet:FailedToOpenSource " & aSourceSpecification)
            Return False
        End If
    End Function
End Module
