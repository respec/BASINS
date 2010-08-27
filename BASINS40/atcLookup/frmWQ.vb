Imports atcControls
Imports atcUtility
Imports MapWinUtility
Imports System.Drawing

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
    Friend WithEvents agdWQ As atcControls.atcGrid
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmWQ))
        Me.lblSelect = New System.Windows.Forms.Label
        Me.lbxWQ = New System.Windows.Forms.ListBox
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.lblParm = New System.Windows.Forms.Label
        Me.lblCas = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.agdWQ = New atcControls.atcGrid
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblSelect
        '
        Me.lblSelect.Location = New System.Drawing.Point(12, 9)
        Me.lblSelect.Name = "lblSelect"
        Me.lblSelect.Size = New System.Drawing.Size(200, 14)
        Me.lblSelect.TabIndex = 0
        Me.lblSelect.Text = "Select a monitoring parameter:"
        '
        'lbxWQ
        '
        Me.lbxWQ.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lbxWQ.Location = New System.Drawing.Point(12, 26)
        Me.lbxWQ.Name = "lbxWQ"
        Me.lbxWQ.Size = New System.Drawing.Size(204, 121)
        Me.lbxWQ.TabIndex = 1
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.lblParm)
        Me.GroupBox1.Controls.Add(Me.lblCas)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Location = New System.Drawing.Point(222, 20)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(194, 152)
        Me.GroupBox1.TabIndex = 2
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Parameter Information:"
        '
        'lblParm
        '
        Me.lblParm.Location = New System.Drawing.Point(113, 83)
        Me.lblParm.Name = "lblParm"
        Me.lblParm.Size = New System.Drawing.Size(74, 14)
        Me.lblParm.TabIndex = 3
        '
        'lblCas
        '
        Me.lblCas.Location = New System.Drawing.Point(113, 55)
        Me.lblCas.Name = "lblCas"
        Me.lblCas.Size = New System.Drawing.Size(67, 14)
        Me.lblCas.TabIndex = 2
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(7, 83)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(100, 14)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Parameter Code:"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(13, 55)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(94, 14)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "CAS Number:"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'agdWQ
        '
        Me.agdWQ.AllowHorizontalScrolling = True
        Me.agdWQ.AllowNewValidValues = False
        Me.agdWQ.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.agdWQ.CellBackColor = System.Drawing.Color.Empty
        Me.agdWQ.LineColor = System.Drawing.Color.Empty
        Me.agdWQ.LineWidth = 0.0!
        Me.agdWQ.Location = New System.Drawing.Point(12, 178)
        Me.agdWQ.Name = "agdWQ"
        Me.agdWQ.Size = New System.Drawing.Size(404, 137)
        Me.agdWQ.Source = Nothing
        Me.agdWQ.TabIndex = 3
        '
        'frmWQ
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(428, 327)
        Me.Controls.Add(Me.agdWQ)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.lbxWQ)
        Me.Controls.Add(Me.lblSelect)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmWQ"
        Me.Text = "Water Quality Lookup Table"
        Me.GroupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub frmWQ_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim wqFile As String
        Dim i As Integer
        Dim lBasinsBinLoc As String

        agdWQ.Source = New atcGridSource
        agdWQ.Clear()
        With agdWQ.Source
            .Rows = 7
            .Columns = 3
            .FixedRows = 1
            .FixedColumns = 1
            .ColorCells = True
            .CellColor(0, 0) = SystemColors.ControlDark
            .CellColor(0, 1) = SystemColors.ControlDark
            .CellColor(0, 2) = SystemColors.ControlDark

            .CellColor(1, 0) = SystemColors.ControlDark
            .CellColor(2, 0) = SystemColors.ControlDark
            .CellColor(3, 0) = SystemColors.ControlDark
            .CellColor(4, 0) = SystemColors.ControlDark
            .CellColor(5, 0) = SystemColors.ControlDark
            .CellColor(6, 0) = SystemColors.ControlDark

            .CellValue(0, 0) = "Name"
            .CellValue(0, 1) = "Value"
            .CellValue(0, 2) = "Units"

            .CellValue(1, 0) = "Freshwater Acute"
            .CellValue(2, 0) = "Freshwater Chronic"
            .CellValue(3, 0) = "Marine Acute"
            .CellValue(4, 0) = "Marine Chronic"
            .CellValue(5, 0) = "HHRV Water"
            .CellValue(6, 0) = "HHRV Organ"
        End With

        Dim lBasinsFolder As String = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\AQUA TERRA Consultants\BASINS", "Base Directory", "C:\Basins")
        If pProjectFileName Is Nothing Then pProjectFileName = lBasinsFolder & "\data\national\national.mwprj"
        If Not FileExists(pProjectFileName) Then
            lBasinsBinLoc = PathNameOnly(System.Reflection.Assembly.GetEntryAssembly.Location)
            pProjectFileName = Mid(lBasinsBinLoc, 1, Len(lBasinsBinLoc) - 3) & "data\national\national.mwprj"
        End If
        wqFile = FindFile("WQ Database", PathNameOnly(PathNameOnly(pProjectFileName)) & "\national\wqcriter.dbf")

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
            Dim tmpDbf As IatcTable
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
        Dim lIndex As Integer = lbxWQ.SelectedIndex + 1
        lblParm.Text = cParmCode(lIndex)
        lblCas.Text = cCasNumber(lIndex)
        With agdWQ.Source
            .CellValue(1, 1) = cFAcute(lIndex)
            .CellValue(2, 1) = cFCronic(lIndex)
            .CellValue(3, 1) = cMAcute(lIndex)
            .CellValue(4, 1) = cMCronic(lIndex)
            .CellValue(5, 1) = cWater(lIndex)
            .CellValue(6, 1) = cOrgan(lIndex)
            .CellValue(1, 2) = cUnits(lIndex)
            .CellValue(2, 2) = cUnits(lIndex)
            .CellValue(3, 2) = cUnits(lIndex)
            .CellValue(4, 2) = cUnits(lIndex)
            .CellValue(5, 2) = cUnits(lIndex)
            .CellValue(6, 2) = cUnits(lIndex)
        End With
        agdWQ.SizeAllColumnsToContents()
        agdWQ.Refresh()
    End Sub
End Class
