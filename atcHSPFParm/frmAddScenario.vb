Imports atcUtility
Imports MapWinUtility
Imports System.Data
Imports atcUCI

Public Class frmAddScenario
    Friend pScenarioIDs As atcCollection
    Friend pCurrentIndex As Integer
    Friend pGrid As atcControls.atcGrid
    Friend pUci As atcUCI.HspfUci
    <CLSCompliant(False)> Friend Database As atcUtility.atcMDB

    Public Sub InitializeUI(ByVal aDatabase As atcUtility.atcMDB, ByVal aGrid As atcControls.atcGrid, ByVal aWatershedId As String)
        Database = aDatabase
        pGrid = aGrid
        txtWatershedId.Text = aWatershedId
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

        'save to database
        Dim lValues As New Collection
        lValues.Add(lMaxId + 1)
        'lValues.Add("'" & lProject & "'")
        'lValues.Add("'" & lLocation & "'")
        'lValues.Add("'" & lSetting & "'")
        'lValues.Add("'" & lWeather & "'")
        'lValues.Add("'" & lArea & "'")
        'lValues.Add("'" & lHuc & "'")
        'lValues.Add(lLatitude)
        'lValues.Add(lLongitude)
        'lValues.Add(lX)
        'lValues.Add(lY)
        lValues.Add("'" & lComments & "'")
        Database.InsertRowIntoTable("ScenarioData", lValues)

        If Not pUci Is Nothing Then
            'GetScenInfo(scnID)
        End If

        Me.Close()
    End Sub

    Private Sub cmdSet_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSet.Click
        If cdUCI.ShowDialog() = Windows.Forms.DialogResult.OK Then
            pUci = New HspfUci

            Dim pMsg As New HspfMsg
            pMsg.Open("hspfmsg.mdb")

            txtUCIName.Text = cdUCI.FileName
            Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
            'Logger.Status("Reading UCI: " & cdUCI.FileName)
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