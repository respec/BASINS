'********************************************************************************************************
' FILENAME:      frmAutomatic.vb
' DESCRIPTION:  A form-linked class written in visual basic .net used to
'   allow a user to quickly set a base DEM file and find the watershed
'   delineations on it. It has further options for using an outlet point
'   shape file or a stream flow grid, as well as allowing intermediate 
'   files to be displayed in MapWindow.
' NOTES: This form is called from mwTauDemBASINSWrap.vb
'********************************************************************************************************
'The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License") 
'you may not use this file except in compliance with the License. You may obtain a copy of the License at 
'http://www.mozilla.org/MPL/ 
'Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF 
'ANY KIND, either express or implied. See the License for the specificlanguage governing rights and 
'limitations under the License. 
'
'
'Last Update:   10/18/05, ARA
' Change Log: 
' Date          Changed By      Notes
'08/28/2005     ARA             Added Headers
'10/18/05       ARA             Wrapping up and added mozilla comments
'05/26/06       ARA             Copied code from original frmAutomatic and reset change logs on all functions
'********************************************************************************************************
Imports System.Collections.Generic
Imports System.Windows.Forms
Imports MapWinUtility

Public Class frmAutomatic_v2
    Inherits System.Windows.Forms.Form
    Implements MapWinGIS.ICallback
    
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
    Friend WithEvents btnRunAll As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents ttip As System.Windows.Forms.ToolTip
    Friend WithEvents grpbxSetupPreprocess As System.Windows.Forms.GroupBox
    Friend WithEvents btnSelectMask As System.Windows.Forms.Button
    Friend WithEvents btnDrawMask As System.Windows.Forms.Button
    Friend WithEvents chkbxMask As System.Windows.Forms.CheckBox
    Friend WithEvents chkbxBurnStream As System.Windows.Forms.CheckBox
    Friend WithEvents lblSelDem As System.Windows.Forms.Label
    Friend WithEvents grpbxThresh As System.Windows.Forms.GroupBox
    Friend WithEvents cmbxThreshConvUnits As System.Windows.Forms.ComboBox
    Friend WithEvents txtbxThreshConv As System.Windows.Forms.TextBox
    Friend WithEvents txtNumCells As System.Windows.Forms.Label
    Friend WithEvents txtbxThreshold As System.Windows.Forms.TextBox
    Friend WithEvents grpbxOutletDef As System.Windows.Forms.GroupBox
    Friend WithEvents btnSelectOutlets As System.Windows.Forms.Button
    Friend WithEvents btnDrawOutlets As System.Windows.Forms.Button
    Friend WithEvents btnBrowseOutlets As System.Windows.Forms.Button
    Friend WithEvents cmbxOutlets As System.Windows.Forms.ComboBox
    Friend WithEvents chkbxUseOutlet As System.Windows.Forms.CheckBox
    Friend WithEvents btnRunPreproc As System.Windows.Forms.Button
    Friend WithEvents btnRunThreshDelin As System.Windows.Forms.Button
    Friend WithEvents btnAdvanced As System.Windows.Forms.Button
    Friend WithEvents lblMaskSelected As System.Windows.Forms.Label
    Friend WithEvents lblOutletSelected As System.Windows.Forms.Label
    Friend WithEvents btnSnapTo As System.Windows.Forms.Button
    Friend WithEvents txtbxSnapThresh As System.Windows.Forms.TextBox
    Friend WithEvents lblSnapThresh As System.Windows.Forms.Label
    Friend WithEvents lblElevUnits As System.Windows.Forms.Label
    Friend WithEvents btnHelp As System.Windows.Forms.Button
    Friend WithEvents cmbxElevUnits As System.Windows.Forms.ComboBox
    Friend WithEvents rdobtnUseFileMask As System.Windows.Forms.RadioButton
    Friend WithEvents rdobtnUseExtents As System.Windows.Forms.RadioButton
    Friend WithEvents btnSetExtents As System.Windows.Forms.Button
    Friend WithEvents btnBrowseMask As System.Windows.Forms.Button
    Friend WithEvents cmbxMask As System.Windows.Forms.ComboBox
    Friend WithEvents btnBrowseStream As System.Windows.Forms.Button
    Friend WithEvents cmbxStream As System.Windows.Forms.ComboBox
    Friend WithEvents btnBrowseDem As System.Windows.Forms.Button
    Friend WithEvents cmbxSelDem As System.Windows.Forms.ComboBox
    Friend WithEvents lblOutlets As System.Windows.Forms.Label
    Friend WithEvents lblPreproc As System.Windows.Forms.Label
    Friend WithEvents lblDelin As System.Windows.Forms.Label
    Friend WithEvents btnLoadPre As System.Windows.Forms.Button
    Friend WithEvents lblPreOut As System.Windows.Forms.Label
    Friend WithEvents btnLoadDelin As System.Windows.Forms.Button
    Friend WithEvents lblDelinOut As System.Windows.Forms.Label
    Friend WithEvents btnRunOutletFinish As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAutomatic_v2))
        Me.btnRunAll = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.ttip = New System.Windows.Forms.ToolTip(Me.components)
        Me.grpbxSetupPreprocess = New System.Windows.Forms.GroupBox
        Me.lblPreOut = New System.Windows.Forms.Label
        Me.btnLoadPre = New System.Windows.Forms.Button
        Me.lblElevUnits = New System.Windows.Forms.Label
        Me.cmbxElevUnits = New System.Windows.Forms.ComboBox
        Me.rdobtnUseFileMask = New System.Windows.Forms.RadioButton
        Me.rdobtnUseExtents = New System.Windows.Forms.RadioButton
        Me.btnSetExtents = New System.Windows.Forms.Button
        Me.btnRunPreproc = New System.Windows.Forms.Button
        Me.lblMaskSelected = New System.Windows.Forms.Label
        Me.chkbxBurnStream = New System.Windows.Forms.CheckBox
        Me.btnSelectMask = New System.Windows.Forms.Button
        Me.btnDrawMask = New System.Windows.Forms.Button
        Me.btnBrowseMask = New System.Windows.Forms.Button
        Me.cmbxMask = New System.Windows.Forms.ComboBox
        Me.chkbxMask = New System.Windows.Forms.CheckBox
        Me.btnBrowseStream = New System.Windows.Forms.Button
        Me.cmbxStream = New System.Windows.Forms.ComboBox
        Me.btnBrowseDem = New System.Windows.Forms.Button
        Me.cmbxSelDem = New System.Windows.Forms.ComboBox
        Me.lblSelDem = New System.Windows.Forms.Label
        Me.lblPreproc = New System.Windows.Forms.Label
        Me.grpbxThresh = New System.Windows.Forms.GroupBox
        Me.lblDelinOut = New System.Windows.Forms.Label
        Me.btnLoadDelin = New System.Windows.Forms.Button
        Me.btnRunThreshDelin = New System.Windows.Forms.Button
        Me.cmbxThreshConvUnits = New System.Windows.Forms.ComboBox
        Me.txtbxThreshConv = New System.Windows.Forms.TextBox
        Me.txtNumCells = New System.Windows.Forms.Label
        Me.txtbxThreshold = New System.Windows.Forms.TextBox
        Me.lblDelin = New System.Windows.Forms.Label
        Me.grpbxOutletDef = New System.Windows.Forms.GroupBox
        Me.txtbxSnapThresh = New System.Windows.Forms.TextBox
        Me.btnSnapTo = New System.Windows.Forms.Button
        Me.lblOutletSelected = New System.Windows.Forms.Label
        Me.btnRunOutletFinish = New System.Windows.Forms.Button
        Me.btnSelectOutlets = New System.Windows.Forms.Button
        Me.btnDrawOutlets = New System.Windows.Forms.Button
        Me.btnBrowseOutlets = New System.Windows.Forms.Button
        Me.cmbxOutlets = New System.Windows.Forms.ComboBox
        Me.chkbxUseOutlet = New System.Windows.Forms.CheckBox
        Me.lblSnapThresh = New System.Windows.Forms.Label
        Me.lblOutlets = New System.Windows.Forms.Label
        Me.btnAdvanced = New System.Windows.Forms.Button
        Me.btnHelp = New System.Windows.Forms.Button
        Me.grpbxSetupPreprocess.SuspendLayout()
        Me.grpbxThresh.SuspendLayout()
        Me.grpbxOutletDef.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnRunAll
        '
        Me.btnRunAll.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnRunAll.Location = New System.Drawing.Point(361, 496)
        Me.btnRunAll.Name = "btnRunAll"
        Me.btnRunAll.Size = New System.Drawing.Size(75, 23)
        Me.btnRunAll.TabIndex = 28
        Me.btnRunAll.Text = "Run All"
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(279, 496)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 27
        Me.btnCancel.Text = "Close"
        '
        'ttip
        '
        Me.ttip.AutomaticDelay = 100
        Me.ttip.AutoPopDelay = 5000
        Me.ttip.InitialDelay = 100
        Me.ttip.ReshowDelay = 20
        '
        'grpbxSetupPreprocess
        '
        Me.grpbxSetupPreprocess.Controls.Add(Me.lblPreOut)
        Me.grpbxSetupPreprocess.Controls.Add(Me.btnLoadPre)
        Me.grpbxSetupPreprocess.Controls.Add(Me.lblElevUnits)
        Me.grpbxSetupPreprocess.Controls.Add(Me.cmbxElevUnits)
        Me.grpbxSetupPreprocess.Controls.Add(Me.rdobtnUseFileMask)
        Me.grpbxSetupPreprocess.Controls.Add(Me.rdobtnUseExtents)
        Me.grpbxSetupPreprocess.Controls.Add(Me.btnSetExtents)
        Me.grpbxSetupPreprocess.Controls.Add(Me.btnRunPreproc)
        Me.grpbxSetupPreprocess.Controls.Add(Me.lblMaskSelected)
        Me.grpbxSetupPreprocess.Controls.Add(Me.chkbxBurnStream)
        Me.grpbxSetupPreprocess.Controls.Add(Me.btnSelectMask)
        Me.grpbxSetupPreprocess.Controls.Add(Me.btnDrawMask)
        Me.grpbxSetupPreprocess.Controls.Add(Me.btnBrowseMask)
        Me.grpbxSetupPreprocess.Controls.Add(Me.cmbxMask)
        Me.grpbxSetupPreprocess.Controls.Add(Me.chkbxMask)
        Me.grpbxSetupPreprocess.Controls.Add(Me.btnBrowseStream)
        Me.grpbxSetupPreprocess.Controls.Add(Me.cmbxStream)
        Me.grpbxSetupPreprocess.Controls.Add(Me.btnBrowseDem)
        Me.grpbxSetupPreprocess.Controls.Add(Me.cmbxSelDem)
        Me.grpbxSetupPreprocess.Controls.Add(Me.lblSelDem)
        Me.grpbxSetupPreprocess.Controls.Add(Me.lblPreproc)
        Me.grpbxSetupPreprocess.Location = New System.Drawing.Point(5, 3)
        Me.grpbxSetupPreprocess.Name = "grpbxSetupPreprocess"
        Me.grpbxSetupPreprocess.Size = New System.Drawing.Size(431, 272)
        Me.grpbxSetupPreprocess.TabIndex = 28
        Me.grpbxSetupPreprocess.TabStop = False
        Me.grpbxSetupPreprocess.Text = "Setup and Preprocessing"
        '
        'lblPreOut
        '
        Me.lblPreOut.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblPreOut.Location = New System.Drawing.Point(194, 237)
        Me.lblPreOut.Name = "lblPreOut"
        Me.lblPreOut.Size = New System.Drawing.Size(149, 25)
        Me.lblPreOut.TabIndex = 35
        Me.lblPreOut.Text = "Intermediate Files Loaded"
        Me.lblPreOut.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lblPreOut.Visible = False
        '
        'btnLoadPre
        '
        Me.btnLoadPre.Location = New System.Drawing.Point(14, 237)
        Me.btnLoadPre.Name = "btnLoadPre"
        Me.btnLoadPre.Size = New System.Drawing.Size(171, 26)
        Me.btnLoadPre.TabIndex = 34
        Me.btnLoadPre.Text = "Use Existing Intermediate Files"
        Me.btnLoadPre.UseVisualStyleBackColor = True
        '
        'lblElevUnits
        '
        Me.lblElevUnits.Location = New System.Drawing.Point(3, 15)
        Me.lblElevUnits.Name = "lblElevUnits"
        Me.lblElevUnits.Size = New System.Drawing.Size(84, 16)
        Me.lblElevUnits.TabIndex = 33
        Me.lblElevUnits.Text = "Elevation Units"
        '
        'cmbxElevUnits
        '
        Me.cmbxElevUnits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbxElevUnits.ItemHeight = 13
        Me.cmbxElevUnits.Items.AddRange(New Object() {"Meters", "Centimeters", "Feet"})
        Me.cmbxElevUnits.Location = New System.Drawing.Point(6, 34)
        Me.cmbxElevUnits.Name = "cmbxElevUnits"
        Me.cmbxElevUnits.Size = New System.Drawing.Size(81, 21)
        Me.cmbxElevUnits.TabIndex = 32
        '
        'rdobtnUseFileMask
        '
        Me.rdobtnUseFileMask.AutoSize = True
        Me.rdobtnUseFileMask.Location = New System.Drawing.Point(14, 158)
        Me.rdobtnUseFileMask.Name = "rdobtnUseFileMask"
        Me.rdobtnUseFileMask.Size = New System.Drawing.Size(169, 17)
        Me.rdobtnUseFileMask.TabIndex = 31
        Me.rdobtnUseFileMask.Text = "Use Grid or Shapefile for Mask"
        Me.rdobtnUseFileMask.UseVisualStyleBackColor = True
        '
        'rdobtnUseExtents
        '
        Me.rdobtnUseExtents.AutoSize = True
        Me.rdobtnUseExtents.Checked = True
        Me.rdobtnUseExtents.Location = New System.Drawing.Point(14, 135)
        Me.rdobtnUseExtents.Name = "rdobtnUseExtents"
        Me.rdobtnUseExtents.Size = New System.Drawing.Size(189, 17)
        Me.rdobtnUseExtents.TabIndex = 30
        Me.rdobtnUseExtents.TabStop = True
        Me.rdobtnUseExtents.Text = "Use Current View Extents for Mask"
        Me.rdobtnUseExtents.UseVisualStyleBackColor = True
        '
        'btnSetExtents
        '
        Me.btnSetExtents.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnSetExtents.Location = New System.Drawing.Point(347, 132)
        Me.btnSetExtents.Name = "btnSetExtents"
        Me.btnSetExtents.Size = New System.Drawing.Size(75, 23)
        Me.btnSetExtents.TabIndex = 29
        Me.btnSetExtents.Text = "Set Extents"
        Me.btnSetExtents.UseVisualStyleBackColor = True
        '
        'btnRunPreproc
        '
        Me.btnRunPreproc.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRunPreproc.Location = New System.Drawing.Point(350, 239)
        Me.btnRunPreproc.Name = "btnRunPreproc"
        Me.btnRunPreproc.Size = New System.Drawing.Size(75, 23)
        Me.btnRunPreproc.TabIndex = 12
        Me.btnRunPreproc.Text = "Run"
        Me.btnRunPreproc.UseVisualStyleBackColor = True
        '
        'lblMaskSelected
        '
        Me.lblMaskSelected.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblMaskSelected.Location = New System.Drawing.Point(191, 206)
        Me.lblMaskSelected.Name = "lblMaskSelected"
        Me.lblMaskSelected.Size = New System.Drawing.Size(152, 23)
        Me.lblMaskSelected.TabIndex = 28
        Me.lblMaskSelected.Text = "0 Selected"
        Me.lblMaskSelected.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'chkbxBurnStream
        '
        Me.chkbxBurnStream.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkbxBurnStream.Location = New System.Drawing.Point(6, 61)
        Me.chkbxBurnStream.Name = "chkbxBurnStream"
        Me.chkbxBurnStream.Size = New System.Drawing.Size(416, 24)
        Me.chkbxBurnStream.TabIndex = 3
        Me.chkbxBurnStream.Text = " Burn-in Existing Stream Polyline"
        '
        'btnSelectMask
        '
        Me.btnSelectMask.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnSelectMask.Enabled = False
        Me.btnSelectMask.Location = New System.Drawing.Point(101, 206)
        Me.btnSelectMask.Name = "btnSelectMask"
        Me.btnSelectMask.Size = New System.Drawing.Size(84, 23)
        Me.btnSelectMask.TabIndex = 10
        Me.btnSelectMask.Text = "Select Mask"
        Me.btnSelectMask.UseVisualStyleBackColor = True
        '
        'btnDrawMask
        '
        Me.btnDrawMask.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnDrawMask.Enabled = False
        Me.btnDrawMask.Location = New System.Drawing.Point(15, 206)
        Me.btnDrawMask.Name = "btnDrawMask"
        Me.btnDrawMask.Size = New System.Drawing.Size(83, 23)
        Me.btnDrawMask.TabIndex = 9
        Me.btnDrawMask.Text = "Draw Mask"
        Me.btnDrawMask.UseVisualStyleBackColor = True
        '
        'btnBrowseMask
        '
        Me.btnBrowseMask.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnBrowseMask.Image = CType(resources.GetObject("btnBrowseMask.Image"), System.Drawing.Image)
        Me.btnBrowseMask.Location = New System.Drawing.Point(398, 175)
        Me.btnBrowseMask.Name = "btnBrowseMask"
        Me.btnBrowseMask.Size = New System.Drawing.Size(24, 23)
        Me.btnBrowseMask.TabIndex = 8
        '
        'cmbxMask
        '
        Me.cmbxMask.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbxMask.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbxMask.Enabled = False
        Me.cmbxMask.Location = New System.Drawing.Point(14, 177)
        Me.cmbxMask.Name = "cmbxMask"
        Me.cmbxMask.Size = New System.Drawing.Size(378, 21)
        Me.cmbxMask.TabIndex = 7
        '
        'chkbxMask
        '
        Me.chkbxMask.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkbxMask.Location = New System.Drawing.Point(6, 111)
        Me.chkbxMask.Name = "chkbxMask"
        Me.chkbxMask.Size = New System.Drawing.Size(416, 24)
        Me.chkbxMask.TabIndex = 6
        Me.chkbxMask.Text = "Use a Focusing Mask"
        '
        'btnBrowseStream
        '
        Me.btnBrowseStream.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnBrowseStream.Image = CType(resources.GetObject("btnBrowseStream.Image"), System.Drawing.Image)
        Me.btnBrowseStream.Location = New System.Drawing.Point(398, 84)
        Me.btnBrowseStream.Name = "btnBrowseStream"
        Me.btnBrowseStream.Size = New System.Drawing.Size(24, 23)
        Me.btnBrowseStream.TabIndex = 5
        '
        'cmbxStream
        '
        Me.cmbxStream.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbxStream.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbxStream.Items.AddRange(New Object() {"test"})
        Me.cmbxStream.Location = New System.Drawing.Point(14, 86)
        Me.cmbxStream.Name = "cmbxStream"
        Me.cmbxStream.Size = New System.Drawing.Size(378, 21)
        Me.cmbxStream.TabIndex = 4
        '
        'btnBrowseDem
        '
        Me.btnBrowseDem.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnBrowseDem.Image = CType(resources.GetObject("btnBrowseDem.Image"), System.Drawing.Image)
        Me.btnBrowseDem.Location = New System.Drawing.Point(398, 32)
        Me.btnBrowseDem.Name = "btnBrowseDem"
        Me.btnBrowseDem.Size = New System.Drawing.Size(24, 23)
        Me.btnBrowseDem.TabIndex = 2
        '
        'cmbxSelDem
        '
        Me.cmbxSelDem.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbxSelDem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbxSelDem.ItemHeight = 16
        Me.cmbxSelDem.Items.AddRange(New Object() {"Select a Grid"})
        Me.cmbxSelDem.Location = New System.Drawing.Point(102, 34)
        Me.cmbxSelDem.Name = "cmbxSelDem"
        Me.cmbxSelDem.Size = New System.Drawing.Size(290, 21)
        Me.cmbxSelDem.TabIndex = 1
        '
        'lblSelDem
        '
        Me.lblSelDem.Location = New System.Drawing.Point(94, 15)
        Me.lblSelDem.Name = "lblSelDem"
        Me.lblSelDem.Size = New System.Drawing.Size(184, 16)
        Me.lblSelDem.TabIndex = 27
        Me.lblSelDem.Text = "Base Elevation Data (DEM) Layer:"
        '
        'lblPreproc
        '
        Me.lblPreproc.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPreproc.Location = New System.Drawing.Point(6, 16)
        Me.lblPreproc.Name = "lblPreproc"
        Me.lblPreproc.Size = New System.Drawing.Size(419, 253)
        Me.lblPreproc.TabIndex = 33
        Me.lblPreproc.Text = "Setup and Preprocessing Steps Currently Running"
        Me.lblPreproc.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'grpbxThresh
        '
        Me.grpbxThresh.Controls.Add(Me.lblDelinOut)
        Me.grpbxThresh.Controls.Add(Me.btnLoadDelin)
        Me.grpbxThresh.Controls.Add(Me.btnRunThreshDelin)
        Me.grpbxThresh.Controls.Add(Me.cmbxThreshConvUnits)
        Me.grpbxThresh.Controls.Add(Me.txtbxThreshConv)
        Me.grpbxThresh.Controls.Add(Me.txtNumCells)
        Me.grpbxThresh.Controls.Add(Me.txtbxThreshold)
        Me.grpbxThresh.Controls.Add(Me.lblDelin)
        Me.grpbxThresh.Location = New System.Drawing.Point(5, 281)
        Me.grpbxThresh.Name = "grpbxThresh"
        Me.grpbxThresh.Size = New System.Drawing.Size(431, 81)
        Me.grpbxThresh.TabIndex = 30
        Me.grpbxThresh.TabStop = False
        Me.grpbxThresh.Text = "Network Delineation by Threshold Method"
        '
        'lblDelinOut
        '
        Me.lblDelinOut.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblDelinOut.Location = New System.Drawing.Point(194, 48)
        Me.lblDelinOut.Name = "lblDelinOut"
        Me.lblDelinOut.Size = New System.Drawing.Size(149, 25)
        Me.lblDelinOut.TabIndex = 36
        Me.lblDelinOut.Text = "Intermediate Files Loaded"
        Me.lblDelinOut.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lblDelinOut.Visible = False
        '
        'btnLoadDelin
        '
        Me.btnLoadDelin.Location = New System.Drawing.Point(14, 47)
        Me.btnLoadDelin.Name = "btnLoadDelin"
        Me.btnLoadDelin.Size = New System.Drawing.Size(171, 26)
        Me.btnLoadDelin.TabIndex = 35
        Me.btnLoadDelin.Text = "Use Existing Intermediate Files"
        Me.btnLoadDelin.UseVisualStyleBackColor = True
        '
        'btnRunThreshDelin
        '
        Me.btnRunThreshDelin.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRunThreshDelin.Location = New System.Drawing.Point(350, 50)
        Me.btnRunThreshDelin.Name = "btnRunThreshDelin"
        Me.btnRunThreshDelin.Size = New System.Drawing.Size(75, 23)
        Me.btnRunThreshDelin.TabIndex = 17
        Me.btnRunThreshDelin.Text = "Run"
        Me.btnRunThreshDelin.UseVisualStyleBackColor = True
        '
        'cmbxThreshConvUnits
        '
        Me.cmbxThreshConvUnits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbxThreshConvUnits.ItemHeight = 13
        Me.cmbxThreshConvUnits.Items.AddRange(New Object() {"sq. mi", "acres", "hectares", "sq. km", "sq. m", "sq. ft"})
        Me.cmbxThreshConvUnits.Location = New System.Drawing.Point(353, 20)
        Me.cmbxThreshConvUnits.Name = "cmbxThreshConvUnits"
        Me.cmbxThreshConvUnits.Size = New System.Drawing.Size(72, 21)
        Me.cmbxThreshConvUnits.TabIndex = 15
        '
        'txtbxThreshConv
        '
        Me.txtbxThreshConv.Location = New System.Drawing.Point(209, 20)
        Me.txtbxThreshConv.MaxLength = 10
        Me.txtbxThreshConv.Name = "txtbxThreshConv"
        Me.txtbxThreshConv.Size = New System.Drawing.Size(138, 20)
        Me.txtbxThreshConv.TabIndex = 14
        '
        'txtNumCells
        '
        Me.txtNumCells.AutoSize = True
        Me.txtNumCells.Location = New System.Drawing.Point(152, 23)
        Me.txtNumCells.Name = "txtNumCells"
        Me.txtNumCells.Size = New System.Drawing.Size(51, 13)
        Me.txtNumCells.TabIndex = 26
        Me.txtNumCells.Text = "# of Cells"
        '
        'txtbxThreshold
        '
        Me.txtbxThreshold.Location = New System.Drawing.Point(14, 20)
        Me.txtbxThreshold.MaxLength = 10
        Me.txtbxThreshold.Name = "txtbxThreshold"
        Me.txtbxThreshold.Size = New System.Drawing.Size(127, 20)
        Me.txtbxThreshold.TabIndex = 13
        '
        'lblDelin
        '
        Me.lblDelin.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDelin.Location = New System.Drawing.Point(7, 16)
        Me.lblDelin.Name = "lblDelin"
        Me.lblDelin.Size = New System.Drawing.Size(418, 57)
        Me.lblDelin.TabIndex = 27
        Me.lblDelin.Text = "Network Delineation Steps Running"
        Me.lblDelin.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'grpbxOutletDef
        '
        Me.grpbxOutletDef.Controls.Add(Me.txtbxSnapThresh)
        Me.grpbxOutletDef.Controls.Add(Me.btnSnapTo)
        Me.grpbxOutletDef.Controls.Add(Me.lblOutletSelected)
        Me.grpbxOutletDef.Controls.Add(Me.btnRunOutletFinish)
        Me.grpbxOutletDef.Controls.Add(Me.btnSelectOutlets)
        Me.grpbxOutletDef.Controls.Add(Me.btnDrawOutlets)
        Me.grpbxOutletDef.Controls.Add(Me.btnBrowseOutlets)
        Me.grpbxOutletDef.Controls.Add(Me.cmbxOutlets)
        Me.grpbxOutletDef.Controls.Add(Me.chkbxUseOutlet)
        Me.grpbxOutletDef.Controls.Add(Me.lblSnapThresh)
        Me.grpbxOutletDef.Controls.Add(Me.lblOutlets)
        Me.grpbxOutletDef.Location = New System.Drawing.Point(5, 364)
        Me.grpbxOutletDef.Name = "grpbxOutletDef"
        Me.grpbxOutletDef.Size = New System.Drawing.Size(431, 126)
        Me.grpbxOutletDef.TabIndex = 31
        Me.grpbxOutletDef.TabStop = False
        Me.grpbxOutletDef.Text = "Custom Outlet/Inlet Definition and Delineation Completion"
        '
        'txtbxSnapThresh
        '
        Me.txtbxSnapThresh.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtbxSnapThresh.Location = New System.Drawing.Point(185, 98)
        Me.txtbxSnapThresh.Name = "txtbxSnapThresh"
        Me.txtbxSnapThresh.Size = New System.Drawing.Size(85, 20)
        Me.txtbxSnapThresh.TabIndex = 24
        Me.txtbxSnapThresh.Text = "300"
        '
        'btnSnapTo
        '
        Me.btnSnapTo.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnSnapTo.Location = New System.Drawing.Point(15, 96)
        Me.btnSnapTo.Name = "btnSnapTo"
        Me.btnSnapTo.Size = New System.Drawing.Size(84, 23)
        Me.btnSnapTo.TabIndex = 23
        Me.btnSnapTo.Text = "Snap Preview"
        Me.btnSnapTo.UseVisualStyleBackColor = True
        '
        'lblOutletSelected
        '
        Me.lblOutletSelected.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblOutletSelected.Location = New System.Drawing.Point(276, 69)
        Me.lblOutletSelected.Name = "lblOutletSelected"
        Me.lblOutletSelected.Size = New System.Drawing.Size(146, 23)
        Me.lblOutletSelected.TabIndex = 29
        Me.lblOutletSelected.Text = "0 Selected"
        Me.lblOutletSelected.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'btnRunOutletFinish
        '
        Me.btnRunOutletFinish.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRunOutletFinish.Location = New System.Drawing.Point(350, 96)
        Me.btnRunOutletFinish.Name = "btnRunOutletFinish"
        Me.btnRunOutletFinish.Size = New System.Drawing.Size(75, 23)
        Me.btnRunOutletFinish.TabIndex = 25
        Me.btnRunOutletFinish.Text = "Run"
        Me.btnRunOutletFinish.UseVisualStyleBackColor = True
        '
        'btnSelectOutlets
        '
        Me.btnSelectOutlets.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnSelectOutlets.Location = New System.Drawing.Point(145, 69)
        Me.btnSelectOutlets.Name = "btnSelectOutlets"
        Me.btnSelectOutlets.Size = New System.Drawing.Size(125, 23)
        Me.btnSelectOutlets.TabIndex = 22
        Me.btnSelectOutlets.Text = "Select Outlets/Inlets"
        Me.btnSelectOutlets.UseVisualStyleBackColor = True
        '
        'btnDrawOutlets
        '
        Me.btnDrawOutlets.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnDrawOutlets.Location = New System.Drawing.Point(14, 69)
        Me.btnDrawOutlets.Name = "btnDrawOutlets"
        Me.btnDrawOutlets.Size = New System.Drawing.Size(125, 23)
        Me.btnDrawOutlets.TabIndex = 21
        Me.btnDrawOutlets.Text = "Draw Outlets/Inlets"
        Me.btnDrawOutlets.UseVisualStyleBackColor = True
        '
        'btnBrowseOutlets
        '
        Me.btnBrowseOutlets.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnBrowseOutlets.Image = CType(resources.GetObject("btnBrowseOutlets.Image"), System.Drawing.Image)
        Me.btnBrowseOutlets.Location = New System.Drawing.Point(398, 41)
        Me.btnBrowseOutlets.Name = "btnBrowseOutlets"
        Me.btnBrowseOutlets.Size = New System.Drawing.Size(24, 23)
        Me.btnBrowseOutlets.TabIndex = 20
        '
        'cmbxOutlets
        '
        Me.cmbxOutlets.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbxOutlets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbxOutlets.Location = New System.Drawing.Point(15, 43)
        Me.cmbxOutlets.Name = "cmbxOutlets"
        Me.cmbxOutlets.Size = New System.Drawing.Size(378, 21)
        Me.cmbxOutlets.TabIndex = 19
        '
        'chkbxUseOutlet
        '
        Me.chkbxUseOutlet.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkbxUseOutlet.Location = New System.Drawing.Point(6, 19)
        Me.chkbxUseOutlet.Name = "chkbxUseOutlet"
        Me.chkbxUseOutlet.Size = New System.Drawing.Size(416, 24)
        Me.chkbxUseOutlet.TabIndex = 18
        Me.chkbxUseOutlet.Text = "Use a Custom Outlets/Inlets Layer"
        '
        'lblSnapThresh
        '
        Me.lblSnapThresh.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblSnapThresh.Location = New System.Drawing.Point(102, 94)
        Me.lblSnapThresh.Name = "lblSnapThresh"
        Me.lblSnapThresh.Size = New System.Drawing.Size(89, 26)
        Me.lblSnapThresh.TabIndex = 32
        Me.lblSnapThresh.Text = "Snap Threshold"
        Me.lblSnapThresh.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblOutlets
        '
        Me.lblOutlets.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblOutlets.Location = New System.Drawing.Point(5, 16)
        Me.lblOutlets.Name = "lblOutlets"
        Me.lblOutlets.Size = New System.Drawing.Size(420, 103)
        Me.lblOutlets.TabIndex = 0
        Me.lblOutlets.Text = "Outlets and Sub-basin Delineation Steps Currently Running"
        Me.lblOutlets.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnAdvanced
        '
        Me.btnAdvanced.Location = New System.Drawing.Point(5, 496)
        Me.btnAdvanced.Name = "btnAdvanced"
        Me.btnAdvanced.Size = New System.Drawing.Size(112, 23)
        Me.btnAdvanced.TabIndex = 26
        Me.btnAdvanced.Text = "Advanced Settings"
        '
        'btnHelp
        '
        Me.btnHelp.Location = New System.Drawing.Point(196, 496)
        Me.btnHelp.Name = "btnHelp"
        Me.btnHelp.Size = New System.Drawing.Size(75, 23)
        Me.btnHelp.TabIndex = 32
        Me.btnHelp.Text = "Help"
        Me.btnHelp.Visible = False
        '
        'frmAutomatic_v2
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(440, 526)
        Me.Controls.Add(Me.btnHelp)
        Me.Controls.Add(Me.btnAdvanced)
        Me.Controls.Add(Me.grpbxOutletDef)
        Me.Controls.Add(Me.grpbxThresh)
        Me.Controls.Add(Me.grpbxSetupPreprocess)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnRunAll)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.MaximizeBox = False
        Me.Name = "frmAutomatic_v2"
        Me.ShowIcon = False
        Me.Text = "Automatic Watershed Delineation"
        Me.grpbxSetupPreprocess.ResumeLayout(False)
        Me.grpbxSetupPreprocess.PerformLayout()
        Me.grpbxThresh.ResumeLayout(False)
        Me.grpbxThresh.PerformLayout()
        Me.grpbxOutletDef.ResumeLayout(False)
        Me.grpbxOutletDef.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

