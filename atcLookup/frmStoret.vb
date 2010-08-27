Imports atcUtility
Imports MapWinUtility

Public Class frmStoret
    Inherits System.Windows.Forms.Form
    Dim pProjectFileName As String
    Dim cProgram As Collection
    Dim cContact As Collection
    Dim cPhone As Collection

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
    Friend WithEvents lbxAgency As System.Windows.Forms.ListBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents lblProgram As System.Windows.Forms.Label
    Friend WithEvents lblContact As System.Windows.Forms.Label
    Friend WithEvents lblPhone As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmStoret))
        Me.lbxAgency = New System.Windows.Forms.ListBox
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.lblPhone = New System.Windows.Forms.Label
        Me.lblContact = New System.Windows.Forms.Label
        Me.lblProgram = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'lbxAgency
        '
        Me.lbxAgency.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lbxAgency.Location = New System.Drawing.Point(12, 12)
        Me.lbxAgency.Name = "lbxAgency"
        Me.lbxAgency.Size = New System.Drawing.Size(219, 134)
        Me.lbxAgency.TabIndex = 0
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Location = New System.Drawing.Point(237, 6)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(180, 83)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(13, 21)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(154, 48)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Choose a STORET Agency code from the list to display information below."
        '
        'GroupBox2
        '
        Me.GroupBox2.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox2.Controls.Add(Me.lblPhone)
        Me.GroupBox2.Controls.Add(Me.lblContact)
        Me.GroupBox2.Controls.Add(Me.lblProgram)
        Me.GroupBox2.Controls.Add(Me.Label4)
        Me.GroupBox2.Controls.Add(Me.Label3)
        Me.GroupBox2.Controls.Add(Me.Label2)
        Me.GroupBox2.Location = New System.Drawing.Point(12, 152)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(405, 223)
        Me.GroupBox2.TabIndex = 2
        Me.GroupBox2.TabStop = False
        '
        'lblPhone
        '
        Me.lblPhone.Location = New System.Drawing.Point(27, 139)
        Me.lblPhone.Name = "lblPhone"
        Me.lblPhone.Size = New System.Drawing.Size(300, 14)
        Me.lblPhone.TabIndex = 5
        '
        'lblContact
        '
        Me.lblContact.Location = New System.Drawing.Point(27, 97)
        Me.lblContact.Name = "lblContact"
        Me.lblContact.Size = New System.Drawing.Size(300, 14)
        Me.lblContact.TabIndex = 4
        '
        'lblProgram
        '
        Me.lblProgram.Location = New System.Drawing.Point(27, 42)
        Me.lblProgram.Name = "lblProgram"
        Me.lblProgram.Size = New System.Drawing.Size(300, 13)
        Me.lblProgram.TabIndex = 3
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(13, 118)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(120, 14)
        Me.Label4.TabIndex = 2
        Me.Label4.Text = "Phone Number:"
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(13, 76)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(120, 14)
        Me.Label3.TabIndex = 1
        Me.Label3.Text = "Contact:"
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(13, 21)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(120, 14)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Program Name:"
        '
        'frmStoret
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(429, 387)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.lbxAgency)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmStoret"
        Me.Text = "Storet Agency Lookup Table"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub frmStoret_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim StoretFile As String
        Dim i As Integer
        Dim lBasinsBinLoc As String

        Dim lBasinsFolder As String = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\AQUA TERRA Consultants\BASINS", "Base Directory", "C:\Basins")
        If pProjectFileName Is Nothing Then pProjectFileName = lBasinsFolder & "\data\national\national.mwprj"
        If Not FileExists(pProjectFileName) Then
            lBasinsBinLoc = PathNameOnly(System.Reflection.Assembly.GetEntryAssembly.Location)
            pProjectFileName = Mid(lBasinsBinLoc, 1, Len(lBasinsBinLoc) - 3) & "data\national\national.mwprj"
        End If
        StoretFile = FindFile("Storet Database", PathNameOnly(PathNameOnly(pProjectFileName)) & "\national\storetag.dbf")

        cProgram = New Collection
        cContact = New Collection
        cPhone = New Collection

        If FileExists(StoretFile) Then
            Dim tmpDbf As IatcTable
            tmpDbf = atcUtility.atcTableOpener.OpenAnyTable(StoretFile)
            For i = 1 To tmpDbf.NumRecords
                tmpDbf.CurrentRecord = i
                lbxAgency.Items.Add(tmpDbf.Value(1))
                cProgram.Add(tmpDbf.Value(2))
                cContact.Add(tmpDbf.Value(3))
                cPhone.Add(tmpDbf.Value(4))
            Next i
            lbxAgency.SelectedIndex = 0
        Else
            lblProgram.Text = "STORET Agency Codes are not available"
        End If

    End Sub

    Public Sub InitializeUI(ByVal projectfilename As String)
        pProjectFileName = projectfilename
    End Sub

    Private Sub lbxAgency_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbxAgency.SelectedIndexChanged
        lblProgram.Text = cProgram(lbxAgency.SelectedIndex + 1)
        lblContact.Text = cContact(lbxAgency.SelectedIndex + 1)
        lblPhone.Text = cPhone(lbxAgency.SelectedIndex + 1)
    End Sub
End Class
