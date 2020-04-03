Public Class frmProjection
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        pDB = New ProjectionDB
        SetControlsFromDB()
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
    Friend WithEvents lblSpheroid As System.Windows.Forms.Label
    Friend WithEvents lblName As System.Windows.Forms.Label
    Friend WithEvents lblProjection As System.Windows.Forms.Label
    Friend WithEvents txtD1 As System.Windows.Forms.TextBox
    Friend WithEvents lblD1 As System.Windows.Forms.Label
    Friend WithEvents lblD2 As System.Windows.Forms.Label
    Friend WithEvents txtD2 As System.Windows.Forms.TextBox
    Friend WithEvents lblD3 As System.Windows.Forms.Label
    Friend WithEvents txtD3 As System.Windows.Forms.TextBox
    Friend WithEvents lblD4 As System.Windows.Forms.Label
    Friend WithEvents txtD4 As System.Windows.Forms.TextBox
    Friend WithEvents lblD5 As System.Windows.Forms.Label
    Friend WithEvents txtD5 As System.Windows.Forms.TextBox
    Friend WithEvents lblD6 As System.Windows.Forms.Label
    Friend WithEvents txtD6 As System.Windows.Forms.TextBox
    Friend WithEvents radioStandard As System.Windows.Forms.RadioButton
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnAdd As System.Windows.Forms.Button
    Friend WithEvents cboSpheroid As System.Windows.Forms.ComboBox
    Friend WithEvents cboName As System.Windows.Forms.ComboBox
    Friend WithEvents txtSpheroid As System.Windows.Forms.TextBox
    Friend WithEvents txtName As System.Windows.Forms.TextBox
    Friend WithEvents cboProjection As System.Windows.Forms.ComboBox
    Friend WithEvents cboCategory As System.Windows.Forms.ComboBox
    Friend WithEvents fraMain As System.Windows.Forms.Panel
    Friend WithEvents radioCustom As System.Windows.Forms.RadioButton
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents fraNoDatabase As System.Windows.Forms.Panel
    Friend WithEvents lblCategory As System.Windows.Forms.Label
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmProjection))
        Me.fraMain = New System.Windows.Forms.Panel
        Me.lblCategory = New System.Windows.Forms.Label
        Me.lblD6 = New System.Windows.Forms.Label
        Me.txtD6 = New System.Windows.Forms.TextBox
        Me.lblD5 = New System.Windows.Forms.Label
        Me.txtD5 = New System.Windows.Forms.TextBox
        Me.lblD4 = New System.Windows.Forms.Label
        Me.txtD4 = New System.Windows.Forms.TextBox
        Me.lblD3 = New System.Windows.Forms.Label
        Me.txtD3 = New System.Windows.Forms.TextBox
        Me.lblD2 = New System.Windows.Forms.Label
        Me.txtD2 = New System.Windows.Forms.TextBox
        Me.lblD1 = New System.Windows.Forms.Label
        Me.cboName = New System.Windows.Forms.ComboBox
        Me.txtD1 = New System.Windows.Forms.TextBox
        Me.lblSpheroid = New System.Windows.Forms.Label
        Me.lblName = New System.Windows.Forms.Label
        Me.lblProjection = New System.Windows.Forms.Label
        Me.txtName = New System.Windows.Forms.TextBox
        Me.cboCategory = New System.Windows.Forms.ComboBox
        Me.cboProjection = New System.Windows.Forms.ComboBox
        Me.cboSpheroid = New System.Windows.Forms.ComboBox
        Me.txtSpheroid = New System.Windows.Forms.TextBox
        Me.radioStandard = New System.Windows.Forms.RadioButton
        Me.radioCustom = New System.Windows.Forms.RadioButton
        Me.btnOK = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnAdd = New System.Windows.Forms.Button
        Me.fraNoDatabase = New System.Windows.Forms.Panel
        Me.Label1 = New System.Windows.Forms.Label
        Me.fraMain.SuspendLayout()
        Me.fraNoDatabase.SuspendLayout()
        Me.SuspendLayout()
        '
        'fraMain
        '
        Me.fraMain.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.fraMain.Controls.Add(Me.lblCategory)
        Me.fraMain.Controls.Add(Me.lblD6)
        Me.fraMain.Controls.Add(Me.txtD6)
        Me.fraMain.Controls.Add(Me.lblD5)
        Me.fraMain.Controls.Add(Me.txtD5)
        Me.fraMain.Controls.Add(Me.lblD4)
        Me.fraMain.Controls.Add(Me.txtD4)
        Me.fraMain.Controls.Add(Me.lblD3)
        Me.fraMain.Controls.Add(Me.txtD3)
        Me.fraMain.Controls.Add(Me.lblD2)
        Me.fraMain.Controls.Add(Me.txtD2)
        Me.fraMain.Controls.Add(Me.lblD1)
        Me.fraMain.Controls.Add(Me.cboName)
        Me.fraMain.Controls.Add(Me.txtD1)
        Me.fraMain.Controls.Add(Me.lblSpheroid)
        Me.fraMain.Controls.Add(Me.lblName)
        Me.fraMain.Controls.Add(Me.lblProjection)
        Me.fraMain.Controls.Add(Me.txtName)
        Me.fraMain.Controls.Add(Me.cboCategory)
        Me.fraMain.Controls.Add(Me.cboProjection)
        Me.fraMain.Controls.Add(Me.cboSpheroid)
        Me.fraMain.Controls.Add(Me.txtSpheroid)
        Me.fraMain.Location = New System.Drawing.Point(8, 32)
        Me.fraMain.Name = "fraMain"
        Me.fraMain.Size = New System.Drawing.Size(328, 244)
        Me.fraMain.TabIndex = 2
        '
        'lblCategory
        '
        Me.lblCategory.Location = New System.Drawing.Point(8, 16)
        Me.lblCategory.Name = "lblCategory"
        Me.lblCategory.Size = New System.Drawing.Size(64, 16)
        Me.lblCategory.TabIndex = 21
        Me.lblCategory.Text = "Category"
        '
        'lblD6
        '
        Me.lblD6.AutoSize = True
        Me.lblD6.Location = New System.Drawing.Point(8, 216)
        Me.lblD6.Name = "lblD6"
        Me.lblD6.Size = New System.Drawing.Size(19, 16)
        Me.lblD6.TabIndex = 18
        Me.lblD6.Text = "D6"
        '
        'txtD6
        '
        Me.txtD6.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtD6.Location = New System.Drawing.Point(200, 216)
        Me.txtD6.Name = "txtD6"
        Me.txtD6.Size = New System.Drawing.Size(120, 20)
        Me.txtD6.TabIndex = 19
        Me.txtD6.Text = ""
        '
        'lblD5
        '
        Me.lblD5.AutoSize = True
        Me.lblD5.Location = New System.Drawing.Point(8, 192)
        Me.lblD5.Name = "lblD5"
        Me.lblD5.Size = New System.Drawing.Size(19, 16)
        Me.lblD5.TabIndex = 16
        Me.lblD5.Text = "D5"
        '
        'txtD5
        '
        Me.txtD5.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtD5.Location = New System.Drawing.Point(200, 192)
        Me.txtD5.Name = "txtD5"
        Me.txtD5.Size = New System.Drawing.Size(120, 20)
        Me.txtD5.TabIndex = 17
        Me.txtD5.Text = ""
        '
        'lblD4
        '
        Me.lblD4.AutoSize = True
        Me.lblD4.Location = New System.Drawing.Point(8, 168)
        Me.lblD4.Name = "lblD4"
        Me.lblD4.Size = New System.Drawing.Size(19, 16)
        Me.lblD4.TabIndex = 14
        Me.lblD4.Text = "D4"
        '
        'txtD4
        '
        Me.txtD4.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtD4.Location = New System.Drawing.Point(200, 168)
        Me.txtD4.Name = "txtD4"
        Me.txtD4.Size = New System.Drawing.Size(120, 20)
        Me.txtD4.TabIndex = 15
        Me.txtD4.Text = ""
        '
        'lblD3
        '
        Me.lblD3.AutoSize = True
        Me.lblD3.Location = New System.Drawing.Point(8, 144)
        Me.lblD3.Name = "lblD3"
        Me.lblD3.Size = New System.Drawing.Size(19, 16)
        Me.lblD3.TabIndex = 12
        Me.lblD3.Text = "D3"
        '
        'txtD3
        '
        Me.txtD3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtD3.Location = New System.Drawing.Point(200, 144)
        Me.txtD3.Name = "txtD3"
        Me.txtD3.Size = New System.Drawing.Size(120, 20)
        Me.txtD3.TabIndex = 13
        Me.txtD3.Text = ""
        '
        'lblD2
        '
        Me.lblD2.AutoSize = True
        Me.lblD2.Location = New System.Drawing.Point(8, 120)
        Me.lblD2.Name = "lblD2"
        Me.lblD2.Size = New System.Drawing.Size(19, 16)
        Me.lblD2.TabIndex = 10
        Me.lblD2.Text = "D2"
        '
        'txtD2
        '
        Me.txtD2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtD2.Location = New System.Drawing.Point(200, 120)
        Me.txtD2.Name = "txtD2"
        Me.txtD2.Size = New System.Drawing.Size(120, 20)
        Me.txtD2.TabIndex = 11
        Me.txtD2.Text = ""
        '
        'lblD1
        '
        Me.lblD1.AutoSize = True
        Me.lblD1.Location = New System.Drawing.Point(8, 96)
        Me.lblD1.Name = "lblD1"
        Me.lblD1.Size = New System.Drawing.Size(19, 16)
        Me.lblD1.TabIndex = 8
        Me.lblD1.Text = "D1"
        '
        'cboName
        '
        Me.cboName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboName.Location = New System.Drawing.Point(72, 32)
        Me.cboName.Name = "cboName"
        Me.cboName.Size = New System.Drawing.Size(248, 21)
        Me.cboName.TabIndex = 3
        '
        'txtD1
        '
        Me.txtD1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtD1.Location = New System.Drawing.Point(200, 96)
        Me.txtD1.Name = "txtD1"
        Me.txtD1.Size = New System.Drawing.Size(120, 20)
        Me.txtD1.TabIndex = 9
        Me.txtD1.Text = ""
        '
        'lblSpheroid
        '
        Me.lblSpheroid.Location = New System.Drawing.Point(8, 64)
        Me.lblSpheroid.Name = "lblSpheroid"
        Me.lblSpheroid.Size = New System.Drawing.Size(64, 16)
        Me.lblSpheroid.TabIndex = 5
        Me.lblSpheroid.Text = "Spheroid"
        '
        'lblName
        '
        Me.lblName.Location = New System.Drawing.Point(8, 40)
        Me.lblName.Name = "lblName"
        Me.lblName.Size = New System.Drawing.Size(64, 16)
        Me.lblName.TabIndex = 2
        Me.lblName.Text = "Name"
        '
        'lblProjection
        '
        Me.lblProjection.Location = New System.Drawing.Point(8, 16)
        Me.lblProjection.Name = "lblProjection"
        Me.lblProjection.Size = New System.Drawing.Size(64, 16)
        Me.lblProjection.TabIndex = 0
        Me.lblProjection.Text = "Projection"
        '
        'txtName
        '
        Me.txtName.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtName.Location = New System.Drawing.Point(72, 32)
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New System.Drawing.Size(248, 20)
        Me.txtName.TabIndex = 4
        Me.txtName.Text = ""
        '
        'cboCategory
        '
        Me.cboCategory.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboCategory.Location = New System.Drawing.Point(72, 8)
        Me.cboCategory.Name = "cboCategory"
        Me.cboCategory.Size = New System.Drawing.Size(248, 21)
        Me.cboCategory.TabIndex = 20
        '
        'cboProjection
        '
        Me.cboProjection.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboProjection.Location = New System.Drawing.Point(72, 8)
        Me.cboProjection.Name = "cboProjection"
        Me.cboProjection.Size = New System.Drawing.Size(248, 21)
        Me.cboProjection.TabIndex = 1
        Me.cboProjection.Text = "cboProjection"
        '
        'cboSpheroid
        '
        Me.cboSpheroid.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboSpheroid.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSpheroid.Location = New System.Drawing.Point(72, 56)
        Me.cboSpheroid.Name = "cboSpheroid"
        Me.cboSpheroid.Size = New System.Drawing.Size(248, 21)
        Me.cboSpheroid.TabIndex = 6
        '
        'txtSpheroid
        '
        Me.txtSpheroid.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSpheroid.Enabled = False
        Me.txtSpheroid.Location = New System.Drawing.Point(72, 56)
        Me.txtSpheroid.Name = "txtSpheroid"
        Me.txtSpheroid.Size = New System.Drawing.Size(248, 20)
        Me.txtSpheroid.TabIndex = 7
        Me.txtSpheroid.Text = ""
        '
        'radioStandard
        '
        Me.radioStandard.Checked = True
        Me.radioStandard.Location = New System.Drawing.Point(80, 8)
        Me.radioStandard.Name = "radioStandard"
        Me.radioStandard.Size = New System.Drawing.Size(80, 16)
        Me.radioStandard.TabIndex = 0
        Me.radioStandard.TabStop = True
        Me.radioStandard.Text = "Standard"
        '
        'radioCustom
        '
        Me.radioCustom.Location = New System.Drawing.Point(176, 8)
        Me.radioCustom.Name = "radioCustom"
        Me.radioCustom.Size = New System.Drawing.Size(80, 16)
        Me.radioCustom.TabIndex = 1
        Me.radioCustom.Text = "Custom"
        '
        'btnOK
        '
        Me.btnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnOK.Location = New System.Drawing.Point(72, 280)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(64, 24)
        Me.btnOK.TabIndex = 4
        Me.btnOK.Text = "&OK"
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnCancel.Location = New System.Drawing.Point(144, 280)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(64, 24)
        Me.btnCancel.TabIndex = 5
        Me.btnCancel.Text = "Cancel"
        '
        'btnAdd
        '
        Me.btnAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnAdd.Enabled = False
        Me.btnAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAdd.Location = New System.Drawing.Point(216, 280)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(64, 24)
        Me.btnAdd.TabIndex = 6
        Me.btnAdd.Text = "&Add"
        Me.btnAdd.Visible = False
        '
        'fraNoDatabase
        '
        Me.fraNoDatabase.Controls.Add(Me.Label1)
        Me.fraNoDatabase.Location = New System.Drawing.Point(8, 32)
        Me.fraNoDatabase.Name = "fraNoDatabase"
        Me.fraNoDatabase.Size = New System.Drawing.Size(280, 240)
        Me.fraNoDatabase.TabIndex = 7
        Me.fraNoDatabase.Visible = False
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(16, 32)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(248, 80)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Projection Database Could Not Be Opened"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'frmProjection
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(344, 309)
        Me.Controls.Add(Me.btnAdd)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.radioCustom)
        Me.Controls.Add(Me.radioStandard)
        Me.Controls.Add(Me.fraMain)
        Me.Controls.Add(Me.fraNoDatabase)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmProjection"
        Me.Text = "Projection Properties"
        Me.fraMain.ResumeLayout(False)
        Me.fraNoDatabase.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private pDB As ProjectionDB
    '  FColl.FastCollection 'of custom projection specs
    'Private cProjs As FColl.FastCollection 'of standard projection specs
    'Private cElips As FColl.FastCollection 'of ellipsoids
    Private defaultsFlag As Boolean
    Private pOK As Boolean
    'Private pCurProjection As Projection

    Public Function AskUser() As String
        pOK = False
        btnAdd.Enabled = False
        Me.Show()
        Me.Visible = True
        Me.WindowState = System.Windows.Forms.FormWindowState.Normal
        Me.BringToFront()

        cboCategory.SelectedIndex = CInt(GetSetting("Projection", "LastSelected", "Category", "2"))
        cboName.SelectedIndex = CInt(GetSetting("Projection", "LastSelected", "Name", "1"))

        While Me.Visible
            System.Windows.Forms.Application.DoEvents()
            System.Threading.Thread.Sleep(100)
        End While

        If pOK Then
            AskUser = toString()
            SaveSetting("Projection", "LastSelected", "Category", cboCategory.SelectedIndex)
            SaveSetting("Projection", "LastSelected", "Name", cboName.SelectedIndex)
        Else
            AskUser = ""
        End If
    End Function

    'Examines the controls of the form and returns the projection string
    Public Overrides Function toString() As String
        Dim id As Integer
        Dim ProjObj As Projection
        Dim ProjObjC As Projection = Nothing

        toString = "proj" & vbCrLf

        If radioStandard.Checked Then 'standard, not custom
            ProjObj = pDB.StandardProjections.Item(cboCategory.Text & cboName.Text)
            toString += "+proj=" & Trim(ProjObj.ProjectionClass) & vbCrLf
            For Each ProjObjC In pDB.BaseProjections
                If ProjObj.ProjectionClass = ProjObjC.ProjectionClass Or (ProjObj.ProjectionClass = "utm" And ProjObjC.ProjectionClass = "tmerc") Then
                    If ProjObj.ProjectionClass = "utm" Then
                        toString += "+zone=" & ProjObj.Zone & vbCrLf
                    End If
                    Exit For
                End If
            Next
        Else 'custom, not standard
            ProjObjC = pDB.BaseProjections(cboProjection.Text)
            If Not ProjObjC Is Nothing Then
                toString += "+proj=" & Trim(ProjObjC.ProjectionClass) & vbCrLf
            End If
        End If

        ProjObj = pDB.Ellipsoids(Trim(txtSpheroid.Text))
        If Not ProjObj Is Nothing Then
            toString += "+ellps=" & Trim(ProjObj.ProjectionClass) & vbCrLf
        End If

        If Not ProjObjC Is Nothing Then
            Dim paramName As String
            For id = 1 To 6
                paramName = Trim(ProjObjC.d(id))
                If paramName.Length > 0 Then
                    toString += "+" & paramName & "=" & Trim(txtD(id).Text) & vbCrLf
                End If
            Next
        End If

        toString += "end" & vbCrLf
    End Function

    Private Sub txtD_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtD1.TextChanged, txtD2.TextChanged, txtD3.TextChanged, txtD4.TextChanged, txtD5.TextChanged, txtD6.TextChanged
        btnAdd.Enabled = True
    End Sub
    Private Sub cboSpheroid_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboSpheroid.SelectedIndexChanged, cboSpheroid.TextChanged
        txtSpheroid.Text = cboSpheroid.Text
    End Sub

    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        If Len(txtName.Text) = 0 Then
            'Logger.Msg("Enter a new name for this projection in the Name field above before adding.", "Projection")
        Else
            'TODO: fix adding custom projections to table
            'Logger.Msg("Adding projection to database not yet supported.", "Projection")
            'ElseIf Not pCurProjection Is Nothing Then
            '    If Not pDB Is Nothing Then
            '        pDB.AddCustomProjection((txtName.Text), _
            '                                pCurProjection.ProjectionClass, _
            '                                pCurProjection.Zone, _
            '                                txtSpheroid.Text, _
            '                                txtD1.Text, _
            '                                txtD2.Text, _
            '                                txtD3.Text, _
            '                                txtD4.Text, _
            '                                txtD5.Text, _
            '                                txtD6.Text)
            '    End If
        End If
    End Sub

    Private Sub cboCategory_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboCategory.SelectedIndexChanged
        Dim curProjection As Projection

        cboName.Items.Clear()
        If Not pDB Is Nothing Then
            For Each curProjection In pDB.StandardProjections
                If curProjection.Category = cboCategory.Text Then
                    If Not cboName.Items.Contains(curProjection.Name) Then
                        cboName.Items.Add(curProjection.Name)
                    End If
                End If
            Next
            'TODO: remove hard-coded special cases, maybe by re-ordering the database or adding category default projections
            Select Case cboCategory.SelectedIndex
                Case 0 : cboName.SelectedIndex = 3 'World -> Geographic
                Case 2 : cboName.SelectedIndex = 1 'US -> Albers (Conterminous)
                Case Else : cboName.SelectedIndex = 0
            End Select
        End If
    End Sub

    Private Sub SetControlsFromDB()
        Dim prj As Projection
        Dim addit As Boolean

        If pDB.StandardProjections.Count > 0 Then
            For Each prj In pDB.StandardProjections
                If Not cboCategory.Items.Contains(prj.Category) Then
                    cboCategory.Items.Add(prj.Category)
                End If
            Next

            addit = True  'add only every other one
            For Each lprj As Projection In pDB.BaseProjections
                If addit Then
                    cboProjection.Items.Add(lprj.Name)
                    addit = False
                Else
                    addit = True
                End If
            Next
            cboProjection.Visible = False

            addit = True  'add only every other one
            For Each prj In pDB.Ellipsoids
                If addit Then
                    cboSpheroid.Items.Add(prj.Ellipsoid)
                    addit = False
                Else
                    addit = True
                End If
            Next
            radioStandard.Checked = True

            cboCategory.SelectedIndex = 0

            defaultsFlag = True 'used to specify if visible projection defaults should be updated
            fraMain.Visible = True
            fraNoDatabase.Visible = False
            btnOK.Enabled = True
        Else
            fraMain.Visible = False
            fraNoDatabase.Visible = True
            btnOK.Enabled = False
        End If
    End Sub

    Private Sub radioStandard_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radioStandard.CheckedChanged
        If radioStandard.Checked Then
            lblCategory.Visible = True
            cboCategory.Visible = True
            cboName.Visible = True
            txtName.Visible = False
            lblProjection.Visible = False
            cboProjection.Visible = False
            cboSpheroid.Visible = False
            txtSpheroid.Visible = True
            txtSpheroid.Enabled = False
            'txtSpheroid.ForeColor = vbWindowText
            txtD1.Enabled = False
            txtD2.Enabled = False
            txtD3.Enabled = False
            txtD4.Enabled = False
            txtD5.Enabled = False
            txtD6.Enabled = False
            If cboCategory.Text <> "cboCategory" Then
                cboCategory_SelectedIndexChanged(sender, e)
            End If
        End If
    End Sub

    Private Sub radioCustom_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radioCustom.CheckedChanged
        Dim curProjection As Projection
        Dim lClass As String

        If radioCustom.Checked Then
            lblCategory.Visible = False
            cboCategory.Visible = False
            cboName.Visible = False
            txtName.Visible = True
            cboProjection.Visible = True
            lblProjection.Visible = True
            txtSpheroid.Visible = False
            cboSpheroid.Visible = True
            txtD1.Enabled = True
            txtD2.Enabled = True
            txtD3.Enabled = True
            txtD4.Enabled = True
            txtD5.Enabled = True
            txtD6.Enabled = True
            'figure out what to set default custom parms to

            curProjection = pDB.StandardProjections(cboCategory.Text & cboName.Text)
            If curProjection Is Nothing Then
                lClass = "dd"
            Else
                lClass = curProjection.ProjectionClass
                If lClass = "utm" Then lClass = "tmerc"
            End If

            For Each curProjection In pDB.BaseProjections
                If curProjection.ProjectionClass = lClass Then 'And curProjection.Name = lName Then
                    defaultsFlag = False
                    cboProjection.SelectedIndex = cboProjection.FindStringExact(curProjection.Name, 0)
                    defaultsFlag = True
                    'txtSpheroid.Text = curProjection.Ellipsoid
                    Exit For
                End If
            Next
        End If
    End Sub

    Private Function lblD(ByVal index As Integer) As System.Windows.Forms.Label
        Select Case index
            Case 1 : Return lblD1
            Case 2 : Return lblD2
            Case 3 : Return lblD3
            Case 4 : Return lblD4
            Case 5 : Return lblD5
            Case 6 : Return lblD6
            Case Else : Return Nothing
        End Select
    End Function

    Private Function txtD(ByVal index As Integer) As System.Windows.Forms.TextBox
        Select Case index
            Case 1 : Return txtD1
            Case 2 : Return txtD2
            Case 3 : Return txtD3
            Case 4 : Return txtD4
            Case 5 : Return txtD5
            Case 6 : Return txtD6
            Case Else : Return Nothing
        End Select
    End Function

    Private Sub cboProjection_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboProjection.SelectedIndexChanged
        Dim id As Long
        Dim CurProjection As Projection

        If defaultsFlag Then
            CurProjection = pDB.BaseProjections(cboProjection.Text)
            If InStr(1, CurProjection.Ellipsoid, "=") > 0 Then
                'case where projection determines ellipsoid
                cboSpheroid.Visible = False
                txtSpheroid.Text = Mid(CurProjection.Ellipsoid, InStr(1, CurProjection.Ellipsoid, "=") + 1)
                txtSpheroid.Visible = True
            Else
                'user can select ellipsoid
                cboSpheroid.Text = CurProjection.Ellipsoid
                cboSpheroid.Visible = True
                txtSpheroid.Visible = False
            End If
            For id = 1 To 6
                If Len(CurProjection.d(id)) > 0 Then
                    txtD(id).Text = CurProjection.Defaults.d(id)
                    lblD(id).Text = CurProjection.Fieldnames.d(id) & ":"
                    txtD(id).Visible = True
                    lblD(id).Visible = True
                Else
                    txtD(id).Text = ""
                    lblD(id).Visible = False
                    txtD(id).Visible = False
                End If
            Next
        End If
        btnAdd.Enabled = False
    End Sub

    Private Sub cboName_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboName.SelectedIndexChanged
        Dim id As Long
        Dim lClass As String
        Dim curProjection As Projection
        Dim baseProjection As Projection

        curProjection = pDB.StandardProjections(cboCategory.Text & cboName.Text)
        lClass = curProjection.ProjectionClass
        If lClass = "utm" Then lClass = "tmerc"

        cboSpheroid.Text = curProjection.Ellipsoid

        For id = 1 To 6
            If Len(curProjection.d(id)) > 0 Then
                txtD(id).Text = curProjection.d(id)
            Else
                txtD(id).Text = 0
            End If
        Next

        For id = 1 To 6
            lblD(id).Visible = False
            txtD(id).Visible = False
        Next

        If lClass = "dd" Then
            'leave them all invisible
        Else
            baseProjection = pDB.BaseProjections.Item(curProjection.ProjectionClass)
            If Not baseProjection Is Nothing Then
                For id = 1 To 6
                    If Len(baseProjection.Fieldnames.d(id)) > 0 Then
                        lblD(id).Text = baseProjection.Fieldnames.d(id)
                        lblD(id).Visible = True
                        txtD(id).Visible = True
                    End If
                Next id
            End If
        End If
        btnAdd.Enabled = False
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Visible = False
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        pOK = True
        Me.Visible = False
    End Sub
End Class
