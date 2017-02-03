Imports atcUCI
Imports atcData
Imports atcControls
Imports atcUtility
Imports MapWinUtility
Imports System.Collections.ObjectModel


Public Class frmAddExpert

    Dim pCheckedRadioIndex As Integer
    Dim pListBox1DataItems As New Collection
    Dim pListBox2DataItems As New Collection

    Private Sub frmAddExpert_Resized(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.ResizeEnd
        If pCheckedRadioIndex = 1 Then 'applies to calib option
            lstOperation.Width = (Me.Width / 2) - 30
            lstGroup.Width = (Me.Width / 2) - 30
            lstGroup.Left = (Me.Width / 2)
            lblGroup.Left = lstGroup.Left
        End If
    End Sub

    Sub New(ByVal aCheckedRadioIndex As Integer)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.Icon = pIcon
        'Me.MinimumSize = Me.Size
        'Me.MaximumSize = Me.Size

        pCheckedRadioIndex = aCheckedRadioIndex

        Select Case pCheckedRadioIndex
            Case 1 'Hydrology Calibration
                txtDescription.Visible = False
                optH.Visible = False
                optD.Visible = False
                lblTimeStep.Visible = False
                optEnglish.Visible = False
                optMetric.Visible = False
                lblUnits.Visible = False
                lblGroup.Text = "Observed Flow Time Series:"
            Case 2 'Flow
                lstOperation.Size = New Size(350, 173)
                lstGroup.Visible = False
                txtDescription.Visible = False
                lblGroup.Visible = False
            Case 3 'AQUATOX Linkage
                lstOperation.Size = New Size(350, 173)
                lstGroup.Visible = False
                txtDescription.Visible = False
                lblGroup.Visible = False
            Case 5 'Copy All
                lblOperation.Text = "From Operation"
                lblGroup.Text = "To Operation"
                lstGroup.Items.Add("<select origin operation>")
                Me.Text = "WinHSPF - Copy Output"
                txtDescription.Visible = False
        End Select

        pListBox1DataItems.Clear()
        pListBox2DataItems.Clear()

        If pCheckedRadioIndex = 4 Then 'other types
            lstGroup.Enabled = False
            lblGroup.Enabled = False
            For lIndex As Integer = 0 To pUCI.OpnSeqBlock.Opns.Count - 1
                Dim lHspfOperation As HspfOperation = pUCI.OpnSeqBlock.Opn(lIndex)
                If lHspfOperation.Name = "RCHRES" Or lHspfOperation.Name = "PERLND" Or lHspfOperation.Name = "IMPLND" Or lHspfOperation.Name = "BMPRAC" Then
                    lstOperation.Items.Add(lHspfOperation.Name & " " & lHspfOperation.Id & " (" & lHspfOperation.Description & ")")
                    pListBox1DataItems.Add(lHspfOperation.Id)
                End If
            Next
        ElseIf pCheckedRadioIndex = 1 Or pCheckedRadioIndex = 2 Or pCheckedRadioIndex = 3 Then 'calib or flow or aquatox
            lstGroup.Enabled = False
            For lIndex As Integer = 0 To pUCI.OpnSeqBlock.Opns.Count - 1
                Dim lHspfOperation As HspfOperation = pUCI.OpnSeqBlock.Opn(lIndex)
                If lHspfOperation.Name = "RCHRES" Then
                    lstOperation.Items.Add(lHspfOperation.Name & " " & lHspfOperation.Id & " (" & lHspfOperation.Description & ")")
                    pListBox1DataItems.Add(lHspfOperation.Id)
                End If
            Next
        ElseIf pCheckedRadioIndex = 5 Then 'copy
            lstOperation.Items.Clear()
            lstGroup.Enabled = False

            For lIndex As Integer = 1 To pfrmOutput.agdOutput.Source.Rows - 1
                Dim lFoundFlag As Boolean = False
                For lIndex2 As Integer = 0 To lstOperation.Items.Count - 1
                    If pfrmOutput.agdOutput.Source.CellValue(lIndex, 0) = lstOperation.Items.Item(lIndex2) Then
                        lFoundFlag = True
                        Exit For
                    End If
                Next
                If Not lFoundFlag Then
                    lstOperation.Items.Add(pfrmOutput.agdOutput.Source.CellValue(lIndex, 0))
                End If
            Next
        End If

        If pCheckedRadioIndex = 1 Then 'calib 
            lstGroup.Enabled = True
            'look for candidate observed flow WDM datasets
            Dim lts As Collection = pUCI.FindTimser("OBSERVED", "", "FLOW")
            Dim lWDMFileName As String = ""
            For lWdmIndex As Integer = 1 To 4
                If Not pUCI.GetWDMObj(lWdmIndex) Is Nothing Then 'use this as the output wdm
                    lWDMFileName = pUCI.GetWDMObj(lWdmIndex).Specification
                    Exit For
                End If
            Next lWdmIndex
            For Each lTser As atcData.atcTimeseries In lts
                Dim lDsn As Integer = lTser.Attributes.GetValue("ID")
                Dim lLoc As String = lTser.Attributes.GetValue("Location")
                Dim lCon As String = lTser.Attributes.GetValue("Constituent")
                Dim lSen As String = lTser.Attributes.GetValue("Scenario")
                If lTser.Attributes.GetValue("Data Source") = lWDMFileName Then
                    lstGroup.Items.Add(lDsn & " : " & lSen & " " & lCon & " at " & lLoc)
                    pListBox2DataItems.Add(lDsn)
                End If
            Next
        End If

        If pCheckedRadioIndex = 2 Or pCheckedRadioIndex = 3 Or pCheckedRadioIndex = 4 Then
            'give option of output timeunits if hourly run, otherwise always daily
            If pUCI.OpnSeqBlock.Delt <= 60 Then
                optH.Enabled = True
                optD.Enabled = True
            End If
        End If

        txtLoc.Text = "<none>"

    End Sub

    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click

        Dim lDialogResult As Windows.Forms.DialogResult = Nothing

        Dim lOutTu As Integer
        Dim lOutputUnits As String = "ENGL"
        If optH.Checked Then
            lOutTu = 3  'hourly
        End If
        If optD.Checked Then
            lOutTu = 4  'daily
        End If
        If optMetric.Checked Then
            lOutputUnits = "METR"
        Else
            lOutputUnits = "ENGL"
        End If
        Dim lId As Integer
        Dim lOstr(28) As String
        Dim lDsns(28) As Integer
        If pCheckedRadioIndex = 1 Then 'add calib location
            If lstOperation.SelectedIndex > -1 Then
                lId = pListBox1DataItems.Item(lstOperation.SelectedIndex + 1)
                If Not pfrmOutput.IsCalibLocation("RCHRES", lId) Then
                    Me.Cursor = Cursors.WaitCursor
                    Dim lWDMId As Integer = 0
                    Dim lWDMFileName As String = ""
                    For lWdmIndex As Integer = 1 To 4
                        If Not pUCI.GetWDMObj(lWdmIndex) Is Nothing Then 'use this as the output wdm
                            lWDMId = lWdmIndex
                            lWDMFileName = pUCI.GetWDMObj(lWdmIndex).Specification
                            Exit For
                        End If
                    Next lWdmIndex

                    pUCI.AddExpertSystem(lId, txtLoc.Text, lWDMId, atxBase.Text, lDsns, lOstr)
                    pUCI.Edited = True
                    Me.Cursor = Cursors.Arrow

                    'create an EXS file for this location
                    If lstGroup.SelectedIndex < 0 Then
                        'there does not appear to be an observed flow data sets selected, give warning 
                        Logger.Message("No observed flow data set is selected." & vbCrLf & vbCrLf &
                                       "To use this location for hydrology calibration in HSPEXP+, " & vbCrLf &
                                       "observed flow data is required.", "Add Output Notice", MessageBoxButtons.OK, MessageBoxIcon.Information, Windows.Forms.DialogResult.OK)
                    Else
                        lDsns(0) = pListBox2DataItems.Item(lstGroup.SelectedIndex + 1)
                    End If
                    'now build and write the exs file
                    Dim lExpertSystem As HspfSupport.atcExpertSystem
                    lExpertSystem = New HspfSupport.atcExpertSystem(pUCI, "", pUCI.GlobalBlock.SDateJ, pUCI.GlobalBlock.EdateJ)
                    Dim lFileName As String = FilenameNoExt(pUCIFullFileName) & "-" & txtLoc.Text & ".exs"
                    lExpertSystem.CreateEXS(lFileName, FilenameNoExt(FilenameNoPath(lWDMFileName)), txtLoc.Text, lId, lDsns)

                End If
            End If

        ElseIf pCheckedRadioIndex = 2 Then 'add flow location
            If lstOperation.SelectedIndex > -1 Then
                lId = pListBox1DataItems.Item(lstOperation.SelectedIndex + 1)
                Dim lWDMId As Integer
                Dim lNewDsn As Integer
                pUCI.AddOutputWDMDataSetExt(txtLoc.Text, "FLOW", atxBase.Text, lWDMId, lOutTu, "", lNewDsn)
                pUCI.AddExtTarget("RCHRES", lId, "HYDR", "RO", 1, 1, 1.0#, "AVER",
                       "WDM" & CStr(lWDMId), lNewDsn, "FLOW", 1, lOutputUnits, "AGGR", "REPL")
                pUCI.Edited = True
            End If
        ElseIf pCheckedRadioIndex = 4 Then 'add this other output
            If lstOperation.SelectedIndex > -1 And lstGroup.SelectedIndex > -1 Then
                Dim lSpacePos As Integer = InStr(1, lstOperation.Items(lstOperation.SelectedIndex), " ")
                Dim lParenPos As Integer = InStr(1, lstOperation.Items(lstOperation.SelectedIndex), "(")
                If lSpacePos > 0 And lParenPos > 0 Then
                    'parse operation name and id
                    lId = CInt(Mid(lstOperation.Items(lstOperation.SelectedIndex), lSpacePos, lParenPos - lSpacePos))
                    Dim lOpName As String = Mid(lstOperation.Items(lstOperation.SelectedIndex), 1, lSpacePos - 1)
                    Dim lSTANAM As String = lstOperation.SelectedItem.ToString
                    If lId > 0 And Len(lOpName) > 0 Then
                        Dim lSub1 As Integer
                        Dim lSub2 As Integer
                        Dim lGroup As String
                        Dim lMem As String
                        Dim lTempMem As String
                        'parse member name
                        Dim lTmem As String = lstGroup.Items(lstGroup.SelectedIndex)
                        lParenPos = InStr(1, lTmem, "(")
                        Dim lColonPos As Integer = InStr(1, lTmem, ":")
                        If lParenPos > 0 Then 'has subscripts
                            Dim lcRem As String = Mid(lTmem, lParenPos)
                            Dim lCommaPos As String = InStr(1, lcRem, ",")
                            If lCommaPos > 0 Then
                                lSub1 = CInt(Mid(lcRem, 2, lCommaPos - 2))
                                lSub2 = CInt(Mid(lcRem, lCommaPos + 1, Len(lcRem) - lCommaPos - 1))
                            Else
                                lSub1 = CInt(Mid(lcRem, 2, Len(lcRem) - 1))
                                lSub2 = 1
                            End If
                            lMem = Mid(lTmem, lColonPos + 1, lParenPos - lColonPos - 1)
                            lGroup = Mid(lTmem, 1, lColonPos - 1)
                            lTempMem = lMem & CStr(lSub1)

                            '.net conversion note: This is implemented with the assumption that pfrmAddExpert is opened with pfrmOutput as its parent.
                            If pfrmOutput.TSMaxSubscript(2, lGroup, lMem) > 1 Then
                                lTempMem = lTempMem & CStr(lSub2)
                            End If
                        Else
                            lSub1 = 1
                            lSub2 = 1
                            lMem = Mid(lTmem, lColonPos + 1)
                            lTempMem = lMem
                        End If
                        lGroup = Mid(lTmem, 1, lColonPos - 1)
                        'now add the data set
                        Dim lTrans As String
                        If lOutTu = 3 Then
                            'hourly, use blank transform
                            lTrans = "    "
                        Else
                            lTrans = "AVER"
                        End If
                        Dim lWDMId As Integer
                        Dim lNewDsn As Integer
                        Dim lDescription As String = ""
                        OutputDataSetProperties(lTmem, lDescription)
                        pUCI.AddOutputWDMDataSetExt(txtLoc.Text, lTempMem, atxBase.Text, lWDMId, lOutTu, lDescription, lNewDsn, lSTANAM)
                        pUCI.AddExtTarget(lOpName, lId, lGroup, lMem, lSub1, lSub2, 1.0#, lTrans,
                             "WDM" & CStr(lWDMId), lNewDsn, lTempMem, 1, lOutputUnits, "AGGR", "REPL")
                        pUCI.Edited = True
                    End If
                End If
            Else
                lDialogResult = Logger.Message("An operation and group/member must be selected.", "Add Output Problem", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, Windows.Forms.DialogResult.OK)
            End If
        ElseIf pCheckedRadioIndex = 5 Then 'copy
            If lstGroup.Items.Count > 0 And lstGroup.SelectedIndex > -1 Then
                'something selected in both lists
                Dim lctemp As String = lstOperation.Items(lstOperation.SelectedIndex)
                Dim lSpacePos As Integer = InStr(1, lctemp, " ")
                Dim lOpname As String = Mid(lctemp, 1, lSpacePos - 1)
                lId = CInt(Mid(lctemp, lSpacePos + 1))
                Dim lFromOper As HspfOperation = pUCI.OpnBlks(lOpname).OperFromID(lId)
                lctemp = lstGroup.Items(lstOperation.SelectedIndex)
                lSpacePos = InStr(1, lctemp, " ")
                lOpname = Mid(lctemp, 1, lSpacePos - 1)
                lId = CInt(Mid(lctemp, lSpacePos + 1))
                Dim lToOper As HspfOperation = pUCI.OpnBlks(lOpname).OperFromID(lId)
                Dim lFromCalib As Boolean
                If lFromOper.Name = "RCHRES" Then
                    lFromCalib = pfrmOutput.IsCalibLocation(lFromOper.Name, lFromOper.Id)
                Else
                    lFromCalib = False
                End If
                For Each lConn As HspfConnection In lFromOper.Targets
                    If Mid(lConn.Target.VolName, 1, 3) = "WDM" Then
                        If lConn.Source.VolName = "COPY" Then
                            'assume this is a calibration location, skip it
                        Else
                            'this is an output from this operation, copy it
                            If lFromCalib And lConn.Source.Group = "ROFLOW" And lConn.Source.Member = "ROVOL" Then
                            Else
                                'make sure we do not already have this output
                                Dim lFound As Boolean = False
                                For Each lToConn As HspfConnection In lToOper.Targets
                                    If lToConn.Source.Group = lConn.Source.Group And
                                       lToConn.Source.Member = lConn.Source.Member Then
                                        lFound = True
                                    End If
                                Next
                                If lFound = False Then
                                    'now add it
                                    Dim lWDMId As Integer
                                    Dim lNewDsn As Integer
                                    Call pUCI.AddOutputWDMDataSet(txtLoc.Text, lConn.Target.Member, atxBase.Text, lWDMId, lNewDsn)
                                    pUCI.AddExtTarget(lToOper.Name, lToOper.Id, lConn.Source.Group, lConn.Source.Member,
                                       lConn.Source.MemSub1, lConn.Source.MemSub2, lConn.MFact, lConn.Tran,
                                       "WDM" & CStr(lWDMId), lNewDsn, lConn.Target.Member, lConn.Target.MemSub1,
                                       lConn.Ssystem, lConn.Sgapstrg, lConn.Amdstrg)
                                    pUCI.Edited = True
                                End If
                            End If
                        End If
                    End If
                Next
            Else
                lDialogResult = Logger.Message("An operation must be selected in the 'To Operation' list.", "Output Manager Problem", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, Windows.Forms.DialogResult.OK)
            End If
        ElseIf pCheckedRadioIndex = 3 Then 'add aquatox linkage
            If lstOperation.SelectedIndex > -1 Then
                lId = pListBox1DataItems.Item(lstOperation.SelectedIndex + 1)
                If Not frmOutput.IsAQUATOXLocation("RCHRES", (lId)) Then
                    'make sure required sections are on  
                    Dim lOper As HspfOperation = pUCI.OpnBlks("RCHRES").OperFromID(lId)
                    Dim lTable As HspfTable = lOper.Tables("ACTIVITY")
                    If lTable.Parms(0).Value = 1 AndAlso lTable.Parms(3).Value = 1 AndAlso lTable.Parms(4).Value = 1 AndAlso lTable.Parms(6).Value = 1 AndAlso lTable.Parms(7).Value = 1 Then
                        'all required rchres sections are on
                        '(hydr, htrch, sedtrn, oxrx, nutrx)
                        Dim lGqualFg(3) As Integer
                        CheckGQUALs(lId, lGqualFg)
                        Me.Cursor = Cursors.WaitCursor
                        'add data sets
                        Dim lWDMId As Integer
                        Dim lMember(28) As String
                        Dim ltGroup(28) As String
                        Dim lMSub1(28) As Integer
                        pUCI.AddAQUATOXDsnsExt(lId, txtLoc.Text, atxBase.Text, lTable.Parms(8).Value, lGqualFg, lWDMId, lMember, lMSub1, ltGroup, lDsns, lOstr, lOutTu)
                        'add to ext targets block
                        pUCI.AddAQUATOXExtTargetsExt(lId, lWDMId, lMember, lMSub1, ltGroup, lDsns, lOstr, lOutTu)
                        pUCI.Edited = True
                        Me.Cursor = Cursors.Arrow
                    Else
                        Dim lctemp As String = ""
                        If lTable.Parms(0).Value = 0 Then
                            lctemp = lctemp & "HYDR "
                        End If
                        If lTable.Parms(3).Value = 0 Then
                            lctemp = lctemp & "HTRCH "
                        End If
                        If lTable.Parms(4).Value = 0 Then
                            lctemp = lctemp & "SEDTRN "
                        End If
                        If lTable.Parms(6).Value = 0 Then
                            lctemp = lctemp & "OXRX "
                        End If
                        If lTable.Parms(7).Value = 0 Then
                            lctemp = lctemp & "NUTRX "
                        End If
                        lDialogResult = Logger.Message("The following required sections are not on: " & lctemp, "AQUATOX Linkage Problem", MessageBoxButtons.OK, MessageBoxIcon.Error, Windows.Forms.DialogResult.OK)
                    End If
                End If
            End If
        End If

        If lDialogResult = Nothing Then
            pfrmOutput.RefreshAll()
            Me.Dispose()
        End If

    End Sub
    Private Function OutputDataSetProperties(ByRef aConstituentName As String, ByRef aDescription As String)
        Select Case aConstituentName
            Case "NUTRX:DNUST(1,1)"
                aConstituentName = "NO3"
                aDescription = "Nitrate Concentration in mg/l"
            Case "NUTRX:DNUST(2,1)"
                aConstituentName = "TAM"
                aDescription = "Total Ammonia Concentration in mg/l"
            Case "NUTRX:DNUST(3,1)"
                aConstituentName = "NO2"
                aDescription = "Nitrite Concentration in mg/l"
            Case "NUTRX:DNUST(4,1)"
                aConstituentName = "PO4"
                aDescription = "Phosphate Concentration in mg/l"
            Case "NUTRX:DNUST(5,1)"
                aConstituentName = "NH4+"
                aDescription = "Ammonium Concentration in mg/l"
            Case "NUTRX:DNUST(6,1)"
                aConstituentName = "NH3"
                aDescription = "Ammonia Concentration in mg/l"
            Case "PLANK:PKST4(1,1)"
                aConstituentName = "TN"
                aDescription = "Total Nitrogen Concentration in mg/l"
            Case "PLANK:PKST4(2,1)"
                aConstituentName = "TP"
                aDescription = "Total Phosphorus Concentration in mg/l"
            Case "SEDTRN:SSED(1,1)"
                aConstituentName = "SS1"
                aDescription = "Suspended Sand Concentration in mg/l"
            Case "SEDTRN:SSED(2,1)"
                aConstituentName = "SS2"
                aDescription = "Suspended Silt Concentration in mg/l"
            Case "SEDTRN:SSED(3,1)"
                aConstituentName = "SS3"
                aDescription = "Suspended Clay Concentration in mg/l"
            Case "SEDTRN:SSED(4,1)"
                aConstituentName = "TSS"
                aDescription = "Total Suspended Solids Concentration in mg/l"
            Case "OXRX:DOX"
                aConstituentName = "DO"
                aDescription = "Dissolved Oxygen Concentration in mg/l"
            Case "OXRX:BOD"
                aConstituentName = "BOD5"
                aDescription = "BOD-5 day Concentration in mg/l"
            Case "HTRCH:TW"
                aConstituentName = "TW"
                aDescription = "Temperature of Water"
        End Select


        Return aDescription
        Return aConstituentName
    End Function

    Private Sub CheckGQUALs(ByVal aId As Integer, ByVal aGqualFg() As Integer)

        Dim lDialogBoxResult As Windows.Forms.DialogResult

        Dim lNgqual As Integer
        Dim lTemp As Integer
        Dim lOper As HspfOperation = pUCI.OpnBlks("RCHRES").OperFromID(aId)
        Dim lTable As HspfTable = lOper.Tables("ACTIVITY")

        If lTable.Parms(6).Value = 1 Then
            'gqual on
            'figure out how many gquals
            lNgqual = 0
            For Each lOpn As HspfOperation In pUCI.OpnBlks("RCHRES").Ids
                If lOpn.TableExists("GQ-QALDATA") Then
                    If lOpn.TableExists("GQ-GENDATA") Then
                        lTemp = lOpn.Tables("GQ-GENDATA").Parms("NGQUAL").Value
                    Else
                        lTemp = 1
                    End If
                    If lTemp > lNgqual Then
                        lNgqual = lTemp
                    End If
                End If
            Next

            Dim lQname(lNgqual) As String
            For lIndex As Integer = 1 To lNgqual
                Dim lTname As String
                If lIndex = 1 Then
                    lTname = "GQ-QALDATA"
                Else
                    lTname = "GQ-QALDATA:" & lIndex
                End If
                lQname(lIndex) = ""
                For Each lOpn As HspfOperation In pUCI.OpnBlks("RCHRES").Ids
                    If lOpn.TableExists(lTname) And Len(lQname(lIndex)) = 0 Then
                        lQname(lIndex) = Trim(lOpn.Tables(lTname).Parms("GQID").Value)
                    End If
                Next
                lDialogBoxResult = Logger.Message("Do you want to include the total inflow of GQUAL " & lIndex & " (" & lQname(lIndex) & ") in the AQUATOX Linkage?", "AQUATOX Query", MessageBoxButtons.YesNo, MessageBoxIcon.Question, Windows.Forms.DialogResult.Yes)

                If lDialogBoxResult = Windows.Forms.DialogResult.Yes Then
                    aGqualFg(lIndex) = 1
                ElseIf lDialogBoxResult = Windows.Forms.DialogResult.No Then
                    aGqualFg(lIndex) = 0
                End If
            Next
        Else
            aGqualFg(1) = 0
            aGqualFg(2) = 0
            aGqualFg(3) = 0
        End If
    End Sub

    Private Sub ListBox2_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstGroup.SelectedIndexChanged

        If pCheckedRadioIndex <> 1 Then 'does not apply for hydrology calibration locations
            Dim lId As Integer
            Dim lOpName As String
            Dim lSpacePos As Integer = InStr(1, lstGroup.Items.Item(lstGroup.SelectedIndex), " ")
            Dim lParenPos As Integer = InStr(1, lstGroup.Items.Item(lstGroup.SelectedIndex), "(")
            If lParenPos = 0 Then lParenPos = Len(lstGroup.Items.Item(lstGroup.SelectedIndex)) + 1
            If lSpacePos > 0 And lParenPos > 0 Then
                lId = CInt(Mid(lstGroup.Items.Item(lstGroup.SelectedIndex), lSpacePos, lParenPos - lSpacePos))
                lOpName = Mid(lstGroup.Items.Item(lstGroup.SelectedIndex), 1, lSpacePos - 1)
                If lId > 0 And Len(lOpName) > 0 Then
                    If lOpName = "PERLND" Then
                        txtLoc.Text = "P" & CStr(lId)
                    ElseIf lOpName = "IMPLND" Then
                        txtLoc.Text = "I" & CStr(lId)
                    ElseIf lOpName = "RCHRES" Then
                        txtLoc.Text = "RCH" & CStr(lId)
                    ElseIf lOpName = "BMPRAC" Then
                        txtLoc.Text = "BMP" & CStr(lId)
                    End If
                End If
            End If
            'set desc text
            lSpacePos = InStr(1, lstOperation.Items.Item(lstOperation.SelectedIndex), " ")
            lParenPos = InStr(1, lstOperation.Items.Item(lstOperation.SelectedIndex), "(")
            If lSpacePos > 0 And lParenPos > 0 Then
                lId = CInt(Mid(lstOperation.Items.Item(lstOperation.SelectedIndex), lSpacePos, lParenPos - lSpacePos))
                lOpName = Mid(lstOperation.Items.Item(lstOperation.SelectedIndex), 1, lSpacePos - 1)
                If lId > 0 And Len(lOpName) > 0 Then
                    Dim lOper As HspfOperation = pUCI.OpnBlks(lOpName).OperFromID(lId)
                    Dim lColonPos As Integer = InStr(1, lstGroup.Items.Item(lstGroup.SelectedIndex), ":")
                    If lColonPos > 0 Then
                        Dim lGroup As String = Mid(lstGroup.Items.Item(lstGroup.SelectedIndex), 1, lColonPos - 1)
                        lParenPos = InStr(1, lstGroup.Items.Item(lstGroup.SelectedIndex), "(")
                        Dim lMem As String
                        If lParenPos > 0 Then
                            lMem = Mid(lstGroup.Items.Item(lstGroup.SelectedIndex), lColonPos + 1, lParenPos - lColonPos - 1)
                        Else
                            lMem = Mid(lstGroup.Items.Item(lstGroup.SelectedIndex), lColonPos + 1)
                        End If

                        For lIndex As Integer = 0 To pUCI.Msg.TSGroupDefs.Count - 1
                            If pUCI.Msg.TSGroupDefs.Item(lIndex).Name = lGroup Then
                                Dim lGroupDef As HspfTSGroupDef = pUCI.Msg.TSGroupDefs.Item(lIndex)
                                Dim lGetMemberDefAtIndex As Integer

                                '.net conversion issue: Becuase no method to fetch via key in lGroupDef.MemberDefs was found, this loop does it manually.
                                For lIndex2 As Integer = 0 To lGroupDef.MemberDefs.Count - 1
                                    If lGroupDef.MemberDefs.Item(lIndex2).Name = lMem Then
                                        lGetMemberDefAtIndex = lIndex2
                                        Exit For
                                    End If
                                Next

                                'now set the text
                                txtDescription.Text = lGroup & ":" & lMem & " - " & lGroupDef.MemberDefs.Item(lGetMemberDefAtIndex).Defn
                                If pUCI.GlobalBlock.EmFg = 1 Then
                                    txtDescription.Text = txtDescription.Text & " (" & lGroupDef.MemberDefs.Item(lGetMemberDefAtIndex).EUnits & ")"
                                Else
                                    txtDescription.Text = txtDescription.Text & " (" & lGroupDef.MemberDefs.Item(lGetMemberDefAtIndex).MUnits & ")"
                                End If
                                Exit For
                            End If
                        Next
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstOperation.SelectedIndexChanged

        Dim lId As Integer
        If pCheckedRadioIndex = 1 Or pCheckedRadioIndex = 2 Or pCheckedRadioIndex = 3 Then
            lId = pListBox1DataItems.Item(lstOperation.SelectedIndex + 1)
            txtLoc.Text = "RCH" & CStr(lId)
        ElseIf pCheckedRadioIndex = 4 Then
            txtDescription.Text = ""
            Dim lSpacePos As Integer = InStr(1, lstOperation.Items.Item(lstOperation.SelectedIndex), " ")
            Dim lParenPos As Integer = InStr(1, lstOperation.Items.Item(lstOperation.SelectedIndex), "(")
            If lSpacePos > 0 And lParenPos > 0 Then
                lId = CInt(Mid(lstOperation.Items.Item(lstOperation.SelectedIndex), lSpacePos, lParenPos - lSpacePos))
                Dim lOpName As String = Mid(lstOperation.Items.Item(lstOperation.SelectedIndex), 1, lSpacePos - 1)
                If lId > 0 And Len(lOpName) > 0 Then
                    Dim lOper As HspfOperation = pUCI.OpnBlks(lOpName).OperFromID(lId)
                    If Not lOper Is Nothing Then
                        If lOpName = "PERLND" Then
                            txtLoc.Text = "P" & CStr(lId)
                        ElseIf lOpName = "IMPLND" Then
                            txtLoc.Text = "I" & CStr(lId)
                        ElseIf lOpName = "RCHRES" Then
                            txtLoc.Text = "RCH" & CStr(lId)
                        ElseIf lOpName = "BMPRAC" Then
                            txtLoc.Text = "BMP" & CStr(lId)
                        End If
                        'get possible timsers from edit operation method
                        lOper.OutputTimeseriesStatus.UpdateExtTargetsOutputs()
                        Dim lTimsers As Collection(Of HspfStatusType) = lOper.OutputTimeseriesStatus.GetOutputInfo(2)
                        lstGroup.Items.Clear()
                        For Each lTimser As HspfStatusType In lTimsers
                            Dim lExtTarget As Boolean = False
                            If lTimser.Present Then
                                'see if used as external target
                                For Each lConn As HspfConnection In lOper.Targets
                                    If UCase(Mid(lConn.Target.VolName, 1, 3)) = "WDM" Then
                                        'used as ext target, don't add
                                        Dim lIndex As Integer = InStr(1, lTimser.Name, ":")
                                        If lIndex > 0 Then
                                            If lConn.Source.Group = Mid(lTimser.Name, 1, lIndex - 1) Then
                                                If lConn.Source.Member = Mid(lTimser.Name, lIndex + 1) Then
                                                    If lTimser.Max > 1 Then
                                                        If lConn.Source.MemSub1 = lTimser.Occur Mod 1000 Then
                                                            If lConn.Source.MemSub2 = CLng((1 + (lTimser.Occur) / 1000)) Then
                                                                lExtTarget = True
                                                            End If
                                                        End If
                                                    Else
                                                        lExtTarget = True
                                                    End If
                                                End If
                                            End If
                                        End If
                                    End If
                                Next
                            End If
                            If Not lExtTarget Then
                                If lTimser.Max = 1 Then
                                    lstGroup.Items.Add(lTimser.Name)
                                Else
                                    Dim lSub As String = "(" & lTimser.Occur Mod 1000 & "," & CLng(1 + (lTimser.Occur) / 1000) & ")"
                                    lstGroup.Items.Add(lTimser.Name & lSub)
                                End If
                            End If
                        Next
                        lstGroup.Enabled = True
                        lblGroup.Enabled = True
                    End If
                End If
            End If
        ElseIf pCheckedRadioIndex = 5 Then 'copy
            Dim lTemp As String = lstOperation.Items.Item(lstOperation.SelectedIndex)
            Dim lSpacePos As Integer = InStr(1, lTemp, " ")
            Dim lOpName As String = Mid(lTemp, 1, lSpacePos - 1)
            lId = CInt(Mid(lTemp, lSpacePos + 1))
            lstGroup.Items.Clear()
            Dim lOpnBlk As HspfOpnBlk = pUCI.OpnBlks(lOpName)
            For lIndex As Integer = 1 To lOpnBlk.Count
                Dim lOper As HspfOperation = lOpnBlk.NthOper(lIndex)
                If lOper.Id <> lId Then
                    lstGroup.Items.Add(lOper.Name & " " & lOper.Id)
                End If
            Next lIndex
            If lstGroup.Items.Count > 0 Then
                lstGroup.Enabled = True
            Else
                lstGroup.Enabled = False
            End If
        End If
    End Sub

    Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        Me.Dispose()
    End Sub

    Private Sub frmAddExpert_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyValue = Windows.Forms.Keys.F1 Then
            ShowHelp(pWinHSPFManualName)
            ShowHelp("User's Guide\Detailed Functions\Output Manager.html")
        End If
    End Sub
End Class