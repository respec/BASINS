Imports atcUCI
Imports MapWinUtility

Public Class frmBMP

    Private Structure BmpInfo
        Dim Id As Long
        Dim Desc As String
        Dim InUseNow As Boolean 'associated with current reach
        Dim DeletePending As Boolean 'get rid of at next update uci
        Dim col As Long
    End Structure
    Private pBmps As Collection 'of BmpInfo

    Private Structure RchInfo
        Dim Id As Long
        Dim Desc As String
        Dim BMPCnt As Long
    End Structure
    Private pReaches As Collection  'of RchInfo

    Private Structure BmpPtrInfo
        Dim Ind As Long 'into type bmpinfo
        Dim Area As Single
    End Structure

    Private Structure TribInfo
        Dim Id As Long
        Dim OpNam As String
        Dim Desc As String
        Dim Area As Single
        Dim BMPCnt As Long
        Dim BmpPtr() As BmpPtrInfo
    End Structure
    Private pTribs As Collection 'of TribInfo

    Private pMsgTitle As String = "Best Management Practices Editor"
    Private curBMPId As Integer
    '    Public BMPDesc$

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        Me.Icon = pIcon
        Me.MinimumSize = Me.Size
        Me.MaximumSize = Me.Size

        'are any reaches available?
        Dim lOpnBlk As HspfOpnBlk = pUCI.OpnBlks("RCHRES")

        Dim pReaches As New Collection
        pReaches.Clear()
        Dim pBmps As New Collection
        pBmps.Clear()

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
            pfrmBMPEffic.Show()
        Else
            If pfrmBMPEffic.IsDisposed Then
                pfrmBMPEffic = New frmBMPEffic
                pfrmBMPEffic.Show()
            Else
                pfrmBMPEffic.WindowState = FormWindowState.Normal
                pfrmBMPEffic.BringToFront()
            End If
        End If

    End Sub

    Private Sub GetUciInfo(ByVal cur&)

        '    Dim Init&, Id&, rorb&
        '    Dim ctxt$, cmor$, pnam$, rcnt&, tcnt&
        '    Dim i&, j&, k&, h&, bcnt&, found As Boolean
        '    Dim rarea!, barea!
        '    Dim lOper As HspfOperation
        '    Dim lOpnBlk As HspfOpnBlk
        '    Dim vConn As Object
        '    Dim lConn As HspfConnection

        '    Id = MyRch(cur).Id
        '    'assume bmps not in use for this reach
        '    For i = 1 To UBound(myBmp)
        '        myBmp(i).InUseNow = False
        '    Next i

        '    ReDim myTrib(0)
        '    tcnt = 0
        '    bcnt = UBound(myBmp)
        '    lOpnBlk = myUci.OpnBlks("RCHRES")  'reach
        '    lOper = lOpnBlk.operfromid(Id)

        '    If Not lOper Is Nothing Then
        '        'loop through all connections looking for this oper as target
        '        For Each vConn In myUci.Connections
        '            lConn = vConn
        '            If lConn.Typ = 3 Or lConn.Typ = 2 Then 'network or schematic
        '                If Not lConn.Target.Opn Is Nothing Then
        '                    If lConn.Target.Opn.Id = lOper.Id And _
        '                       lConn.Target.Opn.Name = lOper.Name Then  'found a source
        '                        If lConn.Source.volname = "BMPRAC" Then
        '                            For i = 1 To bcnt
        '                                If myBmp(i).Id = lConn.Source.volid Then
        '                                    myBmp(i).InUseNow = True
        '                                    MyRch(cur).BMPCnt = MyRch(cur).BMPCnt + 1
        '                                    Exit For
        '                                End If
        '                            Next i
        '                        Else 'non-bmp source
        '                            tcnt = tcnt + 1
        '                            ReDim Preserve myTrib(tcnt)
        '                            myTrib(tcnt).Id = lConn.Source.volid
        '                            myTrib(tcnt).OpNam = lConn.Source.volname
        '                            myTrib(tcnt).Area = lConn.MFact
        '                            myTrib(tcnt).Desc = lConn.Source.Opn.Description
        '                            myTrib(tcnt).BMPCnt = 0
        '                            ReDim myTrib(tcnt).BmpPtr(0)
        '                        End If
        '                    End If
        '                End If
        '            End If
        '        Next vConn

        '        For i = 1 To bcnt 'loop thru bmps to find their tribs
        '            If myBmp(i).InUseNow Then
        '                Id = myBmp(i).Id
        '                lOpnBlk = myUci.OpnBlks("BMPRAC")  'bmp
        '                lOper = lOpnBlk.operfromid(Id)

        '                For Each vConn In myUci.Connections
        '                    lConn = vConn
        '                    If lConn.Typ = 3 Or lConn.Typ = 2 Then 'network or schematic
        '                        If lConn.Target.Opn.Id = lOper.Id And _
        '                           lConn.Target.Opn.Name = lOper.Name Then 'found a source
        '                            If lConn.Source.volname = "BMPRAC" Then
        '                                MsgBox("PROBLEM - BMPs cant go to BMPs in the WinHSPF BMP Editor", vbOKOnly, msgTitle)
        '                                cmdBMP_Click(2) 'close up bmp editing
        '                            End If
        '                            found = False
        '                            For j = 1 To tcnt
        '                                If myTrib(j).OpNam = lConn.Source.volname Then
        '                                    If myTrib(j).Id = lConn.Source.Opn.Id Then
        '                                        'know about this trib
        '                                        found = True
        '                                        Exit For
        '                                    End If
        '                                End If
        '                            Next j
        '                            If Not (found) Then 'add to trib list
        '                                tcnt = tcnt + 1
        '                                j = tcnt
        '                                ReDim Preserve myTrib(j)
        '                                myTrib(j).Desc = lConn.Source.Opn.Description
        '                                myTrib(j).Id = lConn.Source.Opn.Id
        '                                myTrib(j).OpNam = lConn.Source.volname
        '                            End If

        '                            With myTrib(j)
        '                                .BMPCnt = .BMPCnt + 1
        '                                ReDim Preserve .BmpPtr(.BMPCnt)
        '                                .BmpPtr(.BMPCnt).Ind = i
        '                                .BmpPtr(.BMPCnt).Area = lConn.MFact
        '                                .Area = .Area + lConn.MFact
        '                            End With
        '                        End If
        '                    End If
        '                Next vConn
        '            End If
        '        Next i 'bmp

        '        For i = 1 To bcnt
        '            If myBmp(i).InUseNow Then
        '                For j = 1 To UBound(myTrib)
        '                    found = False
        '                    For k = 1 To myTrib(j).BMPCnt
        '                        If myTrib(j).BmpPtr(k).Ind = i Then
        '                            found = True
        '                            Exit For
        '                        ElseIf myTrib(j).BmpPtr(k).Ind > i Then
        '                            With myTrib(j)
        '                                .BMPCnt = .BMPCnt + 1
        '                                ReDim Preserve .BmpPtr(.BMPCnt)
        '                                For h = .BMPCnt To k + 1 Step -1
        '                                    .BmpPtr(h) = .BmpPtr(h - 1)
        '                                Next h
        '                                .BmpPtr(k).Ind = i
        '                                .BmpPtr(k).Area = 0
        '                            End With
        '                            found = True 'we added it
        '                            Exit For
        '                        End If
        '                    Next k
        '                    If Not found Then 'add to end
        '                        With myTrib(j)
        '                            .BMPCnt = .BMPCnt + 1
        '                            ReDim Preserve .BmpPtr(.BMPCnt)
        '                            .BmpPtr(.BMPCnt).Ind = i
        '                            .BmpPtr(.BMPCnt).Area = 0
        '                        End With
        '                    End If
        '                Next j
        '            End If
        '        Next i
        '    End If
    End Sub

    Private Sub BmpDescUpdate(ByVal aBmpIndex As Integer)
        If aBmpIndex > 0 Then
            With agdSource.Source
                curBMPId = Mid(.CellValue(0, .Columns), 6, Len(.CellValue(0, .Columns)) - 4)
                For lBmpIndex As Integer = 1 To pBmps.Count
                    If curBMPId = pBmps(lBmpIndex).Id Then
                        atxBMPId.Text = pBmps(lBmpIndex).Id
                        atxBMPDesc.Text = pBmps(lBmpIndex).Desc
                        Exit For
                    End If
                Next lBmpIndex
                fraBMPDet.Visible = True
            End With
        Else
            curBMPId = 0
            fraBMPDet.Visible = False
        End If
    End Sub

    Private Sub PopulateGrid()
        Dim lBmpCount As Integer = pBmps.Count
        Dim lTribCount As Integer = pTribs.Count
        Dim lCurrentReach As Integer = cboReach.SelectedIndex

        '    grdSrc.Clear()
        agdSource.Source.Rows = 1
        Dim lRchCount As Integer = 0
        agdSource.Source.FixedColumns = 1

        If lCurrentReach = 0 Then
            'summary style grid
            lblContributing.Text = "Summary of Areas by LandUse and Reach"
            agdSource.Source.Columns = 1
            For lRchIndex As Integer = 1 To cboReach.Items.Count - 1
                lRchCount = lRchCount + 1
                agdSource.Source.CellValue(lRchCount, 0) = "R:" & cboReach.Items(lRchIndex)
                GetUciInfo(lRchIndex)
                For lTribIndex As Integer = 0 To lTribCount - 1
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
                            agdSource.Source.CellEditable(lRchCount, lColIndex) = False 'do for whole column
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
            lblContributing.Text = "Contributing Sources to Reach " & pReaches(lCurrentReach).Id & " (" & pReaches(lCurrentReach).Desc & ")"

            agdSource.Source.CellValue(0, 0) = "Source"
            agdSource.Source.CellEditable(0, 0) = False 'do for whole column
            agdSource.Source.CellValue(0, 1) = "Area"
            agdSource.Source.CellEditable(0, 1) = False 'do for whole column
            agdSource.Source.CellValue(0, 2) = "% No BMP"
            agdSource.Source.CellEditable(0, 2) = True 'do for whole column
            'agdSource.Source.ColMax(2) = 100
            'agdSource.Source.ColType(2) = ATCoSng
            agdSource.Source.Columns = 3

            Dim lCountBmpInUse As Integer = 0
            For lBmpIndex As Integer = 1 To lBmpCount
                If pBmps(lBmpIndex).InUseNow And Not (pBmps(lBmpIndex).DeletePending) Then
                    pBmps(lBmpIndex).col = agdSource.Source.Columns
                    agdSource.Source.Columns = agdSource.Source.Columns + 1
                    lCountBmpInUse = lCountBmpInUse + 1
                    agdSource.Source.CellValue(0, 2 + lCountBmpInUse) = "% BMP " & pBmps(lBmpIndex).Id
                    agdSource.Source.CellEditable(1, 2 + lCountBmpInUse) = True   'do for whole column
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
                agdSource.Source.CellEditable(1, 2) = False 'do for whole column
            Else
                'grdSrc.ColMin(2) = 0
                fraBMPDet.Visible = True
                agdSource.Source.CellEditable(1, 2) = True 'do for whole column
            End If

            'agdSource.Source.ColWidth(0) = agdSource.Source.Width * 0.4
            'agdSource.Source.ColWidth(1) = (agdSource.Source.Width - agdSource.Source.ColWidth(0) - 40) / CSng(2 + buse)
            'For i as integer = 2 To 2 + buse
            'agdSource.Source.ColWidth(i) = agdSource.Source.ColWidth(1)
            'Next i

            For lTribIndex As Integer = 1 To lTribCount 'trib source areas and %
                If pTribs(lTribIndex).OpNam <> "BMPRAC" Then
                    lRchCount = lRchCount + 1
                    agdSource.Source.Rows = lRchCount
                    Dim lTxt As String = pTribs(lTribIndex).OpNam & " : " & pTribs(lTribIndex).Id & " (" & pTribs(lTribIndex).Desc & ")"
                    agdSource.Source.CellValue(lRchCount, 0) = lTxt
                    Dim lBmpArea As Double = 0
                    If lTxt.StartsWith("RCH") Then
                        agdSource.Source.CellValue(lTribIndex, 1) = "NA"
                    Else
                        agdSource.Source.CellValue(lTribIndex, 1) = pTribs(lTribIndex).Area
                    End If
                    lCountBmpInUse = 0
                    Dim lRArea As Double = 0.0
                    For lBmpIndex As Integer = 1 To lBmpCount
                        If pBmps(lBmpIndex).InUseNow And Not (pBmps(lBmpIndex).DeletePending) Then
                            lCountBmpInUse = lCountBmpInUse + 1
                            If lCountBmpInUse <= pTribs(lTribIndex).BMPCnt Then
                                lRArea = pTribs(lTribIndex).BmpPtr(lCountBmpInUse).Area
                            Else
                                lRArea = 0
                            End If
                            agdSource.Source.CellValue(lTribIndex, 2 + lCountBmpInUse) = 100 * (lRArea / pTribs(lTribIndex).Area)
                            lBmpArea = lBmpArea + lRArea
                        End If
                    Next lBmpIndex
                    agdSource.Source.CellValue(lTribIndex, 2) = 100 - 100 * (lBmpArea / pTribs(lTribIndex).Area)
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
        'Dim i&, bcnt&

        'If atxBMPId.Enabled = True Then
        '    bcnt = UBound(myBmp)
        '    For i = 1 To bcnt
        '        If myBmp(i).Id = atxBMPId.Value Then
        '            MsgBox("BMP id " & myBmp(i).Id & " is already in use" & vbCrLf & _
        '                    "Try another ID number", vbOKOnly, msgTitle)
        '            atxBMPId.Value = curBMPId
        '            Exit Sub
        '        End If
        '    Next i

        '    curBMPId = atxBMPId.Value

        '    grdSrc.ColTitle(grdSrc.col) = "% BMP " & curBMPId
        '    myBmp(grdSrc.col - 2).Id = curBMPId

        '    cmdUpU.Enabled = True
        'End If
    End Sub

    Private Sub cboReach_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboReach.Click
        'If cmbReach.ListIndex = 0 Then
        '    rdoOpt(0).Visible = False 'True
        '    rdoOpt(1).Visible = False 'True
        'Else
        '    rdoOpt(0).Visible = False
        '    rdoOpt(1).Visible = False
        'End If
        'Call GetUciInfo(cmbReach.ListIndex)
        'Call PopulateGrid()
        'cmdUpU.Enabled = False
        'atxBMPId.Enabled = False
    End Sub

    Private Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
        'Dim ans&, i&, j&, k&, cur&, nth&, ncnt&, curbmp&
        'Dim done As Boolean

        'cur = cmbReach.ListIndex

        '    i = UBound(myBmp) + 1
        '    ReDim Preserve myBmp(i)
        '    ' assign an id not in use
        '    j = 0
        '    done = False
        '    Do Until done
        '        done = True ' assume the best
        '        For k = 1 To i - 1
        '            If myBmp(k).Id = MyRch(cur).Id + j Then 'in use
        '                j = j + 1
        '                done = False
        '                Exit For
        '            End If
        '        Next k
        '    Loop

        '    myBmp(i).Id = MyRch(cur).Id + j
        '    myBmp(i).Desc = "New BMP"
        '    myBmp(i).InUseNow = True
        '    For j = 1 To UBound(myTrib)
        '        myTrib(j).BMPCnt = myTrib(j).BMPCnt + 1
        '        k = myTrib(j).BMPCnt
        '        ReDim Preserve myTrib(j).BmpPtr(k)
        '        myTrib(j).BmpPtr(k).Area = 0
        '        myTrib(j).BmpPtr(k).Ind = i
        '    Next j
        '    MyRch(cur).BMPCnt = MyRch(cur).BMPCnt + 1
        '    Call PopulateGrid()
        '    cmdUpU.Enabled = True
        '    atxBMPId.Enabled = True
    End Sub

    Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        'Dim ans&, i&, j&, k&, cur&, nth&, ncnt&, curbmp&
        'Dim done As Boolean

        'cur = cmbReach.ListIndex

        'If MyRch(cur).BMPCnt = 0 Then
        '    MsgBox("No BMPs are associated with Reach " & MyRch(cur).Id, _
        '           vbExclamation, msgTitle)
        'ElseIf grdSrc.col > 2 Then 'are in a bmp
        '    'delete nth bmp
        '    nth = grdSrc.col - 2
        '    ncnt = 0
        '    For j = 1 To UBound(myBmp)
        '        If myBmp(j).InUseNow = True Then
        '            ncnt = ncnt + 1
        '            If nth = ncnt Then
        '                'this is the one to delete
        '                curbmp = myBmp(j).Id
        '                nth = j
        '            End If
        '        End If
        '    Next j
        '    curbmp = atxBMPId.Value
        '    ans = MsgBox("Are you sure you want to Delete BMP " & curbmp & "?", _
        '           vbYesNo, msgTitle)
        '    If ans = vbYes Then
        '        myBmp(nth).DeletePending = True
        '        myBmp(nth).InUseNow = False
        '        MyRch(cur).BMPCnt = MyRch(cur).BMPCnt - 1
        '        For j = 1 To UBound(myTrib)
        '            'adjust here
        '        Next j
        '        grdSrc.cols = grdSrc.cols - 1
        '        grdSrc.col = 2
        '        Call PopulateGrid()
        '        cmdUpU.Enabled = True
        '    End If
        'End If
    End Sub

    Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        'If cmdUpU.Enabled = True Then
        '    ans = MsgBox("You have changes to your UCI made which have not been saved. " & vbCrLf & _
        '                 "OK trashes them, Cancel allows you a chance to update your UCI.", _
        '                 vbOKCancel, msgTitle)
        'Else 'no changes pending
        '    ans = vbOK
        'End If

        'If ans = vbOK Then
        '    Unload(Me)
        'End If
    End Sub

    Private Sub cmdUpdateUCI_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpdateUCI.Click
        'Dim i&, j&, k&, ret&, rchid&, c&, bmpId&, itmnam$
        'Dim lOpn As HspfOperation, addit As Boolean, a#
        'Dim lOpnBlk As HspfOpnBlk, addbefore&
        'Dim vConn As Object
        'Dim lConn As HspfConnection
        'Dim lTable As HspfTable

        'rchid = MyRch(cmbReach.ListIndex).Id

        'atxBMPId.Enabled = False
        'For i = 1 To UBound(myBmp) 'process deletes first
        '    If myBmp(i).DeletePending Then 'get rid of it
        '        myUci.DeleteOperation("BMPRAC", myBmp(i).Id)
        '        myBmp(i).InUseNow = False
        '    End If
        'Next i

        ''put areas back for the bmps that are in use
        'For i = 1 To UBound(myBmp)
        '    If myBmp(i).InUseNow Then

        '        'see if we need to add this bmp to opn seq block
        '        lOpnBlk = myUci.OpnBlks("BMPRAC")
        '        addit = True
        '        If lOpnBlk.Count > 0 Then
        '            lOpn = lOpnBlk.operfromid(myBmp(i).Id)
        '            If Not lOpn Is Nothing Then addit = False
        '        End If
        '        If addit Then
        '            'add new bmprac operation
        '            myUci.AddOperation("BMPRAC", myBmp(i).Id)
        '            'figure out where to put it in opn seq block
        '            addbefore = myUci.OpnSeqBlock.Opns.Count
        '            For j = 1 To myUci.OpnSeqBlock.Opns.Count
        '                If myUci.OpnSeqBlock.Opn(j).Name = "RCHRES" And _
        '                   myUci.OpnSeqBlock.Opn(j).Id = rchid Then
        '                    addbefore = j
        '                End If
        '            Next j
        '            'now add to opn seq block
        '            lOpn = lOpnBlk.operfromid(myBmp(i).Id)
        '            lOpn.Description = myBmp(i).Desc
        '            lOpn.Uci = myUci
        '            lOpn.OpnBlk = lOpnBlk
        '            myUci.OpnSeqBlock.addbefore(lOpn, addbefore)
        '            If lOpnBlk.Count > 1 Then
        '                'already have some of this operation
        '                For Each lTable In lOpnBlk.Ids(1).tables
        '                    'add this opn id to this table
        '                    myUci.AddTable("BMPRAC", myBmp(i).Id, lTable.Name)
        '                Next lTable
        '            Else
        '                'added the first bmprac, need to add associated tables
        '                Dim lBlock As HspfBlockDef
        '                Dim vSection As Object, lSection As HspfSectionDef
        '                Dim vTable As Object, lTabledef As HspfTableDef
        '                lBlock = myMsg.BlockDefs("BMPRAC")
        '                For Each vSection In lBlock.SectionDefs
        '                    lSection = vSection
        '                    For Each vTable In lSection.TableDefs
        '                        lTabledef = vTable
        '                        myUci.AddTable("BMPRAC", myBmp(i).Id, lTabledef.Name)
        '                    Next vTable
        '                Next vSection
        '            End If
        '            lTable = lOpnBlk.tables("GEN-INFO")
        '            lTable.Parms("BMPID").Value = myBmp(i).Desc
        '            lTable.Parms("NGQUAL").Value = 0   'assume no gquals
        '            lTable = lOpnBlk.tables("GQ-FRAC")
        '            lTable.Parms("GQID").Value = "unknown"
        '        End If

        '        'look for bmp to rchres connection, add it if not existing
        '        Call PutSchematicRecord("BMPRAC", myBmp(i).Id, "RCHRES", rchid, 1.0#)
        '    End If
        'Next i

        'For j = 1 To UBound(myTrib)
        '    'put area going directly to rch
        '    a = myTrib(j).Area * CSng((grdSrc.TextMatrix(j, 2) / 100))
        '    Call PutSchematicRecord(myTrib(j).OpNam, myTrib(j).Id, "RCHRES", rchid, a)
        '    'put area going to bmps
        '    For k = 1 To myTrib(j).BMPCnt
        '        If myBmp(myTrib(j).BmpPtr(k).Ind).InUseNow Then
        '            bmpId = myBmp(myTrib(j).BmpPtr(k).Ind).Id
        '            c = myBmp(myTrib(j).BmpPtr(k).Ind).col
        '            a = myTrib(j).Area * CSng((grdSrc.TextMatrix(j, c) / 100))
        '            Call PutSchematicRecord(myTrib(j).OpNam, myTrib(j).Id, "BMPRAC", bmpId, a)
        '        End If
        '    Next k
        'Next j

        'Call GetUciInfo(cmbReach.ListIndex) 'refresh with new data
        'Call PopulateGrid()
        'cmdUpU.Enabled = False
    End Sub

    Private Sub agdSource_CellEdited(ByVal aGrid As atcControls.atcGrid, ByVal aRow As Integer, ByVal aColumn As Integer) Handles agdSource.CellEdited
        'Dim t!, i&, r&, c&, a&, cNow&

        'c = ChangeToCol

        'For r = ChangeFromRow To ChangeToRow
        '    t = grdSrc.TextMatrix(r, c) 'highest priority is just set value

        '    cNow = 2
        '    For i = 1 To UBound(myBmp) ' each bmp
        '        If myBmp(i).InUseNow And Not (myBmp(i).DeletePending) Then
        '            cNow = cNow + 1
        '            If cNow <> c Then 'dont double count current
        '                t = t + grdSrc.TextMatrix(r, cNow)
        '                If t > 100 Then
        '                    grdSrc.TextMatrix(r, cNow) = grdSrc.TextMatrix(r, cNow) - t + 100
        '                    t = 100
        '                End If
        '            End If
        '        End If
        '    Next i

        '    If t < 100 Then 'need some more
        '        If c = 2 Then ' changing no bmp col, adjust first bmp
        '            grdSrc.TextMatrix(r, 3) = 100 - (t + grdSrc.TextMatrix(r, 3))
        '        Else 'changing a bmp, adjust no bmp
        '            grdSrc.TextMatrix(r, 2) = 100 - t
        '        End If
        '    ElseIf c > 2 Then
        '        grdSrc.TextMatrix(r, 2) = 100 - t
        '    End If
        'Next r

        'cmdUpU.Enabled = True
    End Sub

    'Private Sub grdSrc_RowColChange()
    '    If grdSrc.col >= 2 And Left(grdSrc.Header, 7) <> "Summary" Then
    '        BmpDescUpdate(grdSrc.col - 2)
    '    End If
    'End Sub

    Private Sub atxBMPDesc_Change() Handles atxBMPDesc.Change
        'Dim i&
        'For i = 1 To UBound(myBmp)
        '    If myBmp(i).InUseNow Then
        '        myBmp(i).Desc = txtBMPDesc.Text
        '    End If
        'Next i
        'cmdUpU.Enabled = True
    End Sub

    Private Sub PutSchematicRecord(ByVal sname$, ByVal sid&, ByVal tname$, ByVal tid&, ByVal multfact#)
        'Dim addit As Boolean, mlid&, deleteit As Boolean
        'Dim vConn As Object, deleteindex&
        'Dim lConn As HspfConnection
        'Dim lOpnBlk As HspfOpnBlk, i&
        'Dim lOpn As HspfOperation, tempOpn As HspfOperation

        'If sname = "RCHRES" And tname = "BMPRAC" Then 'dont do rchres to bmp connections
        'Else
        '    If sname = "RCHRES" And tname = "RCHRES" Then
        '        multfact = 1.0#
        '    End If
        '    addit = True
        '    deleteit = False
        '    For i = 1 To myUci.Connections.Count
        '        lConn = myUci.Connections(i)
        '        If lConn.Typ = 3 Then 'schematic
        '            If lConn.Target.Opn.Id = tid And _
        '               lConn.Target.Opn.Name = tname And _
        '               lConn.Source.Opn.Id = sid And _
        '               lConn.Source.Opn.Name = sname Then
        '                addit = False
        '                lConn.MFact = multfact
        '                If Abs(multfact) < 0.00000001 Then
        '                    deleteit = True
        '                    deleteindex = i
        '                End If
        '            End If
        '        End If
        '    Next i
        '    If addit And Abs(multfact) > 0.00000001 Then 'need to add the connection
        '        lConn = New HspfConnection
        '        lOpnBlk = myUci.OpnBlks(sname)
        '        lOpn = lOpnBlk.operfromid(sid)
        '        lConn.Source.Opn = lOpn
        '        lConn.Source.volname = lOpn.Name
        '        lConn.Source.volid = lOpn.Id
        '        lConn.Typ = 3
        '        lConn.MFact = multfact
        '        lOpnBlk = myUci.OpnBlks(tname)
        '        lOpn = lOpnBlk.operfromid(tid)
        '        lConn.Target.Opn = lOpn
        '        lConn.Target.volname = lOpn.Name
        '        lConn.Target.volid = lOpn.Id
        '        Call GetMassLinkID(sname, tname, mlid)
        '        If mlid = 0 Then
        '            Call AddMassLink(sname, tname, mlid)
        '        End If
        '        lConn.MassLink = mlid
        '        lConn.Uci = myUci
        '        'add targets to source opn
        '        lOpnBlk = myUci.OpnBlks(sname)
        '        lOpn = lOpnBlk.operfromid(sid)
        '        lOpn.targets.Add(lConn)
        '        lConn.Source.Opn = lOpn
        '        'add sources to target opn
        '        lOpnBlk = myUci.OpnBlks(tname)
        '        lOpn = lOpnBlk.operfromid(tid)
        '        lOpn.Sources.Add(lConn)
        '        lConn.Target.Opn = lOpn
        '        'add to collection of connections
        '        myUci.Connections.Add(lConn)
        '    ElseIf deleteit Then 'need to delete the connection
        '        myUci.Connections.Remove(deleteindex)
        '        'remove target from source opn
        '        lOpnBlk = myUci.OpnBlks(sname)
        '        lOpn = lOpnBlk.operfromid(sid)
        '        i = 1
        '        For Each vConn In lOpn.targets
        '            lConn = vConn
        '            If lConn.Target.volname = tname And _
        '               lConn.Target.volid = tid Then
        '                lOpn.targets.Remove(i)
        '            Else
        '                i = i + 1
        '            End If
        '        Next vConn
        '        'remove source from target opn
        '        lOpnBlk = myUci.OpnBlks(tname)
        '        lOpn = lOpnBlk.operfromid(tid)
        '        i = 1
        '        For Each vConn In lOpn.Sources
        '            lConn = vConn
        '            If lConn.Source.volname = sname And _
        '               lConn.Source.volid = sid Then
        '                lOpn.Sources.Remove(i)
        '            Else
        '                i = i + 1
        '            End If
        '        Next vConn
        '    End If
        'End If
    End Sub

    Private Sub GetMassLinkID(ByVal sname$, ByVal tname$, ByVal mlid&)
        'Dim lConn As HspfConnection

        ''determine mass link number
        'mlid = 0
        'For Each lConn In myUci.Connections
        '    If lConn.Typ = 3 Then
        '        If lConn.Source.volname = sname And lConn.Target.volname = tname Then
        '            mlid = lConn.MassLink
        '        End If
        '    End If
        'Next lConn
    End Sub

    Private Sub AddMassLink(ByVal sname$, ByVal tname$, ByVal mlid&)
        'Dim lOpn As HspfOperation
        'Dim cOpns As Collection 'of hspfOperations
        'Dim i&, j&, found As Boolean
        'Dim pml&, iml&, ostr$(10)
        'Dim lConn As HspfConnection, lMassLink As HspfMassLink
        'Dim cMassLink As HspfMassLink, copyid&

        ''need to add masslink, find an unused number
        'found = True
        'mlid = 1
        'Do Until found = False
        '    found = False
        '    For Each lMassLink In myUci.MassLinks
        '        If lMassLink.MassLinkID = mlid Then
        '            mlid = mlid + 1
        '            found = True
        '            Exit For
        '        End If
        '    Next lMassLink
        'Loop
        ''find id of masslink to copy
        'copyid = 0
        'If sname = "BMPRAC" And tname = "RCHRES" Then
        '    'copy from perlnd to rchres masslink
        '    Call GetMassLinkID("PERLND", tname, copyid)
        'ElseIf sname = "PERLND" And tname = "BMPRAC" Then
        '    'copy from perlnd to rchres masslink
        '    Call GetMassLinkID(sname, "RCHRES", copyid)
        'ElseIf sname = "IMPLND" And tname = "BMPRAC" Then
        '    'copy from implnd to rchres masslink
        '    Call GetMassLinkID(sname, "RCHRES", copyid)
        'End If
        'If mlid > 0 And copyid > 0 Then
        '    'now copy masslink
        '    For Each cMassLink In myUci.MassLinks
        '        If cMassLink.MassLinkID = copyid Then
        '            'copy this record
        '            lMassLink = New HspfMassLink
        '            lMassLink.Uci = myUci
        '            lMassLink.MassLinkID = mlid
        '            lMassLink.Source.volname = sname
        '            lMassLink.Source.volid = 0
        '            lMassLink.Source.group = cMassLink.Source.group
        '            lMassLink.Source.member = cMassLink.Source.member
        '            lMassLink.Source.memsub1 = cMassLink.Source.memsub1
        '            lMassLink.Source.memsub2 = cMassLink.Source.memsub2
        '            lMassLink.MFact = cMassLink.MFact
        '            lMassLink.Tran = cMassLink.Tran
        '            lMassLink.Target.volname = tname
        '            lMassLink.Target.volid = 0
        '            lMassLink.Target.group = cMassLink.Target.group
        '            lMassLink.Target.member = cMassLink.Target.member
        '            lMassLink.Target.memsub1 = cMassLink.Target.memsub1
        '            lMassLink.Target.memsub2 = cMassLink.Target.memsub2

        '            If (sname = "PERLND" Or sname = "IMPLND") And _
        '              tname = "BMPRAC" Then  'special cases
        '                If cMassLink.Target.member = "OXIF" Then
        '                    lMassLink.Target.member = "IOX"
        '                ElseIf cMassLink.Target.member = "NUIF1" Then
        '                    lMassLink.Target.member = "IDNUT"
        '                ElseIf cMassLink.Target.member = "NUIF2" Then
        '                    lMassLink.Target.member = "ISNUT"
        '                ElseIf cMassLink.Target.member = "PKIF" Then
        '                    lMassLink.Target.member = "IPLK"
        '                End If
        '            End If

        '            If sname = "BMPRAC" And tname = "RCHRES" Then
        '                'special cases
        '                lMassLink.Source.group = "ROFLOW"
        '                lMassLink.MFact = 1.0#
        '                If cMassLink.Target.member = "IVOL" Then
        '                    lMassLink.Source.member = "ROVOL"
        '                ElseIf cMassLink.Target.member = "CIVOL" Then
        '                    lMassLink.Source.member = "CROVOL"
        '                ElseIf cMassLink.Target.member = "ICON" Then
        '                    lMassLink.Source.member = "ROCON"
        '                ElseIf cMassLink.Target.member = "IHEAT" Then
        '                    lMassLink.Source.member = "ROHEAT"
        '                ElseIf cMassLink.Target.member = "ISED" Then
        '                    lMassLink.Source.member = "ROSED"
        '                ElseIf cMassLink.Target.member = "IDQAL" Then
        '                    lMassLink.Source.member = "RODQAL"
        '                ElseIf cMassLink.Target.member = "ISQAL" Then
        '                    lMassLink.Source.member = "ROSQAL"
        '                ElseIf cMassLink.Target.member = "OXIF" Then
        '                    lMassLink.Source.member = "ROOX"
        '                ElseIf cMassLink.Target.member = "NUIF1" Then
        '                    lMassLink.Source.member = "RODNUT"
        '                ElseIf cMassLink.Target.member = "NUIF2" Then
        '                    lMassLink.Source.member = "ROSNUT"
        '                ElseIf cMassLink.Target.member = "PKIF" Then
        '                    lMassLink.Source.member = "ROPLK"
        '                ElseIf cMassLink.Target.member = "PHIF" Then
        '                    lMassLink.Source.member = "ROPH"
        '                End If
        '                lMassLink.Source.memsub1 = cMassLink.Target.memsub1
        '                lMassLink.Source.memsub2 = cMassLink.Target.memsub2
        '            End If

        '            myUci.MassLinks.Add(lMassLink)
        '        End If
        '    Next cMassLink
        'End If

    End Sub
End Class