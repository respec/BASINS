Imports atcUtility

Public Class frmChangeProjection
  Inherits System.Windows.Forms.Form
  Dim pProjectFileName As String

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
  Friend WithEvents cmdOK As System.Windows.Forms.Button
  Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
  Friend WithEvents cmdIProj As System.Windows.Forms.Button
  Friend WithEvents cmdILayer As System.Windows.Forms.Button
  Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
  Friend WithEvents lblOProj As System.Windows.Forms.Label
  Friend WithEvents cmdOProj As System.Windows.Forms.Button
  Friend WithEvents lblOLayer As System.Windows.Forms.Label
  Friend WithEvents cmdOLayer As System.Windows.Forms.Button
  Friend WithEvents lblILayer As System.Windows.Forms.Label
  Friend WithEvents lblIProj As System.Windows.Forms.Label
    Friend WithEvents ofdInput As System.Windows.Forms.OpenFileDialog
    Friend WithEvents sfdOutput As System.Windows.Forms.SaveFileDialog
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmChangeProjection))
        Me.cmdOK = New System.Windows.Forms.Button
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.lblIProj = New System.Windows.Forms.Label
        Me.cmdIProj = New System.Windows.Forms.Button
        Me.lblILayer = New System.Windows.Forms.Label
        Me.cmdILayer = New System.Windows.Forms.Button
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.lblOProj = New System.Windows.Forms.Label
        Me.cmdOProj = New System.Windows.Forms.Button
        Me.lblOLayer = New System.Windows.Forms.Label
        Me.cmdOLayer = New System.Windows.Forms.Button
        Me.ofdInput = New System.Windows.Forms.OpenFileDialog
        Me.sfdOutput = New System.Windows.Forms.SaveFileDialog
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdOK
        '
        Me.cmdOK.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdOK.DialogResult = System.Windows.Forms.DialogResult.Cancel
    Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.Location = New System.Drawing.Point(160, 272)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(88, 24)
        Me.cmdOK.TabIndex = 0
        Me.cmdOK.Text = "&OK"
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.lblIProj)
        Me.GroupBox1.Controls.Add(Me.cmdIProj)
        Me.GroupBox1.Controls.Add(Me.lblILayer)
        Me.GroupBox1.Controls.Add(Me.cmdILayer)
        Me.GroupBox1.Location = New System.Drawing.Point(16, 16)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(384, 112)
        Me.GroupBox1.TabIndex = 9
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Input"
        '
        'lblIProj
        '
        Me.lblIProj.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblIProj.Location = New System.Drawing.Point(136, 72)
        Me.lblIProj.Name = "lblIProj"
        Me.lblIProj.Size = New System.Drawing.Size(240, 24)
        Me.lblIProj.TabIndex = 12
        Me.lblIProj.Text = "<none>"
        Me.lblIProj.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmdIProj
        '
        Me.cmdIProj.Location = New System.Drawing.Point(16, 72)
        Me.cmdIProj.Name = "cmdIProj"
        Me.cmdIProj.Size = New System.Drawing.Size(104, 24)
        Me.cmdIProj.TabIndex = 11
        Me.cmdIProj.Text = "Set Projection"
        '
        'lblILayer
        '
        Me.lblILayer.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblILayer.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lblILayer.Location = New System.Drawing.Point(136, 24)
        Me.lblILayer.Name = "lblILayer"
        Me.lblILayer.Size = New System.Drawing.Size(240, 24)
        Me.lblILayer.TabIndex = 10
        Me.lblILayer.Text = "<none>"
        Me.lblILayer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmdILayer
        '
        Me.cmdILayer.Location = New System.Drawing.Point(16, 24)
        Me.cmdILayer.Name = "cmdILayer"
        Me.cmdILayer.Size = New System.Drawing.Size(104, 24)
        Me.cmdILayer.TabIndex = 9
        Me.cmdILayer.Text = "Set Layer"
        '
        'GroupBox2
        '
        Me.GroupBox2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox2.Controls.Add(Me.lblOProj)
        Me.GroupBox2.Controls.Add(Me.cmdOProj)
        Me.GroupBox2.Controls.Add(Me.lblOLayer)
        Me.GroupBox2.Controls.Add(Me.cmdOLayer)
        Me.GroupBox2.Location = New System.Drawing.Point(16, 144)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(384, 112)
        Me.GroupBox2.TabIndex = 10
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Output"
        '
        'lblOProj
        '
        Me.lblOProj.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblOProj.Location = New System.Drawing.Point(136, 72)
        Me.lblOProj.Name = "lblOProj"
        Me.lblOProj.Size = New System.Drawing.Size(240, 24)
        Me.lblOProj.TabIndex = 12
        Me.lblOProj.Text = "<none>"
        Me.lblOProj.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmdOProj
        '
        Me.cmdOProj.Location = New System.Drawing.Point(16, 72)
        Me.cmdOProj.Name = "cmdOProj"
        Me.cmdOProj.Size = New System.Drawing.Size(104, 24)
        Me.cmdOProj.TabIndex = 11
        Me.cmdOProj.Text = "Set Projection"
        '
        'lblOLayer
        '
        Me.lblOLayer.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblOLayer.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lblOLayer.Location = New System.Drawing.Point(136, 24)
        Me.lblOLayer.Name = "lblOLayer"
        Me.lblOLayer.Size = New System.Drawing.Size(240, 24)
        Me.lblOLayer.TabIndex = 10
        Me.lblOLayer.Text = "<none>"
        Me.lblOLayer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'cmdOLayer
        '
        Me.cmdOLayer.Location = New System.Drawing.Point(16, 24)
        Me.cmdOLayer.Name = "cmdOLayer"
        Me.cmdOLayer.Size = New System.Drawing.Size(104, 24)
        Me.cmdOLayer.TabIndex = 9
        Me.cmdOLayer.Text = "Set Layer"
        '
        'ofdInput
        '
        Me.ofdInput.Title = "Set Input Layer Name"
        '
        'sfdOutput
        '
        Me.sfdOutput.Title = "Set Output Layer Name"
        '
        'frmChangeProjection
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
        Me.ClientSize = New System.Drawing.Size(416, 312)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.cmdOK)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmChangeProjection"
        Me.Text = "Change Projection"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

  Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
    Dim success As Boolean
    Dim ilen As Integer

    success = False
    If lblILayer.Text <> "<none>" And lblIProj.Text <> "<none>" And _
       lblOLayer.Text <> "<none>" And lblOProj.Text <> "<none>" Then
      If Microsoft.VisualBasic.Right(lblILayer.Text, 4) = ".shp" Then
        'shapefile case
        If FileExists(lblOLayer.Text) Then
          'remove output file
          System.IO.File.Delete(lblOLayer.Text)
          ilen = Len(lblOLayer.Text)
          System.IO.File.Delete(Mid(lblOLayer.Text, 1, ilen - 3) & "dbf")
          System.IO.File.Delete(Mid(lblOLayer.Text, 1, ilen - 3) & "shx")
        End If
        success = MapWinX.SpatialReference.ProjectShapefile(lblIProj.Text, lblOProj.Text, lblILayer.Text, lblOLayer.Text)
        If Not success Then
          MsgBox("Unsuccessful projection attempt", MsgBoxStyle.OKOnly, "Change Projection Problem")
        End If
      ElseIf Microsoft.VisualBasic.Right(lblILayer.Text, 4) = ".bgd" Or Microsoft.VisualBasic.Right(lblILayer.Text, 4) = ".tif" Then
        'grid case
        If FileExists(lblOLayer.Text) Then
          'remove output file
          System.IO.File.Delete(lblOLayer.Text)
        End If
        success = MapWinX.SpatialReference.ProjectGrid(lblIProj.Text, lblOProj.Text, lblILayer.Text, lblOLayer.Text, True)
        If Not success Then
          MsgBox("Unsuccessful projection attempt", MsgBoxStyle.OKOnly, "Change Projection Problem")
        End If
      Else
        MsgBox("This file format is not supported in the Change Projection Utility tool.", MsgBoxStyle.OKOnly, "Change Projection Problem")
      End If
    End If
    If success Then
      Me.Close()
    End If
  End Sub

  Private Sub frmChangeProjection_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
    Dim ProjectionFile As String
    ProjectionFile = PathNameOnly(pProjectFileName) & "\prj.proj"
  End Sub

  Public Sub InitializeUI(ByVal projectfilename As String)
    pProjectFileName = projectfilename
  End Sub

  Private Sub cmdIProj_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdIProj.Click
    Dim ctemp As String
    
    ctemp = Methods.AskUser
    lblIProj.Text = CleanUpUserProjString(ctemp)
  End Sub

  Private Sub cmdOProj_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOProj.Click
    Dim ctemp As String

    ctemp = Methods.AskUser
    lblOProj.Text = CleanUpUserProjString(ctemp)
  End Sub

  Private Function CleanUpUserProjString(ByVal ctemp As String) As String
    Dim ipos As Integer
    Dim first As Boolean

    Do While Mid(ctemp, 1, 1) = "#"
      'eliminate comment lines at beginning
      ipos = InStr(ctemp, vbCrLf)
      ctemp = Mid(ctemp, ipos + 2)
    Loop
    first = True
    Do While InStr(ctemp, vbCrLf) > 0
      'strip out unneeded stuff
      ipos = InStr(ctemp, vbCrLf)
      If first Then
        ctemp = Mid(ctemp, ipos + 2)
        first = False
      Else
        ctemp = Mid(ctemp, 1, ipos - 1) & " " & Mid(ctemp, ipos + 2)
      End If
    Loop
    If InStr(ctemp, " end ") > 0 Then
      ctemp = Mid(ctemp, 1, InStr(ctemp, " end ") - 1)
    End If
    If Len(ctemp) > 0 Then
      If Mid(ctemp, 1, 9) = "+proj=dd " Then
        ctemp = "+proj=longlat"
        ctemp = ctemp & " +datum=NAD83"
      Else
        ctemp = ctemp & " +datum=NAD83 +units=m"
      End If
    Else
      ctemp = "<none>"
    End If
    CleanUpUserProjString = ctemp
  End Function

  Private Sub cmdILayer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdILayer.Click
    If lblOLayer.Text = "<none>" Then
      ofdInput.DefaultExt = "shp"
      ofdInput.Filter = "Shapefiles (*.shp)|*.shp|Binary grids (*.bgd)|*.bgd|GeoTiff (*.tif)|*.tif"
      ofdInput.FileName = "*.shp"
    ElseIf Microsoft.VisualBasic.Right(UCase(lblOLayer.Text), 3) = "SHP" Then
      ofdInput.DefaultExt = "shp"
      ofdInput.Filter = "Shapefiles (*.shp)|*.shp"
      ofdInput.FileName = "*.shp"
    ElseIf Microsoft.VisualBasic.Right(UCase(lblOLayer.Text), 3) = "BGD" Then
      ofdInput.DefaultExt = "bgd"
      ofdInput.Filter = "Binary grids (*.bgd)|*.bgd"
      ofdInput.FileName = "*.bgd"
    ElseIf Microsoft.VisualBasic.Right(UCase(lblOLayer.Text), 3) = "TIF" Then
      ofdInput.DefaultExt = "tif"
      ofdInput.Filter = "GeoTiff (*.tif)|*.tif"
      ofdInput.FileName = "*.tif"
    End If
    If ofdInput.ShowDialog() = DialogResult.OK Then
      lblILayer.Text = ofdInput.FileName
    End If
  End Sub

  Private Sub cmdOLayer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOLayer.Click
    If lblILayer.Text = "<none>" Then
      sfdOutput.DefaultExt = "shp"
      sfdOutput.Filter = "Shapefiles (*.shp)|*.shp|Binary grids (*.bgd)|*.bgd|GeoTiff (*.tif)|*.tif"
      sfdOutput.FileName = "*.shp"
    ElseIf Microsoft.VisualBasic.Right(UCase(lblILayer.Text), 3) = "SHP" Then
      sfdOutput.DefaultExt = "shp"
      sfdOutput.Filter = "Shapefiles (*.shp)|*.shp"
      sfdOutput.FileName = "*.shp"
    ElseIf Microsoft.VisualBasic.Right(UCase(lblILayer.Text), 3) = "BGD" Then
      sfdOutput.DefaultExt = "bgd"
      sfdOutput.Filter = "Binary grids (*.bgd)|*.bgd"
      sfdOutput.FileName = "*.bgd"
    ElseIf Microsoft.VisualBasic.Right(UCase(lblILayer.Text), 3) = "TIF" Then
      sfdOutput.DefaultExt = "tif"
      sfdOutput.Filter = "GeoTiff (*.tif)|*.tif"
      sfdOutput.FileName = "*.tif"
    End If
    If sfdOutput.ShowDialog() = DialogResult.OK Then
      lblOLayer.Text = sfdOutput.FileName
    End If
  End Sub
End Class