#End Region

#Region " Copied file type code "
    Private Enum LayerType
        dem = 1
        fel
        p
        sd8
        ang
        slp
        ad8
        sca
        gord
        plen
        tlen
        src
        ord
        w
        ReachShapefile
        SordFromDropan
        AtanB
        dist
        WatershedShapefile
        Outlets
        sllAccum
        fdr
        fdrn
        dsca
        cla
        q
        sord
        tri
        ann
        net
        wshed
        dep
        dg
        di
        tla
        tc
        tdep
        cs
        ctpt
        tsup
        racc
        dmax
        mask
        outletPreview
        mergeWShed
    End Enum

    Private Function GetFileDescription(ByVal Index As LayerType) As String
        'Returns the description of the raster to be loaded.  Used for the label in the legend.
        Dim Desc As String
        '    Dim LType As LayerType
        '    LType = LoadMapTypes(Index)
        Desc = ""
        Select Case Index
            Case LayerType.dem
                Desc = Caption("dem")
            Case LayerType.fel
                Desc = Caption("fel")
            Case LayerType.p
                Desc = Caption("p")
            Case LayerType.sd8
                Desc = Caption("sd8")
            Case LayerType.ang
                Desc = Caption("ang")
            Case LayerType.slp
                Desc = Caption("slp")
            Case LayerType.ad8
                Desc = Caption("ad8")
            Case LayerType.sca
                Desc = Caption("sca")
            Case LayerType.gord
                Desc = Caption("gord")
            Case LayerType.plen
                Desc = Caption("plen")
            Case LayerType.tlen
                Desc = Caption("tlen")
            Case LayerType.src
                Desc = Caption("src")
            Case LayerType.ord
                Desc = Caption("ord")
            Case LayerType.w
                Desc = Caption("w")
            Case LayerType.ReachShapefile
                Desc = Caption("net")
            Case LayerType.SordFromDropan
                Desc = Caption("SordFromDropan")
            Case LayerType.AtanB
                Desc = Caption("atanb")
            Case LayerType.dist
                Desc = Caption("dist")
            Case LayerType.WatershedShapefile
                Desc = Caption("wshed")
            Case LayerType.Outlets
                Desc = Caption("outletshpfile")
            Case LayerType.sllAccum
                Desc = Caption("sllaccum")
            Case LayerType.fdr
                Desc = Caption("fdr")
            Case LayerType.fdrn
                Desc = Caption("fdrn")
            Case LayerType.dsca
                Desc = Caption("dsca")
            Case LayerType.cla
                Desc = "CLA Grid"
            Case LayerType.q
                Desc = Caption("q")
            Case LayerType.sord
                Desc = "SORD Grid"
            Case LayerType.tri
                Desc = Caption("tri")
            Case LayerType.ann
                Desc = Caption("add")
            Case LayerType.net
                Desc = Caption("net")
            Case LayerType.wshed
                Desc = Caption("wshed")
            Case LayerType.dep
                Desc = Caption("dep")
            Case LayerType.dg
                Desc = Caption("dg")
            Case LayerType.di
                Desc = Caption("di")
            Case LayerType.tla
                Desc = Caption("tla")
            Case LayerType.tc
                Desc = Caption("tc")
            Case LayerType.tdep
                Desc = Caption("tedp")
            Case LayerType.cs
                Desc = Caption("cs")
            Case LayerType.ctpt
                Desc = Caption("ctpt")
            Case LayerType.tsup
                Desc = Caption("tsup")
            Case LayerType.racc
                Desc = Caption("racc")
            Case LayerType.dmax
                Desc = Caption("dmax")
            Case LayerType.mask
                Desc = Caption("mask")
            Case LayerType.outletPreview
                Desc = Caption("outletPreview")
            Case LayerType.mergeWShed
                Desc = Caption("mergeWShed")
        End Select
        GetFileDescription = Desc
    End Function

    'ARA 8/27/05 Copied from modIODefs.bas
    Public Function Caption(ByVal IO As String) As String
        Select Case IO
            Case "ad8"
                Caption = "D8 Contributing Area Grid (ad8)"
            Case "ang"
                Caption = "Dinf Flow direction Grid  (ang)"
            Case "atanb"
                Caption = "Wetness Index Grid (atanb)"
            Case "coord"
                Caption = "NetWork Coordinates(nnncoord.dat)"
            Case "cs"
                Caption = "Concentration in supply grid (cs)"
            Case "ctpt"
                Caption = "Concentration Grid (ctpt)"
            Case "decaymult"
                Caption = "Decay Multiplier Grid "
            Case "dem"
                Caption = "Base DEM grid"
            Case "dep"
                Caption = "Upslope Dependence Grid (dep)"
            Case "dg"
                Caption = "Disturbance Indicator Grid (dg)"
            Case "di"
                Caption = "Downslope influence Grid (di)"
            Case "dist"
                Caption = "Distance to Stream Grid (dist)"
            Case "dmax"
                Caption = "Maximum Downslope Grid (dmax)"
            Case "dsca"
                Caption = "Decayed Specific Catchment Area Grid (dsca)"
            Case "fdr"
                Caption = "Flow Path Grid (fdr)"
            Case "fdrn"
                Caption = "Verified Flow Path Grid (fdrn)"
            Case "fel"
                Caption = "Pit Filled Elevation Grid (fel)"
            Case "gord"
                Caption = "Strahler Network Order Grid (gord)"
            Case "mask"
                Caption = "Focus Mask"
            Case "net"
                Caption = "Stream Reach Shapefile (net)"
            Case "outletshpfile"
                Caption = "Outlets/Inlets ShapeFile"
            Case "ord"
                Caption = "Stream Order Grid (ord)"
            Case "p"
                Caption = "D8 Flow Direction Grid (p)"
            Case "plen"
                Caption = "Longest Upslope length Grid (plen)"
            Case "q"
                Caption = "Weighted Accumulation Grid (q)"
            Case "racc"
                Caption = "Reverse Accmulation Grid (racc)"
            Case "sca"
                Caption = "Dinf Specific Catchment Area Grid  (sca)"
            Case "sd8"
                Caption = "D8 Slope Grid (sd8)"
            Case "slp"
                Caption = "DInf Slope Grid (slp)"
            Case "src"
                Caption = "Stream Raster Grid (src)"
            Case "tc"
                Caption = "Tansport Capacity Grid (tc)"
            Case "tdep"
                Caption = "Deposition Grid"
            Case "tla"
                Caption = "Transport Limited Accumulation Grid (tla)"
            Case "tlen"
                Caption = "Total Upslope Length Grid (tlen)"
            Case "tree"
                Caption = "Network Tree(nnnntree.dat)"
            Case "tsup"
                Caption = "Supply Grid (tsup)"
            Case "w"
                Caption = "Watershed Grid (w)"
            Case "weights"
                Caption = "Weight grid "
            Case "wshed"
                Caption = "Watershed Shapefile"
            Case "mask"
                Caption = "Focus Area Mask"
            Case "outletPreview"
                Caption = "Outlets/Inlets Snap Preview"
            Case "mergeWShed"
                Caption = "Outlet Merged Watershed"
            Case Else
                Caption = IO & " Layer"
        End Select
    End Function
#End Region

#Region "Class Variables"
    Private myWrapper As atcDelinAuto
    Private frmSettings As New frmAdvancedOptions_v2

    ' debug tick timer items
    Private os As System.IO.StreamWriter
    Private tickb, ticka, tickd As Long
    Private doTicks As Boolean = False
    Private timepath As String = "C:\taudem_timing.txt"
#End Region

