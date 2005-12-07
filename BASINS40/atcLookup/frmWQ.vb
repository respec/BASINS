Imports atcUtility

Public Class frmWQ
  Inherits System.Windows.Forms.Form
  Dim pProjectFileName As String
  Dim cCasNumber As Collection
  Dim cParmCode As Collection
  Dim cFAcute As Collection
  Dim cFCronic As Collection
  Dim cMAcute As Collection
  Dim cMCronic As Collection
  Dim cWater As Collection
  Dim cOrgan As Collection
  Dim cUnits As Collection


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
  Friend WithEvents lbxWQ As System.Windows.Forms.ListBox
  Friend WithEvents lblSelect As System.Windows.Forms.Label
  Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
  Friend WithEvents Label1 As System.Windows.Forms.Label
  Friend WithEvents Label2 As System.Windows.Forms.Label
  Friend WithEvents lblCas As System.Windows.Forms.Label
  Friend WithEvents lblParm As System.Windows.Forms.Label
  Friend WithEvents agdWQ As AxATCoCtl.AxATCoGrid
  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
    Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmWQ))
    Me.lblSelect = New System.Windows.Forms.Label
    Me.lbxWQ = New System.Windows.Forms.ListBox
    Me.GroupBox1 = New System.Windows.Forms.GroupBox
    Me.lblParm = New System.Windows.Forms.Label
    Me.lblCas = New System.Windows.Forms.Label
    Me.Label2 = New System.Windows.Forms.Label
    Me.Label1 = New System.Windows.Forms.Label
    Me.agdWQ = New AxATCoCtl.AxATCoGrid
    Me.GroupBox1.SuspendLayout()
    CType(Me.agdWQ, System.ComponentModel.ISupportInitialize).BeginInit()
    Me.SuspendLayout()
    '
    'lblSelect
    '
    Me.lblSelect.Location = New System.Drawing.Point(16, 16)
    Me.lblSelect.Name = "lblSelect"
    Me.lblSelect.Size = New System.Drawing.Size(240, 16)
    Me.lblSelect.TabIndex = 0
    Me.lblSelect.Text = "Select a monitoring parameter:"
    '
    'lbxWQ
    '
    Me.lbxWQ.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.lbxWQ.ItemHeight = 16
    Me.lbxWQ.Location = New System.Drawing.Point(16, 40)
    Me.lbxWQ.Name = "lbxWQ"
    Me.lbxWQ.Size = New System.Drawing.Size(232, 148)
    Me.lbxWQ.TabIndex = 1
    '
    'GroupBox1
    '
    Me.GroupBox1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.GroupBox1.Controls.Add(Me.lblParm)
    Me.GroupBox1.Controls.Add(Me.lblCas)
    Me.GroupBox1.Controls.Add(Me.Label2)
    Me.GroupBox1.Controls.Add(Me.Label1)
    Me.GroupBox1.Location = New System.Drawing.Point(264, 16)
    Me.GroupBox1.Name = "GroupBox1"
    Me.GroupBox1.Size = New System.Drawing.Size(232, 176)
    Me.GroupBox1.TabIndex = 2
    Me.GroupBox1.TabStop = False
    Me.GroupBox1.Text = "Parameter Information:"
    '
    'lblParm
    '
    Me.lblParm.Location = New System.Drawing.Point(136, 96)
    Me.lblParm.Name = "lblParm"
    Me.lblParm.Size = New System.Drawing.Size(88, 16)
    Me.lblParm.TabIndex = 3
    '
    'lblCas
    '
    Me.lblCas.Location = New System.Drawing.Point(136, 64)
    Me.lblCas.Name = "lblCas"
    Me.lblCas.Size = New System.Drawing.Size(80, 16)
    Me.lblCas.TabIndex = 2
    '
    'Label2
    '
    Me.Label2.Location = New System.Drawing.Point(8, 96)
    Me.Label2.Name = "Label2"
    Me.Label2.Size = New System.Drawing.Size(120, 16)
    Me.Label2.TabIndex = 1
    Me.Label2.Text = "Parameter Code:"
    Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
    '
    'Label1
    '
    Me.Label1.Location = New System.Drawing.Point(16, 64)
    Me.Label1.Name = "Label1"
    Me.Label1.Size = New System.Drawing.Size(112, 16)
    Me.Label1.TabIndex = 0
    Me.Label1.Text = "CAS Number:"
    Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
    '
    'agdWQ
    '
    Me.agdWQ.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.agdWQ.Enabled = True
    Me.agdWQ.Location = New System.Drawing.Point(16, 208)
    Me.agdWQ.Name = "agdWQ"
    Me.agdWQ.OcxState = CType(resources.GetObject("agdWQ.OcxState"), System.Windows.Forms.AxHost.State)
    Me.agdWQ.Size = New System.Drawing.Size(480, 144)
    Me.agdWQ.TabIndex = 12
    '
    'frmWQ
    '
    Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
    Me.ClientSize = New System.Drawing.Size(514, 378)
    Me.Controls.Add(Me.agdWQ)
    Me.Controls.Add(Me.GroupBox1)
    Me.Controls.Add(Me.lbxWQ)
    Me.Controls.Add(Me.lblSelect)
    Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
    Me.Name = "frmWQ"
    Me.Text = "Water Quality Lookup Table"
    Me.GroupBox1.ResumeLayout(False)
    CType(Me.agdWQ, System.ComponentModel.ISupportInitialize).EndInit()
    Me.ResumeLayout(False)

  End Sub

