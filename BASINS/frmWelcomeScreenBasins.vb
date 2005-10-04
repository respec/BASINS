'********************************************************************************************************
'File Name: frmWelcomeScreenBasins.vb
'Description: MapWindowBasins Welcome Screen
'********************************************************************************************************
'The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License"); 
'you may not use this file except in compliance with the License. You may obtain a copy of the License at 
'http://www.mozilla.org/MPL/ 
'Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
'ANY KIND, either express or implied. See the License for the specificlanguage governing rights and 
'limitations under the License. 
'
'The Original Code is MapWindow Open Source. 
'
'The Initial Developer of this version of the Original Code is Daniel P. Ames using portions created by 
'Utah State University and the Idaho National Engineering and Environmental Lab that were released as 
'public domain in March 2004.  
'
'Contributor(s): (Open source contributors should list themselves and their modifications here). 
'2/3/2005 - made spacing of text relative if no recent projexts, added TODOs (jlk)
'3/16/2005 - adapted for BASINS welcome
'********************************************************************************************************
Imports System.Windows.Forms.SendKeys
Imports MapWindow.Interfaces
Imports System.Windows.Forms

Public Class frmWelcomeScreenBasins
  Inherits System.Windows.Forms.Form

  Public Shared prj As Project
  Public Shared app As AppInfo

