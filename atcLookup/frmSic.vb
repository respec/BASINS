Imports atcUtility

Public Class frmSic
  Inherits System.Windows.Forms.Form
  Dim pProjectFileName As String
  Dim cSic As Collection
  Dim cSicName As Collection
  Dim cNaics As Collection
  Dim cNaicsName As Collection
  Dim cUSic As Collection
  Dim cUSicName As Collection
  Dim initializing As Boolean

#Region " Windows Form Designer generated code "

  Public Sub New()
    MyBase.New()

    initializing = True
    'This call is required by the Windows Form Designer.
    InitializeComponent()
    initializing = False

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
  Friend WithEvents lbxSic As System.Windows.Forms.ListBox
  Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
  Friend WithEvents rbtNumber As System.Windows.Forms.RadioButton
  Friend WithEvents rbtName As System.Windows.Forms.RadioButton
  Friend WithEvents agdSic As AxATCoCtl.AxATCoGrid
  Friend WithEvents lblSic As System.Windows.Forms.Label
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
    Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmSic))
    Me.lbxSic = New System.Windows.Forms.ListBox
    Me.GroupBox1 = New System.Windows.Forms.GroupBox
    Me.lblSic = New System.Windows.Forms.Label
    Me.rbtName = New System.Windows.Forms.RadioButton
    Me.rbtNumber = New System.Windows.Forms.RadioButton
    Me.agdSic = New AxATCoCtl.AxATCoGrid
    Me.GroupBox1.SuspendLayout()
    CType(Me.agdSic, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SuspendLayout()
    '
    'lbxSic
    '
    Me.lbxSic.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lbxSic.ItemHeight = 16
    Me.lbxSic.Location = New System.Drawing.Point(16, 16)
    Me.lbxSic.Name = "lbxSic"
    Me.lbxSic.Size = New System.Drawing.Size(288, 148)
    Me.lbxSic.TabIndex = 0
    '
    'GroupBox1
    '
    Me.GroupBox1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.GroupBox1.Controls.Add(Me.lblSic)
    Me.GroupBox1.Controls.Add(Me.rbtName)
    Me.GroupBox1.Controls.Add(Me.rbtNumber)
    Me.GroupBox1.Location = New System.Drawing.Point(320, 8)
    Me.GroupBox1.Name = "GroupBox1"
    Me.GroupBox1.Size = New System.Drawing.Size(232, 160)
    Me.GroupBox1.TabIndex = 1
    Me.GroupBox1.TabStop = False
    Me.GroupBox1.Text = "Search by:"
    '
    'lblSic
    '
    Me.lblSic.Location = New System.Drawing.Point(16, 88)
    Me.lblSic.Name = "lblSic"
    Me.lblSic.Size = New System.Drawing.Size(208, 64)
    Me.lblSic.TabIndex = 2
    '
    'rbtName
    '
    Me.rbtName.Location = New System.Drawing.Point(16, 56)
    Me.rbtName.Name = "rbtName"
    Me.rbtName.Size = New System.Drawing.Size(192, 16)
    Me.rbtName.TabIndex = 1
    Me.rbtName.Text = "SIC Name"
    '
    'rbtNumber
    '
    Me.rbtNumber.Checked = True
    Me.rbtNumber.Location = New System.Drawing.Point(16, 32)
    Me.rbtNumber.Name = "rbtNumber"
    Me.rbtNumber.Size = New System.Drawing.Size(192, 16)
    Me.rbtNumber.TabIndex = 0
    Me.rbtNumber.TabStop = True
    Me.rbtNumber.Text = "SIC Number"
    '
    'agdSic
    '
    Me.agdSic.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.agdSic.Enabled = True
    Me.agdSic.Location = New System.Drawing.Point(16, 184)
    Me.agdSic.Name = "agdSic"
    Me.agdSic.OcxState = CType(resources.GetObject("agdSic.OcxState"), System.Windows.Forms.AxHost.State)
    Me.agdSic.Size = New System.Drawing.Size(536, 142)
    Me.agdSic.TabIndex = 11
    '
    'frmSic
    '
    Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
    Me.ClientSize = New System.Drawing.Size(568, 338)
    Me.Controls.Add(Me.agdSic)
    Me.Controls.Add(Me.GroupBox1)
    Me.Controls.Add(Me.lbxSic)
    Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
    Me.Name = "frmSic"
    Me.Text = "Standard Industrial Classification Codes (SIC) Lookup Table"
    Me.GroupBox1.ResumeLayout(False)
    CType(Me.agdSic, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ResumeLayout(False)

  End Sub

#End Region

  Public Sub InitializeUI(ByVal projectfilename As String)
    pProjectFileName = projectfilename
  End Sub

  Public Sub ReadDatabase()
    Dim SicFile As String
    Dim i As Integer

    Me.Refresh()
    Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
    SicFile = PathNameOnly(pProjectFileName) & "\sic.dbf"

    cSic = New Collection
    cSicName = New Collection
    cNaics = New Collection
    cNaicsName = New Collection
    cUSic = New Collection
    cUSicName = New Collection

    If FileExists(SicFile) Then
      Dim tmpDbf As IATCTable
      tmpDbf = atcUtility.atcTableOpener.OpenAnyTable(SicFile)
      For i = 1 To tmpDbf.NumRecords
        tmpDbf.CurrentRecord = i
        cSic.Add(tmpDbf.Value(1))
        cSicName.Add(tmpDbf.Value(2))
        If tmpDbf.Value(3) = "******" Then
          cNaics.Add("")
        Else
          cNaics.Add(tmpDbf.Value(3))
        End If
        cNaicsName.Add(tmpDbf.Value(4))
      Next i
    Else
      lblSic.Text = "Standard Industrial Classification Lookup is not available"
    End If

    'build collections of unique names
    On Error Resume Next
    For i = 1 To cSic.Count
      cUSic.Add(cSic(i), cSic(i))
      cUSicName.Add(cSicName(i), cSicName(i))
    Next i

    rbtName.Checked = True
    Cursor.Current = System.Windows.Forms.Cursors.Default
    lblSic.Text = "SIC search will also return the corresponding 1997 North American Industry Classification System (NAICS) code."
  End Sub

  Private Sub frmSic_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

    With agdSic
      .set_header("")
      .rows = 0
      .set_ColTitle(0, "SIC 1987")
      .set_ColTitle(1, "SIC Name")
      .set_ColTitle(2, "NAICS 1997")
      .set_ColTitle(3, "NAICS Name")
      .set_ColEditable(0, False)
      .set_ColEditable(1, False)
      .set_ColEditable(2, False)
      .set_ColEditable(3, False)
    End With
    lblSic.Text = "Reading Database ..."
    Me.Refresh()

  End Sub

  Private Sub rbtName_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtName.CheckedChanged
    Dim i As Integer
    Dim j As Integer
    Dim inlist As Boolean

    If Not initializing And rbtName.Checked Then
      agdSic.rows = 0
      lbxSic.Items.Clear()
      For i = 1 To cUSicName.Count
        lbxSic.Items.Add(cUSicName(i))
      Next i
    End If
  End Sub

  Private Sub rbtNumber_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtNumber.CheckedChanged
    Dim i As Integer
    Dim j As Integer
    Dim inlist As Boolean

    If Not initializing And rbtNumber.Checked Then
      agdSic.rows = 0
      lbxSic.Items.Clear()
      For i = 1 To cUSic.Count
        lbxSic.Items.Add(cUSic(i))
      Next i
    End If
  End Sub

  Private Sub lbxSic_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbxSic.SelectedIndexChanged
    'look through each database record for a match
    Dim i As Integer

    With agdSic
      .rows = 0
      For i = 1 To cSic.Count
        If rbtName.Checked Then
          If lbxSic.SelectedItem = cSicName(i) Then
            'found one
            .rows = .rows + 1
            .set_TextMatrix(.rows, 0, cSic(i))
            .set_TextMatrix(.rows, 1, cSicName(i))
            .set_TextMatrix(.rows, 2, cNaics(i))
            .set_TextMatrix(.rows, 3, cNaicsName(i))
          End If
        Else
          If lbxSic.SelectedItem = cSic(i) Then
            'found one
            .rows = .rows + 1
            .set_TextMatrix(.rows, 0, cSic(i))
            .set_TextMatrix(.rows, 1, cSicName(i))
            .set_TextMatrix(.rows, 2, cNaics(i))
            .set_TextMatrix(.rows, 3, cNaicsName(i))
          End If
        End If
      Next
      .ColsSizeByContents()
    End With

  End Sub
End Class