#Region "Helper Functions"
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: Initialize
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: A function called to class by setting the wrapper 
    '   link used for adding and removing layers from MapWindow. It also
    '   Does the initial filling, selecting, and enabling of list boxes
    '
    ' INPUTS:   wrapper      Used to set the class variable myWrapper
    '
    ' OUTPUTS:  None
    '
    ' NOTES: This has to be called before any layers are added or removed
    '
    ' Change Log: 
    ' Date          Changed By      Notes
    ' 05/26/2006    ARA             Reset change logs for new version when copying over functionality
    ' 07/03/2006    ARA             Runs form cleanup on initialize to fix any strange errors of locked state
    ' 08/09/2006    ARA             Removed edge contam tooltip as it moved to options form
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Public Sub Initialize(ByVal wrapper As atcDelinAuto)
        myWrapper = wrapper
        runningAll = False
        If doTicks Then
            os = New System.IO.StreamWriter(timepath)
        End If
        cmbxElevUnits.SelectedIndex = 0
        fillCombos() 'ARA. Used to fill combo boxes from existing layers

        LoadFromChoices()
        txtbxThreshold.Text = lastThresh
        ttip.SetToolTip(lblSelDem, "Select a projected Digital Elevation Model grid to delineate from.")
        ttip.SetToolTip(cmbxSelDem, "Select a projected Digital Elevation Model grid to delineate from.")
        ttip.SetToolTip(cmbxElevUnits, "Select the elevation units of the projected grid for correct calculations.")
        ttip.SetToolTip(lblElevUnits, "Select the elevation units of the projected grid for correct calculations.")
        ttip.SetToolTip(chkbxUseOutlet, "Use this option to limit output to only basins" + vbNewLine + "connected to outlet points and exclude basins" + vbNewLine + "connected to inlet points in a point shapefile")
        ttip.SetToolTip(cmbxOutlets, "Use this option to limit output to only basins" + vbNewLine + "connected to outlet points and exclude basins" + vbNewLine + "connected to inlet points in a point shapefile")
        ttip.SetToolTip(chkbxBurnStream, "Use this option to burn in a stream network to" + vbNewLine + "shape portions of the taudem created streams" + vbNewLine + "using a canyon burn-in that lowers elevation" + vbNewLine + "under the stream polyline.")
        ttip.SetToolTip(cmbxStream, "Use this option to burn in a stream network to" + vbNewLine + "shape portions of the taudem created streams" + vbNewLine + "using a canyon burn-in that lowers elevation" + vbNewLine + "under the stream polyline.")
        ttip.SetToolTip(chkbxMask, "Use this option to limit the areas to be delineated to" + vbNewLine + "the current zoom extents, selected polygons" + vbNewLine + "from a shapefile, or the non-nodata values of" + vbNewLine + "a masking grid.")
        ttip.SetToolTip(btnSetExtents, "Click this to zoom and set extents to use for the focusing mask.")
        ttip.SetToolTip(btnLoadPre, "Click this to load pre-existing intermediate pre-processing" + vbNewLine + "files generated from a previous run of the AWD on the base" + vbNewLine + "grid. This allows the skipping of the time-consuming" + vbNewLine + "pre-processing steps.")

        runFormCleanup()
    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: fillCombos
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: Subroutine used to fill the three comboboxes
    '   with existing layers in MapWindow which match the type 
    '   needed (grid for Base Dem and Flow grid, point shape for
    '   the outlets file). Also used after adding new layers in
    '   order to repopulate the comboboxes correctly.
    '
    ' INPUTS: None
    '
    ' OUTPUTS: None
    '
    ' NOTES: The user is capable of browsing and selecting shape 
    '   files of a non-point format, but this function will not 
    '   add them to the outlets combobox as the layertype will 
    '   not be the same.
    '
    ' Change Log: 
    ' Date          Changed By      Notes
    ' 05/26/2006    ARA             Reset change logs for new version when copying over functionality
    ' 09/05/2006    ARA             Fixed to use getgridobject instead of opening the grid object for projection check
    ' 10/02/2006    ARA             Fixed bug where layer index handles get messed up after removed layer. Have to use OCX.getlayerhandle
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Sub fillCombos()
        Dim g As MapWinGIS.Grid
        Dim projCheck As Double
        Dim strProj As String
        Dim currLayer As MapWindow.Interfaces.Layer
        Dim currType As MapWindow.Interfaces.eLayerType
        Dim ocx As AxMapWinGIS.AxMap = g_MapWin.GetOCX()

        cmbxSelDem.Items.Clear()
        cmbxSelDem.Items.Add("Select a DEM Grid")
        cmbxMask.Items.Clear()
        cmbxMask.Items.Add("Select a Mask Grid or Polygon Shapefile or Use Extents")
        cmbxStream.Items.Clear()
        cmbxStream.Items.Add("Select a Stream Polyline Shapefile")
        cmbxOutlets.Items.Clear()
        cmbxOutlets.Items.Add("Select a Point Shapefile, then Select or Draw Outlets/Inlets")

        If g_MapWin.Layers.NumLayers > 0 Then
            Dim arrLayer(g_MapWin.Layers.NumLayers - 1) As MapWindow.Interfaces.Layer

            For idx As Integer = 0 To g_MapWin.Layers.NumLayers - 1
                currLayer = g_MapWin.Layers(g_MapWin.Layers.GetHandle(idx))
                arrLayer(ocx.get_LayerPosition(currLayer.Handle)) = currLayer
            Next

            For i As Integer = 0 To arrLayer.Length - 1
                currLayer = arrLayer(i)
                currType = currLayer.LayerType

                If currType = MapWindow.Interfaces.eLayerType.Grid Then
                    g = currLayer.GetGridObject()
                    g.CellToProj(1, 1, projCheck, projCheck)
                    projCheck = System.Math.Abs(System.Math.Floor(projCheck))
                    strProj = projCheck.ToString
                    If strProj.Length > 3 Then
                        cmbxSelDem.Items.Add(currLayer.Name)
                        cmbxMask.Items.Add(currLayer.Name)
                    End If
                ElseIf currType = MapWindow.Interfaces.eLayerType.PointShapefile Then
                    cmbxOutlets.Items.Add(currLayer.Name)
                ElseIf currType = MapWindow.Interfaces.eLayerType.LineShapefile Then
                    cmbxStream.Items.Add(currLayer.Name)
                ElseIf currType = MapWindow.Interfaces.eLayerType.PolygonShapefile Then
                    cmbxMask.Items.Add(currLayer.Name)
                End If
            Next
        End If

        loadCombosToLast()
    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: SaveToChoices
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: A sub called to set the appropriate tdbchoices properties
    '   based on the checkboxes selected
    '   
    '
    ' INPUTS:   None
    '
    ' OUTPUTS:  None
    '
    ' Change Log: 
    ' Date          Changed By      Notes
    ' 05/26/2006    ARA             Reset change logs for new version when copying over functionality
    ' 08/08/2006    ARA             Moved edge cont checkbox to options form
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Sub SaveToChoices()
        Logger.Dbg("SaveToChoices:Entry")
        tdbChoiceList.useOutlets = chkbxUseOutlet.Checked
        tdbChoiceList.useMaskFileOrExtents = chkbxMask.Checked
        tdbChoiceList.useExtentMask = rdobtnUseExtents.Checked
        tdbChoiceList.useBurnIn = chkbxBurnStream.Checked
        Try
            tdbChoiceList.snapThresh = System.Convert.ToDouble(txtbxSnapThresh.Text)
        Catch e As Exception
            tdbChoiceList.snapThresh = 300.0
        End Try

    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: LoadFromChoices
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: A sub called to set the appropriate checkboxes based on the
    '   values initialized into the tdbChoiceList from the config file
    '
    ' INPUTS:   wrapper      Used to set the class variable myWrapper
    '
    ' OUTPUTS:  None
    '
    ' Change Log: 
    ' Date          Changed By      Notes
    ' 05/26/2006    ARA             Reset change logs for new version when copying over functionality
    ' 08/08/2006    ARA             Moved edge cont checkbox to options form
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Sub LoadFromChoices()
        Logger.Dbg("LoadFromChoices:Entry")
        chkbxUseOutlet.Checked = tdbChoiceList.useOutlets
        chkbxBurnStream.Checked = tdbChoiceList.useBurnIn
        chkbxMask.Checked = tdbChoiceList.useMaskFileOrExtents
        rdobtnUseExtents.Checked = tdbChoiceList.useExtentMask
        rdobtnUseFileMask.Checked = Not tdbChoiceList.useExtentMask
        txtbxSnapThresh.Text = tdbChoiceList.snapThresh.ToString("F4")
        loadCombosToLast()
    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: loadCombosToLast
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: A sub to load the combo boxes from their respective last set values
    '  So that after a fillCombos, the values won't have changed.
    '   
    '
    ' INPUTS:   None
    '
    ' OUTPUTS:  None
    '
    ' Change Log: 
    ' Date          Changed By      Notes
    ' 05/27/2006    ARA             Created
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Sub loadCombosToLast()
        Logger.Dbg("LoadCombosToLast:Entry")
        If Not lastDem = "" Then
            cmbxSelDem.SelectedIndex = cmbxSelDem.Items.IndexOf(lastDem)
            If cmbxSelDem.SelectedIndex = -1 Then
                cmbxSelDem.SelectedIndex = 0
            End If
        Else
            cmbxSelDem.SelectedIndex = 0
        End If

        If Not lastThresh = "" Then
            txtbxThreshold.Text = lastThresh
        End If
        If lastConvUnit = -1 Then
            cmbxThreshConvUnits.SelectedIndex = 0
        Else
            cmbxThreshConvUnits.SelectedIndex = lastConvUnit
        End If


        If Not lastMask = "" Then
            cmbxMask.SelectedIndex = cmbxMask.Items.IndexOf(lastMask)
            If cmbxMask.SelectedIndex = -1 Then
                cmbxMask.SelectedIndex = 0
            End If
        Else
            cmbxMask.SelectedIndex = 0
        End If

        If Not lastStream = "" Then
            cmbxStream.SelectedIndex = cmbxStream.Items.IndexOf(lastStream)
            If cmbxStream.SelectedIndex = -1 Then
                cmbxStream.SelectedIndex = 0
            End If
        Else
            cmbxStream.SelectedIndex = 0
        End If

        If Not lastOutlet = "" Then
            cmbxOutlets.SelectedIndex = cmbxOutlets.Items.IndexOf(lastOutlet)
            If cmbxOutlets.SelectedIndex = -1 Then
                cmbxOutlets.SelectedIndex = 0
            End If
        Else
            cmbxOutlets.SelectedIndex = 0
        End If

    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: layerExists
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: Will cycle the existing layers in MapWindow and
    '   return the layer filename/path of a layer with a matching name
    '
    ' INPUTS: strName : A name string to look for on the layers
    '
    ' OUTPUTS: A string containing the name of the layer with the path
    '
    ' NOTES: It is conceivable that you could have two layers with the 
    '   same path, which would make this take only the first one it hits.
    '   Could be a problem.
    '
    ' Change Log: 
    ' Date          Changed By      Notes
    ' 05/26/2006    ARA             Reset change logs for new version when copying over functionality
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Public Function getPathByName(ByVal strName As String) As String
        Dim arrLayer(g_MapWin.Layers.NumLayers - 1) As MapWindow.Interfaces.Layer
        Dim currlayer As MapWindow.Interfaces.Layer
        Dim ocx As AxMapWinGIS.AxMap = g_MapWin.GetOCX()
        For idx As Integer = 0 To g_MapWin.Layers.NumLayers - 1
            currLayer = g_MapWin.Layers(g_MapWin.Layers.GetHandle(idx))
            arrLayer(ocx.get_LayerPosition(currLayer.Handle)) = currLayer
        Next

        For i As Integer = 0 To arrLayer.Length - 1
            currLayer = arrLayer(i)
            If Not currlayer Is Nothing Then
                If currlayer.Name = strName Then
                    Return currlayer.FileName
                End If
            End If
        Next
        Return ""
    End Function

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: getNameByPath
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: Will cycle the existing layers in MapWindow and
    '   return the layer name of a layer with a matching filename path
    '
    ' INPUTS: strPath : A file path string to look for on the layers
    '
    ' OUTPUTS: A string containing the name of the layer with the path
    '
    ' NOTES: It is conceivable that you could have two layers with the 
    '   same path, which would make this take only the first one it hits.
    '   Could be a problem.
    '
    ' Change Log: 
    ' Date          Changed By      Notes
    ' 05/26/2006    ARA             Reset change logs for new version when copying over functionality
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Function getNameByPath(ByVal strPath As String) As String
        Dim arrLayer(g_MapWin.Layers.NumLayers - 1) As MapWindow.Interfaces.Layer
        Dim currlayer As MapWindow.Interfaces.Layer
        Dim ocx As AxMapWinGIS.AxMap = g_MapWin.GetOCX()
        For idx As Integer = 0 To g_MapWin.Layers.NumLayers - 1
            currlayer = g_MapWin.Layers(g_MapWin.Layers.GetHandle(idx))
            arrLayer(ocx.get_LayerPosition(currlayer.Handle)) = currlayer
        Next

        For i As Integer = 0 To arrLayer.Length - 1
            currlayer = arrLayer(i)
            If Not currlayer Is Nothing Then
                If currlayer.FileName = strPath Then
                    Return currlayer.Name
                End If
            End If
        Next
        Return ""
    End Function

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: getLayerTypeByPath
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: A function which will cycle the list of layers
    '    in MapWindow, looking for a given path, and return the
    '    type of layer it was if the path is found.
    '
    ' INPUTS: strPath : A file path string to look for on the layers
    '
    ' OUTPUTS: The type of layer if found, 
    '       MapWindow.Interfaces.eLayerType.Invalid if not
    '
    ' NOTES: None
    '
    ' Change Log: 
    ' Date          Changed By      Notes
    ' 05/26/2006    ARA             Reset change logs for new version when copying over functionality
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Function getLayerTypeByPath(ByVal strPath As String) As MapWindow.Interfaces.eLayerType
        Dim arrLayer(g_MapWin.Layers.NumLayers - 1) As MapWindow.Interfaces.Layer
        Dim currlayer As MapWindow.Interfaces.Layer
        Dim ocx As AxMapWinGIS.AxMap = g_MapWin.GetOCX()
        For idx As Integer = 0 To g_MapWin.Layers.NumLayers - 1
            currlayer = g_MapWin.Layers(g_MapWin.Layers.GetHandle(idx))
            arrLayer(ocx.get_LayerPosition(currlayer.Handle)) = currlayer
        Next

        For i As Integer = 0 To arrLayer.Length - 1
            currlayer = arrLayer(i)
            If Not currlayer Is Nothing Then
                If currlayer.FileName = strPath Then
                    Return currlayer.LayerType
                End If
            End If
        Next
        Return MapWindow.Interfaces.eLayerType.Invalid
    End Function

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: getIndextByPath
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: A function which will cycle the list of layers
    '    in MapWindow, looking for a given path, and return the
    '    index of that layer
    '
    ' INPUTS: strPath : A file path string to look for on the layers
    '
    ' OUTPUTS: The type of layer if found, 
    '       MapWindow.Interfaces.eLayerType.Invalid if not
    '
    ' NOTES: None
    '
    ' Change Log: 
    ' Date          Changed By      Notes
    ' 05/26/2006    ARA             Reset change logs for new version when copying over functionality
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Public Function getIndexByPath(ByVal strpath As String) As Integer
        Dim arrLayer(g_MapWin.Layers.NumLayers - 1) As MapWindow.Interfaces.Layer
        Dim currlayer As MapWindow.Interfaces.Layer
        Dim ocx As AxMapWinGIS.AxMap = g_MapWin.GetOCX()
        For idx As Integer = 0 To g_MapWin.Layers.NumLayers - 1
            currlayer = g_MapWin.Layers(g_MapWin.Layers.GetHandle(idx))
            arrLayer(ocx.get_LayerPosition(currlayer.Handle)) = currlayer
        Next

        For i As Integer = 0 To arrLayer.Length - 1
            currlayer = arrLayer(i)
            If Not currlayer Is Nothing Then
                If currlayer.FileName = strpath Then
                    Return currlayer.Handle
                End If
            End If
        Next
        Return -1
    End Function

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: layerExists
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: Will cycle the existing layers in MapWindow and
    '   return true if the inputed path is found as an existing
    '   layer.
    '
    ' INPUTS: strPath : A file path string to look for on the layers
    '
    ' OUTPUTS: Boolean true if path found, false otherwise.
    '
    ' NOTES: None
    '
    ' Change Log: 
    ' Date          Changed By      Notes
    ' 05/26/2006    ARA             Reset change logs for new version when copying over functionality
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Function layerExists(ByVal strPath As String) As Boolean
        Dim arrLayer(g_MapWin.Layers.NumLayers - 1) As MapWindow.Interfaces.Layer
        Dim currlayer As MapWindow.Interfaces.Layer
        Dim ocx As AxMapWinGIS.AxMap = g_MapWin.GetOCX()
        For idx As Integer = 0 To g_MapWin.Layers.NumLayers - 1
            currlayer = g_MapWin.Layers(g_MapWin.Layers.GetHandle(idx))
            arrLayer(ocx.get_LayerPosition(currlayer.Handle)) = currlayer
        Next

        For i As Integer = 0 To arrLayer.Length - 1
            currlayer = arrLayer(i)
            If Not currlayer Is Nothing Then
                If currlayer.FileName = strPath Then
                    Return True
                End If
            End If
        Next
        Return False
    End Function

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: getNumCellsByDEMAndMask
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: Will find the number of cells of a grid and if a mask is used
    '   determine the number of cells under the mask. For calculating default threshold
    '   value and limits
    '
    ' INPUTS: head : Header of the grid to find the cell count of for default grid
    '
    ' OUTPUTS: Number of cells as an area
    '
    ' NOTES: If using mask, cells are calculated by area of shapes or by the number of
    '  non-nodata value cells if a grid
    '
    ' Change Log: 
    ' Date          Changed By      Notes
    ' 07/24/2006    ARA             Added header. don't recall when created
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Function getNumCellsByDEMAndMask(ByVal head As MapWinGIS.GridHeader) As Int32
        Dim numCells, maskCells As Int32
        Dim strMask As String

        numCells = head.NumberCols * head.NumberRows

        If chkbxMask.Checked Then
            If rdobtnUseExtents.Checked Then
                maskCells = ((g_MapWin.View.Extents.xMax - g_MapWin.View.Extents.xMin) \ head.dX) * ((g_MapWin.View.Extents.yMax - g_MapWin.View.Extents.yMin) \ head.dY)
                If numCells > maskCells Then
                    numCells = maskCells
                End If
            Else
                If cmbxMask.SelectedIndex > 0 Then
                    strMask = getPathByName(cmbxMask.Items(cmbxMask.SelectedIndex))
                    If IO.Path.GetExtension(strMask) = ".shp" Then
                        If myWrapper.maskShapesIdx.Count > 0 Then
                            maskCells = 0
                            Dim sf As New MapWinGIS.Shapefile
                            sf.Open(strMask)
                            For i As Integer = 0 To myWrapper.maskShapesIdx.Count - 1
                                maskCells = maskCells + MapWinGeoProc.Utils.Area(sf.Shape(myWrapper.maskShapesIdx(i))) / (head.dX * head.dY)
                            Next
                            sf.Close()
                            If numCells > maskCells Then
                                numCells = maskCells
                            End If
                        End If
                    Else
                        Dim g As New MapWinGIS.Grid
                        Dim maskHead As MapWinGIS.GridHeader
                        g.Open(strMask)
                        maskHead = g.Header
                        maskCells = 0
                        For row As Integer = 0 To maskHead.NumberRows - 1
                            For col As Integer = 0 To maskHead.NumberCols - 1
                                If g.Value(col, row) <> maskHead.NodataValue Then
                                    maskCells = maskCells + 1
                                End If
                            Next
                        Next
                        g.Close()
                        If numCells > maskCells Then
                            numCells = maskCells
                        End If
                    End If
                End If
            End If
        End If
        Return numCells
    End Function

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: validateCellThreshAndSet
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: A function to validate the cell threshold entered, raise or lower
    '  it to the minimum or maximum if below or above, and calculate and set the
    '  equivalent converted threshold according to the selected type.
    '   
    '
    ' INPUTS:   None
    '
    ' OUTPUTS:  boolean true on success
    '
    ' Change Log: 
    ' Date          Changed By      Notes
    ' 05/27/2006    ARA             Created
    ' 07/05/2006    ARA             Added square miles
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Function validateCellThreshAndSet() As Boolean
        Logger.Dbg("ValidateCellThreshAndSet:Entry")
        Dim lValidateCellThreshAndSet As Boolean
        Dim numCells As Int32
        Dim minVal, maxVal, currVal, defaultVal As Int32
        Dim demGrid As New MapWinGIS.Grid
        Dim strGrid As String
        Dim head As MapWinGIS.GridHeader
        Dim convVal As Double

        If cmbxSelDem.SelectedIndex > 0 Then
            strGrid = getPathByName(cmbxSelDem.Items(cmbxSelDem.SelectedIndex))
            demGrid.Open(strGrid)
            head = demGrid.Header

            numCells = getNumCellsByDEMAndMask(head)
            maxVal = numCells / 2
            minVal = maxVal * 0.001
            defaultVal = maxVal * 0.02

            ttip.SetToolTip(txtbxThreshold, "Min: " + minVal.ToString() + " cells  Max: " + maxVal.ToString() + " cells")
            ttip.SetToolTip(txtNumCells, "This value is the threshold of grid cells feeding into a" + vbNewLine + "given point before being designated as a stream." + vbNewLine + "The lower the number, the more streams and " + vbNewLine + "sub-basins will be developed." + vbNewLine + "Min: " + minVal.ToString() + "  Max: " + maxVal.ToString())
            ttip.SetToolTip(grpbxThresh, "This value is the threshold of grid cells feeding into a" + vbNewLine + "given point before being designated as a stream." + vbNewLine + "The lower the number, the more streams and " + vbNewLine + "sub-basins will be developed." + vbNewLine + "Min: " + minVal.ToString() + "  Max: " + maxVal.ToString())

            If txtbxThreshold.Text <> "" Then
                Try
                    currVal = System.Convert.ToInt32(txtbxThreshold.Text)
                Catch e As Exception
                    currVal = defaultVal
                End Try

                If currVal > maxVal Then
                    currVal = maxVal
                End If
                If currVal < minVal Then
                    currVal = minVal
                End If
                txtbxThreshold.Text = currVal
                tdbChoiceList.Threshold = currVal
            Else
                If lastThresh = "" Then
                    txtbxThreshold.Text = defaultVal
                    tdbChoiceList.Threshold = defaultVal
                Else
                    Try
                        tdbChoiceList.Threshold = lastThresh
                        txtbxThreshold.Text = lastThresh
                    Catch e As Exception
                        tdbChoiceList.Threshold = defaultVal
                        txtbxThreshold.Text = defaultVal
                    End Try
                End If
            End If

            convVal = head.dX * head.dY * System.Convert.ToInt32(txtbxThreshold.Text)
            If head.Projection.ToUpper.Contains("UNITS=M") Then
                If cmbxThreshConvUnits.SelectedIndex = 0 Then
                    txtbxThreshConv.Text = (convVal / 2589988.110336).ToString("F4")
                ElseIf cmbxThreshConvUnits.SelectedIndex = 1 Then
                    txtbxThreshConv.Text = (convVal / 4046.8564224).ToString("F4")
                ElseIf cmbxThreshConvUnits.SelectedIndex = 2 Then
                    txtbxThreshConv.Text = (convVal / 10000).ToString("F4")
                ElseIf cmbxThreshConvUnits.SelectedIndex = 3 Then
                    txtbxThreshConv.Text = (convVal / 1000000).ToString("F4")
                ElseIf cmbxThreshConvUnits.SelectedIndex = 4 Then
                    txtbxThreshConv.Text = convVal.ToString("F4")
                ElseIf cmbxThreshConvUnits.SelectedIndex = 5 Then
                    txtbxThreshConv.Text = (convVal * 10.763910417).ToString("F4")
                End If
            ElseIf head.Projection.ToUpper.Contains("UNITS=FT") Then
                If cmbxThreshConvUnits.SelectedIndex = 0 Then
                    txtbxThreshConv.Text = (convVal / 27878400).ToString("F4")
                ElseIf cmbxThreshConvUnits.SelectedIndex = 1 Then
                    txtbxThreshConv.Text = (convVal / 43560).ToString("F4")
                ElseIf cmbxThreshConvUnits.SelectedIndex = 2 Then
                    txtbxThreshConv.Text = (convVal / 107639.104167097).ToString("F4")
                ElseIf cmbxThreshConvUnits.SelectedIndex = 3 Then
                    txtbxThreshConv.Text = (convVal / 10763910.416709721).ToString("F4")
                ElseIf cmbxThreshConvUnits.SelectedIndex = 4 Then
                    txtbxThreshConv.Text = (convVal * 0.09290304).ToString("F4")
                ElseIf cmbxThreshConvUnits.SelectedIndex = 5 Then
                    txtbxThreshConv.Text = convVal.ToString("F4")
                End If
            End If
            If txtbxThreshold.Text <> lastThresh Then
                threshDelinHasRan = False
            End If
            lastThresh = txtbxThreshold.Text
            lValidateCellThreshAndSet = True
        Else
            txtbxThreshold.Text = ""
            tdbChoiceList.Threshold = 0
            lValidateCellThreshAndSet = False
        End If
        demGrid.Close()
        Logger.Dbg("ValidateCellThreshAndSet:Exit:" & validateCellThreshAndSet)
        Return lValidateCellThreshAndSet
    End Function

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: validateConvThreshAndSet
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: A function to validate the converted threshold entered, raise or lower
    '  it to the minimum or maximum if below or above, and calculate and set the
    '  equivalent threshold in cells according to the selected type.
    '   
    '
    ' INPUTS:   None
    '
    ' OUTPUTS:  boolean true on success
    '
    ' Change Log: 
    ' Date          Changed By      Notes
    ' 05/27/2006    ARA             Created
    ' 07/05/2006    ARA             Added square miles
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Function validateConvThreshAndSet() As Boolean
        Logger.Dbg("ValidateConvThreshAndSet:Entry")
        Dim lValidateConvThreshAndSet As Boolean
        Dim numCells As Int32
        Dim minVal, maxVal, currVal, defaultVal As Double
        Dim demGrid As New MapWinGIS.Grid
        Dim strGrid As String
        Dim head As MapWinGIS.GridHeader
        Dim convVal As Double

        If cmbxSelDem.SelectedIndex > 0 Then
            strGrid = getPathByName(cmbxSelDem.Items(cmbxSelDem.SelectedIndex))
            demGrid.Open(strGrid)
            head = demGrid.Header

            numCells = getNumCellsByDEMAndMask(head)

            convVal = head.dX * head.dY * numCells
            If head.Projection.ToUpper.Contains("UNITS=M") Then
                If cmbxThreshConvUnits.SelectedIndex = 0 Then 'acres
                    maxVal = (convVal / 2) / 2589988.110336
                    minVal = maxVal * 0.001
                    defaultVal = maxVal * 0.02
                    ttip.SetToolTip(txtbxThreshConv, "Min: " + minVal.ToString("F4") + " sq. mi  Max: " + maxVal.ToString("F4") + " sq. m")
                    ttip.SetToolTip(cmbxThreshConvUnits, "This value is the threshold in square miles flowing into a" + vbNewLine + "given point before being designated as a stream." + vbNewLine + "The lower the number, the more streams and " + vbNewLine + "sub-basins will be developed." + vbNewLine + "Min: " + minVal.ToString("F4") + "  Max: " + maxVal.ToString("F4"))
                ElseIf cmbxThreshConvUnits.SelectedIndex = 1 Then 'sq km
                    maxVal = (convVal / 2) / 4046.8564224
                    minVal = maxVal * 0.001
                    defaultVal = maxVal * 0.02
                    ttip.SetToolTip(txtbxThreshConv, "Min: " + minVal.ToString("F4") + " acres  Max: " + maxVal.ToString("F4") + " acres")
                    ttip.SetToolTip(cmbxThreshConvUnits, "This value is the threshold in acres flowing into a" + vbNewLine + "given point before being designated as a stream." + vbNewLine + "The lower the number, the more streams and " + vbNewLine + "sub-basins will be developed." + vbNewLine + "Min: " + minVal.ToString("F4") + "  Max: " + maxVal.ToString("F4"))
                ElseIf cmbxThreshConvUnits.SelectedIndex = 2 Then 'hectare
                    maxVal = (convVal / 2) / 10000
                    minVal = maxVal * 0.001
                    defaultVal = maxVal * 0.02
                    ttip.SetToolTip(txtbxThreshConv, "Min: " + minVal.ToString("F4") + " hectares  Max: " + maxVal.ToString("F4") + " hectares")
                    ttip.SetToolTip(cmbxThreshConvUnits, "This value is the threshold in hectares flowing into a" + vbNewLine + "given point before being designated as a stream." + vbNewLine + "The lower the number, the more streams and " + vbNewLine + "sub-basins will be developed." + vbNewLine + "Min: " + minVal.ToString("F4") + "  Max: " + maxVal.ToString("F4"))
                ElseIf cmbxThreshConvUnits.SelectedIndex = 3 Then 'sq m
                    maxVal = (convVal / 2) / 1000000
                    minVal = maxVal * 0.001
                    defaultVal = maxVal * 0.02
                    ttip.SetToolTip(txtbxThreshConv, "Min: " + minVal.ToString("F4") + " sq. km  Max: " + maxVal.ToString("F4") + " sq. km")
                    ttip.SetToolTip(cmbxThreshConvUnits, "This value is the threshold in square kilometers flowing into a" + vbNewLine + "given point before being designated as a stream." + vbNewLine + "The lower the number, the more streams and " + vbNewLine + "sub-basins will be developed." + vbNewLine + "Min: " + minVal.ToString("F4") + "  Max: " + maxVal.ToString("F4"))
                ElseIf cmbxThreshConvUnits.SelectedIndex = 4 Then 'sq ft
                    maxVal = convVal / 2
                    minVal = maxVal * 0.001
                    defaultVal = maxVal * 0.02
                    ttip.SetToolTip(txtbxThreshConv, "Min: " + minVal.ToString("F4") + " sq. m  Max: " + maxVal.ToString("F4") + " sq. m")
                    ttip.SetToolTip(cmbxThreshConvUnits, "This value is the threshold in square meters flowing into a" + vbNewLine + "given point before being designated as a stream." + vbNewLine + "The lower the number, the more streams and " + vbNewLine + "sub-basins will be developed." + vbNewLine + "Min: " + minVal.ToString("F4") + "  Max: " + maxVal.ToString("F4"))
                ElseIf cmbxThreshConvUnits.SelectedIndex = 5 Then 'sq mi
                    maxVal = (convVal / 2) * 10.763910417
                    minVal = maxVal * 0.001
                    defaultVal = maxVal * 0.02
                    ttip.SetToolTip(txtbxThreshConv, "Min: " + minVal.ToString("F4") + " sq. ft  Max: " + maxVal.ToString("F4") + " sq. m")
                    ttip.SetToolTip(cmbxThreshConvUnits, "This value is the threshold in square feet flowing into a" + vbNewLine + "given point before being designated as a stream." + vbNewLine + "The lower the number, the more streams and " + vbNewLine + "sub-basins will be developed." + vbNewLine + "Min: " + minVal.ToString("F4") + "  Max: " + maxVal.ToString("F4"))
                End If
            ElseIf head.Projection.ToUpper.Contains("UNITS=FT") Then
                If cmbxThreshConvUnits.SelectedIndex = 0 Then 'acres
                    maxVal = (convVal / 2) / 27878400
                    minVal = maxVal * 0.001
                    defaultVal = maxVal * 0.02
                    ttip.SetToolTip(txtbxThreshConv, "Min: " + minVal.ToString("F4") + " sq. mi  Max: " + maxVal.ToString("F4") + " sq. m")
                    ttip.SetToolTip(cmbxThreshConvUnits, "This value is the threshold in square miles flowing into a" + vbNewLine + "given point before being designated as a stream." + vbNewLine + "The lower the number, the more streams and " + vbNewLine + "sub-basins will be developed." + vbNewLine + "Min: " + minVal.ToString("F4") + "  Max: " + maxVal.ToString("F4"))
                ElseIf cmbxThreshConvUnits.SelectedIndex = 1 Then 'sq km
                    maxVal = (convVal / 2) / 43560
                    minVal = maxVal * 0.001
                    defaultVal = maxVal * 0.02
                    ttip.SetToolTip(txtbxThreshConv, "Min: " + minVal.ToString("F4") + " acres  Max: " + maxVal.ToString("F4") + " acres")
                    ttip.SetToolTip(cmbxThreshConvUnits, "This value is the threshold in acres flowing into a" + vbNewLine + "given point before being designated as a stream." + vbNewLine + "The lower the number, the more streams and " + vbNewLine + "sub-basins will be developed." + vbNewLine + "Min: " + minVal.ToString("F4") + "  Max: " + maxVal.ToString("F4"))
                ElseIf cmbxThreshConvUnits.SelectedIndex = 2 Then 'hectare
                    maxVal = (convVal / 2) / 107639.1041670972
                    minVal = maxVal * 0.001
                    defaultVal = maxVal * 0.02
                    ttip.SetToolTip(txtbxThreshConv, "Min: " + minVal.ToString("F4") + " hectares  Max: " + maxVal.ToString("F4") + " hectares")
                    ttip.SetToolTip(cmbxThreshConvUnits, "This value is the threshold in hectares flowing into a" + vbNewLine + "given point before being designated as a stream." + vbNewLine + "The lower the number, the more streams and " + vbNewLine + "sub-basins will be developed." + vbNewLine + "Min: " + minVal.ToString("F4") + "  Max: " + maxVal.ToString("F4"))
                ElseIf cmbxThreshConvUnits.SelectedIndex = 3 Then 'sq m
                    maxVal = (convVal / 2) / 10763910.416709721
                    minVal = maxVal * 0.001
                    defaultVal = maxVal * 0.02
                    ttip.SetToolTip(txtbxThreshConv, "Min: " + minVal.ToString("F4") + " sq. km  Max: " + maxVal.ToString("F4") + " sq. km")
                    ttip.SetToolTip(cmbxThreshConvUnits, "This value is the threshold in square kilometers flowing into a" + vbNewLine + "given point before being designated as a stream." + vbNewLine + "The lower the number, the more streams and " + vbNewLine + "sub-basins will be developed." + vbNewLine + "Min: " + minVal.ToString("F4") + "  Max: " + maxVal.ToString("F4"))
                ElseIf cmbxThreshConvUnits.SelectedIndex = 4 Then 'sq ft
                    maxVal = (convVal / 2) * 0.09290304
                    minVal = maxVal * 0.001
                    defaultVal = maxVal * 0.02
                    ttip.SetToolTip(txtbxThreshConv, "Min: " + minVal.ToString("F4") + " sq. m  Max: " + maxVal.ToString("F4") + " sq. m")
                    ttip.SetToolTip(cmbxThreshConvUnits, "This value is the threshold in square meters flowing into a" + vbNewLine + "given point before being designated as a stream." + vbNewLine + "The lower the number, the more streams and " + vbNewLine + "sub-basins will be developed." + vbNewLine + "Min: " + minVal.ToString("F4") + "  Max: " + maxVal.ToString("F4"))
                ElseIf cmbxThreshConvUnits.SelectedIndex = 5 Then 'sq ft
                    maxVal = convVal / 2
                    minVal = maxVal * 0.001
                    defaultVal = maxVal * 0.02
                    ttip.SetToolTip(txtbxThreshConv, "Min: " + minVal.ToString("F4") + " sq. ft  Max: " + maxVal.ToString("F4") + " sq. m")
                    ttip.SetToolTip(cmbxThreshConvUnits, "This value is the threshold in square feet flowing into a" + vbNewLine + "given point before being designated as a stream." + vbNewLine + "The lower the number, the more streams and " + vbNewLine + "sub-basins will be developed." + vbNewLine + "Min: " + minVal.ToString("F4") + "  Max: " + maxVal.ToString("F4"))
                End If
            End If

            If txtbxThreshConv.Text <> "" Then
                Try
                    currVal = System.Convert.ToDouble(txtbxThreshConv.Text)
                Catch e As Exception
                    currVal = defaultVal
                End Try

                If currVal > maxVal Then
                    currVal = maxVal
                End If
                If currVal < minVal Then
                    currVal = minVal
                End If
                txtbxThreshConv.Text = currVal

                If head.Projection.ToUpper.Contains("UNITS=M") Then
                    If cmbxThreshConvUnits.SelectedIndex = 0 Then
                        txtbxThreshold.Text = System.Convert.ToString(System.Convert.ToInt32((currVal * 2589988.110336) / (head.dX * head.dY)))
                    ElseIf cmbxThreshConvUnits.SelectedIndex = 1 Then
                        txtbxThreshold.Text = System.Convert.ToString(System.Convert.ToInt32((currVal * 4046.8564224) / (head.dX * head.dY)))
                    ElseIf cmbxThreshConvUnits.SelectedIndex = 2 Then
                        txtbxThreshold.Text = System.Convert.ToString(System.Convert.ToInt32((currVal * 10000) / (head.dX * head.dY)))
                    ElseIf cmbxThreshConvUnits.SelectedIndex = 3 Then
                        txtbxThreshold.Text = System.Convert.ToString(System.Convert.ToInt32((currVal * 1000000) / (head.dX * head.dY)))
                    ElseIf cmbxThreshConvUnits.SelectedIndex = 4 Then
                        txtbxThreshold.Text = System.Convert.ToString(System.Convert.ToInt32(currVal / (head.dX * head.dY)))
                    ElseIf cmbxThreshConvUnits.SelectedIndex = 5 Then
                        txtbxThreshold.Text = System.Convert.ToString(System.Convert.ToInt32((currVal / 10.763910417) / (head.dX * head.dY)))
                    End If
                ElseIf head.Projection.ToUpper.Contains("UNITS=FT") Then
                    If cmbxThreshConvUnits.SelectedIndex = 0 Then
                        txtbxThreshold.Text = System.Convert.ToString(System.Convert.ToInt32((currVal * 27878400) / (head.dX * head.dY)))
                    ElseIf cmbxThreshConvUnits.SelectedIndex = 1 Then
                        txtbxThreshold.Text = System.Convert.ToString(System.Convert.ToInt32((currVal * 43560) / (head.dX * head.dY)))
                    ElseIf cmbxThreshConvUnits.SelectedIndex = 2 Then
                        txtbxThreshold.Text = System.Convert.ToString(System.Convert.ToInt32((currVal * 107639.1041670972) / (head.dX * head.dY)))
                    ElseIf cmbxThreshConvUnits.SelectedIndex = 3 Then
                        txtbxThreshold.Text = System.Convert.ToString(System.Convert.ToInt32((currVal * 10763910.416709721) / (head.dX * head.dY)))
                    ElseIf cmbxThreshConvUnits.SelectedIndex = 4 Then
                        txtbxThreshold.Text = System.Convert.ToString(System.Convert.ToInt32((currVal / 0.09290304) / (head.dX * head.dY)))
                    ElseIf cmbxThreshConvUnits.SelectedIndex = 5 Then
                        txtbxThreshold.Text = System.Convert.ToString(System.Convert.ToInt32(currVal / (head.dX * head.dY)))
                    End If
                End If
            Else
                validateCellThreshAndSet()
            End If

            lValidateConvThreshAndSet = True
        Else
            txtbxThreshConv.Text = ""
            lValidateConvThreshAndSet = False
        End If
        demGrid.Close()
        Logger.Dbg("ValidateConvThreshAndSet:Exit:" & lValidateConvThreshAndSet)
    End Function

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: setMaskSelected
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: A public sub called from the interface layer when finished
    '  selecting or drawing Masks, which is used to populate the selected mask
    '  list and highlight the masks which were selected.
    '
    ' INPUTS:   None
    '
    ' OUTPUTS:  None
    '
    ' Change Log: 
    ' Date          Changed By      Notes
    ' 05/27/2006    ARA             Created
    ' 07/03/2006    ARA             Modified to select all on drawing
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Public Sub setMaskSelected()
        Logger.Dbg("SetMaskSelected:Entry")
        If g_DrawingMask Then
            Logger.Dbg("  UsingDrawingMask")
            Dim sf As New MapWinGIS.Shapefile
            sf.Open(currDrawPath)
            g_MapWin.View.SelectedShapes.ClearSelectedShapes()
            For i As Integer = 0 To sf.NumShapes - 1
                g_MapWin.View.SelectedShapes.AddByIndex(i, Drawing.Color.Yellow)
            Next
            sf.Close()
        End If

        If g_SelectingMask Or g_DrawingMask Then
            myWrapper.maskShapesIdx.Clear()
            For i As Integer = 0 To g_MapWin.View.SelectedShapes.NumSelected - 1
                myWrapper.maskShapesIdx.Add(g_MapWin.View.SelectedShapes(i).ShapeIndex)
            Next
        End If

        If g_SelectingMask Or g_DrawingMask Then
            g_MapWin.View.SelectedShapes.ClearSelectedShapes()
            For i As Integer = 0 To myWrapper.maskShapesIdx.Count - 1
                g_MapWin.View.SelectedShapes.AddByIndex(myWrapper.maskShapesIdx.Item(i), Drawing.Color.Yellow)
            Next
        End If
        lblMaskSelected.Text = myWrapper.maskShapesIdx.Count.ToString + " selected"
        g_MapWin.View.CursorMode = MapWinGIS.tkCursorMode.cmNone
        If cmbxSelDem.SelectedIndex > 0 Then
            'txtbxThreshold.Text = ""
            'lastThresh = ""
            validateCellThreshAndSet()
            validateConvThreshAndSet()
        End If
        Logger.Dbg("SetMaskSelected:Exit")
    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: setOutletsSelected
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: A public sub called from the interface layer when finished
    '  selecting or drawing outlets, which is used to populate the selected outlets
    '  list and highlight the outlets which were selected.
    '
    ' INPUTS:   None
    '
    ' OUTPUTS:  None
    '
    ' Change Log: 
    ' Date          Changed By      Notes
    ' 05/27/2006    ARA             Created
    ' 07/03/2006    ARA             Modified to select all on drawing
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Public Sub setOutletsSelected()
        Logger.Dbg("SetOutletsSelected:Entry")
        Dim i As Integer
        Dim sf As New MapWinGIS.Shapefile
        If g_DrawingOutletsOrInlets Then
            sf.Open(currDrawPath)
            g_MapWin.View.SelectedShapes.ClearSelectedShapes()
            For i = 0 To sf.NumShapes - 1
                g_MapWin.View.SelectedShapes.AddByIndex(i, Drawing.Color.Yellow)
            Next
            sf.Close()
        End If

        If g_SelectingOutlets Or g_DrawingOutletsOrInlets Then
            myWrapper.outletShapesIdx.Clear()
            For i = 0 To g_MapWin.View.SelectedShapes.NumSelected - 1
                myWrapper.outletShapesIdx.Add(g_MapWin.View.SelectedShapes(i).ShapeIndex)
            Next
        End If

        If g_SelectingOutlets Or g_DrawingOutletsOrInlets Then
            sf.Open(currDrawPath)

            Dim inletfieldnum As Integer = -1
            For z As Integer = 0 To sf.NumFields - 1
                If sf.Field(z).Name = "INLET" Then
                    inletfieldnum = z
                    Exit For
                End If
            Next

            g_MapWin.View.SelectedShapes.ClearSelectedShapes()
            For i = 0 To myWrapper.outletShapesIdx.Count - 1
                If sf.CellValue(inletfieldnum, myWrapper.outletShapesIdx(i)) = 0 Then
                    g_MapWin.View.SelectedShapes.AddByIndex(myWrapper.outletShapesIdx.Item(i), Drawing.Color.Yellow)
                Else
                    g_MapWin.View.SelectedShapes.AddByIndex(myWrapper.outletShapesIdx.Item(i), Drawing.Color.Goldenrod)
                End If
            Next
            sf.Close()
        End If

        lblOutletSelected.Text = myWrapper.outletShapesIdx.Count.ToString + " selected"
        g_MapWin.View.CursorMode = MapWinGIS.tkCursorMode.cmNone
        Logger.Dbg("SetOutletsSelected:Exit")
    End Sub

    Private Function AddMap(ByVal fname As String, ByVal gtype As LayerType) As Boolean
        Dim Message As String
        '3/16/2005 - dpa - Updated add map functionality for MapWindow 3.1 plugin wrapper.
        If (tdbFileList.getext(fname) = "") Then
            Message = fname & "\sta.adf"
        Else
            Message = fname
        End If
        Message &= "|" & GetFileDescription(gtype) & "|" & CLng(gtype)
        Logger.Dbg(Message)
        'myWrapper.Progress("Add", 0, Message)
    End Function

    Private Function RemoveLayer(ByVal fname As String, Optional ByVal toPrompt As Boolean = True) As Boolean
        Logger.Dbg("RemoveLayer:" & fname)
        Dim lIndex As Integer = getIndexByPath(fname)
        If lIndex >= 0 Then
            g_MapWin.Layers.Remove(lIndex)
            Logger.Dbg("  Removed")
        Else
            Logger.Dbg("  NotFound")
        End If
        RemoveLayer = True
    End Function

    Private Sub closingCleanup()
        Logger.Dbg("ClosingCleanup:Entry")
        If cmbxSelDem.SelectedIndex > 0 Then
            lastDem = cmbxSelDem.Items.Item(cmbxSelDem.SelectedIndex)
        Else
            lastDem = ""
        End If
        If cmbxMask.SelectedIndex > 0 Then
            lastMask = cmbxMask.Items(cmbxMask.SelectedIndex)
        Else
            lastMask = ""
        End If
        If cmbxStream.SelectedIndex > 0 Then
            lastStream = cmbxStream.Items.Item(cmbxStream.SelectedIndex)
        Else
            lastStream = ""
        End If
        If cmbxOutlets.SelectedIndex > 0 Then
            lastOutlet = cmbxOutlets.Items.Item(cmbxOutlets.SelectedIndex)
        Else
            lastOutlet = ""
        End If
        lastThresh = txtbxThreshold.Text
        SaveToChoices()
        tdbChoiceList.SaveConfig()
        Logger.Dbg("ClosingCleanup:Exit")
    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: runFormInit
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: A sub to initialize the AWD v2 form and the tdbChoicelist
    '
    ' INPUTS: None
    '
    ' OUTPUTS: None
    '
    ' NOTES: None
    '
    ' Change Log: 
    ' Date          Changed By      Notes
    ' 05/30/2006    ARA             Created
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Sub runFormInit()
        Logger.Dbg("RunFormInit:Entry")
        Cursor = Windows.Forms.Cursors.WaitCursor
        btnRunAll.Enabled = False
        btnCancel.Enabled = False
        btnAdvanced.Enabled = False
        btnHelp.Enabled = False
        grpbxSetupPreprocess.Enabled = False
        grpbxThresh.Enabled = False
        grpbxOutletDef.Enabled = False
        SaveToChoices()
        tdbChoiceList.SaveConfig()
        Me.Refresh()
        Logger.Dbg("RunFormInit:Exit")
    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: runFormCleanup
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: A sub to clean up the AWD v2 form
    '
    ' INPUTS: None
    '
    ' OUTPUTS: None
    '
    ' NOTES: None
    '
    ' Change Log: 
    ' Date          Changed By      Notes
    ' 05/30/2006    ARA             Created
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Sub runFormCleanup()
        'myWrapper.Progress("Status", 0, "")
        Logger.Dbg("RunFormCleanup:Entry")
        Cursor = Windows.Forms.Cursors.Default
        btnRunAll.Enabled = True
        btnCancel.Enabled = True
        btnAdvanced.Enabled = True
        btnHelp.Enabled = True
        lblPreproc.Visible = False
        lblDelin.Visible = False
        lblOutlets.Visible = False
        grpbxSetupPreprocess.Enabled = True
        grpbxThresh.Enabled = True
        grpbxOutletDef.Enabled = True
        Me.Refresh()
        Logger.Dbg("RunFormCleanup:Exit")
    End Sub

    Public Sub [Error](ByVal KeyOfSender As String, ByVal ErrorMsg As String) Implements MapWinGIS.ICallback.Error
        Logger.Msg(ErrorMsg, MsgBoxStyle.OkOnly, "Automatic Watershed Delineation Error")
        runFormCleanup()
    End Sub

    Public Sub Progress(ByVal KeyOfSender As String, ByVal Percent As Integer, ByVal Message As String) Implements MapWinGIS.ICallback.Progress
        g_MapWin.StatusBar.ProgressBarValue = Percent
        g_StatusBar.Text = Message
        Application.DoEvents()
    End Sub

