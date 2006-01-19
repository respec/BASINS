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
    Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmWelcomeScreenBasins))
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
    Me.SuspendLayout()
    '
    'lbBuildNew
    '
    Me.lbBuildNew.AccessibleDescription = resources.GetString("lbBuildNew.AccessibleDescription")
    Me.lbBuildNew.AccessibleName = resources.GetString("lbBuildNew.AccessibleName")
    Me.lbBuildNew.Anchor = CType(resources.GetObject("lbBuildNew.Anchor"), System.Windows.Forms.AnchorStyles)
    Me.lbBuildNew.AutoSize = CType(resources.GetObject("lbBuildNew.AutoSize"), Boolean)
    Me.lbBuildNew.Dock = CType(resources.GetObject("lbBuildNew.Dock"), System.Windows.Forms.DockStyle)
    Me.lbBuildNew.Enabled = CType(resources.GetObject("lbBuildNew.Enabled"), Boolean)
    Me.lbBuildNew.Font = CType(resources.GetObject("lbBuildNew.Font"), System.Drawing.Font)
    Me.lbBuildNew.Image = CType(resources.GetObject("lbBuildNew.Image"), System.Drawing.Image)
    Me.lbBuildNew.ImageAlign = CType(resources.GetObject("lbBuildNew.ImageAlign"), System.Drawing.ContentAlignment)
    Me.lbBuildNew.ImageIndex = CType(resources.GetObject("lbBuildNew.ImageIndex"), Integer)
    Me.lbBuildNew.ImeMode = CType(resources.GetObject("lbBuildNew.ImeMode"), System.Windows.Forms.ImeMode)
    Me.lbBuildNew.LinkArea = CType(resources.GetObject("lbBuildNew.LinkArea"), System.Windows.Forms.LinkArea)
    Me.lbBuildNew.Location = CType(resources.GetObject("lbBuildNew.Location"), System.Drawing.Point)
    Me.lbBuildNew.Name = "lbBuildNew"
    Me.lbBuildNew.RightToLeft = CType(resources.GetObject("lbBuildNew.RightToLeft"), System.Windows.Forms.RightToLeft)
    Me.lbBuildNew.Size = CType(resources.GetObject("lbBuildNew.Size"), System.Drawing.Size)
    Me.lbBuildNew.TabIndex = CType(resources.GetObject("lbBuildNew.TabIndex"), Integer)
    Me.lbBuildNew.TabStop = True
    Me.lbBuildNew.Text = resources.GetString("lbBuildNew.Text")
    Me.lbBuildNew.TextAlign = CType(resources.GetObject("lbBuildNew.TextAlign"), System.Drawing.ContentAlignment)
    Me.lbBuildNew.Visible = CType(resources.GetObject("lbBuildNew.Visible"), Boolean)
    '
    'lbOpenProject
    '
    Me.lbOpenProject.AccessibleDescription = resources.GetString("lbOpenProject.AccessibleDescription")
    Me.lbOpenProject.AccessibleName = resources.GetString("lbOpenProject.AccessibleName")
    Me.lbOpenProject.Anchor = CType(resources.GetObject("lbOpenProject.Anchor"), System.Windows.Forms.AnchorStyles)
    Me.lbOpenProject.AutoSize = CType(resources.GetObject("lbOpenProject.AutoSize"), Boolean)
    Me.lbOpenProject.Dock = CType(resources.GetObject("lbOpenProject.Dock"), System.Windows.Forms.DockStyle)
    Me.lbOpenProject.Enabled = CType(resources.GetObject("lbOpenProject.Enabled"), Boolean)
    Me.lbOpenProject.Font = CType(resources.GetObject("lbOpenProject.Font"), System.Drawing.Font)
    Me.lbOpenProject.Image = CType(resources.GetObject("lbOpenProject.Image"), System.Drawing.Image)
    Me.lbOpenProject.ImageAlign = CType(resources.GetObject("lbOpenProject.ImageAlign"), System.Drawing.ContentAlignment)
    Me.lbOpenProject.ImageIndex = CType(resources.GetObject("lbOpenProject.ImageIndex"), Integer)
    Me.lbOpenProject.ImeMode = CType(resources.GetObject("lbOpenProject.ImeMode"), System.Windows.Forms.ImeMode)
    Me.lbOpenProject.LinkArea = CType(resources.GetObject("lbOpenProject.LinkArea"), System.Windows.Forms.LinkArea)
    Me.lbOpenProject.Location = CType(resources.GetObject("lbOpenProject.Location"), System.Drawing.Point)
    Me.lbOpenProject.Name = "lbOpenProject"
    Me.lbOpenProject.RightToLeft = CType(resources.GetObject("lbOpenProject.RightToLeft"), System.Windows.Forms.RightToLeft)
    Me.lbOpenProject.Size = CType(resources.GetObject("lbOpenProject.Size"), System.Drawing.Size)
    Me.lbOpenProject.TabIndex = CType(resources.GetObject("lbOpenProject.TabIndex"), Integer)
    Me.lbOpenProject.TabStop = True
    Me.lbOpenProject.Text = resources.GetString("lbOpenProject.Text")
    Me.lbOpenProject.TextAlign = CType(resources.GetObject("lbOpenProject.TextAlign"), System.Drawing.ContentAlignment)
    Me.lbOpenProject.Visible = CType(resources.GetObject("lbOpenProject.Visible"), Boolean)
    '
    'cbShowDlg
    '
    Me.cbShowDlg.AccessibleDescription = resources.GetString("cbShowDlg.AccessibleDescription")
    Me.cbShowDlg.AccessibleName = resources.GetString("cbShowDlg.AccessibleName")
    Me.cbShowDlg.Anchor = CType(resources.GetObject("cbShowDlg.Anchor"), System.Windows.Forms.AnchorStyles)
    Me.cbShowDlg.Appearance = CType(resources.GetObject("cbShowDlg.Appearance"), System.Windows.Forms.Appearance)
    Me.cbShowDlg.BackgroundImage = CType(resources.GetObject("cbShowDlg.BackgroundImage"), System.Drawing.Image)
    Me.cbShowDlg.CheckAlign = CType(resources.GetObject("cbShowDlg.CheckAlign"), System.Drawing.ContentAlignment)
    Me.cbShowDlg.Dock = CType(resources.GetObject("cbShowDlg.Dock"), System.Windows.Forms.DockStyle)
    Me.cbShowDlg.Enabled = CType(resources.GetObject("cbShowDlg.Enabled"), Boolean)
    Me.cbShowDlg.FlatStyle = CType(resources.GetObject("cbShowDlg.FlatStyle"), System.Windows.Forms.FlatStyle)
    Me.cbShowDlg.Font = CType(resources.GetObject("cbShowDlg.Font"), System.Drawing.Font)
    Me.cbShowDlg.Image = CType(resources.GetObject("cbShowDlg.Image"), System.Drawing.Image)
    Me.cbShowDlg.ImageAlign = CType(resources.GetObject("cbShowDlg.ImageAlign"), System.Drawing.ContentAlignment)
    Me.cbShowDlg.ImageIndex = CType(resources.GetObject("cbShowDlg.ImageIndex"), Integer)
    Me.cbShowDlg.ImeMode = CType(resources.GetObject("cbShowDlg.ImeMode"), System.Windows.Forms.ImeMode)
    Me.cbShowDlg.Location = CType(resources.GetObject("cbShowDlg.Location"), System.Drawing.Point)
    Me.cbShowDlg.Name = "cbShowDlg"
    Me.cbShowDlg.RightToLeft = CType(resources.GetObject("cbShowDlg.RightToLeft"), System.Windows.Forms.RightToLeft)
    Me.cbShowDlg.Size = CType(resources.GetObject("cbShowDlg.Size"), System.Drawing.Size)
    Me.cbShowDlg.TabIndex = CType(resources.GetObject("cbShowDlg.TabIndex"), Integer)
    Me.cbShowDlg.Text = resources.GetString("cbShowDlg.Text")
    Me.cbShowDlg.TextAlign = CType(resources.GetObject("cbShowDlg.TextAlign"), System.Drawing.ContentAlignment)
    Me.cbShowDlg.Visible = CType(resources.GetObject("cbShowDlg.Visible"), Boolean)
    '
    'btnClose
    '
    Me.btnClose.AccessibleDescription = resources.GetString("btnClose.AccessibleDescription")
    Me.btnClose.AccessibleName = resources.GetString("btnClose.AccessibleName")
    Me.btnClose.Anchor = CType(resources.GetObject("btnClose.Anchor"), System.Windows.Forms.AnchorStyles)
    Me.btnClose.BackColor = System.Drawing.SystemColors.Control
    Me.btnClose.BackgroundImage = CType(resources.GetObject("btnClose.BackgroundImage"), System.Drawing.Image)
    Me.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK
    Me.btnClose.Dock = CType(resources.GetObject("btnClose.Dock"), System.Windows.Forms.DockStyle)
    Me.btnClose.Enabled = CType(resources.GetObject("btnClose.Enabled"), Boolean)
    Me.btnClose.FlatStyle = CType(resources.GetObject("btnClose.FlatStyle"), System.Windows.Forms.FlatStyle)
    Me.btnClose.Font = CType(resources.GetObject("btnClose.Font"), System.Drawing.Font)
    Me.btnClose.Image = CType(resources.GetObject("btnClose.Image"), System.Drawing.Image)
    Me.btnClose.ImageAlign = CType(resources.GetObject("btnClose.ImageAlign"), System.Drawing.ContentAlignment)
    Me.btnClose.ImageIndex = CType(resources.GetObject("btnClose.ImageIndex"), Integer)
    Me.btnClose.ImeMode = CType(resources.GetObject("btnClose.ImeMode"), System.Windows.Forms.ImeMode)
    Me.btnClose.Location = CType(resources.GetObject("btnClose.Location"), System.Drawing.Point)
    Me.btnClose.Name = "btnClose"
    Me.btnClose.RightToLeft = CType(resources.GetObject("btnClose.RightToLeft"), System.Windows.Forms.RightToLeft)
    Me.btnClose.Size = CType(resources.GetObject("btnClose.Size"), System.Drawing.Size)
    Me.btnClose.TabIndex = CType(resources.GetObject("btnClose.TabIndex"), Integer)
    Me.btnClose.Text = resources.GetString("btnClose.Text")
    Me.btnClose.TextAlign = CType(resources.GetObject("btnClose.TextAlign"), System.Drawing.ContentAlignment)
    Me.btnClose.Visible = CType(resources.GetObject("btnClose.Visible"), Boolean)
    '
    'lbProject1
    '
    Me.lbProject1.AccessibleDescription = resources.GetString("lbProject1.AccessibleDescription")
    Me.lbProject1.AccessibleName = resources.GetString("lbProject1.AccessibleName")
    Me.lbProject1.Anchor = CType(resources.GetObject("lbProject1.Anchor"), System.Windows.Forms.AnchorStyles)
    Me.lbProject1.AutoSize = CType(resources.GetObject("lbProject1.AutoSize"), Boolean)
    Me.lbProject1.Dock = CType(resources.GetObject("lbProject1.Dock"), System.Windows.Forms.DockStyle)
    Me.lbProject1.Enabled = CType(resources.GetObject("lbProject1.Enabled"), Boolean)
    Me.lbProject1.Font = CType(resources.GetObject("lbProject1.Font"), System.Drawing.Font)
    Me.lbProject1.Image = CType(resources.GetObject("lbProject1.Image"), System.Drawing.Image)
    Me.lbProject1.ImageAlign = CType(resources.GetObject("lbProject1.ImageAlign"), System.Drawing.ContentAlignment)
    Me.lbProject1.ImageIndex = CType(resources.GetObject("lbProject1.ImageIndex"), Integer)
    Me.lbProject1.ImeMode = CType(resources.GetObject("lbProject1.ImeMode"), System.Windows.Forms.ImeMode)
    Me.lbProject1.LinkArea = CType(resources.GetObject("lbProject1.LinkArea"), System.Windows.Forms.LinkArea)
    Me.lbProject1.Location = CType(resources.GetObject("lbProject1.Location"), System.Drawing.Point)
    Me.lbProject1.Name = "lbProject1"
    Me.lbProject1.RightToLeft = CType(resources.GetObject("lbProject1.RightToLeft"), System.Windows.Forms.RightToLeft)
    Me.lbProject1.Size = CType(resources.GetObject("lbProject1.Size"), System.Drawing.Size)
    Me.lbProject1.TabIndex = CType(resources.GetObject("lbProject1.TabIndex"), Integer)
    Me.lbProject1.TabStop = True
    Me.lbProject1.Text = resources.GetString("lbProject1.Text")
    Me.lbProject1.TextAlign = CType(resources.GetObject("lbProject1.TextAlign"), System.Drawing.ContentAlignment)
    Me.lbProject1.Visible = CType(resources.GetObject("lbProject1.Visible"), Boolean)
    '
    'lbProject2
    '
    Me.lbProject2.AccessibleDescription = resources.GetString("lbProject2.AccessibleDescription")
    Me.lbProject2.AccessibleName = resources.GetString("lbProject2.AccessibleName")
    Me.lbProject2.Anchor = CType(resources.GetObject("lbProject2.Anchor"), System.Windows.Forms.AnchorStyles)
    Me.lbProject2.AutoSize = CType(resources.GetObject("lbProject2.AutoSize"), Boolean)
    Me.lbProject2.Dock = CType(resources.GetObject("lbProject2.Dock"), System.Windows.Forms.DockStyle)
    Me.lbProject2.Enabled = CType(resources.GetObject("lbProject2.Enabled"), Boolean)
    Me.lbProject2.Font = CType(resources.GetObject("lbProject2.Font"), System.Drawing.Font)
    Me.lbProject2.Image = CType(resources.GetObject("lbProject2.Image"), System.Drawing.Image)
    Me.lbProject2.ImageAlign = CType(resources.GetObject("lbProject2.ImageAlign"), System.Drawing.ContentAlignment)
    Me.lbProject2.ImageIndex = CType(resources.GetObject("lbProject2.ImageIndex"), Integer)
    Me.lbProject2.ImeMode = CType(resources.GetObject("lbProject2.ImeMode"), System.Windows.Forms.ImeMode)
    Me.lbProject2.LinkArea = CType(resources.GetObject("lbProject2.LinkArea"), System.Windows.Forms.LinkArea)
    Me.lbProject2.Location = CType(resources.GetObject("lbProject2.Location"), System.Drawing.Point)
    Me.lbProject2.Name = "lbProject2"
    Me.lbProject2.RightToLeft = CType(resources.GetObject("lbProject2.RightToLeft"), System.Windows.Forms.RightToLeft)
    Me.lbProject2.Size = CType(resources.GetObject("lbProject2.Size"), System.Drawing.Size)
    Me.lbProject2.TabIndex = CType(resources.GetObject("lbProject2.TabIndex"), Integer)
    Me.lbProject2.TabStop = True
    Me.lbProject2.Text = resources.GetString("lbProject2.Text")
    Me.lbProject2.TextAlign = CType(resources.GetObject("lbProject2.TextAlign"), System.Drawing.ContentAlignment)
    Me.lbProject2.Visible = CType(resources.GetObject("lbProject2.Visible"), Boolean)
    '
    'lbProject3
    '
    Me.lbProject3.AccessibleDescription = resources.GetString("lbProject3.AccessibleDescription")
    Me.lbProject3.AccessibleName = resources.GetString("lbProject3.AccessibleName")
    Me.lbProject3.Anchor = CType(resources.GetObject("lbProject3.Anchor"), System.Windows.Forms.AnchorStyles)
    Me.lbProject3.AutoSize = CType(resources.GetObject("lbProject3.AutoSize"), Boolean)
    Me.lbProject3.Dock = CType(resources.GetObject("lbProject3.Dock"), System.Windows.Forms.DockStyle)
    Me.lbProject3.Enabled = CType(resources.GetObject("lbProject3.Enabled"), Boolean)
    Me.lbProject3.Font = CType(resources.GetObject("lbProject3.Font"), System.Drawing.Font)
    Me.lbProject3.Image = CType(resources.GetObject("lbProject3.Image"), System.Drawing.Image)
    Me.lbProject3.ImageAlign = CType(resources.GetObject("lbProject3.ImageAlign"), System.Drawing.ContentAlignment)
    Me.lbProject3.ImageIndex = CType(resources.GetObject("lbProject3.ImageIndex"), Integer)
    Me.lbProject3.ImeMode = CType(resources.GetObject("lbProject3.ImeMode"), System.Windows.Forms.ImeMode)
    Me.lbProject3.LinkArea = CType(resources.GetObject("lbProject3.LinkArea"), System.Windows.Forms.LinkArea)
    Me.lbProject3.Location = CType(resources.GetObject("lbProject3.Location"), System.Drawing.Point)
    Me.lbProject3.Name = "lbProject3"
    Me.lbProject3.RightToLeft = CType(resources.GetObject("lbProject3.RightToLeft"), System.Windows.Forms.RightToLeft)
    Me.lbProject3.Size = CType(resources.GetObject("lbProject3.Size"), System.Drawing.Size)
    Me.lbProject3.TabIndex = CType(resources.GetObject("lbProject3.TabIndex"), Integer)
    Me.lbProject3.TabStop = True
    Me.lbProject3.Text = resources.GetString("lbProject3.Text")
    Me.lbProject3.TextAlign = CType(resources.GetObject("lbProject3.TextAlign"), System.Drawing.ContentAlignment)
    Me.lbProject3.Visible = CType(resources.GetObject("lbProject3.Visible"), Boolean)
    '
    'pctBasinsLogo
    '
    Me.pctBasinsLogo.AccessibleDescription = resources.GetString("pctBasinsLogo.AccessibleDescription")
    Me.pctBasinsLogo.AccessibleName = resources.GetString("pctBasinsLogo.AccessibleName")
    Me.pctBasinsLogo.Anchor = CType(resources.GetObject("pctBasinsLogo.Anchor"), System.Windows.Forms.AnchorStyles)
    Me.pctBasinsLogo.BackgroundImage = CType(resources.GetObject("pctBasinsLogo.BackgroundImage"), System.Drawing.Image)
    Me.pctBasinsLogo.Dock = CType(resources.GetObject("pctBasinsLogo.Dock"), System.Windows.Forms.DockStyle)
    Me.pctBasinsLogo.Enabled = CType(resources.GetObject("pctBasinsLogo.Enabled"), Boolean)
    Me.pctBasinsLogo.Font = CType(resources.GetObject("pctBasinsLogo.Font"), System.Drawing.Font)
    Me.pctBasinsLogo.Image = CType(resources.GetObject("pctBasinsLogo.Image"), System.Drawing.Image)
    Me.pctBasinsLogo.ImeMode = CType(resources.GetObject("pctBasinsLogo.ImeMode"), System.Windows.Forms.ImeMode)
    Me.pctBasinsLogo.Location = CType(resources.GetObject("pctBasinsLogo.Location"), System.Drawing.Point)
    Me.pctBasinsLogo.Name = "pctBasinsLogo"
    Me.pctBasinsLogo.RightToLeft = CType(resources.GetObject("pctBasinsLogo.RightToLeft"), System.Windows.Forms.RightToLeft)
    Me.pctBasinsLogo.Size = CType(resources.GetObject("pctBasinsLogo.Size"), System.Drawing.Size)
    Me.pctBasinsLogo.SizeMode = CType(resources.GetObject("pctBasinsLogo.SizeMode"), System.Windows.Forms.PictureBoxSizeMode)
    Me.pctBasinsLogo.TabIndex = CType(resources.GetObject("pctBasinsLogo.TabIndex"), Integer)
    Me.pctBasinsLogo.TabStop = False
    Me.pctBasinsLogo.Text = resources.GetString("pctBasinsLogo.Text")
    Me.pctBasinsLogo.Visible = CType(resources.GetObject("pctBasinsLogo.Visible"), Boolean)
    '
    'lbBasinsHelp
    '
    Me.lbBasinsHelp.AccessibleDescription = resources.GetString("lbBasinsHelp.AccessibleDescription")
    Me.lbBasinsHelp.AccessibleName = resources.GetString("lbBasinsHelp.AccessibleName")
    Me.lbBasinsHelp.Anchor = CType(resources.GetObject("lbBasinsHelp.Anchor"), System.Windows.Forms.AnchorStyles)
    Me.lbBasinsHelp.AutoSize = CType(resources.GetObject("lbBasinsHelp.AutoSize"), Boolean)
    Me.lbBasinsHelp.Dock = CType(resources.GetObject("lbBasinsHelp.Dock"), System.Windows.Forms.DockStyle)
    Me.lbBasinsHelp.Enabled = CType(resources.GetObject("lbBasinsHelp.Enabled"), Boolean)
    Me.lbBasinsHelp.Font = CType(resources.GetObject("lbBasinsHelp.Font"), System.Drawing.Font)
    Me.lbBasinsHelp.Image = CType(resources.GetObject("lbBasinsHelp.Image"), System.Drawing.Image)
    Me.lbBasinsHelp.ImageAlign = CType(resources.GetObject("lbBasinsHelp.ImageAlign"), System.Drawing.ContentAlignment)
    Me.lbBasinsHelp.ImageIndex = CType(resources.GetObject("lbBasinsHelp.ImageIndex"), Integer)
    Me.lbBasinsHelp.ImeMode = CType(resources.GetObject("lbBasinsHelp.ImeMode"), System.Windows.Forms.ImeMode)
    Me.lbBasinsHelp.LinkArea = CType(resources.GetObject("lbBasinsHelp.LinkArea"), System.Windows.Forms.LinkArea)
    Me.lbBasinsHelp.Location = CType(resources.GetObject("lbBasinsHelp.Location"), System.Drawing.Point)
    Me.lbBasinsHelp.Name = "lbBasinsHelp"
    Me.lbBasinsHelp.RightToLeft = CType(resources.GetObject("lbBasinsHelp.RightToLeft"), System.Windows.Forms.RightToLeft)
    Me.lbBasinsHelp.Size = CType(resources.GetObject("lbBasinsHelp.Size"), System.Drawing.Size)
    Me.lbBasinsHelp.TabIndex = CType(resources.GetObject("lbBasinsHelp.TabIndex"), Integer)
    Me.lbBasinsHelp.TabStop = True
    Me.lbBasinsHelp.Text = resources.GetString("lbBasinsHelp.Text")
    Me.lbBasinsHelp.TextAlign = CType(resources.GetObject("lbBasinsHelp.TextAlign"), System.Drawing.ContentAlignment)
    Me.lbBasinsHelp.Visible = CType(resources.GetObject("lbBasinsHelp.Visible"), Boolean)
    '
    'lbConvert
    '
    Me.lbConvert.AccessibleDescription = resources.GetString("lbConvert.AccessibleDescription")
    Me.lbConvert.AccessibleName = resources.GetString("lbConvert.AccessibleName")
    Me.lbConvert.Anchor = CType(resources.GetObject("lbConvert.Anchor"), System.Windows.Forms.AnchorStyles)
    Me.lbConvert.AutoSize = CType(resources.GetObject("lbConvert.AutoSize"), Boolean)
    Me.lbConvert.Dock = CType(resources.GetObject("lbConvert.Dock"), System.Windows.Forms.DockStyle)
    Me.lbConvert.Enabled = CType(resources.GetObject("lbConvert.Enabled"), Boolean)
    Me.lbConvert.Font = CType(resources.GetObject("lbConvert.Font"), System.Drawing.Font)
    Me.lbConvert.Image = CType(resources.GetObject("lbConvert.Image"), System.Drawing.Image)
    Me.lbConvert.ImageAlign = CType(resources.GetObject("lbConvert.ImageAlign"), System.Drawing.ContentAlignment)
    Me.lbConvert.ImageIndex = CType(resources.GetObject("lbConvert.ImageIndex"), Integer)
    Me.lbConvert.ImeMode = CType(resources.GetObject("lbConvert.ImeMode"), System.Windows.Forms.ImeMode)
    Me.lbConvert.LinkArea = CType(resources.GetObject("lbConvert.LinkArea"), System.Windows.Forms.LinkArea)
    Me.lbConvert.Location = CType(resources.GetObject("lbConvert.Location"), System.Drawing.Point)
    Me.lbConvert.Name = "lbConvert"
    Me.lbConvert.RightToLeft = CType(resources.GetObject("lbConvert.RightToLeft"), System.Windows.Forms.RightToLeft)
    Me.lbConvert.Size = CType(resources.GetObject("lbConvert.Size"), System.Drawing.Size)
    Me.lbConvert.TabIndex = CType(resources.GetObject("lbConvert.TabIndex"), Integer)
    Me.lbConvert.TabStop = True
    Me.lbConvert.Text = resources.GetString("lbConvert.Text")
    Me.lbConvert.TextAlign = CType(resources.GetObject("lbConvert.TextAlign"), System.Drawing.ContentAlignment)
    Me.lbConvert.Visible = CType(resources.GetObject("lbConvert.Visible"), Boolean)
    '
    'lbProject4
    '
    Me.lbProject4.AccessibleDescription = resources.GetString("lbProject4.AccessibleDescription")
    Me.lbProject4.AccessibleName = resources.GetString("lbProject4.AccessibleName")
    Me.lbProject4.Anchor = CType(resources.GetObject("lbProject4.Anchor"), System.Windows.Forms.AnchorStyles)
    Me.lbProject4.AutoSize = CType(resources.GetObject("lbProject4.AutoSize"), Boolean)
    Me.lbProject4.Dock = CType(resources.GetObject("lbProject4.Dock"), System.Windows.Forms.DockStyle)
    Me.lbProject4.Enabled = CType(resources.GetObject("lbProject4.Enabled"), Boolean)
    Me.lbProject4.Font = CType(resources.GetObject("lbProject4.Font"), System.Drawing.Font)
    Me.lbProject4.Image = CType(resources.GetObject("lbProject4.Image"), System.Drawing.Image)
    Me.lbProject4.ImageAlign = CType(resources.GetObject("lbProject4.ImageAlign"), System.Drawing.ContentAlignment)
    Me.lbProject4.ImageIndex = CType(resources.GetObject("lbProject4.ImageIndex"), Integer)
    Me.lbProject4.ImeMode = CType(resources.GetObject("lbProject4.ImeMode"), System.Windows.Forms.ImeMode)
    Me.lbProject4.LinkArea = CType(resources.GetObject("lbProject4.LinkArea"), System.Windows.Forms.LinkArea)
    Me.lbProject4.Location = CType(resources.GetObject("lbProject4.Location"), System.Drawing.Point)
    Me.lbProject4.Name = "lbProject4"
    Me.lbProject4.RightToLeft = CType(resources.GetObject("lbProject4.RightToLeft"), System.Windows.Forms.RightToLeft)
    Me.lbProject4.Size = CType(resources.GetObject("lbProject4.Size"), System.Drawing.Size)
    Me.lbProject4.TabIndex = CType(resources.GetObject("lbProject4.TabIndex"), Integer)
    Me.lbProject4.TabStop = True
    Me.lbProject4.Text = resources.GetString("lbProject4.Text")
    Me.lbProject4.TextAlign = CType(resources.GetObject("lbProject4.TextAlign"), System.Drawing.ContentAlignment)
    Me.lbProject4.Visible = CType(resources.GetObject("lbProject4.Visible"), Boolean)
    '
    'frmWelcomeScreenBasins
    '
    Me.AccessibleDescription = resources.GetString("$this.AccessibleDescription")
    Me.AccessibleName = resources.GetString("$this.AccessibleName")
    Me.AutoScaleBaseSize = CType(resources.GetObject("$this.AutoScaleBaseSize"), System.Drawing.Size)
    Me.AutoScroll = CType(resources.GetObject("$this.AutoScroll"), Boolean)
    Me.AutoScrollMargin = CType(resources.GetObject("$this.AutoScrollMargin"), System.Drawing.Size)
    Me.AutoScrollMinSize = CType(resources.GetObject("$this.AutoScrollMinSize"), System.Drawing.Size)
    Me.BackColor = System.Drawing.Color.White
    Me.BackgroundImage = CType(resources.GetObject("$this.BackgroundImage"), System.Drawing.Image)
    Me.CancelButton = Me.btnClose
    Me.ClientSize = CType(resources.GetObject("$this.ClientSize"), System.Drawing.Size)
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
    Me.Enabled = CType(resources.GetObject("$this.Enabled"), Boolean)
    Me.Font = CType(resources.GetObject("$this.Font"), System.Drawing.Font)
    Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
    Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
    Me.ImeMode = CType(resources.GetObject("$this.ImeMode"), System.Windows.Forms.ImeMode)
    Me.Location = CType(resources.GetObject("$this.Location"), System.Drawing.Point)
    Me.MaximizeBox = False
    Me.MaximumSize = CType(resources.GetObject("$this.MaximumSize"), System.Drawing.Size)
    Me.MinimizeBox = False
    Me.MinimumSize = CType(resources.GetObject("$this.MinimumSize"), System.Drawing.Size)
    Me.Name = "frmWelcomeScreenBasins"
    Me.RightToLeft = CType(resources.GetObject("$this.RightToLeft"), System.Windows.Forms.RightToLeft)
    Me.StartPosition = CType(resources.GetObject("$this.StartPosition"), System.Windows.Forms.FormStartPosition)
    Me.Text = resources.GetString("$this.Text")
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
    If (System.IO.File.Exists(fileName)) Then
      lProject.Load(fileName)
      Me.DialogResult = Windows.Forms.DialogResult.OK
      Me.Close()
    Else
      'TODO - 2/3/2005 - jlk - need a findFile here 
      Logger.Msg("Could not find " & fileName, "Open BASINS Project", "OK")
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
    Dim lHelpFilename As String = FindFile("Please locate BASINS 4 help file", lAppInfo.DefaultDir & "\docs\Basins4.chm")
    If FileExists(lHelpFilename) Then System.Diagnostics.Process.Start(lHelpFilename)
  End Sub
End Class