#Region " Windows Form Designer generated code "

  Public Sub New()
    MyBase.New()

    'This call is required by the Windows Form Designer.
    InitializeComponent()

    'Add any initialization after the InitializeComponent() call
  End Sub

  'Form overrides dispose to clean up the component list.
  Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
    If disposing Then
      If Not (components Is Nothing) Then
        components.Dispose()
      End If
    End If
    MyBase.Dispose(disposing)
  End Sub

  'Required by the Windows Form Designer
  Private components As System.ComponentModel.IContainer

  'NOTE: The following procedure is required by the Windows Form Designer
  'It can be modified using the Windows Form Designer.  
  'Do not modify it using the code editor.
  Friend WithEvents lbOpenProject As System.Windows.Forms.LinkLabel
  Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
  Friend WithEvents PictureBox3 As System.Windows.Forms.PictureBox
  Friend WithEvents btnClose As System.Windows.Forms.Button
  Friend WithEvents cbShowDlg As System.Windows.Forms.CheckBox
  Friend WithEvents lbProject1 As System.Windows.Forms.LinkLabel
  Friend WithEvents lbProject2 As System.Windows.Forms.LinkLabel
  Friend WithEvents lbProject3 As System.Windows.Forms.LinkLabel
  Friend WithEvents lbBuildNew As System.Windows.Forms.LinkLabel
  Friend WithEvents lbBasinsHelp As System.Windows.Forms.LinkLabel
  Friend WithEvents pctBasinsLogo As System.Windows.Forms.PictureBox
  Friend WithEvents lbConvert As System.Windows.Forms.LinkLabel
  Friend WithEvents lbProject4 As System.Windows.Forms.LinkLabel
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
    Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmWelcomeScreenBasins))
    Me.lbBuildNew = New System.Windows.Forms.LinkLabel
    Me.lbOpenProject = New System.Windows.Forms.LinkLabel
    Me.PictureBox2 = New System.Windows.Forms.PictureBox
    Me.PictureBox3 = New System.Windows.Forms.PictureBox
    Me.cbShowDlg = New System.Windows.Forms.CheckBox
    Me.btnClose = New System.Windows.Forms.Button
    Me.lbProject1 = New System.Windows.Forms.LinkLabel
    Me.lbProject2 = New System.Windows.Forms.LinkLabel
    Me.lbProject3 = New System.Windows.Forms.LinkLabel
    Me.pctBasinsLogo = New System.Windows.Forms.PictureBox
    Me.lbBasinsHelp = New System.Windows.Forms.LinkLabel
    Me.lbConvert = New System.Windows.Forms.LinkLabel
    Me.lbProject4 = New System.Windows.Forms.LinkLabel
    Me.SuspendLayout()
    '
    'lbBuildNew
    '
    Me.lbBuildNew.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lbBuildNew.Location = New System.Drawing.Point(352, 16)
    Me.lbBuildNew.Name = "lbBuildNew"
    Me.lbBuildNew.Size = New System.Drawing.Size(230, 18)
    Me.lbBuildNew.TabIndex = 1
    Me.lbBuildNew.TabStop = True
    Me.lbBuildNew.Text = "Build BASINS Project"
    '
    'lbOpenProject
    '
    Me.lbOpenProject.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lbOpenProject.Location = New System.Drawing.Point(352, 112)
    Me.lbOpenProject.Name = "lbOpenProject"
    Me.lbOpenProject.Size = New System.Drawing.Size(256, 18)
    Me.lbOpenProject.TabIndex = 2
    Me.lbOpenProject.TabStop = True
    Me.lbOpenProject.Text = "Open Existing BASINS Project"
    '
    'PictureBox2
    '
    Me.PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), System.Drawing.Image)
    Me.PictureBox2.Location = New System.Drawing.Point(289, 28)
    Me.PictureBox2.Name = "PictureBox2"
    Me.PictureBox2.Size = New System.Drawing.Size(19, 18)
    Me.PictureBox2.TabIndex = 6
    Me.PictureBox2.TabStop = False
    '
    'PictureBox3
    '
    Me.PictureBox3.Image = CType(resources.GetObject("PictureBox3.Image"), System.Drawing.Image)
    Me.PictureBox3.Location = New System.Drawing.Point(289, 65)
    Me.PictureBox3.Name = "PictureBox3"
    Me.PictureBox3.Size = New System.Drawing.Size(19, 18)
    Me.PictureBox3.TabIndex = 7
    Me.PictureBox3.TabStop = False
    '
    'cbShowDlg
    '
    Me.cbShowDlg.Location = New System.Drawing.Point(16, 216)
    Me.cbShowDlg.Name = "cbShowDlg"
    Me.cbShowDlg.Size = New System.Drawing.Size(192, 23)
    Me.cbShowDlg.TabIndex = 11
    Me.cbShowDlg.Text = "Show this dialog at startup"
    '
    'btnClose
    '
    Me.btnClose.BackColor = System.Drawing.SystemColors.Control
    Me.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK
    Me.btnClose.Location = New System.Drawing.Point(240, 216)
    Me.btnClose.Name = "btnClose"
    Me.btnClose.Size = New System.Drawing.Size(90, 26)
    Me.btnClose.TabIndex = 12
    Me.btnClose.Text = "Close"
    '
    'lbProject1
    '
    Me.lbProject1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lbProject1.Location = New System.Drawing.Point(368, 136)
    Me.lbProject1.Name = "lbProject1"
    Me.lbProject1.Size = New System.Drawing.Size(192, 19)
    Me.lbProject1.TabIndex = 13
    Me.lbProject1.TabStop = True
    Me.lbProject1.Text = "Project1"
    '
    'lbProject2
    '
    Me.lbProject2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lbProject2.Location = New System.Drawing.Point(368, 160)
    Me.lbProject2.Name = "lbProject2"
    Me.lbProject2.Size = New System.Drawing.Size(192, 18)
    Me.lbProject2.TabIndex = 14
    Me.lbProject2.TabStop = True
    Me.lbProject2.Text = "Project2"
    '
    'lbProject3
    '
    Me.lbProject3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lbProject3.Location = New System.Drawing.Point(368, 184)
    Me.lbProject3.Name = "lbProject3"
    Me.lbProject3.Size = New System.Drawing.Size(192, 18)
    Me.lbProject3.TabIndex = 15
    Me.lbProject3.TabStop = True
    Me.lbProject3.Text = "Project3"
    '
    'pctBasinsLogo
    '
    Me.pctBasinsLogo.Image = CType(resources.GetObject("pctBasinsLogo.Image"), System.Drawing.Image)
    Me.pctBasinsLogo.Location = New System.Drawing.Point(0, -8)
    Me.pctBasinsLogo.Name = "pctBasinsLogo"
    Me.pctBasinsLogo.Size = New System.Drawing.Size(344, 216)
    Me.pctBasinsLogo.TabIndex = 17
    Me.pctBasinsLogo.TabStop = False
    '
    'lbBasinsHelp
    '
    Me.lbBasinsHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lbBasinsHelp.Location = New System.Drawing.Point(352, 80)
    Me.lbBasinsHelp.Name = "lbBasinsHelp"
    Me.lbBasinsHelp.Size = New System.Drawing.Size(230, 18)
    Me.lbBasinsHelp.TabIndex = 18
    Me.lbBasinsHelp.TabStop = True
    Me.lbBasinsHelp.Text = "BASINS Help"
    '
    'lbConvert
    '
    Me.lbConvert.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lbConvert.Location = New System.Drawing.Point(352, 48)
    Me.lbConvert.Name = "lbConvert"
    Me.lbConvert.Size = New System.Drawing.Size(230, 18)
    Me.lbConvert.TabIndex = 19
    Me.lbConvert.TabStop = True
    Me.lbConvert.Text = "Convert BASINS 3 Project"
    '
    'lbProject4
    '
    Me.lbProject4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Me.lbProject4.Location = New System.Drawing.Point(368, 208)
    Me.lbProject4.Name = "lbProject4"
    Me.lbProject4.Size = New System.Drawing.Size(192, 18)
    Me.lbProject4.TabIndex = 20
    Me.lbProject4.TabStop = True
    Me.lbProject4.Text = "Project4"
    '
    'frmWelcomeScreenBasins
    '
    Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
    Me.BackColor = System.Drawing.Color.White
    Me.CancelButton = Me.btnClose
    Me.ClientSize = New System.Drawing.Size(610, 250)
    Me.Controls.Add(Me.lbProject4)
    Me.Controls.Add(Me.lbConvert)
    Me.Controls.Add(Me.lbBasinsHelp)
    Me.Controls.Add(Me.pctBasinsLogo)
    Me.Controls.Add(Me.lbProject3)
    Me.Controls.Add(Me.lbProject2)
    Me.Controls.Add(Me.lbProject1)
    Me.Controls.Add(Me.btnClose)
    Me.Controls.Add(Me.cbShowDlg)
    Me.Controls.Add(Me.PictureBox3)
    Me.Controls.Add(Me.PictureBox2)
    Me.Controls.Add(Me.lbOpenProject)
    Me.Controls.Add(Me.lbBuildNew)
    Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
    Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
    Me.MaximizeBox = False
    Me.MinimizeBox = False
    Me.Name = "frmWelcomeScreenBasins"
    Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
    Me.Text = "Welcome to BASINS 4"
    Me.ResumeLayout(False)

  End Sub