#End Region

#Region "Event Handlers"
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: txtbxThreshold_KeyDown
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: An event handler to capture enter key being pressed
    '   and send a tab command to go to the next tabindex item
    '
    ' INPUTS: e is used for keycode eventarg to compare for return key
    '
    ' OUTPUTS: None
    '
    ' NOTES: None
    '
    ' Change Log: 
    ' Date          Changed By      Notes
    ' 05/26/2006    ARA             Reset change logs for new version when copying over functionality
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Sub txtbxThreshold_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtbxThreshold.KeyDown
        If e.KeyCode = Windows.Forms.Keys.Return Then
            Windows.Forms.SendKeys.Send("{TAB}")
        End If
    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: txtbxThreshold_KeyPress
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: An event handler to stop any non number or control
    '   values from being added to the textbox
    '
    ' INPUTS: e is used for keychar eventarg to compare for return key
    '
    ' OUTPUTS: None
    '
    ' NOTES: None
    '
    ' Change Log: 
    ' Date          Changed By      Notes
    ' 05/26/2006    ARA             Reset change logs for new version when copying over functionality
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Sub txtbxThreshold_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtbxThreshold.KeyPress
        If Char.IsNumber(e.KeyChar) = False And Char.IsControl(e.KeyChar) = False Then
            e.Handled = True
        End If
    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: txtbxThreshold_Leave
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: An event handler to capture leaving the threshold textbox and 
    '  call the validate and reset sub
    '
    ' INPUTS: Not used
    '
    ' OUTPUTS: None
    '
    ' NOTES: None
    '
    ' Change Log: 
    ' Date          Changed By      Notes
    ' 05/26/2006    ARA             Reset change logs for new version when copying over functionality
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Sub txtbxThreshold_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtbxThreshold.Leave
        validateCellThreshAndSet()
    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: txtbxThreshConv_KeyDown
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: An event handler to capture enter key being pressed
    '   and send a tab command to go to the next tabindex item
    '
    ' INPUTS: e is used for keycode eventarg to compare for return key
    '
    ' OUTPUTS: None
    '
    ' NOTES: None
    '
    ' Change Log: 
    ' Date          Changed By      Notes
    ' 05/28/2006    ARA             Created
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Sub txtbxThreshConv_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtbxThreshConv.KeyDown
        If e.KeyCode = Windows.Forms.Keys.Return Then
            Windows.Forms.SendKeys.Send("{TAB}")
        End If
    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: txtbxThreshConv_KeyPress
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: An event handler to stop any non number or control
    '   values from being added to the textbox
    '
    ' INPUTS: e is used for keychar eventarg to compare for return key
    '
    ' OUTPUTS: None
    '
    ' NOTES: None
    '
    ' Change Log: 
    ' Date          Changed By      Notes
    ' 05/28/2006    ARA             Created
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Sub txtbxThreshConv_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtbxThreshConv.KeyPress
        If Char.IsNumber(e.KeyChar) = False And Char.IsControl(e.KeyChar) = False And e.KeyChar <> "." Then
            e.Handled = True
        End If
    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: txtbxThreshConv_Leave
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: An event handler to capture leaving the converted threshold textbox and 
    '  call the validate and reset sub
    '
    ' INPUTS: Not used
    '
    ' OUTPUTS: None
    '
    ' NOTES: None
    '
    ' Change Log: 
    ' Date          Changed By      Notes
    ' 05/28/2006    ARA             Created
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Sub txtbxThreshConv_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtbxThreshConv.Leave
        validateConvThreshAndSet()
    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: txtbxSnapThresh_KeyPress
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: An event handler to stop any non number or control
    '   values from being added to the textbox
    '
    ' INPUTS: e is used for keychar eventarg to compare for return key
    '
    ' OUTPUTS: None
    '
    ' NOTES: None
    '
    ' Change Log: 
    ' Date          Changed By      Notes
    ' 06/01/2006    ARA             Created
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Sub txtbxSnapThresh_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtbxSnapThresh.KeyPress
        If Char.IsNumber(e.KeyChar) = False And Char.IsControl(e.KeyChar) = False And e.KeyChar <> "." Then
            e.Handled = True
        End If
    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: txtbxSnapThresh_KeyDown
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: An event handler to capture enter key being pressed
    '   and send a tab command to go to the next tabindex item
    '
    ' INPUTS: e is used for keycode eventarg to compare for return key
    '
    ' OUTPUTS: None
    '
    ' NOTES: None
    '
    ' Change Log: 
    ' Date          Changed By      Notes
    ' 06/01/2006    ARA             Created
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Sub txtbxSnapThresh_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtbxSnapThresh.KeyDown
        If e.KeyCode = Windows.Forms.Keys.Return Then
            Windows.Forms.SendKeys.Send("{TAB}")
        End If
    End Sub

    Private Sub txtbxSnapThresh_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtbxSnapThresh.TextChanged
        threshDelinHasRan = False
    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: cmbxSelDem_SelectedIndexChanged
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: An event handler for the select dem list being changed
    '   merely sets the last dem to the one selected.
    '
    ' INPUTS: Not Used
    '
    ' OUTPUTS: None
    '
    ' NOTES: None
    '
    ' Change Log: 
    ' Date          Changed By      Notes
    ' 05/26/2006    ARA             Reset change logs for new version when copying over functionality
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Sub cmbxSelDem_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbxSelDem.SelectedIndexChanged
        Dim tmpPath, tmpLastDem As String
        If cmbxSelDem.SelectedIndex > 0 Then
            If cmbxSelDem.Items.Item(cmbxSelDem.SelectedIndex) <> lastDem Then
                tmpPath = getPathByName(cmbxSelDem.Items.Item(cmbxSelDem.SelectedIndex))
                tdbFileList.dem = tmpPath
            End If

            tmpLastDem = lastDem
            lastDem = cmbxSelDem.Items(cmbxSelDem.SelectedIndex)

            If tmpLastDem <> lastDem Then
                preProcHasRan = False
                threshDelinHasRan = False
                snapHasRan = False
                outletHasRan = False
                If getPathByName(lastDem).ToLower.Contains("ned") Then
                    cmbxElevUnits.SelectedIndex = 1
                Else
                    cmbxElevUnits.SelectedIndex = 0
                End If
                'txtbxThreshold.Text = ""
                'lastThresh = ""
                validateCellThreshAndSet()
                validateConvThreshAndSet()
            End If
        End If
    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: cmbxStream_SelectedIndexChanged
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: Will update the last stream on new selection
    '
    ' INPUTS: Not used
    '
    ' OUTPUTS: None
    '
    ' NOTES: None
    '
    ' Change Log: 
    ' Date          Changed By      Notes
    ' 07/24/2006    ARA             Created
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Sub cmbxStream_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbxStream.SelectedIndexChanged
        If cmbxStream.SelectedIndex > 0 Then
            lastStream = cmbxStream.Items(cmbxStream.SelectedIndex)
        End If
    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: cmbxOutlets_SelectedIndexChanged
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: Will set the snapHasRan flag to false when changing outlets
    '  to ensure the currently selected outlet will always be snapped correctly
    '
    ' INPUTS: Not Used
    '
    ' OUTPUTS: None
    '
    ' NOTES: None
    '
    ' Change Log: 
    ' Date          Changed By      Notes
    ' 05/26/2006    ARA             Reset change logs for new version when copying over functionality
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Sub cmbxOutlets_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbxOutlets.SelectedIndexChanged
        snapHasRan = False
        If cmbxOutlets.SelectedIndex > 0 Then
            Dim sf As New MapWinGIS.Shapefile
            Dim currOutlet, strpath As String
            currOutlet = cmbxOutlets.Items(cmbxOutlets.SelectedIndex)
            strpath = getPathByName(currOutlet)
            g_MapWin.Layers.CurrentLayer = getIndexByPath(strpath)
            If currOutlet = lastOutlet Then
                If myWrapper.outletShapesIdx.Count > 0 Then
                    g_MapWin.View.SelectedShapes.ClearSelectedShapes()
                    For i As Integer = 0 To myWrapper.outletShapesIdx.Count - 1
                        g_MapWin.View.SelectedShapes.AddByIndex(myWrapper.outletShapesIdx(i), Drawing.Color.Yellow)
                    Next
                Else
                    sf.Open(strpath)
                    g_MapWin.View.SelectedShapes.ClearSelectedShapes()
                    For i As Integer = 0 To sf.NumShapes - 1
                        g_MapWin.View.SelectedShapes.AddByIndex(i, Drawing.Color.Yellow)
                    Next
                    sf.Close()
                End If
            Else
                lastOutlet = currOutlet
                sf.Open(strpath)
                g_MapWin.View.SelectedShapes.ClearSelectedShapes()
                For i As Integer = 0 To sf.NumShapes - 1
                    g_MapWin.View.SelectedShapes.AddByIndex(i, Drawing.Color.Yellow)
                Next
                sf.Close()
            End If
            g_SelectingOutlets = True
            setOutletsSelected()
            g_SelectingOutlets = False
        End If
    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: cmbxThreshConvUnits_SelectedIndexChanged
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: An event handler for the select threshold conversion units which
    '  triggers the cell validate to reset the converted threshold textbox
    '
    ' INPUTS: Not Used
    '
    ' OUTPUTS: None
    '
    ' NOTES: None
    '
    ' Change Log: 
    ' Date          Changed By      Notes
    ' 05/26/2006    ARA             Reset change logs for new version when copying over functionality
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Sub cmbxThreshConvUnits_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbxThreshConvUnits.SelectedIndexChanged
        validateCellThreshAndSet()
        validateConvThreshAndSet()
        lastConvUnit = cmbxThreshConvUnits.SelectedIndex
    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: cmbxMask_SelectedIndexChanged
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: Will update the threshold values when masks change
    '
    ' INPUTS: Not used
    '
    ' OUTPUTS: None
    '
    ' NOTES: None
    '
    ' Change Log: 
    ' Date          Changed By      Notes
    ' 06/19/2006    ARA             Created
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Sub cmbxMask_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbxMask.SelectedIndexChanged
        If cmbxMask.SelectedIndex > 0 Then
            Dim sf As New MapWinGIS.Shapefile
            Dim currMask, strpath As String
            currMask = cmbxMask.Items(cmbxMask.SelectedIndex)
            strpath = getPathByName(currMask)
            g_MapWin.Layers.CurrentLayer = getIndexByPath(strpath)
            If currMask = lastMask Then
                If IO.Path.GetExtension(strpath) = ".shp" Then
                    If myWrapper.maskShapesIdx.Count > 0 Then
                        g_MapWin.View.SelectedShapes.ClearSelectedShapes()
                        For i As Integer = 0 To myWrapper.maskShapesIdx.Count - 1
                            g_MapWin.View.SelectedShapes.AddByIndex(myWrapper.maskShapesIdx(i), Drawing.Color.Yellow)
                        Next
                    Else
                        sf.Open(strpath)
                        g_MapWin.View.SelectedShapes.ClearSelectedShapes()
                        For i As Integer = 0 To sf.NumShapes - 1
                            g_MapWin.View.SelectedShapes.AddByIndex(i, Drawing.Color.Yellow)
                        Next
                        sf.Close()
                    End If
                    g_SelectingMask = True
                    setMaskSelected()
                    g_SelectingMask = False
                End If
            Else
                lastMask = currMask
                If IO.Path.GetExtension(strpath) = ".shp" Then
                    sf.Open(strpath)
                    g_MapWin.View.SelectedShapes.ClearSelectedShapes()
                    For i As Integer = 0 To sf.NumShapes - 1
                        g_MapWin.View.SelectedShapes.AddByIndex(i, Drawing.Color.Yellow)
                    Next
                    sf.Close()
                    g_SelectingMask = True
                    setMaskSelected()
                    g_SelectingMask = False
                End If
            End If

            'txtbxThreshold.Text = ""
            'lastThresh = ""
            validateCellThreshAndSet()
            validateConvThreshAndSet()
        Else
            lastMask = ""
        End If
    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: chkbxMask_CheckedChanged
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: Will update the threshold values when masks change
    '
    ' INPUTS: Not used
    '
    ' OUTPUTS: None
    '
    ' NOTES: None
    '
    ' Change Log: 
    ' Date          Changed By      Notes
    ' 06/19/2006    ARA             Created
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Sub chkbxMask_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkbxMask.CheckedChanged
        If cmbxSelDem.SelectedIndex > 0 Then
            'txtbxThreshold.Text = ""
            'lastThresh = ""
            validateCellThreshAndSet()
            validateConvThreshAndSet()
        End If
    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: rdobtnUseExtents_CheckedChanged
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: Will update the threshold values when masks change and enable
    '  the associated components for opening a mask file
    '
    ' INPUTS: Not used
    '
    ' OUTPUTS: None
    '
    ' NOTES: None
    '
    ' Change Log: 
    ' Date          Changed By      Notes
    ' 06/20/2006    ARA             Created
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Sub rdobtnUseExtents_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdobtnUseExtents.CheckedChanged
        cmbxMask.Enabled = Not rdobtnUseExtents.Checked
        btnDrawMask.Enabled = Not rdobtnUseExtents.Checked
        btnSelectMask.Enabled = Not rdobtnUseExtents.Checked
        lblMaskSelected.Enabled = Not rdobtnUseExtents.Checked
        btnSetExtents.Enabled = rdobtnUseExtents.Checked
        If cmbxSelDem.SelectedIndex > 0 Then
            'txtbxThreshold.Text = ""
            'lastThresh = ""
            validateCellThreshAndSet()
            validateConvThreshAndSet()
        End If
    End Sub

    Private Sub chkbxBurnStream_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkbxBurnStream.Click
        If tdbChoiceList.useBurnIn <> chkbxBurnStream.Checked Then
            preProcHasRan = False
        End If
    End Sub

    Private Sub chkbxMask_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkbxMask.Click
        If tdbChoiceList.useMaskFileOrExtents <> chkbxMask.Checked Then
            preProcHasRan = False
        End If
    End Sub

    Private Sub rdobtnUseExtents_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdobtnUseExtents.Click
        If tdbChoiceList.useExtentMask <> rdobtnUseExtents.Checked Then
            preProcHasRan = False
        End If
    End Sub

    Private Sub rdobtnUseFileMask_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdobtnUseFileMask.Click
        If tdbChoiceList.useExtentMask <> rdobtnUseExtents.Checked Then
            preProcHasRan = False
        End If
    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: btnAdvanced_Click
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: Will display the advancedOptions form to let users select custom
    '   output display and turn off special stream and watershed calculation
    '
    ' INPUTS: Not used
    '
    ' OUTPUTS: None
    '
    ' NOTES: None
    '
    ' Change Log: 
    ' Date          Changed By      Notes
    ' 05/26/2006    ARA             Created
    ' 08/09/2006    ARA             Save Config on leaving.
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Sub btnAdvanced_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdvanced.Click
        frmSettings.BringToFront()
        frmSettings.ShowDialog()
        SaveToChoices()
        tdbChoiceList.SaveConfig()
    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: btnBrowseDem_Click
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: Will show an open dialog filtered for grid data
    '   and allow the user to select a base dem grid file. Once
    '   selected, the file path will be loaded as a layer in 
    '   MapWindow, then refill the comboboxes to add it to the right
    '   lists.
    '
    ' INPUTS: Not used
    '
    ' OUTPUTS: None
    '
    ' NOTES: None
    '
    ' Change Log: 
    ' Date          Changed By      Notes
    ' 05/26/2006    ARA             Reset change logs for new version when copying over functionality
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Sub btnBrowseDem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseDem.Click
        Dim g As New MapWinGIS.Grid
        Dim strPath As String
        Dim projCheck As Double
        Dim strProj As String
        Dim fdiagOpen As New System.Windows.Forms.OpenFileDialog
        fdiagOpen.Filter = g.CdlgFilter
        fdiagOpen.FilterIndex = 1

        If fdiagOpen.ShowDialog() = Windows.Forms.DialogResult.OK Then
            preProcHasRan = False
            threshDelinHasRan = False
            strPath = fdiagOpen.FileName

            If Not layerExists(strPath) Then
                Dim fstream As New System.IO.FileStream(strPath, IO.FileMode.Open)
                If fstream.Length > g_MaxFileSize Then
                    Logger.Msg("The grid selected was above the size limit supported by Automatic Watershed Delineation. Please clip the grid to smaller portions or resample to a lower resolution." & _
                               "(" & fstream.Length & ":" & g_MaxFileSize & ")", _
                               MsgBoxStyle.OkOnly, "Automatic Watershed Delineation Error")
                    Exit Sub
                End If
                fstream.Close()

                g.Open(strPath, , True)
                g.CellToProj(10, 10, projCheck, projCheck)
                g.Close()
                projCheck = System.Math.Abs(System.Math.Floor(projCheck))
                strProj = projCheck.ToString
                If strProj.Length > 3 Then
                    AddMap(strPath, LayerType.dem)
                    fillCombos()
                    If (cmbxSelDem.Items.IndexOf(getNameByPath(strPath)) = -1) Then
                        cmbxSelDem.Items.Add(getNameByPath(strPath))
                    End If
                    cmbxSelDem.SelectedIndex = cmbxSelDem.Items.IndexOf(getNameByPath(strPath))
                Else
                    Logger.Msg("The Grid selected was unprojected. Please reproject the grid using GIS Tools before adding.", MsgBoxStyle.OkOnly, "Automatic Watershed Delineation Error")
                End If
            Else
                Logger.Msg("That grid layer already exists. It will appear in the drop down list if it is a valid format.", MsgBoxStyle.OkOnly, "Automatic Watershed Delineation Error")
            End If
        End If
    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: btnBrowseOutlets_Click
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: Will show an open dialog filtered for shapefile data
    '   and allow the user to select an outlets point shape file. Once
    '   selected, the file path will be loaded as a layer in 
    '   MapWindow, then refill the comboboxes to add it to add it
    '   to the list if it was a point shape type
    '
    ' INPUTS: Not used
    '
    ' OUTPUTS: None
    '
    ' NOTES: None
    '
    ' Change Log: 
    ' Date          Changed By      Notes
    ' 05/26/2006    ARA             Reset change logs for new version when copying over functionality
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Sub btnBrowseOutlets_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseOutlets.Click
        Dim strPath As String
        Dim sf As New MapWinGIS.Shapefile
        Dim fdiagOpen As New System.Windows.Forms.OpenFileDialog

        fdiagOpen.Filter = sf.CdlgFilter
        fdiagOpen.FilterIndex = 1

        If fdiagOpen.ShowDialog() = Windows.Forms.DialogResult.OK Then
            strPath = fdiagOpen.FileName
            chkbxUseOutlet.Checked = True
            If Not layerExists(strPath) Then
                sf.Open(strPath)
                If sf.ShapefileType = MapWinGIS.ShpfileType.SHP_POINT Or _
                    sf.ShapefileType = MapWinGIS.ShpfileType.SHP_POINTM Or _
                    sf.ShapefileType = MapWinGIS.ShpfileType.SHP_POINTZ Or _
                    sf.ShapefileType = MapWinGIS.ShpfileType.SHP_MULTIPOINT _
                Then
                    AddMap(strPath, LayerType.Outlets)
                    lastOutlet = getNameByPath(strPath)
                    fillCombos()
                    currDrawPath = strPath
                Else
                    Logger.Msg("The shape file selected (" & sf.Filename & ")" & vbCrLf & _
                               "was not a point shape file and thus was not added.", MsgBoxStyle.OkOnly, "Automatic Watershed Delineation Error")
                End If
                sf.Close()
            Else
                Logger.Msg("That point layer already exists. It will appear in the drop down list if it is a valid format.", MsgBoxStyle.OkOnly, "Automatic Watershed Delineation Error")
            End If
        End If
    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: btnBrowseStream_Click
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: Will show an open dialog filtered for shape file data
    '   and allow the user to select a stream flow polyline shape file. 
    '   Once selected, the file will be loaded as a layer in 
    '   MapWindow, then refill the comboboxes to add it to the right
    '   list.
    '
    ' INPUTS: Not used
    '
    ' OUTPUTS: None
    '
    ' NOTES: None
    '
    ' Change Log: 
    ' Date          Changed By      Notes
    ' 05/26/2006    ARA             Reset change logs for new version when copying over functionality
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Sub btnBrowseStream_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseStream.Click
        Dim strPath As String
        Dim sf As New MapWinGIS.Shapefile
        Dim lyr As MapWindow.Interfaces.Layer
        Dim fdiagOpen As New System.Windows.Forms.OpenFileDialog

        fdiagOpen.Filter = sf.CdlgFilter
        fdiagOpen.FilterIndex = 1

        If fdiagOpen.ShowDialog() = Windows.Forms.DialogResult.OK Then
            chkbxBurnStream.Checked = True
            strPath = fdiagOpen.FileName
            If Not layerExists(strPath) Then
                sf.Open(strPath)
                If sf.ShapefileType = MapWinGIS.ShpfileType.SHP_POLYLINE Or _
                    sf.ShapefileType = MapWinGIS.ShpfileType.SHP_POLYLINEM Or _
                    sf.ShapefileType = MapWinGIS.ShpfileType.SHP_POLYLINEZ _
                Then
                    lyr = g_MapWin.Layers.Add(sf, "Stream Burn-in (" + System.IO.Path.GetFileName(strPath) + ")")
                    lyr.Color = System.Drawing.Color.Blue
                    fillCombos()
                    lastStream = getNameByPath(strPath)
                    cmbxStream.SelectedIndex = cmbxStream.Items.IndexOf(lastStream)
                Else
                    sf.Close()
                    Logger.Msg("The shape file selected (" & sf.Filename & ")" & vbCrLf & _
                               "was not a polyline shape file and thus was not added.", MsgBoxStyle.OkOnly, "Automatic Watershed Delineation Error")
                End If
            Else
                Logger.Msg("That polyline shape layer already exists. It will appear in the drop down list if it is a valid format.", MsgBoxStyle.OkOnly, "Automatic Watershed Delineation Error")
            End If
        End If
    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: btnBrowseMask_Click
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: Will show an open dialog filtered for grid and shape file data
    '   and allow the user to select a mask grid or polygon shapefile 
    '   Once selected, the file will be loaded as a layer in 
    '   MapWindow, then refill the comboboxes to add it to the right
    '   list.
    '
    ' INPUTS: Not used
    '
    ' OUTPUTS: None
    '
    ' NOTES: None
    '
    ' Change Log: 
    ' Date          Changed By      Notes
    ' 05/26/2006    ARA             Created
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Sub btnBrowseMask_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseMask.Click
        Dim g As New MapWinGIS.Grid
        Dim sf As New MapWinGIS.Shapefile
        Dim strPath As String
        Dim projCheck As Double
        Dim strProj As String
        Dim fdiagOpen As New System.Windows.Forms.OpenFileDialog

        fdiagOpen.Filter = "All Supported Grid and Shapefile Formats|sta.adf;*.bgd;*.asc;*.tif;????cel0.ddf;*.arc;*.aux;*.pix;*.dhm;*.dt0;*.dt1;*.ecw;*.bil;*.sid;*.shp|" + g.CdlgFilter + "|" + sf.CdlgFilter
        fdiagOpen.FilterIndex = 1

        If fdiagOpen.ShowDialog() = Windows.Forms.DialogResult.OK Then
            strPath = fdiagOpen.FileName
            chkbxMask.Checked = True
            rdobtnUseFileMask.Checked = True
            If Not layerExists(strPath) Then
                If System.IO.Path.GetExtension(strPath) = ".shp" Then
                    sf.Open(strPath)
                    If sf.ShapefileType = MapWinGIS.ShpfileType.SHP_POLYGON Or sf.ShapefileType = MapWinGIS.ShpfileType.SHP_POLYGONM Or sf.ShapefileType = MapWinGIS.ShpfileType.SHP_POLYGONZ Then
                        AddMap(strPath, LayerType.mask)
                        fillCombos()
                        lastMask = getNameByPath(strPath)
                        cmbxMask.SelectedIndex = cmbxMask.Items.IndexOf(lastMask)
                        preProcHasRan = False
                        threshDelinHasRan = False
                        snapHasRan = False
                    Else
                        Logger.Msg("The shapefile selected (" & sf.Filename & ")" & vbCrLf & _
                                   "was not a polygon shapefile. Only polygon shapefiles can be used for a mask.", MsgBoxStyle.OkOnly, "Automatic Watershed Delineation Error")
                    End If
                    sf.Close()
                Else
                    Dim fstream As New System.IO.FileStream(strPath, IO.FileMode.Open)
                    If fstream.Length > g_MaxFileSize Then
                        Logger.Msg("The grid selected was above the size limit supported by Automatic Watershed Delineation. Please clip the grid to smaller portions or resample to a lower resolution." & _
                                   "(" & fstream.Length & ":" & g_MaxFileSize & ")", _
                                   MsgBoxStyle.OkOnly, "Automatic Watershed Delineation Error")
                        Exit Sub
                    End If
                    fstream.Close()
                    g.Open(strPath, , True)
                    g.CellToProj(10, 10, projCheck, projCheck)
                    g.Close()
                    projCheck = System.Math.Abs(System.Math.Floor(projCheck))
                    strProj = projCheck.ToString
                    If strProj.Length > 3 Then
                        AddMap(strPath, LayerType.mask)
                        fillCombos()
                        lastMask = getNameByPath(strPath)
                        cmbxMask.SelectedIndex = cmbxMask.Items.IndexOf(lastMask)
                    Else
                        Logger.Msg("The Grid selected was unprojected. Please reproject the grid using GIS Tools before adding.", MsgBoxStyle.OkOnly, "Automatic Watershed Delineation Error")
                    End If
                End If
            Else
                Logger.Msg("layer '" & strPath & "'" & vbCrLf & _
                           "already exists. It will appear in the drop down list if it is a valid format.", MsgBoxStyle.OkOnly, "Automatic Watershed Delineation Error")
            End If

        End If
    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: btnDrawMask_Click
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: Will check for a valid draw layer, prompt to create one if one
    '  doesn't exist, then open the frmDrawSelectShape_v2 form and set the interface
    '  to drawing mask mode so that the interface can handle the functionality.
    '
    ' INPUTS: Not used
    '
    ' OUTPUTS: None
    '
    ' NOTES: None
    '
    ' Change Log: 
    ' Date          Changed By      Notes
    ' 05/26/2006    ARA             Created
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Sub btnDrawMask_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDrawMask.Click
        Dim sf As New MapWinGIS.Shapefile
        Dim g As New MapWinGIS.Grid
        Dim fileSave As New System.Windows.Forms.SaveFileDialog
        Dim strShape As String

        If cmbxMask.SelectedIndex > 0 Then
            strShape = getPathByName(cmbxMask.Items(cmbxMask.SelectedIndex))
        Else
            strShape = ""
        End If
        myWrapper.maskShapesIdx.Clear()
        If strShape = "" Or System.IO.Path.GetExtension(strShape) <> ".shp" Then
            If Logger.Msg("There is not a shapefile Mask selected which can be drawn on, would you like to create a new shapefile mask?", MsgBoxStyle.YesNo, "Create new Mask?") = MsgBoxResult.Yes Then
                If cmbxSelDem.SelectedIndex > 0 Then
                    fileSave.Filter = sf.CdlgFilter
                    fileSave.FilterIndex = 1
                    If fileSave.ShowDialog() = Windows.Forms.DialogResult.OK Then
                        chkbxMask.Checked = True

                        strShape = fileSave.FileName
                        MapWinGeoProc.DataManagement.DeleteShapefile(strShape)
                        sf.CreateNew(strShape, MapWinGIS.ShpfileType.SHP_POLYGON)
                        sf.SaveAs(strShape)
                        Dim idField As New MapWinGIS.Field
                        Dim idFieldNum As Integer

                        sf.StartEditingTable()
                        idField.Name = "MWShapeID"
                        idField.Type = MapWinGIS.FieldType.INTEGER_FIELD
                        idFieldNum = sf.NumFields
                        sf.EditInsertField(idField, idFieldNum)
                        sf.StopEditingTable()

                        g.Open(getPathByName(cmbxSelDem.Items(cmbxSelDem.SelectedIndex)))
                        sf.Projection = g.Header.Projection
                        g.Close()
                        sf.Close()
                        AddMap(strShape, LayerType.mask)
                        lastMask = getNameByPath(strShape)
                        fillCombos()
                        currDrawPath = strShape
                        g_MapWin.Layers.CurrentLayer = getIndexByPath(currDrawPath)
                        g_DrawingMask = True
                        g_MapWin.View.CursorMode = MapWinGIS.tkCursorMode.cmNone
                        frmDrawSelect = New frmDrawSelectShape_v2
                        frmDrawSelect.Initialize(Me, "Left-click to draw a vertex. Right click to finish drawing.", False)
                        preProcHasRan = False
                        threshDelinHasRan = False
                        snapHasRan = False
                    End If
                Else
                    Logger.Msg("There is no Base DEM selected. Without the Base DEM, you cannot draw a mask on the new shapefile layer. Please select a Base DEM first.", MsgBoxStyle.OkOnly, "Automatic Watershed Delineation Error")
                End If
            End If
        Else
            chkbxMask.Checked = True
            currDrawPath = strShape
            g_MapWin.Layers.CurrentLayer = getIndexByPath(currDrawPath)
            g_DrawingMask = True
            g_MapWin.View.CursorMode = MapWinGIS.tkCursorMode.cmNone
            frmDrawSelect = New frmDrawSelectShape_v2
            frmDrawSelect.Initialize(Me, "Left-click to draw a vertex. Right click to finish drawing.", False)
        End If
    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: btnSelectMask_Click
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: Will check for a valid select layer, then open the 
    ' frmDrawSelectShape_v2 form and set the interface to selecting mask mode 
    ' so that the interface and Done button can handle the functionality.
    '
    ' INPUTS: Not used
    '
    ' OUTPUTS: None
    '
    ' NOTES: None
    '
    ' Change Log: 
    ' Date          Changed By      Notes
    ' 05/26/2006    ARA             Created
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Sub btnSelectMask_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectMask.Click
        Dim strShape As String
        Dim sf As New MapWinGIS.Shapefile
        preProcHasRan = False
        threshDelinHasRan = False
        snapHasRan = False
        If cmbxMask.SelectedIndex > 0 Then
            strShape = getPathByName(cmbxMask.Items(cmbxMask.SelectedIndex))
        Else
            strShape = ""
        End If

        If System.IO.Path.GetExtension(strShape) = ".shp" Then
            sf.Open(strShape)
            If sf.NumShapes > 0 Then
                chkbxMask.Checked = True
                currSelectPath = getPathByName(cmbxMask.Items(cmbxMask.SelectedIndex))
                g_MapWin.Layers.CurrentLayer = getIndexByPath(currSelectPath)
                g_MapWin.View.CursorMode = MapWinGIS.tkCursorMode.cmSelection
                g_SelectingMask = True
                frmDrawSelect = New frmDrawSelectShape_v2
                frmDrawSelect.Initialize(Me, "Hold Control and click on each to Select multiple shapes.", False)
            Else
                Logger.Msg("No shapes could be found in the shapefile selected (" & sf.Filename & ")." & vbCrLf & _
                           "Please use Draw Mask to draw a Mask or select a new shapefile.", MsgBoxStyle.OkOnly, "Automatic Watershed Delineation Error")
            End If
            sf.Close()
        Else
            Logger.Msg("There is no shapefile Mask selected to choose shapes from. Please select a Mask from the Mask drop down list or use the browse button beside the list to add one.", MsgBoxStyle.OkOnly, "Automatic Watershed Delineation Error")
        End If
    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: btnSetExtents_Click
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: Will check for selected Base DEM and if found, allow users to zoom
    '  to select interface
    '
    ' INPUTS: Not used
    '
    ' OUTPUTS: None
    '
    ' NOTES: None
    '
    ' Change Log: 
    ' Date          Changed By      Notes
    ' 06/15/2006    ARA             Created
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Sub btnSetExtents_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSetExtents.Click
        If cmbxSelDem.SelectedIndex > 0 Then
            g_MapWin.View.CursorMode = MapWinGIS.tkCursorMode.cmZoomIn
            chkbxMask.Checked = True
            rdobtnUseExtents.Checked = True
            frmDrawSelect = New frmDrawSelectShape_v2
            frmDrawSelect.Initialize(Me, "Zoom to the extents you wish to use for a mask.", False)
            preProcHasRan = False
            threshDelinHasRan = False
            snapHasRan = False
        Else
            Logger.Msg("There is no Base DEM selected to choose the extents from. Please select one from the Base DEM drop down list or open a new one using the browse button next to it.", MsgBoxStyle.OkOnly, "Automatic Watershed Delineation Error")
        End If

    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: btnDrawOutlets_Click
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: Will check for a valid draw layer, prompt to create one if one
    '  doesn't exist, then open the frmDrawSelectShape_v2 form and set the interface
    '  to drawing mask mode so that the interface can handle the functionality.
    '
    ' INPUTS: Not used
    '
    ' OUTPUTS: None
    '
    ' NOTES: None
    '
    ' Change Log: 
    ' Date          Changed By      Notes
    ' 05/26/2006    ARA             Created
    ' 07/06/2006    ARA             Removes snap preview and makes sure draw
    '                                layer is visible
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Sub btnDrawOutlets_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDrawOutlets.Click
        Dim sf As New MapWinGIS.Shapefile
        Dim g As New MapWinGIS.Grid
        Dim fileSave As New System.Windows.Forms.SaveFileDialog
        Dim strShape As String

        If cmbxOutlets.SelectedIndex > 0 Then
            chkbxUseOutlet.Checked = True
            strShape = getPathByName(cmbxOutlets.Items(cmbxOutlets.SelectedIndex))
            RemoveLayer(IO.Path.GetDirectoryName(strShape) + "\" + IO.Path.GetFileNameWithoutExtension(strShape) + "_clip_snap.shp")
            RemoveLayer(IO.Path.GetDirectoryName(strShape) + "\" + IO.Path.GetFileNameWithoutExtension(strShape) + "_snap.shp")
            currDrawPath = strShape
            g_MapWin.Layers.CurrentLayer = getIndexByPath(currDrawPath)
            g_MapWin.Layers.Item(g_MapWin.Layers.CurrentLayer).Visible = True
            g_DrawingOutletsOrInlets = True
            g_MapWin.View.CursorMode = MapWinGIS.tkCursorMode.cmNone
            frmDrawSelect = New frmDrawSelectShape_v2
            frmDrawSelect.Initialize(Me, "Click to place outlets or inlets on or near a stream reach.", True)
        Else
            If Logger.Msg("There is no outlets/inlets shapefile selected which can be drawn on, would you like to create a new outlets/inlets shapefile?", MsgBoxStyle.YesNo, "Create new Outlets/Inlets File?") = MsgBoxResult.Yes Then
                chkbxUseOutlet.Checked = True
                If cmbxSelDem.SelectedIndex > 0 Then
                    fileSave.Filter = sf.CdlgFilter
                    fileSave.FilterIndex = 1
                    If fileSave.ShowDialog() = Windows.Forms.DialogResult.OK Then
                        strShape = fileSave.FileName
                        MapWinGeoProc.DataManagement.DeleteShapefile(strShape)
                        sf.CreateNew(strShape, MapWinGIS.ShpfileType.SHP_POINT)
                        sf.SaveAs(strShape)
                        Dim idField As New MapWinGIS.Field
                        Dim idFieldNum As Integer

                        sf.StartEditingTable()
                        idField.Name = "MWShapeID"
                        idField.Type = MapWinGIS.FieldType.INTEGER_FIELD
                        idFieldNum = sf.NumFields
                        sf.EditInsertField(idField, idFieldNum)
                        sf.StopEditingTable()

                        g.Open(getPathByName(cmbxSelDem.Items(cmbxSelDem.SelectedIndex)))
                        sf.Projection = g.Header.Projection
                        g.Close()
                        sf.Close()
                        RemoveLayer(IO.Path.GetDirectoryName(strShape) + "\" + IO.Path.GetFileNameWithoutExtension(strShape) + "_clip_snap.shp")
                        RemoveLayer(IO.Path.GetDirectoryName(strShape) + "\" + IO.Path.GetFileNameWithoutExtension(strShape) + "_snap.shp")
                        AddMap(strShape, LayerType.Outlets)
                        lastOutlet = getNameByPath(strShape)
                        fillCombos()
                        currDrawPath = strShape
                        g_MapWin.Layers.CurrentLayer = getIndexByPath(currDrawPath)
                        g_DrawingOutletsOrInlets = True
                        g_MapWin.View.CursorMode = MapWinGIS.tkCursorMode.cmNone
                        frmDrawSelect = New frmDrawSelectShape_v2
                        frmDrawSelect.Initialize(Me, "Click to place outlets or inlets on or near a stream reach.", True)
                    End If
                Else
                    Logger.Msg("There is no Base DEM selected. Without the Base DEM, you cannot draw outlets or inlets on the new shapefile layer. Please select a Base DEM first.", MsgBoxStyle.OkOnly, "Automatic Watershed Delineation Error")
                End If
            End If
        End If
        myWrapper.outletShapesIdx.Clear()
    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: btnSelectOutlets_Click
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: Will check for a valid select layer, then open the 
    ' frmDrawSelectShape_v2 form and set the interface to selecting mask mode 
    ' so that the interface and Done button can handle the functionality.
    '
    ' INPUTS: Not used
    '
    ' OUTPUTS: None
    '
    ' NOTES: None
    '
    ' Change Log: 
    ' Date          Changed By      Notes
    ' 05/28/2006    ARA             Created
    ' 07/06/2006    ARA             Removes snap preview and makes sure draw
    '                                layer is visible
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Sub btnSelectOutlets_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectOutlets.Click
        Dim sf As New MapWinGIS.Shapefile

        If cmbxOutlets.SelectedIndex > 0 Then
            chkbxUseOutlet.Checked = True
            currSelectPath = getPathByName(cmbxOutlets.Items(cmbxOutlets.SelectedIndex))
            g_MapWin.Layers.CurrentLayer = getIndexByPath(currSelectPath)
            RemoveLayer(IO.Path.GetDirectoryName(currSelectPath) + "\" + IO.Path.GetFileNameWithoutExtension(currSelectPath) + "_clip_snap.shp")
            RemoveLayer(IO.Path.GetDirectoryName(currSelectPath) + "\" + IO.Path.GetFileNameWithoutExtension(currSelectPath) + "_snap.shp")
            g_MapWin.Layers.Item(g_MapWin.Layers.CurrentLayer).Visible = True
            sf.Open(currSelectPath)
            If sf.NumShapes > 0 Then
                g_MapWin.View.CursorMode = MapWinGIS.tkCursorMode.cmSelection
                currSelectPath = getPathByName(cmbxOutlets.Items(cmbxOutlets.SelectedIndex))
                g_SelectingOutlets = True
                frmDrawSelect = New frmDrawSelectShape_v2
                frmDrawSelect.Initialize(Me, "Hold Control and Click to Select Outlets/Inlets near or on reaches.", False)
            Else
                MsgBox("No shapes could be found in the shapefile selected. Please use Draw Outlets/Inlets to draw new outlets/inlets or select a new shapefile.", MsgBoxStyle.OkOnly, "Automatic Watershed Delineation Error")
            End If
            sf.Close()
        Else
            MsgBox("There is no shapefile of outlets and inlets selected to choose outlets/inlets from. Please select an Outlets/Inlets shapefile from the Outlets/Inlets drop down list or use the browse button beside the list to add one.", MsgBoxStyle.OkOnly, "Automatic Watershed Delineation Error")
        End If
    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: btnSnapTo_Click
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: Will clip and snap the outlets if the stream network has been 
    '  delineated, in order to give the users a preview of where their outlets will
    '  actually be delineated from.
    '
    ' INPUTS: Not used
    '
    ' OUTPUTS: None
    '
    ' NOTES: None
    '
    ' Change Log: 
    ' Date          Changed By      Notes
    ' 06/22/2006    ARA             Created
    ' 07/06/2006    ARA             Updated to run thresh delin if hasn't ran
    '                                rather than giving error
    ' 08/08/2006    ARA             Modified to exit on fail of runThreshDelin
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Sub btnSnapTo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSnapTo.Click
        If cmbxOutlets.SelectedIndex > 0 Then
            If myWrapper.outletShapesIdx.Count > 0 Then
                If Not threshDelinHasRan Then
                    'MsgBox("Snapping outlets cannot be done until the stream network have been delineated. Please click the Run button in the Delineation by Threshold Method section before trying to snap points.")
                    If Not runDelinByThresh() Then
                        Exit Sub
                    End If
                End If
                runFormInit()
                tdbFileList.outletshpfile = getPathByName(cmbxOutlets.Items.Item(cmbxOutlets.SelectedIndex))
                'tmpOutPath = tdbFileList.outletshpfile
                g_MapWin.Layers.Item(getIndexByPath(tdbFileList.outletshpfile)).Visible = False
                RemoveLayer(IO.Path.GetDirectoryName(tdbFileList.outletshpfile) + "\" + IO.Path.GetFileNameWithoutExtension(tdbFileList.outletshpfile) + "_clip_snap.shp")
                RemoveLayer(IO.Path.GetDirectoryName(tdbFileList.outletshpfile) + "\" + IO.Path.GetFileNameWithoutExtension(tdbFileList.outletshpfile) + "_snap.shp")
                runClipSelectedOutlets()
                runAutoSnap()
                AddMap(tdbFileList.outletshpfile, LayerType.outletPreview)
                'lastOutlet = getNameByPath(tdbFileList.outletshpfile)
                fillCombos()

                'Select all shapes in new snap file
                'g_MapWin.View.SelectedShapes.ClearSelectedShapes()
                'Dim sf As New MapWinGIS.Shapefile
                'sf.Open(tdbFileList.outletshpfile)
                'For i As Integer = 0 To sf.NumShapes - 1
                'g_MapWin.View.SelectedShapes.AddByIndex(i, Drawing.Color.Yellow)
                'Next
                'sf.Close()
                'g_SelectingOutlets = True
                'setOutletsSelected()
                'g_SelectingOutlets = False
                'tdbFileList.outletshpfile = tmpOutPath

                snapHasRan = True
                runFormCleanup()
            Else
                MsgBox("There are no outlets/inlets currently selected. Please click the Select Outlets/Inlets button to select outlets and inlets to snap.", MsgBoxStyle.OkOnly, "Automatic Watershed Delineation Error")
            End If
        Else
            MsgBox("There is no outlets/inlets layer selected. Please select an outlets/inlets shapefile from the Outlets/Inlets dropdown list or add a new one using the button beside it.", MsgBoxStyle.OkOnly, "Automatic Watershed Delineation Error")
        End If
    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: btnRunPreproc_Click
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: An event handler for the preprocessing run button being clicked
    '   which activates the runPreprocessing sub
    '
    ' INPUTS: Not Used
    '
    ' OUTPUTS: None
    '
    ' NOTES: None (yet)
    '
    ' Change Log: 
    ' Date          Changed By      Notes
    ' 05/26/2006    ARA             Reset change logs for new version when copying over functionality
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Sub btnRunPreproc_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRunPreproc.Click
        runPreprocessing()
    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: btnRunThreshDelin_Click
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: An event handler for the thresh delin run button being clicked
    '   which activates the runDelinByThresh sub
    '
    ' INPUTS: Not Used
    '
    ' OUTPUTS: None
    '
    ' NOTES: None (yet)
    '
    ' Change Log: 
    ' Date          Changed By      Notes
    ' 05/26/2006    ARA             Reset change logs for new version when copying over functionality
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Sub btnRunThreshDelin_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRunThreshDelin.Click
        runDelinByThresh()
    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: btnRunOutletFinish_Click
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: An event handler for the outlet and finish run button being clicked
    '   which activates the runOutletsAndFinish sub
    '
    ' INPUTS: Not Used
    '
    ' OUTPUTS: None
    '
    ' NOTES: None (yet)
    '
    ' Change Log: 
    ' Date          Changed By      Notes
    ' 05/26/2006    ARA             Reset change logs for new version when copying over functionality
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Sub btnRunOutletFinish_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRunOutletFinish.Click
        runOutletsAndFinish()
    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: btnRunAll_Click
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: An event handler for the run all button being clicked
    '   which activates the runAll sub
    '
    ' INPUTS: Not Used
    '
    ' OUTPUTS: None
    '
    ' NOTES: None (yet)
    '
    ' Change Log: 
    ' Date          Changed By      Notes
    ' 05/26/2006    ARA             Reset change logs for new version when copying over functionality
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Sub btnRunAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRunAll.Click
        If runAll() Then
            closingCleanup()
            Me.Close()
        End If
    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: btnCancel_Click
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: An event handler for the cancel button which cleans and 
    ' closes the form
    '
    ' INPUTS: Not Used
    '
    ' OUTPUTS: None
    '
    ' NOTES: None (yet)
    '
    ' Change Log: 
    ' Date          Changed By      Notes
    ' 05/26/2006    ARA             Reset change logs for new version when copying over functionality
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        closingCleanup()
        Me.Close()
    End Sub

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: btnLoadPre_Click
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: An event handler for the Load Preprocessing button which
    '  is used to load paths of previously generated pre-processing outputs, 
    '  including the pit fill, d8, d8 slope, dinf, and dinf slope files. If
    '  all the paths were specified, then the function sets the choicelist items
    '  used for these
    '
    ' INPUTS: Not Used
    '
    ' OUTPUTS: None
    '
    ' NOTES: None (yet)
    '
    ' Change Log: 
    ' Date          Changed By      Notes
    ' 05/26/2006    ARA             Reset change logs for new version when copying over functionality
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Sub btnLoadPre_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLoadPre.Click
        If cmbxSelDem.SelectedIndex = 0 Then
            MsgBox("You need to select a DEM grid from the Base Elevation Grid drop-down list. If no layers are available to select, you can use the browse button beside the list to open an existing DEM.", MsgBoxStyle.OkOnly, "Automatic Watershed Delineation Error")
        Else
            frmLoadOutput.ShowDialog()
            If frmLoadOutput.fillPath <> "" And frmLoadOutput.sd8Path <> "" And frmLoadOutput.d8Path <> "" Then
                If tdbChoiceList.useDinf And frmLoadOutput.dinfSlopePath <> "" And frmLoadOutput.dInfPath <> "" Then
                    lblPreOut.Visible = True
                    tdbChoiceList.FillGridPath = frmLoadOutput.fillPath
                    tdbChoiceList.D8SlopePath = frmLoadOutput.sd8Path
                    tdbChoiceList.D8Path = frmLoadOutput.d8Path
                    tdbChoiceList.DInfPath = frmLoadOutput.dInfPath
                    tdbChoiceList.DInfSlopePath = frmLoadOutput.dinfSlopePath
                Else
                    lblPreOut.Visible = True
                    tdbChoiceList.FillGridPath = frmLoadOutput.fillPath
                    tdbChoiceList.D8SlopePath = frmLoadOutput.sd8Path
                    tdbChoiceList.D8Path = frmLoadOutput.d8Path
                    tdbChoiceList.DInfPath = ""
                    tdbChoiceList.DInfSlopePath = ""
                End If
            Else
                lblPreOut.Visible = False
                tdbChoiceList.FillGridPath = ""
                tdbChoiceList.D8SlopePath = ""
                tdbChoiceList.D8Path = ""
                tdbChoiceList.DInfPath = ""
                tdbChoiceList.DInfSlopePath = ""
            End If
        End If
    End Sub

    Private Sub btnLoadDelin_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLoadDelin.Click
        If cmbxSelDem.SelectedIndex = 0 Then
            MsgBox("You need to select a DEM grid from the Base Elevation Grid drop-down list. If no layers are available to select, you can use the browse button beside the list to open an existing DEM.", MsgBoxStyle.OkOnly, "Automatic Watershed Delineation Error")
        Else
            frmLoadDelinOutput.ShowDialog()
            If frmLoadDelinOutput.ad8Path <> "" And frmLoadDelinOutput.gordPath <> "" And frmLoadDelinOutput.plenPath <> "" And frmLoadDelinOutput.tlenPath <> "" And frmLoadDelinOutput.srcPath <> "" And frmLoadDelinOutput.ordPath <> "" And frmLoadDelinOutput.coordPath <> "" And frmLoadDelinOutput.treePath <> "" And frmLoadDelinOutput.netPath <> "" And frmLoadDelinOutput.wPath <> "" Then
                If tdbChoiceList.useDinf And frmLoadDelinOutput.scaPath <> "" Then
                    lblDelinOut.Visible = True
                    tdbChoiceList.Ad8Path = frmLoadDelinOutput.ad8Path
                    tdbChoiceList.ScaPath = frmLoadDelinOutput.scaPath
                    tdbChoiceList.GordPath = frmLoadDelinOutput.gordPath
                    tdbChoiceList.PlenPath = frmLoadDelinOutput.plenPath
                    tdbChoiceList.TlenPath = frmLoadDelinOutput.tlenPath
                    tdbChoiceList.SrcPath = frmLoadDelinOutput.srcPath
                    tdbChoiceList.OrdPath = frmLoadDelinOutput.ordPath
                    tdbChoiceList.CoordPath = frmLoadDelinOutput.coordPath
                    tdbChoiceList.TreePath = frmLoadDelinOutput.treePath
                    tdbChoiceList.NetPath = frmLoadDelinOutput.netPath
                    tdbChoiceList.WPath = frmLoadDelinOutput.wPath
                Else
                    lblDelinOut.Visible = True
                    tdbChoiceList.Ad8Path = frmLoadDelinOutput.ad8Path
                    tdbChoiceList.ScaPath = ""
                    tdbChoiceList.GordPath = frmLoadDelinOutput.gordPath
                    tdbChoiceList.PlenPath = frmLoadDelinOutput.plenPath
                    tdbChoiceList.TlenPath = frmLoadDelinOutput.tlenPath
                    tdbChoiceList.SrcPath = frmLoadDelinOutput.srcPath
                    tdbChoiceList.OrdPath = frmLoadDelinOutput.ordPath
                    tdbChoiceList.CoordPath = frmLoadDelinOutput.coordPath
                    tdbChoiceList.TreePath = frmLoadDelinOutput.treePath
                    tdbChoiceList.NetPath = frmLoadDelinOutput.netPath
                    tdbChoiceList.WPath = frmLoadDelinOutput.wPath
                End If
            Else
                lblDelinOut.Visible = False
                tdbChoiceList.Ad8Path = ""
                tdbChoiceList.ScaPath = ""
                tdbChoiceList.GordPath = ""
                tdbChoiceList.PlenPath = ""
                tdbChoiceList.TlenPath = ""
                tdbChoiceList.SrcPath = ""
                tdbChoiceList.OrdPath = ""
                tdbChoiceList.CoordPath = ""
                tdbChoiceList.TreePath = ""
                tdbChoiceList.NetPath = ""
                tdbChoiceList.WPath = ""
            End If
        End If
    End Sub
#End Region

#Region "Hydro Functionality"
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: runPreprocessing
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: A function to run the preprocessing, mask, and pit fill steps
    '
    ' INPUTS: None
    '
    ' OUTPUTS: Boolean true if completed
    '
    ' NOTES: None
    '
    ' Change Log: 
    ' Date          Changed By      Notes
    ' 05/30/2006    ARA             Created
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Function runPreprocessing() As Boolean
        Logger.Dbg("RunPreprocessing:Entry")
        runFormInit()
        Logger.Dbg("RunFormIntComplete")
        lblPreproc.BringToFront()
        lblPreproc.Visible = True
        Me.Refresh()
        preProcHasRan = False
        Try
            If Not validatePreprocessing() Then
                Logger.Dbg("ValidateProcessingFailed")
                runFormCleanup()
                Logger.Dbg("RunFormCleanupComplete")
                Return False
            End If
            'myWrapper.Progress("Status", 0, "Preparing Grid")
            g_BaseDEM = getPathByName(cmbxSelDem.Items(cmbxSelDem.SelectedIndex))
            tdbFileList.formFileNames(g_BaseDEM, tdbChoiceList.OutputPath, True)
            g_Taudem.SetBASEDEM(tdbFileList.dem)

            If tdbChoiceList.FillGridPath <> "" And tdbChoiceList.D8SlopePath <> "" And tdbChoiceList.D8Path <> "" Then
                tdbFileList.fel = tdbChoiceList.FillGridPath
                tdbFileList.sd8 = tdbChoiceList.D8SlopePath
                tdbFileList.p = tdbChoiceList.D8Path
            Else
                If Not runMask() Then runFormCleanup() : Return False
                If Not runPitFill() Then runFormCleanup() : Return False
                If Not runD8() Then runFormCleanup() : Return False

                If tdbChoiceList.useDinf Then
                    If Not runDinf() Then runFormCleanup() : Return False
                End If
            End If
        Catch e As Exception
            runFormCleanup()
            preProcHasRan = False
            runPreprocessing = False
            Logger.Msg(e.Message & vbCrLf & e.StackTrace, MsgBoxStyle.OkOnly, "Automatic Watershed Delineation Error")
            Logger.Dbg("RunPreprocessing:ExitProblem")
            Return False
        End Try

        runFormCleanup()
        preProcHasRan = True
        runPreprocessing = True
        Logger.Dbg("RunPreprocessing:ExitOK")
    End Function

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: validatePreprocessing
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: A sub to check that the necessary files are selected if using the option
    '
    ' INPUTS: None
    '
    ' OUTPUTS: returns true if completed, false if not validated
    '
    ' NOTES: None
    '
    ' Change Log: 
    ' Date          Changed By      Notes
    ' 05/30/2006    ARA             Created
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Function validatePreprocessing() As Boolean
        Logger.Dbg("ValidatePreprocessing:Entry")
        If cmbxSelDem.SelectedIndex = 0 Then
            Logger.Msg("You need to select a DEM grid from the Base Elevation Grid drop-down list. If no layers are available to select, you can use the browse button beside the list to open an existing DEM.", MsgBoxStyle.OkOnly, "Automatic Watershed Delineation Error")
            Return False
        End If

        If chkbxBurnStream.Checked Then
            If cmbxStream.SelectedIndex = 0 Then
                Logger.Msg("You need to select a stream polyline to burn-in from the Burn-in drop-down list. If no layers are available, then you can use the browse button beside the list to open an existing polyline shapefile.", MsgBoxStyle.OkOnly, "Automatic Watershed Delineation Error")
                Return False
            End If
        End If

        If chkbxMask.Checked Then
            If Not rdobtnUseExtents.Checked Then
                If cmbxMask.SelectedIndex = 0 Then
                    Logger.Msg("You need to select a layer from the Mask drop-down list. If no layers are available, then you can use the browse button beside the list to open an existing polygon shapefile or focus mask grid.", MsgBoxStyle.OkOnly, "Automatic Watershed Delineation Error")
                    Return False
                Else
                    Dim strpath As String= getPathByName(cmbxMask.Items(cmbxMask.SelectedIndex))
                    If IO.Path.GetExtension(strpath) = ".shp" Then
                        If myWrapper.maskShapesIdx.Count = 0 Then
                            Logger.Msg("You must select at least one polygon from the mask using Select Mask.", MsgBoxStyle.OkOnly, "Automatic Watershed Delineation Error")
                            Return False
                        End If
                    End If
                End If
            End If
        End If
        Logger.Dbg("ValidatePreprocessing:Exit")
        Return True
    End Function

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: runDelinByThresh
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: A function to run the steps all the way to the stream shape.
    '  Stops there because steps may be rerun based on outlets
    '
    ' INPUTS: None
    '
    ' OUTPUTS: returns true if completed
    '
    ' NOTES: None
    '
    ' Change Log: 
    ' Date          Changed By      Notes
    ' 05/30/2006    ARA             Created
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Function runDelinByThresh() As Boolean
        Logger.Dbg("RunDelinByThresh:Entry")
        runFormInit()
        threshDelinHasRan = False
        snapHasRan = False
        Try
            If Not validatePreprocessing() Or Not validateDelinByThresh() Then
                runFormCleanup()
                Return False
            End If

            'If the preProcessing hasn't been done, run it first and only go on if it succeeded
            If Not preProcHasRan Then
                If Not runPreprocessing() Then
                    runFormCleanup()
                    Return False
                End If
            End If
            runFormInit()
            lblDelin.BringToFront()
            lblDelin.Visible = True
            Me.Refresh()
            'Setting outlets to false because the output of runDelinByThresh should always
            ' be the stream delineation without outlets. This lets the users see the stream
            ' delineated so they can place their points accurately. UseOutlets will be
            ' reset and the steps rerun in runOutletsAndFinish if the use outlets checkbox
            ' is checked.
            tdbChoiceList.useOutlets = False

            If tdbChoiceList.FillGridPath <> "" And tdbChoiceList.D8SlopePath <> "" And tdbChoiceList.D8Path <> "" Then
                MapWinGeoProc.DataManagement.DeleteShapefile(tdbFileList.net)
                MapWinGeoProc.DataManagement.CopyShapefile(tdbChoiceList.NetPath, tdbFileList.net)
            Else
                If Not runAreaD8() Then runFormCleanup() : Return False
                If tdbChoiceList.useDinf Then
                    If Not runAreaDinf() Then runFormCleanup() : Return False
                End If
                If Not runDefineStreamGrids() Then runFormCleanup() : Return False
                If Not runStreamShapeWshedGrid() Then runFormCleanup() : Return False

                If cmbxStream.SelectedIndex > 0 Then
                    'turn off the burn in layer to better view the result
                    g_MapWin.Layers.Item(getIndexByPath(getPathByName(cmbxStream.Items(cmbxStream.SelectedIndex)))).Visible = False
                End If
            End If
        Catch e As Exception
            runFormCleanup()
            threshDelinHasRan = False
            runDelinByThresh = False
            Logger.Msg(e.Message & vbCrLf & e.StackTrace, MsgBoxStyle.OkOnly, "Automatic Watershed Delineation Error")
        End Try

        runFormCleanup()
        threshDelinHasRan = True
        Logger.Dbg("RunDelinByThresh:Exit")
        Return True
    End Function

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: validateDelinByThresh
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: A sub to check that the threshold is validated
    '
    ' INPUTS: None
    '
    ' OUTPUTS: returns true if completed, false if not validated
    '
    ' NOTES: None
    '
    ' Change Log: 
    ' Date          Changed By      Notes
    ' 05/30/2006    ARA             Created
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Function validateDelinByThresh() As Boolean
        Logger.Dbg("ValidateDelinByThresh:Entry")
        Dim lValidateDelinByThresh As Boolean = validateCellThreshAndSet()
        Logger.Dbg("ValidateDelinByThresh:Exit:" & lValidateDelinByThresh)
        Return lValidateDelinByThresh
    End Function

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: runOutletsAndFinish
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: A sub to run the last steps needed, based on whether using outlets or
    '  not. If not using, it will only run the last steps. If outlets are used, it will
    '  autosnap the outlet points to the stream network generated before, then rerun
    '  all steps from the D8 accumulation in order to generate the outlet-defined network
    '  and watershed
    '
    ' INPUTS: None
    '
    ' OUTPUTS: returns true if completed
    '
    ' NOTES: None
    '
    ' Change Log: 
    ' Date          Changed By      Notes
    ' 05/30/2006    ARA             Created
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Function runOutletsAndFinish() As Boolean
        Logger.Dbg("RunOutletsAndFinish:Entry")
        runFormInit()
        runOutletsAndFinish = False
        Try
            If Not validatePreprocessing() Or Not validateDelinByThresh() Or Not validateOutlets() Then
                runFormCleanup()
                Return False
            End If

            'If the Delin portion hasn't been done, run it first and only go on if it succeeded
            If Not threshDelinHasRan Then
                If Not runDelinByThresh() Then
                    runFormCleanup()
                    'If Not stopClose Then
                    '    MsgBox("An error occured while delineating. Please check that your data is in the same projection and overlaps correctly and that your threshold was set to a valid value.")
                    'End If
                    Return False
                End If
            End If
            outletHasRan = False
            runFormInit()
            lblOutlets.BringToFront()
            lblOutlets.Visible = True
            Me.Refresh()
            'If using outlets, have to rerun many of the steps as outlets change the output of them
            If tdbChoiceList.useOutlets Then
                Logger.Dbg("  UsingOutlets")
                tdbFileList.outletshpfile = getPathByName(cmbxOutlets.Items.Item(cmbxOutlets.SelectedIndex))

                If Not runClipSelectedOutlets() Then runFormCleanup() : Return False
                If Not runAutoSnap() Then runFormCleanup() : Return False
                If Not runAreaD8() Then runFormCleanup() : Return False
                If tdbChoiceList.useDinf Then
                    If Not runAreaDinf() Then runFormCleanup() : Return False
                End If
                If Not runDefineStreamGrids() Then runFormCleanup() : Return False

                If Not runStreamShapeWshedGrid() Then runFormCleanup() : Return False
            Else
                If tdbChoiceList.FillGridPath <> "" And tdbChoiceList.D8SlopePath <> "" And tdbChoiceList.D8Path <> "" Then
                    tdbFileList.ad8 = tdbChoiceList.Ad8Path
                    tdbFileList.sca = tdbChoiceList.ScaPath
                    tdbFileList.gord = tdbChoiceList.GordPath
                    tdbFileList.plen = tdbChoiceList.PlenPath
                    tdbFileList.tlen = tdbChoiceList.TlenPath
                    tdbFileList.src = tdbChoiceList.SrcPath
                    tdbFileList.ord = tdbChoiceList.OrdPath
                    tdbFileList.coord = tdbChoiceList.CoordPath
                    tdbFileList.tree = tdbChoiceList.TreePath
                    tdbFileList.w = tdbChoiceList.WPath
                End If
            End If

            If Not runWshedToShape() Then runFormCleanup() : Return False
            If Not runApplyStreamAttributes() Then runFormCleanup() : Return False
            If Not runApplyWatershedAttributes() Then runFormCleanup() : Return False
            If Not runBuildJoinedBasins() Then runFormCleanup() : Return False
            If Not runApplyJoinBasinAttributes() Then runFormCleanup() : Return False
        Catch e As Exception
            runFormCleanup()
            outletHasRan = False
            Logger.Msg(e.Message & vbCrLf & e.StackTrace, MsgBoxStyle.OkOnly, "Automatic Watershed Delineation Error")
            Return False
        End Try
        runFormCleanup()
        outletHasRan = True
        Logger.Dbg("RunOutletsAndFinish:Exit")
        Return True
    End Function

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: validateOutlets
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: A sub to check that the outlets parameters are set correctly
    '
    ' INPUTS: None
    '
    ' OUTPUTS: returns true if completed, false if not validated
    '
    ' NOTES: None
    '
    ' Change Log: 
    ' Date          Changed By      Notes
    ' 05/30/2006    ARA             Created
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Function validateOutlets() As Boolean
        Logger.Dbg("ValidateOutlets:Entry")
        If chkbxUseOutlet.Checked Then
            If cmbxOutlets.SelectedIndex > 0 Then
                If myWrapper.outletShapesIdx.Count = 0 Then
                    Logger.Msg("There are no outlets/inlets currently selected. Please click the Select Outlets/Inlets button to select outlets and inlets.", MsgBoxStyle.OkOnly, "Automatic Watershed Delineation Error")
                    Return False
                End If
            Else
                Logger.Msg("There is no outlets/Inlets shapefile selected. Please select an outlets/inlets shapefile from the Outlets/Inlets drop down list or add a new one using the button beside it.", MsgBoxStyle.OkOnly, "Automatic Watershed Delineation Error")
                Return False
            End If
        End If
        Logger.Dbg("ValidateOutlets:Exit")
        Return True
    End Function

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: runAll
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: A sub executed by the run all button, which merely calls the 
    '  runOutletsAndFinish. 
    '
    ' INPUTS: None
    '
    ' OUTPUTS: boolean true if succeeds, false if not
    '
    ' NOTES: originally, there was going to be special handling for
    '  running all in regards to snapping, but it was made so it always snaps
    '  so this is semi redundanta
    '
    ' Change Log: 
    ' Date          Changed By      Notes
    ' 05/26/2006    ARA             Reset change logs for new version when copying over functionality
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Function runAll() As Boolean
        Logger.Dbg("RunAll:Entry")
        runningAll = True
        runAll = runOutletsAndFinish()
        runningAll = False
        Logger.Dbg("RunAll:Exit")
    End Function

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: runMask
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: A function to run the masking algorithms to limit delineation to
    '  the areas under masks
    '
    ' INPUTS: None
    '
    ' OUTPUTS: boolean true if succeeds, false if not
    '
    ' NOTES: None
    '
    ' Change Log: 
    ' Date          Changed By      Notes
    ' 05/28/2006    ARA             Created
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Function runMask() As Boolean
        Logger.Dbg("RunMask:Entry")

        runMask = False
        If doTicks Then
            tickb = Now().Ticks
        End If
        If tdbChoiceList.useMaskFileOrExtents Then
            Dim maskedPath, strmask As String
            If IO.Path.GetFileName(g_BaseDEM) = "sta.adf" Then
                maskedPath = tdbFileList.getAbsolutePath(tdbChoiceList.OutputPath, IO.Path.GetDirectoryName(g_BaseDEM) + ".bgd") + IO.Path.GetFileNameWithoutExtension(g_BaseDEM) + "_masked.bgd"
            Else
                maskedPath = tdbFileList.getAbsolutePath(tdbChoiceList.OutputPath, g_BaseDEM) + IO.Path.GetFileNameWithoutExtension(g_BaseDEM) + "_masked.bgd"
            End If
            RemoveLayer(maskedPath)

            If rdobtnUseExtents.Checked Then 'mask by extents
                If MapWinGeoProc.Hydrology.Mask(tdbFileList.dem, g_MapWin.View.Extents, maskedPath, Me) = 0 Then
                    tdbFileList.dem = maskedPath
                End If
            Else
                If cmbxMask.SelectedIndex > 0 Then
                    strmask = getPathByName(cmbxMask.Items(cmbxMask.SelectedIndex))
                    If IO.Path.GetExtension(strmask) = ".shp" Then 'Mask by selected shapes
                        If myWrapper.maskShapesIdx.Count > 0 Then
                            If MapWinGeoProc.Hydrology.Mask(tdbFileList.dem, strmask, myWrapper.maskShapesIdx, maskedPath, Me) = 0 Then
                                tdbFileList.dem = maskedPath
                            End If
                        End If
                    Else 'Mask by grid
                        If MapWinGeoProc.Hydrology.Mask(tdbFileList.dem, strmask, maskedPath, Me) = 0 Then
                            tdbFileList.dem = maskedPath
                        End If
                    End If
                End If
            End If
        End If
        If doTicks Then
            ticka = Now().Ticks
            tickd = ticka - tickb
            os.WriteLine(tickd.ToString + " - Mask ")
        End If
        Return True
    End Function

    Private Function runBurn(ByVal strDEM As String) As String
        Logger.Dbg("RunBurn:EntryWithFile:")
        Logger.Dbg("  DEM:" & strDEM)
        Dim strBurn, strBurnResult As String
        runBurn = strDEM
        If tdbChoiceList.useBurnIn Then
            strBurn = getPathByName(cmbxStream.Items.Item(cmbxStream.SelectedIndex))
            If IO.Path.GetFileName(g_BaseDEM) = "sta.adf" Then
                strBurnResult = tdbFileList.getAbsolutePath(tdbChoiceList.OutputPath, IO.Path.GetDirectoryName(g_BaseDEM) + ".bgd") + IO.Path.GetFileNameWithoutExtension(g_BaseDEM) + "_burn.bgd"
            Else
                strBurnResult = tdbFileList.getAbsolutePath(tdbChoiceList.OutputPath, g_BaseDEM) + IO.Path.GetFileNameWithoutExtension(g_BaseDEM) + "_burn.bgd"
            End If
            Logger.Dbg("  Burn:" & strBurn)
            If MapWinGeoProc.Hydrology.CanyonBurnin(strBurn, strDEM, strBurnResult, Me) = 0 Then
                runBurn = strBurnResult
                Logger.Dbg("RunBurn:Result:" & runBurn)
            Else
                Logger.Msg("An error occured while burning in the stream polyline.", MsgBoxStyle.OkOnly, "Automatic Watershed Delineation Error")
            End If
        End If
    End Function

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: runPitFill
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: A function split off from Begin to make TauDem calls
    '
    ' INPUTS: None
    '
    ' OUTPUTS: boolean true if succeeds, false if not
    '
    ' NOTES: None
    '
    ' Change Log: 
    ' Date          Changed By      Notes
    ' 05/26/2006    ARA             Reset change logs for new version when copying over functionality
    ' 07/17/2006    ARA             Changed to use the new fill method
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Function runPitFill() As Boolean
        Logger.Dbg("RunPitFill:Entry")
        Dim usefdrfile As Long = 0
        Dim inRam As Boolean = True
        Dim strToFill As String
        Dim burnFirst As Boolean

        If tdbChoiceList.usefdrfile Then usefdrfile = 1
        If tdbChoiceList.DiskBased = True Then inRam = False
        runPitFill = False

        RemoveLayer(tdbFileList.fel, False)

        If doTicks Then
            tickb = Now().Ticks
        End If

        burnFirst = True
        strToFill = tdbFileList.dem
        If burnFirst Then
            strToFill = runBurn(tdbFileList.dem)
            If strToFill = "" Then
                'myWrapper.Progress("Status", 0, "")
                Cursor = Windows.Forms.Cursors.Default
                Return False
            End If
        End If

        'myWrapper.Progress("Status", 0, "Pit Fill")
        Try
            MapWinGeoProc.Hydrology.Fill(strToFill, tdbFileList.fel, Me)
        Catch ex As Exception
            Logger.Msg("An error occured while filling the grid: " + ex.Message, MsgBoxStyle.OkOnly, "Automatic Watershed Delineation Error")
            'myWrapper.Progress("Status", 0, "")
            Cursor = Windows.Forms.Cursors.Default
            Return False
        End Try

        If Not burnFirst Then
            tdbFileList.fel = runBurn(tdbFileList.fel)
            If tdbFileList.fel = "" Then
                'myWrapper.Progress("Status", 0, "")
                Cursor = Windows.Forms.Cursors.Default
                Return False
            End If
        End If

        If doTicks Then
            ticka = Now().Ticks
            tickd = ticka - tickb
            os.WriteLine(tickd.ToString + " - Pit Fill ")
        End If

        If tdbChoiceList.AddPitfillLayer Then
            AddMap(tdbFileList.fel, LayerType.fel)
        End If

        Logger.Dbg("RunPitFill:Exit")
        Return True
    End Function

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: runD8
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: A function split off from Begin to make TauDem calls
    '
    ' INPUTS: None
    '
    ' OUTPUTS: boolean true if succeeds, false if not
    '
    ' NOTES: None
    '
    ' Change Log: 
    ' Date          Changed By      Notes
    ' 05/26/2006    ARA             Reset change logs for new version when copying over functionality
    ' 07/18/2006    ARA             Changed to call geoproc.hydrology instead of taudem
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Function runD8() As Boolean
        Logger.Dbg("RunD8:EntryWithFiles:" & vbCrLf & _
                   tdbFileList.fel & vbCrLf & _
                   tdbFileList.p & vbCrLf & _
                   tdbFileList.sd8)

        RemoveLayer(tdbFileList.sd8, False)
        RemoveLayer(tdbFileList.p, False)

        If doTicks Then
            tickb = Now().Ticks
        End If

        Dim lResult As Integer = MapWinGeoProc.Hydrology.D8(tdbFileList.fel, tdbFileList.p, tdbFileList.sd8, Nothing)
        If lResult <> 0 Then
            Logger.Dbg("RunD8:ExitWithErrorCode " & lResult)
            Return False
        Else
            If doTicks Then
                ticka = Now().Ticks
                tickd = ticka - tickb
                os.WriteLine(tickd.ToString + " - D8 ")
            End If
            If tdbChoiceList.AddD8Layer And lResult = 0 Then
                AddMap(tdbFileList.sd8, LayerType.sd8)
                AddMap(tdbFileList.p, LayerType.p)
            End If
            Logger.Dbg("RunD8:Exit")
            Return True
        End If
    End Function

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: runAreaD8
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: A function split off from Begin to make TauDem calls
    '
    ' INPUTS: None
    '
    ' OUTPUTS: boolean true if succeeds, false if not
    '
    ' NOTES: None
    '
    ' Change Log: 
    ' Date          Changed By      Notes
    ' 05/26/2006    ARA             Reset change logs for new version when copying over functionality
    ' 07/19/2006    ARA             Changed to call geoproc.hydrology instead of taudem
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Function runAreaD8() As Boolean
        Logger.Dbg("RunAreaD8:Entry")

        RemoveLayer(tdbFileList.ad8, False)
        If doTicks Then
            tickb = Now().Ticks
        End If
        Progress("Message", 0, "Area D8")
        Dim lResult As Integer= MapWinGeoProc.Hydrology.AreaD8(tdbFileList.p, tdbFileList.outletshpfile, tdbFileList.ad8, tdbChoiceList.useOutlets, tdbChoiceList.EdgeContCheck, Nothing)
        If lResult <> 0 Then
            Logger.Dbg("RunAreaD8:ExitWithErrorCode " & lResult)
            Return False
        Else
            If doTicks Then
                ticka = Now().Ticks
                tickd = ticka - tickb
                os.WriteLine(tickd.ToString + " - Area D8 ")
            End If
            If tdbChoiceList.AddD8AreaLayer Then
                AddMap(tdbFileList.ad8, LayerType.ad8)
            End If
            Logger.Dbg("RunAreaD8:Exit")
            Return True
        End If
    End Function

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: runDinf
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: A function to run Dinf
    '
    ' INPUTS: None
    '
    ' OUTPUTS: boolean true if succeeds, false if not
    '
    ' NOTES: None
    '
    ' Change Log: 
    ' Date          Changed By      Notes
    ' 08/23/2006    ARA             Created
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Function runDinf() As Boolean
        Logger.Dbg("RunDinf:Entry")

        runDinf = False

        RemoveLayer(tdbFileList.slp, False)
        RemoveLayer(tdbFileList.ang, False)

        If doTicks Then
            tickb = Now().Ticks
        End If

        Dim lResult As Integer = MapWinGeoProc.Hydrology.DInf(tdbFileList.fel, tdbFileList.ang, tdbFileList.slp, myWrapper)
        If lResult <> 0 Then
            Logger.Dbg("RunDinf:ExitWithErrorCode " & lResult)
            Return False
        Else
            If doTicks Then
                ticka = Now().Ticks
                tickd = ticka - tickb
                os.WriteLine(tickd.ToString + " - Dinf ")
            End If
            If tdbChoiceList.AddD8Layer Then
                AddMap(tdbFileList.slp, LayerType.slp)
                AddMap(tdbFileList.ang, LayerType.ang)
            End If

            Logger.Dbg("RunDinf:Exit")
            Return True
        End If
    End Function

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: runAreaDinf
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: A function to run AreaDinf
    '
    ' INPUTS: None
    '
    ' OUTPUTS: boolean true if succeeds, false if not
    '
    ' NOTES: None
    '
    ' Change Log: 
    ' Date          Changed By      Notes
    ' 08/23/2006    ARA             Created
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Function runAreaDinf() As Boolean
        Logger.Dbg("RunAreaDinf:Entry")


        RemoveLayer(tdbFileList.sca, False)

        If doTicks Then
            tickb = Now().Ticks
        End If

        Dim lResult As Integer = MapWinGeoProc.Hydrology.AreaDInf(tdbFileList.ang, tdbFileList.outletshpfile, tdbFileList.sca, tdbChoiceList.useOutlets, tdbChoiceList.EdgeContCheck, myWrapper)
        If lResult <> 0 Then
            Logger.Dbg("RunAreaDinf:ExitWithErrorCode " & lResult)
            Return False
        Else
            If doTicks Then
                ticka = Now().Ticks
                tickd = ticka - tickb
                os.WriteLine(tickd.ToString + " - Area Dinf ")
            End If
            If tdbChoiceList.AddD8AreaLayer Then
                AddMap(tdbFileList.sca, LayerType.sca)
            End If

            Logger.Dbg("RunAreaDinf:Exit")
            Return True
        End If
    End Function

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: runClipSelectedOutlets
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: Will clip the selected outlets into a new shape file if the
    '  number selected isn't already equal to the total number in the file (which
    '  is the default state). This prevents running this unnecessarily.
    '
    ' INPUTS: None
    '
    ' OUTPUTS: Returns true on completion.
    '
    ' NOTES: Will create a clip file named as the point path with _clip.shp at end
    '
    ' Change Log: 
    ' Date          Changed By      Notes
    ' 06/18/2006    ARA             Created
    ' 08/08/2006    ARA             Updated to copy attributes when 'clipping'
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Function runClipSelectedOutlets() As Boolean
        Logger.Dbg("RunClipSelectedOutlets:Entry")

        Dim strClipShapePath As String

        If IO.Path.GetFileName(g_BaseDEM) = "sta.adf" Then
            strClipShapePath = tdbFileList.getAbsolutePath(tdbChoiceList.OutputPath, IO.Path.GetDirectoryName(g_BaseDEM) + ".bgd") + IO.Path.GetFileNameWithoutExtension(g_BaseDEM) + "_clip.shp"
        Else
            strClipShapePath = tdbFileList.getAbsolutePath(tdbChoiceList.OutputPath, g_BaseDEM) + IO.Path.GetFileNameWithoutExtension(g_BaseDEM) + "_clip.shp"
        End If

        RemoveLayer(strClipShapePath)

        If MapWinGeoProc.Utils.ExtractSelectedPoints(tdbFileList.outletshpfile, strClipShapePath, myWrapper.outletShapesIdx, Me) Then
            tdbFileList.outletshpfile = strClipShapePath
        End If

        Logger.Dbg("RunClipSelectedOutlets:Exit")
        Return True
    End Function

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: runAutoSnap
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: A function which will cycle a set of given points and
    '  snap them to the nearest position on the given polyline file if they
    '  meet the criteria of being under the tdbChoiceList.snapthresh distance 
    '  from the segment and if the new position isn't within 100 m or ft of 
    '  another snapped point (to prevent a nasty runtime error which is triggered
    '  by taudem.
    '
    ' INPUTS: strPolylinePath: String path to the polyline to snap to
    '         strPointPath: String path to the points file to snap
    '
    ' OUTPUTS: Returns true on completion.
    '
    ' NOTES: Will create a snap file named as the point path with _snap.shp at end
    '
    ' Change Log: 
    ' Date          Changed By      Notes
    ' 05/30/2006    ARA             Created
    ' 08/07/2006    ARA             Moved code out to geoproc.utils.SnapPointsToLines
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Function runAutoSnap() As Boolean
        Logger.Dbg("RunAutoSnap:Entry")

        Dim newPointPath As String
        If IO.Path.GetFileName(g_BaseDEM) = "sta.adf" Then
            newPointPath = tdbFileList.getAbsolutePath(tdbChoiceList.OutputPath, IO.Path.GetDirectoryName(g_BaseDEM) + ".bgd") + IO.Path.GetFileNameWithoutExtension(g_BaseDEM) + "_snap.shp"
        Else
            newPointPath = tdbFileList.getAbsolutePath(tdbChoiceList.OutputPath, g_BaseDEM) + IO.Path.GetFileNameWithoutExtension(g_BaseDEM) + "_snap.shp"
        End If

        RemoveLayer(newPointPath)
        Dim g As New MapWinGIS.Grid
        g.Open(tdbFileList.p)
        Dim dx As Double = g.Header.dX
        g.Close()
        runAutoSnap = MapWinGeoProc.Utils.SnapPointsToLines(tdbFileList.outletshpfile, tdbFileList.net, tdbChoiceList.snapThresh, dx / 2, newPointPath, True, Me)

        If runAutoSnap Then
            tdbFileList.outletshpfile = newPointPath
        End If
        Logger.Dbg("RunAutoSnap:Exit")
    End Function

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: runDefineStreamGrids
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: A function which calls the geoproc.hydrology function to
    '  generate stream network associated grids 
    '
    ' INPUTS: None
    '
    ' OUTPUTS: boolean true if succeeds, false if not
    '
    ' NOTES: None
    '
    ' Change Log: 
    ' Date          Changed By      Notes
    ' 07/20/2006    ARA             Created
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Function runDefineStreamGrids() As Boolean
        Logger.Dbg("RunDefineStreamGrids:Entry")

        runDefineStreamGrids = False
        If Not RemoveLayer(tdbFileList.gord, False) Then Logger.Dbg("ProblemRemovingLayer " & tdbFileList.gord)
        RemoveLayer(tdbFileList.plen, False)
        RemoveLayer(tdbFileList.tlen, False)
        RemoveLayer(tdbFileList.src, False)
        RemoveLayer(tdbFileList.ord, False)

        If doTicks Then
            tickb = Now().Ticks
        End If

        Dim lResult As Integer = MapWinGeoProc.Hydrology.DelinStreamGrids(tdbFileList.dem, tdbFileList.fel, tdbFileList.p, tdbFileList.sd8, tdbFileList.ad8, tdbFileList.ang, tdbFileList.outletshpfile, tdbFileList.gord, tdbFileList.plen, tdbFileList.tlen, tdbFileList.src, tdbFileList.ord, tdbFileList.tree, tdbFileList.coord, tdbChoiceList.Threshold, tdbChoiceList.useOutlets, tdbChoiceList.EdgeContCheck, tdbChoiceList.useDinf, Nothing)
        Logger.Dbg("RunDefineStreamGrids:BackFrom:DelinStreamGrids")
        If lResult <> 0 Then Return False

        If doTicks Then
            ticka = Now().Ticks
            tickd = ticka - tickb
            os.WriteLine(tickd.ToString + " - Source Definition ")
        End If
        If tdbChoiceList.AddGridNetLayer And lResult = 0 Then
            AddMap(tdbFileList.gord, LayerType.gord)
            AddMap(tdbFileList.plen, LayerType.plen)
            AddMap(tdbFileList.tlen, LayerType.tlen)
        End If
        If tdbChoiceList.AddRiverRasterLayer Then
            AddMap(tdbFileList.src, LayerType.src)
        End If
        If tdbChoiceList.AddOrderGridLayer Then
            AddMap(tdbFileList.ord, LayerType.ord)
        End If

        Logger.Dbg("RunDefineStreamGrids:Exit")
        Return True
    End Function

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: runStreamShapeWshedGrid
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: A function split off from Begin to make TauDem calls
    '
    ' INPUTS: None
    '
    ' OUTPUTS: boolean true if succeeds, false if not
    '
    ' NOTES: None
    '
    ' Change Log: 
    ' Date          Changed By      Notes
    ' 05/26/2006    ARA             Reset change logs for new version when copying over functionality
    ' 07/20/2006    ARA             Updated to call geoproc.hydrology instead of taudem
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Function runStreamShapeWshedGrid() As Boolean
        Logger.Dbg("RunStreamShapeWshedGrid:Entry")

        Dim i As Integer
        runStreamShapeWshedGrid = False

        RemoveLayer(tdbFileList.w, False)
        RemoveLayer(tdbFileList.net, False)

        If doTicks Then
            tickb = Now().Ticks
        End If

        i = MapWinGeoProc.Hydrology.DelinStreamsAndSubBasins(tdbFileList.p, tdbFileList.tree, tdbFileList.coord, tdbFileList.net, tdbFileList.w, Nothing)
        If i <> 0 Then Return False

        If doTicks Then
            ticka = Now().Ticks
            tickd = ticka - tickb
            os.WriteLine(tickd.ToString + " - Stream Shape and Watershed Grid ")
        End If
        If tdbChoiceList.AddWShedGridLayer Then
            AddMap(tdbFileList.w, LayerType.w)
        End If
        If tdbChoiceList.AddStreamShapeLayer Then
            AddMap(tdbFileList.net, LayerType.ReachShapefile) '15 is a reach shapefile
        End If
        Logger.Dbg("RunStreamShapeWshedGrid:Exit")
        Return True
    End Function

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: runWshedToShape
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: A function split off from Begin to make TauDem calls
    '
    ' INPUTS: None
    '
    ' OUTPUTS: boolean true if successful, false if failed
    '
    ' NOTES: None
    '
    ' Change Log: 
    ' Date          Changed By      Notes
    ' 05/26/2006    ARA             Reset change logs for new version when copying over functionality
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Function runWshedToShape() As Boolean
        Logger.Dbg("RunWshedToShape:Entry")

        If doTicks Then
            tickb = Now().Ticks
        End If

        runWshedToShape = False
        RemoveLayer(tdbFileList.wshed, False)

        Dim i As Integer = MapWinGeoProc.Hydrology.SubbasinsToShape(tdbFileList.p, tdbFileList.w, tdbFileList.wshed, Me)
        If i <> 0 Then Return False

        If doTicks Then
            ticka = Now().Ticks
            tickd = ticka - tickb
            os.WriteLine(tickd.ToString + " - Watershed to Shapefile ")
        End If

        Logger.Dbg("RunWshedToShape:Exit")
        Return True
    End Function

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: ApplyStreamAttributes
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: A subroutine to calculate and apply the new stream net
    '   attributes
    '
    ' INPUTS: strStreamFile : A string to the filename for the stream net
    '                         shapefile
    '         strDemFile : A string to the filename of the dem grid
    '
    ' OUTPUTS: Boolean true on completion
    '
    ' NOTES: None
    '
    ' Change Log: 
    ' Date          Changed By      Notes
    ' 05/26/2006    ARA             Reset change logs for new version when copying over functionality
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Function runApplyStreamAttributes() As Boolean
        Logger.Dbg("RunApplyStreamAttributes:Entry")

        runApplyStreamAttributes = False
        RemoveLayer(tdbFileList.net, False)
        If doTicks Then
            tickb = Now().Ticks
        End If
        'myWrapper.Progress("Status", 0, "Calculating Stream Parameters")
        If tdbChoiceList.CalcSpecialStreamFields Then
            runApplyStreamAttributes = MapWinGeoProc.Hydrology.ApplyStreamAttributes(tdbFileList.net, tdbFileList.dem, tdbFileList.wshed, cmbxElevUnits.SelectedIndex, Nothing)
        Else
            runApplyStreamAttributes = True
        End If

        If doTicks Then
            ticka = Now().Ticks
            tickd = ticka - tickb
            os.WriteLine(tickd.ToString + " - Stream Special Attributes ")
            tickb = Now().Ticks
        End If

        'End If
        '
        ' Add after Applying the attributes so their attr tables will be up to date
        '
        If tdbChoiceList.AddStreamShapeLayer Then
            AddMap(tdbFileList.net, LayerType.ReachShapefile) '15 is a reach shapefile
        End If
        Progress("", 0, "")
        Logger.Dbg("RunApplyStreamAttributes:Exit")
    End Function

    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' TITLE: ApplyWatershedAttributes
    ' AUTHOR: Allen Richard Anselmo
    ' DESCRIPTION: A subroutine to calculate and apply the new watershed
    '   attributes
    '
    ' INPUTS: strShedFile : A string to the filename for the final shed
    '                        shed shapefile
    '         strSlopeFile : A string to the filename for the slope grid
    '         strStreamFile : A string to the filename for the stream net
    '                         shapefile
    '
    ' OUTPUTS: boolean true on completion
    '
    ' NOTES: None
    '
    ' Change Log: 
    ' Date          Changed By      Notes
    ' 05/26/2006    ARA             Reset change logs for new version when copying over functionality
    ' 02/20/2006    CM              Added precision and moved begineditings
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Private Function runApplyWatershedAttributes() As Boolean
        Logger.Dbg("RunApplyWatershedAttributes:Entry")

        runApplyWatershedAttributes = False
        If doTicks Then
            tickb = Now().Ticks
        End If

        runApplyWatershedAttributes = MapWinGeoProc.Hydrology.ApplyWatershedLinkAttributes(tdbFileList.wshed, tdbFileList.net, Me)

        If tdbChoiceList.CalcSpecialWshedFields Then
            runApplyWatershedAttributes = MapWinGeoProc.Hydrology.ApplyWatershedAreaAttributes(tdbFileList.wshed, Me)
            runApplyWatershedAttributes = MapWinGeoProc.Hydrology.ApplyWatershedSlopeAttribute(tdbFileList.w, tdbFileList.wshed, tdbFileList.sd8, cmbxElevUnits.SelectedIndex, Me)
        Else
            runApplyWatershedAttributes = True
        End If

        If doTicks Then
            ticka = Now().Ticks
            tickd = ticka - tickb
            os.WriteLine(tickd.ToString + " - Watershed Special Attributes ")
            os.Close()
        End If

        If tdbChoiceList.AddWShedShapeLayer Then
            AddMap(tdbFileList.wshed, LayerType.WatershedShapefile)
        End If
        Logger.Dbg("RunApplyWatershedAttributes:Exit")
    End Function

    Private Function runBuildJoinedBasins() As Boolean
        If doTicks Then
            tickb = Now().Ticks
        End If

        RemoveLayer(tdbFileList.mergewshed)
        runBuildJoinedBasins = MapWinGeoProc.Hydrology.BuildJoinedBasins(tdbFileList.wshed, tdbFileList.outletshpfile, tdbFileList.mergewshed, Me)

        If doTicks Then
            ticka = Now().Ticks
            tickd = ticka - tickb
            os.WriteLine(tickd.ToString + " - Watershed to Joined Shed ")
        End If
    End Function

    Private Function runApplyJoinBasinAttributes() As Boolean
        runApplyJoinBasinAttributes = False

        If tdbChoiceList.calcSpecialMergeWshedFields Then
            runApplyJoinBasinAttributes = MapWinGeoProc.Hydrology.ApplyJoinBasinAreaAttributes(tdbFileList.mergewshed, cmbxElevUnits.SelectedIndex, Me)
            runApplyJoinBasinAttributes = MapWinGeoProc.Hydrology.ApplyWatershedSlopeAttribute(tdbFileList.w, tdbFileList.mergewshed, tdbFileList.sd8, cmbxElevUnits.SelectedIndex, Me)
            runApplyJoinBasinAttributes = MapWinGeoProc.Hydrology.ApplyWatershedElevationAttribute(tdbFileList.w, tdbFileList.mergewshed, tdbFileList.fel, Me)
            runApplyJoinBasinAttributes = MapWinGeoProc.Hydrology.ApplyJoinBasinStreamAttributes(tdbFileList.net, tdbFileList.w, tdbFileList.mergewshed, Me)
        Else
            runApplyJoinBasinAttributes = True
        End If

        If tdbChoiceList.AddMergedWShedShapeLayer Then
            AddMap(tdbFileList.mergewshed, LayerType.mergeWShed)
        End If
    End Function
#End Region

End Class
