Imports atcUtility
Imports atcControls
Imports atcMwGisUtility
Imports MapWinUtility

Public Class frmReport
    Inherits System.Windows.Forms.Form

    Dim pPlugIn As atcReport.PlugIn

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()
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
    Friend WithEvents lblAreaOfInterest As System.Windows.Forms.Label
    Friend WithEvents cboSub1 As System.Windows.Forms.ComboBox
    Friend WithEvents cboSub2 As System.Windows.Forms.ComboBox
    Friend WithEvents cmdNext As System.Windows.Forms.Button
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents lblSubid As System.Windows.Forms.Label
    Friend WithEvents lblSubname As System.Windows.Forms.Label
    Friend WithEvents lblAvailableReports As System.Windows.Forms.Label
    Friend WithEvents lbxReports As System.Windows.Forms.ListBox
    Friend WithEvents lblReportFolder As System.Windows.Forms.Label
    Friend WithEvents lblFolder As System.Windows.Forms.Label
    Friend WithEvents fbdFolder As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents cboAreas As System.Windows.Forms.ComboBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmReport))
        Me.cboAreas = New System.Windows.Forms.ComboBox
        Me.lblAreaOfInterest = New System.Windows.Forms.Label
        Me.cboSub1 = New System.Windows.Forms.ComboBox
        Me.lblSubid = New System.Windows.Forms.Label
        Me.lblSubname = New System.Windows.Forms.Label
        Me.cboSub2 = New System.Windows.Forms.ComboBox
        Me.cmdNext = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.lbxReports = New System.Windows.Forms.ListBox
        Me.lblAvailableReports = New System.Windows.Forms.Label
        Me.lblReportFolder = New System.Windows.Forms.Label
        Me.lblFolder = New System.Windows.Forms.Label
        Me.fbdFolder = New System.Windows.Forms.FolderBrowserDialog
        Me.SuspendLayout()
        '
        'cboAreas
        '
        Me.cboAreas.AllowDrop = True
        Me.cboAreas.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboAreas.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboAreas.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboAreas.Location = New System.Drawing.Point(165, 12)
        Me.cboAreas.Name = "cboAreas"
        Me.cboAreas.Size = New System.Drawing.Size(287, 21)
        Me.cboAreas.TabIndex = 12
        '
        'lblAreaOfInterest
        '
        Me.lblAreaOfInterest.AutoSize = True
        Me.lblAreaOfInterest.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAreaOfInterest.Location = New System.Drawing.Point(14, 15)
        Me.lblAreaOfInterest.Name = "lblAreaOfInterest"
        Me.lblAreaOfInterest.Size = New System.Drawing.Size(134, 13)
        Me.lblAreaOfInterest.TabIndex = 10
        Me.lblAreaOfInterest.Text = "Area of Interest Layer:"
        '
        'cboSub1
        '
        Me.cboSub1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboSub1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSub1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboSub1.Location = New System.Drawing.Point(165, 39)
        Me.cboSub1.Name = "cboSub1"
        Me.cboSub1.Size = New System.Drawing.Size(287, 21)
        Me.cboSub1.TabIndex = 18
        '
        'lblSubid
        '
        Me.lblSubid.AutoSize = True
        Me.lblSubid.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSubid.Location = New System.Drawing.Point(31, 42)
        Me.lblSubid.Name = "lblSubid"
        Me.lblSubid.Size = New System.Drawing.Size(55, 13)
        Me.lblSubid.TabIndex = 17
        Me.lblSubid.Text = "ID Field:"
        '
        'lblSubname
        '
        Me.lblSubname.AutoSize = True
        Me.lblSubname.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSubname.Location = New System.Drawing.Point(31, 69)
        Me.lblSubname.Name = "lblSubname"
        Me.lblSubname.Size = New System.Drawing.Size(74, 13)
        Me.lblSubname.TabIndex = 21
        Me.lblSubname.Text = "Name Field:"
        '
        'cboSub2
        '
        Me.cboSub2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboSub2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSub2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cboSub2.Location = New System.Drawing.Point(165, 66)
        Me.cboSub2.Name = "cboSub2"
        Me.cboSub2.Size = New System.Drawing.Size(287, 21)
        Me.cboSub2.TabIndex = 22
        '
        'cmdNext
        '
        Me.cmdNext.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdNext.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdNext.Location = New System.Drawing.Point(296, 408)
        Me.cmdNext.Name = "cmdNext"
        Me.cmdNext.Size = New System.Drawing.Size(75, 28)
        Me.cmdNext.TabIndex = 23
        Me.cmdNext.Text = "Generate"
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdCancel.Location = New System.Drawing.Point(377, 408)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(75, 28)
        Me.cmdCancel.TabIndex = 24
        Me.cmdCancel.Text = "Cancel"
        '
        'lbxReports
        '
        Me.lbxReports.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lbxReports.Location = New System.Drawing.Point(34, 125)
        Me.lbxReports.Name = "lbxReports"
        Me.lbxReports.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
        Me.lbxReports.Size = New System.Drawing.Size(418, 225)
        Me.lbxReports.TabIndex = 25
        '
        'lblAvailableReports
        '
        Me.lblAvailableReports.AutoSize = True
        Me.lblAvailableReports.Location = New System.Drawing.Point(14, 96)
        Me.lblAvailableReports.Name = "lblAvailableReports"
        Me.lblAvailableReports.Size = New System.Drawing.Size(111, 13)
        Me.lblAvailableReports.TabIndex = 26
        Me.lblAvailableReports.Text = "Available Reports:"
        '
        'lblReportFolder
        '
        Me.lblReportFolder.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblReportFolder.AutoSize = True
        Me.lblReportFolder.Location = New System.Drawing.Point(14, 371)
        Me.lblReportFolder.Name = "lblReportFolder"
        Me.lblReportFolder.Size = New System.Drawing.Size(88, 13)
        Me.lblReportFolder.TabIndex = 27
        Me.lblReportFolder.Text = "Report Folder:"
        '
        'lblFolder
        '
        Me.lblFolder.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblFolder.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblFolder.Location = New System.Drawing.Point(110, 370)
        Me.lblFolder.Name = "lblFolder"
        Me.lblFolder.Size = New System.Drawing.Size(342, 20)
        Me.lblFolder.TabIndex = 28
        Me.lblFolder.Text = "C:\BASINS\reports\"
        '
        'frmReport
        '
        Me.AcceptButton = Me.cmdNext
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 13)
        Me.CancelButton = Me.cmdCancel
        Me.ClientSize = New System.Drawing.Size(464, 448)
        Me.Controls.Add(Me.lblFolder)
        Me.Controls.Add(Me.lblReportFolder)
        Me.Controls.Add(Me.lblAvailableReports)
        Me.Controls.Add(Me.lbxReports)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdNext)
        Me.Controls.Add(Me.cboSub2)
        Me.Controls.Add(Me.lblSubname)
        Me.Controls.Add(Me.cboSub1)
        Me.Controls.Add(Me.lblSubid)
        Me.Controls.Add(Me.cboAreas)
        Me.Controls.Add(Me.lblAreaOfInterest)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmReport"
        Me.Text = "BASINS Watershed Characterization Reports"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Me.Close()
    End Sub

    Private Sub cboSubbasins_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboAreas.SelectedIndexChanged
        Dim lLayer As Long
        Dim i As Long
        Dim lString As String

        cboSub1.Items.Clear()
        cboSub2.Items.Clear()
        If cboAreas.Items(cboAreas.SelectedIndex) = "<none>" Then
            cboSub1.Visible = False
            cboSub2.Visible = False
            lblSubid.Visible = False
            lblSubname.Visible = False
        Else
            cboSub1.Visible = True
            cboSub2.Visible = True
            lblSubid.Visible = True
            lblSubname.Visible = True
            lLayer = GisUtil.LayerIndex(cboAreas.Items(cboAreas.SelectedIndex))
            If lLayer > -1 Then
                For i = 0 To GisUtil.NumFields(lLayer) - 1
                    lString = GisUtil.FieldName(i, lLayer)
                    cboSub1.Items.Add(lString)
                    cboSub2.Items.Add(lString)
                    If UCase(lString) = "SUBBASIN" Or UCase(lString) = "CU" Then
                        cboSub1.SelectedIndex = i
                    End If
                    If UCase(lString) = "NAME" Or UCase(lString) = "BNAME" Then
                        cboSub2.SelectedIndex = i
                    End If
                Next
            End If
            If cboSub1.Items.Count > 0 And cboSub1.SelectedIndex < 0 Then
                cboSub1.SelectedIndex = 0
            End If
            If cboSub2.Items.Count > 0 And cboSub2.SelectedIndex < 0 Then
                cboSub2.SelectedIndex = 0
            End If
        End If
    End Sub

    Private Sub cmdNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdNext.Click
        Dim lAreaLayerName As String
        Dim lAreaIDFieldName As String
        Dim lAreaNameFieldName As String

        If lbxReports.SelectedItems.Count = 0 Then
            Logger.Msg("At least one report must be selected to generate.", MsgBoxStyle.OkOnly, "BASINS Report Problem")
        Else 'set arguments for scripting
            If cboAreas.SelectedIndex > -1 Then
                lAreaLayerName = cboAreas.Items(cboAreas.SelectedIndex)
            Else
                lAreaLayerName = "<none>"
            End If
            If cboSub1.SelectedIndex > -1 Then
                lAreaIDFieldName = cboSub1.Items(cboSub1.SelectedIndex)
            Else
                lAreaIDFieldName = ""
            End If
            If cboSub2.SelectedIndex > -1 Then
                lAreaNameFieldName = cboSub2.Items(cboSub2.SelectedIndex)
            Else
                lAreaNameFieldName = ""
            End If

            'make sure output folder exists
            If Not FileExists(lblFolder.Text, True, False) Then
                MkDirPath(lblFolder.Text)
            End If

            'now run each script
            For i As Integer = 0 To lbxReports.SelectedItems.Count - 1
                Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
                Dim lOutput As Object
                Dim lProblem As String = ""
                Dim lOutputGridSource As New atcGridSource
                lOutput = pPlugIn.BuildReport(lAreaLayerName, _
                                              lAreaIDFieldName, _
                                              lAreaNameFieldName, _
                                              lbxReports.SelectedIndices(i) + 1)
                Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
                Try
                    lOutputGridSource = lOutput
                Catch
                    lProblem = lOutput
                End Try
                Dim lTitle1 As String = "Watershed Characterization Report"
                Dim lTitle2 As String = IO.Path.GetFileNameWithoutExtension(pPlugIn.Reports(lbxReports.SelectedIndices(i) + 1))
                Dim lReportFilename As String = lblFolder.Text & lTitle2 & ".txt"
                If Not lOutputGridSource Is Nothing And Len(lProblem) = 0 Then
                    'write file
                    SaveFileString(lReportFilename, lTitle1 & vbCrLf & "  " & lTitle2 & vbCrLf & vbCrLf & lOutputGridSource.ToString)

                    'form showing output
                    Dim lfrmResult As New frmResult
                    lfrmResult.InitializeResults(lTitle1, lTitle2, lReportFilename, lOutputGridSource)
                    lfrmResult.Show()
                Else
                    Logger.Msg("atcReport:" & lTitle2 & vbCrLf & lProblem, "BASINS Report Problem")
                End If
            Next i

        End If

    End Sub

    Public Sub InitializeUI(ByVal aPlugIn As Object)
        pPlugIn = aPlugIn
        cboAreas.Items.Add("<none>")

        For lLayer As Long = 0 To GisUtil.NumLayers() - 1
            Dim lString As String = GisUtil.LayerName(lLayer)
            If GisUtil.LayerType(lLayer) = 3 Then
                'PolygonShapefile 
                cboAreas.Items.Add(lString)
                If GisUtil.CurrentLayer = lLayer Then
                    cboAreas.SelectedIndex = cboAreas.Items.Count - 1
                End If
                If UCase(lString) = "SUBBASINS" And cboAreas.SelectedIndex < 0 Then
                    cboAreas.SelectedIndex = cboAreas.Items.Count - 1
                End If
                If UCase(lString) = "CATALOGING UNIT BOUNDARIES" And cboAreas.SelectedIndex < 0 Then
                    cboAreas.SelectedIndex = cboAreas.Items.Count - 1
                End If
            End If
        Next
        If cboAreas.Items.Count > 0 And cboAreas.SelectedIndex < 0 Then
            cboAreas.SelectedIndex = 0
        End If

        For Each lReport As String In pPlugIn.Reports
            lbxReports.Items.Add(IO.Path.GetFileNameWithoutExtension(lReport))
        Next lReport

        Dim lBasinsBinLoc As String = PathNameOnly(System.Reflection.Assembly.GetEntryAssembly.Location)
        lblFolder.Text = Mid(lBasinsBinLoc, 1, Len(lBasinsBinLoc) - 3) & "Reports\"

    End Sub

    Private Sub lblFolder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblFolder.Click
        fbdFolder.ShowDialog()
        lblFolder.Text = fbdFolder.SelectedPath
        If Microsoft.VisualBasic.Right(lblFolder.Text, 1) <> "/" And _
           Microsoft.VisualBasic.Right(lblFolder.Text, 1) <> "\" Then
            lblFolder.Text = lblFolder.Text & "\"
        End If
    End Sub

    Public Overrides Function ToString() As String
        Dim lAllReports As String = ""
        For lReportIndex As Integer = 1 To pPlugIn.Reports.Count
            Dim lReport As String = IO.Path.GetFileNameWithoutExtension(pPlugIn.Reports(lReportIndex))
            lAllReports &= lReport & vbCrLf
            'Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
            'Dim lOutput As Object
            'Dim lProblem As String = ""
            'Dim lOutputGridSource As New atcGridSource
            'lOutput = pPlugIn.BuildReport(lAreaLayerName, _
            '                              lAreaIDFieldName, _
            '                              lAreaNameFieldName, lReportIndex)
            'Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
            'Try
            '  lOutputGridSource = lOutput
            'Catch
            '  lProblem = lOutput
            'End Try
            'Dim lTitle1 As String = "Watershed Characterization Report"
            'Dim lTitle2 As String = IO.Path.GetFileNameWithoutExtension(pPlugIn.Reports(lReportIndex))
            'If Not lOutputGridSource Is Nothing And Len(lProblem) = 0 Then
            '  'write file
            '  lAllReports &= lTitle1 & vbCrLf & "  " _
            '               & lTitle2 & vbCrLf & vbCrLf _
            '               & lOutputGridSource.ToString)
            'Else
            '  Logger.Msg("atcReport:" & lTitle2 & vbCrLf & lProblem, "BASINS Report Problem")
            'End If
        Next
        Return lAllReports
    End Function

    Private Sub frmReport_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyValue = Windows.Forms.Keys.F1 Then
            ShowHelp("BASINS Details\Watershed Characterization Reports.html")
        End If
    End Sub
End Class