#End Region

  Private Sub lbBuildNew_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lbBuildNew.LinkClicked
    'TODO: don't hard code path
    prj.Load("d:\basins\data\national\national.mwprj")
    Me.Close()
  End Sub

  Private Sub lbOpenProject_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lbOpenProject.LinkClicked
    Dim dlg As New OpenFileDialog

    dlg.Filter = "MapWindow Project Files (*.mwprj)|*.mwprj"
    dlg.CheckFileExists = True
    dlg.InitialDirectory = app.DefaultDir
    If dlg.ShowDialog(Me) = DialogResult.OK Then
      prj.Load(dlg.FileName)
      Me.DialogResult = DialogResult.OK
      Me.Close()
    End If
  End Sub

  Private Sub cbShowDlg_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbShowDlg.CheckedChanged
    app.ShowWelcomeScreen = cbShowDlg.Checked
  End Sub

  Private Sub lbProject_LinkClicked(ByVal sender As System.Object, _
      ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) _
      Handles lbProject1.LinkClicked, lbProject2.LinkClicked, lbProject3.LinkClicked, lbProject4.LinkClicked

    Dim fileName As String = CStr(CType(sender, Label).Tag)
    If (System.IO.File.Exists(fileName)) Then
      prj.Load(fileName)
      Me.DialogResult = DialogResult.OK
      Me.Close()
    Else
      'TODO - 2/3/2005 - jlk - need a findFile here 
      MsgBox("Could not find " & fileName, MsgBoxStyle.Exclamation)
    End If
  End Sub

  Private Sub frmWelcomeScreen_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

    cbShowDlg.Checked = app.ShowWelcomeScreen

    'assume no recent projects
    lbProject1.Visible = False
    lbProject2.Visible = False
    lbProject3.Visible = False
    lbProject4.Visible = False

    'check to see if there were any recent projects
    Dim lRecentCount As Integer = 0
    Dim lCurrent As Integer = 0
    Dim lProjectName As String
    Dim lProjectId As String
    Dim lbProject As Label
    While lRecentCount < 4 And lCurrent < prj.RecentProjects.Count
      lProjectName = CType(prj.RecentProjects(lCurrent), String)
      lProjectId = System.IO.Path.GetFileNameWithoutExtension(lProjectName)
      If LCase(lProjectId) <> "national" Then
        If lRecentCount = 0 Then
          lbProject = lbProject1
        ElseIf lRecentCount = 1 Then
          lbProject = lbProject2
        ElseIf lRecentCount = 2 Then
          lbProject = lbProject3
        ElseIf lRecentCount = 3 Then
          lbProject = lbProject4
        End If
        lbProject.Text = lProjectId
        lbProject.Tag = lProjectName
        lbProject.Visible = True
        lRecentCount += 1
      End If
      lCurrent += 1
    End While
  End Sub

  Private Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
    Me.Close()
  End Sub

  Private Sub lbConvert_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lbConvert.LinkClicked
    MsgBox("Select the BASINS Project to convert from the PullDown Menu that appears when you click OK", MsgBoxStyle.OKOnly, "BASINS 4")
    Me.Close()
    SendKeys.Send("%FB")
  End Sub

  Private Sub lbBasinsHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbBasinsHelp.Click
    System.Diagnostics.Process.Start(app.DefaultDir & "\Help\Basins4.chm")
  End Sub
End Class