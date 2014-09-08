Imports atcControls
Imports atcData
Imports atcUCI
Imports atcUtility
Imports MapWinUtility

Public Class frmPoint

    Dim pTimeSeries As New Collection

    '.net conversion issue: tsl was formelry ATCoTSlist
    Dim WithEvents pTsl As atcTimeseries

    'The array InUseFacs(,) is a 2-D array that holds the (dimension 0) facility name, (dimension 1) scenario string
    'it should have the same elements as the sources list that are checked.
    Dim pInUseFacs(1)() As String

    'The array AvailFacs(,) is a 2-D array that holds the (dimension 0) facility name, (dimension 1) scenario string
    'it should have the same elements as the sources list that are NOT checked.
    Dim pAvailFacs(1)() As String

    Dim pCountInUseFacs As Integer
    Dim pCountAvailFacs As Integer
    Dim pConsLinks() As String
    Dim pMemberLinks() As String
    Dim pLinkCount As Integer
    Dim pMSub1Links() As Long
    Dim pMSub2Links() As Long

    'Each integer in pAgdPointRowReference corresponds (in index order) to a row in the current agdPoint grid. 
    'Item(i) is the row number in agdMasterPoint that corresponds to row (i) in agdPoint.
    Dim pAgdPointRowReference As New Collection

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        Me.Icon = pIcon
        lstPoints.SelectionMode = SelectionMode.One
        ExpandedView(False)

        With agdMasterPoint
            .Source = New atcControls.atcGridSource
            .Clear()
            .AllowHorizontalScrolling = False
            .AllowNewValidValues = True
            .Visible = False
        End With

        With agdPoint
            .Source = New atcControls.atcGridSource
            .Clear()
            .AllowHorizontalScrolling = False
            .AllowNewValidValues = True
            .Visible = True
        End With

        'always link flow to ivol
        pLinkCount = 1
        ReDim pConsLinks(pLinkCount)
        ReDim pMemberLinks(pLinkCount)
        ReDim pMSub1Links(pLinkCount)
        ReDim pMSub2Links(pLinkCount)
        pConsLinks(0) = "FLOW"
        pMemberLinks(0) = "IVOL"
        pMSub1Links(0) = 0
        pMSub2Links(0) = 0

        agdPoint.Source = New atcControls.atcGridSource
        agdMasterPoint.Source = New atcControls.atcGridSource

        With agdMasterPoint.Source
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

        'agdPoint properties
        agdPoint.Source.Columns = 4
        agdPoint.Source.Rows = 1
        agdPoint.Source.FixedRows = 1
        agdPoint.AllowNewValidValues = True
        agdPoint.Source.CellValue(0, 0) = "In Use"
        agdPoint.Source.CellValue(0, 1) = "Reach"
        agdPoint.Source.CellValue(0, 2) = "Pollutant"
        agdPoint.Source.CellValue(0, 3) = "Target Member"

        FillMasterGrid()
        '.net conversion issue: tsl formerly atcTSList

        If lstPoints.Items.Count > 0 AndAlso lstPoints.SelectedIndex = -1 Then lstPoints.SelectedIndex = 0
        ExpandedView(True)

        AddHandler chkAllSources.CheckStateChanged, AddressOf chkAllSources_CheckedChanged
        AddHandler lstPoints.ItemCheck, AddressOf lstSources_IndividualCheckChanged

    End Sub

    Private Sub agdMasterPoint2agdPoint()
        Dim lOper As Integer
        Dim lCheckedItemSplit() As String
        Dim lagdPointRow As Integer

        agdPoint.Source.Rows = 1

        If lstPoints.SelectedIndex <> -1 Then
            lCheckedItemSplit = lstPoints.Items.Item(lstPoints.SelectedIndex).ToString.Split(New [Char]() {"("c, ")"c})

            'remove space before "(" in string
            lCheckedItemSplit(0) = RTrim(lCheckedItemSplit(0))
            ReDim Preserve lCheckedItemSplit(1)

            lagdPointRow = 0
            pAgdPointRowReference.Clear()

            For lOper = 1 To agdMasterPoint.Source.Rows - 1
                If Mid(agdMasterPoint.Source.CellValue(lOper, 1), 4) = lCheckedItemSplit(1) AndAlso agdMasterPoint.Source.CellValue(lOper, 3) = lCheckedItemSplit(0) Then
                    lagdPointRow += 1
                    agdPoint.Source.CellValue(lagdPointRow, 0) = agdMasterPoint.Source.CellValue(lOper, 0)
                    agdPoint.Source.CellValue(lagdPointRow, 1) = agdMasterPoint.Source.CellValue(lOper, 2)
                    agdPoint.Source.CellValue(lagdPointRow, 2) = agdMasterPoint.Source.CellValue(lOper, 4)
                    agdPoint.Source.CellValue(lagdPointRow, 3) = agdMasterPoint.Source.CellValue(lOper, 10)

                    pAgdPointRowReference.Add(lOper)

                    agdPoint.Source.CellEditable(lagdPointRow, 0) = True
                    agdPoint.Source.CellEditable(lagdPointRow, 3) = True
                End If
            Next lOper
        End If

        agdPoint.SizeAllColumnsToContents()
        agdPoint.Refresh()

    End Sub

    Private Sub agdPoint2agdMasterPoint()
        Dim lOper As Integer
        Dim lagdMasterPointRow As Integer

        For lOper = 1 To agdPoint.Source.Rows - 1
            lagdMasterPointRow = pAgdPointRowReference(lOper)
            agdMasterPoint.Source.CellValue(lagdMasterPointRow, 0) = agdPoint.Source.CellValue(lOper, 0)
            agdMasterPoint.Source.CellValue(lagdMasterPointRow, 2) = agdPoint.Source.CellValue(lOper, 1)
            agdMasterPoint.Source.CellValue(lagdMasterPointRow, 4) = agdPoint.Source.CellValue(lOper, 2)
            agdMasterPoint.Source.CellValue(lagdMasterPointRow, 10) = agdPoint.Source.CellValue(lOper, 3)
        Next lOper

        agdMasterPoint.Refresh()

    End Sub

    Private Sub agdPoint_MouseDownCell(ByVal aGrid As atcGrid, ByVal aRow As Integer, ByVal aColumn As Integer) Handles agdPoint.MouseDownCell
        Dim lValidValuesCollection As Collection = New Collection

        Select Case aColumn
            Case 0
                'Set valid values for all but the header cell (row=0)
                If aRow <> 0 Then
                    lValidValuesCollection.Add("Yes")
                    lValidValuesCollection.Add("No")
                    agdPoint.ValidValues = lValidValuesCollection
                    agdPoint.AllowNewValidValues = False
                End If
            Case 3
                'Set valid values for all but the header cell (row=0)
                If aRow <> 0 Then
                    'Get the target member names and set them as lValidValuesCollection. Collection is passed as argument via ByRef.
                    TargetMemberNames2Collection(lValidValuesCollection, pAgdPointRowReference(aRow))
                    agdPoint.ValidValues = lValidValuesCollection
                    agdPoint.AllowNewValidValues = False
                End If
        End Select

    End Sub

    Private Sub agdPoint_ValueChanged(ByVal aGrid As atcGrid, ByVal aRow As Integer, ByVal aColumn As Integer) Handles agdPoint.CellEdited
        'check that a target member has been set if In-Use is set to yes.
        If agdPoint.Source.CellValue(aRow, aColumn) = "Yes" AndAlso Len(agdPoint.Source.CellValue(aRow, 3)) = 0 Then
            Logger.Message("No target member has been set for the point source: " & vbCrLf & vbCrLf & agdPoint.Source.CellValue(aRow, 1) & "/" & agdPoint.Source.CellValue(aRow, 2) & vbCrLf & vbCrLf & "Assign a valid target source before setting the pollutant to In-Use. ", "PointSources - Need Target Member", MessageBoxButtons.OK, MessageBoxIcon.Error, Windows.Forms.DialogResult.OK)
            agdPoint.Source.CellValue(aRow, aColumn) = "No"
        End If
    End Sub

    Private Sub UpdateListArrays()
        Dim lOper As Integer
        Dim lCheckedItemSplit() As String

        pInUseFacs(0) = New String() {}
        pInUseFacs(1) = New String() {}
        pAvailFacs(0) = New String() {}
        pAvailFacs(1) = New String() {}

        For lOper = 0 To lstPoints.Items.Count - 1
            'split the string of the newly checked item into (Index 0): description and (Index 1): scenario
            lCheckedItemSplit = lstPoints.Items.Item(lOper).ToString.Split(New [Char]() {"("c, ")"c})

            'remove space before "(" in string
            lCheckedItemSplit(0) = RTrim(lCheckedItemSplit(0))
            ReDim Preserve lCheckedItemSplit(1)

            If lstPoints.GetItemChecked(lOper) Then
                ReDim Preserve pInUseFacs(0)(pInUseFacs(0).GetUpperBound(0) + 1)
                ReDim Preserve pInUseFacs(1)(pInUseFacs(1).GetUpperBound(0) + 1)
                pInUseFacs(0)(pInUseFacs(0).GetUpperBound(0)) = lCheckedItemSplit(0)
                pInUseFacs(1)(pInUseFacs(1).GetUpperBound(0)) = lCheckedItemSplit(1)
            Else
                ReDim Preserve pAvailFacs(0)(pAvailFacs(0).GetUpperBound(0) + 1)
                ReDim Preserve pAvailFacs(1)(pAvailFacs(1).GetUpperBound(0) + 1)
                pAvailFacs(0)(pAvailFacs(0).GetUpperBound(0)) = lCheckedItemSplit(0)
                pAvailFacs(1)(pAvailFacs(1).GetUpperBound(0)) = lCheckedItemSplit(1)
            End If
        Next

        pCountInUseFacs = lstPoints.CheckedItems.Count

    End Sub

    Private Sub FillMasterGrid()

        Dim lDsnPtr() As Integer = Nothing
        lstPoints.Items.Clear()

        pTimeSeries = pUCI.FindTimser("", "", "")

        With agdMasterPoint.Source
            .Rows = 1

            Dim lOpnBlk As HspfOpnBlk = pUCI.OpnBlks("RCHRES")
            Dim lRowIndex As Integer = 1

            For lTSIndex As Integer = 1 To pTimeSeries.Count
                Dim lTS As atcTimeseries = pTimeSeries(lTSIndex)
                Dim lScenario As String = lTS.Attributes.GetValue("Scenario")
                If Mid(lScenario, 1, 3) = "PT-" Then 'this is a pt src
                    Dim lLocation As String = lTS.Attributes.GetValue("Location")
                    If IsNumeric(Mid(lLocation, 4)) Then
                        'get full reach name
                        Dim lOper As HspfOperation = lOpnBlk.OperFromID(CInt(Mid(lLocation, 4)))
                        If Not lOper Is Nothing Then
                            'found a reach with this id
                            lLocation = "RCHRES " & lOper.Id & " - " & lOper.Description
                            Dim lFacility As String = UCase(lTS.Attributes.GetValue("STANAM"))
                            Dim lConstituent As String = lTS.Attributes.GetValue("Constituent")

                            Dim lSelectedItemString As String = lFacility & " (" & Mid(lTS.Attributes.GetValue("Scenario"), 4) & ")"
                            If Not lstPoints.Items.Contains(lSelectedItemString) Then
                                lstPoints.Items.Add(lSelectedItemString, False)
                            End If

                            'see how many times this dsn shows up in pt srcs
                            Dim lDsnCount As Integer = 0
                            Dim lActiveFlag As Boolean = False
                            If Not pUCI.PointSources Is Nothing Then
                                For lPointSourceIndex As Integer = 0 To pUCI.PointSources.Count - 1
                                    With pUCI.PointSources(lPointSourceIndex)
                                        If .Target.VolName = lOper.Name AndAlso _
                                           .Target.VolId = lOper.Id AndAlso _
                                           lTS.Attributes.GetValue("Data Source").ToString.ToUpper.EndsWith(.Source.VolName.ToUpper.Substring(0, 3)) AndAlso _
                                           .Source.VolId = lTS.Attributes.GetValue("Id") Then
                                            'found this dsn in active point sources
                                            lDsnCount += 1
                                            lActiveFlag = True
                                            ReDim Preserve lDsnPtr(lDsnCount)
                                            lDsnPtr(lDsnCount) = lPointSourceIndex
                                            For lPointItemIndex As Integer = 0 To lstPoints.Items.Count - 1
                                                If lstPoints.Items.Item(lPointItemIndex) = lSelectedItemString Then
                                                    lstPoints.SetItemChecked(lPointItemIndex, True)
                                                End If
                                            Next
                                        End If
                                    End With
                                Next
                                If lActiveFlag = False Then
                                    'still add a line for this dsn
                                    lDsnCount = 1
                                End If

                                For lDsnIndex As Integer = 1 To lDsnCount
                                    If lRowIndex > 1 Then .Rows = .Rows + 1
                                    If lActiveFlag = False Then
                                        'not an active point source
                                        .CellValue(lRowIndex, 0) = "No"
                                        .CellValue(lRowIndex, 9) = "INFLOW"
                                    Else
                                        'this is an active point source
                                        .CellValue(lRowIndex, 0) = "Yes"
                                        .CellValue(lRowIndex, 9) = pUCI.PointSources(lDsnPtr(lDsnIndex)).Target.Group
                                        Dim lPS As HspfPointSource = pUCI.PointSources(lDsnPtr(lDsnIndex))
                                        .CellValue(lRowIndex, 10) = MemberLongVersion(lPS.Target.Member, lPS.Target.MemSub1, lPS.Target.MemSub2)
                                    End If

                                    .CellValue(lRowIndex, 1) = lScenario
                                    .CellValue(lRowIndex, 2) = lLocation
                                    .CellValue(lRowIndex, 3) = lTS.Attributes.GetValue("STANAM").ToString.ToUpper
                                    If lTS.Attributes.GetValue("Data Source") Is Nothing Then
                                        .CellValue(lRowIndex, 5) = "WDM"
                                    Else
                                        .CellValue(lRowIndex, 5) = pUCI.GetWDMIdFromName(lTS.Attributes.GetValue("Data Source"))  'save assoc src vol name
                                    End If
                                    .CellValue(lRowIndex, 6) = lTS.Attributes.GetValue("Id")     'save assoc src vol id
                                    .CellValue(lRowIndex, 7) = lOper.Name     'save assoc tar vol name
                                    .CellValue(lRowIndex, 8) = lOper.Id         'save assoc tar vol id
                                    .CellValue(lRowIndex, 11) = lTSIndex 'save index to lts

                                    'look for this con in pollutant list
                                    For Each lPollutantName As String In pPollutantList
                                        If Mid(lConstituent, 1, 5) = Mid(lPollutantName, 1, 5) Then
                                            lConstituent = lPollutantName
                                            Exit For
                                        End If
                                    Next
                                    .CellValue(lRowIndex, 4) = lConstituent

                                    .CellValue(lRowIndex, 14) = lRowIndex 'save row number

                                    'default member based on constituent name if poss
                                    If lActiveFlag Then
                                        'is active, see if we want to remember link
                                        Dim lFoundFlag As Boolean = False
                                        For lLinkIndex As Integer = 1 To pLinkCount
                                            If pConsLinks(lLinkIndex - 1) = UCase(Trim(lConstituent)) Then
                                                lFoundFlag = True
                                                Exit For
                                            End If
                                        Next
                                        If Not lFoundFlag Then
                                            'add this to list
                                            pLinkCount = pLinkCount + 1
                                            ReDim Preserve pConsLinks(pLinkCount)
                                            ReDim Preserve pMemberLinks(pLinkCount)
                                            ReDim Preserve pMSub1Links(pLinkCount)
                                            ReDim Preserve pMSub2Links(pLinkCount)
                                            pConsLinks(pLinkCount - 1) = UCase(Trim(lConstituent))
                                            pMemberLinks(pLinkCount - 1) = MemberFromLongVersion(.CellValue(lRowIndex, 10))
                                            pMSub1Links(pLinkCount - 1) = MemSub1FromLongVersion(.CellValue(lRowIndex, 10))
                                            pMSub2Links(pLinkCount - 1) = MemSub2FromLongVersion(.CellValue(lRowIndex, 10))
                                        End If
                                    End If

                                    lRowIndex += 1
                                Next lDsnIndex
                            End If
                        End If
                    End If
                End If
            Next

            'set default members for all
            For lTempRowIndex As Integer = 1 To .Rows
                For lLinkIndex As Integer = 1 To pLinkCount
                    If pConsLinks(lLinkIndex - 1) = UCase(Trim(.CellValue(lTempRowIndex, 4))) Then
                        .CellValue(lTempRowIndex, 10) = MemberLongVersion(pMemberLinks(lLinkIndex - 1), pMSub1Links(lLinkIndex - 1), pMSub2Links(lLinkIndex - 1))
                        Exit For
                    End If
                Next
            Next
            If lRowIndex = 1 Then .Rows = 0

        End With

        UpdateListArrays()
        agdPoint.Refresh()
        agdPoint.SizeAllColumnsToContents()

    End Sub

    Private Function MemberFromLongVersion(ByVal aString As String) As String
        Dim lOper1 As Integer
        lOper1 = InStr(1, aString, "(")
        If lOper1 > 0 Then
            MemberFromLongVersion = Mid(aString, 1, lOper1 - 1)
        Else
            MemberFromLongVersion = aString
        End If
    End Function

    Private Function MemSub1FromLongVersion(ByVal aString As String) As Long
        Dim lOper1, lOper2 As Integer
        lOper1 = InStr(1, aString, "(")
        If lOper1 > 0 Then
            lOper2 = InStr(1, aString, ",")
            If lOper2 = 0 Then
                lOper2 = InStr(1, aString, ")")
            End If
            MemSub1FromLongVersion = CInt(Mid(aString, lOper1 + 1, lOper2 - lOper1 - 1))
        Else
            MemSub1FromLongVersion = 0
        End If
    End Function

    Private Function MemSub2FromLongVersion(ByVal aString As String) As Long
        Dim lOper1, lOper2 As Integer
        lOper1 = InStr(1, aString, ",")
        If lOper1 > 0 Then
            lOper2 = InStr(1, aString, ")")
            MemSub2FromLongVersion = CInt(Mid(aString, lOper1 + 1, lOper2 - lOper1 - 1))
        Else
            MemSub2FromLongVersion = 0
        End If
    End Function

    Private Function MemberLongVersion(ByVal aMemberName As String, ByVal aSub1 As Integer, ByVal aSub2 As Integer) As String
        Dim lString As String
        lString = aMemberName
        If aSub1 > 0 Then
            lString = lString & "(" & aSub1
            If aSub2 > 0 Then
                lString = lString & "," & aSub2 & ")"
            Else
                lString = lString & ")"
            End If
        End If
        If InStr(1, lString, "|") = 0 Then
            MemberLongVersion = lString & " | " & DescriptionFromMemberSubs(aMemberName, aSub1, aSub2)
        Else
            MemberLongVersion = lString
        End If
    End Function

    Private Function DescriptionFromMemberSubs(ByVal aMemberName As String, ByVal aSub1 As Integer, ByVal aSub2 As Integer)
        Dim lOpnBlk As HspfOpnBlk
        Dim lOpn As HspfOperation
        Dim lTable As HspfTable

        DescriptionFromMemberSubs = Nothing

        If aMemberName = "IVOL" Then
            DescriptionFromMemberSubs = "water"
        ElseIf aMemberName = "CIVOL" Then
            DescriptionFromMemberSubs = "water for category " & CStr(aSub1)
        ElseIf aMemberName = "ICON" Then
            DescriptionFromMemberSubs = "conservative"
        ElseIf aMemberName = "IHEAT" Then
            DescriptionFromMemberSubs = "heat"
        ElseIf aMemberName = "ISED" Then
            Select Case aSub1
                Case 1 : DescriptionFromMemberSubs = "sand"
                Case 2 : DescriptionFromMemberSubs = "silt"
                Case 3 : DescriptionFromMemberSubs = "clay"
            End Select
        ElseIf aMemberName = "IDQAL" Then
            lOpnBlk = pUCI.OpnBlks("RCHRES")
            DescriptionFromMemberSubs = "dissolved gqual"
            If Not lOpnBlk Is Nothing Then
                lOpn = lOpnBlk.Ids(1)
                If Not lOpn Is Nothing Then
                    If aSub1 = 1 Or aSub1 = 0 Then
                        lTable = lOpn.Tables("GQ-QALDATA")
                    Else
                        lTable = lOpn.Tables("GQ-QALDATA:" & CStr(aSub1))
                    End If
                    If Not lTable Is Nothing Then
                        '.net conversion issue: Converted lTable.Parms("GQID") to string with .ToString
                        DescriptionFromMemberSubs = "dissolved " & Trim(lTable.Parms("GQID").Value)
                    End If
                End If
            End If
        ElseIf aMemberName = "ISQAL" Then
            Select Case aSub1
                Case 1 : DescriptionFromMemberSubs = "sand associated "
                Case 2 : DescriptionFromMemberSubs = "silt associated "
                Case 3 : DescriptionFromMemberSubs = "clay associated "
            End Select
            lOpnBlk = pUCI.OpnBlks("RCHRES")
            If Not lOpnBlk Is Nothing Then
                lOpn = lOpnBlk.Ids(1)
                If Not lOpn Is Nothing Then
                    If aSub2 = 1 Then
                        lTable = lOpn.Tables("GQ-QALDATA")
                    Else
                        lTable = lOpn.Tables("GQ-QALDATA:" & CStr(aSub2))
                    End If
                    If Not lTable Is Nothing Then
                        '.net conversion issue: Converted lTable.Parms("GQID") to string with .ToString
                        DescriptionFromMemberSubs = DescriptionFromMemberSubs & Trim(lTable.Parms("GQID").Value)
                    End If
                End If
            End If
        ElseIf aMemberName = "OXIF" Then
            Select Case aSub1
                Case 1 : DescriptionFromMemberSubs = "do"
                Case 2 : DescriptionFromMemberSubs = "bod"
            End Select
        ElseIf aMemberName = "NUIF1" Then
            Select Case aSub1
                Case 1 : DescriptionFromMemberSubs = "no3"
                Case 2 : DescriptionFromMemberSubs = "tam"
                Case 3 : DescriptionFromMemberSubs = "no2"
                Case 4 : DescriptionFromMemberSubs = "po4"
            End Select
        ElseIf aMemberName = "NUIF2" Then
            Select Case aSub2
                Case 1 : DescriptionFromMemberSubs = "particulate nh4 on "
                Case 2 : DescriptionFromMemberSubs = "particulate po4 on "
            End Select
            Select Case aSub1
                Case 1 : DescriptionFromMemberSubs = DescriptionFromMemberSubs & "sand"
                Case 2 : DescriptionFromMemberSubs = DescriptionFromMemberSubs & "silt"
                Case 3 : DescriptionFromMemberSubs = DescriptionFromMemberSubs & "clay"
            End Select
        ElseIf aMemberName = "PKIF" Then
            Select Case aSub1
                Case 1 : DescriptionFromMemberSubs = "phyto"
                Case 2 : DescriptionFromMemberSubs = "zoo"
                Case 3 : DescriptionFromMemberSubs = "orn"
                Case 4 : DescriptionFromMemberSubs = "orp"
                Case 5 : DescriptionFromMemberSubs = "orc"
            End Select
        ElseIf aMemberName = "PHIF" Then
            Select Case aSub1
                Case 1 : DescriptionFromMemberSubs = "tic"
                Case 2 : DescriptionFromMemberSubs = "co2"
            End Select
        End If
    End Function

    Private Sub ExpandedView(ByVal aExpand As Boolean)
        If aExpand Then
            Me.Size = New Size(grpSources.Width + 900, grpSources.Height + 177)
            Me.MinimumSize = New Size(grpSources.Width + 30, grpSources.Height + 177)
            cmdDetailsHide.Visible = True
            cmdDetailsShow.Visible = False
        Else
            Me.Size = New Size(grpSources.Width + 30, grpSources.Height + 177)
            cmdDetailsHide.Visible = False
            cmdDetailsShow.Visible = True
        End If

    End Sub

    Private Sub chkAllSources_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim lRow As Integer
        RemoveHandler lstPoints.ItemCheck, AddressOf lstSources_IndividualCheckChanged

        For lRow = 0 To lstPoints.Items.Count - 1
            lstPoints.SetItemChecked(lRow, chkAllSources.Checked)
        Next

        CheckedSourcesUpdate()

        AddHandler lstPoints.ItemCheck, AddressOf lstSources_IndividualCheckChanged

    End Sub

    Private Sub TargetMemberNames2Collection(ByRef aCollection As Collection, ByVal aRow As Integer)
        Dim lOper1, lOper2, lOper3, lId, lSub1, lSub2, lNcons, lNgqual As Integer
        Dim lOper As HspfOperation
        Dim lTable As HspfTable
        Dim lMemberNames As New Collection

        lId = agdMasterPoint.Source.CellValue(aRow, 8)
        lOper = pUCI.OpnBlks("RCHRES").OperFromID(lId)

        '.net conversion note: The following  was formerly GetMemberNames.

        lTable = lOper.Tables("ACTIVITY")

        If lTable.Parms(0).Value = 1 Then
            'hydr on
            lMemberNames.Add("IVOL")
            If Not lOper.Uci.CategoryBlock Is Nothing Then
                If lOper.Uci.CategoryBlock.Categories.Count > 0 Then
                    lMemberNames.Add("CIVOL")
                End If
            End If
        End If
        If lTable.Parms(2).Value = 1 Then
            'cons on
            lMemberNames.Add("ICON")
        End If
        If lTable.Parms(3).Value = 1 Then
            'ht on
            lMemberNames.Add("IHEAT")
        End If
        If lTable.Parms(4).Value = 1 Then
            'sed on
            lMemberNames.Add("ISED")
        End If
        If lTable.Parms(5).Value = 1 Then
            'gqual on
            lMemberNames.Add("IDQAL")
            lMemberNames.Add("ISQAL")
        End If
        If lTable.Parms(6).Value = 1 Then
            'ox on
            lMemberNames.Add("OXIF")
        End If
        If lTable.Parms(7).Value = 1 Then
            'nut on
            lMemberNames.Add("NUIF1")
            lMemberNames.Add("NUIF2")
        End If
        If lTable.Parms(8).Value = 1 Then
            'plank on
            lMemberNames.Add("PKIF")
        End If
        If lTable.Parms(9).Value = 1 Then
            'ph on
            lMemberNames.Add("PHIF")
        End If

        lTable = lOper.Tables("ACTIVITY")

        For lOper1 = 1 To lMemberNames.Count

            Select Case lMemberNames.Item(lOper1)
                Case "IVOL"
                    lSub1 = 0
                    lSub2 = 0
                Case "CIVOL"
                    lSub1 = lOper.Uci.CategoryBlock.Categories.Count
                    lSub2 = 0
                Case "ICON"
                    If lOper.TableExists("NCONS") Then
                        lNcons = lOper.Tables("NCONS").Parms("NCONS").Value
                    Else
                        lNcons = 1
                    End If
                    lSub1 = lNcons
                    lSub2 = 0
                Case "IHEAT"
                    lSub1 = 0
                    lSub2 = 0
                Case "ISED"
                    lSub1 = 3
                    lSub2 = 0
                Case "IDQAL"
                    If lOper.TableExists("GQ-GENDATA") Then
                        lNgqual = lOper.Tables("GQ-GENDATA").Parms("NGQUAL").Value
                    Else
                        lNgqual = 1
                    End If
                    lSub1 = lNgqual
                    lSub2 = 0
                Case "ISQAL"
                    If lOper.TableExists("GQ-GENDATA") Then
                        lNgqual = lOper.Tables("GQ-GENDATA").Parms("NGQUAL").Value
                    Else
                        lNgqual = 1
                    End If
                    lSub1 = 3
                    lSub2 = lNgqual
                Case "OXIF"
                    lSub1 = 2
                    lSub2 = 0
                Case "NUIF1"
                    lSub1 = 4
                    lSub2 = 0
                Case "NUIF2"
                    lSub1 = 3
                    lSub2 = 2
                Case "PKIF"
                    lSub1 = 5
                    lSub2 = 0
                Case "PHIF"
                    lSub1 = 2
                    lSub2 = 0
            End Select

            If lSub1 > 0 Then
                For lOper2 = 1 To lSub1
                    If lSub2 > 0 Then
                        For lOper3 = 1 To lSub2
                            aCollection.Add(MemberLongVersion(lMemberNames.Item(lOper1), lOper2, lOper3))
                        Next lOper3
                    Else
                        aCollection.Add(MemberLongVersion(lMemberNames.Item(lOper1), lOper2, lSub2))
                    End If
                Next lOper2
            Else
                aCollection.Add(MemberLongVersion(lMemberNames.Item(lOper1), lSub1, lSub2))
            End If
        Next lOper1

    End Sub

    Private Sub lstSources_IndividualCheckChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckEventArgs)

        RemoveHandler chkAllSources.CheckStateChanged, AddressOf chkAllSources_CheckedChanged

        chkAllSources.Checked = False

        AddHandler chkAllSources.CheckStateChanged, AddressOf chkAllSources_CheckedChanged

        'Temporarily removes the event item to update the status of the checked item (A shortcoming of the checkedlistbox)
        RemoveHandler lstPoints.ItemCheck, AddressOf lstSources_IndividualCheckChanged
        lstPoints.SetItemChecked(e.Index, e.NewValue)
        AddHandler lstPoints.ItemCheck, AddressOf lstSources_IndividualCheckChanged

        CheckedSourcesUpdate()

    End Sub

    Private Sub CheckedSourcesUpdate()
        Dim lOper As Integer

        'Update lists to reflect state of the checked item (which caused this event)
        UpdateListArrays()

        For lOper = 1 To agdMasterPoint.Source.Rows - 1
            'Check if facility and scenario are in use.
            'MsgBox(Array.IndexOf(ConsLinks, UCase(agdMasterPoint.Source.CellValue(i, 4))) & Array.IndexOf(pInUseFacs(0), agdMasterPoint.Source.CellValue(i, 3)) & Array.IndexOf(pInUseFacs(1), Mid(agdMasterPoint.Source.CellValue(i, 1), 4)) & ":::" & Mid(agdMasterPoint.Source.CellValue(i, 1), 4))
            If Array.IndexOf(pConsLinks, UCase(agdMasterPoint.Source.CellValue(lOper, 4))) <> -1 AndAlso Array.IndexOf(pInUseFacs(0), agdMasterPoint.Source.CellValue(lOper, 3)) <> -1 AndAlso Array.IndexOf(pInUseFacs(1), Mid(agdMasterPoint.Source.CellValue(lOper, 1), 4)) <> -1 Then
                'set indiv timsers to in use in master grid
                agdMasterPoint.Source.CellValue(lOper, 0) = "Yes"
            Else
                'set indiv timsers to not in use in master grid
                agdMasterPoint.Source.CellValue(lOper, 0) = "No"
            End If
        Next

        agdMasterPoint2agdPoint()

        'rebuild lists
        pCountInUseFacs = lstPoints.Items.Count - lstPoints.CheckedItems.Count
    End Sub

    Private Sub lstSources_SelectionChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstPoints.SelectedIndexChanged
        agdPoint2agdMasterPoint()
        agdMasterPoint2agdPoint()
        grpDetails.Text = "Details of " & lstPoints.SelectedItem
    End Sub

    Private Sub cmdShowDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDetailsShow.Click
        'if there exists points and no point is selected, then choose the first entry
        If lstPoints.Items.Count > 0 AndAlso lstPoints.SelectedIndex = -1 Then lstPoints.SelectedIndex = 0
        ExpandedView(True)
    End Sub

    Private Sub frmPoint_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyValue = Windows.Forms.Keys.F1 Then
            ShowHelp(pWinHSPFManualName)
            ShowHelp("User's Guide\Detailed Functions\Point Sources.html")
        End If
    End Sub

    Private Sub frmPoint_Resized(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.ResizeEnd
        If Me.Size <> Me.MinimumSize Then
            cmdDetailsHide.Visible = True
            cmdDetailsShow.Visible = False
        End If
        
    End Sub
    Private Sub cmdDetailsHide_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDetailsHide.Click
        ExpandedView(False)
    End Sub

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Me.Dispose()
    End Sub

    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
        Dim lOper1, lOper2, lFoundIndex As Integer
        Dim lCurrentConnections, lCurrentMember As String
        Dim lCurrentSub1, lCurrentSub2, lCurrentGroup As String
        Dim lProblemFlag, lFoundFlag2 As Boolean

        agdPoint2agdMasterPoint()

        'go through master list, putting point sources back
        For lOper1 = 1 To agdMasterPoint.Source.Rows - 1
            If agdMasterPoint.Source.CellValue(lOper1, 0) = "Yes" Then
                'this pt src is active
                'check to see if member and subs are filled in
                If Len(agdMasterPoint.Source.CellValue(lOper1, 10)) = 0 Then
                    lProblemFlag = True
                End If

                If Not lProblemFlag Then
                    'is it already in pt src structure
                    lFoundIndex = -1
                    For lOper2 = 0 To pUCI.PointSources.Count - 1
                        If pUCI.PointSources(lOper2).Target.VolName = agdMasterPoint.Source.CellValue(lOper1, 7) AndAlso _
                           pUCI.PointSources(lOper2).Target.VolId = agdMasterPoint.Source.CellValue(lOper1, 8) AndAlso _
                           pUCI.PointSources(lOper2).Source.VolName = agdMasterPoint.Source.CellValue(lOper1, 5) AndAlso _
                           pUCI.PointSources(lOper2).Source.VolId = agdMasterPoint.Source.CellValue(lOper1, 6) Then
                            lFoundIndex = lOper2
                        End If
                    Next lOper2
                    If lFoundIndex > -1 Then
                        pUCI.PointSources(lFoundIndex).Target.Group = agdMasterPoint.Source.CellValue(lOper1, 9)
                        lCurrentMember = agdMasterPoint.Source.CellValue(lOper1, 10)
                        lOper2 = InStr(1, lCurrentMember, "|")
                        If lOper2 > 0 Then
                            lCurrentMember = Mid(lCurrentMember, 1, lOper2 - 2)
                        End If
                        pUCI.PointSources(lFoundIndex).Target.Member = MemberFromLongVersion(lCurrentMember)
                        pUCI.PointSources(lFoundIndex).Target.MemSub1 = MemSub1FromLongVersion(agdMasterPoint.Source.CellValue(lOper1, 10))
                        pUCI.PointSources(lFoundIndex).Target.MemSub2 = MemSub2FromLongVersion(agdMasterPoint.Source.CellValue(lOper1, 10))
                    End If

                    If lFoundIndex = -1 Then
                        'add to point source structure
                        lCurrentConnections = agdMasterPoint.Source.CellValue(lOper1, 4)
                        lCurrentGroup = agdMasterPoint.Source.CellValue(lOper1, 9)
                        lCurrentMember = agdMasterPoint.Source.CellValue(lOper1, 10)
                        lOper2 = InStr(1, lCurrentMember, "|")
                        If lOper2 > 0 Then
                            lCurrentMember = Mid(lCurrentMember, 1, lOper2 - 2)
                        End If
                        lCurrentMember = MemberFromLongVersion(lCurrentMember)
                        lCurrentSub1 = MemSub1FromLongVersion(agdMasterPoint.Source.CellValue(lOper1, 10))
                        lCurrentSub2 = MemSub2FromLongVersion(agdMasterPoint.Source.CellValue(lOper1, 10))
                        If Len(lCurrentGroup) > 0 And Len(lCurrentMember) > 0 Then
                            pUCI.AddPoint(agdMasterPoint.Source.CellValue(lOper1, 5), agdMasterPoint.Source.CellValue(lOper1, 6), agdMasterPoint.Source.CellValue(lOper1, 8), agdMasterPoint.Source.CellValue(lOper1, 3), lCurrentGroup, lCurrentMember, lCurrentSub1, lCurrentSub2)
                        Else
                            agdMasterPoint.Source.CellValue(lOper1, 0) = "No"
                        End If
                        pUCI.Edited = True
                    End If
                End If
            Else
                'this pt src is not active, but is it in pt src structure
                lFoundFlag2 = False
                For lOper2 = 0 To pUCI.PointSources.Count - 1
                    If pUCI.PointSources(lOper2).Target.VolName = agdMasterPoint.Source.CellValue(lOper1, 7) And _
                       pUCI.PointSources(lOper2).Target.VolId = agdMasterPoint.Source.CellValue(lOper1, 8) And _
                       pUCI.PointSources(lOper2).Source.VolName = agdMasterPoint.Source.CellValue(lOper1, 5) And _
                       pUCI.PointSources(lOper2).Source.VolId = agdMasterPoint.Source.CellValue(lOper1, 6) Then
                        lFoundFlag2 = True
                    End If
                Next lOper2
                If lFoundFlag2 Then
                    'remove from point source structure
                    pUCI.RemovePoint(agdMasterPoint.Source.CellValue(lOper1, 5), agdMasterPoint.Source.CellValue(lOper1, 6), agdMasterPoint.Source.CellValue(lOper1, 8))
                    pUCI.Edited = True
                End If
            End If
        Next lOper1
        'myUci.Source2MetSeg
        If lProblemFlag Then
            Logger.Message("At least one of the target members for " & vbCrLf & "a point source 'in use' has not been set." & vbCrLf & vbCrLf & "This must be set before continuing.", "Point Source Problem", MessageBoxButtons.OK, MessageBoxIcon.Error, Windows.Forms.DialogResult.OK)
        End If
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
        Logger.Msg("This option is not yet implemented.", "Point Source Editor")
        ''convert all mutsin
        'myFiles = myUci.filesblock
        'mutcnt = 0
        'k = 1
        'Do While k <= myFiles.Count
        '    S = myFiles.Value(k).Name
        '    fu = myFiles.Value(k).Unit
        '    If UCase(Right(S, 3)) = "MUT" Then  'this is mutsin file
        '        mutcnt = mutcnt + 1
        '        Call ConvertMutsin(S, fu, 1, retc)
        '        If retc = 0 Then
        '            'now remove file
        '            myFiles.Remove(k)
        '        Else
        '            k = k + 1
        '        End If
        '    Else
        '        k = k + 1
        '    End If
        'Loop
        'If mutcnt = 0 Then 'no mutsin files found
        '    MsgBox("No Mutsin files found in this project.", vbOKOnly, "Convert Point Sources Problem")
        'End If
    End Sub

    Private Sub cmdAdvancedGen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdvancedGen.Click
        Logger.Msg("This option is not yet implemented.", "Point Source Editor")
        'If IsNothing(pfrmTSnew) Then
        '    pfrmTSnew = New frmTSnew
        '    pfrmTSnew.Show()
        'Else
        '    If pfrmTSnew.IsDisposed Then
        '        pfrmTSnew = New frmTSnew
        '        pfrmTSnew.Show()
        '    Else
        '        pfrmTSnew.WindowState = FormWindowState.Normal
        '        pfrmTSnew.BringToFront()
        '    End If
        'End If
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

    Public Sub UpdateListsForNewPointSource(ByVal aScenario$, ByVal aFacility$, ByVal aLocation$, ByVal lConstituent$, ByVal aWDMId$, ByVal aDsn&, ByVal aTargetName$, ByVal aTargetID&, ByVal aLongLocation$)
        Dim lFacScenString As String
        Dim lCount As Integer
        Dim lTempTimeSeries As Collection
        Dim lOper1, lOper2 As Integer
        Dim lFoundFlag As Boolean

        lFacScenString = aFacility & " (" & Mid(aScenario, 4) & ")"

        lFoundFlag = False
        For lOper1 = 0 To lstPoints.Items.Count - 1
            If lstPoints.Items.Item(lOper1) = lFacScenString Then
                lFoundFlag = True
                Exit For
            End If
        Next

        If lFoundFlag = False Then lstPoints.Items.Add(lFacScenString, False)

        With agdMasterPoint.Source
            lCount = .Rows
            .CellValue(lCount, 0) = "No"
            .CellValue(lCount, 1) = aScenario
            .CellValue(lCount, 2) = aLongLocation
            .CellValue(lCount, 3) = UCase(aFacility)
            .CellValue(lCount, 4) = lConstituent
            .CellValue(lCount, 5) = aWDMId
            .CellValue(lCount, 6) = aDsn
            .CellValue(lCount, 7) = aTargetName
            .CellValue(lCount, 8) = aTargetID
            .CellValue(lCount, 9) = "INFLOW"

            lTempTimeSeries = pUCI.FindTimser(aScenario, aLocation, lConstituent)

            If lTempTimeSeries.Count <> 0 Then
                pTimeSeries.Add(lTempTimeSeries(lTempTimeSeries.Count))
            End If

            .CellValue(lCount, 11) = pTimeSeries.Count 'save index to lts for list/graph
            .CellValue(lCount, 14) = lCount 'save row number
            '.TextMatrix(icnt, 10) = "IVOL"
            '.TextMatrix(icnt, 12) = 0
            '.TextMatrix(icnt, 13) = 0
            For lOper2 = 1 To pLinkCount
                If pConsLinks(lOper2 - 1) = UCase(Trim(lConstituent)) Then
                    .CellValue(lCount, 10) = MemberLongVersion(pMemberLinks(lOper2 - 1), pMSub1Links(lOper2 - 1), pMSub2Links(lOper2 - 1))
                    '.TextMatrix(icnt, 10) = MemberLinks(k - 1)
                    '.TextMatrix(icnt, 12) = MSub1Links(k - 1)
                    '.TextMatrix(icnt, 13) = MSub2Links(k - 1)

                    lFoundFlag = False
                    For lOper1 = 0 To lstPoints.Items.Count - 1
                        If lstPoints.Items.Item(lOper1) = lFacScenString Then
                            lFoundFlag = True
                            Exit For
                        End If
                    Next

                    If lFoundFlag = True Then .CellValue(lCount, 0) = "Yes"

                End If
            Next lOper2

        End With
    End Sub

    Private Sub cmdDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelete.Click
        '.net conversion note: Waiting for grid function before implementing this
        Logger.Msg("This option is not yet implemented.", "Point Source Editor")
        'Dim tempts As Collection
        'Dim alist As Collection
        'Dim i, lOper As Integer
        'Dim g As Object, vtP As Object
        'Dim tP As HspfPointSource
        'Dim masterrow As Integer
        'Dim istart&, iend&, j&, lsen$, lfac$, k&, S$, iresp&

        'Me.Cursor = Cursors.WaitCursor
        'tempts = New Collection
        'If agdPoint.SelStartRow > 0 Then
        '    For i = agdPoint.SelStartRow To agdPoint.SelEndRow
        '        tempts.Add(lts(CInt(agdPoint.TextMatrix(i, 11))))
        '    Next i
        'End If

        'If tempts.Count > 0 Then
        '    'delete the data sets
        '    j = 0
        '    istart = 1
        '    iend = agdPoint.Source.Rows - 1
        '    iresp = MsgBox("Are you sure you want to delete these timeseries?", vbOKCancel, "Point Sources Graphing Problem")
        '    If iresp = 1 Then
        '        For i = istart To iend Step -1
        '            pUCI.DeleteWDMDataSet(agdMasterPoint.Source.CellValue(i, 5), agdMasterPoint.Source.CellValue(i, 6))
        '            masterrow = agdPoint.Source.CellValue(i, 14)
        '            lsen = agdPoint.Source.CellValue(i, 1)
        '            lfac = agdPoint..Source.CellValue(i, 3)
        '            'remove from point source structure
        '            k = 1
        '            For Each vtP In pUCI.PointSources
        '                tP = vtP
        '                If tP.Target.VolName = agdPoint.Source.CellValue(i, 7) And _
        '                   tP.Target.VolId = agdPoint.Source.CellValue(i, 8) And _
        '                   tP.Source.VolName = agdPoint.Source.CellValue(i, 5) And _
        '                   tP.Source.VolId = agdPoint.Source.CellValue(i, 6) Then
        '                    pUCI.PointSources.Remove(k)
        '                Else
        '                    k = k + 1
        '                End If
        '            Next vtP
        '            agdPoint.DeleteRow(i)
        '            agdMasterPoint.DeleteRow(masterrow)
        '            j = j + 1
        '        Next i
        '        'keep grid in synch
        '        For i = istart To agdPoint.Source.Rows
        '            agdPoint.Source.CellValue(i, 14) = agdPoint.Source.CellValue(i, 14) - j
        '        Next i
        '        For i = 1 To agdMasterPoint.Source.Rows
        '            agdMasterPoint.Source.CellValue(i, 14) = i
        '        Next i
        '        If agdPoint.Source.Rows = 0 Then
        '            'none left, remove from list
        '            alist = New Collection
        '            S = lfac & " (" & Mid(lsen, 4) & ")"
        '            For lOper = 0 To lstPoints.Items.Count - 1
        '                If lstPoints.Items(lOper) = S AndAlso Not lstPoints.GetItemChecked(lOper) Then
        '                    For j = 0 To lstPoints.LeftCount - 1
        '                        If aslPoint.LeftItem(j) <> S Then
        '                            alist.Add(aslPoint.LeftItem(j))
        '                        End If
        '                    Next j
        '                    aslPoint.ClearLeft()
        '                    For j = 1 To alist.Count
        '                        aslPoint.LeftItemFastAdd(alist(j))
        '                    Next j
        '                ElseIf aslPoint.InRightList(S) Then
        '                    For j = 0 To aslPoint.RightCount - 1
        '                        If aslPoint.RightItem(j) <> S Then
        '                            alist.Add(aslPoint.RightItem(j))
        '                        End If
        '                    Next j
        '                    aslPoint.ClearRight()
        '                    For j = 1 To alist.Count
        '                        aslPoint.LeftItemFastAdd(alist(j))
        '                        aslPoint.MoveRight(aslPoint.LeftCount - 1)
        '                    Next j
        '                End If
        '            Next lOper
        '            If aslPoint.RightCount > 0 Then
        '                aslPoint_ItemSelected(aslPoint.RightItem(0))
        '            ElseIf aslPoint.LeftCount > 0 Then
        '                aslPoint_ItemSelected(aslPoint.LeftItem(0))
        '            End If
        '        End If
        '    End If
        'Else 'no rows selected
        '    MsgBox("No timeseries have been selected for deleting.", vbOKOnly, "Point Sources Delete Problem")
        'End If

        'Me.Cursor = Cursors.Default

    End Sub

    Private Sub cmdList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdList.Click
        Logger.Msg("This option is not yet implemented.", "Point Source Editor")
    End Sub

    Private Sub cmdGraph_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdGraph.Click
        Logger.Msg("This option is not yet implemented.", "Point Source Editor")
    End Sub
End Class