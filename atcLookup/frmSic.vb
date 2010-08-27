Imports atcControls
Imports atcUtility
Imports MapWinUtility
Imports System.Drawing

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
    Friend WithEvents lblSic As System.Windows.Forms.Label
    Friend WithEvents agdSic As atcControls.atcGrid
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSic))
        Me.lbxSic = New System.Windows.Forms.ListBox
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.lblSic = New System.Windows.Forms.Label
        Me.rbtName = New System.Windows.Forms.RadioButton
        Me.rbtNumber = New System.Windows.Forms.RadioButton
        Me.agdSic = New atcControls.atcGrid
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'lbxSic
        '
        Me.lbxSic.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lbxSic.Location = New System.Drawing.Point(12, 12)
        Me.lbxSic.Name = "lbxSic"
        Me.lbxSic.Size = New System.Drawing.Size(239, 121)
        Me.lbxSic.TabIndex = 0
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.lblSic)
        Me.GroupBox1.Controls.Add(Me.rbtName)
        Me.GroupBox1.Controls.Add(Me.rbtNumber)
        Me.GroupBox1.Location = New System.Drawing.Point(257, 6)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(203, 139)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Search by:"
        '
        'lblSic
        '
        Me.lblSic.Location = New System.Drawing.Point(13, 76)
        Me.lblSic.Name = "lblSic"
        Me.lblSic.Size = New System.Drawing.Size(174, 56)
        Me.lblSic.TabIndex = 2
        '
        'rbtName
        '
        Me.rbtName.Location = New System.Drawing.Point(13, 49)
        Me.rbtName.Name = "rbtName"
        Me.rbtName.Size = New System.Drawing.Size(160, 18)
        Me.rbtName.TabIndex = 1
        Me.rbtName.Text = "SIC Name"
        '
        'rbtNumber
        '
        Me.rbtNumber.Checked = True
        Me.rbtNumber.Location = New System.Drawing.Point(13, 28)
        Me.rbtNumber.Name = "rbtNumber"
        Me.rbtNumber.Size = New System.Drawing.Size(160, 18)
        Me.rbtNumber.TabIndex = 0
        Me.rbtNumber.TabStop = True
        Me.rbtNumber.Text = "SIC Number"
        '
        'agdSic
        '
        Me.agdSic.AllowHorizontalScrolling = True
        Me.agdSic.AllowNewValidValues = False
        Me.agdSic.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.agdSic.CellBackColor = System.Drawing.Color.Empty
        Me.agdSic.LineColor = System.Drawing.Color.Empty
        Me.agdSic.LineWidth = 0.0!
        Me.agdSic.Location = New System.Drawing.Point(12, 152)
        Me.agdSic.Name = "agdSic"
        Me.agdSic.Size = New System.Drawing.Size(444, 128)
        Me.agdSic.Source = Nothing
        Me.agdSic.TabIndex = 2
        '
        'frmSic
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(472, 292)
        Me.Controls.Add(Me.agdSic)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.lbxSic)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmSic"
        Me.Text = "Standard Industrial Classification Codes (SIC) Lookup Table"
        Me.GroupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Public Sub InitializeUI(ByVal projectfilename As String)
        pProjectFileName = projectfilename
    End Sub

    Public Sub ReadDatabase()
        Dim SicFile As String
        Dim i As Integer
        Dim lBasinsBinLoc As String

        Me.Refresh()
        Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
        Dim lBasinsFolder As String = My.Computer.Registry.GetValue("HKEY_LOCAL_MACHINE\SOFTWARE\AQUA TERRA Consultants\BASINS", "Base Directory", "C:\Basins")
        If pProjectFileName Is Nothing Then pProjectFileName = lBasinsFolder & "\data\national\national.mwprj"
        If Not FileExists(pProjectFileName) Then
            lBasinsBinLoc = PathNameOnly(System.Reflection.Assembly.GetEntryAssembly.Location)
            pProjectFileName = Mid(lBasinsBinLoc, 1, Len(lBasinsBinLoc) - 3) & "data\national\national.mwprj"
        End If
        SicFile = FindFile("SIC Database", PathNameOnly(PathNameOnly(pProjectFileName)) & "\national\sic.dbf")

        cSic = New Collection
        cSicName = New Collection
        cNaics = New Collection
        cNaicsName = New Collection
        cUSic = New Collection
        cUSicName = New Collection

        If FileExists(SicFile) Then
            Dim tmpDbf As IatcTable
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
        Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
        lblSic.Text = "SIC search will also return the corresponding 1997 North American Industry Classification System (NAICS) code."
    End Sub

    Private Sub frmSic_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

        agdSic.Source = New atcGridSource
        agdSic.Clear()
        With agdSic.Source
            .Rows = 3
            .Columns = 4
            .FixedRows = 1
            .ColorCells = True
            .CellColor(0, 0) = SystemColors.ControlDark
            .CellColor(0, 1) = SystemColors.ControlDark
            .CellColor(0, 2) = SystemColors.ControlDark
            .CellColor(0, 3) = SystemColors.ControlDark
            .CellValue(0, 0) = "SIC 1987"
            .CellValue(0, 1) = "SIC Name"
            .CellValue(0, 2) = "NAICS 1997"
            .CellValue(0, 3) = "NAICS Name"
        End With
        lblSic.Text = "Reading Database ..."
        Me.Refresh()
    End Sub

    Private Sub rbtName_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtName.CheckedChanged
        If Not initializing And rbtName.Checked Then
            agdSic.Source.Rows = 1
            lbxSic.Items.Clear()
            For i As Integer = 1 To cUSicName.Count
                lbxSic.Items.Add(cUSicName(i))
            Next
            If lbxSic.Items.Count > 0 Then lbxSic.SelectedIndex = 0
        End If
    End Sub

    Private Sub rbtNumber_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtNumber.CheckedChanged
        If Not initializing And rbtNumber.Checked Then
            agdSic.Source.Rows = 1
            lbxSic.Items.Clear()
            For i As Integer = 1 To cUSic.Count
                lbxSic.Items.Add(cUSic(i))
            Next
            If lbxSic.Items.Count > 0 Then lbxSic.SelectedIndex = 0
        End If
    End Sub

    Private Sub lbxSic_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbxSic.SelectedIndexChanged
        'look through each database record for a match
        Dim i As Integer
        Dim lNextRow As Integer
        Dim lCurrentSic As String
        Dim lCompareName As Boolean = rbtName.Checked
        Dim lCompareCode As Boolean = Not lCompareName
        lCurrentSic = lbxSic.SelectedItem

        With agdSic.Source
            .Rows = 1
            For i = 1 To cSic.Count
                lNextRow = .Rows
                If (lCompareName AndAlso lCurrentSic = cSicName(i)) OrElse _
                   (lCompareCode AndAlso lCurrentSic = cSic(i)) Then
                    'found one
                    .Rows = lNextRow + 1
                    .CellValue(lNextRow, 0) = cSic(i)
                    .CellValue(lNextRow, 1) = cSicName(i)
                    .CellValue(lNextRow, 2) = cNaics(i)
                    .CellValue(lNextRow, 3) = cNaicsName(i)
                End If
            Next
        End With
        agdSic.SizeAllColumnsToContents()
        agdSic.Refresh()
    End Sub
End Class
