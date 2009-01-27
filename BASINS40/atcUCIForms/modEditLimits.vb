Imports atcUCI

Module modEditLimits
    Public Sub SetLimitsWDM(ByVal aValidValues As Collection, ByVal aUci As HspfUci)
        Dim lFiles As HspfFilesBlk = aUci.FilesBlock

        For lIndex As Integer = 1 To aUci.FilesBlock.Count
            Dim lFile As HspfFile = aUci.FilesBlock.Value(lIndex)
            If lFile.Typ.Length > 2 Then
                If lFile.Typ.Substring(0, 3) = "WDM" Then
                    aValidValues.Add(lFile.Typ)
                End If
            End If
        Next
    End Sub

    Public Sub CheckValidDsn(ByVal aValidValues As Collection, ByVal aWDMid As String, ByVal aUci As HspfUci)
        Dim lWdmId As Integer = WDMInd(aWDMid)
        For lDsn As Integer = 1 To 9999
            Dim lDsnObj As atcData.atcTimeseries = aUci.GetDataSetFromDsn(lWdmId, lDsn)
            If Not lDsnObj Is Nothing Then
                aValidValues.Add(lDsn)
            End If
        Next
    End Sub

    Public Sub CheckValidMemberName(ByVal aValidValues As Collection, ByVal aDsn As Integer, ByVal aWDMid As String, ByVal aUci As HspfUci)
        Dim lDsnObj As atcData.atcTimeseries = aUci.GetDataSetFromDsn(WDMInd(aWDMid), aDsn)
        If Not lDsnObj Is Nothing Then
            aValidValues.Add(lDsnObj.Attributes.GetDefinedValue("TSTYPE").Value)
        End If
    End Sub

    Public Sub SetValidTrans(ByVal aValidValues As Collection)
        aValidValues.Add(" ") 'allow default blank
        aValidValues.Add("SAME")
        aValidValues.Add("AVER")
        aValidValues.Add("DIV ")
        aValidValues.Add("INTP")
        aValidValues.Add("LAST")
        aValidValues.Add("MAX ")
        aValidValues.Add("MIN ")
        aValidValues.Add("SUM ")
    End Sub

    Public Sub SetValidOperations(ByVal aValidValues As Collection, ByVal aUci As HspfUci)
        For Each lOpnBlk As HspfOpnBlk In aUci.OpnBlks
            If lOpnBlk.Count > 0 Then
                aValidValues.Add(lOpnBlk.Name)
            End If
        Next
    End Sub

    Public Sub SetAllOperations(ByVal aValidValues As Collection, ByVal aUci As HspfUci)
        For Each lOpnBlk As HspfOpnBlk In aUci.OpnBlks
            aValidValues.Add(lOpnBlk.Name)
        Next
    End Sub

    Public Sub SetOperationMinMax(ByVal aValidValues As Collection, ByVal aUci As HspfUci, ByVal aOperType As String)
        If aOperType.Length > 0 Then
            Dim lOpnBlk As HspfOpnBlk = aUci.OpnBlks(Trim(aOperType))
            If lOpnBlk.Count > 0 Then
                For lIndex As Integer = 0 To lOpnBlk.Count - 1
                    aValidValues.Add(lOpnBlk.Ids(lIndex).Id)
                Next
            End If
        End If
    End Sub

    Public Sub SetGroupNames(ByVal aValidValues As Collection, ByVal aMsg As HspfMsg, ByVal aOperName As String)
        If Not aOperName Is Nothing AndAlso aOperName.Length > 0 Then
            Dim lOper As Integer = HspfOperNum(aOperName)
            If lOper > 0 Then
                For Each lGroup As HspfTSGroupDef In aMsg.TSGroupDefs
                    If lGroup.BlockID = 120 + lOper Then
                        aValidValues.Add(lGroup.Name)
                    End If
                Next
            End If
        End If
    End Sub

    Public Sub SetMemberNames(ByVal aValidValues As Collection, ByVal aMsg As HspfMsg, ByVal aOperName As String, ByVal aGroupName As String)
        If Not aOperName Is Nothing AndAlso aOperName.Length > 0 Then
            Dim lOper As Integer = HspfOperNum(aOperName)
            If lOper > 0 Then
                For Each lGroup As HspfTSGroupDef In aMsg.TSGroupDefs
                    If lGroup.BlockID = 120 + lOper And lGroup.Name = aGroupName Then
                        'this is the one we want
                        Dim lSkey As String = CStr(lGroup.Id)
                        For Each lMember As HspfTSMemberDef In aMsg.TSGroupDefs(lSkey).MemberDefs
                            aValidValues.Add(lMember.Name)
                        Next
                        Exit Sub
                    End If
                Next
            End If
        End If
    End Sub

    Public Sub SetMemberSubscript(ByVal aValidValues As Collection, ByVal aMsg As HspfMsg, ByVal aOperName As String, ByVal aGroupName As String, ByVal aMemberName As String, ByVal aFirst As Boolean)
        If Not aOperName Is Nothing AndAlso aOperName.Length > 0 Then
            Dim lOper As Integer = HspfOperNum(aOperName)
            If lOper > 0 Then
                For Each lGroup As HspfTSGroupDef In aMsg.TSGroupDefs
                    If lGroup.BlockID = 120 + lOper And lGroup.Name = aGroupName Then
                        Dim lSkey As String = CStr(lGroup.Id)
                        For Each lMember As HspfTSMemberDef In aMsg.TSGroupDefs(lSkey).MemberDefs
                            If lMember.Name = aMemberName Then
                                If aFirst Then
                                    If lMember.Maxsb1 = 1 Then aValidValues.Add(0)
                                    For lSub As Integer = 1 To lMember.Maxsb1
                                        aValidValues.Add(lSub)
                                    Next
                                Else
                                    If lMember.Maxsb2 = 1 Then aValidValues.Add(0)
                                    For lSub As Integer = 1 To lMember.Maxsb2
                                        aValidValues.Add(lSub)
                                    Next
                                End If
                                Exit Sub
                            End If
                        Next
                    End If
                Next
            End If
        End If
    End Sub

    Public Sub SetValidMassLinks(ByVal aValidValues As Collection, ByVal aUci As HspfUci)
        For Each lMassLink As HspfMassLink In aUci.MassLinks
            If Not aValidValues.Contains(lMassLink.MassLinkId) Then
                aValidValues.Add(lMassLink.MassLinkId)
            End If
        Next
    End Sub

    Public Function WDMInd(ByRef wdmid As String) As Integer
        Dim w As String

        If Len(wdmid) > 3 Then
            w = Mid(wdmid, 4, 1)
            If w = " " Then w = "1"
        Else
            w = "1"
        End If
        WDMInd = CInt(w)
    End Function

    Private Function HspfOperNum(ByRef aName As String) As HspfData.HspfOperType
        Dim lHspfOperNum As HspfData.HspfOperType

        Select Case aName
            Case "PERLND" : lHspfOperNum = HspfData.HspfOperType.hPerlnd
            Case "IMPLND" : lHspfOperNum = HspfData.HspfOperType.hImplnd
            Case "RCHRES" : lHspfOperNum = HspfData.HspfOperType.hRchres
            Case "COPY" : lHspfOperNum = HspfData.HspfOperType.hCopy
            Case "PLTGEN" : lHspfOperNum = HspfData.HspfOperType.hPltgen
            Case "DISPLY" : lHspfOperNum = HspfData.HspfOperType.hDisply
            Case "DURANL" : lHspfOperNum = HspfData.HspfOperType.hDuranl
            Case "GENER" : lHspfOperNum = HspfData.HspfOperType.hGener
            Case "MUTSIN" : lHspfOperNum = HspfData.HspfOperType.hMutsin
            Case "BMPRAC" : lHspfOperNum = HspfData.HspfOperType.hBmprac
            Case "REPORT" : lHspfOperNum = HspfData.HspfOperType.hReport
            Case Else : lHspfOperNum = 0
        End Select

        Return lHspfOperNum
    End Function
End Module
