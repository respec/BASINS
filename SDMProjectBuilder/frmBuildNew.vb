Imports atcUtility
Imports MapWinUtility

Public Class frmBuildNew
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        txtInstructions.Text = "To Build a New " & g_AppNameShort & " Project, " & _
           "zoom/pan to your geographic area of interest, select (highlight) it, " & _
           "and then click 'Build'.  " '& _
        '"If your area is outside the US, then click 'Build' " & _
        '"with no features selected to create an international project."

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
    Friend WithEvents btnBuild As System.Windows.Forms.Button
    Friend WithEvents txtInstructions As System.Windows.Forms.TextBox
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents chkHSPF As System.Windows.Forms.CheckBox
    Friend WithEvents chkSWAT As System.Windows.Forms.CheckBox
    Friend WithEvents atxSize As atcControls.atcText
    Friend WithEvents lblSize As System.Windows.Forms.Label
    Friend WithEvents lblLength As System.Windows.Forms.Label
    Friend WithEvents atxLength As atcControls.atcText
    Friend WithEvents lblLU As System.Windows.Forms.Label
    Friend WithEvents atxLU As atcControls.atcText
    Friend WithEvents cmdSet As System.Windows.Forms.Button
    Friend WithEvents lblSWAT As System.Windows.Forms.Label
    Friend WithEvents lblFile As System.Windows.Forms.Label
    Friend WithEvents lblSimulationStartYear As System.Windows.Forms.Label
    Friend WithEvents txtSimulationStartYear As atcControls.atcText
    Friend WithEvents lblSimulationEndYear As System.Windows.Forms.Label
    Friend WithEvents txtSimulationEndYear As atcControls.atcText
    Friend WithEvents lblProjectSize As System.Windows.Forms.Label
    Friend WithEvents btnFind As System.Windows.Forms.Button
    Friend WithEvents txtFind As System.Windows.Forms.TextBox
    Friend WithEvents rdoHUC8 As System.Windows.Forms.RadioButton
    Friend WithEvents rdoHUC12 As System.Windows.Forms.RadioButton
    Friend WithEvents tabsAreaSelection As System.Windows.Forms.TabControl
    Friend WithEvents tabSelectOnMap As System.Windows.Forms.TabPage
    Friend WithEvents tabSelectList As System.Windows.Forms.TabPage
    Friend WithEvents txtHucList As System.Windows.Forms.TextBox
    Friend WithEvents lblHucList As System.Windows.Forms.Label
    Friend WithEvents txtSelected As System.Windows.Forms.TextBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.btnBuild = New System.Windows.Forms.Button
        Me.txtInstructions = New System.Windows.Forms.TextBox
        Me.txtSelected = New System.Windows.Forms.TextBox
        Me.btnCancel = New System.Windows.Forms.Button
        Me.chkHSPF = New System.Windows.Forms.CheckBox
        Me.chkSWAT = New System.Windows.Forms.CheckBox
        Me.lblSize = New System.Windows.Forms.Label
        Me.lblLength = New System.Windows.Forms.Label
        Me.lblLU = New System.Windows.Forms.Label
        Me.cmdSet = New System.Windows.Forms.Button
        Me.lblSWAT = New System.Windows.Forms.Label
        Me.lblFile = New System.Windows.Forms.Label
        Me.lblSimulationStartYear = New System.Windows.Forms.Label
        Me.lblSimulationEndYear = New System.Windows.Forms.Label
        Me.lblProjectSize = New System.Windows.Forms.Label
        Me.btnFind = New System.Windows.Forms.Button
        Me.txtFind = New System.Windows.Forms.TextBox
        Me.rdoHUC8 = New System.Windows.Forms.RadioButton
        Me.rdoHUC12 = New System.Windows.Forms.RadioButton
        Me.tabsAreaSelection = New System.Windows.Forms.TabControl
        Me.tabSelectOnMap = New System.Windows.Forms.TabPage
        Me.tabSelectList = New System.Windows.Forms.TabPage
        Me.lblHucList = New System.Windows.Forms.Label
        Me.txtHucList = New System.Windows.Forms.TextBox
        Me.txtSimulationEndYear = New atcControls.atcText
        Me.txtSimulationStartYear = New atcControls.atcText
        Me.atxLU = New atcControls.atcText
        Me.atxLength = New atcControls.atcText
        Me.atxSize = New atcControls.atcText
        Me.tabsAreaSelection.SuspendLayout()
        Me.tabSelectOnMap.SuspendLayout()
        Me.tabSelectList.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnBuild
        '
        Me.btnBuild.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnBuild.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnBuild.Location = New System.Drawing.Point(354, 374)
        Me.btnBuild.Name = "btnBuild"
        Me.btnBuild.Size = New System.Drawing.Size(80, 28)
        Me.btnBuild.TabIndex = 1
        Me.btnBuild.Text = "Build"
        '
        'txtInstructions
        '
        Me.txtInstructions.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtInstructions.BackColor = System.Drawing.SystemColors.Control
        Me.txtInstructions.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtInstructions.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInstructions.Location = New System.Drawing.Point(3, 6)
        Me.txtInstructions.Multiline = True
        Me.txtInstructions.Name = "txtInstructions"
        Me.txtInstructions.Size = New System.Drawing.Size(494, 28)
        Me.txtInstructions.TabIndex = 2
        Me.txtInstructions.TabStop = False
        '
        'txtSelected
        '
        Me.txtSelected.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSelected.BackColor = System.Drawing.SystemColors.Menu
        Me.txtSelected.Location = New System.Drawing.Point(0, 67)
        Me.txtSelected.Multiline = True
        Me.txtSelected.Name = "txtSelected"
        Me.txtSelected.Size = New System.Drawing.Size(500, 89)
        Me.txtSelected.TabIndex = 3
        Me.txtSelected.TabStop = False
        Me.txtSelected.Text = "Selected:"
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.Location = New System.Drawing.Point(440, 374)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(80, 28)
        Me.btnCancel.TabIndex = 4
        Me.btnCancel.Text = "Cancel"
        '
        'chkHSPF
        '
        Me.chkHSPF.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkHSPF.AutoSize = True
        Me.chkHSPF.Checked = True
        Me.chkHSPF.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkHSPF.Location = New System.Drawing.Point(12, 200)
        Me.chkHSPF.Name = "chkHSPF"
        Me.chkHSPF.Size = New System.Drawing.Size(54, 17)
        Me.chkHSPF.TabIndex = 5
        Me.chkHSPF.Text = "HSPF"
        Me.chkHSPF.UseVisualStyleBackColor = True
        '
        'chkSWAT
        '
        Me.chkSWAT.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkSWAT.AutoSize = True
        Me.chkSWAT.Checked = True
        Me.chkSWAT.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkSWAT.Location = New System.Drawing.Point(72, 200)
        Me.chkSWAT.Name = "chkSWAT"
        Me.chkSWAT.Size = New System.Drawing.Size(58, 17)
        Me.chkSWAT.TabIndex = 6
        Me.chkSWAT.Text = "SWAT"
        Me.chkSWAT.UseVisualStyleBackColor = True
        '
        'lblSize
        '
        Me.lblSize.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblSize.AutoSize = True
        Me.lblSize.Location = New System.Drawing.Point(69, 229)
        Me.lblSize.Name = "lblSize"
        Me.lblSize.Size = New System.Drawing.Size(216, 13)
        Me.lblSize.TabIndex = 8
        Me.lblSize.Text = "Minimum Catchment Size (square kilometers)"
        '
        'lblLength
        '
        Me.lblLength.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblLength.AutoSize = True
        Me.lblLength.Location = New System.Drawing.Point(69, 253)
        Me.lblLength.Name = "lblLength"
        Me.lblLength.Size = New System.Drawing.Size(181, 13)
        Me.lblLength.TabIndex = 10
        Me.lblLength.Text = "Minimum Flowline Length (kilometers)"
        '
        'lblLU
        '
        Me.lblLU.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblLU.AutoSize = True
        Me.lblLU.Location = New System.Drawing.Point(69, 277)
        Me.lblLU.Name = "lblLU"
        Me.lblLU.Size = New System.Drawing.Size(184, 13)
        Me.lblLU.TabIndex = 12
        Me.lblLU.Text = "Ignore Landuse Areas Below Fraction"
        '
        'cmdSet
        '
        Me.cmdSet.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdSet.Location = New System.Drawing.Point(491, 348)
        Me.cmdSet.Name = "cmdSet"
        Me.cmdSet.Size = New System.Drawing.Size(29, 20)
        Me.cmdSet.TabIndex = 13
        Me.cmdSet.Text = "..."
        Me.cmdSet.UseVisualStyleBackColor = True
        '
        'lblSWAT
        '
        Me.lblSWAT.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblSWAT.AutoSize = True
        Me.lblSWAT.Location = New System.Drawing.Point(12, 353)
        Me.lblSWAT.Name = "lblSWAT"
        Me.lblSWAT.Size = New System.Drawing.Size(107, 13)
        Me.lblSWAT.TabIndex = 14
        Me.lblSWAT.Text = "SWAT Database File"
        '
        'lblFile
        '
        Me.lblFile.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblFile.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblFile.Location = New System.Drawing.Point(125, 351)
        Me.lblFile.Name = "lblFile"
        Me.lblFile.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblFile.Size = New System.Drawing.Size(360, 15)
        Me.lblFile.TabIndex = 15
        Me.lblFile.Text = "SWAT2005.mdb"
        Me.lblFile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblSimulationStartYear
        '
        Me.lblSimulationStartYear.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblSimulationStartYear.AutoSize = True
        Me.lblSimulationStartYear.Location = New System.Drawing.Point(69, 301)
        Me.lblSimulationStartYear.Name = "lblSimulationStartYear"
        Me.lblSimulationStartYear.Size = New System.Drawing.Size(105, 13)
        Me.lblSimulationStartYear.TabIndex = 17
        Me.lblSimulationStartYear.Text = "Simulation Start Year"
        '
        'lblSimulationEndYear
        '
        Me.lblSimulationEndYear.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblSimulationEndYear.AutoSize = True
        Me.lblSimulationEndYear.Location = New System.Drawing.Point(69, 325)
        Me.lblSimulationEndYear.Name = "lblSimulationEndYear"
        Me.lblSimulationEndYear.Size = New System.Drawing.Size(102, 13)
        Me.lblSimulationEndYear.TabIndex = 19
        Me.lblSimulationEndYear.Text = "Simulation End Year"
        '
        'lblProjectSize
        '
        Me.lblProjectSize.AutoSize = True
        Me.lblProjectSize.Location = New System.Drawing.Point(6, 42)
        Me.lblProjectSize.Name = "lblProjectSize"
        Me.lblProjectSize.Size = New System.Drawing.Size(66, 13)
        Me.lblProjectSize.TabIndex = 21
        Me.lblProjectSize.Text = "Project Size:"
        '
        'btnFind
        '
        Me.btnFind.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnFind.Location = New System.Drawing.Point(422, 40)
        Me.btnFind.Name = "btnFind"
        Me.btnFind.Size = New System.Drawing.Size(75, 21)
        Me.btnFind.TabIndex = 22
        Me.btnFind.Text = "Find on Map"
        Me.btnFind.UseVisualStyleBackColor = True
        '
        'txtFind
        '
        Me.txtFind.AcceptsReturn = True
        Me.txtFind.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtFind.Location = New System.Drawing.Point(232, 40)
        Me.txtFind.Multiline = True
        Me.txtFind.Name = "txtFind"
        Me.txtFind.Size = New System.Drawing.Size(184, 20)
        Me.txtFind.TabIndex = 25
        '
        'rdoHUC8
        '
        Me.rdoHUC8.AutoSize = True
        Me.rdoHUC8.Location = New System.Drawing.Point(78, 40)
        Me.rdoHUC8.Name = "rdoHUC8"
        Me.rdoHUC8.Size = New System.Drawing.Size(57, 17)
        Me.rdoHUC8.TabIndex = 26
        Me.rdoHUC8.Text = "HUC-8"
        Me.rdoHUC8.UseVisualStyleBackColor = True
        '
        'rdoHUC12
        '
        Me.rdoHUC12.AutoSize = True
        Me.rdoHUC12.Checked = True
        Me.rdoHUC12.Location = New System.Drawing.Point(141, 41)
        Me.rdoHUC12.Name = "rdoHUC12"
        Me.rdoHUC12.Size = New System.Drawing.Size(63, 17)
        Me.rdoHUC12.TabIndex = 27
        Me.rdoHUC12.TabStop = True
        Me.rdoHUC12.Text = "HUC-12"
        Me.rdoHUC12.UseVisualStyleBackColor = True
        '
        'tabsAreaSelection
        '
        Me.tabsAreaSelection.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tabsAreaSelection.Controls.Add(Me.tabSelectOnMap)
        Me.tabsAreaSelection.Controls.Add(Me.tabSelectList)
        Me.tabsAreaSelection.Location = New System.Drawing.Point(12, 12)
        Me.tabsAreaSelection.Name = "tabsAreaSelection"
        Me.tabsAreaSelection.SelectedIndex = 0
        Me.tabsAreaSelection.Size = New System.Drawing.Size(508, 182)
        Me.tabsAreaSelection.TabIndex = 28
        '
        'tabSelectOnMap
        '
        Me.tabSelectOnMap.Controls.Add(Me.rdoHUC12)
        Me.tabSelectOnMap.Controls.Add(Me.rdoHUC8)
        Me.tabSelectOnMap.Controls.Add(Me.txtSelected)
        Me.tabSelectOnMap.Controls.Add(Me.lblProjectSize)
        Me.tabSelectOnMap.Controls.Add(Me.btnFind)
        Me.tabSelectOnMap.Controls.Add(Me.txtFind)
        Me.tabSelectOnMap.Controls.Add(Me.txtInstructions)
        Me.tabSelectOnMap.Location = New System.Drawing.Point(4, 22)
        Me.tabSelectOnMap.Name = "tabSelectOnMap"
        Me.tabSelectOnMap.Padding = New System.Windows.Forms.Padding(3)
        Me.tabSelectOnMap.Size = New System.Drawing.Size(500, 156)
        Me.tabSelectOnMap.TabIndex = 0
        Me.tabSelectOnMap.Text = "Select One Project Area On Map"
        Me.tabSelectOnMap.UseVisualStyleBackColor = True
        '
        'tabSelectList
        '
        Me.tabSelectList.Controls.Add(Me.lblHucList)
        Me.tabSelectList.Controls.Add(Me.txtHucList)
        Me.tabSelectList.Location = New System.Drawing.Point(4, 22)
        Me.tabSelectList.Name = "tabSelectList"
        Me.tabSelectList.Padding = New System.Windows.Forms.Padding(3)
        Me.tabSelectList.Size = New System.Drawing.Size(500, 156)
        Me.tabSelectList.TabIndex = 1
        Me.tabSelectList.Text = "Specify List of HUCs to Build Separate Projects"
        Me.tabSelectList.UseVisualStyleBackColor = True
        '
        'lblHucList
        '
        Me.lblHucList.AutoSize = True
        Me.lblHucList.Location = New System.Drawing.Point(3, 3)
        Me.lblHucList.Name = "lblHucList"
        Me.lblHucList.Size = New System.Drawing.Size(453, 13)
        Me.lblHucList.TabIndex = 9
        Me.lblHucList.Text = "Paste or type 8 or 12-digit hydrologic unit codes separated by any delimiter or o" & _
            "n separate lines:"
        '
        'txtHucList
        '
        Me.txtHucList.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtHucList.Location = New System.Drawing.Point(0, 19)
        Me.txtHucList.Multiline = True
        Me.txtHucList.Name = "txtHucList"
        Me.txtHucList.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtHucList.Size = New System.Drawing.Size(500, 137)
        Me.txtHucList.TabIndex = 0
        '
        'txtSimulationEndYear
        '
        Me.txtSimulationEndYear.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.txtSimulationEndYear.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtSimulationEndYear.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.txtSimulationEndYear.DefaultValue = "0.0"
        Me.txtSimulationEndYear.HardMax = 3000
        Me.txtSimulationEndYear.HardMin = 0
        Me.txtSimulationEndYear.InsideLimitsBackground = System.Drawing.Color.White
        Me.txtSimulationEndYear.Location = New System.Drawing.Point(12, 320)
        Me.txtSimulationEndYear.MaxWidth = 20
        Me.txtSimulationEndYear.Name = "txtSimulationEndYear"
        Me.txtSimulationEndYear.NumericFormat = "0.#####"
        Me.txtSimulationEndYear.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.txtSimulationEndYear.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.txtSimulationEndYear.SelLength = 0
        Me.txtSimulationEndYear.SelStart = 0
        Me.txtSimulationEndYear.Size = New System.Drawing.Size(49, 18)
        Me.txtSimulationEndYear.SoftMax = -999
        Me.txtSimulationEndYear.SoftMin = -999
        Me.txtSimulationEndYear.TabIndex = 18
        Me.txtSimulationEndYear.ValueDouble = 2000
        Me.txtSimulationEndYear.ValueInteger = 2000
        '
        'txtSimulationStartYear
        '
        Me.txtSimulationStartYear.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.txtSimulationStartYear.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.txtSimulationStartYear.DataType = atcControls.atcText.ATCoDataType.ATCoInt
        Me.txtSimulationStartYear.DefaultValue = "0.0"
        Me.txtSimulationStartYear.HardMax = 3000
        Me.txtSimulationStartYear.HardMin = 0
        Me.txtSimulationStartYear.InsideLimitsBackground = System.Drawing.Color.White
        Me.txtSimulationStartYear.Location = New System.Drawing.Point(12, 296)
        Me.txtSimulationStartYear.MaxWidth = 20
        Me.txtSimulationStartYear.Name = "txtSimulationStartYear"
        Me.txtSimulationStartYear.NumericFormat = "0.#####"
        Me.txtSimulationStartYear.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.txtSimulationStartYear.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.txtSimulationStartYear.SelLength = 0
        Me.txtSimulationStartYear.SelStart = 0
        Me.txtSimulationStartYear.Size = New System.Drawing.Size(49, 18)
        Me.txtSimulationStartYear.SoftMax = -999
        Me.txtSimulationStartYear.SoftMin = -999
        Me.txtSimulationStartYear.TabIndex = 16
        Me.txtSimulationStartYear.ValueDouble = 1990
        Me.txtSimulationStartYear.ValueInteger = 1990
        '
        'atxLU
        '
        Me.atxLU.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.atxLU.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.atxLU.DataType = atcControls.atcText.ATCoDataType.ATCoDbl
        Me.atxLU.DefaultValue = "0.07"
        Me.atxLU.HardMax = 1
        Me.atxLU.HardMin = 0
        Me.atxLU.InsideLimitsBackground = System.Drawing.Color.White
        Me.atxLU.Location = New System.Drawing.Point(12, 272)
        Me.atxLU.MaxWidth = 20
        Me.atxLU.Name = "atxLU"
        Me.atxLU.NumericFormat = "0.#####"
        Me.atxLU.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.atxLU.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.atxLU.SelLength = 0
        Me.atxLU.SelStart = 0
        Me.atxLU.Size = New System.Drawing.Size(49, 18)
        Me.atxLU.SoftMax = -999
        Me.atxLU.SoftMin = -999
        Me.atxLU.TabIndex = 11
        Me.atxLU.ValueDouble = 0.07
        Me.atxLU.ValueInteger = 0
        '
        'atxLength
        '
        Me.atxLength.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.atxLength.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.atxLength.DataType = atcControls.atcText.ATCoDataType.ATCoDbl
        Me.atxLength.DefaultValue = "1.0"
        Me.atxLength.HardMax = -999
        Me.atxLength.HardMin = 0
        Me.atxLength.InsideLimitsBackground = System.Drawing.Color.White
        Me.atxLength.Location = New System.Drawing.Point(12, 248)
        Me.atxLength.MaxWidth = 20
        Me.atxLength.Name = "atxLength"
        Me.atxLength.NumericFormat = "0.#####"
        Me.atxLength.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.atxLength.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.atxLength.SelLength = 0
        Me.atxLength.SelStart = 0
        Me.atxLength.Size = New System.Drawing.Size(49, 18)
        Me.atxLength.SoftMax = -999
        Me.atxLength.SoftMin = -999
        Me.atxLength.TabIndex = 9
        Me.atxLength.ValueDouble = 5
        Me.atxLength.ValueInteger = 5
        '
        'atxSize
        '
        Me.atxSize.Alignment = System.Windows.Forms.HorizontalAlignment.Left
        Me.atxSize.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.atxSize.DataType = atcControls.atcText.ATCoDataType.ATCoDbl
        Me.atxSize.DefaultValue = "1.0"
        Me.atxSize.HardMax = -999
        Me.atxSize.HardMin = 0
        Me.atxSize.InsideLimitsBackground = System.Drawing.Color.White
        Me.atxSize.Location = New System.Drawing.Point(12, 223)
        Me.atxSize.MaxWidth = 20
        Me.atxSize.Name = "atxSize"
        Me.atxSize.NumericFormat = "0.#####"
        Me.atxSize.OutsideHardLimitBackground = System.Drawing.Color.Coral
        Me.atxSize.OutsideSoftLimitBackground = System.Drawing.Color.Yellow
        Me.atxSize.SelLength = 0
        Me.atxSize.SelStart = 0
        Me.atxSize.Size = New System.Drawing.Size(49, 19)
        Me.atxSize.SoftMax = -999
        Me.atxSize.SoftMin = -999
        Me.atxSize.TabIndex = 7
        Me.atxSize.ValueDouble = 1
        Me.atxSize.ValueInteger = 1
        '
        'frmBuildNew
        '
        Me.AcceptButton = Me.btnBuild
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(532, 414)
        Me.Controls.Add(Me.lblSimulationEndYear)
        Me.Controls.Add(Me.txtSimulationEndYear)
        Me.Controls.Add(Me.lblSimulationStartYear)
        Me.Controls.Add(Me.txtSimulationStartYear)
        Me.Controls.Add(Me.lblFile)
        Me.Controls.Add(Me.lblSWAT)
        Me.Controls.Add(Me.cmdSet)
        Me.Controls.Add(Me.lblLU)
        Me.Controls.Add(Me.atxLU)
        Me.Controls.Add(Me.lblLength)
        Me.Controls.Add(Me.atxLength)
        Me.Controls.Add(Me.lblSize)
        Me.Controls.Add(Me.atxSize)
        Me.Controls.Add(Me.chkSWAT)
        Me.Controls.Add(Me.chkHSPF)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnBuild)
        Me.Controls.Add(Me.tabsAreaSelection)
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.Name = "frmBuildNew"
        Me.Text = "Build Project"
        Me.TopMost = True
        Me.tabsAreaSelection.ResumeLayout(False)
        Me.tabSelectOnMap.ResumeLayout(False)
        Me.tabSelectOnMap.PerformLayout()
        Me.tabSelectList.ResumeLayout(False)
        Me.tabSelectList.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private pLayerDBFs As New Generic.List(Of atcUtility.atcTableDBF)

    Private Sub cmdBuild_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuild.Click
        SaveSetting(g_AppNameRegistry, "Window Positions", "BuildTop", Me.Top)
        SaveSetting(g_AppNameRegistry, "Window Positions", "BuildLeft", Me.Left)

        g_DoHSPF = chkHSPF.Checked
        g_DoSWAT = chkSWAT.Checked
        g_MinCatchmentKM2 = atxSize.Text
        g_MinFlowlineKM = atxLength.Text
        g_LandUseIgnoreBelowFraction = atxLU.Text
        g_SimulationStartYear = txtSimulationStartYear.ValueInteger
        g_SimulationEndYear = txtSimulationEndYear.ValueInteger
        g_SWATDatabaseName = lblFile.Text

        If tabsAreaSelection.SelectedTab Is tabSelectList Then
            g_HucList = New Generic.List(Of String)
            Dim g_AllHucsString As String = txtHucList.Text
            Dim lInHuc As Boolean = False
            Dim lHucStart As Integer = 0
            For lCharPos As Integer = 0 To g_AllHucsString.Length - 1
                If IsNumeric(g_AllHucsString.Substring(lCharPos, 1)) Then
                    If Not lInHuc Then
                        lInHuc = True
                        lHucStart = lCharPos
                    End If
                Else
                    If lInHuc Then
                        lInHuc = False
                        g_HucList.Add(HucStringRoundLength(g_AllHucsString.Substring(lHucStart, lCharPos - lHucStart)))
                    End If
                End If
            Next
            If lInHuc Then
                lInHuc = False
                g_HucList.Add(HucStringRoundLength(g_AllHucsString.Substring(lHucStart, g_AllHucsString.Length - lHucStart)))
            End If
        Else
            g_HucList = Nothing
        End If

        Me.Visible = False

        Dim lCreatedMapWindowProjectFilename As String = SpecifyAndCreateNewProject()

        If IO.File.Exists(lCreatedMapWindowProjectFilename) Then
            Me.Close()
        Else
            Me.Visible = True
            modSDM.pBuildFrm = Me
        End If
    End Sub

    ''' <summary>
    ''' Prepend a zero if needed to make sure string has even number of characters
    ''' </summary>
    Private Function HucStringRoundLength(ByVal aHucString As String) As String
        If Math.Floor((aHucString.Length) / 2) * 2 < (aHucString.Length) Then
            Return "0" & aHucString
        Else
            Return aHucString
        End If
    End Function

    Private Sub frmBuildNew_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        'Me.Opacity = 1
    End Sub

    Private Sub frmBuildNew_Deactivate(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Deactivate
        'Me.Opacity = 0.8
    End Sub

    Private Sub frmBuildNew_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyValue = Windows.Forms.Keys.F1 Then
            ShowHelp("BASINS Details\Welcome to BASINS 4 Window\Build BASINS Project.html")
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub frmBuildNew_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Icon = g_MapWin.ApplicationInfo.FormIcon
        Me.Text = "Build " & g_AppNameLong & " Project"

        chkHSPF.Checked = g_DoHSPF
        chkSWAT.Checked = g_DoSWAT
        atxSize.Text = g_MinCatchmentKM2
        atxLength.Text = g_MinFlowlineKM
        atxLU.Text = g_LandUseIgnoreBelowFraction
        txtSimulationStartYear.ValueInteger = g_SimulationStartYear
        txtSimulationEndYear.ValueInteger = g_SimulationEndYear
        lblFile.Text = g_SWATDatabaseName

        If g_HucList IsNot Nothing AndAlso g_HucList.Count > 0 Then
            txtHucList.Text = String.Join(vbCrLf, g_HucList.ToArray)
        End If
    End Sub

    Private Sub cmdSet_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSet.Click
        g_SWATDatabaseName = FindFile("Please locate SWAT2005.mdb", "SWAT2005.mdb").Replace("swat", "SWAT")
        lblFile.Text = g_SWATDatabaseName
    End Sub

    Private Sub btnFind_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFind.Click
        Find()
    End Sub

    Private Sub Find()
        FindText(txtFind.Text)
    End Sub

    Public Function FindText(ByVal aText As String) As Boolean
        Dim lMatchingRecord As Integer = -1
        If aText.Length > 0 Then
            Dim lLayerHandle As Integer = -1
            Dim lLoadedHuc12 As Boolean = False
            If IsNumeric(aText) Then 'Numeric search
                Select Case aText.Length
                    Case 8 : lLayerHandle = Huc8Layer()
                    Case 12
FindHuc12:
                        Dim lHuc8 As String = SafeSubstring(aText, 0, 8)
                        For iLayer As Integer = g_MapWin.Layers.NumLayers - 1 To 0 Step -1
                            Dim lSearchLayerHandle As Integer = g_MapWin.Layers.GetHandle(iLayer)
                            If lSearchLayerHandle >= 0 Then
                                Dim lSearchLayer As MapWindow.Interfaces.Layer = g_MapWin.Layers(lSearchLayerHandle)
                                If lSearchLayer IsNot Nothing AndAlso lSearchLayer.FileName.ToLower.Contains(lHuc8 & g_PathChar & "huc12") Then
                                    lLayerHandle = lSearchLayerHandle
                                    Exit For
                                End If
                            End If
                        Next

                        If lLayerHandle < 0 AndAlso Not lLoadedHuc12 Then
                            lLoadedHuc12 = True
                            LoadHUC12(lHuc8)
                            GoTo FindHuc12
                        End If

                        If lLayerHandle < 0 Then
                            lLayerHandle = Huc8Layer()
                            If lLayerHandle >= 0 Then
                                aText = lHuc8
                            End If
                        End If
                    Case Else
                        'TODO: partial search for number that is not 8 or 12 digits?
                End Select
                If lLayerHandle > -1 Then
                    lMatchingRecord = MatchingKeyRecord(g_MapWin.Layers(lLayerHandle).FileName, aText)
                    If aText.Length = 8 AndAlso Not lLoadedHuc12 AndAlso pBuildFrm.rdoHUC12.Checked Then
                        lLoadedHuc12 = True
                        LoadHUC12(aText)
                    End If
                End If
            Else
                For iLayer As Integer = g_MapWin.Layers.NumLayers - 1 To 0 Step -1
                    If lMatchingRecord < 0 Then
                        Dim lSearchLayerHandle As Integer = g_MapWin.Layers.GetHandle(iLayer)
                        If lSearchLayerHandle >= 0 Then
                            Dim lSearchLayer As MapWindow.Interfaces.Layer = g_MapWin.Layers(lSearchLayerHandle)
                            If lSearchLayer IsNot Nothing Then
                                lLayerHandle = lSearchLayerHandle

                                Dim lDescFieldName As String = DBFDescriptionFieldName(lSearchLayer.FileName).ToLower
                                If lDescFieldName.Length > 0 Then
                                    Dim lDBF As atcTableDBF = LayerDBF(lSearchLayer.FileName)
                                    Dim lDescFieldNum As Integer = lDBF.FieldNumber(lDescFieldName)
                                    If lDescFieldNum > 0 Then
                                        For lRecord As Integer = 1 To lDBF.NumRecords
                                            lDBF.CurrentRecord = lRecord
                                            If lDBF.Value(lDescFieldNum).ToLower.Contains(aText.ToLower) Then
                                                lMatchingRecord = lRecord
                                                Exit For
                                            End If
                                        Next
                                    End If
                                End If
                            End If
                        End If
                    End If
                Next
            End If

            If lMatchingRecord >= 0 Then
                g_MapWin.Layers.CurrentLayer = lLayerHandle
                g_MapWin.View.ClearSelectedShapes()
                g_MapWin.View.SelectedShapes.AddByIndex(lMatchingRecord, g_MapWin.View.SelectColor)
                g_MapWin.Layers(lLayerHandle).Shapes(lMatchingRecord).ZoomTo()
            End If
        End If
        UpdateSelectedFeatures()
        Return (lMatchingRecord >= 0)
    End Function

    Private Function MatchingKeyRecord(ByVal aShapeFilename As String, ByVal aText As String) As Integer
        Dim lKeyFieldName As String = DBFKeyFieldName(aShapeFilename).ToLower
        If lKeyFieldName.Length > 0 Then
            Dim lDBF As atcTableDBF = LayerDBF(aShapeFilename)
            Dim lKeyFieldNum As Integer = lDBF.FieldNumber(lKeyFieldName)
            If lKeyFieldNum > 0 AndAlso lDBF.FindFirst(lKeyFieldNum, aText) Then
                Return lDBF.CurrentRecord - 1
            End If
        End If
        Return -1
    End Function

    'Private Function MatchingDescriptionRecord(ByVal aShapeFilename As String, ByVal aText As String) As Integer
    '    Dim lDescriptionFieldName As String = DBFDescriptionFieldName(aShapeFilename).ToLower
    '    If lDescriptionFieldName.Length > 0 Then
    '        Dim lDBF As atcTableDBF = LayerDBF(aShapeFilename)
    '        Dim lDescriptionFieldNum As Integer = lDBF.FieldNumber(lDescriptionFieldName)
    '        If lDescriptionFieldNum > 0 Then
    '            aText = aText.Trim.ToLower
    '            Dim lLastRecord As Integer = lDBF.NumRecords
    '            While lDBF.CurrentRecord <= lLastRecord
    '                If lDBF.Value(lDescriptionFieldNum).ToLower.Contains(aText) Then
    '                    Return lDBF.CurrentRecord - 1
    '                End If
    '                If lDBF.CurrentRecord < lLastRecord Then
    '                    lDBF.CurrentRecord += 1
    '                Else 'force exit since attempting to set CurrentRecord beyond NumRecords sets it back to 1
    '                    Exit While
    '                End If
    '            End While
    '        End If
    '    End If
    '    Return -1
    'End Function

    Private Function LayerDBF(ByVal aShapeFileName As String) As atcTableDBF
        Dim lDBFFileName As String = IO.Path.ChangeExtension(aShapeFileName, "dbf").ToLower
        Dim lLayer As atcTableDBF
        For Each lLayer In pLayerDBFs
            If lLayer.FileName = lDBFFileName Then
                Return lLayer
            End If
        Next
        If IO.File.Exists(lDBFFileName) Then
            lLayer = New atcTableDBF
            If lLayer.OpenFile(lDBFFileName) Then
                pLayerDBFs.Add(lLayer)
                Return lLayer
            End If
        End If
        Return Nothing
    End Function

    Private Sub rdoHUC8_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdoHUC8.CheckedChanged
        If rdoHUC8.Checked Then
            chkHSPF.Checked = False
            'Remove HUC-12 layers from map
            For iLayer As Integer = g_MapWin.Layers.NumLayers - 1 To 0 Step -1
                Dim lLayerHandle As Integer = g_MapWin.Layers.GetHandle(iLayer)
                If g_MapWin.Layers(lLayerHandle).FileName.ToLower.EndsWith("huc12.shp") Then
                    g_MapWin.Layers.Remove(lLayerHandle)
                End If
            Next
        Else
        End If
        UpdateSelectedFeatures
    End Sub

    Private Sub chkHSPF_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkHSPF.CheckedChanged
        If chkHSPF.Checked AndAlso rdoHUC8.Checked Then
            rdoHUC12.Checked = True
        End If
    End Sub

    Private Sub txtFind_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtFind.KeyDown
        If e.KeyCode = Windows.Forms.Keys.Enter Then
            e.Handled = True
            Find()
        End If
    End Sub

    Private Sub txtFind_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtFind.KeyPress
        If e.KeyChar = vbCr Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtFind_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtFind.KeyUp
        If e.KeyCode = Windows.Forms.Keys.Enter Then
            e.Handled = True
        End If
    End Sub
End Class
