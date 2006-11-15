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
Imports MapWinUtility
Imports atcUtility

Public Class frmWelcomeScreenBasins
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
    Friend WithEvents lbOpenProject As System.Windows.Forms.LinkLabel
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmWelcomeScreenBasins))
        Me.lbBuildNew = New System.Windows.Forms.LinkLabel
        Me.lbOpenProject = New System.Windows.Forms.LinkLabel
        Me.cbShowDlg = New System.Windows.Forms.CheckBox
        Me.btnClose = New System.Windows.Forms.Button
        Me.lbProject1 = New System.Windows.Forms.LinkLabel
        Me.lbProject2 = New System.Windows.Forms.LinkLabel
        Me.lbProject3 = New System.Windows.Forms.LinkLabel
        Me.pctBasinsLogo = New System.Windows.Forms.PictureBox
        Me.lbBasinsHelp = New System.Windows.Forms.LinkLabel
        Me.lbConvert = New System.Windows.Forms.LinkLabel
        Me.lbProject4 = New System.Windows.Forms.LinkLabel
        CType(Me.pctBasinsLogo, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lbBuildNew
        '
        resources.ApplyResources(Me.lbBuildNew, "lbBuildNew")
        Me.lbBuildNew.Name = "lbBuildNew"
        Me.lbBuildNew.TabStop = True
        '
        'lbOpenProject
        '
        resources.ApplyResources(Me.lbOpenProject, "lbOpenProject")
        Me.lbOpenProject.Name = "lbOpenProject"
        Me.lbOpenProject.TabStop = True
        '
        'cbShowDlg
        '
        resources.ApplyResources(Me.cbShowDlg, "cbShowDlg")
        Me.cbShowDlg.Name = "cbShowDlg"
        '
        'btnClose
        '
        Me.btnClose.BackColor = System.Drawing.SystemColors.Control
        Me.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK
        resources.ApplyResources(Me.btnClose, "btnClose")
        Me.btnClose.Name = "btnClose"
        Me.btnClose.UseVisualStyleBackColor = False
        '
        'lbProject1
        '
        resources.ApplyResources(Me.lbProject1, "lbProject1")
        Me.lbProject1.Name = "lbProject1"
        Me.lbProject1.TabStop = True
        Me.lbProject1.UseCompatibleTextRendering = True
        '
        'lbProject2
        '
        resources.ApplyResources(Me.lbProject2, "lbProject2")
        Me.lbProject2.Name = "lbProject2"
        Me.lbProject2.TabStop = True
        Me.lbProject2.UseCompatibleTextRendering = True
        '
        'lbProject3
        '
        resources.ApplyResources(Me.lbProject3, "lbProject3")
        Me.lbProject3.Name = "lbProject3"
        Me.lbProject3.TabStop = True
        Me.lbProject3.UseCompatibleTextRendering = True
        '
        'pctBasinsLogo
        '
        resources.ApplyResources(Me.pctBasinsLogo, "pctBasinsLogo")
        Me.pctBasinsLogo.Name = "pctBasinsLogo"
        Me.pctBasinsLogo.TabStop = False
        '
        'lbBasinsHelp
        '
        resources.ApplyResources(Me.lbBasinsHelp, "lbBasinsHelp")
        Me.lbBasinsHelp.Name = "lbBasinsHelp"
        Me.lbBasinsHelp.TabStop = True
        '
        'lbConvert
        '
        resources.ApplyResources(Me.lbConvert, "lbConvert")
        Me.lbConvert.Name = "lbConvert"
        Me.lbConvert.TabStop = True
        '
        'lbProject4
        '
        resources.ApplyResources(Me.lbProject4, "lbProject4")
        Me.lbProject4.Name = "lbProject4"
        Me.lbProject4.TabStop = True
        Me.lbProject4.UseCompatibleTextRendering = True
        '
        'frmWelcomeScreenBasins
        '
        resources.ApplyResources(Me, "$this")
        Me.BackColor = System.Drawing.Color.White
        Me.CancelButton = Me.btnClose
        Me.Controls.Add(Me.lbProject4)
        Me.Controls.Add(Me.lbConvert)
        Me.Controls.Add(Me.lbBasinsHelp)
        Me.Controls.Add(Me.pctBasinsLogo)
        Me.Controls.Add(Me.lbProject3)
        Me.Controls.Add(Me.lbProject2)
        Me.Controls.Add(Me.lbProject1)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.cbShowDlg)
        Me.Controls.Add(Me.lbOpenProject)
        Me.Controls.Add(Me.lbBuildNew)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmWelcomeScreenBasins"
        CType(Me.pctBasinsLogo, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub lbBuildNew_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lbBuildNew.LinkClicked
        Me.Visible = False
        Application.DoEvents()
        LoadNationalProject()
        Me.Close()
    End Sub

    Private Sub lbOpenProject_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lbOpenProject.LinkClicked
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

    Private Sub cbShowDlg_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbShowDlg.CheckedChanged
        lAppInfo.ShowWelcomeScreen = cbShowDlg.Checked
    End Sub

    Private Sub lbProject_LinkClicked(ByVal sender As System.Object, _
                                      ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) _
        Handles lbProject1.LinkClicked, lbProject2.LinkClicked, lbProject3.LinkClicked, lbProject4.LinkClicked

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

        cbShowDlg.Checked = lAppInfo.ShowWelcomeScreen

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
        While lRecentCount < 4 And lCurrent < lProject.RecentProjects.Count
            lProjectName = CType(lProject.RecentProjects(lCurrent), String)
            lProjectId = System.IO.Path.GetFileNameWithoutExtension(lProjectName)
            If FileExists(lProjectName) AndAlso LCase(lProjectId) <> "national" Then
                Select Case lRecentCount
                    Case 0 : lbProject = lbProject1
                    Case 1 : lbProject = lbProject2
                    Case 2 : lbProject = lbProject3
                    Case Else : lbProject = lbProject4
                End Select
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
        Logger.Msg("Select the BASINS Project to convert from the PullDown Menu that appears when you click OK", MsgBoxStyle.OkOnly, "BASINS 4")
        Me.Close()
        SendKeys.Send("%FB")
    End Sub

    Private Sub lbBasinsHelp_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbBasinsHelp.Click
        ShowHelp("")
    End Sub

End Class