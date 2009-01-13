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

    Dim pFacilityName As String
    Dim pConstituentNames As New Collection
    Dim pNumberOfDates, pFileReadStatus As Integer 'lFileReadStatus (-1 = not read, 0 = read w/ no problem, 1 = error)
    Dim pLoadValuesCollection, pJDatesCollection As New Collection

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

        pFileReadStatus = -1


    End Sub

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Me.Dispose()
    End Sub

    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
        Dim sen$, loc$, con$, stanam$, tstype$
        Dim imready As Boolean
        Dim dashpos&, i&, j&, newdsn&, longloc$
        Dim newwdmid As Integer = 0
        Dim lJDatesArray() As Double
        Dim lLoadValuesArray() As Double


        imready = True

        If pFileReadStatus <> 0 Then
            MsgBox("An input file be specified.", vbOKOnly, "Import Point Source Problem")
            imready = False
        Else
            If Len(txtScen.Text) = 0 Then
                MsgBox("A scenario name must be entered.", vbOKOnly, "Import Point Source Problem")
                imready = False
            End If
            If Len(cboReach.Items.Item(cboReach.SelectedIndex)) = 0 Then
                MsgBox("A reach must be selected.", vbOKOnly, "Import Point Source Problem")
                imready = False
            End If

            If imready Then

                sen = "PT-" & UCase(Trim(txtScen.Text))
                longloc = Trim(cboReach.Items.Item(cboReach.SelectedIndex))
                dashpos = InStr(1, longloc, "-")
                loc = "RCH" & Trim(Mid(longloc, 7, dashpos - 7))
                For i = 1 To pConstituentNames.Count

                    con = UCase(pConstituentNames.Item(i))
                    If Len(con) > 8 Then
                        con = Trim(Mid(con, 1, 8))
                    End If
                    stanam = UCase(pFacilityName)
                    tstype = Mid(con, 1, 4)

                    ReDim lJDatesArray(pJDatesCollection.Count)
                    ReDim lLoadValuesArray(pConstituentNames.Count)

                    For j = 1 To pJDatesCollection.Count
                        lJDatesArray(j) = pJDatesCollection.Item(j)
                    Next j

                    For j = 1 To pConstituentNames.Count
                        lLoadValuesArray(j) = pLoadValuesCollection.Item((i - 1) * pConstituentNames.Count + j)
                        MsgBox(pLoadValuesCollection.Item((i - 1) * pConstituentNames.Count + j))
                    Next

                    pUCI.AddPointSourceDataSet(sen, loc, con, stanam, tstype, lJDatesArray.Length - 1, lJDatesArray, lLoadValuesArray, newwdmid, newdsn)
                    pfrmPoint.UpdateListsForNewPointSource(sen, stanam, loc, con, newwdmid, newdsn, "RCHRES", CInt(Mid(loc, 4)), longloc)
                Next i
            End If
        End If
        If imready Then
            Me.Dispose()
        End If
    End Sub

    Private Sub cmdFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdFile.Click
        Dim lFileName As String = Nothing
        Dim lDelimiter, lQuote, lLineString As String
        Dim lCurrentLineNumber, lOperLength, lHeaderCount, j, lDateArray(6) As Integer
        Dim lStreamReader As StreamReader

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

            lStreamReader = New StreamReader(lFileName)

            lDelimiter = " "
            lQuote = """"

            lLineString = lStreamReader.ReadLine() 'line with number of header lines

            lCurrentLineNumber = 0

            lCurrentLineNumber += 1
            lOperLength = Len(lLineString)
            If IsNumeric(Mid(lLineString, 43, 5)) Then
                lHeaderCount = CInt(Mid(lLineString, 43, 5))
            Else
                lHeaderCount = 25
            End If

            lLineString = lStreamReader.ReadLine()  'line with facility name
            lCurrentLineNumber += 1
            pFacilityName = Trim(lLineString)

            For j = 1 To 5
                lLineString = lStreamReader.ReadLine()  'unused lines
                lCurrentLineNumber += 1
            Next j

            Do Until Len(lLineString) = 0
                lLineString = lStreamReader.ReadLine()  'read cons
                lCurrentLineNumber += 1
                If Len(Trim(lLineString)) > 0 Then
                    pConstituentNames.Add(Trim(lLineString))
                End If
            Loop

            For j = lCurrentLineNumber + 1 To lHeaderCount
                lLineString = lStreamReader.ReadLine()  'unused lines
            Next j

            Do While lStreamReader.Peek() >= 0
                lLineString = lStreamReader.ReadLine()
                lDateArray(0) = StrSplit(lLineString, lDelimiter, lQuote)
                lDateArray(1) = StrSplit(lLineString, lDelimiter, lQuote)
                lDateArray(2) = StrSplit(lLineString, lDelimiter, lQuote)
                lDateArray(3) = StrSplit(lLineString, lDelimiter, lQuote)
                lDateArray(4) = StrSplit(lLineString, lDelimiter, lQuote)
                lDateArray(5) = 0
                pJDatesCollection.Add(Date2J(lDateArray))

                For j = 1 To pConstituentNames.Count
                    pLoadValuesCollection.Add(StrSplit(lLineString, lDelimiter, lQuote))
                Next j

            Loop

            lStreamReader.Close()

            pFileReadStatus = 0
            cboFac.Items.Add(pFacilityName)
            cboFac.SelectedIndex = cboFac.Items.Count - 1



        Catch ex As Exception
            Logger.Message("There was an error reading the selected MUSTIN file." & vbCrLf & "Ensure that the file selected is formatted properly.", "Error Reading MUSTIN file", MessageBoxButtons.OK, MessageBoxIcon.Error, Windows.Forms.DialogResult.OK)
            pFileReadStatus = 1
        End Try
    End Sub

End Class