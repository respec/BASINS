Imports VB = Microsoft.VisualBasic
Friend Class frmRunSimulation
	Inherits System.Windows.Forms.Form
	
	'******************************************************************************
	'   Application: Mercury Model - Hydrology
	'   File Name:   Dialog box - FrmRunSimulation
	'   Purpose:     The starting point of the application, prompting the
	'                user to enter required parameters
	'   Author:      Ragothaman Bhimarao
	'   Modification History:   Created - 11/04/2005
	'
	'
	'******************************************************************************
	
    '   Private Const BIF_RETURNONLYFSDIRS As Short = 1
    'Private Const BIF_DONTGOBELOWDOMAIN As Short = 2
    'Private Const MAX_PATH As Short = 260

    '   Private Declare Function SHBrowseForFolder Lib "shell32" (ByRef lpbi As BrowseInfo) As Integer

    'Private Declare Function SHGetPathFromIDList Lib "shell32" (ByVal pidList As Integer, ByVal lpBuffer As String) As Integer

    'Private Declare Function lstrcat Lib "kernel32"  Alias "lstrcatA"(ByVal lpString1 As String, ByVal lpString2 As String) As Integer

    'Private Structure BrowseInfo
    '	Dim hwndOwner As Integer
    '	Dim pIDLRoot As Integer
    '	Dim pszDisplayName As Integer
    '	Dim lpszTitle As Integer
    '	Dim ulFlags As Integer
    '	Dim lpfnCallback As Integer
    '	Dim lParam As Integer
    '	Dim iImage As Integer
    'End Structure
	

	Private range() As String
	Private dryhgrange() As String
	Private wethgrange() As String

	Private Sub cmdCancel_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdCancel.Click
        Me.Close()
	End Sub
	
    Private Sub btnPath_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles btnDataPath.Click, btnOutputPath.Click
        Dim txt As TextBox = Controls(CType(eventSender, Button).Name.Replace("btn", "txt"))
        With New FolderBrowserDialog
            .SelectedPath = txt.Text
            .ShowNewFolderButton = True
            If .ShowDialog() = Windows.Forms.DialogResult.OK Then
                txt.Text = .SelectedPath
            End If
        End With
    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdOK.Click
        Try
            If dtEndSim.Value.Subtract(dtStartSim.Value).TotalDays < 0 Or dtEnd.Value.Subtract(dtStart.Value).TotalDays < 0 Or dtStartDry.Value.Subtract(dtEndDry.Value).TotalDays < 0 Or dtStartWet.Value.Subtract(dtEndWet.Value).TotalDays < 0 Then
                WarningMsg("All start dates must precede their corresponding end dates; please check your input data.")
                Exit Sub
            End If

            If Not IsNumeric(txtWhAEMDuration.Text) Then
                Exit Sub
            Else
                Dim duration As Double = dtEndSim.Value.Subtract(dtStartSim.Value).TotalDays + 1
                If txtWhAEMDuration.Text < 1 Then
                    WarningMsg("Average Duration for Groundwater Recharge cannot be less than 1 day")
                    Exit Sub
                ElseIf txtWhAEMDuration.Text > duration Then
                    WarningMsg("Average Duration for Groundwater Recharge cannot be more than than {0:0} days", duration)
                    Exit Sub
                End If
            End If

            If ComputeMercuryFlag Then
                If InputDataDictionary("chkTime") Then
                    If dtEndSim.Value.Subtract(dtStartSim.Value).TotalDays < 364 Then
                        If Not (dtStartSim.Value = dtStartDry.Value And dtEndSim.Value = dtEndDry.Value And _
                                dtStartSim.Value = dtStartWet.Value And dtEndSim.Value = dtEndWet.Value And _
                                dtStartSim.Value = dtStart.Value And dtEndSim.Value = dtEnd.Value) Then
                            WarningMsg("Selected simulation date range is less than a year; the Clmate, Dry Hg, and Wet Hg date ranges must all match the simulation date range.")
                            Exit Sub
                        End If
                    End If
                End If
            End If

            If InputFileName.Text = "" Then
                WarningMsg("Provide a name for the input file.")
                Exit Sub
            End If

            If txtDataPath.Text = "" Or txtOutputPath.Text = "" Then
                WarningMsg("Enter valid folder paths.")
                Exit Sub
            End If

            If Not My.Computer.FileSystem.DirectoryExists(txtDataPath.Text) Then My.Computer.FileSystem.CreateDirectory(txtDataPath.Text)
            If Not My.Computer.FileSystem.DirectoryExists(txtOutputPath.Text) Then My.Computer.FileSystem.CreateDirectory(txtOutputPath.Text)

            If modFormInteract.BlankCheck(Me) Then
                modFormInteract.WritetoDict(Me)
                modFormInteract.SaveDict()
            End If

            Close()

            Dim strpath As String = txtDataPath.Text
            If Not strpath.EndsWith("\") Then strpath &= "\"
            With New frmProcess
                .Show()
                ModClipper.CreateCalibrationdataNew()
                modNewUtilities.ConvertAscii(strpath)
                modNewUtilities.Convertascii2(strpath)
                modNewUtilities.CopyTextToTemp(strpath)
                ModCreateInputFile.WriteInputTextFile()
                modNewUtilities.CleanTempShapes()
                modNewUtilities.CleanTempRasters()
                .Close()
                .Dispose()
            End With
            MessageBox.Show(String.Format("Input file {0}.inp created in {1}", InputFileName.Text, txtOutputPath.Text), "Run Simulation", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            ErrorMsg(, ex)
        End Try
    End Sub

    Private Sub frmRunSimulation_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        Try
            modFormInteract.Filer()
            '  Initialize  'Initialize the map variable

            'Initialize globals
            InitializeInputDataDictionary()
            modInit.Initialize()

            'Get Climatestation info
            modCreateInputFile.CreateDict()

            Dim tmplines() As String = ExtractStationIDInfo.split(vbNewLine)

            Dim tmdict As New Generic.Dictionary(Of String, String)
            For Each s As String In templines
                Dim tmstr() As String = s.Split(vbTab)
                tmdict.Add(tmstr(2), tmstr(0))
            Next

            Dim range() As String = modNewUtilities.FindRange(tmdict, "ClimateSummary.txt").split("-")

            SSTab1.TabPages.Item(2).Enabled = False
            fraDryHg.Enabled = False
            fraWetHg.Enabled = False
            Dim chkTime As Boolean = False

            If InputDataDictionary("cbxMercury") Then
                If InputDataDictionary.TryGetValue("chkTime", chkTime) AndAlso chkTime Then
                    If Not (My.Computer.FileSystem.FileExists(gMapInputFolder & "\dryhgsummary.txt") Or My.Computer.FileSystem.FileExists(gMapInputFolder & "\wethgsummary.txt")) Then
                        WarningMsg("Mercury Atmospheric Deposition Time Series are selected.\nRun 'Process Input Grids'\nDefine Assessment points (optional) and Delineate Watershed\nCreate GBMM Input file")
                        Close()
                        Exit Sub
                    End If

                    SSTab1.TabPages.Item(2).Enabled = True
                    fraDryHg.Enabled = True
                    fraWetHg.Enabled = True
                    Dim dryhgrange() As String = modNewUtilities.FindRange(tmdict, "dryhgsummary.txt").split("-")
                    dtStartDry.Value = CDate(dryhgrange(0))
                    dtEndDry.Value = CDate(dryhgrange(0)).AddDays(1)
                    dtStartDry.MinDate = CDate(dryhgrange(0))
                    dtStartDry.MaxDate = CDate(dryhgrange(1))
                    dtEndDry.MinDate = CDate(dryhgrange(0))
                    dtEndDry.MaxDate = CDate(dryhgrange(1))

                    lblDateRange2.Text = String.Format("Available Hg Deposition data: ({0:MM/dd/yyyy} - {1:MM/dd/yyyy})", dryhgrange(0), dryhgrange(1))

                    Dim wethgrange() As String = modNewUtilities.FindRange(tmdict, "wethgsummary.txt").split("-")
                    dtStartWet.Value = CDate(wethgrange(0))
                    dtEndWet.Value = CDate(wethgrange(0)).AddDays(1)
                    dtStartWet.MinDate = CDate(wethgrange(0))
                    dtStartWet.MaxDate = CDate(wethgrange(1))
                    dtEndWet.MinDate = CDate(wethgrange(0))
                    dtEndWet.MaxDate = CDate(wethgrange(1))

                    lblDateRange3.Text = String.Format("Available Hg Deposition data: ({0:MM/dd/yyyy} - {1:MM/dd/yyyy})", wethgrange(0), wethgrange(1))
                Else
                    SSTab1.TabPages.Item(2).Enabled = False
                    fraDryHg.Enabled = False
                    fraWetHg.Enabled = False
                End If
            Else
                SSTab1.TabPages.Item(2).Enabled = False
                fraDryHg.Enabled = False
                fraWetHg.Enabled = False
            End If
            LblDateRange.Text = String.Format("Available Climate data: ({0:MM/dd/yyyy} - {1:MM/dd/yyyy})", range(0), range(1))
            dtStartSim.Value = CDate(range(0))
            dtEndSim.Value = dtStartSim.Value.AddDays(1)
            dtStart.MinDate = CDate(range(0))
            dtEnd.MinDate = CDate(range(0))
            dtStart.MaxDate = CDate(range(1))
            dtEnd.MaxDate = CDate(range(1))

            dtStart.Value = CDate(range(0))
            dtEnd.Value = dtStart.Value.AddDays(1)
            txtDataPath.Text = gApplicationPath & "\INPUT"
            txtOutputPath.Text = gApplicationPath


            If InputDataDictionary("cbxWhAEM") Then
                fraWhAEM.Enabled = True
                whaemLabel.Enabled = True
                txtWhAEMDuration.Enabled = True
                txtWhAEMDuration.Text = dtEndSim.Value.Subtract(dtStartSim.Value).TotalDays + 1
            Else
                fraWhAEM.Enabled = False
                whaemLabel.Enabled = False
                txtWhAEMDuration.Enabled = False
            End If
        Catch ex As Exception
            ErrorMsg(, ex)
        End Try
    End Sub

    Private Sub dtStartSim_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtStartSim.ValueChanged, dtEndSim.ValueChanged
        Dim duration As Double = dtEndSim.Value.Subtract(dtStartSim.Value).TotalDays
        If duration < 1 Then
            txtWhAEMDuration.Text = 1
        Else
            txtWhAEMDuration.Text = duration + 1
        End If
    End Sub
End Class