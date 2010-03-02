Imports System.Collections.ObjectModel
Imports atcUCI
Imports atcUtility
Imports MapWinUtility

Public Class frmBMP
    Friend Class BmpInfo
        Friend Id As Long
        Friend Desc As String
        Friend InUseNow As Boolean 'associated with current reach
        Friend DeletePending As Boolean 'get rid of at next update uci
        Friend col As Long
    End Class
    Friend pBmps As Collection(Of BmpInfo)

    Friend Class RchInfo
        Friend Id As Long
        Friend Desc As String
        Friend BMPCnt As Long
    End Class
    Friend pReaches As Collection(Of RchInfo)

    Friend Class BmpPtrInfo
        Friend Ind As Long 'into type bmpinfo
        Friend Area As Single
    End Class

    Friend Class TribInfo
        Friend Id As Long
        Friend OpNam As String
        Friend Desc As String
        Friend Area As Single
        'Friend BMPCnt As Long
        Friend BmpPtrs As Collection(Of BmpPtrInfo)
    End Class
    Friend pTribs As Collection(Of TribInfo)

    Private pMsgTitle As String = "Best Management Practices Editor"
    Private pCurrentBMPId As Integer
    Private pInitializing As Boolean

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        pInitializing = True
        InitializeComponent()
        pInitializing = False

        Me.Icon = pIcon

        'are any reaches available?
        Dim lOpnBlk As HspfOpnBlk = pUCI.OpnBlks("RCHRES")

        pReaches = New Collection(Of RchInfo)
        pBmps = New Collection(Of BmpInfo)
        pTribs = New Collection(Of TribInfo)

        If lOpnBlk.Count > 0 Then
            'what reaches are available?
            cboReach.Items.Clear()
            cboReach.Items.Add("Summary")
            'get ids
            For Each lOper As HspfOperation In lOpnBlk.Ids
                Dim lRch As New RchInfo
                lRch.Id = lOper.Id
                'get reach names
                Dim lTable As HspfTable = lOper.Tables("GEN-INFO")
                lRch.Desc = lTable.Parms("RCHID").Value
                If InStr(lRch.Desc, ":") Then 'assume id already present by convention
                    cboReach.Items.Add(lRch.Desc)
                Else
                    cboReach.Items.Add(lRch.Id & ":" & lRch.Desc)
                End If
                pReaches.Add(lRch)
            Next

            'get all bmp info
            lOpnBlk = pUCI.OpnBlks("BMPRAC")
            If lOpnBlk.Count > 0 Then
                For Each lOper As HspfOperation In lOpnBlk.Ids
                    Dim lBmp As New BmpInfo
                    lBmp.Id = lOper.Id
                    Dim lTable As HspfTable = lOper.Tables("GEN-INFO")
                    lBmp.Desc = lTable.Parms("BMPID").Value
                    lBmp.DeletePending = False
                    pBmps.Add(lBmp)
                Next
            End If

            With agdSource
                .Source = New atcControls.atcGridSource
                .Clear()
                .AllowHorizontalScrolling = False
                .AllowNewValidValues = True
                .Visible = True
            End With

            cboReach.SelectedIndex = 0
            PopulateGrid()

        Else
            Logger.Msg("The BMP option requires a scenario containing a RCHRES block." & vbCrLf & _
                       "The current scenario does not have a RCHRES block.", _
                       vbExclamation, pMsgTitle)
            Me.Dispose()
        End If
    End Sub

    Private Sub cmdBMPEffic_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdBMPEffic.Click

        If IsNothing(pfrmBMPEffic) Then
            pfrmBMPEffic = New frmBMPEffic
            pfrmBMPEffic.Init(atxBMPId.Text, atxBMPDesc.Text)
            pfrmBMPEffic.Show()
        Else
            If pfrmBMPEffic.IsDisposed Then
                pfrmBMPEffic = New frmBMPEffic
                pfrmBMPEffic.Init(atxBMPId.Text, atxBMPDesc.Text)
                pfrmBMPEffic.Show()
            Else
                pfrmBMPEffic.WindowState = FormWindowState.Normal
                pfrmBMPEffic.BringToFront()
            End If
        End If

    End Sub

    Private Sub GetUciInfo(ByVal aCurrentReach As Integer)

        Dim lId As Integer = pReaches(aCurrentReach).Id
        'assume bmps not in use for this reach
        For lBmpIndex As Integer = 0 To pBmps.Count - 1
            pBmps(lBmpIndex).InUseNow = False
        Next lBmpIndex

        pTribs = New Collection(Of TribInfo)
        pTribs.Clear()
        Dim lBmpCount As Integer = pBmps.Count
        Dim lOpnBlk As HspfOpnBlk = pUCI.OpnBlks("RCHRES")
        Dim lOper As HspfOperation = lOpnBlk.OperFromID(lId)

        If Not lOper Is Nothing Then
            'loop through all connections looking for this oper as target
            For Each lConn As HspfConnection In pUCI.Connections
                If lConn.Typ = 3 Or lConn.Typ = 2 Then 'network or schematic
                    If Not lConn.Target.Opn Is Nothing Then
                        If lConn.Target.Opn.Id = lOper.Id And _
                           lConn.Target.Opn.Name = lOper.Name Then  'found a source
                            If lConn.Source.VolName = "BMPRAC" Then
                                For lBmpIndex As Integer = 0 To lBmpCount - 1
                                    If pBmps(lBmpIndex).Id = lConn.Source.VolId Then
                                        pBmps(lBmpIndex).InUseNow = True
                                        pReaches(aCurrentReach).BMPCnt = pReaches(aCurrentReach).BMPCnt + 1
                                        Exit For
                                    End If
                                Next lBmpIndex
                            Else 'non-bmp source
                                Dim lTrib As New TribInfo
                                lTrib.Id = lConn.Source.VolId
                                lTrib.OpNam = lConn.Source.VolName
                                lTrib.Area = lConn.MFact
                                lTrib.Desc = lConn.Source.Opn.Description
                                Dim lBmpPtrs As New Collection(Of BmpPtrInfo)
                                lTrib.BmpPtrs = lBmpPtrs
                                pTribs.Add(lTrib)
                            End If
                        End If
                    End If
                End If
            Next

            For lBmpIndex As Integer = 0 To lBmpCount - 1 'loop thru bmps to find their tribs
                If pBmps(lBmpIndex).InUseNow Then
                    lId = pBmps(lBmpIndex).Id
                    lOpnBlk = pUCI.OpnBlks("BMPRAC")  'bmp
                    lOper = lOpnBlk.OperFromID(lId)

                    For Each lConn As HspfConnection In pUCI.Connections
                        If lConn.Typ = 3 Or lConn.Typ = 2 Then 'network or schematic
                            If lConn.Target.Opn.Id = lOper.Id And _
                               lConn.Target.Opn.Name = lOper.Name Then 'found a source
                                If lConn.Source.VolName = "BMPRAC" Then
                                    Logger.Msg("PROBLEM - BMPs cant go to BMPs in the WinHSPF BMP Editor", vbOKOnly, pMsgTitle)
                                    Me.Dispose() 'close up bmp editing
                                End If
                                Dim found As Boolean = False
                                For lTribIndex As Integer = 0 To pTribs.Count - 1
                                    If pTribs(lTribIndex).OpNam = lConn.Source.VolName Then
                                        If pTribs(lTribIndex).Id = lConn.Source.Opn.Id Then
                                            'know about this trib
                                            found = True
                                            Dim lBmpPtr As New BmpPtrInfo
                                            lBmpPtr.Ind = lBmpIndex
                                            lBmpPtr.Area = lConn.MFact
                                            Dim lBmpPtrs As New Collection(Of BmpPtrInfo)
                                            pTribs(lTribIndex).BmpPtrs = lBmpPtrs
                                            pTribs(lTribIndex).BmpPtrs.Add(lBmpPtr)
                                            pTribs(lTribIndex).Area = pTribs(lTribIndex).Area + lConn.MFact
                                            Exit For
                                        End If
                                    End If
                                Next lTribIndex
                                If Not (found) Then 'add to trib list
                                    Dim lTrib As New TribInfo
                                    lTrib.Desc = lConn.Source.Opn.Description
                                    lTrib.Id = lConn.Source.Opn.Id
                                    lTrib.OpNam = lConn.Source.VolName
                                    Dim lBmpPtr As New BmpPtrInfo
                                    lBmpPtr.Ind = lBmpIndex
                                    lBmpPtr.Area = lConn.MFact
                                    Dim lBmpPtrs As New Collection(Of BmpPtrInfo)
                                    lTrib.BmpPtrs = lBmpPtrs
                                    lTrib.BmpPtrs.Add(lBmpPtr)
                                    lTrib.Area = lTrib.Area + lConn.MFact
                                    pTribs.Add(lTrib)
                                End If
                            End If
                        End If
                    Next lConn
                End If
            Next lBmpIndex 'bmp

            For lBmpIndex As Integer = 0 To lBmpCount - 1
                If pBmps(lBmpIndex).InUseNow Then
                    For lTribIndex As Integer = 0 To pTribs.Count - 1
                        Dim lFound As Boolean = False
                        For lTribBmpIndex As Integer = 0 To pTribs(lTribIndex).BmpPtrs.Count - 1
                            If pTribs(lTribIndex).BmpPtrs(lTribBmpIndex).Ind = lBmpIndex Then
                                lFound = True
                                Exit For
                            ElseIf pTribs(lTribIndex).BmpPtrs(lTribBmpIndex).Ind > lBmpIndex Then
                                With pTribs(lTribIndex)
                                    Dim lBmpPtr As New BmpPtrInfo
                                    lBmpPtr.Ind = lBmpIndex
                                    lBmpPtr.Area = 0
                                    .BmpPtrs.Add(lBmpPtr)
                                End With
                                lFound = True 'we added it
                                Exit For
                            End If
                        Next lTribBmpIndex
                        If Not lFound Then 'add to end
                            With pTribs(lTribIndex)
                                Dim lBmpPtr As New BmpPtrInfo
                                lBmpPtr.Ind = lBmpIndex
                                lBmpPtr.Area = 0
                                .BmpPtrs.Add(lBmpPtr)
                            End With
                        End If
                    Next lTribIndex
                End If
            Next lBmpIndex
        End If
    End Sub

    Private Sub BmpDescUpdate(ByVal aBmpIndex As Integer)
        If aBmpIndex > 0 Then
            With agdSource.Source
                pCurrentBMPId = Mid(.CellValue(0, .Columns - 1), 6, Len(.CellValue(0, .Columns - 1)) - 4)
                For lBmpIndex As Integer = 0 To pBmps.Count - 1
                    If pCurrentBMPId = pBmps(lBmpIndex).Id Then
                        atxBMPId.Text = pBmps(lBmpIndex).Id
                        atxBMPDesc.Text = pBmps(lBmpIndex).Desc
                        Exit For
                    End If
                Next lBmpIndex
                fraBMPDet.Visible = True
            End With
        Else
            pCurrentBMPId = 0
            fraBMPDet.Visible = False
        End If
    End Sub

    Private Sub PopulateGrid()
        Dim lBmpCount As Integer = pBmps.Count
        Dim lCurrentReach As Integer = cboReach.SelectedIndex

        '    grdSrc.Clear()
        agdSource.Source.Rows = 1
        Dim lRchCount As Integer = 0
        agdSource.Source.FixedColumns = 1

        If lCurrentReach = 0 Then
            'summary style grid
            lblContributing.Text = "Summary of Areas by LandUse and Reach"
            agdSource.Source.Columns = 1
            agdSource.Source.FixedColumns = 1
            agdSource.Source.FixedRows = 1
            For lRchIndex As Integer = 1 To cboReach.Items.Count - 1
                lRchCount = lRchCount + 1
                agdSource.Source.CellValue(lRchCount, 0) = "R:" & cboReach.Items(lRchIndex)
                agdSource.Source.CellColor(lRchCount, 0) = Me.BackColor
                GetUciInfo(lRchIndex - 1)
                For lTribIndex As Integer = 0 To pTribs.Count - 1
                    If pTribs(lTribIndex).OpNam <> "RCHRES" Then
                        Dim lExists As Boolean = False
                        Dim lTempTitle As String = ""
                        If InStr(1, pTribs(lTribIndex).Desc, ":") Then
                            lTempTitle = pTribs(lTribIndex).Desc
                        Else
                            lTempTitle = Mid(pTribs(lTribIndex).OpNam, 1, 1) & ":" & pTribs(lTribIndex).Desc
                        End If
                        Dim lColIndex As Integer = 0
                        For lColIndex = 1 To agdSource.Source.Columns
                            If agdSource.Source.CellValue(0, lColIndex) = lTempTitle Then
                                lExists = True
                                Exit For
                            End If
                        Next lColIndex
                        If Not lExists Then
                            lColIndex = agdSource.Source.Columns
                            agdSource.Source.CellValue(0, lColIndex) = lTempTitle
                            agdSource.Source.CellEditable(lRchCount, lColIndex) = False
                            'agdSource.Source.ColType(lcolindex) = ATCoSng
                        End If
                        agdSource.Source.CellValue(lRchCount, lColIndex) = pTribs(lTribIndex).Area
                    End If
                Next lTribIndex
            Next lRchIndex
            fraBMPDet.Visible = False
            cmdAdd.Visible = False
            cmdDelete.Visible = False
        Else
            'build reach/bmp grid
            lblContributing.Text = "Contributing Sources to Reach " & pReaches(lCurrentReach - 1).Id & " (" & pReaches(lCurrentReach - 1).Desc & ")"

            agdSource.Source.CellValue(0, 0) = "Source"
            agdSource.Source.CellEditable(0, 0) = False
            agdSource.Source.CellValue(0, 1) = "Area"
            agdSource.Source.CellEditable(0, 1) = False
            agdSource.Source.CellValue(0, 2) = "% No BMP"
            agdSource.Source.CellEditable(0, 2) = False
            'agdSource.Source.ColMax(2) = 100
            'agdSource.Source.ColType(2) = ATCoSng
            agdSource.Source.Columns = 3
            agdSource.Source.FixedColumns = 1
            agdSource.Source.FixedRows = 1

            Dim lCountBmpInUse As Integer = 0
            For lBmpIndex As Integer = 0 To lBmpCount - 1
                If pBmps(lBmpIndex).InUseNow And Not (pBmps(lBmpIndex).DeletePending) Then
                    pBmps(lBmpIndex).col = agdSource.Source.Columns
                    agdSource.Source.Columns = agdSource.Source.Columns + 1
                    lCountBmpInUse = lCountBmpInUse + 1
                    agdSource.Source.CellValue(0, 2 + lCountBmpInUse) = "% BMP " & pBmps(lBmpIndex).Id
                    agdSource.Source.CellEditable(1, 2 + lCountBmpInUse) = True
                    'agdSource.Source.ColMin(2 + lCountBmpInUse) = 0
                    'agdSource.Source.ColMax(2 + lCountBmpInUse) = 100
                    'grdSrc.ColType(2 + lCountBmpInUse) = ATCoSng
                Else
                    pBmps(lBmpIndex).col = 0
                End If
            Next lBmpIndex

            If lCountBmpInUse = 0 Then ' no bmps, % must be 100
                'grdSrc.ColMin(2) = 100
                fraBMPDet.Visible = False
                agdSource.Source.CellEditable(1, 2) = False
            Else
                'grdSrc.ColMin(2) = 0
                fraBMPDet.Visible = True
                agdSource.Source.CellEditable(1, 2) = True
            End If

            'agdSource.Source.ColWidth(0) = agdSource.Source.Width * 0.4
            'agdSource.Source.ColWidth(1) = (agdSource.Source.Width - agdSource.Source.ColWidth(0) - 40) / CSng(2 + buse)
            'For i as integer = 2 To 2 + buse
            'agdSource.Source.ColWidth(i) = agdSource.Source.ColWidth(1)
            'Next i

            For lTribIndex As Integer = 0 To pTribs.Count - 1 'trib source areas and %
                If pTribs(lTribIndex).OpNam <> "BMPRAC" Then
                    lRchCount = lRchCount + 1
                    agdSource.Source.Rows = lRchCount
                    Dim lTxt As String = pTribs(lTribIndex).OpNam & " : " & pTribs(lTribIndex).Id & " (" & pTribs(lTribIndex).Desc & ")"
                    agdSource.Source.CellValue(lRchCount, 0) = lTxt
                    agdSource.Source.CellEditable(lRchCount, 0) = False
                    agdSource.Source.CellColor(lRchCount, 0) = Me.BackColor
                    Dim lBmpArea As Double = 0
                    agdSource.Source.CellEditable(lRchCount, 1) = False
                    If lTxt.StartsWith("RCH") Then
                        agdSource.Source.CellValue(lRchCount, 1) = "NA"
                    Else
                        agdSource.Source.CellValue(lRchCount, 1) = pTribs(lTribIndex).Area
                    End If
                    lCountBmpInUse = 0
                    Dim lRArea As Double = 0.0
                    For lBmpIndex As Integer = 0 To lBmpCount - 1
                        If pBmps(lBmpIndex).InUseNow And Not (pBmps(lBmpIndex).DeletePending) Then
                            lCountBmpInUse = lCountBmpInUse + 1
                            If lCountBmpInUse <= pTribs(lTribIndex).BmpPtrs.Count Then
                                lRArea = pTribs(lTribIndex).BmpPtrs(lCountBmpInUse - 1).Area
                            Else
                                lRArea = 0
                            End If
                            agdSource.Source.CellValue(lRchCount, 2 + lCountBmpInUse) = 100 * (lRArea / pTribs(lTribIndex).Area)
                            agdSource.Source.CellEditable(lRchCount, 2 + lCountBmpInUse) = True
                            lBmpArea = lBmpArea + lRArea
                        End If
                    Next lBmpIndex
                    agdSource.Source.CellValue(lRchCount, 2) = 100 - 100 * (lBmpArea / pTribs(lTribIndex).Area)
                    If lCountBmpInUse = 0 Then ' no bmps, % must be 100
                        agdSource.Source.CellEditable(lRchCount, 2) = False
                    Else
                        agdSource.Source.CellEditable(lRchCount, 2) = True
                    End If
                End If
            Next lTribIndex
            ' default pos to first row, bmp col (if avail)
            'agdSource.Source.row = 1
            'agdSource.Source.col = 2 + buse
            BmpDescUpdate(lCountBmpInUse)
            If lCountBmpInUse > 0 Then
                fraBMPDet.Visible = True
            End If
            cmdAdd.Visible = True
            cmdDelete.Visible = True
        End If

        agdSource.SizeAllColumnsToContents()
        agdSource.Refresh()

    End Sub

    Private Sub atxBMPId_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles atxBMPId.LostFocus
        If atxBMPId.Enabled = True Then
            For lBmpIndex As Integer = 0 To pBmps.Count - 1
                If pBmps(lBmpIndex).Id = atxBMPId.Text Then
                    Logger.Msg("BMP id " & pBmps(lBmpIndex).Id & " is already in use" & vbCrLf & _
                            "Try another ID number", vbOKOnly, pMsgTitle)
                    atxBMPId.Text = pCurrentBMPId
                    Exit Sub
                End If
            Next lBmpIndex

            pCurrentBMPId = atxBMPId.Text

            agdSource.Source.CellValue(0, agdSource.Source.Columns) = "% BMP " & pCurrentBMPId
            pBmps(agdSource.Source.Columns - 2).Id = pCurrentBMPId

            cmdUpdateUCI.Enabled = True
        End If
    End Sub

    Private Sub cboReach_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboReach.SelectedIndexChanged
        If cboReach.SelectedIndex > 0 Then
            GetUciInfo(cboReach.SelectedIndex - 1)
        End If
        PopulateGrid()
        cmdUpdateUCI.Enabled = False
        atxBMPId.Enabled = False
    End Sub

    Private Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        Dim lCurReach As Integer = cboReach.SelectedIndex - 1

        Dim lNewBmp As New BmpInfo
        ' assign an id not in use
        Dim lNewIndex As Integer = 0
        Dim lDone As Integer = False
        Do Until lDone
            lDone = True ' assume the best
            For lBmpIndex As Integer = 0 To pBmps.Count - 1
                If pBmps(lBmpIndex).Id = pReaches(lCurReach).Id + lNewIndex Then 'in use
                    lNewIndex = lNewIndex + 1
                    lDone = False
                    Exit For
                End If
            Next lBmpIndex
        Loop

        lNewBmp.Id = pReaches(lCurReach).Id + lNewIndex
        lNewBmp.Desc = "New BMP"
        lNewBmp.InUseNow = True
        For lNewIndex = 0 To pTribs.Count - 1
            Dim lBmpPtr As New BmpPtrInfo
            lBmpPtr.Area = 0
            lBmpPtr.Ind = pBmps.Count
            pTribs(lNewIndex).BmpPtrs.Add(lBmpPtr)
        Next lNewIndex
        pReaches(lCurReach).BMPCnt = pReaches(lCurReach).BMPCnt + 1

        pBmps.Add(lNewBmp)

        PopulateGrid()
        cmdUpdateUCI.Enabled = True
        atxBMPId.Enabled = True
    End Sub

    Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        Dim lCurReach As Integer = cboReach.SelectedIndex - 1

        If pReaches(lCurReach).BMPCnt = 0 Then
            Logger.Msg("No BMPs are associated with Reach " & pReaches(lCurReach).Id, _
                       vbExclamation, pMsgTitle)
        ElseIf agdSource.Source.Columns > 2 Then 'are in a bmp
            'delete nth bmp
            Dim lNth As Integer = agdSource.Source.Columns - 2
            Dim lNCnt As Integer = 0
            Dim lCurBmp As Integer
            For lBmpIndex As Integer = 0 To pBmps.Count - 1
                If pBmps(lBmpIndex).InUseNow = True Then
                    lNCnt = lNCnt + 1
                    If lNth = lNCnt Then
                        'this is the one to delete
                        lCurBmp = pBmps(lBmpIndex).Id
                        lNth = lBmpIndex
                    End If
                End If
            Next lBmpIndex
            lCurBmp = atxBMPId.Text
            If Logger.Msg("Are you sure you want to Delete BMP " & lCurBmp & "?", _
                   vbYesNo, pMsgTitle) = vbYes Then
                pBmps(lNth).DeletePending = True
                pBmps(lNth).InUseNow = False
                pReaches(lCurReach).BMPCnt = pReaches(lCurReach).BMPCnt - 1
                For lTribIndex As Integer = 1 To pTribs.Count
                    'adjust here
                Next lTribIndex
                agdSource.Source.Columns = agdSource.Source.Columns - 1
                'agdSource.Source.col = 2
                PopulateGrid()
                cmdUpdateUCI.Enabled = True
            End If
        End If
    End Sub

    Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        If cmdUpdateUCI.Enabled = True Then
            If Logger.Msg("You have changes to your UCI made which have not been saved. " & vbCrLf & _
                             "OK trashes them, Cancel allows you a chance to update your UCI.", _
                             vbOKCancel, pMsgTitle) = MsgBoxResult.Ok Then
                Me.Dispose()
            End If
        Else 'no changes pending
            Me.Dispose()
        End If
    End Sub

    Private Sub cmdUpdateUCI_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpdateUCI.Click

        Dim lRchId As Integer = pReaches(cboReach.SelectedIndex - 1).Id

        atxBMPId.Enabled = False
        For lBmpIndex As Integer = 0 To pBmps.Count - 1 'process deletes first
            If pBmps(lBmpIndex).DeletePending Then 'get rid of it
                pUCI.DeleteOperation("BMPRAC", pBmps(lBmpIndex).Id)
                pBmps(lBmpIndex).InUseNow = False
            End If
        Next lBmpIndex

        'put areas back for the bmps that are in use
        For lBmpIndex As Integer = 0 To pBmps.Count - 1
            If pBmps(lBmpIndex).InUseNow Then

                'see if we need to add this bmp to opn seq block
                Dim lOpnBlk As HspfOpnBlk = pUCI.OpnBlks("BMPRAC")
                Dim lAddIt As Boolean = True
                If lOpnBlk.Count > 0 Then
                    Dim lOpn As HspfOperation = lOpnBlk.OperFromID(pBmps(lBmpIndex).Id)
                    If Not lOpn Is Nothing Then lAddIt = False
                End If
                If lAddIt Then
                    'add new bmprac operation
                    pUCI.AddOperation("BMPRAC", pBmps(lBmpIndex).Id)
                    'figure out where to put it in opn seq block
                    Dim lAddBefore As Integer = pUCI.OpnSeqBlock.Opns.Count
                    For lOpIndex As Integer = 0 To pUCI.OpnSeqBlock.Opns.Count - 1
                        If pUCI.OpnSeqBlock.Opn(lOpIndex).Name = "RCHRES" And _
                           pUCI.OpnSeqBlock.Opn(lOpIndex).Id = lRchId Then
                            lAddBefore = lOpIndex
                        End If
                    Next lOpIndex
                    'now add to opn seq block
                    Dim lOpn As HspfOperation = lOpnBlk.OperFromID(pBmps(lBmpIndex).Id)
                    lOpn.Description = pBmps(lBmpIndex).Desc
                    lOpn.Uci = pUCI
                    lOpn.OpnBlk = lOpnBlk
                    pUCI.OpnSeqBlock.AddBefore(lOpn, lAddBefore)
                    Dim lTable As HspfTable
                    If lOpnBlk.Count > 1 Then
                        'already have some of this operation
                        For Each lTable In lOpnBlk.Ids(0).Tables
                            'add this opn id to this table
                            pUCI.AddTable("BMPRAC", pBmps(lBmpIndex).Id, lTable.Name)
                        Next lTable
                    Else
                        'added the first bmprac, need to add associated tables
                        Dim lBlock As HspfBlockDef
                        Dim lSection As HspfSectionDef
                        Dim lTabledef As HspfTableDef
                        lBlock = pMsg.BlockDefs("BMPRAC")
                        For Each lSection In lBlock.SectionDefs
                            For Each lTabledef In lSection.TableDefs
                                pUCI.AddTable("BMPRAC", pBmps(lBmpIndex).Id, lTabledef.Name)
                            Next
                        Next
                    End If
                    lTable = lOpn.Tables("GEN-INFO")
                    lTable.Parms("BMPID").Value = pBmps(lBmpIndex).Desc
                    lTable.Parms("NGQUAL").Value = 0   'assume no gquals
                    lTable = lOpn.Tables("GQ-FRAC")
                    lTable.Parms("GQID").Value = "unknown"
                End If

                'look for bmp to rchres connection, add it if not existing
                PutSchematicRecord("BMPRAC", pBmps(lBmpIndex).Id, "RCHRES", lRchId, 1.0#)
            End If
        Next lBmpIndex

        For lTribIndex As Integer = 0 To pTribs.Count - 1
            'put area going directly to rch
            Dim lArea As Double = pTribs(lTribIndex).Area * CSng((agdSource.Source.CellValue(lTribIndex + 1, 2) / 100))
            PutSchematicRecord(pTribs(lTribIndex).OpNam, pTribs(lTribIndex).Id, "RCHRES", lRchId, lArea)
            'put area going to bmps
            For lTribBmpIndex As Integer = 0 To pTribs(lTribIndex).BmpPtrs.Count - 1
                If pBmps(pTribs(lTribIndex).BmpPtrs(lTribBmpIndex).Ind).InUseNow Then
                    Dim lBmpId As Integer = pBmps(pTribs(lTribIndex).BmpPtrs(lTribBmpIndex).Ind).Id
                    Dim lColId As Integer = pBmps(pTribs(lTribIndex).BmpPtrs(lTribBmpIndex).Ind).col
                    lArea = pTribs(lTribIndex).Area * CSng((agdSource.Source.CellValue(lTribIndex + 1, lColId) / 100))
                    pTribs(lTribIndex).BmpPtrs(lTribBmpIndex).Area = lArea
                    PutSchematicRecord(pTribs(lTribIndex).OpNam, pTribs(lTribIndex).Id, "BMPRAC", lBmpId, lArea)
                End If
            Next lTribBmpIndex
        Next lTribIndex

        GetUciInfo(cboReach.SelectedIndex - 1) 'refresh with new data
        PopulateGrid()
        With pWinHSPF.SchematicDiagram
            .ClearTree()
            .BuildTree()
            .UpdateLegend()
            .UpdateDetails()
        End With
        cmdUpdateUCI.Enabled = False
    End Sub

    Private Sub agdSource_CellEdited(ByVal aGrid As atcControls.atcGrid, ByVal aRow As Integer, ByVal aColumn As Integer) Handles agdSource.CellEdited
        'was on commit change event, not sure what this is now
        Dim lCol As Integer = aColumn

        Dim lMinValue As Integer = 0
        Dim lMaxValue As Integer = 100

        Dim lNewValue As String = aGrid.Source.CellValue(aRow, aColumn)
        Dim lNewValueNumeric As Double = -999
        If IsNumeric(lNewValue) Then lNewValueNumeric = CDbl(lNewValue)
        Dim lNewColor As Color = aGrid.Source.CellColor(aRow, aColumn)
        If (lNewValueNumeric >= lMinValue And lMinValue <> -999) AndAlso (lNewValueNumeric <= lMaxValue And lMaxValue <> -999) Then
            lNewColor = aGrid.CellBackColor
        Else
            lNewColor = Color.Pink
        End If
        If Not lNewColor.Equals(aGrid.Source.CellColor(aRow, aColumn)) Then
            aGrid.Source.CellColor(aRow, aColumn) = lNewColor
        End If

        For lRow As Integer = 1 To aRow
            Dim lVal As Double = agdSource.Source.CellValue(lRow, lCol) 'highest priority is just set value

            Dim lcNow As Integer = 2
            For lBmpIndex As Integer = 0 To pBmps.Count - 1 ' each bmp
                If pBmps(lBmpIndex).InUseNow And Not (pBmps(lBmpIndex).DeletePending) Then
                    lcNow = lcNow + 1
                    If lcNow <> lCol Then 'dont double count current
                        lVal = lVal + agdSource.Source.CellValue(lRow, lcNow)
                        If lVal > 100 Then
                            agdSource.Source.CellValue(lRow, lcNow) = agdSource.Source.CellValue(lRow, lcNow) - lVal + 100
                            lVal = 100
                        End If
                    End If
                End If
            Next lBmpIndex

            If lVal < 100 Then 'need some more
                If lCol = 2 Then ' changing no bmp col, adjust first bmp
                    agdSource.Source.CellValue(lRow, 3) = 100 - (lVal + agdSource.Source.CellValue(lRow, 3))
                Else 'changing a bmp, adjust no bmp
                    agdSource.Source.CellValue(lRow, 2) = 100 - lVal
                End If
            ElseIf lCol > 2 Then
                agdSource.Source.CellValue(lRow, 2) = 100 - lVal
            End If
        Next lRow

        agdSource.Refresh()
        cmdUpdateUCI.Enabled = True
    End Sub

    'Private Sub grdSrc_RowColChange()
    '    If grdSrc.col >= 2 And Left(grdSrc.Header, 7) <> "Summary" Then
    '        BmpDescUpdate(grdSrc.col - 2)
    '    End If
    'End Sub

    Private Sub atxBMPDesc_Change() Handles atxBMPDesc.Change
        If Not pInitializing Then
            For lBmpIndex As Integer = 0 To pBmps.Count - 1
                If pBmps(lBmpIndex).InUseNow Then
                    pBmps(lBmpIndex).Desc = atxBMPDesc.Text
                End If
            Next
            cmdUpdateUCI.Enabled = True
        End If
    End Sub

    Private Sub PutSchematicRecord(ByVal aSName As String, ByVal aSId As Integer, ByVal aTName As String, ByVal aTId As Integer, ByVal aMultFact As Double)

        If aSName = "RCHRES" And aTName = "BMPRAC" Then 'dont do rchres to bmp connections
        Else
            If aSName = "RCHRES" And aTName = "RCHRES" Then
                aMultFact = 1.0#
            End If
            Dim lAddIt As Boolean = True
            Dim lDeleteIt As Boolean = False
            Dim lDeleteIndex As Integer = 0
            For lIndex As Integer = 0 To pUCI.Connections.Count - 1
                Dim lConn As HspfConnection = pUCI.Connections(lIndex)
                If lConn.Typ = 3 Then 'schematic
                    If lConn.Target.Opn.Id = aTId And _
                       lConn.Target.Opn.Name = aTName And _
                       lConn.Source.Opn.Id = aSId And _
                       lConn.Source.Opn.Name = aSName Then
                        lAddIt = False
                        lConn.MFact = aMultFact
                        If Math.Abs(aMultFact) < 0.00000001 Then
                            lDeleteIt = True
                            lDeleteIndex = lIndex
                        End If
                    End If
                End If
            Next lIndex
            If lAddIt And Math.Abs(aMultFact) > 0.00000001 Then 'need to add the connection
                Dim lConn = New HspfConnection
                Dim lOpnBlk As HspfOpnBlk = pUCI.OpnBlks(aSName)
                Dim lOpn As HspfOperation = lOpnBlk.OperFromID(aSId)
                lConn.Source.Opn = lOpn
                lConn.Source.volname = lOpn.Name
                lConn.Source.volid = lOpn.Id
                lConn.Typ = 3
                lConn.MFact = aMultFact
                lOpnBlk = pUCI.OpnBlks(aTName)
                lOpn = lOpnBlk.OperFromID(aTId)
                lConn.Target.Opn = lOpn
                lConn.Target.volname = lOpn.Name
                lConn.Target.volid = lOpn.Id
                Dim lMLId As Integer = 0
                GetMassLinkID(aSName, aTName, lMLId)
                If lMLId = 0 Then
                    AddMassLink(aSName, aTName, lMLId)
                End If
                lConn.MassLink = lMLId
                lConn.Uci = pUCI
                'add targets to source opn
                lOpnBlk = pUCI.OpnBlks(aSName)
                lOpn = lOpnBlk.OperFromID(aSId)
                lOpn.Targets.Add(lConn)
                lConn.Source.Opn = lOpn
                'add sources to target opn
                lOpnBlk = pUCI.OpnBlks(aTName)
                lOpn = lOpnBlk.OperFromID(aTId)
                lOpn.Sources.Add(lConn)
                lConn.Target.Opn = lOpn
                'add to collection of connections
                pUCI.Connections.Add(lConn)
            ElseIf lDeleteIt Then 'need to delete the connection
                pUCI.Connections.RemoveAt(lDeleteIndex)
                'remove target from source opn
                Dim lOpnBlk As HspfOpnBlk = pUCI.OpnBlks(aSName)
                Dim lOpn As HspfOperation = lOpnBlk.OperFromID(aSId)
                Dim lIndex As Integer = 1
                For Each lConn As HspfConnection In lOpn.Targets
                    If lConn.Target.VolName = aTName And _
                       lConn.Target.VolId = aTId Then
                        lOpn.Targets.RemoveAt(lIndex)
                    Else
                        lIndex = lIndex + 1
                    End If
                Next
                'remove source from target opn
                lOpnBlk = pUCI.OpnBlks(aTName)
                lOpn = lOpnBlk.OperFromID(aTId)
                lIndex = 1
                For Each lConn As HspfConnection In lOpn.Sources
                    If lConn.Source.VolName = aSName And _
                       lConn.Source.VolId = aSId Then
                        lOpn.Sources.RemoveAt(lIndex)
                    Else
                        lIndex = lIndex + 1
                    End If
                Next
            End If
        End If
    End Sub

    Private Sub GetMassLinkID(ByVal aSName As String, ByVal aTName As String, ByRef aMLId As Integer)
        Dim lConn As HspfConnection

        'determine mass link number
        aMLId = 0
        For Each lConn In pUCI.Connections
            If lConn.Typ = 3 Then
                If lConn.Source.VolName = aSName And lConn.Target.VolName = aTName Then
                    aMLId = lConn.MassLink
                End If
            End If
        Next lConn
    End Sub

    Private Sub AddMassLink(ByVal aSName As String, ByVal aTName As String, ByRef aMLId As Integer)
        'need to add masslink, find an unused number
        Dim lFound As Boolean = True
        aMLId = 1
        Do Until lFound = False
            lFound = False
            For Each lMassLink As HspfMassLink In pUCI.MassLinks
                If lMassLink.MassLinkId = aMLId Then
                    aMLId = aMLId + 1
                    lFound = True
                    Exit For
                End If
            Next lMassLink
        Loop
        'find id of masslink to copy
        Dim lCopyId As Integer = 0
        If aSName = "BMPRAC" And aTName = "RCHRES" Then
            'copy from perlnd to rchres masslink
            Call GetMassLinkID("PERLND", aTName, lCopyId)
        ElseIf aSName = "PERLND" And aTName = "BMPRAC" Then
            'copy from perlnd to rchres masslink
            Call GetMassLinkID(aSName, "RCHRES", lCopyId)
        ElseIf aSName = "IMPLND" And aTName = "BMPRAC" Then
            'copy from implnd to rchres masslink
            Call GetMassLinkID(aSName, "RCHRES", lCopyId)
        End If
        If aMLId > 0 And lCopyId > 0 Then
            'now copy masslink
            For lIndex As Integer = 0 To pUCI.MassLinks.Count - 1
                Dim lcMassLink As HspfMassLink = pUCI.MassLinks(lIndex)
                If lcMassLink.MassLinkId = lCopyId Then
                    'copy this record
                    Dim lMassLink As New HspfMassLink
                    lMassLink.Uci = pUCI
                    lMassLink.MassLinkId = aMLId
                    lMassLink.Source.VolName = aSName
                    lMassLink.Source.VolId = 0
                    lMassLink.Source.Group = lcMassLink.Source.Group
                    lMassLink.Source.Member = lcMassLink.Source.Member
                    lMassLink.Source.MemSub1 = lcMassLink.Source.MemSub1
                    lMassLink.Source.MemSub2 = lcMassLink.Source.MemSub2
                    lMassLink.MFact = lcMassLink.MFact
                    lMassLink.Tran = lcMassLink.Tran
                    lMassLink.Target.VolName = aTName
                    lMassLink.Target.VolId = 0
                    lMassLink.Target.Group = lcMassLink.Target.Group
                    lMassLink.Target.Member = lcMassLink.Target.Member
                    lMassLink.Target.MemSub1 = lcMassLink.Target.MemSub1
                    lMassLink.Target.MemSub2 = lcMassLink.Target.MemSub2

                    If (aSName = "PERLND" Or aSName = "IMPLND") And _
                      aTName = "BMPRAC" Then  'special cases
                        If lcMassLink.Target.Member = "OXIF" Then
                            lMassLink.Target.Member = "IOX"
                        ElseIf lcMassLink.Target.Member = "NUIF1" Then
                            lMassLink.Target.Member = "IDNUT"
                        ElseIf lcMassLink.Target.Member = "NUIF2" Then
                            lMassLink.Target.Member = "ISNUT"
                        ElseIf lcMassLink.Target.Member = "PKIF" Then
                            lMassLink.Target.Member = "IPLK"
                        End If
                    End If

                    If aSName = "BMPRAC" And aTName = "RCHRES" Then
                        'special cases
                        lMassLink.Source.Group = "ROFLOW"
                        lMassLink.MFact = 1.0#
                        If lcMassLink.Target.Member = "IVOL" Then
                            lMassLink.Source.Member = "ROVOL"
                        ElseIf lcMassLink.Target.Member = "CIVOL" Then
                            lMassLink.Source.Member = "CROVOL"
                        ElseIf lcMassLink.Target.Member = "ICON" Then
                            lMassLink.Source.Member = "ROCON"
                        ElseIf lcMassLink.Target.Member = "IHEAT" Then
                            lMassLink.Source.Member = "ROHEAT"
                        ElseIf lcMassLink.Target.Member = "ISED" Then
                            lMassLink.Source.Member = "ROSED"
                        ElseIf lcMassLink.Target.Member = "IDQAL" Then
                            lMassLink.Source.Member = "RODQAL"
                        ElseIf lcMassLink.Target.Member = "ISQAL" Then
                            lMassLink.Source.Member = "ROSQAL"
                        ElseIf lcMassLink.Target.Member = "OXIF" Then
                            lMassLink.Source.Member = "ROOX"
                        ElseIf lcMassLink.Target.Member = "NUIF1" Then
                            lMassLink.Source.Member = "RODNUT"
                        ElseIf lcMassLink.Target.Member = "NUIF2" Then
                            lMassLink.Source.Member = "ROSNUT"
                        ElseIf lcMassLink.Target.Member = "PKIF" Then
                            lMassLink.Source.Member = "ROPLK"
                        ElseIf lcMassLink.Target.Member = "PHIF" Then
                            lMassLink.Source.Member = "ROPH"
                        End If
                        lMassLink.Source.MemSub1 = lcMassLink.Target.MemSub1
                        lMassLink.Source.MemSub2 = lcMassLink.Target.MemSub2
                    End If

                    pUCI.MassLinks.Add(lMassLink)
                End If
            Next
        End If

    End Sub

    Private Sub frmBMP_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyValue = Windows.Forms.Keys.F1 Then
            ShowHelp(pWinHSPFManualName)
            ShowHelp("User's Guide\Detailed Functions\Edit BMP.html")
        End If
    End Sub
End Class