#End Region

  Private Sub frmWQ_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
    Dim wqFile As String
    Dim i As Integer

    With agdWQ
      .set_header("")
      .rows = 6
      .set_ColTitle(0, "Name")
      .set_ColTitle(1, "Value")
      .set_ColTitle(2, "Units")
      .set_ColEditable(0, False)
      .set_ColEditable(1, False)
      .set_ColEditable(2, False)
      .set_TextMatrix(1, 0, "Freshwater Acute")
      .set_TextMatrix(2, 0, "Freshwater Cronic")
      .set_TextMatrix(3, 0, "Marine Acute")
      .set_TextMatrix(4, 0, "Marine Cronic")
      .set_TextMatrix(5, 0, "HHRV Water")
      .set_TextMatrix(6, 0, "HHRV Organ")
    End With

    wqFile = PathNameOnly(pProjectFileName) & "\wqcriter.dbf"

    cCasNumber = New Collection
    cParmCode = New Collection
    cFAcute = New Collection
    cFCronic = New Collection
    cMAcute = New Collection
    cMCronic = New Collection
    cWater = New Collection
    cOrgan = New Collection
    cUnits = New Collection

    If FileExists(wqFile) Then
      Dim tmpDbf As IATCTable
      tmpDbf = atcUtility.atcTableOpener.OpenAnyTable(wqFile)
      For i = 1 To tmpDbf.NumRecords
        tmpDbf.CurrentRecord = i
        lbxWQ.Items.Add(tmpDbf.Value(3))
        cCasNumber.Add(tmpDbf.Value(2))
        cParmCode.Add(tmpDbf.Value(1))
        cUnits.Add(tmpDbf.Value(5))
        If tmpDbf.Value(6) = "" Then
          cFAcute.Add("Unknown")
        Else
          cFAcute.Add(tmpDbf.Value(6))
        End If
        If tmpDbf.Value(7) = "" Then
          cFCronic.Add("Unknown")
        Else
          cFCronic.Add(tmpDbf.Value(7))
        End If
        If tmpDbf.Value(8) = "" Then
          cMAcute.Add("Unknown")
        Else
          cMAcute.Add(tmpDbf.Value(8))
        End If
        If tmpDbf.Value(9) = "" Then
          cMCronic.Add("Unknown")
        Else
          cMCronic.Add(tmpDbf.Value(9))
        End If
        If tmpDbf.Value(12) = "" Then
          cWater.Add("Unknown")
        Else
          cWater.Add(tmpDbf.Value(12))
        End If
        If tmpDbf.Value(13) = "" Then
          cOrgan.Add("Unknown")
        Else
          cOrgan.Add(tmpDbf.Value(13))
        End If
      Next i
      lbxWQ.SelectedIndex = 0
    Else
      lblSelect.Text = "Water Quality Criteria table is not available"
    End If
  End Sub

  Public Sub InitializeUI(ByVal projectfilename As String)
    pProjectFileName = projectfilename
  End Sub

  Private Sub lbxWQ_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbxWQ.SelectedIndexChanged
    lblParm.Text = cParmCode(lbxWQ.SelectedIndex + 1)
    lblCas.Text = cCasNumber(lbxWQ.SelectedIndex + 1)
    With agdWQ
      .set_TextMatrix(1, 1, cFAcute(lbxWQ.SelectedIndex + 1))
      .set_TextMatrix(2, 1, cFCronic(lbxWQ.SelectedIndex + 1))
      .set_TextMatrix(3, 1, cMAcute(lbxWQ.SelectedIndex + 1))
      .set_TextMatrix(4, 1, cMCronic(lbxWQ.SelectedIndex + 1))
      .set_TextMatrix(5, 1, cWater(lbxWQ.SelectedIndex + 1))
      .set_TextMatrix(6, 1, cOrgan(lbxWQ.SelectedIndex + 1))
      .set_TextMatrix(1, 2, cUnits(lbxWQ.SelectedIndex + 1))
      .set_TextMatrix(2, 2, cUnits(lbxWQ.SelectedIndex + 1))
      .set_TextMatrix(3, 2, cUnits(lbxWQ.SelectedIndex + 1))
      .set_TextMatrix(4, 2, cUnits(lbxWQ.SelectedIndex + 1))
      .set_TextMatrix(5, 2, cUnits(lbxWQ.SelectedIndex + 1))
      .set_TextMatrix(6, 2, cUnits(lbxWQ.SelectedIndex + 1))
    End With
  End Sub
End Class
