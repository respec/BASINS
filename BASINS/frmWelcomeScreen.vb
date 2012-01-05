'********************************************************************************************************
'File Name: frmWelcomeScreen.vb
'Description: MapWindow Welcome Screen
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
Imports MapWinUtility
Imports atcUtility

Public Class frmWelcomeScreen
    Inherits System.Windows.Forms.Form

    Private lProject As Project
    Private lAppInfo As AppInfo

#Region " Windows Form Designer generated code "
    <CLSCompliant(False)> _
    Public Sub New(ByVal aProject As Project, ByVal aAppInfo As AppInfo)
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        lProject = aProject
        lAppInfo = aAppInfo
    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal aDisposing As Boolean)
        If aDisposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(aDisposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents lblOpenProject As System.Windows.Forms.LinkLabel
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents cboShowDlg As System.Windows.Forms.CheckBox
    Friend WithEvents lblProject1 As System.Windows.Forms.LinkLabel
    Friend WithEvents lblProject2 As System.Windows.Forms.LinkLabel
    Friend WithEvents lblProject3 As System.Windows.Forms.LinkLabel
    Friend WithEvents lblProject4 As System.Windows.Forms.LinkLabel
    Friend WithEvents lblBuildNew As System.Windows.Forms.LinkLabel
    Friend WithEvents lblHelp As System.Windows.Forms.LinkLabel
    Friend WithEvents picProgramLogo As System.Windows.Forms.PictureBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmWelcomeScreen))
        Me.lblBuildNew = New System.Windows.Forms.LinkLabel
        Me.lblOpenProject = New System.Windows.Forms.LinkLabel
        Me.cboShowDlg = New System.Windows.Forms.CheckBox
        Me.btnClose = New System.Windows.Forms.Button
        Me.lblProject1 = New System.Windows.Forms.LinkLabel
        Me.lblProject2 = New System.Windows.Forms.LinkLabel
        Me.lblProject3 = New System.Windows.Forms.LinkLabel
        Me.lblProject4 = New System.Windows.Forms.LinkLabel
        Me.picProgramLogo = New System.Windows.Forms.PictureBox
        Me.lblHelp = New System.Windows.Forms.LinkLabel
        CType(Me.picProgramLogo, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblBuildNew
        '
        resources.ApplyResources(Me.lblBuildNew, "lblBuildNew")
        Me.lblBuildNew.Name = "lblBuildNew"
        Me.lblBuildNew.TabStop = True
        '
        'lblOpenProject
        '
        resources.ApplyResources(Me.lblOpenProject, "lblOpenProject")
        Me.lblOpenProject.Name = "lblOpenProject"
        Me.lblOpenProject.TabStop = True
        '
        'cboShowDlg
        '
        resources.ApplyResources(Me.cboShowDlg, "cboShowDlg")
        Me.cboShowDlg.Name = "cboShowDlg"
        '
        'btnClose
        '
        Me.btnClose.BackColor = System.Drawing.SystemColors.Control
        Me.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK
        resources.ApplyResources(Me.btnClose, "btnClose")
        Me.btnClose.Name = "btnClose"
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'lblProject1
        '
        resources.ApplyResources(Me.lblProject1, "lblProject1")
        Me.lblProject1.Name = "lblProject1"
        Me.lblProject1.TabStop = True
        Me.lblProject1.UseCompatibleTextRendering = True
        '
        'lblProject2
        '
        resources.ApplyResources(Me.lblProject2, "lblProject2")
        Me.lblProject2.Name = "lblProject2"
        Me.lblProject2.TabStop = True
        Me.lblProject2.UseCompatibleTextRendering = True
        '
        'lblProject3
        '
        resources.ApplyResources(Me.lblProject3, "lblProject3")
        Me.lblProject3.Name = "lblProject3"
        Me.lblProject3.TabStop = True
        Me.lblProject3.UseCompatibleTextRendering = True
        '
        'lblProject4
        '
        resources.ApplyResources(Me.lblProject4, "lblProject4")
        Me.lblProject4.Name = "lblProject4"
        Me.lblProject4.TabStop = True
        Me.lblProject4.UseCompatibleTextRendering = True
        '
        'picProgramLogo
        '
        resources.ApplyResources(Me.picProgramLogo, "picProgramLogo")
        Me.picProgramLogo.Name = "picProgramLogo"
        Me.picProgramLogo.TabStop = False
        '
        'lblHelp
        '
        resources.ApplyResources(Me.lblHelp, "lblHelp")
        Me.lblHelp.Name = "lblHelp"
        Me.lblHelp.TabStop = True
        '
        'frmWelcomeScreen
        '
        resources.ApplyResources(Me, "$this")
        Me.BackColor = System.Drawing.Color.White
        Me.CancelButton = Me.btnClose
        Me.Controls.Add(Me.lblHelp)
        Me.Controls.Add(Me.picProgramLogo)
        Me.Controls.Add(Me.lblProject4)
        Me.Controls.Add(Me.lblProject3)
        Me.Controls.Add(Me.lblProject2)
        Me.Controls.Add(Me.lblProject1)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.cboShowDlg)
        Me.Controls.Add(Me.lblOpenProject)
        Me.Controls.Add(Me.lblBuildNew)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmWelcomeScreen"
        CType(Me.picProgramLogo, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub lbBuildNew_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lblBuildNew.LinkClicked
        Me.Visible = False
        Application.DoEvents()
        LoadNationalProject()
        Me.Close()
    End Sub

    Private Sub lbOpenProject_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lblOpenProject.LinkClicked
        Dim lOpenFileDialog As New OpenFileDialog

        With lOpenFileDialog
            .Filter = "MapWindow Project Files (*.mwprj)|*.mwprj"
            .CheckFileExists = True
            .InitialDirectory = lAppInfo.DefaultDir
            If .ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                lProject.Load(.FileName)
                Me.DialogResult = Windows.Forms.DialogResult.OK
                Me.Close()
            End If
        End With
    End Sub

    Private Sub cbShowDlg_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboShowDlg.CheckedChanged
        lAppInfo.ShowWelcomeScreen = cboShowDlg.Checked
    End Sub

    Private Sub lbProject_LinkClicked(ByVal sender As System.Object, _
                                      ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) _
        Handles lblProject1.LinkClicked, lblProject2.LinkClicked, lblProject3.LinkClicked, lblProject4.LinkClicked

        Dim fileName As String = CStr(CType(sender, Label).Tag)
        If lProject.Load(fileName) Then
            Logger.Dbg("Loaded Project '" & fileName & "'")
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Close()
        Else
            Logger.Msg("Could not load '" & fileName & "'", "Could Not Load Project")
        End If
    End Sub

    Private Sub frmWelcomeScreen_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Me.Icon = g_MapWin.ApplicationInfo.FormIcon
        Me.Text = "Welcome to " & g_AppNameLong

        If g_MapWin.ApplicationInfo.SplashPicture IsNot Nothing AndAlso g_MapWin.ApplicationInfo.SplashPicture.Width > 32 Then
            picProgramLogo.Width = g_MapWin.ApplicationInfo.SplashPicture.Width
            picProgramLogo.Image = g_MapWin.ApplicationInfo.SplashPicture
        End If

        cboShowDlg.Checked = lAppInfo.ShowWelcomeScreen

        lblBuildNew.Text = "Build New Project"
        lblBuildNew.LinkArea = New LinkArea(0, lblBuildNew.Text.Length)

        lblHelp.Text = "View Documentation"
        lblHelp.LinkArea = New LinkArea(0, lblHelp.Text.Length)

        lblOpenProject.Text = "Open Existing Project"
        lblOpenProject.LinkArea = New LinkArea(0, lblOpenProject.Text.Length)

        'clear recent project labels of designer text
        lblProject1.Visible = False
        lblProject2.Visible = False
        lblProject3.Visible = False
        lblProject4.Visible = False

        'check to see if there were any recent projects
        Dim lRecentCount As Integer = 0
        Dim lCurrent As Integer = 0
        Dim lProjectName As String
        Dim lProjectId As String
        Dim lblProject As Label
        While lRecentCount < 4 And lCurrent < lProject.RecentProjects.Count
            lProjectName = CType(lProject.RecentProjects(lCurrent), String)
            lProjectId = System.IO.Path.GetFileNameWithoutExtension(lProjectName)
            If FileExists(lProjectName) AndAlso LCase(lProjectId) <> "national" Then
                Select Case lRecentCount
                    Case 0 : lblProject = lblProject1
                    Case 1 : lblProject = lblProject2
                    Case 2 : lblProject = lblProject3
                    Case Else : lblProject = lblProject4
                End Select
                lblProject.Text = lProjectId
                lblProject.Tag = lProjectName
                lblProject.Visible = True
                lRecentCount += 1
            End If
            lCurrent += 1
        End While
    End Sub

    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub lbConvert_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs)
        Logger.Msg("Select the BASINS 3.x Project to convert from the PullDown Menu that appears when you click OK", MsgBoxStyle.OkOnly, "BASINS 4")
        Me.Close()
        SendKeys.Send("%FB")
    End Sub

    Private Sub lbBasinsHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblHelp.Click
        ShowHelp("")
    End Sub

End Class