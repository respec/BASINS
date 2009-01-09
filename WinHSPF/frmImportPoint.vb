Imports atcControls
Imports atcData
Imports atcUCI
Imports atcUCIForms
Imports atcUtility
Imports MapWinUtility
Imports WinHSPF
Imports System.IO
Imports System.Windows.Forms

Public Class frmImportPoint

    Dim facilityname$, nconstits$, consnames$(), ndates&
    Dim jdates!(), rloads!()
    Dim retcod As Integer

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        Me.Icon = pIcon
        Me.MinimumSize = Me.Size
        Me.MaximumSize = Me.Size

        Dim lHspfOperation As HspfOperation
        Dim lHspfOpnBlk As HspfOpnBlk
        'Dim vpol As Object, vfac As Object
        Dim i, lOper2 As Integer
        Dim ctmp As String
        Dim lFoundFlag As Boolean

        cboReach.Items.Clear()
        lHspfOpnBlk = pUCI.OpnBlks("RCHRES")
        For i = 0 To lHspfOpnBlk.Count - 1
            lHspfOperation = lHspfOpnBlk.Ids(i)
            cboReach.Items.Add("RCHRES " & lHspfOperation.Id & " - " & lHspfOperation.Description)
        Next i
        cboReach.SelectedIndex = 0

        cboPollutant.Items.Clear()
        'For Each vpol In PollutantList
        '  cboPollutant.AddItem vpol
        'Next vpol
        For i = 1 To frmPoint.agdMasterPoint.Source.Rows - 1
            ctmp = frmPoint.agdMasterPoint.Source.CellValue(i, 4)

            'search cboPollutant for lTempString
            lFoundFlag = False
            For lOper2 = 0 To cboPollutant.Items.Count - 1
                If cboPollutant.Items.Item(lOper2) = ctmp Then
                    lFoundFlag = True
                    Exit For
                End If
            Next

            If Not lFoundFlag Then
                cboPollutant.Items.Add(ctmp)
            End If
        Next i

        cboFac.Items.Clear()
        'For i = 1 To frmPoint.cmbFac.ListCount - 1
        '  cboFac.AddItem frmPoint.cmbFac.List(i)
        'Next i
        For i = 1 To pfrmPoint.agdMasterPoint.Source.Rows - 1
            ctmp = pfrmPoint.agdMasterPoint.Source.CellValue(i, 3)


            'search cboFac for lTempString
            lFoundFlag = False
            For lOper2 = 0 To cboFac.Items.Count - 1
                If cboFac.Items.Item(lOper2) = ctmp Then
                    lFoundFlag = True
                    Exit For
                End If
            Next

            If Not lFoundFlag Then
                cboFac.Items.Add(ctmp)
            End If
        Next i

        retcod = -1

    End Sub

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Me.Dispose()
    End Sub

    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
        Me.Dispose()
    End Sub

    Private Sub cmdFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdFile.Click
        Dim lFileName As String = Nothing

        'Dim i&, s$, f$, fun&, wid$
        'Dim tmetseg, ret&

        Try
            OpenFileDialog1.InitialDirectory = System.Reflection.Assembly.GetEntryAssembly.Location
            OpenFileDialog1.Filter = "MUTSIN Files in BASINS 2 Format(*.mut)|*.mut"
            OpenFileDialog1.FileName = "*.mut"
            OpenFileDialog1.Title = "Select MUTSIN File"

            If OpenFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                lFileName = OpenFileDialog1.FileName
            Else
                Exit Try
            End If

            lblFile.Text = OpenFileDialog1.FileName
            'read mustin file
            Call ReadMutsin(lFileName, facilityname, retcod, nconstits, consnames, ndates, jdates, rloads)
            cboFac.Text = facilityname

        Catch ex As Exception
            pPollutantList.Clear()
            Logger.Message("There was an error reading the selected pollutant list." & vbCrLf & "Ensure that the pollutant file selected is formatted properly.", "Error Reading the pollutant file", MessageBoxButtons.OK, MessageBoxIcon.Error, Windows.Forms.DialogResult.OK)
        End Try
    End Sub

    Public Sub ReadMutsin(ByVal aFileName As String, ByVal facname$, ByVal ret&, ByVal ncons$, ByVal cons$(), ByVal ndate&, ByVal jdates!(), ByVal rVal!())

        Dim delim$, quote$, i&
        'Dim tname$, amax&, tstr$, tcnt&
        Dim lLineString As String
        Dim reccnt&, ilen&, nhead&, j&
        Dim idate(6) As Integer

        ret = 0
        delim = " "
        quote = """"

        'read mut file
        Try
            Using sr As StreamReader = New StreamReader(aFileName)

                lLineString = sr.ReadLine() 'line with number of header lines

                reccnt = 0

                reccnt += 1
                ilen = Len(lLineString)
                If IsNumeric(Mid(lLineString, 43, 5)) Then
                    nhead = CInt(Mid(lLineString, 43, 5))
                Else
                    nhead = 25
                End If

                lLineString = sr.ReadLine()  'line with facility name
                reccnt = reccnt + 1
                facname = Trim(lLineString)

                For j = 1 To 5
                    lLineString = sr.ReadLine()  'unused lines
                    reccnt = reccnt + 1
                Next j

                ncons = 0
                Do Until Len(lLineString) = 0
                    lLineString = sr.ReadLine()  'read cons
                    reccnt = reccnt + 1
                    If Len(Trim(lLineString)) > 0 Then
                        ncons = ncons + 1
                        ReDim Preserve cons(ncons)
                        cons(ncons) = Trim(lLineString)
                    End If
                Loop

                For j = reccnt + 1 To nhead
                    lLineString = sr.ReadLine()  'unused lines
                Next j

                ndate = 0
                Do Until EOF(i)
                    lLineString = sr.ReadLine()
                    ndate = ndate + 1
                    ReDim Preserve jdates(ndate)
                    ReDim Preserve rVal(ndate * ncons)
                    idate(0) = StrSplit(lLineString, delim, quote)
                    idate(1) = StrSplit(lLineString, delim, quote)
                    idate(2) = StrSplit(lLineString, delim, quote)
                    idate(3) = StrSplit(lLineString, delim, quote)
                    idate(4) = StrSplit(lLineString, delim, quote)
                    idate(5) = 0
                    jdates(ndate) = Date2J(idate)
                    For j = 1 To ncons
                        rVal(((ndate - 1) * ncons) + j) = StrSplit(lLineString, delim, quote)
                    Next j
                Loop
                sr.Close()
            End Using
            Exit Sub
        Catch Ex As Exception
            Logger.Message("Error Opening File", "Import Problem", MessageBoxButtons.OK, MessageBoxIcon.Error, Windows.Forms.DialogResult.OK)
            ret = 1
        End Try

    End Sub

End Class