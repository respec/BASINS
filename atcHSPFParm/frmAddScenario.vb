Imports atcUtility
Imports MapWinUtility
Imports System.Data
Imports atcUCI

Public Class frmAddScenario
    Friend pGrid As atcControls.atcGrid
    Friend pUci As atcUCI.HspfUci
    Friend pWatershedName As String
    <CLSCompliant(False)> Friend Database As atcUtility.atcMDB

    Public Sub InitializeUI(ByVal aDatabase As atcUtility.atcMDB, ByVal aGrid As atcControls.atcGrid, ByVal aWatershedId As String, ByVal aWatershedName As String)
        Database = aDatabase
        pGrid = aGrid
        txtWatershedId.Text = aWatershedId
        pWatershedName = aWatershedName
    End Sub

    Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        Me.Close()
    End Sub

    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
        'get values from user interface
        Dim lUciName As String = txtUCIName.Text
        Dim lName As String = txtName.Text
        Dim lType As String = txtType.Text
        If lType.Length = 0 Then lType = " "
        Dim lWatershedId As String = txtWatershedId.Text
        Dim lStartDate As String = txtStartDate.Text
        If lStartDate.Length = 0 Then lStartDate = " "
        Dim lEndDate As String = txtEndDate.Text
        If lEndDate.Length = 0 Then lEndDate = " "
        Dim lUnits As String = txtUnits.Text
        If lUnits.Length = 0 Then lUnits = " "
        Dim lNumSegments As String = txtNumSegments.Text
        If lNumSegments.Length = 0 Then lNumSegments = " "
        Dim lNumReaches As String = txtNumReaches.Text
        If lNumReaches.Length = 0 Then lNumReaches = " "
        Dim lLandUseType As String = txtLandUseType.Text
        If lLandUseType.Length = 0 Then lLandUseType = " "
        Dim lChannels As String = txtChannels.Text
        If lChannels.Length = 0 Then lChannels = " "
        Dim lWQConstituents As String = txtWQConstituents.Text
        If lWQConstituents.Length = 0 Then lWQConstituents = " "
        Dim lSources As String = txtSources.Text
        If lSources.Length = 0 Then lSources = " "
        Dim lPurpose As String = txtPurpose.Text
        If lPurpose.Length = 0 Then lPurpose = " "
        Dim lVersion As String = txtVersion.Text
        If lVersion.Length = 0 Then lVersion = " "
        Dim lReference As String = txtReference.Text
        If lReference.Length = 0 Then lReference = " "
        Dim lContactName As String = txtContactName.Text
        If lContactName.Length = 0 Then lContactName = " "
        Dim lOrganization As String = txtOrganization.Text
        If lOrganization.Length = 0 Then lOrganization = " "
        Dim lPhone As String = txtPhone.Text
        If lPhone.Length = 0 Then lPhone = " "
        Dim lComments As String = txtComments.Text
        If lComments.Length = 0 Then lComments = " "

        'check values
        If lUciName.Trim.Length = 0 Or lName.Trim.Length = 0 Or lWatershedId.Trim.Length = 0 Then
            Logger.Msg("One or more required fields does not have a value.", MsgBoxStyle.OkOnly, "BASINS HSPFParm")
            Exit Sub
        End If

        Logger.Status("SHOW")
        Logger.Status("Adding Scenario to database...")
        'find next available id number 
        Dim lTable As DataTable = Database.GetTable("ScenarioData")
        Dim lMaxId As Integer = 0
        Dim lTmpId As Integer = 0
        For lRow As Integer = 0 To lTable.Rows.Count - 1
            lTmpId = lTable.Rows(lRow).Item(0).ToString
            If lTmpId > lMaxId Then
                lMaxId = lTmpId
            End If
        Next
        Dim lNewScenarioId As Integer = lMaxId + 1

        'save to database
        Dim lValues As New Collection
        lValues.Add(lNewScenarioId)
        lValues.Add("'" & lName & "'")
        lValues.Add("'" & lType & "'")
        lValues.Add("'" & lUciName & "'")
        lValues.Add(lWatershedId)
        lValues.Add("'" & lStartDate & "'")
        lValues.Add("'" & lEndDate & "'")
        lValues.Add(lUnits)
        lValues.Add(lNumSegments)
        lValues.Add(lNumReaches)
        lValues.Add("'" & lLandUseType & "'")
        lValues.Add("'" & lChannels & "'")
        lValues.Add("'" & lWQConstituents & "'")
        lValues.Add("'" & lSources & "'")
        lValues.Add("'" & lPurpose & "'")
        lValues.Add("'" & lVersion & "'")
        lValues.Add("'" & lReference & "'")
        lValues.Add("'" & lContactName & "'")
        lValues.Add("'" & lOrganization & "'")
        lValues.Add("'" & lPhone & "'")
        lValues.Add("'" & lComments & "'")
        Database.InsertRowIntoTable("ScenarioData", lValues)

        Dim lRangeCheck As Boolean = cbxRange.Checked
        If Not pUci Is Nothing Then
            'look for perlnd, implnd, rchres operation types
            If pUci.OpnBlks("PERLND").Count > 0 Then
                'this operation type exists
                Logger.Status("Adding PERLND to database...")
                GetOperInfo("PERLND", lNewScenarioId, lRangeCheck)
            End If
            If pUci.OpnBlks("IMPLND").Count > 0 Then
                'this operation type exists
                Logger.Status("Adding IMPLND to database...")
                GetOperInfo("IMPLND", lNewScenarioId, lRangeCheck)
            End If
            If pUci.OpnBlks("RCHRES").Count > 0 Then
                'this operation type exists
                Logger.Status("Adding RCHRES to database...")
                GetOperInfo("RCHRES", lNewScenarioId, lRangeCheck)
            End If
        End If
        Logger.Status("")
        Logger.Status("HIDE")

        With pGrid.Source
            Dim lRow As Integer = .Rows
            .CellValue(lRow, 0) = lName   'name
            .CellValue(lRow, 1) = pWatershedName  'wat name
            .CellValue(lRow, 2) = lNewScenarioId   'id
        End With
        pGrid.Refresh()

        Me.Close()
    End Sub

    Private Sub GetOperInfo(ByVal aOpName As String, ByVal aScenarioID As Integer, ByVal aRangeCheck As Boolean)
        'find next available id number 
        Dim lTable As DataTable = Database.GetTable("SegData")
        Dim lNewSegId As Integer = 0
        Dim lTmpId As Integer = 0
        lNewSegId = lTable.Rows(lTable.Rows.Count - 1).Item(0).ToString

        'find next available parm data id number 
        Dim lParmDataTable As DataTable = Database.GetTable("ParmData")
        Dim lParmDataId As Integer = 0
        lParmDataId = lParmDataTable.Rows(lParmDataTable.Rows.Count - 1).Item(0).ToString

        Dim lOpTypId As Integer = 0
        If aOpName = "PERLND" Then
            lOpTypId = 1
        ElseIf aOpName = "IMPLND" Then
            lOpTypId = 2
        ElseIf aOpName = "RCHRES" Then
            lOpTypId = 3
        End If

        Dim lProgressTotal As Integer = 0
        For Each lOpn As HspfOperation In pUci.OpnBlks(aOpName).Ids
            lProgressTotal += lOpn.Tables.Count
        Next
        Dim lProgressCount As Integer = 0
        'find information about hspf operation type
        For Each lOpn As HspfOperation In pUci.OpnBlks(aOpName).Ids
            lNewSegId += 1
            Dim lValues As New Collection
            lValues.Add(lNewSegId)
            Dim lTmp As String = lOpn.Name & lOpn.Id.ToString.PadLeft(8)
            lValues.Add("'" & lTmp & "'")
            lValues.Add("'" & lOpn.Description & "'")
            lValues.Add(lOpTypId)
            lValues.Add(aScenarioID)
            Database.InsertRowIntoTable("SegData", lValues)

            For Each lTab As HspfTable In lOpn.Tables
                lProgressCount += 1
                Logger.Progress(lProgressCount, lProgressTotal)
                Dim lOccur As Integer = 1
                If lTab.OccurCount > 1 Or lTab.OccurIndex > 1 Then
                    'get occurrance number 
                    If lTab.OccurIndex > 0 Then
                        lOccur = lTab.OccurIndex
                    Else
                        lOccur = lTab.OccurNum
                    End If
                End If
                Dim lTableId As Integer = TableIDFromTableName(lTab.Name, lOpTypId)   'like PRINT-INFO 1(PERLND) returns 2
                If lTableId > 0 Then
                    For Each lParm As HspfParm In lTab.Parms
                        lParmDataId += 1
                        'myParmData = myDB.OpenRecordset("ParmData", dbOpenDynaset)
                        'ID	    ParmID	SegID	Occur	Value
                        '1	    1	    1	    1	    0
                        '157	13	    1	    1	    4
                        '545033	600	    1364	5	    1.4E3
                        Dim lParmId As Integer = ParmIDFromParmName(lParm.Name, lTableId)   'like AIRTPR 2 returns 13
                        If lParmId > 0 Then
                            If aRangeCheck AndAlso lParm.Def.Typ > 1 Then
                                'check here to see if this real number is within the normal range
                                Dim lMax As Single = 0.0
                                Dim lMin As Single = 0.0
                                Dim lVal As Single = 0.0
                                Dim lHSPFMax As Single = lParm.Def.Max
                                Dim lHSPFMin As Single = lParm.Def.Min
                                If pUci.GlobalBlock.EmFg = 2 Then
                                    lHSPFMax = lParm.Def.MetricMax
                                    lHSPFMin = lParm.Def.MetricMin
                                End If
                                ParmMinMax(lParmId, lMin, lMax)
                                If IsNumeric(lParm.Value) Then
                                    lVal = CSng(lParm.Value)
                                    If lVal > lMax Then
                                        Logger.Msg("The value for parameter " & lParm.Name & " is greater than the greatest value of this parameter " & _
                                                   "in the HSPFParm database." & vbCrLf & vbCrLf & "Operation: " & lOpn.Name & " " & lOpn.Id & _
                                                   "   Value: " & lVal & "   Max: " & lMax & "   HSPF Max: " & lHSPFMax, MsgBoxStyle.Critical, _
                                                   "HSPFParm Value Beyond Normal Range")
                                    End If
                                    If lVal < lMin Then
                                        Logger.Msg("The value for parameter " & lParm.Name & " is lower than the lowest value of this parameter " & _
                                                   "in the HSPFParm database." & vbCrLf & vbCrLf & "Operation: " & lOpn.Name & " " & lOpn.Id & _
                                                   "   Value: " & lVal & "   Min: " & lMin & "   HSPF Min: " & lHSPFMin, MsgBoxStyle.Critical, _
                                                   "HSPFParm Value Beyond Normal Range")
                                    End If
                                End If
                            End If
                            Dim lParmValues As New Collection
                            lParmValues.Add(lParmDataId)
                            lParmValues.Add(lParmId)
                            lParmValues.Add(lNewSegId)
                            lParmValues.Add(lOccur)
                            lParmValues.Add("'" & lParm.Value & "'")
                            Database.InsertRowIntoTable("ParmData", lParmValues)
                        End If
                    Next
                End If
            Next
        Next
    End Sub

    Private Function TableIDFromTableName(ByVal aTableName As String, ByVal aOpTypId As Integer) As Integer
        'myParmTableDefn = myDB.OpenRecordset("ParmTableDefn", dbOpenDynaset)
        'ID	Name	OpnTypID	Alias	TableNumber	Definition
        '1	ACTIVITY	1	FALSE	1	
        '222	GQ-VALUES	3	FALSE	55	
        Dim lTableId As Integer = 0
        Dim lCrit As String = " Name = '" & aTableName & "' AND " & " OpnTypID = " & aOpTypId
        Dim lStr As String = "SELECT DISTINCTROW ParmTableDefn.ID " & _
                                                "From ParmTableDefn " & _
                                                "WHERE (" & lCrit & ")"
        Dim lTable As DataTable = Database.GetTable(lStr)
        If lTable.Rows.Count > 0 Then
            lTableId = lTable.Rows(0).Item(0).ToString
        End If
        Return lTableId
    End Function

    Private Function ParmIDFromParmName(ByVal aParmName As String, ByVal aTableId As Integer) As Integer
        'myParmDefn = myDB.OpenRecordset("ParmDefn", dbOpenDynaset)
        'ID	Name	Assoc	AssocID	ParmTypeID	ParmTableID	Min	Max	Def	StartCol	Width	Definition
        '1	AIRTFG	 	    0	    2	        1	0	1	0	11	5	
        '2	SNOWFG	 	    0	    2	        1	0	1	0	16	5	
        '13	AIRTPR	 	    0	    2	        2	2	6	4	11	5	
        '1478	BEDDEP	 	0	    3	        204	0	<none>	0	11	10	
        '1479	SANDFR	 	0	    3	        204	9.999999E-05	1	1	21	10	
        Dim lParmId As Integer = 0
        Dim lCrit As String = " Name = '" & aParmName & "' AND " & " ParmTableID = " & aTableId
        Dim lStr As String = "SELECT DISTINCTROW ParmDefn.ID " & _
                                                "From ParmDefn " & _
                                                "WHERE (" & lCrit & ")"
        Dim lTable As DataTable = Database.GetTable(lStr)
        If lTable.Rows.Count > 0 Then
            lParmId = lTable.Rows(0).Item(0).ToString
        End If
        Return lParmId
    End Function

    Sub ParmMinMax(ByVal aParmId As Integer, ByRef aMin As Single, ByRef aMax As Single)
        'find min and max values for this parameter
        Dim lParmCrit As String = "ParmID = " & aParmId & " OR AssocID = " & aParmId
        Dim lStr As String = "SELECT DISTINCTROW ParmTableData.SegID, " & _
                                                "ParmTableData.OpnTypID, " & _
                                                "ParmTableData.Name, " & _
                                                "ParmTableData.ParmID, " & _
                                                "ParmTableData.Value, " & _
                                                "ParmTableData.Table, " & _
                                                "ParmTableData.Occur, " & _
                                                "ParmTabledata.AliasInfo " & _
                                                "From ParmTableData " & _
                                                "WHERE (" & lParmCrit & ")"
        Dim lTable As DataTable = Database.GetTable(lStr)
        aMin = lTable.Rows(0).Item(4)
        aMax = lTable.Rows(0).Item(4)
        Dim lTmp As Single
        If lTable.Rows.Count > 0 Then
            For lRow As Integer = 1 To lTable.Rows.Count - 1
                lTmp = lTable.Rows(lRow).Item(4)
                If lTmp > aMax Then
                    aMax = lTmp
                End If
                If lTmp < aMin Then
                    aMin = lTmp
                End If
            Next
        End If

    End Sub

    Private Sub cmdSet_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSet.Click
        If cdUCI.ShowDialog() = Windows.Forms.DialogResult.OK Then
            pUci = New HspfUci

            Dim pMsg As New HspfMsg
            pMsg.Open("hspfmsg.mdb")

            txtUCIName.Text = cdUCI.FileName
            Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
            Logger.Status("SHOW")
            Logger.Status("Reading UCI: " & cdUCI.FileName)
            pUci.FastReadUciForStarter(pMsg, cdUCI.FileName)
            Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
            'Logger.Status("")

            txtName.Text = FilenameNoExt(FilenameNoPath(cdUCI.FileName))
            txtStartDate.Text = DumpDate(Date2J(pUci.GlobalBlock.SDate(0), pUci.GlobalBlock.SDate(1), pUci.GlobalBlock.SDate(2), pUci.GlobalBlock.SDate(3), pUci.GlobalBlock.SDate(4)))
            txtEndDate.Text = DumpDate(Date2J(pUci.GlobalBlock.EDate(0), pUci.GlobalBlock.EDate(1), pUci.GlobalBlock.EDate(2), pUci.GlobalBlock.EDate(3), pUci.GlobalBlock.EDate(4)))
            txtNumReaches.Text = pUci.OpnBlks("RCHRES").Count
            txtNumSegments.Text = pUci.MetSegs.Count
            txtUnits.Text = pUci.GlobalBlock.EmFg
        End If
    End Sub

End Class