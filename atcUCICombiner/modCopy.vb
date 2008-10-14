Imports MapWinUtility
Imports atcdata

Module modData
    Public Function CopyDataSet(ByVal aSourceDataSet As atcDataSet, _
                                ByVal aTargetSource As atcDataSource, _
                                ByVal aTargetId As Integer, _
                                Optional ByVal aExistAction As atcData.atcDataSource.EnumExistAction = atcData.atcDataSource.EnumExistAction.ExistReplace) As Boolean
        'just change id in the copy
        aSourceDataSet.Attributes.SetValue("id", aTargetId)

        Dim lResult As Boolean = aTargetSource.AddDataSet(aSourceDataSet, aExistAction)

        Logger.Dbg("CopyDataSet:Add " & aSourceDataSet.ToString & " as ID " & aTargetId & " Result:" & lResult)
        Return lResult
    End Function

    Public Function CopyDataSet(ByVal aSourceDataSet As atcDataSet, _
                                ByVal aTargetType As String, _
                                ByVal aTargetSpecification As String, _
                                ByVal aTargetId As Integer, _
                                Optional ByVal aExistAction As atcData.atcDataSource.EnumExistAction = atcData.atcDataSource.EnumExistAction.ExistReplace) As Boolean

        Dim lTargetDataSource As New atcWDM.atcDataSourceWDM

        If lTargetDataSource.Open(aTargetSpecification) Then
            Return CopyDataSet(aSourceDataSet, lTargetDataSource, aTargetId, aExistAction)
        Else
            Logger.Dbg("CopyDataSet:FailedToOpenTarget " & aTargetSpecification)
            Return False
        End If

    End Function

    Public Function CopyDataSet(ByVal aSourceType As String, _
                                ByVal aSourceSpecification As String, _
                                ByVal aSourceId As Integer, _
                                ByVal aTargetType As String, _
                                ByVal aTargetSpecification As String, _
                                ByVal aTargetId As Integer, _
                                Optional ByVal aExistAction As atcData.atcDataSource.EnumExistAction = atcData.atcDataSource.EnumExistAction.ExistReplace) As Boolean

        Dim lSourceDataSource As New atcWDM.atcDataSourceWDM
        'Select Case aSourceType
        '    Case "wdm"
        '        lSourceDataSource = New atcWDM.atcDataSourceWDM
        '    Case Else
        '        Logger.Dbg("CopyDataSet:NotWDM (" & aSourceType & ") " & aSourceSpecification)
        '        Return False
        'End Select
        If lSourceDataSource.Open(aSourceSpecification) Then
            Dim lSourceDataSetIndex As Integer = lSourceDataSource.DataSets.IndexFromKey(aSourceId)
            Dim lSourceDataSet As atcTimeseries = Nothing
            If lSourceDataSetIndex >= 0 Then
                lSourceDataSet = lSourceDataSource.DataSets.ItemByIndex(lSourceDataSetIndex)
                lSourceDataSource.ReadData(lSourceDataSet)
            End If
            If lSourceDataSet Is Nothing Then
                Logger.Dbg("CopyDataSet:FailedToOpenSourceId " & aSourceId & " from " & aSourceSpecification)
                Return False
            Else
                Return CopyDataSet(lSourceDataSet, aTargetType, aTargetSpecification, aTargetId, aExistAction)
            End If
        Else
            Logger.Dbg("CopyDataSet:FailedToOpenSource " & aSourceSpecification)
            Return False
        End If
    End Function

    Public Function SetWDMAttribute(ByVal aTargetSpecification As String, _
                                    ByVal aTargetId As Integer, _
                                    ByVal aAttName As String, _
                                    ByVal aAttVal As String) As Boolean
        Dim lTargetDataSource As atcData.atcDataSource = New atcWDM.atcDataSourceWDM
        If lTargetDataSource.Open(aTargetSpecification) Then
            Dim lTargetDataSet As atcTimeseries = lTargetDataSource.DataSets(lTargetDataSource.DataSets.IndexFromKey(aTargetId))
            lTargetDataSet.EnsureValuesRead()
            lTargetDataSet.Attributes.SetValue(aAttName, aAttVal)
            Return lTargetDataSource.AddDataSet(lTargetDataSet, atcData.atcDataSource.EnumExistAction.ExistReplace)
        Else
            Return False
        End If
    End Function
End Module
