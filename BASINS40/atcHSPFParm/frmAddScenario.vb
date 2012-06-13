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
                GetOperInfo(Database, pUci, "PERLND", lNewScenarioId, lRangeCheck)
            End If
            If pUci.OpnBlks("IMPLND").Count > 0 Then
                'this operation type exists
                Logger.Status("Adding IMPLND to database...")
                GetOperInfo(Database, pUci, "IMPLND", lNewScenarioId, lRangeCheck)
            End If
            If pUci.OpnBlks("RCHRES").Count > 0 Then
                'this operation type exists
                Logger.Status("Adding RCHRES to database...")
                GetOperInfo(Database, pUci, "RCHRES", lNewScenarioId, lRangeCheck)
            End If
        End If
        Logger.Status("")
        Logger.Status("HIDE")

        If FileExists(pUci.Name) Then 'make a copy of the UCI file just added as a new scenario
            Dim lArchiveFolder As String = IO.Path.Combine(IO.Path.GetDirectoryName(Database.Name), "Archive")
            If Not IO.Directory.Exists(lArchiveFolder) Then MkDir(lArchiveFolder)
            TryCopy(pUci.Name, IO.Path.Combine(lArchiveFolder, IO.Path.GetFileName(pUci.Name)))
        End If
        With pGrid.Source
            Dim lRow As Integer = .Rows
            .CellValue(lRow, 0) = lName   'name
            .CellValue(lRow, 1) = pWatershedName  'wat name
            .CellValue(lRow, 2) = lNewScenarioId   'id
        End With
        pGrid.Refresh()

        Me.Close()
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