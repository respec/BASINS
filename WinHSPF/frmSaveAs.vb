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

        pCurrentDirectory = PathNameOnly(pUCIFullFileName)
        atxName.Text = FilenameNoExt(pUCI.Name)

    End Sub

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Me.Dispose()
    End Sub

    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click

        Dim lRelAbs As Integer
        If rbnRelative.Checked Then
            lRelAbs = 1
        Else
            lRelAbs = 2
        End If

        Dim lOldName As String = FilenameNoExt(pUCI.Name)

        pUCI.Name = FilenameNoPath(txtPath.Text)
        If Not pUCI.Name.ToUpper.EndsWith(".UCI") Then  'force to have uci extension
            pUCI.Name = pUCI.Name & ".uci"
        End If

        Dim lNewName As String = FilenameNoExt(pUCI.Name)
        pUCI.SaveAs(lOldName, lNewName, CInt(atxBase.Text), lRelAbs)
        'AddRecentFile(mnuRecent, pUci.Name)

        'set UCI name in caption
        If pWinHSPF IsNot Nothing Then
            pWinHSPF.Text = pWinHSPF.Tag & ": " & pUCI.Name
        End If

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