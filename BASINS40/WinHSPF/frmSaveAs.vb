Imports atcUtility
Imports MapWinUtility

Public Class frmSaveAs

    Dim pCurrentDirectory As String = ""

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        Me.Icon = pIcon
        Me.MinimumSize = Me.Size
        Me.MaximumSize = Me.Size

        pCurrentDirectory = PathNameOnly(AbsolutePath(pUCI.Name, CurDir))
        atxName.Text = FilenameNoExt(pUCI.Name)
        'HSPFMain.newname = ""

    End Sub

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Me.Dispose()
    End Sub

    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
        'HSPFMain.newname = txtPath
        'If optRelAbs(0).Value Then
        '    HSPFMain.relabs = 1
        'Else
        '    HSPFMain.relabs = 2
        'End If
        'HSPFMain.basedsn = ATCoTextBase.Value

        '    myUci.Name = newname
        '    If Len(myUci.Name) > 3 Then
        '        If UCase(Right(myUci.Name, 4)) <> ".UCI" Then  'force to have uci extension
        '            myUci.Name = myUci.Name & ".uci"
        '        End If
        '    End If
        '    newname = FilenameOnly(myUci.Name)
        '    myUci.SaveAs(oldname, newname, basedsn, relabs)
        '    AddRecentFile(mnuRecent, myUci.Name)
        '    Caption = BaseCaption & ": " & FilenameOnly(myUci.Name)
        Me.Dispose()
    End Sub

    Private Sub cmdFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdFile.Click

        SaveFileDialog1.Filter = "HSPF User Control Input Files (*.uci)|*.uci"
        SaveFileDialog1.FileName = atxName.Text & ".uci"
        SaveFileDialog1.InitialDirectory = CurDir()
        SaveFileDialog1.Title = "Select Scenario Path"

        If SaveFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            pCurrentDirectory = PathNameOnly(SaveFileDialog1.FileName)
            atxName.Text = FilenameNoPath(FilenameNoExt(SaveFileDialog1.FileName))
            atxName_Change()
        End If
    End Sub

    Private Sub atxName_Change() Handles atxName.Change
        If Len(atxName.Text) > 8 Then atxName.Text = atxName.Text.Substring(0, 8)
        txtPath.Text = pCurrentDirectory & "\" & atxName.Text & ".uci"
    End Sub
End Class