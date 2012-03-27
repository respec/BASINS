Imports atcUtility
Imports System.Data

Public Class frmScenarioDetails
    Friend pScenarioIDs As atcCollection
    Friend pCurrentIndex As Integer
    <CLSCompliant(False)> Friend Database As atcUtility.atcMDB

    Public Sub InitializeUI(ByVal aIds As atcCollection, ByVal aFirstIndex As Integer, ByVal aDatabase As atcUtility.atcMDB)
        pScenarioIDs = aIds
        If aFirstIndex = -1 Then
            aFirstIndex = 1
        End If
        pCurrentIndex = aFirstIndex
        Database = aDatabase
        LoadScenario(pScenarioIDs(pCurrentIndex - 1))
    End Sub

    Private Sub LoadScenario(ByVal aScenarioID As Integer)
        Dim lCrit As String = "ID = " & aScenarioID.ToString
        Dim lStr As String = "SELECT DISTINCTROW ScenarioData.ID, " & _
                                                "ScenarioData.Name, " & _
                                                "ScenarioData.Type, " & _
                                                "ScenarioData.UCIName, " & _
                                                "ScenarioData.WatershedID, " & _
                                                "ScenarioData.StartDate, " & _
                                                "ScenarioData.EndDate, " & _
                                                "ScenarioData.UCIUnits, " & _
                                                "ScenarioData.NumSegments, " & _
                                                "ScenarioData.NumReaches, " & _
                                                "ScenarioData.LandUseType, " & _
                                                "ScenarioData.Channels, " & _
                                                "ScenarioData.WQConstituents, " & _
                                                "ScenarioData.ChemicalSources, " & _
                                                "ScenarioData.StudyPurpose, " & _
                                                "ScenarioData.Version, " & _
                                                "ScenarioData.ApplicationReference, " & _
                                                "ScenarioData.ContactName, " & _
                                                "ScenarioData.ContactOrganization, " & _
                                                "ScenarioData.ContactPhoneEmail, " & _
                                                "ScenarioData.Comments " & _
                                                "From ScenarioData " & _
                                                "WHERE (" & lCrit & ")"
        Dim lTable As DataTable = Database.GetTable(lStr)

        TextBox1.Text = lTable.Rows(0).Item(0).ToString
        TextBox2.Text = lTable.Rows(0).Item(1).ToString
        TextBox3.Text = lTable.Rows(0).Item(2).ToString
        TextBox4.Text = lTable.Rows(0).Item(3).ToString
        TextBox5.Text = lTable.Rows(0).Item(4).ToString
        TextBox6.Text = lTable.Rows(0).Item(5).ToString
        TextBox7.Text = lTable.Rows(0).Item(6).ToString
        TextBox8.Text = lTable.Rows(0).Item(7).ToString
        TextBox9.Text = lTable.Rows(0).Item(8).ToString
        TextBox10.Text = lTable.Rows(0).Item(9).ToString
        TextBox11.Text = lTable.Rows(0).Item(10).ToString
        TextBox12.Text = lTable.Rows(0).Item(11).ToString
        TextBox13.Text = lTable.Rows(0).Item(12).ToString
        TextBox14.Text = lTable.Rows(0).Item(13).ToString
        TextBox15.Text = lTable.Rows(0).Item(14).ToString
        TextBox16.Text = lTable.Rows(0).Item(15).ToString
        TextBox17.Text = lTable.Rows(0).Item(16).ToString
        TextBox18.Text = lTable.Rows(0).Item(17).ToString
        TextBox19.Text = lTable.Rows(0).Item(18).ToString
        TextBox20.Text = lTable.Rows(0).Item(19).ToString
        TextBox21.Text = lTable.Rows(0).Item(20).ToString
    End Sub

    Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        Me.Close()
    End Sub

    Private Sub cmdFirst_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdFirst.Click
        pCurrentIndex = 1
        LoadScenario(pScenarioIDs(0))
    End Sub

    Private Sub cmdPrev_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPrev.Click
        If pCurrentIndex > 1 Then
            pCurrentIndex -= 1
            LoadScenario(pScenarioIDs(pCurrentIndex - 1))
        End If
    End Sub

    Private Sub cmdNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdNext.Click
        If pCurrentIndex < pScenarioIDs.Count Then
            pCurrentIndex += 1
            LoadScenario(pScenarioIDs(pCurrentIndex - 1))
        End If
    End Sub

    Private Sub cmdLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdLast.Click
        pCurrentIndex = pScenarioIDs.Count
        LoadScenario(pScenarioIDs(pScenarioIDs.Count - 1))
    End Sub
End Class