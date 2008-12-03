Imports atcControls
Imports atcData
Imports atcUCI
Imports atcUCIForms
Imports atcUtility
Imports MapWinUtility
Imports WinHSPF
Imports System.Collections.ObjectModel

Public Class frmPoint

    Dim DoneBuild As Boolean
    'Dim lts As Collection(Of atcData.atcTimeseries)
    Dim lts As Collection
    '.net conversion issue: tsl was formelry ATCoTSlist
    Dim WithEvents tsl As atcTimeseries

    Dim InUseFacs() As String, CountInUseFacs&
    Dim AvailFacs() As String = {""}
    Dim CountAvailFacs As Integer
    Dim ConsLinks() As String, MemberLinks() As String, LinkCount&
    Dim MSub1Links() As Long, MSub2Links() As Long

    Private HsashDragging As Boolean
    Private HsashDragStart As Single

    Public Sub New()
        Dim LinkCount As Integer
        Dim i As Integer

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        Me.Icon = pIcon
        lstPoints.SelectionMode = SelectionMode.One
        ExpandedView(False)

        grpDetails.Text = "Details of " & lstPoints.SelectedItem


        With agdMasterPoint
            .Source = New atcControls.atcGridSource
            .Clear()
            .AllowHorizontalScrolling = False
            .AllowNewValidValues = True
            .Visible = True
        End With

        With agdPoint
            .Source = New atcControls.atcGridSource
            .Clear()
            .AllowHorizontalScrolling = False
            .AllowNewValidValues = True
            .Visible = True
        End With


        'always link flow to ivol
        LinkCount = 1
        ReDim ConsLinks(LinkCount)
        ReDim MemberLinks(LinkCount)
        ReDim MSub1Links(LinkCount)
        ReDim MSub2Links(LinkCount)
        ConsLinks(0) = "FLOW"
        MemberLinks(0) = "IVOL"
        MSub1Links(0) = 0
        MSub2Links(0) = 0

        agdPoint.Source = New atcControls.atcGridSource
        agdMasterPoint.Source = New atcControls.atcGridSource

        DoneBuild = False
        SetGrid(agdPoint)
        SetGrid(agdMasterPoint)
        FillMasterGrid()
        DoneBuild = True
        '.net conversion issue: tsl formerly atcTSList
        Dim tsl As New List(Of atcTimeseries)

        CountInUseFacs = lstPoints.CheckedItems.Count

        For i = 0 To lstPoints.Items.Count - 1
            If lstPoints.GetItemChecked(i) Then
                ReDim InUseFacs(UBound(InUseFacs))
                InUseFacs(UBound(InUseFacs)) = lstPoints.Items.Item(i)
            Else
                ReDim AvailFacs(UBound(AvailFacs))
                AvailFacs(UBound(AvailFacs)) = lstPoints.Items.Item(i)
            End If
        Next

        agdMasterPoint.SizeAllColumnsToContents()
        agdMasterPoint.Refresh()

        agdPoint.SizeAllColumnsToContents()
        agdPoint.Refresh()


        AddHandler chkAllSources.CheckStateChanged, AddressOf chkAllSources_CheckedChanged
        AddHandler lstPoints.ItemCheck, AddressOf lstSources_IndividualCheckChanged

    End Sub

    Private Sub FillMasterGrid()
        Dim lOper As HspfOperation
        Dim lOpnBlk As HspfOpnBlk
        Dim vpol As Object
        Dim i&, j&, lloc$, lsen$, icnt&, k&, lfac$
        Dim lcon$, lpol$, S$, dsncnt&
        Dim dsnptr() As Integer = Nothing
        Dim ifound As Boolean
        Dim activeflag As Boolean
        Dim lPointItemIndex As Integer

        lstPoints.Items.Clear()
        'pUCI.Source2Point()
        'pUCI.Point2Source()
        'pUCI.PollutantsBuild()

        lts = pUCI.FindTimser("", "", "")

        With agdMasterPoint.Source
            .Rows = 1

            lOpnBlk = pUCI.OpnBlks("RCHRES")
            icnt = 1

            For i = 1 To lts.Count - 1
                lsen = lts(i).Attributes.GetValue("Scenario")
                If Mid(lsen, 1, 3) = "PT-" Then 'this is a pt src
                    lloc = lts(i).Attributes.GetValue("Location")
                    If IsNumeric(Mid(lloc, 4)) Then
                        'get full reach name
                        lOper = lOpnBlk.OperFromID(CInt(Mid(lloc, 4)))
                        If Not lOper Is Nothing Then

                            'found a reach with this id

                            'TEMPORARY BANDAID
                            lfac = "WESTERN WWTP"


                            lcon = lts(i).Attributes.GetValue("Constituent")
                            S = lfac & " (" & Mid(lts(i).Attributes.GetValue("Scenario"), 4) & ")"

                            If Not lstPoints.Items.Contains(S) Then
                                lstPoints.Items.Add(S, False)
                            End If

                            'see how many times this dsn shows up in pt srcs
                            dsncnt = 0
                            activeflag = False
                            If Not pUCI.PointSources Is Nothing Then
                                For j = 1 To pUCI.PointSources.Count
                                    If pUCI.PointSources(j).Target.VolName = lOper.Name And _
                                       pUCI.PointSources(j).Target.VolId = lOper.Id And _
                                       Microsoft.VisualBasic.Left(pUCI.PointSources(j).Source.VolName, 3) = lts(i).File.ToString And pUCI.PointSources(j).Source.VolId = lts(i).Attributes.GetValue("Id") Then
                                        'found this dsn in active point sources
                                        dsncnt = dsncnt + 1
                                        activeflag = True
                                        ReDim Preserve dsnptr(dsncnt)
                                        dsnptr(dsncnt) = j


                                        For lPointItemIndex = 0 To lstPoints.Items.Count - 1
                                            If lstPoints.Items.Item(lPointItemIndex) = S AndAlso lstPoints.GetItemChecked(lPointItemIndex) Then
                                                lstPoints.SetItemChecked(lPointItemIndex, True)
                                            End If
                                        Next
                                    End If
                                Next
                                If activeflag = False Then
                                    'still add a line for this dsn
                                    dsncnt = 1
                                End If

                                For j = 1 To dsncnt
                                    If icnt > 1 Then .Rows = .Rows + 1
                                    If activeflag = False Then
                                        'not an active point source
                                        .CellValue(icnt, 0) = "No"
                                        .CellValue(icnt, 9) = "INFLOW"
                                        '.CellValue(icnt, 10) = "IVOL"
                                        '.CellValue(icnt, 12) = 0
                                        '.CellValue(icnt, 13) = 0
                                    Else
                                        'this is an active point source
                                        .CellValue(icnt, 0) = "Yes"
                                        .CellValue(icnt, 9) = pUCI.PointSources(dsnptr(j)).Target.Group
                                        .CellValue(icnt, 10) = MemberLongVersion(pUCI.PointSources(dsnptr(j)).Target.Member, pUCI.PointSources(dsnptr(j)).Target.MemSub1, pUCI.PointSources(dsnptr(j)).Target.MemSub2)
                                        '.CellValue(icnt, 12) = myUci.PointSources(dsnptr(j)).Target.memsub1
                                        '.CellValue(icnt, 13) = myUci.PointSources(dsnptr(j)).Target.memsub2
                                    End If

                                    .CellValue(icnt, 1) = lsen
                                    .CellValue(icnt, 2) = lloc
                                    '.net conversion issue: TEMPORARY BANDAID
                                    '.CellValue(icnt, 3) = UCase(lts(i).Header.Desc)
                                    .CellValue(icnt, 3) = lfac
                                    '.net conversion issue: TEMPORARY BANDAID
                                    '.CellValue(icnt, 5) = pUCI.GetWDMIdFromName(lts(i).File.Filename)  'save assoc src vol name
                                    .CellValue(icnt, 5) = lts(i).Attributes.GetValue("DataSource")  'save assoc src vol name
                                    .CellValue(icnt, 6) = lts(i).Attributes.GetValue("Id")     'save assoc src vol id
                                    .CellValue(icnt, 7) = lOper.Name     'save assoc tar vol name
                                    .CellValue(icnt, 8) = lOper.Id         'save assoc tar vol id
                                    .CellValue(icnt, 11) = i 'save index to lts

                                    'look for this con in pollutant list
                                    For Each vpol In pUCI.Pollutants
                                        lpol = vpol
                                        If Mid(lcon, 1, 5) = Mid(lpol, 1, 5) Then
                                            lcon = lpol
                                            Exit For
                                        End If
                                    Next vpol
                                    .CellValue(icnt, 4) = lcon

                                    .CellValue(icnt, 14) = icnt 'save row number

                                    'default member based on constituent name if poss
                                    If activeflag Then
                                        'is active, see if we want to remember link
                                        ifound = False
                                        For k = 1 To LinkCount
                                            If ConsLinks(k - 1) = UCase(Trim(lcon)) Then
                                                ifound = True
                                                Exit For
                                            End If
                                        Next k
                                        If Not ifound Then
                                            'add this to list
                                            LinkCount = LinkCount + 1
                                            ReDim Preserve ConsLinks(LinkCount)
                                            ReDim Preserve MemberLinks(LinkCount)
                                            ReDim Preserve MSub1Links(LinkCount)
                                            ReDim Preserve MSub2Links(LinkCount)
                                            ConsLinks(LinkCount - 1) = UCase(Trim(lcon))
                                            MemberLinks(LinkCount - 1) = MemberFromLongVersion(.CellValue(icnt, 10))
                                            MSub1Links(LinkCount - 1) = MemSub1FromLongVersion(.CellValue(icnt, 10))
                                            MSub2Links(LinkCount - 1) = MemSub2FromLongVersion(.CellValue(icnt, 10))
                                        End If
                                    End If

                                    icnt = icnt + 1
                                Next j
                            End If
                        End If
                    End If
                End If
            Next i

            'agdMasterPoint.SizeAllColumnsToContents()
            ''set default members for all
            'For i = 1 To .Rows
            '    For k = 1 To LinkCount
            '        If ConsLinks(k - 1) = UCase(Trim(.CellValue(i, 4))) Then
            '            .CellValue(i, 10) = MemberLongVersion(MemberLinks(k - 1), MSub1Links(k - 1), MSub2Links(k - 1))
            '            '.TextMatrix(i, 12) = MSub1Links(K - 1)
            '            '.TextMatrix(i, 13) = MSub2Links(K - 1)
            '            Exit For
            '    End If
            'Next k
            'Next i
            'If icnt = 1 Then .Rows = 0

        End With

    End Sub

    Private Function MemberFromLongVersion(ByVal S$) As String
        Dim i&
        i = InStr(1, S, "(")
        If i > 0 Then
            MemberFromLongVersion = Mid(S, 1, i - 1)
        Else
            MemberFromLongVersion = S
        End If
    End Function

    Private Function MemSub1FromLongVersion(ByVal S$) As Long
        Dim i&, j&
        i = InStr(1, S, "(")
        If i > 0 Then
            j = InStr(1, S, ",")
            If j = 0 Then
                j = InStr(1, S, ")")
            End If
            MemSub1FromLongVersion = CInt(Mid(S, i + 1, j - i - 1))
        Else
            MemSub1FromLongVersion = 0
        End If
    End Function

    Private Function MemSub2FromLongVersion(ByVal S$) As Long
        Dim i&, j&
        i = InStr(1, S, ",")
        If i > 0 Then
            j = InStr(1, S, ")")
            MemSub2FromLongVersion = CInt(Mid(S, i + 1, j - i - 1))
        Else
            MemSub2FromLongVersion = 0
        End If
    End Function

    Private Function MemberLongVersion(ByVal mem$, ByVal sub1&, ByVal sub2&) As String
        Dim S$
        S = mem
        If sub1 > 0 Then
            S = S & "(" & sub1
            If sub2 > 0 Then
                S = S & "," & sub2 & ")"
            Else
                S = S & ")"
            End If
        End If
        If InStr(1, S, "|") = 0 Then
            MemberLongVersion = S & " | " & DescriptionFromMemberSubs(mem, sub1, sub2)
        Else
            MemberLongVersion = S
        End If
    End Function

    Private Function DescriptionFromMemberSubs(ByVal lmem As String, ByVal sub1&, ByVal sub2&)
        Dim lOpnBlk As HspfOpnBlk
        Dim lOpn As HspfOperation
        Dim lTable As HspfTable

        DescriptionFromMemberSubs = Nothing

        If lmem = "IVOL" Then
            DescriptionFromMemberSubs = "water"
        ElseIf lmem = "CIVOL" Then
            DescriptionFromMemberSubs = "water for category " & CStr(sub1)
        ElseIf lmem = "ICON" Then
            DescriptionFromMemberSubs = "conservative"
        ElseIf lmem = "IHEAT" Then
            DescriptionFromMemberSubs = "heat"
        ElseIf lmem = "ISED" Then
            Select Case sub1
                Case 1 : DescriptionFromMemberSubs = "sand"
                Case 2 : DescriptionFromMemberSubs = "silt"
                Case 3 : DescriptionFromMemberSubs = "clay"
            End Select
        ElseIf lmem = "IDQAL" Then
            lOpnBlk = pUCI.OpnBlks("RCHRES")
            DescriptionFromMemberSubs = "dissolved gqual"
            If Not lOpnBlk Is Nothing Then
                lOpn = lOpnBlk.Ids(1)
                If Not lOpn Is Nothing Then
                    If sub1 = 1 Or sub1 = 0 Then
                        lTable = lOpn.Tables("GQ-QALDATA")
                    Else
                        lTable = lOpn.Tables("GQ-QALDATA:" & CStr(sub1))
                    End If
                    If Not lTable Is Nothing Then
                        '.net conversion issue: Converted lTable.Parms("GQID") to string with .ToString
                        DescriptionFromMemberSubs = "dissolved " & Trim(lTable.Parms("GQID").ToString)
                    End If
                End If
            End If
        ElseIf lmem = "ISQAL" Then
            Select Case sub1
                Case 1 : DescriptionFromMemberSubs = "sand associated "
                Case 2 : DescriptionFromMemberSubs = "silt associated "
                Case 3 : DescriptionFromMemberSubs = "clay associated "
            End Select
            lOpnBlk = pUCI.OpnBlks("RCHRES")
            If Not lOpnBlk Is Nothing Then
                lOpn = lOpnBlk.Ids(1)
                If Not lOpn Is Nothing Then
                    If sub2 = 1 Then
                        lTable = lOpn.Tables("GQ-QALDATA")
                    Else
                        lTable = lOpn.Tables("GQ-QALDATA:" & CStr(sub2))
                    End If
                    If Not lTable Is Nothing Then
                        '.net conversion issue: Converted lTable.Parms("GQID") to string with .ToString
                        DescriptionFromMemberSubs = DescriptionFromMemberSubs & Trim(lTable.Parms("GQID").ToString)
                    End If
                End If
            End If
        ElseIf lmem = "OXIF" Then
            Select Case sub1
                Case 1 : DescriptionFromMemberSubs = "do"
                Case 2 : DescriptionFromMemberSubs = "bod"
            End Select
        ElseIf lmem = "NUIF1" Then
            Select Case sub1
                Case 1 : DescriptionFromMemberSubs = "no3"
                Case 2 : DescriptionFromMemberSubs = "tam"
                Case 3 : DescriptionFromMemberSubs = "no2"
                Case 4 : DescriptionFromMemberSubs = "po4"
            End Select
        ElseIf lmem = "NUIF2" Then
            Select Case sub2
                Case 1 : DescriptionFromMemberSubs = "particulate nh4 on "
                Case 2 : DescriptionFromMemberSubs = "particulate po4 on "
            End Select
            Select Case sub1
                Case 1 : DescriptionFromMemberSubs = DescriptionFromMemberSubs & "sand"
                Case 2 : DescriptionFromMemberSubs = DescriptionFromMemberSubs & "silt"
                Case 3 : DescriptionFromMemberSubs = DescriptionFromMemberSubs & "clay"
            End Select
        ElseIf lmem = "PKIF" Then
            Select Case sub1
                Case 1 : DescriptionFromMemberSubs = "phyto"
                Case 2 : DescriptionFromMemberSubs = "zoo"
                Case 3 : DescriptionFromMemberSubs = "orn"
                Case 4 : DescriptionFromMemberSubs = "orp"
                Case 5 : DescriptionFromMemberSubs = "orc"
            End Select
        ElseIf lmem = "PHIF" Then
            Select Case sub1
                Case 1 : DescriptionFromMemberSubs = "tic"
                Case 2 : DescriptionFromMemberSubs = "co2"
            End Select
        End If
    End Function


    Private Sub SetGrid(ByVal g As atcGrid)
        With g.Source
            .Columns = 14
            .CellValue(0, 0) = "In Use"
            .CellValue(0, 1) = "HIDE" 'scenario
            .CellValue(0, 2) = "Reach"
            .CellValue(0, 3) = "HIDE" 'facility
            .CellValue(0, 4) = "Pollutant"
            .CellValue(0, 5) = "HIDE" 'src vol name
            .CellValue(0, 6) = "HIDE" 'src vol id
            .CellValue(0, 7) = "HIDE" 'tar vol name
            .CellValue(0, 8) = "HIDE" 'tar vol id
            .CellValue(0, 9) = "HIDE" 'tar group
            .CellValue(0, 10) = "Target Member"
            .CellValue(0, 11) = "HIDE" 'lts index
            '.CellValue(0, 12) = "MemSub1"
            '.CellValue(0, 13) = "MemSub2"
            .CellValue(0, 12) = "HIDE"
            .CellValue(0, 13) = "HIDE"
            .CellValue(0, 14) = "HIDE" 'row number
        End With
    End Sub


    Private Sub ExpandedView(ByVal aExpand As Boolean)
        If aExpand Then
            Me.Size = New Size(800, 475)
            cmdDetailsHide.Visible = True
            cmdDetailsShow.Visible = False
            grpDetails.Visible = True
        Else
            Me.Size = New Size(280, 475)
            cmdDetailsHide.Visible = False
            cmdDetailsShow.Visible = True
            grpDetails.Visible = False
        End If

    End Sub

    Private Sub chkAllSources_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim lRow As Integer
        RemoveHandler lstPoints.ItemCheck, AddressOf lstSources_IndividualCheckChanged

        For lRow = 0 To lstPoints.Items.Count - 1
            lstPoints.SetItemChecked(lRow, chkAllSources.Checked)
        Next

        AddHandler lstPoints.ItemCheck, AddressOf lstSources_IndividualCheckChanged

    End Sub

    Private Sub lstSources_IndividualCheckChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckEventArgs)

        RemoveHandler chkAllSources.CheckStateChanged, AddressOf chkAllSources_CheckedChanged

        chkAllSources.Checked = False

        AddHandler chkAllSources.CheckStateChanged, AddressOf chkAllSources_CheckedChanged

        RemoveHandler lstPoints.ItemCheck, AddressOf lstSources_IndividualCheckChanged
        lstPoints.SetItemChecked(e.Index, e.NewValue)
        AddHandler lstPoints.ItemCheck, AddressOf lstSources_IndividualCheckChanged

    End Sub

    Private Sub lstSources_SelectionChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstPoints.SelectedIndexChanged
        grpDetails.Text = "Details of " & lstPoints.SelectedItem
    End Sub
    Private Sub cmdShowDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDetailsShow.Click
        ExpandedView(True)
    End Sub

    Private Sub cmdDetailsHide_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDetailsHide.Click
        ExpandedView(False)
    End Sub

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Me.Dispose()
    End Sub

    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
        Me.Dispose()
    End Sub

    Private Sub cmdSimpleCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSimpleCreate.Click

        If IsNothing(pfrmAddPoint) Then
            pfrmAddPoint = New frmAddPoint
            pfrmAddPoint.Show()
        Else
            If pfrmAddPoint.IsDisposed Then
                pfrmAddPoint = New frmAddPoint
                pfrmAddPoint.Show()
            Else
                pfrmAddPoint.WindowState = FormWindowState.Normal
                pfrmAddPoint.BringToFront()
            End If
        End If

    End Sub

    Private Sub cmdImportMustin_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdImportMustin.Click

        If IsNothing(pfrmImportPoint) Then
            pfrmImportPoint = New frmImportPoint
            pfrmImportPoint.Show()
        Else
            If pfrmImportPoint.IsDisposed Then
                pfrmImportPoint = New frmImportPoint
                pfrmImportPoint.Show()
            Else
                pfrmImportPoint.WindowState = FormWindowState.Normal
                pfrmImportPoint.BringToFront()
            End If
        End If

    End Sub

    Private Sub cmdConvertMustin_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdConvertMustin.Click

    End Sub

    Private Sub cmdAdvancedGen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdvancedGen.Click

        If IsNothing(pfrmTSnew) Then
            pfrmTSnew = New frmTSnew
            pfrmTSnew.Show()
        Else
            If pfrmTSnew.IsDisposed Then
                pfrmTSnew = New frmTSnew
                pfrmTSnew.Show()
            Else
                pfrmTSnew.WindowState = FormWindowState.Normal
                pfrmTSnew.BringToFront()
            End If
        End If

    End Sub

    Private Sub CreateScenarioToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdScenario.Click

        If IsNothing(pfrmPointScenario) Then
            pfrmPointScenario = New frmPointScenario
            pfrmPointScenario.Show()
        Else
            If pfrmPointScenario.IsDisposed Then
                pfrmPointScenario = New frmPointScenario
                pfrmPointScenario.Show()
            Else
                pfrmPointScenario.WindowState = FormWindowState.Normal
                pfrmPointScenario.BringToFront()
            End If
        End If

    End Sub
End Class